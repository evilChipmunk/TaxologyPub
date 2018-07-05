using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace ShoppingCart.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running Shopping Cart Service!");


            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://*:8083/")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<ShoppingStartup>()
                .Build();

            host.Run();
        }
    }
}
