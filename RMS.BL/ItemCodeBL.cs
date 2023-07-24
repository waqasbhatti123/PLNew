using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.Linq;

namespace RMS.BL
{
   public class ItemCodeBL
    {
       public ItemCodeBL()
       {
       }
      public static int BranchId;

      public List<tblItem_Code> GetAllGeneralItems(RMSDataContext Data)
      {
          if (Data == null) { Data = RMSDB.GetOject(); }
          return Data.tblItem_Codes.Where(i => i.itm_typ == "UG" && i.ct_id == "D").OrderBy(i => i.itm_dsc).ToList();
      }

      public List<tblItem_Code> GetAllFinishedItems(RMSDataContext Data)
      {
          if (Data == null) { Data = RMSDB.GetOject(); }
          return Data.tblItem_Codes.Where(i => i.itm_typ == "FG" && i.ct_id == "D").OrderBy(i => i.itm_dsc).ToList();
      }
      
      public string GetItemDescByItemCode(string itmCd, RMSDataContext Data)
      {
          if (Data == null) { Data = RMSDB.GetOject(); }
          return Data.tblItem_Codes.Where(i => i.itm_cd == itmCd).Single().itm_dsc;
      }

      public bool IsItemExists(string itmCd, RMSDataContext Data)
      {
          if (Data == null) { Data = RMSDB.GetOject(); }
          try
          {
             tblItem_Code itm =  Data.tblItem_Codes.Where(i => i.itm_cd == itmCd).Single();
              return true;
          }
          catch
          {
              return false;
          }
      }

      public List<spSrchItemResult> GetSearchItems(int brid, string itm, string itmtyp, int locid, RMSDataContext Data)
      {
          if (Data == null) { Data = RMSDB.GetOject(); }
          if (locid > 0)
          {
              return Data.spSrchItem(brid, itm, itmtyp, locid).Where(item=> item.stk_qty > 0).ToList();
          }
          else
              return Data.spSrchItem(brid, itm, itmtyp, null).ToList();
      }
/********************************************************************/
       public bool SaveItem(tblItem_Code itm, RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }
               Data.tblItem_Codes.InsertOnSubmit(itm);
               Data.SubmitChanges();
               return true;
           }
           catch
           {
               return false;
           }
       }
       //-----------------check if code or description already exist or not----

       public bool codeDescriptionExits(string itmCd, string itmDescription, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               List<tblItem_Code> ls = (from a in Data.tblItem_Codes
                                        where a.itm_cd == itmCd || a.itm_dsc == itmDescription
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
       //------------------------

       public bool checkIfDescriptionExist(string Desc,RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               List<tblItem_Code> ls = (from a in Data.tblItem_Codes
                                        where a.itm_dsc == Desc
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
           catch //(Exception ex)
           {
               //throw ex;
               return true;
           }
       }
       //---------------------------
       public bool updateItemCodeDet(tblItem_Code tblItmCd, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               tblItem_Code tbl = (from a in Data.tblItem_Codes
                                   where a.itm_cd == tblItmCd.itm_cd && a.br_id == tblItmCd.br_id
                                   select a).Single();

               tbl.itm_dsc = tblItmCd.itm_dsc;
               tbl.AltItem = tblItmCd.AltItem;
               tbl.uom_cd = tblItmCd.uom_cd;
               tbl.ItemSpecs = tblItmCd.ItemSpecs;
               tbl.BarCode = tblItmCd.BarCode;
               tbl.DrawingNo = tblItmCd.DrawingNo;
               tbl.Imp_Itm = tblItmCd.Imp_Itm;
               tbl.Exp_Days = tblItmCd.Exp_Days;
               tbl.Batch = tblItmCd.Batch;
               tbl.TaxID = tblItmCd.TaxID;

               tbl.Std_rate = tblItmCd.Std_rate;
               tbl.Avg_rate = tblItmCd.Avg_rate;
               tbl.Min_Level = tblItmCd.Min_Level;
               tbl.Max_Level = tblItmCd.Max_Level;
               tbl.reorder_qty = tblItmCd.reorder_qty;
               tbl.reorder_lvl = tblItmCd.reorder_lvl;
               tbl.Lead_Time = tblItmCd.Lead_Time;
               tbl.Exp_Days = tblItmCd.Exp_Days;
               tbl.itm_typ = tblItmCd.itm_typ;
               tbl.itm_grp_id = tblItmCd.itm_grp_id;
              
               Data.SubmitChanges();
               return true;
           }
           catch //(Exception ex)
           {
               //throw ex;
           }
           return false;
       }

       public bool updateItemCode(tblItem_Code tblItmCd, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               tblItem_Code tbl = (from a in Data.tblItem_Codes
                                   where a.itm_cd == tblItmCd.itm_cd && a.br_id == tblItmCd.br_id
                                   select a).Single();
               tbl.itm_dsc = tblItmCd.itm_dsc;
               tbl.itm_grp_id = tblItmCd.itm_grp_id;
               Data.SubmitChanges();
               
               return true;
           }
           catch //(Exception ex)
           {
               //throw ex;
           }
           return false;
       }

       public bool updateItemCode1(tblItem_Code tblItmCd, bool isGroup, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               tblItem_Code tbl = (from a in Data.tblItem_Codes
                                   where a.itm_cd == tblItmCd.itm_cd && a.br_id == tblItmCd.br_id
                                   select a).Single();
               tbl.itm_dsc = tblItmCd.itm_dsc;
               tbl.itm_grp_id = tblItmCd.itm_grp_id;
               if (!string.IsNullOrEmpty(tblItmCd.itm_pur_acc) || !string.IsNullOrEmpty(tblItmCd.itm_isu_acc))
               {
                   tbl.itm_pur_acc = tblItmCd.itm_pur_acc;
                   tbl.itm_isu_acc = tblItmCd.itm_isu_acc;
               }
               Data.SubmitChanges();
               if (isGroup)
               {

                   

                   List<tblItem_Code> lst = (from a in Data.tblItem_Codes
                                             where a.itm_cd != tblItmCd.itm_cd && a.itm_cd.StartsWith(tblItmCd.itm_cd) && a.br_id == tblItmCd.br_id
                                             select a).ToList();
                   if (lst.Count > 0)
                   {
                       foreach (var a in lst)
                       {
                           a.itm_grp_id = tblItmCd.itm_grp_id;
                           Data.SubmitChanges();
                       }
                   }
               }

               return true;
           }
           catch //(Exception ex)
           {
               //throw ex;
           }
           return false;
       }

       public tblItem_Code GetRecByID(string itmCode, int branchId, RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }
               tblItem_Code rec = (from a in Data.tblItem_Codes
                                   where a.itm_cd == itmCode && a.br_id == branchId
                                   select a).Single();
               return rec;
           }
           catch
           {
               return null;
           }
       }

       public List<spSrchGrid4ItemCdResult> GetCodeDetail4Grid(string itmtyp, string cd, string dsc, char typ, RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }
               List<spSrchGrid4ItemCdResult> lst = Data.spSrchGrid4ItemCd(cd, dsc, Convert.ToString(typ), itmtyp).ToList();
               return lst;
           }
           catch (Exception ex)
           {
               RMSDB.closeConn(Data);
               throw ex;
           }
       }

       public List<tblItem_Group> GetItemGroup(RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }
               List<tblItem_Group> lst = Data.tblItem_Groups.Where(grp => grp.status == true).ToList();
               return lst;
           }
           catch (Exception ex)
           {
               RMSDB.closeConn(Data);
               throw ex;
           }
       }

       public tblItem_Code GetItemCode(string itmCode, int brId, RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }

               return Data.tblItem_Codes.Where(itm => itm.itm_cd.Equals(itmCode) && itm.br_id.Equals(brId)).SingleOrDefault();
           }
           catch //(Exception ex)
           {
               //RMSDB.closeConn(Data);
               //throw ex;
           }
           return null;
       }

       public ItemCode_Type GetItemCodeType(char id, RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }

               return Data.ItemCode_Types.Where(typ => typ.ct_id.Equals(id)).SingleOrDefault();
           }
           catch //(Exception ex)
           {
               //RMSDB.closeConn(Data);
               //throw ex;
           }
           return null;
       }

       public string GetGroupCode(char ctid, int ItemCodeLength, RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }

               Int64 codes = Data.tblItem_Codes.Where(cd => cd.ct_id == Convert.ToString(ctid)).Max(cd => Convert.ToInt64(cd.itm_cd));
               return Convert.ToString(codes + 1).PadLeft(ItemCodeLength, '0');
           }
           catch //(Exception ex)
           {
               //RMSDB.closeConn(Data);
               //throw ex;
           }
           return Convert.ToString(1).PadLeft(ItemCodeLength, '0');
       }


       public string GetCode(string controlItemCode, char ctid, int ItemCodeLength, RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }

               Int64 codes = Data.tblItem_Codes.Where(cd => cd.ct_id == Convert.ToString(ctid) && cd.cnt_itm_cd == controlItemCode).Max(cd => Convert.ToInt64(cd.itm_cd));
               return Convert.ToString(codes + 1).PadLeft(ItemCodeLength, '0');
           }
           catch //(Exception ex)
           {
               //RMSDB.closeConn(Data);
               //throw ex;
           }
           return controlItemCode + Convert.ToString(1).PadLeft(ItemCodeLength - controlItemCode.Length, '0');
       }


       public IQueryable<ItemCode_Type> GetCodeType(RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }

               return Data.ItemCode_Types.AsQueryable();
           }
           catch (Exception ex)
           {
               RMSDB.closeConn(Data);
               throw ex;
           }
       }

       public int GetGroupCodeLength(RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }

               return Data.ItemCode_Types.Where(typ => typ.ct_typ == "Group" && typ.cnt_ct_id == null).Single().ct_len;
           }
           catch (Exception ex)
           {
               RMSDB.closeConn(Data);
               throw ex;
           }
       }

       

       public object GetCodeHead(string itmtyp, ItemCode_Type itmCodeTyp, RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }
               var obj = (from a in Data.tblItem_Codes
                          join b in Data.ItemCode_Types
                          on a.ct_id equals b.ct_id
                          where a.ct_id.Equals(itmCodeTyp.cnt_ct_id) && b.ct_id.Equals(itmCodeTyp.cnt_ct_id) //&& !a.itm_cd.StartsWith("1")
                          && a.itm_typ == itmtyp
                          select new
                          {
                              a.itm_cd,
                              itm_dsc = a.itm_cd + "-" + a.itm_dsc
                          }).ToList();
               return obj.OrderBy(o=> o.itm_cd);
           }
           catch (Exception ex)
           {
               RMSDB.closeConn(Data);
               throw ex;
           }
       }

       public object CodeVal(string typeid1, string typeid2, RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }

               ItemCode_Type codetype   = Data.ItemCode_Types.Where(typ => typ.ct_id.ToString().Equals(typeid1)).SingleOrDefault();
               ItemCode_Type codetype1  = Data.ItemCode_Types.Where(typ1 => typ1.ct_id.Equals(codetype.cnt_ct_id)).SingleOrDefault();

               var heads = (from t in Data.ItemCode_Types
                            join g in Data.tblItem_Codes on t.cnt_ct_id equals g.ct_id.ToString() into leftjoin
                            from g in leftjoin.DefaultIfEmpty()
                            where t.ct_id.ToString().Equals(typeid1) && g.itm_cd.Equals(typeid2) && g.br_id.Equals(BranchId)
                            select new
                            {
                                t.ct_len,
                                g.itm_cd,
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

       public List<spGetAllA_CResult> GetAllAcc(string code, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           return Data.spGetAllA_C(code).ToList();
       }


       public object GetUOM(RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }
               var obj = (from a in Data.Item_Uoms
                          select a).ToList();
               return obj;
           }
           catch (Exception ex)
           {
               RMSDB.closeConn(Data);
               throw ex;
           }
       }
    }
}
