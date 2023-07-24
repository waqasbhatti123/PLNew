using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;



namespace RMS.BL
{
    public class DesignColorBL
    {
        public List<tblItemColor> getAll(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                List<tblItemColor> color = (from a in Data.tblItemColors
                                            select a).OrderBy(o=> o.Color).ToList();


                return color;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data); throw ex;
            }
        }

        public bool save(int br,string ci,string des,char st,RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                //RMSDataContext Data = new RMSDataContext();
                

                    tblItemColor tbl = new tblItemColor();
                    tbl.br_id = br;

                    tbl.ColorId = ci;
                    tbl.Color = des;
                    tbl.Status = st.ToString();


                    Data.tblItemColors.InsertOnSubmit(tbl);
                    Data.SubmitChanges();
                    return true;
                  
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }

        }



        public bool delete(string code, string desc, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                //RMSDataContext Data = new RMSDataContext();
                var ed = (from a in Data.tblItemColors
                          where a.ColorId == code && a.Color == desc
                          select a).Single();
                Data.tblItemColors.DeleteOnSubmit(ed);
                Data.SubmitChanges();
                return true;

            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }

        public bool exist(string cid, RMSDataContext Data)
        {
            try
            {

                if (Data == null) { Data = RMSDB.GetOject(); }
                bool existChk = false;

                var ex = (from e in Data.tblItemColors
                          select e).ToList();

                foreach (var chk in ex)
                {
                    if (chk.ColorId == cid)
                    {
                        existChk = true;
                        break;
                    }
                    else
                    {
                        existChk = false;
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
