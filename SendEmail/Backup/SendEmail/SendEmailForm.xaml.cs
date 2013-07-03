using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.EnterpriseManagement.UI.WpfWizardFramework;
using Microsoft.EnterpriseManagement.UI.DataModel;

namespace SendEmail
{
    public partial class SendEmailForm : WizardRegularPageBase
    {
        public static Guid guidMessageTypeEnumRoot = new Guid(Constants.MP_ENUM_TYPE_INCIDENT_MESSAGE_TYPE_ROOT);
        public static Guid guidIncidentStatusEnumRoot = new Guid(Constants.MP_ENUM_TYPE_INCIDENT_STATUS_ROOT);

        public SendEmailForm(WizardData wizarddata)
        {
            InitializeComponent();
        }

        public static bool? LaunchDialog(String strWindowTitle, ref IDataItem dataitemMessageType, ref IDataItem dataItemIncidentStatus, out String strMessage, out Boolean boolAddToActionLog)
        {
            WizardStory wizardStory = new WizardStory();
            SendEmailWizardData wizardData = new SendEmailWizardData();
            wizardStory.WizardData = wizardData;
            wizardStory.AddLast(new WizardStep(strWindowTitle,typeof(SendEmailForm), wizardStory.WizardData));
            TabbedPropertySheetDialog propertyDialog = new TabbedPropertySheetDialog(wizardStory);
            propertyDialog.Width = 350;
            propertyDialog.Height = 350;
            propertyDialog.MinWidth = 350;
            propertyDialog.MinHeight = 350;
            
            propertyDialog.ResizeMode = ResizeMode.CanResizeWithGrip;
            propertyDialog.Title = strWindowTitle;
            wizardData.IncidentStatus = dataItemIncidentStatus;
            wizardData.AddToActionLog = true;
            bool? result = propertyDialog.ShowDialog();
            strMessage = wizardData.Message;
            dataitemMessageType = wizardData.MessageType;
            dataItemIncidentStatus = wizardData.IncidentStatus;
            boolAddToActionLog = wizardData.AddToActionLog;
            return result;
        }

        private void txtMessage_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.txtMessage.Text != null)
            {
                this.lblRemainingCharacters.Content = 4000 - this.txtMessage.Text.Length;
            }
        }
    }
}
