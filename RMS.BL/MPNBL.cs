using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Linq;

namespace RMS.BL
{
    public class MPNBL
    {
        public MPNBL()
        { }

        
        
        public List<spGetGpByPmnRefResult> GetGPRecords(string pmnRef, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                List<spGetGpByPmnRefResult> lst = Data.spGetGpByPmnRef(pmnRef).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }


       

        public tblStkGP GetIGPById(int igpNo, int igpCode, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                tblStkGP rec = (from a in Data.tblStkGPs
                                where a.vr_no == igpNo && a.vt_cd == igpCode
                                select a).Single();
                return rec;
            }
            catch //(Exception ex)
            {
                //RMSDB.closeConn(Data);
                //throw ex;
            }
            return null;
        }


        public string GetItemDesc(string itmCode, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                string itmDsc = (from a in Data.tblItem_Codes
                                 where a.itm_cd == itmCode && a.ct_id == "D"
                                 select a).Single().itm_dsc;
                return itmDsc;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }

        public decimal GetWHTPercentByPORef(int brid, int poRef, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                string whtId = "";
                decimal percent = 0;
                whtId = (from a in Data.tblPOrders
                         where a.vr_no == poRef && a.br_id == brid
                         select a).Single().WHTid;
                if (whtId != null)
                {
                    DateTime currDate = RMS.BL.Common.MyDate(Data);
                    DateTime maxDate = (from a in Data.tblTaxRates
                                        where a.TaxID == whtId && a.EffDate <= currDate && a.Status == "OP"
                                        select a).Max(d => d.EffDate);
                    percent = Data.tblTaxRates.Where(tx => tx.TaxID == whtId && tx.EffDate == maxDate).Single().TaxRate;
                }
                return percent;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }

        public List<tblStkData> GetRecordList(int mpnNo, int mpnCode, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                List<tblStkData> lst = (from a in Data.tblStkDatas
                                           where a.vr_no == mpnNo && a.vt_cd == mpnCode && !a.vr_apr.Equals('C')
                                           select a).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }



        public List<tblStkDataDet> GetEditTableList(int mpnNo, int mpnCode, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                List<tblStkDataDet> lst = (from a in Data.tblStkDatas
                                           join b in Data.tblStkDataDets
                                           on a.vr_id equals b.vr_id
                                           where a.vr_no == mpnNo && a.vt_cd == mpnCode && !a.vr_apr.Equals('C')
                                           orderby b.vr_seq
                                           select b).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }


        public List<spChemSrch4MPNResult> GetMPNGrid(DateTime fromDt, DateTime toDt, string party, char status, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                List<spChemSrch4MPNResult> lst = Data.spChemSrch4MPN(fromDt, toDt, party, Convert.ToString(status)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }




        public bool SaveMPN(tblStkData stkData, EntitySet<tblStkDataDet> entStkDet, Glmf_Data glmf, string vendorid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                Data.tblStkDatas.InsertOnSubmit(stkData);
                stkData.tblStkDataDets = entStkDet;

                Data.SubmitChanges();

                if (stkData.vr_apr == "A")
                {
                    EntitySet<tblStkDataDet> entStkMatRec = new EntitySet<tblStkDataDet>();
                    foreach (var det in entStkDet)
                    {
                        tblStkDataDet matdet = Data.tblStkDataDets.Where(stk => stk.vr_id == det.matrec_id && stk.itm_cd == det.itm_cd).Single();
                        matdet.vr_val = det.vr_val;
                        matdet.Mat_val = det.Mat_val;
                        matdet.Otr_val = det.Otr_val;
                        matdet.GSTamt = det.GSTamt; 
                        matdet.TaxID = det.TaxID;
                        matdet.cust_duty = det.cust_duty;
                        matdet.freight = det.freight;
                        matdet.WHT_Amnt = det.WHT_Amnt;
                        matdet.NetGST = det.NetGST;
                        matdet.overall_disc = det.overall_disc;
                        entStkMatRec.Add(matdet);
                    }
                    glmf.vr_nrtn = "MPN No: " + stkData.vr_no + glmf.vr_nrtn;
                    Data.Glmf_Datas.InsertOnSubmit(glmf);


                    /*FILL BILL DETAILS*/
                    int billid = 0;
                    try
                    {
                        billid = Data.tblBills.Max(y => y.vrid) + 1;
                    }
                    catch { billid = 1; }
                    tblBill bill = new tblBill();
                    bill.vrid = billid;
                    bill.brid = stkData.br_id;
                    bill.IV_Type = stkData.vt_cd.ToString();
                    bill.IV_NO = stkData.vr_no;
                    bill.PartyID = vendorid;
                    bill.IV_Ref = stkData.vr_id.ToString();
                    bill.IV_Date = stkData.vr_dt;
                    bill.IV_Due_Date = stkData.Due_Date;
                    bill.IV_Total_Amt = entStkDet.Sum(x => x.vr_val + x.GSTamt + x.cust_duty + x.freight - x.overall_disc) + stkData.ImpFreight + stkData.Tax;//stkData.Freight + 
                    bill.IV_Net_Discount = entStkDet.Sum(x=> x.overall_disc);
                    bill.IV_WHT = stkData.Tax;
                    bill.Settled_Amt = 0;
                    bill.PrtSeq = 0;
                    bill.Rmk = null;
                    bill.IV_Status = "OP";
                    bill.WHTid = null;
                    bill.IV_Total_Amt_Diff = 0;
                    //EntitySet<tblBillDet> entBillDet = new EntitySet<tblBillDet>();
                    //tblBillDet billdet;
                    //int seq = 0;
                    //foreach (tblStkDataDet det in entStkDet)
                    //{
                    //    seq++;
                    //    billdet = new tblBillDet();
                    //    billdet.vrid = bill.vrid;
                    //    billdet.vrseq = seq;
                    //    //billdet.TranType = null;
                    //    //billdet.TranRef = null;
                    //    //billdet.stk_id = null;
                    //    billdet.Qty = det.vr_qty;
                    //    billdet.Tax = det.GSTamt;
                    //    billdet.Discount = det.overall_disc;
                    //    billdet.IV_Amount = det.vr_val + det.GSTamt + det.WHT_Amnt + det.cust_duty + det.freight - det.overall_disc;
                    //    billdet.IV_Status = "OP";
                    //    billdet.GSTid = det.TaxID;
                    //    entBillDet.Add(billdet);
                    //}
                    Data.tblBills.InsertOnSubmit(bill);
                    //Data.tblBillDets.InsertAllOnSubmit(entBillDet);
                    /*END FILL BILL DETAILS*/

                    Data.SubmitChanges();
                }
                return true;
            }
            catch //(Exception ex)
            {
                //RMSDB.closeConn(Data);
                //throw ex;
            }
            return false;
        }




        public bool EditMPN(int brid, int mpnNo, int mpnCode, tblStkData stkData, EntitySet<tblStkDataDet> entStkDet, Glmf_Data glmf, string vendorid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                var recsDet =  from a in Data.tblStkDataDets
                               join b in Data.tblStkDatas
                               on a.vr_id equals b.vr_id
                               where b.vr_no == mpnNo && b.vt_cd == mpnCode && b.br_id == brid
                               select a;
                Data.tblStkDataDets.DeleteAllOnSubmit(recsDet);
                Data.SubmitChanges();
                tblStkData rec = (from a in Data.tblStkDatas
                          where a.vr_no == mpnNo && a.vt_cd == mpnCode && a.br_id == brid
                          select a).Single();
                Data.tblStkDatas.DeleteOnSubmit(rec);
                Data.SubmitChanges();

                Data.tblStkDatas.InsertOnSubmit(stkData);
                stkData.tblStkDataDets = entStkDet;

                Data.SubmitChanges();

                if (stkData.vr_apr == "A")
                {
                    EntitySet<tblStkDataDet> entStkMatRec = new EntitySet<tblStkDataDet>();
                    foreach (var det in entStkDet)
                    {
                        tblStkDataDet matdet = Data.tblStkDataDets.Where(stk => stk.vr_id == det.matrec_id && stk.itm_cd == det.itm_cd).Single();
                        matdet.vr_val = det.vr_val;
                        matdet.Mat_val = det.Mat_val;
                        matdet.Otr_val = det.Otr_val;
                        matdet.GSTamt = det.GSTamt;
                        matdet.TaxID = det.TaxID;
                        matdet.cust_duty = det.cust_duty;
                        matdet.freight = det.freight;
                        matdet.WHT_Amnt = det.WHT_Amnt;
                        matdet.NetGST = det.NetGST;
                        matdet.overall_disc = det.overall_disc;
                        entStkMatRec.Add(matdet);
                    }

                    glmf.vr_nrtn = "MPN No: " + stkData.vr_no + glmf.vr_nrtn;
                    Data.Glmf_Datas.InsertOnSubmit(glmf);

                    /*FILL BILL DETAILS*/
                    int billid = 0;
                    try
                    {
                        billid = Data.tblBills.Max(y => y.vrid) + 1;
                    }
                    catch { billid = 1; }
                    tblBill bill = new tblBill();
                    bill.vrid = billid;
                    bill.brid = stkData.br_id;
                    bill.IV_Type = stkData.vt_cd.ToString();
                    bill.IV_NO = stkData.vr_no;
                    bill.PartyID = vendorid;
                    bill.IV_Ref = stkData.vr_id.ToString();
                    bill.IV_Date = stkData.vr_dt;
                    bill.IV_Due_Date = stkData.Due_Date;
                    bill.IV_Total_Amt = entStkDet.Sum(x => x.vr_val + x.GSTamt + x.cust_duty + x.freight - x.overall_disc) + stkData.ImpFreight + stkData.Tax;//stkData.Freight +
                    bill.IV_Net_Discount = entStkDet.Sum(x => x.overall_disc);
                    bill.IV_WHT = stkData.Tax;
                    bill.Settled_Amt = 0;
                    bill.PrtSeq = 0;
                    bill.Rmk = null;
                    bill.IV_Status = "OP";
                    bill.WHTid = null;
                    bill.IV_Total_Amt_Diff = 0;
                    //EntitySet<tblBillDet> entBillDet = new EntitySet<tblBillDet>();
                    //tblBillDet billdet;
                    //int seq = 0;
                    //foreach (tblStkDataDet det in entStkDet)
                    //{
                    //    seq++;
                    //    billdet = new tblBillDet();
                    //    billdet.vrid = bill.vrid;
                    //    billdet.vrseq = seq;
                    //    //billdet.TranType = null;
                    //    //billdet.TranRef = null;
                    //    //billdet.stk_id = null;
                    //    billdet.Qty = det.vr_qty;
                    //    billdet.Tax = det.GSTamt;
                    //    billdet.Discount = det.overall_disc;
                    //    billdet.IV_Amount = det.vr_val + det.GSTamt + det.WHT_Amnt + det.cust_duty + det.freight - det.overall_disc;
                    //    billdet.IV_Status = "OP";
                    //    billdet.GSTid = det.TaxID;
                    //    entBillDet.Add(billdet);
                    //}
                    Data.tblBills.InsertOnSubmit(bill);
                    //Data.tblBillDets.InsertAllOnSubmit(entBillDet);
                    /*END FILL BILL DETAILS*/

                    Data.SubmitChanges();
                }

                

                return true;
            }
            catch //(Exception ex)
            {
                //RMSDB.closeConn(Data);
                //throw ex;
            }
            return false;
        }

        public string CancelMPN(int brid, int mpnCode, int mpnNo, string potype, RMSDataContext Data)
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
                //Cancel MPN
                tblStkData stkdata = (from a in Data.tblStkDatas
                                  where a.vr_no == mpnNo && a.vt_cd == mpnCode && a.br_id == brid
                                  select a).Single();
                stkdata.vr_apr = "C";
                //Update PMN_Ref, PMN_RefTemp fields to null
                List<tblStkGP> gplist = (from a in Data.tblStkGPs
                               where a.br_id == brid && a.PMN_Ref == stkdata.DocRef
                               select a).ToList();
                foreach (var l in gplist)
                {
                    l.PMN_Ref = "";
                    l.PMN_RefTemp = "";
                }
                //Cancel IPV in GL
                Glmf_Data gldata = (from a in Data.Glmf_Datas
                                    where a.br_id == brid && a.vt_cd == 6 && a.Ref_no == mpnNo.ToString()
                                    select a).Single();
                gldata.vr_apr = "D";
                //Subtracting stock
                List<tblStkDataDet> stkdet = (from a in Data.tblStkDataDets
                                              where a.vr_id == stkdata.vr_id
                                              select a).ToList(); 
                List<tblStk> stklist = new List<tblStk>();
                tblStk rec = null;
                foreach (var det in stkdet)
                {
                    rec = (from a in Data.tblStks
                                  where a.itm_cd == det.itm_cd && a.LocId == stkdata.LocId && a.br_id == brid
                                  select a).Single();
                    rec.itm_pur_qty = rec.itm_pur_qty - det.vr_qty;
                    rec.itm_pur_val = rec.itm_pur_val - (Convert.ToDecimal(det.vr_val + det.cust_duty)
                                        + (potype == "F" ? Convert.ToDecimal(det.freight) : 0)
                                        + (potype == "L" ? Convert.ToDecimal(det.WHT_Amnt) : 0));
                    stklist.Add(rec);
                }
                Data.SubmitChanges();
                /*COMMIT*/
                trans.Commit();

                return "Done";
            }
            catch (Exception ex)
            {
                /*ROLL BACK*/
                if (trans != null)
                    trans.Rollback();

                return ex.Message;
            }
        }

        public List<spChemIgps4MPNResult> GetIGPs(int lcId, int vrCode, string docRef, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                List<spChemIgps4MPNResult> lst = Data.spChemIgps4MPN(lcId, vrCode, docRef).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }



        public void DiscardFalseRecords(int vrCode, int lcId, char status, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                var recsStk = (from r in Data.tblStkGPs
                               join d in Data.tblStock_Locs
                               on r.LocId equals d.LocId
                               where r.PMN_Ref != r.PMN_RefTemp && r.vt_cd == vrCode && r.vr_apr != Convert.ToString(status)
                               && !d.LocCategory.Equals("R")//&& r.LocId == lcId
                               select r).ToList();
                foreach (var r in recsStk)
                {
                    r.PMN_Ref = "";
                    r.PMN_RefTemp = "";
                }
                Data.SubmitChanges();

            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }



        public bool UpdateRecord(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                Data.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }



        public tblStkGP GetRecord(int brid, int igpNo, int vrCode, int lcId, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                tblStkGP rec = (from a in Data.tblStkGPs
                                where a.br_id == brid && a.vr_no == igpNo && a.vt_cd == vrCode && a.LocId == lcId
                                select a).Single();
                return rec;
            }
            catch //(Exception ex)
            {
                //RMSDB.closeConn(Data);
                //throw ex;
            }
            return null;
        }

        



        public string GetDocRefByIGP(int brid, int igpNo, int vrCode, int lcId, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                string docRef = (from a in Data.tblStkGPs
                                 where a.vr_no == igpNo && a.vt_cd == vrCode && a.br_id == brid && a.LocId == lcId
                                 select a).Single().DocRef;
                return docRef;
            }
            catch //(Exception ex)
            {
                //RMSDB.closeConn(Data);
                //throw ex;
            }
            return "";
        }



        public object GetIGPsOfVendor(string vndrId, int locId, int vrCode, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                return Data.spVendorsWiseIGPs(vndrId, locId, vrCode).ToList();

                //var igpsList = (from a in Data.tblStkGPs
                //                join b in Data.Glmf_Codes
                //                on a.VendorId equals b.gl_cd
                //                join c in Data.tblStkDatas
                //                on a.vr_no equals c.IGPNo
                //                join d in Data.tblStock_Locs
                //                on c.LocId equals d.LocId
                //                where a.vt_cd == vrCode && a.vr_apr == 'A' && b.ct_id == 'D' && a.VendorId == vndrId && a.LocId == locId
                //                 && (a.PMN_Ref == null || a.PMN_Ref == "") && c.vt_cd == 16 && c.vr_apr == 'A'&& c.LocId == locId
                //                 && !d.LocCategory.Equals("R")
                //                orderby a.vr_no.ToString().Substring(0, 4), a.vr_no.ToString().Substring(4)
                //                select new
                //                {
                //                    Sr = "0",
                //                    IGPNo = a.vr_no,
                //                    IGPDate = a.vr_dt,
                //                    Party = b.gl_dsc,
                //                    a.LocId,
                //                    c.tblStkDataDets.First().PO_Ref,
                //                    PO_Ref_Formatted = c.tblStkDataDets.First().PO_Ref.Value.ToString().Substring(0, 4) + "/" + c.tblStkDataDets.First().PO_Ref.Value.ToString().Substring(4)
                //                }).ToList();
                //if (igpsList.Count > 0)
                //{
                //    return igpsList;
                //}
                //else
                //{
                //    return null;
                //}
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }



        public string GetMPNNo(DateTime dtTime, int code, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                List<int> lst = new List<int>();
                FIN_PERD fin = (from a in Data.FIN_PERDs
                                where a.Start_Date <= dtTime && a.End_Date >= dtTime
                                select a).Single();
                string finYear = fin.Gl_Year.ToString();



                var records = from n in Data.tblStkDatas
                              join d in Data.tblStock_Locs
                              on n.LocId equals d.LocId
                              where !d.LocCategory.Equals("R")//n.LocId == 5
                              select n;
                foreach (var rec in records)
                {
                    string year = rec.vr_no.ToString().Substring(0, 4);
                    if (year == finYear && rec.vt_cd == code)
                    {
                        lst.Add(Convert.ToInt32(rec.vr_no.ToString().Substring(4)));
                    }
                }
                if (lst.Count > 0)
                {
                    return finYear + "/" + Convert.ToInt32(lst.Max() + 1);
                }
                else
                {
                    return finYear + "/1";
                }
            }
            catch //(Exception ex)
            {
                //RMSDB.closeConn(Data); throw ex;
            }
            return "";
        }



        public object GetSelectedPurchaseVendor(int vrCode, int locId, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                return Data.spVendorsForMPN(vrCode).ToList();
                //var vendorList = (from a in Data.tblStkGPs
                //                  join b in Data.Glmf_Codes
                //                  on a.VendorId equals b.gl_cd
                //                  join c in Data.tblStkDatas
                //                  on a.vr_no equals c.IGPNo
                //                  join d in Data.tblStock_Locs
                //                  on c.LocId equals d.LocId
                //                  where a.vt_cd == vrCode && a.vr_apr == 'A' && b.ct_id == 'D' //&& a.LocId == locId
                //                   && (a.PMN_Ref == null || a.PMN_Ref == "") && c.vt_cd == 16 && c.vr_apr == 'A'// && c.LocId == 5
                //                   && !d.LocCategory.Equals("R")
                //                  orderby b.gl_dsc
                //                  select new
                //                  {
                //                      VendorId = a.VendorId +"-"+ a.LocId,
                //                      gl_dsc = b.gl_dsc +" - "+ d.LocName
                //                  }).Distinct().ToList();
                //return vendorList;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }
        public List<Item_Uom> GetUOM(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                List<Item_Uom> lst = (from a in Data.Item_Uoms
                                      select a).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }


        //public List<Glmf_Code> GetShowVendor(RMSDataContext Data)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = RMSDB.GetOject(); }
        //        var lst = from l in Data.Glmf_Codes
        //                  join r in Data.Preferences
        //                  on l.cnt_gl_cd equals r.ctrl_Vndr
        //                  where l.ct_id == 'D'
        //                  orderby l.gl_dsc
        //                  select l;
        //        return lst.ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.closeConn(Data);
        //        throw ex;
        //    }
        //}



        public List<tblCity> GetCity(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var lst = from l in Data.tblCities
                          orderby l.CityName
                          select l;
                return lst.ToList();
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }

        public List<tblStock_Loc> GetStockLoc(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var lst = from l in Data.tblStock_Locs
                          where !l.LocCategory.Equals("R")
                          orderby l.LocName
                          select l;
                return lst.ToList();
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }


        //Posting to tblStk=================================================================



        public bool PostToTblStkInsert(tblStk stk, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                Data.tblStks.InsertOnSubmit(stk);
                Data.SubmitChanges();
                return true;
            }
            catch //(Exception ex)
            {
                //RMSDB.closeConn(Data);
                //throw ex;
            }
            return false;
        }


        public bool PostToTblStkUpdate(tblStk stk, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                Data.SubmitChanges();
                return true;
            }
            catch //(Exception ex)
            {
                //RMSDB.closeConn(Data);
                //throw ex;
            }
            return false;
        }

         public tblStk GetRecByItemCode(int brid, string itemCd, decimal glYear, int locId, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                tblStk rec = (from a in Data.tblStks
                              where a.itm_cd == itemCd && a.LocId == locId && a.br_id == brid
                              select a).Single();
                return rec;
            }
            catch //(Exception ex)
            {
                //RMSDB.closeConn(Data);
                //throw ex;
            }
            return null;
        }


        public string GetGlYear(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                DateTime date = Common.MyDate(Data);
                FIN_PERD fin = (from a in Data.FIN_PERDs
                                where a.Start_Date <= date.Date && a.End_Date >= date.Date
                                select a).Single();
                string finYear = fin.Gl_Year.ToString();
                return finYear;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }

        //Purchase Cost Sheet===============================================================


        public bool SaveCostSheet(int vrId, tblStkCost stkCost, EntitySet<tblStkCostDet> enCostDet, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                List<tblStkCostDet> lst = (from a in Data.tblStkCostDets
                                           where a.vr_id == vrId
                                           select a).ToList();
                if (lst.Count > 0)
                {
                    tblStkCost rec = (from a in Data.tblStkCosts
                                      where a.vr_id == vrId
                                      select a).Single();
                    Data.tblStkCostDets.DeleteAllOnSubmit(lst);
                    Data.SubmitChanges();
                    Data.tblStkCosts.DeleteOnSubmit(rec);
                    Data.SubmitChanges();
                }

                Data.tblStkCosts.InsertOnSubmit(stkCost);
                stkCost.tblStkCostDets = enCostDet;
                Data.SubmitChanges();

                return true;
            }
            catch //(Exception ex)
            {
                //RMSDB.closeConn(Data);
                //throw ex;
            }
            return false;
        }



        public bool UpdateCostSheet(int vrId, int prvVrId, char status, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                int count=1;
                EntitySet<tblStkCostDet> entCost = new EntitySet<tblStkCostDet>();
                List<tblStkCostDet> lst = (from a in Data.tblStkCostDets
                                           where a.vr_id == prvVrId
                                           select a).ToList();
                foreach (tblStkCostDet r in lst)
                {
                    tblStkCostDet CostRec = new tblStkCostDet();

                    CostRec.vr_id = vrId;
                    CostRec.vr_seq = Convert.ToByte(count++);
                    CostRec.CostId = r.CostId;
                    CostRec.DocRef = r.DocRef;
                    CostRec.DocRef_Date = r.DocRef_Date;
                    CostRec.Claim_Amt = r.Claim_Amt;
                    CostRec.Paid_Amt = r.Paid_Amt;
                    CostRec.Pay_Date = r.Pay_Date;
                    CostRec.vr_rmk = r.vr_rmk;

                    entCost.Add(CostRec);
                }

                if (lst.Count > 0)
                {
                    tblStkCost rec = (from a in Data.tblStkCosts
                                      where a.vr_id == prvVrId
                                      select a).Single();
                    tblStkCost Cost = new tblStkCost();
                    Cost.vr_id = vrId;
                    Cost.vr_dt = rec.vr_dt;
                    Cost.DocRef = rec.DocRef;
                    Cost.vr_nrtn = rec.vr_nrtn;
                    Cost.updateon = rec.updateon;
                    Cost.updateby = rec.updateby;
                    Cost.post_2gl = rec.post_2gl;
                    Cost.vr_apr = Convert.ToString(status);
                    
                    
                    //Deleting
                    Data.tblStkCostDets.DeleteAllOnSubmit(lst);
                    Data.SubmitChanges();
                    Data.tblStkCosts.DeleteOnSubmit(rec);
                    Data.SubmitChanges();
                    //Inserting
                    Data.tblStkCosts.InsertOnSubmit(Cost);
                    Cost.tblStkCostDets = entCost;

                    Data.SubmitChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }

        public bool UpdateStatus(int vrId, RMSDataContext Data)
        {
            try
            {
                tblStkCost rec = (from a in Data.tblStkCosts
                                  where a.vr_id == vrId
                                  select a).Single();
                rec.vr_apr ="C";
                Data.SubmitChanges();
                return true;
            }
            catch //(Exception ex)
            {
                //RMSDB.closeConn(Data);
                //throw ex;
            }
            return false;
        }
        
        public List<Glmf_Code> GetVendor(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var lst = from l in Data.Glmf_Codes
                          join r in Data.Preferences
                          on l.cnt_gl_cd equals r.ctrl_Vndr
                          where l.ct_id == "D"
                          orderby l.gl_dsc
                          select l;
                return lst.ToList();
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }

        public List<tblStkCostDet> GetPreviousRec(int mpnNo, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                int vrId = (from a in Data.tblStkDatas
                            where a.vr_no == mpnNo && a.vt_cd == 21 && a.vr_apr != "C"
                            select a).Single().vr_id;

                List<tblStkCostDet> lst = (from a in Data.tblStkCostDets
                                           where a.vr_id == vrId
                                           select a).ToList();
                return lst;
            }
            catch //(Exception ex)
            {
                //RMSDB.closeConn(Data);
                //throw ex;
            }
            return null;
        }

        public string GetPOType(int brid, int poref, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                int maxSeq = Convert.ToInt32(Data.tblPOrders.Where(po => po.br_id == brid && po.vr_no == poref).Max(x => x.RevSeq));
                return Data.tblPOrders.Where(po => po.br_id == brid && po.vr_no == poref && po.RevSeq == maxSeq).Single().PO_Type;
            }
            catch //(Exception ex)
            {
                //RMSDB.closeConn(Data);
                //throw ex;
            }
            return null;
        }

        public decimal GetPOItemDetail(int brid, int poref, string itmcd, RMSDataContext Data)
        {
            try
            {
                int vrid = 0;
                if (Data == null) { Data = RMSDB.GetOject(); }
                int maxSeq = Convert.ToInt32(Data.tblPOrders.Where(po => po.br_id == brid && po.vr_no == poref).Max(x => x.RevSeq));
                vrid = Data.tblPOrders.Where(po => po.br_id == brid && po.vr_no == poref && po.RevSeq == maxSeq).Single().vr_id;

                tblPOrderDet det = Data.tblPOrderDets.Where(pod => pod.vr_id == vrid && pod.itm_cd == itmcd).SingleOrDefault();
                return det.vr_Rate;
            }
            catch //(Exception ex)
            {
                //RMSDB.closeConn(Data);
                //throw ex;
            }
            return 0;
        }

        public List<tblCost_TempDet> GetCostIdDet(string potyp, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                List<tblCost_TempDet> lst = null;
                if (!string.IsNullOrEmpty(potyp))
                {
                    lst = (from a in Data.tblCost_Temps
                                                 join b in Data.tblCost_TempDets
                                                 on a.TempId equals b.TempId
                                                 where a.TempId.ToLower() == (potyp == "L" ? "local" : "import")
                                                 orderby b.TempId, b.CostId
                                                 select b).ToList();
                }
                else
                {
                    lst = (from a in Data.tblCost_Temps
                           join b in Data.tblCost_TempDets
                           on a.TempId equals b.TempId
                           where a.TempId.ToLower() == "local" 
                           orderby b.TempId, b.CostId
                           select b).ToList();
                }
                return lst;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }

        public int GetVrIDByIGP(int igpNo, int vrNo, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                
                tblStkData rec = (from a in Data.tblStkDatas
                                  join d in Data.tblStock_Locs
                                  on a.LocId equals d.LocId
                                  where a.IGPNo == igpNo && a.vt_cd == Convert.ToInt16(21) && a.vr_no == vrNo && a.vr_apr != "C"
                                  && a.br_id.Equals(d.br_id) && !d.LocCategory.Equals("R")//&& a.LocId == 5
                                  select a).Single();
                return rec.vr_id;
                }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
            //return 0;
        }


        public decimal GetChargedCost(int vrId, string IsCharged, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                decimal cost =Convert.ToDecimal((from a in Data.tblStkCostDets
                                                join b in Data.tblCost_TempDets
                                                on a.CostId equals b.CostId
                                                where a.vr_id == vrId && b.Charg_Cost == IsCharged
                                                select a.Paid_Amt).Sum());
                return cost;
            }
            catch //(Exception ex)
            {
                //RMSDB.closeConn(Data);
                //throw ex;
            }
            return 0;
        }


        public string GetChargeCostStts(decimal cost_id, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                tblCost_TempDet rec = (from a in Data.tblCost_TempDets
                                  where a.CostId == cost_id
                                  select a).Single();
                return rec.Charg_Cost;
            }
            catch //(Exception ex)
            {
                //RMSDB.closeConn(Data);
                //throw ex;
            }
            return "-";
        }


    }
}
