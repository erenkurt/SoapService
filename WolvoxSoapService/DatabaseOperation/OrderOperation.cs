using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Web;
using WolvoxSoapService.ConfigHelper;
using WolvoxSoapService.Models;

namespace WolvoxSoapService.DatabaseOperation
{
    public class OrderOperation : IWolvoxService
    {
        FirebirdDataProvider firebirdDataProvider = new FirebirdDataProvider(Constants.ConnectionString);


        public bool AddOrder(Order order)
        {


            string orderControlSql = "Select BLKODU from SIPARIS where SIPARIS_NO = '" + order.OrderFiche.OrderCode + "'";


            var result = firebirdDataProvider.ExeCuteScalar(orderControlSql);
            if (result != null)
            {
                return true;
            }
            //Cari Ekle

            int customerId = AddCustomer(order.Customer);

            //Siparişi ekle  

            int orderId = AddOrderFiche(order.OrderFiche, customerId, order.Customer);

            //Sipariş Detay Ekle
            AddOrderDetail(order.OrderDetail, orderId);

            return true;
        }

        private void AddOrderDetail(List<OrderDetail> orderDetail, int orderId)
        {
            string orderDetailQuery = "";
            using (StreamReader reader = new StreamReader(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "SqlText\\OrderDetail.txt"))
            {
                orderDetailQuery = reader.ReadToEnd();
            }

            foreach (var detail in orderDetail)
            {
                string productCode = detail.SubProductCode == "" ? detail.ProductCode : detail.SubProductCode;
                string productReferenceQuery = "Select BLKODU from STOK where STOKKODU ='" + productCode + "'";

                int stokRef = Convert.ToInt32(firebirdDataProvider.ExeCuteScalar(productReferenceQuery));

                int blCode = Convert.ToInt32(firebirdDataProvider.ExeCuteScalar("select COALESCE(MAX(gen_id(SIPARISHR_GEN, 0)),0) as Blkodu from SIPARISHR"));

                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@Blkodu", blCode + 1);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@SellingPriceWithoutVat", detail.SellingPriceWithoutVat);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@SellingPrice", detail.SellingPrice);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@SiparisId", orderId);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@Vat", detail.Vat);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@ProductCode", detail.SubProductCode != "" ? detail.SubProductCode : detail.ProductCode);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@Barcode", detail.Barcode);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@Quantity", detail.Quantity);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@ProductName", detail.ProductName);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@StokRef", stokRef);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@WarehouseName", detail.WarehouseName);
                try
                {
                    firebirdDataProvider.ExeCuteScalar(orderDetailQuery);
                    firebirdDataProvider.ExeCuteScalar("SET GENERATOR SIPARISHR_GEN TO " + Convert.ToInt32(blCode + 1));
                    firebirdDataProvider.FbCommand.Parameters.Clear();

                }
                catch (Exception exception)
                {

                    throw new FaultException(exception.Message);
                }

            }


        }
        private int AddOrderFiche(OrderFiche orderFiche, int customerId, Customer customer)
        {

            string orderControlSql = "Select BLKODU from SIPARIS where SIPARIS_NO = '" + orderFiche.OrderCode + "'";


            var result = firebirdDataProvider.ExeCuteScalar(orderControlSql);
            if (result != null)
            {
                return Convert.ToInt32(result);
            }
            else
            {
                int retVal = 0;
                string orderFicheQuery = "";
                string orderCustomerCode = firebirdDataProvider.ExeCuteScalar("Select CARIKODU from CARI where BLKODU = " + customerId).ToString();

                using (StreamReader reader = new StreamReader(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "SqlText\\OrderFiche.txt"))
                {
                    orderFicheQuery = reader.ReadToEnd();
                }

                int blCode = Convert.ToInt32(firebirdDataProvider.ExeCuteScalar("select COALESCE(MAX(gen_id(SIPARIS_GEN, 0)),0) as Blkodu from SIPARIS"));

                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@Blkodu", blCode + 1);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@OrderCode", orderFiche.OrderCode);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@UserID", customerId);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@CustomerCode", orderCustomerCode);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@InvoiceCompany", orderFiche.InvoiceCompany);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@InvoiceName", orderFiche.InvoiceName);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@InvoiceTaxno", orderFiche.InvoiceTaxno);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@InvoiceTaxdep", orderFiche.InvoiceTaxdep);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@InvoiceMobile", orderFiche.InvoiceMobile);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@InvoiceAddress", orderFiche.InvoiceAddress);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@InvoiceCity", orderFiche.InvoiceCity);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@InvoiceTown", orderFiche.InvoiceTown);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@DeliveryAddress", orderFiche.DeliveryAddress);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@DeliveryCity", orderFiche.DeliveryCity);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@DeliveryTown", orderFiche.DeliveryTown);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@OrderSubtotal", orderFiche.OrderSubTotal);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@TaxTotal", orderFiche.TaxTotal);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@OrderTotalPrice", orderFiche.OrderTotalPrice);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@WarehouseCode", orderFiche.WarehouseName);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@InvoiceCountry", orderFiche.InvoiceCountry);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@DeliveryMobile", orderFiche.DeliveryMobile);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@DeliveryName", orderFiche.DeliveryName);
                try
                {
                    firebirdDataProvider.ExeCuteScalar(orderFicheQuery);
                    firebirdDataProvider.ExeCuteScalar("SET GENERATOR SIPARIS_GEN TO " + Convert.ToInt32(blCode + 1));
                    retVal = Convert.ToInt32(firebirdDataProvider.ExeCuteScalar("select COALESCE(MAX(gen_id(SIPARIS_GEN, 0)),0) from SIPARIS"));
                    firebirdDataProvider.FbCommand.Parameters.Clear();

                }
                catch (Exception exception)
                {

                    throw new FaultException(exception.Message);

                }
                return retVal;
            }
        }


        private int AddCustomer(Customer customer)
        {
            string customerControlSql = "Select BLKODU from CARI where E_MAIL ='" + customer.Email + "'";
            var result = firebirdDataProvider.ExeCuteScalar(customerControlSql);
            if (result != null)
            {
                return Convert.ToInt32(result);
            }
            else
            {
                string customerQuery = "";
                int retVal = 0;

                using (StreamReader reader = new StreamReader(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "SqlText\\Customer.txt"))
                {
                    customerQuery = reader.ReadToEnd();
                }



                int blCode = Convert.ToInt32(firebirdDataProvider.ExeCuteScalar("select COALESCE(MAX(gen_id(CARI_GEN, 0)),0) as Blkodu from CARI"));

                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@BlKodu", blCode + 1);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@CustomerCode", customer.CustomerCode);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@Name", customer.Name);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@Surname", customer.Surname);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@CompanyName", customer.CompanyName);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@TaxOffice", customer.TaxOffice);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@TaxNo", customer.TaxNo);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@MobilePhone", customer.MobilePhone);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@Phone", customer.Phone);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@Email", customer.Email);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@IsCurrency", customer.IsCurrency);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@CurrencyUnit", customer.CurrencyUnit);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@Address", customer.Address);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@City", customer.City);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@Town", customer.Town);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@Address2", customer.Address2);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@IsActive", customer.IsActive);
                firebirdDataProvider.FbCommand.Parameters.AddWithValue("@BranchCode", customer.BranchCode);
                try
                {
                    firebirdDataProvider.ExeCuteScalar(customerQuery);
                    firebirdDataProvider.ExeCuteScalar("SET GENERATOR CARI_GEN TO " + Convert.ToInt32(blCode + 1));
                    retVal = Convert.ToInt32(firebirdDataProvider.ExeCuteScalar("select COALESCE(MAX(gen_id(CARI_GEN, 0)),0) from CARI"));
                    firebirdDataProvider.FbCommand.Parameters.Clear();

                }
                catch (Exception exception)
                {

                    throw new FaultException(exception.Message);

                }

                return retVal;
            }


        }

        public List<Product> GetAllProducts(int start, int length)
        {
            throw new NotImplementedException();
        }

        public List<SubProduct> GetAllSubProducts()
        {
            throw new NotImplementedException();
        }

        public List<Stock> GetProductsStock(DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}