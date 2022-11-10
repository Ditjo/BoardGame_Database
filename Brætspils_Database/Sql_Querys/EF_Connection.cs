using Brætspils_Database.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brætspils_Database.Sql_Querys
{
    //Entity Framework Connection:
    //In use
    internal class EF_Connection : IConnection
    {

        public void Dispose()
        {
            //if (IsOpen())
            //{
            //    context.Database.Connection.Close();
            //}
            //return;
        }

        public bool IsOpen()
        {
            //if (context == null)
            //{
            //    return false;
            //}
            //if (context.Database.Connection.State == ConnectionState.Open)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            return false;
        }

        GameContext context { get; set; }

        public int AddNewInstance(Game game)
        {

            using (context = new GameContext())
            {
                context.Database.Connection.Open();
                var test = context.Database.Connection.State;
                //List<Game> list = new List<Game>();
                context.Games.Add(game);
                context.SaveChanges();
            }
            return game.Id;
        }

        public void DeleteInstance(Game game)
        {
            using(context = new GameContext())
            {
                context.Entry(game).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public void UpdateInstance(Game game)
        {
            using (context = new GameContext())
            {
                context.Games.Attach(game);
                context.Entry(game).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public async Task<IEnumerable<Game>?> GetAllGamesAsync(string sqlQuery)
        {
            using (context = new GameContext())
            {
                IQueryable<Game> query = context.Games;
                
                //List<Game> list = new();
                //await Task.Run(() =>
                //{
                //    list = context.Games.ToList();
                //});
                //return list;
                return await query.ToListAsync();
            }
        }

    }
}