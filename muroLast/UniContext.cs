using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace muroLast
{
    class UniContext : DbContext
    {
        public UniContext() : base("UniContext") { }
        public DbSet<Inventar> Inventars { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Bina> Binas { get; set; }
        public DbSet<Checked> CheckedItem { get; set; }
    }
}
