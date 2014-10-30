using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagement_API.Models.DTO
{
    public class documentDTO
    {
        public string filePath;
        public int typeID;
        public int status;
        public languageDTO translateDirections;
        public int sourceLanguageID;
        //public int targetLanguageID[]; // must generate?

        public int version;
        public int documentID;
        public int projectID;
        public string name;
    }
}