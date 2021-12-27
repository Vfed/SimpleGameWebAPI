
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;

//using SimpleGameWebAPI.Hubs;
//using SimpleGameWebAPI.Hubs.Clients;
using SimpleGameWebAPI.Data.Models;
using SimpleGameWebAPI.Data.Dto;
using Microsoft.EntityFrameworkCore;

namespace SimpleGameWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly DbService _dbService;
        public GamesController(DbService dbService)
        {
            _dbService = dbService;
        }

        // Get session by users
        // GameUsersDto {string Player1, string Player2}
        // returns ( formated session / null )

        [HttpPost("getsession")]
        public IActionResult GetGame(GameUsersDto dto)
        {
            GameSession game = _dbService.GameSessions
                .Where(x =>
                    x.Users.FirstOrDefault(y =>
                        y.Name == dto.Player1) != null &&
                    x.Users.FirstOrDefault(y =>
                        y.Name == dto.Player2) != null)
                .FirstOrDefault();
            if (game != null)
            {
                return Ok(
                    new
                    {
                        Id = game.Id,
                        Users = game.Users,
                        CurrentPlayer = game.CurrentPlayer,
                        GameStatus = game.GameStatus,
                        dateTime = game.dateTime,
                        Field = JsonSerializer.Deserialize<string[][]>(game.Field),
                    }
                    );
            }
            return Ok(null);
        }

        // Get session by id
        // GameUsersDto {string Player1, string Player2}
        // returns ( formated session / null )

        [HttpPost("getsessionbyid")]
        public IActionResult GetGameById(SessionDto dto)
        {
            GameSession game = _dbService.GameSessions
                .Where(x =>
                    x.Id == dto.Id)
                .Include("Users")
                .Include("CurrentPlayer")
                .FirstOrDefault();
            if (game != null)
            {
                return Ok(
                    new
                    {
                        Id = game.Id,
                        Users = game.Users,
                        CurrentPlayer = game.CurrentPlayer,
                        GameStatus = game.GameStatus,
                        dateTime = game.dateTime,
                        Field = JsonSerializer.Deserialize<string[][]>(game.Field),
                    }
                    );
            }
            return Ok(null);
        }
        // Get session id
        // GameUsersDto {string Player1, string Player2}
        // returns ( formated session / null )

        [HttpPost("getsessionid")]
        public IActionResult GetSessionId(GameUsersDto dto)
        {
            GameSession game = _dbService.GameSessions
                .Where(x =>
                    x.Users.FirstOrDefault(y =>
                        y.Name == dto.Player1) != null &&
                    x.Users.FirstOrDefault(y =>
                        y.Name == dto.Player2) != null)
                .FirstOrDefault();
            if (game != null)
            {
                return Ok(
                    new
                    {
                        Id = game.Id
                    }
                    );
            }
            return Ok(null);
        }

        // Add new session  
        // GameUsersDto {string Player1, string Player2}
        // returns ( string )

        [HttpPost("add")]
        public IActionResult AddGame(GameUsersDto dto)
        {
            if( dto.Player1 != dto.Player2)
            {
                return Ok("It`s not a solo game )");
            }

            GameSession game = _dbService.GameSessions
                .Where(x => 
                    x.Users.FirstOrDefault(y => 
                        y.Name == dto.Player1) != null && 
                    x.Users.FirstOrDefault(y => 
                        y.Name == dto.Player2) != null)
                .FirstOrDefault();

            if (game == null )
            {
                User user1 = _dbService.Users.Where(x => x.Name == dto.Player1).FirstOrDefault();
                User user2 = _dbService.Users.Where(x => x.Name == dto.Player2).FirstOrDefault();

                if (user1 != null && user2 != null)
                {
                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true
                    };
                    var arr = new string[][] { new string[]{ "", "", "" } , new string[] { "", "", "" } , new string[] { "", "", "" } };
                    var serialized = JsonSerializer.Serialize(arr);
                    GameSession newGame = new GameSession
                    {
                        Id = Guid.NewGuid(),
                        Users = new List<User>() { user1, user2 },
                        CurrentPlayer = user1,
                        GameStatus = "Active",
                        dateTime = DateTime.Now,
                        Field = serialized,
                    };

                    //string[][] arr2 = JsonSerializer.Deserialize<string[][]>(serialized);
                    _dbService.GameSessions.Add(newGame);
                    _dbService.SaveChanges();
                    return Ok("All is Ok");
                }
                return Ok("No users with such name");
                }
            return Ok("Session exists");
        }

        // Do Game Action in session  
        // GameSessionDoActionDto 
        // returns ( string / null )

        [HttpPost("do")]
        public IActionResult DoActionGame(GameSessionDoActionDto dto)
        {
            GameSession game = _dbService.GameSessions
                .Where(x => x.Id == dto.Id)
                .Include("Users").Include("CurrentPlayer")
                .FirstOrDefault();

            if (game != null) 
            { 
                if(dto.UserName == game.CurrentPlayer.Name && game.GameStatus != "Complite")
                {
                    try
                    {
                        string[][] arr = JsonSerializer.Deserialize<string[][]>(game.Field);
                        if (arr[dto.PointX][dto.PointY] == "") 
                        {
                            arr[dto.PointX][dto.PointY] = dto.UserName;
                            game.Field = JsonSerializer.Serialize(arr);

                            bool actions = true;
                            foreach (var item1 in arr)
                            {
                                foreach (var item2 in item1)
                                {
                                    if (item2 != "")
                                    {
                                        actions = false;
                                    }
                                }
                            }
                            if (actions)
                            {
                                game.GameStatus = "NoAction";
                            }

                            if (WinCheck(arr)) {
                                game.GameStatus = "Complite";
                                game.CurrentPlayer.Score += 1;

                                _dbService.GameSessions.Update(game);
                                _dbService.SaveChanges();
                                return Ok("Game Complite");
                            }

                            game.CurrentPlayer = game.Users.Where(x => x.Name != game.CurrentPlayer.Name).FirstOrDefault();

                            _dbService.GameSessions.Update(game);
                            _dbService.SaveChanges();
                            
                            return Ok("Action is Ok");
                        }
                    }
                    catch { }
                }
            }
            return Ok("Wrong enter");
        }

        // ADITIONAL FUNCTION Check win  
        // string[][] 
        // returns ( boolean )

        private bool WinCheck(string[][] arr)
        {
            bool win = false;
            int[][] winarrX = new int[][]{
                new int[] { 0, 0, 0 }, new int[] { 1, 1, 1 }, new int[] { 2, 2, 2 }, //stovpci
                new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }, //row
                new int[] { 0, 1, 2 }, new int[] { 2, 1, 0 } //diagonal
            };
            int[][] winarrY = new int[][] {
                new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 },
                new int[] { 0, 0, 0 }, new int[] { 1, 1, 1 }, new int[] { 2, 2, 2 },
                new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }
            };
            for (int i = 0; i < winarrX.Length; i++)
            {
                string val = arr[winarrX[i][0]][winarrY[i][0]];
                if (val != "" && val == arr[winarrX[i][1]][winarrY[i][1]] && val == arr[winarrX[i][2]][winarrY[i][2]])
                {
                    win = true;
                }
            }
            return win;
        }

        // Restart game in room  
        // (Session id, username: string)
        // returns ( string / null )

        [HttpPost("restart")]
        public IActionResult RestartGame(RestartGameDto dto)
        {

            var item = dto;
            GameSession game = _dbService.GameSessions
                .Where(x => x.Id == dto.Id).Include("CurrentPlayer").Include("Users")
                .FirstOrDefault();
            if (game != null)
            {
                if (dto.UserName == game.CurrentPlayer.Name && game.GameStatus == "Complite")
                {
                    var arr = new string[][] { new string[] { "", "", "" }, new string[] { "", "", "" }, new string[] { "", "", "" } };
                    game.CurrentPlayer = game.Users.Where(x => game.CurrentPlayer.Name != x.Name).FirstOrDefault();
                    game.dateTime = DateTime.Now;
                    game.GameStatus = "Active";
                    game.Field = JsonSerializer.Serialize(arr);

                    _dbService.GameSessions.Update(game);
                    _dbService.SaveChanges();

                    return Ok("Restart complite");
                }
            }

            return Ok(null);
        }
    }
}