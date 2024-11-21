using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Registration.Data;
using Registration.DTO.Account;
using Registration.Interface;
using Registration.Models;

namespace Registration.Controllers
{
    [Route("api/v1[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var user = new User
                {
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    UserName = registerDto.FirstName.ToLower(),
                    Email = registerDto.Email,
                    PhoneNumber = registerDto.PhoneNumber,
                    NormalizedEmail = registerDto.Email.ToUpper()
                };

                var newUser = await _userManager.CreateAsync(user, registerDto.Password);

                if (!newUser.Succeeded) return StatusCode(500, newUser.Errors);

                var role = await _userManager.AddToRoleAsync(user, UserRoles.User);

                if (role.Succeeded)
                {
                    return Ok("User Registered Successfully");
                }

                return StatusCode(500, role.Errors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var user = await _userManager.FindByEmailAsync(loginDto.Email);

                if (user == null) return Unauthorized("Invalid email address");

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

                if (result.Succeeded)
                {
                    return Ok(new ResponseDto
                    {
                        Message = "Login successful",
                        Name = $"{user.FirstName} {user.LastName}",
                        Email = user.Email,
                        Token = _tokenService.CreateToken(user),
                    });
                }

                return Unauthorized("Invalid username or password");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
