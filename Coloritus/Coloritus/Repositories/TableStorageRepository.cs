using System;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Coloritus.Models.Entities;
using Coloritus.Repositories.Abstract;
using Coloritus.Services.Abstract;

namespace Coloritus.Repositories;

public class TableStorageRepository : ITableStorageRepository
{
    private const string TableName = "ImageRecord";

    private TableClient GetTableClient()
    {
        var serviceClient = new TableServiceClient(Environment.GetEnvironmentVariable("StorageConnectionString"));
        var tableClient = serviceClient.GetTableClient(TableName);
        
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