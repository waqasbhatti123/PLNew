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

namespace RMS.BL
{
    public class InvSearch_BL
    {
        public InvSearch_BL()
        {
        }
        public List<spGetSearchIGP4FeetageResult> GetIGPs4FeetagSrch(string prefix)
        {
            try
            {
                RMSDataContext Data = new RMSDataContext();
                if(Data == null){Data = RMSDB.GetOject();}

                List<spGetSearchIGP4FeetageResult> lst = (from a in Data.spGetSearchIGP4Feetage()
                                                          where
                                                          a.VrNo.Contains(prefix) || a.Product.Contains(prefix) ||
                                                          a.Party.Contains(prefix)
                                                          select a).ToList();
                return lst;



            }
            catch (Exception ex)
            {
                 throw ex;
            }
        }

        public List<vwLotNoMTN> GetLot4MTNSrch(string prefix)
        {
            try
            {
                RMSDataContext Data = new RMSDataContext();
                if (Data == null) { Data = RMSDB.GetOject(); }

                List<vwLotNoMTN> lst = (from a in Data.vwLotNoMTNs
                                        where a.LotNo.Value.ToString().Contains(prefix)
                                        select a).ToList();
                return lst;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<vwLotNoGradingCard> GetLot4GradingSrch(string prefix)
        {
            try
            {
                RMSDataContext Data = new RMSDataContext();
                if (Data == null) { Data = RMSDB.GetOject(); }

                List<vwLotNoGradingCard> lst = (from a in Data.vwLotNoGradingCards
                                                where a.LotNo.Value.ToString().Contains(prefix)
                                                select a).ToList();
                return lst;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<vwLotNoMPN> GetLot4PPNSrch(string prefix)
        {
            try
            {
                RMSDataContext Data = new RMSDataContext();
                if (Data == null) { Data = RMSDB.GetOject(); }

                List<vwLotNoMPN> lst = (from a in Data.vwLotNoMPNs
                                        where a.LotNo.Value.ToString().Contains(prefix)
                                        select a).ToList();
                return lst;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
