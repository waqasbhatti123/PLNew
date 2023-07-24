using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Linq;

namespace RMS.BL
{
    public class IGPBL
    {
        public IGPBL()
        {}

        static public bool IsEditable;
        static public tblStkGP stkGatePass;

        public object VariantData(string poref, string seqno, string qty, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                tblPOrder order = Data.tblPOrders.Where(o => o.vr_no.ToString().Equals(poref) && o.Status.ToLower() == "op").SingleOrDefault();
                tblPOrderDet orderdetail = order.tblPOrderDets.Where(det => det.vr_seq == Convert.ToByte(seqno)).SingleOrDefault();
                tblStkGPDet detgp = null;
                if(stkGatePass != null)
                detgp = stkGatePass.tblStkGPDets.Where(gp => gp.Itm_cd == orderdetail.itm_cd).SingleOrDefault();
                if (order != null)
                {
                    if (order.QTY_Var_Pc > 0)
                    {
                        var values = (from porder in Data.spSrchPO(order.vr_id, order.VendorId)
                                      where porder.vr_seq == Convert.ToByte(seqno)
                                      select new
                                      {
                                          percentageexist = order.QTY_Var_Pc > 0 ? true : false,
                                          max = Math.Round((IsEditable == true ? detgp.vr_qty : 0) +
                                                                Convert.ToDecimal(porder.rem_qty) + Convert.ToDecimal(porder.rem_qty) * Convert.ToDecimal(order.QTY_Var_Pc) * Convert.ToDecimal(.01), 3, MidpointRounding.AwayFromZero),
                                          min = Convert.ToDecimal(0),
                                          flag = Convert.ToDecimal(qty) > 0
                                                 &&
                                                 Convert.ToDecimal(qty) <= Math.Round((IsEditable == true ? detgp.vr_qty : 0) +
                                                                                        Convert.ToDecimal(porder.rem_qty) + (Convert.ToDecimal(porder.rem_qty) * Convert.ToDecimal(order.QTY_Var_Pc) * Convert.ToDecimal(.01)), 3, MidpointRounding.AwayFromZero)
                                                 ?
                                                 true : false
                                      }).ToList();
                        return values;
                    }
                    else
                    {
                        var values = (from porder in Data.spSrchPO(order.vr_id, order.VendorId)
                                      where porder.vr_seq == Convert.ToByte(seqno)
                                      select new
                                      {
                                          percentageexist = true,
                                          max = Math.Round((IsEditable == true ? detgp.vr_qty : 0) + Convert.ToDecimal(porder.rem_qty), 3, MidpointRounding.AwayFromZero),
                                          min = Convert.ToDecimal(0),
                                          flag = Convert.ToDecimal(qty) <= Math.Round((IsEditable == true ? detgp.vr_qty : 0) + Convert.ToDecimal(porder.rem_qty),3) ? true : false,
                                      }).ToList();

                        if (values.Count == 0 && IsEditable)
                        {
                            decimal PoQty = orderdetail.vr_qty;//Total po qty of item
                            decimal aprIgpQty = (from a in Data.tblStkGPs
                                                 join b in Data.tblStkGPDets
                                                 on new { a.br_id, a.LocId, a.vt_cd, a.vr_no } equals new { b.br_id, b.LocId, b.vt_cd, b.vr_no }
                                                 where a.vt_cd == 12 && b.vt_cd == 12 && a.vr_apr == "A" && b.Itm_cd == orderdetail.itm_cd && b.PO_Ref == order.vr_no
                                                 select b).ToList().Sum(b=> b.vr_qty);

                            decimal penIgpQty = (from a in Data.tblStkGPs
                                                 join b in Data.tblStkGPDets
                                                 on new { a.br_id, a.LocId, a.vt_cd, a.vr_no } equals new { b.br_id, b.LocId, b.vt_cd, b.vr_no }
                                                 where a.vt_cd == 12 && b.vt_cd == 12 && a.vr_apr == "P" && b.Itm_cd == orderdetail.itm_cd && b.PO_Ref == order.vr_no
                                                 select b).ToList().Sum(b => b.vr_qty);
                            decimal curIgpQty = detgp.vr_qty;


                            values = (from porder in Data.tblStkGPs
                                      where porder.vr_no == stkGatePass.vr_no
                                          select new
                                          {
                                              percentageexist = true,
                                              max = Math.Round(PoQty - aprIgpQty - (penIgpQty - curIgpQty),3, MidpointRounding.AwayFromZero),
                                              min = Convert.ToDecimal(0),
                                              flag = Convert.ToDecimal(qty) <= Math.Round(PoQty - aprIgpQty - (penIgpQty - curIgpQty),3) ? true : false,
                                          }).ToList();

                        }
                        return values;
                    }
                }
                else
                    return null;
            }
            catch { }
            return null;
        }

        public Object GetAllChemIGPs(string igpNo, string party, int cityId, string status, int loc_id, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            var purchList = Data.Sp_Filter_ChemIGP(igpNo, party, cityId, status, loc_id).ToList();
                            //from po in Data.tblStkGPs
                            //join vend in Data.Glmf_Codes on po.VendorId equals vend.gl_cd 
                            //join c in Data.tblCities on po.VendorCity equals c.CityID.ToString()
                            //where po.vt_cd == 12 //&& po.LocId == 5
                            //orderby po.vr_no descending
                            //select new
                            //{
                            //    po.vr_no,
                            //    po.DocRef,
                            //    po.vr_dt, //= po.vr_dt.ToString("dd-MMM-yy"),
                            //    po.vr_apr ,//= po.vr_apr.Equals("P") ? "Pending" : "Approved",
                            //    po.LocId,
                            //    po.vt_cd,
                            //    po.br_id,
                            //    po.VehicleNo,
                            //    po.VendorCity,
                            //    po.VendorId,
                            //    vend.gl_dsc,
                            //    c.CityName,
                            //    po.BiltyNo

                            //};

            return purchList;
        }
        //public List<Cost_Center> GetAllCostCenter(RMSDataContext Data)
        //{
        //    if (Data == null) { Data = RMSDB.GetOject(); }
        //    return Data.Cost_Centers.ToList();
        //}

        //public List<Depart_ment> GetAllDepartment(RMSDataContext Data)
        //{
        //    if (Data == null) { Data = RMSDB.GetOject(); }
        //    return Data.Depart_ments.ToList();
        //}
        public List<tblItem_Code> GetAllItem(RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Data.tblItem_Codes.Where(i => !i.itm_cd.StartsWith("1") && i.ct_id == "D").OrderBy(i => i.itm_dsc).ToList();
        }
        public List<Item_Uom> GetAllUOM(RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Data.Item_Uoms.ToList();
        }


        public string GetUOMDescFromID(byte uomid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Data.Item_Uoms.Where(p => p.uom_cd == uomid).Single().uom_dsc;
        }

        public List<spGPResult> GetGPRecs(int brid, int locid, int vtcd, int vrno, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Data.spGP(brid, locid, vtcd, vrno).ToList();
        }
        
        public tblStkGP GetByID(int locid, int brid, int vrno, int vtcd, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                tblStkGP stk = Data.tblStkGPs.Single(p =>p.LocId == locid && p.br_id == brid && p.vr_no == vrno && p.vt_cd == vtcd);

                return stk;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public string checkIGP(int vrno, int brid, int vt_cd, int locid, RMSDataContext Data) 
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                var obj = (from a in Data.tblStkDatas
                           join b in Data.tblStkDataDets
                           on a.vr_id equals b.vr_id
                           where a.vt_cd == 16 &&
                           a.vr_apr != "C" &&
                           a.IGPNo == vrno
                           select a).ToList();
                if (obj.Count > 0)
                    return obj.First().vr_no.ToString();
                else
                    return "OK";
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string cancelIGP(int vrno, int brid, int vt_cd, int locid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                tblStkGP obj = (from a in Data.tblStkGPs
                                where a.vr_no == vrno && a.br_id == brid && a.vt_cd == vt_cd && a.LocId == locid

                                select a).Single();
                obj.vr_apr = "C";
                Data.SubmitChanges();

                return "OK"; 
            }
            catch(Exception e) 
            {
               return e.Message;
            }
            
        }

        //public List<tblPOrderDet> GetPODet(int vrid, RMSDataContext Data)
        //{
        //    if (Data == null) { Data = RMSDB.GetOject(); }
        //    try
        //    {
        //        return Data.tblPOrderDets.Where(m => m.vr_id == vrid).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }
        //}

        public List<spSrchPOResult> GetPODet1(int vrid, string vendorid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                return Data.spSrchPO(vrid, vendorid).Where(m => m.vr_id == vrid).ToList();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public string SaveIGPFull(tblStkGP stkGp, RMSDataContext Data)
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
                Data.tblStkGPs.InsertOnSubmit(stkGp);
                Data.SubmitChanges();
                /*******************************/
                bool closePO = true;
                decimal itmGpQty = 0;
                try
                {
                    int porRef = Convert.ToInt32(stkGp.tblStkGPDets.Where(gp => gp.PO_Ref != 99999).First().PO_Ref);
                    int maxSeq = Convert.ToInt32(Data.tblPOrders.Where(po => po.br_id == stkGp.br_id && po.vr_no == porRef).Max(x => x.RevSeq));
                    if (stkGp.vr_apr != "C")
                    {
                        var listPOItems = (from a in Data.tblPOrders
                                           join b in Data.tblPOrderDets
                                           on a.vr_id equals b.vr_id
                                           where a.br_id == stkGp.br_id && a.vr_no == stkGp.tblStkGPDets.Where(gp => gp.PO_Ref != 99999).First().PO_Ref && a.RevSeq == maxSeq
                                           select new
                                           {
                                               a.br_id,
                                               a.vr_no,
                                               b.itm_cd,
                                               b.vr_qty
                                           }).ToList();

                        foreach (var det in listPOItems)
                        {
                            itmGpQty = 0;
                            try
                            {
                                //itmGpQty = Data.tblStkGPDets.Where(gp => gp.br_id == det.br_id &&
                                //                                                gp.vt_cd == 12 &&
                                //                                                gp.Itm_cd == det.itm_cd &&
                                //                                                gp.PO_Ref == det.vr_no
                                                                                
                                //                                          ).Sum(x => x.vr_qty);

                                itmGpQty = (from a in Data.tblStkGPs
                                            join b in Data.tblStkGPDets
                                            on new { a.br_id, a.LocId, a.vt_cd, a.vr_no } equals new { b.br_id, b.LocId, b.vt_cd, b.vr_no }
                                            where a.vt_cd == 12 && b.vt_cd == 12 && a.vr_apr != "C" && b.Itm_cd == det.itm_cd && b.PO_Ref == det.vr_no &&
                                            a.br_id == det.br_id && b.br_id == det.br_id
                                            select b).ToList().Sum(b => b.vr_qty);
                            }
                            catch { }
                            if (det.vr_qty != itmGpQty)
                            {
                                closePO = false;
                                break;
                            }
                        }
                        if (closePO)
                        {
                            tblPOrder porder = Data.tblPOrders.Where(po => po.br_id == stkGp.br_id && po.vr_no == porRef && po.RevSeq == maxSeq).Single();
                            porder.Status = "Cl";
                            Data.SubmitChanges();
                        }
                    }
                    else
                    {
                        tblPOrder porder = Data.tblPOrders.Where(po => po.br_id == stkGp.br_id && po.vr_no == porRef && po.RevSeq == maxSeq).Single();
                        porder.Status = "Op";
                        Data.SubmitChanges();
                    }
                }
                catch { }
                /*******************************/
                

                /*COMMIT*/
                trans.Commit();

                return "Done";
            }
            catch (Exception e) 
            {
                /*ROLL BACK*/
                if (trans != null)
                    trans.Rollback();

                return e.Message;
            }
        }
        public byte GetItemUOMValue(string item, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Convert.ToByte(Data.tblItem_Codes.Where(p => p.itm_cd.Equals(item)).Single().uom_cd.Value);
        }
        public string GetItemUOMLabel(string item, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            byte uomid = Convert.ToByte(Data.tblItem_Codes.Where(p => p.itm_cd.Equals(item)).Single().uom_cd.Value);
            string uomval = Data.Item_Uoms.Where(p => p.uom_cd.Equals(uomid)).Single().uom_dsc;
            return uomval;

        }
        public byte GetItemUOMFromLabel(string uomdsc, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Convert.ToByte(Data.Item_Uoms.Where(p => p.uom_dsc.Equals(uomdsc)).Single().uom_cd);;
        }
        public string GetItemUOMDesc(byte uomcode, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Data.Item_Uoms.Where(p => p.uom_cd == uomcode).Single().uom_dsc; ;
        }
        public string UpdIGPFull(tblStkGP stkGp, EntitySet<tblStkGPDet> stkGpDets, RMSDataContext Data)
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
                var dets = from p in Data.tblStkGPDets
                                where p.LocId == stkGp.LocId && p.br_id == stkGp.br_id && p.vr_no == stkGp.vr_no && p.vt_cd == stkGp.vt_cd
                                select p;

                Data.tblStkGPDets.DeleteAllOnSubmit(dets);
                stkGp.tblStkGPDets = stkGpDets;
                Data.SubmitChanges();
                /*******************************/
                bool closePO = true;
                decimal itmGpQty = 0;
                try
                {
                    int porRef = Convert.ToInt32(stkGp.tblStkGPDets.Where(gp => gp.PO_Ref != 99999).First().PO_Ref);
                    int maxSeq = Convert.ToInt32(Data.tblPOrders.Where(po => po.br_id == stkGp.br_id && po.vr_no == porRef).Max(x=> x.RevSeq));
                    if (stkGp.vr_apr != "C")
                    {
                        var listPOItems = (from a in Data.tblPOrders
                                           join b in Data.tblPOrderDets
                                           on a.vr_id equals b.vr_id
                                           where a.br_id == stkGp.br_id && a.vr_no == stkGp.tblStkGPDets.Where(gp => gp.PO_Ref != 99999).First().PO_Ref && a.RevSeq == maxSeq
                                           select new
                                           {
                                               a.br_id,
                                               a.vr_no,
                                               b.itm_cd,
                                               b.vr_qty
                                           }).ToList();

                        foreach (var det in listPOItems)
                        {
                            itmGpQty = 0;
                            try
                            {
                                itmGpQty = Data.tblStkGPDets.Where(gp => gp.br_id == det.br_id &&
                                                                                gp.vt_cd == 12 &&
                                                                                gp.Itm_cd == det.itm_cd &&
                                                                                gp.PO_Ref == det.vr_no
                                                                          ).Sum(x => x.vr_qty);
                            }
                            catch { }
                            if (det.vr_qty != itmGpQty)
                            {
                                closePO = false;
                                break;
                            }
                        }
                        if (closePO)
                        {
                            tblPOrder porder = Data.tblPOrders.Where(po => po.br_id == stkGp.br_id && po.vr_no == porRef && po.RevSeq == maxSeq).Single();
                            porder.Status = "Cl";
                            Data.SubmitChanges();
                        }
                    }
                    else
                    {
                        tblPOrder porder = Data.tblPOrders.Where(po => po.br_id == stkGp.br_id && po.vr_no == porRef && po.RevSeq == maxSeq).Single();
                        porder.Status = "Op";
                        Data.SubmitChanges();
                    }
                }
                catch { }
                /*******************************/
  
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
        public List<tblStock_Loc> GetStockLoc1(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var lst = from l in Data.tblStock_Locs
                          where !l.LocCategory.Equals("R") && !l.LocCode.Equals("FG")
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
        public string GetGpNo(DateTime dtTime, int code, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                List<int> lst = new List<int>();
                FIN_PERD fin = (from a in Data.FIN_PERDs
                                where a.Start_Date <= dtTime && a.End_Date >= dtTime
                                select a).Single();
                string finYear = fin.Gl_Year.ToString();



                var records = from n in Data.tblStkGPs
                              //where n.LocId == 5
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
        public string GetDocReference(DateTime dtTime, int code, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                string docRef = "";
                string yr = "";


                FIN_PERD fin = (from a in Data.FIN_PERDs
                                where a.Start_Date <= dtTime && a.End_Date >= dtTime
                                select a).Single();
                yr = fin.Gl_Year.ToString();




                List<int> lst = new List<int>();
                List<tblStkGP> obj = (from r in Data.tblStkGPs
                                      where r.vr_no.ToString().Substring(0, 4) == yr && r.vt_cd == code
                                      //&& r.LocId == 5
                                      select r).ToList();
                if (obj.Count > 0)
                {
                    foreach (var rec in obj)
                    {
                        docRef = rec.DocRef;
                        docRef = docRef.Substring(9);
                        lst.Add(Convert.ToInt32(docRef));
                    }
                    return "Ref-" + yr + "-" + Convert.ToString(lst.Max() + 1);
                }
                else
                {
                    return "Ref-" + yr + "-1";
                }
            }
            catch //(Exception ex)
            {
                //RMSDB.closeConn(Data); throw ex;
            }
            return "";
        }
    }
}
