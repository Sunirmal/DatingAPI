using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DatingAPI.Data;
using DatingAPI.DTO;
using DatingAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace DatingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly DataContext _context;
        private readonly DatingAPI.Interfaces.ITokerService _tokenService;
        
        public AccountController(DataContext dataContext, 
            DatingAPI.Interfaces.ITokerService tokenService)
        {
            this._context = dataContext;
            this._tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                UserName = registerDTO.UserName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
                PasswordSalt = hmac.Key
            };
            this._context.Users.Add(user);
            await this._context.SaveChangesAsync();
            return new UserDTO
            {
                USerName= user.UserName,
                Token= this._tokenService.CreateToken(user)

            };
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await this._context.Users.SingleOrDefaultAsync
                    (x => x.UserName == loginDTO.UserName);

            if(user== null)
            {
                return Unauthorized("Invalid User " + loginDTO);
            }
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));
            
            for (int i=0; i< computeHash.Length;i++ )
            {
                if(computeHash[i] != user.PasswordHash[i])
                {
                    return Unauthorized("Invalid Password " + loginDTO);
                }
            }

            return new UserDTO
            {
                USerName= user.UserName,
                Token= this._tokenService.CreateToken(user)
            };
        }  
    }
}