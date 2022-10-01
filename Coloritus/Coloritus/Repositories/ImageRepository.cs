using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using Coloritus.Extensions;
using Coloritus.Repositories.Abstract;

namespace Coloritus.Repositories;

public class ImageRepository : IImageRepository
{
    public async Task AddAsync(string containerName, byte[] image, string fileName)
    {
        var connection = Environment.GetEnvironmentVariable("StorageConnectionString");
        
        var blobContainerClient = new BlobContainerClient(connection, containerName);
        var blobClient = blobContainerClient.GetBlobClient(fileName);

        using var ms = new MemoryStream(image, false);
        await blobClient.UploadAsync(ms);
    }
    
    public async Task<byte[]> GetAsync(string containerName, string fileName)
    {
        var connection = Environment.GetEnvironmentVariable("StorageConnectionString");
        
        var blobContainerClient = new BlobContainerClient(connection, containerName);
        var blobClient = blobContainerClient.GetBlobClient(fileName);

        var stream = await blobClient.OpenReadAsync();

        return await stream.GetBytes();
    }
    
    public Uri GetUri(string containerName, string fileName)
    {
        var connection = Environment.GetEnvironmentVariable("StorageConnectionString");
        
        var blobContainerClient = new BlobContainerClient(connection, containerName);
        var blobClient = blobContainerClient.GetBlobClient(fileName);

        var sasBuilder = new BlobSasBuilder()
        {
            BlobContainerName = blobClient.GetParentBlobContainerClient().Name,
            BlobName = blobClient.Name,
            Resource = "b"
        };
        
        sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddHours(1);
        sasBuilder.SetPermissions(BlobSasPermissions.Read |
                                  BlobSasPermissions.Write);

        return blobClient.GenerateSasUri(sasBuilder);
    }
}