using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMS.BL
{
    public class Anonymous4Grid
    {
        string DocRef;
        public string Doc_Ref
        {
            get { return DocRef; }
            set { DocRef = value; }
        }
        string itmDesc;
        public string itm_Desc
        {
            get { return itmDesc; }
            set { itmDesc = value; }
        }

        string vendor;
        public string Vendor
        {
            get { return vendor; }
            set { vendor = value; }
        }

        int LotNo;
        public int Lot_No
        {
            get { return LotNo; }
            set { LotNo = value; }
        }

        string brkr;
        public string Broker
        {
            get { return brkr; }
            set { brkr = value; }
        }

        string prdct;
        public string Product
        {
            get { return prdct; }
            set { prdct = value; }
        }

        string sLotNo;
        public string strLotNo
        {
            get { return sLotNo; }
            set { sLotNo = value; }
        }

        string sQty;
        public string LotQty
        {
            get { return sQty; }
            set { sQty = value; }
        }

        string igp;
        public string IgpNo
        {
            get { return igp; }
            set { igp = value; }
        }

        string vrno;
        public string vr_no
        {
            get { return vrno; }
            set { vrno = value; }
        }
        decimal vrqty;
        public decimal vr_qty
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
    
    }


}
