using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagement_API.Models.DTO
{
    public class resourceDTO
    {
        public string firstName;
        public string lastName;
        public string company;
        public string address;
        public string type;
        public languageDTO languageAssociations;
        public List<documentDTO> documents;
        public bool isCompany;
        public int resourceID;

    }
}