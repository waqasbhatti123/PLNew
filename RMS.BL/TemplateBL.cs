using System;
using System.Collections.Generic;
using System.Linq;
//using System.Transactions;


namespace RMS.BL
{
    public class TemplateBL
    {
        public TemplateBL()
        {

        }
        public Template GetTemplateByID(int templateId, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                Template template = Data.Templates.Single(p => p.TemplateID.Equals(templateId));

                return template;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
        public List<TemplateDetail> GetTemplateDetailsByID(int templateId, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                List<TemplateDetail> templateDetails = Data.TemplateDetails.Where(p => p.TemplateID.Equals(templateId)).ToList();

                return templateDetails;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
    }
}