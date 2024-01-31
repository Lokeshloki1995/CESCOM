using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.DAL;
using System.Data.OleDb;
using System.Data;
namespace IIITS.DTLMS.BL
{
    public class clsStoreIndent
    {
        CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        string strFormCode = "clsTcTransfer";
        public string sQuantity { get; set; }
        public string sTcCapacity { get; set; }
        public string sIndentNo { get; set; }
        public string sIndentDate { get; set; }
        public string sDescription { get; set; }
        public string sFromStoreId { get; set; }
        public string sToStoreId { get; set; }
        public string sSoId { get; set; }
        public string sCrBy { get; set; }
        public string sSiId { get; set; }
        public string sOfficeCode { get; set; }
        public DataTable ddtCapacityGrid { get; set; }
        public string sIndentId { get; set; }
        public string sToStoreName { get; set; }

        public string sIndentObjectid { get; set; }

        // Workflow
        public string sFormName { get; set; }
        public string sClientIP { get; set; }
        public string sWFDataId { get; set; }
        public string sActionType { get; set; }

        public string[] SaveStoreTransfer(clsStoreIndent objTcTransfer)
        {
            string strQry = string.Empty;
            string[] Arr = new string[2];
            //OleDbDataReader dr;
            DataTable dt = new DataTable();
            OleDbCommand oleDbCommand = new OleDbCommand();
            try
            {
                
                //To check whether Entered Indent No Already Exists or Not

                if (objTcTransfer.sSiId == null)
                {
                    //To get the Store id of logged in user
                    oleDbCommand.Parameters.AddWithValue("SINO", objTcTransfer.sIndentNo);
                    dt = ObjCon.getDataTable("select SI_NO from TBLSTOREINDENT where SI_NO=:SINO", oleDbCommand);
                    if (dt.Rows.Count > 0)
                    {                        
                        Arr[0] = "Entered Indent Number Already Exists";
                        Arr[1] = "2";
                        return Arr;
                    }

                    OleDbCommand oleDbCommand1 = new OleDbCommand();
                    oleDbCommand1.Parameters.AddWithValue("OfficeCode", objTcTransfer.sOfficeCode.Substring(0, 2));
                    string strFromStoreId = ObjCon.get_value("SELECT SM_ID FROM TBLSTOREMAST WHERE SM_OFF_CODE=:OfficeCode", oleDbCommand1);

                    objTcTransfer.sSiId = Convert.ToString(ObjCon.Get_max_no("SI_ID", "TBLSTOREINDENT"));

                    strQry = "INSERT INTO TBLSTOREINDENT(SI_ID,SI_NO,SI_DATE,SI_DESC,SI_FROM_STORE,SI_TO_STORE,SI_CRBY,SI_CRON,SI_TRANSFER_FLAG) ";
                    strQry += " VALUES('" + objTcTransfer.sSiId + "','" + objTcTransfer.sIndentNo + "',";
                    strQry += " TO_DATE('" + objTcTransfer.sIndentDate + "','dd/MM/yyyy'),'" + objTcTransfer.sDescription + "',";
                    strQry += " '" + strFromStoreId + "','" + objTcTransfer.sToStoreId + "','" + objTcTransfer.sCrBy + "',SYSDATE,0)";
                   // ObjCon.Execute(strQry);


                    //for (int i = 0; i < objTcTransfer.ddtCapacityGrid.Rows.Count; i++)
                    //{
                    //    objTcTransfer.sSoId = Convert.ToString(ObjCon.Get_max_no("SO_ID", "TBLSINDENTOBJECTS"));
                    //    strQry = "INSERT INTO TBLSINDENTOBJECTS(SO_ID,SO_SI_ID,SO_CAPACITY,SO_QNTY,SO_CRBY,SO_CRON) ";
                    //    strQry += " VALUES('" + objTcTransfer.sSoId + "','" + objTcTransfer.sSiId + "','" + objTcTransfer.ddtCapacityGrid.Rows[i]["SO_CAPACITY"] + "',";
                    //    strQry += " '" + objTcTransfer.ddtCapacityGrid.Rows[i]["SO_QNTY"] + "','" + objTcTransfer.sCrBy + "',SYSDATE)";
                    //    //ObjCon.Execute(strQry);
                     
                    //}
                   
                    #region Workflow

                    strQry = "INSERT INTO TBLSTOREINDENT(SI_ID,SI_NO,SI_DATE,SI_DESC,SI_FROM_STORE,SI_TO_STORE,SI_CRBY,SI_CRON,SI_TRANSFER_FLAG) ";
                    strQry += " VALUES('{0}',(SELECT INVOICENUMBER('" + objTcTransfer.sOfficeCode + "') FROM DUAL),";
                    strQry += " TO_DATE('" + objTcTransfer.sIndentDate + "','dd/MM/yyyy'),'" + objTcTransfer.sDescription + "',";
                    strQry += " '" + strFromStoreId + "','" + objTcTransfer.sToStoreId + "','" + objTcTransfer.sCrBy + "',SYSDATE,0)";

                    strQry = strQry.Replace("'", "''");


                    StringBuilder sbQuery = new StringBuilder();
                   // StringBuilder sbQueryParameter = new StringBuilder();
                    string sCapacityValues = string.Empty;
                    string sQuantityValues = string.Empty;


                    int iObject = 1;
                    string strQry1 = string.Empty;
                    for (int i = 0; i < objTcTransfer.ddtCapacityGrid.Rows.Count; i++)
                    {
                        //objTcTransfer.sSoId = Convert.ToString(ObjCon.Get_max_no("SO_ID", "TBLSINDENTOBJECTS"));

                        strQry1 = "INSERT INTO TBLSINDENTOBJECTS(SO_ID,SO_SI_ID,SO_CAPACITY,SO_QNTY,SO_CRBY,SO_CRON) ";
                        strQry1 += " VALUES((SELECT NVL(MAX(SO_ID),0)+1 FROM TBLSINDENTOBJECTS),'{0}','" + objTcTransfer.ddtCapacityGrid.Rows[i]["SO_CAPACITY"] + "',";
                        strQry1 += " '" + objTcTransfer.ddtCapacityGrid.Rows[i]["SO_QNTY"] + "','" + objTcTransfer.sCrBy + "',SYSDATE)";
                        //ObjCon.Execute(strQry);
                        sbQuery.Append(strQry1);
                        sbQuery.Append(";");

                        //sbQueryParameter.Append("SELECT NVL(MAX(SO_ID),0)+1 FROM TBLSINDENTOBJECTS");
                        //sbQueryParameter.Append(";");

                        sCapacityValues += objTcTransfer.ddtCapacityGrid.Rows[i]["SO_CAPACITY"]+"`";
                        sQuantityValues += objTcTransfer.ddtCapacityGrid.Rows[i]["SO_QNTY"] + "`";

                        iObject++;
                    }
                    sbQuery = sbQuery.Replace("'", "''");

                    string sParam = "SELECT NVL(MAX(SI_ID),0)+1 FROM TBLSTOREINDENT";

                    clsApproval objApproval = new clsApproval();

                    objApproval.sFormName = objTcTransfer.sFormName;
                    // objApproval.sRecordId = objFailureDetails.sFailureId;
                    objApproval.sOfficeCode = objTcTransfer.sOfficeCode;
                    objApproval.sClientIp = objTcTransfer.sClientIP;
                    objApproval.sCrby = objTcTransfer.sCrBy;
                    objApproval.sQryValues = strQry + ";" + sbQuery;
                    objApproval.sParameterValues = sParam ;
                    objApproval.sMainTable = "TBLSTOREINDENT";
                    objApproval.sDataReferenceId = strFromStoreId;
                    objApproval.sDescription = "Inter Store Indent Request for Specified Capacity Transformer To Store Name "+ objTcTransfer.sToStoreName;
                    objApproval.sRefOfficeCode = objTcTransfer.sOfficeCode;

                    string sPrimaryKey = "{0}";
                    string sSecPrimaryKey = "{1}";

                    objApproval.sColumnNames = "SI_ID,SI_NO,SI_DATE,SI_DESC,SI_FROM_STORE,SI_TO_STORE,SI_CRBY";
                    objApproval.sColumnNames += ";SO_ID,SO_CAPACITY,SO_QNTY";
                    objApproval.sColumnValues = "" + sPrimaryKey + "," + objTcTransfer.sIndentNo + "," + objTcTransfer.sIndentDate + "," + objTcTransfer.sDescription + ",";
                    objApproval.sColumnValues += "" + strFromStoreId + "," + objTcTransfer.sToStoreId + "," + objTcTransfer.sCrBy + "";
                    objApproval.sColumnValues += ";" + sSecPrimaryKey + "," + sCapacityValues + "," + sQuantityValues + "";

                    objApproval.sTableNames = "TBLSTOREINDENT;TBLSINDENTOBJECTS";

                    if (objTcTransfer.sActionType == "M")
                    {
                        objApproval.SaveWorkFlowData(objApproval);
                        objTcTransfer.sWFDataId = objApproval.sWFDataId;
                    }
                    else
                    {

                        objApproval.SaveWorkFlowData(objApproval);
                        objApproval.sRecordId = objApproval.GetRecordIdForWorkFlow();
                        objApproval.SaveWorkflowObjects(objApproval);
                    }

                    #endregion



                    Arr[0] = "Saved Successfully";
                    Arr[1] = "0";
                    return Arr;
                }
                else
                {
                    OleDbCommand oleDbCommand1 = new OleDbCommand();
                    oleDbCommand1.Parameters.AddWithValue("IndentNO", objTcTransfer.sIndentNo);
                    
                    ObjCon.BeginTrans();

                    dt = ObjCon.getDataTable("select SI_NO from TBLSTOREINDENT where SI_NO=:IndentNO and SI_ID <> '" + objTcTransfer.sSiId + "'");
                    if (dt.Rows.Count >0)
                    {                        
                        Arr[0] = "Entered Indent Number Already Exists";
                        Arr[1] = "2";
                        return Arr;
                    }
                    strQry = "UPDATE TBLSTOREINDENT SET SI_TO_STORE='" + objTcTransfer.sToStoreId + "',SI_DESC='" + objTcTransfer.sDescription + "',SI_NO='" + objTcTransfer.sIndentNo + "',SI_DATE=to_date('" + objTcTransfer.sIndentDate + "','dd/MM/yyyy') WHERE SI_ID='" + objTcTransfer.sSiId + "'";
                    ObjCon.Execute(strQry);

                    //deleting old records
                    strQry = "DELETE FROM TBLSINDENTOBJECTS WHERE SO_SI_ID='" + objTcTransfer.sSiId + "'";
                    ObjCon.Execute(strQry);

                    for (int i = 0; i < objTcTransfer.ddtCapacityGrid.Rows.Count; i++)
                    {
                        //inserting updated grid records
                        objTcTransfer.sSoId = Convert.ToString(ObjCon.Get_max_no("SO_ID", "TBLSINDENTOBJECTS"));
                        strQry = "INSERT INTO TBLSINDENTOBJECTS(SO_ID,SO_SI_ID,SO_CAPACITY,SO_QNTY,SO_CRBY,SO_CRON) VALUES('" + objTcTransfer.sSoId + "','" + objTcTransfer.sSiId + "','" + objTcTransfer.ddtCapacityGrid.Rows[i]["SO_CAPACITY"] + "','" + objTcTransfer.ddtCapacityGrid.Rows[i]["SO_QNTY"] + "','" + objTcTransfer.sCrBy + "',SYSDATE)";
                        ObjCon.Execute(strQry);
                    }

                    ObjCon.CommitTrans();
                    Arr[0] = "Updated Successfully";
                    Arr[1] = "1";
                    return Arr;
                }

            }

            catch (Exception ex)
            {
                ObjCon.RollBack();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "SaveStoreTransfer");
                return Arr;
            }
            finally
            {
                
            }
        }

        public DataTable LoadIndentGrid(string sOfficeCode)
        {
            string strQry = string.Empty;
            DataTable dtIndentDetails = new DataTable();
            OleDbCommand oleDbCommand = new OleDbCommand();
            try
            {
                
                if (sOfficeCode.Length >= 3)
                {
                    sOfficeCode = sOfficeCode.Substring(0, 2);
                }

                strQry = "SELECT SI_ID,TO_CHAR(SI_DATE,'dd-MON-yyyy')SI_DATE,SI_NO,SUM(SO_QNTY)SO_QNTY,";
                strQry += " (SELECT SM_NAME FROM TBLSTOREMAST WHERE SM_ID=SI_TO_STORE)SI_TO_STORE FROM TBLSTOREINDENT,TBLSINDENTOBJECTS ";
                strQry += " WHERE SI_ID=SO_SI_ID and SI_TRANSFER_FLAG=0 AND (SELECT SM_OFF_CODE FROM TBLSTOREMAST WHERE SM_ID=SI_FROM_STORE) LIKE :OfficeCode ||'%'";
                strQry += " GROUP BY SI_NO,SI_ID,SI_DATE,SI_TO_STORE ORDER BY SI_NO DESC";
                //strQry += " AND  (SELECT SM_OFF_CODE FROM TBLSTOREMAST WHERE SM_ID=SI_FROM_STORE)";
                //LIKE '" + sOfficeCode + "%'
                oleDbCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                dtIndentDetails = ObjCon.getDataTable(strQry, oleDbCommand);
                return dtIndentDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadIndentGrid");
                return dtIndentDetails;
            }
            finally
            {
                
            }
        }
        
        public DataTable LoadCompletedIndentGrid(string sOfficeCode)
        {
            string strQry = string.Empty;
            DataTable dtIndentDetails = new DataTable();
            OleDbCommand oleDbCommand = new OleDbCommand();
            try
            {
                
                if (sOfficeCode.Length >= 3)
                {
                    sOfficeCode = sOfficeCode.Substring(0, 2);
                }

                strQry = "SELECT SI_ID,TO_CHAR(SI_DATE,'dd-MON-yyyy')SI_DATE,SI_NO,SUM(SO_QNTY)SO_QNTY,";
                strQry += " (SELECT SM_NAME FROM TBLSTOREMAST WHERE SM_ID=SI_TO_STORE)SI_TO_STORE FROM TBLSTOREINDENT,TBLSINDENTOBJECTS ";
                strQry += " WHERE SI_ID=SO_SI_ID and SI_TRANSFER_FLAG=1 AND (SELECT SM_OFF_CODE FROM TBLSTOREMAST WHERE SM_ID=SI_FROM_STORE) LIKE :OfficeCode || '%'";
                strQry += " GROUP BY SI_NO,SI_ID,SI_DATE,SI_TO_STORE ORDER BY SI_NO DESC";
                //strQry += " AND  (SELECT SM_OFF_CODE FROM TBLSTOREMAST WHERE SM_ID=SI_FROM_STORE)";
                //LIKE '" + sOfficeCode + "%'
                oleDbCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
                dtIndentDetails = ObjCon.getDataTable(strQry, oleDbCommand);
                return dtIndentDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadCompletedIndentGrid");
                return dtIndentDetails;
            }
            finally
            {
                
            }
        }

        public Boolean CheckForInvoice(string strIndentId)
        {
            string strQry = string.Empty;
            bool istrue = false;
            DataTable dt = new DataTable();
            OleDbCommand oleDbCommand = new OleDbCommand();
            try
            {
                
                strQry = "SELECT IS_SI_ID FROM TBLSTOREINVOICE WHERE IS_SI_ID='" + strIndentId + "'";
                oleDbCommand.Parameters.AddWithValue("SIID", strIndentId);
                dt = ObjCon.getDataTable(strQry, oleDbCommand);
                if (dt.Rows.Count >0)
                {
                    istrue = true;
                }
                return istrue;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "CheckForInvoice");
                return istrue;
            }
            finally
            {
                
            }
        }

        public object GetIndentDetails(clsStoreIndent objTcTransfer)
        {
            string strQry = string.Empty;
            OleDbCommand oleDbCommand = new OleDbCommand();
            try
            {
                
                DataTable dtIndentDetails = new DataTable();
                strQry = "SELECT SI_ID,TO_CHAR(SI_DATE,'dd/MM/yyyy')SI_DATE,SI_DESC,SI_TO_STORE,SI_NO,SO_CAPACITY,";
                strQry += " SO_QNTY FROM TBLSTOREINDENT,TBLSINDENTOBJECTS WHERE SI_ID=SO_SI_ID AND SI_ID=:SIID";
                oleDbCommand.Parameters.AddWithValue("SIID", objTcTransfer.sIndentId);
                dtIndentDetails = ObjCon.getDataTable(strQry, oleDbCommand);
                objTcTransfer.sSiId = Convert.ToString(dtIndentDetails.Rows[0]["SI_ID"]);
                objTcTransfer.sIndentNo = Convert.ToString(dtIndentDetails.Rows[0]["SI_NO"]);
                objTcTransfer.sIndentDate = Convert.ToString(dtIndentDetails.Rows[0]["SI_DATE"]);
                objTcTransfer.sDescription = Convert.ToString(dtIndentDetails.Rows[0]["SI_DESC"]);
                objTcTransfer.sToStoreId = Convert.ToString(dtIndentDetails.Rows[0]["SI_TO_STORE"]);
                return objTcTransfer;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetIndentDetails");
                return objTcTransfer;

            }
            finally
            {
                
            }
        }

        public DataTable LoadCapacityGrid(clsStoreIndent objTcTransfer)
        {
            string strQry = string.Empty;
            DataTable dtCapacityDetails = new DataTable();
            OleDbCommand oleDbCommand = new OleDbCommand();
            try
            {                
                strQry = "SELECT SO_ID,SO_CAPACITY,SO_QNTY FROM TBLSINDENTOBJECTS,TBLSTOREINDENT WHERE SO_SI_ID=SI_ID AND SI_ID=:SIID";
                oleDbCommand.Parameters.AddWithValue("SIID", objTcTransfer.sIndentId);
                dtCapacityDetails = ObjCon.getDataTable(strQry, oleDbCommand);

                return dtCapacityDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadCapacityGrid");
                return dtCapacityDetails;

            }
            finally
            {
                
            }
        }

        public string GetTransformerCount(string sStoreId)
        {
            try
            {
                OleDbCommand oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;
               
                strQry = "SELECT COUNT(*) FROM TBLTCMASTER WHERE TC_STORE_ID =:STOREID AND ";
                strQry += " TC_STATUS IN (1,2) AND TC_CURRENT_LOCATION=1";
                oleDbCommand.Parameters.AddWithValue("STOREID", sStoreId);
                return ObjCon.get_value(strQry, oleDbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetTransformerCount");
                return "";
            }
            finally
            {
                
            }
        }

        public DataTable LoadStoreCapacityGrid(string sStoreId)
        {
            string strQry = string.Empty;
            DataTable dtCapacityDetails = new DataTable();
            OleDbCommand oleDbCommand = new OleDbCommand();
            try
            {
                
                strQry = " SELECT COUNT(*) STOCKCOUNT,TO_CHAR(TC_CAPACITY)TC_CAPACITY,(SELECT SUBSTR(OFF_NAME, INSTR(OFF_NAME, ':')+2) ";
                strQry += " FROM VIEW_ALL_OFFICES WHERE OFF_CODE=SM_OFF_CODE) SM_OFF_CODE FROM TBLTCMASTER,TBLSTOREMAST WHERE TC_STORE_ID =:STOREID AND TC_STORE_ID=SM_ID ";
                strQry += " AND TC_STATUS IN (1,2) AND TC_CURRENT_LOCATION=1 AND TC_CAPACITY IS NOT NULL GROUP BY TC_CAPACITY,SM_OFF_CODE";
                oleDbCommand.Parameters.AddWithValue("STOREID", sStoreId);
                dtCapacityDetails = ObjCon.getDataTable(strQry, oleDbCommand);

                return dtCapacityDetails;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadStoreCapacityGrid");
                return dtCapacityDetails;

            }
            finally
            {
                
            }
        }

        #region WorkFlow XML

        public clsStoreIndent GetStoreIndentDetailsFromXML(clsStoreIndent objStoreIndent)
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                DataTable dtIndentDetails = new DataTable();
                DataSet ds = new DataSet();

                ds = objApproval.GetDatatableFromMultipleXML(objStoreIndent.sWFDataId);
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    if (ds.Tables[i].Rows.Count > 0)
                    {
                        dtIndentDetails = ds.Tables[i];
                        if (i == 0)
                        {
                            //objStoreIndent.sSiId = Convert.ToString(dtIndentDetails.Rows[0]["SI_ID"]);
                            objStoreIndent.sIndentNo = Convert.ToString(dtIndentDetails.Rows[0]["SI_NO"]);
                            objStoreIndent.sIndentDate = Convert.ToString(dtIndentDetails.Rows[0]["SI_DATE"]);
                            objStoreIndent.sDescription = Convert.ToString(dtIndentDetails.Rows[0]["SI_DESC"]);
                            objStoreIndent.sToStoreId = Convert.ToString(dtIndentDetails.Rows[0]["SI_TO_STORE"]);
                            objStoreIndent.sCrBy = Convert.ToString(dtIndentDetails.Rows[0]["SI_CRBY"]);
                        }
                        else
                        {
                            objStoreIndent.sIndentObjectid = Convert.ToString(dtIndentDetails.Rows[0]["SO_ID"]);
                            objStoreIndent.sTcCapacity = Convert.ToString(dtIndentDetails.Rows[0]["SO_CAPACITY"]);
                            objStoreIndent.sQuantity = Convert.ToString(dtIndentDetails.Rows[0]["SO_QNTY"]);

                            if (objStoreIndent.sTcCapacity.EndsWith("`"))
                            {
                                objStoreIndent.sTcCapacity = objStoreIndent.sTcCapacity.Remove(objStoreIndent.sTcCapacity.Length - 1);
                            }

                            if (objStoreIndent.sQuantity.EndsWith("`"))
                            {
                                objStoreIndent.sQuantity = objStoreIndent.sQuantity.Remove(objStoreIndent.sQuantity.Length - 1);
                            }


                            objStoreIndent.ddtCapacityGrid = CreateDatatableFromString(objStoreIndent);
                            //objStoreIndent.sIndentObjectid = "{1}";
                            //objStoreIndent.sTcCapacity = "750`125`";
                            //objStoreIndent.sQuantity = "2`1`";
                        }
                    }
                }
               
                return objStoreIndent;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetStoreIndentDetailsFromXML");
                return objStoreIndent;
            }
            finally
            {
                
            }
        }


        public DataTable CreateDatatableFromString(clsStoreIndent objStoreIndent)
        {
          
            DataTable dt = new DataTable();

            dt.Columns.Add("SO_ID");
            dt.Columns.Add("SO_CAPACITY");
            dt.Columns.Add("SO_QNTY");

            string[] strdtColumns = objStoreIndent.sTcCapacity.Split('`');
            string[] strdtParametres = objStoreIndent.sQuantity.Split('`');
            for (int i = 0; i < strdtColumns.Length; i++)
            {

                for (int j = 0; j < strdtParametres.Length; j++)
                {
                    if (strdtColumns[j] != "")
                    {
                        DataRow dRow = dt.NewRow();
                        dRow["SO_ID"] = i;
                        dRow["SO_CAPACITY"] = strdtColumns[i];
                        dRow["SO_QNTY"] = strdtParametres[j];
                        dt.Rows.Add(dRow);
                        dt.AcceptChanges();
                    }
                    i++;
                }
            }
            return dt;
        }

        #endregion

        public string GetOfficeCodeFromStore(string sStoreId)
        {
            try
            {
                OleDbCommand oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;
                strQry = "SELECT SM_OFF_CODE FROM TBLSTOREMAST WHERE SM_ID=:SMID";
                oleDbCommand.Parameters.AddWithValue("SMID", sStoreId);
                return ObjCon.get_value(strQry, oleDbCommand);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetOfficeCodeFromStore");
                return ex.Message;
            }
            finally
            {
                
            }
        }

    }
}