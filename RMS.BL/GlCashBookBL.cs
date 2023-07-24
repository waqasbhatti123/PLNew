using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.Linq;


namespace RMS.BL
{
    public class GlCashBookBL
    {

        public GlCashBookBL()
        {
        }

    //    public List<spGL_Cash_BookResult> GetGLCashBookResults(int brId, DateTime dtFrom, DateTime dtTo, string codefrom, string codeto, RMSDataContext Data)
    //    {
    //        if (Data == null) { Data = RMSDB.GetOject(); }

    //        try
    //        {
    //            List<int> lst = new List<int>();
    //            FIN_PERD fin = (from a in Data.FIN_PERDs
    //                            where a.Start_Date <= dtFrom && a.End_Date >= dtFrom
    //                            select a).Single();
    //            string finYear = fin.Gl_Year.ToString();

    //            return Data.spGL_Cash_Book(brId, Convert.ToDecimal(2014), dtFrom, dtTo, codefrom, codeto).ToList();
    //        }
    //        catch (Exception ex)
    //        {
    //            RMSDB.closeConn(Data); 
    //            throw ex;
    //        }
    //    }
    }
}
