using System;
using System.Linq;
using System.Collections.Generic;

namespace RMS.BL
{
    public class PlLoanBL
    {
        public PlLoanBL()
        {

        }

        public Object GetAll(int empid,int cid,RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var emps = from emp in Data.tblPlLoans
                           where emp.EmpID==empid && emp.CompID==cid 
                           orderby emp.tblPlEmpData.EmpCode
                           select new
                           {
                               CompID=emp.CompID,
                               EmpID=emp.EmpID,
                               emp.LoanID,
                               LoanTypeID=emp.LoanTypeID,
                               PaymentRef=emp.PaymentRef,
                               EmpCode=emp.tblPlEmpData.EmpCode,
                               emp.tblPlEmpData.FullName,
                               RefDate = emp.PaymentDate,
                               Effdate = emp.InstStartDate,
                               DedAmt=emp.InstAmt,
                               emp.LoanAmt,
                               emp.NoOfInst,
                               emp.LoanAppStatus
                           };
                return emps;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public Object GetAllLoanTypeCombo(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var lonTypes = from lonType in Data.tblPlLoanTypes
                               where lonType.LoanTypeID == "ADV"
                               orderby lonType.LoanTypeDesc
                               select new
                               {
                                   lonType.LoanTypeID,
                                   lonType.LoanTypeDesc

                               };
                return lonTypes;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public bool ISAlreadyExist(tblPlLoan lon, RMSDataContext Data)
        {

            bool isalready = false;
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                IQueryable<tblPlLoan> allows = from allow in Data.tblPlLoans
                                                where (allow.CompID == lon.CompID && allow.EmpID == lon.EmpID && allow.LoanTypeID==lon.LoanTypeID && allow.PaymentRef == lon.PaymentRef) || ( allow.LoanTypeID==lon.LoanTypeID && allow.EmpID==lon.EmpID && allow.InstStartDate==lon.InstStartDate)
                                                select allow;

                if (allows != null & allows.Count<tblPlLoan>() > 0)
                    isalready = true;
                return isalready;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public tblPlLoan GetByID(int Cmpid, int empid,int loanid, string LoanTypeID ,string PaymentRef, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                tblPlLoan allows = Data.tblPlLoans.Single(p => 
                    p.CompID == Cmpid && p.EmpID == empid && p.LoanID == loanid && p.LoanTypeID == LoanTypeID && p.PaymentRef== PaymentRef);

                return allows;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }


        public void DeleteByID(int Cmpid, int empid, DateTime effdate, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                tblPlAlow allow = Data.tblPlAlows.Single(p =>
                    p.CompID == Cmpid && p.EmpID == empid && p.EffDate == effdate);

                Data.tblPlAlows.DeleteOnSubmit(allow);
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }

        }

        public void Insert(tblPlLoan lon, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.tblPlLoans.InsertOnSubmit(lon);
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public void Update(tblPlLoan lon, RMSDataContext Data)
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


        public bool DeleteLoan(tblPlLoan tblLn, int vrid, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                tblPlLoan tblLoan = Data.tblPlLoans.Where(lv => lv.CompID.Equals(tblLn.CompID) && lv.EmpID.Equals(tblLn.EmpID) && lv.LoanTypeID.Equals(tblLn.LoanTypeID) && lv.PaymentRef.Equals(tblLn.PaymentRef)).SingleOrDefault();
                Data.tblPlLoans.DeleteOnSubmit(tblLoan);

                //List<Glmf_Data_Det> glmfDet = Data.Glmf_Data_Dets.Where(glDet => glDet.vrid.Equals(vrid)).ToList();
                //Data.Glmf_Data_Dets.DeleteAllOnSubmit(glmfDet);

                //Glmf_Data_chq chq = Data.Glmf_Data_chqs.Where(c => c.vrid.Equals(vrid)).SingleOrDefault();
                //Data.Glmf_Data_chqs.DeleteOnSubmit(chq);

                //Glmf_Data data = Data.Glmf_Datas.Where(d => d.vrid.Equals(vrid)).SingleOrDefault();
                //Data.Glmf_Datas.DeleteOnSubmit(data);


                Data.SubmitChanges();
                return true;
            }
            catch //(Exception ex)
            {
                //RMSDB.SetNull();
                //throw ex;
            }
            return false;
        }

    }
}