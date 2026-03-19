using Agnus.Models.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Models
{
    public class Credencial
    {
        public string ID { get; set; }
        public string Nome { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpired { get; set; }
        public string tenantId { get; set; }

        public ICollection<DB.Permissions>? Permissions { get; set; }

    }
}
