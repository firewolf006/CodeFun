using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Content_Centre_API.Messaging
{
    public class RotationGrillResponseObject
    {
        DataTable results = null;
        string jsonResults = null;
        int rotationID = -1;


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

        public int RotationID
        {
            get { return rotationID; }
            set { rotationID = value; }
        }
    }
}
