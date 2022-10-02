using System;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Coloritus.Models.Entities;
using Coloritus.Repositories.Abstract;
using Coloritus.Services.Abstract;

namespace Coloritus.Repositories;

public class TableStorageRepository : ITableStorageRepository
{
    private TableClient GetTableClient()
    {
        var serviceClient = new TableServiceClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"));
        var tableClient = serviceClient.GetTableClient(Environment.GetEnvironmentVariable("ImageTable"));
        
        return tableClient;
    }

    public async Task<ImageRecord> GetEntityAsync(string partitionKey, string id)
    {
        var tableClient = GetTableClient();
        return await tableClient.GetEntityAsync<ImageRecord>(partitionKey, id);
    }
    
    public async Task<ImageRecord> UpsertEntityAsync(ImageRecord entity)
    {
        var tableClient = GetTableClient();
        await tableClient.UpsertEntityAsync(entity);
        return entity;
    }
}