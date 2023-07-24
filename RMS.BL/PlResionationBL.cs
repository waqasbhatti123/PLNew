using System;
using System.Linq;

namespace RMS.BL
{
    public class PlResionationBL
    {
        public PlResionationBL()
        {

        }

        public Object GetAll(string empname, string empcode, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            object tran = new object();
            try
            {
                tran = from t in Data.tblPlRsgTrans
                       join e in Data.tblPlEmpDatas on t.EmpId equals e.EmpID
                       join ty in Data.tblPlRsgTypes on t.Rsg_Code equals ty.Rsg_Code
                       where e.FullName.Contains(empname) && e.EmpCode.Contains(empcode)
                       && t.IsResigned == true
                       orderby e.EmpCode
                       //  where e.EmpID == empid
                       select new
                       {
                           emp_Id = "EN-"+e.EmpID,
                           Code = e.EmpCode,
                           Name = e.FullName,
                           Date = t.TransDate,
                           type = ty.Rsg_Desc,
                           reason = t.Reason,
                           t.EmpId,
                           t.Status,
                           t.IsResigned
                       };
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                // throw ex;
            }
            return tran;
        }

        public tblPlRsgTran GetByID_Last(int empid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            tblPlRsgTran tran = new tblPlRsgTran();
            try
            {
                tran = Data.tblPlRsgTrans.Where(p => p.EmpId == empid && p.IsResigned == true).OrderByDescending(p => p.TransDate).SingleOrDefault();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                // throw ex;
            }
            return tran;
        }
        public tblPlRsgTran GetResgByID(int brid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                tblPlRsgTran emps = Data.tblPlRsgTrans.Single(p => p.EmpId == brid && p.IsResigned == true);

                return emps;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public Object GetAllReasonCombo(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var Reason = from Res in Data.tblPlRsgTypes
                             orderby Res.Rsg_Desc
                             select new
                             {
                                 Res.Rsg_Code,
                                 Res.Rsg_Desc

                             };
                return Reason;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public void Insert(tblPlRsgTran tran, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.tblPlRsgTrans.InsertOnSubmit(tran);
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public void InsertUpdate(tblPlRsgTran tran, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public void Delete(tblPlRsgTran tran, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.tblPlRsgTrans.DeleteOnSubmit(tran);
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public void Update(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

    }
}