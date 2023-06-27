using AzureServices;
using AzureSolutionsTestWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AzureSolutionsTestWebApp.Pages
{
    public class StorageQueueTestModel : PageModel
    {
        private readonly ILogger _logger;
        [BindProperty]
        public MessageFormModel Model { get; set; }
        public StorageQueueTestModel(ILogger<StorageQueueTestModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            
        }
      
        public async void OnGetPeek(int? count)
        {
            var client = new QueueStorageClient(Model.ConnectionString, Model.QueName);
            var response = await client.PeekMessage(count);
            
        }

        public async void OnGetDequeue(int? count)
        {
            if(count > 0)
            {
                var client = new QueueStorageClient(Model.ConnectionString, Model.QueName);
                var response = await client.DequeueMessages(count.HasValue ? count.Value : 0);
            }
        }

        public void OnPost()
        {
            if(ValidAll())
            {
                var client = new QueueStorageClient(Model.ConnectionString, Model.QueName);
                var result = client.InsertMessage(Model.Message).Result;
                if(result)
                {
                    _logger.Log(LogLevel.Information, "Succesfully!");
                }
                else
                {
                    _logger.Log(LogLevel.Error, $"Something error with: {Model.Message}");
                }
            }
        }

        private bool ValidAll(params string[] values)
        {
            foreach(var value in values)
            {
                if (string.IsNullOrEmpty(value)) return false;
            }
            return true;
        }

    }
}
