using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WolvoxSoapService.ConfigHelper
{
    public class Constants
    {
        public static string ConnectionString = AppConfigHelper.GetConnectionString("FirebirdConnectionString");

    }
}