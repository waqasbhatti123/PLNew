using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Linq;

namespace RMS.BL
{
    public class POrderBL
    {
        public POrderBL()
        { }

        public bool IsItemExistsInPo(int brid, int pono, string itmcd, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                var obj = (from a in Data.tblPOrders
                           join b in Data.tblPOrderDets
                           on a.vr_id equals b.vr_id
                           where a.br_id == brid && a.vr_no == pono && b.itm_cd == itmcd
                           select b).ToList();
                if (obj.Count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
           
        }

        public string GetPOType(int brid, int pono, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            
            try
            {
                List<tblPOrder> lstOrder = Data.tblPOrders.Where(ord => ord.br_id == brid && ord.vr_no == pono && ord.vr_apr == "A").ToList();
                int maxRev = lstOrder.Max(x => x.RevSeq.Value);

               return Data.tblPOrders.Where(po => po.br_id == brid && po.vr_no == pono && po.vr_apr == "A" && po.RevSeq == maxRev).SingleOrDefault().PO_Type;
            }
            catch //(Exception ex)
            {
                //RMSDB.closeConn(Data); throw ex;
            }
            return null;
        }

        public tblPOrder GetPORec(int brid, int pono, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                return Data.tblPOrders.Where(po => po.br_id == brid && po.vr_no == pono && po.vr_apr == "A").SingleOrDefault();
            }
            catch //(Exception ex)
            {
                //RMSDB.closeConn(Data); throw ex;
            }
            return null;
        }

        public decimal GetPOItemRate(int brid, int pono, string itmcd, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                int vrid = Data.tblPOrders.Where(po => po.br_id == brid && po.vr_no == pono && po.vr_apr == "A").SingleOrDefault().vr_id;
                return Data.tblPOrderDets.Where(pod => pod.vr_id == vrid && pod.itm_cd == itmcd).SingleOrDefault().vr_Rate;
            }
            catch //(Exception ex)
            {
                //RMSDB.closeConn(Data); throw ex;
            }
            return 0;
        }

        public decimal GetPOItemDiscount(int brid, int pono, string itmcd, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                int vrid = Data.tblPOrders.Where(po => po.br_id == brid && po.vr_no == pono && po.vr_apr == "A").SingleOrDefault().vr_id;
                return Convert.ToDecimal(Data.tblPOrderDets.Where(pod => pod.vr_id == vrid && pod.itm_cd == itmcd).SingleOrDefault().overall_disc);
            }
            catch //(Exception ex)
            {
                //RMSDB.closeConn(Data); throw ex;
            }
            return 0;
        }


        public string GetPoNo(DateTime dtTime, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            
            try
            {
                List<int> lst = new List<int>();
                FIN_PERD fin = (from a in Data.FIN_PERDs
                                where a.Start_Date <= dtTime && a.End_Date >= dtTime
                                select a).Single();
                string finYear = fin.Gl_Year.ToString();



                var records = from n in Data.tblPOrders
                              //where n.LocId == 5
                              select n;
                foreach (var rec in records)
                {
                    string year = rec.vr_no.ToString().Substring(0, 4);
                    if (year == finYear)// && rec.vt_cd == code)
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
        public Object GetPOrders(int brid, string poNo, string partyNme, string status, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            var purchList = //Data.Sp_Filter_Purchase_Req(poNo, partyNme, deptId, status).ToList();
                            from po in Data.tblPOrders
                            join d in Data.Glmf_Codes on po.VendorId equals d.gl_cd
                            where (!status.Equals("0") ? po.vr_apr.ToString().Equals(status) : po.vr_apr.Equals(po.vr_apr.ToString())) &&
                                  (!partyNme.Equals("") ? d.gl_dsc.ToString().Contains(partyNme) : d.gl_dsc.Equals(d.gl_dsc)) &&
                                  (!poNo.Equals("") ? po.vr_no.ToString().Equals(poNo) : po.vr_no.Equals(po.vr_no)) &&
                                  po.br_id == brid
                            orderby Convert.ToInt32(po.vr_no.ToString().Substring(0, 4)) descending,
                            Convert.ToInt32(po.vr_no.ToString().Substring(4)) descending
                            select new
                            {
                                po.vr_no,
                                po.DocRef,
                                po.vr_dt,
                                po.vr_apr,
                                VendorNme = d.gl_dsc,
                                po.CC_cd,
                                po.vr_id,
                                po.PO_Type,
                                RevSeq = po.RevSeq == 0 ? "" : po.RevSeq.Value.ToString(),
                            };

            return purchList;
        }
        public Object GetPORevOrders(int brid, string poNo, string partyNme, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            var purchList = //Data.Sp_Filter_Purchase_Req(poNo, partyNme, deptId, status).ToList();
                            from po in Data.tblPOrders
                            join d in Data.Glmf_Codes on po.VendorId equals d.gl_cd
                            where po.vr_apr.ToString() == "A" && po.Status.ToLower() == "op" &&
                                  (!partyNme.Equals("") ? d.gl_dsc.ToString().Contains(partyNme) : d.gl_dsc.Equals(d.gl_dsc)) &&
                                  (!poNo.Equals("") ? po.vr_no.ToString().Equals(poNo) : po.vr_no.Equals(po.vr_no)) &&
                                  po.br_id == brid
                            orderby po.vr_no descending
                            select new
                            {
                                po.vr_no,
                                po.DocRef,
                                po.vr_dt,
                                po.vr_apr,
                                VendorNme = d.gl_dsc,
                                po.CC_cd,
                                po.vr_id,
                                RevSeq = po.RevSeq == 0 ? "" : po.RevSeq.Value.ToString(),
                            };

            return purchList;
        }

        public Object GetPOsToClose(int brid, string poNo, string partyNme, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            if (string.IsNullOrEmpty(poNo))
                poNo = "0";

            return Data.spPOsToClose(brid, Convert.ToInt32(poNo), partyNme).ToList();
        }

        public List<Cost_Center> GetAllCostCenter(RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Data.Cost_Centers.ToList();
        }
        public byte GetItemUOM(string item, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Convert.ToByte(Data.tblItem_Codes.Where(p => p.itm_cd.Equals(item)).Single().uom_cd.Value);
        }
        //public List<Glmf_Code> GetVendor(RMSDataContext Data)
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
        public List<tblItem_Code> GetAllItem(RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Data.tblItem_Codes.Where(i => !i.itm_cd.StartsWith("1") && i.ct_id == "D").OrderBy(i => i.itm_dsc).ToList();
        }

        public List<Item_Uom> GetUOM(RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Data.Item_Uoms.ToList();
        }
       
        public tblPOrder GetByID(int vrid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Data.tblPOrders.Where(p => p.vr_id == vrid).Single();
        }

        public decimal GetOverAllDiscount(int vrid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }


            return Data.tblPOrderDets.Where(det => det.vr_id == vrid).Sum(sm => sm.overall_disc).Value;
        }


        public decimal GetPODiscountOnItem(int brid, int pono, string itmcd,  RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                int vrid = Data.tblPOrders.Where(po => po.br_id == brid && po.vr_no == pono).SingleOrDefault().vr_id;
                decimal discount = Convert.ToDecimal((from a in Data.tblPOrders
                                    join b in Data.tblPOrderDets
                                    on a.vr_id equals b.vr_id
                                    where a.vr_id == vrid && b.vr_id == vrid && b.itm_cd == itmcd
                                    select b).Single().overall_disc);
                return discount;
            }
            catch { }
            return 0;
        }

        public object GetPoDetailsByID(int vrid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                var pod = (from a in Data.tblPOrders
                           join b in Data.tblPOrderDets
                           on a.vr_id equals b.vr_id
                           join c in Data.Item_Uoms
                           on b.vr_uom equals c.uom_cd
                           where a.vr_id == vrid && b.vr_id == vrid
                           select new
                           {
                               a.vr_id,
                               b.itm_cd,
                               itm_dsc = Data.tblItem_Codes.Where(itm => itm.itm_cd == b.itm_cd).Single().itm_dsc,
                               c.uom_dsc,
                               b.vr_qty,
                               b.overall_disc,
                               rec_qty = ((from k in Data.tblStkGPDets
                                           where k.br_id == a.br_id &&
                                                  k.Itm_cd == b.itm_cd &&
                                                  k.PO_Ref == a.vr_no &&
                                                  k.vt_cd == 12
                                           select k).Sum(x => x.vr_qty) == null ? 0 : (from k in Data.tblStkGPDets
                                                                                       where k.br_id == a.br_id &&
                                                                                              k.Itm_cd == b.itm_cd &&
                                                                                              k.PO_Ref == a.vr_no &&
                                                                                              k.vt_cd == 12
                                                                                       select k).Sum(x => x.vr_qty)),
                               balance = b.vr_qty - ((from k in Data.tblStkGPDets
                                                      where k.br_id == a.br_id &&
                                                             k.Itm_cd == b.itm_cd &&
                                                             k.PO_Ref == a.vr_no &&
                                                             k.vt_cd == 12
                                                      select k).Sum(x => x.vr_qty) == null ? 0 : (from k in Data.tblStkGPDets
                                                                                                  where k.br_id == a.br_id &&
                                                                                                         k.Itm_cd == b.itm_cd &&
                                                                                                         k.PO_Ref == a.vr_no &&
                                                                                                         k.vt_cd == 12
                                                                                                  select k).Sum(x => x.vr_qty)),


                           }).ToList();
                return pod;
            }
            catch { }
            return null;
        }

        public int GetRevSeq(int vrno, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Data.tblPOrders.Where(p => p.vr_no == vrno).Max(x=> x.RevSeq.Value) + 1;
        }

        public string GetUserNameByID(int userid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Data.tblAppUsers.Where(p => p.UserID == userid).Single().UserName;
        }

        public decimal GetPOQty(string itmcd, int vrno, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            decimal qty = 0;
            try
            {
                qty = (from a in Data.tblPOReqs
                       join b in Data.tblPOReqDets
                       on a.vr_id equals b.vr_id
                       where a.vr_no == vrno && b.itm_cd == itmcd
                       select b.vr_qty_req).Max();
            }
            catch
            {
                qty = 0;
            }
            return qty;
        }

        public tblPOrder GetByVrNo(int vrno, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                return Data.tblPOrders.Where(p => p.vr_no == vrno).Single();
            }
            catch
            {
            }
            return null;
        }

        public tblPOrder GetByVrID(int vrid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                return Data.tblPOrders.Where(p => p.vr_id== vrid).Single();
            }
            catch
            {
            }
            return null;
        }


        public List<spPORptResult> GetPORptRes(int vrId, char status, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                return Data.spPORpt(vrId, status.ToString()).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Anonymous4PO> GetPORec(int vrid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                var obj = (from a in Data.tblPOrders
                           join b in Data.Glmf_Codes
                           on a.VendorId equals b.gl_cd
                           join c in Data.glmf_ven_cus_dets
                           on b.gl_cd equals c.gl_cd into leftjoin
                           from x in leftjoin.DefaultIfEmpty()
                           where a.vr_id == vrid 
                           select new Anonymous4PO
                           {
                               vr_id = a.vr_id.ToString(),
                               vr_no = a.vr_no.ToString().Substring(0, 4) + "/" + a.vr_no.ToString().Substring(4)
                                       + " " + (a.RevSeq.Value == 0 ? "" : "Rev-" + a.RevSeq.Value.ToString())
                               ,
                               vr_dt = a.vr_dt.ToString(),
                               vendor = b.gl_dsc,
                               vendordocref = a.DocRef,
                               docrefdate = a.DocRefDate == null ? "1-1-1990" : a.DocRefDate.Value.ToString(),
                               supplytype = a.PO_Nature == "G" ? "Goods" : "Services",
                               currency = a.CRUNCY,
                               Del_Loc = a.Del_Loc,
                               Del_Days = a.Del_Days.ToString(),
                               Pay_Days = a.Pay_Days.Value.ToString(),
                               QTY_Var_Pc = a.QTY_Var_Pc.Value.ToString(),
                               Pay_Terms = a.Pay_Terms,
                               PO_Instructions = a.PO_Instructions,
                               vr_nrtn = a.vr_nrtn,
                               potype = a.PO_Type == "L" ? "Local" : "Foreign",
                               status = a.vr_apr == "A" ? "Approved" : a.vr_apr == "P" ? "Pending" : a.vr_apr == "C" ? "Cancelled" : "Null",

                               venNTN = x.NTN,
                               venAdd = x.address,
                               venTel = x.tel,
                               venCont = x.Cont_Person,
                               venCell = x.cell_no,
                               venEmail = x.email,
                               venCity = x.city,
                               venFax = x.fax_no,
                               venSTR = x.STN,

                               CreatedBy = a.CreatedBy,
                               CreatedOn = a.CreatedOn.ToString(),
                               ApprovedBy = a.ApprovedBy,
                               ApprovedOn = a.ApprovedOn.Value.ToString()

                           }).ToList();
                return obj;
            }
            catch
            {
                return null;
            }
        }

        public string SavePurchReqFull(tblPOrder poReq, RMSDataContext Data)
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
                Data.tblPOrders.InsertOnSubmit(poReq);
                Data.SubmitChanges();

                if (poReq.vr_apr == "C")
                {
                    List<tblPOrder> porder = Data.tblPOrders.Where(po => po.vr_no == poReq.vr_no).ToList();
                    foreach (tblPOrder po in porder)
                    {
                        po.vr_apr = "C";
                    }
                    Data.SubmitChanges();
                }


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


        public bool CanRevisionBeDone(int brid, int pono, string itmToChk, decimal qtyToChk, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                decimal gpQty = 0;
                try
                {
                    //gpQty = Data.tblStkGPDets.Where(gp => gp.br_id == brid &&
                    //                                             gp.PO_Ref == pono &&
                    //                                             gp.Itm_cd == itmToChk &&
                    //                                             gp.vt_cd == 12).Sum(gp => gp.vr_qty);

                    gpQty = (from a in Data.tblStkGPs
                             join b in Data.tblStkGPDets
                             on new { a.br_id, a.LocId, a.vt_cd, a.vr_no } equals new { b.br_id, b.LocId, b.vt_cd, b.vr_no }
                             where  a.br_id == brid && 
                                    b.br_id == brid && 
                                    b.PO_Ref == pono && 
                                    b.Itm_cd == itmToChk && 
                                    a.vt_cd == 12 && 
                                    b.vt_cd == 12 &&
                                    a.vr_apr != "C"
                             select b).Sum(x => x.vr_qty);
                }
                catch { }
                if (qtyToChk < gpQty)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CanPOBeCancelled(int brid, int pono, string itmToChk, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                decimal gpQty = 0;
                try
                {
                    //gpQty = Data.tblStkGPDets.Where(gp => gp.br_id == brid &&
                    //                                             gp.PO_Ref == pono &&
                    //                                             gp.Itm_cd == itmToChk &&
                    //                                             gp.vt_cd == 12).Sum(gp => gp.vr_qty);
                    gpQty = (from a in Data.tblStkGPs
                             join b in Data.tblStkGPDets
                             on new { a.br_id, a.LocId, a.vt_cd, a.vr_no } equals new { b.br_id, b.LocId, b.vt_cd, b.vr_no }
                             where a.br_id == brid &&
                                    b.br_id == brid &&
                                    b.PO_Ref == pono &&
                                    b.Itm_cd == itmToChk &&
                                    a.vt_cd == 12 &&
                                    b.vt_cd == 12 &&
                                    a.vr_apr != "C"
                             select b).Sum(x => x.vr_qty);
                }
                catch { }
                if (gpQty > 0)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public string SavePurchRevFull(tblPOrder prevOrder, tblPOrder poReq, RMSDataContext Data)
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
                Data.tblPOrders.InsertOnSubmit(poReq);
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



        public string UpdPurchReqFull(tblPOrder po, EntitySet<tblPOrderDet> poDets, RMSDataContext Data)
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
                var dets = from p in Data.tblPOrderDets
                           where p.vr_id == po.vr_id
                           select p;

                Data.tblPOrderDets.DeleteAllOnSubmit(dets);

                po.tblPOrderDets = poDets;
                Data.SubmitChanges();

                if (po.vr_apr == "C")
                {
                    List<tblPOrder> porder = Data.tblPOrders.Where(ord => ord.vr_no == po.vr_no).ToList();
                    foreach (tblPOrder ord in porder)
                    {
                        ord.vr_apr = "C";
                    }
                    Data.SubmitChanges();
                }

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


        public string Update(tblPOrder po, RMSDataContext Data)
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


        public List<spSrchPOReqResult> AutoCompletSrchPOReq(string txt)
        {
            RMSDataContext Data = new RMSDataContext();
            try
            {
                return Data.spSrchPOReq().Where(m => m.DeptNme.ToLower().Contains(txt) || m.cc_nme.ToLower().Contains(txt) || m.DocRef.ToLower().Contains(txt) || m.vr_no_formated.Contains(txt) || m.vr_no.ToString().Contains(txt)).ToList();
                                             
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        public List<spSrchPOResult> AutoCompletSrchPO(string txt,  string contextKey)
        {
            RMSDataContext Data = new RMSDataContext();
            try
            {
                List<spSrchPOResult> lst = Data.spSrchPO(0,contextKey).Where(m => m.gl_dsc.ToLower().Contains(txt) || m.vr_no_formated.Contains(txt) || m.vr_no.ToString().Contains(txt)).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public List<tblPOReqDet> GetRecDetById(int id, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                return Data.tblPOReqDets.Where(m => m.vr_id == id).ToList();
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }



        public tblPOReq GetPOReq(int id, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                return Data.tblPOReqs.Where(m => m.vr_id == id).Single();
            }
            catch //(Exception ex)
            {
                //RMSDB.closeConn(Data);
               //throw ex;
            }
            return null;
        }



        //public int GetVoucherNo(RMSDataContext Data,int vouchtypid,decimal finanyear)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = RMSDB.GetOject(); }
        //        int vouhno = Data.Glmf_Datas.Where(t => t.vt_cd.Equals(vouchtypid) && t.br_id == 1 && t.Gl_Year == finanyear).Max(g => g.vr_no) + 1;

        //        return vouhno;
        //    }
        //    catch { }
        //    return 1;
        //}
        //public Glmf_Data_chq GetCheqDetByID(int vrId, RMSDataContext Data)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = RMSDB.GetOject(); }
        //        Glmf_Data_chq glmfChq = Data.Glmf_Data_chqs.Single(t => t.vrid == vrId);

        //        return glmfChq;
        //    }
        //    catch 
        //    {
        //        return null;
        //    }
           
        //}

        //public decimal GetFinancialYear(RMSDataContext Data)
        //{
        //    if (Data == null) { Data = RMSDB.GetOject(); }
        //    decimal year = (from t in Data.FIN_PERDs
        //                    where t.Cur_Year.Equals("CUR")
        //                    select t.Gl_Year).Single();
        //    return year;
        //  //  return Data.FIN_PERDs.Where(t => t.Cur_Year.Equals("CUR")).Single().Gl_Year;
        //}

        //public void SaveVoucher(RMSDataContext Data, Glmf_Data gldata, EntitySet<Glmf_Data_Det> gldatadet)
        //{
        //    if (Data == null) { Data = RMSDB.GetOject(); }
        //        Data.Glmf_Datas.InsertOnSubmit(gldata);
        //        gldata.Glmf_Data_Dets = gldatadet;
        //        Data.SubmitChanges();

           
        
        //}
        //public void PostVoucherDet(RMSDataContext Data, Glmf_Data glmfData, EntitySet<Glmf_Data_Det> gldatadet)
        //{
        //    if (Data == null) { Data = RMSDB.GetOject(); }
        //    Glmf glmf = null;
        //    foreach (Glmf_Data_Det glmfDet in gldatadet)
        //    {
        //        glmf = Data.Glmfs.Single(g => g.br_id == glmfData.br_id && g.gl_cd == glmfDet.gl_cd && g.gl_year == glmfData.Gl_Year);

        //        glmf.gl_cr = glmf.gl_cr + glmfDet.vrd_credit;
        //        glmf.gl_db = glmf.gl_db + glmfDet.vrd_debit;
        //        glmf.updateby = glmfData.updateby;
        //        glmf.updateon = Common.MyDate(Data);
        //        Data.SubmitChanges();
        //    }
        //}
        //public void PostVoucherOpeningBalDet(RMSDataContext Data, Glmf_Data glmfData, EntitySet<Glmf_Data_Det> gldatadet)
        //{
        //    if (Data == null) { Data = RMSDB.GetOject(); }
        //    Glmf glmf = null;
        //    foreach (Glmf_Data_Det glmfDet in gldatadet)
        //    {
        //        glmf = Data.Glmfs.Single(g => g.br_id == glmfData.br_id && g.gl_cd == glmfDet.gl_cd && g.gl_year == glmfData.Gl_Year);

        //        glmf.gl_op = (glmf.gl_op + glmfDet.vrd_debit) - glmfDet.vrd_credit;
                
        //        glmf.updateby = glmfData.updateby;
        //        glmf.updateon = Common.MyDate(Data);
        //        Data.SubmitChanges();
        //    }
        //}
        //public void PostVoucher4mHome(RMSDataContext Data, int vrId,string username)
        //{
        //    if (Data == null) { Data = RMSDB.GetOject(); }

        //    Glmf_Data glmfData = Data.Glmf_Datas.Single(g => g.vrid == vrId);

        //    Glmf glmf = null;

        //    if (glmfData.vt_cd == 55)
        //    {
        //        foreach (Glmf_Data_Det glmfDet in glmfData.Glmf_Data_Dets)
        //        {
        //            glmf = Data.Glmfs.Single(g => g.br_id == glmfData.br_id && g.gl_cd == glmfDet.gl_cd && g.gl_year == glmfData.Gl_Year);

        //            glmf.gl_op = (glmf.gl_op + glmfDet.vrd_debit )- glmfDet.vrd_credit;
                    
        //            glmf.updateby = username;
        //            glmf.updateon = Common.MyDate(Data);
                    
        //            Data.SubmitChanges();
        //        }
        //    }
        //    else
        //    {
        //        foreach (Glmf_Data_Det glmfDet in glmfData.Glmf_Data_Dets)
        //        {
        //            glmf = Data.Glmfs.Single(g => g.br_id == glmfData.br_id && g.gl_cd == glmfDet.gl_cd && g.gl_year == glmfData.Gl_Year);

        //            glmf.gl_cr = glmf.gl_cr + glmfDet.vrd_credit;
        //            glmf.gl_db = glmf.gl_db + glmfDet.vrd_debit;
        //            glmf.updateby = username;
        //            glmf.updateon = Common.MyDate(Data);
        //            Data.SubmitChanges();
        //        }
        //    }
        //}
        //public void SaveVoucherCheqDet(RMSDataContext Data, int vrId, Glmf_Data_chq glmfChq)
        //{
        //    if (Data == null) { Data = RMSDB.GetOject(); }
        //        glmfChq.vrid = vrId;
        //        Data.Glmf_Data_chqs.InsertOnSubmit(glmfChq);
        //        Data.SubmitChanges();
           
        //}
        //public string VoucherDesc(int vrTypeID, RMSDataContext Data)
        //{
        //    if (Data == null) { Data = RMSDB.GetOject(); }
        //    int vrType = Convert.ToInt16(vrTypeID);
        //    string str = (from s in Data.Vr_Types
        //                  where s.vt_cd == vrType
        //                  select s).Single().vt_dsc.ToString();
        //    return str;
        //}
        //public void UpdateVoucher(RMSDataContext Data, Glmf_Data gldata, EntitySet<Glmf_Data_Det> gldatadet, int vrID, int vrNo)
        //{
        //    if (Data == null) { Data = RMSDB.GetOject(); }
        //    var glmfDetData = from t in Data.Glmf_Data_Dets
        //                    where t.vrid == vrID
        //                    select t;
        //    foreach (var det in glmfDetData)
        //    {
        //        Data.Glmf_Data_Dets.DeleteOnSubmit(det);
        //    }

        //    // ALSO DELETE CHEQUE DELETES B4 GLMF_DATA
        //    var chqData = from t in Data.Glmf_Data_chqs
        //                    where t.vrid == vrID
        //                    select t;
        //    foreach (var det in chqData)
        //    {
        //        Data.Glmf_Data_chqs.DeleteOnSubmit(det);
        //    }

        //    var glmfData = from t in Data.Glmf_Datas
        //                    where t.vrid == vrID
        //                    select t;
        //    Data.Glmf_Datas.DeleteAllOnSubmit(glmfData);
        //    Data.SubmitChanges();

        //    Data.Glmf_Datas.InsertOnSubmit(gldata);
        //    gldata.Glmf_Data_Dets = gldatadet;
        //    Data.SubmitChanges();
                
        //}
        ////public void UpdateVoucherCheqDet(RMSDataContext Data, int prevVrID, int newVrID, Glmf_Data_chq glmfChq)
        ////{
        ////    var dataToDel = from t in Data.Glmf_Data_chqs
        ////                    where t.vrid == prevVrID
        ////                    select t;
        ////    foreach (var det in dataToDel)
        ////    {
        ////        Data.Glmf_Data_chqs.DeleteOnSubmit(det);
        ////    }

        ////    try
        ////    {
        ////        glmfChq.vrid = newVrID;
        ////        Data.Glmf_Data_chqs.InsertOnSubmit(glmfChq);
        ////        Data.SubmitChanges();
        ////    }
        ////    catch { }

        ////}
    }
}
