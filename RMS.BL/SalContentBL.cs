using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.BL
{
    public class SalContentBL
    {
        public SalContentBL()
        {

        }
        public object GetAllDed(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var alldd = from all in Data.SalaryContentTypes
                            
                            orderby all.Name
                            select all;

                return alldd;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public object GetAllDedRep(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var alldd = from all in Data.SalaryContents
                            join sal in Data.SalaryContentTypes
                            on all.SalaryContentTypeID equals sal.SalaryContentTypeID
                            orderby all.Sort ascending
                            select new
                            {
                                all.SalaryContentID,
                                all.Name,
                                sao = sal.Name,
                                all.Size,
                                all.IsActive,
                                all.Sort
                                //all.IsValue
                            };

                return alldd;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }


        public void Insert(SalaryContent sal, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.SalaryContents.InsertOnSubmit(sal);
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public SalaryContent GetByID(int emped, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                SalaryContent EmpEdu = Data.SalaryContents.Single(p => p.SalaryContentID.Equals(emped));

                return EmpEdu;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public void update(SalaryContent id, RMSDataContext Data)
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
