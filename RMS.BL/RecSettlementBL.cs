using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Linq;

namespace RMS.BL
{
    public class RecSettlementBL
    {
        public RecSettlementBL()
        { }


        public string SaveSettlements(EntitySet<tblBill> entBill, EntitySet<tblSettlement> entSet, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            /*OPEN AND BEGIN TRANS*/
            System.Data.Common.DbTransaction trans = null;
            if (Data.Connection.State != ConnectionState.Open)
            {
                Data.Connection.Open();
            }
            trans = Data.Connection.BeginTransaction();
            Data.Transaction = trans;
            try
            {
                //PROCESSING
                tblBill tempbill = null;
                tblSettlement tempset = null;
                tblSettlementDet tempsetdet = null;
                decimal tempbalance = 0;
                foreach (tblBill bill in entBill)
                {
                    foreach (tblSettlement set in entSet)
                    {
                        tempbill = Data.tblBills.Where(b => b.vrid == bill.vrid && b.IV_Status == "OP").SingleOrDefault();
                        tempset = Data.tblSettlements.Where(s => s.TransNo == set.TransNo && s.Status == "OP").SingleOrDefault();
                        if (tempbill != null && tempset != null)
                        {
                            if (tempbill.IV_Total_Amt - tempbill.Settled_Amt > tempset.TransAmt - tempset.SettledAmt)
                            {
                                tempbalance = Convert.ToDecimal(tempset.TransAmt - tempset.SettledAmt);
                                tempbill.IV_Status = "OP";
                                tempbill.Settled_Amt = tempbill.Settled_Amt + tempbalance;
                                tempset.Status = "CL";
                                tempset.SettledAmt = tempset.SettledAmt + tempbalance;
                                
                                tempsetdet = new tblSettlementDet();
                                tempsetdet.TransNo = tempset.TransNo;
                                tempsetdet.BillRef = tempbill.vrid;
                                tempsetdet.SettleAmt = tempbalance;
                                tempsetdet.SettleDate = RMS.BL.Common.MyDate(Data);
                                Data.tblSettlementDets.InsertOnSubmit(tempsetdet);

                                Data.SubmitChanges();
                            }
                            else if (tempbill.IV_Total_Amt - tempbill.Settled_Amt == tempset.TransAmt - tempset.SettledAmt)
                            {
                                tempbalance = Convert.ToDecimal(tempset.TransAmt - tempset.SettledAmt);
                                tempbill.IV_Status = "CL";
                                tempbill.Settled_Amt = tempbill.Settled_Amt + tempbalance;
                                tempset.Status = "CL";
                                tempset.SettledAmt = tempset.SettledAmt + tempbalance;

                                tempsetdet = new tblSettlementDet();
                                tempsetdet.TransNo = tempset.TransNo;
                                tempsetdet.BillRef = tempbill.vrid;
                                tempsetdet.SettleAmt = tempbalance;
                                tempsetdet.SettleDate = RMS.BL.Common.MyDate(Data);
                                Data.tblSettlementDets.InsertOnSubmit(tempsetdet);

                                Data.SubmitChanges();
                            }
                            else if (tempbill.IV_Total_Amt - tempbill.Settled_Amt < tempset.TransAmt - tempset.SettledAmt)
                            {
                                tempbalance = Convert.ToDecimal(tempbill.IV_Total_Amt - tempbill.Settled_Amt);
                                tempbill.IV_Status = "CL";
                                tempbill.Settled_Amt = tempbill.Settled_Amt + tempbalance;
                                tempset.Status = "OP";
                                tempset.SettledAmt = tempset.SettledAmt + tempbalance;

                                tempsetdet = new tblSettlementDet();
                                tempsetdet.TransNo = tempset.TransNo;
                                tempsetdet.BillRef = tempbill.vrid;
                                tempsetdet.SettleAmt = tempbalance;
                                tempsetdet.SettleDate = RMS.BL.Common.MyDate(Data);
                                Data.tblSettlementDets.InsertOnSubmit(tempsetdet);

                                Data.SubmitChanges();
                            }
                        }
                    }
                }
                //END PROCESSING
                /*COMMIT*/
                trans.Commit();
                return "Done";
            }
            catch (Exception exx)
            {
                /*ROLL BACK*/
                if (trans != null)
                    trans.Rollback();
                return exx.Message;
            }
        }

        public object GetAllBillByVendorID(string vendorId, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                var recs = from a in Data.tblBills
                           where a.IV_Status == "OP" && a.IV_Type == "35" && a.PartyID == vendorId
                           select new
                           {
                               a.vrid,
                               a.IV_NO,
                               a.IV_Date,
                               a.IV_Due_Date,
                               invtotal = a.IV_Total_Amt,
                               settled_amnt = a.Settled_Amt,
                               balance = a.IV_Total_Amt - a.Settled_Amt
                           };
                return recs;
            }
            catch { }
            return null;
        }

        public object GetAllPaymentsByVendorID(string vendorId, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                var obj = from a in Data.tblSettlements
                          join b in Data.Glmf_Datas
                          on a.TransGlId equals b.vrid
                          join c in Data.Vr_Types
                          on b.vt_cd equals c.vt_cd
                          where a.Status == "OP" && a.PartyId == vendorId && (b.vt_cd == 4 || b.vt_cd == 5)
                          select new
                          {
                              a.TransNo,
                              a.TransGlId,
                              vr_no = c.vt_use + (b.source != null && c.vt_use != b.source ? "-" + b.source : "") + '-' + b.vr_no,
                              a.PartyId,
                              a.TransDate,
                              a.TransAmt,
                              settled_amnt = a.SettledAmt,
                              balance = a.TransAmt - a.SettledAmt
                          };
                return obj;
            }
            catch { }
            return null;
        }
    
    }
}
