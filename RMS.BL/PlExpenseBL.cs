using System;
using System.Linq;

namespace RMS.BL
{
    public class PlExpenseBL
    {
        public PlExpenseBL()
        {

        }

        public Object GetAll(int empid,RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var emps = from emp in Data.tblPlExpClaims
                           where emp.EmpID == empid 
                           from typ in Data.tblPlExpTypes.Where(p=> p.ExpTypeID==emp.ExpTypeID)
                           from det in Data.tblPlExpClaimDets.Where(a=> a.CompID== emp.CompID && a.EmpID==emp.EmpID && a.ExpTypeID==emp.ExpTypeID && a.ExpYear==emp.ExpYear)
                           orderby emp.tblPlEmpData.EmpCode
                           select new
                           {   
                               CompID=emp.CompID,
                               EmpID=emp.EmpID,
                               ExpTypeID=emp.ExpTypeID,
                               ExpYear=emp.ExpYear,
                               EmpCode=emp.tblPlEmpData.EmpCode,
                               FullName=emp.tblPlEmpData.FullName,
                               type = typ.ExpTypeDesc,
                               ExpRef=det.ExpRef,
                               ExpAprovby=det.ExpAprovedBy,
                               Amount=det.ExpClaim,
                               LimitAprove=det.ExpAproved,
                               AproveBy=det.ExpAprovedBy,
                               ExpDate = det.ExpDate
                           };
                return emps;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public Object GetAllLoanTypeCombo(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var lonTypes = from lonType in Data.tblPlLoanTypes
                               orderby lonType.LoanTypeDesc
                               select new
                               {
                                   lonType.LoanTypeID,
                                   lonType.LoanTypeDesc

                               };
                return lonTypes;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public bool ISAlreadyExist(int compid, int empid, string expType, decimal year, RMSDataContext Data)
        {

            bool isalready = false;
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {

                IQueryable<tblPlExpClaimDet> det = from dtail in Data.tblPlExpClaimDets
                                                   where dtail.CompID == compid  && dtail.EmpID == empid && 
                                                   dtail.ExpTypeID == expType && dtail.ExpYear == year
                                                  select dtail;

                if (det != null & det.Count<tblPlExpClaimDet>() > 0)
                    isalready = true;
                return isalready;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public tblPlExpClaim GetclaimByID(int Cmpid, int empid, string ExpTypeID, string expyear, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                int year = Convert.ToInt32(expyear);

                tblPlExpClaim allows = (from p in Data.tblPlExpClaims
                                       where
                    p.CompID == Cmpid && p.EmpID == empid && p.ExpTypeID == ExpTypeID 
                    && p.ExpYear == decimal.Parse(expyear)
                                           select p).SingleOrDefault();

                return allows;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public tblPlExpClaimDet GetClaimDetailByID(int Cmpid, int empid, string ExpTypeID, string expyear, string refer, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                int year = Convert.ToInt32(expyear);

                tblPlExpClaimDet allows = Data.tblPlExpClaimDets.Single(p =>
                    p.CompID == Cmpid && p.EmpID == empid && p.ExpTypeID == ExpTypeID && p.ExpYear == year && p.ExpRef == refer);

                return allows;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public int GetBasicPay(int Cmpid, int empid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {

                tblPlSalary emp = Data.tblPlSalaries.Single(p => p.CompID == Cmpid && p.EmpID == empid);

                return Convert.ToInt32(emp.Basic);
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }


        public void DeleteByID(int Cmpid, int empid, DateTime effdate, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                tblPlAlow allow = Data.tblPlAlows.Single(p =>
                    p.CompID == Cmpid && p.EmpID == empid && p.EffDate == effdate);

                Data.tblPlAlows.DeleteOnSubmit(allow);
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }

        }

        
        public void Insert(tblPlExpClaimDet det, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.tblPlExpClaimDets.InsertOnSubmit(det);
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public tblPlEmpData GetEmpByID(int Cmpid, int empid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                tblPlEmpData allows = Data.tblPlEmpDatas.Single(p =>
                    p.CompID == Cmpid && p.EmpID == empid);

                return allows;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public void Update(tblPlExpClaim exp, tblPlExpClaimDet det, RMSDataContext Data)
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