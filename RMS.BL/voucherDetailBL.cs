using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Linq;

namespace RMS.BL
{
    public class voucherDetailBL
    {
        public voucherDetailBL()
        { }


        public Glmf_Data GetGlmf_Data(int vrid, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                return Data.Glmf_Datas.Where(glmf => glmf.vrid.Equals(vrid)).SingleOrDefault();
            }
            catch { }
            return null;
        }

        public Glmf_Data_Det GetGlmf_Data_Det(int vrid, int seq, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                return Data.Glmf_Data_Dets.Where(glmf => glmf.vrid.Equals(vrid) && glmf.vr_seq.Equals(seq)).SingleOrDefault();
            }
            catch { }
            return null;
        }

        public object GetAllVouchers(int brID, DateTime today ,RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var grdObject = (from a in Data.Glmf_Datas
                                 where  a.br_id == brID && 
                                 a.vr_dt == today
                                 orderby a.vr_dt descending, a.vr_no descending
                                 select new
                                 {
                                     vrid = a.vrid,
                                     vt_cd = a.vt_cd,
                                     Gl_Year = a.Gl_Year,
                                     vr_no = a.Vr_Type.vt_use + "-" +a.vr_no,
                                     headsInvolved = GetGLMFCode(a.vrid, Data),
                                     ref_no = a.Ref_no,
                                     vr_dt = a.vr_dt,
                                     status = a.vr_apr == "P" ? "Pending" :
                                                    a.vr_apr == "A" ? "Approved" :
                                                    a.vr_apr == "D" ? "Cancelled" : "NULL",
                                     vr_nrtn = a.vr_nrtn,
                                     a.source
                                 }).ToList();



                return grdObject;
                //if (Data == null) { Data = RMSDB.GetOject(); }
                //var vouch = (from vch in Data.Glmf_Datas
                //            where vch.br_id == brID
                //            && vch.vr_dt == today
                //            orderby vch.vrid descending
                //            select new
                //            {
                //                Vrid = vch.vrid,
                //                srno = vch.Vr_Type.vt_use + "-" + vch.vr_no,
                //                refno = vch.Ref_no,
                //                ntn = vch.vr_nrtn,
                //                date = vch.vr_dt,
                //                status = vch.vr_apr == "P" ? "Pending" :
                //                                    vch.vr_apr == "A" ? "Approved" :
                //                                    vch.vr_apr == "D" ? "Cancelled" : "NULL",

                //            }).ToList();
                //return vouch;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public string GetGLMFCode(int vrid, RMSDataContext Data)
        {
            try
            {
                var codeDesc =
                               from c in Data.Glmf_Data_Dets
                               join d in Data.Glmf_Codes on c.gl_cd equals d.gl_cd
                               where c.vrid == vrid
                               select d.gl_dsc;

                string code = "";
                foreach (var item in codeDesc)
                {
                    code += item + "\r\n";
                }
                return code;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        public Glmf_Data_chq GetGlmf_Data_Chq(int vrid,  RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                return Data.Glmf_Data_chqs.Where(glmf => glmf.vrid.Equals(vrid)).SingleOrDefault();
            }
            catch { }
            return null;
        }

        public string GetCodeDesc(string glCd, int brId, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                //return Data.Glmf_Codes.Where(cd => cd.gl_cd.Equals(glCd) || cd.code.Equals(glCd)).SingleOrDefault().gl_dsc;

                var codes = from c in Data.Glmf_Codes
                             join v in Data.glmf_ven_cus_dets
                             on c.gl_cd equals v.gl_cd into sub
                            from v in sub.DefaultIfEmpty()
                            where brId == (v != null && v.br_id != null ? v.br_id : brId)
                             select c.gl_dsc;







            }
            catch { }
                return null;
        }



        public List<spGetBankA_CResult> GetBranch(int Brid, string bank, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Data.spGetBankA_C(Brid,bank).ToList();
        }

        public object GetGLTypeByGlCd(string glcd, int brId, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            var data = (from a in Data.Glmf_Codes
                        join v in Data.glmf_ven_cus_dets
                        on a.gl_cd equals v.gl_cd into sub
                        from v in sub.DefaultIfEmpty()
                        where a.code == glcd
                        && brId == (v != null && v.br_id != null ? v.br_id : brId)
                        select new
                        {
                            a.gl_cd,
                            code = a.code != null ? a.code: a.gl_cd,
                            a.gt_cd
                        }).ToList();
            return data;
        }


        public object GetSource(int vtcd, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                var obj = from a in Data.Vr_Type_Descs
                          where a.vt_cd == vtcd
                          select new
                          {
                              vtcd = a.vt_cd + ":" + a.abr,
                              a.desc
                          };
                return obj.ToList();
            }
            catch { }
            return null;
        }

        public object GetSingleAccount(int templateID, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                var obj = from a in Data.TemplateDetails
                          join b in  Data.Glmf_Codes
                          on a.Account equals b.gl_cd
                          where a.TemplateID == templateID && a.IsSingle && a.IsActive
                          select new
                          {
                              b.gl_dsc,
                              b.gl_cd,
                              b.code
                          };
                return obj.ToList();
            }
            catch { }
            return null;
        }

        public Glmf_Data_Det GetSingleAccountfromSavedVoucher(int voucherID, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                var voucher = (from a in Data.Glmf_Datas
                               where a.vrid == voucherID
                               select a).SingleOrDefault();

                if(voucher != null)
                {
                    var td = (from a in Data.TemplateDetails
                              where a.TemplateID == voucher.vt_cd && a.IsActive && a.IsSingle
                              select a).ToList();


                    var vd = (from a in td
                              join b in Data.Glmf_Data_Dets
                              on a.Account equals b.gl_cd
                              where b.vrid == voucherID
                              select b).FirstOrDefault();

                    return vd;
                }
            }
            catch(Exception ex) 
            {
                return null;
            }
            return null;
        }

        public List<spGetAllA_CResult> GetAllAcc(string code, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Data.spGetAllA_C(code).ToList();
        }

        public List<spGetAllCtrlA_CResult> GetAllCtrlAcc(string code, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Data.spGetAllCtrlA_C(code).ToList();
        }

        public List<spVouchersFill2GridResult> GetVoucher(RMSDataContext Data, int vrId, int branchId)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            List<spVouchersFill2GridResult> lst = Data.spVouchersFill2Grid(vrId, branchId).ToList();
            return lst;
        }

        //public List<Cost_Center> GetAllCostCenter(RMSDataContext Data)
        //{
        //    if (Data == null) { Data = RMSDB.GetOject(); }
        //    return Data.Cost_Centers.ToList();
        //}

        public Glmf_Code GetGlmfCodeByID(string glCd, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                return Data.Glmf_Codes.Where(gl => gl.gl_cd.Equals(glCd)).SingleOrDefault();
            }
            catch { }
            return null;
        }

        public int GetVoucherNo(RMSDataContext Data,int brId, int vouchtypid, decimal finanyear,string source)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                if (!string.IsNullOrEmpty(source))
                    return Data.Glmf_Datas.Where(t => t.vt_cd.Equals(vouchtypid) && t.br_id == brId && t.Gl_Year == finanyear && t.source == source).Max(g => g.vr_no) + 1;
                else
                    return Data.Glmf_Datas.Where(t => t.vt_cd.Equals(vouchtypid) && t.br_id == brId && t.Gl_Year == finanyear).Max(g => g.vr_no) + 1;
            }
            catch { }
            return 1;
        }
        public Glmf_Data_chq GetCheqDetByID(int vrId, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Glmf_Data_chq glmfChq = Data.Glmf_Data_chqs.Single(t => t.vrid == vrId);

                return glmfChq;
            }
            catch 
            {
                return null;
            }
           
        }

        public decimal GetFinancialYearByDate(DateTime date,  RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            decimal year;
            try
            {
                year = (from t in Data.FIN_PERDs
                                where date >= t.Start_Date && date <= t.End_Date
                                select t.Gl_Year).Single();
                
            }
            catch
            {
                year = (from t in Data.FIN_PERDs
                        where t.Cur_Year.Equals("CUR")
                        select t.Gl_Year).Single();
            }
            return year;
        }

        public decimal GetFinancialYear(RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            decimal year = (from t in Data.FIN_PERDs
                            where t.Cur_Year.Equals("CUR")
                            select t.Gl_Year).Single();
            return year;
          //  return Data.FIN_PERDs.Where(t => t.Cur_Year.Equals("CUR")).Single().Gl_Year;
        }

        public string SaveLoanAdvPaymentVoucher(RMSDataContext Data, Glmf_Data gldata, Glmf_Data_chq glChq, bool saveChq, EntitySet<Glmf_Data_Det> gldatadet)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.Glmf_Datas.InsertOnSubmit(gldata);
                gldata.Glmf_Data_Dets = gldatadet;
                if (saveChq)
                {
                    gldata.Glmf_Data_chq = glChq;
                }
                Data.SubmitChanges();

                return "ok";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public string SaveLoanAdvPaymentVoucherWithoutChq(RMSDataContext Data, Glmf_Data gldata, bool saveChq, EntitySet<Glmf_Data_Det> gldatadet)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.Glmf_Datas.InsertOnSubmit(gldata);
                gldata.Glmf_Data_Dets = gldatadet;

                Data.SubmitChanges();

                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public bool UpdateLoanAdvPaymentVoucher(RMSDataContext Data, Glmf_Data gldata, Glmf_Data_chq glChq, EntitySet<Glmf_Data_Det> gldatadet)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                
                Data.SubmitChanges();

                return true;
            }
            catch
            { }
            return false;
        }


        public void SaveVoucher(RMSDataContext Data, Glmf_Data gldata, EntitySet<Glmf_Data_Det> gldatadet)
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

                Data.Glmf_Datas.InsertOnSubmit(gldata);
                gldata.Glmf_Data_Dets = gldatadet;
                Data.SubmitChanges();

                /*IF CP OR BP MAKE ENTRY TO tblSettlement TABLE*/
                if (gldata.vr_apr == "A" && (gldata.vt_cd == 2 || gldata.vt_cd == 3 || gldata.vt_cd == 4 || gldata.vt_cd == 5))
                {
                    tblSettlement settlement = null;
                    EntitySet<tblSettlement> entSettlement = new EntitySet<tblSettlement>();
                    string cntrl_Vendor = Data.Preferences.First().ctrl_Vndr;
                    foreach (Glmf_Data_Det det in gldatadet)
                    {
                        if (det.gl_cd.StartsWith(cntrl_Vendor))
                        {
                            settlement = new tblSettlement();
                            settlement.TransGlId = det.vrid;
                            settlement.PartyId = det.gl_cd;
                            settlement.TransDate = gldata.vr_dt;
                            if (gldata.vt_cd == 4 || gldata.vt_cd == 5)//CR,BR
                                settlement.TransAmt = det.vrd_credit;
                            else//BP,CP
                                settlement.TransAmt = det.vrd_debit;
                            settlement.SettledAmt = 0;
                            settlement.Status = "OP";
                            settlement.vt_cd = gldata.vt_cd;
                            entSettlement.Add(settlement);
                        }
                    }
                    if (entSettlement.Count > 0)
                    {
                        Data.tblSettlements.InsertAllOnSubmit(entSettlement);
                        Data.SubmitChanges();
                    }
                }
                else if (gldata.vr_apr == "A" && gldata.vt_cd == 1) //If JV
                {
                    string vendorCtrl = Data.Preferences.First().ctrl_Vndr;
                    EntitySet<tblBill> entBill = new EntitySet<tblBill>();
                    tblBill bill=null;
                    int billid = 0;
                    /*FILL BILL DETAILS*/
                    foreach (Glmf_Data_Det det in gldatadet)
                    {
                        if (det.gl_cd.StartsWith(vendorCtrl))
                        {
                            try
                            {
                                billid = Data.tblBills.Max(y => y.vrid) + 1;
                            }
                            catch { billid = 1; }
                            bill = new tblBill();
                            bill.vrid = billid;
                            bill.brid = gldata.br_id;
                            bill.IV_Type = gldata.vt_cd.ToString();
                            bill.IV_NO = gldata.vr_no;
                            bill.PartyID = det.gl_cd;
                            bill.IV_Ref = gldata.vrid.ToString();
                            bill.IV_Date = gldata.vr_dt;
                            bill.IV_Due_Date = gldata.vr_dt;
                            bill.IV_Total_Amt = det.vrd_debit; 
                            bill.IV_Net_Discount = 0;
                            bill.IV_WHT = 0;
                            bill.Settled_Amt = 0;
                            bill.PrtSeq = 0;
                            bill.Rmk = null;
                            bill.IV_Status = "OP";
                            bill.WHTid = null;
                            bill.IV_Total_Amt_Diff = 0;
                            entBill.Add(bill);
                        }
                    }
                    if(entBill.Count > 0)
                    {
                        Data.tblBills.InsertAllOnSubmit(entBill);
                    }
                }
                /*COMMIT*/
                trans.Commit();
            }
            catch
            {
                /*ROLL BACK*/
                if (trans != null)
                    trans.Rollback();
            }
        }
        public void UpdateVoucher(RMSDataContext Data, Glmf_Data gldata, EntitySet<Glmf_Data_Det> gldet)
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
                IQueryable<Glmf_Data_Det> det = Data.Glmf_Data_Dets.Where(gl => gl.vrid == gldata.vrid).AsQueryable();
                Data.Glmf_Data_Dets.DeleteAllOnSubmit(det);
                Data.SubmitChanges();

                Data.Refresh(RefreshMode.KeepChanges);

                Data.Glmf_Data_Dets.InsertAllOnSubmit(gldet);
                Data.SubmitChanges();

                /*IF CP OR BP MAKE ENTRY TO tblSettlement TABLE*/
                if (gldata.vr_apr == "A" && (gldata.vt_cd == 2 || gldata.vt_cd == 3 || gldata.vt_cd == 4 || gldata.vt_cd == 5))
                {
                    tblSettlement settlement = null;
                    EntitySet<tblSettlement> entSettlement = new EntitySet<tblSettlement>();
                    string cntrl_Vendor = Data.Preferences.First().ctrl_Vndr;
                    foreach (Glmf_Data_Det detail in gldet)
                    {
                        if (detail.gl_cd.StartsWith(cntrl_Vendor))
                        {
                            settlement = new tblSettlement();
                            settlement.TransGlId = detail.vrid;
                            settlement.PartyId = detail.gl_cd;
                            settlement.TransDate = gldata.vr_dt;
                            if (gldata.vt_cd == 4 || gldata.vt_cd == 5)
                                settlement.TransAmt = detail.vrd_credit;
                            else
                                settlement.TransAmt = detail.vrd_debit;
                            settlement.SettledAmt = 0;
                            settlement.Status = "OP";
                            settlement.vt_cd = gldata.vt_cd;
                            entSettlement.Add(settlement);
                        }
                    }
                    if (entSettlement.Count > 0)
                    {
                        Data.tblSettlements.InsertAllOnSubmit(entSettlement);
                        Data.SubmitChanges();
                    }
                }
                else if (gldata.vr_apr == "A" && gldata.vt_cd == 1) //If JV
                {
                    string vendorCtrl = Data.Preferences.First().ctrl_Vndr;
                    EntitySet<tblBill> entBill = new EntitySet<tblBill>();
                    tblBill bill = null;
                    int billid = 0;
                    /*FILL BILL DETAILS*/
                    foreach (Glmf_Data_Det det1 in gldet)
                    {
                        if (det1.gl_cd.StartsWith(vendorCtrl))
                        {
                            try
                            {
                                billid = Data.tblBills.Max(y => y.vrid) + 1;
                            }
                            catch { billid = 1; }
                            bill = new tblBill();
                            bill.vrid = billid;
                            bill.brid = gldata.br_id;
                            bill.IV_Type = gldata.vt_cd.ToString();
                            bill.IV_NO = gldata.vr_no;
                            bill.PartyID = det1.gl_cd;
                            bill.IV_Ref = gldata.vrid.ToString();
                            bill.IV_Date = gldata.vr_dt;
                            bill.IV_Due_Date = gldata.vr_dt;
                            bill.IV_Total_Amt = det1.vrd_debit;
                            bill.IV_Net_Discount = 0;
                            bill.IV_WHT = 0;
                            bill.Settled_Amt = 0;
                            bill.PrtSeq = 0;
                            bill.Rmk = null;
                            bill.IV_Status = "OP";
                            bill.WHTid = null;
                            bill.IV_Total_Amt_Diff = 0;
                            entBill.Add(bill);
                        }
                    }
                    if (entBill.Count > 0)
                    {
                        Data.tblBills.InsertAllOnSubmit(entBill);
                    }
                }

                /*COMMIT*/
                trans.Commit();
            }
            catch (Exception ex)
            {
                
                /*ROLL BACK*/
                if (trans != null)
                    trans.Rollback();
                ex.Message.ToString();
            }

        }
        public void PostVoucherDet(RMSDataContext Data, Glmf_Data glmfData, EntitySet<Glmf_Data_Det> gldatadet)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            Glmf glmf = null;
            foreach (Glmf_Data_Det glmfDet in gldatadet)
            {
                glmf = Data.Glmfs.SingleOrDefault(g => g.br_id == glmfData.br_id && g.gl_cd == glmfDet.gl_cd && g.gl_year == glmfData.Gl_Year);


                if(glmf != null)
                {
                    glmf.gl_cr = glmf.gl_cr + glmfDet.vrd_credit;
                    glmf.gl_db = glmf.gl_db + glmfDet.vrd_debit;
                    glmf.updateby = glmfData.updateby;
                    glmf.updateon = Common.MyDate(Data);
                }
                else
                {
                    var row = new Glmf();
                    row.br_id = glmfData.br_id;
                    row.gl_cd = glmfDet.gl_cd;
                    row.gl_year = glmfData.Gl_Year;
                    row.gl_op = 0;
                    row.gl_db = glmfDet.vrd_debit;
                    row.gl_cr = glmfDet.vrd_credit;
                    row.gl_not = 0;
                    row.gl_obc = 0;
                    row.gl_cl = 0;
                    row.updateby = glmfData.updateby;
                    row.updateon = Common.MyDate(Data);


                    Data.Glmfs.InsertOnSubmit(row);
                }
                                               
                Data.SubmitChanges();
            }
        }
        public void PostVoucherOpeningBalDet(RMSDataContext Data, Glmf_Data glmfData, EntitySet<Glmf_Data_Det> gldatadet)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Glmf glmf = null;
                foreach (Glmf_Data_Det glmfDet in gldatadet)
                {
                    glmf = Data.Glmfs.FirstOrDefault(g => g.br_id == glmfData.br_id && g.gl_cd == glmfDet.gl_cd && g.gl_year == glmfData.Gl_Year);
                    if (glmf != null)
                    {
                        glmf.gl_op = (glmf.gl_op + glmfDet.vrd_debit) - glmfDet.vrd_credit;

                        glmf.updateby = glmfData.updateby;
                        glmf.updateon = Common.MyDate(Data);
                    }
                    else
                    {
                        var row = new Glmf();
                        row.br_id = glmfData.br_id;
                        row.gl_cd = glmfDet.gl_cd;
                        row.gl_year = glmfData.Gl_Year;
                        row.gl_op = glmfDet.vrd_debit - glmfDet.vrd_credit;
                        row.gl_db = 0;
                        row.gl_cr = 0;
                        row.gl_not = 0;
                        row.gl_obc = 0;
                        row.gl_cl = 0;
                        row.updateby = glmfData.updateby;
                        row.updateon = Common.MyDate(Data);


                        Data.Glmfs.InsertOnSubmit(row);
                    }
                    Data.SubmitChanges();

                }
            }
            catch(Exception ex)
            {
                throw new Exception("Exception on posting obening balance, Exception: " + ex.Message);
            }
        }
        public void PostVoucher4mHome(RMSDataContext Data, int vrId,string username)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            Glmf_Data glmfData = Data.Glmf_Datas.Single(g => g.vrid == vrId);

            Glmf glmf = null;

            if (glmfData.vt_cd == 55)
            {
                foreach (Glmf_Data_Det glmfDet in glmfData.Glmf_Data_Dets)
                {
                    glmf = Data.Glmfs.Single(g => g.br_id == glmfData.br_id && g.gl_cd == glmfDet.gl_cd && g.gl_year == glmfData.Gl_Year);

                    glmf.gl_op = (glmf.gl_op + glmfDet.vrd_debit )- glmfDet.vrd_credit;
                    
                    glmf.updateby = username;
                    glmf.updateon = Common.MyDate(Data);
                    
                    Data.SubmitChanges();
                }
            }
            else
            {
                foreach (Glmf_Data_Det glmfDet in glmfData.Glmf_Data_Dets)
                {
                    glmf = Data.Glmfs.Single(g => g.br_id == glmfData.br_id && g.gl_cd == glmfDet.gl_cd && g.gl_year == glmfData.Gl_Year);

                    glmf.gl_cr = glmf.gl_cr + glmfDet.vrd_credit;
                    glmf.gl_db = glmf.gl_db + glmfDet.vrd_debit;
                    glmf.updateby = username;
                    glmf.updateon = Common.MyDate(Data);
                    Data.SubmitChanges();
                }
            }
        }
        public void SaveVoucherCheqDet(RMSDataContext Data, int vrId, Glmf_Data_chq glmfChq)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
                glmfChq.vrid = vrId;
                Data.Glmf_Data_chqs.InsertOnSubmit(glmfChq);
                Data.SubmitChanges();
           
        }
        //public void ChequeLog(RMSDataContext Data,  tblChequeLog cL)
        //{
        //    if (Data == null) { Data = RMSDB.GetOject(); }
        //    //glmfChq.vrid = vrId;
        //    Data.tblChequeLogs.InsertOnSubmit(cL);
        //    Data.SubmitChanges();

        //}
        public string VoucherDesc(int vrTypeID, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            int vrType = Convert.ToInt16(vrTypeID);
            string str = (from s in Data.Vr_Types
                          where s.vt_cd == vrType
                          select s).Single().vt_dsc.ToString();
            return str;
        }
        

        public void UdateChq(RMSDataContext Data, Glmf_Data_chq glChq)
        {
            Data.SubmitChanges();
        }
        //public void UpdateVoucherCheqDet(RMSDataContext Data, int prevVrID, int newVrID, Glmf_Data_chq glmfChq)
        //{
        //    var dataToDel = from t in Data.Glmf_Data_chqs
        //                    where t.vrid == prevVrID
        //                    select t;
        //    foreach (var det in dataToDel)
        //    {
        //        Data.Glmf_Data_chqs.DeleteOnSubmit(det);
        //    }

        //    try
        //    {
        //        glmfChq.vrid = newVrID;
        //        Data.Glmf_Data_chqs.InsertOnSubmit(glmfChq);
        //        Data.SubmitChanges();
        //    }
        //    catch { }

        //}
    }
}
