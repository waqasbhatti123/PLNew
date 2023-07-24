using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.BL
{
    public class ExcessSurrenderBL
    {
        
        public ExcessSurrenderBL()
        {
           
        }

        public IList<sp_ExcessSurrenderResult> GetExcessReport(int brId, int glyear, int ESID)
        {
            using (RMSDataContext db = new RMSDataContext())
            {
                IList<sp_ExcessSurrenderResult> exc = db.sp_ExcessSurrender(brId, glyear,ESID).ToList();
                return exc;
            }
        }
    }
}
