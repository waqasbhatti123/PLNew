using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.Linq;

namespace RMS.BL
{
   public class GlCodeBL1
    {
       public GlCodeBL1()
        { 
        }

       public IQueryable<tblglactype> GetAccountTypes(RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               return Data.tblglactypes.Where(acc => acc.Status.Value.Equals(true)).AsQueryable();
           }
           catch
           { }
           return null;
       }

       public glmf_ven_cus_det Get_glmf_ven_cus_det(string glCd, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               return Data.glmf_ven_cus_dets.Where(ven => ven.gl_cd.Equals(glCd)).SingleOrDefault();
           }
           catch
           { }
           return null;
       }

       public bool Insert_glmf_ven_cus_det(glmf_ven_cus_det ven, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               Data.glmf_ven_cus_dets.InsertOnSubmit(ven);
               Data.SubmitChanges();
               return true;
           }
           catch
           { }
           return false;
       }
       
       public bool Update_glmf_ven_cus_det(glmf_ven_cus_det ven, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               Data.SubmitChanges();
               return true;
           }
           catch
           { }
           return false;
       }

       public object GetCities(RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               return Data.tblCities.ToList();
           }
           catch
           { }
           return null;
       }

       public int GetCount(string cntGlCd, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               if (cntGlCd == null)
               {
                   return Data.Glmf_Codes.Where(cd => cd.cnt_gl_cd == null).Count();
               }
               else
               {
                   return Data.Glmf_Codes.Where(cd => cd.cnt_gl_cd == cntGlCd).Count();
               }
           }
           catch { }
           return -1;
       }

       public int GetMax(string cntGlCd, int startindex,int codelength, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               if (cntGlCd == null)
               {
                   string glcd = Data.Glmf_Codes.Where(cd => cd.cnt_gl_cd == null).Select(gl => gl.gl_cd).Max().Substring(startindex, codelength);
                   int gl_cd = Convert.ToInt32(glcd) + 1;
                   return gl_cd;
               }
               else
               {
                   string glcd = Data.Glmf_Codes.Where(cd => cd.cnt_gl_cd == cntGlCd).Select(gl => gl.gl_cd).Max().Substring(startindex, codelength);
                   int gl_cd = Convert.ToInt32(glcd) + 1;
                   return gl_cd;
               }
           }
           catch { }
           //return -1;
           return 1;
       }

       public bool InsertIntoGlmf(EntitySet<Glmf> setGlmf, RMSDataContext Data)
       {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                Data.Glmfs.InsertAllOnSubmit(setGlmf);
                Data.SubmitChanges();
                return true;
            }
            catch { }
            return false;
       }

       public List<Branch> GetBranches(RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           List<Branch> branches = (from b in Data.Branches
                                    select b).ToList();
           return branches;

       }

       public bool IsGlmfCodeExist(Glmf glmf, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               Glmf glmaster = Data.Glmfs.Where(gl=> gl.gl_cd == glmf.gl_cd && gl.br_id == glmf.br_id && gl.gl_year == glmf.gl_year).Single();
               return true;
           }
           catch //(Exception ex)
           {
               //throw ex;
           }
           return false;
       }


       public bool updateCode(Glmf_Code tblGlCd, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               Glmf_Code tbl = (from a in Data.Glmf_Codes
                                   where a.gl_cd == tblGlCd.gl_cd
                                   select a).Single();
               tbl.gl_dsc = tblGlCd.gl_dsc;
               tbl.code = tblGlCd.code;
               Data.SubmitChanges();
               return true;
           }
           catch //(Exception ex)
           {
               //throw ex;
           }
           return false;
       }

       public Glmf_Code GetRecByID(string glCd,  RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }
               Glmf_Code rec = (from a in Data.Glmf_Codes
                                where a.gl_cd == glCd
                                select a).SingleOrDefault();
               return rec;
           }
           catch
           {
               return null;
           }
       }


       public List<spGetALLGLCodeResult> GetAllGlCodeFilteredData(RMSDataContext Data, string glCode, string description, char glType, string codeType)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
                return Data.spGetALLGLCode(codeType ,glCode, description, Convert.ToString(glType)).ToList();
           }
           catch
           {
               return null;
           }
       }


       public string SaveItem(Glmf_Code glCd, RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }
               Data.Glmf_Codes.InsertOnSubmit(glCd);
               Data.SubmitChanges();
               return "ok";
           }
           catch(Exception ex)
           {
               return ex.Message;
           }
       }


       public List<Gl_Type> GetGlType(RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           return Data.Gl_Types.ToList();

       }

       public object CodeVal(string typeid1, string typeid2, RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }

               Code_Type codetype = Data.Code_Types.Where(typ => typ.ct_id.ToString().Equals(typeid1)).SingleOrDefault();
               Code_Type codetype1 = Data.Code_Types.Where(typ1 => typ1.ct_id.Equals(codetype.cnt_ct_id)).SingleOrDefault();

               var heads = (from t in Data.Code_Types
                            join g in Data.Glmf_Codes on t.cnt_ct_id equals g.ct_id into leftjoin
                            from g in leftjoin.DefaultIfEmpty()
                            where t.ct_id.ToString().Equals(typeid1) && g.gl_cd.ToString().Equals(typeid2) 
                            select new
                            {
                                t.ct_len,
                                g.gl_cd,
                                p_ct_len = codetype1.ct_len
                            }).ToList();


               return heads;
           }
           catch //(Exception ex)
           {
               //RMSDB.SetNull();
               // throw ex;
           }
           return null;
       }


       public bool CodeExits(string glCd, string glDescription, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               List<Glmf_Code> ls = (from a in Data.Glmf_Codes
                                        where a.gl_cd == glCd
                                        select a).ToList();
               if (ls.Count > 0)
               {
                   return true;
               }
               else
               {
                   return false;
               }
           }
           catch//(Exception ex)
           {
               //throw ex;
               return true;
           }
       }

       public bool DescExits(string glCd, string glDescription, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               List<Glmf_Code> ls = (from a in Data.Glmf_Codes
                                     where a.gl_dsc == glDescription && a.gl_cd != glCd
                                     select a).ToList();
               if (ls.Count > 0)
               {
                   return true;
               }
               else
               {
                   return false;
               }
           }
           catch//(Exception ex)
           {
               //throw ex;
               return true;
           }
       }


       public Glmf_Code GetCode(string Code, RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }

               return Data.Glmf_Codes.Where(cd => cd.gl_cd.Equals(Code)).SingleOrDefault();
           }
           catch //(Exception ex)
           {
               //RMSDB.closeConn(Data);
               //throw ex;
           }
           return null;
       }

       public object GetCodeHead(Code_Type glCodeTyp, RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }
               var obj = (from a in Data.Glmf_Codes
                          join b in Data.Code_Types
                          on a.ct_id equals b.ct_id
                          where a.ct_id.Equals(glCodeTyp.cnt_ct_id) && b.ct_id.Equals(glCodeTyp.cnt_ct_id)
                          select new
                          {
                              a.gl_cd,
                              gl_dsc = a.code != null ? a.code : a.gl_cd + "-" + a.gl_dsc
                          }).ToList();
               return obj.OrderBy(o=> o.gl_cd);
           }
           catch (Exception ex)
           {
               RMSDB.closeConn(Data);
               throw ex;
           }
       }

      

       public Code_Type GetCodeType(char id, RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }

               return Data.Code_Types.Where(typ => typ.ct_id.Equals(id)).SingleOrDefault();
           }
           catch //(Exception ex)
           {
               //RMSDB.closeConn(Data);
               //throw ex;
           }
           return null;
       }

       public IQueryable<Code_Type> GetCodeType(RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }

               return Data.Code_Types.AsQueryable();
           }
           catch (Exception ex)
           {
               RMSDB.closeConn(Data);
               throw ex;
           }
       }
    }
}
