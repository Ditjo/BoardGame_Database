using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brætspils_Database.Sql_Querys
{
    //Sql Querys used by ADO
    public class SqlStatment
    {
        public const string SqlQueryDelete = $"DELETE FROM Game WHERE GameID = @Id";

        public const string SqlQueryInsert = $"INSERT INTO Game (Titel) VALUES (@Titel); SELECT SCOPE_IDENTITY();";

        public const string SqlQuaryUpdate = $"UPDATE Game SET Titel = @Titel, Players = @Players, Playtime = @Time WHERE GameID = @Id";

        public const string SqlQuarySelectAll = $"SELECT * FROM Game";
    }
}
