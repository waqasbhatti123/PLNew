using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.BL
{
    public class NewExpenditure
    {
        public NewExpenditure()
        {

        }

        public IList<sp_SneBranchWiseResult> GetSNEDemand(int glyear,RMSDataContext db)
        {
            IList<sp_SneBranchWiseResult> sne = db.sp_SneBranchWise(glyear).ToList();
            return sne;
        }

        public IList<sp_SneAllocationResult> GetSNEAllocation(int glyear, RMSDataContext db)
        {
            IList<sp_SneAllocationResult> sne = db.sp_SneAllocation(glyear).ToList();
            return sne;
        }

        public IList<sp_DistGrantAllCouncilResult> GistGrant(int glyear, RMSDataContext db)
        {
            IList<sp_DistGrantAllCouncilResult> sne = db.sp_DistGrantAllCouncil(glyear).ToList();
            return sne;
        }

        public IList<sp_TheatreInformationResult> GetTheInfo(int br, RMSDataContext db)
        {
            IList<sp_TheatreInformationResult> theatre = db.sp_TheatreInformation(br).ToList();
            return theatre;
        }

        public IList<sp_ExpVsAllocationResult> GetExpAll(int glyear,int br, RMSDataContext db)
        {
            IList<sp_ExpVsAllocationResult> exp = db.sp_ExpVsAllocation(glyear,br).ToList();
            return exp;
        }

        public IList<sp_AppBudVsExpenditureResult> GetAppExp(int glyear, int br, RMSDataContext db)
        {
            IList<sp_AppBudVsExpenditureResult> exp = db.sp_AppBudVsExpenditure(glyear, br).ToList();
            return exp;
        }
        public IList<sp_ADPSchemeReportResult> GetAdpScheme(int glyear, RMSDataContext db)
        {
            IList<sp_ADPSchemeReportResult> adp = db.sp_ADPSchemeReport(glyear).ToList();
            return adp;
        }

        public IList<Sp_ExpenditureStatementHeadWiseResult> GetExpenditureStatement(int BranchID,int glyear, RMSDataContext db)
        {
            IList<Sp_ExpenditureStatementHeadWiseResult> Exp = db.Sp_ExpenditureStatementHeadWise(BranchID,glyear).ToList();
            return Exp;
        }

        public IList<sp_ExpenditureStatementMonthWiseResult> GetMonExpenditureStatement(int BranchID, decimal glyear, RMSDataContext db)
        {
            IList<sp_ExpenditureStatementMonthWiseResult> Exp = db.sp_ExpenditureStatementMonthWise(BranchID, glyear).ToList();
            return Exp;
        }

        public IList<sp_BmForm10Result> GetFarm10Data(int BranchID, RMSDataContext db)
        {
            IList<sp_BmForm10Result> bm = db.sp_BmForm10(BranchID).ToList();
            return bm;
        }

        public IList<sp_BMFormthreeResult> GetFarm11Data(int BranchID, RMSDataContext db)
        {
            IList<sp_BMFormthreeResult> bm = db.sp_BMFormthree(BranchID).ToList();
            return bm;
        }

        public IList<sp_BankWiseExpenseResult> GetBankWiseExp(int bnk,int yr, int br, RMSDataContext db)
        {
            IList<sp_BankWiseExpenseResult> bm = db.sp_BankWiseExpense(bnk, yr, br).ToList();
            return bm;
        }
    }
}
