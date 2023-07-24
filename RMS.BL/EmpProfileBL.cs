using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.BL
{
    public class EmpProfileBL
    {
        public EmpProfileBL()
        {

        }

        public List<spEmpBasicInfoResult> GetEmpBasicInfo(int? empid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                List<spEmpBasicInfoResult> emp = new List<spEmpBasicInfoResult>();
                emp = Data.spEmpBasicInfo(empid).ToList();
                return emp;
            }
            catch(Exception ex) { }
            return null;
        }

        public List<sp_GetEmployeeSearchResult> GetEmployeeSearch(int brid, string name, RMSDataContext db)
        {
            
            List<sp_GetEmployeeSearchResult> emp = db.sp_GetEmployeeSearch(brid, name).ToList();
            return emp;
        }

        public IList<SpProfileGetAllEmployeesResult> getEmployees( RMSDataContext db)
        {
            IList<SpProfileGetAllEmployeesResult> emp = db.SpProfileGetAllEmployees().ToList();
            return emp;
        }

        public IList<SpProfileEducationResult> getEdu(int comID,int br, int empID,int scal,int scalto,string deg, RMSDataContext db)
        {
            IList<SpProfileEducationResult> edu = db.SpProfileEducation(comID, br,empID,"","").ToList();

            if (scal != 0 && scalto != 0)
            {
                edu = edu.Where(x => x.ScaleID >= scal && x.ScaleID <= scalto).ToList();
            }
            if (deg != "")
            {
                edu = edu.Where(x => x.Degreetype == deg).ToList();
            }
            return edu;
        }

        public IList<SpProfileExperienceResult> getPriorExpe(int comID,  int br, int empID, int scal, RMSDataContext db)
        {
            try
            {
                IList<SpProfileExperienceResult> exp = db.SpProfileExperience(comID, br, empID, "").ToList();
                return exp;
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public IList<SpProfileTenureExperienceResult> getTenureExpe(int comID,int br, int empID, RMSDataContext db)
        {
            IList<SpProfileTenureExperienceResult> Texp = db.SpProfileTenureExperience(comID, br,empID).ToList();
            return Texp;
        }

        public IList<SpProfileAcrResult> getAcrRecord(int comID,int br,int empID, RMSDataContext db)
        {
            IList<SpProfileAcrResult> acr = db.SpProfileAcr(comID, br,empID).ToList();
            return acr;
        }
        public IList<SpProfileEnquiryResult> getEnquiryRecord(int comID,int br,int empID, RMSDataContext db)
        {
            IList<SpProfileEnquiryResult> enq = db.SpProfileEnquiry(comID, br,empID).ToList();
            return enq;
        }

        public IList<SpProfileLitigationResult> getLitigationRecord(int comID, int br, int empID, RMSDataContext db)
        {
            IList<SpProfileLitigationResult> liti = db.SpProfileLitigation(comID, br, empID).ToList();
            return liti;
        }
        public IList<SpProfilePromotionResult> getPermotion(int comID, int br, int empID, RMSDataContext db)
        {
            IList<SpProfilePromotionResult> per = db.SpProfilePromotion(comID, br, empID).ToList();
            return per;
        }
        public IList<sp_EmpEducationResult> getEmpEdu( int empID, RMSDataContext db)
        {
            IList<sp_EmpEducationResult> edu = db.sp_EmpEducation(empID).ToList();
            return edu;
        }

        public IList<ap_EmployeeBasicInfoResult> getEmployeeBasicInfo(int empID, RMSDataContext db)
        {
            IList<ap_EmployeeBasicInfoResult> info = db.ap_EmployeeBasicInfo(empID).ToList();
            return info;
        }

        public IList<sp_EmpTenureResult> GetEmpTenureExp(int empID, RMSDataContext db)
        {
            IList<sp_EmpTenureResult> ten = db.sp_EmpTenure(empID).ToList();
            return ten;
        }

        public IList<sp_EmpTenureForAdditionalChargeResult> GetEmpTenureExpForAdditionalCharge(int empID, RMSDataContext db)
        {
            IList<sp_EmpTenureForAdditionalChargeResult> ten = db.sp_EmpTenureForAdditionalCharge(empID).ToList();
            return ten;
        }

        public IList<sp_EmpPriorExperienceResult> GetEmpPriorExp(int empID, RMSDataContext db)
        {
            IList<sp_EmpPriorExperienceResult> ten = db.sp_EmpPriorExperience(empID).ToList();
            return ten;
        }

        public IList<sp_EmployeeEnquiryResult> GetEmpEnq(int empID, RMSDataContext db)
        {
            IList<sp_EmployeeEnquiryResult> enq = db.sp_EmployeeEnquiry(empID).ToList();
            return enq;
        }

        public IList<sp_EmployeeLitigationResult> GetEmpLitigation(int empID, RMSDataContext db)
        {
            IList<sp_EmployeeLitigationResult> lit = db.sp_EmployeeLitigation(empID).ToList();
            return lit;
        }

        public IList<sp_EmpPermotionResult> GetEmpPermotion(int empID, RMSDataContext db)
        {
            IList<sp_EmpPermotionResult> per = db.sp_EmpPermotion(empID).ToList();
            return per;
        }

        public IList<sp_EmpAcrRecordResult> GetEmpAcrRecord(int empID, RMSDataContext db)
        {
            IList<sp_EmpAcrRecordResult> acr = db.sp_EmpAcrRecord(empID).ToList();
            return acr;
        }
    }
}
