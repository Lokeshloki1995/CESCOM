using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;

namespace IIITS.DTLMS.DTCFailure
{
    public partial class RIApprove : System.Web.UI.Page
    {

        string strFormCode = "RIApprove";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                Form.DefaultButton = cmdApprove.UniqueID;
                objSession = (clsSession)Session["clsSession"];
                lblMessage.Text = string.Empty;
                if (!IsPostBack)
                {

                    txtAckDate.Attributes.Add("onblur", "return ValidateDate(" + txtAckDate.ClientID + ");");

                    if (Request.QueryString["DecommId"] != null && Request.QueryString["DecommId"].ToString() != "")
                    {
                        txtDecommId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["DecommId"]));
                        txtFailureId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["FailureId"]));
                        txtType.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["TypeValue"]));
                        GenerateAckNo();
                        txtAckDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                        GetTransformerDetails();
                        
                    }


                    //WorkFlow / Approval
                    WorkFlowConfig();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "Page_Load");
            }

        }
        protected void cmdApprove_Click(object sender, EventArgs e)
        {
            try
            {
                clsRIApproval objRIApproval = new clsRIApproval();
                if (ValidateForm() == true)
                {
                    string[] Arr = new string[2];

                    objRIApproval.sDecommId = txtDecommId.Text;
                    objRIApproval.sRemarks = txtCommentFromStoreKeeper.Text.Replace("'", "").Replace("\"", "").Replace(";", "").Replace(",", "ç");
                    objRIApproval.sFailureId = txtFailureId.Text;
                    objRIApproval.sTasktype = txtType.Text;
                    objRIApproval.sOilQuantitySK = txtOilQtySK.Text;
                    objRIApproval.sTCCode = txtDtrCode.Text;
                    objRIApproval.sCrby = objSession.UserId;
                    objRIApproval.sRVNo = txtAckNo.Text;
                    objRIApproval.sRVDate = txtAckDate.Text;
                    objRIApproval.sOilQuantity = txtOilQuantity.Text;
                    objRIApproval.sOfficeCode = objSession.OfficeCode;
                    objRIApproval.sDTCCode = hdfDTCCode.Value;
                    objRIApproval.sManualRVACKNo = txtManualAckNo.Text;
                    objRIApproval.sDecomWorkOrder = txtDeCommWO.Text;

                    if (cmdApprove.Text == "View")
                    {
                        if (hdfApproveStatus.Value != "")
                        {
                            if (hdfApproveStatus.Value == "1" || hdfApproveStatus.Value == "2")
                            {
                                GenerateRIAckReport();
                            }
                        }
                        else
                        {
                            GenerateRIAckReport();
                        }
                        return;
                    }

                    //Workflow
                    WorkFlowObjects(objRIApproval);

                    #region Modify and Approve

                    // For Modify and Approve
                    if (txtActiontype.Text == "M")
                    {
                        if (hdfRejectApproveRef.Value != "RA")
                        {
                            if (txtComment.Text.Trim() == "")
                            {
                                ShowMsgBox("Enter Comments/Remarks");
                                txtComment.Focus();
                                return;

                            }
                        }
                     
                        objRIApproval.sActionType = txtActiontype.Text;
                        objRIApproval.sCrby = hdfCrBy.Value;
                        Arr = objRIApproval.UpdateReplaceDetails(objRIApproval);
                        Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "(RI)-Failure");
                        if (Arr[1].ToString() == "1")
                        {
                            hdfWFDataId.Value = objRIApproval.sWFDataId;
                            ApproveRejectAction();

                            return;
                        }
                        if (Arr[1].ToString() == "2")
                        {
                            ShowMsgBox(Arr[0]);
                            return;
                        }
                    }

                    #endregion

                    if (objSession.RoleId == "5")
                    {
                        Arr = objRIApproval.UpdateReplaceDetails(objRIApproval);
                        cmdApprove.Enabled = false;

                        if (Arr[1].ToString() == "1")
                        {
                            ShowMsgBox(Arr[0]);
                            return;
                        }
                        if (Arr[1].ToString() == "2")
                        {
                            ShowMsgBox(Arr[0]);
                            return;
                        }
                        //txtWFDataId.Text = objRIApproval.sWFDataId;
                    }

                  
                    if (txtActiontype.Text == "A" || txtActiontype.Text == "R")
                    {
                        if (hdfWFOAutoId.Value  == "0")
                        {
                            ApproveRejectAction();
                            return;
                        }

                    }
                    Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "(RI)-Failure");
                   
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdApprove_Click");
            }
        }


        private void ShowMsgBox(string sMsg)
        {
            try
            {
                string sShowMsg = string.Empty;
                sShowMsg = "<script language=javascript> alert ('" + sMsg + "')</script>";
                this.Page.RegisterStartupScript("Msg", sShowMsg);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ShowMsgBox");
            }
        }




        bool ValidateForm()
        {
            bool bValidate = false;
            try
            {
                if (txtOilQtySK.Text.Trim() == "")
                {
                    txtOilQtySK.Focus();
                    ShowMsgBox("Please Enter Oil Quantity");
                    return bValidate;
                }
                if (txtCommentFromStoreKeeper.Text.Trim() == "")
                {
                    txtCommentFromStoreKeeper.Focus();
                    ShowMsgBox("Please Enter the Remarks/Comments");
                    return bValidate;
                }
                string sResult = Genaral.DateComparision(txtAckDate.Text, hdfDecommDate.Value, false, false);
                if (sResult == "2")
                {
                    txtAckDate.Focus();
                    ShowMsgBox("Ack Date should be Greater than Decommission Date");
                    return bValidate;
                }

                //if (txtFailureDate.Text  != "")
                //{
                //    sResult = Genaral.DateComparision(txtAckDate.Text, txtFailureDate.Text, false, false);
                //    if (sResult == "2")
                //    {
                //        txtAckDate.Focus();
                //        ShowMsgBox("Ack Date should be Greater than Failure Date");
                //        return bValidate;
                //    }
                //}

                bValidate = true;
                return bValidate;
            }
              

            catch(Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ValidateForm");
                return bValidate;
            }
                
            
        }

       
        protected void cmdApproveView_Click(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["ActionType"] != null && Request.QueryString["ActionType"].ToString() != "")
                {
                    Response.Redirect("/Approval/ApprovalInbox.aspx", false);
                }
                else
                {
                    Response.Redirect("RIApprovalView.aspx", false);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdApproveView_Click");
            }

        }

        public void GetTransformerDetails()
        {
            try
            {
                clsRIApproval objRIApproval = new clsRIApproval();
                objRIApproval.sFailureId = txtFailureId.Text;
                objRIApproval.sDecommId = txtDecommId.Text;
                objRIApproval.GetFailureTCDetails(objRIApproval);

                txtDeCommWO.Text = objRIApproval.sDecomWorkOrder;
                txtDtrCode.Text = objRIApproval.sTCCode;
                txtMake.Text = objRIApproval.sTcMake;
                txtDTrSlno.Text = objRIApproval.sTcSlno;
                txtDTrId.Text = objRIApproval.sTCId;
                txtFailureDate.Text = objRIApproval.sFailureDate;

                hdfDTCCode.Value = objRIApproval.sDTCCode;

                objRIApproval.sDecommId = txtDecommId.Text;

                objRIApproval.GetRIDetails(objRIApproval);

                txtOilQuantity.Text = objRIApproval.sOilQuantity;
                txtCommentFromStoreKeeper.Text = objRIApproval.sRemarks;
                hdfDecommDate.Value = objRIApproval.sDecommDate;

                if (objRIApproval.sRVNo != "")
                {
                    txtAckDate.Text = objRIApproval.sRVDate;
                    txtAckNo.Text = objRIApproval.sRVNo;
                    txtOilQtySK.Text = objRIApproval.sOilQuantitySK;
                    txtManualAckNo.Text = objRIApproval.sManualRVACKNo;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetTransformerDetails");
            }

        }

        public void WorkFlowObjects(clsRIApproval objRIApproval)
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


                objRIApproval.sFormName = "RIApprove";
                objRIApproval.sOfficeCode = objSession.OfficeCode;
                objRIApproval.sClientIP = sClientIP;
                objRIApproval.sWFObjectId = hdfWFOId.Value;
                objRIApproval.sWFAutoId = hdfWFOAutoId.Value;
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
                    cmdApprove.Text = "Approve";
                    pnlApproval.Enabled = false;
                }
                if (txtActiontype.Text == "R")
                {
                    cmdApprove.Text = "Reject";
                    pnlApproval.Enabled = false;
                }
                if (txtActiontype.Text == "M")
                {
                    cmdApprove.Text = "Modify and Approve";
                    pnlApproval.Enabled = true;
                }

                if (objSession.RoleId == "2")
                {
                    dvComments.Style.Add("display", "block");
                }
                //cmdReset.Enabled = false;
              
                if (hdfWFOAutoId.Value  != "0")
                {
                    cmdApprove.Text = "Save";
                    
                }

                // Check for Creator of Form
                bool bResult = CheckFormCreatorLevel();
                if (bResult == true)
                {
                    cmdApprove.Text = "Save";
                    pnlApproval.Enabled = true;


                    // To handle Record From Reject 
                    if (txtActiontype.Text == "A" && hdfWFOAutoId.Value == "0")
                    {
                        txtActiontype.Text = "M";
                        hdfRejectApproveRef.Value = "RA";
                    }
                }
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

                if (objSession.RoleId != "5")
                {
                    if (txtComment.Text.Trim() == "")
                    {
                        ShowMsgBox("Enter Comments/Remarks");
                        txtComment.Focus();
                        return;

                    }
                }

             
                objApproval.sCrby = objSession.UserId;
                objApproval.sOfficeCode = objSession.OfficeCode;
                objApproval.sApproveComments = txtComment.Text.Trim();
                objApproval.sWFObjectId = hdfWFOId.Value;
                objApproval.sWFAutoId = hdfWFOAutoId.Value;

                if (txtActiontype.Text == "A")
                {
                    objApproval.sApproveStatus = "1";
                }
                if (txtActiontype.Text == "R")
                {
                    objApproval.sApproveStatus = "3";
                }
                if (txtActiontype.Text == "M")
                {
                    objApproval.sApproveStatus = "2";
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
                objApproval.sWFDataId = hdfWFDataId.Value;
                objApproval.sNewRecordId = txtDecommId.Text;

                bool bResult = false;
                if (txtActiontype.Text == "M")
                {
                    objApproval.sWFDataId = hdfWFDataId.Value;
                    if (hdfRejectApproveRef.Value == "RA")
                    {
                        objApproval.sApproveStatus = "1";
                    }
                    bResult = objApproval.ModifyApproveWFRequest(objApproval);
                }
                else
                {
                    bResult = objApproval.ApproveWFRequest(objApproval);
                }
                if (bResult == true)
                {

                    if (objApproval.sApproveStatus == "1")
                    {
                        ShowMsgBox("Approved Successfully");

                        if (objSession.RoleId == "2")
                        {
                            clsRIApproval objRI = new clsRIApproval();
                            //objRI.SendSMStoSectionOfficer(txtDtrCode.Text, txtDecommId.Text,txtFailureId.Text);

                        }
                        GenerateRIAckReport();

                        cmdApprove.Enabled = false;
                    }
                    else if (objApproval.sApproveStatus == "3")
                    {

                        ShowMsgBox("Rejected Successfully");
                        cmdApprove.Enabled = false;
                    }
                    else if (objApproval.sApproveStatus == "2")
                    {
                        ShowMsgBox("Modified and Approved Successfully");

                        if (objSession.RoleId == "2")
                        {
                            clsRIApproval objRI = new clsRIApproval();
                            //objRI.SendSMStoSectionOfficer(txtDtrCode.Text, txtDecommId.Text, txtFailureId.Text);

                        }
                        GenerateRIAckReport();
                        cmdApprove.Enabled = false;
                    }
                }
                else
                {
                    ShowMsgBox("Selected Record Already Approved");
                    return;
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdApprove_Click");
            }
        }

        public void WorkFlowConfig()
        {
            try
            {
                if (Request.QueryString["ActionType"] != null && Request.QueryString["ActionType"].ToString() != "")
                {


                    if (Session["WFOId"] != null && Session["WFOId"].ToString() != "")
                    {
                        hdfWFDataId.Value = Session["WFDataId"].ToString();
                        hdfWFOId.Value = Convert.ToString(Session["WFOId"]);
                        hdfWFOAutoId.Value = Convert.ToString(Session["WFOAutoId"]);
                        hdfApproveStatus.Value = Convert.ToString(Session["ApproveStatus"]);

                        Session["WFDataId"] = null;
                        Session["WFOId"] = null;
                        Session["WFOAutoId"] = null;
                        Session["ApproveStatus"] = null;
                    }

                    if (hdfWFDataId.Value != "0")
                    {
                        GetRIDetailsFromXML(hdfWFDataId.Value);
                    }
                    SetControlText();
                    if (txtActiontype.Text == "V")
                    {
                        cmdApprove.Text = "View";
                        dvComments.Style.Add("display", "none");
                    }
                }
                else
                {
                    cmdApprove.Text = "View";
                }

                DisableControlForView();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "WorkFlowConfig");
            }
        }

        public bool CheckFormCreatorLevel()
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                string sResult = objApproval.GetFormCreatorLevel("", objSession.RoleId, "RIApprove");
                if (sResult == "1")
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetFormCreatorLevel");
                return false;
            }
        }

        public void DisableControlForView()
        {
            try
            {
                if (cmdApprove.Text.Contains("View"))
                {
                    pnlApproval.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "DisableControlForView");
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
                else
                {
                    Response.Redirect("RIApprovalView.aspx", false);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdClose_Click");
            }
        }

        protected void lnkDTrDetails_Click(object sender, EventArgs e)
        {
            try
            {
                string sTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtDTrId.Text));

                string url = "/MasterForms/TcMaster.aspx?TCId=" + sTCId;
                string s = "window.open('" + url + "', '_blank');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "lnkDTrDetails_Click");
            }
        }

        public void GenerateAckNo()
        {
            try
            {
                clsRIApproval objRI = new clsRIApproval();

                txtAckNo.Text = objRI.GenerateAckNo(objSession.OfficeCode);
                txtAckNo.ReadOnly = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GenerateAckNo");
            }
        }


        public void GenerateRIAckReport()
        {
            try
            {
                string strParam = string.Empty;
                strParam = "id=RIAckReport&DecommId=" + txtDecommId.Text;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GenerateRIAckReport");
            }
        }


        #region Load From XML
        public void GetRIDetailsFromXML(string sWFDataId)
        {
            try
            {

                clsRIApproval objRIApproval = new clsRIApproval();
                objRIApproval.sWFDataId = sWFDataId;

                objRIApproval.GetRIDetailsFromXML(objRIApproval);

                txtOilQuantity.Text = objRIApproval.sOilQuantity;
                txtCommentFromStoreKeeper.Text = objRIApproval.sRemarks;
                txtManualAckNo.Text = objRIApproval.sManualRVACKNo;

                txtAckDate.Text = objRIApproval.sRVDate;
                //txtAckNo.Text = objRIApproval.sRVNo;
                txtOilQtySK.Text = objRIApproval.sOilQuantitySK;
                hdfCrBy.Value = objRIApproval.sCrby;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetRIDetailsFromXML");

            }
        }
        #endregion

        protected void cmdViewDecomm_Click(object sender, EventArgs e)
        {
            try
            {
                clsFormValues objApproval = new clsFormValues();
                string sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtType.Text));              
                string sFailureId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtFailureId.Text));
                string sDecommId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtDecommId.Text));

                string url = "/DTCFailure/DeCommissioning.aspx?TypeValue=" + sTaskType + "&ReferID=" + sFailureId + "&ReplaceId=" + sDecommId;
                string s = "window.open('" + url + "', '_blank');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdViewDecomm_Click");
            }
        }
        
    }
}