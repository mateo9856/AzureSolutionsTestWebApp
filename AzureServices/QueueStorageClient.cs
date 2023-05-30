using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureServices
{
    public class QueueStorageClient : IDisposable
    {
        private QueueClient Client { get; set; }

        public QueueStorageClient(string connString, string queue)
        {
            Client = new QueueClient(connString, queue);
            Client.CreateIfNotExistsAsync();
        }

        public async Task<bool> InsertMessage(string message)
        {
            if(Client.Exists())
            {
                await Client.SendMessageAsync(message);
                return true;
            }
            return false;
        }

        public async Task<PeekedMessage[]> PeekMessage(int? maxMessages = null)
        {
            if(Client.Exists())
            {
                var messages = maxMessages.HasValue ? await Client.PeekMessagesAsync(maxMessages.Value)
                    : await Client.PeekMessagesAsync();

                return messages.Value;

            }
            return null;
        }

        public async Task<BinaryData[]> DequeueMessages(int count)
        {
            if(Client.Exists())
            {
                var messages = await Client.ReceiveMessagesAsync(count);
                var returnValue = messages.Value.Select(d => d.Body).ToArray();

                foreach(var item in messages.Value)
                {
                    await Client.DeleteMessageAsync(item.MessageId, item.PopReceipt);
                }

                return returnValue;
            }
            return null;
        }

        public bool DeleteQueue()
        {
            if(Client.Exists())
            {
                Client.Delete();
                return true;
            }

            return false;

        }

        public void Dispose()
        {
            Client = null;
        }
    }

}
