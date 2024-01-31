using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;

namespace IIITS.DTLMS.DTCFailure
{
    public partial class CRReport : System.Web.UI.Page
    {
        string strFormCode = "CRReport";
        clsSession objSession;
        String tempRiDate = string.Empty;
        String tempCiDate = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                objSession = (clsSession)Session["clsSession"];
                lblMessage.Text = string.Empty;
                txtCrDate.Attributes.Add("readonly", "readonly");
                if (!IsPostBack)
                {
                    CalendarExtender2.EndDate = System.DateTime.Now;
                    if (Request.QueryString["DecommId"] != null && Request.QueryString["DecommId"].ToString() != "")
                    {
                        txtDecommId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["DecommId"]));
                    }
                    if (Request.QueryString["TypeValue"] != null && Request.QueryString["TypeValue"].ToString() != "")
                    {
                        txtType.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["TypeValue"]));
                    }
                    GetDetailsForCR();

                    CalendarExtender2.StartDate = DateTime.ParseExact(txtRVdate.Text, "dd/MM/yyyy", null);
                    
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

        public void GetDetailsForCR()
        {
            try
            {
                clsCRReport objRIApproval = new clsCRReport();
                objRIApproval.sDecommId = txtDecommId.Text;

                objRIApproval.GetDetailsForCR(objRIApproval);

                txtWrkOrderDate.Text = objRIApproval.sWorkOrderDate;
                txtcomWO.Text = objRIApproval.sComWorkOrder;
                txtDEcomWO.Text = objRIApproval.sDecomWorkOrder;
                
                txtStoreKeepName.Text = objRIApproval.sStoreKeeperName;
                txtStoreOffName.Text = objRIApproval.sStoreOfficerName;
                txtRemarksStoreKeeper.Text = objRIApproval.sCommentByStoreKeeper.Replace("ç", ",");
                txtRemStoreOfficer.Text = objRIApproval.sCommentByStoreOfficer;
                txtOilCapacity.Text = objRIApproval.sOilQuantity;
                txtAcceptDate.Text = objRIApproval.sApprovedDate;
                txtRINo.Text = objRIApproval.sRINo;
                txtRIDate.Text = objRIApproval.sRIDate;
                txtRVNumber.Text = objRIApproval.sRVNo;
                txtRVdate.Text = objRIApproval.sRVDate;
                txtInvNumber.Text = objRIApproval.sManInvNumber;
                txtInvDate.Text = objRIApproval.sInvDate;


                txtDTCCode.Text = objRIApproval.sDTCCode;
                txtDTCId.Text = objRIApproval.sDTCId;
                txtFailureDTr.Text = objRIApproval.sTCCode;
                txtFailDTrId.Text = objRIApproval.sFailureTCId;
                txtNewDTr.Text = objRIApproval.sNewTCCode;
                txtNewDTrId.Text = objRIApproval.sNewTCId;
                hdfFailureId.Value = objRIApproval.sFailureId;

                txtInvQty.Text = objRIApproval.sInventoryQty;
                txtDecommInventry.Text = objRIApproval.sDecommInventoryQty;



                if (txtType.Text == "2")
                {
                    lblFailDTr.Text = "Enhance DTr Code";
                }
                if (txtType.Text == "5")
                {
                    lblFailDTr.Text = "Reduced DTr Code";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetDetailsForCR");
            }
        }

        protected bool ValidateForm()
        {
            bool btnvalidate = false;
            try
            {
                   
                if (txtCrDate.Text.Length == 0)
                {
                    ShowMsgBox("Please Enter CR Date");
                    return btnvalidate;
                }
                else
                {
                    string temp = Genaral.DateValidation(txtCrDate.Text);
                    if (!(temp.Length == 0 ))
                    {
                        ShowMsgBox(temp);
                        return btnvalidate;
                    }
                    else
                    {
                        DateTime myRVDate = DateTime.ParseExact(txtRVdate.Text, "dd/MM/yyyy", null);
                         tempRiDate = myRVDate.ToString("d/M/yyyy");

                         DateTime myCrDate = DateTime.ParseExact(txtCrDate.Text, "dd/MM/yyyy",null);
                         tempCiDate = myCrDate.ToString("d/M/yyyy");

                        temp = Genaral.DateComparision(tempCiDate,tempRiDate, false, false);
                        if (temp == "2")
                        {
                            ShowMsgBox("CR date should be greatee than RV Date");
                            return btnvalidate;
                        }
                    }
                }
                btnvalidate = true;
                return btnvalidate;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message + txtDTCCode, strFormCode, "ValidateForm");
                return btnvalidate;
            }
        }

        protected void cmdCR_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateForm())
                {
                    
                    clsApproval objAproval = new clsApproval();

                    objAproval.sPrevWFOId = txtWFOId.Text;
                    objAproval.sWFObjectId = txtWFOId.Text;

                    //objAproval.UpdateWFOAutoObject(objAproval);

                    if (cmdCR.Text == "View")
                    {
                        if (hdfApproveStatus.Value != "")
                        {
                            if (hdfApproveStatus.Value == "1" || hdfApproveStatus.Value == "2"  )
                            {
                                GenerateCRReport();
                            }
                        }
                        else
                        {
                            GenerateCRReport();
                        }
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "CRReport");
                        }
                        return;
                    }

                    clsCRReport objCR = new clsCRReport();
                    string[] Arr = new string[2];



                    objCR.sDTCCode = txtDTCCode.Text;
                    objCR.sCrby = objSession.UserId;

                    objCR.sInventoryQty = txtInvQty.Text;
                    objCR.sDecommId = txtDecommId.Text;
                    objCR.sDecommInventoryQty = txtDecommInventry.Text;
                    objCR.sFailureId = hdfFailureId.Value;

                    objCR.sCrDate = txtCrDate.Text;


                    //Workflow
                    WorkFlowObjects(objCR);

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

                        objCR.sActionType = txtActiontype.Text;

                        Arr = objCR.SaveCompletionReport(objCR);
                        if (Arr[1].ToString() == "0")
                        {
                            hdfWFDataId.Value = objCR.sWFDataId;
                            ApproveRejectAction();
                            if (objSession.sTransactionLog == "1")
                            {
                                Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "CRReport-Failure");
                            }
                            return;
                        }
                        if (Arr[1].ToString() == "2")
                        {
                            ShowMsgBox(Arr[0]);
                            if (objSession.sTransactionLog == "1")
                            {
                                Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "CRReport-Failure");
                            } return;
                        }
                    }

                    #endregion

                    if (txtActiontype.Text == "RA")
                    {
                        ApproveRejectAction();
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "CRReport-Failure");
                        }
                        return;
                    }

                    if (objSession.RoleId == "4")
                    {
                        if (txtComment.Text.Trim() == "")
                        {
                            ShowMsgBox("Enter Comments/Remarks");
                            txtComment.Focus();
                            return;
                        }

                        Arr = objCR.SaveCompletionReport(objCR);
                        cmdCR.Enabled = false;
                        

                        if (Arr[1].ToString() == "0")
                        {
                            ShowMsgBox(Arr[0]);
                            if (objSession.sTransactionLog == "1")
                            {
                                Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "CRReport-Failure");
                            }
                            return;
                        }
                        if (Arr[1].ToString() == "2")
                        {
                            ShowMsgBox(Arr[0]);
                            if (objSession.sTransactionLog == "1")
                            {
                                Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "CRReport-Failure");
                            }
                            return;
                        }
                        //txtWFDataId.Text = objRIApproval.sWFDataId;
                    }

                    if (txtActiontype.Text == "A" || txtActiontype.Text == "R")
                    {
                        ApproveRejectAction();
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "CRReport-Failure");
                        }
                        return;
                    }
                }

               
            }
            catch (Exception ex)
            {
                //lblMessage.Text = clsException.ErrorMsg();
                ShowMsgBox("Something Went Wrong please approve once again");
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdCR_Click");
            }
        }


        public void GenerateCRReport()
        {
            try
            {
                //To show Report Only for Failure Entry
                if (txtType.Text == "1" || txtType.Text == "4")
                {
                    string strParam = "id=CRReport&DecommId=" + txtDecommId.Text+"~"+txtCrDate.Text+" ";
                    RegisterStartupScript("PrintD", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                }
                else if (txtType.Text == "2")
                {
                    string strParam = "id=EnhanceCRReport&DecommId=" + txtDecommId.Text + "~" + txtCrDate.Text + " ";
                    RegisterStartupScript("PrintD", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetDetailsForCR");
            }
        }

        public bool CheckFailureEntry()
        {
            try
            {
                clsFormValues objForm = new clsFormValues();
                objForm.sDecommisionId = txtDecommId.Text;
                string sResult = objForm.GetStatusFlagForDecommission(objForm);
                if (sResult == "1")
                {
                    return true;
                }
                return false;
              
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "CheckFailureEntry");
                return false;
            }
        }

        protected void lnkDTCDetails_Click(object sender, EventArgs e)
        {
            try
            {
                string sDTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtDTCId.Text));

                string url = "/MasterForms/DTCCommision.aspx?QryDtcId=" + sDTCId;
                string s = "window.open('" + url + "', '_blank');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "lnkDTCDetails_Click");
            }
        }

        protected void lnkFailDTrDetails_Click(object sender, EventArgs e)
        {
            try
            {
                string sTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtFailDTrId.Text));

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

        protected void lnkNewDTr_Click(object sender, EventArgs e)
        {
            try
            {
                string sTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtNewDTrId.Text));

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

        #region Workflow/Approval
        public void SetControlText()
        {
            try
            {
                txtActiontype.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));

                if (txtActiontype.Text == "A")
                {
                    cmdCR.Text = "Approve";
                    pnlApproval.Enabled = false;
                }
                if (txtActiontype.Text == "R")
                {
                    cmdCR.Text = "Reject";
                    pnlApproval.Enabled = false;
                }
                if (txtActiontype.Text == "M")
                {
                    cmdCR.Text = "Modify and Approve";
                    pnlApproval.Enabled = true;
                }

               
                 dvComments.Style.Add("display", "block");
                             
                if (txtWFOAuto.Text  != "0")
                {
                    //cmdApprove.Text = "Save";

                }

                // Check for Creator of Form
                bool bResult = CheckFormCreatorLevel();
                if (bResult == true)
                {
                    cmdCR.Text = "Save";
                    pnlApproval.Enabled = true;

                    // To handle Record From Reject 
                    if (txtActiontype.Text == "A" && txtWFOAuto.Text == "0")
                    {
                        txtActiontype.Text = "M";
                        hdfRejectApproveRef.Value = "RA";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SetControlText");
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
                objApproval.sCrby = objSession.UserId;
                objApproval.sOfficeCode = objSession.OfficeCode;
                objApproval.sApproveComments = txtComment.Text.Trim();
                objApproval.sWFObjectId = txtWFOId.Text;
                objApproval.sWFAutoId = txtWFOAuto.Text;

                if (txtActiontype.Text == "A")
                {
                    objApproval.sApproveStatus = "1";
                }
                if (txtActiontype.Text == "R")
                {
                    objApproval.sApproveStatus = "3";
                }
                if (txtActiontype.Text == "RA")
                {
                    objApproval.sApproveStatus = "1";
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
                objApproval.sDescription = "Completion Report For DTC Code " + txtDTCCode.Text;
                bool bResult = objApproval.ApproveWFRequest1(objApproval);
                if (bResult == true)
                {

                    if (objApproval.sApproveStatus == "1")
                    {
                        ShowMsgBox("Approved Successfully");

                        if (objSession.RoleId == "1" || objSession.RoleId == "3" || objSession.RoleId == "4" || objSession.RoleId == "6")
                        {
                            GenerateCRReport();
                        }
                        cmdCR.Enabled = false;
                        if (objSession.RoleId == "3")
                        {
                            clsCRReport objCrReport = new clsCRReport();
                            objCrReport.sDTCCode = txtDTCCode.Text;
                            objCrReport.sCrDate = tempCiDate;
                            objCrReport.sRemarks = txtComment.Text;
                            objCrReport.sComWorkOrder = txtcomWO.Text;
                            string[] updateStatus = objCrReport.UpdateCRStatusMMS(objCrReport);
                        }
                    }
                    else if (objApproval.sApproveStatus == "3")
                    {
                        ShowMsgBox("Rejected Successfully");
                        cmdCR.Enabled = false;
                    }
                    else if (objApproval.sApproveStatus == "2")
                    {
                        ShowMsgBox("Modified and Approved Successfully");

                        if (objSession.RoleId == "3")
                        {

                            clsCRReport objCrReport = new clsCRReport();
                            objCrReport.sDTCCode = txtDTCCode.Text;
                            objCrReport.sCrDate = tempCiDate;
                            objCrReport.sRemarks = txtComment.Text;
                            objCrReport.sComWorkOrder = txtcomWO.Text;
                            string[] updateStatus = objCrReport.UpdateCRStatusMMS(objCrReport);

                            GenerateCRReport();
                        }
                        cmdCR.Enabled = false;
                    }
                }
                if (objSession.sTransactionLog == "1")
                {
                    Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "CRReport-Failure");
                }
            }
            catch (Exception ex)
            {
                //lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdApprove_Click");
                throw ex;
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
                        txtWFOId.Text = Convert.ToString(Session["WFOId"]);
                        txtWFOAuto.Text = Convert.ToString(Session["WFOAutoId"]);
                        hdfApproveStatus.Value = Convert.ToString(Session["ApproveStatus"]);

                        //Session["WFDataId"] = null;
                        Session["WFOId"] = null;
                        Session["WFOAutoId"] = null;
                        Session["ApproveStatus"] = null;
                    }


                    if (hdfWFDataId.Value != "0")
                    {
                        GetCRDetailsFromXML(hdfWFDataId.Value);
                    }
                    SetControlText();
                    if (txtActiontype.Text == "V")
                    {
                        //cmdCR.Enabled = false;
                        cmdCR.Text = "View";
                        dvComments.Style.Add("display", "none");
                        txtInvQty.ReadOnly = true;
                        txtDecommInventry.ReadOnly = true;
                        if (!(txtCrDate.Text.Length == 0))
                        {
                            txtCrDate.ReadOnly = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "WorkFlowConfig");
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


                objRIApproval.sFormName = "CRReport";
                objRIApproval.sOfficeCode = objSession.OfficeCode;
                objRIApproval.sClientIP = sClientIP;
                objRIApproval.sWFObjectId = txtWFOId.Text;
                objRIApproval.sWFAutoId = txtWFOAuto.Text;
                objRIApproval.sCrDate = txtCrDate.Text;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "WorkFlowObjects");
            }
        }

        public bool CheckFormCreatorLevel()
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                string sResult = objApproval.GetFormCreatorLevel("", objSession.RoleId, "CRReport");
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
        #endregion

        #region Load From XML
        public void GetCRDetailsFromXML(string sWFDataId)
        {
            try
            {

                clsCRReport objRIApproval = new clsCRReport();
                objRIApproval.sWFDataId = sWFDataId;

                objRIApproval.GetCRDetailsFromXML(objRIApproval);

                txtInvQty.Text = objRIApproval.sInventoryQty;
                txtDecommInventry.Text = objRIApproval.sDecommInventoryQty;
                txtCrDate.Text = objRIApproval.sCrDate;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetRIDetailsFromXML");

            }
        }
        #endregion

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
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowMsgBox");
            }
        }
        protected void cmdClose_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("/Approval/ApprovalInbox.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdClose_Click");
            }
        }

        protected void cmdViewRI_Click(object sender, EventArgs e)
        {
            try
            {
                clsFormValues objApproval = new clsFormValues();
                string sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtType.Text));    
                string sFailureId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfFailureId.Value));
                string sDecommId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtDecommId.Text));

                string url = "/DTCFailure/RIApprove.aspx?TypeValue=" + sTaskType + "&DecommId=" + sDecommId + "&FailureId=" + sFailureId;
                string s = "window.open('" + url + "', '_blank');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdViewRI_Click");
            }
        }

       
    }
}