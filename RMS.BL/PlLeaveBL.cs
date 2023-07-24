using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace RMS.BL
{
    public class PlLeaveBL
    {
        public PlLeaveBL()
        {
        }

        public bool DeleteLeave(tblPlLeave tblLve, RMSDataContext Data)
        {
            try
            {
                //lv.CompID.Equals(tblLve.CompID) &&
                if (Data == null) { Data = RMSDB.GetOject(); }

                tblPlLeave tblLeave = Data.tblPlLeaves.Where(lv =>  lv.EmpID.Equals(tblLve.EmpID) && lv.StartDate.Equals(tblLve.StartDate)).SingleOrDefault();
                Data.tblPlLeaves.DeleteOnSubmit(tblLeave);
                Data.SubmitChanges();
                return true;
            }
            catch //(Exception ex)
            {
                //RMSDB.SetNull();
                //throw ex;
            }
            return false;
        }

        //public IQueryable<spLeaveBalRptResult> GetLeaveBalanceData(int compId, DateTime toDate, RMSDataContext Data)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = RMSDB.GetOject(); }

        //        return Data.spLeaveBalRpt(compId, toDate).AsQueryable();
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }
        //}


        //public IQueryable<spLeaveRptResult> GetLeaveData(int compId, DateTime frmDate, DateTime toDt, RMSDataContext Data)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = RMSDB.GetOject(); }

        //        return Data.spLeaveRpt(compId, frmDate.Date, toDt.Date).AsQueryable();
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }
        //}

        public object GetAll( RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var emps = from emp in Data.tblPlLeaves
                                   from typ in Data.tblPlLeaveTypes
                                   orderby emp.EmpID 
                                   select new
                                   {
                                       emp.LeaveID,
                                       EmpID = emp.EmpID,
                                      // CompID = emp.CompID,
                                      fName = emp.tblPlEmpData.FullName,
                                       LeaveDate = emp.StartDate,
                                       endDate = emp.EndDate,
                                       LeaveTypeID = typ.LeaveTypeDesc,
                                       LeaveDays = emp.LeaveDays,
                                       Remarks = emp.Remarks,
                                       Status = emp.Status == null ? "Pending" : emp.Status.ToString(),
                                       isac =  emp.IsActive

                                   };
                return emps;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        //public List<spLeaveBalRptResult> GetEmpLeaveStatus(int compid, int empid, DateTime tilldate, RMSDataContext Data)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = RMSDB.GetOject(); }

        //        return Data.spLeaveBalRpt(compid, tilldate).Where(emp => emp.EmpID.Equals(empid)).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }
        //}

        public Object GetAllLeaveTypeCombo(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var levTypes = from expType in Data.tblPlLeaveTypes
                               orderby expType.LeaveTypeID ascending
                               select new
                               {
                                   expType.LeaveTypeDesc,
                                   expType.LeaveTypeID

                               };
                return levTypes;
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
        public bool ISAlreadyExist(tblPlLeave lon, RMSDataContext Data)
        {

            bool isalready = false;
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                //|| (lon.StartDate >= allow.StartDate && lon.StartDate <= allow.StartDate
                //allow.CompID == lon.CompID
                IQueryable<tblPlLeave> allows = from allow in Data.tblPlLeaves
                                                where 
                                                       allow.EmpID == lon.EmpID 
                                                      && ((lon.StartDate >= allow.EndDate && lon.StartDate <= allow.EndDate
                                                            || lon.EndDate >= allow.StartDate && lon.EndDate <= allow.StartDate)) // && (lon.LeaveDate < allow.LeaveDate ? lon.LeaveDate.AddDays(Convert.ToDouble(lon.LeaveDays)) < allow.LeaveDate : lon.LeaveDate > allow.LeaveDate.AddDays(Convert.ToDouble(allow.LeaveDays)))
                                                //where allow.CompID == lon.CompID 
                                                //   && allow.EmpID == lon.EmpID 
                                                //   && (lon.LeaveDate >= allow.LeaveDate && lon.LeaveDate <= allow.LeaveDate.AddDays(Convert.ToDouble(allow.LeaveDays - 1)))
                                                select allow;
                //&& ((lon.LeaveDate>=allow.LeaveDate 
                //&& lon.LeaveDate<=allow.LeaveDate.AddDays(Convert.ToDouble(allow.LeaveDays)) )
                //||(lon.LeaveDate.AddDays(Convert.ToDouble(lon.LeaveDays))>=allow.LeaveDate&&lon.LeaveDate<=allow.LeaveDate)) // && (lon.LeaveDate < allow.LeaveDate ? lon.LeaveDate.AddDays(Convert.ToDouble(lon.LeaveDays)) < allow.LeaveDate : lon.LeaveDate > allow.LeaveDate.AddDays(Convert.ToDouble(allow.LeaveDays)))





                if (allows != null & allows.Count<tblPlLeave>() > 0) { 
                    isalready = true;
                return isalready;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public bool ISAlreadyExist_ForUpdate(tblPlLeave lon, tblPlLeave prevlev, RMSDataContext Data)
        {

            bool isalready = false;
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                IQueryable<tblPlLeave> allows = from allow in Data.tblPlLeaves
                                                where ( allow.EmpID == lon.EmpID
                                                          && ((lon.StartDate >= allow.EndDate && lon.StartDate <= allow.EndDate)
                                                                || (lon.EndDate >= allow.StartDate && lon.EndDate <= allow.StartDate)))
                                                       && allow.StartDate != prevlev.StartDate
                
                                                // && (lon.LeaveDate < allow.LeaveDate ? lon.LeaveDate.AddDays(Convert.ToDouble(lon.LeaveDays)) < allow.LeaveDate : lon.LeaveDate > allow.LeaveDate.AddDays(Convert.ToDouble(allow.LeaveDays)))
                                                //where allow.CompID == lon.CompID 
                                                //   && allow.EmpID == lon.EmpID 
                                                //   && (lon.LeaveDate >= allow.LeaveDate && lon.LeaveDate <= allow.LeaveDate.AddDays(Convert.ToDouble(allow.LeaveDays - 1)))
                                                select allow;
                //&& ((lon.LeaveDate>=allow.LeaveDate 
                //&& lon.LeaveDate<=allow.LeaveDate.AddDays(Convert.ToDouble(allow.LeaveDays)) )
                //||(lon.LeaveDate.AddDays(Convert.ToDouble(lon.LeaveDays))>=allow.LeaveDate&&lon.LeaveDate<=allow.LeaveDate)) // && (lon.LeaveDate < allow.LeaveDate ? lon.LeaveDate.AddDays(Convert.ToDouble(lon.LeaveDays)) < allow.LeaveDate : lon.LeaveDate > allow.LeaveDate.AddDays(Convert.ToDouble(allow.LeaveDays)))
                //(allow.EmpID == lon.EmpID
                //                                          && ((lon.LeaveDate >= allow.LeaveDate && lon.LeaveDate <= allow.LeaveDate.AddDays(Convert.ToDouble(allow.LeaveDays - 1)))
                //                                                || (lon.LeaveDate.AddDays(Convert.ToDouble(lon.LeaveDays - 1)) >= allow.LeaveDate && lon.LeaveDate <= allow.LeaveDate)))
                //                                       && allow.LeaveDate != prevlev.LeaveDate




                if (allows != null & allows.Count<tblPlLeave>() > 0)
                    isalready = true;
                return isalready;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }


        public tblPlLeave GetByID( int empid, DateTime LeaveDate, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                //p.CompID == Cmpid &&
                //tblPlLeave allows = Data.tblPlLeaves.Single(p =>
                //    p.EmpID == empid && p.StartDate == LeaveDate);

                tblPlLeave allows = Data.tblPlLeaves.Where(x => x.EmpID == empid && x.StartDate == LeaveDate).FirstOrDefault();

                return allows;
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

        public void Insert(tblPlLeave lon, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.tblPlLeaves.InsertOnSubmit(lon);
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public void Update(tblPlLeave lev, RMSDataContext Data)
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