using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMS.BL
{
    public class LedgerCardBL
    {
        public LedgerCardBL()
        { }

        public List<sp_LedgerResult>  GetLedgerBal(int brId, DateTime dtFrom, DateTime dtTo, string fromcode, string tocode, decimal glyear, char status, RMSDataContext Data, char ledgerType)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            List<sp_LedgerResult> LedgerObject = Data.sp_Ledger(brId, glyear, dtFrom, dtTo, ledgerType,status).ToList();

            if (fromcode != "" && tocode != "")
            {

                LedgerObject = LedgerObject.Where(t => Convert.ToInt64(t.gl_cd) >= Convert.ToInt64(fromcode) && Convert.ToInt64(t.gl_cd) <= Convert.ToInt64(tocode)).ToList();


            }

            return LedgerObject;
        }


        public List<sp_LedgerInfoResult> GetLedgerBal1(DateTime dtFrom, DateTime dtTo, string glcode, decimal glyear, RMSDataContext Data, char ledgerType)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            List<sp_LedgerInfoResult> LedgerObject = Data.sp_LedgerInfo(1, glcode,  glyear, dtFrom, dtTo, ledgerType).ToList();

            return LedgerObject;
        }


        public List<sp_LedgerSummaryResult> GetLedgerBalSummary(int brId, DateTime dtFrom, DateTime dtTo, string glcode, decimal glyear, RMSDataContext Data, char ledgerType, string type, char status)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            List<sp_LedgerSummaryResult> LedgerObject=Data.sp_LedgerSummary(brId, glyear, dtFrom, dtTo, ledgerType, type, status).ToList();
            if ((type.Equals("D") || type.Equals("C2") || type.Equals("C1") || type.Equals("G")) && (glcode == ""))
            {
                
            }
            else if (type.Equals("D") && glcode != "")
            {
                LedgerObject = LedgerObject.Where(t => Convert.ToInt64(t.code) == Convert.ToInt64(glcode)).ToList();
            }
            else if (type.Equals("C2") && glcode != "")
            {
                LedgerObject = LedgerObject.Where(t => Convert.ToInt64(t.code) == Convert.ToInt64(glcode)).ToList();
            }
            else if (type.Equals("C1") && glcode != "")
            {
                LedgerObject = LedgerObject.Where(t => Convert.ToInt64(t.code) == Convert.ToInt64(glcode)).ToList();
            }
            else if (type.Equals("G") && glcode != "")
            {
                LedgerObject = LedgerObject.Where(t => Convert.ToInt64(t.code) == Convert.ToInt64(glcode)).ToList();
            }
            //else if (type.Equals("G") && glcode != "")
            //{
            //    LedgerObject = LedgerObject.Where(t => Convert.ToInt64(t.code) >= Convert.ToInt64(glcode.Substring(0,2)) && Convert.ToInt64(t.code) <= Convert.ToInt64(tocode.Substring(0,2))).ToList();
            //}
            else
            {
                
            }


            return LedgerObject;
        }

        public List<sp_StkLedgerResult> GetWetBlueStkLedgerBal(DateTime dtFrom, DateTime dtTo, int locId, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            List<sp_StkLedgerResult> LedgerObject = Data.sp_StkLedger(1, locId, dtFrom, dtTo).ToList();

            return LedgerObject;
        }

        public FIN_PERD GetFinancialYear(RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Data.FIN_PERDs.Where(t => t.Cur_Year == "CUR").Single();
        }

        public FIN_PERD GetFinancialYearByDate(DateTime fromdate, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Data.FIN_PERDs.Where(t => fromdate >= t.Start_Date && fromdate <= t.End_Date).Single();
        }
    }
}
