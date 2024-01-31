using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IIITS.DTLMS.BL;
using System.Globalization;
using System.Data.OleDb;


namespace IIITS.DTLMS.DTCFailure
{
    public partial class PseudoWorkOrder : System.Web.UI.Page
    {
        string strFormCode = "PseudoWorkOrder";     
        clsSession objSession;  
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {              

                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                Form.DefaultButton = cmdSave.UniqueID;
                objSession = (clsSession)Session["clsSession"];
                lblMessage.Text = string.Empty;
                if (!IsPostBack)
                {


                    if (Request.QueryString["TypeValue"] != null && Request.QueryString["TypeValue"].ToString() != "")
                    {
                        txtType.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["TypeValue"])); 
                        ChangeLabelText();
                    }

                    if (Request.QueryString["ReferID"] != null && Request.QueryString["ReferID"].ToString() != "")
                    {
                        //From New DTC
                        if (txtType.Text == "3")
                        {
                            txtWOId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ReferID"]));
                            GetWODetailsNewDTC();
                        }
                        else
                        {
                            txtFailureId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ReferID"]));
                            GetFailureDetails();
                          
                        }

                       
                        
                    }
                  
                    string strQry = string.Empty;
                  
                    strQry = "Title=Search and Select DTC failure Details&";
                    strQry += "Query=SELECT DF_ID,DT_NAME,DT_CODE from TBLDTCMAST,TBLDTCFAILURE WHERE DF_DTC_CODE= DT_CODE AND DF_REPLACE_FLAG=0 AND  DF_ID ";
                    strQry += " NOT IN (SELECT WO_DF_ID FROM  TBLWORKORDER WHERE WO_DF_ID IS NOT NULL) ";
                    strQry += " AND DF_LOC_CODE LIKE '" + objSession.OfficeCode + "%' ";
                    strQry += " AND DF_STATUS_FLAG="+ txtType.Text  +" AND {0} like %{1}% order by DF_ID&";
                    strQry += "DBColName=DF_ID~DT_NAME~DT_CODE&";
                    strQry += "ColDisplayName="+ lblIDText.Text  +"~DTC_NAME~DTC_CODE&";

                    strQry = strQry.Replace("'", @"\'");

                    cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + hdfFailureId.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + hdfFailureId.ClientID + ")");


                    //WorkFlow / Approval
                    if (Request.QueryString["ActionType"] != null && Request.QueryString["ActionType"].ToString() != "")
                    {

                        SetControlText();
                        if (txtActiontype.Text == "V")
                        {
                            cmdSave.Enabled = false;
                            cmdReset.Enabled = false;
                            dvComments.Style.Add("display", "none");
                        }
                    }
                }
            }

            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "Page_Load");

            }

        }


        public void GetFailureDetails()
        {
            try
            {

                clsFailureEntry objFailure = new clsFailureEntry();
                objFailure.sFailureId = txtFailureId.Text;

                objFailure.GetFailureDetails(objFailure);


                txtFailureId.Text = objFailure.sFailureId;
                txtFailureDate.Text = objFailure.sFailureDate;
                hdfFailureId.Value = objFailure.sFailureId;
                txtDTCCode.Text = objFailure.sDtcCode;
                txtDTCName.Text  = objFailure.sDtcName;
                txtTCCode.Text = objFailure.sDtcTcCode;
                txtDeclaredBy.Text = objFailure.sCrby;
                txtDTCId.Text = objFailure.sDtcId;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, " GetWorkOrderDetails");
            }

        }

        public void GetWODetailsNewDTC()
        {
            try
            {
                clsWorkOrder objWorkOrder = new clsWorkOrder();
                objWorkOrder.sWOId = txtWOId.Text;
                objWorkOrder.GetWODetailsForNewDTC(objWorkOrder);

              
                cmdSave.Text = "Update";
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetWODetailsNewDTC");

            }
        }

        private void ShowMsgBox(string sMsg)
        {
            try
            {
                String sShowMsg = string.Empty;
                sShowMsg = "<script language=javascript> alert ('" + sMsg + "')</script>";
                this.Page.RegisterStartupScript("Msg", sShowMsg);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ShowMsgBox");
            }
        }

   

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                string[] Arr = new string[2];
                clsWorkOrder objWorkOrder = new clsWorkOrder();

                
                    if (txtActiontype.Text == "A" || txtActiontype.Text == "R")
                    {
                        ApproveRejectAction();
                        return;
                    }

            }

            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, " cmdSave_Click");
            }


        }

     
        public void ChangeLabelText()
        {
            try
            {
                if (txtType.Text == "1" || txtType.Text == "4")
                {
                    lblIDText.Text = "Failure ID";
                    lblDateText.Text = "Failure Date";
                }
                else if (txtType.Text == "2")
                {
                    lblIDText.Text = "Enhancement ID";
                    lblDateText.Text = "Enhancement Date";
                }
               
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ChangeLabelText");
            }
        }

        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "WorkOrder";
                objApproval.sRoleId = objSession.RoleId;
                objApproval.sAccessType = "1" + "," + sAccessType;
                bool bResult = objApproval.CheckAccessRights(objApproval);
                if (bResult == false)
                {
                    if (sAccessType == "4")
                    {
                        Response.Redirect("~/UserRestrict.aspx", false);
                    }
                    else
                    {
                        ShowMsgBox("Sorry , You are not authorized to Access");
                    }
                }
                return bResult;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "CheckAccessRights");
                return false;

            }
        }

        #endregion

        public void WorkFlowObjects(clsWorkOrder objWorkOrder)
        {
            try
            {
                string sClientIP = string.Empty;

                string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(ip))
                {
                    string[] ipRange = ip.Split(',');
                    int le = ipRange.Length - 1;
                    sClientIP = ipRange[0];
                }
                else
                {
                    sClientIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }


                objWorkOrder.sFormName = "WorkOrder";
                objWorkOrder.sOfficeCode = objSession.OfficeCode;
                objWorkOrder.sClientIP = sClientIP;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "WorkFlowObjects");
            }
        }

        #region Workflow/Approval

        public void SetControlText()
        {
            try
            {
                txtActiontype.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));

                if (txtActiontype.Text == "A")
                {
                    cmdSave.Text = "Approve";
                }
                if (txtActiontype.Text == "R")
                {
                    cmdSave.Text = "Reject";
                }
                if (txtActiontype.Text == "M")
                {
                    cmdSave.Text = "Modify and Approve";
                }

                dvComments.Style.Add("display", "block");
                cmdReset.Enabled = false;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "SetControlText");
            }
        }

        public void ApproveRejectAction()
        {
            try
            {
                clsApproval objApproval = new clsApproval();


                if (txtComment.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Comments/Remarks");
                    txtComment.Focus();
                    return;

                }

                //string sActionType = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));
                string sWFOId = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["WFOId"]));
                string sWFOAutoId = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["WFOAutoId"]));

                objApproval.sCrby = objSession.UserId;
                objApproval.sOfficeCode = objSession.OfficeCode;
                objApproval.sApproveComments = txtComment.Text.Trim();
                objApproval.sWFObjectId = sWFOId;
                objApproval.sWFAutoId = sWFOAutoId;

                if (txtActiontype.Text == "A")
                {
                    objApproval.sApproveStatus = "1";
                }
                if (txtActiontype.Text == "R")
                {
                    objApproval.sApproveStatus = "3";
                }

                string sClientIP = string.Empty;

                string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(ip))
                {
                    string[] ipRange = ip.Split(',');
                    int le = ipRange.Length - 1;
                    sClientIP = ipRange[0];
                }
                else
                {
                    sClientIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }

                objApproval.sClientIp = sClientIP;

                bool bResult = objApproval.ApproveWFRequest(objApproval);
                if (bResult == true)
                {

                    if (objApproval.sApproveStatus == "1")
                    {
                        ShowMsgBox("Approved Successfully");
                        cmdSave.Enabled = false;
                    }
                    else if (objApproval.sApproveStatus == "3")
                    {

                        ShowMsgBox("Rejected Successfully");
                        cmdSave.Enabled = false;
                    }
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdApprove_Click");
            }
        }


        #endregion

        protected void cmdClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["ActionType"] != null && Request.QueryString["ActionType"].ToString() != "")
                {
                    Response.Redirect("/Approval/ApprovalInbox.aspx", false);
                }
               
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdClose_Click");
            }
        }

        protected void lnkEstReport_Click(object sender, EventArgs e)
        {
            try
            {
                string strParam = string.Empty;
                strParam = "id=Estimation&FailureId=" + txtFailureId.Text;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "lnkEstReport_Click");
            }
        }

        protected void lnkViewFailure_Click(object sender, EventArgs e)
        {
            try
            {
                string sDTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtDTCId.Text));
                string sFailureId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtFailureId.Text));

                string url = "FailureEntry.aspx?DTCId=" + sDTCId + "&FailureId=" + sFailureId;
                string s = "window.open('" + url + "', '_blank');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "lnkViewFailure_Click");
            }
        }

  
    }

}