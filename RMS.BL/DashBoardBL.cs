using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMS.BL
{
    public class DashBoardBL
    {

        //public List<tblAppMenu> GetParentMenu(RMSDataContext Data)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = RMSDB.GetOject(); }
        //        List<tblAppMenu> menuz = (from mnu in Data.tblAppMenus 
        //                                where mnu.AmIDParent == null
        //                                select mnu).ToList();

        //        return menuz;
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }
        //}

        //public List<tblAppMenu> GetChildMenu(int parentId, RMSDataContext Data)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = RMSDB.GetOject(); }
        //        List<tblAppMenu> menuz = (from mnu in Data.tblAppMenus
        //                                  where mnu.AmIDParent == parentId
        //                                  select mnu).ToList();

        //        return menuz;
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }
        //}
        //public string GetTeamLeadRegions(int empId)
        //{
        //    if (Data == null) { Data = KSBSalesDB.GetOject(); }
        //    IQueryable<EmpRegion> regIds = from empReg in Data.EmpRegions
        //                 where empReg.EmpID == empId
        //                 select empReg;

        //    string regidz = "";

        //    foreach (EmpRegion er in regIds)
        //    {
        //        regidz += ","+er.RegionID;
        //    }

        //    return regidz;
        //}





    }
}
