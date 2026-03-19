using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Domus.Models.DB;

namespace Domus.Models.DB {
    
    public class Unit {

        #region Property
        [Key]
        public int ID {get; set;}
        public string Name {get; set;}
        public long CNPJ {get; set;}
        #endregion

        #region Navigation
        public ICollection<NF_Output>? NF_OutputFK {get; set;}
        #endregion
    }
}