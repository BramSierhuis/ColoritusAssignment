using Azure;
using Azure.Data.Tables;
using Coloritus.Models.Enums;

namespace Coloritus.Models.Entities;

public class ImageRecord : ITableEntity
{
    public string FileName { get; set; }
    public Status Status { get; set; }
    public string PrimaryColor { get; set; }    
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}