using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WolvoxSoapService.Models
{
    public class Order
    {
        public OrderFiche OrderFiche { get; set; }
        public List<OrderDetail> OrderDetail { get; set; }
        public Customer Customer { get; set; }
    }
}