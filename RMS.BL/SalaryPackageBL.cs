using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.BL
{
    public class SlalaryPacakageBL
    {
        public SlalaryPacakageBL()
        {

        }

        public IList<sp_SalarySlipForWResult> SalarySlipSReport(int monthId, int empID, int branchId, int jobType, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                IList<sp_SalarySlipForWResult> pckgs =
                    Data.sp_SalarySlipForW(monthId, empID, branchId, jobType).ToList();

                return pckgs;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public IList<sp_GetOneClickSalarySlipResult> SalarySlipSReportListAllowance(int monthId, int empID, int branchId, int jobType, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                IList<sp_GetOneClickSalarySlipResult> pckgs =
                    Data.sp_GetOneClickSalarySlip(monthId, empID, branchId, jobType).ToList();

                return pckgs = pckgs.Where(x => x.Basic != 0).ToList();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public IList<sp_GetAllowanceResult> SalarySlipSReportListAllwo(int monthId, int empID, int branchId, int jobType, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                IList<sp_GetAllowanceResult> pckgs =
                    Data.sp_GetAllowance(monthId, empID, branchId, jobType).ToList();
                return pckgs;
                //return pckgs = pckgs.Where(x => x.AllownanceType == "Allowance").ToList();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public IList<sp_SalaryReportConsolaResult> ConsolidatedReport(int monthId, int empID, int branchId, int jobType, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                IList<sp_SalaryReportConsolaResult> pckgs =
                    Data.sp_SalaryReportConsola(monthId, empID, branchId, jobType).ToList();

                return pckgs;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public IList<SP_SalaryReportsResult> SalaryPackageReport (int monthId, int empID, int branchId,int jobType, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                IList<SP_SalaryReportsResult> pckgs =
                    Data.SP_SalaryReports(monthId, empID, branchId, jobType).ToList();

                return pckgs;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public IList<sp_SalaryForLACResult> SalaryPackageForLAC(  int branchId, int jobType, int empID, int monthId, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                IList<sp_SalaryForLACResult> pckgs =
                    Data.sp_SalaryForLAC(branchId, jobType, monthId).ToList();

                return pckgs;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }


        public IQueryable<SP_SalarySummaryReportResult> SalarySummaryReport(int monthId,int branchId,int jobType, int empID , RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                IQueryable<SP_SalarySummaryReportResult> salSummaryList =
                    Data.SP_SalarySummaryReport(monthId, branchId, jobType,empID).AsQueryable();

                return salSummaryList;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }


        public IList<SP_MonthlySalarySummaryEmpReportResult> SalarySummaryEmpReport(int monthId, int branchId,int jobType, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                IList<SP_MonthlySalarySummaryEmpReportResult> salSummaryEmpList =
                    Data.SP_MonthlySalarySummaryEmpReport(monthId, branchId, jobType).ToList();

                return salSummaryEmpList;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }


    }
}
