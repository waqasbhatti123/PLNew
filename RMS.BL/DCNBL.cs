using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.Linq;

namespace RMS.BL
{
   public class DCNBL
    {
       public DCNBL()
       {

       }

       public List<tblSaleOrderDet> GetSaleOrderItemList(int orderid, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               return Data.tblSaleOrderDets.Where(ord => ord.OrderID == orderid && ord.Status.ToLower() == "op").ToList();
           }
           catch (Exception ex)
           {
               RMSDB.SetNull();
               throw ex;
           }
       }

       public tblSaleOrder GetSaleOrderByID(int orderid, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               return Data.tblSaleOrders.Where(ord => ord.OrderID == orderid).Single();
           }
           catch (Exception ex)
           {
               RMSDB.SetNull();
               throw ex;
           }
       }

       //public object GetSaleOrderDetailByID(int orderid, RMSDataContext Data)
       //{
       //    if (Data == null) { Data = RMSDB.GetOject(); }
       //    try
       //    {
       //        var obj = (from a in Data.tblSaleOrderDets
       //                   join c in Data.tblSaleOrders
       //                   on a.OrderID equals c.OrderID
       //                   join b in Data.tblItem_Codes
       //                   on a.ItemID equals b.itm_cd
       //                   where a.OrderID == orderid && a.Status.ToLower() == "op"
       //                   select new
       //                   {
       //                       a.OrderID,
       //                       a.Oseq,
       //                       OrderNo = c.OrderNo.ToString().Substring(0, 4) + "/" + c.OrderNo.ToString().Substring(4),
       //                       a.ItemID,
       //                       b.itm_dsc,
       //                       OrderQty = a.OrderQty,
       //                       ShippedQty = a.ShipQty,
       //                       PedingQty = (from x in Data.tblStkDatas
       //                                                 join y in Data.tblStkDataDets
       //                                                 on x.vr_id equals y.vr_id
       //                                                 where x.vr_apr == "P" && x.OrderID == orderid && y.itm_cd == a.ItemID
       //                                                 select y.vr_qty).Sum() == null ? 0 : (from x in Data.tblStkDatas
       //                                                                                       join y in Data.tblStkDataDets
       //                                                                                       on x.vr_id equals y.vr_id
       //                                                                                       where x.vr_apr == "P" && x.OrderID == orderid && y.itm_cd == a.ItemID
       //                                                                                       select y.vr_qty).Sum(),
       //                       Balance = a.OrderQty - a.ShipQty - ((from x in Data.tblStkDatas
       //                                                            join y in Data.tblStkDataDets
       //                                                            on x.vr_id equals y.vr_id
       //                                                            where x.vr_apr == "P" && x.OrderID == orderid && y.itm_cd == a.ItemID
       //                                                            select y.vr_qty).Sum() == null ? 0 : (from x in Data.tblStkDatas
       //                                                                                                  join y in Data.tblStkDataDets
       //                                                                                                  on x.vr_id equals y.vr_id
       //                                                                                                  where x.vr_apr == "P" && x.OrderID == orderid && y.itm_cd == a.ItemID
       //                                                                                                  select y.vr_qty).Sum()),
       //                   }).ToList();
       //        return obj;
       //    }
       //    catch (Exception ex)
       //    {
       //        RMSDB.SetNull();
       //        throw ex;
       //    }
       //}

       public tblItem_Code GetItemByCode(string itemcd, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               return Data.tblItem_Codes.Where(itm => itm.itm_cd == itemcd).Single();
           }
           catch (Exception ex)
           {
               RMSDB.SetNull();
               throw ex;
           }
       }

       public string IsItemQtyGreaterThanSaleOrderQty(tblSaleOrderDet det, decimal grdqty, bool isedit, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               tblSaleOrderDet slDet;
               if (isedit)
               {
                   slDet = Data.tblSaleOrderDets.Where(sl => sl.OrderID == det.OrderID && sl.ItemID == det.ItemID).Single();
               }
               else
               {
                   slDet = new tblSaleOrderDet();
                   slDet.OrderQty = 0;
               }
               var obj = (from x in Data.tblStkDatas
                          join y in Data.tblStkDataDets
                          on x.vr_id equals y.vr_id
                          where x.vr_apr == "P" && x.OrderID == det.OrderID && y.itm_cd == det.ItemID
                          select y);
               decimal qty = 0;
               foreach (var o in obj)
               {
                   qty = qty + o.vr_qty;
               }
               if (det.OrderQty < (det.ShipQty + qty + grdqty - slDet.OrderQty))
               {
                   slDet = null;
                   return "Item Quantity cannot be greater than order quantity";
               }
               else
               {
                   slDet = null;
                   return "ok";
               }
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
                         where a.vr_no.ToString().StartsWith(docno.Replace("/", "")) && a.vr_apr == "A" && a.vt_cd == 31 && a.Status.ToLower().Equals("op")
                         orderby a.vr_dt descending
                         select new
                         {
                             a.vr_id,
                             a.vr_no,
                             vr_no_formtd = a.vr_no.ToString().Substring(0, 4) + "/" + a.vr_no.ToString().Substring(4),
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


       public object GetSaleOrders(string party, string orderId , RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               var obj = (from a in Data.tblSaleOrders
                          join b in Data.Glmf_Codes
                          on a.Party equals b.gl_cd
                          join c in Data.tblSalesPersons
                          on a.SalesPerson equals c.ID into leftjoin
                          from d in leftjoin.DefaultIfEmpty()
                          where a.Party == party && a.OrderNo.ToString().StartsWith(orderId.Replace("/", "")) && a.Ord_Apr == 'A' && a.Status.ToLower().Equals("op")
                          select new
                          {
                              a.OrderID,
                              OrderNo = a.OrderNo.ToString().Substring(0,4) + "/" + a.OrderNo.ToString().Substring(4),
                              a.OrderDate,
                              party = b.gl_dsc,
                              salesperson = d.SalesPerson + " / " + d.Designation
                          }).ToList();
               return obj.OrderBy(o=> o.OrderID).ToList();
           }
           catch (Exception ex)
           {
               RMSDB.SetNull();
               throw ex;
           }
       }

       public decimal GetWHTax(DateTime date, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {

               DateTime effDate = Data.tblTaxRates.Where(tx => tx.TaxID == "WHT" && tx.EffDate <= date).Max(tx => tx.EffDate);

               return Data.tblTaxRates.Where(tax => tax.TaxID == "WHT" && tax.EffDate == effDate).Single().TaxRate / 100;


           }
           catch //(Exception ex)
           {
               //RMSDB.SetNull();
               //throw ex;
           }
           return 0;
       }

       public decimal GetTaxPercentByItemId(string itmcd, DateTime date, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
                       
               tblItem_Code item = Data.tblItem_Codes.Where(itm=> itm.itm_cd == itmcd).Single();
               if (item.TaxID != null)
               {
                   DateTime effDate = Data.tblTaxRates.Where(tx => tx.TaxID == item.TaxID && tx.EffDate <= date).Max(tx => tx.EffDate);

                   return Data.tblTaxRates.Where(tax => tax.TaxID == item.TaxID && tax.EffDate == effDate).Single().TaxRate/100;
               }
               else
                   return 0;

           }
           catch //(Exception ex)
           {
               //RMSDB.SetNull();
               //throw ex;
           }
           return 0;
       }

       public decimal GetTaxRateByTaxId(string taxid, DateTime date, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               if (taxid != "0")
               {
                   DateTime effDate = Data.tblTaxRates.Where(tx => tx.TaxID == taxid && tx.EffDate <= date && tx.Status.ToLower() == "op").Max(tx => tx.EffDate);

                   return Data.tblTaxRates.Where(tax => tax.TaxID == taxid && tax.EffDate == effDate).Single().TaxRate / 100;
               }
               else
                   return 0;

           }
           catch //(Exception ex)
           {
               //RMSDB.SetNull();
               //throw ex;
           }
           return 0;
       }

       public IQueryable<spPrintNoteResult> PrintNote(int vrid, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               return Data.spPrintNote(vrid).AsQueryable();
           }
           catch (Exception ex)
           {
               RMSDB.SetNull();
               throw ex;
           }
       }

       public IQueryable<spPrintInvoiceResult> PrintInvoice(int vrid, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               return Data.spPrintInvoice(vrid).AsQueryable();
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

       public object GetDCNote(string docno, char status, RMSDataContext Data)
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
                          where a.vt_cd == 34 && a.vr_apr == (status == '0' ? a.vr_apr : status.ToString())
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
               else
               {
                   obj = (from a in Data.tblStkDatas
                          join b in Data.tblStock_Locs
                          on a.LocId equals b.LocId
                          where a.vt_cd == 34 && a.vr_apr == (status == '0' ? a.vr_apr : status.ToString()) && a.vr_no.ToString() == docno
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

       public int GetStkIdByBatch(string batchid, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }

           return Data.tblBatches.Single(p => p.batch_id.Equals(batchid)).stk_id;
       }

       public int GetStkId(int brid, int locid, string itmcode, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }

           return Data.tblStks.Single(p => p.br_id.Equals(brid) && p.LocId.Equals(locid) && p.itm_cd.Equals(itmcode)).stk_id;
       }

       public decimal GetStk(int brid, string itmcd, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }

           return Convert.ToDecimal(Data.GetItemDesc(brid, itmcd).Single().stkqty);
       }

       public decimal GetBatchQty(int brid, string batchid, string itmcode, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           var rec = Data.GetItemBatchDesc(brid, batchid, itmcode).Where(p=> p.batch_id.Equals(batchid)).Single();
           if (rec != null)
               return Convert.ToDecimal(rec.batch_qty);
           else
               return 0;
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
               tblBatch batch = null;
               tblBillDet billDet = null;
               int seq = 0, billId = GetBillId(stkD.br_id, Data);
               decimal billTotal = 0, tax=0, totaltax=0;

               DateTime date = Common.MyDate(Data);
               FIN_PERD fin = fin = (from a in Data.FIN_PERDs
                                      where a.Start_Date <= date.Date && a.End_Date >= date.Date
                                      select a).Single();
                           
              Data.tblStkDatas.InsertOnSubmit(stkD);
              Data.SubmitChanges();

               if (stkD.vr_apr.ToString().Equals("A"))
               {
                   foreach (tblStkDataDet det in stkD.tblStkDataDets)
                   {
                       stk = Data.tblStks.Single(p => p.itm_cd.Equals(det.itm_cd) && p.br_id == stkD.br_id && p.LocId == stkD.LocId);
                       stk.itm_isu_qty = stk.itm_isu_qty + det.vr_qty;

                       if (!string.IsNullOrEmpty(det.batch_id) && det.stk_id != null)
                       {
                           batch = Data.tblBatches.Single(p => p.stk_id.Equals(det.stk_id) && p.batch_id.Equals(det.batch_id));
                           batch.itm_isu_qty = batch.itm_isu_qty + det.vr_qty;
                       }
                       /*Bill Detail*/
                       billDet = new tblBillDet();
                       billDet.vrid = billId;
                       billDet.vrseq = ++seq;
                       billDet.TranType = 1;
                       billDet.TranRef = null;
                       billDet.stk_id = det.stk_id;
                       billDet.Qty = det.vr_qty;
                       tax = GetItemAmount(stkD.gl_cd, det.itm_cd, Data) * det.vr_qty * GetTaxPercentByItemId(det.itm_cd, stkD.vr_dt, Data);
                       totaltax = totaltax + tax;
                       billDet.Tax = tax;
                       billDet.Discount = 0;
                       billDet.IV_Amount = GetItemAmount(stkD.gl_cd, det.itm_cd, Data) * det.vr_qty;
                       billTotal = billTotal + Convert.ToDecimal(billDet.IV_Amount) + tax;
                       billDet.IV_Status = "OP";

                       Data.tblBillDets.InsertOnSubmit(billDet);
                       /*End Of Bill Detail*/

                       //Update Sale Order Details
                       if (stkD.OrderID > 0)
                       {
                           tblSaleOrderDet slDet = Data.tblSaleOrderDets.Where(ord => ord.OrderID == stkD.OrderID && ord.ItemID == det.itm_cd).Single();

                           slDet.ShipQty = slDet.ShipQty + det.vr_qty;
                           if (slDet.OrderQty == slDet.ShipQty)
                           {
                               slDet.Status = "CL";
                           }
                       }
                       //End Of Update Sale Order Details
                       Data.SubmitChanges();
                   }

                   /*Bill Master*/
                   tblBill bill = new tblBill();
                   bill.vrid = billId;
                   bill.brid = stkD.br_id;
                   bill.IV_Type = "35";
                   bill.IV_NO = GetInvNo(Convert.ToInt32(fin.Gl_Year), stkD.br_id, bill.IV_Type, Data);
                   bill.PartyID = stkD.gl_cd;
                   bill.IV_Ref = stkD.vr_id.ToString();
                   bill.IV_Date = stkD.vr_dt;
                   bill.IV_Due_Date = stkD.vr_dt;
                   bill.IV_Total_Amt = billTotal;
                   bill.Settled_Amt = 0;
                   bill.PrtSeq = 0;
                   bill.Rmk = "";
                   bill.IV_Status = "OP";
                   bill.IV_WHT = GetWHTax(stkD.vr_dt, Data) * (billTotal - totaltax);

                   Data.tblBills.InsertOnSubmit(bill);
                   Data.SubmitChanges();
                   /*End Of Bill Master*/

                   //Update Sale Order Master
                   if (stkD.OrderID > 0)
                   {
                       bool updMaster = true;
                       var lst = Data.tblSaleOrderDets.Where(det => det.OrderID == stkD.OrderID).ToList();
                       foreach (var a in lst)
                       {
                           if (a.Status.ToLower() == "op")
                           {
                               updMaster = false;
                               break;
                           }
                       }
                       if (updMaster)
                       {
                           tblSaleOrder ord = Data.tblSaleOrders.Where(o => o.OrderID == stkD.OrderID).Single();
                           ord.Status = "CL";
                           Data.SubmitChanges();
                       }
                   }
                   //End Of Update Sale Order Master
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

       public string SaveDCN(tblStkData stkD, RMSDataContext Data)
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
               tblBatch batch = null;
               tblBillDet billDet = null;
               int seq = 0, billId = GetBillId(stkD.br_id, Data);
               decimal billTotal = 0, tax = 0, totaltax = 0;

               DateTime date = Common.MyDate(Data);
               FIN_PERD fin = fin = (from a in Data.FIN_PERDs
                                     where a.Start_Date <= date.Date && a.End_Date >= date.Date
                                     select a).Single();

               Data.tblStkDatas.InsertOnSubmit(stkD);
               Data.SubmitChanges();
               string gstid = "0", whtid = "0";
               decimal itmrate = 0;
               if (stkD.vr_apr.ToString().Equals("A"))
               {
                   foreach (tblStkDataDet det in stkD.tblStkDataDets)
                   {
                       stk = Data.tblStks.Single(p => p.itm_cd.Equals(det.itm_cd) && p.br_id == stkD.br_id && p.LocId == stkD.LocId);
                       stk.itm_isu_qty = stk.itm_isu_qty + det.vr_qty;

                       if (!string.IsNullOrEmpty(det.batch_id) && det.stk_id != null)
                       {
                           batch = Data.tblBatches.Single(p => p.stk_id.Equals(det.stk_id) && p.batch_id.Equals(det.batch_id));
                           batch.itm_isu_qty = batch.itm_isu_qty + det.vr_qty;
                       }
                       /*Bill Detail*/
                       billDet = new tblBillDet();
                       billDet.vrid = billId;
                       billDet.vrseq = ++seq;
                       billDet.TranType = 1;
                       billDet.TranRef = null;
                       billDet.stk_id = det.stk_id;
                       billDet.Qty = det.vr_qty;

                       //tax = GetItemAmount(stkD.gl_cd, det.itm_cd, Data) * det.vr_qty * GetTaxPercentByItemId(det.itm_cd, stkD.vr_dt, Data);
                       gstid = GetGSTIdOfSaleOrder(Convert.ToInt32(stkD.OrderID), det.itm_cd, Data);
                       itmrate = GetItemRate(stkD.gl_cd, Convert.ToInt32(stkD.OrderID), det.itm_cd, Data);
                       tax = itmrate * det.vr_qty * GetTaxRateByTaxId(gstid, date, Data);

                       totaltax = totaltax + tax;
                       billDet.Tax = tax;
                       billDet.GSTid = gstid;
                       billDet.Discount = 0;
                       //billDet.IV_Amount = GetItemAmount(stkD.gl_cd, det.itm_cd, Data) * det.vr_qty;
                       billDet.IV_Amount = itmrate * det.vr_qty;
                       billTotal = billTotal + Convert.ToDecimal(billDet.IV_Amount) + tax;
                       billDet.IV_Status = "OP";

                       Data.tblBillDets.InsertOnSubmit(billDet);
                       /*End Of Bill Detail*/

                       //Update Sale Order Details
                       if (stkD.OrderID > 0)
                       {
                           tblSaleOrderDet slDet = Data.tblSaleOrderDets.Where(ord => ord.OrderID == stkD.OrderID && ord.ItemID == det.itm_cd).Single();

                           slDet.ShipQty = slDet.ShipQty + det.vr_qty;
                           if (slDet.OrderQty == slDet.ShipQty)
                           {
                               slDet.Status = "CL";
                           }
                       }
                       //End Of Update Sale Order Details
                       Data.SubmitChanges();
                   }

                   /*Bill Master*/
                   tblBill bill = new tblBill();
                   bill.vrid = billId;
                   bill.brid = stkD.br_id;
                   bill.IV_Type = "35";
                   bill.IV_NO = GetInvNo(Convert.ToInt32(fin.Gl_Year), stkD.br_id, bill.IV_Type, Data);
                   bill.PartyID = stkD.gl_cd;
                   bill.IV_Ref = stkD.vr_id.ToString();
                   bill.IV_Date = stkD.vr_dt;
                   bill.IV_Due_Date = stkD.vr_dt;
                   bill.IV_Total_Amt = billTotal;
                   bill.IV_Net_Discount = 0;
                   bill.Settled_Amt = 0;
                   bill.PrtSeq = 0;
                   bill.Rmk = "";
                   bill.IV_Status = "OP";
                   //bill.IV_WHT = GetWHTax(stkD.vr_dt, Data) * (billTotal - totaltax);
                   whtid = GetWHTIdOfSaleOrder(Convert.ToInt32(stkD.OrderID), Data);
                   //bill.IV_WHT = (billTotal + totaltax) * GetTaxRateByTaxId(whtid, date, Data);
                   bill.IV_WHT = (billTotal) * GetTaxRateByTaxId(whtid, date, Data);
                   bill.IV_Total_Amt_Diff = (billTotal + bill.IV_WHT) - Math.Round((billTotal + Convert.ToDecimal(bill.IV_WHT)));
                   bill.WHTid = whtid;
                   Data.tblBills.InsertOnSubmit(bill);
                   Data.SubmitChanges();
                   /*End Of Bill Master*/

                   //Update Sale Order Master
                   if (stkD.OrderID > 0)
                   {
                       bool updMaster = true;
                       var lst = Data.tblSaleOrderDets.Where(det => det.OrderID == stkD.OrderID).ToList();
                       foreach (var a in lst)
                       {
                           if (a.Status.ToLower() == "op")
                           {
                               updMaster = false;
                               break;
                           }
                       }
                       if (updMaster)
                       {
                           tblSaleOrder ord = Data.tblSaleOrders.Where(o => o.OrderID == stkD.OrderID).Single();
                           ord.Status = "CL";
                           Data.SubmitChanges();
                       }
                   }
                   //End Of Update Sale Order Master
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

       public decimal GetItemRate(string gl_cd, int orderid, string itmcd, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               decimal rate = 0;
               try
               {
                   rate = Data.tblSaleOrderDets.Where(so => so.OrderID == orderid && so.ItemID == itmcd).Single().ItemRate;
               }
               catch { rate = 0; }

               if (rate == 0)
               {
                   rate = Data.tblSaleRates.Where(rt => rt.PartyID == gl_cd && rt.ItemID == itmcd && rt.Status.ToLower() == "op").SingleOrDefault().SaleRate;
               }
               return rate;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public string GetGSTIdOfSaleOrder(int orderid, string itmcd, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               string gst = Data.tblSaleOrderDets.Where(so => so.OrderID == orderid && so.ItemID == itmcd).Single().GSTid;
               return gst;

           }
           catch { }
           return "0";
       }

       public string GetWHTIdOfSaleOrder(int orderid, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               string gst = Data.tblSaleOrders.Where(so => so.OrderID == orderid ).Single().WHTid;
               return gst;

           }
           catch { }
           return "0";
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
               tblBatch batch = null;
               tblBillDet billDet = null;
               int seq = 0, billId = GetBillId(stkData.br_id, Data);
               decimal billTotal = 0, tax=0, totaltax=0;

               DateTime date = Common.MyDate(Data);
               FIN_PERD fin = fin = (from a in Data.FIN_PERDs
                                     where a.Start_Date <= date.Date && a.End_Date >= date.Date
                                     select a).Single();

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
                       stk = Data.tblStks.Single(p => p.itm_cd.Equals(det.itm_cd) && p.br_id == stkData.br_id && p.LocId == stkData.LocId);
                       stk.itm_isu_qty = stk.itm_isu_qty + det.vr_qty;


                       if (!string.IsNullOrEmpty(det.batch_id) && det.stk_id != null)
                       {
                           batch = Data.tblBatches.Single(p => p.stk_id.Equals(det.stk_id) && p.batch_id.Equals(det.batch_id));
                           batch.itm_isu_qty = batch.itm_isu_qty + det.vr_qty;
                       }
                       /*Bill Detail*/
                       billDet = new tblBillDet();
                       billDet.vrid = billId;
                       billDet.vrseq = ++seq;
                       billDet.TranType = 1;
                       billDet.TranRef = null;
                       billDet.stk_id = det.stk_id;
                       billDet.Qty = det.vr_qty;
                       tax = GetItemAmount(stkData.gl_cd, det.itm_cd, Data) * det.vr_qty * GetTaxPercentByItemId(det.itm_cd, stkData.vr_dt, Data);
                       totaltax = totaltax + tax;
                       billDet.Tax = tax;
                       billDet.Discount = 0;
                       billDet.IV_Amount = GetItemAmount(stkData.gl_cd, det.itm_cd, Data) * det.vr_qty;
                       billTotal = billTotal + Convert.ToDecimal(billDet.IV_Amount) + tax;
                       billDet.IV_Status = "OP";

                       Data.tblBillDets.InsertOnSubmit(billDet);
                       /*End Of Bill Detail*/


                        //Update Sale Order Details
                       if (stkData.OrderID > 0)
                       {
                           tblSaleOrderDet slDet = Data.tblSaleOrderDets.Where(ord => ord.OrderID == stkData.OrderID && ord.ItemID == det.itm_cd).Single();

                           slDet.ShipQty = slDet.ShipQty + det.vr_qty;
                           if (slDet.OrderQty == slDet.ShipQty)
                           {
                               slDet.Status = "CL";
                           }
                       }
                       //End Of Update Sale Order Details
                       Data.SubmitChanges();
                   }
                   /*Bill Master*/
                   tblBill bill = new tblBill();
                   bill.vrid = billId;
                   bill.brid = stkData.br_id;
                   bill.IV_Type = "35";
                   bill.IV_NO = GetInvNo(Convert.ToInt32(fin.Gl_Year), stkData.br_id, bill.IV_Type, Data);
                   bill.PartyID = stkData.gl_cd;
                   bill.IV_Ref = stkData.vr_id.ToString();
                   bill.IV_Date = stkData.vr_dt;
                   bill.IV_Due_Date = stkData.vr_dt;
                   bill.IV_Total_Amt = billTotal;
                   bill.Settled_Amt = 0;
                   bill.PrtSeq = 0;
                   bill.Rmk = "";
                   bill.IV_Status = "OP";
                   bill.IV_WHT = GetWHTax(stkData.vr_dt,Data)*(billTotal - totaltax);

                   Data.tblBills.InsertOnSubmit(bill);
                   Data.SubmitChanges();
                   /*End Of Bill Master*/

                   //Update Sale Order Master
                   if (stkData.OrderID > 0)
                   {
                       bool updMaster = true;
                       var lst = Data.tblSaleOrderDets.Where(det => det.OrderID == stkData.OrderID).ToList();
                       foreach (var a in lst)
                       {
                           if (a.Status.ToLower() == "op")
                           {
                               updMaster = false;
                               break;
                           }
                       }
                       if (updMaster)
                       {
                           tblSaleOrder ord = Data.tblSaleOrders.Where(o => o.OrderID == stkData.OrderID).Single();
                           ord.Status = "CL";
                           Data.SubmitChanges();
                       }
                   }
                   //End Of Update Sale Order Master
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

       public string UpdateDCN(tblStkData stkData, EntitySet<tblStkDataDet> newDets, RMSDataContext Data)
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
               tblBatch batch = null;
               tblBillDet billDet = null;
               int seq = 0, billId = GetBillId(stkData.br_id, Data);
               decimal billTotal = 0, tax = 0, totaltax = 0;

               DateTime date = Common.MyDate(Data);
               FIN_PERD fin = fin = (from a in Data.FIN_PERDs
                                     where a.Start_Date <= date.Date && a.End_Date >= date.Date
                                     select a).Single();

               List<tblStkDataDet> existingDets = (from p in Data.tblStkDataDets
                                                   where p.vr_id == stkData.vr_id
                                                   select p).ToList();

               //delete
               Data.tblStkDataDets.DeleteAllOnSubmit(existingDets);
               Data.SubmitChanges();

               //insert again
               stkData.tblStkDataDets = newDets;
               Data.SubmitChanges();

               string gstid = "0", whtid = "0";
               decimal itmrate = 0;
               if (stkData.vr_apr.ToString().Equals("A"))
               {
                   foreach (tblStkDataDet det in newDets)
                   {
                       stk = Data.tblStks.Single(p => p.itm_cd.Equals(det.itm_cd) && p.br_id == stkData.br_id && p.LocId == stkData.LocId);
                       stk.itm_isu_qty = stk.itm_isu_qty + det.vr_qty;


                       if (!string.IsNullOrEmpty(det.batch_id) && det.stk_id != null)
                       {
                           batch = Data.tblBatches.Single(p => p.stk_id.Equals(det.stk_id) && p.batch_id.Equals(det.batch_id));
                           batch.itm_isu_qty = batch.itm_isu_qty + det.vr_qty;
                       }
                       /*Bill Detail*/
                       billDet = new tblBillDet();
                       billDet.vrid = billId;
                       billDet.vrseq = ++seq;
                       billDet.TranType = 1;
                       billDet.TranRef = null;
                       billDet.stk_id = det.stk_id;
                       billDet.Qty = det.vr_qty;
                       //tax = GetItemAmount(stkData.gl_cd, det.itm_cd, Data) * det.vr_qty * GetTaxPercentByItemId(det.itm_cd, stkData.vr_dt, Data);
                       gstid = GetGSTIdOfSaleOrder(Convert.ToInt32(stkData.OrderID), det.itm_cd, Data);
                       itmrate = GetItemRate(stkData.gl_cd, Convert.ToInt32(stkData.OrderID), det.itm_cd, Data);
                       tax = itmrate * det.vr_qty * GetTaxRateByTaxId(gstid, date, Data);
                       billDet.GSTid = gstid;
                       totaltax = totaltax + tax;
                       billDet.Tax = tax;
                       billDet.Discount = 0;
                       //billDet.IV_Amount = GetItemAmount(stkData.gl_cd, det.itm_cd, Data) * det.vr_qty;
                       billDet.IV_Amount = itmrate * det.vr_qty;
                       billTotal = billTotal + Convert.ToDecimal(billDet.IV_Amount) + tax;
                       billDet.IV_Status = "OP";


                       Data.tblBillDets.InsertOnSubmit(billDet);
                       /*End Of Bill Detail*/


                       //Update Sale Order Details
                       if (stkData.OrderID > 0)
                       {
                           tblSaleOrderDet slDet = Data.tblSaleOrderDets.Where(ord => ord.OrderID == stkData.OrderID && ord.ItemID == det.itm_cd).Single();

                           slDet.ShipQty = slDet.ShipQty + det.vr_qty;
                           if (slDet.OrderQty == slDet.ShipQty)
                           {
                               slDet.Status = "CL";
                           }
                       }
                       //End Of Update Sale Order Details
                       Data.SubmitChanges();
                   }
                   /*Bill Master*/
                   tblBill bill = new tblBill();
                   bill.vrid = billId;
                   bill.brid = stkData.br_id;
                   bill.IV_Type = "35";
                   bill.IV_NO = GetInvNo(Convert.ToInt32(fin.Gl_Year), stkData.br_id, bill.IV_Type, Data);
                   bill.PartyID = stkData.gl_cd;
                   bill.IV_Ref = stkData.vr_id.ToString();
                   bill.IV_Date = stkData.vr_dt;
                   bill.IV_Due_Date = stkData.vr_dt;
                   bill.IV_Total_Amt = billTotal;
                   bill.Settled_Amt = 0;
                   bill.PrtSeq = 0;
                   bill.Rmk = "";
                   bill.IV_Status = "OP";
                   //bill.IV_WHT = GetWHTax(stkData.vr_dt, Data) * (billTotal - totaltax);
                   whtid = GetWHTIdOfSaleOrder(Convert.ToInt32(stkData.OrderID), Data);
                   bill.IV_WHT = (billTotal + totaltax) * GetTaxRateByTaxId(whtid, date, Data);
                   
                   bill.IV_Total_Amt_Diff = (billTotal + bill.IV_WHT) - Math.Round((billTotal + Convert.ToDecimal(bill.IV_WHT)));
                   bill.IV_Net_Discount = 0;
                   bill.WHTid = whtid;
                   Data.tblBills.InsertOnSubmit(bill);
                   Data.SubmitChanges();
                   /*End Of Bill Master*/

                   //Update Sale Order Master
                   if (stkData.OrderID > 0)
                   {
                       bool updMaster = true;
                       var lst = Data.tblSaleOrderDets.Where(det => det.OrderID == stkData.OrderID).ToList();
                       foreach (var a in lst)
                       {
                           if (a.Status.ToLower() == "op")
                           {
                               updMaster = false;
                               break;
                           }
                       }
                       if (updMaster)
                       {
                           tblSaleOrder ord = Data.tblSaleOrders.Where(o => o.OrderID == stkData.OrderID).Single();
                           ord.Status = "CL";
                           Data.SubmitChanges();
                       }
                   }
                   //End Of Update Sale Order Master
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

       public decimal GetItemAmount(string party, string itmcode, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }

           try
           {
               List<tblSaleRate> sRate = Data.tblSaleRates.Where(rt => rt.PartyID == party && rt.Status == "OP").ToList();

               foreach (tblSaleRate rt in sRate)
               {
                   if (itmcode == rt.ItemID)
                   {
                       return rt.SaleRate;
                   }

               }
      
           }
           catch
           { }
           return 0;
       }


       public int GetInvNo(int year, int brid, string ivtyp, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           string ivno = "", counter = "";
           try
           {
               ivno = Data.tblBills.Where(bill => bill.brid == brid && bill.IV_Type == ivtyp).Max(bill => bill.IV_NO).Value.ToString();
               counter = Convert.ToString(Convert.ToInt32(ivno.Substring(4)) + 1).PadLeft(5,'0');
               
               return Convert.ToInt32(year + counter);
           }
           catch
           {}
           return Convert.ToInt32(year + "00001");
       }

       public int GetBillId(int brid, RMSDataContext Data)
       {

           if (Data == null) { Data = RMSDB.GetOject(); }
           int vrid = 1;
           try
           {
               vrid = Data.tblBills.Max(bill=> bill.vrid) + vrid;
               return vrid;
           }
           catch
           {}
           return vrid;
       }

       public object wmGetBatchDetail(string batchid, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           
           string[] arr = batchid.Split('-').ToArray();

           if (!string.IsNullOrEmpty(arr[1]) && arr[1] != "0")
           {
               return Data.GetItemBatchDesc(1, arr[0], arr[1]).ToList();
           }
           else
           {
               return null;
           }
       }
    }
}
