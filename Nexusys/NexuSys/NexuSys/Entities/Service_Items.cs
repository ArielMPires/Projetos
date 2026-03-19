using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexuSys.Entities
{
    public class Service_Items
    {
        #region Property

        public int ID { get; set; }
        public int OS { get; set; }
        public int Product { get; set; }
        public int History { get; set; }
        public decimal Value { get; set; }
        public decimal Amount { get; set; }

        #endregion

        #region Navigation

        public History historyFK { get; set; }
        public Service_Order service_orderFK { get; set; }
        public Products productsFK { get; set; }

        #endregion
    }
}
