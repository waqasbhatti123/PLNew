using System;
using System.Linq;
//using System.Transactions;


namespace RMS.BL
{
    public class BankBL
    {
        public BankBL()
        {

        }

        public IQueryable GetAll(int id,RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable banks = from bnk in Data.tblBanks
                                   where bnk.brID == id
                                   orderby bnk.BankCode
                                   select new
                                   {

                                       bnk.BankCode,
                                       bnk.BankName,
                                       bnk.BankABv,
                                       GlAccCd = Data.Glmf_Codes.Where(cd=> cd.gl_cd.Equals(bnk.GlAccCd)).SingleOrDefault().gl_cd
                                                 +" - "+
                                                 Data.Glmf_Codes.Where(cd => cd.gl_cd.Equals(bnk.GlAccCd)).SingleOrDefault().gl_dsc
                                   };
                return banks;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public IQueryable<tblAppGroup> GetAllGroupsCombo(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<tblAppGroup> banks = from bnk in Data.tblAppGroups
                                                 orderby bnk.GroupName
                                                 select bnk;
                return banks;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public bool ISAlreadyExist(string bnkCode, string bankNme, RMSDataContext Data)
        {

            bool isalready = false;
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                IQueryable<tblBank> banks = from bnk in Data.tblBanks
                                            where bnk.BankCode == bnkCode || bnk.BankName == bankNme
                                            select bnk;

                if (banks != null & banks.Count<tblBank>() > 0)
                    isalready = true;
                return isalready;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public bool ISAlreadyExistUpd(string oldBnkCode, string bankNme, RMSDataContext Data)
        {

            bool isalready = false;
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                IQueryable<tblBank> banks = from bnk in Data.tblBanks
                                            where bnk.BankCode != oldBnkCode && (bnk.BankName == bankNme)
                                            select bnk;

                if (banks != null & banks.Count<tblBank>() > 0)
                    isalready = true;
                return isalready;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public tblBank GetByID(string bnkid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                tblBank banks = Data.tblBanks.Single(p => p.BankCode.Equals(bnkid));

                return banks;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public void Insert(tblBank bnk, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.tblBanks.InsertOnSubmit(bnk);
                Data.SubmitChanges();

            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public void Update(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
    }
}