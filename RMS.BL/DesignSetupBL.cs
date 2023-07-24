using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMS.BL
{
    public class DesignSetupBL
    {

        public bool SaveDesign(tblItemDesign design, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                Data.tblItemDesigns.InsertOnSubmit(design);
                
                Data.SubmitChanges();
                return true;
            }
            catch //(Exception ex)
            {
                //RMSDB.closeConn(Data);
               //throw ex;
            }
            return false;
        }


        public bool EditDesign(tblItemDesign design, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                Data.SubmitChanges();
                return true;
            }
            catch //(Exception ex)
            {
                //RMSDB.closeConn(Data);
                //throw ex;
            }
            return false;
        }


        public object GetGridObject(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                var obj = (from a in Data.tblItemDesigns
                           join b in Data.tblItemThicks
                           on a.ThickId equals b.ThickId
                           join c in Data.tblItemColors
                           on a.ColorId equals c.ColorId
                           orderby a.Design_Desc
                           select new
                           {
                               DesignId = a.DesignId,
                               DesignDesc = a.Design_Desc,
                               ThickId = b.ThickId,
                               ThickDesc = b.Thick_Desc,
                               Thick = b.Thickness,
                               ColorId = c.ColorId,
                               ColorDesc = c.Color,
                               Status = a.Status
                           }).ToList();
                return obj;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }


        public tblItemDesign GetDesById(string desId, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                tblItemDesign rec = (from a in Data.tblItemDesigns
                                     where a.DesignId == desId
                                     select a).Single();
                return rec;
            }
            catch //(Exception ex)
            {
                //RMSDB.closeConn(Data);
               //throw ex;
            }
            return null;
        }


        public tblItemDesign GetDesByDesc(string desDesc, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                tblItemDesign rec = (from a in Data.tblItemDesigns
                                     where a.Design_Desc == desDesc
                                     select a).Single();
                return rec;
            }
            catch //(Exception ex)
            {
                //RMSDB.closeConn(Data);
                //throw ex;
            }
            return null;
        }


        public List<tblItemThick> GetThickness(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                List<tblItemThick> lst = (from a in Data.tblItemThicks
                                          where a.Status == "A"
                                          orderby a.Thick_Desc
                                          select a).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }


        public List<tblItemColor> GetColor(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                List<tblItemColor> lst = (from a in Data.tblItemColors
                                          where a.Status == "A"
                                          orderby a.Color
                                          select a).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }

    }
}
