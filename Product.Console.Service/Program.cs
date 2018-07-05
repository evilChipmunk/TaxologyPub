using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Product.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running Product Service!");


            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://*:8082/")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<ProductStartup>()
                .Build();

            host.Run();
        }
    }
}
