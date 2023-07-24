using RMS.BL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.BL
{
    public class BudgetBL
    {
        public BudgetBL()
        {

        }




        public List<BudgetModel> GetDeevBudget(RMSDataContext Data, int budgetTypeID, decimal glYear, int brID, int SchIDd)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            var budgetHeads = Data.BudgetHeads.Where(x => x.IsActive).ToList();


            var codes = (from c in Data.Glmf_Codes
                         where c.cnt_gl_cd == "010301" &&
                          c.ct_id == "D"
                         select c).ToList();

            var list = new List<BudgetModel>();
            foreach (var code in codes)
            {
                var b = new BudgetModel();
                b.Account = code.gl_cd;
                b.GlYear = glYear;
                b.GlDesc = (code.code != null ? code.code : "") + " - " + code.gl_dsc;

                var dbBudget = Data.Budgets.Where(x => x.BudgetTypeID == budgetTypeID && x.GlYear == glYear && x.Account == code.gl_cd && x.br_id == brID && x.QuarterID == 0 && x.SchemeID == SchIDd).SingleOrDefault();
                if (dbBudget != null)
                {
                    b.Income = dbBudget.Income;
                    b.Grant = dbBudget.Grant;
                    b.Aid = dbBudget.Aid;
                }
                else
                {
                    //b.Income = 0;
                    //b.Grant = 0;
                    //b.Aid = 0;
                }

                list.Add(b);
            }

            return list.OrderBy(x => x.Account).ToList();
        }


        public List<BudgetModel> GetBudget(RMSDataContext Data, int budgetTypeID, decimal glYear, int brID)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            var budgetHeads = Data.BudgetHeads.Where(x => x.IsActive).ToList();


            var codes = (from bh in budgetHeads
                         join c in Data.Glmf_Codes
                         on bh.Account equals c.cnt_gl_cd
                         where c.ct_id == "D"
                         select c).ToList();

            var list = new List<BudgetModel>();
            foreach (var code in codes)
            {
                var b = new BudgetModel();
                b.Account = code.gl_cd;
                b.GlYear = glYear;
                b.GlDesc = (code.code != null ? code.code: "" ) +" - "+ code.gl_dsc;

                var dbBudget = Data.Budgets.Where(x => x.BudgetTypeID == budgetTypeID && x.GlYear == glYear && x.Account == code.gl_cd && x.br_id == brID && x.QuarterID == 0).SingleOrDefault();
                if (dbBudget != null)
                {
                    b.Income = dbBudget.Income;
                    b.Grant = dbBudget.Grant;
                    b.Aid = dbBudget.Aid;
                }
                else
                {
                    //b.Income = 0;
                    //b.Grant = 0;
                    //b.Aid = 0;
                }

                list.Add(b);
            }

            return list.OrderBy(x => x.Account).ToList();
        }

        public List<BudgetModel> GetSNE(RMSDataContext Data, int budgetTypeID, decimal glYear, int brID)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            var budgetHeads = Data.BudgetHeads.Where(x => x.IsActive).ToList();


            var codes = (from bh in budgetHeads
                         join c in Data.Glmf_Codes
                         on bh.Account equals c.cnt_gl_cd
                         where c.ct_id == "D"
                         select c).ToList();

            var list = new List<BudgetModel>();
            foreach (var code in codes)
            {
                var b = new BudgetModel();
                b.Account = code.gl_cd;
                b.GlYear = glYear;
                b.GlDesc = (code.code != null ? code.code : "") + " - " + code.gl_dsc;

                var dbBudget = Data.Budgets.Where(x => x.BudgetTypeID == budgetTypeID && x.GlYear == glYear && x.Account == code.gl_cd && x.br_id == brID && x.QuarterID == 0).SingleOrDefault();
                if (dbBudget != null)
                {
                    b.Income = dbBudget.Income;
                    b.Grant = dbBudget.Grant;
                    b.Aid = dbBudget.Aid;
                }
                else
                {
                    //b.Income = 0;
                    //b.Grant = 0;
                    //b.Aid = 0;
                }

                list.Add(b);
            }

            return list.OrderBy(x => x.Account).ToList();
        }


        public List<BudgetModel> GetExpenditure(RMSDataContext Data, int budgetTypeID, decimal glYear, int brID)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            var budgetHeads = Data.BudgetHeads.Where(x => x.IsActive).ToList();


            var codes = (from bh in budgetHeads
                         join c in Data.Glmf_Codes
                         on bh.Account equals c.cnt_gl_cd
                         where c.ct_id == "D" && bh.IsActive
                         select c).ToList();

            List<Sp_ExpenditureStatementHeadWiseResult> exp = Data.Sp_ExpenditureStatementHeadWise(brID, glYear).ToList();

            var list = new List<BudgetModel>();
            foreach (var code in exp)
            {
                var b = new BudgetModel();
                b.Account = code.glcd;
                b.GlDesc = (code.code != null ? code.code : "") + " - " + code.Account;
                b.ParentAccount = code.ParentCode;
                b.Expenditure = code.Expense;
                //var b = new BudgetModel();
                //b.Account = code.gl_cd;
                //b.GlYear = glYear;
                //b.GlDesc = (code.code != null ? code.code : "") + " - " + code.gl_dsc;
                //b.ParentAccount = code.cnt_gl_cd;

                ////var dbBudget = Data.Budgets.Where(x => x.BudgetTypeID == budgetTypeID && x.GlYear == glYear && x.Account == code.gl_cd && x.br_id == brID && x.QuarterID == 0).SingleOrDefault();
                //var dbBudget = Data.Budgets.Where(x => x.BudgetTypeID == budgetTypeID && x.GlYear == glYear && x.Account == code.gl_cd && x.br_id == brID && x.QuarterID == 0).SingleOrDefault();
                //if (dbBudget != null)
                //{
                //    b.Income = dbBudget.Income;
                //    b.Grant = dbBudget.Grant;
                //    b.Aid = dbBudget.Aid;
                //    b.ParentAccount = dbBudget.Account;
                //}
                //else
                //{
                //    //b.Income = 0;
                //    //b.Grant = 0;
                //    //b.Aid = 0;
                //}

                list.Add(b);
            }

            return list.OrderBy(x => x.Account).ToList();
        }

        public List<ExcessSurrenderModel> GetExcessSurrender1(RMSDataContext Data, int budgetTypeID, decimal glYear, int brID)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            var budgetHeads = Data.BudgetHeads.Where(x => x.IsActive).ToList();


            var codes = (from bh in budgetHeads
                         join c in Data.Glmf_Codes
                         on bh.Account equals c.cnt_gl_cd
                         where c.ct_id == "D" && bh.IsActive
                         select c).ToList();

            var list = new List<ExcessSurrenderModel>();
            var excess = Data.ExcessSurrenders.Where(x => x.brId == brID && x.glyear == glYear && x.ESID == 1).ToList();

            if (excess.Count != 0)
            {
                foreach (var i in excess)
                {
                    var b = new ExcessSurrenderModel();
                    b.Accounts = i.HeadOfAccount;
                    b.code = Data.Glmf_Codes.Where(x => x.gl_cd == i.HeadOfAccount).FirstOrDefault().code;
                    b.Gldsc =  Data.Glmf_Codes.Where(x => x.gl_cd == i.HeadOfAccount).FirstOrDefault().gl_dsc;
                    b.AppBudget = Convert.ToDecimal(i.OriBudget);
                    b.SupplementryBudget = Convert.ToDecimal(i.SupBudget);
                    b.ApprovedAmountReapp = Convert.ToDecimal(i.AppAmoun);
                    b.ModifiedEstimateBudget = Convert.ToDecimal(i.ModifiedBudget);
                    b.fourMonthExpCurrentFinancialYear = Convert.ToDecimal(i.CurrentYearExp);
                    b.eightmonthpreviousFinancialYear = Convert.ToDecimal(i.PreYearExp);
                    b.TotalActual = Convert.ToDecimal(i.TotalActual);
                    b.ProposedReappropriationbyBudgetAD = Convert.ToDecimal(i.ProposedReapproperaition);
                    b.AntiRevisedExpcurrentyear = Convert.ToDecimal(i.AnticipatedRevisedExp);
                    b.Excess = Convert.ToDecimal(i.Excess);
                    b.Surrender = Convert.ToDecimal(i.Surrender);
                    list.Add(b);
                }
            }
            else
            {
            
            List<sp_ExcessSurrender1Result> exp = Data.sp_ExcessSurrender1(brID, glYear).ToList();
            
            foreach (var code in exp)
            {
                var b = new ExcessSurrenderModel();
                b.code = (code.code != null? code.code : "");
                    b.Accounts = code.glcd;
                b.Gldsc =  code.Account;
                //b.ParentAccount = code.ParentCode;
                b.AppBudget = Convert.ToDecimal(code.ApprovedGrant);
                b.fourMonthExpCurrentFinancialYear = Convert.ToDecimal(code.Expense4Months);
                b.eightmonthpreviousFinancialYear = Convert.ToDecimal(code.Expense8Months);
                //}

                list.Add(b);
            }
            }
            return list.OrderBy(x => x.Accounts).ToList();
        }

        public List<ExcessSurrenderModel> GetExcessSurrender2(RMSDataContext Data, int budgetTypeID, decimal glYear, int brID)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            var budgetHeads = Data.BudgetHeads.Where(x => x.IsActive).ToList();


            var codes = (from bh in budgetHeads
                         join c in Data.Glmf_Codes
                         on bh.Account equals c.cnt_gl_cd
                         where c.ct_id == "D" && bh.IsActive
                         select c).ToList();

            var list = new List<ExcessSurrenderModel>();

            var excess = Data.ExcessSurrenders.Where(x => x.brId == brID && x.glyear == glYear && x.ESID == 2).ToList();

            if (excess.Count != 0)
            {
                foreach (var i in excess)
                {
                    var b = new ExcessSurrenderModel();
                    b.Accounts = i.HeadOfAccount;
                    b.code = Data.Glmf_Codes.Where(x => x.gl_cd == i.HeadOfAccount).FirstOrDefault().code;
                    b.Gldsc = Data.Glmf_Codes.Where(x => x.gl_cd == i.HeadOfAccount).FirstOrDefault().gl_dsc;
                    b.AppBudget = Convert.ToDecimal(i.OriBudget);
                    b.SupplementryBudget = Convert.ToDecimal(i.SupBudget);
                    b.ApprovedAmountReapp = Convert.ToDecimal(i.AppAmoun);
                    b.ModifiedEstimateBudget = Convert.ToDecimal(i.ModifiedBudget);
                    b.eightmonthcurrentfinancialyear = Convert.ToDecimal(i.CurrentYearExp);
                    b.fourmonthpreviousfinancialyer = Convert.ToDecimal(i.PreYearExp);
                    b.TotalActual = Convert.ToDecimal(i.TotalActual);
                    b.ProposedReappropriationbyBudgetAD = Convert.ToDecimal(i.ProposedReapproperaition);
                    b.AntiRevisedExpcurrentyear = Convert.ToDecimal(i.AnticipatedRevisedExp);
                    b.Excess = Convert.ToDecimal(i.Excess);
                    b.Surrender = Convert.ToDecimal(i.Surrender);
                    list.Add(b);
                }
            }
            else
            { 
            List<sp_ExcessSurrender2Result> exp = Data.sp_ExcessSurrender2(brID, glYear).ToList();
                           
            foreach (var code in exp)
            {
                var b = new ExcessSurrenderModel();
                b.Accounts = code.glcd;
                    b.code = code.code;
                b.Gldsc = code.Account;
                //b.ParentAccount = code.ParentCode;
                b.AppBudget = Convert.ToDecimal(code.ApprovedGrant);
                b.eightmonthcurrentfinancialyear = Convert.ToDecimal(code.Expense8Months);
                b.fourmonthpreviousfinancialyer = Convert.ToDecimal(code.Expense4Months);
                

                list.Add(b);
            }

            }

            return list.OrderBy(x => x.Accounts).ToList();
        }

        public List<SNEModel> GetSNERelease(RMSDataContext Data, decimal glyear, int brID)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            var bugdetHeads = Data.BudgetHeads.Where(x => x.IsActive).ToList();

            var codes = (from bh in bugdetHeads
                         join c in Data.Glmf_Codes
                         on bh.Account equals c.cnt_gl_cd
                         where c.ct_id == "D"
                         select c).ToList();
            var list = new List<SNEModel>();
            foreach (var code in codes)
            {
                var b = new SNEModel();
                b.Account = code.gl_cd;
                b.GlYear = glyear;
                b.GlDesc = (code.code != null ? code.code : "") + " - " + code.gl_dsc;
                var dbBudget = Data.Budgets.Where(x => x.BudgetTypeID == 4 && x.GlYear == glyear && x.Account == code.gl_cd && x.br_id == brID && x.QuarterID == 0).SingleOrDefault();
                if (dbBudget != null)
                {

                    b.ApGrant = dbBudget.Grant;

                }

                var dbSNE = Data.Budgets.Where(x => x.BudgetTypeID == 3 && x.GlYear == glyear && x.Account == code.gl_cd && x.br_id == brID && x.QuarterID == 0).SingleOrDefault();
                if (dbSNE != null)
                {
                    b.Aid = dbSNE.Grant;
                }
                list.Add(b);
            }
            return list.OrderBy(x => x.Account).ToList();
        }


        public List<BudgetModel> GetGrantRelease(RMSDataContext Data, int budgetTypeID, decimal glYear, int brID)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }

            var budgetHeads = Data.BudgetHeads.Where(x => x.IsActive).ToList();


            var codes = (from bh in budgetHeads
                         join c in Data.Glmf_Codes
                         on bh.Account equals c.cnt_gl_cd
                         where c.ct_id == "D"
                         select c).ToList();

            var list = new List<BudgetModel>();
            foreach (var code in codes)
            {
                var b = new BudgetModel();
                b.Account = code.gl_cd;
                b.GlYear = glYear;
                b.GlDesc = (code.code != null ? code.code : "") + " - " + code.gl_dsc;

                var dbBudget = Data.Budgets.Where(x => x.BudgetTypeID == budgetTypeID && x.GlYear == glYear && x.Account == code.gl_cd && x.br_id == brID && x.QuarterID == 0).SingleOrDefault();
                if (dbBudget != null)
                {

                    b.Grant = dbBudget.Grant;

                }
                else
                {
                    //b.Income = 0;
                    //b.Grant = 0;
                    //b.Aid = 0;
                }

                var quarter1 = Data.Budgets.Where(x => x.BudgetTypeID == budgetTypeID && x.GlYear == glYear && x.Account == code.gl_cd && x.br_id == brID && x.QuarterID == 1).SingleOrDefault();
                if (quarter1 != null)
                {
                    b.GrantinQ1 = quarter1.Grant;
                }

                var quarter2 = Data.Budgets.Where(x => x.BudgetTypeID == budgetTypeID && x.GlYear == glYear && x.Account == code.gl_cd && x.br_id == brID && x.QuarterID == 2).SingleOrDefault();
                if (quarter2 != null)
                {
                    b.GrantinQ2 = quarter2.Grant;
                }
                var quarter3 = Data.Budgets.Where(x => x.BudgetTypeID == budgetTypeID && x.GlYear == glYear && x.Account == code.gl_cd && x.br_id == brID && x.QuarterID == 3).SingleOrDefault();
                if (quarter3 != null)
                {
                    b.GrantinQ3 = quarter3.Grant;
                }
                var quarter4 = Data.Budgets.Where(x => x.BudgetTypeID == budgetTypeID && x.GlYear == glYear && x.Account == code.gl_cd && x.br_id == brID && x.QuarterID == 4).SingleOrDefault();
                if (quarter4 != null)
                {
                    b.GrantinQ4 = quarter4.Grant;
                }

                var FirstExe = Data.Budgets.Where(x => x.BudgetTypeID == budgetTypeID && x.GlYear == glYear && x.Account == code.gl_cd && x.br_id == brID && x.QuarterID == 5).SingleOrDefault();
                if (FirstExe != null)
                {
                    b.firstExcess = FirstExe.Grant;
                }
                var SeocndExe = Data.Budgets.Where(x => x.BudgetTypeID == budgetTypeID && x.GlYear == glYear && x.Account == code.gl_cd && x.br_id == brID && x.QuarterID == 6).SingleOrDefault();
                if (SeocndExe != null)
                {
                    b.secondExcess = SeocndExe.Grant;
                }
                var firstappp = Data.Budgets.Where(x => x.BudgetTypeID == budgetTypeID && x.GlYear == glYear && x.Account == code.gl_cd && x.br_id == brID && x.QuarterID == 7).SingleOrDefault();
                if (firstappp != null)
                {
                    b.firstapprop = firstappp.Grant;
                }
                var SecondApp = Data.Budgets.Where(x => x.BudgetTypeID == budgetTypeID && x.GlYear == glYear && x.Account == code.gl_cd && x.br_id == brID && x.QuarterID == 8).SingleOrDefault();
                if (SecondApp != null)
                {
                    b.secondapprop = SecondApp.Grant;
                }
                var thirdAppp = Data.Budgets.Where(x => x.BudgetTypeID == budgetTypeID && x.GlYear == glYear && x.Account == code.gl_cd && x.br_id == brID && x.QuarterID == 9).SingleOrDefault();
                if (thirdAppp != null)
                {
                    b.thirdapprop = thirdAppp.Grant;
                }
                var forhtapp = Data.Budgets.Where(x => x.BudgetTypeID == budgetTypeID && x.GlYear == glYear && x.Account == code.gl_cd && x.br_id == brID && x.QuarterID == 10).SingleOrDefault();
                if (forhtapp != null)
                {
                    b.forthapprop = forhtapp.Grant;
                }

                var firstsurender = Data.Budgets.Where(x => x.BudgetTypeID == budgetTypeID && x.GlYear == glYear && x.Account == code.gl_cd && x.br_id == brID && x.QuarterID == 11).SingleOrDefault();
                if (forhtapp != null)
                {
                    b.firstSurrender = firstsurender.Grant;
                }

                var secondsurrender = Data.Budgets.Where(x => x.BudgetTypeID == budgetTypeID && x.GlYear == glYear && x.Account == code.gl_cd && x.br_id == brID && x.QuarterID == 12).SingleOrDefault();
                if (forhtapp != null)
                {
                    b.secondSurrender = secondsurrender.Grant;
                }

                list.Add(b);
            }

            return list.OrderBy(x => x.Account).ToList();
        }

        public string SubmitDeveBud(decimal glYear, int budgetTypeID, int BranchId, List<RMS.BL.Budget> budgets, int schID , RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }



                List<RMS.BL.Budget> aInserted = new List<RMS.BL.Budget>();
                List<RMS.BL.Budget> aUpdated = new List<RMS.BL.Budget>();

                var dbAll = Data.Budgets.Where(x => x.BudgetTypeID == budgetTypeID && x.GlYear == glYear && x.br_id == BranchId && x.QuarterID == 0 && x.SchemeID == schID).ToList();

                bool found = false;
                foreach (var a in budgets)
                {
                    foreach (var dba in dbAll)
                    {
                        if (a.Account == dba.Account)
                        {
                            dba.Grant = a.Grant;
                            dba.IsActive = true;
                            aUpdated.Add(dba);
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        var newAll = new RMS.BL.Budget();
                        newAll.BudgetTypeID = budgetTypeID;
                        newAll.GUID = a.GUID;
                        newAll.QuarterID = 0;
                        newAll.GlYear = glYear;
                        newAll.Account = a.Account;
                        newAll.Income = a.Income;
                        newAll.Grant = a.Grant;
                        newAll.Aid = a.Aid;
                        newAll.br_id = BranchId;
                        newAll.SchemeID = schID;
                        newAll.IsActive = true;

                        aInserted.Add(newAll);
                    }
                }

                Data.Budgets.InsertAllOnSubmit(aInserted);

                Data.SubmitChanges();
                return "OK";

            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                return ex.Message.ToString();
                //throw ex;

            }
        }

        public string Submit(decimal glYear, int budgetTypeID,int BranchId, List<RMS.BL.Budget> budgets, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }


              
                List<RMS.BL.Budget> aInserted = new List<RMS.BL.Budget>();
                List<RMS.BL.Budget> aUpdated = new List<RMS.BL.Budget>();

                var dbAll = Data.Budgets.Where(x => x.BudgetTypeID == budgetTypeID && x.GlYear == glYear && x.br_id == BranchId && x.QuarterID == 0).ToList();

                bool found = false;
                foreach (var a in budgets)
                {
                    foreach (var dba in dbAll)
                    {
                        if (a.Account == dba.Account)
                        {
                            dba.Income = a.Income;
                            dba.Grant = a.Grant;
                            dba.Aid = a.Aid;
                            dba.IsActive = true;
                            aUpdated.Add(dba);
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        var newAll = new RMS.BL.Budget();
                        newAll.BudgetTypeID = budgetTypeID;
                        newAll.GUID = a.GUID;
                        newAll.QuarterID = 0;
                        newAll.GlYear = glYear;
                        newAll.Account = a.Account;
                        newAll.Income = a.Income;
                        newAll.Grant = a.Grant;
                        newAll.Aid = a.Aid;
                        newAll.br_id = BranchId;

                        newAll.IsActive = true;

                        aInserted.Add(newAll);
                    }
                }

                Data.Budgets.InsertAllOnSubmit(aInserted);

                Data.SubmitChanges();
                return "OK";

            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                return ex.Message.ToString();
                //throw ex;
                
            }
        }

        public string SubmitExcess1(decimal glYear, int BranchId, int esid ,List<RMS.BL.ExcessSurrender> excexx, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }



                List<RMS.BL.ExcessSurrender> aInserted = new List<RMS.BL.ExcessSurrender>();
                List<RMS.BL.ExcessSurrender> aUpdated = new List<RMS.BL.ExcessSurrender>();

                var dbAll = Data.ExcessSurrenders.Where(x =>  x.glyear == glYear && x.brId == BranchId && x.ESID == esid).ToList();

                bool found = false;
                foreach (var a in excexx)
                {
                    foreach (var dba in dbAll)
                    {
                        if (a.HeadOfAccount == dba.HeadOfAccount)
                        {
                            dba.HeadOfAccount = a.HeadOfAccount;
                            dba.HeadCode = a.HeadCode;
                            dba.OriBudget = a.OriBudget;
                            dba.SupBudget = a.SupBudget;
                            dba.AppAmoun = a.AppAmoun;
                            dba.ModifiedBudget = a.ModifiedBudget;
                            dba.CurrentYearExp = a.CurrentYearExp;
                            dba.PreYearExp = a.PreYearExp;
                            dba.TotalActual = a.TotalActual;
                            dba.ProposedReapproperaition = a.ProposedReapproperaition;
                            dba.AnticipatedRevisedExp = a.AnticipatedRevisedExp;
                            dba.Excess = a.Excess;
                            dba.Surrender = a.Surrender;
                            dba.brId = BranchId;
                            dba.glyear = glYear;
                            dba.ESID = 1;
                            aUpdated.Add(dba);
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        var newAll = new RMS.BL.ExcessSurrender();
                        newAll.HeadOfAccount = a.HeadOfAccount;
                        newAll.HeadCode = a.HeadCode;
                        newAll.OriBudget = a.OriBudget;
                        newAll.SupBudget = a.SupBudget;
                        newAll.AppAmoun = a.AppAmoun;
                        newAll.ModifiedBudget = a.ModifiedBudget;
                        newAll.CurrentYearExp = a.CurrentYearExp;
                        newAll.PreYearExp = a.PreYearExp;
                        newAll.TotalActual = a.TotalActual;
                        newAll.ProposedReapproperaition = a.ProposedReapproperaition;
                        newAll.AnticipatedRevisedExp = a.AnticipatedRevisedExp;
                        newAll.Excess = a.Excess;
                        newAll.Surrender = a.Surrender;
                        newAll.brId = BranchId;
                        newAll.glyear = glYear;
                        newAll.ESID = 1;
                        aInserted.Add(newAll);
                    }
                }

                Data.ExcessSurrenders.InsertAllOnSubmit(aInserted);

                Data.SubmitChanges();
                return "OK";

            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                return ex.Message.ToString();
                //throw ex;

            }
        }

        public string SubmitExcess2(decimal glYear, int BranchId, int esid, List<RMS.BL.ExcessSurrender> excexx, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }



                List<RMS.BL.ExcessSurrender> aInserted = new List<RMS.BL.ExcessSurrender>();
                List<RMS.BL.ExcessSurrender> aUpdated = new List<RMS.BL.ExcessSurrender>();

                var dbAll = Data.ExcessSurrenders.Where(x => x.glyear == glYear && x.brId == BranchId && x.ESID == esid).ToList();

                bool found = false;
                foreach (var a in excexx)
                {
                    foreach (var dba in dbAll)
                    {
                        if (a.HeadOfAccount == dba.HeadOfAccount)
                        {
                            dba.HeadOfAccount = a.HeadOfAccount;
                            dba.HeadCode = a.HeadCode;
                            dba.OriBudget = a.OriBudget;
                            dba.SupBudget = a.SupBudget;
                            dba.AppAmoun = a.AppAmoun;
                            dba.ModifiedBudget = a.ModifiedBudget;
                            dba.CurrentYearExp = a.CurrentYearExp;
                            dba.PreYearExp = a.PreYearExp;
                            dba.TotalActual = a.TotalActual;
                            dba.ProposedReapproperaition = a.ProposedReapproperaition;
                            dba.AnticipatedRevisedExp = a.AnticipatedRevisedExp;
                            dba.Excess = a.Excess;
                            dba.Surrender = a.Surrender;
                            dba.brId = BranchId;
                            dba.glyear = glYear;
                            dba.ESID = 2;
                            aUpdated.Add(dba);
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        var newAll = new RMS.BL.ExcessSurrender();
                        newAll.HeadOfAccount = a.HeadOfAccount;
                        newAll.HeadCode = a.HeadCode;
                        newAll.OriBudget = a.OriBudget;
                        newAll.SupBudget = a.SupBudget;
                        newAll.AppAmoun = a.AppAmoun;
                        newAll.ModifiedBudget = a.ModifiedBudget;
                        newAll.CurrentYearExp = a.CurrentYearExp;
                        newAll.PreYearExp = a.PreYearExp;
                        newAll.TotalActual = a.TotalActual;
                        newAll.ProposedReapproperaition = a.ProposedReapproperaition;
                        newAll.AnticipatedRevisedExp = a.AnticipatedRevisedExp;
                        newAll.Excess = a.Excess;
                        newAll.Surrender = a.Surrender;
                        newAll.brId = BranchId;
                        newAll.glyear = glYear;
                        newAll.ESID = 2;
                        aInserted.Add(newAll);
                    }
                }

                Data.ExcessSurrenders.InsertAllOnSubmit(aInserted);

                Data.SubmitChanges();
                return "OK";

            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                return ex.Message.ToString();
                //throw ex;

            }
        }


        public string SubmitAllocation(decimal glYear, int budgetTypeID, int BranchId, List<RMS.BL.Budget> budgets, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }



                List<RMS.BL.Budget> aInserted = new List<RMS.BL.Budget>();
                List<RMS.BL.Budget> aUpdated = new List<RMS.BL.Budget>();

                var dbAll = Data.Budgets.Where(x => x.BudgetTypeID == budgetTypeID && x.GlYear == glYear && x.br_id == BranchId && x.QuarterID == 0).ToList();

                bool found = false;
                foreach (var a in budgets)
                {
                    foreach (var dba in dbAll)
                    {
                        if (a.Account == dba.Account)
                        {
                            dba.Income = a.Income;
                            dba.Grant = a.Grant;
                            dba.Aid = a.Aid;
                            dba.IsActive = true;
                            aUpdated.Add(dba);
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        var newAll = new RMS.BL.Budget();
                        newAll.BudgetTypeID = budgetTypeID;
                        newAll.GUID = a.GUID;
                        newAll.QuarterID = 0;
                        newAll.GlYear = glYear;
                        newAll.Account = a.Account;
                        newAll.Income = a.Income;
                        newAll.Grant = a.Grant;
                        newAll.Aid = a.Aid;
                        newAll.br_id = BranchId;

                        newAll.IsActive = true;

                        aInserted.Add(newAll);
                    }
                }

                Data.Budgets.InsertAllOnSubmit(aInserted);

                Data.SubmitChanges();
                return "OK";

            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                return ex.Message.ToString();
                //throw ex;

            }
        }

        // Submit SNE
        public string SubmitSNE(decimal glYear, int budgetTypeID, int BranchId, List<RMS.BL.Budget> budgets, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }



                List<RMS.BL.Budget> aInserted = new List<RMS.BL.Budget>();
                List<RMS.BL.Budget> aUpdated = new List<RMS.BL.Budget>();

                var dbAll = Data.Budgets.Where(x => x.BudgetTypeID == budgetTypeID && x.GlYear == glYear && x.br_id == BranchId && x.QuarterID == 0).ToList();

                bool found = false;
                foreach (var a in budgets)
                {
                    foreach (var dba in dbAll)
                    {
                        if (a.Account == dba.Account)
                        {
                            dba.Grant = a.Grant;
                            dba.IsActive = true;
                            aUpdated.Add(dba);
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        var newAll = new RMS.BL.Budget();
                        newAll.BudgetTypeID = budgetTypeID;
                        newAll.GUID = a.GUID;
                        newAll.QuarterID = 0;
                        newAll.GlYear = glYear;
                        newAll.Account = a.Account;
                        newAll.Grant = a.Grant;
                        newAll.br_id = BranchId;

                        newAll.IsActive = true;

                        aInserted.Add(newAll);
                    }
                }

                Data.Budgets.InsertAllOnSubmit(aInserted);

                Data.SubmitChanges();
                return "OK";

            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                return ex.Message.ToString();
                //throw ex;

            }
        }

        public string SubmitExpenditure(decimal glYear, int budgetTypeID, int BranchId, List<RMS.BL.Budget> budgets, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }



                List<RMS.BL.Budget> aInserted = new List<RMS.BL.Budget>();
                List<RMS.BL.Budget> aUpdated = new List<RMS.BL.Budget>();

                var dbAll = Data.Budgets.Where(x => x.BudgetTypeID == budgetTypeID && x.GlYear == glYear && x.br_id == BranchId && x.QuarterID == 0).ToList();

                bool found = false;
                foreach (var a in budgets)
                {
                    foreach (var dba in dbAll)
                    {
                        if (a.Account == dba.Account)
                        {
                            dba.Grant = a.Grant;
                            dba.IsActive = true;
                            aUpdated.Add(dba);
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        var newAll = new RMS.BL.Budget();
                        newAll.BudgetTypeID = budgetTypeID;
                        newAll.GUID = a.GUID;
                        newAll.QuarterID = 0;
                        newAll.GlYear = glYear;
                        newAll.Account = a.Account;
                        newAll.Grant = a.Grant;
                        newAll.br_id = BranchId;

                        newAll.IsActive = true;

                        aInserted.Add(newAll);
                    }
                }

                Data.Budgets.InsertAllOnSubmit(aInserted);

                Data.SubmitChanges();
                return "OK";

            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                return ex.Message.ToString();
                //throw ex;

            }
        }

        public string SubmitRelease(decimal glYear, int budgetTypeID, int BranchId, List<BudgetModel> budgets, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }


               
                List<RMS.BL.Budget> aInserted = new List<RMS.BL.Budget>();
                List<RMS.BL.Budget> aUpdated = new List<RMS.BL.Budget>();

                var dbAllQ1 = Data.Budgets.Where(x => x.BudgetTypeID == budgetTypeID && x.GlYear == glYear && x.br_id == BranchId && x.QuarterID == 1).ToList();

                var dbAllQ2 = Data.Budgets.Where(x => x.BudgetTypeID == budgetTypeID && x.GlYear == glYear && x.br_id == BranchId && x.QuarterID == 2).ToList();
                var dbAllQ3 = Data.Budgets.Where(x => x.BudgetTypeID == budgetTypeID && x.GlYear == glYear && x.br_id == BranchId && x.QuarterID == 3).ToList();
                var dbAllQ4 = Data.Budgets.Where(x => x.BudgetTypeID == budgetTypeID && x.GlYear == glYear && x.br_id == BranchId && x.QuarterID == 4).ToList();
                var FirstExces = Data.Budgets.Where(x => x.BudgetTypeID == budgetTypeID && x.GlYear == glYear && x.br_id == BranchId && x.QuarterID == 5).ToList();
                var SecondExces = Data.Budgets.Where(x => x.BudgetTypeID == budgetTypeID && x.GlYear == glYear && x.br_id == BranchId && x.QuarterID == 6).ToList();
                var FirstApp = Data.Budgets.Where(x => x.BudgetTypeID == budgetTypeID && x.GlYear == glYear && x.br_id == BranchId && x.QuarterID == 7).ToList();
                var SecondAPp = Data.Budgets.Where(x => x.BudgetTypeID == budgetTypeID && x.GlYear == glYear && x.br_id == BranchId && x.QuarterID == 8).ToList();
                var ThirdApp = Data.Budgets.Where(x => x.BudgetTypeID == budgetTypeID && x.GlYear == glYear && x.br_id == BranchId && x.QuarterID == 9).ToList();
                var forthApp = Data.Budgets.Where(x => x.BudgetTypeID == budgetTypeID && x.GlYear == glYear && x.br_id == BranchId && x.QuarterID == 10).ToList();
                var FirstSurrender = Data.Budgets.Where(x => x.BudgetTypeID == budgetTypeID && x.GlYear == glYear && x.br_id == BranchId && x.QuarterID == 11).ToList();
                var secondSurrender = Data.Budgets.Where(x => x.BudgetTypeID == budgetTypeID && x.GlYear == glYear && x.br_id == BranchId && x.QuarterID == 12).ToList();

                bool found = false;
                foreach (var a in budgets)
                {
                    foreach (var dba1 in dbAllQ1)
                    {
                        if (a.Account == dba1.Account)
                        {
                            dba1.Grant = a.GrantinQ1;
                            //dba.Aid = a.Aid;
                            dba1.IsActive = true;
                            aUpdated.Add(dba1);
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        var newAll = new RMS.BL.Budget();
                        newAll.BudgetTypeID = budgetTypeID;
                        newAll.GUID = a.GUID;
                        newAll.QuarterID = 1;
                        newAll.GlYear = glYear;
                        newAll.Account = a.Account;
                        newAll.Grant = a.GrantinQ1;
  
                        newAll.br_id = BranchId;

                        newAll.IsActive = true;

                        aInserted.Add(newAll);
                    }

                    foreach (var dba2 in dbAllQ2)
                    {
                        if (a.Account == dba2.Account)
                        {
                            dba2.Grant = a.GrantinQ2;
                            //dba.Aid = a.Aid;
                            dba2.IsActive = true;
                            aUpdated.Add(dba2);
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        var newAll = new RMS.BL.Budget();
                        newAll.BudgetTypeID = budgetTypeID;
                        newAll.GUID = a.GUID;
                        newAll.QuarterID = 2;
                        newAll.GlYear = glYear;
                        newAll.Account = a.Account;
                        newAll.Grant = a.GrantinQ2;

                        newAll.br_id = BranchId;

                        newAll.IsActive = true;

                        aInserted.Add(newAll);
                    }
                    foreach (var dba3 in dbAllQ3)
                    {
                        if (a.Account == dba3.Account)
                        {
                            dba3.Grant = a.GrantinQ3;
                            //dba.Aid = a.Aid;
                            dba3.IsActive = true;
                            aUpdated.Add(dba3);
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        var newAll = new RMS.BL.Budget();
                        newAll.BudgetTypeID = budgetTypeID;
                        newAll.GUID = a.GUID;
                        newAll.QuarterID = 3;
                        newAll.GlYear = glYear;
                        newAll.Account = a.Account;
                        newAll.Grant = a.GrantinQ3;

                        newAll.br_id = BranchId;

                        newAll.IsActive = true;

                        aInserted.Add(newAll);
                    }

                    foreach (var dba in dbAllQ4)
                    {
                        if (a.Account == dba.Account)
                        {
                            dba.Grant = a.GrantinQ4;
                            //dba.Aid = a.Aid;
                            dba.IsActive = true;
                            aUpdated.Add(dba);
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        var newAll = new RMS.BL.Budget();
                        newAll.BudgetTypeID = budgetTypeID;
                        newAll.GUID = a.GUID;
                        newAll.QuarterID = 4;
                        newAll.GlYear = glYear;
                        newAll.Account = a.Account;
                        newAll.Grant = a.GrantinQ4;

                        newAll.br_id = BranchId;

                        newAll.IsActive = true;

                        aInserted.Add(newAll);
                    }
                    foreach (var FirstExx in FirstExces)
                    {
                        if (a.Account == FirstExx.Account)
                        {
                            FirstExx.Grant = a.firstExcess;
                            //dba.Aid = a.Aid;
                            FirstExx.IsActive = true;
                            aUpdated.Add(FirstExx);
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        var newAll = new RMS.BL.Budget();
                        newAll.BudgetTypeID = budgetTypeID;
                        newAll.GUID = a.GUID;
                        newAll.QuarterID = 5;
                        newAll.GlYear = glYear;
                        newAll.Account = a.Account;
                        newAll.Grant = a.firstExcess;

                        newAll.br_id = BranchId;

                        newAll.IsActive = true;

                        aInserted.Add(newAll);
                    }
                    foreach (var Firstsurr in FirstSurrender)
                    {
                        if (a.Account == Firstsurr.Account)
                        {
                            Firstsurr.Grant = a.firstSurrender;
                            //dba.Aid = a.Aid;
                            Firstsurr.IsActive = true;
                            aUpdated.Add(Firstsurr);
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        var newAll = new RMS.BL.Budget();
                        newAll.BudgetTypeID = budgetTypeID;
                        newAll.GUID = a.GUID;
                        newAll.QuarterID = 11;
                        newAll.GlYear = glYear;
                        newAll.Account = a.Account;
                        newAll.Grant = a.firstSurrender;

                        newAll.br_id = BranchId;
                        newAll.IsActive = true;

                        aInserted.Add(newAll);
                    }
                    foreach (var SecondExx in SecondExces)
                    {
                        if (a.Account == SecondExx.Account)
                        {
                            SecondExx.Grant = a.secondExcess;
                            //dba.Aid = a.Aid;
                            SecondExx.IsActive = true;
                            aUpdated.Add(SecondExx);
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        var newAll = new RMS.BL.Budget();
                        newAll.BudgetTypeID = budgetTypeID;
                        newAll.GUID = a.GUID;
                        newAll.QuarterID = 6;
                        newAll.GlYear = glYear;
                        newAll.Account = a.Account;
                        newAll.Grant = a.secondExcess;

                        newAll.br_id = BranchId;

                        newAll.IsActive = true;

                        aInserted.Add(newAll);
                    }

                    foreach (var Secondsurr in secondSurrender)
                    {
                        if (a.Account == Secondsurr.Account)
                        {
                            Secondsurr.Grant = a.secondSurrender;
                            //dba.Aid = a.Aid;
                            Secondsurr.IsActive = true;
                            aUpdated.Add(Secondsurr);
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        var newAll = new RMS.BL.Budget();
                        newAll.BudgetTypeID = budgetTypeID;
                        newAll.GUID = a.GUID;
                        newAll.QuarterID = 12;
                        newAll.GlYear = glYear;
                        newAll.Account = a.Account;
                        newAll.Grant = a.secondSurrender;

                        newAll.br_id = BranchId;

                        newAll.IsActive = true;

                        aInserted.Add(newAll);
                    }

                    foreach (var FirstAppA in FirstApp)
                    {
                        if (a.Account == FirstAppA.Account)
                        {
                            FirstAppA.Grant = a.firstapprop;
                            //dba.Aid = a.Aid;
                            FirstAppA.IsActive = true;
                            aUpdated.Add(FirstAppA);
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        var newAll = new RMS.BL.Budget();
                        newAll.BudgetTypeID = budgetTypeID;
                        newAll.GUID = a.GUID;
                        newAll.QuarterID = 7;
                        newAll.GlYear = glYear;
                        newAll.Account = a.Account;
                        newAll.Grant = a.firstapprop;

                        newAll.br_id = BranchId;

                        newAll.IsActive = true;

                        aInserted.Add(newAll);
                    }
                    foreach (var SecondAppp in SecondAPp)
                    {
                        if (a.Account == SecondAppp.Account)
                        {
                            SecondAppp.Grant = a.secondapprop;
                            //dba.Aid = a.Aid;
                            SecondAppp.IsActive = true;
                            aUpdated.Add(SecondAppp);
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        var newAll = new RMS.BL.Budget();
                        newAll.BudgetTypeID = budgetTypeID;
                        newAll.GUID = a.GUID;
                        newAll.QuarterID = 8;
                        newAll.GlYear = glYear;
                        newAll.Account = a.Account;
                        newAll.Grant = a.secondapprop;

                        newAll.br_id = BranchId;

                        newAll.IsActive = true;

                        aInserted.Add(newAll);
                    }
                    foreach (var ThirdAppp in ThirdApp)
                    {
                        if (a.Account == ThirdAppp.Account)
                        {
                            ThirdAppp.Grant = a.thirdapprop;
                            //dba.Aid = a.Aid;
                            ThirdAppp.IsActive = true;
                            aUpdated.Add(ThirdAppp);
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        var newAll = new RMS.BL.Budget();
                        newAll.BudgetTypeID = budgetTypeID;
                        newAll.GUID = a.GUID;
                        newAll.QuarterID = 9;
                        newAll.GlYear = glYear;
                        newAll.Account = a.Account;
                        newAll.Grant = a.thirdapprop;

                        newAll.br_id = BranchId;

                        newAll.IsActive = true;

                        aInserted.Add(newAll);
                    }
                    foreach (var ForthAppp in forthApp)
                    {
                        if (a.Account == ForthAppp.Account)
                        {
                            ForthAppp.Grant = a.forthapprop;
                            //dba.Aid = a.Aid;
                            ForthAppp.IsActive = true;
                            aUpdated.Add(ForthAppp);
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        var newAll = new RMS.BL.Budget();
                        newAll.BudgetTypeID = budgetTypeID;
                        newAll.GUID = a.GUID;
                        newAll.QuarterID = 10;
                        newAll.GlYear = glYear;
                        newAll.Account = a.Account;
                        newAll.Grant = a.forthapprop;

                        newAll.br_id = BranchId;

                        newAll.IsActive = true;

                        aInserted.Add(newAll);
                    }

                }

                Data.Budgets.InsertAllOnSubmit(aInserted);

                Data.SubmitChanges();
                return "OK";

            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                return ex.Message.ToString();
                //throw ex;

            }
        }



        public IQueryable<SP_BudgetVarianceResult> BgtReport(RMSDataContext Data, decimal year, int brId)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                IQueryable<SP_BudgetVarianceResult> budget =
                    Data.SP_BudgetVariance(year, brId).AsQueryable();

                return budget;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }




        public IQueryable<GrantRelaseReportResult> GrantReport(RMSDataContext Data, decimal year, int brId)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }

                IQueryable<GrantRelaseReportResult> rlease =
                    Data.GrantRelaseReport(year, brId).AsQueryable();

                return rlease;
            }
            catch (Exception ex)
            {
                RMSDB.SetNull();
                throw ex;
            }
        }
    }
}
