using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagement_API.Models.DTO
{
    public class invoiceDTO
    {
        public double price;
        public string status;
        public double version;
        public int documentID;
        public int invoiceID;
    }
}