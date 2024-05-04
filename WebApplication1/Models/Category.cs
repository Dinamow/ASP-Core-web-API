using System.Reflection.Metadata.Ecma335;

namespace WebApplication1.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<PokemonCategory> pokemonCategories { get; set; }
    }
}
