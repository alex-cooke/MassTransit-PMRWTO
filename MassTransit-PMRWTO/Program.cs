using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MassTransit.Configuration;
using System;

namespace PMRWTO
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
                        x.AddConsumer<ParentMessageConsumer>();

                        //  Enable the transactional outbox
                        x.AddEntityFrameworkOutbox<OutboxContext>(o =>
                        {
                            o.UseSqlServer();
                            o.UseBusOutbox();
                        });

                        //x.UsingInMemory((context, cfg) =>
                        //{
                        //    cfg.ConfigureEndpoints(context);
                        //});

                        // Use RabbitMQ
                        // docker run -p 15672:15672 -p 5672:5672 masstransit/rabbitmq
                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.ConfigureEndpoints(context);
                        });

                    });

                    services.AddHostedService<Worker>();
                });
    }
}
