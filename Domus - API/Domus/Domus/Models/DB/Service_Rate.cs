using System.ComponentModel.DataAnnotations;

namespace Domus.Models.DB
{
    public class Service_Rate
    {
        #region Property
        [Key]
        public int ID { get; set; }
        public int Order { get; set; }
        public int Score { get; set; }
        public string? Reason { get; set; }
        #endregion

        #region Navigation
        public Service_Order? OrderFK { get; set; }
        #endregion
    }
}
