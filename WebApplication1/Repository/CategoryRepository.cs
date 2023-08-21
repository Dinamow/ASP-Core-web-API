using WebApplication1.Data;
using WebApplication1.interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }
        public bool CategoryExists(int id)
        {
            return _context.Categories.Any(c => c.Id == id);
        }

        public ICollection<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.Where(e => e.Id == id).FirstOrDefault();
        }

        public ICollection<Pokemon> GetPokemonByCategory(int CategoryId)
        {
            return _context.pokemonCategories.Where(e => e.CategoryId == CategoryId).Select(e => e.Pokemon).ToList();
        }
    }
}
