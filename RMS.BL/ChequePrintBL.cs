using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMS.BL
{
    public class ChequePrintBL
    {
        public ChequePrintBL()
        {
        
        }

        public List<Preference> GetAll(RMSDataContext Data)
        {

            return Data.Preferences.ToList();
        }

        //public tblChequePrint GetByID(int id, RMSDataContext Data)
        //{

        //    //return Data.Preferences.Where(t=>t.p_id.Equals(id)).Single();
        //    return Data.tblChequePrints.FirstOrDefault();
        //}
        //public List<Sp_Cheque_DetailResult> ChequeDetail(RMSDataContext Data)
        //{
        //    List<Sp_Cheque_DetailResult> data= Data.Sp_Cheque_Detail().ToList();
        //    return data;
        //}
        
        //public void Insert(tblChequePrint cp, RMSDataContext Data)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = RMSDB.GetOject(); }
        //        Data.tblChequePrints.InsertOnSubmit(cp);
        //        Data.SubmitChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }
        //}

        //public void Update(tblChequePrint cp, RMSDataContext Data)
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
        public void UpdateInventCode(Preference cty, RMSDataContext Data)
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

        public List<string> GetControlAccount(string gl_cd, RMSDataContext Data)
        {

           return (from u in Data.Glmf_Codes
                  where (u.gl_cd.StartsWith(gl_cd) || u.gl_dsc.StartsWith(gl_cd)) && u.ct_id.Equals("C")
                  orderby u.gl_cd
                   select u.gl_cd + ' ' + u.gl_dsc).ToList();
                  //orderby u.gl_cd // optional but it'll look nicer
                  
                  
            
        
        }
        public object GetDetailAccount1(string gl_cd, RMSDataContext Data)
        {

            return (from u in Data.Glmf_Codes
                    where (u.gl_cd.Contains(gl_cd) || u.gl_dsc.Contains(gl_cd)) && u.ct_id.Equals("D")
                    orderby u.gl_cd
                    select new
                    {
                        u.gl_cd,
                        u.gl_dsc

                    }).Take(5).ToList();
        }
        public List<string> GetDetailAccount(string gl_cd, RMSDataContext Data)
        {

            return (from u in Data.Glmf_Codes
                    where (u.gl_cd.StartsWith(gl_cd) || u.gl_dsc.StartsWith(gl_cd)) && u.ct_id.Equals("D")
                    orderby u.gl_cd
                    select u.gl_cd + ' ' + u.gl_dsc).Take(5).ToList();
            //orderby u.gl_cd // optional but it'll look nicer




        }

        public List<string> GetCostCenter(string gl_cd, RMSDataContext Data)
        {

            return (from u in Data.Cost_Centers
                    where (u.cc_cd.StartsWith(gl_cd) || u.cc_nme.StartsWith(gl_cd))
                    orderby u.cc_cd
                    select u.cc_cd + ' ' + u.cc_nme).ToList();
            //orderby u.gl_cd // optional but it'll look nicer




        }

        public void insertInventCode(Preference cty, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.Preferences.InsertOnSubmit(cty);
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            } 
        }
        public int GetMaxID(RMSDataContext Data)
        {
            List<int> id = new List<int>();
            if (Data == null) { Data = RMSDB.GetOject(); }

            try 
            {
                //var obj =(from a in Data.tblChequePrints select a.Id);
                //if (obj.Count() > 0)
                //    return Convert.ToInt32(obj.Max()) + 1;
                //else
                   return 1;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            } 
        }
    }

  }

