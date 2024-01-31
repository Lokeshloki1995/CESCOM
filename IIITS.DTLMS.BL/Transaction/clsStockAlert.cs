using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IIITS.DAL;
using System.Data;
using System.Data.OleDb;


namespace IIITS.DTLMS.BL
{
    public class clsStockAlert
    {
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        string strFormCode = "clsStockAlert";
       
        
        public string sFailureId { get; set; }
        public string sTcCapacity { get; set; }
        public string sIndentNo { get; set; }
        public string sCreatedBy { get; set; }
        public string sIndentid { get; set; }
        OleDbCommand oledbCommand;
        public object GetTcDetails(clsStockAlert objStockAlert)
        {
            try
            {
                oledbCommand = new OleDbCommand();
                
                string sQry = string.Empty;
                DataTable dt = new DataTable();
                string[] Arr = new string[2];
                OleDbDataReader dr;
                //to check whether entered indent number exists or not
                oledbCommand.Parameters.AddWithValue("sIndentNo", objStockAlert.sIndentNo);
                dr = ObjCon.Fetch("select  TI_INDENT_NO from TBLINDENT where TI_INDENT_NO=:sIndentNo", oledbCommand);
                if (dr.Read())
                {
                    oledbCommand = new OleDbCommand();
                    sQry = "SELECT TI_ID, DF_ID,TO_CHAR(TC_CAPACITY)TC_CAPACITY FROM TBLTCMASTER ,TBLDTCFAILURE,TBLINDENT,TBLWORKORDER";
                    sQry += " WHERE WO_SLNO=TI_WO_SLNO and WO_DF_ID=DF_ID AND tc_code=df_equipment_id and  TI_INDENT_NO=:sIndentNo1";
                    oledbCommand.Parameters.AddWithValue("sIndentNo1", objStockAlert.sIndentNo);
                    dt = ObjCon.getDataTable(sQry, oledbCommand);
                    if (dt.Rows.Count > 0)
                    {
                        objStockAlert.sFailureId = dt.Rows[0]["DF_ID"].ToString();
                        objStockAlert.sTcCapacity = dt.Rows[0]["TC_CAPACITY"].ToString();
                        objStockAlert.sIndentid = dt.Rows[0]["TI_ID"].ToString();

                    }
                }
                else
                {
                    objStockAlert.sIndentid = "";
                }
                dr.Close();
                return objStockAlert;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetTcDetails");
                return objStockAlert;
            }
            finally
            {
                
            }

        }

        public string[] SaveStockAlert(clsStockAlert objStockAlert)
        {
            string strQry = string.Empty;
            string[] Arr = new string[2];
            OleDbDataReader dr;
            try
            {
                oledbCommand = new OleDbCommand();
                oledbCommand.Parameters.AddWithValue("sCreatedBy", objStockAlert.sCreatedBy);
                oledbCommand.Parameters.AddWithValue("sIndentid", objStockAlert.sIndentid);
                //to check whether indent created user and loggedin user are the same
                dr = ObjCon.Fetch("select  TI_INDENT_NO from TBLINDENT where TI_CRBY=:sCreatedBy AND TI_ID =:sIndentid ", oledbCommand);
                if (!dr.Read())
                {
                    dr.Close();
                    Arr[0] = "Authorised Person only can created the Alert for this Indent";
                    Arr[1] = "2";
                    return Arr;
                }
                dr.Close();
                //to check whether alert has already been generated for entered indent number
                oledbCommand = new OleDbCommand();
                oledbCommand.Parameters.AddWithValue("sIndentid1", objStockAlert.sIndentid);
               // oledbCommand.Parameters.AddWithValue("sIndentid", objStockAlert.sIndentid);
                dr = ObjCon.Fetch("select  TI_INDENT_NO from TBLINDENT where  TI_ID =:sIndentid1 AND TI_ALERT_FLAG=1 ",oledbCommand);
                if (dr.Read())
                {
                    dr.Close();
                    Arr[0] = "Alert has been already created for this Indent ";
                    Arr[1] = "2";
                    return Arr;
                }
                dr.Close();
                //to check whether invoice has been generated for the entered indent number
                oledbCommand = new OleDbCommand();
                oledbCommand.Parameters.AddWithValue("sIndentid2", objStockAlert.sIndentid);
                dr = ObjCon.Fetch("select  * from TBLDTCINVOICE where  IN_TI_NO =:sIndentid2 ",oledbCommand);
                if (dr.Read())
                {
                    dr.Close();
                    Arr[0] = "Invoice Already Issued for this Indent";
                    Arr[1] = "2";
                    return Arr;
                }
                dr.Close();
                //to check whether tc available for the entered indent number
                oledbCommand = new OleDbCommand();
                oledbCommand.Parameters.AddWithValue("sTcCapacity", objStockAlert.sTcCapacity);
                dr = ObjCon.Fetch("select * from TBLTCMASTER where TC_STATUS IN (1,2) and TC_CURRENT_LOCATION=1 and TC_CAPACITY=:sTcCapacity", oledbCommand);
                if (dr.Read())
                {
                    dr.Close();
                    Arr[0] = "Requested Transformer Available in Store, Please collect it";
                    Arr[1] = "2";
                    return Arr;
                }
                dr.Close();
                //updating alert flag to 1 in Indent table
                strQry = "UPDATE TBLINDENT SET TI_ALERT_FLAG=1 WHERE TI_INDENT_NO='" + objStockAlert.sIndentNo + "' ";
                ObjCon.Execute(strQry);
                Arr[0] = "Alert has been Created, Once it is Available you will get the Alert";
                Arr[1] = "0";
                return Arr;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "SaveStockAlert");
                return Arr;
            }
            finally
            {
                
            }

        }
    }
}
