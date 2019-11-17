using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem
{
    namespace LibraryManagementSystem
    {
        public class Program
        {
            public static void Main(string[] args)
            {
                var host = CreateWebHostBuilder(args).Build();


                //Seed database
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    try
                    {
                        var databaseInitializer = services.GetRequiredService<IDatabaseInitializer>();
                        databaseInitializer.SeedAsync().Wait();
                    }
                    catch (Exception ex)
                    {
                       throw new Exception(ex.Message);
                    }
                }

                host.Run();
            }

            public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
                WebHost.CreateDefaultBuilder(args)
                    .UseStartup<Startup>();                    
        }
    }

}
