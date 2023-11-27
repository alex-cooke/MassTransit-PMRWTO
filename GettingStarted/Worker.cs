using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GettingStarted
{
    public class Worker : BackgroundService
    {
        IServiceProvider serviceProvider;
        public Worker(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

            //  Let's automatically drop/create the db
            var db = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<OutboxContext>();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                using (var scope = serviceProvider.CreateScope())
                {

                    var publisher = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
                    var context = scope.ServiceProvider.GetRequiredService<OutboxContext>();

                    await publisher.Publish(new Message { Text = $"The time is {DateTimeOffset.Now}" }, stoppingToken);

                    await context.SaveChangesAsync();
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}