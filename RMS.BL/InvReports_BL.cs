using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web;
using System.Data.Linq;
using System.Reflection;
namespace RMS.BL
{
    public partial class InvReports_BL
    {
        public InvReports_BL()
        {

        }
        public List<spPurchOrderSatusResult> GetPurchOrderStatus(int brid, string status, string orderby, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                if (status == "All")
                {
                    if (orderby == "item")
                        return Data.spPurchOrderSatus(brid).OrderBy(x=> x.itm_cd).ToList();
                    else if(orderby == "pono")
                        return Data.spPurchOrderSatus(brid).OrderBy(x=> x.vr_no).ToList();
                    else if (orderby == "vendor")
                        return Data.spPurchOrderSatus(brid).OrderBy(x=> x.gl_dsc).ToList();
                    else
                        return Data.spPurchOrderSatus(brid).ToList();
                }
                else
                {
                    if (orderby == "item")
                        return Data.spPurchOrderSatus(brid).Where(itm => itm.Status == status).OrderBy(x => x.itm_cd).ToList();
                    else if (orderby == "pono")
                        return Data.spPurchOrderSatus(brid).Where(itm => itm.Status == status).OrderBy(x => x.vr_no).ToList();
                    else if (orderby == "vendor")
                        return Data.spPurchOrderSatus(brid).Where(itm => itm.Status == status).OrderBy(x => x.gl_dsc).ToList();
                    else
                        return Data.spPurchOrderSatus(brid).Where(itm => itm.Status == status).ToList();
                }
            }
            catch { }
            return null;
        }


        public List<spItemLastPurchaseResult> GetItemLastPurchase(string primaryctrl, string orderby, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                if (primaryctrl == "0")
                {
                    if (orderby == "item")
                        return Data.spItemLastPurchase().OrderBy(x => x.itm_cd).ToList();
                    else if (orderby == "pono")
                        return Data.spItemLastPurchase().OrderBy(x => x.vr_no).ToList();
                    else if (orderby == "vendor")
                        return Data.spItemLastPurchase().OrderBy(x => x.gl_dsc).ToList();
                    else
                        return Data.spItemLastPurchase().OrderBy(x => x.itm_cd).ToList();
                }
                else
                {
                    if (orderby == "item")
                        return Data.spItemLastPurchase().Where(itm=> itm.itm_cd.StartsWith(primaryctrl)).OrderBy(x => x.itm_cd).ToList();
                    else if (orderby == "pono")
                        return Data.spItemLastPurchase().Where(itm => itm.itm_cd.StartsWith(primaryctrl)).OrderBy(x => x.vr_no).ToList();
                    else if (orderby == "vendor")
                        return Data.spItemLastPurchase().Where(itm => itm.itm_cd.StartsWith(primaryctrl)).OrderBy(x => x.gl_dsc).ToList();
                    else
                        return Data.spItemLastPurchase().Where(itm => itm.itm_cd.StartsWith(primaryctrl)).OrderBy(x => x.itm_cd).ToList();
                }
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }
        public List<spSalesTaxItemsResult> GetSalesTaxItems(int brid,int loc_id,string itmgrp, string party, DateTime fromDt, DateTime toDt, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                if (itmgrp == "0")
                   return Data.spSalesTaxItems(brid,loc_id, party,fromDt,toDt).ToList();
                else
                    return Data.spSalesTaxItems(brid, loc_id, party, fromDt, toDt).Where(itm=>itm.itm_cd.StartsWith(itmgrp)).ToList();
               
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

        public List<spPartywiseSalesTaxResult> GetPartyWiseSalesTax(int brid, int loc_id,  DateTime fromDt, DateTime toDt, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                    return Data.spPartywiseSalesTax(brid, loc_id, fromDt, toDt).ToList();

            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }


        public object GetItemGroup(int brid, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                var obj = from a in Data.tblItem_Codes
                          where a.br_id == brid && a.ct_id == "A" && a.status == "A" && a.itm_typ == "UG"
                          orderby a.itm_cd
                          select new
                          {
                              a.itm_cd,
                              itm_dsc = a.itm_cd + " - "+ a.itm_dsc
                          };
                return obj.ToList();

            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }


        public List<Depart_ment> GetDeptarments(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                List<Depart_ment> lst = (from a in Data.Depart_ments
                                         select a).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }


        public List<spDeptWiseConsumptionResult> GetDeptConsumptionRecs(int br_id, string itmgrp, int loc_id, int deptId, int month, int year, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                if(itmgrp == "0")
                    return Data.spDeptWiseConsumption(loc_id, deptId, br_id, month, year).ToList();
                else
                     return Data.spDeptWiseConsumption(loc_id, deptId, br_id, month, year).Where(itm=> itm.itm_cd.StartsWith(itmgrp)).ToList();
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }
        //-----------
        public List<spDeptWiseConsumptionDetResult> getDeptConsumptionDetail(int br_id,string itmgrp, int loc_id, int deptId, int month, int year, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                if(itmgrp == "0")
                     return Data.spDeptWiseConsumptionDet(loc_id, deptId, br_id, month, year).ToList();
                else
                    return Data.spDeptWiseConsumptionDet(loc_id, deptId, br_id, month, year).Where(itm => itm.itm_cd.StartsWith(itmgrp)).ToList();
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

        public List<spDeptWiseConsumptionDetResult> getDeptConsumptionDetailOfCC(int br_id,string itmgrp, int loc_id, int deptId, int month, int year, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                if(itmgrp == "0")
                    return Data.spDeptWiseConsumptionDet(loc_id, deptId, br_id, month, year).Where(c=> !string.IsNullOrEmpty(c.cc_nme)).ToList();
                else
                    return Data.spDeptWiseConsumptionDet(loc_id, deptId, br_id, month, year).Where(c => !string.IsNullOrEmpty(c.cc_nme)).Where(itm => itm.itm_cd.StartsWith(itmgrp)).ToList();
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }
       
        //------------------------


        //public List<spRawHideStockResult> GetRawHideStockLst(DateTime fromDt, DateTime toDt, RMSDataContext Data)
        //{
        //    if (Data == null) {Data = RMSDB.GetOject(); }

        //    try
        //    {
        //        List<spRawHideStockResult> list = Data.spRawHideStock(fromDt, toDt).ToList();
        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.closeConn(Data); throw ex;
        //    }
        //}
        //===========================
        public List<spRawHideStockByStatusResult> GetRawHideStockLstByStatus(DateTime fromDt, DateTime toDt,string status, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                List<spRawHideStockByStatusResult> list = Data.spRawHideStockByStatus(fromDt, toDt, status).ToList();
                return list;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }



        public List<spRawHideLotRptResult> GetRawHideLotData(DateTime fromDt, DateTime toDt, char status, string sortBy, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                List<spRawHideLotRptResult> list = Data.spRawHideLotRpt(fromDt, toDt, status.ToString(), sortBy).ToList();
                return list;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

//===========================================================




        //public List<spWetBlueStockRptResult> GetWetBlueStock(DateTime fromDt, DateTime toDt, RMSDataContext Data)
        //{
        //    if (Data == null) { Data = RMSDB.GetOject(); }

        //    try
        //    {
        //        List<spWetBlueStockRptResult> list = Data.spWetBlueStockRpt(fromDt, toDt).ToList();
        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.closeConn(Data); throw ex;
        //    }
        //}

        //-------------------------------

        //public List<sp_StkStatusResult> GetWetBlueStkStatus(DateTime fromDt, DateTime toDt, RMSDataContext Data)
        //{
        //    if (Data == null) { Data = RMSDB.GetOject(); }
        //    int br_id=1;
        //    decimal loc_id = 2;
        //    try
        //    {
        //        List<sp_StkStatusResult> list = Data.sp_StkStatus(br_id, loc_id, fromDt, toDt).ToList();
        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.closeConn(Data); throw ex;
        //    }
        //}


        public List<sp_InvLedgerResult> GetInvenStockLedger(int br_id, string itmprimaryctrl, decimal loc_id, DateTime fromDt, DateTime toDt, string codeFrm, string codeTo, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                List<sp_InvLedgerResult> list = null;

                if (codeFrm == "" && codeTo == "")
                {
                    if (itmprimaryctrl == "0")
                        list = Data.sp_InvLedger(br_id, loc_id, fromDt, toDt).ToList();
                    else
                        list = Data.sp_InvLedger(br_id, loc_id, fromDt, toDt).Where(itm => itm.itm_cd.StartsWith(itmprimaryctrl)).ToList();
                }
                else if (codeFrm != "" && codeTo == "")
                {
                    if (itmprimaryctrl == "0")
                        list = Data.sp_InvLedger(br_id, loc_id, fromDt, toDt).Where(itm => Convert.ToDouble(itm.itm_cd) >= Convert.ToDouble(codeFrm)).ToList();
                    else
                        list = Data.sp_InvLedger(br_id, loc_id, fromDt, toDt).Where(itm => itm.itm_cd.StartsWith(itmprimaryctrl) && Convert.ToDouble(itm.itm_cd) >= Convert.ToDouble(codeFrm)).ToList();
                }
                else if (codeFrm == "" && codeTo != "")
                {
                    if (itmprimaryctrl == "0")
                        list = Data.sp_InvLedger(br_id, loc_id, fromDt, toDt).Where(itm => Convert.ToDouble(itm.itm_cd) <= Convert.ToDouble(codeTo)).ToList();
                    else
                        list = Data.sp_InvLedger(br_id, loc_id, fromDt, toDt).Where(itm => itm.itm_cd.StartsWith(itmprimaryctrl) && Convert.ToDouble(itm.itm_cd) >= Convert.ToDouble(codeTo)).ToList();
                }
                else if (codeFrm != "" && codeTo != "")
                {
                    if (itmprimaryctrl == "0")
                        list = Data.sp_InvLedger(br_id, loc_id, fromDt, toDt).Where(itm => Convert.ToDouble(itm.itm_cd) >= Convert.ToDouble(codeFrm) && Convert.ToDouble(itm.itm_cd) <= Convert.ToDouble(codeTo)).ToList();
                    else
                        list = Data.sp_InvLedger(br_id, loc_id, fromDt, toDt).Where(itm => itm.itm_cd.StartsWith(itmprimaryctrl) && Convert.ToDouble(itm.itm_cd) >= Convert.ToDouble(codeFrm) && Convert.ToDouble(itm.itm_cd) <= Convert.ToDouble(codeTo)).ToList();
                }
                else
                {
                    list = Data.sp_InvLedger(br_id, loc_id, fromDt, toDt).ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

        public List<ItemsMovementResult> GetSlowMovingItems(string deadstkdays, int br_id,string itmgrp, int loc_id, DateTime toDt, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                if (string.IsNullOrEmpty(deadstkdays) && itmgrp =="0" )
                    return Data.ItemsMovement(br_id, loc_id, toDt).ToList();
                else
                    return Data.ItemsMovement(br_id, loc_id, toDt).Where(itm => itm.dead_stk_days >= Convert.ToInt32(deadstkdays) && itm.itm_cd.StartsWith(itmgrp)).ToList();
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

        public List<spCriticalItemsResult> GetCriticalItems(int type, int br_id,string itmgrp, int loc_id, DateTime frmDt, DateTime toDt, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                List<spCriticalItemsResult> list = null;
                if (type == 0 && itmgrp == "0")
                {
                    list = Data.spCriticalItems(br_id, loc_id, frmDt, toDt).Where(itm => itm.stk_on_hand < itm.min_lvl).ToList();
                }
                else
                {
                    list = Data.spCriticalItems(br_id, loc_id, frmDt, toDt).Where(itm => itm.stk_on_hand > itm.max_lvl && itm.itm_cd.StartsWith(itmgrp)).ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

        public List<tblStock_Loc> GetStockLoc(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var lst = from l in Data.tblStock_Locs
                          where !l.LocCategory.Equals("R") && !l.LocCode.Equals("FG")
                          orderby l.LocName
                          select l;
                return lst.ToList();
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }

        public List<sp_InvStatusResult> GetInvenStockStatus(int br_id, string itmprimaryctrl, decimal loc_id, DateTime fromDt, DateTime toDt, string codeFrm, string codeTo, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                List<sp_InvStatusResult> list = null;


                if (codeFrm == "" && codeTo == "")
                {
                    if (itmprimaryctrl == "0")
                        list = Data.sp_InvStatus(br_id, loc_id, fromDt, toDt).ToList();
                    else
                        list = Data.sp_InvStatus(br_id, loc_id, fromDt, toDt).Where(itm => itm.itm_cd.StartsWith(itmprimaryctrl)).ToList();
                }
                else if (codeFrm != "" && codeTo == "")
                {
                    if (itmprimaryctrl == "0")
                        list = Data.sp_InvStatus(br_id, loc_id, fromDt, toDt).Where(itm => Convert.ToDouble(itm.itm_cd) >= Convert.ToDouble(codeFrm)).ToList();
                    else
                        list = Data.sp_InvStatus(br_id, loc_id, fromDt, toDt).Where(itm => itm.itm_cd.StartsWith(itmprimaryctrl) && Convert.ToDouble(itm.itm_cd) >= Convert.ToDouble(codeFrm)).ToList();
                }
                else if (codeFrm == "" && codeTo != "")
                {
                    if (itmprimaryctrl == "0")
                        list = Data.sp_InvStatus(br_id, loc_id, fromDt, toDt).Where(itm => Convert.ToDouble(itm.itm_cd) <= Convert.ToDouble(codeTo)).ToList();
                    else
                        list = Data.sp_InvStatus(br_id, loc_id, fromDt, toDt).Where(itm => itm.itm_cd.StartsWith(itmprimaryctrl) && Convert.ToDouble(itm.itm_cd) >= Convert.ToDouble(codeTo)).ToList();
                }
                else if (codeFrm != "" && codeTo != "")
                {
                    if (itmprimaryctrl == "0")
                        list = Data.sp_InvStatus(br_id, loc_id, fromDt, toDt).Where(itm => Convert.ToDouble(itm.itm_cd) >= Convert.ToDouble(codeFrm) && Convert.ToDouble(itm.itm_cd) <= Convert.ToDouble(codeTo)).ToList();
                    else
                        list = Data.sp_InvStatus(br_id, loc_id, fromDt, toDt).Where(itm => itm.itm_cd.StartsWith(itmprimaryctrl) && Convert.ToDouble(itm.itm_cd) >= Convert.ToDouble(codeFrm) && Convert.ToDouble(itm.itm_cd) <= Convert.ToDouble(codeTo)).ToList();

                }
                else
                {
                    list = Data.sp_InvStatus(br_id, loc_id, fromDt, toDt).Where(itm => itm.itm_cd.StartsWith(itmprimaryctrl)).ToList();
                }
                
                return list;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }
        
      //-------------------
        public List<sp_InvMonthlyStatusResult> getItemWiseStkMovementSummary(int br_id, string itmgroup, decimal loc_id, DateTime fromDt, DateTime toDt, string codeFrm, string codeTo,  RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                List<sp_InvMonthlyStatusResult> list = null;

                if (codeFrm == "" && codeTo == "")
                {
                    if (itmgroup == "0")
                        list = Data.sp_InvMonthlyStatus(br_id, loc_id, fromDt, toDt).ToList();
                    else
                        list = Data.sp_InvMonthlyStatus(br_id, loc_id, fromDt, toDt).Where(itm => itm.itm_cd.StartsWith(itmgroup)).ToList();
                }
                else if (codeFrm != "" && codeTo == "")
                {
                    if (itmgroup == "0")
                        list = Data.sp_InvMonthlyStatus(br_id, loc_id, fromDt, toDt).Where(itm => Convert.ToDouble(itm.itm_cd) >= Convert.ToDouble(codeFrm)).ToList();
                    else
                        list = Data.sp_InvMonthlyStatus(br_id, loc_id, fromDt, toDt).Where(itm => itm.itm_cd.StartsWith(itmgroup) && Convert.ToDouble(itm.itm_cd) >= Convert.ToDouble(codeFrm)).ToList();
                }
                else if (codeFrm == "" && codeTo != "")
                {
                    if (itmgroup == "0")
                        list = Data.sp_InvMonthlyStatus(br_id, loc_id, fromDt, toDt).Where(itm => Convert.ToDouble(itm.itm_cd) <= Convert.ToDouble(codeTo)).ToList();
                    else
                        list = Data.sp_InvMonthlyStatus(br_id, loc_id, fromDt, toDt).Where(itm => itm.itm_cd.StartsWith(itmgroup) && Convert.ToDouble(itm.itm_cd) <= Convert.ToDouble(codeTo)).ToList();
                }
                else if (codeFrm != "" && codeTo != "")
                {
                    if (itmgroup == "0")
                        list = Data.sp_InvMonthlyStatus(br_id, loc_id, fromDt, toDt).Where(itm => Convert.ToDouble(itm.itm_cd) >= Convert.ToDouble(codeFrm) && Convert.ToDouble(itm.itm_cd) <= Convert.ToDouble(codeTo)).ToList();
                    else
                        list = Data.sp_InvMonthlyStatus(br_id, loc_id, fromDt, toDt).Where(itm => itm.itm_cd.StartsWith(itmgroup)).ToList();
                }
                else
                {
                    list = Data.sp_InvMonthlyStatus(br_id, loc_id, fromDt, toDt).Where(itm => itm.itm_cd.StartsWith(itmgroup)).ToList();
                }

                return list;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }
        //----------------

        //public List<> GetInvenStockStatus(DateTime fromDt, DateTime toDt, RMSDataContext Data)
        //{
        //    if (Data == null) { Data = RMSDB.GetOject(); }
        //    int br_id = 1;
        //    decimal loc_id = 2;
        //    try
        //    {
        //        List<sp_InvStatusResult> list = Data.sp_InvStatus(br_id, loc_id, fromDt, toDt).ToList();
        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.closeConn(Data); throw ex;
        //    }
        //}

        //-------------------------------
        public List<spRHPartyPayablesResult> GetPartyPayables(DateTime fromDt, DateTime toDt, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                List<spRHPartyPayablesResult> list = Data.spRHPartyPayables(fromDt, toDt).ToList();
                return list;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }
        //---------------------------

        public List<spAllIGPViewResult> GetIGPReport(DateTime fromDt, DateTime toDt, String status, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                List<spAllIGPViewResult> list = Data.spAllIGPView(fromDt, toDt, status).ToList();
                return list;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }
        //-----------------------------
        public List<spRawHidePurchByYearResult> GetRawHidesPurchBookByYear(string year, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                List<spRawHidePurchByYearResult> list = Data.spRawHidePurchByYear(year).ToList();
                return list;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }
        //----------------

        public List<spRawHidePurchByMonthlyResult> GetRawHidesPurchBookByMonthly(string year,string month, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                List<spRawHidePurchByMonthlyResult> list = Data.spRawHidePurchByMonthly(year,month).ToList();
                return list;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

        public List<spRawHidePurchByYrVendorResult> GetRawHidesPurchBookByYrVendor(string year, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                List<spRawHidePurchByYrVendorResult> list = Data.spRawHidePurchByYrVendor(year).ToList();
                return list;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }
        //-------------
        public List<spRawHidePurchaseByMonthVendorResult> GetRawHidesPurchBookByMonthVendor(string year,string month, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                List<spRawHidePurchaseByMonthVendorResult> list = Data.spRawHidePurchaseByMonthVendor(year,month).ToList();
                return list;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }
        //------------------------
        public List<spRawHidePurchByVendorWholeYearResult> GetRawHidesPurchBookByVendorWholeYear(string year, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                List<spRawHidePurchByVendorWholeYearResult> list = Data.spRawHidePurchByVendorWholeYear(year).ToList();
                return list;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

        public List<spRawHidePurchByMonthResult> GetRawHidesPurchBook(string year, string month, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                List<spRawHidePurchByMonthResult> list = Data.spRawHidePurchByMonth(year, month).ToList();
                return list;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

        //-----------------------
        public List<sp_Inv_ItemWise_Purch_RptResult> GetStkPurchReport(int br_id, string itmgrp, decimal loc_id, string tp, string codeFrm, string codeTo, DateTime frmDt, DateTime toDt, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                List<sp_Inv_ItemWise_Purch_RptResult> list = null;

                if (codeFrm == "" && codeTo == "")
                {
                    if (itmgrp == "0")
                        list = Data.sp_Inv_ItemWise_Purch_Rpt(br_id, loc_id, frmDt, toDt, tp).ToList();
                    else
                        list = Data.sp_Inv_ItemWise_Purch_Rpt(br_id, loc_id, frmDt, toDt, tp).Where(itm => itm.ItemCode.StartsWith(itmgrp)).ToList();
                }
                else if (codeFrm != "" && codeTo == "")
                {
                    if (itmgrp == "0")
                        list = Data.sp_Inv_ItemWise_Purch_Rpt(br_id, loc_id, frmDt, toDt, tp).Where(itm => Convert.ToDouble(itm.ItemCode) >= Convert.ToDouble(codeFrm)).ToList();
                    else
                        list = Data.sp_Inv_ItemWise_Purch_Rpt(br_id, loc_id, frmDt, toDt, tp).Where(itm => itm.ItemCode.StartsWith(itmgrp) && Convert.ToDouble(itm.ItemCode) >= Convert.ToDouble(codeFrm)).ToList();
                }
                else if (codeFrm == "" && codeTo != "")
                {
                    if (itmgrp == "0")
                        list = Data.sp_Inv_ItemWise_Purch_Rpt(br_id, loc_id, frmDt, toDt, tp).Where(itm => Convert.ToDouble(itm.ItemCode) <= Convert.ToDouble(codeTo)).ToList();
                    else
                        list = Data.sp_Inv_ItemWise_Purch_Rpt(br_id, loc_id, frmDt, toDt, tp).Where(itm => itm.ItemCode.StartsWith(itmgrp) && Convert.ToDouble(itm.ItemCode) >= Convert.ToDouble(codeTo)).ToList();
                   
                }
                else if (codeFrm != "" && codeTo != "")
                {
                    if (itmgrp == "0")
                        list = Data.sp_Inv_ItemWise_Purch_Rpt(br_id, loc_id, frmDt, toDt, tp).Where(itm => Convert.ToDouble(itm.ItemCode) >= Convert.ToDouble(codeFrm) && Convert.ToDouble(itm.ItemCode) <= Convert.ToDouble(codeTo)).ToList();
                    else
                        list = Data.sp_Inv_ItemWise_Purch_Rpt(br_id, loc_id, frmDt, toDt, tp).Where(itm => itm.ItemCode.StartsWith(itmgrp) && Convert.ToDouble(itm.ItemCode) >= Convert.ToDouble(codeFrm) && Convert.ToDouble(itm.ItemCode) <= Convert.ToDouble(codeTo)).ToList();
                   
                }
                else
                {
                    list = Data.sp_Inv_ItemWise_Purch_Rpt(br_id, loc_id, frmDt, toDt, tp).Where(itm => itm.ItemCode.StartsWith(itmgrp)).ToList();
                }

                return list;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }
        //----------------------------

        public List<spWetBlueInProcessRptResult> GetWetBlueSelectionInProcess(DateTime fromDt, DateTime toDt, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                List<spWetBlueInProcessRptResult> list = Data.spWetBlueInProcessRpt(fromDt, toDt).ToList();
                return list;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }
        //=======================
        public List<spWetBlueInProcStatusRptResult> GetWetBlueSelectionByStatus(DateTime fromDt, DateTime toDt,string sts, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                List<spWetBlueInProcStatusRptResult> list = Data.spWetBlueInProcStatusRpt(fromDt, toDt, sts).ToList();
                return list;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

        //=======================
        //public List<spComparativeStkStatusRptResult> getComparativeStockStatusRpt(string year, RMSDataContext Data)
        //{
        //    if (Data == null) { Data = RMSDB.GetOject(); }

        //    try
        //    {
        //        List<spComparativeStkStatusRptResult> list = Data.spComparativeStkStatusRpt(year).ToList();
        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.closeConn(Data); throw ex;
        //    }

        //}
        ////--------------
        public List<sp_Inv_Purch_RptResult> getInventoryPurchRpt(string party, int br_id, string itmgrp, decimal loc_id, string codeFrm, string codeTo, DateTime frmdt, DateTime todt, RMSDataContext Data)
        {
            
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                List<sp_Inv_Purch_RptResult> list = null;

                if (codeFrm == "" && codeTo == "")
                {
                    if (itmgrp == "0")
                        list = Data.sp_Inv_Purch_Rpt(br_id, loc_id,frmdt, todt, "", party).ToList();
                    else
                        list = Data.sp_Inv_Purch_Rpt(br_id, loc_id, frmdt, todt, "", party).Where(itm => itm.ItemCode.StartsWith(itmgrp)).ToList();
                }
                else if (codeFrm != "" && codeTo == "")
                {
                    if (itmgrp == "0")
                        list = Data.sp_Inv_Purch_Rpt(br_id, loc_id, frmdt, todt, "", party).Where(itm => Convert.ToDouble(itm.ItemCode) >= Convert.ToDouble(codeFrm)).ToList();
                    else
                        list = Data.sp_Inv_Purch_Rpt(br_id, loc_id, frmdt, todt, "", party).Where(itm => itm.ItemCode.StartsWith(itmgrp) && Convert.ToDouble(itm.ItemCode) >= Convert.ToDouble(codeFrm)).ToList().ToList();

                }
                else if (codeFrm == "" && codeTo != "")
                {
                    if (itmgrp == "0")
                        list = Data.sp_Inv_Purch_Rpt(br_id, loc_id, frmdt, todt, "", party).Where(itm => Convert.ToDouble(itm.ItemCode) <= Convert.ToDouble(codeTo)).ToList();
                    else
                        list = Data.sp_Inv_Purch_Rpt(br_id, loc_id, frmdt, todt, "", party).Where(itm => itm.ItemCode.StartsWith(itmgrp) && Convert.ToDouble(itm.ItemCode) >= Convert.ToDouble(codeTo)).ToList();

                }
                else if (codeFrm != "" && codeTo != "")
                {
                    if (itmgrp == "0")
                        list = Data.sp_Inv_Purch_Rpt(br_id, loc_id, frmdt, todt, "", party).Where(itm => Convert.ToDouble(itm.ItemCode) >= Convert.ToDouble(codeFrm) && Convert.ToDouble(itm.ItemCode) <= Convert.ToDouble(codeTo)).ToList();
                    else
                        list = Data.sp_Inv_Purch_Rpt(br_id, loc_id, frmdt, todt, "", party).Where(itm => itm.ItemCode.StartsWith(itmgrp) && Convert.ToDouble(itm.ItemCode) >= Convert.ToDouble(codeFrm) && Convert.ToDouble(itm.ItemCode) <= Convert.ToDouble(codeTo)).ToList();

                }
                else
                {
                    list = Data.sp_Inv_Purch_Rpt(br_id, loc_id, frmdt, todt, "", party).Where(itm => itm.ItemCode.StartsWith(itmgrp)).ToList();
                }

               
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //------------------------
        public List<spInv_PurchSubRptResult> getInventoryPurchSubRpt(int vrId, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                List<spInv_PurchSubRptResult> list = Data.spInv_PurchSubRpt(vrId).ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //-------------------

        public List<spMonthlyDemandRptResult> getMonthlyDemand(string itmgrp, string year, string month, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                List<spMonthlyDemandRptResult> list = null;
                if(itmgrp == "0") 
                     list = Data.spMonthlyDemandRpt(year, month).ToList();
                else
                    list = Data.spMonthlyDemandRpt(year, month).Where(itm => itm.itm_cd.StartsWith(itmgrp)).ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //-------------------

        public List<spItemMasterListReportResult> getItemMasterListReportData(string gpCd, string cntrlCd, int itmGrpId, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                return Data.spItemMasterListReport(gpCd, cntrlCd, itmGrpId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<spPartyWiseUnallocatedFundsResult> getUnallocatedFunds(string val, int brid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                List<spPartyWiseUnallocatedFundsResult> list = Data.spPartyWiseUnallocatedFunds(brid, val).ToList();
                return list;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }

        }
        //public List<spPartyWiseUnpaidInvoicesResult> getUnpaidInvoice(string val, int brid, RMSDataContext Data)
        //{
        //    if (Data == null) { Data = RMSDB.GetOject(); }

        //    try
        //    {
        //        List<spPartyWiseUnpaidInvoicesResult> list = Data.spPartyWiseUnpaidInvoices(brid, val).ToList();
        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.closeConn(Data); throw ex;
        //    }

        //}

        public List<string> GetDetailAccount(string itm_cd, RMSDataContext Data)
        {

            return (from u in Data.tblItem_Codes
                    where (u.itm_cd.StartsWith(itm_cd) || u.itm_cd.StartsWith(itm_cd)) && u.itm_typ == "UG" && u.ct_id == "D"
                    orderby u.itm_cd
                    select u.itm_cd + ' ' + u.itm_dsc).Take(5).ToList();
            //orderby u.gl_cd // optional but it'll look nicer




        }
        //---------------------
    }
}
