using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Content_Centre_API_CORS.Messaging
{
    public class LanguageTransferObject
    {
        int languageID = -1;
        string value = string.Empty;
        Guid userID = Guid.Empty;
        string accountID = string.Empty;
        string languageCode = string.Empty;


        public int LanguageID
        {
            get { return languageID; }
            set { languageID = value; }
        }

        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public string LanguageCode
        {
            get { return languageCode; }
            set { languageCode = value; }
        }

        public string AccountID
        {
            get { return accountID; }
            set { this.accountID = value; }
        }


        public Guid UserID
        {
            get { return userID; }
            set { userID = value; }
        }
    }
}
