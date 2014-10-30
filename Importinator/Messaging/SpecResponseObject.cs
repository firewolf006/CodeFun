using Content_Centre_API_CORS.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Content_Centre_API.Messaging
{
    public class SpecResponseObject
    {
        private int specID = -1;
        //private DataTable results = null;
        public DataTable Results {get; set;}
        private string jsonResults = null;
        private int rotationID = -1;

        public bool Error { get; set; }
        public List<SpecificationError> ErrorList { get; set; }

        public string JsonResults
        {
            get { return jsonResults; }
            set { jsonResults = value; }
        }
        //public DataTable Results
        //{
        //    get { return results; }
        //    set { results = value; }
        //}
        public int RotationID
        {
            get { return rotationID; }
            set { rotationID = value; }
        }
        public int SpecID
        {
            get { return specID; }
            set { specID = value; }
        }

    }
}