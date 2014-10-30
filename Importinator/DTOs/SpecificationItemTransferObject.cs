using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Content_Centre_API_CORS.Messaging
{
    public class SpecificationItemTransferObject
    {
        private bool quarkImport = false;
        private Int64 specificationItemID = -1;
        private Int64 parentID = -1;
        private int languageID = -1;
        private int associatedID = -1;
        private Int64 specificationID = -1;
        private int orderNumber = -1;
        private string value = string.Empty;
        private string notes = string.Empty;
        private string quarkFileName = string.Empty;
        private Guid userID = Guid.Empty;
        private string accountID = string.Empty;
        private bool toBeDeleted = false;


        public bool QuarkImport
        {
            get { return quarkImport; }
            set { quarkImport = value; }
        }

        public Int64 SpecificationItemID
        {
            get { return specificationItemID; }
            set { specificationItemID = value; }
        }

        public Int64 ParentID
        {
            get { return parentID; }
            set { parentID = value; }
        }

        public int LanguageID
        {
            get { return languageID; }
            set { languageID = value; }
        }

        public int AssociatedID
        {
            get { return associatedID; }
            set { associatedID = value; }
        }

        public Int64 SpecificationID
        {
            get { return specificationID; }
            set { specificationID = value; }
        }

        public int OrderNumber
        {
            get { return orderNumber; }
            set { orderNumber = value; }
        }

        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public string Notes
        {
            get { return notes; }
            set { notes = value; }
        }

        public string QuarkFileName
        {
            get { return quarkFileName; }
            set { quarkFileName = value; }
        }

        public Guid UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        public string AccountID
        {
            get { return accountID; }
            set { accountID = value; }
        }

        public bool ToBeDeleted
        {
            get { return toBeDeleted; }
            set { toBeDeleted = value; }
        }

    }
}
