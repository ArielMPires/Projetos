using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexuSys.Entities
{
    public class Service_Equipment
    {
        #region Property

        public int ID { get; set; }
        public int OS { get; set; }
        public int Equipment { get; set; }

        #endregion

        #region Navigation
        public Service_Order service_orderFK { get; set; }
        public Equipment equipmentFK { get; set; }

        #endregion
    }
}
