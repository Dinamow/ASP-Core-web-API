using WebApplication1.Models;

namespace WebApplication1.interfaces
{
    public interface IOwnerRepository
    {
        ICollection<Owner> GetOwners();
        Owner GetOwner(int ownerId);
        ICollection<Owner> GetOwnerOfAPokemon(int PokeId);
        ICollection<Pokemon> GetPokemonByOwner(int OwnerId);
        bool OwnerExists(int ownerId);
    }
}
