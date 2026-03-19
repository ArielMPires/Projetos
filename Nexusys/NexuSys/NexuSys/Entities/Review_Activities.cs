using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexuSys.Entities
{
    public class Review_Activities
    {
        #region Property

        public int ID { get; set; }
        public int Review { get; set; }
        public int Activities { get; set; }
        public int History { get; set; }

        #endregion

        #region Navigation
        public Repair_Activities ActivitiesFK { get; set; }
        public Review ReviewFK { get; set; }
        public History historyFK { get; set; }
        
        #endregion
    }
}
