using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Linq;

namespace RMS.BL
{
    public class GLYearBL
    {
        public GLYearBL()
        { }

        
        
        public decimal GetCurrentGLYear( RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
               decimal year = (from t in Data.FIN_PERDs
                        where t.Cur_Year.Equals("CUR")
                        select t.Gl_Year).Single();
               return year;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }

        public int GetPendingVouchers(decimal glyr, char status, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                if (status == '0')
                {
                    return Data.Glmf_Datas.Where(gl => gl.Gl_Year == glyr).Count();
                }
                else
                {
                    return Data.Glmf_Datas.Where(gl => gl.Gl_Year == glyr && gl.vr_apr == Convert.ToString(status)).Count();
                }
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }

        public string UpdateGLYear(int brid, string glType, decimal glyr, string updby, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                var objCUR = Data.FIN_PERDs.Where(p => p.Cur_Year == "CUR").Single();
                var objNXT = Data.FIN_PERDs.Where(p => p.Cur_Year == "NXT").Single();
                
                if (glType == "0")//Move to next gl year
                {
                    int vCount = GetPendingVouchers(glyr, 'P', Data);

                    if (vCount > 0)
                    {
                        return "There are pending vouchers in current GL Year, Please approve them to continue.";
                     
                    }
                    objCUR.Cur_Year = "PRV";
                    //Data.SubmitChanges();
                    
                    objNXT.Cur_Year = "CUR";
                    //Data.SubmitChanges();
                    
                    FIN_PERD perd = new FIN_PERD();
                    perd.Gl_Year = glyr+2;
                    perd.Start_Date = Convert.ToDateTime("01-Jul-" + (glyr + 1));
                    perd.End_Date = Convert.ToDateTime("30-Jun-" + (glyr + 2));
                    perd.Cur_Year = "NXT";
                    Data.FIN_PERDs.InsertOnSubmit(perd);
                    //Data.SubmitChanges();
                }
                else if (glType == "1")//Move to previous gl year
                {

                    int count = GetPendingVouchers(objNXT.Gl_Year, '0', Data);
                    if (count > 0)
                    {
                        return "Cannot move to previous gl year as vouchers exist in next financial year";
                    }


                    decimal yr = Data.FIN_PERDs.Where(p=> p.Cur_Year == "PRV").Max(p => p.Gl_Year);
                    FIN_PERD highestPrevRec = Data.FIN_PERDs.Where(fp => fp.Gl_Year == yr).Single();
                    highestPrevRec.Cur_Year = "CUR";
                    //Data.SubmitChanges();

                    objCUR.Cur_Year = "NXT";
                    //Data.SubmitChanges();

                    Data.FIN_PERDs.DeleteOnSubmit(objNXT);
                    //Data.SubmitChanges();
                }
                else{}
                if (glType == "0")
                {
                    Data.spYearEndGlmf(brid, Convert.ToInt32(objNXT.Gl_Year), updby);
                }
                else if (glType == "1")
                {

                }
                else{}
                Data.SubmitChanges();
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message; ;
            }
        }

    }
}
