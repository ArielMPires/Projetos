using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexuSys.Entities
{
    public class History
    {
        #region Property

        public int ID { get; set; }
        public int CreateBy{ get; set; }
        public DateTime DateCreate { get; set; }
        public int? ChangedBy { get; set; }
        public DateTime? DateChanged { get; set; }

        #endregion

        #region Navigation

        public Users? CreateByFK { get; set; }
        public Users? ChangeByFK { get; set; }

        #endregion
    }
}
