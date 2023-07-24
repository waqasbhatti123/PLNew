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
    public class PlCodeBL
    {

        public PlCodeBL()
        {       }

        public IQueryable<tblPlCode> GetAll(tblPlCode cto, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<tblPlCode> otblPlCode = from cty in Data.tblPlCodes
                                 
                                 where cty.CompID == cto.CompID && cty.CodeTypeID == cto.CodeTypeID
                                 && cty.Enabled == true
                                 orderby cty.sort
                                 select cty;
                                                                         
                                     
                return otblPlCode;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public Object GetAll4Grid(byte codeTypeID, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var otblPlCode = from cty in Data.tblPlCodes
                                 orderby cty.sort
                                 where cty.CodeTypeID == codeTypeID
                                 && cty.Enabled == true

                                 select new
                                 {
                                     cty.CodeDesc,
                                     cty.CodeID,
                                     cty.Enabled
                                 };


                return otblPlCode;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
       
        

        public tblPlCode GetByID(int codeid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                tblPlCode allows = Data.tblPlCodes.Single(p =>
                    p.CodeID == codeid );

                return allows;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }


        public Glmf_Code GetGlCdByID(string glcd, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                return Data.Glmf_Codes.Where(gl => gl.gl_cd == glcd).Single();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }




        public bool ISAlreadyExist(tblPlCode cto, RMSDataContext Data)
        {
            try
            {
                bool isalready = false;
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<tblPlCode> cty = from ct in Data.tblPlCodes
                                          where ct.CodeID != cto.CodeID
                                          && (ct.CompID == cto.CompID) 
                                          && ct.CodeTypeID == cto.CodeTypeID 
                                          && ct.CodeDesc.Equals(cto.CodeDesc)
                                          select ct;

                if (cty != null & cty.Count<tblPlCode>() > 0)
                    isalready = true;
                return isalready;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }



        //public void DeleteByID(int ctyid, RMSDataContext Data)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = RMSDB.GetOject(); }
        //        tblPlCode cty = Data.tblPlCodes.Single(p => p.CodeID == ctyid);
        //        Data.tblPlCodes.DeleteOnSubmit(cty);
        //        Data.SubmitChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }

        //}

        public void Insert(tblPlCode code, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                int maxid = 0;
                try
                {
                    maxid = (from t in Data.tblPlCodes
                             select t.CodeID).Max();
                }
                catch { maxid = 0; }
                code.CodeID = Convert.ToInt16(maxid + 1); 
                Data.tblPlCodes.InsertOnSubmit(code);
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public void Update(tblPlCode cty, RMSDataContext Data)
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