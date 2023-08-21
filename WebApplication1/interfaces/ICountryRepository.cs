using WebApplication1.Models;

namespace WebApplication1.interfaces
{
    public interface ICountryRepository
    {
        ICollection<Country> GetCountries();
        Country GetCountry(int id);
        Country GetCountryByOwner(int OwnerId);
        ICollection<Owner> GetOwnersFromACountry(int CountryId);
        bool CountryExists(int id);
    }
}
