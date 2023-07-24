using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.BL.Model
{
    public class ExcessSurrenderModel
    {
        public string Accounts { get; set; }
        public string code { get; set; }
        public decimal AppBudget { get; set; }
        public decimal SupplementryBudget { get; set; }
        public decimal ApprovedAmountReapp { get; set; }
        public decimal ModifiedEstimateBudget { get; set; }
        public decimal fourMonthExpCurrentFinancialYear { get; set; }
        public decimal eightmonthpreviousFinancialYear { get; set; }
        public decimal eightmonthcurrentfinancialyear { get; set; }
        public decimal fourmonthpreviousfinancialyer { get; set; }
        public decimal ProposedReappropriationbyBudgetAD { get; set; }
        public decimal AntiRevisedExpcurrentyear { get; set; }
        public decimal Excess { get; set; }
        public decimal Surrender { get; set; }
        public string Gldsc { get; set; }

        public decimal TotalActual { get; set; }
    }
}
