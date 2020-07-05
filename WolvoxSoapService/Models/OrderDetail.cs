using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WolvoxSoapService.Models
{
    public class OrderDetail
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal SellingPriceWithoutVat { get; set; }
        public decimal SellingPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Vat { get; set; }
        public string WarehouseName { get; set; }
        public string Barcode { get; internal set; }
        public string SubProductCode { get; set; }
    }
}
