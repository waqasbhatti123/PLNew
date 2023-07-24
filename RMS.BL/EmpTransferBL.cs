using System;
using System.Linq;

namespace RMS.BL
{
    public class EmpTransferBL
    {
        public EmpTransferBL()
        {

        }

        public Object GetAll(int empid, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var emps = from emp in Data.tblPlEmpTransfers
                           where emp.EmpID == empid
                           
                           orderby emp.EfDate descending
                           select new
                           {
                               emp.EmpID,
                               emp.tblPlEmpData.FullName,
                               Dept = emp.tblPlCode.CodeDesc,
                               Desig = emp.tblPlCode1.CodeDesc,
                               Div = emp.tblPlCode2.CodeDesc,
                               Reg = emp.tblPlCode3.CodeDesc,
                               emp.tblCity.CityName,
                               emp.tblPlLocation.LocName,
                               emp.Grade, emp.EfDate
                               
                           };
                return emps;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public void Insert(tblPlEmpTransfer emp, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.tblPlEmpTransfers.InsertOnSubmit(emp);
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