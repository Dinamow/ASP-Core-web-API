namespace WebApplication1.Models
{
    public class Country
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public ICollection<Owner> Owners { get; set; }
    }
}
