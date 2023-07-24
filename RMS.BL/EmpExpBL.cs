using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.BL
{
    public class EmpExpBL
    {
        public EmpExpBL()
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

                IQueryable EmpExp = from exp in Data.tblPlEmpExps
                                    orderby exp.EmpExpID
                                    select new
                                    {
                                        exp.Appointedas,
                                        exp.Postedas,
                                        exp.Department,
                                        exp.CodeID,
                                        exp.Scale,
                                        exp.joinDate,
                                        exp.YOE
                                    };
                return EmpExp;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public IQueryable<tblPlEmpExp> GetEmployeeExperience(int empID, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<tblPlEmpExp> edu = from emp in Data.tblPlEmpExps
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
        public tblPlEmpExp GetByID(int empexp, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                tblPlEmpExp EmpExp = Data.tblPlEmpExps.Single(p => p.EmpExpID.Equals(empexp));

                return EmpExp;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public void Insert(tblPlEmpExp exp, RMSDataContext Data)
        {
            //ye BL banai Emp Edu k lye 
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.tblPlEmpExps.InsertOnSubmit(exp);
                Data.SubmitChanges();

            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }


        public void Update(tblPlEmpExp emp, RMSDataContext Data)
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
