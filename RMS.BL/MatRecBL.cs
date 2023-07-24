using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Linq;

namespace RMS.BL
{
    public class MatRecBL
    {
        public MatRecBL()
        { }

        public Object GetAllMatRecs(string docNo, string igpNo, string vendorId, string status, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            var stkdataList = Data.Sp_Filter_MatReceiving(docNo, igpNo, vendorId, status).ToList();
                            //from stkD in Data.tblStkDatas
                            //join vend in Data.Glmf_Codes on po.VendorId equals vend.gl_cd 
                            //join c in Data.tblCities on po.VendorCity equals c.CityID.ToString()
                            //where stkD.vt_cd == 16// && po.LocId == 5
                            //orderby stkD.vr_no descending
                            //select new
                            //{
                            //    stkD.vr_no,
                            //    stkD.DocRef,
                            //    stkD.vr_dt, //= po.vr_dt.ToString("dd-MMM-yy"),
                            //    stkD.vr_apr ,//= po.vr_apr.Equals("P") ? "Pending" : "Approved",
                            //    stkD.LocId,
                            //    stkD.vt_cd,
                            //    stkD.br_id,
                            //    //po.VehicleNo,
                            //    //po.VendorCity,
                            //    //po.VendorId,
                            //    //vend.gl_dsc,
                            //    //c.CityName,
                            //    stkD.vr_id,
                            //    stkD.IGPNo

                            //};

            return stkdataList;
        }
        public string GetPartyName(string vendId, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                return Data.Glmf_Codes.Single(p => p.gl_cd.Equals(vendId)).gl_dsc;
            }
            catch
            {
                return vendId;
            }
        }
        public Object GetAllMatRecsSrch4IGP(string igp,RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            List<tblStkData> stkdatalist = (from stkD in Data.tblStkDatas
                                            join d in Data.tblStock_Locs
                                            on stkD.LocId equals d.LocId
                                            where stkD.br_id == 1 && stkD.vt_cd == 16 && stkD.vr_apr != "C"
                                            && !d.LocCategory.Equals("R")//stkD.LocId == 5 
                                            select stkD).ToList();

            List<Anonymous4MatRec> stkgpList = (from stkgps in Data.tblStkGPs
                                                join vend in Data.Glmf_Codes on stkgps.VendorId equals vend.gl_cd
                                                join c in Data.tblCities on stkgps.VendorCity equals c.CityID.ToString()
                                                where stkgps.vt_cd == 12 && stkgps.vr_no.ToString().Contains(igp) && stkgps.vr_apr.ToString().ToUpper().Equals("A")
                                                //&& !(stkdatalist.Any(p => p.IGPNo == stkgps.vr_no))
                                                orderby stkgps.vr_no descending
                                                select new Anonymous4MatRec
                                                {
                                                    vr_no = stkgps.vr_no,
                                                    DocRef = stkgps.DocRef,
                                                    vr_dt = stkgps.vr_dt, //= po.vr_dt.ToString("dd-MMM-yy"),
                                                    vr_apr = stkgps.vr_apr.ToString(),//= po.vr_apr.Equals("P") ? "Pending" : "Approved",
                                                    LocId = stkgps.LocId,
                                                    vt_cd = stkgps.vt_cd,
                                                    br_id = stkgps.br_id,
                                                    gl_dsc = vend.gl_dsc,
                                                    LocName = stkgps.LocId > 0 ? Data.tblStock_Locs.Where(lc=> lc.LocId.Equals(stkgps.LocId)).SingleOrDefault().LocName : ""
                                                }).ToList();


            var finalFilteredList = from fin in stkgpList
                                    where !(stkdatalist.Any(p => p.IGPNo.Value == fin.vr_no))
                                    orderby fin.LocName, fin.vr_no descending
                                    select fin;

            return finalFilteredList;
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
            return Data.tblItem_Codes.Where(p => p.ct_id.ToString().ToUpper().Equals("D")).ToList();
        }
        public List<Item_Uom> GetAllUOM(RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Data.Item_Uoms.ToList();
        }

        public string GetVendorNmeFromId(string vid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            return Data.Glmf_Codes.Where(p => p.gl_cd.Equals(vid)).Single().gl_dsc;
        }
        public string GetVendorNmeFromIGP(int igpno, int locid,int vt_cd,int brId, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            string vid = Data.tblStkGPs.Where(p => p.vr_no == igpno && p.LocId == locid && p.vt_cd == vt_cd && p.br_id == brId).Single().VendorId;

            return Data.Glmf_Codes.Where(p => p.gl_cd.Equals(vid)).Single().gl_dsc;
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
        public tblStkGP GetByID4StkGP(int locid, int brid, int vrno, int vtcd, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                tblStkGP stk = Data.tblStkGPs.Single(p => p.LocId == locid && p.br_id == brid && p.vr_no == vrno && p.vt_cd == vtcd);

                return stk;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public List<spMatRecResult> GetMaterialRecieveRecs(int vrid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                return Data.spMatRec(vrid).ToList();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public string GetItemNameFromCode(string itemcode, RMSDataContext Data)
        {
            return Data.tblItem_Codes.Single(p => p.itm_cd.Equals(itemcode)).itm_dsc;
        }
        public string GetUomCodeFromUomId(string uomid, RMSDataContext Data)
        {
            return Data.Item_Uoms.Single(p => p.uom_cd.Equals(uomid)).uom_dsc;
        }
        public string SaveMatRecFull(tblStkData stkData, RMSDataContext Data)
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
                Data.tblStkDatas.InsertOnSubmit(stkData);
                Data.SubmitChanges();

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
        public string UpdMatRecFull(tblStkData stkData, EntitySet<tblStkDataDet> stkDataDets, RMSDataContext Data)
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
                var dets = from p in Data.tblStkDataDets
                           where p.vr_id == stkData.vr_id
                           select p;

                Data.tblStkDataDets.DeleteAllOnSubmit(dets);

                stkData.tblStkDataDets = stkDataDets;

                Data.SubmitChanges();
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
        public string GetDocNo(DateTime dtTime, int code, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                List<int> lst = new List<int>();
                FIN_PERD fin = (from a in Data.FIN_PERDs
                                where a.Start_Date <= dtTime && a.End_Date >= dtTime
                                select a).Single();
                string finYear = fin.Gl_Year.ToString();



                var records = from n in Data.tblStkDatas
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
            catch 
            {
                //RMSDB.closeConn(Data);
                //throw ex;
            }
            return null;
        }

        public int checkMRN(int vrid, int brid, int vt_cd, int locid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                var obj = (from a in Data.tblStkDatas
                           join b in Data.tblStkDataDets
                           on a.vr_id equals b.vr_id
                           where a.vt_cd == 21 &&
                           a.vr_apr != "C" &&
                           b.matrec_id == vrid
                           select new
                           {
                            a.vr_no
                           }).ToList();
                if (obj.Count > 0)
                    return obj.First().vr_no;
                else
                    return 0;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string cancelMRN(int vrid, RMSDataContext Data)
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
                tblStk stk = null;
                tblStkData obj = (from a in Data.tblStkDatas
                                  where a.vr_id == vrid

                                  select a).Single();
                obj.vr_apr = "C";
                Data.SubmitChanges();
                foreach (var d in obj.tblStkDataDets.ToList())
                {
                    stk = Data.tblStks.Where(s => s.br_id == obj.br_id && s.LocId == obj.LocId && s.itm_cd == d.itm_cd).Single();

                    stk.itm_pur_qty = stk.itm_pur_qty - (d.vr_qty - d.vr_qty_Rej - d.vr_qty_shrt);
                    stk.itm_pur_val = stk.itm_pur_val - d.vr_val;
                    Data.SubmitChanges();
                }

                /*COMMIT*/
                trans.Commit();

                return "OK";
            }
            catch (Exception exx)
            {
                /*ROLL BACK*/
                if (trans != null)
                    trans.Rollback();

                return exx.Message;
            }
        }
    }
}
