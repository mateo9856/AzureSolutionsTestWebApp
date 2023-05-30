using AzureServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AzureSolutionsTestWebApp.Pages
{
    public class StorageQueueTestModel : PageModel
    {
        public void OnGet()
        {
            
        }

        public void OnGetPeek()
        {
            //TODO: Connectionstring and queue saved to cache
        }

        public void OnGetDequeue(int count)
        {
            if(count > 0)
            {
                //TODO: Set connString and queName to save cache
            }
        }

        public async void OnPostInsert(string connectionString, string queName, string message)
        {
            if(ValidAll())
            {
                var client = new QueueStorageClient(connectionString, queName);
                var result = await client.InsertMessage(message);
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
