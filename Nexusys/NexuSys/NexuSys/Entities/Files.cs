using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexuSys.Entities
{
    public class Files
    {
        #region Property

        public int ID { get; set; }
        public string Name { get; set; }
        public int? Folder { get; set; }
        public int? History { get; set; }

        #endregion
        
        #region Navigation
        public History? historyFK { get; set; }
        public FileFolder? folderFK { get; set; }
        
        #endregion
    }
}
