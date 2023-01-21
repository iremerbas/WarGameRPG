using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarGameRPG.Models
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Health { get; set; }
        public int Durability { get; set; }
        public int Power { get; set; }
        public int Speed { get; set; }
        public string Level { get; set; }
        public int Score { get; set; }
    }
}
