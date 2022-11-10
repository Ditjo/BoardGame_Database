using System.Data.Entity;

namespace Game.DataModel
{
    public class GameContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
    }
}