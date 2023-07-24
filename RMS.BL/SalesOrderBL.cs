using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Linq;

namespace RMS.BL
{
    public class SalesOrderBL
    {
        public SalesOrderBL()
        { }

        public string GetSalesPersonInfo(int id, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                var heads = (from a in Data.tblSalesPersons
                             where a.ID == id
                             select new
                             {
                                 info = a.SalesPerson + " / " + a.Designation
                             }).Single();


                return heads.info;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }

        }

        public List<spSalesOrderResult> GetSalesOrder(int orderid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                return Data.spSalesOrder(orderid).ToList();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public object GetSaleOrder(string docno, string status, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {

                object obj;
                if (docno == "")
                {
                    obj = (from a in Data.tblSaleOrders
                           join b in Data.Glmf_Codes
                           on a.Party equals b.gl_cd 
                           join c in Data.tblSalesPersons
                           on a.SalesPerson equals c.ID into leftjoin
                           from c in leftjoin.DefaultIfEmpty()
                           where a.Status == (status == "OP" ? status : a.Status)
                            && (a.Req_Apr == null || a.Req_Apr == "A")
                           select new
                           {
                               a.OrderID,
                               a.OrderDate,
                               OrderNo = a.OrderNo == 0 ? "" : a.OrderNo.ToString().Substring(0, 4) + "/" + a.OrderNo.ToString().Substring(4),
                               ReqNo = a.ReqNo == 0 ? "" : a.ReqNo.ToString().Substring(0, 4) + "/" + a.ReqNo.ToString().Substring(4),
                               Status = a.Ord_Apr == null ? "" : a.Ord_Apr == 'A' ? "Approved" : a.Ord_Apr == 'P' ? "Peding" : "Cancelled",
                               ReqStatus = a.Req_Apr == null ? "" : a.Req_Apr == "A" ? "Confirmed" : a.Req_Apr == "P" ? "In Progress" : "Cancelled",
                               a.Remarks,
                               b.gl_dsc,
                               SalesPerson = c.SalesPerson + " / " + c.Designation
                           }).OrderBy(o => o.OrderNo).ToList();
                }
                else
                {
                    obj = (from a in Data.tblSaleOrders
                           join b in Data.Glmf_Codes
                           on a.Party equals b.gl_cd
                           join c in Data.tblSalesPersons
                           on a.SalesPerson equals c.ID into leftjoin
                           from c in leftjoin.DefaultIfEmpty()
                           where a.Status == (status == "OP" ? status : a.Status) && a.OrderNo.ToString() == docno
                           && (a.Req_Apr == null || a.Req_Apr == "A")
                           select new
                           {
                               a.OrderID,
                               a.OrderDate,
                               OrderNo = a.OrderNo == 0 ? "" : a.OrderNo.ToString().Substring(0, 4) + "/" + a.OrderNo.ToString().Substring(4),
                               ReqNo = a.ReqNo == 0 ? "" : a.ReqNo.ToString().Substring(0, 4) + "/" + a.ReqNo.ToString().Substring(4),
                               Status = a.Ord_Apr == null ? "" : a.Ord_Apr == 'A' ? "Approved" : a.Ord_Apr == 'P' ? "Pending" : "Cancelled",
                               ReqStatus = a.Req_Apr == null ? "" : a.Req_Apr == "A" ? "Confirmed" : a.Req_Apr == "P" ? "In Progress" : "Cancelled",
                               a.Remarks,
                               SalesPerson = c.SalesPerson + " / " + c.Designation,
                               b.gl_dsc
                           }).OrderBy(o => o.OrderNo).ToList();
                }
                return obj;
            }
            catch (Exception exx)
            {
                return exx.Message;
            }
        }

        public object GetSaleReqOrder(string party, string docno, string status, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {

                object obj;
                if (docno == "")
                {
                    obj = (from a in Data.tblSaleOrders
                           join b in Data.Glmf_Codes
                           on a.Party equals b.gl_cd
                           join c in Data.tblSalesPersons
                           on a.SalesPerson equals c.ID into leftjoin
                           from c in leftjoin.DefaultIfEmpty()
                           where a.Status == (status == "OP" ? status : a.Status)
                           && a.Req_Apr != null
                           && b.gl_cd == party
                           select new
                           {
                               a.OrderID,
                               a.OrderDate,
                               OrderNo = a.OrderNo == 0 ? "" : a.OrderNo.ToString().Substring(0, 4) + "/" + a.OrderNo.ToString().Substring(4),
                               ReqNo = a.ReqNo == 0 ? "" : a.ReqNo.ToString().Substring(0, 4) + "/" + a.ReqNo.ToString().Substring(4),
                               Status = a.Ord_Apr == null ? "" : a.Ord_Apr == 'A' ? "Approved" : a.Ord_Apr == 'P' ? "Pending" : "Cancelled",
                               ReqStatus = a.Req_Apr == null ? "" : a.Req_Apr == "A" ? "Confirmed" : a.Req_Apr == "P" ? "In Progress" : "Cancelled",
                               a.Remarks,
                               b.gl_dsc,
                               SalesPerson = c.SalesPerson + " / " + c.Designation
                           }).OrderBy(o => o.OrderNo).ToList();
                }
                else
                {
                    obj = (from a in Data.tblSaleOrders
                           join b in Data.Glmf_Codes
                           on a.Party equals b.gl_cd
                           join c in Data.tblSalesPersons
                           on a.SalesPerson equals c.ID into leftjoin
                           from c in leftjoin.DefaultIfEmpty()
                           where a.Status == (status == "OP" ? status : a.Status) && a.ReqNo.ToString() == docno
                           && a.Req_Apr != null
                           && b.gl_cd == party
                           select new
                           {
                               a.OrderID,
                               a.OrderDate,
                               OrderNo = a.OrderNo == 0 ? "" : a.OrderNo.ToString().Substring(0, 4) + "/" + a.OrderNo.ToString().Substring(4),
                               ReqNo = a.ReqNo == 0 ? "" : a.ReqNo.ToString().Substring(0, 4) + "/" + a.ReqNo.ToString().Substring(4),
                               Status = a.Ord_Apr == null ? "" : a.Ord_Apr == 'A' ? "Approved" : a.Ord_Apr == 'P' ? "Pending" : "Cancelled",
                               ReqStatus = a.Req_Apr == null ? "" : a.Req_Apr == "A" ? "Confirmed" : a.Req_Apr == "P" ? "In Progress" : "Cancelled",
                               a.Remarks,
                               SalesPerson = c.SalesPerson + " / " + c.Designation,
                               b.gl_dsc
                           }).OrderBy(o => o.OrderNo).ToList();
                }
                return obj;
            }
            catch (Exception exx)
            {
                return exx.Message;
            }
        }

        public object SrchDocByNo(string docno, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                var obj = from a in Data.tblSdSaleReqs
                          where a.vr_no.ToString().StartsWith(docno.Replace("/", "")) && a.vr_apr == "A" && a.Status.ToLower().Equals("op")
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

        public tblSdSaleReq GetByIDReq(int vrid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                tblSdSaleReq req = Data.tblSdSaleReqs.Where(p => p.vr_id == vrid).Single();

                return req;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public tblSaleOrder GetByID(int orderid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                return Data.tblSaleOrders.Single(p => p.OrderID == orderid);
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public List<tblSaleOrderDet> GetDetailByID(int orderid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                return Data.tblSaleOrderDets.Where(p => p.OrderID == orderid).ToList();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public string Save(tblSaleOrder slOrd, EntitySet<tblSaleOrderDet> slOrdDet, RMSDataContext Data)
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
                Data.tblSaleOrders.InsertOnSubmit(slOrd);
                Data.tblSaleOrderDets.InsertAllOnSubmit(slOrdDet);
                Data.SubmitChanges();

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

        public string Update(tblSaleOrder slOrd, EntitySet<tblSaleOrderDet> slOrdDet, RMSDataContext Data)
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
                IQueryable<tblSaleOrderDet> orddet = Data.tblSaleOrderDets.Where(sl => sl.OrderID == slOrd.OrderID).AsQueryable();
                Data.tblSaleOrderDets.DeleteAllOnSubmit(orddet);
                Data.SubmitChanges();

                Data.tblSaleOrderDets.InsertAllOnSubmit(slOrdDet);
                Data.SubmitChanges();

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

        public string GetDocNo(DateTime dtTime, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                List<int> lst = new List<int>();
                FIN_PERD fin = (from a in Data.FIN_PERDs
                                where a.Start_Date <= dtTime && a.End_Date >= dtTime
                                select a).Single();
                string finYear = fin.Gl_Year.ToString();



                var records = from n in Data.tblSaleOrders
                              select n;
                string year = "";
                foreach (var rec in records)
                {
                    if (rec.OrderNo != 0)
                        year = rec.OrderNo.ToString().Substring(0, 4);
                    else
                        year = "";
                    if (year == finYear)
                    {
                        lst.Add(Convert.ToInt32(rec.OrderNo.ToString().Substring(4)));
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

        public string GetReqNo(DateTime dtTime, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                List<int> lst = new List<int>();
                FIN_PERD fin = (from a in Data.FIN_PERDs
                                where a.Start_Date <= dtTime && a.End_Date >= dtTime
                                select a).Single();
                string finYear = fin.Gl_Year.ToString();



                var records = from n in Data.tblSaleOrders
                              select n;
                string year = "";
                foreach (var rec in records)
                {
                    if (rec.ReqNo != 0)
                        year = rec.ReqNo.ToString().Substring(0, 4);
                    else
                        year = "";
                    if (year == finYear)
                    {
                        lst.Add(Convert.ToInt32(rec.ReqNo.ToString().Substring(4)));
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

        public int GetOrderIDByBranch(int brid, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                return Data.tblSaleOrders.Where(ordr => ordr.br_id == brid).Max(orderid => orderid.OrderID) + 1;
            }
            catch //(Exception ex)
            {
                //RMSDB.SetNull();
                //throw ex;
            }
            return 1;
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
                                 gstid = Data.tblItem_Codes.Where(itm=> itm.itm_cd == itemid).Single().TaxID
                                
                             }).ToList();


                return heads;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }

        }

        public object wmGetSalesPerson(string salepersoninfo, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                var heads = (from a in Data.tblSalesPersons
                             where a.SalesPerson.StartsWith(salepersoninfo) || a.Designation.Contains(salepersoninfo)
                             select new
                             {
                                 a.ID,
                                 info = a.SalesPerson + " / " + a.Designation
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
