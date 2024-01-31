using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIITS.DAL;
using System.Data;

namespace IIITS.DTLMS.BL
{
   public  class clsFormValues
    {
       string strFormCode = "clsFormValues";
       public string sFailureId { get; set; }
       public string sWorkOrderId { get; set; }
       public string sIndentId { get; set; }
       public string sInvoiceId { get; set; }
       public string sDecommisionId { get; set; }

       public string sWFInitialId { get; set; }
       public string sTaskType { get; set; }
       public string sTCcode { get; set; }

       CustOledbConnection objcon = new CustOledbConnection(Constants.Password);
       public string GetDTCId(clsFormValues objForm)
       {
           try
           {
               string strQry = string.Empty;
               strQry = "SELECT (SELECT DT_ID FROM TBLDTCMAST WHERE DT_CODE=DF_DTC_CODE) DT_ID FROM TBLDTCFAILURE WHERE DF_ID='"+ objForm.sFailureId +"'";
               return objcon.get_value(strQry);
           }
            catch (Exception ex)
           {
               clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetDTCId");
               return ex.Message;
           }
       }

       #region WorkOrder

       public string GetStatusFlagForWO(clsFormValues objForm)
       {
           try
           {
               string strQry = string.Empty;
               strQry = "SELECT DF_STATUS_FLAG FROM TBLDTCFAILURE WHERE DF_ID='" + objForm.sFailureId + "'";
               return objcon.get_value(strQry);
           }
           catch (Exception ex)
           {
               clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetStatusFlagForWO");
               return ex.Message;
           }
       }

       public clsFormValues GetStatusFlagForWOFromWF(clsFormValues objForm)
       {
           try
           {
               string strQry = string.Empty;
               DataTable dt = new DataTable();

               strQry = "SELECT DF_STATUS_FLAG,WO_RECORD_ID FROM TBLWORKFLOWOBJECTS,TBLDTCFAILURE WHERE ";
               strQry += " (SELECT WOA_PREV_APPROVE_ID FROM TBLWO_OBJECT_AUTO WHERE WOA_INITIAL_ACTION_ID='" + objForm.sWFInitialId + "')= WO_ID";
               strQry += " AND  DF_ID=WO_RECORD_ID";
               dt = objcon.getDataTable(strQry);
               if (dt.Rows.Count > 0)
               {
                   objForm.sFailureId = Convert.ToString(dt.Rows[0]["WO_RECORD_ID"]);
                   objForm.sTaskType = Convert.ToString(dt.Rows[0]["DF_STATUS_FLAG"]);

               }
               else
               {
                   strQry = "SELECT WO_RECORD_ID FROM TBLWORKFLOWOBJECTS WHERE ";
                   strQry += " (SELECT WOA_PREV_APPROVE_ID FROM TBLWO_OBJECT_AUTO WHERE WOA_INITIAL_ACTION_ID='" + objForm.sWFInitialId + "')= WO_ID";
                   objForm.sFailureId = objcon.get_value(strQry);
                   objForm.sTaskType = "3";
               }
              
               return objForm;
           }
           catch (Exception ex)
           {
               clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetStatusFlagForWOFromWF");
               return objForm;
           }
       }
       #endregion

       #region Indent
      

       public string GetStatusFlagForIndent(clsFormValues objForm)
       {
           try
           {
               string strQry = string.Empty;
               string sResult = string.Empty;

               strQry = "SELECT DF_STATUS_FLAG FROM TBLWORKORDER,TBLDTCFAILURE WHERE DF_ID=WO_DF_ID AND WO_SLNO='" + objForm.sWorkOrderId + "' ";
               sResult = objcon.get_value(strQry);
               if (sResult == "")
               {
                   strQry = "SELECT WO_NO FROM TBLWORKORDER WHERE  WO_DF_ID IS NULL AND WO_SLNO='" + objForm.sWorkOrderId + "'";
                   sResult = objcon.get_value(strQry);
                   if (sResult != "")
                   {
                       sResult= "3";
                   }
               }
               else
               {
                   return sResult;
               }

               return sResult;
           }
           catch (Exception ex)
           {
               clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetStatusFlagForIndent");
               return ex.Message;
           }


       }

       public clsFormValues GetStatusFlagForIndentFromWF(clsFormValues objForm)
       {
           try
           {
               string strQry = string.Empty;
               DataTable dt = new DataTable();
             

               strQry = "SELECT DF_STATUS_FLAG,WO_RECORD_ID FROM TBLWORKFLOWOBJECTS,TBLDTCFAILURE,TBLWORKORDER WHERE ";
               strQry += " (SELECT WOA_PREV_APPROVE_ID FROM TBLWO_OBJECT_AUTO WHERE WOA_INITIAL_ACTION_ID='" + objForm.sWFInitialId + "')= WO_ID";
               strQry += " AND  WO_SLNO=WO_RECORD_ID AND DF_ID=WO_DF_ID";
               dt = objcon.getDataTable(strQry);
               if (dt.Rows.Count > 0)
               {
                   objForm.sWorkOrderId = Convert.ToString(dt.Rows[0]["WO_RECORD_ID"]);
                   objForm.sTaskType = Convert.ToString(dt.Rows[0]["DF_STATUS_FLAG"]);
               }
               else
               {
                  
                    strQry = "SELECT WO_RECORD_ID FROM TBLWORKFLOWOBJECTS,TBLWORKORDER WHERE ";
                    strQry += " (SELECT WOA_PREV_APPROVE_ID FROM TBLWO_OBJECT_AUTO WHERE WOA_INITIAL_ACTION_ID='" + objForm.sWFInitialId + "')= WO_ID";
                    strQry += " AND  WO_SLNO=WO_RECORD_ID AND WO_DF_ID IS NULL";
                    objForm.sWorkOrderId = objcon.get_value(strQry);

                    objForm.sTaskType = "3";
                   
               }
               return objForm;
              
           }
           catch (Exception ex)
           {
               clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetStatusFlagForIndent");
               return objForm;
           }


       }

       #endregion


       #region Invoice
     
       public string  GetStatusFlagForInvoiceFromIndent(clsFormValues objForm)
       {
           try
           {
               string strQry = string.Empty;
               string sResult = string.Empty;

               strQry = "SELECT DF_STATUS_FLAG FROM TBLWORKORDER,TBLDTCFAILURE,TBLINDENT WHERE DF_ID=WO_DF_ID  ";
               strQry += " AND WO_SLNO=TI_WO_SLNO  AND TI_ID='" + objForm.sIndentId + "'";
               sResult = objcon.get_value(strQry);
               if (sResult == "")
               {
                   strQry = "SELECT WO_NO FROM TBLWORKORDER,TBLINDENT WHERE WO_SLNO=TI_WO_SLNO AND WO_DF_ID IS NULL ";
                   strQry += " AND TI_ID='" + objForm.sIndentId + "' ";
                   sResult = objcon.get_value(strQry);
                   if (sResult != "")
                   {
                       sResult = "3";
                   }
               }
               else
               {
                   return sResult;
               }

               return sResult;
           }
           catch (Exception ex)
           {
               clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetStatusFlagForInvoiceFromIndent");
               return ex.Message;
           }
       }

       public clsFormValues GetStatusFlagForInvoiceFromWF(clsFormValues objForm)
       {
           try
           {

               string strQry = string.Empty;
               DataTable dt = new DataTable();

               strQry = "SELECT DF_STATUS_FLAG,WO_RECORD_ID FROM TBLWORKFLOWOBJECTS,TBLDTCFAILURE,TBLWORKORDER,TBLINDENT WHERE ";
               strQry += " (SELECT WOA_PREV_APPROVE_ID FROM TBLWO_OBJECT_AUTO WHERE WOA_INITIAL_ACTION_ID='" + objForm.sWFInitialId + "')= WO_ID";
               strQry += " AND  TI_ID=WO_RECORD_ID AND DF_ID=WO_DF_ID AND WO_SLNO=TI_WO_SLNO";
               dt = objcon.getDataTable(strQry);
               if (dt.Rows.Count > 0)
               {
                   objForm.sIndentId = Convert.ToString(dt.Rows[0]["WO_RECORD_ID"]);
                   objForm.sTaskType = Convert.ToString(dt.Rows[0]["DF_STATUS_FLAG"]);
               }
               else
               {
                  
                    strQry = "SELECT WO_RECORD_ID FROM TBLWORKFLOWOBJECTS,TBLWORKORDER,TBLINDENT WHERE ";
                    strQry += " (SELECT WOA_PREV_APPROVE_ID FROM TBLWO_OBJECT_AUTO WHERE WOA_INITIAL_ACTION_ID='" + objForm.sWFInitialId + "')= WO_ID";
                    strQry += " AND  TI_ID=WO_RECORD_ID AND WO_DF_ID IS NULL AND WO_SLNO=TI_WO_SLNO";
                    objForm.sIndentId = objcon.get_value(strQry);

                    objForm.sTaskType = "3";
                   
               }
               return objForm;

           }
           catch (Exception ex)
           {
               clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetStatusFlagForInvoice");
               return objForm;
           }
       }

       #endregion

       #region Decommission
       public string GetStatusFlagForDecommissionFromInvoice(clsFormValues objForm)
       {
           try
           {
               string strQry = string.Empty;
               string sResult = string.Empty;

               strQry = "SELECT DF_STATUS_FLAG FROM TBLWORKORDER,TBLDTCFAILURE,TBLINDENT,TBLDTCINVOICE WHERE DF_ID=WO_DF_ID  ";
               strQry += " AND WO_SLNO=TI_WO_SLNO AND TI_ID=IN_TI_NO  AND IN_NO='" + objForm.sInvoiceId + "'";
               sResult = objcon.get_value(strQry);
               if (sResult == "")
               {
                   //string sInvoiceId = GetInvoiceId(objForm.sDecommisionId);
                   strQry = "SELECT WO_NO FROM TBLWORKORDER,TBLINDENT,TBLDTCINVOICE WHERE WO_SLNO=TI_WO_SLNO AND WO_DF_ID IS NULL ";
                   strQry += " AND IN_NO='" + objForm.sInvoiceId + "' AND TI_ID=IN_TI_NO";
                   sResult = objcon.get_value(strQry);
                   if (sResult != "")
                   {
                       sResult = "3";
                   }
               }
               else
               {
                   return sResult;
               }

               return sResult;
           }
           catch (Exception ex)
           {
               clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetStatusFlagForDecommission");
               return ex.Message;
           }
       }

       public clsFormValues GetStatusFlagForDecommFromWF(clsFormValues objForm)
       {
           try
           {

               string strQry = string.Empty;
               DataTable dt = new DataTable();

               strQry = "SELECT DF_STATUS_FLAG,WO_RECORD_ID FROM TBLWORKFLOWOBJECTS,TBLDTCFAILURE,TBLTCDRAWN WHERE ";
               strQry += " (SELECT WOA_PREV_APPROVE_ID FROM TBLWO_OBJECT_AUTO WHERE WOA_INITIAL_ACTION_ID='" + objForm.sWFInitialId + "')= WO_ID";
               strQry += " AND  TD_INV_NO=WO_RECORD_ID AND DF_ID=TD_DF_ID";
               dt = objcon.getDataTable(strQry);
               if (dt.Rows.Count > 0)
               {
                   objForm.sInvoiceId = Convert.ToString(dt.Rows[0]["WO_RECORD_ID"]);
                   objForm.sTaskType = Convert.ToString(dt.Rows[0]["DF_STATUS_FLAG"]);


               }
               else
               {
                   
                    strQry = "SELECT WO_RECORD_ID FROM TBLWORKFLOWOBJECTS,TBLWORKORDER,TBLINDENT,TBLDTCINVOICE WHERE ";
                    strQry += " (SELECT WOA_PREV_APPROVE_ID FROM TBLWO_OBJECT_AUTO WHERE WOA_INITIAL_ACTION_ID='" + objForm.sWFInitialId + "')= WO_ID";
                    strQry += " AND  IN_NO=WO_RECORD_ID AND WO_DF_ID IS NULL AND WO_SLNO=TI_WO_SLNO AND TI_ID=IN_TI_NO";
                    objForm.sInvoiceId = objcon.get_value(strQry);

                    objForm.sTaskType = "3";
                   
               }
               return objForm;

           }
           catch (Exception ex)
           {
               clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetStatusFlagForDecommission");
               return objForm;
           }
       }


       public string GetStatusFlagForDecommission(clsFormValues objForm)
       {
           try
           {
               string strQry = string.Empty;
               string sResult = string.Empty;

               strQry = "SELECT DISTINCT DF_STATUS_FLAG FROM TBLDTCFAILURE,TBLTCDRAWN,TBLTCREPLACE WHERE DF_ID=TD_DF_ID AND TD_INV_NO=TR_IN_NO   ";
               strQry += " AND TR_ID='" + objForm.sDecommisionId + "'";
               sResult = objcon.get_value(strQry);
               if (sResult == "")
               {
                   string sInvoiceId = GetInvoiceId(objForm.sDecommisionId);
                   strQry = "SELECT WO_NO FROM TBLWORKORDER,TBLINDENT,TBLDTCINVOICE WHERE WO_SLNO=TI_WO_SLNO AND WO_DF_ID IS NULL ";
                   strQry += " AND IN_NO='" + sInvoiceId + "' AND  TI_ID=IN_TI_NO";
                   sResult = objcon.get_value(strQry);
                   if (sResult != "")
                   {
                       sResult = "3";
                   }
               }
               else
               {
                   return sResult;
               }

               return sResult;
           }
           catch (Exception ex)
           {
               clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetStatusFlagForDecommission");
               return ex.Message;
           }
       }


       public clsFormValues GetWOnoForDTCCommission(clsFormValues objForm)
       {
           try
           {
               string strQry = string.Empty;
               DataTable dt = new DataTable();

               //string sInvoiceId = GetInvoiceId(objForm.sDecommisionId);

               strQry = "SELECT TD_TC_NO,WO_SLNO FROM TBLWORKORDER,TBLINDENT,TBLDTCINVOICE,TBLTCDRAWN WHERE ";
               strQry += " WO_SLNO=TI_WO_SLNO AND TI_ID=IN_TI_NO AND IN_NO=TD_INV_NO AND IN_NO='" + objForm.sInvoiceId + "'";
               dt = objcon.getDataTable(strQry);
               if (dt.Rows.Count > 0)
               {
                   objForm.sWorkOrderId = Convert.ToString(dt.Rows[0]["WO_SLNO"]);
                   objForm.sTCcode = Convert.ToString(dt.Rows[0]["TD_TC_NO"]);
               }
               return objForm;
           }
           catch (Exception ex)
           {
               clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetWOnoForDTCCommission");
               return objForm;
           }
       }

       public string GetDTCIdFromWO(string sWOSlno)
       {
           try
           {
               string strQry = string.Empty;

               strQry = "SELECT DT_ID FROM TBLDTCMAST WHERE DT_WO_ID='"+ sWOSlno +"'";
               return objcon.get_value(strQry);
           }
           catch (Exception ex)
           {
               clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetDTCIdFromWO");
               return ex.Message;
           }
       }

       #endregion
    

       public string GetInvoiceId(string sDecommId)
       {
           try
           {
               string strQry = string.Empty;
               strQry = "SELECT IN_NO FROM TBLTCREPLACE,TBLDTCINVOICE WHERE TR_IN_NO=IN_NO AND TR_ID='" + sDecommId + "'";
               return objcon.get_value(strQry);
           }
           catch (Exception ex)
           {
               clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetInvoiceId");
               return ex.Message;
           }
       }

       public string GetWorkOrderId(string sIndentId)
       {
           try
           {
               string strQry = string.Empty;
               strQry = "SELECT TI_WO_SLNO FROM TBLINDENT WHERE  TI_ID='" + sIndentId + "'";
               return objcon.get_value(strQry);
           }
           catch (Exception ex)
           {
               clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetInvoiceId");
               return ex.Message;
           }
       }

       public string GetFailureIdFromWO(string sWOId)
       {
           try
           {
               string strQry = string.Empty;
               strQry = "SELECT WO_DF_ID FROM TBLWORKORDER WHERE WO_SLNO='" + sWOId + "'";
               return objcon.get_value(strQry);
           }
           catch (Exception ex)
           {
               clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetInvoiceId");
               return ex.Message;
           }
       }

       public string GetIndentId(string sInvoiceId)
       {
           try
           {
               string strQry = string.Empty;
               strQry = "SELECT IN_TI_NO FROM TBLDTCINVOICE WHERE  IN_NO='" + sInvoiceId + "'";
               return objcon.get_value(strQry);
           }
           catch (Exception ex)
           {
               clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetIndentId");
               return ex.Message;
           }
       }


       public string GetFailureIdFromInvoice(string sInvoiceId)
       {
           try
           {
               string strQry = string.Empty;
               strQry = "SELECT DF_ID FROM TBLWORKORDER,TBLDTCFAILURE,TBLINDENT,TBLDTCINVOICE WHERE DF_ID=WO_DF_ID  ";
               strQry += " AND WO_SLNO=TI_WO_SLNO AND TI_ID=IN_TI_NO AND IN_NO='" + sInvoiceId + "'";
              
               return objcon.get_value(strQry);
           }
           catch (Exception ex)
           {
               clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetFailureIdFromInvoice");
               return ex.Message;
           }
       }

       public string GetFailureIdFromDecommId(string sDecomm)
       {
           try
           {
               string strQry = string.Empty;
               strQry = "SELECT DISTINCT DF_ID FROM TBLDTCFAILURE,TBLTCDRAWN,TBLTCREPLACE WHERE DF_ID=TD_DF_ID AND ";
               strQry += " TD_INV_NO=TR_IN_NO AND TR_ID='"+ sDecomm +"'";

               return objcon.get_value(strQry);
           }
           catch (Exception ex)
           {
               clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetIndentId");
               return ex.Message;
           }
       }

        public string GetWODataIdForRepairerInvoice(string sRecordId, string sWFOId)
        {
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT WO_DATA_ID FROM TBLWORKFLOWOBJECTS WHERE  WO_ID='" + sWFOId + "'";
                return objcon.get_value(strQry);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetWODataIdForRepairerInvoice");
                return ex.Message;
            }
        }

        #region Store Invoice

        public string GetStoreIndentIdFromWF(string sWFInitialId,string sWFObjectId)
       {
           try
           {
               string strQry = string.Empty;
               string sResult = string.Empty;
               strQry = "SELECT WO_RECORD_ID FROM TBLWORKFLOWOBJECTS WHERE ";
               strQry += " (SELECT WOA_PREV_APPROVE_ID FROM TBLWO_OBJECT_AUTO WHERE WOA_INITIAL_ACTION_ID='" + sWFInitialId + "')= WO_ID";
               sResult = objcon.get_value(strQry);
               if (sResult == "")
               {
                   strQry = "SELECT WO_RECORD_ID FROM TBLWORKFLOWOBJECTS WHERE WO_ID='" + sWFObjectId + "'";
                   sResult = objcon.get_value(strQry);
               }
               return sResult;
           }
           catch (Exception ex)
           {
               clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetStoreIndentIdFromWF");
               return ex.Message;
           }
       }

       public string GetStoreInvoiceIdFromWF( string sWFObjectId)
       {
           try
           {
               string strQry = string.Empty;
               string sResult = string.Empty;
              
               strQry = "SELECT WO_RECORD_ID FROM TBLWORKFLOWOBJECTS WHERE WO_ID='" + sWFObjectId + "'";
               sResult = objcon.get_value(strQry);
               
               return sResult;
           }
           catch (Exception ex)
           {
               clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetStoreIndentIdFromWF");
               return ex.Message;
           }
       }
        #endregion
    }
}
