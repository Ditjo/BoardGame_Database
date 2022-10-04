using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brætspils_Database.Model
{
    //The Model every game object is based on.
    public class Game
    {
        public int Id { get; set; }
        public string? Titel { get; set; }

        public string? Players { get; set; }

        public string? Time { get; set; }

    }
}
