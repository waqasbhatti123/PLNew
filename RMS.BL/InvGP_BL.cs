using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web;
using System.Data.Linq;
using System.Reflection;
namespace RMS.BL
{
    public class InvGP_BL
    {
        public InvGP_BL()
        {
        }


        public bool GetIfFeetageCardExists(string docRef, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                List<tblItemData> lst = (from a in Data.tblItemDatas
                                         where a.vt_cd == 14 && a.DocRef == docRef && a.vr_apr != "C"
                                         select a).ToList();
                if (lst.Count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }


        public string GetGPQty(string docRef, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                int sum = 0;
                var obj = from a in Data.tblStkGPs
                          join b in Data.tblStkGPDets
                          on a.vr_no equals b.vr_no
                          where a.DocRef == docRef && a.vt_cd == 11
                          select new
                          {
                              b.vr_qty
                          };
                foreach (var c in obj)
                {
                    sum = sum + Convert.ToInt32(c.vr_qty);
                }
                return sum.ToString();
            }
            catch
            {
                return "0";
            }
        }


        public bool EditRec(string docRef, int yr, int code, tblStkGP stkGp, EntitySet<tblStkGPDet> enttyGpDet, RMSDataContext Data)
        {
            try
            {

                //RMSDataContext Data = new RMSDataContext();
                if (Data == null) { Data = RMSDB.GetOject(); }
                //Deleting prvious records
                var recDet = from r in Data.tblStkGPDets
                             join c in Data.tblStkGPs
                             on r.vr_no equals c.vr_no
                             where c.DocRef == docRef && Convert.ToInt32(r.vr_no.ToString().Substring(0, 4)) == yr && r.vt_cd == code
                             select r;
                Data.tblStkGPDets.DeleteAllOnSubmit(recDet);
                Data.SubmitChanges();
                //Deleting parent
                if (Data == null) { Data = RMSDB.GetOject(); }
                var rec1 = from r in Data.tblStkGPs
                           where r.DocRef == docRef && Convert.ToInt32(r.vr_no.ToString().Substring(0, 4)) == yr && r.vt_cd == code
                           select r;
                Data.tblStkGPs.DeleteAllOnSubmit(rec1);
                Data.SubmitChanges();
                ////Inserting new records
                //Data.tblStkGPs.InsertOnSubmit(stkGp);
                //stkGp.tblStkGPDets = enttyGpDet;
                ////Changes submitted
                //Data.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }


        public List<tblStkGPDet> GetRecDet(string docRef, int yr, int code, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            List<tblStkGPDet> recs = (from r in Data.tblStkGPDets
                                      join c in Data.tblStkGPs
                                      on r.vr_no equals c.vr_no
                                      where c.DocRef == docRef && r.vt_cd == code
                                      orderby r.vr_seq
                                      select r).ToList();
            return recs;
        }


        public tblStkGP GetRec(string docRef, int yr, int code, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            tblStkGP rec = (from r in Data.tblStkGPs
                            where r.DocRef == docRef && r.vt_cd == code
                            select r).Take(1).Single();
            return rec;
        }


        public bool ChngedIGPStatus(string docRef, int yr, int code, char sttus, string updBy, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                // RMSDataContext Data = new RMSDataContext();
                List<tblStkGP> rec = (from r in Data.tblStkGPs
                                      where r.DocRef == docRef && r.vt_cd == code
                                      select r).ToList();

                foreach (tblStkGP obj in rec)
                {
                    obj.vr_apr = Convert.ToString(sttus);
                    obj.updateby = updBy;
                    obj.updateon = Common.MyDate(Data);
                    Data.SubmitChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }


        public object GetGrid_IGP(DateTime fromDt, DateTime toDt, char sttus, RMSDataContext Data)
        {
            if (sttus != 'M')
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var grdObject = (from l in Data.tblStkGPs
                                 join g in Data.tblStkGPDets
                                 on l.vr_no equals g.vr_no
                                 join m in Data.Glmf_Codes
                                 on l.VendorId equals m.gl_cd

                                 where l.vr_dt >= fromDt && l.vr_dt <= toDt && l.vr_apr == Convert.ToString(sttus) && l.vt_cd == 11
                                 select new
                                 {
                                     vr_no = (from b in Data.tblStkGPs
                                              where b.DocRef == l.DocRef && b.vt_cd == 11
                                              select b).First().vr_no.ToString().Substring(0, 4)
                                              + "/"
                                              +
                                              (from b in Data.tblStkGPs
                                               where b.DocRef == l.DocRef && b.vt_cd == 11
                                               select b).First().vr_no.ToString().Substring(4),

                                     GPRef = (from e in Data.tblStkGPs
                                              join f in Data.tblStkGPDets
                                              on e.vr_no equals f.vr_no
                                              where e.DocRef == l.DocRef && e.vt_cd == 11 && f.vt_cd == 11
                                              select f).First().GPRef,

                                     gl_dsc = m.gl_dsc,
                                     vr_dt = l.vr_dt,
                                     DocRef = l.DocRef,
                                     status = l.vr_apr == "P" ? "Pending" :
                                                         l.vr_apr == "A" ? "Approved" :
                                                         l.vr_apr == "C" ? "Cancelled" : "NULL"
                                 }).Distinct().ToList();
                //return grdObject;
                //var obj = (from o in grdObject
                //           orderby Convert.ToInt32(o.vr_no.Substring(0, 4) + o.vr_no.Substring(5)) descending
                //           select o).ToList();

                var obj = (from o in grdObject
                           orderby Convert.ToInt32(o.vr_no.Substring(0, 4)) descending, Convert.ToInt32(o.vr_no.Substring(5)) descending
                           select o).ToList();
                return obj;
            }
            else
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var grdObject = (from l in Data.tblStkGPs
                                 join g in Data.tblStkGPDets
                                 on l.vr_no equals g.vr_no
                                 join m in Data.Glmf_Codes
                                 on l.VendorId equals m.gl_cd

                                 where l.vr_dt >= fromDt && l.vr_dt <= toDt && l.vt_cd == 11
                                 select new
                                 {
                                     vr_no = (from b in Data.tblStkGPs
                                              where b.DocRef == l.DocRef && b.vt_cd == 11 
                                              select b).First().vr_no.ToString().Substring(0, 4)
                                               + "/"
                                               +
                                               (from b in Data.tblStkGPs
                                                where b.DocRef == l.DocRef && b.vt_cd == 11
                                                select b).First().vr_no.ToString().Substring(4),

                                     GPRef = (from e in Data.tblStkGPs
                                              join f in Data.tblStkGPDets
                                              on e.vr_no equals f.vr_no
                                              where e.DocRef == l.DocRef && e.vt_cd == 11 && f.vt_cd == 11
                                              select f).First().GPRef,

                                     gl_dsc = m.gl_dsc,
                                     vr_dt = l.vr_dt,
                                     DocRef = l.DocRef,
                                     status = l.vr_apr == "P" ? "Pending" :
                                                         l.vr_apr == "A" ? "Approved" :
                                                         l.vr_apr == "C" ? "Cancelled" : "NULL"
                                 }).Distinct().ToList();
                //return grdObject;
                var obj = (from o in grdObject
                           orderby Convert.ToInt32(o.vr_no.Substring(0, 4) + o.vr_no.Substring(5)) descending
                           select o).ToList();
                return obj;
            }
        }


        public bool checkIfGpExist(string gpRef, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                List<tblStkGPDet> lst = (from a in Data.tblStkGPDets join 
                                         d in Data.tblStkGPs on
                                         a.vr_no equals d.vr_no 
                                         where a.GPRef == gpRef && a.vt_cd==11 && d.vt_cd==11
                                         && d.vr_apr != "C"
                                         select a).ToList();
                if (lst.Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }

        }


        public bool SaveStkIGP(tblStkGP gp, EntitySet<tblStkGPDet> gpDet, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                //RMSDataContext Data1 = new RMSDataContext();
                Data.tblStkGPs.InsertOnSubmit(gp);
                gp.tblStkGPDets = gpDet;
                Data.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }
        //public List<string> GetControlProduct(string sname)
        //{

        //    RMSDataContext Data = new RMSDataContext();
        //    if (Data == null) { Data = RMSDB.GetOject(); }
        //    List<string> lst = new List<string>();
        //    var list = from l in Data.tblItem_Codes
        //              where l.itm_dsc.Contains(sname)
        //              select l;
        //    foreach (var l in list)
        //    {
        //        lst.Add(l.itm_cd.ToString()+'-' + l.itm_dsc.ToString());
        //    }
        //    return lst.ToList();
        //}

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
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
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
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }
        public List<tblStock_Loc> GetStockLoc(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var lst = from l in Data.tblStock_Locs
                          where l.LocCode.Equals("RH") || l.LocCode.Equals("WB")
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

        public List<spGetMPNsVendorListResult> GetSelectedVendor(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                 List<spGetMPNsVendorListResult> lst =  Data.spGetMPNsVendorList().ToList();
                return lst;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }

        public string GetProductName(string itemCode, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            string pName = (from n in Data.tblItem_Codes
                            where n.itm_cd == itemCode
                            select n).Single().itm_dsc.ToString();
            return pName;
        }
        public List<tblItem_Code> GetProduct(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var lst = from l in Data.tblItem_Codes
                          where l.ct_id == "D" && l.itm_cd.StartsWith("10")
                          orderby l.itm_cd
                          select l;
                return lst.ToList();
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }

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
    }
}
