using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;

namespace RMS.BL
{
    public class voucherHomeBL
    {
        public voucherHomeBL()
        { }

        public bool UpdateRemarksStatus(RMSDataContext Data, int vrID)
        {
            try
            {
                EntitySet<Glmf_Data_Rmk> enntyset = new EntitySet<Glmf_Data_Rmk>();
                List<Glmf_Data_Rmk> rmk = Data.Glmf_Data_Rmks.Where(rem => rem.vrid.Equals(vrID)).ToList();

                foreach (Glmf_Data_Rmk r in rmk)
                {
                    r.Enabled = true;

                    enntyset.Add(r);
                }
                Data.SubmitChanges();
                return true;
            }
            catch
            { }
            return false;
        }

        public bool GetRemarksStatus(RMSDataContext Data, int vrID)
        {
            try
            {
                int  count = Data.Glmf_Data_Rmks.Where(rem => rem.vrid.Equals(vrID) && rem.Enabled.Equals(false)).Count();
                if (count > 0)
                    return true;
                else
                    return false;
            }
            catch { }
            return true;
        }

        public Glmf_Data_Rmk GetRemarksByID(RMSDataContext Data, int vrID, int seq)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                return Data.Glmf_Data_Rmks.Where(rem => rem.vrid.Equals(vrID) && rem.Rmk_seq.Equals(seq)).SingleOrDefault();
            }
            catch
            { }
            return null;
        }

        public Glmf_Data GetGlmf_DataByID(int vrID, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                return Data.Glmf_Datas.Where(glmf => glmf.vrid.Equals(vrID)).SingleOrDefault();
            }
            catch
            { }
            return null;
        }

        public List<Glmf_Data_Rmk> GetRemarks(RMSDataContext Data, int vrID)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                return Data.Glmf_Data_Rmks.Where(rem => rem.vrid.Equals(vrID)).OrderByDescending(rem=> rem.Rmk_seq ).ToList();
            }
            catch
            { }
            return null;
        }

        public bool SaveRemarks(RMSDataContext Data, Glmf_Data_Rmk glmfRems)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.Glmf_Data_Rmks.InsertOnSubmit(glmfRems);
                Data.SubmitChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public int RemarksSeq(RMSDataContext Data, int vrID)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                return Data.Glmf_Data_Rmks.Where(rem => rem.vrid.Equals(vrID)).Max(rem => rem.Rmk_seq) + 1;
            }
            catch
            {
                return 1;
            }
        }

        public bool CancelVoucher(RMSDataContext Data, int vrID)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                foreach (var t in Data.Glmf_Datas)
                {
                    if (t.vrid == vrID)
                    {
                        t.vr_apr = "D";
                    }
                }
                Data.SubmitChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
        public int GetVoucherTypeID(int vrid,RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            Glmf_Data obj = (from a in Data.Glmf_Datas where a.vrid == vrid select a).SingleOrDefault();
            return obj.vt_cd;
        }
        public Glmf_Data GetVoucherMasterDetail(int vrid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            Glmf_Data obj = (from a in Data.Glmf_Datas where a.vrid == vrid select a).SingleOrDefault();
            return obj;
        }
        public Glmf_Data GetByID(int vrid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            Glmf_Data data = (from a in Data.Glmf_Datas where a.vrid == vrid select a).SingleOrDefault();
            return data;
        }
        public object GetDataForGrid(int vt_cd,DateTime dtFrm, DateTime dtTo,char status, int branchID, bool isSearch,RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

         


            if (status == '0')
            {
                var grdObject = (from a in Data.Glmf_Datas
                                 join b in Data.Vr_Types on a.vt_cd equals b.vt_cd
                                 where a.vt_cd == (vt_cd == 0 ? a.vt_cd : vt_cd)
                                 && a.vr_dt >= dtFrm && a.vr_dt <= dtTo
                                 && a.br_id == branchID 

                                 orderby a.vr_dt descending, a.vr_no descending
                                 select new
                                 {
                                     vrid = a.vrid,
                                     vt_cd = a.vt_cd,
                                     Gl_Year = a.Gl_Year,
                                     vt_use = b.vt_use,
                                     vr_no = b.vt_use + (a.source != null && b.vt_use != a.source ? "-" + a.source : "") + '-' + a.vr_no,
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
            }
            else
            {

                var grdObject = (from a in Data.Glmf_Datas
                                 join b in Data.Vr_Types on a.vt_cd equals b.vt_cd                                     
                                 where a.vt_cd == (vt_cd == 0 ? a.vt_cd : vt_cd)
                                 && a.vr_dt >= dtFrm && a.vr_dt <= dtTo && a.vr_apr == status.ToString()
                                 && a.br_id == branchID
                                 orderby a.vr_dt descending, a.vr_no descending
                                 select new
                                 {
                                     vrid = a.vrid,
                                     vt_cd = a.vt_cd,
                                     Gl_Year = a.Gl_Year,
                                     vt_use = b.vt_use,
                                     vr_no = b.vt_use + (a.source != null && b.vt_use != a.source ? "-" + a.source : "") + '-' + a.vr_no,
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


            }


            //Branch branchObj = Data.Branches.Where(x => x.br_id == branchID).FirstOrDefault();
            //if(isSearch == true)
            //{
               
            //}
            //else
            //{
            //    if(branchID == 1)
            //    {
            //        if (status == '0')
            //        {
            //            var grdObject = (from a in Data.Glmf_Datas
            //                             join b in Data.Vr_Types on a.vt_cd equals b.vt_cd
            //                             where a.vt_cd == (vt_cd == 0 ? a.vt_cd : vt_cd)
            //                             && a.vr_dt >= dtFrm && a.vr_dt <= dtTo

            //                             orderby a.vr_dt descending, a.vr_no descending
            //                             select new
            //                             {
            //                                 vrid = a.vrid,
            //                                 vt_cd = a.vt_cd,
            //                                 Gl_Year = a.Gl_Year,
            //                                 vt_use = b.vt_use,
            //                                 vr_no = b.vt_use + (a.source != null && b.vt_use != a.source ? "-" + a.source : "") + '-' + a.vr_no,
            //                                 ref_no = a.Ref_no,
            //                                 vr_dt = a.vr_dt,
            //                                 status = a.vr_apr == "P" ? "Pending" :
            //                                                a.vr_apr == "A" ? "Approved" :
            //                                                a.vr_apr == "D" ? "Cancelled" : "NULL",
            //                                 vr_nrtn = a.vr_nrtn,
            //                                 a.source

            //                             }).ToList();

            //            return grdObject;
            //        }
            //        else
            //        {

            //            var grdObject = (from a in Data.Glmf_Datas
            //                             join b in Data.Vr_Types on a.vt_cd equals b.vt_cd
            //                             where a.vt_cd == (vt_cd == 0 ? a.vt_cd : vt_cd)
            //                             && a.vr_dt >= dtFrm && a.vr_dt <= dtTo && a.vr_apr == status.ToString()
            //                             orderby a.vr_dt descending, a.vr_no descending
            //                             select new
            //                             {
            //                                 vrid = a.vrid,
            //                                 vt_cd = a.vt_cd,
            //                                 Gl_Year = a.Gl_Year,
            //                                 vt_use = b.vt_use,
            //                                 vr_no = b.vt_use + (a.source != null && b.vt_use != a.source ? "-" + a.source : "") + '-' + a.vr_no,
            //                                 ref_no = a.Ref_no,
            //                                 vr_dt = a.vr_dt,
            //                                 status = a.vr_apr == "P" ? "Pending" :
            //                                                a.vr_apr == "A" ? "Approved" :
            //                                                a.vr_apr == "D" ? "Cancelled" : "NULL",
            //                                 vr_nrtn = a.vr_nrtn,
            //                                 a.source

            //                             }).ToList();
            //            return grdObject;


            //        }
            //    }
            //    else
            //    {
            //        if (branchObj.IsDisplay == true)
            //        {
            //            if (status == '0')
            //            {
            //                var grdObject = (from a in Data.Glmf_Datas
            //                                 join b in Data.Vr_Types on a.vt_cd equals b.vt_cd
            //                                 where a.vt_cd == (vt_cd == 0 ? a.vt_cd : vt_cd)
            //                                 && a.vr_dt >= dtFrm && a.vr_dt <= dtTo
            //                                 && a.br_id == branchID

            //                                 orderby a.vr_dt descending, a.vr_no descending
            //                                 select new
            //                                 {
            //                                     vrid = a.vrid,
            //                                     vt_cd = a.vt_cd,
            //                                     Gl_Year = a.Gl_Year,
            //                                     vt_use = b.vt_use,
            //                                     vr_no = b.vt_use + (a.source != null && b.vt_use != a.source ? "-" + a.source : "") + '-' + a.vr_no,
            //                                     ref_no = a.Ref_no,
            //                                     vr_dt = a.vr_dt,
            //                                     status = a.vr_apr == "P" ? "Pending" :
            //                                                    a.vr_apr == "A" ? "Approved" :
            //                                                    a.vr_apr == "D" ? "Cancelled" : "NULL",
            //                                     vr_nrtn = a.vr_nrtn,
            //                                     a.source

            //                                 }).ToList();

            //                var grdSubBranchObject = (from a in Data.Glmf_Datas
            //                                 join b in Data.Vr_Types on a.vt_cd equals b.vt_cd
            //                                 where a.vt_cd == (vt_cd == 0 ? a.vt_cd : vt_cd)
            //                                 && a.vr_dt >= dtFrm && a.vr_dt <= dtTo
            //                                 && a.Branch.br_idd == branchID
            //                                 && a.Branch.br_status == true

            //                                          orderby a.vr_dt descending, a.vr_no descending
            //                                 select new
            //                                 {
            //                                     vrid = a.vrid,
            //                                     vt_cd = a.vt_cd,
            //                                     Gl_Year = a.Gl_Year,
            //                                     vt_use = b.vt_use,
            //                                     vr_no = b.vt_use + (a.source != null && b.vt_use != a.source ? "-" + a.source : "") + '-' + a.vr_no,
            //                                     ref_no = a.Ref_no,
            //                                     vr_dt = a.vr_dt,
            //                                     status = a.vr_apr == "P" ? "Pending" :
            //                                                    a.vr_apr == "A" ? "Approved" :
            //                                                    a.vr_apr == "D" ? "Cancelled" : "NULL",
            //                                     vr_nrtn = a.vr_nrtn,
            //                                     a.source

            //                                 }).ToList();

            //                return grdObject.Concat(grdSubBranchObject).ToList();
            //            }
            //            else
            //            {

            //                var grdObject = (from a in Data.Glmf_Datas
            //                                 join b in Data.Vr_Types on a.vt_cd equals b.vt_cd
            //                                 where a.vt_cd == (vt_cd == 0 ? a.vt_cd : vt_cd)
            //                                 && a.vr_dt >= dtFrm && a.vr_dt <= dtTo && a.vr_apr == status.ToString()
            //                                 && a.br_id == branchID

            //                                 orderby a.vr_dt descending, a.vr_no descending
            //                                 select new
            //                                 {
            //                                     vrid = a.vrid,
            //                                     vt_cd = a.vt_cd,
            //                                     Gl_Year = a.Gl_Year,
            //                                     vt_use = b.vt_use,
            //                                     vr_no = b.vt_use + (a.source != null && b.vt_use != a.source ? "-" + a.source : "") + '-' + a.vr_no,
            //                                     ref_no = a.Ref_no,
            //                                     vr_dt = a.vr_dt,
            //                                     status = a.vr_apr == "P" ? "Pending" :
            //                                                    a.vr_apr == "A" ? "Approved" :
            //                                                    a.vr_apr == "D" ? "Cancelled" : "NULL",
            //                                     vr_nrtn = a.vr_nrtn,
            //                                     a.source

            //                                 }).ToList();
            //                var grdSubBranchObject = (from a in Data.Glmf_Datas
            //                                          join b in Data.Vr_Types on a.vt_cd equals b.vt_cd
            //                                          where a.vt_cd == (vt_cd == 0 ? a.vt_cd : vt_cd)
            //                                          && a.vr_dt >= dtFrm && a.vr_dt <= dtTo
            //                                          && a.Branch.br_idd == branchID
            //                                          && a.Branch.br_status == true

            //                                          orderby a.vr_dt descending, a.vr_no descending
            //                                          select new
            //                                          {
            //                                              vrid = a.vrid,
            //                                              vt_cd = a.vt_cd,
            //                                              Gl_Year = a.Gl_Year,
            //                                              vt_use = b.vt_use,
            //                                              vr_no = b.vt_use + (a.source != null && b.vt_use != a.source ? "-" + a.source : "") + '-' + a.vr_no,
            //                                              ref_no = a.Ref_no,
            //                                              vr_dt = a.vr_dt,
            //                                              status = a.vr_apr == "P" ? "Pending" :
            //                                                             a.vr_apr == "A" ? "Approved" :
            //                                                             a.vr_apr == "D" ? "Cancelled" : "NULL",
            //                                              vr_nrtn = a.vr_nrtn,
            //                                              a.source

            //                                          }).ToList();

            //                return grdObject.Concat(grdSubBranchObject).ToList();
            //                return grdObject;


            //            }


            //        }
            //        else
            //        {
            //            if (status == '0')
            //            {
            //                var grdObject = (from a in Data.Glmf_Datas
            //                                 join b in Data.Vr_Types on a.vt_cd equals b.vt_cd
            //                                 where a.vt_cd == (vt_cd == 0 ? a.vt_cd : vt_cd)
            //                                 && a.vr_dt >= dtFrm && a.vr_dt <= dtTo
            //                                 && a.br_id == branchID

            //                                 orderby a.vr_dt descending, a.vr_no descending
            //                                 select new
            //                                 {
            //                                     vrid = a.vrid,
            //                                     vt_cd = a.vt_cd,
            //                                     Gl_Year = a.Gl_Year,
            //                                     vt_use = b.vt_use,
            //                                     vr_no = b.vt_use + (a.source != null && b.vt_use != a.source ? "-" + a.source : "") + '-' + a.vr_no,
            //                                     ref_no = a.Ref_no,
            //                                     vr_dt = a.vr_dt,
            //                                     status = a.vr_apr == "P" ? "Pending" :
            //                                                    a.vr_apr == "A" ? "Approved" :
            //                                                    a.vr_apr == "D" ? "Cancelled" : "NULL",
            //                                     vr_nrtn = a.vr_nrtn,
            //                                     a.source

            //                                 }).ToList();

            //                return grdObject;
            //            }
            //            else
            //            {

            //                var grdObject = (from a in Data.Glmf_Datas
            //                                 join b in Data.Vr_Types on a.vt_cd equals b.vt_cd
            //                                 where a.vt_cd == (vt_cd == 0 ? a.vt_cd : vt_cd)
            //                                 && a.vr_dt >= dtFrm && a.vr_dt <= dtTo && a.vr_apr == status.ToString()
            //                                 && a.br_id == branchID

            //                                 orderby a.vr_dt descending, a.vr_no descending
            //                                 select new
            //                                 {
            //                                     vrid = a.vrid,
            //                                     vt_cd = a.vt_cd,
            //                                     Gl_Year = a.Gl_Year,
            //                                     vt_use = b.vt_use,
            //                                     vr_no = b.vt_use + (a.source != null && b.vt_use != a.source ? "-" + a.source : "") + '-' + a.vr_no,
            //                                     ref_no = a.Ref_no,
            //                                     vr_dt = a.vr_dt,
            //                                     status = a.vr_apr == "P" ? "Pending" :
            //                                                    a.vr_apr == "A" ? "Approved" :
            //                                                    a.vr_apr == "D" ? "Cancelled" : "NULL",
            //                                     vr_nrtn = a.vr_nrtn,
            //                                     a.source

            //                                 }).ToList();
            //                return grdObject;


            //            }
            //        }
                    

                    
            //    }
            //}
           

          
                        
        
        }
        public object GetDataForGrid(int vt_cd, DateTime dtFrm, DateTime dtTo, char status, int branchID, bool isSearch, decimal FinYear, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }




            if (status == '0')
            {
                var grdObject = (from a in Data.Glmf_Datas
                                 join b in Data.Vr_Types on a.vt_cd equals b.vt_cd
                                 where a.vt_cd == (vt_cd == 0 ? a.vt_cd : vt_cd)
                                 && a.vr_dt >= dtFrm && a.vr_dt <= dtTo
                                 && a.br_id == branchID && a.Gl_Year == FinYear

                                 orderby a.vr_dt descending, a.vr_no descending
                                 select new
                                 {
                                     vrid = a.vrid,
                                     vt_cd = a.vt_cd,
                                     Gl_Year = a.Gl_Year,
                                     vt_use = b.vt_use,
                                     vr_no = b.vt_use + (a.source != null && b.vt_use != a.source ? "-" + a.source : "") + '-' + a.vr_no,
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
            }
            else
            {

                var grdObject = (from a in Data.Glmf_Datas
                                 join b in Data.Vr_Types on a.vt_cd equals b.vt_cd
                                 where a.vt_cd == (vt_cd == 0 ? a.vt_cd : vt_cd)
                                 && a.vr_dt >= dtFrm && a.vr_dt <= dtTo && a.vr_apr == status.ToString()
                                 && a.br_id == branchID && a.Gl_Year == FinYear
                                 orderby a.vr_dt descending, a.vr_no descending
                                 select new
                                 {
                                     vrid = a.vrid,
                                     vt_cd = a.vt_cd,
                                     Gl_Year = a.Gl_Year,
                                     vt_use = b.vt_use,
                                     vr_no = b.vt_use + (a.source != null && b.vt_use != a.source ? "-" + a.source : "") + '-' + a.vr_no,
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


            }


            //Branch branchObj = Data.Branches.Where(x => x.br_id == branchID).FirstOrDefault();
            //if(isSearch == true)
            //{

            //}
            //else
            //{
            //    if(branchID == 1)
            //    {
            //        if (status == '0')
            //        {
            //            var grdObject = (from a in Data.Glmf_Datas
            //                             join b in Data.Vr_Types on a.vt_cd equals b.vt_cd
            //                             where a.vt_cd == (vt_cd == 0 ? a.vt_cd : vt_cd)
            //                             && a.vr_dt >= dtFrm && a.vr_dt <= dtTo

            //                             orderby a.vr_dt descending, a.vr_no descending
            //                             select new
            //                             {
            //                                 vrid = a.vrid,
            //                                 vt_cd = a.vt_cd,
            //                                 Gl_Year = a.Gl_Year,
            //                                 vt_use = b.vt_use,
            //                                 vr_no = b.vt_use + (a.source != null && b.vt_use != a.source ? "-" + a.source : "") + '-' + a.vr_no,
            //                                 ref_no = a.Ref_no,
            //                                 vr_dt = a.vr_dt,
            //                                 status = a.vr_apr == "P" ? "Pending" :
            //                                                a.vr_apr == "A" ? "Approved" :
            //                                                a.vr_apr == "D" ? "Cancelled" : "NULL",
            //                                 vr_nrtn = a.vr_nrtn,
            //                                 a.source

            //                             }).ToList();

            //            return grdObject;
            //        }
            //        else
            //        {

            //            var grdObject = (from a in Data.Glmf_Datas
            //                             join b in Data.Vr_Types on a.vt_cd equals b.vt_cd
            //                             where a.vt_cd == (vt_cd == 0 ? a.vt_cd : vt_cd)
            //                             && a.vr_dt >= dtFrm && a.vr_dt <= dtTo && a.vr_apr == status.ToString()
            //                             orderby a.vr_dt descending, a.vr_no descending
            //                             select new
            //                             {
            //                                 vrid = a.vrid,
            //                                 vt_cd = a.vt_cd,
            //                                 Gl_Year = a.Gl_Year,
            //                                 vt_use = b.vt_use,
            //                                 vr_no = b.vt_use + (a.source != null && b.vt_use != a.source ? "-" + a.source : "") + '-' + a.vr_no,
            //                                 ref_no = a.Ref_no,
            //                                 vr_dt = a.vr_dt,
            //                                 status = a.vr_apr == "P" ? "Pending" :
            //                                                a.vr_apr == "A" ? "Approved" :
            //                                                a.vr_apr == "D" ? "Cancelled" : "NULL",
            //                                 vr_nrtn = a.vr_nrtn,
            //                                 a.source

            //                             }).ToList();
            //            return grdObject;


            //        }
            //    }
            //    else
            //    {
            //        if (branchObj.IsDisplay == true)
            //        {
            //            if (status == '0')
            //            {
            //                var grdObject = (from a in Data.Glmf_Datas
            //                                 join b in Data.Vr_Types on a.vt_cd equals b.vt_cd
            //                                 where a.vt_cd == (vt_cd == 0 ? a.vt_cd : vt_cd)
            //                                 && a.vr_dt >= dtFrm && a.vr_dt <= dtTo
            //                                 && a.br_id == branchID

            //                                 orderby a.vr_dt descending, a.vr_no descending
            //                                 select new
            //                                 {
            //                                     vrid = a.vrid,
            //                                     vt_cd = a.vt_cd,
            //                                     Gl_Year = a.Gl_Year,
            //                                     vt_use = b.vt_use,
            //                                     vr_no = b.vt_use + (a.source != null && b.vt_use != a.source ? "-" + a.source : "") + '-' + a.vr_no,
            //                                     ref_no = a.Ref_no,
            //                                     vr_dt = a.vr_dt,
            //                                     status = a.vr_apr == "P" ? "Pending" :
            //                                                    a.vr_apr == "A" ? "Approved" :
            //                                                    a.vr_apr == "D" ? "Cancelled" : "NULL",
            //                                     vr_nrtn = a.vr_nrtn,
            //                                     a.source

            //                                 }).ToList();

            //                var grdSubBranchObject = (from a in Data.Glmf_Datas
            //                                 join b in Data.Vr_Types on a.vt_cd equals b.vt_cd
            //                                 where a.vt_cd == (vt_cd == 0 ? a.vt_cd : vt_cd)
            //                                 && a.vr_dt >= dtFrm && a.vr_dt <= dtTo
            //                                 && a.Branch.br_idd == branchID
            //                                 && a.Branch.br_status == true

            //                                          orderby a.vr_dt descending, a.vr_no descending
            //                                 select new
            //                                 {
            //                                     vrid = a.vrid,
            //                                     vt_cd = a.vt_cd,
            //                                     Gl_Year = a.Gl_Year,
            //                                     vt_use = b.vt_use,
            //                                     vr_no = b.vt_use + (a.source != null && b.vt_use != a.source ? "-" + a.source : "") + '-' + a.vr_no,
            //                                     ref_no = a.Ref_no,
            //                                     vr_dt = a.vr_dt,
            //                                     status = a.vr_apr == "P" ? "Pending" :
            //                                                    a.vr_apr == "A" ? "Approved" :
            //                                                    a.vr_apr == "D" ? "Cancelled" : "NULL",
            //                                     vr_nrtn = a.vr_nrtn,
            //                                     a.source

            //                                 }).ToList();

            //                return grdObject.Concat(grdSubBranchObject).ToList();
            //            }
            //            else
            //            {

            //                var grdObject = (from a in Data.Glmf_Datas
            //                                 join b in Data.Vr_Types on a.vt_cd equals b.vt_cd
            //                                 where a.vt_cd == (vt_cd == 0 ? a.vt_cd : vt_cd)
            //                                 && a.vr_dt >= dtFrm && a.vr_dt <= dtTo && a.vr_apr == status.ToString()
            //                                 && a.br_id == branchID

            //                                 orderby a.vr_dt descending, a.vr_no descending
            //                                 select new
            //                                 {
            //                                     vrid = a.vrid,
            //                                     vt_cd = a.vt_cd,
            //                                     Gl_Year = a.Gl_Year,
            //                                     vt_use = b.vt_use,
            //                                     vr_no = b.vt_use + (a.source != null && b.vt_use != a.source ? "-" + a.source : "") + '-' + a.vr_no,
            //                                     ref_no = a.Ref_no,
            //                                     vr_dt = a.vr_dt,
            //                                     status = a.vr_apr == "P" ? "Pending" :
            //                                                    a.vr_apr == "A" ? "Approved" :
            //                                                    a.vr_apr == "D" ? "Cancelled" : "NULL",
            //                                     vr_nrtn = a.vr_nrtn,
            //                                     a.source

            //                                 }).ToList();
            //                var grdSubBranchObject = (from a in Data.Glmf_Datas
            //                                          join b in Data.Vr_Types on a.vt_cd equals b.vt_cd
            //                                          where a.vt_cd == (vt_cd == 0 ? a.vt_cd : vt_cd)
            //                                          && a.vr_dt >= dtFrm && a.vr_dt <= dtTo
            //                                          && a.Branch.br_idd == branchID
            //                                          && a.Branch.br_status == true

            //                                          orderby a.vr_dt descending, a.vr_no descending
            //                                          select new
            //                                          {
            //                                              vrid = a.vrid,
            //                                              vt_cd = a.vt_cd,
            //                                              Gl_Year = a.Gl_Year,
            //                                              vt_use = b.vt_use,
            //                                              vr_no = b.vt_use + (a.source != null && b.vt_use != a.source ? "-" + a.source : "") + '-' + a.vr_no,
            //                                              ref_no = a.Ref_no,
            //                                              vr_dt = a.vr_dt,
            //                                              status = a.vr_apr == "P" ? "Pending" :
            //                                                             a.vr_apr == "A" ? "Approved" :
            //                                                             a.vr_apr == "D" ? "Cancelled" : "NULL",
            //                                              vr_nrtn = a.vr_nrtn,
            //                                              a.source

            //                                          }).ToList();

            //                return grdObject.Concat(grdSubBranchObject).ToList();
            //                return grdObject;


            //            }


            //        }
            //        else
            //        {
            //            if (status == '0')
            //            {
            //                var grdObject = (from a in Data.Glmf_Datas
            //                                 join b in Data.Vr_Types on a.vt_cd equals b.vt_cd
            //                                 where a.vt_cd == (vt_cd == 0 ? a.vt_cd : vt_cd)
            //                                 && a.vr_dt >= dtFrm && a.vr_dt <= dtTo
            //                                 && a.br_id == branchID

            //                                 orderby a.vr_dt descending, a.vr_no descending
            //                                 select new
            //                                 {
            //                                     vrid = a.vrid,
            //                                     vt_cd = a.vt_cd,
            //                                     Gl_Year = a.Gl_Year,
            //                                     vt_use = b.vt_use,
            //                                     vr_no = b.vt_use + (a.source != null && b.vt_use != a.source ? "-" + a.source : "") + '-' + a.vr_no,
            //                                     ref_no = a.Ref_no,
            //                                     vr_dt = a.vr_dt,
            //                                     status = a.vr_apr == "P" ? "Pending" :
            //                                                    a.vr_apr == "A" ? "Approved" :
            //                                                    a.vr_apr == "D" ? "Cancelled" : "NULL",
            //                                     vr_nrtn = a.vr_nrtn,
            //                                     a.source

            //                                 }).ToList();

            //                return grdObject;
            //            }
            //            else
            //            {

            //                var grdObject = (from a in Data.Glmf_Datas
            //                                 join b in Data.Vr_Types on a.vt_cd equals b.vt_cd
            //                                 where a.vt_cd == (vt_cd == 0 ? a.vt_cd : vt_cd)
            //                                 && a.vr_dt >= dtFrm && a.vr_dt <= dtTo && a.vr_apr == status.ToString()
            //                                 && a.br_id == branchID

            //                                 orderby a.vr_dt descending, a.vr_no descending
            //                                 select new
            //                                 {
            //                                     vrid = a.vrid,
            //                                     vt_cd = a.vt_cd,
            //                                     Gl_Year = a.Gl_Year,
            //                                     vt_use = b.vt_use,
            //                                     vr_no = b.vt_use + (a.source != null && b.vt_use != a.source ? "-" + a.source : "") + '-' + a.vr_no,
            //                                     ref_no = a.Ref_no,
            //                                     vr_dt = a.vr_dt,
            //                                     status = a.vr_apr == "P" ? "Pending" :
            //                                                    a.vr_apr == "A" ? "Approved" :
            //                                                    a.vr_apr == "D" ? "Cancelled" : "NULL",
            //                                     vr_nrtn = a.vr_nrtn,
            //                                     a.source

            //                                 }).ToList();
            //                return grdObject;


            //            }
            //        }



            //    }
            //}





        }
        public object GetDataForHeadOff(int vt_cd, DateTime dtFrm, DateTime dtTo, char status, int branchID, bool isSearch, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }




            if (status == '0')
            {
                var grdObject = (from a in Data.Glmf_Datas
                                 join b in Data.Vr_Types on a.vt_cd equals b.vt_cd
                                 where a.vt_cd == (vt_cd == 0 ? a.vt_cd : vt_cd)
                                 && a.vr_dt >= dtFrm && a.vr_dt <= dtTo
                                 && a.br_id == branchID

                                 orderby a.vr_dt descending, a.vr_no descending
                                 select new
                                 {
                                     vrid = a.vrid,
                                     vt_cd = a.vt_cd,
                                     Gl_Year = a.Gl_Year,
                                     vt_use = b.vt_use,
                                     vr_no = b.vt_use + (a.source != null && b.vt_use != a.source ? "-" + a.source : "") + '-' + a.Ref_no,
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
            }
            else
            {

                var grdObject = (from a in Data.Glmf_Datas
                                 join b in Data.Vr_Types on a.vt_cd equals b.vt_cd
                                 where a.vt_cd == (vt_cd == 0 ? a.vt_cd : vt_cd)
                                 && a.vr_dt >= dtFrm && a.vr_dt <= dtTo && a.vr_apr == status.ToString()
                                 && a.br_id == branchID
                                 orderby a.vr_dt descending, a.vr_no descending
                                 select new
                                 {
                                     vrid = a.vrid,
                                     vt_cd = a.vt_cd,
                                     Gl_Year = a.Gl_Year,
                                     vt_use = b.vt_use,
                                     vr_no = b.vt_use + (a.source != null && b.vt_use != a.source ? "-" + a.source : "") + '-' + a.vr_no,
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
          catch(Exception ex)
            {
                return "";
            }
        }

        public object GetDataForBPAGrid(int vt_cd, DateTime dtFrm, DateTime dtTo, char status, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            if (status == '0')
            {
                var grdObject = (from a in Data.Glmf_Datas
                                 join b in Data.Vr_Types on a.vt_cd equals b.vt_cd
                                 where a.vt_cd == (vt_cd == 0 ? a.vt_cd : vt_cd)
                                 && a.vr_dt >= dtFrm && a.vr_dt <= dtTo
                                 && a.source.ToLower().Contains("bpa")
                                 orderby a.vr_dt descending, a.vr_no descending
                                 select new
                                 {
                                     vrid = a.vrid,
                                     vt_cd = a.vt_cd,
                                     Gl_Year = a.Gl_Year,
                                     vt_use = b.vt_use,
                                     vr_no = b.vt_use + (a.source != null && b.vt_use != a.source ? "-" + a.source : "") + '-' + a.vr_no,
                                     ref_no = a.Ref_no,
                                     vr_dt = a.vr_dt,
                                     status = a.vr_apr == "P" ? "Pending" :
                                                    a.vr_apr == "A" ? "Approved" :
                                                    a.vr_apr == "D" ? "Cancelled" : "NULL",
                                     vr_nrtn = a.vr_nrtn,
                                     a.source

                                 }).ToList();

                return grdObject;
            }
            else
            {

                var grdObject = (from a in Data.Glmf_Datas
                                 join b in Data.Vr_Types on a.vt_cd equals b.vt_cd
                                 where a.vt_cd == (vt_cd == 0 ? a.vt_cd : vt_cd)
                                 && a.vr_dt >= dtFrm && a.vr_dt <= dtTo && a.vr_apr == status.ToString()
                                 && a.source.ToLower().Contains("bpa")
                                 orderby a.vr_dt descending, a.vr_no descending
                                 select new
                                 {
                                     vrid = a.vrid,
                                     vt_cd = a.vt_cd,
                                     Gl_Year = a.Gl_Year,
                                     vt_use = b.vt_use,
                                     vr_no = b.vt_use + (a.source != null && b.vt_use != a.source ? "-" + a.source : "") + '-' + a.vr_no,
                                     ref_no = a.Ref_no,
                                     vr_dt = a.vr_dt,
                                     status = a.vr_apr == "P" ? "Pending" :
                                                    a.vr_apr == "A" ? "Approved" :
                                                    a.vr_apr == "D" ? "Cancelled" : "NULL",
                                     vr_nrtn = a.vr_nrtn,
                                     a.source

                                 }).ToList();
                return grdObject;


            }




        }

        public Branch GetBranch(int branchid, int compid, RMSDataContext Data)
        {
            return Data.Branches.Where(br => br.br_id.Equals(branchid) && br.CompID.Value.Equals(compid)).FirstOrDefault();
        }

        public tblAppUser GetUsersss(int branchid, RMSDataContext Data)
        {
            return Data.tblAppUsers.Where(br => br.UserID == branchid).FirstOrDefault();
        }

        public List<spTestGLResult> GetReport(RMSDataContext Data, int id)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Data.spTestGL(id).ToList();
        
        }
        public string GetNarration(RMSDataContext Data, int vrID)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            string narration = "";
            foreach (var t in Data.Glmf_Datas)
            {
                if (t.vrid == vrID)
                {
                     narration = t.vr_nrtn.ToString();
                }
            }
            Data.SubmitChanges();

            return narration;
        }
        
        public List<spBPnBRResult> GetReport2(RMSDataContext Data, int id)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Data.spBPnBR(id).ToList();

        }

        public string Unpost(RMSDataContext Data, int vrid, string username, int userid, byte[] bytes)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                decimal year = 0;
                List< Glmf_Data_Det> glmDet = (from a in Data.Glmf_Data_Dets
                                        where a.vrid.Equals(vrid)
                                        select a).ToList();

                Glmf_Data glObject = (from t in Data.Glmf_Datas
                                      where t.vrid.Equals(vrid)
                                      select t).Single();



                year = (from t in Data.FIN_PERDs
                        where t.Cur_Year.Equals("CUR")
                        select t.Gl_Year).Single();

                if (glObject.Gl_Year < year)
                {
                    return "GL year must be greater than equal to " + year;
                }




                glObject.vr_apr = "P";
                if (username != null && username.Length > 15)
                {
                    username = username.Substring(0, 14);
                }
                glObject.approvedby = null;
                glObject.approvedon = null;
                
                /*Save to Glmf_Data_Img*/

                Glmf_Data_Img img = new Glmf_Data_Img();

                img.vrid_ref = vrid;
                img.datestamp = Common.MyDate(Data);
                img.userid = userid;
                img.file = bytes;

                Data.Glmf_Data_Imgs.InsertOnSubmit(img);


                /*REMOVING EFFECT FROM GLMF*/
                     
                if (glObject.vt_cd == 55)
                {
                    Glmf glmf = null;
                    foreach (Glmf_Data_Det glmfDet in glmDet)
                    {
                        glmf = Data.Glmfs.Single(g => g.br_id == glObject.br_id && g.gl_cd == glmfDet.gl_cd);

                        glmf.gl_op = (glmf.gl_op - glmfDet.vrd_debit) + glmfDet.vrd_credit;

                        glmf.updateby = glObject.updateby;
                        glmf.updateon = Common.MyDate(Data);
                        Data.SubmitChanges();
                    }
                }
                else
                {
                    Glmf glmf = null;
                    foreach (Glmf_Data_Det glmfDet in glmDet)
                    {
                        glmf = Data.Glmfs.Single(g => g.br_id == glObject.br_id && g.gl_cd == glmfDet.gl_cd);

                        glmf.gl_cr = glmf.gl_cr - glmfDet.vrd_credit;
                        glmf.gl_db = glmf.gl_db - glmfDet.vrd_debit;
                        glmf.updateby = glObject.updateby;
                        glmf.updateon = Common.MyDate(Data);
                        Data.SubmitChanges();
                    }
                }
                /*END REMOVING EFFECT FROM GLMF*/

                /*REMOVING EFFECT FROM tblBill, tblSettlement and tblSettlmentDet*/
                if (glObject.vt_cd == 1)//JV
                {
                    tblBill bill = Data.tblBills.Where(b => b.IV_Type.Equals(1) && b.IV_Ref.Equals(vrid)).SingleOrDefault();
                    if (bill != null)
                    {
                        if (bill.IV_Status == "CL")
                        {
                            List<tblSettlementDet> setldet = Data.tblSettlementDets.Where(det => det.BillRef.Equals(bill.vrid)).ToList();
                            List<tblSettlement> setllist = new List<tblSettlement>();
                            tblSettlement setl = null;
                            foreach (tblSettlementDet det in setldet)
                            {
                                setl = Data.tblSettlements.Where(stl => stl.TransNo.Equals(det.TransNo)).SingleOrDefault();
                                if (setl != null)
                                {
                                    setl.Status = "OP";
                                    setl.SettledAmt = setl.SettledAmt - det.SettleAmt;
                                    setllist.Add(setl);

                                    det.SettleAmt = 0;
                                }
                            }
                        }
                        bill.IV_Status = "CA";
                    }
                }
                else if (glObject.vt_cd == 2 || glObject.vt_cd == 3)//CP, BP
                {
                    List<tblSettlement> setlCollection = new List<tblSettlement>();
                    List<tblBill> billCollection = new List<tblBill>();
                    List<tblSettlement> setllist = Data.tblSettlements.Where(s => s.TransGlId.Value.Equals(vrid) && !s.Status.Equals("CA")).ToList();
                    foreach (var setl in setllist)
                    {
                        if (setl.Status.Equals("OP") && setl.SettledAmt.Value.Equals(0))
                        {
                            setl.Status = "CA";
                            setlCollection.Add(setl);
                        }
                        else if (setl.SettledAmt.Value > 0)
                        {
                            tblSettlementDet setldet = Data.tblSettlementDets.Where(det => det.TransNo == setl.TransNo).Single();
                            tblBill setldBill = Data.tblBills.Where(bill => bill.vrid == setldet.BillRef).Single();
                            setldBill.Settled_Amt = setldBill.Settled_Amt - setldet.SettleAmt;
                            if (setldBill.IV_Status.Equals("CL"))
                            {
                                setldBill.IV_Status = "OP";
                            }

                            setl.Status = "CA";
                            setlCollection.Add(setl);
                            billCollection.Add(setldBill);
                        }
                    }
                }
                /*END REMOVING EFFECT FROM tblBill, tblSettlement and tblSettlmentDet*/



                Data.SubmitChanges();

                return "ok";
            }catch(Exception ex)
            {
                return ex.Message;
            }
        }


        public void Save(RMSDataContext Data, int vrid, char status,string username)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            Glmf_Data glObject = (from t in Data.Glmf_Datas
                                 where t.vrid.Equals(vrid)
                                 select t).Single();
            glObject.vr_apr = status.ToString();
            if (username != null && username.Length > 15)
            {
                username = username.Substring(0, 14);
            }
            glObject.approvedby = username;
            glObject.approvedon = Common.MyDate(Data);
            Data.SubmitChanges();

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

        public string GetBankByCode(string code, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                return Data.Glmf_Codes.Where(cd => cd.gl_cd.Equals(code)).SingleOrDefault().gl_dsc;
            }
            catch
            {
                return "";
            }

        }

        public List<spSearchVoucherResult> GetSearchedVouchers(int brid, char status, string narr, string title,
            string chqno, decimal debit, decimal credit, string ccntr, string vouchno, int vouchtyp, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                return Data.spSearchVoucher(brid, status.ToString(), narr, title, chqno, debit, credit, ccntr, vouchno, vouchtyp).ToList();
            }
            catch
            {
                return null;
            }
        }
    }
}
