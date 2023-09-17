using E_Pharmacy.Sefvices.AuthenticationServices;
using E_Pharmacy.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Pharmacy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles ="Admin")]
    public class RoleController : ControllerBase
    {
        private readonly IAuthService authService;

        public RoleController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole(AddToRole model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await authService.AddRoleAsync(model);
            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(model);
        }
    }
}
