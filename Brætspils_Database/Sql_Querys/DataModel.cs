using Brætspils_Database.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brætspils_Database.Sql_Querys
{
    //DataModel used in Entity Framework
    public class GameContext : DbContext
    {

        public DbSet<Game> Games { get; set; }

        public GameContext() : base("name=GameContext") 
        {
            Database.SetInitializer<GameContext>(null);
            this.Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Game>()
                .HasKey(x => x.Id)
                ;
        }
    }
}
