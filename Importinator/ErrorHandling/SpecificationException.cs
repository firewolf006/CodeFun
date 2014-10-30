using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Content_Centre_API_CORS.ErrorHandling
{
    public class SpecificationException : Exception
    {
        public SpecificationException()
            {
            }

        public SpecificationException(string message)
             : base(message)
            {
            }

        public SpecificationException(string message, Exception inner)
                : base(message, inner)
            {
            }
    }
}