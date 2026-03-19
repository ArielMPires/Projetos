using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexuSys.Entities
{
    public class Purchase_Items
    {
        #region Property

        public int ID { get; set; }
        public int Purchase { get; set; }
        public int Products { get; set; }
        public Decimal Value { get; set; }
        public Decimal Amount { get; set; }
    

        #endregion

        #region Navigation

        public Purchase purchaseFK { get; set; }
        public Products productsFK { get; set; }

        #endregion
    }
}
