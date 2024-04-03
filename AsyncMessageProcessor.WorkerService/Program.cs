using AsyncMessageProcessor.Application.Configuration;
using AsyncMessageProcessor.Application.Interfaces;
using AsyncMessageProcessor.Application.Services;
using AsyncMessageProcessor.WorkerService;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((hostContext, services) =>
{
    // Configuración de servicios
    services.ConfigureServices();
 
});

builder.ConfigureServices((hostContext, services) =>
{
    // Agregar el Worker Service
    services.AddHostedService<Worker>();
});

var host = builder.Build();
host.Run();
