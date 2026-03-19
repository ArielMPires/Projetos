using System.ComponentModel.DataAnnotations;
using Agnus.Models.DB;

namespace Agnus.Models.DB
{
    public class Suppliers{
        #region Property
        [Key]
        public int ID { get; set; }
        public long CNPJ_CPF { get; set; }
        public string Name { get; set; }
        public string? Razao_Social { get; set; }
        public string? Agencia { get; set; }
        public string? Banco { get; set; }
        public string? Numero_conta { get; set; }
        public string? Vendedor { get; set; }
        public string? Telefone { get; set; }
        public string? Celular { get; set; }
        public string? Email { get; set; }
        public string? Endereco { get; set; }
        public int? Numero { get; set; }
        public string? Bairro { get; set; }
        public int? Cep { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? DateCreate { get; set; }
        public int? ChangedBy { get; set; }
        public DateTime? DateChanged { get; set; }
        #endregion

        #region Navigation
        public Users? CreateByFK { get; set; }
        public Users? ChangedByFK { get; set; }
        public ICollection <Product_Supplier> SupplierFK { get; set; }
        public ICollection <Purchase_Order> PurchaseFK { get; set; }
        public ICollection <NF_Input> InputFK { get; set; }
        #endregion

    }
}