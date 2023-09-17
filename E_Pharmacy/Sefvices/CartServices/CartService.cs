using E_Pharmacy.Models;
using E_Pharmacy.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace E_Pharmacy.Sefvices.CartServices
{
    public class CartService : ICartService
    {
        private readonly ApplicationDBContext dBContext;

        public CartService(ApplicationDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        public async Task<Cart> AddtoCartAsync(Cart model)
        {

            await dBContext.Carts.AddAsync(model);
            await dBContext.SaveChangesAsync();
            return model;
        }

        public async Task<IEnumerable<CartDetails>> GetAllAsync()
        {
            //return dBContext.Carts.ToList();
            var res = await dBContext.Carts.Include(u => u.User)
                    .Select(u=>new CartDetails
                    {
                        Id = u.Id,
                        User=u.User.UserName,
                        Address=u.User.Address,
                        Phone=u.User.PhoneNumber,
                        Medicine=u.medicine.Name,
                        Date=u.Date,
                        Price=u.Price,
                        Quantity=u.Quantity
                    }).OrderBy(u=>u.User).ToListAsync();
            return res;
        }

        public async Task<IEnumerable<CartForUser>> GetByUsername(string username)
        {
            var res =await dBContext.Carts.Include(u => u.User)
                    .Select(u => new CartForUser {
                        Id = u.Id,
                        Email=u.User.Email,
                        Medicine = u.medicine.Name,
                        Date = u.Date,
                        Price = u.Price,
                        Quantity = u.Quantity
                    }).Where(x=>x.Email==username).OrderBy(u=>u.Medicine).ToListAsync();

            return res;
        }
        public async Task<double> CalculateTotalPrice(string username)
        {
            //var totalPrice = 0.0; 
            //foreach (var cart in model)
            //{
            //    totalPrice += cart.Price;
            //}
            
            var cart= dBContext.Carts.Where(x => x.User.UserName == username);
            
            return cart.Sum(x => x.Price);//totalPrice;
        }

        public async Task<bool> isExisted(Cart model)
        {
            var cart= await dBContext.Carts
                    .Where(x => x.User.UserName == model.User.UserName)
                    .FirstOrDefaultAsync(m => m.medicine.Name == model.medicine.Name);
            
            if (cart == null)
                return true;
            return false;
        }

        public async Task<string> Delete(string username)
        {
            var cart = dBContext.Carts.Where(u => u.User.UserName == username);

            if (!cart.Any()) {
               
                return string.Empty;
            }
            
            dBContext.Carts.RemoveRange(cart);
            await dBContext.SaveChangesAsync();
            return "Your cart is free now";
                
                
        }

        
    }
}
