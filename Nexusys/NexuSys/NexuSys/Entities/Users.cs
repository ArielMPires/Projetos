using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexuSys.Entities
{
    public class Users
    {
        #region Property

        public int ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }
        public int? Department { get; set; }
        public byte[]? Photo { get; set; }
        public int? Role { get; set; }
        public int? History { get; set; }

        #endregion

        #region Navigation
        public Department? DepartmentFK { get; set; }
        public History? historyFK { get; set; }
        public Role? RoleFK { get; set; }
        #endregion

    }
}
