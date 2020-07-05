using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WolvoxSoapService.Models
{
    public class Customer
    {
        public string CustomerCode { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string CompanyName { get; set; }
        public string TaxOffice { get; set; }
        public string TaxNo { get; set; }
        public string MobilePhone { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string IsCurrency { get; set; }
        public string CurrencyUnit { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Town { get; set; }
        public string Address2 { get; set; }
        public int IsActive { get; set; }
        public string BranchCode { get; set; }

    }
}