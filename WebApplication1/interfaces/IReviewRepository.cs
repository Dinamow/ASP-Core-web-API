using WebApplication1.Models;

namespace WebApplication1.interfaces
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review GetReview(int reviewid);
        ICollection<Review> GetReviewsOfAPokemon(int pokeId);
        bool ReviewExists(int reviewId);
        bool CreateReview(Review pokemon);
        bool UpdateReview(Review pokemon);
        bool Save();
    }
}
