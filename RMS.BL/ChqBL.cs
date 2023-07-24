using System;
using System.Linq;
using System.Collections.Generic;
//using System.Transactions;


namespace RMS.BL
{
    public class ChqBL
    {
        public ChqBL()
        {}

        public tblAppUser GetUserByLoginId(string logid, RMSDataContext Data)
        {
            return Data.tblAppUsers.Where(u => u.LoginID == logid).Single();
        }

        public List<spCheqDetailsResult> GetChqDetails(int type, string bank, string vchNo, string chq, DateTime fromdt, DateTime todt, int brnachID, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                if (type == 0)//Not cleared
                    return Data.spCheqDetails(bank, chq, vchNo, "", fromdt, todt, brnachID).Where(c=> c.Chq_clr_dt == null).ToList();
                else//cleared
                    return Data.spCheqDetails(bank, chq, vchNo, "", fromdt, todt, brnachID).Where(c => c.Chq_clr_dt != null).ToList();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public spCheqDetailsResult GetChqDetailById(int vrid, int branchId, RMSDataContext Data)
        {     try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                return Data.spCheqDetails("", "", "", "", Convert.ToDateTime("01-Jan-1900"), Convert.ToDateTime("01-Jan-2099"), branchId).Where(c => c.vrid == vrid).Single();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public Glmf_Data_chq GetChkDataById(int vrid, RMSDataContext Data)
        {
            return Data.Glmf_Data_chqs.Where(chq => chq.vrid == vrid).Single();
        }

        public string Update(Glmf_Data_chq chq, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                Data.SubmitChanges();
                return "ok";
            }
            catch (Exception ex)
            {
                //RMSDB.SetNull();
                //throw ex;
                return ex.Message;
            }
        }
    }
}