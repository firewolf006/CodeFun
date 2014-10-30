using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Content_Centre_API_CORS.Messaging
{
    public class MealTypeTransferObject
    {
        private int mealTypeID = -1;
        private string mealType = string.Empty;
        private string accountID = string.Empty;


        public int MealTypeID
        {
            get { return mealTypeID; }
            set { mealTypeID = value; }
        }

        public string MealType
        {
            get { return mealType; }
            set { mealType = value; }
        }

        public string AccountID
        {
            get { return accountID; }
            set { accountID = value; }
        }
    }
}
