using Azure.Core.Serialization;
using Azure.Messaging.EventGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureServices
{
    public class EventGridClient
    {
        EventGridPublisherClient Client { get; set; }
        JsonObjectSerializer JsonSerializer { get; set; }
        private List<EventGridEvent> Events { get; set; }
        public EventGridClient(string topicEndpoint, string key)
        {
            Client = new EventGridPublisherClient(new Uri(topicEndpoint),
                new Azure.AzureKeyCredential(key));
            JsonSerializer = new JsonObjectSerializer(
                new System.Text.Json.JsonSerializerOptions()
                {
                    PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
                });
            Events = new List<EventGridEvent>();
        }

        public void AddEvent(string subject, string type, object data)
        {
            Events.Add(new EventGridEvent(subject, type, "1.0", JsonSerializer.Serialize(data)));
        }

        public void AddEventToDomain(string subject, string type, object data, string topic)
        {
            var ev = new EventGridEvent(subject, type, "1.0", JsonSerializer.Serialize(data)) {
                Topic = topic
            };
            Events.Add(ev);
        }

        public async Task<bool> SendEventsAsync()
        {
            try
            {
                await Client.SendEventsAsync(Events);
                return true;
            } catch(Exception ex)
            {
                return false;
            }
            
        }

    }
}
