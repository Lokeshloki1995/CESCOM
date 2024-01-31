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
using System.IO;
using System.Collections;


namespace IIITS.DTLMS.DTCFailure
{
    public partial class WorkOrder : System.Web.UI.Page
    {
        string strFormCode = "WorkOrder";
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
                txtCommdate.Attributes.Add("readonly", "readonly");
                txtDeDate.Attributes.Add("readonly", "readonly");
                if (!IsPostBack)
                {
                    Genaral.Load_Combo("SELECT MD_ID,MD_NAME from TBLMASTERDATA WHERE MD_TYPE='I' ORDER BY MD_ID", "--Select--", cmbIssuedBy);
                    Genaral.Load_Combo("SELECT MD_NAME,MD_NAME from TBLMASTERDATA WHERE MD_TYPE='C' ORDER BY MD_ID", "--Select--", cmbCapacity);
                    Genaral.Load_Combo("SELECT SCHM_ID,SCHM_NAME FROM TBLDTCSCHEME ORDER BY SCHM_ID", "--Select--", cmbDtc_Scheme_Type);


                    CalendarExtender_txtCommdate.EndDate = System.DateTime.Now;
                    CalendarExtender_txtDeDate.EndDate = System.DateTime.Now;

                    if (Request.QueryString["TypeValue"] != null && Request.QueryString["TypeValue"].ToString() != "")
                    {
                        txtType.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["TypeValue"]));
                        if (txtType.Text == "3")
                        {
                            ShowMsgBox("This Functionality had moved to MMS Please Login to MMS to create New DTC");
                        }
                        ChangeLabelText();
                        cmbIssuedBy.SelectedValue = "2";

                    }

                    if (Request.QueryString["ReferID"] != null && Request.QueryString["ReferID"].ToString() != "")
                    {

                        //From New DTC
                        if (txtType.Text == "3")
                        {
                            txtWOId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ReferID"]));
                            if (!txtWOId.Text.Contains("-"))
                            {
                                GetWODetailsNewDTC();
                                //cmbCapacity_SelectedIndexChanged(sender, e);
                            }

                            cmdSave.Text = "View";
                        }
                        else
                        {
                            txtFailureId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ReferID"]));
                            cmbCapacity.Enabled = false;
                            lblDtcScheme.Visible = false;
                            cmbDtc_Scheme_Type.Visible = false;

                            GetCommAndDecommAmount();
                            // GetCommAndDecommAccountCode();
                            if (!txtFailureId.Text.Contains("-"))
                            {
                                GetWorkOrderDetails();
                                cmdSearch_Click(sender, e);
                                cmbCapacity_SelectedIndexChanged(sender, e);
                                txtAcCode.Enabled = false;
                            }
                            if (txtType.Text == "2")
                            {
                                lnkBudgetstat.Visible = true;
                                txtCommAmount.Enabled = false;
                                txtDeAmount.Enabled = false;
                            }
                        }

                        //Check Indent Done to Update Work Order Details If Indent Done Restrict to Update
                        //ValidateFormUpdate();
                    }

                    // Call Search Window
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


        public void GetWorkOrderDetails()
        {
            try
            {

                clsWorkOrder objWorkOrder = new clsWorkOrder();
                objWorkOrder.sFailureId = txtFailureId.Text;

                objWorkOrder.GetWorkOrderDetails(objWorkOrder);

                txtWOId.Text = objWorkOrder.sWOId;
                txtFailureId.Text = objWorkOrder.sFailureId;
                txtFailureDate.Text = objWorkOrder.sFailureDate;
                hdfFailureId.Value = objWorkOrder.sFailureId;
                if (txtType.Text == "2" || txtType.Text == "4")
                {
                    cmbCapacity.SelectedValue = objWorkOrder.sEnhancedCapacity;
                    //Genaral.Load_Combo("SELECT MD_NAME,MD_NAME from TBLMASTERDATA WHERE MD_TYPE='C' ORDER BY MD_ID", "--Select--", cmbCapacity);
                }
                else
                    cmbCapacity.SelectedValue = objWorkOrder.sCapacity;

                if (objWorkOrder.sCommWoNo != null)
                {
                    cmbIssuedBy.SelectedValue = objWorkOrder.sIssuedBy;
                    //txtAcCode.SelectedValue = objWorkOrder.sAccCode;
                    txtAcCode.Items.Add(objWorkOrder.sAccCode);

                    txtComWoNo1.Text = objWorkOrder.sCommWoNo.Split('/').GetValue(0).ToString();
                    txtComWoNo2.Text = objWorkOrder.sCommWoNo.Split('/').GetValue(1).ToString();
                    txtComWoNo3.Text = objWorkOrder.sCommWoNo.Split('/').GetValue(2).ToString();

                    //txtComWoNo.Text = objWorkOrder.sCommWoNo;
                    txtCommdate.Text = objWorkOrder.sCommDate;
                    txtCommAmount.Text = objWorkOrder.sCommAmmount;

                    txtDeWoNo1.Text = objWorkOrder.sDeWoNo.Split('/').GetValue(0).ToString();
                    txtDeWoNo2.Text = objWorkOrder.sDeWoNo.Split('/').GetValue(1).ToString();
                    txtDeWoNo3.Text = objWorkOrder.sDeWoNo.Split('/').GetValue(2).ToString();

                    // txtDeWoNo.Text = objWorkOrder.sDeWoNo;
                    txtDeDate.Text = objWorkOrder.sDeCommDate;
                    txtDeAmount.Text = objWorkOrder.sDeCommAmmount;
                    txtDecAccCode.Text = objWorkOrder.sDecomAccCode;
                    cmbCapacity.SelectedValue = objWorkOrder.sNewCapacity;

                    if (txtType.Text == "3")
                    {
                        cmbSection.SelectedValue = objWorkOrder.sRequestLoc;
                    }

                    //cmdSave.Text = "Update";
                    txtFailureId.Enabled = false;
                    txtFailureDate.Enabled = false;
                    cmdSearch.Visible = false;
                }

                else
                {
                    //cmdSave.Text = "Save";
                    txtFailureId.Enabled = false;
                    txtFailureDate.Enabled = false;
                    cmdSearch.Visible = false;
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, " GetWorkOrderDetails");
            }

        }

        public void GetWODetailsNewDTC()
        {
            try
            {
                clsWorkOrder objWorkOrder = new clsWorkOrder();
                objWorkOrder.sWOId = txtWOId.Text;
                objWorkOrder.GetWODetailsForNewDTC(objWorkOrder);

                cmbIssuedBy.SelectedValue = objWorkOrder.sIssuedBy;
                //txtAcCode.SelectedValue = objWorkOrder.sAccCode;
                txtAcCode.Items.Add(objWorkOrder.sAccCode);

                if (objWorkOrder.sCommWoNo != null)
                {
                    txtComWoNo1.Text = objWorkOrder.sCommWoNo.Split('/').GetValue(0).ToString();
                    txtComWoNo2.Text = objWorkOrder.sCommWoNo.Split('/').GetValue(1).ToString();
                    txtComWoNo3.Text = objWorkOrder.sCommWoNo.Split('/').GetValue(2).ToString();
                }
                //txtComWoNo.Text = objWorkOrder.sCommWoNo;
                txtCommdate.Text = objWorkOrder.sCommDate;
                txtCommAmount.Text = objWorkOrder.sCommAmmount;
                cmbCapacity.SelectedValue = objWorkOrder.sNewCapacity;
                cmbSection.SelectedValue = objWorkOrder.sRequestLoc;
                //cmdSave.Text = "Update";
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetWODetailsNewDTC");

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
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowMsgBox");
            }
        }

        public bool ValidateForm()
        {
            bool bValidate = false;
            try
            {
                if (txtType.Text != "3")
                {
                    if (txtFailureId.Text.Trim().Length == 0)
                    {
                        txtFailureId.Focus();
                        ShowMsgBox("Enter Failure Id");
                        return bValidate;
                    }
                }

                if (cmbIssuedBy.SelectedIndex == 0)
                {
                    cmbIssuedBy.Focus();
                    ShowMsgBox("Select Issued By");
                    return bValidate;
                }
                if (cmbCapacity.SelectedIndex == 0)
                {
                    cmbCapacity.Focus();
                    ShowMsgBox("Select Capacity");
                    return bValidate;
                }

                if (txtType.Text == "3")
                {
                    if (cmbSection.SelectedIndex == 0)
                    {
                        cmbSection.Focus();
                        ShowMsgBox("Select Section");
                        return bValidate;
                    }
                }

                if (txtComWoNo1.Text.Trim().Length == 0 || txtComWoNo2.Text.Trim().Length == 0 || txtComWoNo3.Text.Trim().Length == 0)
                {
                    //txtComWoNo.Focus();
                    ShowMsgBox("Enter Commission WO Number");
                    return bValidate;
                }

                if (txtCommdate.Text.Trim() == "")
                {
                    txtCommdate.Focus();
                    ShowMsgBox("Enter Commissioning Date");
                    return bValidate;
                }
                string sResult = Genaral.DateComparision(txtCommdate.Text, txtFailureDate.Text, false, false);
                if (sResult == "2")
                {
                    ShowMsgBox("Commisioning Work Order Date should be Greater than " + lblDateText.Text + "");
                    return bValidate;
                }
                if (txtCommAmount.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Enter Commissioning Amount");
                    txtCommAmount.Focus();
                    return bValidate;
                }
                if (txtAcCode.SelectedValue == "")
                {
                    txtAcCode.Focus();
                    ShowMsgBox("Enter Commissioning Account Code");
                    return bValidate;
                }
                if (txtType.Text != "3")
                {
                    if (txtDeWoNo1.Text.Trim().Length == 0 || txtDeWoNo2.Text.Trim().Length == 0 || txtDeWoNo3.Text.Trim().Length == 0)
                    {
                        ShowMsgBox("Enter Decommissioning Wo No");
                        //txtDeWoNo.Focus();
                        return bValidate;
                    }
                    if (txtDeDate.Text.Trim().Length == 0)
                    {
                        ShowMsgBox("Enter DeCommissioning Date");
                        txtDeDate.Focus();
                        return bValidate;
                    }

                    sResult = Genaral.DateComparision(txtDeDate.Text, txtFailureDate.Text, false, false);
                    if (sResult == "2")
                    {
                        ShowMsgBox("DeCommisioning Work Order Date should be Greater than " + lblDateText.Text + "");
                        return bValidate;
                    }

                    if (txtDeAmount.Text.Length == 0)
                    {
                        txtDeAmount.Focus();
                        ShowMsgBox("Enter DeCommissioning Amount");
                        return bValidate;
                    }
                    if (txtDecAccCode.Text.Trim().Length == 0)
                    {
                        txtDecAccCode.Focus();
                        ShowMsgBox("Enter DeCommissioning Account Code");
                        return bValidate;
                    }
                    if (!System.Text.RegularExpressions.Regex.IsMatch(txtDeAmount.Text, "^(\\d{1,6})?(\\.\\d{1,2})?$"))
                    {
                        ShowMsgBox("Enter Valid DeCommissioning Amount (eg:111111.00)");
                        return false;
                    }

                    if (!System.Text.RegularExpressions.Regex.IsMatch(txtDeAmount.Text, "[-+]?[0-9]{0,5}\\.?[0-9]{0,2}"))
                    {
                        ShowMsgBox("Enter Valid DeCommissioning Amount (eg:111111.00)");
                        return false;
                    }
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(txtCommAmount.Text, "^(\\d{1,6})?(\\.\\d{1,2})?$"))
                {
                    ShowMsgBox("Enter Valid Commissioning Amount (eg:111111.00)");
                    return false;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(txtCommAmount.Text, "[-+]?[0-9]{0,5}\\.?[0-9]{0,2}"))
                {
                    ShowMsgBox("Enter Valid Commissioning Amount (eg:111111.00)");
                    return false;
                }

                //  Photo Save DTLMSDocs
                string sFileExt = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["NotAllowFileFormat"]);

                if (fupWODocument.PostedFile.ContentLength != 0)
                {

                    string sWOFileExt = string.Empty;
                    string sWOFileName = string.Empty;
                    string sDirectory = string.Empty;


                    sWOFileExt = System.IO.Path.GetExtension(fupWODocument.FileName).ToString().ToLower();
                    sWOFileExt = ";" + sWOFileExt.Remove(0, 1) + ";";

                    if (sFileExt.Contains(sWOFileExt))
                    {
                        ShowMsgBox("Invalid File Format");
                        return false;
                    }

                }
                if (txtActiontype.Text != "M")
                {

                    if (txtType.Text == "2" && objSession.RoleId == "7")
                    {
                        if (Convert.ToDouble(hdfBudget.Value) < Convert.ToDouble(txtCommAmount.Text))
                        {
                            ShowMsgBox("Commission Amount Less Than Budget Value");
                            return false;
                        }
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

        public void Reset()
        {
            try
            {
                txtFailureId.Text = string.Empty;
                txtFailureDate.Text = string.Empty;
                cmbIssuedBy.SelectedIndex = 0;
                txtDTCCode.Text = string.Empty;
                txtDTCName.Text = string.Empty;
                txtTCCode.Text = string.Empty;
                txtWOId.Text = string.Empty;


                txtComWoNo1.Text = string.Empty;
                txtComWoNo2.Text = string.Empty;
                txtComWoNo3.Text = string.Empty;

                txtCommdate.Text = string.Empty;
                txtCommAmount.Text = string.Empty;
                txtAcCode.SelectedValue = string.Empty;

                txtDeDate.Text = string.Empty;
                txtDeAmount.Text = string.Empty;


                txtDeWoNo1.Text = string.Empty;
                txtDeWoNo2.Text = string.Empty;
                txtDeWoNo3.Text = string.Empty;

                txtDecAccCode.Text = string.Empty;
                hdfFailureId.Value = string.Empty;
                cmdSave.Text = "Save";

                cmdSearch.Visible = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Reset");
            }

        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                string[] Arr = new string[2];
                clsWorkOrder objWorkOrder = new clsWorkOrder();
                int year = DateTime.Now.Year;

                objWorkOrder.sFinancialYear = Convert.ToString(year) + "-" + Convert.ToString(year + 1);
                if (DateTime.Now.ToString("MM") == "01" || DateTime.Now.ToString("MM") == "02" || DateTime.Now.ToString("MM") == "03")
                {
                    objWorkOrder.sFinancialYear = Convert.ToString(year - 1) + "-" + Convert.ToString(year);
                }
                objWorkOrder.sOfficeCode = objSession.OfficeCode;
                objWorkOrder.sDivCode = objWorkOrder.getdivcode(objWorkOrder);
                string Acccode = objWorkOrder.getBudgetAccountCode();
                objWorkOrder.sBudgetAccCode = Acccode.Substring(0, 6);
                if (txtActiontype.Text != "M")
                {

                    if (txtType.Text == "2" || txtType.Text == "5")
                    {
                        if (objSession.RoleId == "7")
                        {
                            objWorkOrder.sBudgetAmount = objWorkOrder.getAvailableBudget(objWorkOrder);
                            if (Convert.ToDouble(objWorkOrder.sBudgetAmount) <= 0)
                            {
                                ShowMsgBox("Budget Is Unavailable For The Account Code = " + Acccode);
                                return;
                            }
                            else
                            {
                                hdfBudget.Value = objWorkOrder.sBudgetAmount;
                            }
                        }
                    }
                }
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
                    clsWorkOrder objWrkOrder = new clsWorkOrder();
                    if (hdfApproveStatus.Value != "")
                    {

                        if (hdfApproveStatus.Value != "3")
                        {
                            objWrkOrder.sWFDataId = hdfWFDataId.Value;
                            objWrkOrder.sTaskType = txtType.Text;
                            if (txtType.Text == "3")
                            {
                            }
                            else
                            {
                                GenerateWorkOrderReport(objWrkOrder);
                            }
                        }
                    }
                    else
                    {
                        if (txtType.Text == "1" || txtType.Text == "2")
                        {
                            objWrkOrder.sWFDataId = objWorkOrder.getWoDataId(txtFailureId.Text);
                            objWrkOrder.sTaskType = txtType.Text;
                            GenerateWorkOrderReport(objWrkOrder);
                        }
                        else
                            if (txtType.Text == "3")
                        {
                            objWrkOrder.sWOId = txtWOId.Text;
                            objWrkOrder.sTaskType = txtType.Text;

                            if (txtType.Text == "3")
                            {
                            }
                            else
                            {
                                GenerateWorkOrderReport(objWrkOrder);
                            }
                        }
                    }

                    return;
                }

                if (ValidateForm() == true)
                {
                    objWorkOrder.sWOId = txtWOId.Text;

                    objWorkOrder.sFailureId = txtFailureId.Text.Trim();
                    objWorkOrder.sFailureDate = txtFailureDate.Text.Replace("'", "");
                    objWorkOrder.sIssuedBy = cmbIssuedBy.SelectedValue;
                    objWorkOrder.sCapacity = txtCapacity.Text.Trim();
                    objWorkOrder.sDTCCode = txtDTCCode.Text.Trim();
                    objWorkOrder.sDTCName = txtDTCName.Text.Trim();

                    objWorkOrder.sNewCapacity = cmbCapacity.SelectedValue.Trim();

                    string sCommWONo = txtComWoNo1.Text.Trim().Replace("'", "") + "/" + txtComWoNo2.Text.Trim().Replace("'", "") + "/" + txtComWoNo3.Text.Trim().Replace("'", "");
                    objWorkOrder.sCommWoNo = sCommWONo.Trim().ToUpper();
                    objWorkOrder.sCommDate = txtCommdate.Text.Trim().Replace("'", "");
                    objWorkOrder.sCommAmmount = txtCommAmount.Text.Trim().Replace("'", "");
                    objWorkOrder.sAccCode = txtAcCode.SelectedValue.Trim().Replace("'", "");

                    string sDeWoNo = txtDeWoNo1.Text.Trim().Replace("'", "") + "/" + txtDeWoNo2.Text.Trim().Replace("'", "") + "/" + txtDeWoNo3.Text.Trim().Replace("'", "");
                    objWorkOrder.sDeWoNo = sDeWoNo.Trim().ToUpper();
                    objWorkOrder.sDeCommDate = txtDeDate.Text.Trim().Replace("'", "");
                    objWorkOrder.sDeCommAmmount = txtDeAmount.Text.Trim().Replace("'", "");
                    objWorkOrder.sDecomAccCode = txtDecAccCode.Text.Trim().Replace("'", "");

                    objWorkOrder.sCrBy = objSession.UserId;
                    objWorkOrder.sLocationCode = objSession.OfficeCode;
                    objWorkOrder.sTaskType = txtType.Text;
                    objWorkOrder.sDtcScheme = cmbDtc_Scheme_Type.SelectedIndex;
                    objWorkOrder.Budget = hdfBudget.Value;
                    objWorkOrder.sRoleId = objSession.RoleId;

                    if (txtType.Text == "3")
                    {
                        objWorkOrder.sRequestLoc = cmbSection.SelectedValue.Trim();
                    }

                    if (fupWODocument.PostedFile.ContentLength != 0)
                    {

                        string sWOFileName = System.IO.Path.GetFileName(fupWODocument.PostedFile.FileName);

                        fupWODocument.SaveAs(Server.MapPath("~/DTLMSFiles" + "/" + sWOFileName));
                        string sDirectory = Server.MapPath("~/DTLMSFiles" + "/" + sWOFileName);
                        objWorkOrder.sWOFilePath = sDirectory;
                    }

                    #region Approve And Reject

                    if (txtActiontype.Text == "A" || txtActiontype.Text == "R")
                    {
                        if (hdfWFDataId.Value != "0")
                        {
                            ApproveRejectAction();
                            Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "(WorkOrder)-Failure");
                            return;
                        }
                    }

                    #endregion

                    //Workflow
                    WorkFlowObjects(objWorkOrder);

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
                        objWorkOrder.sWOId = "";
                        objWorkOrder.sActionType = txtActiontype.Text;
                        objWorkOrder.sCrBy = hdfCrBy.Value;

                        Arr = objWorkOrder.SaveUpdateWorkOrder(objWorkOrder);
                        if (Arr[1].ToString() == "0")
                        {
                            hdfWFDataId.Value = objWorkOrder.sWFDataId;
                            hdfAppDesc.Value = objWorkOrder.sApprovalDesc;
                            ApproveRejectAction();
                            Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "(WorkOrder)-Failure");
                            return;
                        }
                        if (Arr[1].ToString() == "2")
                        {
                            ShowMsgBox(Arr[0]);
                            return;
                        }
                    }

                    #endregion

                    Arr = objWorkOrder.SaveUpdateWorkOrder(objWorkOrder);


                    if (Arr[1].ToString() == "0")
                    {
                        ShowMsgBox(Arr[0]);
                        txtWOId.Text = objWorkOrder.sWOId;
                        //ApproveRejectAction();
                        cmdSave.Text = "Update";
                        cmdSave.Enabled = false;
                        if (txtType.Text == "3")
                        {
                        }
                        else
                        {
                            GenerateWorkOrderReport(objWorkOrder);
                        }
                        return;
                    }
                    if (Arr[1].ToString() == "1")
                    {

                        if (txtActiontype.Text == "A" || txtActiontype.Text == "R")
                        {
                            ApproveRejectAction();
                            return;
                        }

                        if (txtActiontype.Text == "M")
                        {
                            ShowMsgBox("Modified and Approved Successfully");
                        }
                        else
                        {
                            ShowMsgBox(Arr[0]);
                        }
                        return;
                    }
                    if (Arr[1].ToString() == "2")
                    {
                        ShowMsgBox(Arr[0]);
                        return;
                    }
                    Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "(WorkOrder)-Failure");
                }

            }

            catch (Exception ex)
            {

                //lblMessage.Text = clsException.ErrorMsg();
                ShowMsgBox("Something Went Wrong Please Approve Once Again.");
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, " cmdSave_Click");
            }
        }
        private void GenerateWorkOrderReport(clsWorkOrder objWorkOrder)
        {
            string strParam = string.Empty;
            string sAET = string.Empty;
            string sSTO = string.Empty;
            string sAO = string.Empty;
            string sDo = string.Empty;
            string sLevelOfApproval = string.Empty;
            ArrayList sNameList = new ArrayList();
            string soffCode = objSession.OfficeCode;
            string sOffcName = objSession.OfficeName;
            //sLevelOfApproval = objWorkOrder.getLevelOfApproval(objWorkOrder, txtFailureId.Text, soffCode);
            sLevelOfApproval = getApprovalLevel().ToString();
            sNameList = objWorkOrder.getCreatedByUserName(txtFailureId.Text, soffCode);

            string sWFDataId = objWorkOrder.sWFDataId;
            string sWoId = objWorkOrder.sWOId;
            string sTaskType = objWorkOrder.sTaskType;
            string sFailureid = txtFailureId.Text;
            Session["UserNameList"] = sNameList;

            strParam = "id=WorkOrderPreview&WFDataId=" + sWFDataId + "&LApprovel=" + sLevelOfApproval + "&OffCode=" + sOffcName + "&TaskType=" + sTaskType + "&WoId=" + sWoId + "&FailureID=" + sFailureid;
            RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
        }




        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                Reset();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdReset_Click");
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("WorkOrderView.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "btnClose_Click");
            }
        }

        public void ValidateFormUpdate()
        {
            try
            {
                clsWorkOrder objWorkOrder = new clsWorkOrder();

                if (objWorkOrder.ValidateUpdate(txtFailureId.Text, txtWOId.Text, txtType.Text) == true)
                {
                    cmdReset.Enabled = false;
                    cmdSave.Enabled = false;
                }
                else
                {
                    cmdReset.Enabled = true;
                    cmdSave.Enabled = true;
                }


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ValidateFormUpdate");
            }
        }

        protected void cmdSearch_Click(object sender, EventArgs e)
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
                txtDeclaredBy.Text = objFailure.sCrby;
                txtTCCode.Text = objFailure.sDtcTcCode;
                cmbCapacity.SelectedValue = objFailure.sEnhancedCapacity;
                txtDTCId.Text = objFailure.sDtcId;
                txtTCId.Text = objFailure.sTCId;
                txtSubdivisionName.Text = objFailure.sSubDivName;
                txtSectionName.Text = objFailure.sDtcLocation;
                txtCustomerName.Text = objFailure.sCustomerName;
                txtCustomerNumber.Text = objFailure.sCustomerNumber;
                txtNumberofInstalmets.Text = objFailure.sNumberOfInstalments;
                //if (cmbCapacity.SelectedIndex == 0)
                //{
                //txtCapacity.Text = objFailure.sDtcCapacity;

                if (txtType.Text == "1")
                {
                    txtCapacity.Text = objFailure.sDtcCapacity;
                    cmbCapacity.SelectedValue = objFailure.sDtcCapacity;
                    txtDtrCapacity.Text = objFailure.sDtcCapacity;
                    txtFailureType.Text = "FAILURE";
                }
                else
                {
                    txtCapacity.Text = objFailure.sDtcCapacity;
                    cmbCapacity.SelectedValue = objFailure.sEnhancedCapacity;
                    txtDtrCapacity.Text = objFailure.sDtcCapacity;

                    if (txtType.Text == "2")
                        txtFailureType.Text = "GOOD ENHANCEMENT";
                    else if (txtType.Text == "4")
                        txtFailureType.Text = "FAILURE ENHANCEMENT";
                    else if (txtType.Text == "5")
                        txtFailureType.Text = "GOOD REDUCTION";
                    else
                        txtFailureType.Text = "Others";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdSearch_Click");
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
                    lblDateText.Text = "Enhancement Entry Date";
                }
                else if (txtType.Text == "5")
                {
                    lblIDText.Text = "Reduction ID";
                    lblDateText.Text = "Reduction Entry Date";
                }
                else
                {
                    dvDecomm.Style.Add("display", "none");
                    dvComm.Attributes.Add("class", "span12");
                    dvBasic.Style.Add("display", "none");
                    dvSection.Style.Add("display", "block");
                    Genaral.Load_Combo("SELECT OM_CODE, OM_NAME FROM TBLOMSECMAST WHERE OM_CODE LIKE '" + objSession.OfficeCode + "%' ORDER BY OM_CODE ", "--Select--", cmbSection);
                    lnkDTCDetails.Visible = false;
                    lnkDTrDetails.Visible = false;

                    cmdViewEstimate.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ChangeLabelText");
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
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "CheckAccessRights");
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
                objWorkOrder.sWFOId = hdfWFOId.Value;
                objWorkOrder.sWFAutoId = hdfWFOAutoId.Value;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "WorkFlowObjects");
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
                if (hdfWFOAutoId.Value == "0")
                {
                    dvComments.Style.Add("display", "block");
                }

                if (hdfWFOAutoId.Value != "0")
                {
                    cmdSave.Text = "Save";
                    dvComments.Style.Add("display", "none");
                }

                cmdReset.Enabled = false;


                // Check for Creator of Form
                bool bResult = CheckFormCreatorLevel();
                if (bResult == true)
                {
                    cmdSave.Text = "Save";
                    pnlApproval.Enabled = true;
                    //dvComments.Style.Add("display", "none");

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
                    objApproval.sDescription = hdfAppDesc.Value;
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

                    if (objApproval.sApproveStatus == "1")
                    {
                        ShowMsgBox("Approved Successfully");
                        cmdSave.Enabled = false;
                        clsWorkOrder objWO = new clsWorkOrder();
                        objWO.sWFDataId = objApproval.sWFDataId;
                        objWO.sWFObjectId = objApproval.sWFObjectId;
                        objWO.sTaskType = txtType.Text;
                        if (txtType.Text == "3")
                        {
                        }
                        else
                        {
                            GenerateWorkOrderReport(objWO);
                        }
                        if (objSession.RoleId == "3")
                        {

                            string sCommWONo = txtComWoNo1.Text.Trim().Replace("'", "") + "/" + txtComWoNo2.Text.Trim().Replace("'", "") + "/" + txtComWoNo3.Text.Trim().Replace("'", "");
                            //objWO.SendSMStoSectionOfficer(txtFailureId.Text,txtDTCCode.Text, sCommWONo.ToUpper(), txtDTCName.Text);
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
                        clsWorkOrder objWo = new clsWorkOrder();
                        objWo.sWFDataId = objApproval.sWFDataId;
                        objWo.sWFObjectId = objApproval.sWFObjectId;
                        objWo.sTaskType = txtType.Text;
                        if (txtType.Text == "3")
                        {
                        }
                        else
                        {
                            GenerateWorkOrderReport(objWo);
                        }
                        if (objSession.RoleId == "3")
                        {
                            clsWorkOrder objWO = new clsWorkOrder();
                            string sCommWONo = txtComWoNo1.Text.Trim().Replace("'", "") + "/" + txtComWoNo2.Text.Trim().Replace("'", "") + "/" + txtComWoNo3.Text.Trim().Replace("'", "");
                            //objWO.SendSMStoSectionOfficer(txtFailureId.Text, txtDTCCode.Text, sCommWONo.ToUpper(), txtDTCName.Text);
                        }
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
                // lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdApprove_Click");
                throw ex;
            }
        }


        public void WorkFlowConfig()
        {
            try
            {
                //WorkFlow / Approval
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
                        GetWODetailsFromXML(hdfWFDataId.Value);
                    }
                    SetControlText();
                    ControlEnableDisable();
                    if (txtActiontype.Text == "V")
                    {
                        cmdSave.Text = "View";
                        cmdSave.Enabled = true;
                        cmdReset.Enabled = false;
                        dvComments.Style.Add("display", "none");
                    }
                }
                else
                {
                    //if (cmdSave.Text != "Save" && cmdSave.Text != "View")
                    //{
                    //    cmdSave.Enabled = false;
                    //}
                    if (txtType.Text != "3")
                    {
                        cmdSave.Text = "View";
                    }
                }

                DisableControlForView();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                // clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "WorkFlowConfig");
            }
        }

        public bool CheckFormCreatorLevel()
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                string sResult = objApproval.GetFormCreatorLevel("", objSession.RoleId, "WorkOrder");
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
                    Response.Redirect("WorkOrderView.aspx", false);
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdClose_Click");
            }
        }

        public void GetCommAndDecommAmount()
        {
            try
            {
                clsEstimation objEst = new clsEstimation();
                objEst.sFailureId = txtFailureId.Text;
                objEst.GetCommAndDecommAmount(objEst);

                txtCommAmount.Text = objEst.sTotal;
                txtDeAmount.Text = objEst.sDecommTotal;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetCommAndDecommAmount");
            }
        }

        public void GetCommAndDecommAccountCode()
        {
            try
            {
                clsWorkOrder objWO = new clsWorkOrder();
                System.Web.UI.WebControls.ListItem lstCommAccCodes = new System.Web.UI.WebControls.ListItem();
                //List<string> lstCommAccCodes = new List<string>();
                objWO.sCapacity = cmbCapacity.SelectedValue;


                objWO.GetCommDecommAccCode(objWO);

                if (txtType.Text == "1")
                {
                    //txtAcCode.SelectedValue = objWO.sAccCode;
                    txtAcCode.Items.Add(objWO.sAccCode);
                    txtDecAccCode.Text = objWO.sDecomAccCode;
                }
                else if (txtType.Text == "4")
                {
                    //lstCommAccCodes.Add(objWO.sAccCode);
                    //lstCommAccCodes.Add(objWO.sEnhanceAccCode);

                    // txtAcCode.Items.Add(objWO.sAccCode);
                    txtAcCode.Items.Add(objWO.sEnhanceAccCode);
                    txtDecAccCode.Text = objWO.sDecomAccCode;
                }
                else if (txtType.Text == "2")
                {
                    //txtAcCode.SelectedValue = objWO.sEnhanceAccCode;
                    //txtAcCode.Items.Add(objWO.sEnhanceAccCode);
                    txtAcCode.Items.Add(objWO.sGoodEnhanceAccCode);
                    txtDecAccCode.Text = objWO.sDecomAccCode;
                }
                else if (txtType.Text == "5")
                {
                    // txtAcCode.Items.Add(objWO.sEnhanceAccCode);
                    txtAcCode.Items.Add(objWO.sGoodEnhanceAccCode);
                    txtDecAccCode.Text = objWO.sDecomAccCode;
                }
                else if (txtType.Text == "3")
                {
                    //txtAcCode.SelectedValue = objWO.sAccCode;
                    txtAcCode.Items.Add(objWO.sAccCode);
                    txtDecAccCode.Text = objWO.sDecomAccCode;
                }


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetCommAndDecommAccountCode");
            }
        }

        protected void lnkDTrDetails_Click(object sender, EventArgs e)
        {
            try
            {
                string sTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtTCId.Text));

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

        protected void cmbCapacity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GetCommAndDecommAccountCode();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbCapacity_SelectedIndexChanged");
            }
        }

        public void ControlEnableDisable()
        {
            try
            {
                txtFailureId.Enabled = false;
                cmdSearch.Visible = false;
                if (txtType.Text == "2" && txtActiontype.Text == "M")
                {
                    txtCommAmount.Enabled = false;
                    txtDeAmount.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ControlEnableDisable");
            }
        }


        #region Load From XML
        public void GetWODetailsFromXML(string sWFDataId)
        {
            try
            {
                // If the Data saved in Main Table then this function shd not execute, so done restriction like below
                // And commented for temprary purpose.. nee to change in future

                //if (!txtWOId.Text.Contains("-") && txtWOId.Text!="" )
                //{
                //    return;
                //}

                clsWorkOrder objWorkOrder = new clsWorkOrder();
                objWorkOrder.sWFDataId = sWFDataId;

                objWorkOrder.GetWODetailsFromXML(objWorkOrder);

                if (txtType.Text != "3")
                {

                    //txtWOId.Text = objWorkOrder.sWOId;
                    txtFailureId.Text = objWorkOrder.sFailureId;
                    //txtFailureDate.Text = objWorkOrder.sFailureDate;
                    hdfFailureId.Value = objWorkOrder.sFailureId;
                    if (objWorkOrder.sCommWoNo != null)
                    {
                        cmbIssuedBy.SelectedValue = objWorkOrder.sIssuedBy;
                        //txtAcCode.SelectedValue = objWorkOrder.sAccCode;
                        // txtAcCode.Items.RemoveAt(0);
                        txtAcCode.Items.Remove(new ListItem(txtAcCode.SelectedValue));
                        txtAcCode.Items.Add(objWorkOrder.sAccCode);

                        txtComWoNo1.Text = objWorkOrder.sCommWoNo.Split('/').GetValue(0).ToString();
                        txtComWoNo2.Text = objWorkOrder.sCommWoNo.Split('/').GetValue(1).ToString();
                        txtComWoNo3.Text = objWorkOrder.sCommWoNo.Split('/').GetValue(2).ToString();

                        //txtComWoNo.Text = objWorkOrder.sCommWoNo;
                        txtCommdate.Text = objWorkOrder.sCommDate;
                        txtCommAmount.Text = objWorkOrder.sCommAmmount;

                        txtDeWoNo1.Text = objWorkOrder.sDeWoNo.Split('/').GetValue(0).ToString();
                        txtDeWoNo2.Text = objWorkOrder.sDeWoNo.Split('/').GetValue(1).ToString();
                        txtDeWoNo3.Text = objWorkOrder.sDeWoNo.Split('/').GetValue(2).ToString();

                        // txtDeWoNo.Text = objWorkOrder.sDeWoNo;
                        txtDeDate.Text = objWorkOrder.sDeCommDate;
                        txtDeAmount.Text = objWorkOrder.sDeCommAmmount;
                        txtDecAccCode.Text = objWorkOrder.sDecomAccCode;
                        cmbCapacity.SelectedValue = objWorkOrder.sNewCapacity;


                        hdfCrBy.Value = objWorkOrder.sCrBy;
                        if (txtType.Text == "3")
                        {
                            cmbSection.SelectedValue = objWorkOrder.sRequestLoc;
                        }

                        //cmdSave.Text = "Update";
                        txtFailureId.Enabled = false;
                        txtFailureDate.Enabled = false;
                        cmdSearch.Visible = false;
                    }

                    else
                    {
                        //cmdSave.Text = "Save";
                        txtFailureId.Enabled = false;
                        txtFailureDate.Enabled = false;
                        cmdSearch.Visible = false;
                    }
                }
                else
                {
                    cmbIssuedBy.SelectedValue = objWorkOrder.sIssuedBy;
                    // txtAcCode.SelectedValue = objWorkOrder.sAccCode;//
                    txtAcCode.Items.Add(objWorkOrder.sAccCode);

                    txtComWoNo1.Text = objWorkOrder.sCommWoNo.Split('/').GetValue(0).ToString();
                    txtComWoNo2.Text = objWorkOrder.sCommWoNo.Split('/').GetValue(1).ToString();
                    txtComWoNo3.Text = objWorkOrder.sCommWoNo.Split('/').GetValue(2).ToString();
                    //txtComWoNo.Text = objWorkOrder.sCommWoNo;
                    txtCommdate.Text = objWorkOrder.sCommDate;
                    txtCommAmount.Text = objWorkOrder.sCommAmmount;
                    cmbCapacity.SelectedValue = objWorkOrder.sNewCapacity;
                    cmbSection.SelectedValue = objWorkOrder.sRequestLoc;
                    hdfCrBy.Value = objWorkOrder.sCrBy;
                    cmbDtc_Scheme_Type.SelectedIndex = objWorkOrder.sDtcScheme;
                    //cmdSave.Text = "Update";
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetFailureDetailsFromXML");
            }

        }
        #endregion

        public void LoadSearchWindow()
        {
            try
            {
                string strQry = string.Empty;

                strQry = "Title=Search and Select DTC failure Details&";
                strQry += "Query=SELECT DF_ID,DT_NAME,DT_CODE from TBLDTCMAST,TBLDTCFAILURE WHERE DF_DTC_CODE= DT_CODE AND DF_REPLACE_FLAG=0 AND  DF_ID ";
                strQry += " NOT IN (SELECT WO_DF_ID FROM  TBLWORKORDER WHERE WO_DF_ID IS NOT NULL) ";
                strQry += " AND DF_LOC_CODE LIKE '" + objSession.OfficeCode + "%' ";
                strQry += " AND DF_STATUS_FLAG=" + txtType.Text + " AND {0} like %{1}% order by DF_ID&";
                strQry += "DBColName=DF_ID~DT_NAME~DT_CODE&";
                strQry += "ColDisplayName=" + lblIDText.Text + "~DTC_NAME~DTC_CODE&";

                strQry = strQry.Replace("'", @"\'");

                cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + hdfFailureId.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + hdfFailureId.ClientID + ")");


                txtCommdate.Attributes.Add("onblur", "return ValidateDate(" + txtCommdate.ClientID + ");");
                txtDeDate.Attributes.Add("onblur", "return ValidateDate(" + txtDeDate.ClientID + ");");

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadSearchWindow");
            }
        }

        protected void cmdViewEstimate_Click(object sender, EventArgs e)
        {
            try
            {
                string strParam = string.Empty;

                if (txtType.Text == "1" || txtType.Text == "4")
                {
                    strParam = "id=Estimation&FailureId=" + txtFailureId.Text;
                    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                }
                if (txtType.Text == "2")
                {
                    strParam = "id=EnhanceEstimation&EnhanceId=" + txtFailureId.Text;
                    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdViewEstimate_Click");
            }
        }

        public int getApprovalLevel()
        {

            int Level = 0;
            try
            {
                if (objSession.RoleId == "7")
                {
                    Level = 1;
                }
                else if (objSession.RoleId == "2")
                {
                    Level = 2;
                }
                else if (objSession.RoleId == "6")
                {
                    Level = 3;
                }
                else
                    Level = 4;
                return Level;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "getApprovalLevel");
                return Level;
            }

        }

        protected void cmbDtc_Scheme_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int Scapacity = cmbCapacity.SelectedIndex;
                clsWorkOrder objWO = new clsWorkOrder();
                objWO.sDtcScheme = cmbDtc_Scheme_Type.SelectedIndex;

                objWO.GetDTCAccCode(objWO);
                //txtAcCode.SelectedValue = objWO.sAccCode;
                txtAcCode.Items.Add(objWO.sAccCode);

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbDtc_Scheme_Type_SelectedIndexChanged");
            }
        }

        protected void lnkBudgetstat_Click(object sender, EventArgs e)
        {
            try
            {
                //Response.Redirect("/OilFlow/BudgetStatus.aspx", false);

                string url = "/DTCFailure/CommBudgetStatus.aspx";

                string s = "window.open('" + url + "','mypopup','width=1100,height=800');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
    }

}