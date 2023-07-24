using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMS.BL
{
    public class Anonymous4MatRec
    {
        string Doc_Ref;
        public string DocRef
        {
            get { return Doc_Ref; }
            set { Doc_Ref = value; }
        }

        int vendor;
        public int br_id
        {
            get { return vendor; }
            set { vendor = value; }
        }

        int LotNo;
        public int LocId
        {
            get { return LotNo; }
            set { LotNo = value; }
        }

        string locname;
        public string LocName
        {
            get { return locname; }
            set { locname = value; }
        }

        int vrno;
        public int vr_no
        {
            get { return vrno; }
            set { vrno = value; }
        }
        int vrqty;
        public int vt_cd
        {
            get { return vrqty; }
            set { vrqty = value; }
        }

        DateTime vrdt;
        public DateTime vr_dt
        {
            get { return vrdt; }
            set { vrdt = value; }
        }
        string gldsc;
        public string gl_dsc
        {
            get { return gldsc; }
            set { gldsc = value; }
        }
        string vrapr;
        public string vr_apr
        {
            get { return vrapr; }
            set { vrapr = value; }
        }

        string dpt;
        public string DeptNme
        {
            get { return dpt; }
            set { dpt = value; }
        }
        int vrid;
        public int vr_id
        {
            get { return vrid; }
            set { vrid = value; }
        }
    }


}
