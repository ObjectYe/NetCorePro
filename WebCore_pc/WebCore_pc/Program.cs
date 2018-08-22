using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WebCore_pc
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hosting.json", optional: true)
                .Build();
             
            CreateWebHostBuilder(args, config).Run();
        }

        public static IWebHost CreateWebHostBuilder(string[] args, IConfiguration config) =>
            WebHost.CreateDefaultBuilder(args)
            .UseConfiguration(config)
            .UseStartup<Startup>()
            .Build();
    }
}
