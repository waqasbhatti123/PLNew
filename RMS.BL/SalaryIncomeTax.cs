using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.BL
{
    public class SalaryIncomeTax
    {
        public SalaryIncomeTax()
        {

        }

        public IList<sp_TaxDedFromSalaryResult> GetTaxFromsalary(int MonID, int JobType, int BranchID,string Allowance, RMSDataContext db)
        {
            IList<sp_TaxDedFromSalaryResult> tax = db.sp_TaxDedFromSalary(MonID, JobType,BranchID, Allowance).ToList();
            return tax;
        }
        public IList<sp_CpfDedFromSalaryResult> GetCPFFromsalary(int BranchID, int MonID, int JobType, RMSDataContext db)
        {
            IList<sp_CpfDedFromSalaryResult> cpf = db.sp_CpfDedFromSalary(BranchID, MonID, JobType).ToList();
            return cpf;
        }
    }
}
