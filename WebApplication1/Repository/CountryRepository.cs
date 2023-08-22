using AutoMapper;
using WebApplication1.Data;
using WebApplication1.interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext _context;

        public CountryRepository(DataContext context)
        {
            _context = context;
        }
        public bool CountryExists(int id)
        {
            return _context.Countries.Any(c => c.Id == id);
        }

        public ICollection<Country> GetCountries()
        {
            return _context.Countries.ToList();
        }

        public Country GetCountry(int id)
        {
            return _context.Countries.Where(e => e.Id == id).FirstOrDefault();
        }

        public Country GetCountryByOwner(int OwnerId)
        {
            return _context.Owners.Where(o => o.Id == OwnerId).Select(c => c.Country).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnersFromACountry(int CountryId)
        {
            return _context.Owners.Where(c => c.Country.Id == CountryId).ToList();
        }
    }
}
