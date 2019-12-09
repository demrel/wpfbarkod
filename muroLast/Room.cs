using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace muroLast
{
    public class Room
    {
        public int RoomID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Inventar> Inventars { get; set; }
    }
}
