using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.BL.Model
{
    public class sp_Profiledbo
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public int? designation { get; set; }
        public int? Section { get; set; }
        public string Des { get; set; }
        public string Sec { get; set; }
        public int AppointedID { get; set; }
        public string AppointedName { get; set; }
        public int LastPerID { get; set; }
        public string LastPerName { get; set; }
        public int? Age { get; set; }
        public int? scale { get; set; }
        public int? JobType { get; set; }
        public string ScaleName { get; set; }
        public string JobeTypeName { get; set; }
        public string Domicil { get; set; }
        public int? br { get; set; }
        public string brName { get; set; }
        public string Gender { get; set; }
        public char Religion { get; set; }
        public string AddtionCharg { get; set; }
        public DateTime? JoinDate { get; set; }
        public bool? police { get; set; }
        public string Quota { get; set; }
        public string Disablity { get; set; }
        public string SectionName { get; set; }
        public string Contactno { get; set; }
        public string Mobno { get; set; }
        public string Appointed { get; set; }
        public DateTime? BOD { get; set; }
        public string Email { get; set; }
        public int  SortDest { get; set; }
        public int EmpStatus { get; set; }
        public string RelievingDate { get; set; }
        public string coln { get; set; }
    }
}
