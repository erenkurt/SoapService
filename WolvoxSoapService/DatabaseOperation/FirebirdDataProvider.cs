using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace WolvoxSoapService.DatabaseOperation
{
    public class FirebirdDataProvider
    {
        public FbConnection FbConnection { get; set; }
        public FbCommand FbCommand { get; set; }


        public FirebirdDataProvider(string connectionString)
        {
            this.FbConnection = new FbConnection(connectionString);
            this.FbCommand = this.FbConnection.CreateCommand();

        }

        public DataTable GetDataTable(string sql)
        {
            DataTable dataTable = new DataTable();
            this.FbCommand.CommandText = sql;
            try
            {
                FbDataAdapter fbDataAdapter = new FbDataAdapter(this.FbCommand);
                fbDataAdapter.Fill(dataTable);
            }
            catch (Exception exception)
            {

                throw new FaultException(exception.Message);
            }
           

            return dataTable;
        }

        public object ExeCuteScalar(string sql)
        {
            this.FbCommand.CommandText = sql;
            this.FbConnection.Open();
            var retVal = FbCommand.ExecuteScalar();
            this.FbConnection.Close();
            return retVal;
        }
    }
}