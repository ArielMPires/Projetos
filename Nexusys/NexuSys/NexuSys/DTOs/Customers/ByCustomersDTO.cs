using System.Numerics;

namespace NexuSys.DTOs.Customers
{
    public class ByCustomersDTO
    {
        public int ID { get; set; }
        public long CNPJ_CPF { get; set; }
        public string Name { get; set; }
        public string Name_Fantasy { get; set; }
        public string Address { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string UF { get; set; }
        public int CEP { get; set; }
        public int Seller { get; set; }
        public int History { get; set; }
        public int CreateBy { get; set; }
        public DateTime DateCreate { get; set; }
        public int ChangedBy { get; set; }
        public DateTime DateChanged { get; set; }
    }
}
