using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SendEmail
{
    public class Constants
    {
        public const string MP_CLASS_TYPE_ACTION_LOG = "DBB6A632-0A7E-CEF8-1FC9-405D5CD4D911";

        public const string MP_PROPERTY_ID_DOLLAR = "$Id$";

        public const string MP_PROPERTY_INCIDENT_STATUS = "Status";
        public const string MP_PROPERTY_INCIDENT_MESSAGE = "Message";
        public const string MP_PROPERTY_INCIDENT_MESSAGE_TYPE = "MessageType";

        public const string MP_PROPERTY_ACTION_LOG_ENTERED_DATE = "EnteredDate";
        public const string MP_PROPERTY_ACTION_LOG_DESCRIPTION = "Description";
        public const string MP_PROPERTY_ACTION_LOG_ID = "Id";
        public const string MP_PROPERTY_ACTION_LOG_ACTION_TYPE = "ActionType";
        public const string MP_PROPERTY_ACTION_LOG_TITLE = "Title";
        public const string MP_PROPERTY_ACTION_LOG_ENTERED_BY = "EnteredBy";
        public const string MP_PROPERTY_ACTION_LOG_ACTION_LOGS = "ActionLogs";

        public const string MP_ENUM_TYPE_SENT_EMAIL = "15E86D4A-1B55-01BE-C9FA-660A3CB3FC26";
        public const string MP_ENUM_TYPE_INCIDENT_STATUS_ROOT = "89B34802-671E-E422-5E38-7DAE9A413EF8";
        public const string MP_ENUM_TYPE_INCIDENT_MESSAGE_TYPE_ROOT = "809E6ED8-F976-C121-AF56-46C934A5952F";
        
        public const string MP_PROJECTION_TYPE_INCIDENT = "285CB0A2-F276-BCCB-563E-BB721DF7CDEC";
        
        public const string ADD_TO_ACTION_LOG = "AddToActionLog";            
    }
}
