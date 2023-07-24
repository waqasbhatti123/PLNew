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
    public class EmpProfRptBL
    {

        public EmpProfRptBL()
        {

        }

        public List<spEmpBasicInfoResult> GetEmpBasicInfo(int empid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                return Data.spEmpBasicInfo(empid).ToList();
            }
            catch { }
            return null;
        }


        public List<spCurrentSalaryPackageResult> GetCurrentSalaryPackage(int compid, int empid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                return Data.spCurrentSalaryPackage(compid, empid).ToList();
            }
            catch { }
            return null;
        }

    }
}