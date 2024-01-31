using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.DAL;
using System.Data.OleDb;
using System.Data;

namespace IIITS.DTLMS.BL
{

    public class clsOmSecMast
    {
        CustOledbConnection objCon = new CustOledbConnection(Constants.Password);
        string strQry = string.Empty;
        string strFormCode = "clsOmSecMast";
        public string sSubDivCode { get; set; }


        OleDbCommand oledbcommand;
        public string[] SaveOmSecMastDetails(string strOmCode, string strOmName, string strSubDivCode, string strOmHeadEmp, string strOmMobile, string strUserLogged)
        {
           
            string[] Arrmsg = new string[2];
            try
            {
                oledbcommand = new OleDbCommand();
                oledbcommand.Parameters.AddWithValue("OmName", strOmName.Replace("'", "''"));
                OleDbDataReader dr = objCon.Fetch("select * from TBLOMSECMAST where OM_NAME=:OmName", oledbcommand);
                if (dr.Read())
                {
                    Arrmsg[0] = "OmSec With This Name Already Exists";
                    Arrmsg[1] = "4";
                    
                    return Arrmsg;
                    
                }
                dr.Close();

                oledbcommand = new OleDbCommand();
                oledbcommand.Parameters.AddWithValue("OmCode", strOmCode);
                OleDbDataReader dr1 = objCon.Fetch("select * from TBLOMSECMAST where OM_CODE=:OmCode", oledbcommand);
                if (dr1.Read())
                {
                    Arrmsg[0] = "OmSec With This Code Already Exists";
                    Arrmsg[1] = "4";
                    
                    return Arrmsg;
                   
                }
                dr1.Close();
 

              

                string strInsqry = "insert into TBLOMSECMAST(OM_SLNO,OM_CODE,OM_NAME,OM_SUBDIV_CODE,OM_HEAD_EMP,OM_MOBILE_NO,OM_ENTRY_AUTH)";
                strInsqry += " values(" + objCon.Get_max_no("OM_SLNO", "TBLOMSECMAST") + ",:strOmCode,:strOmName,";
               strInsqry += ":strSubDivCode,:strOmHeadEmp,:strOmMobile,:strUserLogged)";
                     OleDbCommand command = new OleDbCommand();
                    ///command.Parameters.AddWithValue("sMaxid", objDivision.sMaxid);
                    command.Parameters.AddWithValue("strOmCode", strOmCode.ToUpper());
                    command.Parameters.AddWithValue("strOmName", strOmName.ToUpper().Replace("'", "''"));
                    command.Parameters.AddWithValue("strSubDivCode", strSubDivCode.ToUpper());
                    command.Parameters.AddWithValue("strOmHeadEmp", strOmHeadEmp.ToUpper().Replace("'", "''"));

                    command.Parameters.AddWithValue("strOmMobile", strOmMobile.ToUpper());
                    command.Parameters.AddWithValue("strUserLogged", strUserLogged);
                    objCon.Execute(strInsqry, command);

                Arrmsg[0] = "O&M Section Information has been Saved Sucessfully.";
                Arrmsg[1] = "0";
                return Arrmsg;
               
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "SaveOmSecMastDetails");
                return Arrmsg;
            }
        }

        public string[] UpdateOmSecMastDetails(string strOldId, string strOmCode, string strOmName, string strSubDivCode, string strOmHeadEmp, string strOmMobile, string strUserLogged)
        {
            oledbcommand = new OleDbCommand();
            string[] Arrmsg = new string[2];
            try
            {
                oledbcommand = new OleDbCommand();
                oledbcommand.Parameters.AddWithValue("OmName34", strOmName.Replace("'", "''"));
                oledbcommand.Parameters.AddWithValue("OmSlNo23", strOldId);
                OleDbDataReader dr = objCon.Fetch("select * from TBLOMSECMAST where OM_NAME=:OmName34 and OM_SLNO<> :OmSlNo23", oledbcommand);
                if (dr.Read())
                {
                    Arrmsg[0] = "OmSec With This Name Already Exists";
                    Arrmsg[1] = "4";
                    return Arrmsg;
                }
                dr.Close();

                oledbcommand = new OleDbCommand();
                oledbcommand.Parameters.AddWithValue("OmCode15", strOmCode);
                oledbcommand.Parameters.AddWithValue("omslno17", strOldId);
                OleDbDataReader dr1 = objCon.Fetch("select * from TBLOMSECMAST where OM_CODE=:OmCode15 and OM_SLNO<> :omslno17", oledbcommand);
                if (dr1.Read())
                {
                    Arrmsg[0] = "OmSec With This Code Already Exists";
                    Arrmsg[1] = "4";
                    return Arrmsg;
                }
                dr1.Close();
                string strUpdqry = "update TBLOMSECMAST set OM_CODE=:strOmCode,OM_NAME =:strOmName,OM_SUBDIV_CODE=:strSubDivCode,OM_HEAD_EMP=:strOmHeadEmp ,";
                strUpdqry += " OM_MOBILE_NO =:strOmMobile,OM_UPDATED_BY=:strUserLogged,OM_UPDATED_ON=SYSDATE where OM_SLNO =:strOldId";
                OleDbCommand command = new OleDbCommand();
                ///command.Parameters.AddWithValue("sMaxid", objDivision.sMaxid);
                command.Parameters.AddWithValue("strOmCode", strOmCode.ToUpper());
                command.Parameters.AddWithValue("strOmName", strOmName.ToUpper().Replace("'", "''"));
                command.Parameters.AddWithValue("strSubDivCode", strSubDivCode.ToUpper());
                command.Parameters.AddWithValue("strOmHeadEmp", strOmHeadEmp.ToUpper().Replace("'", "''"));

                command.Parameters.AddWithValue("strOmMobile", strOmMobile.ToUpper());
                command.Parameters.AddWithValue("strUserLogged", strUserLogged);
                command.Parameters.AddWithValue("strOldId", strOldId.ToUpper());
                objCon.Execute(strUpdqry, command);

                Arrmsg[0] = "O&M Section Information has been Updated Sucessfully.";
                Arrmsg[1] = "0";
                return Arrmsg;
                            
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "UpdateOmSecMastDetails");
                return Arrmsg;
            }
        }

        public DataTable LoadOmSecvOffDet(string strOmSecID = "")
        {
            oledbcommand = new OleDbCommand();
            DataTable DtDivOffDet = new DataTable();
            try
            {
                strQry = "SELECT to_char(OM_SLNO) OM_SLNO,To_char(OM_CODE)OM_CODE,OM_NAME,OM_SUBDIV_CODE,";
               strQry += " SD_SUBDIV_NAME,OM_HEAD_EMP,OM_MOBILE_NO FROM TBLSUBDIVMAST,TBLOMSECMAST where OM_SUBDIV_CODE=SD_SUBDIV_CODE ";
                if (strOmSecID != "")
                {
                    oledbcommand.Parameters.AddWithValue("OmSlNo", strOmSecID);
                    strQry += " and OM_SLNO=:OmSlNo";
                }
                strQry += "  order by OM_CODE";
                DtDivOffDet = objCon.getDataTable(strQry, oledbcommand);
                

                return DtDivOffDet;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadOmSecvOffDet");
                return DtDivOffDet;
            }
          }

        public string GenerateOmSecCode(clsOmSecMast objOmSec)
        {
            oledbcommand = new OleDbCommand();
            try
            {
                string sMaxNo = string.Empty;
                string sFinancialYear = string.Empty;
                oledbcommand.Parameters.AddWithValue("OmSubDivCode", objOmSec.sSubDivCode);
                string sOmSecCode = objCon.get_value(" SELECT NVL(MAX(OM_CODE),0)+1 FROM TBLOMSECMAST  where OM_SUBDIV_CODE=:OmSubDivCode", oledbcommand);
                if (sOmSecCode.Length > 0)
                {
                    sSubDivCode = sOmSecCode.ToString();
                }
                return sOmSecCode;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GenerateOmSecCode");
                return "";
            }
        }
    }
}




