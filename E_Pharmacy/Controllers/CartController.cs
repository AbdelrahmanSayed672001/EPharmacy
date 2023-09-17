using E_Pharmacy.Models;
using E_Pharmacy.Sefvices.CartServices;
using E_Pharmacy.Sefvices.MedicineServices;
using E_Pharmacy.Sefvices.ShortCommingServices;
using E_Pharmacy.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Pharmacy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;
        private readonly IMedicine medicine ;
        private readonly IShortComming shortComming ;
        private readonly UserManager<ApplicationUser> userManager;

        public CartController(ICartService cartService, IMedicine medicine, UserManager<ApplicationUser> userManager, IShortComming shortComming)
        {
            this.cartService = cartService;
            this.medicine = medicine;
            this.userManager = userManager;
            this.shortComming = shortComming;
        }

        [HttpPost]
        public async Task<IActionResult> Add(CartModel model)
        {
            var user =await userManager.FindByEmailAsync(model.Username);
            var med = await medicine.GetByName(model.Medicine);

            if (user == null)return BadRequest($"{model.Username} is not existed");

            if (med is null) return BadRequest($"{model.Medicine} is not existed");

            if (model.Quantity <= 0)
                model.Quantity = 1;
            
            var cart = new Cart
            {
                Date = DateTime.Now,
                medicine = med,
                Quantity = model.Quantity,
                User = user,
                Price = model.Quantity * med.Price
            };

            if (!await cartService.isExisted(cart))
                cart.Quantity += 1;
                //return BadRequest($"{cart.medicine.Name} is already added to your cart");
            if (cart.Quantity >= med.Quantity) return BadRequest("Invalid quantity");

            await cartService.AddtoCartAsync(cart);
            med.Quantity -= model.Quantity;
            medicine.Update(med);

            var warningMSG= string.Empty;
            if(med.Quantity <= 5)
            {
                var result= await shortComming.isExisted(med.Name);
                var shrt = new ShortComming { medicine = med };
                if (result is null)
                {
                    await shortComming.Add(shrt);
                    warningMSG = $"{med.Name} is added to shortcomming table";
                }

            }
            
            return Ok($"{cart.medicine.Name} is added to your cart");
            //return Ok($"Date: {cart.Date} \nUser: {cart.User} " +
            //    $"\nMedicine: {cart.medicine.Name} \nQuantity: {cart.Quantity} " +
            //    $"\nPrice: {cart.Price} " +
            //    $"\n{warningMSG} ");
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAsync()
        {
            
            //var result = await cartService.GetAllAsync();
            var result = await cartService.GetAllAsync();
            return Ok(result);
        }
        
        [HttpGet("GetByUsername")]
        public async Task<IActionResult> GetByUsernameAsync([FromQuery] string name)
        {
            
            var result = await cartService.GetByUsername(name);
            if (result.Count() ==0)
                return NotFound($"{name} is not found");
            return Ok(result);
        }

        [HttpGet("Calc")]
        public async Task<IActionResult> CalcTotalPrice([FromQuery] GetCheck model)
        {
            var user = await userManager.FindByEmailAsync(model.Username);

            if (user == null) return BadRequest($"{model.Username} is not existed");

            var result= await cartService.CalculateTotalPrice(user.UserName);

            return Ok($"Total: {result}");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync([FromQuery] GetCheck model)
        {
            var user = await userManager.FindByEmailAsync(model.Username);

            if (user == null) return BadRequest($"{model.Username} is not existed");

            return Ok(await cartService.Delete(user.UserName));
            
        }

    }
}
