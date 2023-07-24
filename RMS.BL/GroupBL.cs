using System;
using System.Linq;
//using System.Transactions;


namespace RMS.BL
{
    public class GroupBL
    {
        public GroupBL()
        {

        }

        public tblAppPrivilage GetPrivilageStatus(int groupId, int pid,RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                return Data.tblAppPrivilages.Where(prvlg => prvlg.GroupID.Equals(groupId) && prvlg.AmID.Equals(pid)).SingleOrDefault();
            }
            catch (Exception ex)
            {
                //RMSDB.SetNull();
                throw ex;
            }
            //return null;
        }

        public IQueryable<tblAppGroup> GetAll(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<tblAppGroup> groups = from grp in Data.tblAppGroups
                                                 orderby grp.GroupName
                                                 select grp;
                return groups;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public IQueryable<tblAppGroup> GetAllGroupsCombo(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                IQueryable<tblAppGroup> groups = from grp in Data.tblAppGroups
                                                 orderby grp.GroupName
                                                 select grp;
                return groups;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public bool ISAlreadyExist(tblAppGroup groupo, RMSDataContext Data)
        {

            bool isalready = false;
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                IQueryable<tblAppGroup> groups = from grp in Data.tblAppGroups
                                                 where grp.GroupID != groupo.GroupID && grp.GroupName == groupo.GroupName
                                                 select grp;

                if (groups != null & groups.Count<tblAppGroup>() > 0)
                    isalready = true;
                return isalready;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public tblAppGroup GetByID(int grpid, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                tblAppGroup groups = Data.tblAppGroups.Single(p => p.GroupID == grpid);

                return groups;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }


        public void DeleteByID(int groupid, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                tblAppGroup grp = Data.tblAppGroups.Single(p => p.GroupID == groupid);
                Data.tblAppGroups.DeleteOnSubmit(grp);
                Data.SubmitChanges();
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }

        }

        public void Insert(tblAppGroup grp, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.tblAppGroups.InsertOnSubmit(grp);
                Data.SubmitChanges();

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

        public void DeleteAllPrivilages(int groupid, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                var privilaes = from p in Data.tblAppPrivilages
                                where
                                p.GroupID == groupid
                                select p
                                    ;
                Data.tblAppPrivilages.DeleteAllOnSubmit(privilaes);
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }


        }
        public bool hasEnabledPrivilage(int groupID, RMSDataContext Data)
        {
            IQueryable<tblAppPrivilage> priviliges;
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                priviliges = from p in Data.tblAppPrivilages
                             where p.GroupID == groupID && (p.CanEdit || p.CanDel || p.CanAdd || p.CanPrint || p.Enabled)
                             select p;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }

            return priviliges.Count() != 0;
        }
    }
}