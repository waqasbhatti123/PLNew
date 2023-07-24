using System;
using System.Linq;
using System.Collections.Generic;
//using System.Transactions;


namespace RMS.BL
{
    public class SalesPersonBL
    {
        public SalesPersonBL()
        {

        }

        //public tblSalesPerson GetSalesPersonByID(int id,RMSDataContext Data)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = RMSDB.GetOject(); }
        //        tblSalesPerson salesPerson = (from saleP in Data.tblSalesPersons
        //                                      where saleP.ID==id
        //                                            orderby saleP.SalesPerson
        //                                            select saleP).SingleOrDefault();
        //        return salesPerson;
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }
        //}

        public List<tblSalesPerson> GetAllSalesPerson(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                List<tblSalesPerson> salesPerson = (from saleP in Data.tblSalesPersons
                                                        orderby saleP.SalesPerson
                                                        select saleP).ToList();
                return salesPerson; 
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

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
    }
}