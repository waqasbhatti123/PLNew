using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Linq;

namespace RMS.BL
{
    public class STNBL
    {
        public STNBL()
        { }

        public decimal GetQtyOnHandofItem(string itemCode, int brId, int locId, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                DateTime date = Common.MyDate(Data);
                //FIN_PERD fin = fin = (from a in Data.FIN_PERDs
                //                      where a.Start_Date <= date.Date && a.End_Date >= date.Date
                //                      select a).Single();

                tblStk stk = Data.tblStks.Single(p => p.itm_cd.Equals(itemCode) && p.br_id == brId && p.LocId == locId );

                decimal qty =  stk.itm_op_qty + stk.itm_pur_qty - stk.itm_isu_qty;
                return qty;
            }
            catch
            {
                return 0;
            }
        }

        public decimal GetLocBasedQtyOnHandofItem(string itemCode, int brId, int locId, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                DateTime date = Common.MyDate(Data);

                return Convert.ToDecimal(Data.GetLocItemDesc(brId, locId, itemCode).Single().stkqty);
            }
            catch
            {
                return 0;
            }
        }
        public Object GetAllSTN(string igp, int brId, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            var obj = from a in Data.tblStkDatas
                      join b in Data.tblStkDataDets 
                      on a.vr_id equals b.vr_id 
                      join c in Data.tblStock_Locs
                      on a.LocId equals c.LocId
                      join d in Data.tblStock_Locs
                      on a.ToLocId equals d.LocId
                      where a.br_id == brId && c.br_id == brId && d.br_id == brId && a.vt_cd == 19 && a.vr_apr == "A" && a.Status == "OP" && a.vr_no.ToString().StartsWith(igp)
                      select new 
                      {
                          a.vr_id,
                          a.vr_no,
                          a.vr_dt,
                          fromloc = c.LocName,
                          toloc = d.LocName

                      };
            return obj.Distinct();
        }
        public tblStkData GetStkData(int vrid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                tblStkData stk = Data.tblStkDatas.Single(p => p.vr_id == vrid);

                return stk;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public tblStkData GetByID(int vrid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                tblStkData stk = Data.tblStkDatas.Single(p => p.vr_id == vrid);

                return stk;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public string Save(tblStkData stkD, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                tblStk stk = null;
                FIN_PERD fin = null;
                DateTime date = Common.MyDate(Data);
                fin = (from a in Data.FIN_PERDs
                       where a.Start_Date <= date.Date && a.End_Date >= date.Date
                       select a).Single();

                Data.tblStkDatas.InsertOnSubmit(stkD);
                Data.SubmitChanges();

                ////tblStkDataDet valDet = null;
                //if (stkD.vr_apr.ToString().Equals("A"))
                //{
                //    foreach (tblStkDataDet det in stkD.tblStkDataDets)
                //    {
                //        stk = Data.tblStks.Single(p => p.itm_cd.Equals(det.itm_cd) && p.br_id == stkD.br_id && p.LocId == stkD.LocId );
                //        stk.itm_isu_qty = stk.itm_isu_qty + det.vr_qty;
                //        Data.SubmitChanges();

                //        stk = Data.tblStks.Single(p => p.itm_cd.Equals(det.itm_cd) && p.br_id == stkD.br_id && p.LocId == stkD.ToLocId);
                //        stk.itm_pur_qty = stk.itm_pur_qty + det.vr_qty;
                //        Data.SubmitChanges();
                //    }
                //}

                return "Done";
            }
            catch (Exception exx)
            {
                return exx.Message;
            }
        }

        public string SaveReceipt(int stkTrnsfrVrId, tblStkData stkD, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                tblStk stk = null;
                //FIN_PERD fin = null;
                //DateTime date = Common.MyDate(Data);
                //fin = (from a in Data.FIN_PERDs
                //       where a.Start_Date <= date.Date && a.End_Date >= date.Date
                //       select a).Single();

                Data.tblStkDatas.InsertOnSubmit(stkD);
                Data.SubmitChanges();

                tblStkData stkTrnsfrData = Data.tblStkDatas.Where(stktr => stktr.vr_id == stkTrnsfrVrId).Single();
                if (stkD.vr_apr.ToString().Equals("A") || stkD.vr_apr.ToString().Equals("P"))
                {
                    stkTrnsfrData.Status = "CL";
                }
                else
                {
                    stkTrnsfrData.Status = "OP";
                }
                Data.SubmitChanges();
                decimal voh = 0, issueVal = 0, qoh = 0;
                //tblStkDataDet valDet = null;
                if (stkD.vr_apr.ToString().Equals("A"))
                {
                    foreach (tblStkDataDet det in stkD.tblStkDataDets)
                    {
                        stk = Data.tblStks.Single(p => p.itm_cd.Equals(det.itm_cd) && p.br_id == stkD.br_id && p.LocId == stkD.LocId);
                        
                        voh = stk.itm_op_val + stk.itm_pur_val - stk.itm_isu_val;
                        qoh = stk.itm_op_qty + stk.itm_pur_qty - stk.itm_isu_qty;
                        issueVal = Math.Round((det.vr_qty - det.vr_qty_Rej) * (voh / qoh), 2);
                        stk.itm_isu_val = stk.itm_isu_val + issueVal; // value
                        stk.itm_isu_qty = stk.itm_isu_qty + det.vr_qty - det.vr_qty_Rej; // qty

                        det.vr_val = issueVal;

                        Data.SubmitChanges();
                        try
                        {
                            stk = Data.tblStks.Single(p => p.itm_cd.Equals(det.itm_cd) && p.br_id == stkD.br_id && p.LocId == stkD.ToLocId);
                            stk.itm_pur_qty = stk.itm_pur_qty + det.vr_qty - det.vr_qty_Rej;
                            stk.itm_pur_val = stk.itm_pur_val + issueVal; // value
                        }
                        catch
                        {
                            tblStk toLocStk = new tblStk();
                            toLocStk.br_id = stkD.br_id;
                            toLocStk.LocId = Convert.ToInt16(stkD.ToLocId);
                            toLocStk.itm_cd = det.itm_cd;
                            toLocStk.itm_pur_qty = det.vr_qty - det.vr_qty_Rej;
                            toLocStk.itm_pur_val = issueVal; // value
                            toLocStk.updateon = stkD.updateon;
                            toLocStk.updateby = stkD.updateby;
                            Data.tblStks.InsertOnSubmit(toLocStk);
                        }
                        tblStkDataDet stkdet = stkTrnsfrData.tblStkDataDets.Where(dd => dd.itm_cd == det.itm_cd).Single();
                        stkdet.vr_qty_Rej = det.vr_qty_Rej;
                        Data.SubmitChanges();
                    }
                }

                return "Done";
            }
            catch (Exception exx)
            {
                return exx.Message;
            }
        }

        public string UpdateReceipt(int stkTrnsfrVrId, tblStkData stkData, EntitySet<tblStkDataDet> newDets, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                tblStk stk = null;
                FIN_PERD fin = null;
                DateTime date = Common.MyDate(Data);
                fin = (from a in Data.FIN_PERDs
                       where a.Start_Date <= date.Date && a.End_Date >= date.Date
                       select a).Single();


                List<tblStkDataDet> existingDets = (from p in Data.tblStkDataDets
                                                    where p.vr_id == stkData.vr_id
                                                    select p).ToList();

                ////stock out
                //foreach (tblStkDataDet det in existingDets)
                //{
                //    stk = Data.tblStks.Single(p => p.itm_cd.Equals(det.itm_cd) && p.br_id == stkData.br_id && p.LocId == stkData.LocId );
                //    stk.itm_isu_qty = stk.itm_isu_qty - (det.vr_qty - det.vr_qty_Rej);
                //    Data.SubmitChanges();

                //    stk = Data.tblStks.Single(p => p.itm_cd.Equals(det.itm_cd) && p.br_id == stkData.br_id && p.LocId == stkData.ToLocId);
                //    stk.itm_pur_qty = stk.itm_pur_qty - (det.vr_qty - det.vr_qty_Rej);
                //    Data.SubmitChanges();
                //}
                //delete
                Data.tblStkDataDets.DeleteAllOnSubmit(existingDets);
                Data.SubmitChanges();

                //insert again
                stkData.tblStkDataDets = newDets;
                Data.SubmitChanges();

                tblStkData stkTrnsfrData = Data.tblStkDatas.Where(stktr => stktr.vr_id == stkTrnsfrVrId).Single();
                if (stkData.vr_apr.ToString().Equals("A") || stkData.vr_apr.ToString().Equals("P"))
                {
                    stkTrnsfrData.Status = "CL";
                }
                else
                {
                    stkTrnsfrData.Status = "OP";
                }
                Data.SubmitChanges();

                decimal voh = 0, issueVal = 0, qoh = 0;
                if (stkData.vr_apr.ToString().Equals("A"))
                {
                    foreach (tblStkDataDet det in stkData.tblStkDataDets)
                    {
                        stk = Data.tblStks.Single(p => p.itm_cd.Equals(det.itm_cd) && p.br_id == stkData.br_id && p.LocId == stkData.LocId);
                        
                        voh = stk.itm_op_val + stk.itm_pur_val - stk.itm_isu_val;
                        qoh = stk.itm_op_qty + stk.itm_pur_qty - stk.itm_isu_qty;
                        issueVal = Math.Round((det.vr_qty - det.vr_qty_Rej) * (voh / qoh), 2);
                        stk.itm_isu_val = stk.itm_isu_val + issueVal; // value
                        
                        stk.itm_isu_qty = stk.itm_isu_qty + det.vr_qty - det.vr_qty_Rej;

                        det.vr_val = issueVal;

                        Data.SubmitChanges();
                        try
                        {
                            stk = Data.tblStks.Single(p => p.itm_cd.Equals(det.itm_cd) && p.br_id == stkData.br_id && p.LocId == stkData.ToLocId);
                            stk.itm_pur_qty = stk.itm_pur_qty + det.vr_qty - det.vr_qty_Rej;

                            stk.itm_pur_val = stk.itm_pur_val + issueVal; // value
                        }
                        catch
                        {
                            tblStk toLocStk = new tblStk();
                            toLocStk.br_id = stkData.br_id;
                            toLocStk.LocId = Convert.ToInt16(stkData.ToLocId);
                            toLocStk.itm_cd = det.itm_cd;
                            toLocStk.itm_pur_qty = det.vr_qty - det.vr_qty_Rej;
                            toLocStk.itm_pur_val = issueVal; // value
                            toLocStk.updateon = stkData.updateon;
                            toLocStk.updateby = stkData.updateby;
                            Data.tblStks.InsertOnSubmit(toLocStk);
                        }

                        tblStkDataDet stkdet = stkTrnsfrData.tblStkDataDets.Where(dd => dd.itm_cd == det.itm_cd).Single();
                        stkdet.vr_qty_Rej = det.vr_qty_Rej;
                        Data.SubmitChanges();
                    }
                }
                return "Done";
            }
            catch (Exception exx)
            {
                return exx.Message;
            }
        }

        public string Update(tblStkData stkData, EntitySet<tblStkDataDet> newDets, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                tblStk stk = null;
                FIN_PERD fin = null;
                DateTime date = Common.MyDate(Data);
                fin = (from a in Data.FIN_PERDs
                       where a.Start_Date <= date.Date && a.End_Date >= date.Date
                       select a).Single();


                List<tblStkDataDet> existingDets = (from p in Data.tblStkDataDets
                                                    where p.vr_id == stkData.vr_id
                                                    select p).ToList();

                ////stock out
                //foreach (tblStkDataDet det in existingDets)
                //{
                //    stk = Data.tblStks.Single(p => p.itm_cd.Equals(det.itm_cd) && p.br_id == stkData.br_id && p.LocId == stkData.LocId );
                //    stk.itm_isu_qty = stk.itm_isu_qty - (det.vr_qty - det.vr_qty_Rej);
                //    Data.SubmitChanges();

                //    stk = Data.tblStks.Single(p => p.itm_cd.Equals(det.itm_cd) && p.br_id == stkData.br_id && p.LocId == stkData.ToLocId);
                //    stk.itm_pur_qty = stk.itm_pur_qty - (det.vr_qty - det.vr_qty_Rej);
                //    Data.SubmitChanges();
                //}
                //delete
                Data.tblStkDataDets.DeleteAllOnSubmit(existingDets);
                Data.SubmitChanges();

                //insert again
                stkData.tblStkDataDets = newDets;
                Data.SubmitChanges();

                //if (stkData.vr_apr.ToString().Equals("A"))
                //{
                //    foreach (tblStkDataDet det in newDets)
                //    {
                //        stk = Data.tblStks.Single(p => p.itm_cd.Equals(det.itm_cd) && p.br_id == stkData.br_id && p.LocId == stkData.LocId );
                //        stk.itm_isu_qty = stk.itm_isu_qty + (det.vr_qty - det.vr_qty_Rej);
                //        Data.SubmitChanges();

                //        stk = Data.tblStks.Single(p => p.itm_cd.Equals(det.itm_cd) && p.br_id == stkData.br_id && p.LocId == stkData.ToLocId );
                //        stk.itm_pur_qty = stk.itm_pur_qty + (det.vr_qty - det.vr_qty_Rej);
                //        Data.SubmitChanges();
                //    }
                //}
                return "Done";
            }
            catch (Exception exx)
            {
                return exx.Message;
            }
        }

        public object GetStoreTransferNote(string docno, char status, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {

                object obj;
                if (docno == "")
                {
                    obj = (from a in Data.tblStkDatas
                           join b in Data.tblStock_Locs
                           on a.LocId equals b.LocId
                           where a.vt_cd == 19 && a.vr_apr == (status == '0' ? a.vr_apr : status.ToString())
                           select new
                           {
                               a.vr_id,
                               a.LocId,
                               vr_no = a.vr_no.ToString().Substring(0, 4) + "/" + a.vr_no.ToString().Substring(4),
                               a.vr_dt,
                               a.vr_apr,
                               a.vr_nrtn,
                               b.LocName
                           }).ToList();
                }
                else
                {
                    obj = (from a in Data.tblStkDatas
                           join b in Data.tblStock_Locs
                           on a.LocId equals b.LocId
                           where a.vt_cd == 31 && a.vr_apr == (status == '0' ? a.vr_apr : status.ToString()) && a.vr_no.ToString() == docno
                           select new
                           {
                               a.vr_id,
                               a.LocId,
                               vr_no = a.vr_no.ToString().Substring(0, 4) + "/" + a.vr_no.ToString().Substring(4),
                               a.vr_dt,
                               a.vr_apr,
                               a.vr_nrtn,
                               b.LocName
                           }).ToList();
                }
                return obj;
            }
            catch (Exception exx)
            {
                return exx.Message;
            }
        }


        public object GetStoreTransferReceiptNote(string docno, char status, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {

                object obj;
                if (docno == "")
                {
                    obj = (from a in Data.tblStkDatas
                           join b in Data.tblStock_Locs
                           on a.LocId equals b.LocId
                           where a.vt_cd == 20 && a.vr_apr == (status == '0' ? a.vr_apr : status.ToString())
                           select new
                           {
                               a.vr_id,
                               a.LocId,
                               vr_no = a.vr_no.ToString().Substring(0, 4) + "/" + a.vr_no.ToString().Substring(4),
                               a.vr_dt,
                               a.vr_apr,
                               a.vr_nrtn,
                               b.LocName
                           }).ToList();
                }
                else
                {
                    obj = (from a in Data.tblStkDatas
                           join b in Data.tblStock_Locs
                           on a.LocId equals b.LocId
                           where a.vt_cd == 20 && a.vr_apr == (status == '0' ? a.vr_apr : status.ToString()) && a.vr_no.ToString() == docno
                           select new
                           {
                               a.vr_id,
                               a.LocId,
                               vr_no = a.vr_no.ToString().Substring(0, 4) + "/" + a.vr_no.ToString().Substring(4),
                               a.vr_dt,
                               a.vr_apr,
                               a.vr_nrtn,
                               b.LocName
                           }).ToList();
                }
                return obj;
            }
            catch (Exception exx)
            {
                return exx.Message;
            }
        }


        public object wmGetItemDetail(string itemid, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                
                //var heads = Data.GetItemDesc(itemid).ToList();

                var heads = (from a in Data.GetItemDesc(1, itemid)
                         select new
                         {
                             a.uom_dsc,
                             a.balance,
                             Batch = a.Batch == null ? false : a.Batch,
                             Exp_Days = a.Exp_Days == null ? 0 : a.Exp_Days,
                             a.stkqty
                         }).ToList();
        

                return heads;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public object wmGetSTNItemDetail(string itemid, int locid, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                //var heads = Data.GetItemDesc(itemid).ToList();

                var heads = (from a in Data.GetLocItemDesc(1, locid, itemid)
                             select new
                             {
                                 a.uom_dsc,
                                 a.balance,
                                 Batch = a.Batch == null ? false : a.Batch,
                                 Exp_Days = a.Exp_Days == null ? 0 : a.Exp_Days,
                                 a.stkqty
                             }).ToList();


                return heads;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
    }
}
