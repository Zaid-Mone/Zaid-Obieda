using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using Zaid_Obieda.Db;
using Zaid_Obieda.Models;
using Zaid_Obieda.Services;
using Zaid_Obieda.DTOs;
using Zaid_Obieda.Utility;

namespace Zaid_Obieda.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly DataDbContext _context;
        private readonly UserManager<AppUser> _appUserManager;
        private readonly ITokenGenerator _tokenGenerator;
        public UserController(DataDbContext context, UserManager<AppUser> appUserManager,
            IConfiguration config, ITokenGenerator tokenGenerator)
        {
            _context = context;
            _appUserManager = appUserManager;
            _config = config;
            _tokenGenerator = tokenGenerator;
        }


        [HttpPost("Register")]
        public async Task<ActionResult<RegisterDTO>> Post(RegisterDTO registerDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new AppUser
                    {
                        UserName = registerDTO.Email,
                        FirstName = registerDTO.FirstName,
                        Email = registerDTO.Email,
                        PhoneNumber = registerDTO.PhoneNumber,
                        LastName = registerDTO.LastName,
                    };
                    var check = await _appUserManager.FindByNameAsync(registerDTO.Email);
                    if (check != null)
                    {
                        return BadRequest(new { message = "sorry the email is already used" });
                    }
                    var result = await _appUserManager.CreateAsync(user, registerDTO.Password);
                }
                return Ok(new { message = "User addedd successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }


        }


        [HttpPost("login")]
        public async Task<ActionResult<string>> Post(LoginDTO loginDTO)
        {
            try
            {
                var user = await _appUserManager.FindByEmailAsync(loginDTO.Email);
                if (user == null)
                {
                    return BadRequest(new { message = "User is not exist please register" });
                }
                var result = await _appUserManager.CheckPasswordAsync(user, loginDTO.Password);

                if (result)
                {
                    //return Ok(new
                    //{
                    //    Token = _tokenGenerator.CreateToken(user)
                    //});
                    int x = HttpContext.Response.StatusCode;
                    var login = new LoginResponseDTO();
                    login.Status = HttpContext.Response.StatusCode.ToString();
                    login.Message = ReponseStatus.ReponseStateMessage(HttpContext.Response.StatusCode);
                    login.Token = _tokenGenerator.CreateToken(user);

                    return Ok(login);
                }
                else
                {
                    return BadRequest(new { message = "The Password doesn't match" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Something went wrong" });
            }

        }

    }
}
