using System;
using System.Collections.Generic;
using System.Linq;

namespace RMS.BL
{
    public class UserBL
    {
        //RMSDataContext Data;

        public UserBL()
        {

        }

        public Object GetAll(string userName, int groupId, int compId, int cityId, int branchId, bool isSearch,  RMSDataContext Data)
        {
            try
            {
                if (Data == null)
                {
                    Data = RMSDB.GetOject();
                }


                var user = Data.spFilterUsers(userName, groupId, compId, cityId, branchId);
                return user.ToList();

                //Branch branchObj = Data.Branches.Where(x => x.br_id == branchId).FirstOrDefault();
                //if(isSearch == true)
                //{
                    
                //}
                //else
                //{
                //    if(branchId == 1)
                //    {
                //        var alluser = Data.spFilterUsers(userName, groupId, compId, cityId, 0);
                //        return alluser.ToList();
                //    }
                //    else 
                //    {
                //        var brnachuser = Data.spFilterUsers(userName, groupId, compId, cityId, branchId);
                        
                //        if(branchObj.IsDisplay == true)
                //        {
                //            List<Branch> SubBranchList = Data.Branches.Where(x => x.br_idd == branchId && x.br_status == true).ToList();
                //            foreach(Branch subBranchObj in SubBranchList)
                //            {
                //                int subBranchID = subBranchObj.br_id;
                //                var subBranchuser = Data.spFilterUsers(userName, groupId, compId, cityId, subBranchID);
                //                if(subBranchuser != null  && subBranchuser.ReturnValue != null)
                //                {
                //                    return brnachuser.Concat(subBranchuser).ToList();
                //                }
                //                else
                //                {
                //                    return brnachuser.ToList();
                //                }
                                

                //            }

                           

                //        }
                //        else
                //        {
                //            return brnachuser.ToList();
                //        }
                //    }
                //}

                return null;
                
                

            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }

        }

        public IQueryable<tblAppGroup> GetAllGroupsCombo(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<tblAppGroup> groups = from grp in Data.tblAppGroups
                                                 orderby grp.GroupName
                                                 select grp;
                return groups;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public IQueryable<tblCity> GetAllCityCombo(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<tblCity> otblCity = from cty in Data.tblCities
                                               where cty.Enabled == true
                                               orderby cty.CityName
                                               select cty;
                return otblCity;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public IQueryable<tblCompany> GetAllCompCombo(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<tblCompany> otblCity = from cty in Data.tblCompanies
                                               where cty.Enabled == true
                                               orderby cty.CompName
                                               select cty;
                return otblCity;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public IQueryable<Branch> GetAllBranchCombo(int compid, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<Branch> otblCity = from cty in Data.Branches
                                                  where cty.CompID == compid && cty.br_status == true
                                                  orderby cty.br_nme
                                                  select cty;
                return otblCity;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        //public IQueryable<UserRole> GetAllRolesCombo(RMSDataContext Data)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = RMSDB.GetOject(); }
        //        IQueryable<UserRole> usrRoles = from usrRole
        //                                        in Data.UserRoles
        //                                        where usrRole.Enabled == true
        //                                        orderby usrRole.RoleDesc
        //                                        select usrRole;
        //        return usrRoles;
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }
        //}
        //public Object GetAllUserRoles(int empId, RMSDataContext Data)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = RMSDB.GetOject(); }
        //        var eRegs = from er in Data.tblAppUserRoles
        //                    orderby er.GroupID
        //                    where er.UserID == empId
        //                    select new
        //                    {
        //                        er.GroupID,
        //                        er.tblAppGroup.Name,
        //                        er.RoleID,
        //                        er.UserRole.RoleDesc,
        //                        er.Enabled
        //                    };
        //        return eRegs;
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }

        //}
        //public Object GetAdditionalRoles(int empId, RMSDataContext Data)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = RMSDB.GetOject(); }
        //        var eRegs = from er in Data.tblAppUserRoles
        //                    orderby er.GroupID
        //                    where er.UserID == empId && er.Enabled == true
        //                    select new
        //                    {
        //                        er.GroupID,
        //                        er.tblAppGroup.Name,
        //                        er.RoleID,
        //                        er.UserRole.RoleDesc,
        //                        er.Enabled,
        //                        er.Region.RegionName,
        //                        er.Segment.SegmentDesc
        //                    };
        //        return eRegs;
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }

        //}
        //public Object GetDefaulRole(int empId, RMSDataContext Data)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = RMSDB.GetOject(); }
        //        var defRole = from appUsr in Data.tblAppUsers
        //                    where appUsr.UserID == empId
        //                    select new
        //                    {
        //                        appUsr.GroupID,
        //                        appUsr.tblAppGroup.Name,
        //                        appUsr.RoleID,
        //                        appUsr.UserRole.RoleDesc,
        //                        appUsr.Employee.Region.RegionName,
        //                        appUsr.Employee.Segment.SegmentDesc
        //                    };
        //        return defRole;
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }

        //}
        //public void DeleteAllUserPrivilages(int userid)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = KSBSalesDB.GetOject(); }
        //        var usr = from p in Data.UserPrivilages
        //                  where p.UserId == userid
        //                  select p;

        //        Data.UserPrivilages.DeleteAllOnSubmit(usr);
        //    }
        //    catch (Exception ex)
        //    {
        //        KSBSalesDB.SetNull();
        //        throw ex;
        //    }


        //}


        //public IQueryable<Employee> GetAllEmpCombo(string regId, RMSDataContext Data)
        //{
        //    // if regId == NULL then all emps will be in result
        //    try
        //    {
        //        if (Data == null) { Data = RMSDB.GetOject(); }
        //        IQueryable<Employee> empz = from emp in Data.Employees
        //                                       //join emp in Data.Employees on usr.EmpID equals emp.EmpID
        //                                       //join cty in Data.Cities on emp.CityID equals cty.CityID
        //                                       orderby emp.FirstName
        //                                       where (regId != null ? emp.RegionID == Convert.ToInt32(regId) : emp.RegionID == emp.RegionID)
        //                                        && (emp.EmpID != 1 || emp.EmpID!=2)
        //                                        select emp;

        //        return empz;
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }

        //}

        public Object GetAllUsers4PrivilageCombo(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var emps = from emp in Data.tblAppUsers
                           where emp.Enabled == true
                           //orderby emp.Employee.FirstName
                           select new
                           {
                               emp.UserName,
                               emp.UserID
                           };
                return emps;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        //public Object GetDesignation()
        //{
        //    try
        //    {
        //        if (Data == null) { Data = KSBSalesDB.GetOject(); }
        //        var user = (from usr in Data.UserRoles
        //                    orderby usr.RoleDesc
        //                    select usr.RoleDesc).Distinct();
        //        return user;
        //    }
        //    catch (Exception ex)
        //    {
        //        KSBSalesDB.SetNull();
        //        throw ex;
        //    }

        //}

        public bool ISAlreadyExist(tblAppUser cnto, RMSDataContext Data)
        {
            try
            {
                bool isalready = false;
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<tblAppUser> user = from usr in Data.tblAppUsers
                                              where usr.LoginID == cnto.LoginID && usr.UserID != cnto.UserID
                                              select usr;

                if (user != null & user.Count<tblAppUser>() > 0)
                    isalready = true;
                return isalready;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public bool ISAlreadyExist(string login, RMSDataContext Data)
        {
            try
            {
                bool isalready = false;
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<tblAppUser> user = from usr in Data.tblAppUsers
                                              where usr.LoginID == login
                                              select usr;

                if (user != null & user.Count<tblAppUser>() > 0)
                    isalready = true;
                return isalready;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }


        public int IsValid(string Email, string PassWord, ref Int32? userid, ref RMSDataContext Data)
        {
            try
            {
                int? outValid = 0;

                if (Data == null) { Data = RMSDB.GetOject(); }

                var str = Data.SP_Users_Validat(Email, PassWord, ref outValid, ref userid);
                return outValid.Value;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        //public tblAppUsers_ResetPassResult ResetPassword(string Email)
        //{
        //    if (Data == null) { Data = KSBSalesDB.GetOject(); }
        //    tblAppUsers_ResetPassResult password = new tblAppUsers_ResetPassResult();
        //    try
        //    {

        //        password = Data.tblAppUsers_ResetPass(Email).Single();
        //    }
        //    catch (Exception ex)
        //    {
        //        KSBSalesDB.SetNull();
        //        //throw ex;
        //    }
        //    return password;
        //}

        //public void tblAppUserDisable(string Email)
        //{
        //    if (Data == null) { Data = KSBSalesDB.GetOject(); }
        //    int result = 0;
        //    try
        //    {
        //        result = Data.tblAppUsers_Disable(Email);
        //    }
        //    catch (Exception ex)
        //    {
        //        KSBSalesDB.SetNull();
        //        throw ex;
        //    }
        //}

        public void GettblAppUserInfo(string Email, ref string PassWord, ref Boolean? userExist, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                //var str = Data.GettblAppUser_forEmail(Email, ref PassWord, ref userExist);
                IQueryable<tblAppUser> user = from usr in Data.tblAppUsers
                                              where usr.LoginID == Email
                                              select usr;
                if (user.Count() > 0)
                {
                    PassWord = user.First().Password;
                    userExist = true;
                }
                else
                {
                    PassWord = "";
                    userExist = false;
                }
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                PassWord = "";
                userExist = false;
                //throw ex;
            }
        }
        //public void DeleteBranchs(int userid)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = KSBSalesDB.GetOject(); }
        //        IQueryable<tblAppUserBranch> userbranchs = from s in Data.tblAppUserBranches
        //                                             where s.tblAppUserId == userid
        //                                             select s;

        //        //user.Password = Data.DecryptString(user.Password);
        //        Data.tblAppUserBranches.DeleteAllOnSubmit(userbranchs);
        //        Data.SubmitChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        KSBSalesDB.SetNull();
        //        throw ex;
        //    }
        //}

        public tblAppUser GetByID(int userid, RMSDataContext Data)
        {

            if (Data == null) { Data = RMSDB.GetOject(); }
            tblAppUser user = null;// new tblAppUser();
            //Users_GetResult usr = new Users_GetResult();
            try
            {
                //if (Data == null) { Data = KSBSalesDB.GetOject(); }
                
                    user = Data.tblAppUsers.Single(p => p.UserID == userid);
            
               
                //usr = Data.Users_Get(userid).Single();
                //user.Password = Data.DecryptString(user.Password);
                //user.Password = usr.password;
                return user;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
            //return user;
        }


        public tblAppUser GetByIDUser(int userid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            tblAppUser user = null;// new tblAppUser();
            //Users_GetResult usr = new Users_GetResult();
            try
            {
                //if (Data == null) { Data = KSBSalesDB.GetOject(); }
                if (user.UserID == userid)
                {
                    user = Data.tblAppUsers.Single(p => p.UserID == userid);
                }
               


                //usr = Data.Users_Get(userid).Single();
                //user.Password = Data.DecryptString(user.Password);
                //user.Password = usr.password;
                return user;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }



        public tblAppUser GetByIDDetailed(int userid, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                tblAppUser userGrid = Data.tblAppUsers.Single(p => p.UserID == userid);
                return userGrid;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public tblCompany GetByIDComp(string compid, RMSDataContext Data)
        {
            
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                tblCompany userGrid = Data.tblCompanies.Single(p => p.CompID.ToString().Equals(compid));
                return userGrid;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public Branch GetByIDBranch(string brid, RMSDataContext Data)
        {

            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Branch brnch = Data.Branches.Single(p => p.br_id.ToString().Equals(brid));
                return brnch;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        //public tblAppUserBranch GetBranchByID(int userid, int branchid)
        //{
        //    tblAppUserBranch userBr = new tblAppUserBranch();
        //    try
        //    {
        //        if (Data == null) { Data = KSBSalesDB.GetOject(); }
        //        userBr = Data.tblAppUserBranches.Single(p => p.tblAppUserId == userid && p.br_id == branchid);

        //    }
        //    catch (Exception ex)
        //    {
        //        KSBSalesDB.SetNull();
        //        if (userBr == null)
        //            userBr = new tblAppUserBranch();
        //        //throw ex;
        //    }
        //    return userBr;

        //}

        public string GetWelcome(int userid, ref string location, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<Welcome> userObj =
                    from usr in Data.tblAppUsers
                    //join emp in Data.Employees on usr.EmpID equals emp.EmpID
                    //join reg in Data.Regions on emp.RegionID equals reg.RegionID
                    where usr.UserID == userid
                    select new Welcome
                    {
                        //Name = emp.FirstName + " || "+ emp.MiddleName + " || " + emp.LastName ,
                        Name = usr.UserName,
                        Location = usr.tblCity.CityName
                    };
                location = userObj.First().Location;
                return userObj.First().Name;
            }
            catch
            {
                return "relogin4301paspioh234892214@#$!$!$@$!@$!@$!@fsdg";
            }
        }

        public void DeleteByID(int userid, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                tblAppUser usr = Data.tblAppUsers.Single(p => p.UserID == userid);
                //Employee emp = GetEmpByID(Convert.ToInt32(usr.EmpID),Data);
                Data.tblAppUsers.DeleteOnSubmit(usr);
                //Data.Employees.DeleteOnSubmit(emp);
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public void Insert(tblAppUser usr, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.tblAppUsers.InsertOnSubmit(usr);

                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        //public void InsertEmp(Employee emp, RMSDataContext Data)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = RMSDB.GetOject(); }
        //        Data.Employees.InsertOnSubmit(emp);

        //        Data.SubmitChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }
        //}


        public void Update(tblAppUser usr, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                // Data.groups.InsertOnSubmit(usr);
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            RMSDB.SetNull();
        }
        public void Update(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.SubmitChanges();
            }
            catch { }
        }

        //public void SaveBranch(tblAppUserBranch usrBR)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = KSBSalesDB.GetOject(); }
        //        //Data.tblAppUserBranches.InsertOnSubmit(usrBR);
        //        Data.SubmitChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        KSBSalesDB.SetNull();
        //        throw ex;
        //    }

        //}





        //public IQueryable<spPreferences_GetResult> Preference()
        //{
        //    try
        //    {
        //        if (Data == null) { Data = KSBSalesDB.GetOject(); }
        //        IQueryable<spPreferences_GetResult> pref =
        //          Data.spPreferences_Get().AsQueryable();

        //        return pref;
        //    }
        //    catch (Exception ex)
        //    {
        //        KSBSalesDB.SetNull();
        //        throw ex;
        //    }
        //}

        //public void tblAppUsers_update_lang(int userid, string lang)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = KSBSalesDB.GetOject(); }
        //        Data.tblAppUsers_Update_lang(userid, lang);
        //        Data.SubmitChanges();
        //    }
        //    catch
        //    {
        //    }
        //}

        //public DateTime MyDate()
        //{
        //    if (Data == null) { Data = KSBSalesDB.GetOject(); }
        //    return Data.mydate().Value;
        //}

    }

    public class Welcome
    {
        string _Name;
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        string _Location;
        public string Location
        {
            get { return _Location; }
            set { _Location = value; }
        }

    }

}