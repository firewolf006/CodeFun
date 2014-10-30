using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagement_API.Models.DTO
{
    public class projectDTO
    {
        public int projectID;
        public clientDTO client;
        public string projectName;
        public int sourceLanguageID;
        public List<resourceDTO> resources; // DTO
        public List<documentDTO> documents;
        public List<invoiceDTO> invoices; //DTO
        public string trackPath;
        public string trackStatus;
    }
}