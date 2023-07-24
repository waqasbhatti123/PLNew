using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.BL
{
    public class EmpCpfBL
    {
        public EmpCpfBL()
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

                IQueryable EmpCpf = from cpf in Data.tblPlEmpCpfs
                                    orderby cpf.EmpAcrID
                                    select new
                                    {
                                        cpf.ApplicationDate,
                                        cpf.SnactOrderDate,
                                        cpf.AdvRelDate,
                                        cpf.AccountHead
                                    };
                return EmpCpf;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public IQueryable<tblPlEmpCpf> GetEmployeeCpf(int empID, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<tblPlEmpCpf> edu = from emp in Data.tblPlEmpCpfs
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

        public tblPlEmpCpf GetByID(int empcpf, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                tblPlEmpCpf EmpCpf = Data.tblPlEmpCpfs.Single(p => p.EmpAcrID.Equals(empcpf));

                return EmpCpf;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public void Insert(tblPlEmpCpf cpf, RMSDataContext Data)
        {
            //ye BL banai Emp Edu k lye 
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.tblPlEmpCpfs.InsertOnSubmit(cpf);
                Data.SubmitChanges();

            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }


        public void Update(tblPlEmpCpf cpf,RMSDataContext Data)
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
