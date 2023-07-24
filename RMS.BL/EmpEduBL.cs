using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.BL
{
    public class EmpEduBL
    {
        public EmpEduBL()
        {

        }
       
        public IQueryable GetEmployeeEducation(int empID, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable edu = from emp in Data.tblPlEmpEdus
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

        public IQueryable GetAll(RMSDataContext Data)
        {
            try
            {
                if (Data == null)
                {
                    Data = RMSDB.GetOject();
                }

                IQueryable EmpEdu = from edu in Data.tblPlEmpEdus
                                    orderby edu.EmpEduID
                                    select new
                                    {
                                        edu.DegreeTitle,
                                        edu.UniversityBoard,
                                        //edu.Year,
                                        edu.Verified
                                    };
                return EmpEdu;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public tblPlEmpEdu GetByID(int emped, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                tblPlEmpEdu EmpEdu = Data.tblPlEmpEdus.Single(p => p.EmpEduID.Equals(emped));

                return EmpEdu;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        

        public void Insert(tblPlEmpEdu edu, RMSDataContext Data)
        {
            //ye BL banai Emp Edu k lye 
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.tblPlEmpEdus.InsertOnSubmit(edu);
                Data.SubmitChanges();
               
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }


        public void Update(tblPlEmpEdu id,  RMSDataContext Data)
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
