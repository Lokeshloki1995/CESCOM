using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using IIITS.DAL;
using System.Data;


namespace IIITS.DTLMS.BL
{
   public  class clsDivision
    {
        CustOledbConnection objcon = new CustOledbConnection(Constants.Password);
        string strFormCode = "clsDivision";
        public string sCircleCode { get; set; }
        public string sDivisionCode { get; set; }
        public string sDivisionName { get; set; }
        public string sName { get; set; }
        public string sPhone { get; set; }
        public string sMobileNo { get; set; }
        public string sEmail { get; set; }
        public string sMaxid { get; set; }

        OleDbCommand oledbcommand;
        public string sUserId { get; set; }

        public string[] SaveDivision(clsDivision objDivision)
        {
            oledbcommand = new OleDbCommand();
            string[] Arr = new string[2];
            try
            {

                string strQry = string.Empty;
                if (objDivision.sMaxid == "")
                {
                    oledbcommand.Parameters.AddWithValue("DivisionCode", objDivision.sDivisionCode);
                    OleDbDataReader dr = objcon.Fetch("Select * from TBLDIVISION where DIV_CODE=:DivisionCode", oledbcommand);
                    if (dr.Read())
                    {
                        
                        Arr[0] = "division code already exists";
                        Arr[1] = "3";
                        return Arr;
                    }
                    dr.Close();

                    objDivision.sMaxid = objcon.Get_max_no("DIV_ID", "TBLDIVISION").ToString();

                    strQry = "INSERT INTO TBLDIVISION(DIV_ID, DIV_CODE ,DIV_NAME,DIV_CICLE_CODE,DIV_HEAD_EMP,DIV_MOBILE_NO,DIV_PHONE,DIV_EMAIL)";
                    strQry += "VALUES(:sMaxid,:sDivisionCode,:sDivisionName,:sCircleCode,";
                    strQry += ":sName,'"+ objDivision.sMobileNo + "',:sPhone,:sEmail)";
                    OleDbCommand command = new OleDbCommand();
                    command.Parameters.AddWithValue("sMaxid", objDivision.sMaxid);
                    command.Parameters.AddWithValue("sDivisionName", objDivision.sDivisionName);
                    command.Parameters.AddWithValue("sName", objDivision.sName);
                    command.Parameters.AddWithValue("sDivisionCode", objDivision.sDivisionCode);
                  //  command.Parameters.AddWithValue("sMobileNo", objDivision.sMobileNo);
                    command.Parameters.AddWithValue("sCircleCode", objDivision.sCircleCode);
                    command.Parameters.AddWithValue("sPhone", objDivision.sPhone);
                    command.Parameters.AddWithValue("sEmail", objDivision.sEmail);
                    objcon.Execute(strQry, command);
                    Arr[0] = "Saved Successfully ";
                    Arr[1] = "0";
                    return Arr;
                }
                else
                {
                    strQry = " UPDATE TBLDIVISION SET DIV_NAME=:sDivisionName , DIV_HEAD_EMP=:sName , DIV_CODE=:sDivisionCode";
                    strQry += " ,DIV_MOBILE_NO=:sMobileNo, DIV_PHONE=:sPhone,DIV_EMAIL=:sEmail ,  DIV_UPDATED_ON  =SYSDATE , DIV_UPDATED_BY=:sUserId where DIV_ID = '" + objDivision.sMaxid + "'";



                    OleDbCommand command = new OleDbCommand();
                    ///command.Parameters.AddWithValue("sMaxid", objDivision.sMaxid);
                    command.Parameters.AddWithValue("sDivisionName", objDivision.sDivisionName);
                    command.Parameters.AddWithValue("sName", objDivision.sName);
                    command.Parameters.AddWithValue("sDivisionCode", objDivision.sDivisionCode);
                    command.Parameters.AddWithValue("sMobileNo", objDivision.sMobileNo);

                    command.Parameters.AddWithValue("sPhone", objDivision.sPhone);
                    command.Parameters.AddWithValue("sEmail", objDivision.sEmail);
                    command.Parameters.AddWithValue("sUserId", objDivision.sUserId);

                    objcon.Execute(strQry, command);






                    Arr[0] = "Updated Successfully ";
                    Arr[1] = "1";
                    return Arr;
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveDivision");
                return Arr;
            }
        }

        public DataTable LoadAllDivisionDetails()
        {
            DataTable dt = new DataTable();
            try
            {

                string strQry = string.Empty;
                strQry = "SELECT DIV_ID,  To_char(DIV_CODE)DIV_CODE ,DIV_NAME FROM TBLDIVISION  ORDER BY DIV_CODE ";
                dt = objcon.getDataTable(strQry);
                return dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadAllDivisionDetails");
                return dt;
            }

        }

        public object getDivisionDetails(clsDivision objDivision)
        {
            oledbcommand = new OleDbCommand();
            DataTable dtDetails = new DataTable();
            try
            {
                oledbcommand.Parameters.AddWithValue("DivisionId", objDivision.sMaxid);
                String strQry = "SELECT  DIV_CODE ,DIV_NAME,DIV_CICLE_CODE,DIV_HEAD_EMP,DIV_MOBILE_NO,DIV_PHONE,DIV_EMAIL FROM TBLDIVISION ";
                strQry += " WHERE DIV_ID =:DivisionId";
                dtDetails = objcon.getDataTable(strQry, oledbcommand);

                if (dtDetails.Rows.Count > 0)
                {
                    objDivision.sDivisionName = Convert.ToString(dtDetails.Rows[0]["DIV_NAME"].ToString());
                    objDivision.sDivisionCode = Convert.ToString(dtDetails.Rows[0]["DIV_CODE"].ToString());
                    objDivision.sCircleCode = Convert.ToString(dtDetails.Rows[0]["DIV_CICLE_CODE"].ToString());
                    objDivision.sName = Convert.ToString(dtDetails.Rows[0]["DIV_HEAD_EMP"].ToString());
                    objDivision.sMobileNo = Convert.ToString(dtDetails.Rows[0]["DIV_MOBILE_NO"].ToString());
                    objDivision.sPhone = Convert.ToString(dtDetails.Rows[0]["DIV_PHONE"].ToString());
                    objDivision.sEmail = Convert.ToString(dtDetails.Rows[0]["DIV_EMAIL"].ToString());
                }
                return objDivision;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "getCircleDetails");
                return objDivision;
            }

        }

        public string GenerateDivCode(clsDivision objDivision)
        {
            oledbcommand = new OleDbCommand();
            try
            {
                string sMaxNo = string.Empty;
                string sFinancialYear = string.Empty;
                oledbcommand.Parameters.AddWithValue("DivisionCircleCode", objDivision.sCircleCode);
                string sCircleCodeNo = objcon.get_value(" SELECT NVL(MAX(DIV_CODE),0)+1 FROM TBLDIVISION  where DIV_CICLE_CODE=:DivisionCircleCode", oledbcommand);
                if (sCircleCodeNo.Length > 0)
                {
                    sCircleCode = sCircleCodeNo.ToString();
                }
                return sCircleCodeNo;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GenerateDivCode");
                return "";
            }
        }



    }
}
