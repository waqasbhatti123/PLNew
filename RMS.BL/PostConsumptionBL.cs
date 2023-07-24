using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.Linq;

namespace RMS.BL
{
    public class PostConsumptionBL
    {
        public PostConsumptionBL()
        {
        }


        public object GetCosumptionGrdData(int brid, DateTime tillDate, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                var obj = (from a in Data.spPostConsumptionMaster(brid, tillDate)
                           select new
                           {
                               a.cc_cd,
                               a.prnt_cd,
                               cc_nme = a.cc_cd + "-" + a.cc_nme + "  /  " + (!string.IsNullOrEmpty(a.isu_acc) ? (a.isu_acc + "-" + a.isu_dsc) : " [Code Error]"),
                               a.vr_val
                           }).ToList();
                return obj;

            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }

        public List<spPostConsumptionChildResult> GetCosumptionGrdDataDetail(int brid, string cc, string prnt_cd, DateTime tillDate, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }


                return Data.spPostConsumptionChild(brid, cc, prnt_cd, tillDate).ToList();
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }

        public string PostConsumption(Glmf_Data glmf, List<int> vridList, DateTime tillDate, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }


                Data.Glmf_Datas.InsertOnSubmit(glmf);
                //update vt_cd == 17(issue code) according to vridList
                List<tblStkData> stk = new List<tblStkData>();
                tblStkData data = null;
                foreach (int vrid in vridList)
                {
                    data = Data.tblStkDatas.Where(x => x.vr_id == vrid).Single();
                    data.post_2gl = true;
                    data.PostDt = tillDate;
                    stk.Add(data);
                }
                //update vt_cd == 18(return code) according to vridList of vt_cd = 17
                List<tblStkData> stkRet = new List<tblStkData>();
                tblStkData dataIssue = null;
                tblStkData dataRet= null;
                foreach (int vrid in vridList)
                {
                    dataIssue = Data.tblStkDatas.Where(x => x.vr_id == vrid).Single();

                    dataRet = Data.tblStkDatas.Where(y => y.br_id == dataIssue.br_id &&
                                                         y.LocId == dataIssue.LocId &&
                                                         y.IGPNo == dataIssue.vr_no &&
                                                         y.DeptId == dataIssue.DeptId &&
                                                         y.vr_apr == "A" &&
                                                         y.vt_cd == 18 &&
                                                         y.post_2gl == null
                                                         ).SingleOrDefault();

                    if (dataRet != null)
                    {
                        dataRet.post_2gl = true;
                        dataRet.PostDt = tillDate;
                        stkRet.Add(dataRet);
                    }
                }



                tblCompany comp = Data.tblCompanies.First();

                string consumptionDate = tillDate.AddMonths(2).Month.ToString() + "-01-" + tillDate.AddMonths(2).Year.ToString();
                comp.ConsPostDt = Convert.ToDateTime(consumptionDate).AddDays(-1);

                Data.SubmitChanges();
                return "ok";
            }
            catch (Exception ex)
            {
                
                return ex.Message;
            }
        }


        public string GetConsumptionMonth(RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                return Data.tblCompanies.First().ConsPostDt.Value.Month.ToString();
                
            }
            catch (Exception ex)
            {
                
                return ex.Message;
            }
        }

        public DateTime  GetConsumptionDate(RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            DateTime dt = new DateTime();
            try
            {
                dt = Data.tblCompanies.First().ConsPostDt.Value;

            }
            catch (Exception ex)
            {

                //return ex.Message;
            }

            return dt;
        }

    }
}
