using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.BL.Model
{
    public class BudgetModel
    {
        public string ParentAccount { get; set; }
        public int BudgetID { get; set; }
		public int br_id { get; set; }

		public int BudgetTypeID { get; set; }

		public string GUID { get; set; }

		public System.Nullable<int> QuarterID { get; set; }

		public System.Nullable<decimal> GlYear { get; set; }

		public string Account { get; set; }

		public System.Nullable<decimal> Income { get; set; }

		public System.Nullable<decimal> Grant { get; set; }

		public System.Nullable<decimal> Aid { get; set; }

		public System.Nullable<bool> IsActive { get; set; }

		public System.Nullable<int> brid { get; set; }
        public int SchemeID { get; set; }

        public System.Nullable<decimal> GrantinQ1 { get; set; }
        public System.Nullable<decimal> GrantinQ2 { get; set; }
        public System.Nullable<decimal> GrantinQ3 { get; set; }
        public System.Nullable<decimal> GrantinQ4 { get; set; }
        public System.Nullable<decimal> firstExcess { get; set; }
        public System.Nullable<decimal> firstSurrender { get; set; }
        public System.Nullable<decimal> secondExcess { get; set; }
        public System.Nullable<decimal> secondSurrender { get; set; }
        public System.Nullable<decimal> firstapprop { get; set; }
        public System.Nullable<decimal> secondapprop { get; set; }
        public System.Nullable<decimal> thirdapprop { get; set; }
        public System.Nullable<decimal> forthapprop { get; set; }
        public System.Nullable<decimal> total { get; set; }


        public int Variance { get; set; }

        public string GlDesc { get; set; }

        public System.Nullable<decimal> Expenditure { get; set; }


	}
}
