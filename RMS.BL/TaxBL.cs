using System;
using System.Linq;
using System.Collections.Generic;
//using System.Transactions;


namespace RMS.BL
{
    public class TaxBL
    {

        RMSDataContext Data = new RMSDataContext();
        public TaxBL()
        {

        }


        

        public object GetPercentByTaxID(string taxId, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }


                DateTime currDate = RMS.BL.Common.MyDate(Data);
                DateTime maxDate = (from a in Data.tblTaxRates
                                    where a.TaxID == taxId && a.EffDate <= currDate && a.Status == "OP"
                                    select a).Max(d => d.EffDate);

                var heads = (from a in Data.tblTaxRates
                             where a.TaxID == taxId && a.EffDate == maxDate
                             select new
                             {
                                 a.TaxRate
                             }).ToList();


                return heads;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }

        public decimal GetPercentValByTaxID(string taxId, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }


                DateTime currDate = RMS.BL.Common.MyDate(Data);
                DateTime maxDate = (from a in Data.tblTaxRates
                                    where a.TaxID == taxId && a.EffDate <= currDate && a.Status == "OP"
                                    select a).Max(d => d.EffDate);

                var tax = (from a in Data.tblTaxRates
                             where a.TaxID == taxId && a.EffDate == maxDate
                             select a).Single().TaxRate;


                return tax;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }

        public decimal GetWHTByPoRef(int brid, int poref, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                string whtid = Data.tblPOrders.Where(po => po.br_id == brid && po.vr_no == poref).Single().WHTid;
                if (whtid != null)
                {
                    DateTime currDate = RMS.BL.Common.MyDate(Data);
                    DateTime maxDate = (from a in Data.tblTaxRates
                                        where a.TaxID == whtid && a.EffDate <= currDate && a.Status == "OP"
                                        select a).Max(d => d.EffDate);
                    return Data.tblTaxRates.Where(tx => tx.TaxID == whtid && tx.EffDate == maxDate).Single().TaxRate;
                }
                else
                    return 0;
            }
            catch //(Exception ex)
            {
                //RMSDB.closeConn(Data);
                //throw ex;
            }
            return 0;
        }

        public decimal GetGSTPercentByPORefItemCode(int brid, int poRef, string itmCd, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                string gstId = "";
                decimal percent = 0;
                int maxSeq = Convert.ToInt32(Data.tblPOrders.Where(po => po.br_id == brid && po.vr_no == poRef).Max(x => x.RevSeq));
                gstId = (from a in Data.tblPOrders
                         join b in Data.tblPOrderDets
                         on a.vr_id equals b.vr_id
                         where a.vr_no == poRef && b.itm_cd == itmCd && a.RevSeq == maxSeq
                         select b).Single().GSTid;
                if (gstId != null && gstId != "0")
                {
                    DateTime currDate = RMS.BL.Common.MyDate(Data);
                    DateTime maxDate = (from a in Data.tblTaxRates
                                        where a.TaxID == gstId && a.EffDate <= currDate && a.Status == "OP"
                                        select a).Max(d => d.EffDate);
                    percent = Data.tblTaxRates.Where(tx => tx.TaxID == gstId && tx.EffDate == maxDate).Single().TaxRate;
                }
                return percent;
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }

        public string GetGSTIdByPORefItemCode(int brid, int poRef, string itmCd, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                int maxSeq = Convert.ToInt32(Data.tblPOrders.Where(po => po.br_id == brid && po.vr_no == poRef).Max(x => x.RevSeq));
                string gstId = "";
                gstId = (from a in Data.tblPOrders
                         join b in Data.tblPOrderDets
                         on a.vr_id equals b.vr_id
                         where a.vr_no == poRef && b.itm_cd == itmCd && a.RevSeq == maxSeq
                         select b).Single().GSTid;
                if (gstId != null && gstId != "0")
                {
                    return gstId;
                }
                return "0";
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }

        public object GetAllTaxes(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                return Data.tblTaxes.AsQueryable();
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }

        public object GetWHTTaxes(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                var obj = from a in Data.tblTaxes
                          where a.Type == "WHT" //|| a.Type == "VAR"
                          select a;
                return obj.OrderBy(o => o.TaxID).ToList();
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }

        public object GetGSTTaxes(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                var obj = from a in Data.tblTaxes
                          where a.Type == "GST" || a.Type == "VAR"
                          select a;
                return obj.OrderBy(o => o.TaxID).ToList();
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }



        /*Shahbaz work*/

        public object GetTaxDesc(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var obj = (from a in Data.tblTaxes where a.TaxID != "VAR" select new { a.TaxDesc, TaxIdTaxType = a.TaxID.ToString() + ":" + a.Type });
                return obj.OrderBy(o=> o.TaxDesc).ToList();
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }
        public object BindGridTaxDesc(RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                var obj = (from a in Data.tblTaxes where a.TaxID != "VAR" select a);
                return obj.OrderBy(o => o.Type).ThenBy(p=> p.TaxDesc).ToList();
            }
            catch (Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }
        public object BindGridTaxRate(RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                var obj = (from a in Data.tblTaxRates
                           join b in Data.tblTaxes on a.TaxID equals b.TaxID
                           where a.TaxID != "VAR"
                           select new
                           {
                               a.TaxID,
                               a.TaxRate,
                               a.EffDate,
                               b.TaxDesc,
                               b.Type,
                               TaxIdTaxType = a.TaxID.ToString() +":"+ b.Type
                           });
                return obj.OrderBy(o => o.Type).ThenByDescending(e=> e.EffDate).ToList();
            }
            catch(Exception ex)
            {
                RMSDB.closeConn(Data);
                throw ex;
            }
        }
        public void insertTaxDesc(tblTax tax, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                Data.tblTaxes.InsertOnSubmit(tax);
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public void insertTaxRate(tblTaxRate taxRate, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                Data.tblTaxRates.InsertOnSubmit(taxRate);
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public int GetByType(string type, RMSDataContext Data)
        {
            int taxId;
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                List<int> lstint = new List<int>();

                List<tblTax> obj = Data.tblTaxes.Where(i => i.Type == type).OrderBy(o => o.TaxID).ToList();
                if (obj.Count > 0)
                {
                    foreach (tblTax id in obj)
                    {
                        if (id.Type == "GST")
                        {
                            int temp = Convert.ToInt32(id.TaxID.Substring(2));
                            lstint.Add(temp);
                        }
                        else
                        {
                            int temp = Convert.ToInt32(id.TaxID.Substring(3));
                            lstint.Add(temp);
                        }
                    }
                    taxId = lstint.Max() + 1;
                    return taxId;
                }
                else
                {
                    return taxId = 1;
                }
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public tblTax GetByIDTaxDesc(string taxID, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                tblTax tax = Data.tblTaxes.Single(p => p.TaxID == taxID);

                return tax;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public tblTaxRate GetByIDTaxRate(DateTime efDt, string taxRateID, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                tblTaxRate taxRate = Data.tblTaxRates.Where(p => p.EffDate == efDt && p.TaxID == taxRateID).Single();

                return taxRate;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public void UpdateTaxDesc(RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public void UpdateTaxRate(RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public bool CheckEffectiveDate(string type, DateTime efDt, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                var obj = (from a in Data.tblTaxRates
                           join b in Data.tblTaxes on a.TaxID equals b.TaxID
                           where b.Type == type
                           select new
                           {
                               a.EffDate
                           }).ToList();
                if (obj.Count > 0)
                {
                    DateTime dt = obj.Max(o => o.EffDate);

                    if (dt < efDt)
                        return true;
                    else
                        return false;
                }
                else
                {
                    return true;
                }
            
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public bool IsAlreadyExist(tblTax tax, RMSDataContext Data)
        {
            bool IsAlready = false;
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                var obj = (from a in Data.tblTaxes
                           where a.TaxDesc == tax.TaxDesc && tax.TaxID != a.TaxID
                           select a);
                if (obj != null && obj.Count() > 0)
                    IsAlready = true;
                return IsAlready;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
    }
}