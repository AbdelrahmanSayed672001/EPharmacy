using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_Pharmacy.Models
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }


        public DbSet<Medicine> Medicines{ get; set; }
        public DbSet<MedicineType> MedicineTypes{ get; set; }
        public DbSet<Cart> Carts{ get; set; }
        public DbSet<ShortComming> ShortCommings{ get; set; }

    }
}
