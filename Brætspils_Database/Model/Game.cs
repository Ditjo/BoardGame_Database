using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brætspils_Database.Model 
{
    //The Model every game object is based on.
    //Tells witch table to get
    [Table("Game")]
    public class Game 
    {
        //Tells that the Id is the same as GameId
        [Column("GameId")]
        public int Id { get; set; }

        public string? Titel { get; set; }

        public string? Players { get; set; }

        [Column("Playtime")]
        public string? Time { get; set; }

    }
}
