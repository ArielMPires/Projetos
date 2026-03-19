using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexuSys.Entities
{
    public class Equipment
    {
        #region Property

        public int ID { get; set; }
        public int Customer { get; set; }
        public int Serial { get; set; }
        public int Product { get; set; }
        public DateTime Manufacturing_Date { get; set; }
        public int History { get; set; }
        public string? Optional { get; set; }

        #endregion


        #region Navigation
        public History? historyFK { get; set; }
        public Customers? customersFK { get; set; }
        public Products? productsFK { get; set; }

        #endregion

    }

}
