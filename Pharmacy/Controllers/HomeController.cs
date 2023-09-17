using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pharmacy.Models;
using Pharmacy.ViewModel;
using RestSharp;
using System.Diagnostics;

namespace Pharmacy.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private const double PAGE_SIZE=3.0;

        
        [HttpGet]
        public async Task<IActionResult> Index(string? name, int page)
        {
            ViewBag.Search = name;
            var result2 = new MedicinesViewModel();
            if (!string.IsNullOrEmpty(name))
            {
                result2 = await GetMedicine(name);
                ViewBag.OutputName = result2.name;
                ViewBag.OutputQuantity = result2.quantity;
                ViewBag.OutputPrice = result2.price;
                ViewBag.OutputID = result2.id;
                ViewBag.OutputSize = 1;

            }
            else
            {
                try
                {
                    var result = await PaginationAsync(page);
                    //var result= await GetAllMedicines();

                    var lst = new List<List<object>>();
                    var lstName = new List<object>();
                    var lstID = new List<object>();
                    var lstQuantity = new List<object>();
                    var lstPrice = new List<object>();

                    if (result != null)
                    {
                        if (result.Count == 1)
                        {
                            ViewBag.OutputName = result[0].name;
                            ViewBag.OutputQuantity = result[0].quantity;
                            ViewBag.OutputPrice = result[0].price;
                            ViewBag.OutputID = result[0].id;
                            ViewBag.OutputSize = result.Count;
                            
                        }
                        else
                        {
                            for (int i = 0; i < result.Count; i++)
                            {

                                lstName.Add(result[i].name);
                                lstID.Add(result[i].id);
                                lstQuantity.Add(result[i].quantity);
                                lstPrice.Add(result[i].price);
                            }

                            ViewBag.OutputName = lstName;
                            ViewBag.OutputID = lstID;
                            ViewBag.OutputQuantity = lstQuantity;
                            ViewBag.OutputPrice = lstPrice;
                            ViewBag.OutputSize = lstName.Count;
                        }

                    }
                    ViewBag.Size = await GetAllMedicines();
                }
                catch
                {
                    ViewBag.Message = "Something went wrong";
                }
            }

            return View();
        }

        [HttpGet]
        public async Task<int> GetAllMedicines()
        {
            var result = new List<MedicinesViewModel>();
            try
            {
                var options = new RestClientOptions("https://localhost:7049")
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest("/api/Medicine", Method.Get);
                RestResponse response = await client.ExecuteAsync(request);
                result = JsonConvert.DeserializeObject<List<MedicinesViewModel>>(response.Content);
            }
            catch
            {
                ViewBag.Message = "Something went wrong";
            }
            var size = result.Count / PAGE_SIZE;
            return Convert.ToInt32(Math.Ceiling(size));
        }

        [HttpGet]
        public async Task<MedicinesViewModel> GetMedicine(string name)
        {
            var result = new MedicinesViewModel();
            try
            {
                var options = new RestClientOptions("https://localhost:7049")
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest($"/api/Medicine/{name}", Method.Get);
                RestResponse response = await client.ExecuteAsync(request);
                result = JsonConvert.DeserializeObject<MedicinesViewModel>(response.Content);
            }
            catch
            {
                ViewBag.Message = "Something went wrong";

            }

            return result;
        }


        public async Task<List<MedicinesViewModel>> PaginationAsync(int page)
        {

            if (page <= 0)
                page = 1;

            var options = new RestClientOptions("https://localhost:7049")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest($"/api/Medicine/GetAllPagination/{page}", Method.Get);
            RestResponse response = await client.ExecuteAsync(request);

            var result = JsonConvert.DeserializeObject<List<MedicinesViewModel>>(response.Content);

            return result;
        }

        public async Task<IActionResult> AddMedicine()
        {
            try
            {

                var result = await GetAllMedicineTypes();

                var lstID = new List<object>();
                var lstName = new List<object>();

                if (result.Count > 0)
                {
                    for (int i = 0; i < result.Count; i++)
                    {
                        lstID.Add(result[i].id);
                        lstName.Add(result[i].typeName);
                    }
                    ViewBag.MedicineTypesName = lstName;
                    ViewBag.MedicineTypesID = lstID;
                    ViewBag.MedicineTypeSize = lstName.Count;
                }
            }
            catch
            {
                ViewBag.Message = "Something went wrong";
            }
            return View();
        }        

        [HttpGet]
        private async Task<List<MedicineTypeViewModel>> GetAllMedicineTypes()
        {
            var result = new List<MedicineTypeViewModel>();
            var options = new RestClientOptions("https://localhost:7049")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("/api/MedicineType", Method.Get);
            RestResponse response = await client.ExecuteAsync(request);
            result = JsonConvert.DeserializeObject<List<MedicineTypeViewModel>>(response.Content);

            return result;
        }

        [HttpPost]
        public async Task<IActionResult> Add(NewMedicineViewModel model)//Add new Medicine
        {
            try
            {
                var options = new RestClientOptions("https://localhost:7049")
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest("/api/Medicine", Method.Post);
                request.AddHeader("Content-Type", "application/json");
                var body = JsonConvert.SerializeObject(model);
                request.AddStringBody(body, DataFormat.Json);
                RestResponse response = await client.ExecuteAsync(request);
                var result = JsonConvert.DeserializeObject<string>(response.Content);
                ViewBag.OutputMSG = result;
            }
            catch
            {
                ViewBag.Message = "Something went wrong";
            }
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(string name)
        {
            if (name is null)
                ViewBag.Delete = "Not null";
            else
            {
                try
                {
                    var options = new RestClientOptions("https://localhost:7049")
                    {
                        MaxTimeout = -1,
                    };
                    var client = new RestClient(options);
                    var request = new RestRequest($"/api/Medicine/{name}", Method.Delete);
                    RestResponse response = await client.ExecuteAsync(request);
                    var result = JsonConvert.DeserializeObject<string>(response.Content);
                    ViewBag.Delete = result;
                }
                catch
                {
                    ViewBag.Message = "Something went wrong";
                }
            } 
            return RedirectToAction(nameof(Index));
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}