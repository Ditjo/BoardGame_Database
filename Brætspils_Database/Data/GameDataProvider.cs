using Brætspils_Database.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brætspils_Database.Data
{
    public interface IGameDataProvider
    {
        Task<IEnumerable<Game>?> GetAllAsync();
    }
    // Old DataPrvider \/
    public class GameDataProvider : IGameDataProvider
    {
        public async Task<IEnumerable<Game>?> GetAllAsync()
        {
            //await Task.Delay(100); //Simulate server work
            List<Game> list = null;
            await Task.Run(() =>
           {
               list = new List<Game>
           {
                new Game{Id=1, Titel="Carcassone", Players="2-6", Time="60-90 min"},
                new Game{Id=2, Titel="Ticket to Ride", Players="2-5", Time="60 min"},
                new Game{Id=3, Titel="Raiders of the Northsea", Players="2-4", Time="60-90 min"},
                new Game{Id=4, Titel="Love Letter", Players="2-4", Time="20-30 min"},
           };
           });
            return list;
        }

    }

    //New Data Provider (DataBase)
    public class GameDataDbProvider : IGameDataProvider
    {
        public async Task<IEnumerable<Game>?> GetAllAsync()
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DataBaseConnection"].ConnectionString;

            string sqlQuary = "SELECT * FROM Games";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sqlQuary, conn))
                {
                    List<Game> list = null;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        await Task.Run(() =>
                        {
                            list = new List<Game>();

                            while (reader.Read())
                            {
                                Game game = new Game()
                                {
                                    Id = reader.GetInt32("ID"),
                                    Titel = reader.IsDBNull("Titel") ? null : reader.GetString("Titel"),
                                    Players = reader.IsDBNull("Players") ? null : reader.GetString("Players"),
                                    Time = reader.IsDBNull(3) ? null : reader.GetString(3)
                                };
                                list.Add(game);
                            }
                        });
                    }
                    return list;

                }
            }
        }
    }
}

