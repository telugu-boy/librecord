using AutoMapper;
using lcapis.Entities;
using lcapis.Helpers;
using lcapis.Models.LCUsers;
using lcapis.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace lcapis.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/auth/[controller]")]
    public class LCUsersController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public LCUsersController(
            IUserService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        public bool IsAuthorized(long id)
        {
            return id == long.Parse(User.Identity.Name);
        }

        [AllowAnonymous]
        [HttpGet("")]
        public string Default()
        {
            return "Welcome :D";
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateUserModel model)
        {
            LCUser user = _userService.Authenticate(model.Email, model.Password);

            if (user == null)
                return BadRequest(new { message = "Invalid credentials" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserID.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info and authentication token
            return Ok(new
            {
                user.UserID,
                user.Email,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterUserModel model)
        {
            // map model to entity
            var user = _mapper.Map<LCUser>(model);

            try
            {
                // create user
                _userService.Create(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            if(!IsAuthorized(id))
                return BadRequest(new { message = "Unauthorized" });
            var user = _userService.GetByUserID(id);
            var model = _mapper.Map<UserModel>(user);
            return Ok(model);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] UpdateUserModel model)
        {
            if (!IsAuthorized(id))
                return BadRequest(new { message = "Unauthorized" });
            // map model to entity and set id
            var user = _mapper.Map<LCUser>(model);
            user.UserID = id;

            try
            {
                // update user 
                _userService.Update(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            if (!IsAuthorized(id))
                return BadRequest(new { message = "Unauthorized" });
            _userService.Delete(id);
            return Ok();
        }
    }
}
