using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TrainAPI.Helpers;
using TrainAPI.Models;
using TrainAPI.Services;

namespace TrainAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
    
        public UsersController(IUserService userService, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }
    
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]UserDTO userDTO)
        {
            var user = _userService.Authenticate(userDTO.Login, userDTO.Password);
    
            if (user == null)
                return BadRequest(new { message = "Login or password is incorrect" });
    
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
    
            return Ok(new
            {
                UserId = user.UserId,
                Login = user.Login,
                Token = tokenString
            });
        }
    
        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]UserDTO userDTO)
        {
            var user = _mapper.Map<User>(userDTO);
    
            try
            {
                _userService.Create(user, userDTO.Password);
                return NoContent();
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            var userDTOs = _mapper.Map<IList<UserDTO>>(users);
            return Ok(userDTOs);
        }
    
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            var userDTO = _mapper.Map<UserDTO>(user);
            return Ok(userDTO);
        }
    
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]UserDTO userDTO)
        {
            var user = _mapper.Map<User>(userDTO);
            user.UserId = id;
    
            try
            {
                _userService.Update(user, userDTO.Password);
                return NoContent();
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return NoContent();
        }
    }
}
