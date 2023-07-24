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
    public partial class InvGC_BL
    {
        public InvGC_BL()
        {
        }

        public string GetTrnsfrdQty(int ltNo, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                int sumQty = 0;
                var rows = (from a in Data.tblItemDatas
                            join b in Data.tblItemDataDets
                            on a.vr_no equals b.vr_no
                            where a.LotNo == ltNo && a.vt_cd == 15 && b.vt_cd == 15 && a.vr_apr != "C"
                            select new
                            {
                                a.vr_apr,
                                b.vr_qty
                            }).ToList();
                foreach (var r in rows)
                {
                    sumQty = sumQty + Convert.ToInt32(r.vr_qty);
                }
                return sumQty.ToString();
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
               // RMSDB.closeConn(Data);
                throw ex;
            }
        }

        public int GetVrQty(int vrNo, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                int vrQty = 0;
                var rows = (from a in Data.tblItemDataDets
                           where a.vr_no == vrNo && a.vt_cd == 15
                           select a).ToList();
                foreach (var r in rows)
                {
                  vrQty=vrQty+  Convert.ToInt32(r.vr_qty);
                }
                return vrQty;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

        public bool CheckIfAlreadyTrnsfrd(int ltNo, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                int lotQtyFtg = 0;
                int lotQtyGrading = 0;
                var rows = (from a in Data.tblItemDatas
                            join b in Data.tblItemDataDets
                            on a.vr_no equals b.vr_no
                            where a.LotNo == ltNo && a.vt_cd == 15 && b.vt_cd == 15 && a.vr_apr != "C"
                            select new
                            {
                                a.vr_apr,
                                b.vr_qty
                            }).ToList();

                var rowsDet = (from a in Data.tblItemDatas
                               join b in Data.tblItemDataDets
                               on a.vr_no equals b.vr_no
                               where a.LotNo == ltNo && a.vt_cd == 14 && b.vt_cd == 14 && a.vr_apr == "A"
                               select new
                               {
                                   a.vr_apr,
                                   b.vr_qty
                               }).ToList();

                foreach (var r in rowsDet)
                {
                    lotQtyFtg = lotQtyFtg + Convert.ToInt32(r.vr_qty);
                }

                foreach (var res in rows)
                {
                    if (res.vr_apr == "A" || res.vr_apr == "P")
                    {
                        lotQtyGrading = lotQtyGrading + Convert.ToInt32(res.vr_qty);
                    }
                }
                if (rows.Count > 0)
                {
                    if (lotQtyGrading == lotQtyFtg)
                    {
                        return true;
                    }
                    else if (lotQtyGrading < lotQtyFtg)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
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
        public string GetMTQty(int lotNo, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                int sum = 0;
                var obj = from a in Data.tblItemDatas
                          join b in Data.tblItemDataDets
                          on a.vr_no equals b.vr_no
                          where a.LotNo == lotNo && a.vt_cd == 23 && b.vt_cd == 23 && a.vr_apr == "A"
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
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

        public int GetLotQty(int lotNo, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                int sum = 0;
                var obj = from a in Data.tblItemDatas
                          join b in Data.tblItemDataDets
                          on a.vr_no equals b.vr_no
                          where a.LotNo == lotNo && a.vt_cd == 14 && b.vt_cd == 14 && a.vr_apr == "A"
                          select new
                          {
                              b.vr_qty
                          };
                foreach (var c in obj)
                {
                    sum = sum + Convert.ToInt32(c.vr_qty);
                }
                return sum;
                //int sum = 0;
                //var obj = from a in Data.tblItemDatas
                //          join b in Data.tblItemDataDets
                //          on a.vr_no equals b.vr_no
                //          where a.LotNo == lotNo && a.vt_cd == 23 && b.vt_cd == 23 && a.vr_apr == 'A'
                //          select new
                //          {
                //              b.vr_qty
                //          };
                //foreach (var c in obj)
                //{
                //    sum = sum + Convert.ToInt32(c.vr_qty);
                //}
                //return sum;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
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
                                      where r.cnt_itm_cd == itemCode && d.LocId == 2
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

        public string GetItemUzngLot(int ltNo, RMSDataContext Data)
        {
            string itmCode = "0";

            if (Data == null) { Data = RMSDB.GetOject(); }
            var obj = (from r in Data.tblItemDatas
                       join d in Data.tblItemDataDets
                       on r.vr_no equals d.vr_no
                       where r.LotNo == ltNo && r.vt_cd == 23 && d.vt_cd == 23
                       select new
                       {
                           Itm_cd = d.itm_cd,
                           LotNo = r.LotNo
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
                                 
                                 where l.vr_dt >= fromDt && l.vr_dt <= toDt && l.vr_apr == Convert.ToString(sttus) && l.vt_cd == 15
                                 orderby l.vr_no descending
                                 select new
                                 {
                                     vr_no = l.vr_no.ToString().Substring(0, 4) + "/" + l.vr_no.ToString().Substring(4),
                                     
                                     vr_dt = l.vr_dt,
                                     LotNo = l.LotNo.Value.ToString().Substring(0, 4) + "-" + l.LotNo.Value.ToString().Substring(4),
                                     status = l.vr_apr == "P" ? "Pending" :
                                                         l.vr_apr == "A" ? "Approved" :
                                                         l.vr_apr == "C" ? "Cancelled" : "NULL"
                                 }).Distinct().ToList();

                var obj = (from o in grdObject
                           orderby Convert.ToInt32(o.LotNo.Substring(0,2)) descending,Convert.ToInt32(o.LotNo.Substring(2,2)) descending, Convert.ToInt32(o.LotNo.Substring(5)) descending,
                                   Convert.ToInt32(o.vr_no.Substring(0, 4)) descending, Convert.ToInt32(o.vr_no.Substring(5)) descending 
                           select o).ToList();
                return obj;
            }
            else
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var grdObject = (from l in Data.tblItemDatas
                                 join g in Data.tblItemDataDets
                                 on l.vr_no equals g.vr_no
                                 
                                 where l.vr_dt >= fromDt && l.vr_dt <= toDt && l.vt_cd == 15
                                 orderby l.vr_no descending
                                 select new
                                 {
                                     vr_no = l.vr_no.ToString().Substring(0, 4) + "/" + l.vr_no.ToString().Substring(4),
                                     
                                     vr_dt = l.vr_dt,
                                     LotNo = l.LotNo.Value.ToString().Substring(0, 4) + "-" + l.LotNo.Value.ToString().Substring(4),
                                     status = l.vr_apr == "P" ? "Pending" :
                                                         l.vr_apr == "A" ? "Approved" :
                                                         l.vr_apr == "C" ? "Cancelled" : "NULL"
                                 }).Distinct().ToList();
    
                var obj = (from o in grdObject
                           orderby Convert.ToInt32(o.LotNo.Substring(0, 2)) descending, Convert.ToInt32(o.LotNo.Substring(2, 2)) descending, Convert.ToInt32(o.LotNo.Substring(5)) descending,
                                   Convert.ToInt32(o.vr_no.Substring(0, 4)) descending, Convert.ToInt32(o.vr_no.Substring(5)) descending 
                           select o).ToList();
                return obj;
            }
        }



        public string getSelectionID(string itmCode, string gradeID, string itmDesc,RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                string selectionID = (from r in Data.tblStkGradeDets
                                      where r.itm_cd == itmCode && r.GradeId == gradeID && r.SizeGrade_Desc == itmDesc
                                      select r).Take(1).Single().SelectionId;
                return selectionID;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

        public tblItem GetItemUsingSelection(string itmCode, string selectionId, int locId, RMSDataContext Data)
        {
            try
            {
                tblItem itm = (from a in Data.tblItems
                               where a.itm_cd == itmCode && a.SelectionId == selectionId && a.LocId == locId
                                     select a).Single();
                return itm;
            }
            catch (Exception ex)
            {
                //RMSDB.closeConn(Data);
                //throw ex;
                return null;
            }
        }

        public bool GetUpdTblItemGrading(int locId, string itmCd, string selId, int purQty, RMSDataContext Data)
        {
            try
            {
                Data.spUpdateTblItem4Grading(locId, itmCd, selId, purQty);
                return true;
            }
            catch (Exception ex)
            {
                //RMSDB.closeConn(Data);
                throw ex;
            }
        }


        public bool GetInsrtTblitem(int brId, int locId, string itmCd, string selId, int purQty, string desId, decimal glYr, RMSDataContext Data)
        {
            try
            {
                Data.spInsertxTblItem(brId, locId, itmCd, selId, purQty, desId, glYr);
                return true;
            }
            catch (Exception ex)
            {
                //RMSDB.closeConn(Data);
                throw ex;
            }
        }

        public string GetGlYear(RMSDataContext Data)
        {
            try
            {
                DateTime date = Common.MyDate(Data);
                FIN_PERD fin = (from a in Data.FIN_PERDs
                                where a.Start_Date <= date.Date && a.End_Date >= date.Date
                                select a).Single();
                string finYear = fin.Gl_Year.ToString();
                return finYear;
            }
            catch (Exception ex)
            {
                //RMSDB.closeConn(Data);
                throw ex;
            }
        }

        public List<tblItemDataDet> GetItemDetUsingVrNo(int vrNo, RMSDataContext Data)
        {
            try
            {
                List<tblItemDataDet> lst = (from a in Data.tblItemDataDets
                                            where a.vr_no == vrNo && a.vt_cd == 15 && a.vt_cd != 'C'
                                            select a).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                //RMSDB.closeConn(Data);
                throw ex;
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
                //RMSDataContext Data = new RMSDataContext();

                if (Data == null) { Data = RMSDB.GetOject(); }
                //Deleting prvious records
                var recDet = from r in Data.tblItemDataDets
                             where Convert.ToInt32(r.vr_no.ToString().Substring(4)) == sgNo && Convert.ToInt32(r.vr_no.ToString().Substring(0, 4)) == yr && r.vt_cd == code
                             select r;
                Data.tblItemDataDets.DeleteAllOnSubmit(recDet);
                Data.SubmitChanges();
                //Deleting parent
                if (Data == null) { Data = RMSDB.GetOject(); }
                tblItemData rec1 = (from r in Data.tblItemDatas
                                 where Convert.ToInt32(r.vr_no.ToString().Substring(4)) == sgNo && Convert.ToInt32(r.vr_no.ToString().Substring(0, 4)) == yr && r.vt_cd == code
                                 select r).Single();
                Data.tblItemDatas.DeleteOnSubmit(rec1);
                Data.SubmitChanges();
                //Inserting new records
                sG.tblItemDataDets = sgDet;
                Data.tblItemDatas.InsertOnSubmit(sG);
                //Changes submitted
                Data.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                //RMSDB.closeConn(Data);
                throw ex;
            }
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
                          orderby d.SizeGrade_Desc
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
                             GradeId = d.GradeId+":"+d.SelectionId
                          };
                return lst.ToList();
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
