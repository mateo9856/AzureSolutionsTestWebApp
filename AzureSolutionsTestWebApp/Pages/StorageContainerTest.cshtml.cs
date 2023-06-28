using AzureSolutionsTestWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AzureSolutionsTestWebApp.Pages
{
    public class StorageContainerTestModel : PageModel
    {
        public BlobFormModel Model { get; set; }
        public void OnGet()
        {
        }

        public void OnPost()
        {

        }
    }
}
