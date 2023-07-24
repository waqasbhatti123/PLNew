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
    public class MailBL
    {

        public MailBL()
        { }

        public IQueryable<tblPlCode> GetAll(tblPlCode cto, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<tblPlCode> otblPlCode = from cty in Data.tblPlCodes
                                                   orderby cty.CodeDesc
                                                   where cty.CompID == cto.CompID && cty.CodeTypeID == cto.CodeTypeID
                                                   select cty;


                return otblPlCode;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public Object GetAll4Grid( RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var otblMailSetup = "";
                    //from cty in Data.tblMailSetups
                //                 orderby cty.MailFrom
                //                 //where cty.CodeTypeID == codeTypeID
                //                 select new
                //                 {
                //                     cty.MailFrom,
                //                     cty.MailHead,
                //                     cty.MailAddress,
                //                     cty.Status,
                //                     cty.MailID
                //                 };


                return otblMailSetup;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }


        //public tblMailSetup GetByID(int mid, RMSDataContext Data)
        //{
        //    if (Data == null) { Data = RMSDB.GetOject(); }
        //    try
        //    {
        //        tblMailSetup allows = Data.tblMailSetups.Single(p =>
        //            p.MailID == mid);

        //        return allows;
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }
        //}


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

        public bool getMailStatus(RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                //List<tblMailSetup> mailData = (from mail in Data.tblMailSetups select mail).ToList();
                //foreach (tblMailSetup chkStatus in mailData)
                //{
                //    bool status = (bool)chkStatus.Status;
                //    if (status)
                //    {
                //        chkStatus.Status = false;
                //        Data.SubmitChanges();
                //   }
               // }

            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
            return true;
        }




        //public bool ISAlreadyExist(tblMailSetup cto, RMSDataContext Data)
        //{
        //    try
        //    {
        //        bool isalready = false;
        //        if (Data == null) { Data = RMSDB.GetOject(); }
        //        IQueryable<tblMailSetup> cty = from ct in Data.tblMailSetups
        //                                  where ct.MailID != cto.MailID
        //                                  && (ct.MailFrom == cto.MailFrom) 
        //                                  && ct. == cto.CodeTypeID 
        //                                  && ct.CodeDesc.Equals(cto.CodeDesc)
        //                                  select ct;

        //        if (cty != null & cty.Count<tblPlCode>() > 0)
        //            isalready = true;
        //        return isalready;
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }
        //}



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

        //public void Insert(tblMailSetup code, RMSDataContext Data)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = RMSDB.GetOject(); }
        //        //int maxid = 0;
        //        //try
        //        //{
        //        //    maxid = (from t in Data.tblPlCodes
        //        //             select t.CodeID).Max();
        //        //}
        //        //catch { maxid = 0; }
        //        //code.CodeID = Convert.ToInt16(maxid + 1);
        //        Data.tblMailSetups.InsertOnSubmit(code);
        //        Data.SubmitChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }
        //}

        //public void Update(tblMailSetup cty, RMSDataContext Data)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = RMSDB.GetOject(); }
        //        Data.SubmitChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }
        //}

    }
}