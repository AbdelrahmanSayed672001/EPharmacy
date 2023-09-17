using E_Pharmacy.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Pharmacy.Sefvices.ShortCommingServices
{
    public class ShortCommingService : IShortComming
    {
        private readonly ApplicationDBContext dBContext;

        public ShortCommingService(ApplicationDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        public async Task<ShortComming> Add(ShortComming shortComming)
        {
            await dBContext.ShortCommings.AddAsync(shortComming);
            await dBContext.SaveChangesAsync();
            return shortComming;
        }

        public void Delete(ShortComming shortComming)
        {
            dBContext.ShortCommings.Remove(shortComming);
            dBContext.SaveChanges();
        }

        public async Task<ShortComming> isExisted(string medicineName)
        {
            var res = await dBContext.ShortCommings
                 .FirstOrDefaultAsync(x => x.medicine.Name == medicineName);

            return res;
        }

        public async Task<IEnumerable<ShortComming>> GetAll()
        {
            return await dBContext.ShortCommings.ToListAsync();
        }

        
    }
}
