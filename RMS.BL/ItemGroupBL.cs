using System;
using System.Linq;
using System.Collections.Generic;
//using System.Transactions;


namespace RMS.BL
{
    public class ItemGroupBL
    {
        public ItemGroupBL()
        {

        }
        //public bool IsAlreadyExist(tblItem_Group itmGroup, RMSDataContext Data)
        //{
        //    if (Data == null) { Data = RMSDB.GetOject(); }
        //    try
        //    {
        //        var itmG = ( from a in Data.tblItem_Groups 
        //                      where a.itm_grp_id == itmGroup.itm_grp_id select a);
        //    }
        //}

        public void InsertOnSubmit(tblItem_Group itm, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                Data.tblItem_Groups.InsertOnSubmit(itm);
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public object BindGrid(RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                var obj = (from a in Data.tblItem_Groups select a);
                return obj.OrderBy(o => o.itm_grp_desc).ToList();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public tblItem_Group GetByID(int itmID, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                tblItem_Group itms = Data.tblItem_Groups.Single(p => p.itm_grp_id == itmID);

                return itms;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public void UpdateItmGroup(RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
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