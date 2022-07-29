using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistent.Contexts
{
    static class Configuration
    {
        public static string ConnectionString
        {
            get
            {
                ConfigurationManager configurationManager = new(); //dotnet 6 da geldi 

                  configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/ETicaretAPI.API/")); //json dosyası alınacak katmanı belirtiyoruz.
               
                configurationManager.AddJsonFile("appsettings.json");

                return configurationManager.GetConnectionString("PostreSQL");
            } 
        }
    }
}
