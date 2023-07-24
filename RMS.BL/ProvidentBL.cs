using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.BL
{
    public class ProvidentBL
    {
        public ProvidentBL()
        {

        }

        public void SubmitPro(tblPlProvidentFund PFund, RMSDataContext Data)
        {
            try
            {
                
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.tblPlProvidentFunds.InsertOnSubmit(PFund);
                Data.SubmitChanges();

            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public object GetByID(int pFund, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                var fund = from p in Data.tblPlProvidentFunds
                           where p.PfID == pFund
                           select p;

                return fund;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public object GetByYear(int pFund, int emp,  RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                var fund = from p in Data.tblPlProvidentFundDetails
                           where p.Year == pFund && p.EmpID == emp
                           orderby p.Month 
                           select new
                           {
                               p.PfdID,
                               p.Year,
                               // Month = Convert.ToDateTime(p.Year.ToString() +"-"+ p.Month.ToString()).ToString("MMMM"),
                               Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(p.Month)),
                                 p.value,
                               p.EmpID,
                               p.tblPlEmpData.FullName,
                               p.tblPlProvidentFund.closeBlnc,
                               p.tblPlProvidentFund.OpenBlnc,
                               p.tblPlProvidentFund.createdon,
                               p.tblPlProvidentFund.createdBy
                           };

                return fund;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
    }
}
