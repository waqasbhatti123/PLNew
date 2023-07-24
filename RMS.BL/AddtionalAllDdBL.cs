using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.BL
{
    public class AddtionalAllDdBL
    {
        public AddtionalAllDdBL()
        {

        }

        public object GetAll(int brID, bool isSearch, RMSDataContext Data)
        {
            try
            {
                if (Data == null)
                {
                    Data = RMSDB.GetOject();
                }


                IQueryable branchList = from all in Data.tblPlAddtionalAllDds
                                        where all.tblPlEmpData.BranchID == brID
                                        orderby all.AddID
                                        select new
                                        {
                                            all.AddID,
                                            all.Name,
                                            all.size,
                                            all.fromd,
                                            all.tod,
                                            all.isActive,
                                            all.isValue,
                                            EmpName = all.tblPlEmpData.FullName,
                                            salName = all.SalaryContentType.Name
                                        };
                return branchList;

                //Branch branchObj = Data.Branches.Where(x => x.br_id == brID).FirstOrDefault();
                //if(isSearch == true)
                //{
                   
                //}

                //if(brID == 1)
                //{
                //    IQueryable EmpEdu = from all in Data.tblPlAddtionalAllDds
                //                        orderby all.AddID
                //                        select new
                //                        {
                //                            all.AddID,
                //                            all.Name,
                //                            all.size,
                //                            all.fromd,
                //                            all.tod,
                //                            all.isActive,
                //                            all.isValue,
                //                            EmpName = all.tblPlEmpData.FullName,
                //                            salName = all.SalaryContentType.Name
                //                        };
                //    return EmpEdu;
                //}
                //else
                //{
                //    var branchList = from all in Data.tblPlAddtionalAllDds
                //                            orderby all.AddID
                //                            where all.tblPlEmpData.BranchID == brID
                //                            select new
                //                            {
                //                                all.AddID,
                //                                all.Name,
                //                                all.size,
                //                                all.fromd,
                //                                all.tod,
                //                                all.isActive,
                //                                all.isValue,
                //                                EmpName = all.tblPlEmpData.FullName,
                //                                salName = all.SalaryContentType.Name
                //                            };
                //    if (branchObj.IsDisplay == true)
                //    {
                //        var branchSubList = from all in Data.tblPlAddtionalAllDds
                //                                orderby all.AddID
                //                                where all.tblPlEmpData.Branch1.br_idd == brID
                //                                && all.tblPlEmpData.Branch1.br_status == true
                //                               select new
                //                                {
                //                                    all.AddID,
                //                                    all.Name,
                //                                    all.size,
                //                                    all.fromd,
                //                                    all.tod,
                //                                    all.isActive,
                //                                    all.isValue,
                //                                    EmpName = all.tblPlEmpData.FullName,
                //                                    salName = all.SalaryContentType.Name
                //                                };


                //        return branchList.Concat(branchSubList).ToList();
                //    }
                //    else
                //    {
                //        return branchList.ToList();
                //    }
                   
                    
                //}
                
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public void Insert(tblPlAddtionalAllDd all, RMSDataContext Data)
        {
            //ye BL banai Emp Edu k lye 
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.tblPlAddtionalAllDds.InsertOnSubmit(all);
                Data.SubmitChanges();

            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public tblPlAddtionalAllDd GetByID(int addall, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                tblPlAddtionalAllDd EmpEdu = Data.tblPlAddtionalAllDds.Single(p => p.AddID.Equals(addall));

                return EmpEdu;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public void Update(tblPlAddtionalAllDd id, RMSDataContext Data)
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
