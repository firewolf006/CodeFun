using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Content_Centre_API.Messaging
{
    public class StatusResponseObject
    {
        DataTable results = null;
        string jsonResults = null;


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
