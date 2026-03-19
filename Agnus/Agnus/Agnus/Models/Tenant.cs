using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Models
{
    public class Tenant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public static List<Tenant> Tenants { get; } = new List<Tenant>
            {
                new Tenant() { Id = 1, Name = "Waldesa Comercio"},
                new Tenant() { Id = 2, Name = "Waldesa MotoMercantil Jundiapeba"},
                new Tenant() { Id = 3, Name = "Waldesa MotoMercantil BrazCubaz"},
                new Tenant() { Id = 4, Name = "Waldesa MotoMercantil RJ"},
            };
    }
}
