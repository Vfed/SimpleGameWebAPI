using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleGameWebAPI.Data.Models
{
    public class GameSession
    {
        public Guid Id { get; set; }
        public List<User> Users { get; set; }
        public User CurrentPlayer { get; set; }
        public string GameStatus { get; set; }
        public DateTime dateTime { get; set; }
        public string Field { get; set; }
    }
}
