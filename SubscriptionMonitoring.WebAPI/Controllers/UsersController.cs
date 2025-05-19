using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SubscriptionMonitoring.MVC.Data;
using SubscriptionMonitoring.WebAPI.Data;
using SubscriptionMonitoring.WebAPI.Models;
using SubscriptionMonitoring.WebAPI.Services;

namespace SubscriptionMonitoring.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PasswordService _passwordService;
        private readonly IConfiguration _configuration;

        public UsersController(AppDbContext context,PasswordService passwordService, IConfiguration configuration)
        {
            _context = context;
            _passwordService = passwordService;
            _configuration = configuration;
        }

        // GET: api/Users
        [HttpGet]

        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {

            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return null;
                //return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]Register model)
        {
            if (await _context.Users.AnyAsync(u => u.UserId == model.UserId))
                return BadRequest(new { message = "User Id Already Existe" });
            var hashedPwd = _passwordService.HashPassword(model.UserPassword);
            var user = new User { UserId = model.UserId, UserName = model.UserName, UserEmail = model.UserEmail, UserPasswordHash = hashedPwd,UserCreatedAt = DateTime.Now};
            _context.Users.Add(user);
           
            await _context.SaveChangesAsync();
            return Ok(new { Message = "User Created Successfully"});
        }

        //Company User Register
        [HttpPost("Company/register")]
        public async Task<IActionResult> CompanyUserRegister([FromBody] Register model)
        {
            if (await _context.Users.AnyAsync(u => u.UserId == model.UserId))
                return BadRequest(new { message = "User Id Already Existe" });
            var hashedPwd = _passwordService.HashPassword(model.UserPassword);
            var user = new User { UserId = model.UserId, UserName = model.UserName, UserEmail = model.UserEmail, UserPasswordHash = hashedPwd, Role = string.Empty, UserCreatedAt = DateTime.Now };
            _context.Users.Add(user);

            await _context.SaveChangesAsync();
            return Ok(new { Message = "User Created Successfully" });
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginModel model)
        {

            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserId == model.LoginUserId);
            if (user == null || !_passwordService.VerifyPassword(user.UserPasswordHash, model.LoginUserPassword))
            {
                return Unauthorized("Invalid username or password");
            }

            var token = GenerateJwtToken(user);
            //return Ok(new { Token = token });
            if (token == "Invalid credentials")
            {
                return BadRequest("Invalid credentials");
            }
            Console.WriteLine(token);
            return Ok(new { Token = token,UserId= user.UserId, Role=user.Role });

        }

        private string GenerateJwtToken(User _user)
        {
            if (_user != null && _user.UserId != null)
            {
                var user = GetUser(_user.UserId).Result.Value;
                
                if (user != null)
                {
                    //create claims details based on the user information
                    var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Email", user.UserId),
                    new Claim("Password", user.UserPasswordHash)
                   };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);

                    return new JwtSecurityTokenHandler().WriteToken(token);
                }
                else
                {
                    return "Invalid credentials";
                }
            }
            else
            {
                return "No Valid credentials";
            }
        }



        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
