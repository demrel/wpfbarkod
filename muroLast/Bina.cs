using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace muroLast
{
   
    public class Bina
    {
        public int BinaId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Inventar> Inventars { get; set; }
    }
}

