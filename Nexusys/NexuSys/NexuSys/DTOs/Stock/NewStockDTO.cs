using System.Numerics;

namespace NexuSys.DTOs.Stock
{
    public class NewStockDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public BigInteger CNPJ_CPF { get; set; }
        public int CreateBy { get; set; }
        public DateTime DateCreate { get; set; }
    }
}
