using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Order.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running Order Service!");


            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://*:8084/")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<OrderStartup>()
                .Build();

            host.Run();
        }
    }
}
