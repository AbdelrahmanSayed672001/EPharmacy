using E_Pharmacy.Models;
using E_Pharmacy.Sefvices.AuthenticationServices;
using E_Pharmacy.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Pharmacy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthenticationController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result=await authService.RegisterAsync(model);
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }
        
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result=await authService.LoginAsync(model);
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }
        
        
    }
}
