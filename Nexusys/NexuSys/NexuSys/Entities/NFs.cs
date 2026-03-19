using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexuSys.Entities
{
    public class NFs
    {
        #region Property
        public int ID { get; set; }
        public int Number { get; set; }
        public int Customers { get; set; }
        public int Type { get; set; }
        public DateTime DateIn { get; set; }
        public decimal? Total_Value { get; set; }
        public decimal? Tax { get; set; }
        public int Folder { get; set; }

        #endregion

        #region Navigation
        public Customers? customersFK { get; set; }
        public FileFolder? folderFK { get; set; }

        #endregion
    }
}
