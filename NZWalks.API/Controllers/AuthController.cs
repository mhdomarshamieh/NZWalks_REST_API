using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenReposiroty _repository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenReposiroty reposiroty)
        {
            _userManager = userManager;
            _repository = reposiroty;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.UserName,
                Email = registerRequestDto.UserName,
              
            };
            var identityResult = await _userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if(identityResult.Succeeded)
            {
                // Add Roles to the user
                if(registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {

                   identityResult =  await _userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);

                    if(identityResult.Succeeded)
                    {
                        return Ok("User Registered Successfully");
                    }
                }
            }

            return BadRequest("Something Wen Wrong");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDto.UserName);

            if(user == null)
            {
                return BadRequest("invalid user");
            }

            var checkPass = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            if (checkPass)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                //Create Token
                if(userRoles != null)
                {
                    var jwtToken = _repository.CreateJwtToken(user, userRoles.ToList());

                    var response = new LoginResponseDto() {
                        JwtToken = jwtToken,
                    };
                    return Ok(response);
                }

            }
            
            return BadRequest("Password is no correct");
            
    }
    }

    
}
 