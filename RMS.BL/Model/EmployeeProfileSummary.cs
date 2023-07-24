using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.BL.Model
{
    public class EmployeeProfileSummary
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public int? designation { get; set; }
        public int? Section { get; set; }
        public string Des { get; set; }
        public string Sec { get; set; }
        public int? Age { get; set; }
        public int? scale { get; set; }
        public int? JobType { get; set; }
        public string ScaleName { get; set; }
        public string JobeTypeName { get; set; }
        public string Domicil { get; set; }
        public int? br { get; set; }
        public string brName { get; set; }
        public char? Gender { get; set; }
        public char Religion { get; set; }
        public string AddtionCharg { get; set; }
        public DateTime? JoinDate { get; set; }
        public bool? police  { get; set; }
        public string Quota { get; set; }
        public string Disablity { get; set; }

    }
}
