using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Content_Centre_API_CORS.Messaging
{
    public class StatusTransferObject
    {
        private string roleID = string.Empty;
        private int statusID = -1;
        private string accountID = string.Empty;

        // ADDED THIS 
        private string status = string.Empty;

        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        public string RoleID
        {
            get { return roleID; }
            set { roleID = value; }
        }

        public int StatusID
        {
            get { return statusID; }
            set { statusID = value; }
        }

        public string AccountID
        {
            get { return accountID; }
            set { accountID = value; }
        }
    }
}
