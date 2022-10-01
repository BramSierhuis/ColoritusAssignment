using Azure.Storage.Queues;

namespace Coloritus.Services;

public class InitialUploadQueueClient : QueueClient
{
    public InitialUploadQueueClient(string connectionString) : base(connectionString, "initialuploadqueue", new QueueClientOptions(){MessageEncoding = QueueMessageEncoding.Base64})
    {
    }
}