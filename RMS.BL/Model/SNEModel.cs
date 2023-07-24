using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.BL.Model
{
    public class SNEModel
    {
        public int BudgetID { get; set; }
        public int br_id { get; set; }

        public int BudgetTypeID { get; set; }

        public System.Nullable<int> QuarterID { get; set; }

        public System.Nullable<decimal> GlYear { get; set; }

        public System.Nullable<decimal> ApGrant { get; set; }

        public string Account { get; set; }

        public System.Nullable<decimal> Income { get; set; }

        public System.Nullable<decimal> Grant { get; set; }

        public System.Nullable<decimal> Aid { get; set; }

        public System.Nullable<bool> IsActive { get; set; }

        public System.Nullable<int> brid { get; set; }
        
        public string GlDesc { get; set; }
        public int Variance { get; set; }
    }
}
