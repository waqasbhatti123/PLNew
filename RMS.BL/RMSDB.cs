using System;
namespace RMS.BL
{
    public class RMSDB
    {
        //private static RMSDataContext RMSDBContxtObj;

        public static RMSDataContext GetOject()
        {
            return new RMSDataContext();
            //if (RMSDBContxtObj == null)
            //{
            //    //return DataContextFactory.GetDataContext<RMSDataContext>();
            //    RMSDBContxtObj = DataContextFactory.GetDataContext<RMSDataContext>();
            //    //RMSDBContxt = new RMSDataContext();
            //    return RMSDBContxtObj;
            //}
            //else
            //    return RMSDBContxtObj;
        }

        public static void SetNull()
        {
            //RMSDBContxtObj = null;
        }
        public static void closeConn(RMSDataContext rmsdb)
        {
            try
            {
                //if (RMSDBContxtObj != null)
                //{
                //    RMSDBContxtObj.Connection.Close();
                //    RMSDBContxtObj.Connection.Dispose();
                //    RMSDBContxtObj = null;
                //}
                
                //rmsdb.Connection.Close();

                //rmsdb.Connection.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void closeConnLogout(RMSDataContext rmsdb)
        {
            try
            {
                //if (RMSDBContxtObj != null)
                //{
                //    RMSDBContxtObj.Connection.Close();
                //    RMSDBContxtObj.Connection.Dispose();
                //    RMSDBContxtObj = null;
                //}
                if (rmsdb != null)
                    rmsdb.Connection.Close();

                //rmsdb.Connection.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}