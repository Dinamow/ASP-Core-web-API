using WebApplication1.Models;

namespace WebApplication1.interfaces
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int id);
        ICollection<Pokemon> GetPokemonByCategory(int CategoryId);
        bool CategoryExists(int CategoryId);
        bool CreateCategory(Category category);
        bool Save();
    }
}
