using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Models
{
    public class Priority
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public static List<Priority> Priorities { get; } = new List<Priority>
    {
        new Priority { Id = 1, Name = "N1" },
        new Priority { Id = 2, Name = "N2" },
        new Priority { Id = 3, Name = "N3" },
        new Priority { Id = 4, Name = "N4" },
    };

    }
}
