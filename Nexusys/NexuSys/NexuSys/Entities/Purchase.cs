using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexuSys.Entities
{
    public class Purchase
    {
        #region Property

        public int ID { get; set; }
        public int NF { get; set; }
        public int Supplier { get; set; }
        public DateTime Date { get; set; }
        public decimal Value_total { get; set; }
   

        #endregion

        #region Navigation

     
        public NFs NfsFK { get; set; }
        public Suppliers suppliersFK { get; set; }


        #endregion
    }
}
