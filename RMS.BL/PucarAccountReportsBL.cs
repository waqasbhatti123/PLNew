using RMS.BL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.BL
{
    public class PucarAccountReportsBL
    {
        public PucarAccountReportsBL()
        {

        }


        


        public IQueryable<spTestGLResult> SingleVoucherReport(int vrId, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                IQueryable<spTestGLResult> pckgs =
                    Data.spTestGL(vrId).AsQueryable();

                return pckgs;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }





        public IQueryable<spBPnBRResult> SingleVoucherReport1(int vrId, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                IQueryable<spBPnBRResult> pckgs =
                    Data.spBPnBR(vrId).AsQueryable();

                return pckgs;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }


        public IList<sp_Leadgerdbo> LedgerReports(int brID, decimal glyear, DateTime? dateFrom, DateTime? dateTo,char ledgerType, char status, string fromCode, string toCode, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                List<sp_LedgerResult> pckgs =
                    Data.sp_Ledger(brID, glyear, dateFrom, dateTo, ledgerType,status).ToList();
                var list = new List<sp_Leadgerdbo>();
                foreach (var item in pckgs)
                {
                    var vrId = item.vrid;
                    sp_Leadgerdbo lead = new sp_Leadgerdbo();
                    lead.vrid = vrId;
                    lead.Code = item.Code;
                    lead.ParentCode = item.ParentCode;
                    lead.Ref_no = item.Ref_no;
                    lead.br_id = item.br_id;
                    lead.GlYear = item.gl_year;
                    lead.vt_cd = item.vt_cd;
                    lead.vr_no = item.vr_no;
                    lead.vt_use = item.vt_use;
                    lead.source = item.source;
                    lead.vt_dsc = item.vt_dsc;
                    lead.vr_dt = item.vr_dt;
                    lead.vr_nrtn = item.vr_nrtn;
                    lead.vr_apr = item.vr_apr;
                    lead.updateon = item.updateon;
                    lead.updateby = item.updateby;
                    lead.vr_seq = item.vr_seq;
                    lead.amount = item.amount;
                    lead.AmountForCloseBalnceDebit = item.AmountForCloseBalnceDebit;
                    lead.AmountForCloseBalnceCredit = item.AmountForCloseBalnceCredit;
                    lead.vrd_debit = item.vrd_debit;
                    lead.vrd_credit = item.vrd_credit;
                    lead.vrd_nrtn = item.vrd_nrtn;
                    lead.gl_cd = item.gl_cd;
                    lead.cc_cd = item.cc_cd;
                    lead.gl_dsc = item.gl_dsc;
                    lead.cc_nme = item.cc_nme;
                    lead.cnt_gl_cd = item.cnt_gl_cd;
                    lead.cnt_gl_dsc = item.cnt_gl_dsc;
                    lead.act_status = item.act_status;
                    lead.vr_chq = item.vr_chq;
                    lead.vr_chq_dt = item.vr_chq_dt;
                    lead.gl_op = GetOpeningBalance(item.gl_cd, item.gl_year, item.br_id);
                    if (vrId > 0)
                    {
                        var debitrecord = Data.Glmf_Data_Dets.Where(x => x.vrid == vrId).ToList();
                        foreach (var iu in debitrecord)
                        {
                            lead.vrdebit += iu.vrd_debit;
                        }
                        
                        var nt = Data.Glmf_Datas.Where(x => x.vrid == vrId).FirstOrDefault().NTN;
                        lead.ntn = nt;
                    }
                    list.Add(lead);
                }

                //List<sp_LedgerResult> LedgerObject = Data.sp_Ledger(brId, glyear, dtFrom, dtTo, ledgerType, status).ToList();

                if(fromCode != "" && toCode == "")
                {
                    list = list.Where(t => Convert.ToInt64(t.gl_cd) == Convert.ToInt64(fromCode)).ToList();
                }

                if (fromCode != "" && toCode != "")
                {
                    list = list.Where(t => Convert.ToInt64(t.gl_cd) >= Convert.ToInt64(fromCode) && Convert.ToInt64(t.gl_cd) <= Convert.ToInt64(toCode)).ToList();
                }

                return list;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }


        public decimal? GetOpeningBalance(string gl_cd, decimal? glyear, int? brid)
        {
            using (RMSDataContext db = new RMSDataContext())
            {
             var op =   db.Budgets.Where(x => x.Account == gl_cd && x.GlYear == glyear && x.BudgetTypeID == 2 && (x.QuarterID == 1 || x.QuarterID == 2 || x.QuarterID == 3
                || x.QuarterID == 4) && x.IsActive == true && x.br_id == brid).Sum(x => x.Grant);
                return op;
            }
            
        }


        public List<vwLedger> VoucherSummaryReport(int brID,  DateTime? dateFrom, DateTime? dateTo, string status, string type, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }               

                List<vwLedger> voucherObject = Data.vwLedgers.Where(t => t.vr_dt >= dateFrom && t.vr_dt <= dateTo && t.br_id == brID).ToList();
                if (status == "All" && type != "0")
                {
                    voucherObject = voucherObject.Where(t => t.vt_cd == Convert.ToInt32(type)).ToList();
                }
                if (type == "0" && status != "All")
                {
                    voucherObject = voucherObject.Where(t => t.vr_apr == status).ToList();
                }

                if (status != "All" && type != "0")
                {
                    voucherObject = voucherObject.Where(t => t.vr_apr == status).ToList();
                    voucherObject = voucherObject.Where(t => t.vt_cd == Convert.ToInt32(type)).ToList();
                    //&& t.vt_cd.Equals(Convert.ToInt32(type))).ToList();

                }

                return voucherObject;

               
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }





        public List<sp_GL_Ledger_CCResult> CostCentreReport(int brID, decimal glyear, DateTime? dateFrom, DateTime? dateTo, string cc, string glCode,  RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                List<sp_GL_Ledger_CCResult> pckgs =
                    Data.sp_GL_Ledger_CC(brID, glyear, dateFrom, dateTo, cc, glCode).ToList();

                return pckgs;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }





        public List<spCheqDetailsResult> ChequeReport(int type, string bank, string cheque, string voucherNo, string glCode,  DateTime? dateFrom, DateTime? dateTo, int brId, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }


                if (type == 0)//Not cleared
                    return Data.spCheqDetails(bank, cheque, voucherNo, glCode, dateFrom, dateTo, brId).Where(c => c.Chq_clr_dt == null).ToList();
                else//cleared
                    return Data.spCheqDetails(bank, cheque, voucherNo, glCode, dateFrom, dateTo, brId).Where(c => c.Chq_clr_dt != null).ToList();

               

            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }




        public IQueryable<sp_GL_Ledger_CC_SummaryResult> CostCentreSummaryReport(int brId,decimal glYear, DateTime? dateFrom, DateTime? dateTo,string grpCC,char status, string ccFrom, string ccTo, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                IQueryable<sp_GL_Ledger_CC_SummaryResult> pckgs =
                    Data.sp_GL_Ledger_CC_Summary(brId,glYear, dateFrom, dateTo, grpCC, status, ccFrom, ccTo).AsQueryable();

                return pckgs;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }




        public IQueryable<sp_GL_Ledger_CC_DetailResult> CostCentreDetailReport(int brId, decimal glYear, DateTime? dateFrom, DateTime? dateTo, string grpCC, char status, string ccFrom, string ccTo, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                IQueryable<sp_GL_Ledger_CC_DetailResult> pckgs =
                    Data.sp_GL_Ledger_CC_Detail(brId, glYear, dateFrom, dateTo, grpCC, status, ccFrom, ccTo).AsQueryable();

                return pckgs;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }



    }
}
