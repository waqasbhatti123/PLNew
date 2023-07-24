using System;
using System.Linq;

namespace RMS.BL
{
    public class PrivilageBL
    {

        public object GetPrivilagesByGroup(int groupID, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var query = from app in Data.tblAppMenus

                            join p in Data.tblAppPrivilages on app.AmID equals p.AmID into gp
                            from x in gp.DefaultIfEmpty()
                            where x.GroupID == groupID && x.tblAppMenu.Enabled == true //&& app.AmName != ""
                            orderby app.ModuleID,  app.AmID.ToString()
                            select new { app.ModuleID, AmID = app.AmID, AmIDParent = app.AmIDParent, app.AmName, CanEdit = x.CanEdit == null ? false : x.CanEdit, CanAdd = x.CanAdd == null ? false : x.CanAdd, CanDel = (x.CanDel != null) ? x.CanDel : false, CanPrint = (x.CanPrint != null) ? x.CanPrint : false, Enabled = x.Enabled == null ? false : x.Enabled };

                if (query.Count() == 0)
                    query = from app in Data.tblAppMenus

                            join p in Data.tblAppPrivilages on app.AmID equals p.AmID into gp
                            from x in gp.DefaultIfEmpty()
                            where x.GroupID == 1 && x.tblAppMenu.Enabled == true //&& app.AmName != ""
                            orderby app.ModuleID, app.AmID.ToString()
                            select new { app.ModuleID, AmID = app.AmID, AmIDParent = app.AmIDParent, app.AmName, CanEdit = false, CanAdd = false, CanDel = false, CanPrint = false, Enabled = false };
                return query;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public tblAppPrivilage GetByPageID(int pageID, int groupid, RMSDataContext Data)
        {
            tblAppPrivilage privilage = null;
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                privilage = Data.tblAppPrivilages.Single(p => p.GroupID == groupid && p.AmID == pageID);
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
            return privilage;
        }

        public bool HasAccess(int pageID, int groupid, string url, RMSDataContext Data)
        {

            if (Data == null) { Data = RMSDB.GetOject(); }
            bool isaccess = false;
            try
            {
                string amUrl = (from p in Data.tblAppPrivilages
                                where p.GroupID == groupid && p.AmID == pageID && p.Enabled == true
                                select p).Single().tblAppMenu.AmURL;

                amUrl = amUrl.Substring(2, amUrl.Length - 2);
                
                if (url.ToLower().Contains(amUrl.ToLower()))
                {
                    isaccess = true;
                }
                else
                {
                    isaccess = false;
                }
            }
            catch
            {
                return false;
            }
            return isaccess;
        }
    }
}