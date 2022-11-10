using Brætspils_Database.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Brætspils_Database.Sql_Querys
{
    //Interface to tell ADO & EF what to implement
    public interface IConnection : IDisposable
    {
        void Dispose();
        bool IsOpen();
        int AddNewInstance(Game game);
        void DeleteInstance(Game game);
        void UpdateInstance(Game game);
        Task<IEnumerable<Game>?> GetAllGamesAsync(string sqlQuery);
    }
}