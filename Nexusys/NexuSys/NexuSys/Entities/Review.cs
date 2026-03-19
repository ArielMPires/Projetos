using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexuSys.Entities
{
    public class Review
    {
        #region Property 

        public int ID { get; set; }
        public int Budget_number { get; set; }
        public int Type_Defect { get; set; }
        public string Logged_Faults { get; set; }
        public string Defect_Reported { get; set; }

        #endregion

        #region Navigation
        public Budget budgetFK { get; set; }
        public List<Review_Activities> ActiviesFK { get; set; }
        public List<Review_Defects> DefectsFK { get; set; }

        #endregion

    }
}
