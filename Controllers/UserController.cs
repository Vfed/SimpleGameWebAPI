using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SimpleGameWebAPI.Data.Models;
using SimpleGameWebAPI.Data.Dto;

namespace SimpleGameWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DbService _dbService;
        public UserController(DbService dbService)
        {
            _dbService = dbService;
        }

        // Add new User 
        //Usedto {string Name}
        // returns ( true / false )

        [HttpPost("add")]
        public IActionResult AddUser(UserDto dto)
        {
            User user = _dbService.Users.Where(x => x.Name == dto.Name).FirstOrDefault();
            if (user == null)
            {
                User newUser = new User
                {
                    Id = Guid.NewGuid(),
                    Name = dto.Name,
                    Score = 0 
                };
                _dbService.Users.Add(newUser);
                _dbService.SaveChanges();
                return Ok(true);
            }
            return Ok(false);
        }

        // Find User 
        //Usedto {string Name}
        // returns ( user / null )

        [HttpPost("find")]
        public IActionResult FindUser(UserDto dto)
        {
            User user = _dbService.Users.Where(x => x.Name == dto.Name).FirstOrDefault();
            if (user != null)
            {
                return Ok(user);
            }
            return Ok(null);
        }


        // List of All User names 
        //
        // returns ( list < string > )

        [HttpGet("all")]
        public IActionResult AllUsers()
        {
            List<string> users = _dbService.Users.Select(x => x.Name).ToList();
            return Ok(users);
        }

        // Get User with all data 
        // Usedto {string Name}
        // returns ( User )

        [HttpPost("getuser")]
        public IActionResult CheckUserExists(UserDto dto)
        {
            UserDto user = _dbService.Users.Where(x => x.Name == dto.Name).Select(x => new UserDto{ Name = x.Name}).FirstOrDefault();
            return Ok(user);
        }

        // Check request 
        // 
        // returns ( string )

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Check text string");
        }

        // Get User formated
        // name: string
        // returns ( name: string , score: int / null)

        [HttpGet("login")]
        public IActionResult LoginUser(string name)
        {
            User user = _dbService.Users.Where(x => x.Name == name).FirstOrDefault();
            if( user != null)
            {
                return Ok(new { name = user.Name, score = user.Score });
            }
            else
            {
                return Ok(null);
            }
        }
    }
}
