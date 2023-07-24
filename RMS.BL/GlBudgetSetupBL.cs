using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.Linq;


namespace RMS.BL
{
    public class GlBudgetSetupBL
    {

        public GlBudgetSetupBL()
        {
        }

    
        //----------------
        public List<Anonymous4BudgetGrid> GetAll(RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            var li=(from a in Data.tblBgtHeads
                    orderby a.Bgt_Code
                    select new Anonymous4BudgetGrid
                    {
                      Bgt_Code= a.Bgt_Code,
                      Bgt_Type= a.Bgt_Type ==null ? ' ' : Convert.ToChar(a.Bgt_Type),
                      Code_Type = Convert.ToChar(a.Code_Type),
                      Headg_Desc= a.Headg_Desc,
                      cnt_bgt_code= a.cnt_bgt_code == null ? "":a.cnt_bgt_code,
                      AC_Fr_To =  a.GL_AC_Fr + " - " + a.GL_AC_To
                      
                   }).ToList();
            return li;
        }
        //-------------------------

        //-----------------------
        public List<GetAllBudgetCodeResult> getAllBudgetCodeFiltered(RMSDataContext Data,string bgCd, string heading,char cdType,char bgType)
        {
            if (Data == null)
            {Data = RMSDB.GetOject();}

            List<GetAllBudgetCodeResult> list = Data.GetAllBudgetCode(bgCd,heading,cdType.ToString(),bgType.ToString()).ToList();

            return list;
        }

            //---------------
       
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
        //----------
        public object GetCodeHead(RMSDataContext Data, string ct_id)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            var code = (from a in Data.tblBgtHeads
                        where a.Code_Type.Equals(ct_id) //&& !a.cnt_bgt_code.Equals(null)
                        select new
                        {
                            Headg_Desc = a.Bgt_Code + "-" + a.Headg_Desc,
                           
                            Bgt_Code = a.Bgt_Code
                        });
            return code.ToList();


            
        }
        //===============================
      

        public char getParent(string s,RMSDataContext Data)
        {
            if(Data==null){Data= RMSDB.GetOject();}

            var cd = (from a in Data.Code_Types
                      where a.ct_id == s && a.cnt_ct_id!=null
                      select a.cnt_ct_id).Single();

            return Convert.ToChar(cd);
        }

        //---------------

        //public string GetCodeHeadControl(RMSDataContext Data, string gl_cd)
        //{
        //    if (Data == null) { Data = RMSDB.GetOject(); }
        //    string code = (from t in Data.tblBgtHeads
        //                   where t.Bgt_Code.Equals(gl_cd)
        //                   select t.cnt_bgt_code).Single();

        //    return code;
            
        //}

        public int GetCodeTypeLength(RMSDataContext Data, char gt_cd)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            return Data.Code_Types.Where(t => t.ct_id.Equals(gt_cd)).Select(s => s.ct_len).Single();
        }

        public char GetGLTypeCode(RMSDataContext Data, string gl_cd)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            char querystr = Convert.ToChar((from t in Data.tblBgtHeads
                             where t.Bgt_Code.Equals(gl_cd)
                             select t.Bgt_Type).Single());

            return querystr;


        }
        public tblBgtHead GetByID(string code, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                tblBgtHead costobj = Data.tblBgtHeads.Single(p => p.Bgt_Code.Equals(code));

                return costobj;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        //------------------
        
       
        public bool CodeExists(tblBgtHead cto, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            var glCode = from c in Data.tblBgtHeads
                         where c.Bgt_Code == cto.Bgt_Code
                         select c;
            if (glCode.Count() == 0)
            {
                return false;
            }
            else
                return true;
        }
        public bool ISAlreadyExist(tblBgtHead cto, RMSDataContext Data)
        {
            try
            {
                bool isalready = false;
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<tblBgtHead> cty = (from ct in Data.tblBgtHeads
                                             where ct.Bgt_Code == cto.Bgt_Code && (ct.Headg_Desc == cto.Headg_Desc)
                                             select ct).AsQueryable();
             
                if (cty != null && cty.Count<tblBgtHead>() > 0)
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
                var glheads = (from g in Data.tblBgtHeads
                               where g.Bgt_Code == glCode
                               select new
                               {
                                   //g.gt_cd,
                                   g.Bgt_Code
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
                                   join g in Data.tblBgtHeads on t.cnt_ct_id equals g.Code_Type.ToString() into leftjoin
                                   from g in leftjoin.DefaultIfEmpty()
                                   where g.Code_Type.Equals("A")
                                   select new
                                   {
                                       g.cnt_bgt_code,
                                       g.Bgt_Code,
                                       g.Headg_Desc,
                                       t.ct_len,
                                       //g.gt_cd
                                   }).ToList();
                    return glheads;

                }
                else if (typeId == "D")
                {
                    var glheads = (from t in Data.Code_Types
                                   join g in Data.tblBgtHeads on t.cnt_ct_id equals g.Code_Type.ToString() into leftjoin
                                   from g in leftjoin.DefaultIfEmpty()
                                   where g.Code_Type.Equals("C")
                                   select new
                                   {
                                       g.cnt_bgt_code,
                                       g.Bgt_Code,
                                       g.Headg_Desc,
                                       t.ct_len,
                                       //g.gt_cd
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

       
        public List<Branch> GetBranches(RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            List<Branch> branches = (from b in Data.Branches
                                     select b).ToList();
            return branches;

        }

       

        public void Insert(tblBgtHead cty, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }


                Data.tblBgtHeads.InsertOnSubmit(cty);
               
                Data.SubmitChanges();

            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public bool Update(string code,string desc,string fr,string to,char bgType,DateTime upOn,string upBy, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                tblBgtHead tblBgt = Data.tblBgtHeads.Single(p => p.Bgt_Code.Equals(code));

                tblBgt.Bgt_Type = bgType.ToString();
                tblBgt.GL_AC_Fr = fr;
                tblBgt.GL_AC_To = to;
                tblBgt.Headg_Desc = desc;
                tblBgt.Updated_On = upOn;
                tblBgt.Updated_By = upBy;

                Data.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        //----------------For BudgetSetup AutoComplete for A/C Range
        public List<Glmf_Code> AutoCompletAcRange(string prefix)
        {
            try
            {
                RMSDataContext Data = new RMSDataContext();

                List<Glmf_Code> list = (from a in Data.Glmf_Codes
                                        where a.ct_id == "D" && a.gl_cd.Contains(prefix)
                                        select a).Take(5).ToList();

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //---------------Cost Center
        public List<Cost_Center> AutoCompletCostCenter(string prefix)
        {
            try
            {
                RMSDataContext Data = new RMSDataContext();

                return Data.Cost_Centers.Where(cc=> cc.status == true && (cc.cc_cd.Contains(prefix) || cc.cc_nme.Contains(prefix)) && (cc.typ.ToLower() == "gl" || cc.typ.ToLower() == "al")).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //==================For Budget Entry=========

        public List<GetAllBgtEntryResult> getAllBgtEntryGrid(RMSDataContext Data,Decimal yr,string cd, string desc, char type)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                List<GetAllBgtEntryResult> list = Data.GetAllBgtEntry(yr,cd,desc,type.ToString()).ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Decimal getCurrnetFinYear(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                var FinYr = (from a in Data.FIN_PERDs
                             where a.Cur_Year.Equals("CUR")
                             select a.Gl_Year).Single();
                return Convert.ToDecimal(FinYr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public tblBgtHead getBgtHeadData(RMSDataContext Data, string cd)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                tblBgtHead bgH = (from a in Data.tblBgtHeads
                                        where a.Bgt_Code.Equals(cd)
                                        select a).Single();

                return bgH;
                                   
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public tblBgtHeadDet getLastYearBudget(RMSDataContext Data, string yr)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                tblBgtHeadDet bgt = (from a in Data.tblBgtHeadDets
                           where a.Bgt_Year.Equals(yr)
                           select a).Single();

                return bgt;
            }
            catch (Exception ex)
            {
                return null;
                
            }
        }

        //------------
        public List<Decimal>  getBudgetYear(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                List<Decimal> yr = (from a in Data.tblBgtHeadDets
                                   select a.Bgt_Year).Distinct().ToList();
                if (yr.Count > 0)
                {
                    return yr;
                }
                else
                {
                    yr.Add(Common.MyDate(Data).Year);
                    return yr;
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //------------

        public bool isExisting(RMSDataContext Data, string cd,string yr)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                List<tblBgtHeadDet> list = (from a in Data.tblBgtHeadDets
                                            where a.Bgt_Code.Equals(cd) && a.Bgt_Year.Equals(yr)
                                            select a).ToList();
                if (list.Count > 0)
                {
                    return true;
                }
                else
                    return false;

            }
            catch (Exception ex)
            {
                return true;
            }

        }

        public bool Save(RMSDataContext Data, tblBgtHeadDet tbl)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                Data.tblBgtHeadDets.InsertOnSubmit(tbl);
                Data.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void getRecordForUpdate(RMSDataContext Data, string cd, Decimal yr,int q1,int q2,int q3, int q4, string upB, DateTime dt)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                tblBgtHeadDet tbl = (from a in Data.tblBgtHeadDets
                                     where a.Bgt_Code.Equals(cd) && a.Bgt_Year.Equals(yr)
                                     select a).Single();

                tbl.Q1_Amt=q1;
                tbl.Q2_Amt=q2;
                tbl.Q3_Amt=q3;
                tbl.Q4_Amt=q4;
                tbl.Updated_By=upB;
                tbl.Updated_On=dt;

                Data.SubmitChanges();
               // return tbl;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool updateBgEntry(RMSDataContext Data, tblBgtHeadDet tbl) 
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.SubmitChanges();
               
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        
        }

        //-----------------------
        //public void RefreshBgtEntry(RMSDataContext Data,tblBgtHeadDet tbl)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = RMSDB.GetOject(); }
        //        Data.SubmitChanges();
        //        Data.Refresh(RefreshMode.KeepChanges, tbl);
                
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}
        //---------------------------
        
        public List<string> getByDataKeys(RMSDataContext Data, string cd, Decimal yr)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var ob = from a in Data.tblBgtHeads
                         join b in Data.tblBgtHeadDets
                         on a.Bgt_Code equals b.Bgt_Code
                         where b.Bgt_Year.Equals(yr) && b.Bgt_Code.Equals(cd)
                         select new
                         {
                             a.Bgt_Code,
                             a.Bgt_Type,
                             a.Headg_Desc,
                             a.GL_AC_Fr,
                             a.GL_AC_To,
                             b.Bgt_Year,
                             b.Q1_Amt,
                             b.Q2_Amt,
                             b.Q3_Amt,
                             b.Q4_Amt
                         };
                List<string> ls = new List<string>();
                foreach (var ab in ob)
                {
                    ls.Add(ab.Bgt_Code.ToString());
                    ls.Add(ab.Bgt_Type.ToString());
                    ls.Add(ab.Headg_Desc.ToString());
                    ls.Add(ab.GL_AC_Fr.ToString());
                    ls.Add(ab.GL_AC_To.ToString());
                    ls.Add(ab.Bgt_Year.ToString());
                    ls.Add(ab.Q1_Amt.ToString());
                    ls.Add(ab.Q2_Amt.ToString());
                    ls.Add(ab.Q3_Amt.ToString());
                    ls.Add(ab.Q4_Amt.ToString());

                }

                return ls;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //----------------For BudgetSetup AutoComplete for BudgetCode
        public List<tblBgtHead> AutoCompletBgtCode(string prefix)
        {
            try
            {
                RMSDataContext Data = new RMSDataContext();

                List<tblBgtHead> list = (from a in Data.tblBgtHeads
                                        where a.Code_Type == "D" && (a.Bgt_Code.Contains(prefix) || a.Headg_Desc.Contains(prefix))
                                        select a).Take(5).ToList();

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //=================Reports===========================

        public List<spBgtVarianceRptResult> BudgetVarianceRpt(Decimal yr, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                List<spBgtVarianceRptResult> list = Data.spBgtVarianceRpt(yr).ToList();
                return list;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }
        //---------------------rptBudgetList-----------------------------

        public List<tblBgtHead> rptBudgetList(RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                List<tblBgtHead> li = (from a in Data.tblBgtHeads
                                       where a.Code_Type.Equals('D')
                                       orderby a.Bgt_Code
                                       select a
                          ).ToList();
                return li;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //----------------------------------------


        //----------------11 April 2013------
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
        
        public object GetCodeHead4Budget(Code_Type BgtCodeTyp, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var obj = (from a in Data.tblBgtHeads
                           join b in Data.Code_Types
                           on a.Code_Type.ToString() equals b.ct_id
                           where a.Code_Type.Equals(BgtCodeTyp.cnt_ct_id) && b.ct_id.Equals(BgtCodeTyp.cnt_ct_id)
                           select new
                           {
                               a.Bgt_Code,
                               Headg_Desc = a.Bgt_Code + "-" + a.Headg_Desc
                           }).ToList();
                return obj;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }

        public tblBgtHead GetCode(string Code, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                return Data.tblBgtHeads.Where(cd => cd.Bgt_Code.Equals(Code)).SingleOrDefault();
            }
            catch //(Exception ex)
            {
                //RMSDB.closeConn(Data);
                //throw ex;
            }
            return null;
        }

        public int GetCount(string cntBgtCd, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                if (cntBgtCd == null)
                {
                    return Data.tblBgtHeads.Where(cd => cd.cnt_bgt_code == null).Count();
                }
                else
                {
                    return Data.tblBgtHeads.Where(cd => cd.cnt_bgt_code == cntBgtCd).Count();
                }
            }
            catch { }
            return -1;
        }


        public object CodeVal(string typeid1, string typeid2, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                Code_Type codetype = Data.Code_Types.Where(typ => typ.ct_id.ToString().Equals(typeid1)).SingleOrDefault();
                Code_Type codetype1 = Data.Code_Types.Where(typ1 => typ1.ct_id.Equals(codetype.cnt_ct_id)).SingleOrDefault();

                var heads = (from t in Data.Code_Types
                             join g in Data.tblBgtHeads on t.cnt_ct_id equals g.Code_Type.ToString() into leftjoin
                             from g in leftjoin.DefaultIfEmpty()
                             where t.ct_id.ToString().Equals(typeid1) && g.Bgt_Code.ToString().Equals(typeid2)
                             select new
                             {
                                 t.ct_len,
                                 g.Bgt_Code,
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


        //--------------------------------
    }
}
