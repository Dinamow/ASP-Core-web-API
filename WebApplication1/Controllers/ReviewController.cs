using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dto;
using WebApplication1.interfaces;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;

        public ReviewController(IReviewRepository reviewRepository,IReviewerRepository reviewerRepository,
            IPokemonRepository pokemonRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _reviewerRepository = reviewerRepository;
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        public IActionResult GetReview()
        {
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviews());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviews);
        }
        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();
            var review = _mapper.Map<ReviewDto>(_reviewRepository.GetReview(reviewId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(review);
        }
        [HttpGet("pokemon/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsOfAPokemon(int pokeId)
        {
            var review = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviewsOfAPokemon(pokeId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(review);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReview([FromQuery] int PokeId, [FromQuery] int reviwerId, [FromBody]ReviewDto ReviewCreate)
        {
            if (ReviewCreate == null)
                return BadRequest();

            var review = _reviewRepository.GetReviews()
                .Where(c => c.Title.Trim().ToUpper() == ReviewCreate.Title.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (review != null)
            {
                ModelState.AddModelError("", "review already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewmap = _mapper.Map<Review>(ReviewCreate);

            reviewmap.pokemon = _pokemonRepository.GetPokemon(PokeId);
            reviewmap.Reviewer = _reviewerRepository.GetReviewer(reviwerId);

            if (!_reviewRepository.CreateReview(reviewmap))
            {
                ModelState.AddModelError("", "Something went worng");

                return StatusCode(500, ModelState);
            }

            return Ok("Successfully Created");
        }

        [HttpPut("{ReviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult Updatecountry(int ReviewId, [FromBody] ReviewDto updateReviewId)
        {
            if (updateReviewId == null)
                return BadRequest(ModelState);

            if (ReviewId != updateReviewId.Id)
                return BadRequest(ModelState);

            if (!_reviewRepository.ReviewExists(ReviewId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var Reviewmap = _mapper.Map<Review>(updateReviewId);

            if (!_reviewRepository.UpdateReview(Reviewmap))
            {
                ModelState.AddModelError("", "something went wrong");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{ReviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReview(int ReviewId)
        {
            if (!_reviewRepository.ReviewExists(ReviewId))
                return NotFound();

            var ReviewDelete = _reviewRepository.GetReview(ReviewId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.DeleteReview(ReviewDelete))
            {
                ModelState.AddModelError("", "Something went wrong Deleting category");

                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
