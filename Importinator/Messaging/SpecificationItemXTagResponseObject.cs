using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace Content_Centre_API_CORS.Messaging
{
    public class SpecificationItemXTagResponseObject
    {
        private DataTable results = null;
        public DataTable Results
        {
            get { return results; }
            set { results = value; }
        }
    }
}