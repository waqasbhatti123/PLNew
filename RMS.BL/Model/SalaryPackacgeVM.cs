using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.BL.Model
{
    public class SalaryPackacgeVM
    {
        public int TrfDetailID { get; set; }
        public string Name { get; set; }
        public int trfID { get; set; }
        public int EmpID { get; set; }
        public int? Value { get; set; }
        public int? Basic { get; set; }
        public int? ContentType { get; set; }
        public int? DeduType { get; set; }
    }
}
