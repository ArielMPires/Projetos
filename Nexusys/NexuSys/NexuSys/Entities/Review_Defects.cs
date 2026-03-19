using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexuSys.Entities
{
    public class Review_Defects
    {
        #region Property
        
        public int ID { get; set; }
        public int Review { get; set; }
        public int Defects { get; set; }
        public int History { get; set; }

        #endregion

        #region Navigation
        public Possible_Defects DefectsFK { get; set; }
        public Review ReviewFK { get; set; }
        public History historyFK { get; set; }

        #endregion
    }
}
