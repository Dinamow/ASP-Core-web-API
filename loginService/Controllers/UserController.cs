using Microsoft.AspNetCore.Mvc;
using loginService.Repository;
using Microsoft.AspNetCore.Http;
using loginService.Interfaces;
using AutoMapper;
using loginService.Dto;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace loginService.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _Engine;
        private readonly IMapper _mapper;
        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _Engine = userRepository;
            _mapper = mapper;
        }
        // GET: api/v1/<UserController>
        // create enpoint that return all users in the database
        [HttpGet]
        public IActionResult Get()
        {
            var users = _mapper.Map<List<UserDto>>(_Engine.GetUsers());
            return Ok(users);
        }

        // GET api/v1/<UserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/v1/<UserController>
        [HttpPost, ActionName("Sign Up")]
        [ProducesResponseType(201)]
        public IActionResult Signup([FromBody] Dictionary<string, string> value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                _Engine.SignUp(value).Wait();
                return Created("User Created", "");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST api/v1/<UserController>
        [HttpPost, ActionName("Log In")]
        [ProducesResponseType(201)]
        public async Task<IActionResult> LogInAsync([FromBody] Dictionary<string, string> value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (value == null)
                return BadRequest("Null was passed");
            if (!value.TryGetValue("Email", out var email))
                return BadRequest("Email is required");
            if (!value.TryGetValue("Password", out var password))
                return BadRequest("Password is required");
            try
            {
                string session = await _Engine.Login(email, password);
                HttpContext.Response.Cookies.Append("SessionId", session);
                return Ok("Session Created");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
