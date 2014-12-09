using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
//using ProjectManagement_API.Models.DTO;

namespace Importinator
{
    public class DataList
    {
        public String key; // Specification
        public List<MEAL> mealList;
        public String datemaj;
        public String validityBegin;
        public String validityEnd;
        public String caterer;
        public List<DALIST> departureArrivalList;
        public String pairingGrid;
        public String cycle;
        public String state;
        public String version;
        public String language;
        public List<CLASSCODE> classCodeList;
        public String mealType;
        public List<MENU> menusList;
        public int OSCKey = -1;

        public string specMatchKey;
        public string newCode;
        public string newClass;
        public string newStation;
        public int tempSpecID = -1;

    }

    public class DAListCode
    {
        public String code;
    }

    public class DALIST
    {
        public DAListCode departure;
        public DAListCode arrival;
    }

    public class CLASSCODE
    {
        public String code;
    }

    public class MEAL
    {
        public String code;
    }

    public class COURSE
    {
        public String label;
        public String content;
        public String sequence;
        public List<ALLERGEN> allergens = new List<ALLERGEN>();
    }

    public class MENU
    {
        public List<COURSE> coursesList;
    }

    public class ALLERGEN
    {
        public String allergen =string.Empty;
    }

    public class theList
    {
        public List<DataList> menuDataList;
    }

    public class responseObject
    {
        public DataTable results;
        public string JsonResults;
    }
}


//    // allergen fun
//    public class AllergenTransferObject
//    {
//        string accountID = string.Empty;
//        Guid userID = Guid.Empty;

//        string value = string.Empty;
//        int allergenID = -1;


//        public string AccountID
//        {
//            get { return accountID; }
//            set { accountID = value; }
//        }

//        public Guid UserID
//        {
//            get { return userID; }
//            set { userID = value; }
//        }

//        public string Value
//        {
//            get { return value; }
//            set { this.value = value; }
//        }

//        public int AllergenID
//        {
//            get { return allergenID; }
//            set { allergenID = value; }
//        }
//    }
//}
