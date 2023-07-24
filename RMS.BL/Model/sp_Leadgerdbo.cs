using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.BL.Model
{
    public class sp_Leadgerdbo
    {
        public int? vrid { get; set; }

        public string Code { get; set; }
        public string ParentCode { get; set; }
        public string Ref_no { get; set; }
        public int? br_id { get; set; }
        public decimal? GlYear { get; set; }
        public int? vt_cd { get; set; }
        public int? vr_no { get; set; }
        public string vt_use { get; set; }
        public string source { get; set; }
        public string vt_dsc { get; set; }
        public DateTime? vr_dt { get; set; }
        public string vr_nrtn { get; set; }
        public string vr_apr { get; set; }
        public DateTime? updateon { get; set; }
        public string updateby { get; set; }
        public int? vr_seq { get; set; }
        public decimal? amount { get; set; }
        public string ntn { get; set; }
        public decimal? AmountForCloseBalnceDebit { get; set; }
        public decimal? AmountForCloseBalnceCredit { get; set; }
        public decimal? vrd_debit { get; set; }
        public decimal? vrd_credit { get; set; }
        public string vrd_nrtn { get; set; }

        public string gl_cd { get; set; }
        public string cc_cd { get; set; }
        public string gl_dsc { get; set; }
        public string cc_nme { get; set; }
        public string cnt_gl_cd { get; set; }
        public string cnt_gl_dsc { get; set; }
        public int? act_status { get; set; }
        public string vr_chq { get; set; }
        public DateTime? vr_chq_dt { get; set; }
        public decimal vrdebit { get; set; }

        public decimal? gl_op { get; set; }

    }
}
