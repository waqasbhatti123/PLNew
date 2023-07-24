using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using RMS.BL;
namespace RMS.InvSetupSupport
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]

   



    public class AutoCompleteSearch : System.Web.Services.WebService
    {

        InvSearch_BL sG = new InvSearch_BL();
        GlBudgetSetupBL Bgt = new GlBudgetSetupBL();
        POrderBL po = new POrderBL();
        GLReportParameterBL glRptParam = new GLReportParameterBL();


        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetCompletionListIGP(string prefixText, int count)
        {
            List<spGetSearchIGP4FeetageResult> tbl = sG.GetIGPs4FeetagSrch(prefixText);

            return tbl.Select(m => AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(m.VrNo + "," +m.Date+","+ m.Party , m.VrNo.ToString())).ToArray();

        }


        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetCompletionListMTNLot(string prefixText, int count)
        {
            List<vwLotNoMTN> tbl = sG.GetLot4MTNSrch(prefixText);

            return tbl.Select(m => AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(m.LotNo.Value.ToString().Substring(0, 4) + "-" + m.LotNo.Value.ToString().Substring(4), m.LotNo.Value.ToString())).ToArray();

        }


        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetCompletionListGradingLot(string prefixText, int count)
        {
            List<vwLotNoGradingCard> tbl = sG.GetLot4GradingSrch(prefixText);

            return tbl.Select(m => AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(m.LotNo.Value.ToString().Substring(0, 4) + "-" + m.LotNo.Value.ToString().Substring(4), m.LotNo.Value.ToString())).ToArray();

        }


        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetCompletionListMPNLot(string prefixText, int count)
        {
            List<vwLotNoMPN> tbl = sG.GetLot4PPNSrch(prefixText);

            return tbl.Select(m => AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(m.LotNo.Value.ToString().Substring(0, 4) + "-" + m.LotNo.Value.ToString().Substring(4), m.LotNo.Value.ToString())).ToArray();

        }

        //----------------For BudgetSetup AutoComplete for A/C Range

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] getAcRange(string prefixText, int count)
        {
            List<Glmf_Code> tbl = Bgt.AutoCompletAcRange(prefixText);

            return tbl.Select(m=> AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(m.gl_cd+"-"+m.gl_dsc,m.gl_cd)).ToArray();
        }

        //------------------For Budget Entry AutoComplete for BudgetCode
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]

        public string[] AutoCompletBgtCode(string prefixText, int count)
        {
            List<tblBgtHead> tbl = Bgt.AutoCompletBgtCode(prefixText);
            return tbl.Select(m => AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(m.Bgt_Code + "-" + m.Headg_Desc,m.Bgt_Code)).ToArray();
        }


        //------------------ PO Request Search
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]

        public string[] AutoCompletSrchPOReq(string prefixText, int count)
        {
            List<spSrchPOReqResult> tbl = po.AutoCompletSrchPOReq(prefixText);
            return tbl.Select(m => AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(m.vr_no_formated + ",     " + m.DocRef +",     " + m.DeptNme + ",     " + m.cc_nme, m.vr_id.ToString())).ToArray();
        }


        //------------------ PO Search
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]

        public string[] AutoCompletSrchPO(string prefixText, int count, string contextKey)
        {
            List<spSrchPOResult> tbl = po.AutoCompletSrchPO(prefixText, contextKey);
            return tbl.Select(m => AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(m.vr_no_formated + ",     " + m.gl_dsc , m.vr_id.ToString())).Distinct().ToArray();
        }


        //------------------ Cost Center
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetCostCenter(string prefixText, int count)
        {
            List<Cost_Center> tbl = Bgt.AutoCompletCostCenter(prefixText);

            return tbl.Select(m => AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(m.cc_cd + "-" + m.cc_nme, m.cc_cd)).ToArray();
        }

        //------------------ Cost Center Group
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetCostCenterGroup(string prefixText, int count)
        {
            List<tblglCCGrp> tbl = glRptParam.GetCCGrpsForAutComp(prefixText);

            return tbl.Select(m => AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(m.CCGrp + "-" + m.GrpDesc, m.CCGrp.ToString())).ToArray();
        }
    }
}
