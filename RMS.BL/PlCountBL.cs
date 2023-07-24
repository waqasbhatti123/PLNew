using System;
using System.Linq;

namespace RMS.BL
{
    public class PlCountBL
    {
        public PlCountBL()
        {

        }

        public Object GetAll(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var emps = from emp in Data.tblPlLeaves
                           orderby emp.EmpID
                           select new
                           {
                               EmpID = emp.EmpID,
                              // CompID = emp.CompID,
                               emp.tblPlEmpData.FullName,
                               LeaveDate = emp.StartDate,
                               endDate = emp.EndDate,
                               LeaveTypeID = emp.LeaveTypeID,
                               LeaveDays = emp.LeaveDays,
                               isac = emp.IsActive

                           };
                return emps;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public Object GetAllLeaveTypeCombo(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var levTypes = from expType in Data.tblPlLeaveTypes
                               orderby expType.LeaveTypeDesc
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
        public object getDivisionalData(string month, string year, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var Emp = (from emp in Data.tblPlEmpDatas
                           join div in Data.tblPlCodes on emp.DivID equals div.CodeID
                           join reg in Data.tblPlCodes on emp.RegID equals reg.CodeID
                           join dept in Data.tblPlCodes on emp.DeptID equals dept.CodeID
                           join des in Data.tblPlCodes on emp.DesigID equals des.CodeID  
                          //from lev in Data.tblPlEmpDatas.Where(a => a.DOJ.Value.Month == Convert.ToInt32(month) && a.DOJ.Value.Year == Convert.ToInt32(year))
                          //                 from jin in Data.tblPlEmpDatas.Where(a => a.DOL.Value.Month.ToString() == month && a.DOL.Value.Year.ToString() == year).Count()
                          where emp.EmpStatus.ToString() == "True"
                          orderby emp.DivID, emp.RegID, emp.DeptID, emp.DesigID, emp.tblCity.CityName
                          select new
                          {
                              Devision = div.CodeDesc,
                              Region = reg.CodeDesc,
                              Department = dept.CodeDesc,
                              Design = des.CodeDesc,
                              City = emp.tblCity.CityName,
                              Joiners = Data.tblPlEmpDatas.Where(a => a.DOJ.Value.Month == Convert.ToInt32(month) && a.DOJ.Value.Year == Convert.ToInt32(year)).Count(),
                              Leavers = Data.tblPlEmpDatas.Where(a => a.DOL.Value.Month == Convert.ToInt32(month) && a.DOL.Value.Year == Convert.ToInt32(year)).Count(),
                              OnBoard = Data.tblPlEmpDatas.Where(a => a.EmpStatus.ToString() == "True" && a.DivID == div.CodeID && a.RegID == reg.CodeID && a.DeptID == dept.CodeID && a.DesigID == des.CodeID && a.tblCity.CityName == emp.tblCity.CityName).Count()
                          }).Distinct().AsQueryable();
                return Emp;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public object getRegionalData(string month, string year, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var Emp = (from emp in Data.tblPlEmpDatas
                           join div in Data.tblPlCodes on emp.DivID equals div.CodeID
                           join reg in Data.tblPlCodes on emp.RegID equals reg.CodeID
                           join dept in Data.tblPlCodes on emp.DeptID equals dept.CodeID
                           join des in Data.tblPlCodes on emp.DesigID equals des.CodeID  
                          //from lev in Data.tblPlEmpDatas.Where(a => a.DOJ.Value.Month == Convert.ToInt32(month) && a.DOJ.Value.Year == Convert.ToInt32(year))
                          //                 from jin in Data.tblPlEmpDatas.Where(a => a.DOL.Value.Month.ToString() == month && a.DOL.Value.Year.ToString() == year).Count()
                          where emp.EmpStatus.ToString() == "True"
                          orderby emp.RegID, emp.DivID, emp.DeptID, emp.DesigID, emp.tblCity.CityName
                          select new
                          {
                              Devision = div.CodeDesc,
                              Region = reg.CodeDesc,
                              Department = dept.CodeDesc,
                              Design = des.CodeDesc,
                              City = emp.tblCity.CityName,
                              Joiners = Data.tblPlEmpDatas.Where(a => a.DOJ.Value.Month == Convert.ToInt32(month) && a.DOJ.Value.Year == Convert.ToInt32(year)).Count(),
                              Leavers = Data.tblPlEmpDatas.Where(a => a.DOL.Value.Month == Convert.ToInt32(month) && a.DOL.Value.Year == Convert.ToInt32(year)).Count(),
                              OnBoard = Data.tblPlEmpDatas.Where(a => a.EmpStatus.ToString() == "True" && a.DivID == div.CodeID && a.RegID == reg.CodeID && a.DeptID == dept.CodeID && a.DesigID == des.CodeID && a.tblCity.CityName == emp.tblCity.CityName).Count()
                          }).Distinct().AsQueryable();
                return Emp;
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
            //allow.CompID == lon.CompID &&
            bool isalready = false;
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                IQueryable<tblPlLeave> allows = from allow in Data.tblPlLeaves
                                                where  allow.EmpID == lon.EmpID && allow.StartDate == lon.StartDate
                                                select allow;

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

        public tblPlLeave GetByID(int Cmpid, int empid, DateTime LeaveDate, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                //p.CompID == Cmpid &&
                tblPlLeave allows = Data.tblPlLeaves.Single(p =>
                     p.EmpID == empid && p.StartDate == LeaveDate);

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