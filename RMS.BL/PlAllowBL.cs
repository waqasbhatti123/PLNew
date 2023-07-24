using System;
using System.Linq;
using System.Text;

namespace RMS.BL
{
    public class PlAllowBL
    {
        public PlAllowBL()
        {

        }

        public Object GetAllSalaryPackage(int brID,string empName, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                var empSearchList = from emp in Data.tblPlAlows
                                    orderby emp.IsActive descending 
                                    where  emp.tblPlEmpData.BranchID == brID
                                    && emp.tblPlEmpData.FullName.Contains(empName)
                                    select new
                                    {
                                        emp.tblPlEmpData.EmpID,
                                        emp.tblPlEmpData.BranchID,
                                        emp_Id = "EN-" + emp.tblPlEmpData.EmpID,
                                        emp.tblPlEmpData.EmpCode,
                                        emp.tblPlEmpData.FullName,
                                        emp.CompID,
                                        emp.Basic,
                                        emp.HR,
                                        emp.Utilities,
                                        emp.EffDate,
                                        emp.NSHA,
                                        emp.SplAlow,
                                        emp.CA,
                                        emp.MessDed,
                                        emp.TaxDed,
                                        emp.OtherDed,
                                        emp.OtrAlow,
                                        emp.IsActive,
                                        emp.fromPeriod,
                                        emp.toPeriod

                                    };
                return empSearchList;

                //Branch branchObj = Data.Branches.Where(x => x.br_id == brID).FirstOrDefault();
                //if(isSearch == true)
                //{
                   
                //}
                //else
                //{
                //    if (brID == 1)
                //    {
                //        var emps = from emp in Data.tblPlAlows
                //                   orderby emp.EffDate descending
                //                   //where emp.EmpID == (EmpID == null ? emp.EmpID : EmpID)
                //                   select new
                //                   {
                //                       emp.tblPlEmpData.EmpID,
                //                       emp.tblPlEmpData.BranchID,
                //                       emp_Id = "EN-" + emp.tblPlEmpData.EmpID,
                //                       emp.tblPlEmpData.EmpCode,
                //                       emp.tblPlEmpData.FullName,
                //                       emp.CompID,
                //                       emp.Basic,
                //                       emp.HR,
                //                       emp.Utilities,
                //                       emp.EffDate,
                //                       emp.NSHA,
                //                       emp.SplAlow,
                //                       emp.CA,
                //                       emp.MessDed,
                //                       emp.TaxDed,
                //                       emp.OtherDed,
                //                       emp.OtrAlow,
                //                       emp.IsActive

                //                   };
                //        return emps;
                //    }
                //    else
                //    {
                //        var empsBranchList = from emp in Data.tblPlAlows
                //                             orderby emp.EffDate descending
                //                             where emp.tblPlEmpData.BranchID == brID
                //                             select new
                //                             {
                //                                 emp.tblPlEmpData.EmpID,
                //                                 emp.tblPlEmpData.BranchID,
                //                                 emp_Id = "EN-" + emp.tblPlEmpData.EmpID,
                //                                 emp.tblPlEmpData.EmpCode,
                //                                 emp.tblPlEmpData.FullName,
                //                                 emp.CompID,
                //                                 emp.Basic,
                //                                 emp.HR,
                //                                 emp.Utilities,
                //                                 emp.EffDate,
                //                                 emp.NSHA,
                //                                 emp.SplAlow,
                //                                 emp.CA,
                //                                 emp.MessDed,
                //                                 emp.TaxDed,
                //                                 emp.OtherDed,
                //                                 emp.OtrAlow,
                //                                 emp.IsActive

                //                             };

                //        if(branchObj.IsDisplay == true)
                //        {
                //            var empsSubBranchList = from emp in Data.tblPlAlows
                //                                    orderby emp.EffDate descending
                //                                    where emp.tblPlEmpData.Branch1.br_idd == brID
                //                                    && emp.tblPlEmpData.Branch1.br_status == true

                //                                    //where emp.tblPlEmpData.Branch1.IsDisplay.Equals(true) ?
                //                                    //emp.tblPlEmpData.Branch1.br_idd == brID :
                //                                    //emp.tblPlEmpData.Branch1.br_idd == -1

                //                                    select new
                //                                    {
                //                                        emp.tblPlEmpData.EmpID,
                //                                        emp.tblPlEmpData.BranchID,
                //                                        emp_Id = "EN-" + emp.tblPlEmpData.EmpID,
                //                                        emp.tblPlEmpData.EmpCode,
                //                                        emp.tblPlEmpData.FullName,
                //                                        emp.CompID,
                //                                        emp.Basic,
                //                                        emp.HR,
                //                                        emp.Utilities,
                //                                        emp.EffDate,
                //                                        emp.NSHA,
                //                                        emp.SplAlow,
                //                                        emp.CA,
                //                                        emp.MessDed,
                //                                        emp.TaxDed,
                //                                        emp.OtherDed,
                //                                        emp.OtrAlow,
                //                                        emp.IsActive

                //                                    };


                //            return empsBranchList.Concat(empsSubBranchList).ToList();
                //        }

                //        else
                //        {
                //            return empsBranchList.ToList();
                //        }
                //    }
                //}
                
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public Object GetAllSalaryTranfers(int brID,int monthID, string empName,  RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var objList = from trfObj in Data.SalaryTranferTbls
                              orderby trfObj.SalTrfID descending
                              where trfObj.IsActive == true
                              && trfObj.tblPlEmpData.BranchID == brID
                              && trfObj.tblPlEmpData.FullName.Contains(empName)
                              && trfObj.SalaryMonth == (monthID == 0? trfObj.SalaryMonth : monthID)
                              select new
                              {
                                  trfObj.SalTrfID,
                                  trfObj.tblPlEmpData.EmpID,
                                  trfObj.tblPlEmpData.FullName,
                                  trfObj.Basic,
                                  trfObj.SalaryMonth,
                                  trfObj.TblSalaryMonth.MonthVal


                              };
                return objList;

                //if(brID == 1)
                //{

                //}
                //else
                //{
                //    var objBranchList = from trfObj in Data.SalaryTranferTbls
                //                  orderby trfObj.SalTrfID descending
                //                  where trfObj.IsActive == true
                //                  && trfObj.tblPlEmpData.BranchID == brID
                //                  select new
                //                  {
                //                      trfObj.SalTrfID,
                //                      trfObj.tblPlEmpData.EmpID,
                //                      trfObj.tblPlEmpData.FullName,
                //                      trfObj.Basic,
                //                      trfObj.SalaryMonth,
                //                      trfObj.TblSalaryMonth.MonthVal


                //                  };

                //    var objSubBranchList = from trfObj in Data.SalaryTranferTbls
                //                        orderby trfObj.SalTrfID descending
                //                        where trfObj.IsActive == true
                //                        && trfObj.tblPlEmpData.Branch1.IsDisplay == true ?
                //                           trfObj.tblPlEmpData.Branch1.br_idd == brID :
                //                           trfObj.tblPlEmpData.Branch1.br_idd == -1
                //                        select new
                //                        {
                //                            trfObj.SalTrfID,
                //                            trfObj.tblPlEmpData.EmpID,
                //                            trfObj.tblPlEmpData.FullName,
                //                            trfObj.Basic,
                //                            trfObj.SalaryMonth,
                //                            trfObj.TblSalaryMonth.MonthVal


                //                        };

                //    return objBranchList.Concat(objSubBranchList).ToList();
                //}

            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }


        public Object GetAll(int? EmpID, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var emps = from emp in Data.tblPlAlows
                           orderby emp.EffDate descending
                           where emp.EmpID == (EmpID==null? emp.EmpID: EmpID)
                           select new
                           {
                               emp.tblPlEmpData.EmpID,
                               emp_Id = "EN-"+emp.tblPlEmpData.EmpID,
                               emp.tblPlEmpData.EmpCode,
                               emp.tblPlEmpData.FullName,
                               emp.CompID,
                               emp.Basic,
                               emp.HR,
                               emp.Utilities,
                               emp.EffDate,
                               emp.NSHA,
                               emp.SplAlow,
                               emp.CA,
                               emp.MessDed,
                               emp.TaxDed,
                               emp.OtherDed

                           };
                return emps;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public Object GetAllPaymentTypeCombo(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var expTypes = from expType in Data.tblPlExpTypes
                               orderby expType.ExpTypeDesc
                               select new
                               {
                                   expType.ExpTypeDesc,
                                   expType.ExpTypeID

                               };
                return expTypes;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public bool ISAlreadyExist(tblPlAlow brancho, RMSDataContext Data)
        {

            bool isalready = false;
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                int effperiod = 0;
                int.TryParse(brancho.EffDate.ToString("yyyyMM"), out effperiod);

                IQueryable<tblPlAlow> allows = from allow in Data.tblPlAlows
                                               join comp in Data.tblCompanies on allow.CompID equals comp.CompID
                                                where allow.CompID == brancho.CompID && allow.EmpID == brancho.EmpID
                                                && comp.CurPayPeriod == effperiod 
                                                select allow;

                if (allows != null & allows.Count<tblPlAlow>() > 0)
                    isalready = true;
                return isalready;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public bool ISAlreadyExist4UpdateSal(tblPlAlow updSal, RMSDataContext Data)
        {

            bool isalready = false;
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                

                IQueryable<tblPlAlow> allows = from allow in Data.tblPlAlows
                                               join comp in Data.tblCompanies on allow.CompID equals comp.CompID
                                               where allow.CompID == updSal.CompID && allow.EmpID == updSal.EmpID
                                               && allow.EffDate == updSal.EffDate
                                               select allow;

                if (allows != null & allows.Count<tblPlAlow>() > 0)
                    isalready = true;
                return isalready;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public tblPlAlow GetByID(int Cmpid, int empid, DateTime effdate, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                tblPlAlow allows = Data.tblPlAlows.Single(p => 
                    p.CompID == Cmpid && p.EmpID == empid && p.EffDate == effdate);

                return allows;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public tblPlAlow GetByID(int Cmpid, int empid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                tblPlAlow allows = (from dt in Data.tblPlAlows
                                   where dt.CompID == Cmpid && dt.EmpID == empid
                                   orderby dt.EffDate descending
                                   select dt).SingleOrDefault();

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

        public string Insert(tblPlAlow allow, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.tblPlAlows.InsertOnSubmit(allow);

                string str = SaveGlAC(allow, Data);
                
                if (!str.Equals("ok"))
                    return str;

                Data.SubmitChanges();

                return "ok";
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public string Update(tblPlAlow allow, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                string str = SaveGlAC(allow, Data);

                if (!str.Equals("ok"))
                    return str;

                Data.SubmitChanges();

                return "ok";
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public string SaveGlAC(tblPlAlow allow, RMSDataContext Data)
        {
            StringBuilder str = new StringBuilder();
            str.Append("ok");
            if (Data == null) { Data = RMSDB.GetOject(); }
            if (allow.OtherDed > 0)
            {
                Int16 depid = Convert.ToInt16(Data.tblPlEmpDatas.Where(emp => emp.EmpID == allow.EmpID).Single().DeptID);
                /////////////////////////////OTHER DEDUCTION
                if (string.IsNullOrEmpty(new PlCodeBL().GetByID(depid, Data).OtrDed))
                {
                    str.Remove(0, 2);
                    str.Append("Ctrl Other A/C is missing, Plz update department");
                }
                string ctrlglcode = new PlCodeBL().GetByID(depid, Data).OtrDed;
                string ctrlgtcd = Data.Glmf_Codes.Where(gl=> gl.gl_cd == ctrlglcode).Single().gt_cd;
                string glcd = ctrlglcode + allow.EmpID.ToString().PadLeft(4, '0');
                Glmf_Code cd = Data.Glmf_Codes.Where(c => c.gl_cd == glcd).SingleOrDefault();
                if (cd == null)
                {
                    Glmf_Code code = new Glmf_Code();
                    code.gl_cd = glcd;
                    code.gl_dsc = "EN-" + allow.EmpID.ToString() + " Other Deduction";
                    code.ct_id = "D";
                    code.cnt_gl_cd = ctrlglcode;
                    code.updateon = RMS.BL.Common.MyDate(Data).Date;
                    code.updateby = "System";
                    code.gt_cd = ctrlgtcd;

                    Data.Glmf_Codes.InsertOnSubmit(code);
                    Data.SubmitChanges();
                }
            }
            if (allow.MessDed > 0)
            {
                Int16 depid = Convert.ToInt16(Data.tblPlEmpDatas.Where(emp => emp.EmpID == allow.EmpID).Single().DeptID);
                /////////////////////////////MISC DEDUCTION
                if (string.IsNullOrEmpty(new PlCodeBL().GetByID(depid, Data).MiscDed))
                {
                    str.AppendLine();
                    str.Append("Ctrl Misc A/C is missing, Plz update department");
                }
                string ctrlglcodemisc = new PlCodeBL().GetByID(depid, Data).MiscDed;
                string ctrlgtcdmisc = Data.Glmf_Codes.Where(gl => gl.gl_cd == ctrlglcodemisc).Single().gt_cd;
                string glcdmisc = ctrlglcodemisc + allow.EmpID.ToString().PadLeft(4, '0');
                Glmf_Code cdmisc = Data.Glmf_Codes.Where(c => c.gl_cd == glcdmisc).SingleOrDefault();
                if (cdmisc == null)
                {
                    Glmf_Code code = new Glmf_Code();
                    code.gl_cd = glcdmisc;
                    code.gl_dsc = "EN-" + allow.EmpID.ToString() + " Misc. Deduction";
                    code.ct_id = "D";
                    code.cnt_gl_cd = ctrlglcodemisc;
                    code.updateon = RMS.BL.Common.MyDate(Data).Date;
                    code.updateby = "System";
                    code.gt_cd = ctrlgtcdmisc;

                    Data.Glmf_Codes.InsertOnSubmit(code);
                    Data.SubmitChanges();
                }

            }
            return Convert.ToString(str);
        }
    }
}