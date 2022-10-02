using System;
using Azure.Storage.Queues;
using Coloritus;
using Coloritus.Controllers;
using Coloritus.Repositories;
using Coloritus.Repositories.Abstract;
using Coloritus.Services;
using Coloritus.Services.Abstract;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Coloritus;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddHttpClient<IColorApiClient, ColorApiClient>()
            .ConfigureHttpClient(httpClient =>
            {
                httpClient.BaseAddress = 
                    new Uri(Environment.GetEnvironmentVariable("ColorApiBaseAddress") 
                            ?? throw new InvalidOperationException());
            });
        
        builder.Services.AddHttpClient<IRelatedTextClient, RelatedTextClient>()
            .ConfigureHttpClient(httpClient =>
            {
                httpClient.BaseAddress = 
                    new Uri(Environment.GetEnvironmentVariable("RelatedTextApiBaseAddress") 
                            ?? throw new InvalidOperationException());
            });

        builder.Services.AddAzureClients(clientFactoryBuilder =>
        {
            clientFactoryBuilder.AddClient<InitialUploadQueueClient, QueueClientOptions>((_, _, _) 
                => new InitialUploadQueueClient(
                    Environment.GetEnvironmentVariable("AzureWebJobsStorage")));
            clientFactoryBuilder.AddClient<PrimaryEditQueueClient, QueueClientOptions>((_, _, _) 
                => new PrimaryEditQueueClient(
                    Environment.GetEnvironmentVariable("AzureWebJobsStorage")));
        });

        builder.Services.AddScoped<IImageRepository, ImageRepository>();
        builder.Services.AddScoped<ITableStorageRepository, TableStorageRepository>();
        builder.Services.AddScoped<IImageService, ImageService>();
        builder.Services.AddScoped<IStatusService, StatusService>();
    }
}