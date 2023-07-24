using System;

namespace RMS.BL
{
    public partial class Common
    {
      
        public static DateTime MyDate(RMSDataContext Data)
        {
            DateTime dt;
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                dt = Data.funcGetCurrentDate().Value;
            }
            catch
            {
                dt = DateTime.Now;
                //throw ex;
            }
            return dt;
        }
    }
}
