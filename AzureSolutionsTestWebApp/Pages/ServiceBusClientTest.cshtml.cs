using AzureServices;
using AzureSolutionsTestWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AzureSolutionsTestWebApp.Pages
{
    public class ServiceBusClientTestModel : PageModel
    {
        [BindProperty]
        public MessageFormModel Model { get; set; }
        public void OnGet()
        {
        }

        public void OnPostReceive()
        {
            Console.WriteLine();
        }

        public async void OnPost() {
            if(!string.IsNullOrEmpty(Model.ConnectionString) && !string.IsNullOrEmpty(Model.Message) && !string.IsNullOrEmpty(Model.QueName)) {
                var client = new SerBusClient(Model.ConnectionString, Model.QueName);
                await client.CreateMessage(Model.Message);
            }
        }
    }
}
