using System;
using System.Linq;
using System.Collections.Generic;

namespace RMS.BL
{
    public class SalaryBL
    {
        RMSDataContext Data = new RMSDataContext();
        public SalaryBL()
        {

        }
        public string rptGenerateReportyTax(string Acc)
        {
            var tax = (from a in Data.Glmf_Datas
                       join type in Data.Vr_Types on a.vt_cd equals type.vt_cd
                       join chq in Data.Glmf_Data_chqs on a.vrid equals chq.vrid
                       join text in Data.tblTextPayables on a.vrid equals text.VrID
                       into textpay
                       from textpayable in textpay.DefaultIfEmpty()
                       where a.vt_cd == 64 && chq.vr_chq == Acc
                       orderby a.vr_dt ascending, a.vr_no descending
                       select new
                       {
                           IncomeTax = textpayable.ITAmount,
                           GST = textpayable.GSTAmount,
                           PRA = textpayable.PRA,
                           a.vrid,
                           vr_nrtn = a.vr_nrtn,
                           ref_no = a.Ref_no,
                           type = type.vt_use + (a.source != null && type.vt_use != a.source ? "-" + a.source : "") + '-' + a.vr_no,
                           cheNo = chq.vr_chq,
                           cheDat = chq.vr_chq_dt,
                           a.source,
                           headsInvolved = GetGLMFCode(a.vrid),
                       }).ToList();

            return tax.ToString();
        }


        public string GetGLMFCode(int vrId)
        {
            try
            {
                var codeDesc = from c in Data.Glmf_Data_Dets
                               join d in Data.Glmf_Codes on c.gl_cd equals d.gl_cd
                               where c.vrid == vrId
                               select d.gl_dsc;

                string code = "";
                foreach (var item in codeDesc)
                {
                    code += item + "\r\n";
                }
                return code;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public Object GetAll(RMSDataContext Data)
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

        public Object GetStartPaymentTypeCombo(int Compid, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var PayTypes = from x in Data.tblCompanies
                               where x.CompID==Compid
                               
                               select new
                               {
                                   x.CurPayPeriod
                               };
                return PayTypes;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        
        public Object GetAllPaymentTypeCombo(int empid,RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var PayTypes = from x in Data.tblPlSalaries
                               where x.EmpID==empid && x.PayType=='0'
                               orderby x.PayPerd
                               select new
                               {
                                    x.PayPerd    
                               };
                return PayTypes;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public Object GetPayPeriods(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var emps = (from emp in Data.tblPlSalaries
                            orderby emp.PayPerd
                            select new
                            {
                                emp.PayPerd
                            }).Distinct().OrderByDescending(t => t.PayPerd);
                return emps;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
   
        public tblPlEmpData GetEmpByCode(string Code, int compid, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                tblPlEmpData emp = Data.tblPlEmpDatas.Single(p => p.EmpCode == Code && p.CompID == compid);

                return emp;
            }
            catch (Exception ex)
            {
                tblPlEmpData emp=new tblPlEmpData(); 
                RMSDB.SetNull();
                return emp;
                throw ex;
            }
        }

        public tblPlSalary GetSalByid(int id,int compid,int paypred, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                
                //IEquatable<decimal> maxpred=(from a in Data.tblPlSalaries
                //                where a.EmpID==id && a.CompID==compid && a.PayType=='0'
                //                select a.PayPerd).Max();
                
                //tblPlSalary salary=from x in Data.tblPlSalaries
                //where x.EmpID==id && x.CompID==compid && x.PayType=='0'
                //select x;
                
                tblPlSalary salary = Data.tblPlSalaries.Single(p => p.EmpID == id && p.CompID==compid && p.PayType=='0' && p.PayPerd==paypred);
//from sal in Data.tblPlSalaries
//                           where sal.EmpID==id&& sal.CompID==compid
//                           select sal;
                           //{
                           // sal.Basic,
                           // sal.HR,
                           // sal.Utilities,
                           // sal.MedBil,
                           // sal.EDA,
                           // sal.PSMInc,
                           // sal.ExpClaim,
                           // sal.SIMInc,
                           // sal.LWOP,
                           // sal.MobDed,
                           // sal.OtrDed,
                           // sal.MobAlow,
                           // sal.ShopingDed,
                           // sal.EOBIDed,
                           // sal.PayPerd,
                           // sal.SalPerd,
                           // sal.PDays,
                           // sal.TaxDed
                           // };
                               //emp.EmpID,
                               //emp.EmpCode,
                               //emp.FullName,
                               ////Gender = emp.Sex.Equals("M") ? "Male" : "Female",
                               ////emp.CityName,
                               //Dept = emp.tblPlCode.CodeDesc,
                               //Desig = emp.tblPlCode1.CodeDesc,
                               ////   Div = emp.tblPlCode2.CodeDesc,
                               ////Reg = emp.tblPlCode3.CodeDesc,
                               ////emp.tblPlLocation.LocName

                           
                return salary;
            }
            catch (Exception ex)
            {
                tblPlSalary salary=new tblPlSalary();
                RMSDB.SetNull();
                return salary;
    //            throw ex;
            }
        }
        public IQueryable<tblPlEmpData> GetAllBranchCombo(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<tblPlEmpData> emps = from emp in Data.tblPlEmpDatas
                                                orderby emp.FullName
                                                select emp;
                return emps;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        
        public IQueryable<tblCompany> GetAllCompaniesCombo(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<tblCompany> comps = from c in Data.tblCompanies
                                               orderby c.CompName
                                               select c;
                return comps;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool ISAlreadyExist(tblPlEmpData empo, RMSDataContext Data)
        {

            bool isalready = false;
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                IQueryable<tblPlEmpData> emps = from emp in Data.tblPlEmpDatas
                                                where emp.EmpID != empo.EmpID && emp.FullName == empo.FullName
                                                select emp;

                if (emps != null & emps.Count<tblPlEmpData>() > 0)
                    isalready = true;
                return isalready;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public tblPlEmpData GetEmpByID(string id, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                tblPlEmpData emps = Data.tblPlEmpDatas.Single(p => p.EmpCode == id);

                return emps;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public tblPlLocation GetLoc(int locId, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                tblPlLocation locs = Data.tblPlLocations.Single(p => p.LocID == locId);

                return locs;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public void DeleteByID(int branchid, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                tblPlEmpData emp = Data.tblPlEmpDatas.Single(p => p.EmpID == branchid);
                Data.tblPlEmpDatas.DeleteOnSubmit(emp);
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }

        }
        public int CAlculateArrears(int empid, int salperd , RMSDataContext Data)
        {

            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                IQueryable<tblPlSalary> emp = from x in Data.tblPlSalaries
                                              where x.EmpID==empid && x.PayType=='1' && x.tblPlEmpData.EmpStatus==1 && x.PayPerd==salperd
                                              select x;
                int arr=0;
                
                if(emp!=null && emp.Count<tblPlSalary>()>0)
                {
                    foreach(tblPlSalary sal in emp)
                    {
                        arr+=(Convert.ToInt32(sal.Basic) + Convert.ToInt32(sal.HR) + Convert.ToInt32(sal.Utilities) + Convert.ToInt32(sal.CA) + Convert.ToInt32(sal.NSHA) + Convert.ToInt32(sal.UniformAlow) + Convert.ToInt32(sal.SplAlow));
                    }
                }
                return arr;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }


        }

        public void Insert(tblPlEmpData emp, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.tblPlEmpDatas.InsertOnSubmit(emp);
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public void Update(tblPlEmpData emp, RMSDataContext Data)
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

        public IQueryable<spSalaryTransferResult> RptSalTransferCash(int CompId, int payPeriod, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                IQueryable<spSalaryTransferResult> emps =
                    Data.spSalaryTransfer(CompId, payPeriod).AsQueryable();

                return emps;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public IQueryable<spSalaryTransferBankResult> RptSalTransferBank(int CompId, int payPeriod, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                IQueryable<spSalaryTransferBankResult> emps =
                    Data.spSalaryTransferBank(CompId, payPeriod).AsQueryable();

                return emps;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public IQueryable<spSalaryTransferBankNewResult> RptSalTransferBankNew(int CompId, int payPeriod, int deptid, int minsal, int maxsal, string jobtyp, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                IQueryable<spSalaryTransferBankNewResult> emps =
                    Data.spSalaryTransferBankNew(CompId, payPeriod, deptid, minsal, maxsal, jobtyp).AsQueryable();

                return emps;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public List<spSalaryPaymentSearchResult> GetSalPaymentSearch(int CompId, int payPeriod, int deptid, int minsal, int maxsal, string bankcode, string jobtyp, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                List<spSalaryPaymentSearchResult> emps =
                    Data.spSalaryPaymentSearch(CompId, payPeriod, deptid, minsal, maxsal, bankcode, jobtyp).ToList();

                return emps;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }


        public List<spSalaryPayableReportResult> GetSalPayable(int CompId, int payPeriod, int deptid, int minsal, int maxsal, string bankcode, string jobtyp, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                List<spSalaryPayableReportResult> emps =
                    Data.spSalaryPayableReport(CompId, payPeriod, deptid, minsal, maxsal, bankcode, jobtyp).ToList();

                return emps;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }


        public List<spSalaryPaymentGridResult> GetSalPaymentGrid(int CompId, int payPeriod, int vrid, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                List<spSalaryPaymentGridResult> emps =
                    Data.spSalaryPaymentGrid(CompId, payPeriod, vrid).ToList();

                return emps;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public IQueryable<spSalaryListResult> rptEmpListEOBI(int CompId, int payPeriod, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                IQueryable<spSalaryListResult> emps =
                    Data.spSalaryList(CompId, payPeriod).AsQueryable();

                return emps;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public IQueryable<spRptEmployeeListResult> rptEmpSlip(int compid, int payPeriod, 
            string empcode, string empcodeto, RMSDataContext Data)
        {
            IQueryable<spRptEmployeeListResult> emp;
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                emp = Data.spRptEmployeeList(compid, payPeriod).AsQueryable();

                if (empcode.Trim().Length > 0)
                {
                    if (empcodeto.Trim().Length == 0)
                    {
                        emp = emp.Where(t => t.empcode == empcode).AsQueryable();
                    }
                    else
                    {
                        emp = emp.Where(t => int.Parse(t.empcode) >= int.Parse(empcode) && int.Parse(t.empcode) <= int.Parse(empcodeto));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return emp;
        }

        public List<spGratuityStatmentResult> GetGratuityStatement(int compid, DateTime todate, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                return Data.spGratuityStatment(compid, todate).ToList();
            }
            catch{ }
            return null;
        }


        public List<spEarningRecResult> GetEarningRecords(int compid, int fromPayPrd, int toPayPrd, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                return Data.spEarningRec(compid, fromPayPrd, toPayPrd).ToList();
            }
            catch { }
            return null;
        }

    }
}