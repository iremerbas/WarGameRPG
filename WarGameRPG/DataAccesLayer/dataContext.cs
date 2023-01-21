using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using WarGameRPG.Models;

namespace WarGameRPG.DataAccesLayer
{
    public class dataContext:DbContext
    {
        public dataContext() : base("DbConnection") { }

        public DbSet<Character> Character { get; set; }
    }
}
