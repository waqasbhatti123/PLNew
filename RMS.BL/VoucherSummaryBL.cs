using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMS.BL
{
   public class VoucherSummaryBL
    {
       public VoucherSummaryBL()
       { }

       public List<Vr_Type> GetVoucherType(RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           return Data.Vr_Types.Where(t => t.vt_cd  > 60).ToList();
       }

       public List<spCOAResult> GetReport(RMSDataContext Data, char searchType)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           if (searchType == 'X')
           {
               return Data.spCOA().ToList();
           }
           else
           {
               return Data.spCOA().Where(t => t.gt_cd.Equals(searchType)).ToList();

           }
       }

       public FIN_PERD GetFinancialYear(RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }

           return Data.FIN_PERDs.Where(t=>t.Cur_Year =="CUR").Single();
       }

       public List<vwLedger> GetVoucherSummary(int branchId, DateTime dtFrom,DateTime dtTo,string status,string type,RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           List<vwLedger> voucherObject = Data.vwLedgers.Where(t => t.vr_dt >= dtFrom && t.vr_dt <= dtTo && t.br_id == branchId).ToList();
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

    }
}
