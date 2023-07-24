using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.Linq;

namespace RMS.BL
{
   public class AreaCodeBL
    {
       public AreaCodeBL()
        { 
        }

       //public IQueryable<tblglactype> GetAccountTypes(RMSDataContext Data)
       //{
       //    if (Data == null) { Data = RMSDB.GetOject(); }
       //    try
       //    {
       //        return Data.tblglactypes.Where(acc => acc.Status.Value.Equals(true)).AsQueryable();
       //    }
       //    catch
       //    { }
       //    return null;
       //}

       //public glmf_ven_cus_det Get_glmf_ven_cus_det(string glCd, RMSDataContext Data)
       //{
       //    if (Data == null) { Data = RMSDB.GetOject(); }
       //    try
       //    {
       //        return Data.glmf_ven_cus_dets.Where(ven => ven.gl_cd.Equals(glCd)).SingleOrDefault();
       //    }
       //    catch
       //    { }
       //    return null;
       //}

       //public bool Insert_glmf_ven_cus_det(glmf_ven_cus_det ven, RMSDataContext Data)
       //{
       //    if (Data == null) { Data = RMSDB.GetOject(); }
       //    try
       //    {
       //        Data.glmf_ven_cus_dets.InsertOnSubmit(ven);
       //        Data.SubmitChanges();
       //        return true;
       //    }
       //    catch
       //    { }
       //    return false;
       //}
       
       //public bool Update_glmf_ven_cus_det(glmf_ven_cus_det ven, RMSDataContext Data)
       //{
       //    if (Data == null) { Data = RMSDB.GetOject(); }
       //    try
       //    {
       //        Data.SubmitChanges();
       //        return true;
       //    }
       //    catch
       //    { }
       //    return false;
       //}

       //public object GetCities(RMSDataContext Data)
       //{
       //    if (Data == null) { Data = RMSDB.GetOject(); }
       //    try
       //    {
       //        return Data.tblCities.ToList();
       //    }
       //    catch
       //    { }
       //    return null;
       //}

       public int GetCount(string cntGlCd, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               if (cntGlCd == null)
               {
                   return Data.tblSlAreaCds.Where(cd => cd.cnt_ar_cd == null).Count();
               }
               else
               {
                   return Data.tblSlAreaCds.Where(cd => cd.cnt_ar_cd == cntGlCd).Count();
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
                   string glcd = Data.tblSlAreaCds.Where(cd => cd.cnt_ar_cd == null).Select(gl => gl.ar_cd).Max().Substring(startindex, codelength);
                   int gl_cd = Convert.ToInt32(glcd) + 1;
                   return gl_cd;
               }
               else
               {
                   string glcd = Data.tblSlAreaCds.Where(cd => cd.cnt_ar_cd == cntGlCd).Select(gl => gl.ar_cd).Max().Substring(startindex, codelength);
                   int gl_cd = Convert.ToInt32(glcd) + 1;
                   return gl_cd;
               }
           }
           catch { }
           //return -1;
           return 1;
       }

       //public bool InsertIntoGlmf(EntitySet<Glmf> setGlmf, RMSDataContext Data)
       //{
       //     if (Data == null) { Data = RMSDB.GetOject(); }
       //     try
       //     {
       //         Data.Glmfs.InsertAllOnSubmit(setGlmf);
       //         Data.SubmitChanges();
       //         return true;
       //     }
       //     catch { }
       //     return false;
       //}

       //public List<Branch> GetBranches(RMSDataContext Data)
       //{
       //    if (Data == null) { Data = RMSDB.GetOject(); }
       //    List<Branch> branches = (from b in Data.Branches
       //                             select b).ToList();
       //    return branches;

       //}


       public bool updateCode(tblSlAreaCd tblGlCd, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               tblSlAreaCd tbl = (from a in Data.tblSlAreaCds
                                   where a.ar_cd == tblGlCd.ar_cd
                                   select a).Single();
               tbl.ar_dsc = tblGlCd.ar_dsc;
               Data.SubmitChanges();
               return true;
           }
           catch //(Exception ex)
           {
               //throw ex;
           }
           return false;
       }

       public tblSlAreaCd GetRecByID(string glCd,  RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }
               tblSlAreaCd rec = (from a in Data.tblSlAreaCds
                                where a.ar_cd == glCd
                                select a).SingleOrDefault();
               return rec;
           }
           catch
           {
               return null;
           }
       }

       public string SaveItem(tblSlAreaCd glCd, RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }
               Data.tblSlAreaCds.InsertOnSubmit(glCd);
               Data.SubmitChanges();
               return "ok";
           }
           catch(Exception ex)
           {
               return ex.Message;
           }
       }


       public List<tblSlAreaCdTyp> GetGlType(RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           return Data.tblSlAreaCdTyps.ToList();

       }

       public object CodeVal(string typeid1, string typeid2, RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }

               tblSlAreaCdTyp codetype = Data.tblSlAreaCdTyps.Where(typ => typ.ar_id.ToString().Equals(typeid1)).SingleOrDefault();
               tblSlAreaCdTyp codetype1 = Data.tblSlAreaCdTyps.Where(typ1 => typ1.ar_id.Equals(codetype.cnt_ar_id)).SingleOrDefault();

               var heads = (from t in Data.tblSlAreaCdTyps
                            join g in Data.tblSlAreaCds on t.cnt_ar_id equals g.ar_id into leftjoin
                            from g in leftjoin.DefaultIfEmpty()
                            where t.ar_id.ToString().Equals(typeid1) && g.ar_cd.ToString().Equals(typeid2) 
                            select new
                            {
                                t.ar_len,
                                g.ar_cd,
                                p_ct_len = codetype1.ar_len
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
               List<tblSlAreaCd> ls = (from a in Data.tblSlAreaCds
                                        where a.ar_cd == glCd
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
               List<tblSlAreaCd> ls = (from a in Data.tblSlAreaCds
                                     where a.ar_dsc == glDescription
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


       public tblSlAreaCd GetCode(string Code, RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }

               return Data.tblSlAreaCds.Where(cd => cd.ar_cd.Equals(Code)).SingleOrDefault();
           }
           catch //(Exception ex)
           {
               //RMSDB.closeConn(Data);
               //throw ex;
           }
           return null;
       }

       public object GetCodeHead(tblSlAreaCdTyp glCodeTyp, RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }
               var obj = (from a in Data.tblSlAreaCds
                          join b in Data.tblSlAreaCdTyps
                          on a.ar_id equals b.ar_id
                          where a.ar_id.Equals(glCodeTyp.cnt_ar_id) && b.ar_id.Equals(glCodeTyp.cnt_ar_id)
                          select new
                          {
                              gl_cd = a.ar_cd,
                              gl_dsc = a.ar_cd + "-" + a.ar_dsc
                          }).ToList();
               return obj.OrderBy(o=> o.gl_cd);
           }
           catch (Exception ex)
           {
               RMSDB.closeConn(Data);
               throw ex;
           }
       }

      

       public tblSlAreaCdTyp GetCodeType(char id, RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }

               return Data.tblSlAreaCdTyps.Where(typ => typ.ar_id.Equals(id)).SingleOrDefault();
           }
           catch //(Exception ex)
           {
               //RMSDB.closeConn(Data);
               //throw ex;
           }
           return null;
       }

       public IQueryable<tblSlAreaCdTyp> GetCodeType(RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }

               return Data.tblSlAreaCdTyps.AsQueryable();
           }
           catch (Exception ex)
           {
               RMSDB.closeConn(Data);
               throw ex;
           }
       }

       public List<spGetALLAreaCodeResult> GetAllGlCodeFilteredData(RMSDataContext Data, string glCode, string description, string codeType)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               return Data.spGetALLAreaCode(codeType, glCode, description).ToList();
           }
           catch
           {
               return null;
           }
       }

       // M.A to fill outlet popup at SalesPromotionResponse.aspx
       public List<tblSlAreaCd> GetAllDetaildRecords(RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               return Data.tblSlAreaCds.Where(m => m.ar_id == "D").ToList();
           }
           catch
           {
               return null;
           }
       }
       // M.A. to fill retailer auto complete at RetailerVisitPlan.aspx
       //public List<spSearchRetailerResult> GetDetaildRecordsForRetailer(string searchText,RMSDataContext Data)
       //{
       //    if (Data == null) { Data = RMSDB.GetOject(); }
       //    try
       //    {
       //        return Data.spSearchRetailer(searchText).ToList();
       //    }
       //    catch
       //    {
       //        return null;
       //    }
       //}
       //// M.A. select shop by shop id at RetailerVisitPlan.aspx.cs
       public tblSlAreaCd GetDetailByID(int id,RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               return Data.tblSlAreaCds.Where(m => m.ar_id == "D" && m.id==id).SingleOrDefault();
           }
           catch
           {
               return null;
           }
       }

       // M.A. get all areas from tblSlAreaCd 
       public List<tblSlAreaCd> GetAllAreas(RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               return Data.tblSlAreaCds.Where(m => m.ar_id ==  "A").ToList();
           }
           catch
           {
               return null;
           }
       }

       // M.A. get all sub areas from tblSlAreaCd 
       public List<tblSlAreaCd> GetAllSubAreasByAreaCode(string areaCode,RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               List<tblSlAreaCd> lstSubAreas=(from a in Data.tblSlAreaCds
                                    where a.ar_id=="C" && a.cnt_ar_cd==(areaCode== "0" ? a.cnt_ar_cd:areaCode)
                                    select a).ToList();

               tblSlAreaCd tbl = new tblSlAreaCd();
               tbl.ar_cd = "0";
               tbl.ar_dsc="All";

               lstSubAreas.Insert(0,tbl);
               return lstSubAreas;
           }
           catch
           {
               return null;
           }
       }
    }
}
