using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NexuSys.Entities
{
    public class Stock
    {

        #region Property

        public int ID { get; set; }
        public int Product { get; set; }
        public decimal Amount { get; set; }
    

        #endregion

        #region Navigation
       
        public Products productsFK { get; set; }


        #endregion

    }
}
