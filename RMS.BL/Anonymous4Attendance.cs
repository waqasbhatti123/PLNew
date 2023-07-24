using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMS.BL
{
    public class Anonymous4Attendance
    {
        string region;
        public string Region
        {
            get { return region; }
            set { region = value; }
        }
        string depart;
        public string Department
        {
            get { return depart; }
            set { depart = value; }
        }

        string div;
        public string Division
        {
            get { return div; }
            set { div = value; }
        }

        string des;
        public string Designation
        {
            get { return des; }
            set { des = value; }
        }

        string empid;
        public string EmpID
        {
            get { return empid; }
            set { empid = value; }
        }

        string empcode;
        public string EmpCode
        {
            get { return empcode; }
            set { empcode = value; }
        }

        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        DateTime doj;
        public DateTime DOJ
        {
            get { return doj; }
            set { doj = value; }
        }

        DateTime dol;
        public DateTime DOL
        {
            get { return dol; }
            set { dol = value; }
        }

     }


}
