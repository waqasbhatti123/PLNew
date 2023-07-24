using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.Linq;

namespace RMS.BL
{
   public class InvCode
    {
       public InvCode()
       {
       }


       public tblItem_Code GetRecByID(string itmCode, char itmType,  RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }
               tblItem_Code rec = (from a in Data.tblItem_Codes
                                   where a.itm_cd == itmCode && a.ct_id == itmType.ToString()
                                   select a).Single();
               return rec;
           }
           catch
           {
               return null;
           }
       }
       //----------------For Update-----------
       public bool updateGroup_Control(string itmCode, char itmType, string itmDesc, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               tblItem_Code tbl = (from a in Data.tblItem_Codes
                                   where a.itm_cd == itmCode && a.ct_id == itmType.ToString()
                                   select a).Single();
               tbl.itm_dsc = itmDesc;
               Data.SubmitChanges();
               return true;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       //----------------
       public bool updateItemD(string itmCode, char itmType,string altItem,string itmDesc,string itmSpecs,string barCode,byte uom_code,string DrawingNo,string ImpItmSts, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           try
           {
               tblItem_Code tbl = (from a in Data.tblItem_Codes
                                   where a.itm_cd == itmCode && a.ct_id == itmType.ToString()
                                   select a).Single();
               tbl.AltItem = altItem;
               tbl.itm_dsc = itmDesc;
               tbl.ItemSpecs = itmSpecs;
               tbl.BarCode = barCode;
               tbl.uom_cd = uom_code;
               tbl.DrawingNo = DrawingNo;
               tbl.Imp_Itm = ImpItmSts;
               Data.SubmitChanges();
               return true;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
//-----------------------


       public List<spSrchGrid4ItemCdResult> GetCodeDetail4Grid(string cd, string dsc, char typ, RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }
               List<spSrchGrid4ItemCdResult> lst = Data.spSrchGrid4ItemCd(cd, dsc, typ.ToString(),"UG").ToList();
               return lst;
           }
           catch (Exception ex)
           {
               RMSDB.closeConn(Data);
               throw ex;
           }
       }



       public bool CheckIfCodeExists(tblItem_Code itm, RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }
               List<tblItem_Code> lst = (from a in Data.tblItem_Codes
                                         where a.itm_cd == itm.itm_cd && a.ct_id == itm.ct_id
                                         select a).ToList();
               if (lst.Count > 0)
                   return true;
               else
                   return false;
           }
           catch(Exception ex)
           {
               RMSDB.closeConn(Data);
               throw ex;
           }
       }



       public bool CheckIfDscExists(tblItem_Code itm, RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }
               List<tblItem_Code> lst = (from a in Data.tblItem_Codes
                                         where a.itm_dsc == itm.itm_dsc && a.ct_id == itm.ct_id
                                         select a).ToList();
               if (lst.Count > 0)
                   return true;
               else
                   return false;
           }
           catch (Exception ex)
           {
               RMSDB.closeConn(Data);
               throw ex;
           }
       }




       public bool SaveItem(tblItem_Code itm,RMSDataContext Data)
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



       public bool UpdateItem(tblItem_Code itm, RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }
               Data.SubmitChanges();
               return true;
           }
           catch
           {
               return false;
           }
       }




       public int GetTypeCodeLength(char id, RMSDataContext Data)
       {
           if (Data == null) { Data = RMSDB.GetOject(); }
           int length =  (from a in Data.ItemCode_Types
                         where a.ct_id == id.ToString()
                         select a).Single().ct_len;
           return length;
       }



       public object GetGroups(char id, RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }
               var obj = (from a in Data.tblItem_Codes
                          join b in Data.ItemCode_Types
                          on a.ct_id equals b.ct_id
                          where a.ct_id == id.ToString() && b.cnt_ct_id == null && b.ct_id == id.ToString() //&& !a.itm_cd.StartsWith("1")
                          select new
                          {
                              a.itm_cd,
                              itm_dsc = a.itm_cd+"-"+a.itm_dsc
                          }).ToList();
               return obj.OrderBy(itm=> itm.itm_cd);
           }
           catch (Exception ex)
           {
               RMSDB.closeConn(Data); 
               throw ex;
           }
       }

      


       public object GetControlss(char id, RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }
               var obj = (from a in Data.tblItem_Codes
                          join b in Data.ItemCode_Types
                          on a.ct_id equals b.ct_id
                          where a.ct_id == id.ToString() && b.cnt_ct_id == "A" && b.ct_id == id.ToString() //&& !a.itm_cd.StartsWith("1")
                          select new
                          {
                              a.itm_cd,
                              itm_dsc = a.itm_cd + "-" + a.itm_dsc
                          }).ToList();
               return obj.OrderBy(itm=> itm.itm_cd);
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
               return Data.ItemCode_Types.Where(typ => typ.cnt_ct_id == null && typ.ct_typ == "Group").SingleOrDefault().ct_len;
           }
           catch (Exception ex)
           {
               RMSDB.closeConn(Data);
               throw ex;
           }
       }

       public string GetGroupPurchaseAccount(int brid, string itmcd, RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }
               return Data.tblItem_Codes.Where(cd => cd.br_id == brid && cd.itm_cd == itmcd).SingleOrDefault().itm_pur_acc;
           }
           catch (Exception ex)
           {
               RMSDB.closeConn(Data);
               throw ex;
           }
       }

       public string GetGroupIssueAccount(int brid, string itmcd, RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }
               return Data.tblItem_Codes.Where(cd => cd.br_id == brid && cd.itm_cd == itmcd).SingleOrDefault().itm_isu_acc;
           }
           catch (Exception ex)
           {
               RMSDB.closeConn(Data);
               throw ex;
           }
       }

       //---------------------------------
       public object GetControlsForItemReport(char id,string cntId, RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }
               var obj = (from a in Data.tblItem_Codes
                          join b in Data.ItemCode_Types
                          on a.ct_id equals b.ct_id
                          where a.ct_id == id.ToString() && b.cnt_ct_id == "A" && b.ct_id == id.ToString() //&& !a.itm_cd.StartsWith("1") 
                          && a.cnt_itm_cd==cntId
                          select new
                          {
                              a.itm_cd,
                              itm_dsc = a.itm_cd + "-" + a.itm_dsc
                          }).ToList();
               return obj;
           }
           catch (Exception ex)
           {
               RMSDB.closeConn(Data);
               throw ex;
           }
       }
       //-----------------------------------

       public object GetUOM(char id, RMSDataContext Data)
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
