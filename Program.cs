using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DotNetApp
{
    public class Program
    {
      public static void Main(string[] args)
      {
          Globals.ReadEnviromentVariables();
          hostingUrl = (Globals.env["DOTNET_ENV"] == "Production") ? "http://0.0.0.0:5000" : "http://localhost:5000";
          BuildWebHost(args).Run();
      }
      private static string hostingUrl;

      public static IWebHost BuildWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .UseUrls(hostingUrl)
            .Build();
    }
}
