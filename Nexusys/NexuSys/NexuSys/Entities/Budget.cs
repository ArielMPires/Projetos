using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexuSys.Entities
{
    public class Budget
    {
        #region Property
        
        public int ID { get; set; }
        public int OS { get; set; }
        public DateTime Date { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public bool? Situation { get; set; }
        public decimal Value { get; set; }
        public string Note { get; set; }
        public int FileFolder { get; set; }
        public int History { get; set; }

        #endregion

        #region Navigation
        public FileFolder FileFolderFK { get; set; }
        public History historyFK { get; set; }
        public Service_Order service_orderFK { get; set; }
        public Review ReviewFK { get; set; }
        
        #endregion
    }
}
