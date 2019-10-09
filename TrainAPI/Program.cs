using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NLog.Web;
using Microsoft.Extensions.Logging;

namespace TrainAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                        .UseKestrel()
                        .UseUrls("http://0.0.0.0:51841/")
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseIISIntegration()
                        .UseStartup<Startup>()
                        .ConfigureLogging(logging =>
                        {
                            logging.ClearProviders();
                        })
                        .UseNLog()  // NLog: setup NLog for Dependency injection
                        .UseConfiguration(new ConfigurationBuilder()
                                                .AddCommandLine(args)
                                                .Build());
    }
}
