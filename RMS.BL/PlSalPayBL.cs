using System;
using System.Linq;
using System.Collections.Generic;

namespace RMS.BL
{
    public class PlSalPayBL
    {
        public PlSalPayBL()
        {

        }

        public tblPlSalVou GetVoucher(int vrid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                return Data.tblPlSalVous.Where(voucher => voucher.Vrid.Equals(vrid)).SingleOrDefault();
            }
            catch
            { }
            return null;
        }

        public List<tblPlSalVouDet> GetVoucherDet(int vrid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                return Data.tblPlSalVouDets.Where(voucher=> voucher.Vrid.Equals(vrid)).ToList();
            }
            catch
            { }
            return null;
        }


        public int GetVoucherAmount(int vrid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                return Convert.ToInt32(Data.tblPlSalVouDets.Where(voucher => voucher.Vrid.Equals(vrid)).Sum(voucher => voucher.PayAmt));
            }
            catch
            { }
            return 0;
        }

        public bool SaveSalaryPayment(tblPlSalVou voucher, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                Data.tblPlSalVous.InsertOnSubmit(voucher);
                Data.SubmitChanges();
                return true;
            }
            catch
            { }
            return false;
        }

        public bool UpdateSalaryPayment(tblPlSalVou voucher, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                Data.SubmitChanges();
                return true;
            }
            catch
            { }
            return false;
        }


        public bool DeleteSalaryPayment(tblPlSalVou voucher, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                Data.tblPlSalVous.DeleteOnSubmit(voucher);
                Data.SubmitChanges();
                return true;
            }
            catch
            { }
            return false;
        }

        public object GetSalPayment(int compid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                var obj = (from a in Data.tblPlSalVous
                           join b in Data.tblPlSalVouDets
                           on a.Vrid equals b.Vrid
                           where a.CompID == compid && a.VouStatus != "C"
                           orderby a.VouDat descending
                           select new
                           {
                               Vrid = a.Vrid,
                               RefNo = a.VouRef,
                               RefDate = a.VouDat,
                               PayPerd = a.PayPerd,
                               Amount = (from c in Data.tblPlSalVouDets
                                         where c.Vrid == a.Vrid
                                         select c).Sum(c => c.PayAmt),
                               Status = a.VouStatus == "P" ? "Pending" : "Approved"
                               
                           }).Distinct().ToList();
                return obj.OrderByDescending(x=> x.RefDate).ToList();
            }
            catch { }
            return null;
        }


        public object GetBankBranch(RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                var obj = (from a in Data.tblBanks
                           select new
                           {
                               BankCode = a.BankCode,
                               BankBranchName = a.BankABv + " (" + a.BankName+")"
                           }).ToList();
                return obj;
            }
            catch { }
            return null;
        }

    }
}