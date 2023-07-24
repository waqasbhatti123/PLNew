using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.BL
{
    public class PucarAccoutsNewReportBL
    {
        public PucarAccoutsNewReportBL()
        {

        }


        public IQueryable<SP_Heads_CCDetailResult> SpecificHeadsDivsionalReport(decimal glYear,  string acctHead, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                IQueryable<SP_Heads_CCDetailResult> pckgs =
                    Data.SP_Heads_CCDetail(glYear, acctHead).AsQueryable();

                return pckgs;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }





        public IQueryable<SP_AnnualGrantResult> AnnualGrantDivsionalReport(decimal glYear, int duration, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                IQueryable<SP_AnnualGrantResult> pckgs =
                    Data.SP_AnnualGrant(glYear, duration).AsQueryable();

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
