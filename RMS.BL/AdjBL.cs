using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Linq;

namespace RMS.BL
{
    public class AdjBL
    {
        public AdjBL()
        { }

        public List<Depart_ment> GetAllDepartment(RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Data.Depart_ments.ToList();
        }
        public List<Cost_Center> GetAllCostCenter(RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Data.Cost_Centers.ToList();
        }
        public decimal GetQtyOnHandofItem(string itemCode, int brId, int locId, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                DateTime date = Common.MyDate(Data);
                //FIN_PERD fin = fin = (from a in Data.FIN_PERDs
                //                      where a.Start_Date <= date.Date && a.End_Date >= date.Date
                //                      select a).Single();

                tblStk stk = Data.tblStks.Single(p => p.itm_cd.Equals(itemCode) && p.br_id == brId && p.LocId == locId );
                
                return stk.itm_op_qty + stk.itm_pur_qty - stk.itm_isu_qty;
            }
            catch 
            {
                return 0;
            }
        }

        public decimal GetValueOnHandofItem(string itemCode, int brId, int locId, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                DateTime date = Common.MyDate(Data);
                //FIN_PERD fin = fin = (from a in Data.FIN_PERDs
                //                      where a.Start_Date <= date.Date && a.End_Date >= date.Date
                //                      select a).Single();

                tblStk stk = Data.tblStks.Single(p => p.itm_cd.Equals(itemCode) && p.br_id == brId && p.LocId == locId);

                return stk.itm_op_val + stk.itm_pur_val - stk.itm_isu_val;
            }
            catch
            {
                return 0;
            }
        }
        
        public Object GetAllAdjNote(string docNo, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            var matIssList = Data.Sp_Filter_AdjNote(docNo).ToList();
          

            return matIssList;
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
            return Data.Item_Uoms.Where(p=>p.uom_cd == uomid).Single().uom_dsc;
        }


        public object GetEmpByDept(int deptid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            //return Data.tblPlEmpDatas.Where(emp => emp.DeptID == deptid).ToList();
            var obj = (from a in Data.tblPlEmpDatas
                       where a.DeptID == deptid
                       select new
                       {
                           EmpID = a.EmpID,
                           FullName = a.FullName,
                           Name = a.EmpCode + " / " + a.FullName
                       }).OrderBy(emp => emp.FullName).ToList();
            return obj;
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

        public List<spMatIssResult> GetMatIssRec(int vrid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                return Data.spMatIss(vrid).ToList();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public byte GetUOMIdFromLabel(string uomcd, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Convert.ToByte(Data.Item_Uoms.Where(p => p.uom_dsc.Equals(uomcd)).Single().uom_cd);
        }
        public byte GetUOMIdFromItmCode(string itmcode, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Convert.ToByte(Data.tblItem_Codes.Where(p => p.itm_cd.Equals(itmcode)).Single().uom_cd);
        }
        public string SaveAdjNoteFull(tblStkData stkD, RMSDataContext Data)
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
                DateTime date = Common.MyDate(Data);
            
                Data.tblStkDatas.InsertOnSubmit(stkD);
                Data.SubmitChanges();

                if (!stkD.vr_apr.ToString().Equals("C"))
                {
                    foreach (tblStkDataDet det in stkD.tblStkDataDets)
                    {
                        stk = Data.tblStks.Single(p => p.itm_cd.Equals(det.itm_cd) && p.br_id == stkD.br_id && p.LocId == stkD.LocId);

                        stk.itm_pur_qty = stk.itm_pur_qty + det.vr_qty;
                        stk.itm_pur_val = stk.itm_pur_val + det.vr_val;

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
        public string UpdateAdjNoteFull(tblStkData stkData, EntitySet<tblStkDataDet> newDets, RMSDataContext Data)
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
                DateTime date = Common.MyDate(Data);

                List<tblStkDataDet> existingDets = (from p in Data.tblStkDataDets
                                                    where p.vr_id == stkData.vr_id
                                                    select p).ToList();
                foreach (tblStkDataDet det in existingDets)//Change back stock status
                {
                    stk = Data.tblStks.Single(p => p.itm_cd.Equals(det.itm_cd) && p.br_id == stkData.br_id && p.LocId == stkData.LocId);

                    stk.itm_pur_qty = stk.itm_pur_qty - det.vr_qty;
                    stk.itm_pur_val = stk.itm_pur_val - det.vr_val;

                    Data.SubmitChanges();
                }
                
                if (!stkData.vr_apr.ToString().Equals("C"))//Change again stock status with new entries
                {
                    foreach (tblStkDataDet det in newDets)
                    {
                        stk = Data.tblStks.Single(p => p.itm_cd.Equals(det.itm_cd) && p.br_id == stkData.br_id && p.LocId == stkData.LocId);

                        stk.itm_pur_qty = stk.itm_pur_qty + det.vr_qty;
                        stk.itm_pur_val = stk.itm_pur_val + det.vr_val;

                        Data.SubmitChanges();
                    }
                }

                Data.tblStkDataDets.DeleteAllOnSubmit(existingDets);
                stkData.tblStkDataDets = newDets;
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
        public List<tblStock_Loc> GetStockLoc(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var lst = from l in Data.tblStock_Locs
                          where !l.LocCategory.Equals("R") && l.LocCode != "FG"
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
        public List<tblStock_Loc> GetFinishedGoodsStockLoc(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var lst = from l in Data.tblStock_Locs
                          where !l.LocCategory.Equals("R") && l.LocCode == "FG"
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
                List<tblStkData> obj = (from r in Data.tblStkDatas
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
