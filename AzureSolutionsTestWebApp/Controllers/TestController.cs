using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AzureSolutionsTestWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public TestController()
        {

        }

        [HttpGet]
        public IActionResult ReturnRandomNumArray()
        {
            var random = new Random();
            int[] arr = new int[random.Next(3, 10)];

            for(int i = 0; i< arr.Length; i++)
            {
                arr[i] = random.Next(0, 20);
            }

            return Ok(arr);
        }

    }
}
