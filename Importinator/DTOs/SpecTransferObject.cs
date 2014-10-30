using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Content_Centre_API_CORS.Messaging
{
    public class SpecTransferObject
    {
        private int specID = -1;
        private int rotationID = -1;
        private string roleID = string.Empty;
        private string accountID = string.Empty;
        private string code = string.Empty;
        private string cClass = string.Empty;
        private string rotation = string.Empty;
        private string station = string.Empty;
        private string data = string.Empty;
        private string text = string.Empty;
        private string validDate = string.Empty;
        private Guid userId = Guid.Empty;
        private string fileName = string.Empty;

        private int resultsPerPage = -1;
        private int pageCount = -1;
        private string catererName = string.Empty;
        private string note = string.Empty;

        private string orderBy = string.Empty;
        private int direction = -1;

        private int statusID = -1;
        private int oldStatusID = -1;

        private int mealTypeID = -1;

        public string Departure { get; set; }
        public string Arrival { get; set; }
        public string Lot { get; set; }


        public string AccountID
        {
            get { return accountID; }
            set { accountID = value; }
        }


        public string RoleID
        {
            get { return roleID; }
            set { roleID = value; }
        }



        public int SpecID
        {
            get { return specID; }
            set { specID = value; }
        }



        public int RotationID
        {
            get { return rotationID; }
            set { rotationID = value; }
        }


        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        public string Class
        {
            get { return cClass; }
            set { cClass = value; }
        }

        public string Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public string Station
        {
            get { return station; }
            set { station = value; }
        }

        public string Data
        {
            get { return data; }
            set { data = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public string ValidDate
        {
            get { return validDate; }
            set { this.validDate = value; }
        }

        public Guid UserID
        {
            get { return userId; }
            set { userId = value; }
        }

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        public int ResultsPerPage
        {
            get { return resultsPerPage; }
            set { resultsPerPage = value; }
        }

        public int PageCount
        {
            get { return pageCount; }
            set { pageCount = value; }
        }



        public string CatererName
        {
            get { return catererName; }
            set { catererName = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        public string OrderBy
        {
            get { return orderBy; }
            set { orderBy = value; }
        }

        public int Direction
        {
            get { return direction; }
            set { direction = value; }
        }


        public int StatusID
        {
            get { return statusID; }
            set { statusID = value; }
        }

        public int OldStatusID
        {
            get { return oldStatusID; }
            set { oldStatusID = value; }
        }

        public int MealTypeID
        {
            get { return mealTypeID; }
            set { mealTypeID = value; }
        }
    }
}
