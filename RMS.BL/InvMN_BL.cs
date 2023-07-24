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
    public partial class InvMN_BL
    {
        public InvMN_BL()
        {
        }
 
        public List<spGetGrdByIGPResult> GetGradingRecByIGP(string pmnRef, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                return Data.spGetGrdByIGP(pmnRef).ToList();
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }


        public List<spGetFtgByIGPResult> GetFeetageRecByIGP(string pmnRef, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                return Data.spGetFtgByIGP(pmnRef).ToList();
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }


        public tblStkGP GetVendorInfo(string vendorId, int igpNo, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                tblStkGP rec = (from a in Data.tblStkGPs
                                where a.vr_no == igpNo && a.VendorId == vendorId && a.vr_apr == "A"
                                select a).Single();
                return rec;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

        public List<spGetFtgTempTableRecsResult> GetFeetageRecords(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                List<spGetFtgTempTableRecsResult> lst = Data.spGetFtgTempTableRecs().ToList();
                return lst;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }


        public List<spGetGrdTempTableRecsResult> GetGradingRecords(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                List<spGetGrdTempTableRecsResult> lst =  Data.spGetGrdTempTableRecs().ToList();
                return lst;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

        public void DeleteFtgTempTable(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.spDeleteFtgTempTable();
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

        public void DeleteGrdTempTable(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.spDeleteGrdTempTable();
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

        public void InsertingIntoFtgTempTable(string docRef, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.spInsertIntoFtgTempTable(docRef);
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }


        public void InsertingIntoGrdTempTable(string docRef, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.spInsertIntoGrdTempTable(14, docRef);
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }


        public List<spGetIgpsDetailByVendor4MPNResult> GetIGPsDetailByVendor4MPN(int vendorId,string itemCode, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                List<spGetIgpsDetailByVendor4MPNResult> lst = Data.spGetIgpsDetailByVendor4MPN(vendorId, itemCode).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }



        public List<spSearchLot4MPNResult> GetSearchRes4MPN(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                List<spSearchLot4MPNResult> lst = Data.spSearchLot4MPN().ToList();
                
                return lst;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

        public List<spSearchLot4MPN4MultiLotsResult> GetSearchRes4MPN4MultiLots(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                List<spSearchLot4MPN4MultiLotsResult> lst = Data.spSearchLot4MPN4MultiLots().ToList();
                return lst;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

        public List<spGetAvgSqftResult> GetAvgSqft(string itmCode, int lcID, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                List<spGetAvgSqftResult> lst = Data.spGetAvgSqft(itmCode, lcID).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

        public string GetPartyByIGP(int igpNo, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                tblStkGPDet rec = (from a in Data.tblStkGPDets
                               join b in Data.tblStkGPs
                               on a.vr_no equals b.vr_no
                               where a.vr_no == igpNo && b.vr_no == igpNo && b.vr_apr !="C" && a.vt_cd== b.vt_cd
                               && a.vt_cd == 11 && b.vt_cd == 11
                               select a).Single();
                Glmf_Code row = (from c in Data.Glmf_Codes
                                 where c.gl_cd == rec.PartyId
                                 select c).Single();
                return row.gl_dsc;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }


        public string GetGPRefByIGP(int igpNo, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                string gpRef = (from a in Data.tblStkGPDets
                                            join b in Data.tblStkGPs
                                            on a.vr_no equals b.vr_no
                                            where a.vt_cd == b.vt_cd && a.vt_cd == 11 && b.vt_cd == 11 && a.vr_no == igpNo && b.vr_no == igpNo
                                            select a).SingleOrDefault().GPRef;
               return gpRef;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }


        public List<tblItemDataDet> GetIGPByLotEdit(int lotNo, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                List<tblItemDataDet> lst = (from a in Data.tblItemDataDets
                                            join b in Data.tblItemDatas
                                            on a.vr_no equals b.vr_no
                                            where a.vt_cd == b.vt_cd && a.vt_cd == 21 && b.vt_cd == 21 && b.LotNo == lotNo && b.vr_apr != "C"
                                            select a).ToList();

                return lst;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }

        public List<tblItemDataDet> GetIGPByLotEditCancel(int lotNo, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                List<tblItemDataDet> lst = (from a in Data.tblItemDataDets
                                            join b in Data.tblItemDatas
                                            on a.vr_no equals b.vr_no
                                            where a.vt_cd == b.vt_cd && a.vt_cd == 21 && b.vt_cd == 21 && b.LotNo == lotNo
                                            select a).ToList();

                return lst;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }

        public List<spGetIGPByLotResult> GetIGPByLot(int lotNo, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                List<spGetIGPByLotResult> lst = Data.spGetIGPByLot(lotNo).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }


        public List<string> GetSelectionGrade(string itmCode, int lcID, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                List<string> lstObj = new List<string>();
                var lst = from d in Data.tblStkGradeDets
                          join l in Data.tblStkGrades
                          on d.itm_cd equals l.itm_cd
                          where l.itm_cd == itmCode && d.itm_cd == itmCode && l.LocId == lcID && d.LocId == lcID
                          orderby d.SizeGrade_Desc
                          select new
                          {
                              SizeGrade_Desc = d.SizeGrade_Desc,
                              GradeId = d.GradeId + ":" + d.SelectionId
                          };

                foreach (var r in lst)
                {
                    lstObj.Add(r.SizeGrade_Desc+","+r.GradeId);
                }

                return lstObj;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }


        public bool QtyCheck(decimal fullPc, string itmCode, int lcID, string Sid, bool edit, int vrNo, RMSDataContext Data1)
        {
            bool chk = false ;
            try
            {
               //if (Data == null) { Data = RMSDB.GetOject(); }
                RMSDataContext Data = new RMSDataContext();
                decimal CardQty = 0;
                //Getting pending quantity
                int pQty = 0;
                spGetWetBluePendingQtyResult pendingQty = Data.spGetWetBluePendingQty(lcID, itmCode, Sid, "P").Single();
                if (pendingQty == null)
                {
                    pQty = 0;
                }
                else
                {
                    pQty = Convert.ToInt32(pendingQty.qtyfull) + Convert.ToInt32(pendingQty.qtyhalf);
                    if (edit == true)
                    {
                        CardQty =  GetCardSelectionQty(vrNo, 24, Sid, Data);
                        pQty = pQty - Convert.ToInt32(CardQty);
                    }
                }


                //Getting approved quantity
                var check = (from a in Data.tblItems
                             where a.LocId==lcID && a.itm_cd == itmCode && a.SelectionId==Sid
                             select a ).Single();

                if (check != null)
                {
                    decimal rs = Convert.ToDecimal((check.itm_op_qty + check.itm_pur_qty) - (check.itm_isu_qty + check.itm_isu_half / 2));

                    if (fullPc <= rs - pQty)
                    {
                        chk = true;
                    }
                    else
                    {
                        chk = false;
                    }
                }
                else
                {
                    chk = false;
                }

                return chk;
            }
            catch //(Exception ex)
            {
               // RMSDB.closeConn(Data); 
              //throw ex;
            }
            return false;
        }


        public decimal GetCardSelectionQty(int vrNo, int vtCd, string selId, RMSDataContext Data)
        {
            try
            {
                tblItemDataDet rec = (from a in Data.tblItemDataDets
                                      where a.vt_cd == vtCd && a.vr_no == vrNo && a.SelectionId == selId
                                      select a).Single();

                return Convert.ToDecimal(rec.vr_qty + rec.vr_half/2);
            }
            catch //(Exception ex)
            {
                //RMSDB.closeConn(Data); throw ex;
            }
            return 0;
        }


        public List<tblItemDesign> GetDesign(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                List<tblItemDesign> lst = (from a in Data.tblItemDesigns
                                          select a).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }   
        }


        public List<tblItemThick> GetThickness(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                List<tblItemThick> lst = (from a in Data.tblItemThicks
                                           select a).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }


        public List<Glmf_Code> GetCustomer(RMSDataContext Data)
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
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }


        public List<tblItemColor> GetColor(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                List<tblItemColor> lst = (from a in Data.tblItemColors
                                          select a).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
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


        public bool GetIfQtyMathced(int lotNo, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                int sumFtgQty = 0;
                int sumGrdQty = 0;
                var lstFtg = (from a in Data.tblItemDatas
                           join b in Data.tblItemDataDets
                           on a.vr_no equals b.vr_no
                           where a.LotNo == lotNo && a.vt_cd == 15 && b.vt_cd == 15 && a.vr_apr =="A"
                           select new
                           {
                               a.vr_apr,
                               b.vr_qty
                           }).ToList();
                var lstGrd = (from a in Data.tblItemDatas
                           join b in Data.tblItemDataDets
                           on a.vr_no equals b.vr_no
                           where a.LotNo == lotNo && a.vt_cd == 14 && b.vt_cd == 14 && a.vr_apr == "A"
                           select new
                           {
                               a.vr_apr,
                               b.vr_qty
                           }).ToList();
                foreach (var r in lstFtg)
                {
                    sumFtgQty = sumFtgQty + Convert.ToInt32(r.vr_qty);
                }
                foreach (var p in lstGrd)
                {
                    sumGrdQty = sumGrdQty + Convert.ToInt32(p.vr_qty);
                }

                if (sumFtgQty == sumGrdQty)
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


        public bool GetIfMPExists(int lotNo, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                int count = 0;
                int countC = 0;
                List<tblItemData> lst = (from a in Data.tblItemDatas
                                         where a.LotNo == lotNo && a.vt_cd == 21
                                         select a).ToList();
                foreach (var res in lst)
                {
                    if (res.vr_apr == "A" || res.vr_apr == "P")
                    {
                        count++;
                    }
                    else
                    {
                        countC++;
                    }
                }
                if (lst.Count > 0)
                {
                    if (lst.Count - countC > 0)
                        return true;
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }


        public List<tblItemDataDet> GetObjByLot(int lotNo,int code, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                List<tblItemDataDet> obj = (from a in Data.tblItemDatas
                          join b in Data.tblItemDataDets
                          on a.vr_no equals b.vr_no
                          where a.LotNo == lotNo && a.vt_cd == code && b.vt_cd== code && a.vr_apr != "C"                          select b).ToList();
                return obj;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }


        public spGetSelectionParam4MultiLotsResult GetObjSelectionByMultiLot(int lotNo, int code, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                spGetSelectionParam4MultiLotsResult obj = Data.spGetSelectionParam4MultiLots(lotNo, code).Single();
                return obj;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

        public spGetFeetageParam4MultiLotsResult GetObjFeetageByMultiLot(int lotNo, int code, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                spGetFeetageParam4MultiLotsResult obj = Data.spGetFeetageParam4MultiLots(code, lotNo).Single();
                return obj;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

        public List<tblStkGP> GetIGPRecByLot(int lotNo, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                string docRef = (from a in Data.tblItemDatas
                                 where a.LotNo == lotNo && a.vt_cd == 14
                                 select a).Take(1).Single().DocRef.ToString();
                List<tblStkGP> rec = (from b in Data.tblStkGPs
                                      orderby b.vr_dt descending
                                      where b.DocRef == docRef && b.vt_cd == 11
                                      select b).ToList();
                return rec;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }


        public List<tblStkGP> GetIGPRecsByStrtIGP(int igpNo, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                string docRef = (from a in Data.tblStkGPs
                                 where a.vr_no == igpNo && a.vt_cd == 11
                                 select a).Take(1).Single().DocRef.ToString();
                List<tblStkGP> rec = (from b in Data.tblStkGPs
                                      orderby b.vr_dt descending
                                      where b.DocRef == docRef && b.vt_cd == 11
                                      select b).ToList();
                return rec;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

        public List<tblStkGPDet> GetIGPRecsDetailByStrtIGP(int igpNo, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                string docRef = (from a in Data.tblStkGPs
                                 where a.vr_no == igpNo && a.vt_cd == 11
                                 select a).Take(1).Single().DocRef.ToString();
                List<tblStkGPDet> rec = (from b in Data.tblStkGPs
                                         join c in Data.tblStkGPDets
                                         on b.vr_no equals c.vr_no
                                      orderby b.vr_dt descending
                                      where b.DocRef == docRef && b.vt_cd == 11 && c.vt_cd == 11
                                      select c).ToList();
                return rec;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

        public List<tblStkGP> GetIGPRecByLotUzingPMNRef(int lotNo, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                string docRef = (from a in Data.tblItemDatas
                                 where a.LotNo == lotNo && a.vt_cd == 14
                                 select a).Take(1).Single().DocRef.ToString();
                List<tblStkGP> rec = (from b in Data.tblStkGPs
                                      orderby b.vr_dt descending
                                      where b.PMN_Ref == docRef && b.vt_cd == 11
                                      select b).ToList();
                return rec;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }



        public List<spGetIGPs4MPNResult> GetIGPByDocRef(string doc_Ref,string sr, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                List<spGetIGPs4MPNResult> results = Data.spGetIGPs4MPN(doc_Ref,sr).ToList();
                return results;
                
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }


        public List<spGetIGPByLotUsingPMNRefResult> GetIGPByLotUzingPMNRef(int lotNo, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                List<spGetIGPByLotUsingPMNRefResult> results = Data.spGetIGPByLotUsingPMNRef(lotNo).ToList();
                return results;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

        public void DiscardFalseRecords(RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                var recsStk = (from r in Data.tblStkGPs
                            where r.PMN_Ref != r.PMN_RefTemp && r.vt_cd == 11 && r.vr_apr != "C"
                            select r).ToList();
                foreach (var r in recsStk)
                {
                    r.PMN_Ref = "";
                    r.PMN_RefTemp = "";
                }
                Data.SubmitChanges();
                var recsItemData = (from d in Data.tblItemDatas
                                    where d.PMN_Ref != d.PMN_RefTemp && d.vt_cd == 14 && d.vr_apr != "C"
                                    select d).ToList();
                foreach (var d in recsItemData)
                {
                    d.PMN_Ref = "";
                    d.PMN_RefTemp = "";
                }
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

        public void UpdateTblStkGP(RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

        public void UpdateTblItemData(List<tblItemData> recs, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

        public List<tblStkGP> GetRecsStkGPByStrtIGP1(int strtIGP, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                string docRef = "";
                docRef = (from a in Data.tblStkGPs
                          where a.vr_no == strtIGP && a.vt_cd == 11
                          select a).Single().DocRef;

                List<tblStkGP> recs = (from b in Data.tblStkGPs
                                       where b.DocRef == docRef && b.vt_cd == 11
                                       select b).ToList();
                return recs;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

        public List<tblStkGP> GetRecsStkGPByStrtIGP(int strtIGP, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                string docRef = "";
                docRef = (from a in Data.tblStkGPs
                          where a.vr_no == strtIGP && a.vt_cd == 11
                          select a).Single().DocRef;

                List<tblStkGP> recs = (from b in Data.tblStkGPs
                                       where b.PMN_Ref == docRef && b.vt_cd == 11   
                                       select b).ToList();
                return recs;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

        public List<tblItemData> GetRecsTblItemDataByStrtIGP(string doc_Ref, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                     List<tblItemData> recs = (from b in Data.tblItemDatas
                                                where b.PMN_Ref == doc_Ref && b.vt_cd == 14 && b.vr_apr =="A" 
                                               select b).ToList();
                return recs;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }


        public void UndoMerge(int igpNo, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                string pmnRef = "";


                tblStkGP obj = (from a in Data.tblStkGPs
                                   where a.vr_no == igpNo && a.vt_cd == 11 && a.vr_apr != "C"
                                   select a).Take(1).Single();
                pmnRef = obj.DocRef;
                pmnRef = obj.DocRef;
                pmnRef = obj.DocRef;

                List<tblItemData> lstFtg = (from a in Data.tblItemDatas
                                   where a.PMN_Ref == pmnRef && a.vt_cd == 14 && a.vr_apr != "C"
                                   select a).ToList();
                foreach (var r in lstFtg)
                {
                    r.PMN_Ref = "";
                    r.PMN_RefTemp = "";
                }
                Data.SubmitChanges();

                List<tblStkGP> lstIGP = (from a in Data.tblStkGPs
                                         where a.PMN_Ref == pmnRef && a.vt_cd == 11 && a.vr_apr != "C"
                                         select a).ToList();
                foreach (var m in lstIGP)
                {
                    m.PMN_Ref = "";
                    m.PMN_RefTemp = "";
                }
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

        public string GetDocRefByLot(int lotNo, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                string docRef = "";


                tblItemData obj = (from a in Data.tblItemDatas
                           where a.LotNo == lotNo && a.vt_cd == 14
                           select a).Take(1).Single();
                docRef = obj.DocRef;
                return docRef;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }


        public string GetDocRefByIGP(int igpNo, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                string docRef = "";


                tblStkGP obj = (from a in Data.tblStkGPs
                                   where a.vr_no == igpNo && a.vt_cd == 11
                                   select a).Take(1).Single();
                docRef = obj.DocRef;
                return docRef;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

        public Anonymous4Grid GetRecByDocRef (string docRef, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                Anonymous4Grid obj = (from a in Data.tblStkGPs
                                            join b in Data.tblStkGPDets
                                            on a.vr_no equals b.vr_no
                                            where a.DocRef == docRef && a.vt_cd == 11 && b.vt_cd == 11
                                            select new Anonymous4Grid
                                            {
                                                Doc_Ref = a.DocRef,
                                                itm_Desc = (from c in Data.tblItem_Codes
                                                           where c.itm_cd == b.Itm_cd
                                                           select c).Single().itm_dsc,
                                                Vendor = (from d in Data.Glmf_Codes
                                                          where d.gl_cd == a.VendorId
                                                          select d).Single().gl_dsc

                                                
                                            }).Take(1).Single();
                return obj;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }


        public decimal GetLotQty(int lotNo, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                List<decimal> lstQty = (from a in Data.tblItemDatas
                                            join b in Data.tblItemDataDets
                                            on a.vr_no equals b.vr_no
                                            where a.LotNo == lotNo && a.vr_apr == "A" && a.vt_cd == 14 && b.vt_cd == 14
                                            select b.vr_qty).ToList();
                decimal sum = 0;
                foreach (decimal qty in lstQty)
                {
                    sum = sum + qty;
                }
                return sum;

            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }
        

        public object GetLotRecByLotNo(int lotNo, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                var lst = Data.spGetLotRec(lotNo);
                return lst;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

        public string GetVrQty(int vrNo, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                int qty=0;
                var row = (from a in Data.tblItemDataDets
                           where a.vr_no == vrNo && a.vt_cd == 23
                           select new
                           {
                               a.vr_qty
                           }).ToList();
                foreach (var r in row)
                {
                    qty = qty + Convert.ToInt32(r.vr_qty);
                }
                return qty.ToString();
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }


        public string GetPrevQty(int ltNo, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                int sumQty = 0;
                var rows = (from a in Data.tblItemDatas
                            join b in Data.tblItemDataDets
                            on a.vr_no equals b.vr_no
                            where a.LotNo == ltNo && a.vt_cd == 23 && b.vt_cd == 23
                            select new
                            {
                                a.vr_apr,
                                b.vr_qty
                            }).ToList();

                foreach (var res in rows)
                {
                    if (res.vr_apr == "A" || res.vr_apr == "P")
                    {
                        sumQty = sumQty + Convert.ToInt32(res.vr_qty);
                    }
                }
                return sumQty.ToString();
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

        public bool CheckIfAlreadyTrnsfrd(int ltNo, string lotQty,RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                int sumQty = 0;
                var rows = (from a in Data.tblItemDatas
                            join b in Data.tblItemDataDets
                            on a.vr_no equals b.vr_no
                            where a.LotNo == ltNo && a.vt_cd == 23 && b.vt_cd == 23
                            select new
                            {
                                a.vr_apr,
                                b.vr_qty
                            }).ToList();

                foreach (var res in rows)
                {
                    if (res.vr_apr == "A" || res.vr_apr == "P")
                    {
                        sumQty = sumQty +Convert.ToInt32( res.vr_qty);
                    }
                }
                if (rows.Count > 0)
                {
                    if (sumQty == Convert.ToInt32(lotQty))
                    {
                        return true;
                    }
                    else if (sumQty < Convert.ToInt32(lotQty))
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
            catch 
            {
                return false;
            }

        }


        public bool CheckIfPendingOrNotExist(int ltNo, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                bool rt = true;
                string docRef = "";
                int igpQty = 0;
                int sum = 0;
                List<tblItemData> lst = (from t in Data.tblItemDatas
                                         where t.vt_cd == 14 && t.LotNo == ltNo && t.vr_apr == "A"
                                         select t).ToList();
                foreach (var a in lst)
                {
                    if (a.vr_apr == "P")
                    {
                        rt = false;
                        docRef = a.DocRef;
                        break;
                    }
                    docRef = a.DocRef;
                }
                if (rt == false)
                {
                    return rt;
                }

                List<tblItemDataDet> lstDet = (from t in Data.tblItemDatas
                                               join m in Data.tblItemDataDets
                                               on t.vr_no equals m.vr_no
                                               where t.vt_cd == 14 && t.LotNo == ltNo && m.vt_cd == 14 && t.vr_apr == "A"
                                               select m).ToList();
                foreach (var a in lstDet)
                {
                    sum = sum + Convert.ToInt32(a.vr_qty);
                }

                List<tblStkGPDet> igpLst = (from f in Data.tblStkGPs
                                            join p in Data.tblStkGPDets
                                                on f.vr_no equals p.vr_no
                                            where f.vt_cd == 11 && p.vt_cd == 11 && f.DocRef == docRef
                                            select p).ToList();


                foreach (var d in igpLst)
                {
                    igpQty = igpQty + Convert.ToInt32(d.vr_qty);
                }
                if (sum <= igpQty)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

        }
        //Change1
        //public bool CheckIfPendingOrNotExist(int ltNo, RMSDataContext Data)
        //{
        //    if (Data == null) { Data = RMSDB.GetOject(); }
        //    try
        //    {
        //        bool rt = true;
        //        string docRef = "";
        //        int igpQty = 0;
        //        int sum = 0;
        //        List<tblItemData> lst = (from t in Data.tblItemDatas
        //                                 where t.vt_cd == 14 && t.LotNo == ltNo && t.vr_apr == 'A'
        //                                 select t).ToList();
        //        foreach (var a in lst)
        //        {
        //            if (a.vr_apr == 'P')
        //            {
        //                rt = false;
        //                docRef = a.DocRef;
        //                break;
        //            }
        //            docRef = a.DocRef;
        //        }
        //        if (rt == false)
        //        {
        //            return rt;
        //        }
              
        //        List<tblItemDataDet> lstDet = (from t in Data.tblItemDatas
        //                                       join m in Data.tblItemDataDets
        //                                       on t.vr_no equals m.vr_no
        //                                       where t.vt_cd == 14 && t.LotNo == ltNo && m.vt_cd == 14 && t.vr_apr == 'A'
        //                                       select m).ToList();
        //        foreach (var a in lstDet)
        //        {
        //            sum = sum + Convert.ToInt32(a.vr_qty);
        //        }

        //        List<tblStkGPDet> igpLst = (from f in Data.tblStkGPs
        //                                    join p in Data.tblStkGPDets
        //                                        on f.vr_no equals p.vr_no
        //                                    where f.vt_cd == 11 && p.vt_cd == 11 && f.DocRef == docRef
        //                                    select p).ToList();


        //        foreach (var d in igpLst)
        //        {
        //            igpQty = igpQty + Convert.ToInt32(d.vr_qty);
        //        }
        //        if (sum != igpQty)
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //    catch
        //    {
        //        return false;
        //    }

        //}
        //--------------------

        public string GetItemUzngLot(int ltNo, RMSDataContext Data)
        {
            string itmCode = "0";
            string sttus = "";

            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var obj = (from r in Data.tblItemDatas
                           join d in Data.tblItemDataDets
                           on r.vr_no equals d.vr_no
                           where r.LotNo == ltNo && r.vt_cd == 14 && d.vt_cd == 14 && r.vr_apr == "A"
                           select new
                           {
                               Itm_cd = d.itm_cd,
                               LotNo = r.LotNo,
                               Status = r.vr_apr

                           }).Take(1).ToList();
                if (obj != null)
                {
                    foreach (var i in obj)
                    {
                        itmCode = i.Itm_cd;
                        sttus = i.Status.ToString();
                    }
                    if(sttus !="")
                        return sttus+":" + itmCode;
                    else
                        return "0:0";
                }
                else
                {
                    return "0:0";
                }

            }
            catch
            {
                return "0:0";
            }
        }

        public string GetProductByItmCode(string itmCode, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                tblItem_Code rec = (from a in Data.tblItem_Codes
                                   where a.itm_cd == itmCode
                                   select a).Single();
                return rec.itm_dsc.ToString();
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }


        public string GetItemUzngLot4mItemData4Cancel(int ltNo, RMSDataContext Data)
        {
            string str = "";
            if (Data == null) { Data = RMSDB.GetOject(); }
            var obj = (from r in Data.tblItemDatas
                       join d in Data.tblItemDataDets
                       on r.vr_no equals d.vr_no
                       where r.LotNo == ltNo && r.vt_cd == 21 && d.vt_cd == 21
                       select new
                       {
                           vr_val = d.vr_val,
                           vr_rate = d.vr_rate,
                           Commission = r.Commission

                       }).Take(1).Single();
            if (obj != null)
            {
                str = obj.vr_rate + ":" + obj.vr_val + ":" + obj.Commission;
            }

            return str;
        }




        public string GetItemUzngLot4mItemData(int ltNo, RMSDataContext Data)
        {
            string str = "";
            if (Data == null) { Data = RMSDB.GetOject(); }
            var obj = (from r in Data.tblItemDatas
                       join d in Data.tblItemDataDets
                       on r.vr_no equals d.vr_no
                       where r.LotNo == ltNo && r.vt_cd == 21 && d.vt_cd == 21 && r.vr_apr != "C"
                       select new
                       {
                           vr_val = d.vr_val,
                           vr_rate = d.vr_rate,
                           Commission = r.Commission

                       }).Take(1).Single();
            if (obj != null)
            
                {
                    str = obj.vr_rate + ":" + obj.vr_val + ":" + obj.Commission;
                }
            
            return str;
        }
        public decimal sumQty(int vrCode, int vrNo, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            decimal sum = 0;
            var rec = from r in Data.tblItemDataDets
                      where r.vt_cd == vrCode && r.vr_no == vrNo
                      select r;
            foreach (var r in rec)
            {
                sum = sum + r.vr_qty;
            }
            return sum;
        }
        public decimal sumArea(int vrCode, int vrNo, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            decimal sum = 0;
            var rec = from r in Data.tblItemDataDets
                      where r.vt_cd == vrCode && r.vr_no == vrNo
                      select r;
            foreach (var r in rec)
            {
                sum = sum + Convert.ToDecimal(r.Feetage);
            }
            return sum;
        }

        public tblItemData GetRecord(int vrCode, int LtNo, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            tblItemData rec = (from r in Data.tblItemDatas
                               where r.vt_cd == vrCode && r.LotNo == LtNo && r.vr_apr !="C"
                               select r).Single();
            return rec;
        }

        public List<tblItemData> GetRecordsTblItemData(int vrCode, int igpNo, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            List<tblItemData> recs = (from a in Data.tblItemDatas
                                     where a.IGPNo == igpNo && a.vt_cd == vrCode && a.vr_apr != "C"
                                     select a).ToList();
            return recs;
        }

        public tblStkGP GetRecordIGP(int vrCode, int LtNo, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            tblStkGP rec = (from r in Data.tblStkGPs
                              join d in Data.tblStkGPDets
                              on r.vr_no equals d.vr_no
                              where r.vt_cd == vrCode && d.LotNo == LtNo
                               select r).Take(1).Single();
            return rec;
        }



        public List<spGetFeetageGradingRecResult> GetFeetageGradingRec(int vrCode, int LtNo, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            List<spGetFeetageGradingRecResult> res = Data.spGetFeetageGradingRec(vrCode, LtNo).ToList();
            return res;
        }

        public List<spGetFeetageGradingRec4MultiLotsResult> GetFeetageGradingRec4MultiLots(int vrCode, int LtNo, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            List<spGetFeetageGradingRec4MultiLotsResult> res = Data.spGetFeetageGradingRec4MultiLots(vrCode, LtNo).ToList();
            return res;
        }

        public List<spGetSelectionRecResult> GetSelectionRec(int vrCode, int LtNo, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            List<spGetSelectionRecResult> res = Data.spGetSelectionRec(vrCode, LtNo).ToList();
            return res;
        }

        public List<spGetSelectionRec4MuliLotsResult> GetSelectionRec4MultiLots(int vrCode, int LtNo, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            List<spGetSelectionRec4MuliLotsResult> res = Data.spGetSelectionRec4MuliLots(vrCode, LtNo).ToList();
            return res;
        }

        public tblItemData GetRecWetBlueTrnsfr(int GPNo, int yr, int code, RMSDataContext Data)
    {
            if (Data == null) { Data = RMSDB.GetOject(); }
            tblItemData rec = (from r in Data.tblItemDatas
                               where Convert.ToInt32(r.vr_no) == GPNo && Convert.ToInt32(r.vr_no.ToString().Substring(0, 4)) == yr && r.vt_cd == code
                               select r).Single();
            return rec;
        }


        public List<tblItemDataDet> GetRecDetWetBlueTrnsfr(int sgNo, int yr, int code, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            List<tblItemDataDet> recs = (from r in Data.tblItemDataDets
                                         where Convert.ToInt32(r.vr_no) == sgNo && Convert.ToInt32(r.vr_no.ToString().Substring(0, 4)) == yr && r.vt_cd == code
                                         orderby r.vr_seq
                                         select r).ToList();
            return recs;
        }

        public tblItemData GetCardRec(int cardNo, int vrCode, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                tblItemData rec = (from a in Data.tblItemDatas
                                   where a.vr_no == cardNo && a.vt_cd == vrCode
                                   select a).Single();
                return rec;
            }
            catch //(Exception ex)
            {
                //RMSDB.closeConn(Data);
               //throw ex;
            }
            return null;
        }

        public List<tblItemDataDet> GetCardRecDet(int cardNo, int vrCode, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                List<tblItemDataDet> recs = (from a in Data.tblItemDataDets
                                             where a.vr_no == cardNo && a.vt_cd == vrCode
                                             select a).ToList();
                return recs;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
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



        public object GetGrid_MPN(DateTime fromDt, DateTime toDt, char sttus, RMSDataContext Data)
        {
            if (sttus != 'M')
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var grdObject = (from l in Data.tblItemDatas
                                 join g in Data.tblItemDataDets
                                 on l.vr_no equals g.vr_no

                                 where l.vr_dt >= fromDt && l.vr_dt <= toDt && l.vr_apr == Convert.ToString(sttus) && l.vt_cd == 21
                                 orderby l.vr_no descending
                                 select new
                                 {
                                     vr_no = l.vr_no.ToString().Substring(0,4) + "/" + l.vr_no.ToString().Substring(4),

                                     vr_dt = l.vr_dt,
                                     IGPNo = (from a in Data.tblItemDatas
                                              where l.DocRef == a.DocRef && a.vt_cd == 14
                                              select a).First().IGPNo.ToString().Substring(0, 4)
                                              + "/" +
                                              (from a in Data.tblItemDatas
                                               where l.DocRef == a.DocRef && a.vt_cd == 14
                                               select a).First().IGPNo.ToString().Substring(4),

                                     GPRef = (from b in Data.tblStkGPs
                                              join c in Data.tblStkGPDets
                                                    on b.vr_no equals c.vr_no
                                              where l.DocRef == b.DocRef && b.vt_cd == 11 && c.vt_cd == 11
                                              select c).First().GPRef,
                                     
                                     Party = (from a in Data.tblStkGPs
                                              join b in Data.Glmf_Codes
                                              on a.VendorId equals b.gl_cd
                                              where a.DocRef == l.DocRef && a.vt_cd == 11
                                              select b).First().gl_dsc,

                                     status = l.vr_apr == "P" ? "Pending" :
                                                         l.vr_apr == "A" ? "Approved" :
                                                         l.vr_apr == "C" ? "Cancelled" : "NULL"
                                 }).Distinct().ToList();

                var obj = (from o in grdObject
                           orderby Convert.ToInt32(o.IGPNo.Substring(0, 4)) descending, Convert.ToInt32(o.IGPNo.Substring(5)) descending,
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

                                 where l.vr_dt >= fromDt && l.vr_dt <= toDt && l.vt_cd == 21
                                 orderby l.vr_no descending
                                 select new
                                 {
                                     vr_no = l.vr_no.ToString().Substring(0, 4) + "/" + l.vr_no.ToString().Substring(4),

                                     vr_dt = l.vr_dt,
                                     IGPNo = (from a in Data.tblItemDatas
                                              where l.DocRef == a.DocRef && a.vt_cd == 14
                                              select a).First().IGPNo.ToString().Substring(0, 4) 
                                              + "/" + 
                                              (from a in Data.tblItemDatas
                                              where l.DocRef == a.DocRef && a.vt_cd == 14
                                              select a).First().IGPNo.ToString().Substring(4),

                                     GPRef = (from b in Data.tblStkGPs
                                              join c in Data.tblStkGPDets
                                                  on b.vr_no equals c.vr_no
                                              where l.DocRef == b.DocRef && b.vt_cd == 11 && c.vt_cd == 11
                                              select c).First().GPRef,

                                     Party = (from a in Data.tblStkGPs
                                              join b in Data.Glmf_Codes
                                              on a.VendorId equals b.gl_cd
                                              where a.DocRef == l.DocRef && a.vt_cd == 11
                                              select b).First().gl_dsc,

                                     status = l.vr_apr == "P" ? "Pending" :
                                                         l.vr_apr == "A" ? "Approved" :
                                                         l.vr_apr == "C" ? "Cancelled" : "NULL"
                                 }).Distinct().ToList();

                var obj = (from o in grdObject
                           orderby Convert.ToInt32(o.IGPNo.Substring(0, 4)) descending, Convert.ToInt32(o.IGPNo.Substring(5)) descending,
                                   Convert.ToInt32(o.vr_no.Substring(0, 4)) descending, Convert.ToInt32(o.vr_no.Substring(5)) descending 
                           select o).ToList();
                return obj;
            }
        }


        public tblStkGP GetRecStkByIGP(int igpNo, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                tblStkGP rec = (from a in Data.tblStkGPs
                                where a.vr_no == igpNo && a.vt_cd == 11
                                select a).Single();
                return rec;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }

        public tblStkGPDet GetRecStkDetByIGP(int igpNo, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                tblStkGPDet recDet = (from a in Data.tblStkGPs
                                join b in Data.tblStkGPDets
                                on a.vr_no equals b.vr_no
                                where a.vr_no == igpNo && a.vt_cd == 11 && b.vt_cd == 11
                                select b).Single();
                return recDet;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }


        public bool ChngedWetBlueTrnsfr_Status(int sgNo, int yr, int code, char sttus, string updBy, RMSDataContext Data)
        {
            try
            {
                EntitySet<tblItemDataDet> itmDetEnt = new EntitySet<tblItemDataDet>();
                int sr =1;
                ////RMSDataContext Data = new RMSDataContext();
                if (Data == null) { Data = RMSDB.GetOject(); }
                //Deleting prvious records
                var recDet = from r in Data.tblItemDataDets
                             where Convert.ToInt32(r.vr_no) == sgNo && Convert.ToInt32(r.vr_no.ToString().Substring(0, 4)) == yr && r.vt_cd == code
                             select r;

                foreach (var rcd in recDet)
                {
                    if (rcd.vr_qty + rcd.vr_half > 0)
                    {
                        tblItemDataDet det = new tblItemDataDet();
                        det.br_id = rcd.br_id;
                        det.LocId = rcd.LocId;
                        det.vt_cd = rcd.vt_cd;
                        det.vr_no = rcd.vr_no;
                        det.vr_seq = (byte) sr++;
                        det.itm_cd = rcd.itm_cd;
                        det.vr_qty = rcd.vr_qty;
                        det.vr_val = rcd.vr_val;
                        det.vr_rate = rcd.vr_rate;
                        det.Feetage = rcd.Feetage;
                        det.KGSwt = rcd.KGSwt;
                        det.GradeId = rcd.GradeId;
                        det.LotRef = rcd.LotRef;
                        det.updateon = rcd.updateon;
                        det.DrumNo = rcd.DrumNo;
                        det.vr_rmk = rcd.vr_rmk;
                        det.SelectionId = rcd.SelectionId;
                        det.IGP_Ref = rcd.IGP_Ref;
                        det.DesignId = rcd.DesignId;
                        det.ColorId = rcd.ColorId;
                        det.ThickId = rcd.ThickId;
                        det.vr_half = rcd.vr_half;
                        itmDetEnt.Add(det);
                    }
                }
                Data.tblItemDataDets.DeleteAllOnSubmit(recDet);
                Data.SubmitChanges();
                //Deleting parent
                
                tblItemData rec1 = (from r in Data.tblItemDatas
                                    where Convert.ToInt32(r.vr_no) == sgNo && Convert.ToInt32(r.vr_no.ToString().Substring(0, 4)) == yr && r.vt_cd == code
                                    select r).Single();
                

                tblItemData rec = new tblItemData();
                rec.br_id = rec1.br_id;
                rec.LocId = rec1.LocId;
                rec.vr_no = rec1.vr_no;
                rec.vt_cd = rec1.vt_cd;
                rec.vr_dt = rec1.vr_dt;
                rec.vr_nrtn = rec1.vr_nrtn;
                rec.gl_cd = rec1.gl_cd;
                rec.post_2gl = rec1.post_2gl;
                rec.LotNo = rec1.LotNo;
                rec.ToLocId = rec1.ToLocId;
                rec.Freight = rec1.Freight;
                rec.Tax = rec1.Tax;
                rec.Commission = rec1.Commission;
                rec.DocRef = rec1.DocRef;
                rec.IGPNo = rec1.IGPNo;
                rec.Status = rec1.Status;
                rec.Due_Date = rec1.Due_Date;
                rec.Pay_Status = rec1.Pay_Status;
                rec.vr_apr = Convert.ToString(sttus);
                rec.updateby = updBy;
                rec.updateon = Common.MyDate(Data);

                Data.tblItemDatas.DeleteOnSubmit(rec1);
                Data.SubmitChanges();

                //Inserting new records
                rec.tblItemDataDets = itmDetEnt;
                Data.tblItemDatas.InsertOnSubmit(rec);
                //Changes submitted
                Data.SubmitChanges();

               

                return true;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }


        public bool ChngedSG_Status(int sgNo, int yr, int code, char sttus, string updBy, RMSDataContext Data)
        {
            try
            {
                //RMSDataContext Data = new RMSDataContext();
               if (Data == null) { Data = RMSDB.GetOject(); }
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

        public object GetGrid_SG(DateTime fromDt, DateTime toDt, int code, char sttus, RMSDataContext Data)
        {
            if (sttus != 'M')
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var grdObject = (from l in Data.tblItemDatas
                                 join g in Data.tblItemDataDets
                                 on l.vr_no equals g.vr_no

                                 join c in Data.tblStock_Locs
                                 on l.LocId equals c.LocId
                                 join d in Data.tblStock_Locs
                                 on l.ToLocId equals d.LocId

                                 where l.vr_dt >= fromDt && l.vr_dt <= toDt && l.vr_apr == Convert.ToString(sttus) && l.vt_cd == code
                                 orderby l.vr_no descending
                                 select new
                                 {
                                     vr_no = l.vr_no.ToString().Substring(0, 4) + "/" + l.vr_no.ToString().Substring(4),
                                     LotNo= l.LotNo.ToString().Substring(0,4)+"-"+l.LotNo.ToString().Substring(4),
                                     vr_dt = l.vr_dt,
                                     product = (from a in Data.tblItem_Codes
                                                where a.itm_cd == g.itm_cd
                                                    select a).Single().itm_dsc,
                                     design = (from a in Data.tblItemDesigns
                                                   where a.DesignId == g.DesignId
                                                   select a).Single().Design_Desc,
                                     color = (from a in Data.tblItemColors
                                              where a.ColorId == g.ColorId
                                              select a).Single().Color,
                                     thick = (from a in Data.tblItemThicks
                                              where a.ThickId == g.ThickId
                                              select a).Single().Thick_Desc,
                                     fromLoc = c.LocName,
                                     toLoc = d.LocName,
                                     status = l.vr_apr == "P" ? "Pending" :
                                                         l.vr_apr == "A" ? "Approved" :
                                                         l.vr_apr == "C" ? "Cancelled" : "NULL"
                                 }).Distinct().ToList();
                //var obj = (from o in grdObject
                //           orderby Convert.ToInt32(o.LotNo.Substring(0, 4) + o.LotNo.Substring(5)) descending
                //           select o).ToList();

                var obj = (from o in grdObject
                           orderby Convert.ToInt32(o.LotNo.Substring(0, 2)) descending, Convert.ToInt32(o.LotNo.Substring(2, 2)) descending, Convert.ToInt32(o.LotNo.Substring(5)) descending
                           select o).ToList();
                return obj;
            }
            else
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var grdObject = (from l in Data.tblItemDatas
                                 join g in Data.tblItemDataDets
                                 on l.vr_no equals g.vr_no

                                 join c in Data.tblStock_Locs
                                 on l.LocId equals c.LocId
                                 join d in Data.tblStock_Locs
                                 on l.ToLocId equals d.LocId

                                 where l.vr_dt >= fromDt && l.vr_dt <= toDt && l.vt_cd == code
                                 orderby l.vr_no descending
                                 select new
                                 {
                                     vr_no = l.vr_no.ToString().Substring(0, 4) + "/" + l.vr_no.ToString().Substring(4),
                                     LotNo = l.LotNo.ToString().Substring(0, 4) + "-" + l.LotNo.ToString().Substring(4),
                                     vr_dt = l.vr_dt,
                                     product = (from a in Data.tblItem_Codes
                                                where a.itm_cd == g.itm_cd
                                                select a).Single().itm_dsc,
                                     design = (from a in Data.tblItemDesigns
                                               where a.DesignId == g.DesignId
                                               select a).Single().Design_Desc,
                                     color = (from a in Data.tblItemColors
                                              where a.ColorId == g.ColorId
                                              select a).Single().Color,
                                     thick = (from a in Data.tblItemThicks
                                              where a.ThickId == g.ThickId
                                              select a).Single().Thick_Desc,
                                     fromLoc = c.LocName,
                                     toLoc = d.LocName,
                                     status = l.vr_apr == "P" ? "Pending" :
                                                         l.vr_apr == "A" ? "Approved" :
                                                         l.vr_apr == "C" ? "Cancelled" : "NULL"
                                 }).Distinct().ToList();
                //var obj = (from o in grdObject
                //           orderby Convert.ToInt32(o.LotNo.Substring(0, 4) + o.LotNo.Substring(5)) descending
                //           select o).ToList();
                var obj = (from o in grdObject
                           orderby Convert.ToInt32(o.LotNo.Substring(0, 2)) descending, Convert.ToInt32(o.LotNo.Substring(2, 2)) descending, Convert.ToInt32(o.LotNo.Substring(5)) descending
                           select o).ToList();
                return obj;
            }
        }



        public object GetGrid_Records(DateTime fromDt, DateTime toDt, int code, char sttus, RMSDataContext Data)
        {
            if (sttus != 'M')
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var grdObject = (from l in Data.tblItemDatas
                                 join g in Data.tblItemDataDets
                                 on l.vr_no equals g.vr_no

                                 join c in Data.tblStock_Locs
                                 on l.LocId equals c.LocId
                                 join d in Data.tblStock_Locs
                                 on l.ToLocId equals d.LocId

                                 where l.vr_dt >= fromDt && l.vr_dt <= toDt && l.vr_apr == Convert.ToString(sttus) && l.vt_cd == code
                                 orderby l.vr_no descending
                                 select new
                                 {
                                     vr_no = l.vr_no.ToString().Substring(0, 4) + "/" + l.vr_no.ToString().Substring(4),
                                     LotNo = l.LotNo.ToString().Substring(0, 4) + "-" + l.LotNo.ToString().Substring(4),
                                     vr_dt = l.vr_dt,
                                     fromLoc = c.LocName,
                                     toLoc = d.LocName,
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
            else
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var grdObject = (from l in Data.tblItemDatas
                                 join g in Data.tblItemDataDets
                                 on l.vr_no equals g.vr_no

                                 join c in Data.tblStock_Locs
                                 on l.LocId equals c.LocId
                                 join d in Data.tblStock_Locs
                                 on l.ToLocId equals d.LocId

                                 where l.vr_dt >= fromDt && l.vr_dt <= toDt && l.vt_cd == code
                                 orderby l.vr_no descending
                                 select new
                                 {
                                     vr_no = l.vr_no.ToString().Substring(0, 4) + "/" + l.vr_no.ToString().Substring(4),
                                     LotNo = l.LotNo.ToString().Substring(0, 4) + "-" + l.LotNo.ToString().Substring(4),
                                     vr_dt = l.vr_dt,
                                     fromLoc = c.LocName,
                                     toLoc = d.LocName,
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


        public object GetGrid_WetBlueTrnsfr(DateTime fromDt, DateTime toDt,int code, int locId,  char sttus, RMSDataContext Data)
        {
            if (sttus != 'M')
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var grdObject = (from l in Data.tblItemDatas
                                 join g in Data.tblItemDataDets
                                 on l.vr_no equals g.vr_no

                                 join c in Data.tblStock_Locs
                                 on l.LocId equals c.LocId
                                 join d in Data.tblStock_Locs
                                 on l.ToLocId equals d.LocId

                                 where l.vr_dt >= fromDt && l.vr_dt <= toDt && l.vt_cd == code && g.vt_cd == code && l.vr_apr == Convert.ToString(sttus)

                                 orderby l.vr_no descending
                                 select new
                                 {
                                     vr_no = l.vr_no.ToString().Substring(0, 4) + "/" + l.vr_no.ToString().Substring(4),

                                     vr_dt = l.vr_dt,
                                     product = (from a in Data.tblItem_Codes
                                                where a.itm_cd == g.itm_cd
                                                select a).Single().itm_dsc,
                                     design  = (from a in Data.tblItemDesigns
                                                where a.DesignId == g.DesignId
                                                select a).Single().Design_Desc,
                                     color   = (from a in Data.tblItemColors
                                                where a.ColorId == g.ColorId
                                                select a).Single().Color,
                                     thick   = (from a in Data.tblItemThicks
                                                where a.ThickId == g.ThickId
                                                select a).Single().Thickness,

                                     fromLoc = c.LocName,
                                     toLoc = d.LocName,
                                     status = l.vr_apr == "P" ? "Pending" :
                                                         l.vr_apr == "A" ? "Approved" :
                                                         l.vr_apr == "C" ? "Cancelled" : "NULL"
                                 }).Distinct().ToList();
               
                var obj = (from o in grdObject
                           orderby Convert.ToInt32(o.vr_no.Substring(0, 4)) descending, Convert.ToInt32(o.vr_no.Substring(5)) descending
                           select o).ToList();
                return obj;
            }
            else
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var grdObject = (from l in Data.tblItemDatas
                                 join g in Data.tblItemDataDets
                                 on l.vr_no equals g.vr_no

                                 join c in Data.tblStock_Locs
                                 on l.LocId equals c.LocId
                                 join d in Data.tblStock_Locs
                                 on l.ToLocId equals d.LocId

                                 where l.vr_dt >= fromDt && l.vr_dt <= toDt && l.vt_cd == code && g.vt_cd == code 
                                
                                 orderby l.vr_no descending
                                 select new
                                 {
                                     vr_no = l.vr_no.ToString().Substring(0, 4) + "/" + l.vr_no.ToString().Substring(4),
                                     vr_dt = l.vr_dt,
                                     product = (from a in Data.tblItem_Codes
                                                where a.itm_cd == g.itm_cd
                                                select a).Single().itm_dsc,
                                     design = (from a in Data.tblItemDesigns
                                               where a.DesignId == g.DesignId
                                               select a).Single().Design_Desc,
                                     color = (from a in Data.tblItemColors
                                              where a.ColorId == g.ColorId
                                              select a).Single().Color,
                                     thick = (from a in Data.tblItemThicks
                                              where a.ThickId == g.ThickId
                                              select a).Single().Thickness,

                                     fromLoc = c.LocName,
                                     toLoc = d.LocName,
                                     status = l.vr_apr == "P" ? "Pending" :
                                                         l.vr_apr == "A" ? "Approved" :
                                                         l.vr_apr == "C" ? "Cancelled" : "NULL"
                                 }).Distinct().ToList();

                var obj = (from o in grdObject
                           orderby Convert.ToInt32(o.vr_no.Substring(0, 4)) descending, Convert.ToInt32(o.vr_no.Substring(5)) descending
                           select o).ToList();
                return obj;
            }
        }


        public List<spGetFeetageParamByLotNoResult> GetFeetCardPara(int vrCode, int ltNo, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                List<spGetFeetageParamByLotNoResult> lst = Data.spGetFeetageParamByLotNo(vrCode, ltNo).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

        public List<tblItemDataDet> GetItemDetUsingVrNo(int vrNo, int vtCd, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                List<tblItemDataDet> lst = (from a in Data.tblItemDataDets
                                            where a.vr_no == vrNo && a.vt_cd == vtCd && a.vt_cd != 'C'
                                            select a).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                //RMSDB.closeConn(Data);
                throw ex;
            }
        }


        public tblItem GetItemUsingAttributes(string itmCode, string selectionId, int locId, string desid, decimal glyear,RMSDataContext Data1)
        {
            try
            {
                //if (Data == null) { Data = RMSDB.GetOject(); }
                RMSDataContext Data = new RMSDataContext();
                tblItem itm = (from a in Data.tblItems
                               where a.itm_cd == itmCode && a.SelectionId == selectionId && a.LocId == locId && a.DesignId == desid
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

        public tblItem GetItemUsingSelection(string itmCode, string selectionId, int locId, RMSDataContext Data1)
        {
            try
            {
                //if (Data == null) { Data = RMSDB.GetOject(); }
                RMSDataContext Data = new RMSDataContext();
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

        public tblItem GetItemUsingSelectionDesgin(string itmCode, string selectionId, string desId,int locId, RMSDataContext Data1)
        {
            try
            {
                //if (Data == null) { Data = RMSDB.GetOject(); }
                RMSDataContext Data = new RMSDataContext();
                tblItem itm = (from a in Data.tblItems
                               where a.itm_cd == itmCode && a.SelectionId == selectionId && a.LocId == locId && a.DesignId == desId
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

      
        public bool GetInsrtTblItemCrustTrnsfr(int brid, int locId, string itmCd, string selId, int purQty, int purhalf, decimal ftg, string desid, int glyear, RMSDataContext Data1)
        {
            try
            {
                //if (Data == null) { Data = RMSDB.GetOject(); }
                RMSDataContext Data = new RMSDataContext();
                Data.spInsertxTblItem4CrustTrnsfr(brid, locId, itmCd, selId, purQty, purhalf, ftg, desid, glyear);
                return true;
            }
            catch (Exception ex)
            {
                //RMSDB.closeConn(Data);
                throw ex;
            }
        }

        public bool GetUpdTblItemCrustTrnsfr(int locId, string itmCd, string selId, int isuQty, int isuhalf, decimal ftg, string desid,  decimal glyear, RMSDataContext Data1)
        {
            try
            {
                //if (Data == null) { Data = RMSDB.GetOject(); }
                RMSDataContext Data = new RMSDataContext();
                Data.spUpdateTblItem4CrustTrnsr(locId, itmCd, selId,  desid, glyear, isuQty, isuhalf, ftg);
                return true;
            }
            catch (Exception ex)
            {
                //RMSDB.closeConn(Data);
                throw ex;
            }
        }

        public bool GetUpdTblItemWetBlue(int locId, string itmCd, string selId, string desId, int isuQty, int isuhalf, RMSDataContext Data1)
        {
            try
            {
                //if (Data == null) { Data = RMSDB.GetOject(); }
                RMSDataContext Data = new RMSDataContext();
                //Data.spUpdateTblItem4WetBlue(locId, itmCd, selId, isuQty,isuhalf, desId);
                tblItem rec = (from a in Data.tblItems
                               where a.LocId == locId &&
                                     a.itm_cd == itmCd &&
                                     a.SelectionId == selId
                               select a).Single();
                rec.itm_isu_qty = rec.itm_isu_qty + isuQty;
                rec.itm_isu_half = rec.itm_isu_half + isuhalf;

                Data.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                //RMSDB.closeConn(Data);
                throw ex;
            }
        }



        public bool GetUpdTblItemWetBlue1(int locId, string itmCd, string selId, string desId, int isuQty, int isuhalf, RMSDataContext Data1)
        {
            RMSDataContext Data = new RMSDataContext();
            try
            {
                //if (Data == null) { Data = RMSDB.GetOject(); }
                 tblItem rec = (from  a in Data.tblItems
                               where a.LocId == locId &&
                                     a.DesignId == desId &&
                                     a.itm_cd == itmCd
                               select a).First();

                rec.itm_isu_qty = rec.itm_isu_qty + isuQty;
                rec.itm_isu_half =rec.itm_isu_half + isuhalf;

                Data.SubmitChanges();
                return true;
            }
            catch
            {
            }
            return false;
        }




        public bool GetUpdTblItemDispatch(int locId, string itmCd, string selId, string desId, int isuQty, int isuhalf, decimal ftg, RMSDataContext Data1)
        {
            RMSDataContext Data = new RMSDataContext();
            try
            {
                //if (Data == null) { Data = RMSDB.GetOject(); }
                tblItem rec = (from a in Data.tblItems
                               where a.LocId == locId &&
                                     a.DesignId == desId &&
                                     a.SelectionId == selId &&
                                     a.itm_cd == itmCd
                               select a).First();

                rec.itm_isu_qty = rec.itm_isu_qty + isuQty;
                rec.itm_isu_half = rec.itm_isu_half + isuhalf;
                rec.itm_isu_sqft = rec.itm_isu_sqft + ftg;

                Data.SubmitChanges();
                return true;
            }
            catch
            {
            }
            return false;
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

        public bool EditRecWetBlu(int sgNo, int yr, int code, tblItemData sG, EntitySet<tblItemDataDet> sgDet, RMSDataContext Data)
        {
            try
            {
                //RMSDataContext Data = new RMSDataContext();
                if (Data == null) { Data = RMSDB.GetOject(); }
                //Deleting prvious records
                var recDet = from r in Data.tblItemDataDets
                             where Convert.ToInt32(r.vr_no) == sgNo && Convert.ToInt32(r.vr_no.ToString().Substring(0, 4)) == yr && r.vt_cd == code
                             select r;
                Data.tblItemDataDets.DeleteAllOnSubmit(recDet);
                Data.SubmitChanges();
                //Deleting parent
                if (Data == null) { Data = RMSDB.GetOject(); }
                tblItemData rec1 = (from r in Data.tblItemDatas
                                    where Convert.ToInt32(r.vr_no) == sgNo && Convert.ToInt32(r.vr_no.ToString().Substring(0, 4)) == yr && r.vt_cd == code
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
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
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
                if (Data == null) { Data = RMSDB.GetOject(); }
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
                //throw ex;
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



        public List<tblStkGrade> GetSizeGradeCode(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var lst = from l in Data.tblStkGrades
                          //orderby l.SizeGrade_Desc
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


        public tblItemDesign GetDesignByID(string desId, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                tblItemDesign design = (from a in Data.tblItemDesigns
                                     where a.DesignId == desId
                                     select a).Single();
                return design;
            }
            catch //(Exception ex)
            {
                //RMSDB.closeConn(Data);
               //throw ex;
            }
            return null;
        }




        public List<tblItem> GetSelectionByDesignId(string itmCode, string desId, int locId, RMSDataContext Data1)
        {
            try
            {
                //if (Data == null) { Data = RMSDB.GetOject(); }
                RMSDataContext Data = new RMSDataContext();
                List<tblItem> lst = (from a in Data.tblItems
                                     where a.DesignId == desId && a.LocId == locId && a.itm_cd == itmCode
                                     select a).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data1);
                throw ex;
            }
        }



        public spGetCrustPendingQtyResult GetCrustDeptPendingQty(string itmCode, string desId, int locId, RMSDataContext Data1)
        {
            try
            {
                //if (Data == null) { Data = RMSDB.GetOject(); }
                RMSDataContext Data = new RMSDataContext();
                spGetCrustPendingQtyResult rec = (Data.spGetCrustPendingQty(locId, itmCode, desId, "P")).Single();
                return rec;
            }
            catch //(Exception ex)
            {
                //RMSDB.closeConn(Data1);
               //throw ex;
            }
            return null;
        }


        public spGetDispatchValResult GetDispatchPendingQty(string itmCode, string desId, int locId, string selid, RMSDataContext Data1)
        {
            try
            {
                //if (Data == null) { Data = RMSDB.GetOject(); }
                RMSDataContext Data = new RMSDataContext();
                spGetDispatchValResult rec = (Data.spGetDispatchVal(locId, itmCode, desId, "C")).Where(i=> i.selectionid == selid).Single();
                return rec;
            }
            catch //(Exception ex)
            {
                //RMSDB.closeConn(Data1);
                //throw ex;
            }
            return null;
        }


        public tblItemDataDet GetDispatchCurrentRecDet(int vrCode, int vrNo, int locId,string itmCode, string desId, string selid, RMSDataContext Data1)
        {
            try
            {
                //if (Data == null) { Data = RMSDB.GetOject(); }
                RMSDataContext Data = new RMSDataContext();
                tblItemDataDet rec = Data.tblItemDataDets.Where(i => i.vt_cd == Convert.ToInt16(vrCode) && i.vr_no == vrNo && i.LocId == locId && i.itm_cd == itmCode && i.DesignId == desId && i.SelectionId == selid).Single();
                return rec;
            }
            catch //(Exception ex)
            {
                //RMSDB.closeConn(Data1);
                //throw ex;
            }
            return null;
        }

        public string GetSelectionDesc(string itmCode ,string selId, int locId, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                tblStkGradeDet rec = (from a in Data.tblStkGradeDets
                                        where a.itm_cd == itmCode && a.SelectionId == selId && a.LocId == locId
                                        select a).Single();
                return rec.SizeGrade_Desc;
            }
            catch //(Exception ex)
            {
                //RMSDB.closeConn(Data);
                //throw ex;
            }
            return "";
        }



        public List<tblStkGradeDet> GetGrades(string itmCode, int ToLocId, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                List<tblStkGradeDet> recs = (from a in Data.tblStkGradeDets
                                             where a.itm_cd == itmCode && a.LocId == ToLocId
                                             select a).ToList();
                return recs;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }

        public decimal GetAvgFeetage(string mpnDocRef, string igpDocRef, RMSDataContext Data)
        {

            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                decimal avgFtg = 0;

                List<spGetAvgFeetageResult> lst = Data.spGetAvgFeetage(mpnDocRef).ToList();

                if (lst.Count() > 0)
                {
                    foreach (var r in lst)
                    {
                        if (r.docref == igpDocRef)
                        {
                            avgFtg = Convert.ToDecimal(r.avgftg);
                        }
                    }
                    return avgFtg;
                }
                else
                {
                    return 0;
                }
                
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }

    }
}
