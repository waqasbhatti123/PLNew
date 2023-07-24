using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.Common;
//using System.Transactions;


namespace RMS.BL
{
    public class SalesRptBL
    {

        public SalesRptBL()
        {

        }

        public List<spShipmentScheduleResult> GetShipmentSchedule(DateTime fromdt, DateTime todt, int sortby, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                return Data.spShipmentSchedule(fromdt, todt, sortby).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<spRateAnanlysisResult> GetRatesAnalysis(DateTime fromdt, DateTime todt, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                return Data.spRateAnanlysis(fromdt, todt).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<spAgeingOfReceivableRptResult> GetAgeingOfRec(int brid, string party, DateTime fromdt, DateTime todt, string status, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                return Data.spAgeingOfReceivableRpt(brid, party, fromdt, todt, status).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<spPartyWiseSaleRptResult> GetSalesRpt(int brid, string party, DateTime fromdt, DateTime todt, string status, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                return Data.spPartyWiseSaleRpt(brid, party, fromdt, todt, status).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<spPartyWiseSaleConsRptResult> GetConsolidatedSalesRpt(int brid, string party, DateTime fromdt, DateTime todt, string status, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                return Data.spPartyWiseSaleConsRpt(brid, party, fromdt, todt, status).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<spStatementOfAcResult> GetStatementOfAc(int brid, string party, DateTime fromdt, DateTime todt, string status, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                return Data.spStatementOfAc(brid, party, fromdt, todt, status).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}