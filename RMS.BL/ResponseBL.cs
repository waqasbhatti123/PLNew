using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
//using System.Transactions;


namespace RMS.BL
{
    public class ResponseBL
    {
        public ResponseBL()
        {

        }


        #region Response Type

        public List<tblSdPromRespType> GetAllResponseTypes(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                List<tblSdPromRespType> responseTypes = (from respType in Data.tblSdPromRespTypes
                                                         orderby respType.Desc
                                                         select respType).ToList();
                return responseTypes;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public tblSdPromRespType GetResponseTypeByID(int responseTypeId, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                //Data = RMSDB.GetOject();
                tblSdPromRespType responseType = Data.tblSdPromRespTypes.Single(p => p.RespTypeId == responseTypeId);
                //Data.Dispose();

                return responseType;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public bool ISAlreadyExistResponseType(tblSdPromRespType responseType, RMSDataContext Data)
        {

            bool isAlready = false;
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                IQueryable<tblSdPromRespType> responseTypes = from respt in Data.tblSdPromRespTypes
                                                              where respt.RespTypeId != responseType.RespTypeId && respt.Desc == responseType.Desc
                                                              select respt;

                if (responseTypes != null && responseTypes.Count<tblSdPromRespType>() > 0)
                {
                    isAlready = true;

                }

                return isAlready;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public void InsertResponseType(tblSdPromRespType responseType, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.tblSdPromRespTypes.InsertOnSubmit(responseType);
                Data.SubmitChanges();

            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        #endregion


        #region Response

        public List<tblSdPromResp> GetAllResponsesByRespTypeID(int respTypeID,RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                List<tblSdPromResp> lstResponses = (from resp in Data.tblSdPromResps
                                                    where resp.RespTypeId==respTypeID && resp.Enabled==true
                                                    select resp).ToList();

                tblSdPromResp tbl = new tblSdPromResp();
                tbl.RespId = 0;
                tbl.Desc = "NA";

                lstResponses.Add(tbl);

                return lstResponses;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public Object GetAllResponses(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var responses = (from resp in Data.tblSdPromResps
                                 join respType in Data.tblSdPromRespTypes on resp.RespTypeId equals respType.RespTypeId
                                 orderby respType.Desc
                                 select new
                                 {
                                     resp.RespId,
                                     resp.Desc,
                                     RespCode = resp.Response,
                                     ResponseType = respType.Desc,
                                     resp.RespTypeId,
                                     resp.Critical,
                                     resp.Enabled,
                                     resp.CreatedBy,
                                     resp.CreatedOn

                                 }).ToList();
                return responses;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public tblSdPromResp GetResponseByID(int responseId, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                //Data = RMSDB.GetOject();
                tblSdPromResp response = Data.tblSdPromResps.Single(p => p.RespId == responseId);
                //Data.Dispose();
                return response;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public bool ISAlreadyExistResponse(tblSdPromResp response, RMSDataContext Data)
        {

            bool isAlready = false;
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                IQueryable<tblSdPromResp> responses = from resp in Data.tblSdPromResps
                                                      where resp.RespId != response.RespId && resp.Desc == response.Desc
                                                      select resp;

                if (responses != null && responses.Count<tblSdPromResp>() > 0)
                {
                    isAlready = true;
                }
                return isAlready;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public void InsertResponse(tblSdPromResp response, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.tblSdPromResps.InsertOnSubmit(response);
                Data.SubmitChanges();

            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        #endregion


        #region Sales Promotion Response

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



                //var records = "" /* from n in Data.tblSDPromDatas*/
                //              //where n.LocId == 5
                //              //select n;
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

        //public string InsertPromotionData(tblSDPromData promotionData, RMSDataContext Data)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = RMSDB.GetOject(); }
        //        Data.tblSDPromDatas.InsertOnSubmit(promotionData);
        //        Data.SubmitChanges();

        //        return "Done";
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }
        //}

        //public string UpdatePromotionData(tblSDPromData promData, EntitySet<tblSDPromDataDet> promDataDets, RMSDataContext Data)
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
        //        var dets = from p in Data.tblSDPromDataDets
        //                   where p.vr_id == promData.vr_id
        //                   select p;

        //        Data.tblSDPromDataDets.DeleteAllOnSubmit(dets);

        //        promData.tblSDPromDataDets = promDataDets;
                
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

        //public tblSDPromData GetPromDataByID(int vrID, RMSDataContext Data)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = RMSDB.GetOject(); }

        //        tblSDPromData tbl = (from a in Data.tblSDPromDatas
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

        //public object GetAllPromotionData(int docNo,int outletId,int artTypeId,int respTypeId, RMSDataContext Data)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = RMSDB.GetOject(); }

        //        var promData = from /*promD in Data.tblSDPromDatas*/
        //                        outlet in Data.tblSlAreaCds /*on promD.DlrId equals outlet.id*/
        //                       join respType in Data.tblSdPromRespTypes on promD.RespTypeId equals respType.RespTypeId
        //                       join artType in Data.tblSdPromArtTypes on promD.ArtTypeId equals artType.ArtTypeId
        //                       where outlet.ar_id == 'D' && promD.doc_no == (docNo == 0 ? promD.doc_no : docNo)
        //                       && promD.DlrId == (outletId == 0 ? promD.DlrId : outletId) && promD.RespTypeId == (respTypeId == 0 ? promD.RespTypeId : respTypeId)
        //                       && promD.ArtTypeId == (artTypeId == 0 ? promD.ArtTypeId : artTypeId)
        //                       select new
        //                       {
        //                           promD.vr_id,
        //                           promD.doc_no,
        //                           promD.doc_dt,
        //                           promD.DlrId,
        //                           promD.RespTypeId,
        //                           promD.ArtTypeId,
        //                           promD.vr_apr,
        //                           outlet = outlet.ar_dsc,
        //                           respType = respType.Desc,
        //                           artType=artType.Desc
        //                       };
        //        return promData;

        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }
        //}


        //public List<tblSDPromDataDet> GetAllPromDataDetByPromDataID(int vrID, RMSDataContext Data)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = RMSDB.GetOject(); }

        //        List<tblSDPromDataDet> lsPromDataDet = (from a in Data.tblSDPromDataDets
        //                                                where a.vr_id == vrID
        //                                                select a).ToList();
        //        return lsPromDataDet;

        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }
        //}


    }
}