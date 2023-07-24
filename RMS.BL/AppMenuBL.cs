using System;
using System.Linq;
using System.Collections.Generic;

using System.Text;
//using System.Transactions;

namespace RMS.BL
{
    public class AppMenuBL
    {
        public AppMenuBL()
        {
        }

        public IQueryable GetNestedMenu(int userId, int parentId, bool isAddRole, int groupId, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            if (isAddRole)
            {
                return from mnu in Data.tblAppMenus
                       join pv in Data.tblAppPrivilages on mnu.AmID equals pv.AmID
                       //join gr in Data.tblAppGroups on pv.GroupId equals gr.GroupId
                       //join usr in Data.tblAppUsers on gr.GroupId equals usr.GroupID
                       orderby mnu.MenuOrdering ascending
                       where pv.GroupID == groupId && mnu.Enabled == true && pv.Enabled == true && mnu.AmIDParent == parentId
                       select new { mnu.AmID, mnu.AmName, AmURL = mnu.AmURL + "?PID=" + mnu.AmID };
            }
            else
            {
                var query = from mnu in Data.tblAppMenus
                            join pv in Data.tblAppPrivilages on mnu.AmID equals pv.AmID
                            join gr in Data.tblAppGroups on pv.GroupID equals gr.GroupID
                            join usr in Data.tblAppUsers on gr.GroupID equals usr.GroupID
                            orderby mnu.MenuOrdering ascending
                            where usr.UserID == userId && mnu.Enabled == true && pv.Enabled == true && mnu.AmIDParent == parentId
                            select new { mnu.AmID, mnu.AmName, AmURL = mnu.AmURL + "?PID=" + mnu.AmID };
                return query;
            }
        }


        public int GetModuleID(int userId, int pid, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                return (from m in Data.tblAppMenus
                        where m.AmID == pid
                        select m).Single().ModuleID.Value;

            }
            catch
            { }
            return 0;
        }
        public Object GetParentMenu(int userId, int pid, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                int moduleId = (from m in Data.tblAppMenus
                                where m.AmID == pid
                                select m).Single().ModuleID.Value;

                if (pid == 1 || pid == 11 || pid == 12 || pid == 13)
                {
                    var menuz = from mnu in Data.tblAppMenus
                                join pv in Data.tblAppPrivilages on mnu.AmID equals pv.AmID
                                join gr in Data.tblAppGroups on pv.GroupID equals gr.GroupID
                                join usr in Data.tblAppUsers on gr.GroupID equals usr.GroupID
                                orderby mnu.AmID ascending
                                where usr.UserID == userId && mnu.Enabled == true && pv.Enabled == true && mnu.AmIDParent == null && mnu.ModuleID == moduleId
                                select new { mnu.AmID, mnu.AmName, mnu.AmURL };
                    //List<tblAppMenu> menuz = (from mnu in Data.tblAppMenus
                    //                          where mnu.AmIDParent == null && mnu.Enabled == true
                    //                          select mnu).ToList();

                    return menuz;
                }
                else
                {
                    var menuz = from mnu in Data.tblAppMenus
                                join pv in Data.tblAppPrivilages on mnu.AmID equals pv.AmID
                                join gr in Data.tblAppGroups on pv.GroupID equals gr.GroupID
                                join usr in Data.tblAppUsers on gr.GroupID equals usr.GroupID
                                orderby mnu.AmID ascending
                                where usr.UserID == userId && mnu.Enabled == true && pv.Enabled == true && mnu.AmIDParent == null && mnu.ModuleID == moduleId
                                select new { mnu.AmID, mnu.AmName, mnu.AmURL };
                    //List<tblAppMenu> menuz = (from mnu in Data.tblAppMenus
                    //                          where mnu.AmIDParent == null && mnu.Enabled == true
                    //                          select mnu).ToList();

                    return menuz;
                }
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public IQueryable<tblAppMenu> GetByID(int userid, RMSDataContext Data)
        {
            //"select amid MenuID, " + sMenuText + " Text , " + sMenuDesc + " Description, URL NavigateUrl ,  amId_Parent ParentID from tblAppMenu";
            // var query = from person in people
            //    join pet in pets on person equals pet.Owner
            //    select new { OwnerName = person.FirstName, PetName = pet.Name };

            //var query = from person in people
            //   join pet in pets on person equals pet.Owner into gj
            //   from subpet in gj.DefaultIfEmpty()
            //   select new { person.FirstName, PetName = (subpet == null ? String.Empty : subpet.Name) };


            //SELECT m.* 
            //FROM [appmenu] m
            //LEFT JOIN [privilage] p ON m.amid=p.amid
            //LEFT JOIN [user] u ON u.groupid=p.groupid and u.userid=1
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<tblAppMenu> menu = from mnu in Data.tblAppMenus
                                              join pv in Data.tblAppPrivilages on mnu.AmID equals pv.AmID
                                              join gr in Data.tblAppGroups on pv.GroupID equals gr.GroupID
                                              join usr in Data.tblAppUsers on gr.GroupID equals usr.GroupID
                                              where usr.UserID == userid && mnu.Enabled == true && pv.Enabled == true
                                              select mnu;

                return menu;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }

        }

        public string GetModuleName(int moduleid, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                return Data.tblAppModules.Where(md => md.ModuleID.Equals(moduleid)).SingleOrDefault().ModuleDesc;
            }
            catch
            { };
            return "";
        }
        public List<spMenuResult> GetMenu(int moduleid, int grpId, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                return Data.spMenu(moduleid, grpId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
