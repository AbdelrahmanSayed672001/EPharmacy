using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pharmacy.ViewModel;
using RestSharp;
using System.Net.Mail;

namespace Pharmacy.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        public async Task<IActionResult> Cart()
        {
            var user = User.Identity?.Name;
            try
            {
                var result = await GetByUsername(user);
                var totalPrice=await GetTotalPrice(user);

                var lst = new List<List<object>>();
                //var lstUser = new List<object>();
                
                var lstMedicine = new List<object>();
                var lstQuantity = new List<object>();
                var lstPrice = new List<object>();
                var lstDate = new List<object>();

                if (result != null)
                {
                    if (result.Count == 1)
                    {
                        //ViewBag.OutputUser = result[0].email;
                        
                        ViewBag.OutputQuantity = result[0].quantity;
                        ViewBag.OutputPrice = result[0].price;
                        ViewBag.OutputMedicine = result[0].medicine;
                        ViewBag.OutputDate = result[0].date;
                        ViewBag.OutputSize = result.Count;
                    }
                    else
                    {
                        for (int i = 0; i < result.Count; i++)
                        {
                            //lstUser.Add(result[i].user);
                            
                            lstMedicine.Add(result[i].medicine);
                            lstQuantity.Add(result[i].quantity);
                            lstPrice.Add(result[i].price);
                            lstDate.Add(result[i].date);
                        }

                        //ViewBag.OutputUser = lstUser;
                        
                        ViewBag.OutputMedicine = lstMedicine;
                        ViewBag.OutputQuantity = lstQuantity;
                        ViewBag.OutputPrice = lstPrice;
                        ViewBag.OutputDate = lstDate;
                        ViewBag.OutputSize = lstMedicine.Count;
                    }
                }

                
                    ViewBag.TotalPrice = totalPrice;
            }
            catch
            {
                ViewBag.Message = "Something went wrong";

            }
            return View();
        }

        private async Task<string> GetTotalPrice(string email)
        {
            var options = new RestClientOptions("https://localhost:7049")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest($"/api/Cart/Calc?Username={email}", Method.Get);
            RestResponse response = await client.ExecuteAsync(request);
            var res = JsonConvert.DeserializeObject<string>(response.Content);

            return res;
        }
        public async Task<List<CartViewModel>> GetByUsername(string email)
        {
            
            var result = new List<CartViewModel>();

            var options = new RestClientOptions("https://localhost:7049")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest($"/api/Cart/GetByUsername?name={email}", Method.Get);
            RestResponse response = await client.ExecuteAsync(request);
            result = JsonConvert.DeserializeObject<List<CartViewModel>>(response.Content);

            return result;
        }

        public async Task<IActionResult> AddToCart(string name)
        {
            var user = User.Identity?.Name;
            var quantity = 1;
            try
            {
                var options = new RestClientOptions("https://localhost:7049")
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest("/api/Cart", Method.Post);
                request.AddHeader("Content-Type", "application/json");
                var body = JsonConvert.SerializeObject(new { username = user, medicine=name, quantity = quantity });
                request.AddStringBody(body, DataFormat.Json);
                RestResponse response = await client.ExecuteAsync(request);
                var result=JsonConvert.DeserializeObject<string>(response.Content);
                ViewBag.CartMSG = result;
            }
            catch
            {
                ViewBag.Message = "Something went wrong";
            }
            return RedirectToAction(nameof(Index),"Home");
        }
        public async Task<IActionResult> Orders()
        {
            try
            {
                var result = await GetAllAsync();

                var lst = new List<List<object>>();
                var lstUser = new List<object>();
                var lstPhone = new List<object>();
                var lstAddress = new List<object>();
                var lstMedicine = new List<object>();
                var lstQuantity = new List<object>();
                var lstPrice = new List<object>();
                var lstDate = new List<object>();

                if (result != null)
                {
                    if (result.Count == 1)
                    {
                        ViewBag.OutputUser = result[0].user;
                        ViewBag.OutputPhone = result[0].phone;
                        ViewBag.OutputAddress = result[0].address;
                        ViewBag.OutputQuantity = result[0].quantity;
                        ViewBag.OutputPrice = result[0].price;
                        ViewBag.OutputMedicine = result[0].medicine;
                        ViewBag.OutputDate = result[0].date;
                        ViewBag.OutputSize = result.Count;
                    }
                    else
                    {
                        for (int i = 0; i < result.Count; i++)
                        {
                            lstUser.Add(result[i].user);
                            lstPhone.Add(result[i].phone);
                            lstAddress.Add(result[i].address);
                            lstMedicine.Add(result[i].medicine);
                            lstQuantity.Add(result[i].quantity);
                            lstPrice.Add(result[i].price);
                            lstDate.Add(result[i].date);
                        }

                        ViewBag.OutputUser = lstUser;
                        ViewBag.OutputPhone = lstPhone;
                        ViewBag.OutputAddress = lstAddress;
                        ViewBag.OutputMedicine = lstMedicine;
                        ViewBag.OutputQuantity = lstQuantity;
                        ViewBag.OutputPrice = lstPrice;
                        ViewBag.OutputDate = lstDate;
                        ViewBag.OutputSize = lstUser.Count;
                    }
                }
            }
            catch
            {
                ViewBag.Message = "Something went wrong";

            }
            return View();
        }

        private async Task<List<OrdersViewModel>> GetAllAsync()
        {
            var result = new List<OrdersViewModel>();

            var options = new RestClientOptions("https://localhost:7049")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("/api/Cart/GetAll", Method.Get);
            RestResponse response = await client.ExecuteAsync(request);
            result = JsonConvert.DeserializeObject<List<OrdersViewModel>>(response.Content);

            return result;
        }

        

    }
}
