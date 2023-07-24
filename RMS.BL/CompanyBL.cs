using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.Common;
//using System.Transactions;


namespace RMS.BL
{
    public class CompanyBL
    {

        public CompanyBL()
        {

        }

        public object GetAll(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var oCompany = from cmp in Data.tblCompanies
                               orderby cmp.CompName 
                               select cmp;
                return oCompany;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }

        }

 
        public bool ISAlreadyExist(tblCompany cto, RMSDataContext Data)
        {
            try
            {
                bool isalready = false;
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<tblCompany> cty = from ct in Data.tblCompanies 
                                          where ct.CompID != cto.CompID
                                          select ct;

                if (cty != null & cty.Count<tblCompany>() > 0)
                    isalready = true;
                return isalready;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public string GetFirstCompName(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                string oComp = Data.tblCompanies.First().CompName;

                return oComp;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public int GetInvDueDays(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                return Convert.ToInt32( Data.tblCompanies.First().InvDueDays);

               
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public tblCompany GetByID(int cmpid, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                tblCompany oComp = Data.tblCompanies.Single(p => p.CompID == cmpid);

                return oComp;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }


        public void DeleteByID(int cmpid, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                tblCompany cty = Data.tblCompanies.Single(p => p.CompID == cmpid);
                Data.tblCompanies.DeleteOnSubmit(cty);
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }

        }

        public void Insert(tblCompany cty, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.tblCompanies.InsertOnSubmit(cty);
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public void Update(tblCompany cty, RMSDataContext Data)
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