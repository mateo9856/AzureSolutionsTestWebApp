using Azure.Messaging.ServiceBus;
using System.Diagnostics;

namespace AzureServices
{
    public class SerBusClient : IDisposable
    {
        public string ConnectionString { get; set; }
        public string QueueName { get; set; }
        private ServiceBusClient SbClient { get; set; }
        private ServiceBusSender SbSender { get; set; }
        private ServiceBusProcessor SbProcessor { get; set; }

        public SerBusClient(string connString, string queName)
        {
            ConnectionString = connString;
            QueueName = queName;
            SbClient = new ServiceBusClient(connString);
            SbSender = SbClient.CreateSender(queName);
            SbProcessor = SbClient.CreateProcessor(queName, new ServiceBusProcessorOptions());
        }
        public async Task<bool> CreateMessage(string message)
        {
            bool isCreated = false;
            using ServiceBusMessageBatch messageBatch = await SbSender.CreateMessageBatchAsync();
            for(int i = 1; i <= 5; i++)
            {
                Console.WriteLine("Try create message, attempt: {0}", i);

                isCreated = messageBatch.TryAddMessage(new ServiceBusMessage(message));

                if(isCreated)
                {
                    Console.WriteLine("Message created in {0} attempt.", i);
                    await SbSender.SendMessagesAsync(messageBatch);
                    break;
                }
            }
            if(!isCreated)
            {
                Dispose();
                throw new Exception("Error! Message has not send.");
            }
            Dispose();
            return true;
        }
        async Task MessageHandler(ProcessMessageEventArgs args)
        {
            await args.CompleteMessageAsync(args.Message);
        }
        public async void ReceiveMessages()
        {
            SbProcessor.ProcessMessageAsync += MessageHandler;

            await SbProcessor.StartProcessingAsync();

            await SbProcessor.StopProcessingAsync();
        }

        public async void Dispose()
        {
            await SbClient.DisposeAsync();
            await SbSender.DisposeAsync();
        }
    }
}