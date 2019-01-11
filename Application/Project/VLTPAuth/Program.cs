using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace VLTPAuth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }


      // export APPSETTINGS_DIRECTORY="/Users/leetrent/Development/Software/GSA/HR/VLTPAuth/MySQL"
      public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
          WebHost.CreateDefaultBuilder(args)
              .ConfigureAppConfiguration((hostingContext, config) =>
              {
                  config.SetBasePath(Environment.GetEnvironmentVariable("APPSETTINGS_DIRECTORY"));
                  config.AddJsonFile("VLTPAuth_appsettings.json", optional: false, reloadOnChange: true);
              })
              .UseStartup<Startup>();                
    }
}
