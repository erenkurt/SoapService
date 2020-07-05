using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WolvoxSoapService.Models
{
    public class OrderFiche
    {
        public string OrderCode { get; set; }
        public string InvoiceCompany { get; set; }
        public string InvoiceTaxno { get; set; }
        public string InvoiceTaxdep { get; set; }
        public string InvoiceMobile { get; set; }
        public string InvoiceAddress { get; set; }
        public string InvoiceCity { get; set; }
        public string InvoiceTown { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryCity { get; set; }
        public string DeliveryTown { get; set; }
        public decimal OrderSubTotal { get; set; }
        public decimal TaxTotal { get; set; }
        public decimal OrderTotalPrice { get; set; }
        public string InvoiceCountry { get; set; }
        public string DeliveryMobile { get; set; }
        public string DeliveryName { get; set; }
        public string WarehouseName { get; set; }
        public string InvoiceName { get; internal set; }
    }
}
