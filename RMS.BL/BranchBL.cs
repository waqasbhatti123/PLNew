using System;
using System.Linq;

namespace RMS.BL
{
    public class BranchBL
    {
        public BranchBL()
        {

        }

        public static bool IsChequeNoExists(Glmf_Data_chq glmfchq, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<Glmf_Data_chq> list = Data.Glmf_Data_chqs.Where
                    (chq => chq.vr_chq_branch.Equals(glmfchq.vr_chq_branch)
                        &&  chq.vr_chq.Equals(glmfchq.vr_chq)).AsQueryable();

                if (list != null && list.Count<Glmf_Data_chq>() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public static bool IsChequeNoExists1(Glmf_Data_chq glmfchq, int brId, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<Glmf_Data_chq> list =

                    (from c in Data.Glmf_Data_chqs
                     join v in Data.Glmf_Datas
                     on c.vrid equals v.vrid
                     where c.vrid != glmfchq.vrid
                         && c.vr_chq_branch.Equals(glmfchq.vr_chq_branch)
                         && c.vr_chq.Equals(glmfchq.vr_chq)
                         && v.br_id == brId
                     select c).AsQueryable();





                    //Data.Glmf_Data_chqs.Where
                    //(chq => chq.vrid != glmfchq.vrid
                    //    && chq.vr_chq_branch.Equals(glmfchq.vr_chq_branch)
                    //    && chq.vr_chq.Equals(glmfchq.vr_chq)).AsQueryable();

                if (list != null && list.Count<Glmf_Data_chq>() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public IQueryable<Branch> GetDistric(int id,RMSDataContext Data)
        {
            IQueryable<Branch> diss = from dis in Data.Branches
                          where dis.br_idd == id
                          select dis;
            return diss;
        }
        

        public IQueryable<Branch> GetAll(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<Branch> branchs = from br in Data.Branches
                                                //orderby br.br_nme
                                                select br;
                return branchs;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

       
        public IQueryable<Branch> GetAllCompBranchCombo(string compid,RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<Branch> branchs = from br in Data.Branches
                                             where br.CompID.Value.ToString().Equals(compid)
                                             orderby br.br_nme
                                             select br;
                return branchs;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public IQueryable<tblCompany> GetAllCompaniesCombo(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<tblCompany> comps = from c in Data.tblCompanies
                                               orderby c.CompName
                                               select c;
                return comps;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool ISAlreadyExist(Branch brancho, RMSDataContext Data)
        {

            bool isalready = false;
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                IQueryable<Branch> branchs = from br in Data.Branches
                                                where br.br_id != brancho.br_id && br.br_nme == brancho.br_nme
                                                select br;

                if (branchs != null & branchs.Count<Branch>() > 0)
                    isalready = true;
                return isalready;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public Branch GetByID(int brid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                Branch branchs = Data.Branches.Single(p => p.br_id == brid);

                return branchs;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }


        public void DeleteByID(int branchid, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                Branch br = Data.Branches.Single(p => p.br_id == branchid);
                Data.Branches.DeleteOnSubmit(br);
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }

        }

        public void Insert(Branch br, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.Branches.InsertOnSubmit(br);
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public void Update(Branch br, RMSDataContext Data)
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
    }
}