using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.Common;
//using System.Transactions;


namespace RMS.BL
{
    public class AttendanceBL
    {
        public AttendanceBL()
        {

        }


        public IQueryable<sp_AttendacePucarResult> Atten(string month, int jobtypee, int branch, RMSDataContext Data)
        {
            return Data.sp_AttendacePucar(month, jobtypee, branch).AsQueryable();
        }

        //public void Insert(tblPlEmpAttendance att, RMSDataContext Data)
        //{
        //    //ye BL banai Emp Edu k lye 
        //    try
        //    {
        //        if (Data == null) { Data = RMSDB.GetOject(); }
        //        Data.tblPlEmpAttendances.InsertOnSubmit(att);
        //        Data.SubmitChanges();

        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }
        //}

        //public void Update(tblPlEmpAttendance id, RMSDataContext Data)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = RMSDB.GetOject(); }
        //        Data.SubmitChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }
        //}


        public string GetLeaveAbbr( int empid, DateTime leavedate, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                var obj = (from a in Data.tblPlLeaves
                           join b in Data.tblPlLeaveTypes
                           on a.LeaveTypeID equals b.LeaveTypeID
                           where 
                           leavedate >= a.StartDate && leavedate <= a.EndDate
                           select new
                           {
                               LeaveTypeAbbr = b.LeaveTypeAbbr == null ? "L" : b.LeaveTypeAbbr
                           }).SingleOrDefault();
                return obj.LeaveTypeAbbr;
            }
            catch { }
            return "L";
        }

        public List<int> GetEmpLeaves(int compid, int empid, DateTime rptDate, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                var obj = (from lv in Data.tblPlLeaves
                           join lvTyp in Data.tblPlLeaveTypes
                           on lv.LeaveTypeID equals lvTyp.LeaveTypeID
                           where  lv.EmpID.Equals(empid)
                           orderby lv.StartDate
                           select new
                           {
                               StartDate = lv.StartDate,
                               lv.LeaveDays,
                               lvTyp.LeaveTypeID
                           }).ToList();

                List<int> listLeaveDays = new List<int>();
                foreach(var o in obj)
                {
                    
                    for(int i =0; i < o.LeaveDays; i++)
                    {
                        
                        listLeaveDays.Add(Convert.ToDateTime(o.StartDate).Day + i);
                    }
                }

                return listLeaveDays;

            }
            catch { }
            return null;
        }

        public List<Anonymous4Attendance> GetEmployees(int compid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                List<Anonymous4Attendance> emps = (from emp in Data.tblPlEmpDatas
                                                   join codeReg in Data.tblPlCodes
                                                   on emp.RegID equals codeReg.CodeID
                                                   join codeDep in Data.tblPlCodes
                                                   on emp.DeptID equals codeDep.CodeID
                                                   join codeDes in Data.tblPlCodes
                                                   on emp.DesigID equals codeDes.CodeID
                                                   join codeDiv in Data.tblPlCodes
                                                   on emp.DivID equals codeDiv.CodeID into leftjoin
                                                   from codeDiv in leftjoin.DefaultIfEmpty()

                                                   where   Convert.ToInt32(codeDes.CodeTypeID) == 4
                                                        && Convert.ToInt32(codeDep.CodeTypeID) == 3
                                                        && codeDiv != null ? Convert.ToInt32(codeDiv.CodeTypeID) == 2 : codeDiv == null
                                                        && Convert.ToInt32(codeReg.CodeTypeID) == 1
                                                        && emp.CompID == compid 
                                                        && emp.EmpStatus == 1
                                                   orderby codeReg.CodeDesc, codeDep.CodeDesc
                                                   select new Anonymous4Attendance
                                                   {
                                                       Region = codeReg.CodeDesc,
                                                       Department = codeDep.CodeDesc,
                                                       Designation = codeDes.CodeDesc,
                                                       Division = codeDiv.CodeDesc,
                                                       EmpID = emp.EmpID.ToString(),
                                                       EmpCode = emp.EmpCode,
                                                       Name = emp.FullName,
                                                       DOJ = Convert.ToDateTime(emp.DOJ).Date,
                                                       DOL = emp.DOL != null ? Convert.ToDateTime(emp.DOL) : Convert.ToDateTime("01-01-1900")
                                                      

                                                   }).ToList();
                return emps;
                                                    
            }
            catch { }
            return null;
        }
    }
}