using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.BL
{
    public class EmpAcrBL
    {
        public EmpAcrBL()
        {

        }

        public IQueryable GetAll(RMSDataContext Data)
        {
            try
            {
                if (Data == null)
                {
                    Data = RMSDB.GetOject();
                }

                IQueryable EmpAcr = from Acr in Data.tblPlEmpAcrs
                                    orderby Acr.EmpAcrID
                                    select new
                                    {
                                        
                                        Acr.Duration,
                                        Acr.ReportingOfficer,
                                        Acr.CounterSignOff,
                                        Acr.Remarks
                                    };
                return EmpAcr;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public IQueryable<tblPlEmpAcr> GetEmployeeAcr(int empID, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<tblPlEmpAcr> edu = from emp in Data.tblPlEmpAcrs
                                                  //where emp.EmpID == empID//this one

                                              select emp;
                return edu;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public tblPlEmpAcr GetByID(int empacr, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                tblPlEmpAcr EmpAcr = Data.tblPlEmpAcrs.Single(p => p.EmpAcrID.Equals(empacr));

                return EmpAcr;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public void Insert(tblPlEmpAcr acr, RMSDataContext Data)
        {
            //ye BL banai Emp Edu k lye 
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.tblPlEmpAcrs.InsertOnSubmit(acr);
                Data.SubmitChanges();

            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }


        public void Update(tblPlEmpAcr acr,RMSDataContext Data)
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
