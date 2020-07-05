using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WolvoxSoapService.Models
{
    public class SubProduct
    {
        public string MainProductCode { get; set; }
        public string SubProductCode { get; set; }
        public string Color { get; set; }
        public string Body { get; set; }
        public int Stock { get; set; }
        public string Barcode { get; set; }
    }
}