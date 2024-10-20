using Microsoft.EntityFrameworkCore;
using Shop.WebAPI.Data;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Repository.Interfaces;

namespace Shop.WebAPI.Repository
{
    public class ModelRepository : IModelRepository
    {
        private readonly ShopApplicationContext _context;

        public ModelRepository(ShopApplicationContext context)
        {
            _context = context;
        }

        public async Task<Model> GetModelByIdAsync(int id)
        {
            return await _context.Models
                .Include(m => m.Product)
                .Include(m => m.Color)
                .Include(m => m.ModelSizes).ThenInclude(ms => ms.Size)
                .Include(m => m.Photos)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Model>> GetAllModelsAsync()
        {
            return await _context.Models
                .Include(m => m.Product)
                .Include(m => m.Color)
                .Include(m => m.ModelSizes).ThenInclude(ms => ms.Size)
                .Include(m => m.Photos)
                .ToListAsync();
        }

        public async Task<int> AddModelAsync(Model model)
        {
            _context.Models.Add(model);
            await _context.SaveChangesAsync();
            return model.Id;
        }

        public async Task<bool> UpdateModelAsync(Model model)
        {
            var existingModel = await _context.Models.FindAsync(model.Id);
            if (existingModel == null)
            {
                return false;
            }

            _context.Entry(existingModel).CurrentValues.SetValues(model);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteModelAsync(int id)
        {
            var model = await _context.Models.FindAsync(id);
            if (model == null)
            {
                return false;
            }

            _context.Models.Remove(model);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
