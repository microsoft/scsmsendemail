using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading;                                                     //Has AutoResetEvent in it

//Requires Microsoft.EnterpriseManagement.UI.WpfWizardFramework reference
using Microsoft.EnterpriseManagement.UI.WpfWizardFramework;

//Requires Microsoft.EnterpriseManagement.UI.SdkDataAccess reference
using Microsoft.EnterpriseManagement.UI.SdkDataAccess;                      // Has the ConsoleCommand class in it

//Requires Microsoft.EnterpriseManagement.UI.Foundation reference
using Microsoft.EnterpriseManagement.ConsoleFramework;                      //Has the NavigationModelNodeBase, NavigationModelNodeTask, NavigationModel in it
using Microsoft.EnterpriseManagement.UI.DataModel;                          //Has IDataItem in it
using Microsoft.EnterpriseManagement.UI.SdkDataAccess.DataAdapters;         //Has EnterpriseManagementObjectProjectionDataType in it

using Microsoft.EnterpriseManagement.UI.FormsInfra;                         //Has FormView in it

using Microsoft.EnterpriseManagement.ServiceManager.Incident.TaskHandlers;
using Microsoft.EnterpriseManagement.ServiceManager.Incident.Common;

namespace SendEmail
{
    class SendEmailTaskHandler : ConsoleCommand
    {
        public override void ExecuteCommand(IList<NavigationModelNodeBase> navigationNodes, NavigationModelNodeTask task, ICollection<String> parameters)
        {
            foreach(NavigationModelNode navigationNode in navigationNodes)
            {
                IDataItem dataitemIncident = null;
                IDataItem dataitemIncidentStatus = null;
                IDataItem dataitemMessageType = null;
                bool boolIsFormAction = false;
                Uri uriFormsDefinition = new Uri(NavigationModel.NavigationRoot,"Windows/Forms/");
                if (navigationNode != null && uriFormsDefinition.IsBaseOf(navigationNode.Location))
                {
                    boolIsFormAction = true;
                    FrameworkElement userControl = GetIncidentControl(navigationNode);
                    if (userControl != null)
                    {
                        if (userControl.CheckAccess())
                        {
                            dataitemIncident = userControl.DataContext as IDataItem;
                        }
                        else
                        {
                            using (AutoResetEvent autoreset = new AutoResetEvent(false))
                            {
                                EventHandler delegateHandleUserControl =
                                    delegate(object sender, EventArgs e)
                                    {
                                        dataitemIncident = userControl.DataContext as IDataItem;
                                        autoreset.Set();
                                    };

                                userControl.Dispatcher.BeginInvoke(
                                    System.Windows.Threading.DispatcherPriority.Normal,
                                    delegateHandleUserControl,
                                    null,
                                    null);
                                autoreset.WaitOne();
                            }
                        }
                    }
                }
                else
                {
                    //Console Node
                    IDataItem dataItem = navigationNode as IDataItem;
                    if (dataItem != null)
                    {
                        dataitemIncident = DataQueryHelper.GetEmoProjectionItem((Guid)dataItem[Constants.MP_PROPERTY_ID_DOLLAR], new Guid(Constants.MP_PROJECTION_TYPE_INCIDENT));
                    }                
                }
                
                String strMessage = String.Empty;
                Boolean boolAddToActionLog = false;
                dataitemIncidentStatus = (IDataItem)dataitemIncident[Constants.MP_PROPERTY_INCIDENT_STATUS];
                bool? result = SendEmailForm.LaunchDialog(task.DisplayName, ref dataitemMessageType, ref dataitemIncidentStatus, out strMessage, out boolAddToActionLog);
                if (result == null || !result.Value || String.IsNullOrEmpty(strMessage))
                {
                    // The user either did not enter any comment or he clicked the cancel button
                    break;
                }
                dataitemIncident[Constants.MP_PROPERTY_INCIDENT_MESSAGE] = strMessage;
                dataitemIncident[Constants.MP_PROPERTY_INCIDENT_MESSAGE_TYPE] = dataitemMessageType;
                dataitemIncident[Constants.MP_PROPERTY_INCIDENT_STATUS] = dataitemIncidentStatus;

                if (boolAddToActionLog)
                {
                    //Add the Action Log Object
                    IDataItem dataitemSentEmailActionType = DataQueryHelper.GetEnumerations(new Guid(Constants.MP_ENUM_TYPE_SENT_EMAIL), false)[0];
                    IDataItem dataitemActionLogItem = DataQueryHelper.CreateNewInstanceBindableItem(new Guid(Constants.MP_CLASS_TYPE_ACTION_LOG));
                    dataitemActionLogItem[Constants.MP_PROPERTY_ACTION_LOG_ENTERED_DATE] = DateTime.Now;
                    dataitemActionLogItem[Constants.MP_PROPERTY_ACTION_LOG_DESCRIPTION] = strMessage;
                    dataitemActionLogItem[Constants.MP_PROPERTY_ACTION_LOG_ID] = Guid.NewGuid().ToString();
                    dataitemActionLogItem[Constants.MP_PROPERTY_ACTION_LOG_ACTION_TYPE] = dataitemSentEmailActionType;
                    dataitemActionLogItem[Constants.MP_PROPERTY_ACTION_LOG_TITLE] = dataitemSentEmailActionType[DataItemConstants.DisplayName];
                    dataitemActionLogItem[Constants.MP_PROPERTY_ACTION_LOG_ENTERED_BY] = (String)DataQueryHelper.GetCurrentLoggedInUser()[DataItemConstants.DisplayName];

                    if (dataitemIncident.HasProperty(Constants.MP_PROPERTY_ACTION_LOG_ACTION_LOGS))
                    {
                        dataitemIncident[Constants.MP_PROPERTY_ACTION_LOG_ACTION_LOGS] = dataitemActionLogItem;
                    }
                }
                
                if (!boolIsFormAction)
                {
                    //If this was initiated from the console we need to update the incident straight away
                    EnterpriseManagementObjectProjectionDataType.UpdateDataItem(dataitemIncident);
                }   
            }
        }

        internal FrameworkElement GetIncidentControl(NavigationModelNodeBase navigationNode)
        {   
            //Form Node
            FormView formView = NavigationModel.FindView(null, navigationNode.Location,
                FindViewCriteria.ViewIsAssociatedToNode) as FormView;
            if (formView != null)
            {
                if (formView.CheckAccess())
                {
                    return formView.Form as FrameworkElement;
                }
                else
                {
                    FrameworkElement control = null;
                    using (AutoResetEvent autoreset = new AutoResetEvent(false))
                    {
                        EventHandler delegateHandleFormView =
                            delegate(object sender, EventArgs e)
                            {
                                control = formView.Form as FrameworkElement;
                                autoreset.Set();
                            };

                        formView.Dispatcher.BeginInvoke(
                            System.Windows.Threading.DispatcherPriority.Normal,
                            delegateHandleFormView,
                            null,
                            null);
                        autoreset.WaitOne();
                    }
                    return control;
                }
            }
            return null;
        }
        
    }
}
