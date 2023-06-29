using AzureServices;
using AzureSolutionsTestWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AzureSolutionsTestWebApp.Pages
{
    public class StorageContainerTestModel : PageModel
    {
        [BindProperty]
        public BlobFormModel Model { get; set; }
        [BindProperty]
        public string? ExceptionMessage { get; set; }

        private readonly ILogger _logger;

        public StorageContainerTestModel(ILogger<StorageContainerTestModel> logger)
        {
            _logger = logger;
            ExceptionMessage = null;
        }

        public void OnGet()
        {

        }

        public async Task OnPostCreateController()
        {
            try
            {
                var client = new AzureStorageClient(Model.AccountName);
                await client.CreateContainerAsync(Model.ContainerName);

            } catch (Exception ex)
            {
               ExceptionMessage = ex.Message;
            }
        }

        public async Task OnPostRemoveController()
        {
            try
            {
                var client = new AzureStorageClient(Model.AccountName);
                await client.DeleteContainerAsync(Model.ContainerName);

            } catch (Exception ex)
            {
                ExceptionMessage = ex.Message;
            }
        }
        public async Task OnPostUpload()
        {
            if(Model.UploadedFile == null)
            {
                ExceptionMessage = "IFormFile is null !!";
                return;
            }
            try
            {
                var client = new AzureStorageClient(Model.AccountName);
                //TODO:Set method to use IFormFile as a Parameter
                await client.UploadToContainerAsync(Directory.GetCurrentDirectory(), Model.UploadedFile.FileName);

            } catch(Exception ex)
            {
                ExceptionMessage = ex.Message;
            }
        }
    }
}
