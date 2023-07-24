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
    public class CityBL
    {

        public CityBL()
        {

        }

        public IQueryable<tblCity> GetAllCities(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<tblCity> emps = from emp in Data.tblCities
                                           orderby emp.CityName
                                           select emp;
                return emps;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        //public IQueryable<tblCity> GetAllCitiesCombo()
        //{
        //    try
        //    {
        //        if (Data == null) { Data = RMSDB.GetOject(); }
        //        IQueryable<tblCity> otblCity = from cty in Data.tblCities
        //                                       where cty.Enable == true
        //                                       orderby cty.Name
        //                                       select cty;
        //        return otblCity;
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }
        //}

        //public IQueryable<Country> GetAllCountryCombo(RMSDataContext Data)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = RMSDB.GetOject(); }
        //        IQueryable<Country> oCountry = from cntry in Data.Countries
        //                                       where cntry.Enable == true 
        //                                       orderby cntry.CountryName
        //                                       select cntry;
        //        return oCountry;
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }
        //}
        //public Object GetAllLocation(RMSDataContext Data)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = RMSDB.GetOject(); }
        //        var otblCity = from cty in Data.tblCities
        //                       orderby cty.CityName
        //                       select new
        //                       {
        //                           cty.CityID,
        //                           cty.CityName
        //                           //cty.Enabled
        //                           //cty.Country.CountryName
        //                       };
        //        return otblCity;
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }

        //}


        public object GetAll(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var otblCity = from cty in Data.tblCities
                               orderby cty.CityName
                               select new
                               {
                                   cty.CityID,
                                   cty.CityName,
                                   cty.Enabled
                                   //cty.Country.CountryName
                               };
                return otblCity;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }

        }

 
        public bool ISAlreadyExist(tblCity cto, RMSDataContext Data)
        {
            try
            {
                bool isalready = false;
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<tblCity> cty = from ct in Data.tblCities
                                          where ct.CityID != cto.CityID && (ct.CityName == cto.CityName)
                                          select ct;

                if (cty != null & cty.Count<tblCity>() > 0)
                    isalready = true;
                return isalready;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public IQueryable<tblCity> GetAllCityCombo(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<tblCity> otblCity = from cty in Data.tblCities
                                               where cty.Enabled == true
                                               orderby cty.CityName
                                               select cty;
                return otblCity;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public IQueryable<tblPlLocation> GetAllCityLocsCombo(int cityId, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<tblPlLocation> locs = from loc in Data.tblPlLocations
                                                 where loc.CityID == cityId
                                                 orderby loc.LocName
                                                 select loc;
                return locs;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        //public IQueryable<tblCity> GetAlltblCityCombo(int countryId, RMSDataContext Data)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = RMSDB.GetOject(); }
        //        IQueryable<tblCity> otblCity = from cty in Data.tblCities 
        //                                 where cty.Enabled == true 
        //                                 orderby cty.CityName
        //                                 select cty;
        //        return otblCity;
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }
        //}

        public IQueryable<tblCity> GetAlltblCityFiltCombo(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<tblCity> otblCity = from cty in Data.tblCities
                                               where cty.Enabled == true
                                               orderby cty.CityName
                                               select cty;
                return otblCity;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }


        //public IQueryable<tblCity> GetByRegion(int regionID)
        //{
        //  try
        //  {
        //    if (Data == null) { Data = KSBSalesDB.GetOject(); }
        //    IQueryable<tblCity> otblCity = from cty in Data.tblCities 
        //                             where cty.RegionID == regionID
        //                             select cty;
        //    return otblCity;
        //  }
        //  catch (Exception ex)
        //  {
        //    KSBSalesDB.SetNull();
        //    throw ex;
        //  }
        //}


        public tblCity GetByID(int ctyid, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                tblCity otblCity = Data.tblCities.Single(p => p.CityID == ctyid);

                return otblCity;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }


        public void DeleteByID(int ctyid, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                tblCity cty = Data.tblCities.Single(p => p.CityID == ctyid);
                Data.tblCities.DeleteOnSubmit(cty);
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }

        }

        public void Insert(tblCity cty, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.tblCities.InsertOnSubmit(cty);
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public void Update(tblCity cty, RMSDataContext Data)
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