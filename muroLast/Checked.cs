using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace muroLast
{
   
    public class Checked
    {
        public int CheckedID { get; set; }
        public DateTime ChechkedTime { get; set; }
        public virtual Inventar Inventar { get; set; }
    }
}

