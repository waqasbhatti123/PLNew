using System;
using System.Linq;
using System.Collections.Generic;
//using System.Transactions;


namespace RMS.BL
{
    public class ArtifactsBL
    {
        public ArtifactsBL()
        {

        }

        #region Artifacts Type

        //public List<tblSdPromArtType> GetAllArtifactTypes(RMSDataContext Data)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = RMSDB.GetOject(); }
        //        List<tblSdPromArtType> artifactTypes = (from artType in Data.tblSdPromArtTypes
        //                                                orderby artType.Desc
        //                                                select artType).ToList();
        //        return artifactTypes;
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }
        //}

        //public tblSdPromArtType GetArtifactTypeByID(int artifactTypeId, RMSDataContext Data)
        //{
        //    if (Data == null) { Data = RMSDB.GetOject(); }
        //    try
        //    {
        //        tblSdPromArtType artifactType = Data.tblSdPromArtTypes.Single(p => p.ArtTypeId == artifactTypeId);

        //        return artifactType;
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }
        //}

        //public bool ISAlreadyExistArtifactType(tblSdPromArtType artifactType, RMSDataContext Data)
        //{

        //    bool isAlready = false;
        //    if (Data == null) { Data = RMSDB.GetOject(); }
        //    try
        //    {
        //        IQueryable<tblSdPromArtType> artifactTypes = from artType in Data.tblSdPromArtTypes
        //                                                     where artType.ArtTypeId != artifactType.ArtTypeId && artType.Desc == artifactType.Desc
        //                                                     select artifactType;

        //        if (artifactTypes != null && artifactTypes.Count<tblSdPromArtType>() > 0)
        //        {
        //            isAlready = true;

        //        }

        //        return isAlready;
        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }
        //}

        //public void InsertArtifactType(tblSdPromArtType artifactType, RMSDataContext Data)
        //{
        //    try
        //    {
        //        if (Data == null) { Data = RMSDB.GetOject(); }
        //        Data.tblSdPromArtTypes.InsertOnSubmit(artifactType);
        //        Data.SubmitChanges();

        //    }
        //    catch (Exception ex)
        //    {
        //        RMSDB.SetNull();
        //        throw ex;
        //    }
        //}

        #endregion

        #region Artifacts

        public List<tblSdPromArt> GetAllArtifacts(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                List<tblSdPromArt> lsArt = (from prmArt in Data.tblSdPromArts
                                            orderby prmArt.Desc
                                            select prmArt).ToList();
                return lsArt;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public List<tblSdPromArt> GetAllArtifactsByArtTypeId(int artTypeId,RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                List<tblSdPromArt> lsArt = (from prmArt in Data.tblSdPromArts
                                            //where prmArt.ArtTypeId==artTypeId
                                            orderby prmArt.Desc
                                            select prmArt).ToList();
                return lsArt;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public Object GetAllArtifactsWithType(RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Object lsArt = from prmArt in Data.tblSdPromArts
                               //join artType in Data.tblSdPromArtTypes on prmArt.ArtTypeId equals artType.ArtTypeId
                               orderby prmArt.Desc
                               select new
                               {
                                   prmArt.ArtId,
                                   prmArt.Desc,
                                   prmArt.Enabled,
                                  // prmArt.ArtTypeId,
                                   //ArtType = artType.Desc
                               };
                return lsArt;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public tblSdPromArt GetByID(int artId, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                tblSdPromArt prmArt = Data.tblSdPromArts.Single(p => p.ArtId == artId);
                return prmArt;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public bool ISAlreadyExist(tblSdPromArt artifact, RMSDataContext Data)
        {

            bool isAlready = false;
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                IQueryable<tblSdPromArt> artifacts = from art in Data.tblSdPromArts
                                                     where art.ArtId != artifact.ArtId && art.Desc == artifact.Desc
                                                     select art;

                if (artifacts != null && artifacts.Count<tblSdPromArt>() > 0)
                {
                    isAlready = true;
                }
                return isAlready;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        public void Insert(tblSdPromArt artifact, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.tblSdPromArts.InsertOnSubmit(artifact);
                Data.SubmitChanges();

            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }

        #endregion

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
    }
}