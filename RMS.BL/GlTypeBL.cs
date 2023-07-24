using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace RMS.BL
{
   public class GlTypeBL
    {
       public GlTypeBL()
        { 
        
        }

        public List<Gl_Type> GetAll(RMSDataContext Data)
        {

            if (Data == null) { Data = RMSDB.GetOject(); }
            return Data.Gl_Types.ToList();
        
        }

        public Gl_Type GetByID(Char code, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Gl_Type costobj = Data.Gl_Types.Single(p => p.gt_cd.Equals(code));

                return costobj;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public bool ISAlreadyExist(Gl_Type cto, RMSDataContext Data)
        {
            try
            {
                bool isalready = false;
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<Gl_Type> cty = from ct in Data.Gl_Types
                                              where ct.gt_cd != cto.gt_cd && (ct.gt_dsc == cto.gt_dsc)
                                              select ct;

                if (cty != null & cty.Count<Gl_Type>() > 0)
                    isalready = true;
                return isalready;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }

        }

        public void Insert(Gl_Type cty, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.Gl_Types.InsertOnSubmit(cty);
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public void Update(Gl_Type cty, RMSDataContext Data)
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
