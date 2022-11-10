using Brætspils_Database.Data;
using Brætspils_Database.Model;
using Brætspils_Database.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

//ADO Connection: 
//Not in use. Created as a learning experience 
namespace Brætspils_Database.Sql_Querys
{
    public class Connection : IConnection
    {
        public void Dispose()
        {
            if (IsOpen())
            {
                System.Diagnostics.Debug.WriteLine($"Calls IsOpen + {DateTime.Now:dd-MM-yyyy HH:mm:ss}");
                conn.Close();
            }
        }

        public bool IsOpen()
        {
            if (conn == null)
            {
                return false;
            }
            if (conn.State == System.Data.ConnectionState.Open)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private SqlConnection? conn { set; get; }

        private string DBConn = "GameContext";

        public int AddNewInstance(Game game)
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[DBConn].ConnectionString;

            conn = new SqlConnection(connectionString);

            SqlCommand cmd = new SqlCommand(SqlStatment.SqlQueryInsert, conn);

            List<SqlParameter> list = new List<SqlParameter>();

            list.Add(new SqlParameter($"@Titel", game.Titel));

            cmd.Parameters.AddRange(list.ToArray());

            conn.Open();

            int id = 0;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                reader.Read();

                if (int.TryParse(reader[0].ToString(), out int res))
                {
                    id = res;
                }
            }
            return id;
        }

        public void DeleteInstance(Game game)
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[DBConn].ConnectionString;

            conn = new SqlConnection(connectionString);

            SqlCommand cmd = new SqlCommand(SqlStatment.SqlQueryDelete, conn);

            List<SqlParameter> list = new List<SqlParameter>();

            list.Add(new SqlParameter($"@Id", game.Id));

            cmd.Parameters.AddRange(list.ToArray());

            conn.Open();

            cmd.CommandType = System.Data.CommandType.Text;

            cmd.ExecuteNonQuery();
        }

        public void UpdateInstance(Game game)
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[DBConn].ConnectionString;

            conn = new SqlConnection(connectionString);

            SqlCommand cmd = new SqlCommand(SqlStatment.SqlQuaryUpdate, conn);

            List<SqlParameter> list = new List<SqlParameter>();

            list.Add(new SqlParameter($"@Id", game.Id));
            list.Add(new SqlParameter($"@Titel", game.Titel != null ? game.Titel : DBNull.Value));
            list.Add(new SqlParameter($"@Players", game.Players != null ? game.Players : DBNull.Value));
            list.Add(new SqlParameter($"@Time", game.Time != null ? game.Time : DBNull.Value));

            cmd.Parameters.AddRange(list.ToArray());

            conn.Open();

            cmd.CommandType = System.Data.CommandType.Text;

            cmd.ExecuteNonQuery();
        }

        public async Task<IEnumerable<Game>?> GetAllGamesAsync(string sqlQuery)
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[DBConn].ConnectionString;

            conn = new SqlConnection(connectionString);

            SqlCommand cmd = new SqlCommand(sqlQuery, conn);

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
                            Id = reader.GetInt32(0),
                            Titel = reader.IsDBNull(1) ? null : reader.GetString(1),
                            Players = reader.IsDBNull(2) ? null : reader.GetString(2),
                            Time = reader.IsDBNull(3) ? null : reader.GetString(3)
                        };
                        list.Add(game);
                    }
                });
            }
            return list;

        }

    }

    //public class GameDataDbProvider : IGameDataProvider
    //{
    //    public async Task<IEnumerable<Game>?> GetAllAsync(string sqlQuery)
    //    {
    //        var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DataBaseConnection"].ConnectionString;

    //        //string sqlQuary = "SELECT * FROM Games";

    //        using (SqlConnection conn = new SqlConnection(connectionString))
    //        {
    //            using (SqlCommand cmd = new SqlCommand(sqlQuary, conn))
    //            {
    //                List<Game> list = null;
    //                conn.Open();

    //                using (SqlDataReader reader = cmd.ExecuteReader())
    //                {

    //                    await Task.Run(() =>
    //                    {
    //                        list = new List<Game>();

    //                        while (reader.Read())
    //                        {
    //                            Game game = new Game()
    //                            {
    //                                Id = reader.GetInt32("ID"),
    //                                Titel = reader.IsDBNull("Titel") ? null : reader.GetString("Titel"),
    //                                Players = reader.IsDBNull("Players") ? null : reader.GetString("Players"),
    //                                Time = reader.IsDBNull(3) ? null : reader.GetString(3)
    //                            };
    //                            list.Add(game);
    //                        }
    //                    });
    //                }
    //                return list;

    //            }
    //        }
    //    }
    //}

    //public class SqlQueryStatement
    //{
    //    public ObservableCollection<GameItemViewModel> Games { get; } = new();

    //    //private GameItemViewModel? _latestSavedGame;
    //    private GameItemViewModel? _selectedGame;


    //    public GameItemViewModel? SelectedGame
    //    {
    //        get
    //        {
    //            return _selectedGame;
    //        }
    //        set
    //        {
    //            _selectedGame = value;
    //        }
    //    }

    //        public SqlCommand AddParameters(SqlCommand cmd, object? parameter)
    //        {
    //            string par = "@Titel";
    //            List<SqlParameter> list = new List<SqlParameter>();

    //            //foreach (PropertyInfo prop in SelectedGame.GetType().GetProperties())
    //            //{
    //            //    //prop.GetValue(prop.Name)
    //            //    //list.Add(new SqlParameter($"@{prop.Name}", prop.GetValue(SelectedGame, null)));

    //            //    System.Diagnostics.Debug.WriteLine(prop);
    //            //}
    //list.Add(new SqlParameter($"{par}", SelectedGame.Titel));
    //            cmd.Parameters.AddRange(list.ToArray());//.AddWithValue($"{par}", SelectedGame.Titel);

    //            return cmd;
    //        }

    //        public void AddGame(object? parameter)
    //        {

    //            var game = new Game { Titel = "New" };
    //            var viewModel = new GameItemViewModel(game);
    //            Games.Add(viewModel);
    //            SelectedGame = viewModel;


    //            using (Connection sqlQuerys = new Connection())
    //            {
    //                var cmd = sqlQuerys.Query(SqlStatment.SqlQueryInsert);

    //                AddParameters(cmd, parameter);

    //                //cmd.Parameters.AddWithValue("@Titel", SelectedGame.Titel);

    //                using (SqlDataReader reader = cmd.ExecuteReader())
    //                {
    //                    reader.Read();

    //                    if (int.TryParse(reader[0].ToString(), out int res))
    //                    {
    //                        game.Id = res;
    //                    }
    //                }
    //            }
    //        }

    //        public void Edit(object? parameter)
    //        {
    //            _latestSavedGame = new GameItemViewModel(new Game()
    //            {
    //                Id = SelectedGame.Id,
    //                Players = SelectedGame.Players,
    //                Time = SelectedGame.Time,
    //                Titel = SelectedGame.Titel
    //            });
    //        }

    //        public void DeleteGame(object? parameter)
    //        {
    //            if (SelectedGame is not null)
    //            {
    //                using (Connection sqlQuerys = new Connection())
    //                {
    //                    var cmd = sqlQuerys.Query(SqlStatment.SqlQueryDelete);

    //                    cmd.Parameters.AddWithValue("@Id", SelectedGame.Id);

    //                    cmd.CommandType = System.Data.CommandType.Text;

    //                    cmd.ExecuteNonQuery();

    //                    Games.Remove(SelectedGame);
    //                    SelectedGame = null;

    //                }
    //            }
    //        }

    //        public void SaveGame(object? parameter)
    //        {
    //            using (Connection sqlQuerys = new Connection())
    //            {
    //                var cmd = sqlQuerys.Query(SqlStatment.SqlQuaryUpdate);

    //                cmd.Parameters.AddWithValue("@Titel", SelectedGame.Titel != null ? SelectedGame.Titel : DBNull.Value);
    //                cmd.Parameters.AddWithValue("@Players", SelectedGame.Players != null ? SelectedGame.Players : DBNull.Value);
    //                cmd.Parameters.AddWithValue("@Time", SelectedGame.Time != null ? SelectedGame.Time : DBNull.Value);
    //                cmd.Parameters.AddWithValue("@ID", SelectedGame.Id);

    //                cmd.CommandType = System.Data.CommandType.Text;

    //                var res = cmd.ExecuteNonQuery();
    //            }
    //        }

    //        public void Undo(object? parameter)
    //        {
    //            _selectedGame = _latestSavedGame;
    //            GameItemViewModel game = Games.Where(x => x.Id == _selectedGame.Id)
    //                .FirstOrDefault();
    //            var index = Games.IndexOf(game);
    //            Games[index] = _selectedGame;

    ////        }
    //    }

}
