using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WolvoxSoapService.Models;

namespace WolvoxSoapService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IWolvoxService" in both code and config file together.
    [ServiceContract]
    public interface IWolvoxService
    {
        [OperationContract]
        List<Product> GetAllProducts(int start, int length);

        [OperationContract]
        List<SubProduct> GetAllSubProducts();

        [OperationContract]
        List<Stock> GetProductsStock(DateTime date); 

        [OperationContract]
        bool AddOrder(Order order);

    }
}
