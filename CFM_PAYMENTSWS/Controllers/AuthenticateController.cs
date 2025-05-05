using CFM_PAYMENTSWS.Domains.Contracts;
using CFM_PAYMENTSWS.Domains.Models;
using CFM_PAYMENTSWS.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CFM_PAYMENTSWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticateController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)

        {
            var ipOrigem = HttpContext.Connection.RemoteIpAddress?.ToString();

            Debug.Print($"ipOrigemipOrigem {ipOrigem}");
            /* return Ok(new
             {
                 token = "",
                 expiration = "",
                 allowed = true,
                 output_response = "AUTHENTICATED"
             });*/
            try
            {
                Debug.Print("Before user validation");
                var user = await _userManager.FindByNameAsync(model.Username);
                Debug.Print("After user");
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var userRoles = await _userManager.GetRolesAsync(user);

                    var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    Debug.Print($" auth claim {authClaims.ToString()}");
                    var token = GetToken(authClaims);
                    Debug.Print($" AFTER GET TOKEN {authClaims.ToString()}");

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo,
                        allowed = true,
                        output_response = "AUTHENTICATED"
                    });
                }
                return Ok(new
                {
                    token = "",
                    expiration = "",
                    allowed = false,
                    output_response = "BAD_CREDENTIALS"

                });
            }

            catch (Exception ex)
            {

                Debug.Print($"Erro interno  {ex.Message} {ex.StackTrace}");

                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status401Unauthorized, new ResponseDTO(WebTransactionCodes.USERALREADYEXISTS, null, null).ToString());

            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            System.Diagnostics.Debug.WriteLine("Resultado :" + result);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO(WebTransactionCodes.USERCREATIONFAILED, null, null).ToString());

            return Ok(new ResponseDTO(WebTransactionCodes.SUCCESS, null, null).ToString());
        }

        /*
        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status401Unauthorized, new ResponseDTO(WebTransactionCodes.USERALREADYEXISTS, null, null).ToString());

            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO(WebTransactionCodes.USERCREATIONFAILED, null, null).ToString());

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }
            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.User);
            }



            return Ok(new ResponseDTO(WebTransactionCodes.SUCCESS, null, null).ToString());
        }
        */


        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
        


    }
}
