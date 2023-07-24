using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Linq;

namespace RMS.BL
{
    public class PurchReqBL
    {
        public PurchReqBL()
        { }

        public object wmGetItemDetail(string itemid, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                //var heads = Data.GetItemDesc(itemid).ToList();
                decimal qoh =0;
                try
                {
                    qoh = Data.tblStks.Where(stk => stk.br_id == 1 && stk.itm_cd == itemid).Sum(stk => stk.itm_op_qty + stk.itm_pur_qty - stk.itm_isu_qty);
                }
                catch { qoh = 0; }
                var heads = (from a in Data.GetItemDesc(1, itemid)
                             select new
                             {
                                 qoh, 
                                 a.uom_dsc,
                             }).ToList();


                return heads;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }

        }

        public decimal GetQOH(string itemid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            decimal qoh = 0;
            try
            {
                qoh = Data.tblStks.Where(stk => stk.br_id == 1 && stk.itm_cd == itemid).Sum(stk => stk.itm_op_qty + stk.itm_pur_qty - stk.itm_isu_qty);
            }
            catch { qoh = 0; }

            return qoh;
        }

        public List<spPurchRegRptResult> GetPurchReqMaster(int vrid, char status, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                return Data.spPurchRegRpt(vrid, Convert.ToString(status)).ToList();
            }
            catch { }
            return null;
        }
       
        public List<spPurchRegDetRptResult> GetPurchReqDetail(int vrid, char status, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                return Data.spPurchRegDetRpt(vrid, Convert.ToString(status)).ToList();
            }
            catch { }
            return null;
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



                var records = from n in Data.tblPOReqs
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
        public Object GetPurchReqs(string docNo, string docRefNo, int deptId, string status, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            var purchList = Data.Sp_Filter_Purchase_Req(docNo, docRefNo, deptId, status).ToList();
                            //join d in Data.Depart_ments on po.DeptId equals d.DeptId
                            //where docNo.Equals("") ? po.vr_no.ToString().Equals(po.vr_no.ToString()) : po.vr_no.ToString().Contains(docNo)
                            //&& docRefNo.Equals("") ? po.DocRef.Equals(po.DocRef) : po.DocRef.Contains(docRefNo)
                            //&& status.Equals("") ? po.vr_apr.ToString().Equals(po.vr_apr.ToString()) : po.vr_apr.ToString().Contains(status)
                            //select new
                            //{
                            //    po.vr_no,
                            //    po.DocRef,
                            //    po.vr_dt,
                            //    po.vr_apr,
                            //    d.DeptNme,
                            //    po.CC_cd,
                            //    po.vr_id
                            //};

            return purchList;
        }
        //public List<Cost_Center> GetAllCostCenter(RMSDataContext Data)
        //{
        //    if (Data == null) { Data = RMSDB.GetOject(); }
        //    return Data.Cost_Centers.ToList();
        //}

        public List<Depart_ment> GetAllDepartment(RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Data.Depart_ments.ToList();
        }
        public List<tblItem_Code> GetAllItem(RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            return Data.tblItem_Codes.Where(i=> !i.itm_cd.StartsWith("1") && i.ct_id == "D").OrderBy(i=> i.itm_dsc).ToList();
        }
        public List<Item_Uom> GetAllUOM(RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Data.Item_Uoms.ToList();
        }
        public byte GetItemUOM(string item,RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Convert.ToByte(Data.tblItem_Codes.Where(p => p.itm_cd.Equals(item)).Single().uom_cd.Value);
        }
        public tblPOReq GetByID(int vrid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Data.tblPOReqs.Where(p => p.vr_id == vrid).Single();
        }
        public string GetItemUOMLabel(string item, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            byte uomid = Convert.ToByte(Data.tblItem_Codes.Where(p => p.itm_cd.Equals(item)).Single().uom_cd.Value);
            string uomval = Data.Item_Uoms.Where(p => p.uom_cd.Equals(uomid)).Single().uom_dsc;
            return uomval;

        }
        public string GetUOMDescFromID(byte uomid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Data.Item_Uoms.Where(p => p.uom_cd == uomid).Single().uom_dsc;
        }
        public string SavePurchReqFull(tblPOReq poReq, RMSDataContext Data)
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
                Data.tblPOReqs.InsertOnSubmit(poReq);
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
        public string UpdPurchReqFull(tblPOReq poReq, EntitySet<tblPOReqDet> poReqDets, RMSDataContext Data)
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
                var dets = from p in Data.tblPOReqDets
                           where p.vr_id == poReq.vr_id
                           select p;

                Data.tblPOReqDets.DeleteAllOnSubmit(dets);

                poReq.tblPOReqDets = poReqDets;

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
