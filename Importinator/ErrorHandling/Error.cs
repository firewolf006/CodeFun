using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Content_Centre_API_CORS.ErrorHandling
{
    public class SpecificationError
    {
        public Int64 SpecificationID { get; set; }
        public string ErrorMessage { get; set; }
    }
}