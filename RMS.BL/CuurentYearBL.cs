using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.BL
{
    public class CuurentYearBL
    {

        public string fromFinalYearDate(RMSDataContext Data)
        {
            

            string dateFromStr =  Data.FIN_PERDs.Where(x => x.Cur_Year == "CUR").FirstOrDefault().Start_Date.ToString();
            return Convert.ToDateTime(dateFromStr).ToString("dd-MMM-yyyy");
        }


        public string toFinalYearDate(RMSDataContext Data)
        {


            string dateToStr = Data.FIN_PERDs.Where(x => x.Cur_Year == "CUR").FirstOrDefault().End_Date.ToString();
            return Convert.ToDateTime(dateToStr).ToString("dd-MMM-yyyy");
        }

        public FIN_PERD GetFin(RMSDataContext Data)
        {
            var fin = Data.FIN_PERDs.Where(x => x.Cur_Year == "CUR").FirstOrDefault();
            return fin;
        }
    }
}
