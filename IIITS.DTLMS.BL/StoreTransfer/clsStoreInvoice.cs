using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.DAL;
using IIITS.DTLMS.BL;
using System.Data.OleDb;
using System.Data;
namespace IIITS.DTLMS.BL
{
   public class clsStoreInvoice
    {
       CustOledbConnection ObjCon = new CustOledbConnection(Constants.Password);
        string strFormCode = "clsStoreInvoice";
        public string sQuantity { get; set; }
        public string sTcCapacity { get; set; }
        public string sIndentNo { get; set; }
        public string sIndentDate { get; set; }
        public string sDescription { get; set; }
        public string sInvoiceNo { get; set; }
        public string sInvoiceDate { get; set; }
        public string sRemarks{ get; set; }
        public string sInvoiceId { get; set; }
        public string sCrBy { get; set; }
        public DataTable ddtTcGrid { get; set; }
        public string sIndentId { get; set; }
        public string sTcSlNo { get; set; }
        public string sInvoiceObjectId { get; set; }
        public string sSiId { get; set; }
        public string sFromStoreId { get; set; }
        public string sTcCode{ get; set; }
        public string sTcName { get; set; }
        public string sTcId { get; set; }
        public DataTable dIndentTcGrid { get; set; }

        // Workflow
        public string sFormName { get; set; }
        public string sClientIP { get; set; }
        public string sWFDataId { get; set; }
        public string sActionType { get; set; }
        public string sWFOId { get; set; }

        public string sOfficeCode { get; set; }
        public string sRefOfficeCode { get; set; }
        public string sRecordId { get; set; }

        //To Load Indent details to grid      
       public DataTable LoadInvoiceGrid(string sOfficeCode)
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

               strQry = "SELECT SI_ID, SI_DATE,SI_NO, REQ_QNTY, SI_FROM_STORE, (NVL(REQ_QNTY,0) - NVL(SENT_QNT,0)) AS PENDINGCOUNT,IS_NO  ";
               strQry += "FROM (SELECT SI_ID,TO_CHAR(SI_DATE,'dd-MON-yyyy')SI_DATE, SI_NO, SUM(SO_QNTY) REQ_QNTY, ";
               strQry += "(SELECT SM_NAME FROM TBLSTOREMAST WHERE SM_ID=SI_TO_STORE) SI_TO_STORE ,";
               strQry += " (SELECT SM_NAME FROM TBLSTOREMAST WHERE SM_ID=SI_FROM_STORE) SI_FROM_STORE ";
               strQry += "FROM TBLSTOREINDENT, TBLSINDENTOBJECTS WHERE SI_ID = SO_SI_ID and SI_TRANSFER_FLAG=0  ";
               strQry += "AND (SELECT SM_OFF_CODE FROM TBLSTOREMAST WHERE SM_ID=SI_TO_STORE) like :OfficeCode ||'%' GROUP BY SI_ID,SI_DATE,SI_TO_STORE, SI_NO,SI_FROM_STORE)A,";
               strQry += "(SELECT IS_SI_ID, COUNT(IO_CAPACITY) AS SENT_QNT,IS_NO ";
               strQry += "FROM TBLSTOREINVOICE, TBLSINVOICEOBJECTS WHERE IS_ID = IO_IS_ID GROUP BY IS_SI_ID,IS_NO )B WHERE A.SI_ID= B.IS_SI_ID(+) AND B.IS_NO IS NULL ORDER BY SI_NO DESC";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
              dtIndentDetails =ObjCon.getDataTable(strQry, oleDbCommand);
              return dtIndentDetails;
           }
           catch (Exception ex)
           {
               clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadIndentDetails");
               return dtIndentDetails;
           }
           finally
           {
               
           }
       }



       public DataTable LoadCompletedInvoiceGrid(string sOfficeCode)
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

               strQry = "SELECT SI_ID, SI_DATE,SI_NO, REQ_QNTY, SI_FROM_STORE, (NVL(REQ_QNTY,0) - NVL(SENT_QNT,0)) AS PENDINGCOUNT,IS_NO  ";
               strQry += "FROM (SELECT SI_ID,TO_CHAR(SI_DATE,'dd-MON-yyyy')SI_DATE, SI_NO, SUM(SO_QNTY) REQ_QNTY, ";
               strQry += "(SELECT SM_NAME FROM TBLSTOREMAST WHERE SM_ID=SI_TO_STORE) SI_TO_STORE ,";
               strQry += " (SELECT SM_NAME FROM TBLSTOREMAST WHERE SM_ID=SI_FROM_STORE) SI_FROM_STORE ";
               strQry += "FROM TBLSTOREINDENT, TBLSINDENTOBJECTS WHERE SI_ID = SO_SI_ID and SI_TRANSFER_FLAG=1  ";
               strQry += "AND (SELECT SM_OFF_CODE FROM TBLSTOREMAST WHERE SM_ID=SI_TO_STORE) like :OfficeCode ||'%' GROUP BY SI_ID,SI_DATE,SI_TO_STORE, SI_NO,SI_FROM_STORE)A,";
               strQry += "(SELECT IS_SI_ID, COUNT(IO_CAPACITY) AS SENT_QNT,IS_NO ";
               strQry += "FROM TBLSTOREINVOICE, TBLSINVOICEOBJECTS WHERE IS_ID = IO_IS_ID GROUP BY IS_SI_ID,IS_NO )B WHERE A.SI_ID= B.IS_SI_ID(+) ORDER BY SI_NO DESC";
                oleDbCommand.Parameters.AddWithValue("OfficeCode", sOfficeCode);
               dtIndentDetails = ObjCon.getDataTable(strQry, oleDbCommand);
               return dtIndentDetails;
           }
           catch (Exception ex)
           {
               clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadIndentDetails");
               return dtIndentDetails;
           }
           finally
           {
               
           }
       }

       //Function to Populate indent grid values to textbox
       public object LoadIndentDetails(clsStoreInvoice objInvoice)
       {
           string strQry = string.Empty;
           DataTable dtIndentDetails = new DataTable();
            OleDbCommand oleDbCommand = new OleDbCommand();
            try
           {
               
               strQry = "SELECT SI_ID,TO_CHAR(SI_DATE,'dd/MM/yyyy')SI_DATE,SI_NO,SUM(SO_QNTY)SO_QNTY,SI_FROM_STORE FROM TBLSTOREINDENT,TBLSINDENTOBJECTS where SI_ID=SO_SI_ID  ";
               if (objInvoice.sIndentId != "")
               {
                   strQry += " AND SI_ID=:SIID";
                    oleDbCommand.Parameters.AddWithValue("SIID", objInvoice.sIndentId);
               }
               if (objInvoice.sIndentNo != "")
               {
                    OleDbCommand oleDbCommand1 = new OleDbCommand();
                    oleDbCommand1.Parameters.AddWithValue("SINO", objInvoice.sIndentNo);
                    string strIndentNo = ObjCon.get_value("SELECT SI_NO FROM TBLSTOREINDENT WHERE SI_NO=:SINO", oleDbCommand1);
                  if (strIndentNo != "")
                  {
                      strQry += " AND SI_NO=:SINO";
                        oleDbCommand.Parameters.AddWithValue("SINO", objInvoice.sIndentNo);
                    }
                  else
                  {
                      objInvoice.sIndentNo = "";
                      return objInvoice;
                  }
               }
               strQry += " GROUP BY SI_NO,SI_ID,SI_DATE,SI_FROM_STORE";
               dtIndentDetails = ObjCon.getDataTable(strQry, oleDbCommand);
               objInvoice.sIndentId = Convert.ToString(dtIndentDetails.Rows[0]["SI_ID"]);
               objInvoice.sIndentNo = Convert.ToString(dtIndentDetails.Rows[0]["SI_NO"]);
               objInvoice.sIndentDate = Convert.ToString(dtIndentDetails.Rows[0]["SI_DATE"]);
               objInvoice.sQuantity = Convert.ToString(dtIndentDetails.Rows[0]["SO_QNTY"]);
               objInvoice.sFromStoreId= Convert.ToString(dtIndentDetails.Rows[0]["SI_FROM_STORE"]);
               return objInvoice;
           }
           catch (Exception ex)
           {
               clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadIndentDetails");
               return objInvoice;
           }
           finally
           {
               
           }
       }
       //Function to load capacity grid 
       public DataTable LoadCapacityGrid(clsStoreInvoice objInvoice)
       {
           string strQry = string.Empty;
           DataTable dtCapacityDetails = new DataTable();
            OleDbCommand oleDbCommand = new OleDbCommand();
            try
           {
               
               strQry ="SELECT SI_ID,CAPACITY,REQ_QNTY,(NVL(REQ_QNTY,0) - NVL(SENT_QNT,0)) AS PENDINGCOUNT  FROM"; 
               strQry +=" (SELECT SI_ID,SUM(SO_QNTY) REQ_QNTY,SO_CAPACITY AS CAPACITY FROM TBLSTOREINDENT, TBLSINDENTOBJECTS WHERE SI_ID = SO_SI_ID GROUP BY SI_ID, SO_CAPACITY)A,";
               strQry += " (SELECT IS_SI_ID,IO_CAPACITY,COUNT(IO_CAPACITY) AS SENT_QNT FROM TBLSTOREINVOICE, TBLSINVOICEOBJECTS WHERE IS_ID = IO_IS_ID GROUP BY IS_SI_ID,IO_CAPACITY)B";
               strQry += " WHERE A.SI_ID= B.IS_SI_ID(+) AND A.CAPACITY=B.IO_CAPACITY(+) AND A.SI_ID=:SIID";
                oleDbCommand.Parameters.AddWithValue("SIID", objInvoice.sIndentId);
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
       //Function to save invoice details
       public string[] SaveStoreInvoice(clsStoreInvoice objInvoice)
       {
           string strQry = string.Empty;
           string[] Arr = new string[2];
            DataTable dt = new DataTable();
            OleDbCommand oleDbCommand = new OleDbCommand();
            try
           {
               
               if (objInvoice.sInvoiceId == "")
               {
                    //To get the Store id of logged in user
                    oleDbCommand.Parameters.AddWithValue("ISNO", objInvoice.sInvoiceNo);
                    dt = ObjCon.getDataTable("select IS_NO from TBLSTOREINVOICE where IS_NO=:ISNO", oleDbCommand);
                   if (dt.Rows.Count >0)
                   {
                       Arr[0] = "Entered Invoice Number Already Exists";
                       Arr[1] = "2";
                       return Arr;
                   }                   


                   //strQry = "DELETE FROM TBLSINVOICEOBJECTS WHERE IO_IS_ID IN(select IS_ID FROM TBLSTOREINVOICE,TBLSTOREINDENT WHERE IS_SI_ID='" + objInvoice.sIndentId + "')";
                   //ObjCon.Execute(strQry);
                   objInvoice.sInvoiceId = Convert.ToString(ObjCon.Get_max_no("IS_ID", "TBLSTOREINVOICE"));
                   strQry = "INSERT INTO TBLSTOREINVOICE(IS_ID,IS_NO,IS_SI_ID,IS_DATE,IS_REMARKS,IS_CRBY,IS_CRON) VALUES ";
                   strQry += " ('" + objInvoice.sInvoiceId + "','" + objInvoice.sInvoiceNo + "','" + objInvoice.sIndentId + "',";
                   strQry += " to_date('" + objInvoice.sInvoiceDate + "','dd/MM/yyyy'),'" + objInvoice.sRemarks + "','" + objInvoice.sCrBy + "',SYSDATE)";
                   ObjCon.Execute(strQry);
                 
                   for (int i = 0; i < objInvoice.ddtTcGrid.Rows.Count; i++)
                   {
                       objInvoice.sInvoiceObjectId = Convert.ToString(ObjCon.Get_max_no("IO_ID", "TBLSINVOICEOBJECTS"));
                       strQry = "INSERT INTO TBLSINVOICEOBJECTS(IO_ID,IO_IS_ID,IO_CAPACITY,IO_TCCODE,IO_CRBY,IO_CRON) VALUES ";
                       strQry += " ('" + objInvoice.sInvoiceObjectId + "','" + objInvoice.sInvoiceId + "',";
                       strQry += " '" + objInvoice.ddtTcGrid.Rows[i]["TC_CAPACITY"] + "','" + objInvoice.ddtTcGrid.Rows[i]["TC_CODE"] + "',";
                       strQry += " '" + objInvoice.sCrBy + "',SYSDATE)";
                       ObjCon.Execute(strQry);
                       //strQry = "Update TBLSTOREINDENT SET SI_TRANSFER_FLAG=1 WHERE SI_ID='" + objInvoice.sIndentId +"'";
                       //ObjCon.Execute(strQry);
                       strQry = "Update TBLTCMASTER SET TC_CURRENT_LOCATION=4 WHERE TC_CODE='" + ddtTcGrid.Rows[i]["TC_CODE"] + "'";
                       ObjCon.Execute(strQry);
                   }

                   UpdateIndentStatus(objInvoice);


                   #region WorkFlow

                   //Workflow / Approval
                   clsApproval objApproval = new clsApproval();
                   objApproval.sFormName = objInvoice.sFormName;
                   objApproval.sRecordId = objInvoice.sRecordId;
                   objApproval.sNewRecordId = objInvoice.sInvoiceId;
                   objApproval.sOfficeCode = objInvoice.sOfficeCode;
                   objApproval.sClientIp = objInvoice.sClientIP;
                   objApproval.sCrby = objInvoice.sCrBy;
                   objApproval.sWFObjectId = objInvoice.sWFOId;
                   objApproval.sRefOfficeCode = objInvoice.sRefOfficeCode;
                   objApproval.sDescription = "Store Invoice Creation for Indent No " + objInvoice.sIndentNo;
                   objApproval.sApproveStatus = "1";
                   objApproval.sApproveComments = "Approved";
                   objApproval.ApproveWFRequest(objApproval);

                   #endregion

                   Arr[0] = "Saved Successfully";
                   Arr[1] = "0";
                   return Arr;
               }
               else
               {
                    OleDbCommand oleDbCommand1 = new OleDbCommand();
                    oleDbCommand1.Parameters.AddWithValue("InvoiceNO", objInvoice.sInvoiceNo);
                    ObjCon.BeginTrans();

                   dt = ObjCon.getDataTable("select IS_NO from TBLSTOREINVOICE where IS_NO=:InvoiceNO and IS_ID<>'" + objInvoice.sInvoiceId+"'", oleDbCommand1);
                   if (dt.Rows.Count > 0)
                   {
                       Arr[0] = "Entered Invoice Number Already Exists";
                       Arr[1] = "2";
                       return Arr;
                   }                   
                   strQry = "UPDATE TBLSTOREINVOICE SET IS_NO='" + objInvoice.sInvoiceNo + "',IS_REMARKS='" + objInvoice.sRemarks + "',IS_DATE=to_date('" + objInvoice.sInvoiceDate + "','dd/MM/yyyy') WHERE IS_ID='" + objInvoice.sInvoiceId + "'";
                   ObjCon.Execute(strQry);
                   //deleting old records
                   strQry = "DELETE FROM TBLSINVOICEOBJECTS WHERE IO_IS_ID='" + objInvoice.sInvoiceId + "'";
                   ObjCon.Execute(strQry);
                   for (int i = 0; i < objInvoice.ddtTcGrid.Rows.Count; i++)
                   {
                       objInvoice.sInvoiceObjectId = Convert.ToString(ObjCon.Get_max_no("IO_ID", "TBLSINVOICEOBJECTS"));
                       strQry = "INSERT INTO TBLSINVOICEOBJECTS(IO_ID,IO_IS_ID,IO_CAPACITY,IO_TCCODE,IO_CRBY,IO_CRON) VALUES('" + objInvoice.sInvoiceObjectId + "','" + objInvoice.sInvoiceId + "','" + objInvoice.ddtTcGrid.Rows[i]["TC_CAPACITY"] + "','" + objInvoice.ddtTcGrid.Rows[i]["TC_CODE"] + "','" + objInvoice.sCrBy + "',SYSDATE)";
                       ObjCon.Execute(strQry);

                       strQry = "Update TBLTCMASTER SET TC_CURRENT_LOCATION=4 WHERE TC_CODE='" + ddtTcGrid.Rows[i]["TC_CODE"] + "' AND TC_CURRENT_LOCATION<>4";
                       ObjCon.Execute(strQry);
                       
                   }
                   UpdateIndentStatus(objInvoice);

                   ObjCon.CommitTrans();
                   Arr[0] = "Updated Successfully";
                   Arr[1] = "1";
                   return Arr;


               }

           }
           catch (Exception ex)
           {
               ObjCon.RollBack();
               clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "SaveStoreInvoice");
               return Arr;
           }
           finally
           {
               
           }
       }
       public void UpdateIndentStatus(clsStoreInvoice objInvoice)
       {
           string strQry = string.Empty;
            OleDbCommand oleDbCommand = new OleDbCommand();
            try
           {
                oleDbCommand.Parameters.AddWithValue("SIID", objInvoice.sIndentId);
               string strInvoiceCount=ObjCon.get_value("SELECT COUNT(IO_CAPACITY)AS Count FROM TBLSINVOICEOBJECTS,TBLSTOREINVOICE WHERE IO_IS_ID=IS_ID AND IS_SI_ID=:SIID", oleDbCommand);
          
               //if (Convert.ToInt32(strInvoiceCount) ==Convert.ToInt32(objInvoice.sQuantity))
               //{ 
                   strQry = "UPDATE TBLSTOREINDENT SET SI_TRANSFER_FLAG=1 WHERE SI_ID='" + objInvoice.sIndentId + "'";
                   ObjCon.Execute(strQry);
               //}
           }
           catch (Exception ex)
           {
               clsException.LogError(ex.StackTrace,ex.Message, strFormCode, " UpdateIndentStatus");
           }
           finally
           {
               
           }
       }
       public object CheckTc(clsStoreInvoice objInvoice)
       {
           string strQry = string.Empty;
            OleDbCommand oleDbCommand = new OleDbCommand();
            try
           {
                oleDbCommand.Parameters.AddWithValue("TCCODE", objInvoice.sTcCode);
               string strTcCode = ObjCon.get_value("Select TC_CODE FROM TBLTCMASTER WHERE TC_CODE=:TCCODE", oleDbCommand);
               if (strTcCode == "")
              {
                  return objInvoice.sTcCode = "";
              }
              else
              {
                  objInvoice.sTcCode = strTcCode;
                  return objInvoice.sTcCode;
              }

           }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "CheckTc");
                return objInvoice;
            }
           finally
           {
               
           }
       }
        //Function to Load Tc Details Grid
        public object LoadTcDetails(clsStoreInvoice objInvoice)
        {
            string strQry = string.Empty;
            DataTable dtTcDetails = new DataTable();
            OleDbCommand oleDbCommand = new OleDbCommand();
            try
            {
                oleDbCommand.Parameters.AddWithValue("TCCODE", objInvoice.sTcCode);
                string strTcCode = ObjCon.get_value("SELECT IO_TCCODE FROM TBLSINVOICEOBJECTS,TBLSTOREINVOICE WHERE IO_TCCODE=:TCCODE AND IS_APPROVE_FLAG='0' AND IO_IS_ID=IS_ID", oleDbCommand);
                if (strTcCode != "")
                {
                    objInvoice.sTcCode = "";
                }
                else
                {
                    OleDbCommand oleDbCommand1 = new OleDbCommand();
                    strQry = "SELECT TC_ID,TC_SLNO,TC_CODE,TO_CHAR(TC_CAPACITY)TC_CAPACITY,(SELECT TM_NAME FROM TBLTRANSMAKES WHERE ";
                    strQry += " TM_ID =TC_MAKE_ID)TM_NAME FROM tbltcmaster WHERE TC_CODE=:TCCODE AND ";
                    strQry += " TC_CURRENT_LOCATION =1 AND TC_LOCATION_ID = :OfficeCode AND TC_CAPACITY IN ";
                    strQry += "(SELECT SO_CAPACITY FROM TBLSINDENTOBJECTS WHERE SO_SI_ID=:SIID)";
                    oleDbCommand1.Parameters.AddWithValue("TCCODE", objInvoice.sTcCode);
                    oleDbCommand1.Parameters.AddWithValue("OfficeCode", objInvoice.sOfficeCode);
                    oleDbCommand1.Parameters.AddWithValue("SIID", objInvoice.sIndentId);
                    dtTcDetails = ObjCon.getDataTable(strQry, oleDbCommand1);
                    if (dtTcDetails.Rows.Count > 0)
                    {
                        objInvoice.sTcId = Convert.ToString(dtTcDetails.Rows[0]["TC_ID"]);
                        objInvoice.sTcSlNo = Convert.ToString(dtTcDetails.Rows[0]["TC_SLNO"]);
                        objInvoice.sTcCode = Convert.ToString(dtTcDetails.Rows[0]["TC_CODE"]);
                        objInvoice.sTcCapacity = Convert.ToString(dtTcDetails.Rows[0]["TC_CAPACITY"]);
                        objInvoice.sTcName = Convert.ToString(dtTcDetails.Rows[0]["TM_NAME"]);
                    }
                    else
                    {
                        objInvoice.sIndentId = "";
                        objInvoice.sTcId = "";
                    }
                    return objInvoice;
                }
                return objInvoice;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadTcDetails");
                return objInvoice;

            }
            finally
            {
                
            }
        }

        public void UpdateDeleteItem(clsStoreInvoice objInvoice)
       {
           string strQry = string.Empty;
           try
           {
               
               strQry = "UPDATE TBLTCMASTER SET TC_CURRENT_LOCATION=1 WHERE TC_CODE='" + objInvoice.sTcCode + "'";
               ObjCon.Execute(strQry);

           }
           catch (Exception ex)
           {
               clsException.LogError(ex.StackTrace, ex.Message, strFormCode, " UpdateDeleteItem");
           }
           finally
           {
               
           }
       }

       public DataTable LoadDtrDetails(clsStoreInvoice objInvoice)
       {
           string strQry = string.Empty;
           DataTable dtBasicDetails = new DataTable();
           try
           {
                OleDbCommand oleDbCommand = new OleDbCommand();
                strQry = "select TC_CODE,To_char(TC_CAPACITY)TC_CAPACITY,TC_SLNO,TC_MAKE_ID,IS_NO,TO_CHAR(IS_DATE,'dd/MM/yyyy')IS_DATE,(select TM_NAME from TBLTRANSMAKES  where TM_ID=TC_MAKE_ID) as Make ";
               strQry += "from TBLSINVOICEOBJECTS,TBLTCMASTER,TBLSTOREINVOICE where IO_TCCODE=TC_CODE and IS_ID=IO_IS_ID  ";
               if (objInvoice.sIndentId != "")
               {
                   strQry += " AND IS_SI_ID=:SIID";
                    oleDbCommand.Parameters.AddWithValue("SIID", objInvoice.sIndentId);
               }
               dtBasicDetails = ObjCon.getDataTable(strQry, oleDbCommand);
               return dtBasicDetails;
           }
           catch (Exception ex)
           {
               clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadDtrDetails");
               return dtBasicDetails;
           }
           finally
           {
               
           }
       }

       public clsStoreInvoice GetStoreInvoiceDetails(clsStoreInvoice objInvoice)
       {
           try
           {
                OleDbCommand oleDbCommand = new OleDbCommand();
                string strQry = string.Empty;
               DataTable dt = new DataTable();
               strQry = "SELECT IS_NO,TO_CHAR(IS_DATE,'DD/MM/YYYY') IS_DATE,IS_REMARKS FROM TBLSTOREINVOICE WHERE IS_SI_ID=:SIID";
                oleDbCommand.Parameters.AddWithValue("SIID", objInvoice.sIndentId);
               dt = ObjCon.getDataTable(strQry, oleDbCommand);
               if (dt.Rows.Count > 0)
               {
                   objInvoice.sInvoiceNo = Convert.ToString(dt.Rows[0]["IS_NO"]);
                   objInvoice.sInvoiceDate = Convert.ToString(dt.Rows[0]["IS_DATE"]);
                   objInvoice.sRemarks = Convert.ToString(dt.Rows[0]["IS_REMARKS"]);
                  
               }
               return objInvoice;
           }
           catch (Exception ex)
           {
               clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetStoreInvoiceDetails");
               return objInvoice;
           }
           finally
           {
               
           }
       }
    }
     
}
