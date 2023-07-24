using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMS.BL
{
    public class ThicknessSetupBL
    {
        public List<tblItemThick> getAll(RMSDataContext Data)
        {
            try
            {
                if (Data == null)
                {
                    Data = RMSDB.GetOject();
                }

                List<tblItemThick> ItemDes = (from a in Data.tblItemThicks
                                               select a).OrderBy(o=> o.Thick_Desc).ToList();


                return ItemDes;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }



        public List<Item_Uom> GetUOM(RMSDataContext Data)
        {
            try
            {
                return Data.Item_Uoms.ToList();
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }


        public bool save(int br, short thickId,decimal thick, string desc, char st, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                //RMSDataContext Data = new RMSDataContext();

                tblItemThick tbl = new tblItemThick();
                tbl.br_id = br;
                tbl.ThickId= thickId;
                tbl.Thickness = thick;
                tbl.Thick_Desc = desc;
                tbl.Status = Convert.ToString(st);
                tbl.UOM_Cd = 4;
                Data.tblItemThicks.InsertOnSubmit(tbl);
                Data.SubmitChanges();
                return true;

            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;

            }

        }


        public bool delete(int thickId, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                //RMSDataContext Data = new RMSDataContext();

                var del = (from d in Data.tblItemThicks
                           where d.ThickId == thickId
                           select d).Single();
                Data.tblItemThicks.DeleteOnSubmit(del);
                Data.SubmitChanges();
                return true;

            }

            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;

            }
        }

        public bool exist(int thickId, RMSDataContext Data)
        {
            bool existChk = false;
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                var list = (from a in Data.tblItemThicks
                            select a).ToList();

                foreach (var f in list)
                {
                    if (f.ThickId == thickId)
                    {
                        existChk = true;
                        break;

                    }

                }

                return existChk;

            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }
    }
}
