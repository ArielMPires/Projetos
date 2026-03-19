using Agnus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Models
{
    public class Setting
    {
        public static Credencial? credencial { get; set; }

        //public const string BaseUrl = "http://10.1.3.68:5060/Domus/";
        public const string BaseUrl = "http://agnus.ddns.net:5250/Domus/";

        public const string Auth = "Authorization";

        public const string Bearer = "Bearer ";

        public const string Header = "application/json";
    }
}
