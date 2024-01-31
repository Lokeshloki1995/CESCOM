using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using IIITS.DAL;

namespace IIITS.DTLMS.BL
{
    public class clsTaluk
    {
        string sformcode = "ClsTaluk";
        CustOledbConnection objcon = new CustOledbConnection(Constants.Password);
        public string sTalukId { get; set; }
        public string sDistrictName { get; set; }
        public string sTalukCode { get; set; }
        public string sTalukName { get; set; }
        public string sButtonName { get; set; }
        public string sUserID { get; set; }

        OleDbCommand oledbcommand;
        public string[] SaveDetails(clsTaluk objTlk)
        {

            string[] Arr = new string[3];
            string strQry = string.Empty;
            OleDbDataReader dr;
            DataTable dt = new DataTable();
            try
            {
                oledbcommand = new OleDbCommand();
                if (objTlk.sButtonName == "Save")
                {
                    if (objTlk.sTalukId == "")
                    {
                        oledbcommand.Parameters.AddWithValue("TalukCode", objTlk.sTalukCode.ToUpper());
                        strQry = "select * from TBLTALQ where UPPER(TQ_CODE)=:TalukCode";
                        dr = objcon.Fetch(strQry, oledbcommand);
                        if (dr.Read())
                        {
                            Arr[0] = "Taluk Code Exist ";
                            Arr[1] = "1";
                            return Arr;
                        }
                        dr.Close();

                        oledbcommand = new OleDbCommand();
                        oledbcommand.Parameters.AddWithValue("TalukName", objTlk.sTalukName.ToUpper());
                        strQry = "select * from TBLTALQ where UPPER(TQ_NAME)=:TalukName";
                        dr = objcon.Fetch(strQry, oledbcommand);
                        if (dr.Read())
                        {
                            Arr[0] = "Taluk Name Exist ";
                            Arr[1] = "1";
                            return Arr;
                        }
                        dr.Close();

                        string SMaxid = objcon.Get_max_no("TQ_SLNO", "TBLTALQ").ToString();

                        oledbcommand = new OleDbCommand();
                        oledbcommand.Parameters.AddWithValue("DtCode", objTlk.sDistrictName);
                        strQry = "select DT_CODE from TBLDIST where DT_CODE=:DtCode";
                        string sDistcode = objcon.get_value(strQry, oledbcommand);

                        strQry = "insert into TBLTALQ (TQ_CODE,TQ_NAME,TQ_SLNO,TQ_DT_ID) ";
                        strQry += " values('" + objTlk.sTalukCode + "','" + objTlk.sTalukName.Trim().Replace(" ", "") + "',";
                        strQry += " '" + SMaxid + "','" + sDistcode + "')";
                        objcon.Execute(strQry);
                        Arr[0] = SMaxid.ToString();
                        Arr[1] = "2";
                        Arr[2] = "Saved Succesfully";
                    }

                    else
                    {
                        oledbcommand = new OleDbCommand();
                        oledbcommand.Parameters.AddWithValue("dtcode1", objTlk.sDistrictName);
                        strQry = "select * from TBLDIST where DT_CODE=:dtcode1";
                        dt = objcon.getDataTable(strQry, oledbcommand);
                        string sDistCode = Convert.ToString(dt.Rows[0]["DT_CODE"].ToString());


                        oledbcommand = new OleDbCommand();
                        oledbcommand.Parameters.AddWithValue("talukcd1", objTlk.sTalukCode.ToUpper());
                        oledbcommand.Parameters.AddWithValue("discd1", sDistCode);
                        strQry = "select * from TBLTALQ where UPPER(TQ_CODE)=:talukcd1 and TQ_DT_ID=:discd1";
                        dt = objcon.getDataTable(strQry, oledbcommand);

                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["TQ_NAME"].ToString() == objTlk.sTalukName.Trim().Replace("", ""))
                            {
                                Arr[0] = "Taluk Name Exist ";
                                Arr[1] = "1";
                                return Arr;
                            }
                            else
                            {

                                oledbcommand = new OleDbCommand();
                                oledbcommand.Parameters.AddWithValue("dtcd2", objTlk.sDistrictName);
                                strQry = "select DT_CODE from TBLDIST where DT_CODE=:dtcd2";
                                string sDistcode = objcon.get_value(strQry, oledbcommand);

                                string updateQry = "update TBLTALQ set TQ_CODE='" + objTlk.sTalukCode + "',TQ_NAME='" + objTlk.sTalukName.Trim().Replace(" ", "") + "',";
                                updateQry += " TQ_DT_ID='" + sDistcode + "' , TQ_UPDATED_ON  = SYSDATE ,  TQ_UPDATED_BY = '" + objTlk.sUserID + "' where UPPER(TQ_SLNO)='" + objTlk.sTalukId + "'";
                                objcon.Execute(updateQry);
                                Arr[0] = "Updated Successfully ";
                                Arr[1] = "3";
                                return Arr;
                            }
                        }

                        else
                        {
                            oledbcommand = new OleDbCommand();
                            string SMaxid = objcon.Get_max_no("TQ_SLNO", "TBLTALQ").ToString();
                            oledbcommand.Parameters.AddWithValue("dtnm3", objTlk.sDistrictName.ToUpper());
                            strQry = "select DT_CODE from TBLDIST where UPPER(DT_NAME)=:dtnm3";
                            string sDistcode = objcon.get_value(strQry, oledbcommand);

                            strQry = "insert into TBLTALQ (TQ_CODE,TQ_NAME,TQ_SLNO,TQ_DT_ID) values('" + objTlk.sTalukCode + "',";
                            strQry += " '" + objTlk.sTalukName.Trim().Replace(" ", "") + "','" + SMaxid + "','" + sDistcode + "')";
                            objcon.Execute(strQry);
                            Arr[0] = SMaxid.ToString();
                            Arr[1] = "2";
                            Arr[2] = "Created new taluk code and name";
                        }
                    }
                }


                else
                {
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("dtcd1", objTlk.sDistrictName);
                    strQry = "select DT_CODE from TBLDIST where DT_CODE=:dtcd1";
                    string sDistcode = objcon.get_value(strQry, oledbcommand);

                    string updateQry = "update TBLTALQ set TQ_CODE='" + objTlk.sTalukCode + "',TQ_NAME='" + objTlk.sTalukName.Trim().Replace(" ", "") + "',";
                    updateQry += " TQ_DT_ID='" + sDistcode + "' where UPPER(TQ_SLNO)='" + objTlk.sTalukId + "'";
                    objcon.Execute(updateQry);
                    Arr[0] = "Updated Successfully ";
                    Arr[1] = "3";
                    return Arr;
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, sformcode, "SaveDetails");
            }
            return Arr;
        }

        public DataTable Fetchdata()
        {
            //oledbcommand = new OleDbCommand();
            DataTable dt = new DataTable();
            try
            {
                string selQry = "select DT_NAME from TBLDIST";
                dt = objcon.getDataTable(selQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, sformcode, "Fetchdata");
            }
            return dt;
        }

        public object GetTlkDetails(clsTaluk objTaluk)
        {
            oledbcommand = new OleDbCommand();
            string sDistId = string.Empty;
            DataTable dtDetails = new DataTable();
            try
            {
                oledbcommand.Parameters.AddWithValue("TqSlNo", objTaluk.sTalukId);
                String strQry = "SELECT * FROM TBLTALQ WHERE TQ_SLNO=:TqSlNo";
                dtDetails = objcon.getDataTable(strQry, oledbcommand);

                if (dtDetails.Rows.Count > 0)
                {
                    sDistId = Convert.ToString(dtDetails.Rows[0]["TQ_DT_ID"].ToString());
                    objTaluk.sTalukCode = Convert.ToString(dtDetails.Rows[0]["TQ_CODE"].ToString());
                    objTaluk.sTalukName = Convert.ToString(dtDetails.Rows[0]["TQ_NAME"].ToString());
                    objTaluk.sDistrictName = Convert.ToString(dtDetails.Rows[0]["TQ_DT_ID"].ToString());
                    //strQry = "SELECT * FROM TBLDIST WHERE DT_CODE='" + sDistId + "'";
                    //dtDetails = objcon.getDataTable(strQry);

                    //objTaluk.sDistrictName = dtDetails.Rows[0]["DT_NAME"].ToString();
                }
                return objTaluk;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, sformcode, "GetTalkDetails");
                return objTaluk;
            }
        }

        public DataTable LoadAllTalkDetails()
        {
            //oledbcommand = new OleDbCommand();
            DataTable dt = new DataTable();
            try
            {

                string strQry = string.Empty;
                strQry = "SELECT TQ_SLNO,TQ_CODE,TQ_NAME,DT_NAME FROM TBLTALQ,TBLDIST WHERE DT_CODE=TQ_DT_ID ORDER BY TQ_CODE";
                dt = objcon.getDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, sformcode, "LoadAllTalkDetails");
                return dt;
            }

        }


        public string GenerateTalukCode(clsTaluk objtaluk)
        {
            oledbcommand = new OleDbCommand();
            try
            {
                string sMaxNo = string.Empty;
                string sFinancialYear = string.Empty;
                oledbcommand.Parameters.AddWithValue("TqDtNo", objtaluk.sDistrictName);
                string sCircleCodeNo = objcon.get_value(" SELECT NVL(MAX(TQ_CODE),0)+1 FROM TBLTALQ  where TQ_DT_ID=:TqDtNo", oledbcommand);
                if (sCircleCodeNo.Length > 0)
                {
                    sTalukCode = sCircleCodeNo.ToString();
                }
                return sCircleCodeNo;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, sformcode, "GenerateTalukCode");
                return "";
            }
        }
    }
}
