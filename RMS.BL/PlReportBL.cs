using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

namespace RMS.BL
{
    public class PlReportBL
    {
        public PlReportBL()
        {

        }

            //String period = "";
            //int month = int.Parse(ddlPayPerd.SelectedItem.Text.Substring(4, 2));
            //period = 
            //System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month) +
            //ddlPayPerd.SelectedItem.Text.Substring(0,4);


   
        //public Object GetAll(RMSDataContext Data)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = RMSDB.GetOject(); }
        //        var emps = from emp in Data.tblPlEmpDatas
        //                   orderby emp.EmpID,emp.DesigID,emp.DeptID,emp.tblCity.CityName,emp.tblPlLocation.LocName
                          
        //                   select new
        //                   {
                              
        //                      emp.EmpID,
        //                      name = emp.EmpCode + "-" + emp.FullName,
        //                      desig = emp.tblPlCode1.CodeDesc,
        //                      dpt = emp.tblPlCode.CodeDesc,
        //                      divis = emp.tblPlCode2.CodeDesc,
        //                      cty = emp.tblCity.CityName,
        //                      loc = emp.tblPlLocation.LocName,
        //                      emp.DOJ,
        //                      emp.DOL,
        //                      regi = emp.tblPlCode3.CodeDesc
                               
        //                   };
        //        return emps;
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }
        //}


        public List<spDeptWiseSalaryResult> GetDeptWiseSalary(int compid, int payperiod, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                return Data.spDeptWiseSalary(compid, payperiod).ToList();
            }
            catch
            { }
            return null;
        }


        public  Object GetAllDate(DateTime AsOfDate, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var emps = from emp in Data.tblPlEmpDatas
                            where emp.DOL != null ? emp.DOL >= AsOfDate : AsOfDate == AsOfDate
                           orderby emp.EmpID, emp.DesigID, emp.DeptID, emp.tblCity.CityName, emp.tblPlLocation.LocName

                           select new
                           {

                               emp.EmpID,
                               EmployeeId = "EN-"+emp.EmpID,
                               emp.EmpCode,
                               name = emp.FullName,
                               desig = emp.tblPlCode1.CodeDesc,
                               dpt = emp.tblPlCode.CodeDesc,
                               divis = emp.tblPlCode2.CodeDesc,
                               cty = emp.tblCity.CityName,
                               loc = emp.tblPlLocation.LocName,
                               emp.DOJ,
                               emp.DOL,
                               regi = emp.tblPlCode3.CodeDesc
                              

                           };
                return emps;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
       
        public object GetLoanReport( RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var emps = (from emp in Data.tblPlEmpDatas
                           from lon in Data.tblPlLoanTypes
                           from lontype in Data.tblPlLoans
                           orderby emp.EmpID, emp.DesigID, emp.DeptID

                           select new 
                           {
                               emp.EmpID,
                               name = emp.EmpCode + "-" + emp.FullName,
                               desig = emp.tblPlCode1.CodeDesc,
                               dept = emp.tblPlCode.CodeDesc,
                               loantype = lon.LoanTypeDesc,
                               paymentref = lontype.PaymentRef,
                               dat = lontype.PaymentDate,
                               lonamt = lontype.LoanAmt,
                               nos = lontype.NoOfInst,
                               instamt = lontype.InstAmt
                
                           }).Distinct();
                
                return emps;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        
        
        }
        public tblCompany GetByID(int compid,RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                tblCompany otblComp = Data.tblCompanies.Single(p => p.CompID == compid);
                return otblComp;
            
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public tblPlCode GetByPlCode(int codeid, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                tblPlCode otblComp = Data.tblPlCodes.FirstOrDefault(p => p.CodeTypeID == codeid);
                return otblComp;

            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public tblPlCode GetByPlDiv(int divid, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                tblPlCode otblComp = Data.tblPlCodes.FirstOrDefault(p => p.CodeTypeID == divid);
                return otblComp;

            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public IQueryable<spRptEmployeeListResult> RptEmployeeRec(int CompId, int payPeriod, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                IQueryable<spRptEmployeeListResult> emps =
                    Data.spRptEmployeeList(CompId, payPeriod).AsQueryable();

                return emps;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }


        public List<spMontylyReconResult> GetMonthlyRecon(int compid, int payperiod, int prevpayperiod, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                return Data.spMontylyRecon(compid, payperiod, prevpayperiod).ToList();
            }
            catch { }

            return null;
        }


        public object rptSalaryRecon(int CompId, int payPeriod, int empid,
            string empcode, string empcodeto, RMSDataContext Data)
        {
            List<spSalaryReconTORResult> emp;

            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                emp = Data.spSalaryReconTOR(CompId, payPeriod).ToList();
                if (empid.Equals(0))
                {
                    if (empcode.Trim().Length > 0)
                    {
                        if (empcodeto.Trim().Length == 0)
                        {
                            emp = emp.Where(t => t.empcode == empcode).ToList();
                        }
                        else
                        {

                            //emp = emp.Where(t => int.Parse(t.empcode) >= int.Parse(empcode) && int.Parse(t.empcode) <= int.Parse(empcodeto));
                        }
                    }
                }
                else
                {
                    emp = emp.Where(e => e.empid.Equals(empid)).ToList();
                }
                return emp;
            }

            catch //(Exception ex)
            {
                // RMSDB.SetNull();
                //  throw ex;
            }
            return null;


        }

        public IQueryable<Loan_ReportResult> Lreport(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                IQueryable<Loan_ReportResult> emps =
                    Data.Loan_Report().AsQueryable();

                return emps;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
    }
}