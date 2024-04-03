using AsyncMessageProcessor.Application.Interfaces;
using AsyncMessageProcessor.Application.Services;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncMessageProcessor.Application.Configuration
{
    public static class ApplicationConfigure
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            // Otras configuraciones

            services.AddTransient<IEmailService, EmailService>()
                .AddTransient<IRabbitMQManager, RabbitMQManager>()
                .AddTransient<ISendEmailService, SendEmailService>();
            return services;

            // Otras configuraciones
        }

    }
}
