namespace WebApplication1.Models
{
    public class Owner
    {
        public string Gym { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Id { get; set; }
        public Country Country { get; set; }
        public ICollection<PokemonOwner> PokemonOwners { get; set; }
    }
}
