using SimpleGameWebAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace SimpleGameWebAPI
{
    public class DbService : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<GameSession> GameSessions { get; set; }
        public DbService(DbContextOptions<DbService> options) : base(options)
        { }
    }
}
