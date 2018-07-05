using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Taxology.Site
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Running Taxology Site!");

            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<SiteStartup>()
                .UseKestrel()
                .UseUrls("http://*:80/")
                .Build();
    }
}
