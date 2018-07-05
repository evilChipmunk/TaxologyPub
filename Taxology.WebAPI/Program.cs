using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Taxology.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Running Web API!");
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<APIStartup>()
                .UseKestrel() 
                .UseUrls("http://*:5000/")
                .Build();
    }
}
