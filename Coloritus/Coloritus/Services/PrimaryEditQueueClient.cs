using System;
using Azure.Storage.Queues;

namespace Coloritus.Services;

public class PrimaryEditQueueClient : QueueClient
{
    public PrimaryEditQueueClient(string connectionString) : base(connectionString, Environment.GetEnvironmentVariable("PrimaryEditQueue"), new QueueClientOptions(){MessageEncoding = QueueMessageEncoding.Base64})
    {
    }
}