using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WolvoxSoapService.Models
{
    public class Product
    {
        public string MainProductCode { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public decimal Vat { get; set; }
        public string Description { get; set; }
        public string SpecialCode { get; set; }
        public string Barcode { get; set; }
        public bool IsActive { get; set; }
        public int Stock { get; set; }
        public decimal SellingPrice { get; set; }
        public string Currency { get; set; }
        public string Color { get; set; }
        public string Body { get; set; }
        public string Brand { get; set; }
    }
}