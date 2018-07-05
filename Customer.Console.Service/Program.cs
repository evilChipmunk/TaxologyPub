using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Customer.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running Customer Service!");


            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://*:8081/")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<CustomerStartup>()
                .Build();

            host.Run();
        }
    }
}
