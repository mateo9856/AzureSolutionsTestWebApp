using Azure.Messaging.ServiceBus;

namespace AzureServices
{
    public class SerBusClient
    {
        public string ConnectionString { get; set; }
        public string QueueName { get; set; }
        private ServiceBusClient SbClient { get; set; }
        private ServiceBusSender SbSender { get; set; }

        public SerBusClient(string connString, string queName)
        {
            ConnectionString = connString;
            QueueName = queName;
            SbClient = new ServiceBusClient(connString);
            SbSender = SbClient.CreateSender(queName);
        }
    }
}