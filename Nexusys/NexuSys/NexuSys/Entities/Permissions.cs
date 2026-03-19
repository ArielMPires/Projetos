using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexuSys.Entities
{
    public class Permissions
    {
        #region Property
        public int ID { get; set; }
        public int Role { get; set; }
        public string Page { get; set; }
      


        #endregion

        #region Navigation
      

        public Role rolefk { get; set; }


        #endregion
    }
}
