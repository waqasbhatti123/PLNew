using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.Linq;

namespace RMS.BL
{
   public class PTNBL
    {
       public PTNBL()
       {

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

       public bool IsBatchAlreadyExists(bool editmode, string batch, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               var bch = Data.tblBatches.Where(b => b.batch_id == batch && !editmode).ToList();
               if (bch != null && bch.Count > 0)
                   return true;
               else
                   return false;
           }
           catch (Exception ex)
           {
               RMSDB.SetNull();
               throw ex;
           }
       }

       public object GetProductionNote(string docno, char status, RMSDataContext Data)
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
                         where a.vt_cd == 31 && a.vr_apr == (status == '0' ? a.vr_apr : status.ToString())
                         select new
                         {
                             a.vr_id,
                             a.LocId,
                             vr_no = a.vr_no.ToString().Substring(0, 4) + "/" + a.vr_no.ToString().Substring(4),
                             a.vr_dt,
                             a.vr_apr,
                             a.vr_nrtn,
                             b.LocName
                         }).OrderBy(o=> o.vr_no).ToList();
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
                         }).ToList().OrderBy(o => o.vr_no).ToList();
               }
               return obj;
           }
           catch (Exception exx)
           {
               return exx.Message;
           }
       }

       public string Save(tblStkData stkD, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }

           System.Data.Common.DbTransaction trans = null;
           if (Data.Connection.State != ConnectionState.Open)
           {
               Data.Connection.Open();
           }
           trans = Data.Connection.BeginTransaction();
           Data.Transaction = trans;

           try
           {
               tblStk stk = null;
               DateTime date = Common.MyDate(Data);
               Data.tblStkDatas.InsertOnSubmit(stkD);
               Data.SubmitChanges();

               //tblStkDataDet valDet = null;
               if (stkD.vr_apr.ToString().Equals("A"))
               {
                   foreach (tblStkDataDet det in stkD.tblStkDataDets)
                   {
                       try
                       {
                           stk = Data.tblStks.Single(p => p.br_id.Equals(stkD.br_id) && p.LocId.Equals(stkD.LocId) && p.itm_cd.Equals(det.itm_cd));
                           //stk.itm_pur_qty = stk.itm_pur_qty + (det.vr_qty - det.vr_qty_Rej);
                           det.stk_id = stk.stk_id;
                           Data.SubmitChanges();
                       }
                       catch
                       {
                           tblStk nstk = new tblStk();
                           nstk.br_id = stkD.br_id;
                           nstk.LocId = stkD.LocId;
                           nstk.itm_cd = det.itm_cd;
                           nstk.itm_op_qty = 0;
                           nstk.itm_op_val = 0;
                           nstk.itm_isu_qty = 0;
                           nstk.itm_pur_val = 0;
                           //nstk.itm_pur_qty = 0;
                           nstk.itm_isu_val = 0;

                           Data.tblStks.InsertOnSubmit(nstk);
                           Data.SubmitChanges();

                           det.stk_id = nstk.stk_id;
                           Data.SubmitChanges();
                       }

                       if (!string.IsNullOrEmpty(det.batch_id) && det.stk_id != null)
                       {
                           tblItem_Code itmCode = Data.tblItem_Codes.Where(itm => itm.itm_cd.Equals(det.itm_cd) && itm.br_id.Equals(stkD.br_id)).Single();

                           try
                           {
                               tblBatch batch = Data.tblBatches.Where(b => b.stk_id == det.stk_id && b.batch_id == det.batch_id).Single();


                               //batch.itm_pur_qty = 0;
                               //batch.updateby = stkD.updateby;
                               //batch.updateon = stkD.updateon;
                               //batch.Prod_Date = stkD.vr_dt;
                               //batch.Salvage_Qty = 0;
                               //if (itmCode.Exp_Days != null)
                               //    batch.Exp_Date = stkD.vr_dt.AddDays(Convert.ToDouble(itmCode.Exp_Days));
                               //else
                               //    batch.Exp_Date = null;
                               //batch.Status = true;
                               //Data.SubmitChanges();
                           }
                           catch
                           {
                               tblBatch batch = new tblBatch();

                               batch.stk_id = det.stk_id.Value;
                               batch.batch_id = det.batch_id;
                               batch.itm_pur_qty = 0;
                               batch.updateby = stkD.updateby;
                               batch.updateon = stkD.updateon;
                               batch.Prod_Date = stkD.vr_dt;
                               batch.Salvage_Qty = 0;
                               if (itmCode.Exp_Days != null)
                                   batch.Exp_Date = stkD.vr_dt.AddDays(Convert.ToDouble(itmCode.Exp_Days));
                               else
                                   batch.Exp_Date = null;
                               batch.Status = true;
                               Data.tblBatches.InsertOnSubmit(batch);
                               Data.SubmitChanges();
                           }
                       }
                   }
               }
               trans.Commit();
               return "Done";
           }
           catch (Exception exx)
           {
               if (trans != null)
                   trans.Rollback();
               return exx.Message;
           }
       }

       public string Update(tblStkData stkData, EntitySet<tblStkDataDet> newDets, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }

           System.Data.Common.DbTransaction trans = null;
           if (Data.Connection.State != ConnectionState.Open)
           {
               Data.Connection.Open();
           }
           trans = Data.Connection.BeginTransaction();
           Data.Transaction = trans;

           try
           {
               tblStk stk = null;
               //FIN_PERD fin = null;
               DateTime date = Common.MyDate(Data);
               //fin = (from a in Data.FIN_PERDs
               //       where a.Start_Date <= date.Date && a.End_Date >= date.Date
               //       select a).Single();


               List<tblStkDataDet> existingDets = (from p in Data.tblStkDataDets
                                                   where p.vr_id == stkData.vr_id
                                                   select p).ToList();

               //delete
               Data.tblStkDataDets.DeleteAllOnSubmit(existingDets);
               Data.SubmitChanges();

               //insert again
               stkData.tblStkDataDets = newDets;
               Data.SubmitChanges();

               if (stkData.vr_apr.ToString().Equals("A"))
               {
                   foreach (tblStkDataDet det in newDets)
                   {
                       try
                       {
                           stk = Data.tblStks.Single(p => p.br_id.Equals(stkData.br_id) && p.LocId.Equals(stkData.LocId) && p.itm_cd.Equals(det.itm_cd));
                           //stk.itm_pur_qty = stk.itm_pur_qty + (det.vr_qty - det.vr_qty_Rej);
                           det.stk_id = stk.stk_id;
                           Data.SubmitChanges();

                       }
                       catch
                       {
                           tblStk nstk = new tblStk();
                           nstk.br_id = stkData.br_id;
                           nstk.LocId = stkData.LocId;
                           nstk.itm_cd = det.itm_cd;
                           nstk.itm_op_qty = 0;
                           nstk.itm_op_val = 0;
                           nstk.itm_isu_qty = 0;
                           nstk.itm_pur_val = 0;
                           nstk.itm_pur_qty = 0;
                           nstk.itm_isu_val = 0;

                           Data.tblStks.InsertOnSubmit(nstk);
                           Data.SubmitChanges();

                           det.stk_id = nstk.stk_id;
                           Data.SubmitChanges();
                       }

                       if (!string.IsNullOrEmpty(det.batch_id) && det.stk_id != null)
                       {
                           tblItem_Code itmCode = Data.tblItem_Codes.Where(itm => itm.itm_cd.Equals(det.itm_cd) && itm.br_id.Equals(stkData.br_id)).Single();

                           try
                           {
                               tblBatch batch = Data.tblBatches.Where(b => b.stk_id == det.stk_id && b.batch_id == det.batch_id).Single();


                               //batch.itm_pur_qty = 0;
                               //batch.updateby = stkData.updateby;
                               //batch.updateon = stkData.updateon;
                               //batch.Prod_Date = stkData.vr_dt;
                               //batch.Salvage_Qty = 0;
                               //if (itmCode.Exp_Days != null)
                               //    batch.Exp_Date = stkData.vr_dt.AddDays(Convert.ToDouble(itmCode.Exp_Days));
                               //else
                               //    batch.Exp_Date = null;
                               //batch.Status = true;
                               //Data.SubmitChanges();
                           }
                           catch
                           {
                               tblBatch batch = new tblBatch();

                               batch.stk_id = det.stk_id.Value;
                               batch.batch_id = det.batch_id;
                               batch.itm_pur_qty = 0;
                               batch.updateby = stkData.updateby;
                               batch.updateon = stkData.updateon;
                               batch.Prod_Date = stkData.vr_dt;
                               batch.Salvage_Qty = 0;
                               if (itmCode.Exp_Days != null)
                                   batch.Exp_Date = stkData.vr_dt.AddDays(Convert.ToDouble(itmCode.Exp_Days));
                               else
                                   batch.Exp_Date = null;
                               batch.Status = true;
                               Data.tblBatches.InsertOnSubmit(batch);
                               Data.SubmitChanges();
                           }
                       }
                   }
               }
               trans.Commit();
               return "Done";
           }
           catch (Exception exx)
           {
               if (trans != null)
                   trans.Rollback();
               return exx.Message;
           }
       }
    }
}
