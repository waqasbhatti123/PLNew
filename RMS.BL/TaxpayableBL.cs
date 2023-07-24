using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.BL
{
    public class TaxpayableBL
    {
        RMSDataContext Data = new RMSDataContext();
        public TaxpayableBL()
        {

        }

        public IQueryable<sp_TaxPayableResult> rptGenerateReportyTax(string Acc, int vrid)
        {
            IQueryable<sp_TaxPayableResult> results = Data.sp_TaxPayable(Acc, vrid).AsQueryable();
            return results;




            //var tax = (from a in Data.Glmf_Datas
            //           join type in Data.Vr_Types on a.vt_cd equals type.vt_cd
            //           join chq in Data.Glmf_Data_chqs on a.vrid equals chq.vrid
            //           join text in Data.tblTextPayables on a.vrid equals text.VrID
            //           into textpay
            //           from textpayable in textpay.DefaultIfEmpty()
            //           where a.vt_cd == 64 && chq.vr_chq == Acc
            //           orderby a.vr_dt ascending, a.vr_no descending
            //           select new
            //           {
            //               IncomeTax = textpayable.ITAmount,
            //               GST = textpayable.GSTAmount,
            //               PRA = textpayable.PRA,
            //               a.vrid,
            //               vr_nrtn = a.vr_nrtn,
            //               ref_no = a.Ref_no,
            //               type = type.vt_use + (a.source != null && type.vt_use != a.source ? "-" + a.source : "") + '-' + a.vr_no,
            //               cheNo = chq.vr_chq,
            //               cheDat = chq.vr_chq_dt,
            //               a.source,
            //               headsInvolved = GetGLMFCode(a.vrid),
            //           }).ToList();

            //return tax;
        }


        public string GetGLMFCode(int vrId)
        {
            try
            {
                var codeDesc = from c in Data.Glmf_Data_Dets
                               join d in Data.Glmf_Codes on c.gl_cd equals d.gl_cd
                               where c.vrid == vrId
                               select d.gl_dsc;

                string code = "";
                foreach (var item in codeDesc)
                {
                    code += item + "\r\n";
                }
                return code;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
