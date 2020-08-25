using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WolvoxSoapService.ConfigHelper;
using WolvoxSoapService.Models;

namespace WolvoxSoapService.DatabaseOperation
{
    public class ProductOperation : IWolvoxService
    {
        FirebirdDataProvider provider = new FirebirdDataProvider(Constants.ConnectionString);

        public bool AddOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetAllProducts(int start, int length)
        {
            List<Product> productList = new List<Product>();

            string query = "select (case when(s.ANA_STOKKODU) is null then s.STOKKODU else s.ANA_STOKKODU end) as MainProductCode, s.STOKKODU as ProductCode, s.STOK_ADI as ProductName, s.MARKASI as Brand, s.OZEL_KODU1 as SpecialCode, s.BIRIMI as Unit,s.KDV_ORANI as Vat, s.ACIKLAMA1 as Description, s.AKTIF as IsActive, s.BARKODU as Barcode, s.RENK as Color, s.BEDEN as Body,f.FIYATI as SellingPrice, ( sum( case when(KPB_GMIK is null) or(KPB_GMIK = 0) then 0 when KPB_GMIK is not null then KPB_GMIK end) - sum( case when(KPB_CMIK is null) or(KPB_CMIK = 0) then 0 when KPB_CMIK is not null then KPB_CMIK end)) as Stock,(CASE when f.HESAP= '€' then 'EUR' WHEN f.HESAP= '$' THEN 'USD' else 'TL' end) as Currency from STOK as s left join STOKHR as stkh on s.BLKODU = stkh.BLSTKODU left join STOK_FIYAT as f on s.BLKODU = f.BLSTKODU and f.ALIS_SATIS = 2 and f.FIYAT_NO = 1 Group By s.ANA_STOKKODU,s.STOKKODU ,s.STOK_ADI,s.MARKASI ,s.OZEL_KODU1,s.BIRIMI,s.KDV_ORANI,s.ACIKLAMA1,s.AKTIF,s.BARKODU,s.RENK,s.BEDEN,f.FIYATI,f.HESAP ";
            DataTable dt = provider.GetDataTable(query);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Product product = new Product
                {
                    MainProductCode = dt.Rows[i]["MAINPRODUCTCODE"] != DBNull.Value ? dt.Rows[i]["MAINPRODUCTCODE"].ToString() : "",
                    ProductCode = dt.Rows[i]["PRODUCTCODE"] != DBNull.Value ? dt.Rows[i]["PRODUCTCODE"].ToString() : "",
                    ProductName = dt.Rows[i]["PRODUCTNAME"] != DBNull.Value ? dt.Rows[i]["PRODUCTNAME"].ToString() : "",
                    Brand = dt.Rows[i]["BRAND"] != DBNull.Value ? dt.Rows[i]["BRAND"].ToString() : "",
                    Body = dt.Rows[i]["BODY"] != DBNull.Value ? dt.Rows[i]["BODY"].ToString() : "",
                    Color = dt.Rows[i]["COLOR"] != DBNull.Value ? dt.Rows[i]["COLOR"].ToString() : "",
                    Barcode = dt.Rows[i]["BARCODE"] != DBNull.Value ? dt.Rows[i]["BARCODE"].ToString() : "",
                    Description = dt.Rows[i]["DESCRIPTION"] != DBNull.Value ? dt.Rows[i]["DESCRIPTION"].ToString() : "",
                    SpecialCode = dt.Rows[i]["SPECIALCODE"] != DBNull.Value ? dt.Rows[i]["SPECIALCODE"].ToString() : "",
                    Unit = dt.Rows[i]["UNIT"] != DBNull.Value ? dt.Rows[i]["UNIT"].ToString() : "",
                    Vat = dt.Rows[i]["VAT"] != DBNull.Value ? Convert.ToDecimal(dt.Rows[i]["VAT"]) : Convert.ToDecimal(0),
                    Stock = dt.Rows[i]["STOCK"] != DBNull.Value ? Convert.ToInt32(dt.Rows[i]["STOCK"]) : 0,
                    IsActive = dt.Rows[i]["ISACTIVE"] != DBNull.Value ? Convert.ToBoolean(dt.Rows[i]["ISACTIVE"]) : false,
                    SellingPrice = dt.Rows[i]["SELLINGPRICE"] != DBNull.Value ? Convert.ToDecimal(dt.Rows[i]["SELLINGPRICE"]) : Convert.ToDecimal(0),
                    Currency = dt.Rows[i]["CURRENCY"] != DBNull.Value ? dt.Rows[i]["CURRENCY"].ToString() : ""

                };

                productList.Add(product);
            }


            return productList.Skip(start).Take(length).ToList();
        }

        public List<SubProduct> GetAllSubProducts()
        {
            List<SubProduct> subProductList = new List<SubProduct>();

            string query = "Select st.ANA_STOKKODU as MainProductCode,st.STOKKODU as SubProductCode, st.BARKODU as Barcode,st.RENK as Color, st.BEDEN as Body," +
    "( sum( case when(KPB_GMIK is null) or(KPB_GMIK = 0) then 0 when KPB_GMIK is not null then KPB_GMIK end)" +
    "- sum( case when(KPB_CMIK is null) or(KPB_CMIK = 0) then 0 when KPB_CMIK is not null then KPB_CMIK end)" +
    ") as Stock" +
    " from STOK as st left join STOKHR as stkh on st.BLKODU = stkh.BLSTKODU where ANA_STOK = 0" +
    "Group By st.ANA_STOKKODU,st.STOKKODU ,st.BARKODU ,st.RENK ,st.BEDEN";

            DataTable dt = provider.GetDataTable(query);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SubProduct subProduct = new SubProduct
                {
                    MainProductCode = dt.Rows[i]["MAINPRODUCTCODE"].ToString(),
                    SubProductCode = dt.Rows[i]["SUBPRODUCTCODE"].ToString(),
                    Barcode = dt.Rows[i]["BARCODE"].ToString(),
                    Body = dt.Rows[i]["BODY"].ToString(),
                    Color = dt.Rows[i]["COLOR"].ToString(),
                    Stock = Convert.ToInt32(dt.Rows[i]["STOCK"])
                };

                subProductList.Add(subProduct);
            }
            return subProductList;
        }



        public List<Stock> GetProductsStock(DateTime date)
        {
            List<Stock> productQuantityList = new List<Stock>();

            string query = "SELECT ST.STOKKODU AS ProductCode,(sum(case when (KPB_GMIK is null) or (KPB_GMIK=0) then 0 when KPB_GMIK is not null then KPB_GMIK end)-sum(case when (KPB_CMIK is null) or (KPB_CMIK=0) then 0 when KPB_CMIK is not null then KPB_CMIK end)) AS Stock FROM STOKHR AS STHR inner join stok as ST on  STHR.BLSTKODU=ST.BLKODU where STHR.KAYIT_TARIHI >= @CreatedDate    GROUP BY ST.STOKKODU  ";

            provider.FbCommand.Parameters.AddWithValue("@CreatedDate", date);

            DataTable dt = provider.GetDataTable(query);


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Stock stock = new Stock
                {
                    ProductCode = dt.Rows[i]["PRODUCTCODE"].ToString(),
                    Quantity = Convert.ToInt32(dt.Rows[i]["STOCK"])
                };

                productQuantityList.Add(stock);
            }

            return productQuantityList;
        }
    }
}