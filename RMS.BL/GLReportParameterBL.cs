using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;

namespace RMS.BL
{
    public class GLReportParameterBL
    {



        public bool isExist(Int16 compId, Int16 repNo, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                List<tblglrpt> ls = (from a in Data.tblglrpts
                                     where a.CompId == compId && a.RptNo == repNo
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
            catch(Exception ex)
            {
                throw ex;
            }
        }


        public bool save(tblglrpt glrpt,EntitySet<tblglrptdet> glrptDet, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                
                Data.tblglrpts.InsertOnSubmit(glrpt);
                glrpt.tblglrptdets = glrptDet;
                Data.SubmitChanges();
                return true;

            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;

            }

        }

        public bool saveHdr(tblglrpt glrpt, EntitySet<tblglrptHdr> glrptDet, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                tblglrpt gl = (from a in Data.tblglrpts
                          where a.CompId == glrpt.CompId && a.RptNo == glrpt.RptNo
                          select a).Single();

                gl.Rptdesc = glrpt.Rptdesc;
                gl.NoteNo = glrpt.NoteNo;
                gl.PrintDrCr = glrpt.PrintDrCr;
                gl.PrintPrvYr = glrpt.PrintPrvYr;
                gl.CompLevelRpt = glrpt.CompLevelRpt;
                gl.updateby = glrpt.updateby;
                gl.updateon = glrpt.updateon;

                Data.tblglrptHdrs.InsertAllOnSubmit(glrptDet);
                //Data.tblglrpts.InsertOnSubmit(glrpt);
                //glrpt.tblglrptHdrs = glrptDet;
                Data.SubmitChanges();
                return true;

            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;

            }

        }


        public bool deleteRecord(byte compId,Int16 rptNo, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                //RMSDataContext Data = new RMSDataContext();

                var gldet = (from d in Data.tblglrptdets
                           where d.CompId==compId && d.RptNo==rptNo
                           select d);
                Data.tblglrptdets.DeleteAllOnSubmit(gldet);
                Data.SubmitChanges();

                var gl = (from a in Data.tblglrpts
                          where a.CompId == compId && a.RptNo == rptNo
                          select a).Single();

                Data.tblglrpts.DeleteOnSubmit(gl);
                Data.SubmitChanges();
                
                return true;

            }

            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;

            }
        }
        public bool deleteHdrRecord(byte compId, Int16 rptNo, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                //RMSDataContext Data = new RMSDataContext();

                var gldet = (from d in Data.tblglrptHdrs
                             where d.CompId == compId && d.RptNo == rptNo
                             select d);
                Data.tblglrptHdrs.DeleteAllOnSubmit(gldet);
                Data.SubmitChanges();

                //var gl = (from a in Data.tblglrpts
                //          where a.CompId == compId && a.RptNo == rptNo
                //          select a).Single();

                //Data.tblglrpts.DeleteOnSubmit(gl);
                //Data.SubmitChanges();

                return true;

            }

            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;

            }
        }
        //------------------------

        public tblglrpt getByRptNo(Int16 rptNo, byte compId, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                tblglrpt tbl = (from a in Data.tblglrpts
                                where a.RptNo == rptNo && a.CompId == compId
                                select a).Single();
                return tbl;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //------------
        public List<tblglrptdet> getDetailByRptNo(Int16 rptNo, byte compId, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                List<tblglrptdet> ls = (from a in Data.tblglrptdets
                                        where a.RptNo == rptNo && a.CompId == compId
                                        select a).OrderBy(o=> o.RecNo).ToList();
                return ls;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<tblglrptHdr> getDetailByRptNoHdr(Int16 rptNo, byte compId, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                List<tblglrptHdr> ls = (from a in Data.tblglrptHdrs
                                        where a.RptNo == rptNo && a.CompId == compId
                                        select a).OrderBy(o => o.RecNo).ToList();
                return ls;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //---------------
        public List<spGlSetupGetrptParameterHomeGridResult> getHomeGridData(Int16 rptNo,Int16 noteNo, string rptName,byte compId,RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                return Data.spGlSetupGetrptParameterHomeGrid(rptNo, noteNo, rptName, compId).ToList();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        //-----------------------For GL Report---------

        public List<spGlReportResult> getGlReportData(int compid, int brid, int rptNo,DateTime toDate, char status, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                return Data.spGlReport(compid, brid, rptNo,toDate, status).ToList();
               
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public List<spHdrGlReportResult> getHdrGlReportData(int compid, int brid, int rptNo, DateTime toDate, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                return Data.spHdrGlReport(compid, brid, rptNo, toDate).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public List<spGlReportSeriesResult> getGlReportSeriesData(int brid, DateTime toDate, string cdfrom, string cdto, string fromcc, string tocc, char status, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                return Data.spGlReportSeries(brid, toDate, cdfrom, cdto, fromcc, tocc, status).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

       //COST CENTER GROUPS OF REPORT PARAMETER


        public string SaveCCGrp(tblglCCGrp ccgrp, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.tblglCCGrps.InsertOnSubmit(ccgrp);
                Data.SubmitChanges();
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string UpdateCCGrp(tblglCCGrp ccgrp, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.SubmitChanges();
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public tblglCCGrp GetCCGrpById(int compid, int ccgrp, int ccseq, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                return Data.tblglCCGrps.Where(x => x.CompId == compid && x.CCGrp == ccgrp && x.CCSeq == ccseq).Single();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Int16 GetMaxSeq(int compid, int ccgrp, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                return Convert.ToInt16( Data.tblglCCGrps.Where(x => x.CompId == compid && x.CCGrp == ccgrp).Count() + 1);

            }
            catch //(Exception ex)
            {
                //throw ex;
            }
            return 1;
        }

        public object GetAllCCGrps(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var obj = (from a in Data.tblglCCGrps
                          join b in Data.Cost_Centers
                          on a.CC_From equals b.cc_cd into leftjoinCCFrom
                          from x in leftjoinCCFrom.DefaultIfEmpty()
                          join c in Data.Cost_Centers
                          on a.CC_To equals c.cc_cd into leftjoinCCTo
                          from y in leftjoinCCTo.DefaultIfEmpty()
                          orderby a.CCGrp, a.CCSeq
                          select new
                          {
                              a.CompId, 
                              a.CCGrp,
                              a.CCSeq,
                              a.GrpDesc,
                              CC_From = a.CC_From + " - "+ x.cc_nme,
                              CC_To = a.CC_To + " - " + y.cc_nme,
                          }).ToList();
                return obj;

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public List<tblglCCGrp> GetCCGrpsForAutComp(string txt)
        {
            RMSDataContext Data = new RMSDataContext();
            try
            {
                List<tblglCCGrp> obj = (from a in Data.tblglCCGrps
                           where a.CCGrp.ToString().Contains(txt) || a.GrpDesc.Contains(txt)
                           orderby a.CCGrp, a.CCSeq
                           select a).ToList();
                return obj;

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
    }
}
