using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.Linq;
using RMS.BL.Model;

namespace RMS.BL
{
   public class GlCodeBL
    {
       public GlCodeBL()
        { 
        
        }

        public List<SalarySummarySheetModel> GetSalarySummaryCodes(RMSDataContext Data, string type, decimal salaryPeriod)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                var sheet = new List<SalarySummarySheetModel>();

                var heads = Data.SalarySummmaryHeads.Where(x => x.IsActive && x.Type == type).ToList();
                var codes = (from h in heads 
                            join c in Data.Glmf_Codes
                            on h.Account equals c.cnt_gl_cd
                            where c.ct_id == "D"
                            select c).ToList();

                foreach(var code in codes)
                {
                    var ss = new SalarySummarySheetModel();
                    ss.gl_cd = code.gl_cd;
                    ss.gl_dsc = code.gl_dsc;

                    var sss = Data.SalarySummarySheets.Where(x => x.Type == type && x.SalPerd == salaryPeriod && x.Account == code.gl_cd).SingleOrDefault();
                    if(sss != null)
                    {
                        ss.amount = sss.Amount;
                    }
                    else
                    {
                        ss.amount = 0;
                    }

                    sheet.Add(ss);
                }
                return sheet;
            }
            catch { throw; }            
        }

        public List<GetALLGLCodeResult> GetAll(RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Data.GetALLGLCode().ToList();
        }

        public List<GetALLGLCodeResult> GetFilteredData(RMSDataContext Data,string sTR,int flag)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            if(flag ==1)
            {
            return Data.GetALLGLCode().Where(t=>t.gl_cd.Contains(sTR)).ToList();
            }
            else if (flag == 2)
            {

                return Data.GetALLGLCode().Where(t => t.gl_dsc.Contains(sTR)).ToList();

            }
            else if (flag == 3)
            {
                return Data.GetALLGLCode().Where(t => t.gl_type.Contains(sTR)).ToList();

            }
            else
            {
                return Data.GetALLGLCode().ToList();
            
            }
        }
        public List<GetALLGLCodeResult> GetAllGlCodeFilteredData(RMSDataContext Data, string glCode, string description, string glType, string codeType)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            List<GetALLGLCodeResult> list = Data.GetALLGLCode().ToList();
            if (!glCode.Equals(""))
            {
                list = list.Where(t => t.gl_cd.Contains(glCode)).ToList();
            }
            if (!description.Equals(""))
            {
                list = list.Where(t => t.gl_dsc.Contains(description)).ToList();
            }
            if (!glType.Equals("0"))
            {
                list = list.Where(t => t.gl_type.Contains(glType)).ToList();
            }
            if (!codeType.Equals("0"))
            {
                list = list.Where(t => t.codetype.Contains(codeType)).ToList();
            }

            return list;
        }
        public List<Code_Type> GetCodeType(RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Data.Code_Types.ToList();
        
        }

        public List<Gl_Type> GetGlType(RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Data.Gl_Types.ToList();

        }

        public List<Glmf_Code> GetCodeHead(RMSDataContext Data,string ct_id)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            return Data.Glmf_Codes.Where(t=> t.ct_id.Equals(ct_id) && !t.cnt_gl_cd.Equals(null)).ToList();

        }

        public string GetCodeHeadControl(RMSDataContext Data, string gl_cd)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            string code = (from t in Data.Glmf_Codes
                           where t.gl_cd.Equals(gl_cd)
                           select t.cnt_gl_cd).Single();

            return code;
            //return Data.Glmf_Codes.Where(t => t.gl_cd.Equals(gl_cd)).Select(s=>s.cnt_gl_cd).Single();

        }

        public int GetCodeTypeLength(RMSDataContext Data, char gt_cd)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            return Data.Code_Types.Where(t => t.ct_id.Equals(gt_cd)).Select(s => s.ct_len).Single();
        }

        public string GetGLTypeCode(RMSDataContext Data,string gl_cd)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            string querystr = (from t in Data.Glmf_Codes
                            where t.gl_cd.Equals(gl_cd)
                            select t.gt_cd).Single();

            return querystr;
            

        }
        public Glmf_Code GetByID(string code, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Glmf_Code costobj = Data.Glmf_Codes.Single(p => p.gl_cd.Equals(code));

                return costobj;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public bool CodeExists(Glmf_Code cto, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            var glCode = from c in Data.Glmf_Codes
                         where c.gl_cd == cto.gl_cd
                         select c;
            if (glCode.Count() == 0)
            {
                return false;
            }
            else
                return true;
        }
        public bool ISAlreadyExist(Glmf_Code cto, RMSDataContext Data)
        {
            try
            {
                bool isalready = false;
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<Glmf_Code> cty =   (from ct in Data.Glmf_Codes
                                              where ct.gl_cd == cto.gl_cd && (ct.gl_dsc == cto.gl_dsc)
                                              select ct).AsQueryable();
                //List<Glmf_Code> cty1 = (from ct in Data.Glmf_Codes
                //                             where ct.gl_cd == cto.gl_cd 
                //                             select ct).ToList();

                //if (cty1 != null && cty1.Count<Glmf_Code>() > 0)
                //    isalready = true;

                if (cty != null && cty.Count<Glmf_Code>() > 0)
                isalready = true;
                
                return isalready;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }

        }

       public object GLCodeHeadsType(string glCode, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var glheads = (from g in Data.Glmf_Codes
                               where g.gl_cd == glCode
                               select new
                               {
                                   g.gt_cd,
                                   g.gl_cd
                               }).ToList();
                return glheads;
                
            }
            catch (Exception e)
            {
                return false;

            }


        }

        public object GLCodeHeads(string typeId, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                if (typeId == "A")
                {

                    var glheads = (from t in Data.Code_Types
                                   where t.cnt_ct_id == null
                                   select new
                                   {
                                       t.ct_len,

                                   }).ToList();
                    return glheads;

                }
                else if (typeId == "C")
                {
                    var glheads = (from t in Data.Code_Types
                                   join g in Data.Glmf_Codes on t.cnt_ct_id equals g.ct_id into leftjoin
                                   from g in leftjoin.DefaultIfEmpty()
                                   where g.ct_id.Equals("A")
                                   select new
                                   {
                                       g.cnt_gl_cd,
                                       g.gl_cd,
                                       g.gl_dsc,
                                       t.ct_len,
                                       g.gt_cd
                                   }).ToList();
                    return glheads;

                }
                else if (typeId == "D")
                {
                    var glheads = (from t in Data.Code_Types
                                   join g in Data.Glmf_Codes on t.cnt_ct_id equals g.ct_id into leftjoin
                                   from g in leftjoin.DefaultIfEmpty()
                                   where g.ct_id.Equals("C")
                                   select new
                                   {
                                       g.cnt_gl_cd,
                                       g.gl_cd,
                                       g.gl_dsc,
                                       t.ct_len,
                                       g.gt_cd
                                   }).ToList();
                    return glheads;
                }



                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;

            }
        
        
        }

        public object GLCodeVal(string typeid1,string typeid2, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                if (typeid1 == "A")
                {

                    var glheads = (from t in Data.Code_Types
                                   where t.cnt_ct_id == null
                                   select new
                                   {
                                       t.ct_len,

                                   }).ToList();
                    return glheads;
                
                }
                else if (typeid1 == "C")
                {
                    var glheads = (from t in Data.Code_Types
                                   join g in Data.Glmf_Codes on t.cnt_ct_id equals g.ct_id into leftjoin
                                   from g in leftjoin.DefaultIfEmpty()
                                   where g.ct_id.Equals("A") && g.gl_cd.Equals(typeid2)
                                   select new
                                   {
                                       g.cnt_gl_cd,
                                       g.gl_cd,
                                       g.gl_dsc,
                                       t.ct_len,
                                       g.gt_cd
                                   }).ToList();

                    return glheads;
                }
                else if (typeid1 == "D")
                {
                    var glheads = (from t in Data.Code_Types
                                   join g in Data.Glmf_Codes on t.cnt_ct_id equals g.ct_id into leftjoin
                                   from g in leftjoin.DefaultIfEmpty()
                                   where g.ct_id.Equals("C") && g.gl_cd.Equals(typeid2)
                                   select new
                                   {
                                       g.cnt_gl_cd,
                                       g.gl_cd,
                                       g.gl_dsc,
                                       t.ct_len,
                                       g.gt_cd
                                   }).ToList();
                    return glheads;

                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }

        }
        public List<Branch> GetBranches(RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            List<Branch> branches = (from b in Data.Branches
                                  select b).ToList();
            return branches;

        }

        public void Insert(Glmf_Code cty, EntitySet<Glmf> setGlmf, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }


                    Data.Glmf_Codes.InsertOnSubmit(cty);
                if(setGlmf != null)
                    Data.Glmfs.InsertAllOnSubmit(setGlmf);
                    Data.SubmitChanges();

            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public void Update(Glmf_Code cty, RMSDataContext Data)
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
