using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMS.BL
{
   public class DepartmentBL
    {
        public DepartmentBL()
        {
        }
        public List<Depart_ment> getAll(RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                return Data.Depart_ments.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Depart_ment getByName(string deptName, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                Depart_ment obj = Data.Depart_ments.Single(p => p.DeptId.Equals(deptName));
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsAlreadyExist(string deptName, RMSDataContext Data)
        {
            bool exist = true;
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                List<Depart_ment> li = (from a in Data.Depart_ments
                                        where a.DeptNme.Equals(deptName)
                                        select a).ToList();
                if (li.Count > 0)
                {
                    exist = true;
                }
                else exist = false;

                return exist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool insertRecord(Depart_ment dept, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                Data.Depart_ments.InsertOnSubmit(dept);
                Data.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
                //return false;
            }
        }

        public void update(string DeptNameNew,string DeptNameOld,RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                Depart_ment dept = (from a in Data.Depart_ments
                                    where a.DeptNme.Equals(DeptNameOld)
                                    select a).Single();
                dept.DeptNme = DeptNameNew;
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


       
       
    }
}
