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
    public partial class InvSG_BL
    {
        public InvSG_BL()
        {
        }



        public object GetAllSearch4AutoCompl(string prefixText)
        {
            RMSDataContext Data = new RMSDataContext();
            if (Data == null) { Data = RMSDB.GetOject(); }

            var obj = (from a in Data.tblStkGPs
                       join b in Data.tblStkGPDets
                       on a.vr_no equals b.vr_no
                       join c in Data.Glmf_Codes
                       on b.PartyId equals c.gl_cd
                       join d in Data.Glmf_Codes
                       on a.VendorId equals d.gl_cd
                       join e in Data.tblCities
                       on Convert.ToInt32(a.VendorCity) equals e.CityID


                       where a.vt_cd == b.vt_cd && a.vt_cd == 11 && b.vt_cd == 11 ||
                             a.vr_no.ToString().Contains(prefixText) || c.gl_dsc.Contains(prefixText) ||
                             d.gl_dsc.Contains(prefixText) || e.CityName.Contains(prefixText)

                       select new
                       {
                           vr_no = a.vr_no,
                           IGPNo = a.vr_no.ToString().Substring(0, 4) + "/" + a.vr_no.ToString().Substring(4),
                           Party = c.gl_dsc,
                           Vendor = d.gl_dsc,
                           City = e.CityName

                       }).ToList();
            return obj;
        }



        public string GetQtyByIGP(int igpNo, int code, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                tblStkGPDet obj = (from a in Data.tblStkGPDets
                                   where a.vr_no == igpNo && a.vt_cd == code
                                   select a).Single();
                return obj.vr_qty.ToString();
            }
            catch
            {
                return "0";
            }
        }


        public bool GetIfIgpApproved(int igpNo, int code, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                List<tblStkGP> lst = (from a in Data.tblStkGPs
                                      where a.vr_no == igpNo && a.vt_cd == code && a.vr_apr == "A"
                                      select a).ToList();
                if (lst.Count > 0)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }

        public bool getIfCardAlreadyExist(int igpNo, int code, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                List<tblStkGP> results = (from a in Data.tblStkGPs
                                          where a.vr_no == igpNo && a.vt_cd == 11 && a.vr_apr != "C"
                                          select a).ToList();
                if (results.Count > 0)
                {
                    List<tblItemData> res = (from a in Data.tblItemDatas
                                             where a.vt_cd == code && a.DocRef == results.Single().DocRef && a.vr_apr != "C"
                                             select a).ToList();
                    if (res.Count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
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

        public string GetSizedQty(int igpNo, int code, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                string docRef = (from a in Data.tblStkGPs
                                 where a.vr_no == igpNo && a.vt_cd == code
                                 select a).SingleOrDefault().DocRef;

                string qty = (from a in Data.tblItemDatas
                              join b in Data.tblItemDataDets
                              on a.vr_no equals b.vr_no
                              where a.vt_cd == b.vt_cd && a.vt_cd == 14 && b.vt_cd == 14 &&
                              a.DocRef == docRef && a.vr_apr != "C"
                              select b.vr_qty).Sum().ToString();
                return qty;

            }
            catch
            {
                return "0";
            }
        }

        public string GetSizedQtyView(int igpNo, int code, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                string docRef = (from a in Data.tblStkGPs
                                 where a.vr_no == igpNo && a.vt_cd == code
                                 select a).SingleOrDefault().DocRef;

                string qty = (from a in Data.tblItemDatas
                              join b in Data.tblItemDataDets
                              on a.vr_no equals b.vr_no
                              where a.vt_cd == b.vt_cd && a.vt_cd == 14 && b.vt_cd == 14 &&
                              a.DocRef == docRef && a.vr_apr == "A"
                              select b.vr_qty).Sum().ToString();
                return qty;

            }
            catch
            {
                return "0";
            }
        }

        public string GetLotByIGP(int igpNo, DateTime selectedDate, int code, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            DateTime vrDate = Common.MyDate(Data);
            string docRef = (from a in Data.tblStkGPs
                             where a.vr_no == igpNo && a.vt_cd == code
                             select a).SingleOrDefault().DocRef;
           
                return GetNewLot(selectedDate, Data);
        }

        public string GetNewLot(DateTime selectedDate, RMSDataContext Data)
        {
            try
            {
                string lot = "";
                string preLot = "";
                string selectedPreLot = "";

                List<int> newListBel = new List<int>();
                List<int> newListAb = new List<int>();
                List<tblItemData> lstBelow = (from c in Data.tblItemDatas
                                              where c.vt_cd == 14 && c.vr_dt.Year == selectedDate.Year && c.vr_dt.Month <= 6
                                              select c).ToList();
                List<tblItemData> lstAbove = (from c in Data.tblItemDatas
                                              where c.vt_cd == 14 && c.vr_dt.Year == selectedDate.Year && c.vr_dt.Month > 6
                                              select c).ToList();

                if (selectedDate.Month <= 6)
                {
                    if (lstBelow.Count() > 0)
                    {
                        foreach (var l in lstBelow)
                        {
                            if (l.DocRef != null)
                            {
                                lot = l.LotNo.Value.ToString().Substring(4);
                                preLot = l.LotNo.Value.ToString().Substring(0, 4);
                                int lotMonth = Convert.ToInt32(preLot.Substring(2, 2));
                                int mnth = selectedDate.Month;
                                if (lotMonth == mnth)
                                {
                                    selectedPreLot = preLot;
                                    newListBel.Add(Convert.ToInt32(lot));
                                }
                            }
                        }
                        if (newListBel.Count() > 0)
                        {
                            return selectedPreLot + "-" + Convert.ToString(Convert.ToInt32(newListBel.Max()) + 1);
                        }
                        else
                        {
                            return selectedDate.Year.ToString().Substring(2) + "0" + selectedDate.Month.ToString() + "-1";
                        }
                    }
                    else
                    {
                        return selectedDate.Year.ToString().Substring(2) + "0" + selectedDate.Month.ToString() + "-1";
                    }

                }
                else
                {
                    if (lstAbove.Count() > 0)
                    {
                        foreach (var l in lstAbove)
                        {
                            if (l.DocRef != null)
                            {
                                lot = l.LotNo.Value.ToString().Substring(4);
                                preLot = l.LotNo.Value.ToString().Substring(0, 4);
                                int lotMonth = Convert.ToInt32(preLot.Substring(2, 2));
                                int mnth = selectedDate.Month;
                                if (lotMonth == mnth)
                                {
                                    selectedPreLot = preLot;
                                    newListAb.Add(Convert.ToInt32(lot));
                                }
                            }
                        }
                        if (newListAb.Count() > 0)
                        {
                            return selectedPreLot + "-" + Convert.ToString(Convert.ToInt32(newListAb.Max()) + 1);
                        }
                        else
                        {
                            if (selectedDate.Month < 10)
                                return selectedDate.Year.ToString().Substring(2) + "0" + selectedDate.Month.ToString() + "-1";
                            else
                                return selectedDate.Year.ToString().Substring(2) + selectedDate.Month.ToString() + "-1";
                        }
                    }
                    else
                    {
                        if (selectedDate.Month < 10)
                            return selectedDate.Year.ToString().Substring(2) + "0" + selectedDate.Month.ToString() + "-1";
                        else
                            return selectedDate.Year.ToString().Substring(2) + selectedDate.Month.ToString() + "-1";
                    }
                }
            }
            catch
            {
                return "0";
            }
        }

        //For SizingGradingCardMgt2
        //public string GetLotByIGP(int igpNo,DateTime selectedDate, int code, RMSDataContext Data)
        //{
        //    if (Data == null) { Data = RMSDB.GetOject(); }

        //    DateTime vrDate = Common.MyDate(Data);
        //    string lot = "";
        //    string preLot = "";
        //    string selectedPreLot = "";


        //    string docRef = (from a in Data.tblStkGPs
        //                     where a.vr_no == igpNo && a.vt_cd == code
        //                     select a).SingleOrDefault().DocRef;
        //    try
        //    {
        //        tblItemData objItem = (from b in Data.tblItemDatas
        //                               where b.DocRef == docRef && b.vt_cd == 14
        //                               select b).Take(1).Single();


        //        return objItem.LotNo.ToString().Substring(0, 4) + "-" + objItem.LotNo.ToString().Substring(4);


        //    }
        //    catch
        //    {
        //        List<int> newListBel = new List<int>();
        //        List<int> newListAb = new List<int>();
        //        List<tblItemData> lstBelow = (from c in Data.tblItemDatas
        //                                      where c.vt_cd == 14 && c.vr_dt.Year == selectedDate.Year && c.vr_dt.Month <= 6
        //                                      select c).ToList();
        //        List<tblItemData> lstAbove = (from c in Data.tblItemDatas
        //                                      where c.vt_cd == 14 && c.vr_dt.Year == selectedDate.Year && c.vr_dt.Month > 6
        //                                      select c).ToList();

        //        if (selectedDate.Month <= 6)
        //        {
        //            if (lstBelow.Count() > 0)
        //            {
        //                foreach (var l in lstBelow)
        //                {
        //                    if (l.DocRef != null)
        //                    {
        //                        lot = l.LotNo.Value.ToString().Substring(4);
        //                        preLot = l.LotNo.Value.ToString().Substring(0, 4);
        //                        int lotMonth=Convert.ToInt32( preLot.Substring(2, 2));
        //                        int mnth=selectedDate.Month;
        //                        if (lotMonth == mnth)
        //                        {
        //                            selectedPreLot = preLot;
        //                            newListBel.Add(Convert.ToInt32(lot));
        //                        }
        //                    }
        //                }
        //                if (newListBel.Count() > 0)
        //                {
        //                    return selectedPreLot + "-" + Convert.ToString(Convert.ToInt32(newListBel.Max()) + 1);
        //                }
        //                else
        //                {
        //                    return selectedDate.Year.ToString().Substring(2) + "0" + selectedDate.Month.ToString() + "-1";
        //                }
        //            }
        //            else
        //            {
        //                return selectedDate.Year.ToString().Substring(2) + "0" + selectedDate.Month.ToString() + "-1";
        //            }

        //        }
        //        else
        //        {
        //            if (lstAbove.Count() > 0)
        //            {
        //                foreach (var l in lstAbove)
        //                {
        //                    if (l.DocRef != null)
        //                    {
        //                        lot = l.LotNo.Value.ToString().Substring(4);
        //                        preLot = l.LotNo.Value.ToString().Substring(0, 4);
        //                        int lotMonth=Convert.ToInt32( preLot.Substring(2, 2));
        //                        int mnth=selectedDate.Month;
        //                        if (lotMonth == mnth)
        //                        {
        //                            selectedPreLot = preLot;
        //                            newListAb.Add(Convert.ToInt32(lot));
        //                        }
        //                    }
        //                }
        //                if (newListAb.Count() > 0)
        //                {
        //                    return selectedPreLot + "-" + Convert.ToString(Convert.ToInt32(newListAb.Max()) + 1);
        //                }
        //                else
        //                {
        //                    if (selectedDate.Month < 10)
        //                        return selectedDate.Year.ToString().Substring(2) + "0" + selectedDate.Month.ToString() + "-1";
        //                    else
        //                        return selectedDate.Year.ToString().Substring(2) + selectedDate.Month.ToString() + "-1";
        //                }
        //            }
        //            else
        //            {
        //                if (selectedDate.Month < 10)
        //                    return selectedDate.Year.ToString().Substring(2) + "0" + selectedDate.Month.ToString() + "-1";
        //                else
        //                    return selectedDate.Year.ToString().Substring(2) + selectedDate.Month.ToString() + "-1";
        //            }
        //        }

        //    }
        //}
       

        public string GetProducByIGP(int igpNo, int code, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                string itmCode = (from a in Data.tblStkGPDets
                                  where a.vr_no == igpNo && a.vt_cd == code
                                  select a).Take(1).Single().Itm_cd;
                return itmCode;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

        public string GetGPRefByIGP(int igpNo, int code, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                string gpRef = (from a in Data.tblStkGPDets
                                where a.vr_no == igpNo && a.vt_cd == code
                                select a).Take(1).First().GPRef;
                return gpRef;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

        public object getGridList(int igpNo, int code, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                var obj = (from a in Data.tblStkGPs
                           where a.vr_no == igpNo && a.vt_cd == code
                           select a).Take(1).SingleOrDefault().DocRef;
                if (obj != null)
                {
                    string docRef = obj.ToString();
                    List<Anonymous4Grid> lst = (from b in Data.tblStkGPs
                                                join c in Data.tblStkGPDets
                                                on b.vr_no equals c.vr_no
                                                where b.DocRef == docRef && b.vt_cd == code
                                                orderby c.vr_seq
                                                select new Anonymous4Grid
                                                {
                                                    Doc_Ref = b.DocRef,
                                                    vr_no = b.vr_no.ToString().Substring(0, 4) + "/" + b.vr_no.ToString().Substring(4),
                                                    vr_qty = c.vr_qty,
                                                    vr_dt = b.vr_dt,
                                                    gl_dsc = (from d in Data.Glmf_Codes
                                                              where d.gl_cd == c.PartyId
                                                              select d).Single().gl_dsc,

                                                    vr_apr = b.vr_apr == "P" ? "Pending" :
                                                                  b.vr_apr == "A" ? "Approved" :
                                                                  b.vr_apr == "C" ? "Cancelled" : "NULL"
                                                }).ToList();
                    return lst;
                }
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }

        public string GetItemUzngLot(int ltNo, RMSDataContext Data)
        {
            string itmCode = "0";

            if (Data == null) { Data = RMSDB.GetOject(); }
            var obj = (from r in Data.tblStkGPs
                       join d in Data.tblStkGPDets
                       on r.vr_no equals d.vr_no
                       where d.LotNo == ltNo && r.vt_cd == 11
                       select new
                       {
                           Itm_cd = d.Itm_cd,
                           LotNo = d.LotNo
                       }).Take(1).ToList();
            if (obj != null)
            {
                foreach (var i in obj)
                {
                    itmCode = i.Itm_cd;

                }
                return itmCode;
            }
            else
            {
                return itmCode;
            }
        }
        public string GetControlItemCd(string itemCode, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                string cntItemCode = (from r in Data.tblItem_Codes
                                      join d in Data.tblItems
                                      on r.itm_cd equals d.itm_cd
                                      where r.cnt_itm_cd == itemCode && d.LocId == Convert.ToInt16(2) && r.itm_cd != itemCode
                                      select new
                                      {
                                          r.itm_cd
                                      }).Take(1).Single().itm_cd;
                return cntItemCode;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }

        }
        public tblItemData GetRec(int GPNo, int yr, int code, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            tblItemData rec = (from r in Data.tblItemDatas
                               where Convert.ToInt32(r.vr_no.ToString().Substring(4)) == GPNo && Convert.ToInt32(r.vr_no.ToString().Substring(0, 4)) == yr && r.vt_cd == code
                               select r).Single();
            return rec;
        }



        public List<tblItemDataDet> GetRecDet(int sgNo, int yr, int code, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            List<tblItemDataDet> recs = (from r in Data.tblItemDataDets
                                         where Convert.ToInt32(r.vr_no.ToString().Substring(4)) == sgNo && Convert.ToInt32(r.vr_no.ToString().Substring(0, 4)) == yr && r.vt_cd == code
                                         orderby r.vr_seq
                                         select r).ToList();
            return recs;
        }


        public int GetIGpByVoucher(int vrNo, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                tblItemData row = (from a in Data.tblItemDatas
                                   where a.vr_no == vrNo && a.vt_cd == 14
                                   select a).Single();
                return Convert.ToInt32(row.IGPNo);
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }



        public bool ChngedSG_Status(int sgNo, int yr, int code, char sttus, string updBy, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                //RMSDataContext Data = new RMSDataContext();
                tblItemData rec = (from r in Data.tblItemDatas
                                   where Convert.ToInt32(r.vr_no.ToString().Substring(4)) == sgNo && Convert.ToInt32(r.vr_no.ToString().Substring(0, 4)) == yr && r.vt_cd == code
                                   select r).Single();
                rec.vr_apr = Convert.ToString(sttus);
                rec.updateby = updBy;
                rec.updateon = Common.MyDate(Data);
                Data.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }
        public object GetGrid_SG(DateTime fromDt, DateTime toDt, char sttus, RMSDataContext Data)
        {
            if (sttus != 'M')
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var grdObject = (from l in Data.tblItemDatas
                                 join g in Data.tblItemDataDets
                                 on l.vr_no equals g.vr_no
                                 where l.vr_dt >= fromDt && l.vr_dt <= toDt && l.vr_apr == Convert.ToString(sttus) && l.vt_cd == 14

                                 select new
                                 {
                                     vr_no = l.vr_no.ToString().Substring(0, 4) + "/" + l.vr_no.ToString().Substring(4),
                                     igpNo = l.IGPNo.Value.ToString().Substring(0, 4) + "/" + l.IGPNo.Value.ToString().Substring(4),

                                     GPRef = (from e in Data.tblStkGPs
                                              join f in Data.tblStkGPDets
                                              on e.vr_no equals f.vr_no
                                              where e.DocRef == l.DocRef && e.vt_cd == 11 && f.vt_cd == 11
                                              select f).First().GPRef,

                                     Qty = Convert.ToInt32((from e in Data.tblItemDatas
                                                            join f in Data.tblItemDataDets
                                                            on e.vr_no equals f.vr_no
                                                            where e.DocRef == l.DocRef && e.vt_cd == 14 && f.vt_cd == 14 && e.LotNo==l.LotNo
                                                            select f.vr_qty).Sum()),

                                     vr_dt = l.vr_dt,
                                     LotNo = l.LotNo.ToString().Substring(0, 4) + "-" + l.LotNo.ToString().Substring(4),
                                     status = l.vr_apr == "P" ? "Pending" :
                                                         l.vr_apr == "A" ? "Approved" :
                                                         l.vr_apr == "C" ? "Cancelled" : "NULL"
                                 }).Distinct().ToList();

                var obj = (from o in grdObject
                           orderby Convert.ToInt32(o.LotNo.Substring(0, 2)) descending, Convert.ToInt32(o.LotNo.Substring(2, 2)) descending, Convert.ToInt32(o.LotNo.Substring(5)) descending,
                                   Convert.ToInt32(o.vr_no.Substring(0,4))  descending, Convert.ToInt32(o.vr_no.Substring(5))    descending 
                           select o).ToList();
                return obj;
            }
            else
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var grdObject = (from l in Data.tblItemDatas
                                 join g in Data.tblItemDataDets
                                 on l.vr_no equals g.vr_no
                                 where l.vr_dt >= fromDt && l.vr_dt <= toDt && l.vt_cd == 14

                                 select new
                                 {
                                     vr_no = l.vr_no.ToString().Substring(0, 4) + "/" + l.vr_no.ToString().Substring(4),
                                     igpNo = l.IGPNo.Value.ToString().Substring(0, 4) + "/" + l.IGPNo.Value.ToString().Substring(4),

                                     GPRef = (from e in Data.tblStkGPs
                                              join f in Data.tblStkGPDets
                                              on e.vr_no equals f.vr_no
                                              where e.DocRef == l.DocRef && e.vt_cd == 11 && f.vt_cd == 11
                                              select f).First().GPRef,

                                     //Qty = Convert.ToInt32((from e in Data.tblStkGPs
                                     //                       join f in Data.tblStkGPDets
                                     //                       on e.vr_no equals f.vr_no
                                     //                       where e.DocRef == l.DocRef && e.vt_cd == 11 && f.vt_cd == 11
                                     //                       select f.vr_qty).Sum()),

                                     Qty = Convert.ToInt32((from e in Data.tblItemDatas
                                                            join f in Data.tblItemDataDets
                                                            on e.vr_no equals f.vr_no
                                                            where e.DocRef == l.DocRef && e.vt_cd == 14 && f.vt_cd == 14 && e.LotNo == l.LotNo
                                                            select f.vr_qty).Sum()),

                                     vr_dt = l.vr_dt,
                                     LotNo = l.LotNo.ToString().Substring(0, 4) + "-" + l.LotNo.ToString().Substring(4),
                                     status = l.vr_apr == "P" ? "Pending" :
                                                         l.vr_apr == "A" ? "Approved" :
                                                         l.vr_apr == "C" ? "Cancelled" : "NULL"
                                 }).Distinct().ToList();
  
                var obj = (from o in grdObject
                           orderby Convert.ToInt32(o.LotNo.Substring(0, 2)) descending, Convert.ToInt32(o.LotNo.Substring(2, 2)) descending, Convert.ToInt32(o.LotNo.Substring(5)) descending,
                                   Convert.ToInt32(o.vr_no.Substring(0, 4)) descending, Convert.ToInt32(o.vr_no.Substring(5))    descending 
                           select o).ToList();
                return obj;
            }
        }
        public bool SaveItemData(tblItemData sG, EntitySet<tblItemDataDet> sgDet, RMSDataContext Data)
        {
            try
            {


                if (Data == null) { Data = RMSDB.GetOject(); }

                Data.tblItemDatas.InsertOnSubmit(sG);
                sG.tblItemDataDets = sgDet;
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
        public bool EditRec(int sgNo, int yr, int code, tblItemData sG, EntitySet<tblItemDataDet> sgDet, RMSDataContext Data)
        {
            try
            {

                if (Data == null) { Data = RMSDB.GetOject(); }
                //Deleting prvious records
                var recDet = from r in Data.tblItemDataDets
                             where Convert.ToInt32(r.vr_no.ToString().Substring(4)) == sgNo && Convert.ToInt32(r.vr_no.ToString().Substring(0, 4)) == yr && r.vt_cd == code
                             select r;
                Data.tblItemDataDets.DeleteAllOnSubmit(recDet);
                Data.SubmitChanges();
                //Deleting parent
                //if (Data == null) { Data = RMSDB.GetOject(); }
                tblItemData rec1 = (from r in Data.tblItemDatas
                                    where Convert.ToInt32(r.vr_no.ToString().Substring(4)) == sgNo && Convert.ToInt32(r.vr_no.ToString().Substring(0, 4)) == yr && r.vt_cd == code
                                    select r).Single();
                Data.tblItemDatas.DeleteOnSubmit(rec1);
                Data.SubmitChanges();
                
                //Inserting new records
                Data.tblItemDatas.InsertOnSubmit(sG);
                sG.tblItemDataDets = sgDet;
                //Changes submitted
                Data.SubmitChanges();

                return true;
            }
            catch //(Exception ex)
            {
                //RMSDB.closeConn(Data);
               // throw ex;
            }
            return false;
        }
        public string GetSGNo(DateTime dtTime, int code, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                List<int> lst = new List<int>();
                FIN_PERD fin = (from a in Data.FIN_PERDs
                                where a.Start_Date <= dtTime && a.End_Date >= dtTime
                                select a).Single();
                string finYear = fin.Gl_Year.ToString();


                var records = from n in Data.tblItemDatas
                              select n;
                foreach (var rec in records)
                {
                    string year = rec.vr_no.ToString().Substring(0, 4);
                    if (year == finYear && rec.vt_cd == code)
                    {
                        lst.Add(Convert.ToInt32( rec.vr_no.ToString().Substring(4)));
                    }
                }
                if (lst.Count > 0)
                {
                    return finYear + "/" +Convert.ToInt32(lst.Max() + 1);
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

        public List<string> GetSizeGradeCode1(string itmCode, int lcID, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                List<string> lstObj = new List<string>();
                var lst = from d in Data.tblStkGradeDets
                          join l in Data.tblStkGrades
                          on d.itm_cd equals l.itm_cd
                          where l.itm_cd == itmCode && l.LocId == lcID && d.LocId == lcID
                          orderby d.GradeId
                          select new
                          {
                              SizeGrade_Desc = d.SizeGrade_Desc,
                              GradeId = d.GradeId + ":" + d.SelectionId
                          };

                foreach (var r in lst)
                {
                    lstObj.Add(r.SizeGrade_Desc + "," + r.GradeId);
                }

                return lstObj;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }


        public object GetSizeGradeCode(string itmCode, int lcID, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var lst = from d in Data.tblStkGradeDets
                          join l in Data.tblStkGrades
                          on d.itm_cd equals l.itm_cd
                          where l.itm_cd == itmCode && l.LocId == lcID && d.LocId == lcID
                          orderby d.GradeId
                          select new
                          {
                              SizeGrade_Desc = d.SizeGrade_Desc,
                              GradeId = d.GradeId + ":" + d.SelectionId
                          };
                return lst.ToList();
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }

        public string GetItemCode1(int sgNo, int yr, int code, int lcId, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                string lst = (from l in Data.tblItemDatas
                              join d in Data.tblItemDataDets
                              on l.vr_no equals d.vr_no
                              where l.vr_no == sgNo && Convert.ToInt32(l.vr_no.ToString().Substring(0, 4)) == yr && l.vt_cd == code && d.LocId == lcId
                              select new
                              {
                                  d.itm_cd
                              }).Take(1).Single().itm_cd;
                return lst;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }


        public string GetItemCode(int sgNo, int yr, int code, int lcId, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                string lst = (from l in Data.tblItemDatas
                              join d in Data.tblItemDataDets
                              on l.vr_no equals d.vr_no
                              where Convert.ToInt32(l.vr_no.ToString().Substring(4)) == sgNo && Convert.ToInt32(l.vr_no.ToString().Substring(0, 4)) == yr && l.vt_cd == code && d.LocId == lcId
                              select new
                              {
                                  d.itm_cd
                              }).Take(1).Single().itm_cd;
                return lst;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }





        public List<tblItem_Code> GetProduct(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var lst = from l in Data.tblItem_Codes
                          orderby l.itm_dsc
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

    }
}
