using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexuSys.Entities
{
    public class Department
    {
        #region Property

        public int ID { get; set; }
        public string Name { get; set; }
        public int History { get; set; }

        #endregion

        #region Navigation

        public History? historyFK { get; set; }

        #endregion

    }
}
