// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SignalRSamples.Hubs;

namespace SignalRSamples
{
    public class Program
    {
        public static Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();

            var host = Host.CreateDefaultBuilder(args)
                .ConfigureWebHost(webHostBuilder =>
                {
                    webHostBuilder
                    .UseConfiguration(config)
                    .UseSetting(WebHostDefaults.PreventHostingStartupKey, "true")
                    .ConfigureLogging((c, factory) =>
                    {
                        factory.AddConfiguration(c.Configuration.GetSection("Logging"));
                        factory.AddConsole();
                    })
                    .UseKestrel(options =>
                    {
                        // Default port
                        options.ListenLocalhost(5000);

                        // Hub bound to TCP end point
                        options.Listen(IPAddress.Any, 9001, builder =>
                        {
                            builder.UseHub<Chat>();
                        });
                    })
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseIISIntegration()
                    .UseStartup<Startup>();
                }).Build();

            return host.RunAsync();
        }
    }
}
