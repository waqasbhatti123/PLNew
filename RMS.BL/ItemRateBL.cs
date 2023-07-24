using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Linq;

namespace RMS.BL
{
    public class ItemRateBL
    {
        public ItemRateBL()
        { }

        public string Save(string party, EntitySet<tblSaleRate> enttySaleRate, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                DeleteSameDateSaleRates(party, enttySaleRate.First().EffDate, Data);
                if (SaleRateExists(party, Data))
                {
                    List<tblSaleRate> rates = Data.tblSaleRates.Where(tbl => tbl.PartyID == party && tbl.Status == "OP").ToList();
                    foreach (tblSaleRate rate in rates)
                    {
                        rate.Status = "CL";
                    }
                    Data.SubmitChanges();
                }

                foreach (tblSaleRate sRate in enttySaleRate)
                {
                    Data.tblSaleRates.InsertOnSubmit(sRate);
                    Data.SubmitChanges();
                }
                
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public bool SaleRateExists(string party, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                List<tblSaleRate> rates = Data.tblSaleRates.Where(tbl => tbl.PartyID == party ).ToList();
                if (rates.Count > 0)
                    return true;
                else
                    return false;
            }
            catch
            {}
            return false;
        }

        public void DeleteSameDateSaleRates(string party, DateTime effdate, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                List<tblSaleRate> rates = Data.tblSaleRates.Where(tbl => tbl.PartyID == party && tbl.EffDate == effdate).ToList();
                Data.tblSaleRates.DeleteAllOnSubmit(rates);
                Data.SubmitChanges();
            }
            catch
            { }
        }

        public List<Glmf_Code> GetVendor(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var lst = from l in Data.Glmf_Codes
                          join r in Data.Preferences
                          on l.cnt_gl_cd equals r.ctrl_Vndr
                          where l.ct_id == "D"
                          orderby l.gl_dsc
                          select l;
                return lst.ToList();
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }


        public List<spItemRateResult> GetItems(string party, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                return Data.spItemRate(party).ToList(); 
            }
            catch
            {
                return null;
            }
        }

    }
}
