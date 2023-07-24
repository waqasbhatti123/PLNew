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
    public class LocationBL
    {

        public LocationBL()
        {

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
                var otblCity = from cty in Data.tblPlLocations
                               orderby cty.LocName
                               select new
                               {
                                   cty.LocID,
                                   cty.LocName,
                                   cty.tblCity.CityName
                                   
                                   
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

        public bool ISAlreadyExist(tblPlLocation cto, RMSDataContext Data)
        {
            try
            {
                bool isalready = false;
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<tblPlLocation> cty = from ct in Data.tblPlLocations
                                          where ct.LocID != cto.LocID && (ct.LocName == cto.LocName)
                                          select ct;

                if (cty != null & cty.Count<tblPlLocation>() > 0)
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


        public tblPlLocation GetByID(int ctyid, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                tblPlLocation otblCity = Data.tblPlLocations.Single(p => p.LocID == ctyid);

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
       
        public void Insert(tblPlLocation loc, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.tblPlLocations.InsertOnSubmit(loc);
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public void Update(tblPlLocation cty, RMSDataContext Data)
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