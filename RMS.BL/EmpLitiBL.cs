using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.BL
{
    public class EmpLitiBL
    {
        public EmpLitiBL()
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

                IQueryable EmpLit = from lit in Data.tblPlEmpLitigations
                                    orderby lit.LitiID
                                    select new
                                    {
                                       // lit.LitiType,
                                        lit.LitiDate,
                                        lit.Authority,
                                        lit.Remarks
                                    };
                return EmpLit;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public IQueryable<tblPlEmpLitigation> GetEmployeeLiti(int empID, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<tblPlEmpLitigation> edu = from emp in Data.tblPlEmpLitigations
                                                  //where emp.EmpID == empID//this one
                                              //orderby emp.Year descending
                                              select emp;
                return edu;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public tblPlEmpLitigation GetByID(int emplit, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                tblPlEmpLitigation EmpLit = Data.tblPlEmpLitigations.Single(p => p.EmpAcrID.Equals(emplit));

                return EmpLit;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public void Insert(tblPlEmpLitigation lit, RMSDataContext Data)
        {
            //ye BL banai Emp Edu k lye 
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.tblPlEmpLitigations.InsertOnSubmit(lit);
                Data.SubmitChanges();

            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }


        public void Update(tblPlEmpLitigation lit,RMSDataContext Data)
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
