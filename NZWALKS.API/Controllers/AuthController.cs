using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWALKS.API.Models.DTO;
using NZWALKS.API.Repositories;

namespace NZWALKS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }
        [HttpPost]
        [Route("Register")]
        //api/controller/Register
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser()
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };
            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if (identityResult.Succeeded)
            {
                // Add roles to this User
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);

                    if (identityResult.Succeeded)
                    {
                        return Ok("User was registered! Please login.");
                    }
                }
            }
             return BadRequest("Something went wrong!..");
        }

        [HttpPost]
        [Route("Login")]

        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var User = await userManager.FindByEmailAsync(loginRequestDTO.Username);
            if(User != null) 
            { 
            var checkPasswordResult = await userManager.CheckPasswordAsync(User, loginRequestDTO.Password);
            if (checkPasswordResult)
            {
            var roles = await userManager.GetRolesAsync(User);
            if (roles != null) 
            {
            var jwtToken = tokenRepository.CreateJWTToken(User, roles.ToList());

            var response = new LoginResponseDTO
            {
              JwtToken = jwtToken
            };
            return Ok(response);
            }
            }
            }

            return BadRequest("Username and passsword is incorrect!");
            
        }
    }
}
