using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Content_Centre_API.Messaging
{
    public class SpecificationItemResponseObject
    {
        private int specificationItemID = -1;
        private DataTable results = null;
        private string jsonResults = string.Empty;

        public int SpecificationItemID
        {
            get { return specificationItemID; }
            set { specificationItemID = value; }
        }

        public DataTable Results
        {
            get { return results; }
            set { results = value; }
        }

        public string JsonResults
        {
            get { return jsonResults; }
            set { jsonResults = value; }
        }
    }
}
