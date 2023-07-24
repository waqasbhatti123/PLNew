using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.Linq;

namespace RMS.BL
{
   public class CCBL
    {
       public CCBL()
        { 
        }

       public List<Cost_Center> GetGroupCodes(RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               return Data.Cost_Centers.Where(cc => cc.cnt_cc_cd == null ).ToList();
           }
           catch { }
           return null;
       }
       public List<Cost_Center> GetInventoryCC(RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               return Data.Cost_Centers.Where(cc => cc.status == true && (cc.typ.ToLower() == "iv" || cc.typ.ToLower() == "al")).ToList();
           }
           catch { }
           return null;
       }

       public List<Cost_Center> GetGLCC(RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               return Data.Cost_Centers.Where(cc => cc.status == true && (cc.typ.ToLower() == "gl" || cc.typ.ToLower() == "al" && cc.cct_id == "D")).ToList();
           }
           catch { }
           return null;
       }

       public int GetCount(string cntGlCd, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               if (cntGlCd == null)
               {
                   return Data.Cost_Centers.Where(cd => cd.cnt_cc_cd == null).Count();
               }
               else
               {
                   return Data.Cost_Centers.Where(cd => cd.cnt_cc_cd == cntGlCd).Count();
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
                   string glcd = Data.Cost_Centers.Where(cd => cd.cnt_cc_cd == null).Select(gl => gl.cc_cd).Max().Substring(startindex, codelength);
                   int gl_cd = Convert.ToInt32(glcd) + 1;
                   return gl_cd;
               }
               else
               {
                   string glcd = Data.Cost_Centers.Where(cd => cd.cnt_cc_cd == cntGlCd).Select(gl => gl.cc_cd).Max().Substring(startindex, codelength);
                   int gl_cd = Convert.ToInt32(glcd) + 1;
                   return gl_cd;
               }
           }
           catch { }
           //return -1;
           return 1;
       }

       public bool updateCode(Cost_Center tblccCd, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               Cost_Center tbl = (from a in Data.Cost_Centers
                                   where a.cc_cd == tblccCd.cc_cd
                                   select a).Single();
               tbl.cc_nme = tblccCd.cc_nme;
               tbl.typ = tblccCd.typ;
               tbl.status = tblccCd.status;
               Data.SubmitChanges();
               return true;
           }
           catch //(Exception ex)
           {
               //throw ex;
           }
           return false;
       }

       public Cost_Center GetRecByID(string ccCd,  RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }
               Cost_Center rec = (from a in Data.Cost_Centers
                                where a.cc_cd == ccCd
                                select a).SingleOrDefault();
               return rec;
           }
           catch
           {
               return null;
           }
       }


       public List<spGetALLCCResult> GetAllGlCodeFilteredData(RMSDataContext Data, string glCode, string description, string codeType)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
                return Data.spGetALLCC(codeType ,glCode, description ).ToList();
           }
           catch
           {
               return null;
           }
       }


        public List<sp_GL_Ledger_CCResult> GetCCLedgher(int brid, decimal glyr, DateTime fromDate, DateTime toDate,
               string cc, string glCode, char status, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                return Data.sp_GL_Ledger_CC(brid, glyr, fromDate, toDate, cc, glCode).ToList();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public List<sp_GL_Ledger_CC_SummaryResult> GetCCSummaryLedgher(int brid, decimal glyr, DateTime fromDate, DateTime toDate,string grpcc, char status, string cc, string glCode, RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }
               
            
               return Data.sp_GL_Ledger_CC_Summary(brid, glyr, fromDate, toDate, grpcc, status, cc, glCode).ToList();
           }
           catch (Exception ex)
           {
               RMSDB.SetNull();
               throw ex;
           }
       }

       public List<sp_GL_Ledger_CC_DetailResult> GetCCDetailLedgher(int brid, decimal glyr, DateTime fromDate, DateTime toDate, string grpcc, char status, string cc, string glCode, RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }
               
               
               return Data.sp_GL_Ledger_CC_Detail(brid, glyr, fromDate, toDate, grpcc, status, cc, glCode).ToList();
           }
           catch (Exception ex)
           {
               RMSDB.SetNull();
               throw ex;
           }
       }


       public string SaveItem(Cost_Center ccCd, RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }
               Data.Cost_Centers.InsertOnSubmit(ccCd);
               Data.SubmitChanges();
               return "ok";
           }
           catch(Exception ex)
           {
               return ex.Message;
           }
       }


       public object CodeVal(string typeid1, string typeid2, RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }

               tblCCTyp codetype = Data.tblCCTyps.Where(typ => typ.cct_id.ToString().Equals(typeid1)).SingleOrDefault();
               tblCCTyp codetype1 = Data.tblCCTyps.Where(typ1 => typ1.cct_id.Equals(codetype.cnt_cct_id)).SingleOrDefault();

               var heads = (from t in Data.tblCCTyps
                            join g in Data.Cost_Centers on t.cnt_cct_id equals g.cct_id into leftjoin
                            from g in leftjoin.DefaultIfEmpty()
                            where t.cct_id.ToString().Equals(typeid1) && g.cc_cd.ToString().Equals(typeid2) 
                            select new
                            {
                                t.cct_len,
                                g.cc_cd,
                                p_ct_len = codetype1.cct_len
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


       public bool CodeExits(string ccCd, string glDescription, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               List<Cost_Center> ls = (from a in Data.Cost_Centers
                                   where a.cc_cd == ccCd
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

       public bool DescExits(string ccCd, string glDescription, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               List<Cost_Center> ls = (from a in Data.Cost_Centers
                                     where a.cc_nme == glDescription
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


       public Cost_Center GetCode(string Code, RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }

               return Data.Cost_Centers.Where(cd => cd.cc_cd.Equals(Code)).SingleOrDefault();
           }
           catch //(Exception ex)
           {
               //RMSDB.closeConn(Data);
               //throw ex;
           }
           return null;
       }

       public object GetCodeHead(tblCCTyp glCodeTyp, RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }
               var obj = (from a in Data.Cost_Centers
                          join b in Data.tblCCTyps
                          on a.cct_id equals b.cct_id
                          where a.cct_id.Equals(glCodeTyp.cnt_cct_id) && b.cct_id.Equals(glCodeTyp.cnt_cct_id)
                          select new
                          {
                              a.cc_cd,
                              cc_nme = a.cc_cd + "-" + a.cc_nme
                          }).ToList();
               return obj.OrderBy(o=> o.cc_cd);
           }
           catch (Exception ex)
           {
               RMSDB.closeConn(Data);
               throw ex;
           }
       }

      

       public tblCCTyp GetCodeType(char id, RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }

               return Data.tblCCTyps.Where(typ => typ.cct_id.Equals(id)).SingleOrDefault();
           }
           catch //(Exception ex)
           {
               //RMSDB.closeConn(Data);
               //throw ex;
           }
           return null;
       }

       public IQueryable<tblCCTyp> GetCodeType(RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }

               return Data.tblCCTyps.AsQueryable();
           }
           catch (Exception ex)
           {
               RMSDB.closeConn(Data);
               throw ex;
           }
       }
    }
}
