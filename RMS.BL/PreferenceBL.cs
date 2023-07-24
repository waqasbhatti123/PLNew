using RMS.BL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMS.BL
{
    public class PreferenceBL
    {
        
        public PreferenceBL()
        {
        
        }

        public List<Preference> GetAll(RMSDataContext Data)
        {

            return Data.Preferences.ToList();
        }

        public Preference GetByID(int id, RMSDataContext Data)
        {

            //return Data.Preferences.Where(t=>t.p_id.Equals(id)).Single();
            return Data.Preferences.FirstOrDefault();
        }
        
        public void Insert(Preference cty, RMSDataContext Data)
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

        public void Update(Preference cty, RMSDataContext Data)
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
        public object GetDetailAccount1(string gl_cd, int brId, RMSDataContext Data)
     {



            return (from u in Data.Glmf_Codes
                    join v in Data.glmf_ven_cus_dets
                    on u.gl_cd equals v.gl_cd into sub
                    from v in sub.DefaultIfEmpty()
                    where (u.gl_cd.Contains(gl_cd) || u.gl_dsc.Contains(gl_cd)) && u.ct_id.Equals("D")
                    && brId == (v != null && v.br_id != null ? v.br_id : brId)
                    orderby u.gl_cd
                    select new
                    {
                        u.gl_cd,
                        code = u.code != null ? u.code : u.gl_cd,
                        u.gl_dsc,

                        balance = GetRemainingBlc(brId, u.gl_cd)

                    }).Take(10).ToList();

            //return (from u in Data.Glmf_Codes
            //        where (u.gl_cd.Contains(gl_cd) || u.gl_dsc.Contains(gl_cd)) && u.ct_id.Equals("D")
            //        orderby u.gl_cd
            //        select new
            //        {
            //            u.gl_cd,
            //            code = u.code != null ? u.code : u.gl_cd,
            //            u.gl_dsc

            //        }).Take(5).ToList();
        }

        public List<decimal> GetRemainingBlc(int? brId,string glcd)
        {
            
            using (RMSDataContext db = new RMSDataContext())
            {
                decimal FinancialYear = db.FIN_PERDs.Where(x => x.Cur_Year == "CUR").FirstOrDefault().Gl_Year;

                List<sp_RemainingBlncvoucherResult> remai = db.sp_RemainingBlncvoucher(brId, FinancialYear, glcd).ToList();
                List<decimal> list = new List<decimal>();
                foreach (var item in remai)
                {
                    VoucherRemainigBlnc blnc = new VoucherRemainigBlnc();
                    blnc.Release = Convert.ToDecimal(item.ReleaseGrant);
                    blnc.Exp = Convert.ToDecimal(item.expp);
                    blnc.Remaining = blnc.Release - blnc.Exp;
                    list.Add(blnc.Remaining);
                }
                return list;

            } 
        }

        public object GetDetailDropDown( int brId, RMSDataContext Data)
        {


            return (from u in Data.Glmf_Codes
                    join v in Data.glmf_ven_cus_dets
                    on u.gl_cd equals v.gl_cd into sub
                    from v in sub.DefaultIfEmpty()
                    where (u.ct_id.Equals("D"))
                    && brId == (v != null && v.br_id != null ? v.br_id : brId)
                    orderby u.gl_cd
                    select u).ToList();

            //return (from u in Data.Glmf_Codes
            //        where (u.gl_cd.Contains(gl_cd) || u.gl_dsc.Contains(gl_cd)) && u.ct_id.Equals("D")
            //        orderby u.gl_cd
            //        select new
            //        {
            //            u.gl_cd,
            //            code = u.code != null ? u.code : u.gl_cd,
            //            u.gl_dsc

            //        }).Take(5).ToList();
        }

        public object GetDetailAccountForTemplates(string gl_cd, int voucherTypeID, RMSDataContext Data)
        {
            var tha = (from a in Data.TemplateHeadAccounts
                      where a.TemplateID == voucherTypeID
                      select a.Account).ToList();

            if (tha.Count == 0)
            {
                return (from u in Data.Glmf_Codes
                        where (u.code.Contains(gl_cd) || u.gl_dsc.Contains(gl_cd)) && u.ct_id.Equals("D")
                        orderby u.gl_cd
                        select new
                        {
                            u.gl_cd,
                            code = u.code != null ? u.code: u.gl_cd,
                            u.gl_dsc

                        }).Take(5).ToList();
            }
            else
            {
                var td = (from b in Data.TemplateDetails
                          where b.TemplateID == voucherTypeID
                          select b.Account).ToList();

                var result = (
                        from u in Data.Glmf_Codes
                        where (u.code.Contains(gl_cd) || u.gl_dsc.Contains(gl_cd)) && u.ct_id.Equals("D")
                        && tha.Contains(u.cnt_gl_cd)
                        && !td.Contains(u.gl_cd)
                        orderby u.gl_dsc
                        select new
                        {
                            u.gl_cd,
                            code = u.code != null ? u.code : u.gl_cd,
                            u.gl_dsc

                        }).Take(5).ToList();

                return result;
            }
        }
        public List<string> GetDetailAccount(string gl_cd, RMSDataContext Data)
        {

            return (from u in Data.Glmf_Codes
                    where (u.code.StartsWith(gl_cd) || u.gl_dsc.StartsWith(gl_cd)) && u.ct_id.Equals("D")
                    orderby u.gl_cd
                    select u.code !=null ? u.code : u.gl_cd + ' ' + u.gl_dsc).Take(5).ToList();
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
                var obj =(from a in Data.Preferences select a.p_id);
                if (obj.Count() > 0)
                    return Convert.ToInt32(obj.Max()) + 1;
                else
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

