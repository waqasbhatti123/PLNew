using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
//using System.Transactions;


namespace RMS.BL
{
    public class RetailerVisitsBL
    {
        public RetailerVisitsBL()
        {
             
        }


       
        #region Retailer Visits

        public string GetDocNo(DateTime dtTime, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            try
            {
                List<int> lst = new List<int>();
                FIN_PERD fin = (from a in Data.FIN_PERDs
                                where a.Start_Date <= dtTime && a.End_Date >= dtTime
                                select a).Single();
                string finYear = fin.Gl_Year.ToString();



                //var records = from n in Data.tblSdVisits
                //              //where n.LocId == 5
                //              select n;
                //foreach (var rec in records)
                //{
                //    string year = rec.doc_no.ToString().Substring(0, 4);
                //    if (year == finYear)// && rec.vt_cd == code)
                //    {
                //        lst.Add(Convert.ToInt32(rec.doc_no.ToString().Substring(4)));
                //    }
                //}
                if (lst.Count > 0)
                {
                    return finYear + "/" + Convert.ToInt32(lst.Max() + 1);
                }
                else
                {
                    return finYear + "/1";
                }
            }
            catch //(Exception ex)
            {
                //RMSDB.closeConn(Data); throw ex;
            }
            return "";
        }

        #endregion

        public void Update(RMSDataContext Data)
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

        //public string InsertVisitData(tblSdVisit visitData, RMSDataContext Data)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = RMSDB.GetOject(); }
        //        Data.tblSdVisits.InsertOnSubmit(visitData);
        //        Data.SubmitChanges();

        //        return "Done";
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }
        //}

        //public string UpdateVisitData(tblSdVisit visitData, EntitySet<tblSdVisitDet> visitDataDets, RMSDataContext Data)
        //{
        //    if (Data == null) { Data = RMSDB.GetOject(); }
        //    /*OPEN AND BEGIN TRANS*/
        //    System.Data.Common.DbTransaction trans = null;
        //    if (Data.Connection.State != ConnectionState.Open)
        //    {
        //        Data.Connection.Open();
        //    }
        //    trans = Data.Connection.BeginTransaction();
        //    Data.Transaction = trans;


        //    try
        //    {
        //        var dets = from p in Data.tblSdVisitDets
        //                   where p.vr_id == visitData.vr_id
        //                   select p;

        //        Data.tblSdVisitDets.DeleteAllOnSubmit(dets);

        //        visitData.tblSdVisitDets = visitDataDets;
                
        //        Data.SubmitChanges();

        //        /*COMMIT*/
        //        trans.Commit();

        //        return "Done";
        //    }
        //    catch (Exception ex)
        //    {
        //        /*ROLL BACK*/
        //        if (trans != null)
        //            trans.Rollback();

        //        return ex.Message;
        //    }


        //}

        //public tblSdVisit GetVisitDataByID(int vrID, RMSDataContext Data)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = RMSDB.GetOject(); }

        //        tblSdVisit tbl = (from a in Data.tblSdVisits
        //                            where a.vr_id == vrID
        //                            select a).SingleOrDefault();

        //        return tbl;
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }
        //}

        public object GetAllVisitData(int docNo,int salesPersonId, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                var visitData = "";
                   // from visitD in Data.tblSdVisits
                //               join salesP in Data.tblSalesPersons on visitD.sale_person_id equals salesP.ID
                //               where visitD.doc_no == (docNo == 0 ? visitD.doc_no : docNo)
                //               && (visitD.sale_person_id == (salesPersonId == 0 ? visitD.sale_person_id : salesPersonId))
                //               && visitD.status==true
                //               select new
                //               {
                //                   visitD.vr_id,
                //                   visitD.doc_no,
                //                   visitD.doc_dt,
                //                   visitD.sale_person_id,
                //                   visitD.sch_dt,
                //                   salesP.SalesPerson
                                   
                                   
                //               };
                return visitData;

            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }


        //public List<tblSdVisitDet> GetAllVisitDetailByVisitDataID(int vrID, RMSDataContext Data)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = RMSDB.GetOject(); }

        //        List<tblSdVisitDet> lsVisitDet = (from a in Data.tblSdVisitDets
        //                                                where a.vr_id == vrID
        //                                                select a).ToList();
        //        return lsVisitDet;

        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }
        //}

        //public string GetLastVisitDateByShopID(int retailerID, RMSDataContext Data)
        //{
        //    try
        //    {
        //        string lastVisitDate;
        //        if (Data == null) { Data = RMSDB.GetOject(); }

        //        List<tblSdVisit> lsVisits = (from a in Data.tblSdVisits
        //                                          join b in Data.tblSdVisitDets on a.vr_id equals b.vr_id
        //                                          where b.shop_id == retailerID
        //                                          orderby a.sch_dt descending
        //                                          select a).ToList();
        //        if (lsVisits.Count>0)
        //        {
        //            lastVisitDate = lsVisits.FirstOrDefault().sch_dt.ToString("dd-MMM-yyyy");
        //        }
        //        else
        //        {
        //            lastVisitDate = string.Empty;
        //        }

        //        return lastVisitDate;
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }
        //}

        //// Reports

        //public List<spVisitPlanRptResult> GetVisitPlanReport(string area,string subArea,int salesPerson,DateTime fromDate,DateTime toDate, string sortBy, RMSDataContext Data)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = RMSDB.GetOject(); }

        //        return Data.spVisitPlanRpt(area,subArea,salesPerson,fromDate,toDate,sortBy).ToList();

        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }
        //}
    }
}