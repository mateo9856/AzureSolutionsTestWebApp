using AzureServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AzureSolutionsTestWebApp.Pages
{
    public class ServiceBusClientTestModel : PageModel
    {
        public void OnGet()
        {
        }

        public void OnPostReceive(string connectionString, string queName)
        {
            Console.WriteLine();
        }

        public async void OnPost(string connectionString, string queName, string message) {
            if(!string.IsNullOrEmpty(connectionString) && !string.IsNullOrEmpty(message) && !string.IsNullOrEmpty(queName)) {
                var client = new SerBusClient(connectionString, queName);
                await client.CreateMessage(message);
            }
        }
    }
}
