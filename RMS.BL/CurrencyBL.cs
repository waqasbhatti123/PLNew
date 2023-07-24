using System;
using System.Collections.Generic;
using System.Linq;

namespace RMS.BL
{
    public class CurrencyBL
    {
        public CurrencyBL()
        { }

        public List<tblCurrency> GetAllCurrencies(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                return Data.tblCurrencies.ToList();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
