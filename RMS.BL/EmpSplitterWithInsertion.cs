using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RMS.BL
{
    public class EmpSplitterWithInsertion
    {
        public void FileSplitter(string compId, string fileName, RMSDataContext Data)
        {
            string[] header = null;
            try
            {
                StreamReader fileReader = null;
                string line;
                DateTime efDate = Common.MyDate(Data);
                fileReader = new StreamReader(fileName);
                string fname = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                string efdtestr = fname.Substring(4,2);
                efdtestr += "/";
                efdtestr += fname.Substring(6, 2);
                efdtestr += "/";
                efdtestr += fname.Substring(0, 4);

                efDate = DateTime.Parse(efdtestr);
                tblPlEmpData emp = null;
                tblPlAlow empSalAllow = null;

                tblPlCode desig = null;
                tblPlCode dept = null;
                tblPlCode sect = null;
                tblPlCode div = null;
                tblPlCode reg = null;
                tblCity city = null;
                if (Data == null) { Data = RMSDB.GetOject(); }
                while ((line = fileReader.ReadLine()) != null)
                {
                    if (!line.Trim().Equals(""))
                    {
                        header = line.Split('\t');
                        
                        if (header.Length > 14)
                        {
                            try
                            {
                                if (Convert.ToInt32(header[0].Trim()) > 0)
                                {
                                    emp = null;

                                    ReplaceDoubleQoutes(header);

                                    try
                                    {
                                        emp = this.GetEmpByID(header[1].Trim(),Data);
                                    }
                                    catch 
                                    { 
                                        emp = null; 
                                    }

                                    if (emp == null)
                                    {
                                        emp = new tblPlEmpData();

                                        emp.CompID = Convert.ToByte(compId);
                                        emp.EmpStatus = 1;
                                        emp.EmpCode = header[1].Trim();
                                        emp.FullName = header[2].Trim();


                                        emp.Sex = 'M';
                                        emp.MarStatus = 'S';

                                        try
                                        {
                                            desig = Data.tblPlCodes.Single(p => p.CodeDesc.Equals(header[3].Trim()) && p.CodeTypeID == 4);
                                        }
                                        catch
                                        {
                                            desig = null;
                                        }
                                        emp.tblPlCode1 = desig;

                                        try
                                        {
                                            div = Data.tblPlCodes.Single(p => p.CodeDesc.Equals(header[4].Trim()) && p.CodeTypeID == 2);
                                        }
                                        catch
                                        {
                                            div = null;
                                        }
                                        emp.tblPlCode2 = div;

                                        try
                                        {
                                            dept = Data.tblPlCodes.Single(p => p.CodeDesc.Equals(header[5].Trim()) && p.CodeTypeID == 3);
                                        }
                                        catch
                                        {
                                            dept = null;
                                        }
                                        emp.tblPlCode = dept;

                                        try
                                        {
                                            sect = Data.tblPlCodes.Single(p => p.CodeDesc.Equals(header[6].Trim()) && p.CodeTypeID == 5);
                                        }
                                        catch
                                        {
                                            sect = null;
                                        }
                                        emp.tblPlCode4 = sect;

                                        try
                                        {
                                            city = Data.tblCities.Single(p => p.CityName.Equals(header[7].Trim()));
                                        }
                                        catch
                                        {
                                            city = null;
                                        }
                                        emp.tblCity = city;

                                        try
                                        {
                                            reg = Data.tblPlCodes.Single(p => p.CodeDesc.Equals(header[8].Trim()) && p.CodeTypeID == 1);
                                        }
                                        catch
                                        {
                                            reg = null;
                                        }
                                        emp.tblPlCode3 = reg;


                                        emp.AccountNo = header[10].Trim();
                                        emp.Branch = header[11].Trim();

                                        emp.NIC = header[12].Trim();

                                        emp.MobNo = header[13].Trim();


                                        try
                                        {
                                            emp.DOJ = Convert.ToDateTime(header[14].Trim());
                                        }
                                        catch
                                        {
                                            emp.DOJ = null;
                                        }

                                        Data.tblPlEmpDatas.InsertOnSubmit(emp);
                                        Data.SubmitChanges();
                                    }
                                    if (header.Length > 30)
                                    {
                                        if (emp.EmpStatus == 1)
                                        {
                                            empSalAllow = new tblPlAlow();

                                            empSalAllow.tblPlEmpData = emp;
                                            empSalAllow.tblCompany = emp.tblCompany;
                                            empSalAllow.EffDate = efDate;

                                            empSalAllow.Basic = Convert.ToDecimal(header[21]);
                                            empSalAllow.HR = Convert.ToDecimal(header[22]);
                                            empSalAllow.Utilities = Convert.ToDecimal(header[23]);
                                            if (header[26].Trim().Equals(""))
                                            {
                                                empSalAllow.CA = 0;
                                            }
                                            else
                                            {
                                                empSalAllow.CA = Convert.ToDecimal(header[26]);
                                            }
                                            if (header[28].Trim().Equals(""))
                                            {
                                                empSalAllow.SplAlow = 0;
                                            }
                                            else
                                            {
                                                empSalAllow.SplAlow = Convert.ToDecimal(header[28]);
                                            }
                                            if (header[30].Trim().Equals(""))
                                            {
                                                empSalAllow.NSHA = 0;
                                            }
                                            else
                                            {
                                                empSalAllow.NSHA = Convert.ToDecimal(header[30]);
                                            }

                                            Data.tblPlAlows.InsertOnSubmit(empSalAllow);
                                            Data.SubmitChanges();
                                        }
                                    }
                                }
                            }
                            catch
                            {

                            }

                        }
                    }
                }
                fileReader.Close();
            }
            catch { }
        }
        private static void ReplaceDoubleQoutes(string[] hdr)
        {
            try
            {
                string st = null;
                if (hdr != null && hdr.Length > 14)
                {
                    //for (int i = 0; i < hdr.Length; i++ )
                    for (int i = 0; i < 15; i++)
                    {
                        st = hdr[i].Trim();
                        if (st.Substring(0, 1).Equals("\"") && st.Substring(st.Length-1, 1).Equals("\""))
                        {
                            hdr[i] = st.Substring(1, st.Length - 2).Trim();
                        }
                    }
                }
            }
            catch { }
        }
        public tblPlEmpData GetEmpByID(string code, RMSDataContext Data)
        {
            tblPlEmpData emp = Data.tblPlEmpDatas.Single(p => p.EmpCode.Equals(code));
            return emp;
        }
    }
}
