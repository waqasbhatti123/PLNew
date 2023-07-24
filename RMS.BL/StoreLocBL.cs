using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMS.BL
{
    public class StoreLocBL
    {
        public StoreLocBL()
        { }

        public int GetLocID(int brid, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                return Convert.ToInt32(Data.tblStock_Locs.Max(stk => stk.LocId).ToString()) + 1;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<spStoreLocationResult> GetAllStockLoc(int brid, string loccd, string locnm, char strcat, char sts, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                return Data.spStoreLocation(brid, loccd, locnm, Convert.ToString(strcat), Convert.ToString(sts)).Where(loc=> !loc.LocCategory.Equals("R")).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public tblStock_Loc GetStockByID(int brid, int id, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                return Data.tblStock_Locs.Where(stk => stk.br_id.Equals(brid) && stk.LocId.Equals(id)).SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertStockLoc(tblStock_Loc stkLoc, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.tblStock_Locs.InsertOnSubmit(stkLoc);
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateStockLoc(tblStock_Loc stkLoc, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
