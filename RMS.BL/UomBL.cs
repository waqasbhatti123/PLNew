using System;
using System.Linq;
//using System.Transactions;


namespace RMS.BL
{
    public class UomBL
    {
        public UomBL()
        {

        }

        public IQueryable<Item_Uom> GetAllUom(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<Item_Uom> uom = from u in Data.Item_Uoms
                                                 orderby u.uom_dsc
                                                 select u;
                return uom;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public Item_Uom GetByID(int uomid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                Item_Uom uom = Data.Item_Uoms.Single(p => p.uom_cd == uomid);

                return uom;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public string GetUomDescByID(int uomid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                Item_Uom uom = Data.Item_Uoms.Single(p => p.uom_cd == uomid);

                return uom.uom_dsc;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public bool ISAlreadyExist(Item_Uom uom, RMSDataContext Data)
        {

            bool isalready = false;
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                IQueryable<Item_Uom> uoms = from u in Data.Item_Uoms
                                            where u.uom_cd != uom.uom_cd && u.uom_dsc == uom.uom_dsc
                                                 select u;

                if (uoms != null & uoms.Count<Item_Uom>() > 0)
                    isalready = true;
                return isalready;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public void Update(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public void Insert(Item_Uom uom, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.Item_Uoms.InsertOnSubmit(uom);
                Data.SubmitChanges();

            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
    }
}