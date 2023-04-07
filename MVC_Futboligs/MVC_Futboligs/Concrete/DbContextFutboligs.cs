using MVC_Futboligs.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVC_Futboligs.Concrete
{
    public class DbContextFutboligs : DbContext
    {
        public DbContextFutboligs():base("name=DBConnectionString") 
        {

        }
        public virtual DbSet<Teams> TeamsS { get; set; }
        public virtual DbSet<Player> PlayerS { get; set; }

    }
}