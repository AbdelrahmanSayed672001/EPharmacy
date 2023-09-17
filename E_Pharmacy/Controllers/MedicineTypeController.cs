using E_Pharmacy.Models;
using E_Pharmacy.Sefvices.MedicineServices;
using E_Pharmacy.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Pharmacy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineTypeController : ControllerBase
    {
        private readonly IMedicineType medicineType;

        public MedicineTypeController(IMedicineType medicineType)
        {
            this.medicineType = medicineType;
        }

        [HttpPost]
        public async Task<IActionResult> AddMedicineType([FromBody] MedicineTypeViewModel model )
        {
            var Type = new MedicineType
            {   
                TypeName = model.TypeName,
            };

            var result = await medicineType.isExist(Type.TypeName);

            if (result is true)
                return BadRequest($"{Type.TypeName} is already existed.");

            await medicineType.Add(Type);
            return Ok($"{Type.TypeName} is added successfully.");
    
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var res = await medicineType.GetAll();

            if (res.Count() == 0)
                return NotFound("Not found ant types yet");

            return Ok(res);
        }
    }
}
