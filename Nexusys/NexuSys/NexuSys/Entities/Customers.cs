using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NexuSys.Entities
{
    public class Customers
    {

        #region Property
        public int ID { get; set; }
        public long CNPJ_CPF { get; set; }
        public string Name { get; set; }
        public string? Name_Fantasy { get; set; }
        public string Address { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string UF { get; set; }
        public int CEP { get; set; }
        public int Seller { get; set; }
        public int History { get; set; }

        #endregion

        #region Navigation
        public Seller? SellerFK { get; set; }
        public History? historyFK { get; set; }
        
        #endregion

    }
}
