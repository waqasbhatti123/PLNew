using System;
using System.Linq;
using System.Collections.Generic;

namespace RMS.BL
{
    public class SalarySummaryBL
    {
        public static string Allowance = "Allowance";
        public static string Deduction = "Deduction";

        public SalarySummaryBL()
        {

        }

        public object GetAll(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var emps = from emp in Data.tblPlEmpDatas
                           orderby emp.FullName
                           select new
                           {
                               emp.EmpID,
                               emp.EmpCode,
                               emp.FullName,
                               Gender = emp.Sex.Equals("M") ? "Male" : "Female",
                               emp.tblCity.CityName,
                               Dept = emp.tblPlCode.CodeDesc,
                               Desig = emp.tblPlCode1.CodeDesc,
                               Div = emp.tblPlCode2.CodeDesc,
                               Reg = emp.tblPlCode3.CodeDesc,
                               emp.tblPlLocation.LocName
                               
                           };
                return emps;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public void Submit(decimal salaryPeriod, List<SalarySummarySheet> allowances, List<SalarySummarySheet> deductions, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                List<SalarySummarySheet> aInserted = new List<SalarySummarySheet>();
                List<SalarySummarySheet> aUpdated = new List<SalarySummarySheet>();

                List<SalarySummarySheet> dInserted = new List<SalarySummarySheet>();
                List<SalarySummarySheet> dUpdated = new List<SalarySummarySheet>();

                var dbAll = Data.SalarySummarySheets.Where(x => x.SalPerd == salaryPeriod && x.Type == Allowance);
                var dbDed = Data.SalarySummarySheets.Where(x => x.SalPerd == salaryPeriod && x.Type == Deduction);

                bool found = false;
                foreach(var a in allowances)
                {
                    foreach(var dba in dbAll)
                    {
                        if(a.Account == dba.Account)
                        {
                            dba.Amount = a.Amount;
                            aUpdated.Add(dba);
                            found = true;
                        }
                    }
                    if(!found)
                    {
                        var newAll = new SalarySummarySheet();
                        newAll.SalPerd = salaryPeriod;
                        newAll.Type = Allowance;
                        newAll.Account = a.Account;
                        newAll.Amount = a.Amount;
                        newAll.IsActive = true;
                        newAll.CreatedOn = a.CreatedOn;
                        newAll.CreatedBy = a.CreatedBy;

                        aInserted.Add(newAll);
                    }
                }

                found = false;
                foreach (var d in deductions)
                {
                    foreach (var dbd in dbDed)
                    {
                        if (d.Account == dbd.Account)
                        {
                            dbd.Amount = d.Amount;
                            dUpdated.Add(dbd);
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        var newDed = new SalarySummarySheet();
                        newDed.SalPerd = salaryPeriod;
                        newDed.Type = Deduction;
                        newDed.Account = d.Account;
                        newDed.Amount = d.Amount;
                        newDed.IsActive = true;
                        newDed.CreatedOn = d.CreatedOn;
                        newDed.CreatedBy = d.CreatedBy;

                        dInserted.Add(newDed);
                    }
                }

                Data.SalarySummarySheets.InsertAllOnSubmit(aInserted);
                Data.SalarySummarySheets.InsertAllOnSubmit(dInserted);

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