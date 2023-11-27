using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MassTransit.Configuration;
using System;

namespace GettingStarted
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {

                    services.AddDbContext<OutboxContext>(x =>
                    {
                        var connectionString = hostContext.Configuration.GetConnectionString("Default");

                        x.UseSqlServer(connectionString);

                    });


                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<MessageConsumer>();
                        x.AddConsumer<AllMessageConsumer>();

                        x.AddEntityFrameworkOutbox<OutboxContext>(o =>
                        {
                            o.UseSqlServer();
                            o.UseBusOutbox();
                        });

                        //x.UsingInMemory((context, cfg) =>
                        //{
                        //    cfg.ConfigureEndpoints(context);
                        //});

                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.ConfigureEndpoints(context);
                        });



                    });

                    services.AddHostedService<Worker>();
                });
    }
}
