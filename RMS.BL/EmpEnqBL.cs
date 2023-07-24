using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.BL
{
    public class EmpEnqBL
    {
        public EmpEnqBL()
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

                IQueryable EmpEnq = from enq in Data.tblPlEmpEnqs
                                    orderby enq.EmpAcrID
                                    select new
                                    {
                                        enq.EnquiryAud,
                                        enq.EnquiryDate,
                                        enq.IssuAut,
                                        enq.Statuss,
                                        enq.Remarks
                                    };
                return EmpEnq;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }


        public IQueryable<tblPlEmpEnq> GetEmployeeEnq(int empID, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<tblPlEmpEnq> edu = from emp in Data.tblPlEmpEnqs
                                                  //where emp.EmpID == empID//this one
                                             // orderby emp.Year descending
                                              select emp;
                return edu;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public tblPlEmpEnq GetByID(int empenq, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                tblPlEmpEnq EmpEnq = Data.tblPlEmpEnqs.Single(p => p.EmpAcrID.Equals(empenq));

                return EmpEnq;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public void Insert(tblPlEmpEnq enq, RMSDataContext Data)
        {
            //ye BL banai Emp Edu k lye 
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.tblPlEmpEnqs.InsertOnSubmit(enq);
                Data.SubmitChanges();

            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }


        public void Update(tblPlEmpEnq emp,RMSDataContext Data)
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
