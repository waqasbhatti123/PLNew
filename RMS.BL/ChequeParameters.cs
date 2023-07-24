using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMS.BL
{
    public class ChequeParameters
    {
        public string PayAc { get; set; }
        public string PayeeAc { get; set; }
        public string Payee { get; set; }
        public string ChequeNo { get; set; }
        public bool AcPayeeOnly { get; set; }
        public decimal Amount { get; set; }
        public DateTime ChequeDate { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string AmountWords1 { get; set; }
        public string AmountWords2 { get; set; }
        public string PDFName { get; set; }
        public string d1 { get; set; }
        public string d2 { get; set; }
        public string m1 { get; set; }
        public string m2 { get; set; }
        public string y1 { get; set; }
        public string y2 { get; set; }
        public string y3 { get; set; }
        public string y4 { get; set; }


    }
}
