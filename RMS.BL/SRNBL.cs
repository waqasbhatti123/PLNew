using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.Linq;

namespace RMS.BL
{
   public class SRNBL
    {
       public SRNBL()
       {

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
                          join c in Data.tblStkDatas
                          on a.DocRef equals c.vr_id.ToString()
                          where a.vt_cd == 32 && c.vt_cd == 31 && a.vr_apr == (status == '0' ? a.vr_apr : status.ToString()) 
                          select new
                          {
                              a.vr_id,
                              ptnvr_id = c.vr_id,
                              a.LocId,
                              vr_no = a.vr_no.ToString().Substring(0, 4) + "/" + a.vr_no.ToString().Substring(4),
                              ptnvr_no = c.vr_no.ToString().Substring(0, 4) + "/" + c.vr_no.ToString().Substring(4),
                              a.vr_dt,
                              a.vr_apr,
                              a.vr_nrtn,
                              b.LocName
                          }).OrderBy(o => o.vr_no).ToList();
               }
               else
               {
                   obj = (from a in Data.tblStkDatas
                          join b in Data.tblStock_Locs
                          on a.LocId equals b.LocId
                          join c in Data.tblStkDatas
                          on a.DocRef equals c.vr_id.ToString()
                          where a.vt_cd == 32 && c.vt_cd == 31 && a.vr_apr == (status == '0' ? a.vr_apr : status.ToString()) && a.vr_no.ToString() == docno
                          select new
                          {
                              a.vr_id,
                              a.LocId,
                              vr_no = a.vr_no.ToString().Substring(0, 4) + "/" + a.vr_no.ToString().Substring(4),
                              ptnvr_no = c.vr_no.ToString().Substring(0, 4) + "/" + c.vr_no.ToString().Substring(4),
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

       public object SrchDocByNo(string docno, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               var obj = from a in Data.tblStkDatas
                         where a.vr_no.ToString().StartsWith(docno.Replace("/","")) && a.vr_apr == "A" && a.vt_cd == 31 && a.Status.ToLower().Equals("op")
                         orderby a.vr_dt descending
                         select new
                         {
                             a.vr_id,
                             a.vr_no,
                             vr_no_formtd = a.vr_no.ToString().Substring(0,4)+"/"+a.vr_no.ToString().Substring(4),
                             a.vr_dt 
                         };
               return obj.ToList();
           }
           catch (Exception ex)
           {
               RMSDB.SetNull();
               throw ex;
           }
       }

       public string Save(int ptnVrID, tblStkData stkD, RMSDataContext Data)
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

               tblStkData ptnStk = Data.tblStkDatas.Where(stkRow => stkRow.vr_id == ptnVrID).Single();
               if (stkD.vr_apr == "C")
                   ptnStk.Status = "OP";
               else
                   ptnStk.Status = "CL";

               stkD.DocRef = ptnStk.vr_id.ToString();
               
               Data.tblStkDatas.InsertOnSubmit(stkD);
               Data.SubmitChanges();

               

               //tblStkDataDet valDet = null;
               if (stkD.vr_apr.ToString().Equals("A"))
               {
                   foreach (tblStkDataDet det in stkD.tblStkDataDets)
                   {

                       stk = Data.tblStks.Single(p => p.br_id.Equals(stkD.br_id) && p.LocId.Equals(stkD.LocId) && p.itm_cd.Equals(det.itm_cd));
                       stk.itm_pur_qty = stk.itm_pur_qty + (det.vr_qty - det.vr_qty_Rej);
                       det.stk_id = stk.stk_id;
                       Data.SubmitChanges();
   

                       if (!string.IsNullOrEmpty(det.batch_id) && det.stk_id != null)
                       {
                           tblItem_Code itmCode = Data.tblItem_Codes.Where(itm => itm.itm_cd.Equals(det.itm_cd) && itm.br_id.Equals(stkD.br_id)).Single();

                           tblBatch batch = Data.tblBatches.Where(b => b.stk_id == det.stk_id && b.batch_id == det.batch_id).Single();

                           batch.itm_pur_qty = batch.itm_pur_qty + det.vr_qty;
                           batch.updateby = stkD.updateby;
                           batch.updateon = stkD.updateon;
                           //batch.Prod_Date = stkD.vr_dt;
                           //batch.Salvage_Qty = 0;
                           //if (itmCode.Exp_Days != null)
                           //    batch.Exp_Date = stkD.vr_dt.AddDays(Convert.ToDouble(itmCode.Exp_Days));
                           //else
                           //    batch.Exp_Date = null;
                           //batch.Status = true;
                           Data.SubmitChanges();
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

       public string Update(int ptnVrID, tblStkData stkData, EntitySet<tblStkDataDet> newDets, RMSDataContext Data)
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
         

               List<tblStkDataDet> existingDets = (from p in Data.tblStkDataDets
                                                   where p.vr_id == stkData.vr_id
                                                   select p).ToList();

               //delete
               Data.tblStkDataDets.DeleteAllOnSubmit(existingDets);
               Data.SubmitChanges();

               tblStkData ptnStk = Data.tblStkDatas.Where(stkRow => stkRow.vr_id == ptnVrID).Single();
               if (stkData.vr_apr == "C")
                   ptnStk.Status = "OP";
               else
                   ptnStk.Status = "CL";

               stkData.DocRef = ptnStk.vr_id.ToString();

               //insert again
               stkData.tblStkDataDets = newDets;
               Data.SubmitChanges();

               if (stkData.vr_apr.ToString().Equals("A"))
               {
                   foreach (tblStkDataDet det in newDets)
                   {

                       stk = Data.tblStks.Single(p => p.br_id.Equals(stkData.br_id) && p.LocId.Equals(stkData.LocId) && p.itm_cd.Equals(det.itm_cd));
                       stk.itm_pur_qty = stk.itm_pur_qty + (det.vr_qty - det.vr_qty_Rej);
                       det.stk_id = stk.stk_id;
                       Data.SubmitChanges();

                       if (!string.IsNullOrEmpty(det.batch_id) && det.stk_id != null)
                       {
                           tblItem_Code itmCode = Data.tblItem_Codes.Where(itm => itm.itm_cd.Equals(det.itm_cd) && itm.br_id.Equals(stkData.br_id)).Single();

                           tblBatch batch = Data.tblBatches.Where(b => b.stk_id == det.stk_id && b.batch_id == det.batch_id).Single();

                           batch.itm_pur_qty = batch.itm_pur_qty + det.vr_qty;
                           batch.updateby = stkData.updateby;
                           batch.updateon = stkData.updateon;
                           //batch.Prod_Date = stkData.vr_dt;
                           //batch.Salvage_Qty = 0;
                           //if (itmCode.Exp_Days != null)
                           //    batch.Exp_Date = stkData.vr_dt.AddDays(Convert.ToDouble(itmCode.Exp_Days));
                           //else
                           //    batch.Exp_Date = null;
                           //batch.Status = true;
                           Data.SubmitChanges();

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
