using E_Pharmacy.Models;
using E_Pharmacy.Sefvices.MedicineServices;
using E_Pharmacy.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Pharmacy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private readonly IMedicine medicine;
        private const double PAGE_SIZE=3.0;

        public MedicineController(IMedicine medicine)
        {
            this.medicine = medicine;
        }

        [HttpGet("GetAllPagination/{page}")]
        public async Task<IActionResult> GetAllPagination(int page)
        {
            var result = await medicine.GetPagination(page, PAGE_SIZE);
            if (result.Count() == 0) return NotFound("Not found any medicine yet");
            var all = await medicine.GetAll();
            double size=all.Count() / PAGE_SIZE;
            
            return Ok(result);//new { result ,size=Math.Ceiling(size) }
        }
            
        
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await medicine.GetAll();

            if (result.Count() == 0 ) return NotFound("Not found any medicine yet");

            return Ok(result);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetByNameAsync(string name)
        {
            var result = await medicine.GetByName(name);
            if(result is null) return NotFound($"Not found any medicine with name '{name}' ");

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] MedicineViewModel model)
        {

            var med = new Medicine
            {
                Name = model.Name,
                medicineTypeId = model.medicineTypeId,
                Price = model.Price,
                Quantity = model.Quantity,
            };
            var result = await medicine.GetByName(model.Name);
            if (result is not null) return BadRequest($"{model.Name} is already existed");

            await medicine.Add(med);
            return Ok($"{model.Name} is added successfully");

        }

        [HttpPut("{name}")]
        public async Task<IActionResult> UpdateAsync(string name,[FromBody] MedicineViewModel model)
        {
            var med = await medicine.GetByName(name);
            if (med is null) return NotFound($"{name} is not found.");

            med.Name = model.Name;
            med.Price = model.Price;
            med.Quantity = model.Quantity;
            med.medicineTypeId = model.medicineTypeId;

            var result = await medicine.GetByName(med.Name);
            if (result is not null) return BadRequest($"{med.Name} is already existed");

            medicine.Update(med);
            return Ok(med);
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteAsync(string name)
        {
            var res = await medicine.GetByName(name);
            if (res is null)
                return NotFound($"{name} is not found.");

            medicine.Delete(res);
            return Ok($"{res.Name} is deleted successfully.");
        }



    }
}
