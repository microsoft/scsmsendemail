using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EnterpriseManagement.UI.WpfWizardFramework;
using System.ComponentModel;        //Has the INotifyPropertyChanged class in it
using Microsoft.EnterpriseManagement.UI.DataModel;

namespace SendEmail
{
    class SendEmailWizardData : WizardData, INotifyPropertyChanged
    {
        private String strMessage;
        private Boolean boolAddToActionLog;
        private IDataItem dataitemMessageType;
        private IDataItem dataitemIncidentStatus;

        public String Message
        {
            get
            {
                return strMessage;
            }
            set
            {
                strMessage = value;
                this.NotifyPropertyChanged(Constants.MP_PROPERTY_INCIDENT_MESSAGE);
            }
        }

        public IDataItem MessageType
        {
            get
            {
                return dataitemMessageType;
            }
            set
            {
                dataitemMessageType = value;
                this.NotifyPropertyChanged(Constants.MP_PROPERTY_INCIDENT_MESSAGE_TYPE);
            }
        }

        public IDataItem IncidentStatus
        {
            get
            {
                return dataitemIncidentStatus;
            }
            set
            {
                dataitemIncidentStatus = value;
                this.NotifyPropertyChanged(Constants.MP_PROPERTY_INCIDENT_STATUS);
            }
        }

        public Boolean AddToActionLog
        {
            get
            {
                return boolAddToActionLog;
            }
            set
            {
                boolAddToActionLog = value;
                this.NotifyPropertyChanged(Constants.ADD_TO_ACTION_LOG);
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
