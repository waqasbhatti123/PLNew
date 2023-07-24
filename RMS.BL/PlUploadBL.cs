using System;
using System.Linq;
using System.IO;

namespace RMS.BL
{
    public class PlUploadBL
    {
        public PlUploadBL()
        {

        }

        public string EDAUpload(string userName, byte compId, string fileNme, string TransType, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            StreamReader fileReader = null;
            string line;

            fileReader = new StreamReader(fileNme);
            tblPlSheetData sheetData = null;
            tblPlSheetRef sheetRef = new tblPlSheetRef();
            try
            {
                sheetRef.CompID = compId;
                sheetRef.SheetFileName = fileNme.Substring(fileNme.LastIndexOf("\\") + 1);
                sheetRef.LoadDate = Common.MyDate(Data);
                sheetRef.LoadStatus = true;
                sheetRef.TransType = TransType;
                sheetRef.CreatedBy = userName;
                sheetRef.CreatedOn = Common.MyDate(Data);
                sheetRef.RefPerd = Convert.ToDecimal(sheetRef.SheetFileName.Substring(0, 6));

                if (this.ISAlreadyExist(sheetRef.SheetFileName, Data))
                {
                    return sheetRef.SheetFileName;
                }
                else
                {

                    Data.tblPlSheetRefs.InsertOnSubmit(sheetRef);
                    Data.SubmitChanges();
                }
            }
            catch { return "1"; }
            string[] header = null;
            while ((line = fileReader.ReadLine()) != null)
            {
                if (!line.Trim().Equals(""))
                {
                    header = line.Split('\t');

                    if (header.Length > 2)
                    {
                        try
                        {
                            if (Convert.ToInt32(header[0].Trim()) > 0)
                            {
                                if (Convert.ToInt32(header[2]) != 0)
                                {
                                    sheetData = new tblPlSheetData();
                                    sheetData.EmpID = Convert.ToInt32(header[1]);//this.GetEmpByID(Convert.ToInt32(header[1]), Data);
                                    sheetData.SheetRefID = sheetRef.SheetRefID;
                                    sheetData.TransAmt = Convert.ToInt32(header[2]);
                                    sheetData.BillAmt = Convert.ToInt32(header[3]); 
                                    Data.tblPlSheetDatas.InsertOnSubmit(sheetData);
                                    Data.SubmitChanges();
                                }
                            }
                        }
                        catch { }
                    }
                }
            }
            fileReader.Close();
            return "0";
        }
        
        public string LeaveUpload(string userName, byte compId, string fileNme, string TransType, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            StreamReader fileReader = null;
            string line;

            fileReader = new StreamReader(fileNme);
            tblPlSheetRef sheetRef = new tblPlSheetRef();
            tblPlSheetLeave sheetLeave = null;
            try
            {
                sheetRef.CompID = compId;
                sheetRef.SheetFileName = fileNme.Substring(fileNme.LastIndexOf("\\") + 1);
                sheetRef.LoadDate = Common.MyDate(Data);
                sheetRef.LoadStatus = true;
                sheetRef.TransType = TransType;
                sheetRef.CreatedBy = userName;
                sheetRef.CreatedOn = Common.MyDate(Data);
                sheetRef.RefPerd = Convert.ToDecimal(sheetRef.SheetFileName.Substring(0, 6));

                if (this.ISAlreadyExist(sheetRef.SheetFileName, Data))
                {
                    return sheetRef.SheetFileName;
                }
                else
                {

                    Data.tblPlSheetRefs.InsertOnSubmit(sheetRef);
                    Data.SubmitChanges();
                }
            }
            catch { return "1"; }
            string[] header = null;
            while ((line = fileReader.ReadLine()) != null)
            {
                if (!line.Trim().Equals(""))
                {
                    header = line.Split('\t');

                    if (header.Length > 2)
                    {
                        try
                        {
                            if (Convert.ToInt32(header[0].Trim()) > 0)
                            {
                                if (Convert.ToInt32(header[2]) != 0)
                                {
                                    sheetLeave = new tblPlSheetLeave();
                                    sheetLeave.EmpID = Convert.ToInt32(header[1]);//this.GetEmpByID(Convert.ToInt32(header[1]), Data);
                                    sheetLeave.SheetRefID = sheetRef.SheetRefID;
                                    sheetLeave.LWOP = Convert.ToInt32(header[2]);
                                    try
                                    {
                                        sheetLeave.CDays = Convert.ToInt32(header[3]);
                                        sheetLeave.MDays = Convert.ToInt32(header[4]);
                                        sheetLeave.ADays = Convert.ToInt32(header[5]);
                                    }
                                    catch
                                    {
                                        sheetLeave.CDays = 0;
                                        sheetLeave.MDays = 0;
                                        sheetLeave.ADays = 0;
                                    }
                                    Data.tblPlSheetLeaves.InsertOnSubmit(sheetLeave);
                                    Data.SubmitChanges();
                                }
                            }
                        }
                        catch { }
                    }
                }
            }
            fileReader.Close();
            return "0";
        }

        public string CompanyUpload(string userName, byte compId, string fileNme, string TransType, RMSDataContext Data)
        {
            if (Data == null) { Data = RMSDB.GetOject(); }
            StreamReader fileReader = null;
            string line;

            fileReader = new StreamReader(fileNme);
            tblPlSheetRef sheetRef = new tblPlSheetRef();
            tblPlSheetComp sheetCmp = null;
            try
            {
                sheetRef.CompID = compId;
                sheetRef.SheetFileName = fileNme.Substring(fileNme.LastIndexOf("\\") + 1);
                sheetRef.LoadDate = Common.MyDate(Data);
                sheetRef.LoadStatus = true;
                sheetRef.TransType = TransType;
                sheetRef.CreatedBy = userName;
                sheetRef.CreatedOn = Common.MyDate(Data);
                sheetRef.RefPerd = Convert.ToDecimal(sheetRef.SheetFileName.Substring(0, 6));

                if (this.ISAlreadyExist(sheetRef.SheetFileName, Data))
                {
                    return sheetRef.SheetFileName;
                }
                else
                {

                    Data.tblPlSheetRefs.InsertOnSubmit(sheetRef);
                    Data.SubmitChanges();
                }
            }
            catch { return "1"; }
            string[] header = null;
            while ((line = fileReader.ReadLine()) != null)
            {
                if (!line.Trim().Equals(""))
                {
                    header = line.Split('\t');

                    if (header.Length > 2)
                    {
                        try
                        {
                            if (Convert.ToInt32(header[0].Trim()) > 0)
                            {
                                if (Convert.ToInt32(header[1]) != 0)
                                {
                                    sheetCmp = new tblPlSheetComp();
                                    sheetCmp.CompID = compId;
                                    sheetCmp.SheetRefID = sheetRef.SheetRefID;
                                    sheetCmp.RefPerd = Convert.ToDecimal(sheetRef.SheetFileName.Substring(0, 6));
                                    sheetCmp.GLI = Convert.ToInt32(header[1]);
                                    sheetCmp.HlthIns = Convert.ToInt32(header[2]);
                                    sheetCmp.SrvChg = Convert.ToInt32(header[3]);
                                    try
                                    {
                                        sheetCmp.SrvPc = Convert.ToInt32(header[4]);
                                    }
                                    catch
                                    {
                                        sheetCmp.SrvPc = 0;
                                    }
                                    Data.tblPlSheetComps.InsertOnSubmit(sheetCmp);
                                    Data.SubmitChanges();
                                }
                            }
                        }
                        catch { }
                    }
                }
            }
            fileReader.Close();
            return "0";
        }

        public bool ISAlreadyExist(string name, RMSDataContext Data)
        {

            bool isalready = false;
            if (Data == null) { Data = RMSDB.GetOject(); }
            try
            {
                IQueryable<tblPlSheetRef> sheet = from allow in Data.tblPlSheetRefs
                                                  where allow.SheetFileName == name
                                                  select allow;

                if (sheet != null & sheet.Count<tblPlSheetRef>() > 0)
                    isalready = true;
            }
            catch { }
            
            return isalready;
        }

        public int GetEmpByID(int code, RMSDataContext Data)
        {
            tblPlEmpData emp = Data.tblPlEmpDatas.Single(p => p.EmpCode == code.ToString());
            return emp.EmpID;
        }
        public string GetPerd(int compId, RMSDataContext Data)
        {
            tblCompany c = Data.tblCompanies.Single(p => p.CompID == compId);
            string prd = c.CurPayPeriod.ToString().Substring(4, 2);
            string fullprd = c.CurPayPeriod.ToString()+"01";
            fullprd = prd + "-" + c.CurPayPeriod.ToString().Substring(0, 4);
            //DateTime pd = DateTime.Parse(fullprd);
            
            //if (prd.Equals("01"))
            //{

            //}
            return fullprd;
        }
        public void StartSalCalc(int compId, RMSDataContext Data)
        {
            try
            {
                Data.spSalaryCalc(Convert.ToByte(compId));
            }
            catch { }
        }

        public void StartSalCalcXL(int compId, RMSDataContext Data)
        {
            try
            {
                //Data.spSalaryCalcXL(compId);
            }
            catch { }
        }

        public void MonthEnd(int compId, RMSDataContext Data)
        {
            try
            {
                if (Data == null) { Data = RMSDB.GetOject(); }
                Data.spMonthEnd(Convert.ToByte(compId));
            }
            catch { }
        }
    
    }
}