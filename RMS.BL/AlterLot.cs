using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.Linq;

namespace RMS.BL
{
   public class AlterLot
    {
       public AlterLot()
       {
       }


       public bool SaveLot(int exisingLot, int newLot, RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }

               Data.spAlterLotNo(exisingLot, newLot);
               return true;
           }
           catch //(Exception ex)
           {
               //RMSDB.closeConn(Data);
              //throw ex;
           }
           return false;
       }


       public bool CheckMpnExist(int ltNo, RMSDataContext Data)
       {
           bool exist = false;
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }

               tblItemData rec = (from a in Data.tblItemDatas
                                  where a.LotNo == ltNo && a.vt_cd == 14 && a.vr_apr != "C"
                                  select a).Single();

               if (rec.PMN_Ref != "")
               {
                   exist = true;
               }
               return exist;
           }
           catch //(Exception ex)
           {
               //RMSDB.closeConn(Data);
              //throw ex;
           }
           return false;
       }


       public List<Anonymous4Grid> GetLotStatus(int LtNo, RMSDataContext Data)
       {
           try
           {
               if (Data == null) { Data = RMSDB.GetOject(); }

               List<Anonymous4Grid> itmList = (from a in Data.tblItemDatas
                                            where a.LotNo == LtNo && a.vt_cd == 14  && a.vr_apr != "C"
                                            select new Anonymous4Grid
                                            {
                                                strLotNo    =   a.LotNo.Value.ToString().Substring(0, 4) + "-" + a.LotNo.Value.ToString().Substring(4),
                                                IgpNo       =   a.IGPNo.Value.ToString().Substring(0, 4) + "/" + a.IGPNo.Value.ToString().Substring(4),

                                                Broker      =   (from b in Data.tblStkGPs
                                                                join c in Data.Glmf_Codes
                                                                on b.VendorId equals c.gl_cd
                                                                where b.vr_no == a.IGPNo && b.DocRef == a.DocRef && b.vt_cd == 11
                                                                select c).Single().gl_dsc,

                                                Product     =   (from d in Data.tblStkGPs
                                                                join e in Data.tblStkGPDets
                                                                on d.vr_no equals e.vr_no
                                                                join f in Data.tblItem_Codes
                                                                on e.Itm_cd equals f.itm_cd
                                                                where d.vr_no == a.IGPNo && d.DocRef == a.DocRef && d.vt_cd == 11 && e.vt_cd == 11
                                                                select f).Single().itm_dsc,

                                                LotQty      =   Convert.ToInt32((from g in Data.tblItemDataDets
                                                                where a.vt_cd == g.vt_cd && a.vr_no == g.vr_no && g.vt_cd == 14
                                                                select g.vr_qty).Sum()).ToString()

                                            }).ToList();

               return itmList;
           }
           catch (Exception ex)
           {
               RMSDB.closeConn(Data);
               throw ex;
           }
       }
    }
}
