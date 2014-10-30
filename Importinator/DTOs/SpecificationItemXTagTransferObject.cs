using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Content_Centre_API_CORS.Messaging
{
    public class SpecificationItemXTagTransferObject
    {
        private int specificationItemID = -1;
        private int xTagID = -1;
        private Guid userID = Guid.Empty;

        public int SpecificationItemID
        {
            get { return specificationItemID; }
            set { specificationItemID = value; }
        }
        public int XTagID
        {
            get { return xTagID; }
            set { xTagID = value; }
        }
        public Guid UserID
        {
            get { return userID; }
            set { userID = value; }
        }
    }
}