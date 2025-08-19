using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureNotesAPI.Data;
using SecureNotesAPI.DTOs;
using SecureNotesAPI.Models;
using SecureNotesAPI.Services;
using System.Threading.Tasks;

namespace SecureNotesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;
        public UsersController(AppDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserCreateDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.UserName == dto.Username || u.Email == dto.Email))
                return BadRequest("Username or email address is already registered.");

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var user = new User
            {
                UserName = dto.Username,
                Email = dto.Email,
                PasswordHash = passwordHash
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                succes = true,
                message = "User added.",
                data = new { user.Id, user.UserName }
            });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            System.ArgumentNullException.ThrowIfNull(_tokenService);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == dto.Username);
            if (user == null)
                return Unauthorized("User not found.");

            bool verified = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!verified)
                return Unauthorized("The password is incorrect.");

            var token = _tokenService.GenerateToken(user);
            return Ok(new
            {
                success = true,
                message = "Login successful.",
                data = new { token }
            });
        }
    }
}

