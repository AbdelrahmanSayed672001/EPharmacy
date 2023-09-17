using E_Pharmacy.Models;
using E_Pharmacy.Sefvices.ShortCommingServices;
using E_Pharmacy.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Pharmacy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShortCommingController : ControllerBase
    {
        private readonly IShortComming shortComming;

        public ShortCommingController(IShortComming shortComming)
        {
            this.shortComming = shortComming;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var res = await shortComming.GetAll();

            if(res.Count() ==0 ) return NotFound("Not Any shortcommings yet");

           
            return Ok(res);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync([FromQuery] GetCheck model)
        {
            var res= await shortComming.isExisted(model.Username);

            if (res is null) return NotFound($"Not found any shortcomming called {model.Username}");

            shortComming.Delete(res);
            return Ok($"{model.Username} is deleted succefully ");
        }
    }
}
