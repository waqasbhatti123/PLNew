using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMS.BL
{
    public class COA_BL
    {
        public COA_BL()
        { }

        public List<Gl_Type> GetAll(RMSDataContext Data)
        {

            if (Data == null) { Data = RMSDB.GetOject(); }
            return Data.Gl_Types.ToList();
        
        }

        public List<spCOAResult> GetReport(RMSDataContext Data, char searchType)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            List<spCOAResult> spCOAList = Data.spCOA().ToList();
            if (searchType == 'X')
            {
                return spCOAList;
            }
            else
            {
                spCOAList = spCOAList.Where(t => t.gt_cd.Equals(searchType.ToString())).ToList();
                return spCOAList;


            }
            
        
        }

        public List<spGLAccountInfoResult> GetGLAccountInfoReport(RMSDataContext Data, char searchType)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            if (searchType == 'X')
            {
                return Data.spGLAccountInfo().ToList();
            }
            else
            {
                return Data.spGLAccountInfo().Where(t => t.gt_cd.Equals(searchType)).ToList();

            }


        }


    }
}
