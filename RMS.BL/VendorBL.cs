using System;
using System.Linq;
using System.Collections.Generic;
//using System.Transactions;


namespace RMS.BL
{
    public class VendorBL
    {
        public VendorBL()
        {

        }

        public List<Glmf_Code> GetVendor(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                string cntrl_Vendor = Data.Preferences.First().ctrl_Vndr;
                var lst = from l in Data.Glmf_Codes
                          where l.ct_id == "D" && l.cnt_gl_cd.StartsWith(cntrl_Vendor)
                          orderby l.gl_dsc
                          select l;
                return lst.ToList();
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }
    }
}