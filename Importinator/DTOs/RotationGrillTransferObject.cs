using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Content_Centre_API_CORS.Messaging
{
    public class RotationGrillTransferObject
    {
        int rotationID = -1;
        string rotationName = "";
        int rotationNumber = -1;
        string startDate = null;
        string endDate = null;
        string createdBy = null;
        string createdDate = null;
        string updatedBy = null;
        string updatedDate = null;
        string accountID = string.Empty;
        Guid userID = Guid.Empty;
        int rotationDetailID = -1;
        List<int> listOfCheckBoxes = new List<int>();
        string orderBy = null;


        public int RotationDetailID
        {
            get { return rotationDetailID; }
            set { rotationDetailID = value; }
        }


        public List<int> ListOfCheckBoxes
        {
            get { return listOfCheckBoxes; }
            set { listOfCheckBoxes = value; }
        }



        public int RotationID
        {
            get { return rotationID; }
            set { rotationID = value; }
        }




        public string RotationName
        {
            get { return rotationName; }
            set { rotationName = value; }
        }


        public int RotationNumber
        {
            get { return rotationNumber; }
            set { rotationNumber = value; }
        }


        public string StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }


        public string EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }


        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }


        public string CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }


        public string UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }


        public string UpdatedDate
        {
            get { return updatedDate; }
            set { updatedDate = value; }
        }


        public string AccountID
        {
            get { return accountID; }
            set { accountID = value; }
        }
        public Guid UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        public string OrderBy
        {
            get { return orderBy; }
            set { orderBy = value; }
        }

    }
}
