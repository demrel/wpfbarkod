using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace muroLast
{
    public class Inventar
    {
        public int InventarID { get; set; }
        public string SN { get; set; }
        public string Barcode { get; set; }
        public int Count { get; set; }
        public int Year { get; set; }
        public string Person { get; set; }
        public string Note { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual Room Room { get; set; }
        public virtual Item Item { get; set; }
        public virtual Bina Bina { get; set; }

    }
}
