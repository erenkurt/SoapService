using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WolvoxSoapService.DatabaseOperation;
using WolvoxSoapService.Models;

namespace WolvoxSoapService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "WolvoxService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select WolvoxService.svc or WolvoxService.svc.cs at the Solution Explorer and start debugging.
    public class WolvoxService : IWolvoxService
    {
        ProductOperation productOperation = new ProductOperation();

        public bool AddOrder(Order order)
        {
            OrderOperation orderOperation = new OrderOperation();
            try
            {
                orderOperation.AddOrder(order);
                return true;
            }
            catch (Exception ex)
            {

                throw new FaultException(ex.Message);
            }


        }

        public List<Product> GetAllProducts(int start, int length)
        {

            return productOperation.GetAllProducts(start, length);
        }

        public List<SubProduct> GetAllSubProducts()
        {
            return productOperation.GetAllSubProducts();
        }

        public List<Stock> GetProductsStock(DateTime date)
        {
            return productOperation.GetProductsStock(date);
        }
    }
}
