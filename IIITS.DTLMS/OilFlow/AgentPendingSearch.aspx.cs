using IIITS.DTLMS.BL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.OilFlow
{
    public partial class AgentPendingSearch : System.Web.UI.Page
    {
        string strFormCode = "AgentpendingSearch";
        clsSession objSession;
        string Percentage = ConfigurationManager.AppSettings["Percentage"].ToString();
        string AmtRate = ConfigurationManager.AppSettings["AmtRate"].ToString();
        protected void Page_Load(object sender, EventArgs e)

       {
            try
            {
                objSession = (clsSession)Session["clsSession"];

                if (!IsPostBack)
                {
                    int days = ((System.DateTime.Now.Day - 1) <= 1) ? (System.DateTime.Now.Day - 1) : 0;
                    CalendarExtender2.EndDate = System.DateTime.Now;
                    CalendarExtender1.EndDate = System.DateTime.Now;
                    cmbBudgethead.Enabled = false;
                    cmbBudgethead.Visible = false;
                    if (CheckAccessRights("4"))
                    {
                        clsDTrRepairActivity objRepair = new clsDTrRepairActivity();
                        txtPodate.Attributes.Add("readonly", "readonly");
                        txtinvoicedate.Attributes.Add("readonly", "readonly");
                        txtamt.Enabled = false;

                        if (objSession.OfficeCode != null && objSession.OfficeCode != "")
                        {
                            Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION where DIV_CODE='" + objSession.OfficeCode + "' ORDER BY DIV_CODE", cmbDivision);
                            cmbDivision.Attributes.Add("readonly", "readonly");
                            Genaral.Load_Combo("SELECT  RA_ID,RA_NAME FROM TBLREPAIRERAGENCYMASTER inner join  TBLAGENCYDIVMAPPING on    RA_ID = AM_RA_ID where  AM_OFFICE_CODE='" + objSession.OfficeCode + "' and AM_STATUS='A' ORDER BY AM_OFFICE_CODE", "--Select--", cmbAgency);

                        }
                        else
                        {
                            Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION  ORDER BY DIV_CODE", "--Select--", cmbDivision);
                        }

                        string OffCode = objSession.OfficeCode;
                       GenerateInvoiceNo();
             
                        Genaral.Load_Combo("SELECT IT_ID, IT_CODE || '-' ||IT_NAME from TBLMMSITEMMASTER  Where IT_UOM='kLtr' and IT_CODE='601420' ", cmbOilType);

                        if (Request.QueryString["ActionType"] != null && Request.QueryString["ActionType"].ToString() != "")
                        {
                            txtActiontype.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));
                        }

                        objRepair.sItemCode = cmbOilType.SelectedValue;
                        objRepair.sOfficeCode = objSession.OfficeCode;
                        lbltotalquantity.Text = "Total  Available Quantity : " + objRepair.getTotalItemQnty(objRepair);
                        hdftotalqty.Value = objRepair.getTotalItemQnty(objRepair);
  


                        //WorkFlow / Approval
                        WorkFlowConfig();

                        string strQry = string.Empty;



                        strQry = "Title=Search and Select Already Entered PO&";
                        strQry += "Query=SELECT UPPER(OSD_PO_NO)OSD_PO_NO FROM TBLOILSENTMASTER WHERE OSD_OFFICE_CODE = '" + objSession.OfficeCode + "'  AND {0} like %{1}% &";
                        strQry += "DBColName=OSD_PO_NO&";
                        strQry += "ColDisplayName=Purchase Order No&";

                        strQry = strQry.Replace("'", @"\'");

                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load");
            }

        }
        protected void cmdSave_Click(object sender, EventArgs e)
        {
           
            try
            {
                string[] Arr = new string[2];
                string[] arrResult = new string[2];
                string availablebudget = string.Empty;
                string BudgetAmount = string.Empty;

                if (ValidateForm() == true)
                {
                    clsDTrRepairActivity objRepair = new clsDTrRepairActivity();
                    objRepair.sPurchaseOrderNo = txtPoNo.Text;
                    objRepair.sPurchaseDate = txtPodate.Text;
                    objRepair.sInvoiceNo = txtinvoiceno.Text;
                    objRepair.sInvoiceDate = txtinvoicedate.Text;

                    objRepair.sOilQty = txtquantity.Text;
                    objRepair.sQty = txtquantity.Text;
                    objRepair.sAmount  = txtamt.Text;
                    objRepair.sDivision = cmbDivision.SelectedValue.Trim();
                    objRepair.sOfficeCode = objSession.OfficeCode;
                    objRepair.sItemCode = cmbOilType.SelectedValue;
                    objRepair.sAgencyname = cmbAgency.SelectedValue;
                    objRepair.sAgencyName = cmbAgency.SelectedItem.Text;
                    objRepair.sPercentage = Percentage;
                    objRepair.sQuantityLtr = txtoilquantityltr.Text;
                    objRepair.sCrby = objSession.UserId;
                    int year = DateTime.Now.Year;
                    objRepair.sFinancialYear = Convert.ToString(year) + "-" + Convert.ToString(year + 1);
                    if (DateTime.Now.ToString("MM") == "01" || DateTime.Now.ToString("MM") == "02" || DateTime.Now.ToString("MM") == "03")
                    {
                        objRepair.sFinancialYear = Convert.ToString(year - 1) + "-" + Convert.ToString(year);
                    }


                    //if (txtActiontype.Text != "A" && txtActiontype.Text != "V")
                    if (txtActiontype.Text == "")
                    {
                        Arr = objRepair.LoadAlreadyPurchaseOrder(objRepair);
                        if (Arr[1].ToString() == "2")
                        {
                            ShowMsgBox(Arr[0]);
                            return;
                        }
                        if (Convert.ToDouble(txtquantity.Text) > Convert.ToDouble(hdftotalqty.Value))
                        {                      
                            ShowMsgBox("Quantity(in kltr) Should be less than or equals to Total  Availbale Quantity");
                            return;
                        }
                    }

                    if (txtActiontype.Text == "A" || txtActiontype.Text == "R")
                    {
                        ApproveRejectAction();
               
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "AgentpendingSearch");
                            //GeneratePurchaseOrderReport(objRepair);
                        }
                        return;
                    }
                    if(txtActiontype.Text == "V")
                    {
                        GeneratePurchaseOrderReport();
                    }

                    //Workflow
                    WorkFlowObjects(objRepair);

                    // For Modify and Approve
                    if (txtActiontype.Text == "M")
                    {
                        if (txtComment.Text.Trim() == "")
                        {
                            ShowMsgBox("Enter Comments/Remarks");
                            txtComment.Focus();
                            return;
                        }
                        objRepair.sPurchaseOrderNo = txtPoNo.Text;
                        objRepair.sPurchaseDate = txtPodate.Text;
                        objRepair.sInvoiceNo = txtinvoiceno.Text;
                        objRepair.sInvoiceDate = txtinvoicedate.Text;
                        objRepair.sOilQty = txtquantity.Text;
                        objRepair.sQty = txtquantity.Text;
                        objRepair.sDivision = cmbDivision.SelectedValue.Trim();
                        objRepair.sOfficeCode = objSession.OfficeCode;
                        objRepair.sAgencyname = cmbAgency.SelectedValue;
                        objRepair.sActionType = txtActiontype.Text;
                        objRepair.sCrby = hdfCrBy.Value;

                        objRepair.sAmount = txtamt.Text;
                        objRepair.sItemCode = cmbOilType.SelectedValue;
                        objRepair.sAgencyname = cmbAgency.SelectedValue;
                        objRepair.sAgencyName = cmbAgency.SelectedItem.Text;
                        objRepair.sPercentage = Percentage;

                        if (Convert.ToDouble(txtquantity.Text) > Convert.ToDouble(hdftotalqty.Value))
                        {
                            ShowMsgBox("Quantity(in kltr) Should be less than or equals to Total  Availbale Quantity");
                            return;
                        }

                        arrResult = objRepair.getItemQnty(objRepair);
                        if (arrResult != null)
                        {

                            if (arrResult[1] != "SUCCESS")
                            {
                                ShowMsgBox(arrResult[1]);
                                return;
                            }

                        }

                        Arr = objRepair.SaveUpdateAgentdetails(objRepair);


                        if (Arr[1].ToString() == "0")
                        {
                            hdfWFDataId.Value = objRepair.sWFDataId;
                            ApproveRejectAction();
                            if (objSession.sTransactionLog == "1")
                            {
                                Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "AgentpendingSearch");
                            }
                            return;
                        }
                        if (Arr[1].ToString() == "2")
                        {
                            ShowMsgBox(Arr[0]);
                            if (objSession.sTransactionLog == "1")
                            {
                                Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "AgentpendingSearch");
                            }
                            return;
                        }
                    }
                    if (txtActiontype.Text == "A" || txtActiontype.Text == "")
                    {

                        arrResult = objRepair.getItemQnty(objRepair);
                        if (arrResult != null)
                        {

                            if (arrResult[1] != "SUCCESS")
                            {
                                ShowMsgBox(arrResult[1]);
                                return;
                            }

                        }
                        objRepair.sDivCode = objRepair.getdivcode(objRepair);
                        objRepair.sBudgetAccCode = objRepair.getBudgetAccountCode();
                        objRepair.sBudgetAmount = objRepair.getAvailableBudget(objRepair);


                        if (Convert.ToDouble(objRepair.sBudgetAmount) < 0)
                        {
                            ShowMsgBox("Budget Unavailable");
                            return;
                        }

                        Arr = objRepair.SaveUpdateAgentdetails(objRepair);


                        if (Arr[1].ToString() == "0")
                        {

                            ShowMsgBox(Arr[0]);
                            cmdSave.Enabled = false;
                            // Reset();
                            return;
                        }
                    }
                }

           }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdSave_Click");
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
                        GeneratePurchaseOrderReport();
                        cmdSave.Enabled = false;
                        
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
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "cmdApprove_Click");
            }
        }



        protected void cmdReset_Click(object sender, EventArgs e)
        {
            Reset();
        }
        protected void Reset()
        {
            txtPoNo.Text = string.Empty;
            txtPodate.Text = string.Empty;
            txtquantity.Text = string.Empty;
            //txtinvoiceno.Text = string.Empty;
            txtinvoicedate.Text = string.Empty;
            cmbDivision.SelectedIndex = 0;
            cmbAgency.SelectedIndex = 0;
            //txtPonum.Text = string.Empty;
        }
        public bool ValidateForm()
        {
            bool bValidate = false;
            try
            {
                if (txtPoNo.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Enter Purchase Order Number");
                    txtPoNo.Focus();
                    return bValidate;
                }
                if (txtPodate.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Enter Purchase Order Date");
                    return bValidate;
                }
                if (txtquantity.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Enter Quantity");
                    txtquantity.Focus();
                    return bValidate;
                }
                if(txtquantity.Text.Trim() == "0" || txtquantity.Text.Trim() == "00" || txtquantity.Text.Trim() == "000" || txtquantity.Text.Trim() == "0000")
                {
                    ShowMsgBox("Enter Valid Quantity");
                    txtquantity.Focus();
                    return bValidate;
                }
                if (txtinvoiceno.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Enter Invoice Number");
                    txtinvoiceno.Focus();
                    return bValidate;
                }
                if (txtinvoicedate.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Enter Invoice Date");
                    return bValidate;
                }
                if (cmbDivision.SelectedIndex == 0 && cmbDivision.Text.Trim().Length==0)
                {
                    cmbDivision.Focus();
                    ShowMsgBox("Select Division");
                    return bValidate;
                }
                if (cmbAgency.SelectedIndex == 0)
                {
                    cmbAgency.Focus();
                    ShowMsgBox("Select Agency Name");
                    return bValidate;
                }

                string sResult = Genaral.DateComparision(txtinvoicedate.Text, txtPodate.Text, false, false);
                if (sResult == "2")
                {
                    ShowMsgBox("Invoice Date should be Greater than Or Equal To Po Date");
                    return bValidate;
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
        public void SetControlText()
        {
            try
            {
                txtActiontype.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));

                if (txtActiontype.Text == "A")
                {
                    txtPoNo.ReadOnly = true;
                    txtPodate.ReadOnly = true;
                    txtquantity.ReadOnly = true;
                    txtinvoiceno.ReadOnly = true;
                    txtinvoicedate.ReadOnly = true;
                    cmbAgency.Enabled = false;
                    cmbOilType.Enabled = false;
                    rbdNewPo.Visible = false;
                    lbltotalquantity.Visible = false;
                    txtPodate.Enabled = false;
                    txtinvoicedate.Enabled = false;
                    dvComments.Style.Add("display", "block");
                    if(objSession.RoleId=="25"|| objSession.RoleId == "3")
                    {
                        Genaral.Load_Combo("SELECT  MD_ID,MD_NAME from TBLMASTERDATA  Where MD_TYPE='FMSACHEAD' and MD_ID='1' ", cmbBudgethead);
                        lblAvailablebudget.Visible = false;
                        lblTotalAmt.Visible = false;
                        lblTotalBudget.Visible = false;
                        dvhead.Style.Add("display", "block");
                        cmbBudgethead.Visible = true;
                    }
           

                    cmdSave.Text = "Approve";
                    pnlApproval.Enabled = false;
                }
                if (txtActiontype.Text == "R")
                {                    
                    cmdSave.Text = "Reject";
                    pnlApproval.Enabled = false;
                    lbltotalquantity.Visible = false;
                
                }
                if (txtActiontype.Text == "M")
                {
                    dvComments.Style.Add("display", "block");                  
                    cmdSave.Text = "Modify and Approve";
                    pnlApproval.Enabled = true;
                    txtPoNo.ReadOnly = true;
                    cmbAgency.Enabled = false;
                    lbltotalquantity.Visible = true;
                }

                if (txtActiontype.Text == "V" && hdfWFDataId.Value != "")
                {
                    txtPoNo.ReadOnly = true;
                    txtPodate.ReadOnly = true;
                    txtquantity.ReadOnly = true;
                    txtinvoiceno.ReadOnly = true;
                    txtinvoicedate.ReadOnly = true;
                    cmbAgency.Enabled = false;              
                    //cmdSave.Enabled = false;
                    pnlApproval.Enabled = false;
                    rbdNewPo.Visible = false;
                    lbltotalquantity.Visible = false;
                    txtPodate.Enabled = false;
                    txtinvoicedate.Enabled = false;
                    //rbdOldPo.Visible = false;
                    cmdSave.Text = "View";
                }

                cmdReset.Enabled = false;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "SetControlText");
            }
        }
        public bool CheckFormCreatorLevel()
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                string sResult = objApproval.GetFormCreatorLevel("", objSession.RoleId, "AgentpendingSearch");
                if (sResult == "1")
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "GetFormCreatorLevel");
                return false;
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
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "ShowMsgBox");
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

                }
                else
                {
                    //if (txtType.Text != "3")
                    //{
                    //    cmdSave.Text = "View";
                    //}
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "WorkFlowConfig");
            }
        }

        public void WorkFlowObjects(clsDTrRepairActivity objRepair)
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


                objRepair.sFormName = "AgentpendingSearch";
                objRepair.sOfficeCode = objSession.OfficeCode;
                objRepair.sClientIP = sClientIP;
                objRepair.sWFOId = hdfWFOId.Value;
                objRepair.sWFAutoId = hdfWFOAutoId.Value;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "WorkFlowObjects");
            }
        }
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "AgentpendingSearch";
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
                    Response.Redirect("~/Dashboard.aspx", false);

                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdClose_Click");
            }
        }

        public void GetWODetailsFromXML(string sWFDataId)
        {
            try
            {
                clsDTrRepairActivity objRepair = new clsDTrRepairActivity();
                objRepair.sWFDataId = sWFDataId;
                objRepair.GetAgencyDetailsFromXML(objRepair);

                txtPoNo.Text = objRepair.sPurchaseOrderNo;
                txtPodate.Text = objRepair.sPurchaseDate;
                txtquantity.Text = objRepair.sOilQty;
                txtinvoiceno.Text = objRepair.sInvoiceNo;
                txtinvoicedate.Text = objRepair.sInvoiceDate;
                cmbDivision.Text = objRepair.sDivision;
                cmbAgency.SelectedValue = objRepair.sAgencyname;
                txtamt.Text  = objRepair.sAmount;
                txtoilquantityltr.Text = objRepair.sQuantityLtr;

               
                hdfCrBy.Value = objRepair.sCrby;

                if (objRepair.sPurchaseOrderNo != "")
                {

                    cmdReset.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "GetFailureDetailsFromXML");
            }

        }

        protected void rbdNewPo_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtPoNo.Enabled = true;
                txtPodate.Enabled = true;
                cmbAgency.Enabled = true;
                NewPoReset();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "rbdNewPo_CheckedChanged");
            }
        }

        protected void rbdOldPo_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtPoNo.Enabled = false;
                txtPodate.Enabled = false;
                cmbAgency.Enabled = false;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "rbdOldPo_CheckedChanged");
            }
        }
        

        protected int CheckPoDate(string spodate)
        {
            try
            {
                DateTime dt = DateTime.ParseExact(spodate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                Double ts = ((DateTime.Now - dt).TotalDays);
                if ((DateTime.Now - dt).TotalDays > 365)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "CheckPoDate");
                return 0;
            }
        }
        public void GenerateInvoiceNo()
        {
            try
            {
                clsInvoice objInvoice = new clsInvoice();

                txtinvoiceno.Text = objInvoice.GenerateInvoiceNo(objSession.OfficeCode);
                txtinvoiceno.ReadOnly = true;
                txtinvoiceno.Enabled = false;
                //hdfInvoiceNo.Value = txtInvoiceNo.Text;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GenerateInvoiceNo");
            }
        }


        public void NewPoReset()
        {
            try
            {
                txtPoNo.Text = string.Empty;
                txtPodate.Text = string.Empty;
                cmbAgency.SelectedValue = null;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GenerateInvoiceNo");
            }
        }
        private void GeneratePurchaseOrderReport()
        {
            string strParam = string.Empty;
            //string sPono = objRepair.sPurchaseOrderNo;
            string sPono = txtPoNo.Text;
            //string sOilQty = txtquantity.Text;
            string sOilQty = txtoilquantityltr.Text;
            string sOfficecode = objSession.UserId;
            string sAgencyid = cmbAgency.SelectedValue;
            string sLevelOfApproval = getApprovalLevel().ToString();

            if (sLevelOfApproval == "3" || sLevelOfApproval == "4")
            {
                strParam = "id=PurchaseOrderOil&Pono=" + sPono + "&Oilqty=" + sOilQty + "&OfficeCode=" + objSession.OfficeCode + "&AgencyId="+ sAgencyid;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
            }
        }

        protected void txtqty_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string Quantity =txtquantity.Text;
                double Qtyltr = 1000;
                double TotalOilQuantityinLtr = Convert.ToDouble(Quantity) * Qtyltr;
                txtoilquantityltr.Text = Convert.ToString(TotalOilQuantityinLtr);

                double temp = Convert.ToDouble(TotalOilQuantityinLtr) * Convert.ToDouble(AmtRate);

                txtamt.Text = Convert.ToString(temp);

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "txtqty_SelectedIndexChanged");
            }
        }
        public int getApprovalLevel()
        {

            int Level = 0;
            try
            {
                if (objSession.RoleId == "5")
                {
                    Level = 1;
                }
                else if (objSession.RoleId == "2")
                {
                    Level = 2;
                }
                else if (objSession.RoleId == "25")
                {
                    Level = 3;
                }
                else if (objSession.RoleId == "3")
                {
                    Level = 4;
                }
                return Level;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "getApprovalLevel");
                return Level;
            }

        }
        protected void lnkBudgetstat_Click(object sender, EventArgs e)
        {
            try
            {

                string url = "/OilFlow/BudgetStatus.aspx";
               
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