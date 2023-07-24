using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Linq;

namespace RMS.BL
{
    public class StoreRetBL
    {
        public StoreRetBL()
        { }

        public Object GetAllStoreRets(string retDocNo, string issDocNo, string status, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            var stkdataList = Data.Sp_Filter_StoreReturn(retDocNo, issDocNo, status).ToList();
                              //from stkD in Data.tblStkDatas
                              //where stkD.vt_cd == 18
                              //orderby stkD.vr_no descending
                              //select new
                              //{
                              //    stkD.vr_no,
                              //    stkD.DocRef,
                              //    stkD.vr_dt,
                              //    stkD.vr_apr,
                              //    stkD.LocId,
                              //    stkD.vt_cd,
                              //    stkD.br_id,
                              //    stkD.vr_id,
                              //    stkD.IGPNo

                              //};

            return stkdataList;
        }
        public Object GetAllMatIssuedDocNos(string igp, int brId, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            List<tblStkData> stkdatalist = (from stkD in Data.tblStkDatas
                                            where stkD.br_id == brId && stkD.vt_cd == 18 && stkD.vr_apr != "C"
                                            select stkD).ToList();

            List<Anonymous4MatRec> stkdataMatIssList = (from stkD in Data.tblStkDatas
                                                        join vend in Data.tblPlCodes
                                                        on stkD.DeptId equals vend.CodeID
                                                        where stkD.br_id == brId && stkD.vt_cd == 17 
                                                        && stkD.vr_no.ToString() ==  (igp == "" ? stkD.vr_no.ToString() : igp)
                                                        && stkD.vr_apr.ToString().Equals("A")
                                                        orderby stkD.vr_no descending
                                                        select new Anonymous4MatRec
                                                        {
                                                            vr_id = stkD.vr_id,
                                                            vr_dt = stkD.vr_dt,
                                                            vr_no = stkD.vr_no,
                                                            DeptNme = vend.CodeDesc,
                                                            LocName = stkD.LocId > 0 ? Data.tblStock_Locs.Where(lc=> lc.LocId.Equals(stkD.LocId)).SingleOrDefault().LocName : ""

                                                        }).ToList();

            decimal totQty = 0;
            decimal totIss = 0;
            int igpn = 0;
            List<Int32> igpnoList = new List<Int32>();

            tblStkData[] s = stkdatalist.ToArray();

            for (int i = 0; i < s.Length; i++)
            {
                igpn = s[i].IGPNo.Value;
                totQty = 0;
                totIss = 0;
                foreach (tblStkData st in stkdatalist)
                {
                    if (st.IGPNo == igpn)
                    {
                        foreach (tblStkDataDet d in st.tblStkDataDets)
                        {
                            totQty = totQty + d.vr_qty;
                            totIss = d.vr_qty_Rej;
                        }
                    }
                }

                if (totQty == totIss)
                {
                    igpnoList.Add(igpn);
                }
            }

            var finalFilteredList = from fin in stkdataMatIssList
                                    where !(igpnoList.Any(p => p.ToString().Equals(fin.vr_no.ToString())))
                                    orderby fin.vr_no descending
                                    select fin;

            return finalFilteredList;
            //return stkdataMatIssList;
        }
        public byte GetUOMIdFromLabel(string uomcd, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Convert.ToByte(Data.Item_Uoms.Where(p => p.uom_dsc.Equals(uomcd)).Single().uom_cd);
        }
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
        public string GetVendorNmeFromIGP(int igpno, int locid, int vt_cd, int brId, RMSDataContext Data)
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
        public string GetUOMDescFromID(byte uomid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Data.Item_Uoms.Where(p => p.uom_cd == uomid).Single().uom_dsc;
        }
        public List<Cost_Center> GetAllCostCenter(RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Data.Cost_Centers.ToList();
        }
        
        public string GetIssueBalance(int docNoIss, int returnNo, string itmCode, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            decimal currentReturn = 0;
            var CurrentStk = from a in Data.tblStkDatas
                             join b in Data.tblStkDataDets
                             on a.vr_id equals b.vr_id
                             where a.vr_no == returnNo && a.vt_cd == 18 && !a.vr_apr.Equals("C") && b.itm_cd == itmCode
                             select b;
            foreach (tblStkDataDet det in CurrentStk)
            {
                currentReturn = currentReturn + det.vr_qty;
            }
            
            
            
            var stkList = from stks in Data.tblStkDatas
                          where stks.IGPNo == docNoIss && !stks.vr_apr.ToString().Equals("C") && stks.vt_cd == 18
                          select stks;

            if (stkList.Count() > 0)
            {
                decimal tot = 0;
                decimal issTot = 0;
                foreach (tblStkData st in stkList)
                {
                    foreach (tblStkDataDet det in st.tblStkDataDets)
                    {
                        if (det.itm_cd == itmCode)
                        {
                            tot = tot + det.vr_qty;
                            issTot = det.vr_qty_Rej;
                        }
                    }
                }
               
                tot = issTot - tot + currentReturn;
                return tot.ToString();
            }
            else
            {
                return "-1";
            }
        }
        
        public tblStkData GetByID4MatIssueStkData(int vrid, RMSDataContext Data)
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
        public string GetItemNameFromCode(string itemcode, RMSDataContext Data)
        {
            return Data.tblItem_Codes.Single(p => p.itm_cd.Equals(itemcode)).itm_dsc;
        }
        public string GetCostCenterFromCode(string cccode, RMSDataContext Data)
        {
            return Data.Cost_Centers.Single(p => p.cc_cd.Equals(cccode)).cc_nme;
        }
        
        public string SaveStRetFull(tblStkData stkData, RMSDataContext Data)
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
                //FIN_PERD fin = null;
                decimal voh = 0, retVal = 0, qoh = 0;
                DateTime date = Common.MyDate(Data);
                //fin = (from a in Data.FIN_PERDs
                //       where a.Start_Date <= date.Date && a.End_Date >= date.Date
                //       select a).Single();

                Data.tblStkDatas.InsertOnSubmit(stkData);
                Data.SubmitChanges();
                
                if (stkData.vr_apr.ToString().Equals("A"))
                {
                    foreach (tblStkDataDet det in stkData.tblStkDataDets)
                    {
                        //stk = Data.tblStks.Single(p => p.itm_cd.Equals(det.itm_cd) && p.br_id == stkData.br_id && p.LocId == stkData.LocId);
                        //voh = stk.itm_op_val + stk.itm_pur_val - stk.itm_isu_val;
                        //qoh = stk.itm_op_qty + stk.itm_pur_qty - stk.itm_isu_qty;

                        //retVal = (qoh != 0 ? Math.Round(det.vr_qty * (voh / qoh), 2) : 0);

                        //stk.itm_isu_qty = stk.itm_isu_qty - det.vr_qty; // qty
                        //stk.itm_isu_val = stk.itm_isu_val - retVal; // value

                        //// adding issue val to tblStkDataDet also
                        //det.vr_val = retVal;

                        stk = Data.tblStks.Single(p => p.itm_cd.Equals(det.itm_cd) && p.br_id == stkData.br_id && p.LocId == stkData.LocId);
                        voh = stk.itm_op_val + stk.itm_pur_val - stk.itm_isu_val;
                        qoh = stk.itm_op_qty + stk.itm_pur_qty - stk.itm_isu_qty;

                        if (qoh == 0 && stk.itm_pur_qty > 0 && stk.itm_isu_qty > 0 && stk.itm_pur_qty == stk.itm_isu_qty)
                        {
                            retVal = Math.Round(det.vr_qty * (stk.itm_isu_val / stk.itm_isu_qty), 2);
                        }
                        else
                        {
                            retVal = (qoh != 0 ? Math.Round(det.vr_qty * (voh / qoh), 2) : 0);
                        }

                        stk.itm_isu_qty = stk.itm_isu_qty - det.vr_qty; // qty
                        stk.itm_isu_val = stk.itm_isu_val - retVal; // value

                        // adding return val to tblStkDataDet also
                        det.vr_val = retVal;

                        Data.SubmitChanges();
                    }
                }
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
        public string UpdStRetFull(tblStkData stkData, EntitySet<tblStkDataDet> stkDataDets, HashSet<string[]> hashset, RMSDataContext Data)
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
                //FIN_PERD fin = null;
                decimal voh = 0, retVal = 0, qoh = 0;
                DateTime date = Common.MyDate(Data);
                //fin = (from a in Data.FIN_PERDs
                //       where a.Start_Date <= date.Date && a.End_Date >= date.Date
                //       select a).Single();

                var dets = from p in Data.tblStkDataDets
                           where p.vr_id == stkData.vr_id
                           select p;

                string[] tempArr = null;
                foreach (tblStkDataDet det in dets)
                {
                    foreach (Object obj in hashset)
                    {
                        tempArr = (string[])obj;

                       if (tempArr[0].Equals(det.vr_id.ToString()) && tempArr[1].Equals(det.vr_seq.ToString()))
                       {
                           foreach (tblStkDataDet newDet in stkDataDets)
                           {
                               if (det.vr_seq == newDet.vr_seq)
                               {
                                   decimal totalIssued = det.vr_qty_Rej;
                                   decimal balance = Convert.ToDecimal(tempArr[2]);
                                   if (newDet.vr_qty > det.vr_qty)
                                   {
                                       if (balance + det.vr_qty >= newDet.vr_qty)
                                       {
                                           // OK
                                       }
                                       else
                                       {
                                           return "QTY_EXCEEDED";
                                       }
                                   }
                               }
                           }
                       }
                    }
                }
                Data.tblStkDataDets.DeleteAllOnSubmit(dets);

                stkData.tblStkDataDets = stkDataDets;

                Data.SubmitChanges();

                if (stkData.vr_apr.ToString().Equals("A"))
                {
                    foreach (tblStkDataDet det in stkData.tblStkDataDets)
                    {
                        //stk = Data.tblStks.Single(p => p.itm_cd.Equals(det.itm_cd) && p.br_id == stkData.br_id && p.LocId == stkData.LocId);
                        //voh = stk.itm_op_val + stk.itm_pur_val - stk.itm_isu_val;
                        //qoh = stk.itm_op_qty + stk.itm_pur_qty - stk.itm_isu_qty;
                        //issueVal = (qoh != 0 ?  Math.Round(det.vr_qty * (voh / qoh), 2) : 0);

                        //stk.itm_isu_qty = stk.itm_isu_qty - det.vr_qty; // qty
                        //stk.itm_isu_val = stk.itm_isu_val - issueVal; // value

                        //// adding issue val to tblStkDataDet also
                        //det.vr_val = issueVal;

                        stk = Data.tblStks.Single(p => p.itm_cd.Equals(det.itm_cd) && p.br_id == stkData.br_id && p.LocId == stkData.LocId);
                        voh = stk.itm_op_val + stk.itm_pur_val - stk.itm_isu_val;
                        qoh = stk.itm_op_qty + stk.itm_pur_qty - stk.itm_isu_qty;

                        if (qoh == 0 && stk.itm_pur_qty > 0 && stk.itm_isu_qty > 0 && stk.itm_pur_qty == stk.itm_isu_qty)
                        {
                            retVal = Math.Round(det.vr_qty * (stk.itm_isu_val / stk.itm_isu_qty), 2);
                        }
                        else
                        {
                            retVal = (qoh != 0 ? Math.Round(det.vr_qty * (voh / qoh), 2) : 0);
                        }

                        stk.itm_isu_qty = stk.itm_isu_qty - det.vr_qty; // qty
                        stk.itm_isu_val = stk.itm_isu_val - retVal; // value

                        // adding return val to tblStkDataDet also
                        det.vr_val = retVal;

                        Data.SubmitChanges();
                    }
                }
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
        public List<Depart_ment> GetAllDepartment(RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Data.Depart_ments.ToList();
        }
    }
}