using Brætspils_Database.Model;
using System.Data.Entity;

namespace GameContext.DataModel
{
    public class GameContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
    }
}