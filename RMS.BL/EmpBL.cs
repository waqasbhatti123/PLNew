using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Linq;
using RMS.BL;
using RMS.BL.Model;

namespace RMS.BL
{
    public class EmpBL
    {
        public EmpBL()
        {

        }

        public object GetAllEmployees(int br,RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var emps = from emp in Data.tblPlEmpDatas
                           orderby emp.FullName
                           where emp.EmpStatus == 1
                           && emp.BranchID == br
                           select new
                           {
                               emp.EmpID,
                               emp_Id = "EN-" + emp.EmpID,
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
        public IQueryable<tblYear> GetYear( RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<tblYear> edu = from emp in Data.tblYears
                                             // where emp.EmpID == empID//this one
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

        public IQueryable<tblYear> GetYearPro(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<tblYear> edu = from emp in Data.tblYears
                                          where emp.YearID >= 51

                                          // where emp.EmpID == empID//this one
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
        public object GetEmployeeEnq(int empID, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var edu = from emp in Data.tblPlEmpEnqs
                                              where emp.EmpID == empID//this one
                                                                      //orderby emp.Year descending
                                              select new
                                              {
                                                  emp.EmpAcrID,
                                                  emp.EnqTitle,
                                                  emp.EnquiryAud,
                                                  emp.EnquiryDate,
                                                  emp.IssuAut,
                                                  emp.Remarks,
                                                  emp.Statuss,
                                                  emp.Attachment,
                                                  emp.EmpID,
                                                  
                                              };
                return edu;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public object GetEmployeeLiti(int empID, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var edu = from emp in Data.tblPlEmpLitigations
                          where emp.EmpID == empID//this one
                                                  //orderby emp.Year descending
                          select new
                          {
                              emp.EmpID,
                              emp.EmpAcrID,
                              emp.LitiDate,
                              emp.tblPlLiti.LitiName,
                              emp.Authority,
                              emp.Remarks,
                              emp.Status,
                              emp.LitiTitle,
                              emp.Attachment
                          };
                return edu;
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
                                              where emp.EmpID == empID
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

       
        

        public List<sp_Profiledbo> getContact(int brID, int DesignID,int SecID, int fromsclae,int toscale,int jobtypeID,
           int fromage,int toage,string domicile, string gender, string disablity, string religion, string additional,
           string quota,string poli,int sortOrder,int appoited , string ddldrop,RMSDataContext Data)
        {
            List<sp_GetContactsResult> cont = Data.sp_GetContacts(brID, ddldrop).ToList();
            List<sp_Profiledbo> emp = new List<sp_Profiledbo>();

             tblPlEmpData lasst = Data.tblPlEmpDatas.Where(x => x.LastperDes == appoited).FirstOrDefault();
            int? lastperID;
            if (lasst != null)
            {
                lastperID = Convert.ToInt32(lasst.LastperDes);
            }
            else
            {
                 lastperID = null;
            }
            
            tblPlEmpData appIDD = Data.tblPlEmpDatas.Where(x => x.Appointed == appoited).FirstOrDefault();
            int? appID;
            if (appIDD != null)
            {
                 appID = Convert.ToInt32(appIDD.Appointed);
            }
            else
            {
                appID = null;
            }

            foreach (var item in cont)
            {
                var e = new sp_Profiledbo();
                e.ID = item.EmpID;
                e.FullName = item.FullName;
                e.designation = item.DesigID;
                e.Des = item.Designation;
                e.Section = item.DeptID;
                if (item.DOB == null)
                {
                    e.Age = null;
                }
                else
                {
                    e.Age = (DateTime.Now.Year - Convert.ToDateTime(item.DOB).Year) - 1;
                }
                e.br = item.BranchID;
                e.brName = item.BrName;
                e.JobType = item.JobNameID;
                e.Religion = item.Religion;
                e.scale = item.ScaleID;
                e.Domicil = item.Domicil;
                e.Gender = item.Sex.ToString();
                e.police = item.polveri;
                e.JoinDate = item.DOJ;
                e.AddtionCharg = item.AddtionalCharg;
                e.ScaleName = item.ScaleName;
                e.JobeTypeName = item.JobTypeName;
                if (item.Disbality == "0")
                {
                    e.Disablity = "None";
                }
                else
                {
                    e.Disablity = item.Disbality;
                }
                e.Quota = item.Quota;
                e.SectionName = item.Department;
                e.Contactno = item.TelNo;
                e.Mobno = item.MobNo;
                e.BOD = item.DOB;
                e.Email = item.Email;
                e.AppointedID = Convert.ToInt32(item.Appointed);
                e.AppointedName = item.appdesc;
                e.LastPerID = Convert.ToInt32(item.LastperDes);
                e.LastPerName = item.lastpermotion;
                e.SortDest =Convert.ToInt32(item.Desgsort);
                e.coln = item.coluu;
                emp.Add(e);
            }

            if (appoited != 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (appoited == 1)
                {
                    if (sortOrder == 0)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 1)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 2)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderByDescending(x => x.Section).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Section).ToList();
                        }
                    }
                    else if (sortOrder == 3)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 4)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderByDescending(x => x.JobType).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.JobType).ToList();
                        }
                    }
                    else if (sortOrder == 5)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderByDescending(x => x.Domicil).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Domicil).ToList();
                        }
                    }
                    else if (sortOrder == 6)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderByDescending(x => x.Age).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Age).ToList();
                        }
                    }
                    else if (sortOrder == 7)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderByDescending(x => x.Gender).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Gender).ToList();
                        }
                    }
                    else if (sortOrder == 8)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderByDescending(x => x.Disablity).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Disablity).ToList();
                        }
                    }
                    else if (sortOrder == 9)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderByDescending(x => x.Religion).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Religion).ToList();
                        }
                    }
                    else if (sortOrder == 10)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 11)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderByDescending(x => x.Quota).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Quota).ToList();
                        }
                    }
                    else if (sortOrder == 12)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderByDescending(x => x.police).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.police).ToList();
                        }
                    }
                    else if (sortOrder == 13)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 1)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 2)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderByDescending(x => x.Section).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderByDescending(x => x.Section).ToList();
                        }
                    }
                    else if (sortOrder == 3)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 4)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderByDescending(x => x.JobType).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderByDescending(x => x.JobType).ToList();
                        }
                    }
                    else if (sortOrder == 5)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderByDescending(x => x.Domicil).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderByDescending(x => x.Domicil).ToList();
                        }
                    }
                    else if (sortOrder == 6)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderByDescending(x => x.Age).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderByDescending(x => x.Age).ToList();
                        }
                    }
                    else if (sortOrder == 7)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderByDescending(x => x.Gender).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderByDescending(x => x.Gender).ToList();
                        }
                    }
                    else if (sortOrder == 8)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderByDescending(x => x.Disablity).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderByDescending(x => x.Disablity).ToList();
                        }
                    }
                    else if (sortOrder == 9)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderByDescending(x => x.Religion).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderByDescending(x => x.Religion).ToList();
                        }
                    }
                    else if (sortOrder == 10)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 11)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderByDescending(x => x.Quota).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderByDescending(x => x.Quota).ToList();
                        }
                    }
                    else if (sortOrder == 12)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderByDescending(x => x.police).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderByDescending(x => x.police).ToList();
                        }
                    }
                    else if (sortOrder == 13)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderBy(x => x.SortDest).ToList();
                        }
                    }
                }
            }
            
            if (appoited == 0 && DesignID != 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (DesignID == 1)
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderBy(x => x.SortDest).ToList();
                    }
                }
                
            }
            if (appoited == 0 && DesignID == 0 && SecID != 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (SecID == 1)
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderBy(x => x.SortDest).ToList();
                    }
                }
                //emp = emp.Where(x => x.Section == SecID).ToList();
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae != 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (fromsclae == 0)
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.OrderByDescending(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderBy(x => x.SortDest).ToList();
                    }
                }
                //emp =  emp.Where(x => x.scale >= fromsclae).ToList();
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale != 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (toscale == 1)
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderBy(x => x.SortDest).ToList();
                    }
                }
                //emp =  emp.Where(x => x.scale >= toscale).ToList();
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae != 0 && toscale != 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (fromsclae == 1 && toscale == 1)
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderBy(x => x.SortDest).ToList();
                    }
                }
                //emp =  emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).ToList();
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID != 0 && domicile == "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (jobtypeID == 9)
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderBy(x => x.SortDest).ToList();
                    }
                }
               // emp = emp.Where(x => x.JobType == jobtypeID).ToList();
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile != "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (domicile == "1")
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderBy(x => x.SortDest).ToList();
                    }
                }
                //emp = emp.Where(x => x.Domicil == domicile).ToList();
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage != 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (fromage == 1)
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderBy(x => x.SortDest).ToList();
                    }
                }
                //emp = emp.Where(x => x.Age >= fromage).ToList();
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage != 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (toage == 1)
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderBy(x => x.SortDest).ToList();
                    }
                }
                //emp = emp.Where(x => x.Age >= toage).ToList();
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage != 0 && toage != 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (fromage == 1 && toage == 1)
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderBy(x => x.SortDest).ToList();
                    }
                }
                //emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).ToList();
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage == 0 && gender != "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (gender == "1")
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderBy(x => x.SortDest).ToList();
                    }
                }
                //emp = emp.Where(x => x.Gender == gender).ToList();
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity != "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (disablity == "1")
                {
                    if (sortOrder == 0)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 1)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 2)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Section).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Section).ToList();
                        }
                    }
                    else if (sortOrder == 3)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 4)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.JobType).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.JobType).ToList();
                        }
                    }
                    else if (sortOrder == 5)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Domicil).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Domicil).ToList();
                        }
                    }
                    else if (sortOrder == 6)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Age).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Age).ToList();
                        }
                    }
                    else if (sortOrder == 7)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Gender).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Gender).ToList();
                        }
                    }
                    else if (sortOrder == 8)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Disablity).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Disablity).ToList();
                        }
                    }
                    else if (sortOrder == 9)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Religion).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Religion).ToList();
                        }
                    }
                    else if (sortOrder == 10)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 11)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Quota).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Quota).ToList();
                        }
                    }
                    else if (sortOrder == 12)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.police).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.police).ToList();
                        }
                    }
                    else if (sortOrder == 13)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 1)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 2)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderByDescending(x => x.Section).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderByDescending(x => x.Section).ToList();
                        }
                    }
                    else if (sortOrder == 3)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 4)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderByDescending(x => x.JobType).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderByDescending(x => x.JobType).ToList();
                        }
                    }
                    else if (sortOrder == 5)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderByDescending(x => x.Domicil).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderByDescending(x => x.Domicil).ToList();
                        }
                    }
                    else if (sortOrder == 6)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderByDescending(x => x.Age).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderByDescending(x => x.Age).ToList();
                        }
                    }
                    else if (sortOrder == 7)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderByDescending(x => x.Gender).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderByDescending(x => x.Gender).ToList();
                        }
                    }
                    else if (sortOrder == 8)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderByDescending(x => x.Disablity).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderByDescending(x => x.Disablity).ToList();
                        }
                    }
                    else if (sortOrder == 9)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderByDescending(x => x.Religion).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderByDescending(x => x.Religion).ToList();
                        }
                    }
                    else if (sortOrder == 10)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 11)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderByDescending(x => x.Quota).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderByDescending(x => x.Quota).ToList();
                        }
                    }
                    else if (sortOrder == 12)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderByDescending(x => x.police).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderByDescending(x => x.police).ToList();
                        }
                    }
                    else if (sortOrder == 13)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderBy(x => x.SortDest).ToList();
                        }
                    }
                }
                
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion != "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (religion == "1")
                {
                    if (sortOrder == 0)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 1)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 2)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderByDescending(x => x.Section).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Section).ToList();
                        }
                    }
                    else if (sortOrder == 3)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 4)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderByDescending(x => x.JobType).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.JobType).ToList();
                        }
                    }
                    else if (sortOrder == 5)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderByDescending(x => x.Domicil).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Domicil).ToList();
                        }
                    }
                    else if (sortOrder == 6)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderByDescending(x => x.Age).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Age).ToList();
                        }
                    }
                    else if (sortOrder == 7)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderByDescending(x => x.Gender).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Gender).ToList();
                        }
                    }
                    else if (sortOrder == 8)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderByDescending(x => x.Disablity).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Disablity).ToList();
                        }
                    }
                    else if (sortOrder == 9)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderByDescending(x => x.Religion).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Religion).ToList();
                        }
                    }
                    else if (sortOrder == 10)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 11)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderByDescending(x => x.Quota).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Quota).ToList();
                        }
                    }
                    else if (sortOrder == 12)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderByDescending(x => x.police).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.police).ToList();
                        }
                    }
                    else if (sortOrder == 13)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderBy(x => x.Des).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.Des).ToList();
                        }
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 1)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 2)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderByDescending(x => x.Section).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderByDescending(x => x.Section).ToList();
                        }
                    }
                    else if (sortOrder == 3)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 4)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderByDescending(x => x.JobType).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderByDescending(x => x.JobType).ToList();
                        }
                    }
                    else if (sortOrder == 5)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderByDescending(x => x.Domicil).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderByDescending(x => x.Domicil).ToList();
                        }
                    }
                    else if (sortOrder == 6)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderByDescending(x => x.Age).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderByDescending(x => x.Age).ToList();
                        }
                    }
                    else if (sortOrder == 7)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderByDescending(x => x.Gender).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderByDescending(x => x.Gender).ToList();
                        }
                    }
                    else if (sortOrder == 8)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderByDescending(x => x.Disablity).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderByDescending(x => x.Disablity).ToList();
                        }
                    }
                    else if (sortOrder == 9)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderByDescending(x => x.Religion).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderByDescending(x => x.Religion).ToList();
                        }
                    }
                    else if (sortOrder == 10)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 11)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderByDescending(x => x.Quota).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderByDescending(x => x.Quota).ToList();
                        }
                    }
                    else if (sortOrder == 12)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderByDescending(x => x.police).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderByDescending(x => x.police).ToList();
                        }
                    }
                    else if (sortOrder == 13)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderBy(x => x.Des).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderBy(x => x.Des).ToList();
                        }
                    }
                }
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional != "0" && quota == "0" && poli == "0")
            {
                if (additional == "1")
                {
                    if (sortOrder == 0)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 1)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 2)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Section).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Section).ToList();
                        }
                    }
                    else if (sortOrder == 3)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 4)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.JobType).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.JobType).ToList();
                        }
                    }
                    else if (sortOrder == 5)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Domicil).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Domicil).ToList();
                        }
                    }
                    else if (sortOrder == 6)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Age).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Age).ToList();
                        }
                    }
                    else if (sortOrder == 7)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Gender).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Gender).ToList();
                        }
                    }
                    else if (sortOrder == 8)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Disablity).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Disablity).ToList();
                        }
                    }
                    else if (sortOrder == 9)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Religion).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Religion).ToList();
                        }
                    }
                    else if (sortOrder == 10)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 11)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Quota).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Quota).ToList();
                        }
                    }
                    else if (sortOrder == 12)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.police).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.police).ToList();
                        }
                    }
                    else if (sortOrder == 13)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 1)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 2)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderByDescending(x => x.Section).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderByDescending(x => x.Section).ToList();
                        }
                    }
                    else if (sortOrder == 3)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 4)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderByDescending(x => x.JobType).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderByDescending(x => x.JobType).ToList();
                        }
                    }
                    else if (sortOrder == 5)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderByDescending(x => x.Domicil).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderByDescending(x => x.Domicil).ToList();
                        }
                    }
                    else if (sortOrder == 6)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderByDescending(x => x.Age).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderByDescending(x => x.Age).ToList();
                        }
                    }
                    else if (sortOrder == 7)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderByDescending(x => x.Gender).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderByDescending(x => x.Gender).ToList();
                        }
                    }
                    else if (sortOrder == 8)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderByDescending(x => x.Disablity).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderByDescending(x => x.Disablity).ToList();
                        }
                    }
                    else if (sortOrder == 9)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderByDescending(x => x.Religion).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderByDescending(x => x.Religion).ToList();
                        }
                    }
                    else if (sortOrder == 10)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 11)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderByDescending(x => x.Quota).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderByDescending(x => x.Quota).ToList();
                        }
                    }
                    else if (sortOrder == 12)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderByDescending(x => x.police).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderByDescending(x => x.police).ToList();
                        }
                    }
                    else if (sortOrder == 13)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderBy(x => x.SortDest).ToList();
                        }
                    }
                }
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota != "0" && poli == "0")
            {
                if (quota == "1")
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.OrderByDescending(x => x.AddtionCharg).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.OrderBy(x => x.Des).ToList();
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderBy(x => x.Des).ToList();
                    }
                }
               // emp = emp.Where(x => x.Quota == quota).ToList();
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli != "0")
            {
                if (poli == "1")
                {
                    if (sortOrder == 0)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 1)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 2)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderByDescending(x => x.Section).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Section).ToList();
                        }
                    }
                    else if (sortOrder == 3)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 4)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderByDescending(x => x.JobType).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.JobType).ToList();
                        }
                    }
                    else if (sortOrder == 5)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderByDescending(x => x.Domicil).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Domicil).ToList();
                        }
                    }
                    else if (sortOrder == 6)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderByDescending(x => x.Age).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Age).ToList();
                        }
                    }
                    else if (sortOrder == 7)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderByDescending(x => x.Gender).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Gender).ToList();
                        }
                    }
                    else if (sortOrder == 8)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderByDescending(x => x.Disablity).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Disablity).ToList();
                        }
                    }
                    else if (sortOrder == 9)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderByDescending(x => x.Religion).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Religion).ToList();
                        }
                    }
                    else if (sortOrder == 10)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 11)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderByDescending(x => x.Quota).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Quota).ToList();
                        }
                    }
                    else if (sortOrder == 12)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderByDescending(x => x.police).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.police).ToList();
                        }
                    }
                    else if (sortOrder == 13)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 1)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 2)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderByDescending(x => x.Section).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderByDescending(x => x.Section).ToList();
                        }
                    }
                    else if (sortOrder == 3)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 4)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderByDescending(x => x.JobType).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderByDescending(x => x.JobType).ToList();
                        }
                    }
                    else if (sortOrder == 5)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderByDescending(x => x.Domicil).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderByDescending(x => x.Domicil).ToList();
                        }
                    }
                    else if (sortOrder == 6)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderByDescending(x => x.Age).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderByDescending(x => x.Age).ToList();
                        }
                    }
                    else if (sortOrder == 7)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderByDescending(x => x.Gender).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderByDescending(x => x.Gender).ToList();
                        }
                    }
                    else if (sortOrder == 8)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderByDescending(x => x.Disablity).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderByDescending(x => x.Disablity).ToList();
                        }
                    }
                    else if (sortOrder == 9)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderByDescending(x => x.Religion).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderByDescending(x => x.Religion).ToList();
                        }
                    }
                    else if (sortOrder == 10)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 11)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderByDescending(x => x.Quota).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderByDescending(x => x.Quota).ToList();
                        }
                    }
                    else if (sortOrder == 12)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderByDescending(x => x.police).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderByDescending(x => x.police).ToList();
                        }
                    }
                    else if (sortOrder == 13)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderBy(x => x.SortDest).ToList();
                        }
                    }
                }
            }

            return emp;
        }
        public List<sp_Profiledbo> getRelieving(int brID, int DesignID, int SecID, int fromsclae, int toscale, int jobtypeID,
           int fromage, int toage, string domicile, string gender, string disablity, string religion, string additional,
           string quota, string poli, int sortOrder, int appoited, string ddldrop, RMSDataContext Data)
        {
            List<sp_GetRelievingEmpContactResult> cont = Data.sp_GetRelievingEmpContact(brID, ddldrop).ToList();
            List<sp_Profiledbo> emp = new List<sp_Profiledbo>();

            tblPlEmpData lasst = Data.tblPlEmpDatas.Where(x => x.LastperDes == appoited).FirstOrDefault();
            int? lastperID;
            if (lasst != null)
            {
                lastperID = Convert.ToInt32(lasst.LastperDes);
            }
            else
            {
                lastperID = null;
            }

            tblPlEmpData appIDD = Data.tblPlEmpDatas.Where(x => x.Appointed == appoited).FirstOrDefault();
            int? appID;
            if (appIDD != null)
            {
                appID = Convert.ToInt32(appIDD.Appointed);
            }
            else
            {
                appID = null;
            }

            foreach (var item in cont)
            {
                var e = new sp_Profiledbo();
                e.ID = item.EmpID;
                e.FullName = item.FullName;
                e.designation = item.DesigID;
                e.Des = item.Designation;
                e.Section = item.DeptID;
                if (item.DOB == null)
                {
                    e.Age = null;
                }
                else
                {
                    e.Age = (DateTime.Now.Year - Convert.ToDateTime(item.DOB).Year) - 1;
                }
                e.br = item.BranchID;
                e.brName = item.BrName;
                e.JobType = item.JobNameID;
                e.Religion = item.Religion;
                e.scale = item.ScaleID;
                e.Domicil = item.Domicil;
                e.Gender = item.Sex.ToString();
                e.police = item.polveri;
                e.JoinDate = item.DOJ;
                e.AddtionCharg = item.AddtionalCharg;
                e.ScaleName = item.ScaleName;
                e.JobeTypeName = item.JobTypeName;
                if (item.Disbality == "0")
                {
                    e.Disablity = "None";
                }
                else
                {
                    e.Disablity = item.Disbality;
                }
                e.Quota = item.Quota;
                e.SectionName = item.Department;
                e.Contactno = item.TelNo;
                e.Mobno = item.MobNo;
                e.BOD = item.DOB;
                e.Email = item.Email;
                e.AppointedID = Convert.ToInt32(item.Appointed);
                e.AppointedName = item.appdesc;
                e.LastPerID = Convert.ToInt32(item.LastperDes);
                e.LastPerName = item.lastpermotion;
                e.SortDest = Convert.ToInt32(item.Desgsort);
                e.coln = item.coluu;
                e.EmpStatus = Convert.ToInt32(item.EmpStatus);
                e.RelievingDate = Convert.ToDateTime(item.ReliDate).ToString("dd-MMM-yyyy");
                emp.Add(e);
            }

            if (appoited != 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (appoited == 1)
                {
                    if (sortOrder == 0)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 1)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 2)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderByDescending(x => x.Section).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Section).ToList();
                        }
                    }
                    else if (sortOrder == 3)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 4)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderByDescending(x => x.JobType).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.JobType).ToList();
                        }
                    }
                    else if (sortOrder == 5)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderByDescending(x => x.Domicil).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Domicil).ToList();
                        }
                    }
                    else if (sortOrder == 6)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderByDescending(x => x.Age).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Age).ToList();
                        }
                    }
                    else if (sortOrder == 7)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderByDescending(x => x.Gender).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Gender).ToList();
                        }
                    }
                    else if (sortOrder == 8)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderByDescending(x => x.Disablity).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Disablity).ToList();
                        }
                    }
                    else if (sortOrder == 9)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderByDescending(x => x.Religion).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Religion).ToList();
                        }
                    }
                    else if (sortOrder == 10)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 11)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderByDescending(x => x.Quota).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Quota).ToList();
                        }
                    }
                    else if (sortOrder == 12)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderByDescending(x => x.police).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.police).ToList();
                        }
                    }
                    else if (sortOrder == 13)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 1)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 2)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderByDescending(x => x.Section).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderByDescending(x => x.Section).ToList();
                        }
                    }
                    else if (sortOrder == 3)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 4)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderByDescending(x => x.JobType).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderByDescending(x => x.JobType).ToList();
                        }
                    }
                    else if (sortOrder == 5)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderByDescending(x => x.Domicil).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderByDescending(x => x.Domicil).ToList();
                        }
                    }
                    else if (sortOrder == 6)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderByDescending(x => x.Age).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderByDescending(x => x.Age).ToList();
                        }
                    }
                    else if (sortOrder == 7)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderByDescending(x => x.Gender).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderByDescending(x => x.Gender).ToList();
                        }
                    }
                    else if (sortOrder == 8)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderByDescending(x => x.Disablity).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderByDescending(x => x.Disablity).ToList();
                        }
                    }
                    else if (sortOrder == 9)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderByDescending(x => x.Religion).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderByDescending(x => x.Religion).ToList();
                        }
                    }
                    else if (sortOrder == 10)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 11)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderByDescending(x => x.Quota).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderByDescending(x => x.Quota).ToList();
                        }
                    }
                    else if (sortOrder == 12)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderByDescending(x => x.police).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderByDescending(x => x.police).ToList();
                        }
                    }
                    else if (sortOrder == 13)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderBy(x => x.SortDest).ToList();
                        }
                    }
                }
            }

            if (appoited == 0 && DesignID != 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (DesignID == 1)
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderBy(x => x.SortDest).ToList();
                    }
                }

            }
            if (appoited == 0 && DesignID == 0 && SecID != 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (SecID == 1)
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderBy(x => x.SortDest).ToList();
                    }
                }
                //emp = emp.Where(x => x.Section == SecID).ToList();
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae != 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (fromsclae == 0)
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.OrderByDescending(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderBy(x => x.SortDest).ToList();
                    }
                }
                //emp =  emp.Where(x => x.scale >= fromsclae).ToList();
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale != 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (toscale == 1)
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderBy(x => x.SortDest).ToList();
                    }
                }
                //emp =  emp.Where(x => x.scale >= toscale).ToList();
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae != 0 && toscale != 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (fromsclae == 1 && toscale == 1)
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderBy(x => x.SortDest).ToList();
                    }
                }
                //emp =  emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).ToList();
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID != 0 && domicile == "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (jobtypeID == 9)
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderBy(x => x.SortDest).ToList();
                    }
                }
                // emp = emp.Where(x => x.JobType == jobtypeID).ToList();
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile != "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (domicile == "1")
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderBy(x => x.SortDest).ToList();
                    }
                }
                //emp = emp.Where(x => x.Domicil == domicile).ToList();
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage != 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (fromage == 1)
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderBy(x => x.SortDest).ToList();
                    }
                }
                //emp = emp.Where(x => x.Age >= fromage).ToList();
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage != 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (toage == 1)
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderBy(x => x.SortDest).ToList();
                    }
                }
                //emp = emp.Where(x => x.Age >= toage).ToList();
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage != 0 && toage != 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (fromage == 1 && toage == 1)
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderBy(x => x.SortDest).ToList();
                    }
                }
                //emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).ToList();
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage == 0 && gender != "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (gender == "1")
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderBy(x => x.SortDest).ToList();
                    }
                }
                //emp = emp.Where(x => x.Gender == gender).ToList();
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity != "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (disablity == "1")
                {
                    if (sortOrder == 0)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 1)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 2)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Section).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Section).ToList();
                        }
                    }
                    else if (sortOrder == 3)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 4)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.JobType).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.JobType).ToList();
                        }
                    }
                    else if (sortOrder == 5)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Domicil).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Domicil).ToList();
                        }
                    }
                    else if (sortOrder == 6)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Age).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Age).ToList();
                        }
                    }
                    else if (sortOrder == 7)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Gender).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Gender).ToList();
                        }
                    }
                    else if (sortOrder == 8)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Disablity).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Disablity).ToList();
                        }
                    }
                    else if (sortOrder == 9)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Religion).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Religion).ToList();
                        }
                    }
                    else if (sortOrder == 10)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 11)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Quota).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Quota).ToList();
                        }
                    }
                    else if (sortOrder == 12)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.police).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.police).ToList();
                        }
                    }
                    else if (sortOrder == 13)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 1)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 2)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderByDescending(x => x.Section).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderByDescending(x => x.Section).ToList();
                        }
                    }
                    else if (sortOrder == 3)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 4)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderByDescending(x => x.JobType).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderByDescending(x => x.JobType).ToList();
                        }
                    }
                    else if (sortOrder == 5)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderByDescending(x => x.Domicil).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderByDescending(x => x.Domicil).ToList();
                        }
                    }
                    else if (sortOrder == 6)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderByDescending(x => x.Age).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderByDescending(x => x.Age).ToList();
                        }
                    }
                    else if (sortOrder == 7)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderByDescending(x => x.Gender).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderByDescending(x => x.Gender).ToList();
                        }
                    }
                    else if (sortOrder == 8)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderByDescending(x => x.Disablity).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderByDescending(x => x.Disablity).ToList();
                        }
                    }
                    else if (sortOrder == 9)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderByDescending(x => x.Religion).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderByDescending(x => x.Religion).ToList();
                        }
                    }
                    else if (sortOrder == 10)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 11)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderByDescending(x => x.Quota).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderByDescending(x => x.Quota).ToList();
                        }
                    }
                    else if (sortOrder == 12)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderByDescending(x => x.police).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderByDescending(x => x.police).ToList();
                        }
                    }
                    else if (sortOrder == 13)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderBy(x => x.SortDest).ToList();
                        }
                    }
                }

            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion != "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (religion == "1")
                {
                    if (sortOrder == 0)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 1)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 2)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderByDescending(x => x.Section).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Section).ToList();
                        }
                    }
                    else if (sortOrder == 3)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 4)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderByDescending(x => x.JobType).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.JobType).ToList();
                        }
                    }
                    else if (sortOrder == 5)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderByDescending(x => x.Domicil).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Domicil).ToList();
                        }
                    }
                    else if (sortOrder == 6)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderByDescending(x => x.Age).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Age).ToList();
                        }
                    }
                    else if (sortOrder == 7)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderByDescending(x => x.Gender).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Gender).ToList();
                        }
                    }
                    else if (sortOrder == 8)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderByDescending(x => x.Disablity).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Disablity).ToList();
                        }
                    }
                    else if (sortOrder == 9)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderByDescending(x => x.Religion).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Religion).ToList();
                        }
                    }
                    else if (sortOrder == 10)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 11)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderByDescending(x => x.Quota).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Quota).ToList();
                        }
                    }
                    else if (sortOrder == 12)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderByDescending(x => x.police).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.police).ToList();
                        }
                    }
                    else if (sortOrder == 13)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderBy(x => x.Des).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.Des).ToList();
                        }
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 1)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 2)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderByDescending(x => x.Section).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderByDescending(x => x.Section).ToList();
                        }
                    }
                    else if (sortOrder == 3)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 4)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderByDescending(x => x.JobType).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderByDescending(x => x.JobType).ToList();
                        }
                    }
                    else if (sortOrder == 5)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderByDescending(x => x.Domicil).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderByDescending(x => x.Domicil).ToList();
                        }
                    }
                    else if (sortOrder == 6)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderByDescending(x => x.Age).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderByDescending(x => x.Age).ToList();
                        }
                    }
                    else if (sortOrder == 7)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderByDescending(x => x.Gender).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderByDescending(x => x.Gender).ToList();
                        }
                    }
                    else if (sortOrder == 8)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderByDescending(x => x.Disablity).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderByDescending(x => x.Disablity).ToList();
                        }
                    }
                    else if (sortOrder == 9)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderByDescending(x => x.Religion).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderByDescending(x => x.Religion).ToList();
                        }
                    }
                    else if (sortOrder == 10)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 11)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderByDescending(x => x.Quota).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderByDescending(x => x.Quota).ToList();
                        }
                    }
                    else if (sortOrder == 12)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderByDescending(x => x.police).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderByDescending(x => x.police).ToList();
                        }
                    }
                    else if (sortOrder == 13)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderBy(x => x.Des).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderBy(x => x.Des).ToList();
                        }
                    }
                }
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional != "0" && quota == "0" && poli == "0")
            {
                if (additional == "1")
                {
                    if (sortOrder == 0)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 1)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 2)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Section).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Section).ToList();
                        }
                    }
                    else if (sortOrder == 3)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 4)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.JobType).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.JobType).ToList();
                        }
                    }
                    else if (sortOrder == 5)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Domicil).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Domicil).ToList();
                        }
                    }
                    else if (sortOrder == 6)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Age).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Age).ToList();
                        }
                    }
                    else if (sortOrder == 7)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Gender).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Gender).ToList();
                        }
                    }
                    else if (sortOrder == 8)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Disablity).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Disablity).ToList();
                        }
                    }
                    else if (sortOrder == 9)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Religion).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Religion).ToList();
                        }
                    }
                    else if (sortOrder == 10)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 11)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Quota).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Quota).ToList();
                        }
                    }
                    else if (sortOrder == 12)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.police).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.police).ToList();
                        }
                    }
                    else if (sortOrder == 13)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 1)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 2)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderByDescending(x => x.Section).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderByDescending(x => x.Section).ToList();
                        }
                    }
                    else if (sortOrder == 3)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 4)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderByDescending(x => x.JobType).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderByDescending(x => x.JobType).ToList();
                        }
                    }
                    else if (sortOrder == 5)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderByDescending(x => x.Domicil).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderByDescending(x => x.Domicil).ToList();
                        }
                    }
                    else if (sortOrder == 6)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderByDescending(x => x.Age).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderByDescending(x => x.Age).ToList();
                        }
                    }
                    else if (sortOrder == 7)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderByDescending(x => x.Gender).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderByDescending(x => x.Gender).ToList();
                        }
                    }
                    else if (sortOrder == 8)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderByDescending(x => x.Disablity).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderByDescending(x => x.Disablity).ToList();
                        }
                    }
                    else if (sortOrder == 9)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderByDescending(x => x.Religion).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderByDescending(x => x.Religion).ToList();
                        }
                    }
                    else if (sortOrder == 10)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 11)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderByDescending(x => x.Quota).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderByDescending(x => x.Quota).ToList();
                        }
                    }
                    else if (sortOrder == 12)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderByDescending(x => x.police).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderByDescending(x => x.police).ToList();
                        }
                    }
                    else if (sortOrder == 13)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderBy(x => x.SortDest).ToList();
                        }
                    }
                }
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota != "0" && poli == "0")
            {
                if (quota == "1")
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.OrderByDescending(x => x.AddtionCharg).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.OrderBy(x => x.Des).ToList();
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderBy(x => x.Des).ToList();
                    }
                }
                // emp = emp.Where(x => x.Quota == quota).ToList();
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli != "0")
            {
                if (poli == "1")
                {
                    if (sortOrder == 0)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 1)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 2)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderByDescending(x => x.Section).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Section).ToList();
                        }
                    }
                    else if (sortOrder == 3)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 4)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderByDescending(x => x.JobType).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.JobType).ToList();
                        }
                    }
                    else if (sortOrder == 5)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderByDescending(x => x.Domicil).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Domicil).ToList();
                        }
                    }
                    else if (sortOrder == 6)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderByDescending(x => x.Age).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Age).ToList();
                        }
                    }
                    else if (sortOrder == 7)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderByDescending(x => x.Gender).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Gender).ToList();
                        }
                    }
                    else if (sortOrder == 8)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderByDescending(x => x.Disablity).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Disablity).ToList();
                        }
                    }
                    else if (sortOrder == 9)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderByDescending(x => x.Religion).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Religion).ToList();
                        }
                    }
                    else if (sortOrder == 10)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 11)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderByDescending(x => x.Quota).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Quota).ToList();
                        }
                    }
                    else if (sortOrder == 12)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderByDescending(x => x.police).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.police).ToList();
                        }
                    }
                    else if (sortOrder == 13)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 1)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 2)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderByDescending(x => x.Section).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderByDescending(x => x.Section).ToList();
                        }
                    }
                    else if (sortOrder == 3)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 4)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderByDescending(x => x.JobType).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderByDescending(x => x.JobType).ToList();
                        }
                    }
                    else if (sortOrder == 5)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderByDescending(x => x.Domicil).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderByDescending(x => x.Domicil).ToList();
                        }
                    }
                    else if (sortOrder == 6)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderByDescending(x => x.Age).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderByDescending(x => x.Age).ToList();
                        }
                    }
                    else if (sortOrder == 7)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderByDescending(x => x.Gender).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderByDescending(x => x.Gender).ToList();
                        }
                    }
                    else if (sortOrder == 8)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderByDescending(x => x.Disablity).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderByDescending(x => x.Disablity).ToList();
                        }
                    }
                    else if (sortOrder == 9)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderByDescending(x => x.Religion).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderByDescending(x => x.Religion).ToList();
                        }
                    }
                    else if (sortOrder == 10)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 11)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderByDescending(x => x.Quota).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderByDescending(x => x.Quota).ToList();
                        }
                    }
                    else if (sortOrder == 12)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderByDescending(x => x.police).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderByDescending(x => x.police).ToList();
                        }
                    }
                    else if (sortOrder == 13)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderBy(x => x.SortDest).ToList();
                        }
                    }
                }
            }

            return emp;
        }

        public List<sp_Profiledbo> getAllEmployeeForReport(int brID, int DesignID, int SecID, int fromsclae, int toscale, int jobtypeID,
           int fromage, int toage, string domicile, string gender, string disablity, string religion, string additional,
           string quota, string poli, int sortOrder, int appoited, string ddldrop, RMSDataContext Data)
        {
            List<Sp_GetAllEmployeesForReportResult> cont = Data.Sp_GetAllEmployeesForReport(brID, ddldrop).ToList();
            List<sp_Profiledbo> emp = new List<sp_Profiledbo>();

            tblPlEmpData lasst = Data.tblPlEmpDatas.Where(x => x.LastperDes == appoited).FirstOrDefault();
            int? lastperID;
            if (lasst != null)
            {
                lastperID = Convert.ToInt32(lasst.LastperDes);
            }
            else
            {
                lastperID = null;
            }

            tblPlEmpData appIDD = Data.tblPlEmpDatas.Where(x => x.Appointed == appoited).FirstOrDefault();
            int? appID;
            if (appIDD != null)
            {
                appID = Convert.ToInt32(appIDD.Appointed);
            }
            else
            {
                appID = null;
            }

            foreach (var item in cont)
            {
                var e = new sp_Profiledbo();
                e.ID = item.EmpID;
                e.FullName = item.FullName;
                e.designation = item.DesigID;
                e.Des = item.Designation;
                e.Section = item.DeptID;
                if (item.DOB == null)
                {
                    e.Age = null;
                }
                else
                {
                    e.Age = (DateTime.Now.Year - Convert.ToDateTime(item.DOB).Year) - 1;
                }
                e.br = item.BranchID;
                e.brName = item.BrName;
                e.JobType = item.JobNameID;
                e.Religion = item.Religion;
                e.scale = item.ScaleID;
                e.Domicil = item.Domicil;
                e.Gender = item.Sex.ToString();
                e.police = item.polveri;
                e.JoinDate = item.DOJ;
                e.AddtionCharg = item.AddtionalCharg;
                e.ScaleName = item.ScaleName;
                e.JobeTypeName = item.JobTypeName;
                if (item.Disbality == "0")
                {
                    e.Disablity = "None";
                }
                else
                {
                    e.Disablity = item.Disbality;
                }
                e.Quota = item.Quota;
                e.SectionName = item.Department;
                e.Contactno = item.TelNo;
                e.Mobno = item.MobNo;
                e.BOD = item.DOB;
                e.Email = item.Email;
                e.AppointedID = Convert.ToInt32(item.Appointed);
                e.AppointedName = item.appdesc;
                e.LastPerID = Convert.ToInt32(item.LastperDes);
                e.LastPerName = item.lastpermotion;
                e.SortDest = Convert.ToInt32(item.Desgsort);
                e.coln = item.coluu;
                e.EmpStatus = Convert.ToInt32(item.EmpStatus);
                if (item.ReliDate == null)
                {
                    e.RelievingDate = null;
                }
                else
                {
                    e.RelievingDate = Convert.ToDateTime(item.ReliDate).ToString("dd-MMM-yyyy");
                }
                emp.Add(e);
            }

            if (appoited != 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (appoited == 1)
                {
                    if (sortOrder == 0)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 1)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 2)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderByDescending(x => x.Section).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Section).ToList();
                        }
                    }
                    else if (sortOrder == 3)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 4)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderByDescending(x => x.JobType).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.JobType).ToList();
                        }
                    }
                    else if (sortOrder == 5)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderByDescending(x => x.Domicil).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Domicil).ToList();
                        }
                    }
                    else if (sortOrder == 6)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderByDescending(x => x.Age).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Age).ToList();
                        }
                    }
                    else if (sortOrder == 7)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderByDescending(x => x.Gender).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Gender).ToList();
                        }
                    }
                    else if (sortOrder == 8)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderByDescending(x => x.Disablity).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Disablity).ToList();
                        }
                    }
                    else if (sortOrder == 9)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderByDescending(x => x.Religion).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Religion).ToList();
                        }
                    }
                    else if (sortOrder == 10)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 11)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderByDescending(x => x.Quota).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Quota).ToList();
                        }
                    }
                    else if (sortOrder == 12)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderByDescending(x => x.police).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.police).ToList();
                        }
                    }
                    else if (sortOrder == 13)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        if (lastperID != 0 || lastperID != 131)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 1)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 2)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderByDescending(x => x.Section).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderByDescending(x => x.Section).ToList();
                        }
                    }
                    else if (sortOrder == 3)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 4)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderByDescending(x => x.JobType).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderByDescending(x => x.JobType).ToList();
                        }
                    }
                    else if (sortOrder == 5)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderByDescending(x => x.Domicil).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderByDescending(x => x.Domicil).ToList();
                        }
                    }
                    else if (sortOrder == 6)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderByDescending(x => x.Age).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderByDescending(x => x.Age).ToList();
                        }
                    }
                    else if (sortOrder == 7)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderByDescending(x => x.Gender).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderByDescending(x => x.Gender).ToList();
                        }
                    }
                    else if (sortOrder == 8)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderByDescending(x => x.Disablity).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderByDescending(x => x.Disablity).ToList();
                        }
                    }
                    else if (sortOrder == 9)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderByDescending(x => x.Religion).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderByDescending(x => x.Religion).ToList();
                        }
                    }
                    else if (sortOrder == 10)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 11)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderByDescending(x => x.Quota).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderByDescending(x => x.Quota).ToList();
                        }
                    }
                    else if (sortOrder == 12)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderByDescending(x => x.police).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderByDescending(x => x.police).ToList();
                        }
                    }
                    else if (sortOrder == 13)
                    {
                        if (lastperID != 0)
                        {
                            emp = emp.Where(x => x.LastPerID == appoited).OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AppointedID == appoited).OrderBy(x => x.SortDest).ToList();
                        }
                    }
                }
            }

            if (appoited == 0 && DesignID != 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (DesignID == 1)
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.Where(x => x.designation == DesignID).OrderBy(x => x.SortDest).ToList();
                    }
                }

            }
            if (appoited == 0 && DesignID == 0 && SecID != 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (SecID == 1)
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.Where(x => x.Section == SecID).OrderBy(x => x.SortDest).ToList();
                    }
                }
                //emp = emp.Where(x => x.Section == SecID).ToList();
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae != 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (fromsclae == 0)
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.OrderByDescending(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae).OrderBy(x => x.SortDest).ToList();
                    }
                }
                //emp =  emp.Where(x => x.scale >= fromsclae).ToList();
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale != 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (toscale == 1)
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.Where(x => x.scale >= toscale).OrderBy(x => x.SortDest).ToList();
                    }
                }
                //emp =  emp.Where(x => x.scale >= toscale).ToList();
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae != 0 && toscale != 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (fromsclae == 1 && toscale == 1)
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).OrderBy(x => x.SortDest).ToList();
                    }
                }
                //emp =  emp.Where(x => x.scale >= fromsclae && x.scale <= toscale).ToList();
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID != 0 && domicile == "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (jobtypeID == 9)
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.Where(x => x.JobType == jobtypeID).OrderBy(x => x.SortDest).ToList();
                    }
                }
                // emp = emp.Where(x => x.JobType == jobtypeID).ToList();
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile != "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (domicile == "1")
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.Where(x => x.Domicil == domicile).OrderBy(x => x.SortDest).ToList();
                    }
                }
                //emp = emp.Where(x => x.Domicil == domicile).ToList();
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage != 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (fromage == 1)
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.Where(x => x.Age >= fromage).OrderBy(x => x.SortDest).ToList();
                    }
                }
                //emp = emp.Where(x => x.Age >= fromage).ToList();
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage != 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (toage == 1)
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.Where(x => x.Age >= toage).OrderBy(x => x.SortDest).ToList();
                    }
                }
                //emp = emp.Where(x => x.Age >= toage).ToList();
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage != 0 && toage != 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (fromage == 1 && toage == 1)
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).OrderBy(x => x.SortDest).ToList();
                    }
                }
                //emp = emp.Where(x => x.Age >= fromage && x.Age <= toage).ToList();
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage == 0 && gender != "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (gender == "1")
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.Where(x => x.Gender == gender).OrderBy(x => x.SortDest).ToList();
                    }
                }
                //emp = emp.Where(x => x.Gender == gender).ToList();
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity != "0" && religion == "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (disablity == "1")
                {
                    if (sortOrder == 0)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 1)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 2)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Section).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Section).ToList();
                        }
                    }
                    else if (sortOrder == 3)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 4)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.JobType).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.JobType).ToList();
                        }
                    }
                    else if (sortOrder == 5)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Domicil).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Domicil).ToList();
                        }
                    }
                    else if (sortOrder == 6)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Age).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Age).ToList();
                        }
                    }
                    else if (sortOrder == 7)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Gender).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Gender).ToList();
                        }
                    }
                    else if (sortOrder == 8)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Disablity).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Disablity).ToList();
                        }
                    }
                    else if (sortOrder == 9)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Religion).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Religion).ToList();
                        }
                    }
                    else if (sortOrder == 10)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 11)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Quota).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Quota).ToList();
                        }
                    }
                    else if (sortOrder == 12)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.police).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.police).ToList();
                        }
                    }
                    else if (sortOrder == 13)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 1)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 2)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderByDescending(x => x.Section).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderByDescending(x => x.Section).ToList();
                        }
                    }
                    else if (sortOrder == 3)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 4)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderByDescending(x => x.JobType).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderByDescending(x => x.JobType).ToList();
                        }
                    }
                    else if (sortOrder == 5)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderByDescending(x => x.Domicil).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderByDescending(x => x.Domicil).ToList();
                        }
                    }
                    else if (sortOrder == 6)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderByDescending(x => x.Age).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderByDescending(x => x.Age).ToList();
                        }
                    }
                    else if (sortOrder == 7)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderByDescending(x => x.Gender).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderByDescending(x => x.Gender).ToList();
                        }
                    }
                    else if (sortOrder == 8)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderByDescending(x => x.Disablity).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderByDescending(x => x.Disablity).ToList();
                        }
                    }
                    else if (sortOrder == 9)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderByDescending(x => x.Religion).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderByDescending(x => x.Religion).ToList();
                        }
                    }
                    else if (sortOrder == 10)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 11)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderByDescending(x => x.Quota).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderByDescending(x => x.Quota).ToList();
                        }
                    }
                    else if (sortOrder == 12)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderByDescending(x => x.police).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderByDescending(x => x.police).ToList();
                        }
                    }
                    else if (sortOrder == 13)
                    {
                        if (disablity == "Yes")
                        {
                            emp = emp.Where(x => x.Disablity != "None").OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Disablity == "None").OrderBy(x => x.SortDest).ToList();
                        }
                    }
                }

            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion != "0" && additional == "0" && quota == "0" && poli == "0")
            {
                if (religion == "1")
                {
                    if (sortOrder == 0)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 1)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 2)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderByDescending(x => x.Section).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Section).ToList();
                        }
                    }
                    else if (sortOrder == 3)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 4)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderByDescending(x => x.JobType).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.JobType).ToList();
                        }
                    }
                    else if (sortOrder == 5)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderByDescending(x => x.Domicil).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Domicil).ToList();
                        }
                    }
                    else if (sortOrder == 6)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderByDescending(x => x.Age).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Age).ToList();
                        }
                    }
                    else if (sortOrder == 7)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderByDescending(x => x.Gender).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Gender).ToList();
                        }
                    }
                    else if (sortOrder == 8)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderByDescending(x => x.Disablity).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Disablity).ToList();
                        }
                    }
                    else if (sortOrder == 9)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderByDescending(x => x.Religion).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Religion).ToList();
                        }
                    }
                    else if (sortOrder == 10)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 11)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderByDescending(x => x.Quota).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Quota).ToList();
                        }
                    }
                    else if (sortOrder == 12)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderByDescending(x => x.police).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.police).ToList();
                        }
                    }
                    else if (sortOrder == 13)
                    {
                        if (religion == "M")
                        {
                            emp = emp.OrderBy(x => x.Des).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.Des).ToList();
                        }
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 1)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 2)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderByDescending(x => x.Section).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderByDescending(x => x.Section).ToList();
                        }
                    }
                    else if (sortOrder == 3)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 4)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderByDescending(x => x.JobType).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderByDescending(x => x.JobType).ToList();
                        }
                    }
                    else if (sortOrder == 5)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderByDescending(x => x.Domicil).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderByDescending(x => x.Domicil).ToList();
                        }
                    }
                    else if (sortOrder == 6)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderByDescending(x => x.Age).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderByDescending(x => x.Age).ToList();
                        }
                    }
                    else if (sortOrder == 7)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderByDescending(x => x.Gender).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderByDescending(x => x.Gender).ToList();
                        }
                    }
                    else if (sortOrder == 8)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderByDescending(x => x.Disablity).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderByDescending(x => x.Disablity).ToList();
                        }
                    }
                    else if (sortOrder == 9)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderByDescending(x => x.Religion).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderByDescending(x => x.Religion).ToList();
                        }
                    }
                    else if (sortOrder == 10)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 11)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderByDescending(x => x.Quota).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderByDescending(x => x.Quota).ToList();
                        }
                    }
                    else if (sortOrder == 12)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderByDescending(x => x.police).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderByDescending(x => x.police).ToList();
                        }
                    }
                    else if (sortOrder == 13)
                    {
                        if (religion == "M")
                        {
                            emp = emp.Where(x => x.Religion == Convert.ToChar("M")).OrderBy(x => x.Des).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.Religion != Convert.ToChar("M") && x.Religion != Convert.ToChar("0")).OrderBy(x => x.Des).ToList();
                        }
                    }
                }
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional != "0" && quota == "0" && poli == "0")
            {
                if (additional == "1")
                {
                    if (sortOrder == 0)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 1)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 2)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Section).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Section).ToList();
                        }
                    }
                    else if (sortOrder == 3)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 4)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.JobType).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.JobType).ToList();
                        }
                    }
                    else if (sortOrder == 5)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Domicil).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Domicil).ToList();
                        }
                    }
                    else if (sortOrder == 6)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Age).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Age).ToList();
                        }
                    }
                    else if (sortOrder == 7)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Gender).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Gender).ToList();
                        }
                    }
                    else if (sortOrder == 8)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Disablity).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Disablity).ToList();
                        }
                    }
                    else if (sortOrder == 9)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Religion).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Religion).ToList();
                        }
                    }
                    else if (sortOrder == 10)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 11)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.Quota).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Quota).ToList();
                        }
                    }
                    else if (sortOrder == 12)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderByDescending(x => x.police).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.police).ToList();
                        }
                    }
                    else if (sortOrder == 13)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 1)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 2)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderByDescending(x => x.Section).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderByDescending(x => x.Section).ToList();
                        }
                    }
                    else if (sortOrder == 3)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 4)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderByDescending(x => x.JobType).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderByDescending(x => x.JobType).ToList();
                        }
                    }
                    else if (sortOrder == 5)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderByDescending(x => x.Domicil).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderByDescending(x => x.Domicil).ToList();
                        }
                    }
                    else if (sortOrder == 6)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderByDescending(x => x.Age).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderByDescending(x => x.Age).ToList();
                        }
                    }
                    else if (sortOrder == 7)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderByDescending(x => x.Gender).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderByDescending(x => x.Gender).ToList();
                        }
                    }
                    else if (sortOrder == 8)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderByDescending(x => x.Disablity).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderByDescending(x => x.Disablity).ToList();
                        }
                    }
                    else if (sortOrder == 9)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderByDescending(x => x.Religion).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderByDescending(x => x.Religion).ToList();
                        }
                    }
                    else if (sortOrder == 10)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 11)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderByDescending(x => x.Quota).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderByDescending(x => x.Quota).ToList();
                        }
                    }
                    else if (sortOrder == 12)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderByDescending(x => x.police).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderByDescending(x => x.police).ToList();
                        }
                    }
                    else if (sortOrder == 13)
                    {
                        if (additional == "Yes")
                        {
                            emp = emp.Where(x => x.AddtionCharg != "N/A" && x.AddtionCharg != "0").OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.AddtionCharg == "N/A" || x.AddtionCharg == "0").OrderBy(x => x.SortDest).ToList();
                        }
                    }
                }
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota != "0" && poli == "0")
            {
                if (quota == "1")
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.OrderByDescending(x => x.AddtionCharg).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.OrderBy(x => x.Des).ToList();
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 1)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 2)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderByDescending(x => x.Section).ToList();
                    }
                    else if (sortOrder == 3)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderByDescending(x => x.scale).ToList();
                    }
                    else if (sortOrder == 4)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderByDescending(x => x.JobType).ToList();
                    }
                    else if (sortOrder == 5)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderByDescending(x => x.Domicil).ToList();
                    }
                    else if (sortOrder == 6)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderByDescending(x => x.Age).ToList();
                    }
                    else if (sortOrder == 7)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderByDescending(x => x.Gender).ToList();
                    }
                    else if (sortOrder == 8)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderByDescending(x => x.Disablity).ToList();
                    }
                    else if (sortOrder == 9)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderByDescending(x => x.Religion).ToList();
                    }
                    else if (sortOrder == 10)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderBy(x => x.SortDest).ToList();
                    }
                    else if (sortOrder == 11)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderByDescending(x => x.Quota).ToList();
                    }
                    else if (sortOrder == 12)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderByDescending(x => x.police).ToList();
                    }
                    else if (sortOrder == 13)
                    {
                        emp = emp.Where(x => x.Quota == quota).OrderBy(x => x.Des).ToList();
                    }
                }
                // emp = emp.Where(x => x.Quota == quota).ToList();
            }
            if (appoited == 0 && DesignID == 0 && SecID == 0 && fromsclae == 0 && toscale == 0 && jobtypeID == 0 && domicile == "0" && fromage == 0 && toage == 0 && gender == "0" &&
            disablity == "0" && religion == "0" && additional == "0" && quota == "0" && poli != "0")
            {
                if (poli == "1")
                {
                    if (sortOrder == 0)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 1)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 2)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderByDescending(x => x.Section).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Section).ToList();
                        }
                    }
                    else if (sortOrder == 3)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 4)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderByDescending(x => x.JobType).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.JobType).ToList();
                        }
                    }
                    else if (sortOrder == 5)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderByDescending(x => x.Domicil).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Domicil).ToList();
                        }
                    }
                    else if (sortOrder == 6)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderByDescending(x => x.Age).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Age).ToList();
                        }
                    }
                    else if (sortOrder == 7)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderByDescending(x => x.Gender).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Gender).ToList();
                        }
                    }
                    else if (sortOrder == 8)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderByDescending(x => x.Disablity).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Disablity).ToList();
                        }
                    }
                    else if (sortOrder == 9)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderByDescending(x => x.Religion).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Religion).ToList();
                        }
                    }
                    else if (sortOrder == 10)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 11)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderByDescending(x => x.Quota).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.Quota).ToList();
                        }
                    }
                    else if (sortOrder == 12)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderByDescending(x => x.police).ToList();
                        }
                        else
                        {
                            emp = emp.OrderByDescending(x => x.police).ToList();
                        }
                    }
                    else if (sortOrder == 13)
                    {
                        if (poli == "true")
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.OrderBy(x => x.SortDest).ToList();
                        }
                    }
                }
                else
                {
                    if (sortOrder == 0)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 1)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 2)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderByDescending(x => x.Section).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderByDescending(x => x.Section).ToList();
                        }
                    }
                    else if (sortOrder == 3)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderByDescending(x => x.scale).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderByDescending(x => x.scale).ToList();
                        }
                    }
                    else if (sortOrder == 4)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderByDescending(x => x.JobType).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderByDescending(x => x.JobType).ToList();
                        }
                    }
                    else if (sortOrder == 5)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderByDescending(x => x.Domicil).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderByDescending(x => x.Domicil).ToList();
                        }
                    }
                    else if (sortOrder == 6)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderByDescending(x => x.Age).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderByDescending(x => x.Age).ToList();
                        }
                    }
                    else if (sortOrder == 7)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderByDescending(x => x.Gender).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderByDescending(x => x.Gender).ToList();
                        }
                    }
                    else if (sortOrder == 8)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderByDescending(x => x.Disablity).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderByDescending(x => x.Disablity).ToList();
                        }
                    }
                    else if (sortOrder == 9)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderByDescending(x => x.Religion).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderByDescending(x => x.Religion).ToList();
                        }
                    }
                    else if (sortOrder == 10)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderBy(x => x.SortDest).ToList();
                        }
                    }
                    else if (sortOrder == 11)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderByDescending(x => x.Quota).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderByDescending(x => x.Quota).ToList();
                        }
                    }
                    else if (sortOrder == 12)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderByDescending(x => x.police).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderByDescending(x => x.police).ToList();
                        }
                    }
                    else if (sortOrder == 13)
                    {
                        if (poli == "true")
                        {
                            emp = emp.Where(x => x.police == true).OrderBy(x => x.SortDest).ToList();
                        }
                        else
                        {
                            emp = emp.Where(x => x.police == false).OrderBy(x => x.SortDest).ToList();
                        }
                    }
                }
            }

            return emp;
        }


        public object GetEmployeeExperience(int empID, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var edu = from emp in Data.tblPlEmpExps
                          join scale in Data.TblEmpScales on emp.Scale equals scale.ScaleID
                          where emp.EmpID == empID
                          select new
                          {
                              scale.ScaleName,
                              emp.Postedas,
                              emp.Sector,
                              emp.EmpID,
                              emp.EmpExpID,
                              emp.OrgName,
                              emp.joinDate,
                              emp.Attachment,
                              emp.leavDate,
                              emp.YOE
                          };
                return edu;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public object GetEmployeeAcr(int empID, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var edu = from emp in Data.tblPlEmpAcrs
                          where emp.EmpID == empID//this one
                                                  //orderby emp.Year descending
                          select new
                          {
                              emp.EmpAcrID,
                              emp.EmpID,
                              emp.tblPlCode.CodeDesc,
                              emp.DateFrom,
                              emp.DateTo,
                              emp.CounterSignOff,
                              emp.CouOffDes,
                              emp.CouOffDate,
                              emp.Remarks,
                              emp.ReportingOfficer,
                              emp.RepOffDes,
                              emp.RepOffDate,
                              emp.Attachment
                          };
                return edu;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public object GetEmployeeEducation(int empID, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var edu = from emp in Data.tblPlEmpEdus
                          join y in Data.tblYears on emp.Year equals y.YearID
                          where emp.EmpID == empID
                          orderby emp.EmpEduID descending//this one
                                                  //  orderby emp.Year descending
                          select new
                          {
                              y.YearName,
                              emp.DegreeTitle,
                              emp.Verified,
                              emp.UniversityBoard,
                              emp.Percente,
                              emp.Year,
                              emp.EmpEduID,
                              emp.EmpID,
                              emp.tblCity.CityName,
                              emp.filePath,
                              emp.Degreetype
                          };
                return edu;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public IQueryable<tblPlLiti> GeLitiTypes(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<tblPlLiti> emps = from emp in Data.tblPlLitis
                                             orderby emp.LitiName
                                                    select emp;
                return emps;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }


        public Object GetAll(string name, string empNo,int brID, bool isSearch, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                Branch branchObj = Data.Branches.Where(x => x.br_id == brID).FirstOrDefault();

                var empsBranchList = from emp in Data.tblPlEmpDatas
                                     orderby emp.sortRef ascending
                                     where emp.FullName.Contains(name)
                                     && emp.EmpCode.Contains(empNo)
                                     && emp.BranchID == brID

                                     select new
                                     {
                                         emp.EmpID,
                                         emp_Id = "EN-" + emp.EmpID,
                                         emp.EmpCode,
                                         emp.FullName,
                                         Gender = emp.Sex.Equals("M") ? "Male" : "Female",
                                         emp.tblCity.CityName,
                                         Dept = emp.tblPlCode.CodeDesc,
                                         Desig = emp.tblPlCode1.CodeDesc,
                                         branch = emp.Branch1.br_nme,
                                         emp.tblPlLocation.LocName

                                     };
                return empsBranchList;



                //if (isSearch == true)
                //{

                //}
                //else
                //{
                //    if (brID == 1)
                //    {
                //        var emps = from emp in Data.tblPlEmpDatas
                //                   orderby emp.FullName
                //                   where emp.EmpStatus == Convert.ToBoolean(status)
                //                   && emp.FullName.Contains(name)
                //                   && emp.EmpCode.Contains(empNo)

                //                   select new
                //                   {
                //                       emp.EmpID,
                //                       emp_Id = "EN-" + emp.EmpID,
                //                       emp.EmpCode,
                //                       emp.FullName,
                //                       Gender = emp.Sex.Equals("M") ? "Male" : "Female",
                //                       emp.tblCity.CityName,
                //                       Dept = emp.tblPlCode.CodeDesc,
                //                       Desig = emp.tblPlCode1.CodeDesc,
                //                       branch = emp.Branch1.br_nme,
                //                       emp.tblPlLocation.LocName

                //                   };
                //        return emps;
                //    }

                //    else
                //    {

                //        var empsBranchList = from emp in Data.tblPlEmpDatas
                //                             orderby emp.FullName
                //                             where emp.EmpStatus == Convert.ToBoolean(status)
                //                             && emp.FullName.Contains(name)
                //                             && emp.EmpCode.Contains(empNo)
                //                             && emp.BranchID == brID

                //                             select new
                //                             {
                //                                 emp.EmpID,
                //                                 emp_Id = "EN-" + emp.EmpID,
                //                                 emp.EmpCode,
                //                                 emp.FullName,
                //                                 Gender = emp.Sex.Equals("M") ? "Male" : "Female",
                //                                 emp.tblCity.CityName,
                //                                 Dept = emp.tblPlCode.CodeDesc,
                //                                 Desig = emp.tblPlCode1.CodeDesc,
                //                                 branch = emp.Branch1.br_nme,
                //                                 emp.tblPlLocation.LocName

                //                             };
                //        if(branchObj.IsDisplay == true)
                //        {
                //            var empSubBrancList = from emp in Data.tblPlEmpDatas
                //                                  orderby emp.FullName
                //                                  where emp.EmpStatus == Convert.ToBoolean(status)
                //                                  && emp.FullName.Contains(name)
                //                                  && emp.EmpCode.Contains(empNo)
                //                                  && emp.Branch1.br_idd == brID
                //                                  && emp.Branch1.br_status == true 
                //                                           //? emp.Branch1.br_idd.Equals(brID) :
                //                                           //emp.Branch1.br_idd.Equals(-1)
                //                                  select new
                //                                  {
                //                                      emp.EmpID,
                //                                      emp_Id = "EN-" + emp.EmpID,
                //                                      emp.EmpCode,
                //                                      emp.FullName,
                //                                      Gender = emp.Sex.Equals("M") ? "Male" : "Female",
                //                                      emp.tblCity.CityName,
                //                                      Dept = emp.tblPlCode.CodeDesc,
                //                                      Desig = emp.tblPlCode1.CodeDesc,
                //                                      branch = emp.Branch1.br_nme,
                //                                      emp.tblPlLocation.LocName

                //                                  };

                //            return empsBranchList.Concat(empSubBrancList).ToList();
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
        public Object GetAllSearch(string empName, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                var emps = from emp in Data.tblPlEmpDatas
                           where (emp.FullName.Contains(empName) || emp.EmpCode.Contains(empName)) && emp.EmpStatus == 1
                           orderby emp.FullName
                           select new
                           {
                               emp.EmpID,
                               emp_Id = "EN-"+emp.EmpID,
                               emp.EmpCode,
                               emp.FullName,
                               //Gender = emp.Sex.Equals("M") ? "Male" : "Female",
                               //emp.CityName,
                               Dept = emp.tblPlCode.CodeDesc,
                               Desig = emp.tblPlCode1.CodeDesc,
                               //   Div = emp.tblPlCode2.CodeDesc,
                               //Reg = emp.tblPlCode3.CodeDesc,
                               //emp.tblPlLocation.LocName

                           };
                return emps;
                //try
                //{
                //    if (Convert.ToInt32(empName) > 0)
                //    {
                //        var emps = from emp in Data.tblPlEmpDatas
                //                   where emp.EmpCode.Equals(empName) && emp.EmpStatus == true
                //                   orderby emp.FullName
                //                   select new
                //                   {
                //                       emp.EmpID,
                //                       emp.EmpCode,
                //                       emp.FullName,
                //                       //Gender = emp.Sex.Equals("M") ? "Male" : "Female",
                //                       //emp.CityName,
                //                       Dept = emp.tblPlCode.CodeDesc,
                //                       Desig = emp.tblPlCode1.CodeDesc,
                //                       //   Div = emp.tblPlCode2.CodeDesc,
                //                       //Reg = emp.tblPlCode3.CodeDesc,
                //                       //emp.tblPlLocation.LocName

                //                   };
                //        return emps;
                //    }
                //    else
                //    {
                //        var emps = from emp in Data.tblPlEmpDatas
                //                   where (emp.FullName.Contains(empName) || emp.EmpCode.Contains(empName)) && emp.EmpStatus == true
                //                   orderby emp.FullName
                //                   select new
                //                   {
                //                       emp.EmpID,
                //                       emp.EmpCode,
                //                       emp.FullName,
                //                       //Gender = emp.Sex.Equals("M") ? "Male" : "Female",
                //                       //emp.CityName,
                //                       Dept = emp.tblPlCode.CodeDesc,
                //                       Desig = emp.tblPlCode1.CodeDesc,
                //                       //   Div = emp.tblPlCode2.CodeDesc,
                //                       //Reg = emp.tblPlCode3.CodeDesc,
                //                       //emp.tblPlLocation.LocName

                //                   };
                //        return emps;
                //    }
                //}
                //catch 
                //{
                //    var emps = from emp in Data.tblPlEmpDatas
                //               where (emp.FullName.Contains(empName) || emp.EmpCode.Contains(empName)) && emp.EmpStatus == true
                //               orderby emp.FullName
                //               select new
                //               {
                //                   emp.EmpID,
                //                   emp.EmpCode,
                //                   emp.FullName,
                //                   //Gender = emp.Sex.Equals("M") ? "Male" : "Female",
                //                   //emp.CityName,
                //                   Dept = emp.tblPlCode.CodeDesc,
                //                   Desig = emp.tblPlCode1.CodeDesc,
                //                   //   Div = emp.tblPlCode2.CodeDesc,
                //                   //Reg = emp.tblPlCode3.CodeDesc,
                //                   //emp.tblPlLocation.LocName

                //               };
                //    return emps;
                //}
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public Object GetAllSearchForResg(string empName, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                try
                {
                    if (Convert.ToInt32(empName) > 0)
                    {
                        var emps = from emp in Data.tblPlEmpDatas
                                   where emp.EmpCode.Equals(empName) && emp.EmpStatus == 1
                                   orderby emp.FullName
                                   select new
                                   {
                                       emp.EmpID,
                                       emp_Id = "EN-"+emp.EmpID,
                                       emp.EmpCode,
                                       emp.FullName,
                                       //Gender = emp.Sex.Equals("M") ? "Male" : "Female",
                                       //emp.CityName,
                                       Dept = emp.tblPlCode.CodeDesc,
                                       Desig = emp.tblPlCode1.CodeDesc,
                                       //   Div = emp.tblPlCode2.CodeDesc,
                                       //Reg = emp.tblPlCode3.CodeDesc,
                                       //emp.tblPlLocation.LocName

                                   };
                        return emps;
                    }
                    else
                    {
                        var emps = from emp in Data.tblPlEmpDatas
                                   where emp.FullName.Contains(empName) && emp.EmpStatus == 1
                                   orderby emp.FullName
                                   select new
                                   {
                                       emp.EmpID,
                                       emp_Id = "EN-" + emp.EmpID,
                                       emp.EmpCode,
                                       emp.FullName,
                                       //Gender = emp.Sex.Equals("M") ? "Male" : "Female",
                                       //emp.CityName,
                                       Dept = emp.tblPlCode.CodeDesc,
                                       Desig = emp.tblPlCode1.CodeDesc,
                                       //   Div = emp.tblPlCode2.CodeDesc,
                                       //Reg = emp.tblPlCode3.CodeDesc,
                                       //emp.tblPlLocation.LocName

                                   };
                        return emps;
                    }
                }
                catch
                {
                    var emps = from emp in Data.tblPlEmpDatas
                               where emp.FullName.Contains(empName) && emp.EmpStatus == 1
                               orderby emp.FullName
                               select new
                               {
                                   emp.EmpID,
                                   emp_Id = "EN-" + emp.EmpID,
                                   emp.EmpCode,
                                   emp.FullName,
                                   //Gender = emp.Sex.Equals("M") ? "Male" : "Female",
                                   //emp.CityName,
                                   Dept = emp.tblPlCode.CodeDesc,
                                   Desig = emp.tblPlCode1.CodeDesc,
                                   //   Div = emp.tblPlCode2.CodeDesc,
                                   //Reg = emp.tblPlCode3.CodeDesc,
                                   //emp.tblPlLocation.LocName

                               };
                    return emps;
                }
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public Object GetSearch(string empName, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                try
                {
                    if (Convert.ToInt32(empName) > 0)
                    {
                        var emps = from emp in Data.tblPlEmpDatas
                                   where emp.EmpCode.Equals(empName) && emp.EmpStatus == 1
                                   orderby emp.FullName
                                   select new
                                   {
                                       emp.EmpID,
                                       emp.EmpCode,
                                       emp.FullName,
                                       //Gender = emp.Sex.Equals("M") ? "Male" : "Female",
                                       //emp.CityName,
                                       Dept = emp.tblPlCode.CodeDesc,
                                       Desig = emp.tblPlCode1.CodeDesc,
                                       //   Div = emp.tblPlCode2.CodeDesc,
                                       //Reg = emp.tblPlCode3.CodeDesc,
                                       //emp.tblPlLocation.LocName

                                   };
                        return emps;
                    }
                    else
                    {
                        var emps = from emp in Data.tblPlEmpDatas
                                   where emp.FullName.Contains(empName) && emp.EmpStatus == 1
                                   orderby emp.FullName
                                   select new
                                   {
                                       emp.EmpID,
                                       emp.EmpCode,
                                       emp.FullName,
                                       //Gender = emp.Sex.Equals("M") ? "Male" : "Female",
                                       //emp.CityName,
                                       Dept = emp.tblPlCode.CodeDesc,
                                       Desig = emp.tblPlCode1.CodeDesc,
                                       //   Div = emp.tblPlCode2.CodeDesc,
                                       //Reg = emp.tblPlCode3.CodeDesc,
                                       //emp.tblPlLocation.LocName

                                   };
                        return emps;
                    }
                }
                catch
                {
                    var emps = from emp in Data.tblPlEmpDatas
                               where emp.EmpStatus == 1
                               orderby emp.FullName
                               select new
                               {
                                   emp.EmpID,
                                   emp.EmpCode,
                                   emp.FullName,
                                   //Gender = emp.Sex.Equals("M") ? "Male" : "Female",
                                   //emp.CityName,
                                   Dept = emp.tblPlCode.CodeDesc,
                                   Desig = emp.tblPlCode1.CodeDesc,
                                   //   Div = emp.tblPlCode2.CodeDesc,
                                   //Reg = emp.tblPlCode3.CodeDesc,
                                   //emp.tblPlLocation.LocName

                               };
                    return emps;
                }
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
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

        public tblPlEmpData GetByID(int brid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                tblPlEmpData emps = Data.tblPlEmpDatas.Single(p => p.EmpID == brid);

                return emps;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public string GetGt_CtByCtrl(string ctrlGlCd, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                return Data.Glmf_Codes.Where(gl => gl.gl_cd == ctrlGlCd).Single().gt_cd;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public Preference GetPreferenceByID(RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                return Data.Preferences.First();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public tblPlCode GetPlCodeByID(int deptid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                return Data.tblPlCodes.Where(cd => cd.CodeID == deptid).Single();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public bool IsEmpAcCodeExists(string ctrlcd, int empid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                Data.Glmf_Codes.Where(cd => cd.gl_cd == ctrlcd + Convert.ToString(empid).PadLeft(4, '0')).Single();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Glmf_Code GetGlmf_CodeBYGl_Cd(string ctrlcd, int empid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                return Data.Glmf_Codes.Where(cd => cd.gl_cd == ctrlcd + Convert.ToString(empid).PadLeft(4, '0')).Single();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }


        public bool SaveGlmf_Code(Glmf_Code cd,EntitySet<Glmf> ettyglmf, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                Data.Glmf_Codes.InsertOnSubmit(cd);
                Data.Glmfs.InsertAllOnSubmit(ettyglmf);
                Data.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }

        }

        public bool UpdateGlmf_Code(Glmf_Code cd, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                Data.SubmitChanges();
                return true;
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
        
        
        public bool FindDuplicate(string code,RMSDataContext Data)
        {
            IList<tblPlEmpData> emprecords = (from t in Data.tblPlEmpDatas
                                              where t.EmpCode.Equals(code)
                                              select t).ToList();
            if (emprecords.Count > 0)
            {
                return true;
            }
            return false;
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
        public tblPlRsgTran GetResgByID(int brid,RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                tblPlRsgTran emps = Data.tblPlRsgTrans.Single(p => p.EmpId == brid);

                return emps;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public tblPlRsgTran GetResg(int brid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                tblPlRsgTran emps = Data.tblPlRsgTrans.Single(p => p.EmpId == brid);

                return emps;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }


        //EMPLOYEE PROFILE////////////////////////////////////////////////////////////////


        public tblPlEmpData GetByEmpCode(string empCode, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            tblPlEmpData emp = new tblPlEmpData();
            try
            {
                emp = Data.tblPlEmpDatas.Single(p => p.EmpCode == empCode);
                return emp;
            }
            catch
            {
            }
            return null;

        }


        public List<spTurnOverResult> GetTurnOverRpt(DateTime fromDate, DateTime toDate, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                return Data.spTurnOver(fromDate, toDate).ToList();
            }
            catch
            {
            }
            return null;
        }
    }
}