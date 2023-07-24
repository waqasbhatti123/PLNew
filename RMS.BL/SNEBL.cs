using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.BL
{
    public class SNEBL
    {
        public SNEBL()
        {

        }

        public IQueryable<SNEBudgetResult> GetSneData(decimal glYear, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                IQueryable<SNEBudgetResult> pckgs =
                    Data.SNEBudget(glYear).AsQueryable();

                return pckgs;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
    }
}
