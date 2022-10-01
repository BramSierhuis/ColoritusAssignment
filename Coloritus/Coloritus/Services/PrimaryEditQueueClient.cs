using Azure.Storage.Queues;

namespace Coloritus.Services;

public class PrimaryEditQueueClient : QueueClient
{
    public PrimaryEditQueueClient(string connectionString) : base(connectionString, "primaryeditqueue", new QueueClientOptions(){MessageEncoding = QueueMessageEncoding.Base64})
    {
    }
}