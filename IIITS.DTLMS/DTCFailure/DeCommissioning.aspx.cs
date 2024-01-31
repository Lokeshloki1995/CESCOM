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
    public partial class DeCommissioning : System.Web.UI.Page
    {

        string strFormCode = "DeCommissioning";
        clsSession objSession;
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
                txtDecommDate.Attributes.Add("readonly", "readonly");
                txtRIDate.Attributes.Add("readonly", "readonly");
                TxtCommDate.Attributes.Add("readonly", "readonly");

                if (!IsPostBack)
                {
                    Genaral.Load_Combo("SELECT SM_ID,SM_NAME FROM TBLSTOREMAST WHERE SM_STATUS='A' ORDER BY SM_NAME", "-Select-", cmbStore);

                    if (Request.QueryString["TypeValue"] != null && Request.QueryString["TypeValue"].ToString() != "")
                    {
                        txtType.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["TypeValue"]));
                        ChangeLabelText();
                        GenerateRINo();
                    }

                    if (Request.QueryString["ReferID"] != null && Request.QueryString["ReferID"].ToString() != "")
                    {
                        txtFailureId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ReferID"]));

                        if (Request.QueryString["ReplaceId"] != null && Request.QueryString["ReplaceId"].ToString() != "")
                        {
                            txtReplaceId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ReplaceId"]));
                        }

                        hdfFailureId.Value = txtFailureId.Text;

                        GetBasicDetails();

                        if (txtReplaceId.Text != "0" && txtReplaceId.Text != "")
                        {
                            if (!txtReplaceId.Text.Contains("-"))
                            {
                                GetReplaceDetails();
                            }

                        }
                        else
                        {
                            txtReplaceId.Text = "";
                        }
                    }

                    //Search Window Call
                    LoadSearchWindow();

                    //WorkFlow / Approval
                    WorkFlowConfig();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load");
            }


        }

        public void GetBasicDetails()
        {
            try
            {
                clsFailureEntry objFailure = new clsFailureEntry();
                txtFailureId.Text = hdfFailureId.Value;
                objFailure.sFailureId = txtFailureId.Text;

                objFailure.GetFailureDetails(objFailure);

                txtFailureDate.Text = objFailure.sFailureDate;
                txtDTCName.Text = objFailure.sDtcName;
                txtDTCCode.Text = objFailure.sDtcCode;
                txtMake.Text = objFailure.sDtcTcMake;
                txtTCCode.Text = objFailure.sDtcTcCode;
                txtCapcity.Text = objFailure.sDtcCapacity;
                TxtCommDate.Text = objFailure.sDTrCommissionDate;

                hdfDTCId.Value = objFailure.sDtcId;
                hdfTCId.Value = objFailure.sTCId;
                txtTankcapacity.Text = objFailure.sTankcapacity;
                txtQuantityOfOil.Text = objFailure.sOilCapacity;
                //To get Invoice ID
                GetInvoiceNo();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetBasicDetails");
            }
        }

        public void GetReplaceDetails()
        {
            try
            {
                clsDeCommissioning objReplace = new clsDeCommissioning();
                objReplace.sDecommId = txtReplaceId.Text;

                objReplace.GetDecommDetails(objReplace);

                txtRINo.Text = objReplace.sRINo;
                txtRIDate.Text = objReplace.sRIDate;
                txtRemarks.Text = objReplace.sRemarks;
                cmbStore.SelectedValue = objReplace.sStoreId;
                txtOilQuantity.Text = objReplace.sOilQuantity;
                txtTrReading.Text = objReplace.sTRReading;
                txtDecommDate.Text = objReplace.sDecommDate;
                txtManualRINo.Text = objReplace.sManualRINo;
                cmdReset.Enabled = false;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetReplaceDetails");
            }

        }




        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {

                //Check AccessRights
                bool bAccResult = true;
                if (cmdSave.Text == "Update")
                {
                    bAccResult = CheckAccessRights("3");
                }
                else if (cmdSave.Text == "Save")
                {
                    bAccResult = CheckAccessRights("2");
                }

                if (bAccResult == false)
                {
                    return;
                }
                if (cmdSave.Text == "View")
                {
                    if (hdfApproveStatus.Value != "")
                    {
                        if (hdfApproveStatus.Value == "1" || hdfApproveStatus.Value == "2")
                        {
                            GenerateDecommReport();
                        }
                    }
                    else
                    {
                        GenerateDecommReport();
                    }
                    return;
                }

                if (ValidateForm() == true)
                {
                    clsDeCommissioning objReplace = new clsDeCommissioning();
                    string[] Arr = new string[2];

                    objReplace.sInvoiceId = txtInvoiceId.Text;
                    objReplace.sFailureId = txtFailureId.Text;
                    objReplace.sDecommId = txtReplaceId.Text;
                    objReplace.sTRReading = txtTrReading.Text.Replace("'", "");
                    objReplace.sRINo = txtRINo.Text.Replace("'", "");
                    objReplace.sRIDate = txtRIDate.Text.Replace("'", "");
                    objReplace.sRemarks = txtRemarks.Text.Replace("'", "").Replace("\"", "").Replace(";", "").Replace(",", "ç");
                    objReplace.sStoreId = cmbStore.SelectedValue;
                    objReplace.sCrby = objSession.UserId;
                    objReplace.sTaskType = txtType.Text;
                    objReplace.sOfficeCode = objSession.OfficeCode;
                    objReplace.sDTCCode = txtDTCCode.Text;
                    objReplace.sOilQuantity = txtOilQuantity.Text;
                    objReplace.sDecommDate = txtDecommDate.Text;
                    objReplace.sManualRINo = txtManualRINo.Text;
                    objReplace.sCommDate = TxtCommDate.Text;

                    if (txtActiontype.Text == "A" || txtActiontype.Text == "R")
                    {
                        if (hdfWFDataId.Value != "0")
                        {
                            ApproveRejectAction();
                            if (objSession.sTransactionLog == "1")
                            {
                                Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "Decommissiong-Failure");
                            }
                            return;
                        }
                    }

                    //Workflow
                    WorkFlowObjects(objReplace);

                    #region Modify and Approve

                    // For Modify and Approve
                    if (txtActiontype.Text == "M")
                    {
                        if (txtComment.Text.Trim() == "")
                        {
                            ShowMsgBox("Enter Comments/Remarks");
                            txtComment.Focus();
                            return;

                        }
                        objReplace.sDecommId = "";
                        objReplace.sActionType = txtActiontype.Text;
                        objReplace.sCrby = hdfCrBy.Value;
                        objReplace.sOfficeCode = hdfOfficeCode.Value;

                        Arr = objReplace.SaveReplaceDetails(objReplace);
                        Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "(Decommissioning)-Failure");
                        if (Arr[1].ToString() == "0")
                        {
                            hdfWFDataId.Value = objReplace.sWFDataId;
                            ApproveRejectAction();
                            if (objSession.sTransactionLog == "1")
                            {
                                Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "Decommissiong-Failure");
                            }
                            return;
                        }
                        if (Arr[1].ToString() == "2")
                        {
                            ShowMsgBox(Arr[0]);
                            if (objSession.sTransactionLog == "1")
                            {
                                Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "Decommissiong-Failure");
                            }
                            return;
                        }
                    }

                    #endregion

                    Arr = objReplace.SaveReplaceDetails(objReplace);
                    Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "(Decommissioning)-Failure");
                    if (Arr[1].ToString() == "0")
                    {
                        cmdSave.Text = "Update";
                        ShowMsgBox("Decommissioning Done Successfully");
                        cmdSave.Enabled = false;
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "Decommissiong-Failure");
                        }
                        return;
                    }
                    if (Arr[1].ToString() == "1")
                    {
                        ShowMsgBox(Arr[0].ToString());
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "Decommissiong-Failure");
                        }
                        return;
                    }
                    if (Arr[1].ToString() == "2")
                    {
                        ShowMsgBox(Arr[0]);
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "Decommissiong-Failure");
                        }
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                //lblMessage.Text = clsException.ErrorMsg();
                ShowMsgBox("Something Went Wrong Please Approve Once Agian");
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdSave_Click");
            }


        }

        public bool ValidateForm()
        {
            bool bValidate = false;
            try
            {
                if (txtRINo.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Valid RI Number");
                    return bValidate;
                }

                if (!(TxtCommDate.Text.Trim() == ""))
                {
                    string result = Genaral.DateValidation(TxtCommDate.Text);
                    if (!(result == ""))
                    {
                        ShowMsgBox("Please Enter the date in the format dd/MM/yyyy example 31/12/2018");
                        return bValidate;
                    }
                }
                if (txtRIDate.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Valid RI Date");
                    return bValidate;
                }
                if (TxtCommDate.Text.Trim() == "")
                {
                    ShowMsgBox("Please update the DTR Commission Date");
                    return bValidate;
                }
                string sResult = Genaral.DateComparision(txtRIDate.Text, txtFailureDate.Text, false, false);
                if (sResult == "2")
                {
                    ShowMsgBox("RI Date should be Greater than Failure Date");
                    return bValidate;
                }
                sResult = Genaral.DateComparision(txtDecommDate.Text, txtFailureDate.Text, false, false);
                if (sResult == "2")
                {
                    ShowMsgBox("Decommissioning Date should be Greater than Failure Date");
                    return bValidate;
                }
                if (cmbStore.SelectedIndex == 0)
                {
                    ShowMsgBox("Select Store");
                    return bValidate;
                }
                if (txtRemarks.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Valid Remarks");
                    return bValidate;
                }
                if (txtOilQuantity.Text.Length == 0)
                {
                    txtOilQuantity.Focus();
                    ShowMsgBox("Enter Valid Oil Returned To The Store(in Liter)");
                    return false;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(txtOilQuantity.Text, "^(\\d{1,4})?(\\.\\d{1,2})?$"))
                {
                    txtOilQuantity.Focus();
                    ShowMsgBox("Please Enter Valid Oil Returned To The Store(in Liter) (eg:1111.00)");
                    return bValidate;
                }

                if (txtQuantityOfOil.Text.Length != 0)
                {
                    if (!(Convert.ToDouble(txtOilQuantity.Text) <= Convert.ToDouble(txtQuantityOfOil.Text)))
                    {
                        txtOilQuantity.Focus();
                        ShowMsgBox("Oil Returned To The Store Should be less than or equals to Total Oil Quantity");
                        return false;
                    }

                }
                bValidate = true;
                return bValidate;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ValidateForm");
                return bValidate;
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
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowMsgBox");
            }
        }

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                //txtRINo.Text = string.Empty;
                txtRIDate.Text = string.Empty;
                txtTrReading.Text = string.Empty;
                txtRemarks.Text = string.Empty;
                //cmbStore.SelectedIndex = 0;

                hdfFailureId.Value = string.Empty;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdReset_Click");
            }

        }
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
                    Response.Redirect("DeCommissioningView.aspx", false);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdClose_Click");
            }
        }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                GetBasicDetails();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdClose_Click");
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
                else if (txtType.Text == "5")
                {
                    lblIDText.Text = "Reduction ID";
                    lblDateText.Text = "Reduction Date";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ChangeLabelText");
            }
        }

        public void GetInvoiceNo()
        {
            try
            {
                clsDeCommissioning objDecomm = new clsDeCommissioning();
                txtInvoiceId.Text = objDecomm.GetInvoiceNo(txtFailureId.Text);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetInvoiceNo");
            }
        }

        public void GetStoreId()
        {
            try
            {
                clsTcMaster objTcMaster = new clsTcMaster();
                cmbStore.SelectedValue = objTcMaster.GetStoreId(objSession.OfficeCode);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetStoreId");
            }
        }

        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "DeCommissioning";
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
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "CheckAccessRights");
                return false;

            }
        }

        #endregion


        public void WorkFlowObjects(clsDeCommissioning objDecomm)
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


                objDecomm.sFormName = "DeCommissioning";
                objDecomm.sOfficeCode = objSession.OfficeCode;
                objDecomm.sClientIP = sClientIP;
                objDecomm.sWFOId = hdfWFOId.Value;
                objDecomm.sWFAutoId = hdfWFOAutoId.Value;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "WorkFlowObjects");
            }
        }

        public void GenerateRINo()
        {
            try
            {
                clsDeCommissioning objDecomm = new clsDeCommissioning();

                txtRINo.Text = objDecomm.GenerateRINo(objSession.OfficeCode);
                txtRINo.ReadOnly = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GenerateIndentNo");
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
                    pnlApproval.Enabled = false;
                }
                if (txtActiontype.Text == "R")
                {
                    cmdSave.Text = "Reject";
                    pnlApproval.Enabled = false;
                }
                if (txtActiontype.Text == "M")
                {
                    cmdSave.Text = "Modify and Approve";
                    pnlApproval.Enabled = true;
                }

                dvComments.Style.Add("display", "block");
                //cmdReset.Enabled = false;

                if (hdfWFOAutoId.Value != "0")
                {
                    cmdSave.Text = "Save";
                    dvComments.Style.Add("display", "none");
                }

                // Check for Creator of Form
                bool bResult = CheckFormCreatorLevel();
                if (bResult == true)
                {
                    cmdSave.Text = "Save";
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

                bool bResult = false;
                if (txtActiontype.Text == "M")
                {
                    objApproval.sWFDataId = hdfWFDataId.Value;
                    if (hdfRejectApproveRef.Value == "RA")
                    {
                        objApproval.sApproveStatus = "1";
                    }
                    bResult = objApproval.ModifyApproveWFRequest1(objApproval);
                }
                else
                {
                    bResult = objApproval.ApproveWFRequest1(objApproval);
                }
                if (bResult == true)
                {
                    clsDeCommissioning objDecomm = new clsDeCommissioning();


                    if (objApproval.sNewRecordId != "" && objApproval.sNewRecordId != null)
                    {
                        txtReplaceId.Text = objApproval.sNewRecordId;
                    }
                    if (objApproval.sApproveStatus == "1")
                    {
                        ShowMsgBox("Approved Successfully");
                        cmdSave.Enabled = false;

                        if (txtType.Text != "3")
                        {
                            if (objSession.RoleId == "1")
                            {
                                GenerateDecommReport();
                            }
                        }
                    }
                    else if (objApproval.sApproveStatus == "3")
                    {

                        ShowMsgBox("Rejected Successfully");
                        cmdSave.Enabled = false;
                    }
                    else if (objApproval.sApproveStatus == "2")
                    {
                        ShowMsgBox("Modified and Approved Successfully");
                        cmdSave.Enabled = false;

                        if (txtType.Text != "3")
                        {
                            if (objSession.RoleId == "1")
                            {
                                GenerateDecommReport();
                            }
                        }
                    }
                    if (objSession.RoleId == "1")
                    {
                        objDecomm.updateRecord(txtDTCCode.Text);
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
                        GetDecommDetailsFromXML(hdfWFDataId.Value);
                    }

                    SetControlText();
                    if (txtActiontype.Text == "V")
                    {
                        cmdSave.Text = "View";
                        dvComments.Style.Add("display", "none");
                    }
                }
                else
                {
                    cmdSave.Text = "View";
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
                string sResult = objApproval.GetFormCreatorLevel("", objSession.RoleId, "DeCommissioning");
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
                if (cmdSave.Text.Contains("View"))
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

        public void GenerateDecommReport()
        {
            try
            {
                if (txtReplaceId.Text.Contains("-"))
                {
                    return;
                }

                string strParam = string.Empty;
                strParam = "id=RIReport&DecommId=" + txtReplaceId.Text;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GenerateDecommReport");
            }
        }


        #region Load From XML
        public void GetDecommDetailsFromXML(string sWFDataId)
        {
            try
            {
                // If the Data saved in Main Table then this function shd not execute, so done restriction like below
                // And commented for temprary purpose.. nee to change in future

                //if (!txtReplaceId.Text.Contains("-"))
                //{
                //    return;
                //}

                clsDeCommissioning objReplace = new clsDeCommissioning();
                objReplace.sWFDataId = sWFDataId;

                objReplace.GetDecommDetailsFromXML(objReplace);

                //txtRINo.Text = objReplace.sRINo;
                txtRIDate.Text = objReplace.sRIDate;
                txtRemarks.Text = objReplace.sRemarks;

                cmbStore.SelectedValue = objReplace.sStoreId;
                txtOilQuantity.Text = objReplace.sOilQuantity;
                txtTrReading.Text = objReplace.sTRReading;
                txtDecommDate.Text = objReplace.sDecommDate;

                hdfOfficeCode.Value = objReplace.sOfficeCode;
                hdfCrBy.Value = objReplace.sCrby;
                txtManualRINo.Text = objReplace.sManualRINo;
                TxtCommDate.Text = objReplace.sCommDate.Trim();
                //cmdSave.Text = "Update";

                cmdReset.Enabled = false;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                //clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetDecommDetailsFromXML");

            }
        }
        #endregion

        public void LoadSearchWindow()
        {
            try
            {
                string sTypeName = string.Empty;
                if (txtType.Text == "1" || txtType.Text == "4")
                {
                    sTypeName = "FAILURE";
                }
                else if (txtType.Text == "2")
                {
                    sTypeName = "ENHANCEMENT";
                }

                string strQry = string.Empty;

                strQry = "Title=Search and Select DTC Failure Details&";
                strQry += "Query=SELECT DF_ID,DT_NAME,DT_CODE from TBLDTCMAST,TBLDTCFAILURE,TBLWORKORDER,TBLINDENT,TBLDTCINVOICE WHERE DF_DTC_CODE= DT_CODE AND DF_REPLACE_FLAG=0 AND ";
                strQry += " DF_ID=WO_DF_ID AND WO_SLNO=TI_WO_SLNO AND TI_ID=IN_TI_NO AND  IN_NO NOT IN (SELECT TR_IN_NO FROM  TBLTCREPLACE)";
                strQry += " AND DF_STATUS_FLAG=" + txtType.Text + " ";
                strQry += " AND DF_LOC_CODE LIKE '" + objSession.OfficeCode + "%' AND {0} like %{1}% order by DF_ID&";
                strQry += "DBColName=DF_ID~DT_NAME~DT_CODE&";
                strQry += "ColDisplayName=" + sTypeName + " ID~DTC_NAME~DTC_CODE&";

                strQry = strQry.Replace("'", @"\'");

                cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + hdfFailureId.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + hdfFailureId.ClientID + ")");


                txtRIDate.Attributes.Add("onblur", "return ValidateDate(" + txtRIDate.ClientID + ");");


                GetStoreId();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadSearchWindow");

            }
        }

        protected void lnkDTrDetails_Click(object sender, EventArgs e)
        {
            try
            {
                string sTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfTCId.Value));

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

        protected void lnkDTCDetails_Click(object sender, EventArgs e)
        {
            try
            {
                string sDTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfDTCId.Value));

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

        protected void cmdViewInvoice_Click(object sender, EventArgs e)
        {
            try
            {
                clsFormValues objApproval = new clsFormValues();
                string sTaskType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtType.Text));

                string sIndentId = objApproval.GetIndentId(txtInvoiceId.Text);
                sIndentId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sIndentId));
                string sInvoiceId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtInvoiceId.Text));

                string url = "/DTCFailure/InvoiceCreation.aspx?TypeValue=" + sTaskType + "&ReferID=" + sIndentId + "&InvoiceId=" + sInvoiceId;
                string s = "window.open('" + url + "', '_blank');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdViewInvoice_Click");
            }
        }

    }
}