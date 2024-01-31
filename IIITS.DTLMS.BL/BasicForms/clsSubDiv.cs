using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using IIITS.DAL;
using System.Data;

namespace IIITS.DTLMS.BL
{
   public class clsSubDiv
    {
       string strQry = string.Empty;
       string strFormCode = "clsSubDiv";
       CustOledbConnection objCon = new CustOledbConnection(Constants.Password);
       public string sDivCode { get; set; }

       OleDbCommand oledbcommand;
       public string[] SaveUpdateSubDivisionDetails(string strSubDivID, string strDivCode, string strSubDivCode, string strName, string strHead, string strMobile, string strPhone, string strEmail, bool IsSave, string strUserLogged)
        {
            oledbcommand = new OleDbCommand();
            string[] Arrmsg = new string[2];
            try
            {
               
                if (IsSave)
                {
                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("StrSubDivCode", strSubDivCode);
                    OleDbDataReader dr = objCon.Fetch("select * from TBLSUBDIVMAST where SD_SUBDIV_CODE=:StrSubDivCode", oledbcommand);
                    if (dr.Read())
                    {
                        Arrmsg[0] = "Sub Division Code Already exists";
                        Arrmsg[1] = "4";
                        return Arrmsg;
                    }
                    dr.Close();

                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("SdSubDivCode", strName.ToUpper().Replace("'", "''"));
                    oledbcommand.Parameters.AddWithValue("SdDivCode", strDivCode);
                    dr = objCon.Fetch("select * from TBLSUBDIVMAST where UPPER(SD_SUBDIV_NAME)=:SdSubDivCode and SD_DIV_CODE =:SdDivCode", oledbcommand);
                    if (dr.Read())
                    {
                        Arrmsg[0] = "Sub Division Name Already exists";
                        Arrmsg[1] = "4";
                        return Arrmsg;
                    }
                    dr.Close();
                   
                    long MaxNo = objCon.Get_max_no("SD_ID", "TBLSUBDIVMAST");

                    string strInsQry = string.Empty;
                    strInsQry = "INSERT  into TBLSUBDIVMAST (SD_ID,SD_SUBDIV_CODE,SD_SUBDIV_NAME,SD_DIV_CODE,SD_HEAD_EMP,SD_MOBILE,SD_PHONE,SD_EMAIL,SD_ENTRY_AUTH) VALUES";
                    strInsQry += " (:MaxNo,:strSubDivCode,:strName,:strDivCode,:strHead,:strMobile,";
                    strInsQry += ":strPhone,:strEmail,:strUserLogged)";
                    OleDbCommand command = new OleDbCommand();
                    ///command.Parameters.AddWithValue("sMaxid", objDivision.sMaxid);
                    command.Parameters.AddWithValue("MaxNo", MaxNo);
                    command.Parameters.AddWithValue("strSubDivCode", strSubDivCode);
                    command.Parameters.AddWithValue("strName", strName.Trim().ToUpper().Replace("'", "''"));


                    command.Parameters.AddWithValue("strDivCode", strDivCode);
                    command.Parameters.AddWithValue("strHead", strHead.Trim().ToUpper());
                    command.Parameters.AddWithValue("strMobile", strMobile);

                    command.Parameters.AddWithValue("strPhone", strPhone);
                    command.Parameters.AddWithValue("strEmail", strEmail);
                    command.Parameters.AddWithValue("strUserLogged", strUserLogged);

                    objCon.Execute(strInsQry, command);

                    Arrmsg[0] = "SubDivision Details Saved Sucessfully";
                    Arrmsg[1] = "0";
                    return Arrmsg;
                  
                }
                else
                {

                    oledbcommand = new OleDbCommand();
                    oledbcommand.Parameters.AddWithValue("sdsubdivcode1", strName.ToUpper().Replace("'", "''"));
                    oledbcommand.Parameters.AddWithValue("sddivcode1", strDivCode);
                    oledbcommand.Parameters.AddWithValue("strsubdivcode1", strSubDivCode);
                    OleDbDataReader dr1 = objCon.Fetch("select * from TBLSUBDIVMAST where UPPER(SD_SUBDIV_NAME)=:sdsubdivcode1 and SD_DIV_CODE =:sddivcode1 and SD_SUBDIV_CODE <>:strsubdivcode1", oledbcommand);
                    if (dr1.Read())
                    {
                        Arrmsg[0] = "Sub Division Name Already exists";
                        Arrmsg[1] = "4";
                        return Arrmsg;
                    }
                    dr1.Close();

                    string strUpdQuery = "Update TBLSUBDIVMAST set SD_SUBDIV_NAME=:strName,SD_DIV_CODE=:strDivCode,SD_HEAD_EMP=:strHead,";
                    strUpdQuery += "SD_MOBILE=:strMobile,SD_PHONE=:strPhone,SD_EMAIL=:strEmail,SD_SUBDIV_CODE=:strSubDivCode,SD_UPDATED_BY=:strUserLogged,SD_UPDATED_ON=SYSDATE  where SD_ID=:strSubDivID";
                    OleDbCommand command = new OleDbCommand();
                    ///command.Parameters.AddWithValue("sMaxid", objDivision.sMaxid);
                    command.Parameters.AddWithValue("strName", strName.Trim().ToUpper().Replace("'", "''"));
                    command.Parameters.AddWithValue("strDivCode", strDivCode);
                    command.Parameters.AddWithValue("strHead", strHead.Trim().ToUpper().Replace("'", "''"));


                    command.Parameters.AddWithValue("strMobile", strMobile);
                    command.Parameters.AddWithValue("strPhone", strPhone);
                    command.Parameters.AddWithValue("strEmail", strEmail);

                    command.Parameters.AddWithValue("strSubDivCode", strSubDivCode);
                    command.Parameters.AddWithValue("strUserLogged", strUserLogged);
                    command.Parameters.AddWithValue("strSubDivID", strSubDivID);

                    objCon.Execute(strUpdQuery, command);

                    Arrmsg[0] = "Sub Division Details Update Sucessfully";
                    Arrmsg[1] = "0";
                    return Arrmsg;
                    
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "SaveUpdateSubDivisionDetails");
                return Arrmsg;
            }

        }

        public DataTable LoadSubDivOffDet(string strSubDivID = "")
        {
            oledbcommand = new OleDbCommand();
            DataTable DtDivOffDet = new DataTable();
            try
            {
                strQry = string.Empty;
                strQry = "select SD_ID,To_char(SD_SUBDIV_CODE)SD_SUBDIV_CODE,SD_SUBDIV_NAME,DIV_NAME,SD_HEAD_EMP,SD_DIV_CODE,SD_TQ_ID,";
                strQry += "  SD_PHONE,SD_MOBILE,SD_EMAIL,CM_CIRCLE_NAME ";
                strQry += " from TBLDIVISION,TBLSUBDIVMAST,TBLCIRCLE where SD_DIV_CODE=DIV_CODE  AND CM_CIRCLE_CODE= DIV_CICLE_CODE   ";
                if (strSubDivID != "")
                {
                    oledbcommand.Parameters.AddWithValue("SubDivCode", strSubDivID);
                    strQry += " and SD_ID=:SubDivCode";
                }
                strQry += " order by SD_SUBDIV_CODE";
                DtDivOffDet = objCon.getDataTable(strQry, oledbcommand);
                

                return DtDivOffDet;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadSubDivOffDet");
                return DtDivOffDet;
            }
        }

        public string GenerateSubDivCode(clsSubDiv objSubDivision)
        {
            
            try
            {
                oledbcommand = new OleDbCommand();
                string sMaxNo = string.Empty;
                string sFinancialYear = string.Empty;
                string strQry = string.Empty;
                strQry = " SELECT NVL(MAX(SD_SUBDIV_CODE),0)+1 FROM TBLSUBDIVMAST  where SD_DIV_CODE=:SdDivCode ";
                oledbcommand.Parameters.AddWithValue("SdDivCode", objSubDivision.sDivCode);
                string sSubDivCode = objCon.get_value(strQry, oledbcommand);
                if (sSubDivCode.Length > 0)
                {
                    sSubDivCode = sSubDivCode.ToString();
                }
                return sSubDivCode;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GenerateDivCode");
                return "";
            }
        }


    }
}
