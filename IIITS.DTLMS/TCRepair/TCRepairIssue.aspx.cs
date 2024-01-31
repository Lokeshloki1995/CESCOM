using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Collections;
using System.Globalization;
using IIITS.DAL;

namespace IIITS.DTLMS.TCRepair
{
    public partial class TCRepairIssue : System.Web.UI.Page
    {
        string strFormCode = "TCRepairIssue";
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
                Form.DefaultButton = cmdSave.UniqueID;
                txtPonum.Attributes.Add("readonly", "readonly");
                txtIssueDate.Attributes.Add("readonly", "readonly");
                txtPODate.Attributes.Add("readonly", "readonly");
                txtInvoiceDate.Attributes.Add("readonly", "readonly");
                ddlType.Attributes.Add("readonly", "readonly");

                if (!IsPostBack)
                {
                    Genaral.Load_Combo("select US_OFFICE_CODE , DIV_NAME ||'~'|| US_FULL_NAME from TBLUSER inner join TBLDIVISION on US_OFFICE_CODE=DIV_CODE  where US_ROLE_ID='10' and US_STATUS='A'", "--Select--", cmblochtlt);

                    if (Request.QueryString["StoreId"] != null && Request.QueryString["StoreId"].ToString() != "")
                    {
                        if (Session["Table"] != null)
                        {
                            grdFaultTC.DataSource = Session["Table"];
                            grdFaultTC.DataBind();
                            if (objSession.RoleId == "5")
                            {
                                grdFaultTC.Columns[11].Visible = true;
                            }
                        }
                    }


                    //From DTR Tracker
                    GenerateInvoiceNo();
                    if (Request.QueryString["TransId"] != null && Request.QueryString["TransId"].ToString() != "")
                    {
                        txtRepairMasterId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["TransId"]));
                        GetRepairSentDetails();
                        ddlType_SelectedIndexChanged(sender, e);
                        cmbRepairer.SelectedValue = hdfRepairId.Value;
                        cmbRepairer_SelectedIndexChanged(sender, e);
                        LoadRepairSentDTR();
                    }
                    else
                    {

                        string date = GetLastPricingUpdate(objSession.OfficeCode);
                        //     date = "2020-03-10";
                        // commented this one as i need to take the take from the last three days  
                        if (date.Length != 0)
                        {
                            //CalendarExtender1.StartDate = Convert.ToDateTime(date, System.Globalization.CultureInfo.InvariantCulture).AddDays(1);
                            //  CalendarExtender2.StartDate = Convert.ToDateTime(date, System.Globalization.CultureInfo.InvariantCulture).AddDays(1);
                        }

                    }

                    //end
                    string strQry = string.Empty;

                    strQry = "Title=Search and Select DTC Failure Details&";
                    strQry += "Query=select TC_CODE,TC_SLNO FROM TBLTCMASTER WHERE TC_CODE NOT IN ";
                    strQry += "(SELECT RSD_TC_CODE from TBLREPAIRSENTDETAILS where RSD_DELIVARY_DATE is NULL ) AND TC_STATUS=3 AND  TC_CURRENT_LOCATION=1 AND ";
                    strQry += " TC_LOCATION_ID LIKE '" + objSession.OfficeCode + "%' AND  {0} like %{1}% order by TC_CODE&";
                    strQry += "DBColName=TC_CODE~TC_SLNO&";
                    strQry += "ColDisplayName=DTr Code~DTr SlNo&";
                    strQry = strQry.Replace("'", @"\'");
                    cmdSearchTC.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtTcCode.ClientID + "&btn=" + cmdSearchTC.ClientID + "',520,520," + txtTcCode.ClientID + ")");

                    txtInvoiceDate.Attributes.Add("onblur", "return ValidateDate(" + txtInvoiceDate.ClientID + ");");
                    txtIssueDate.Attributes.Add("onblur", "return ValidateDate(" + txtIssueDate.ClientID + ");");
                    txtPODate.Attributes.Add("onblur", "return ValidateDate(" + txtPODate.ClientID + ");");
                    CheckAccessRights("2");
                    //WorkFlow / Approval
                    WorkFlowConfig();
                }

                if (objSession.RoleId == "2")
                {
                    DTrCODE.Visible = false;
                    MAKE.Visible = false;
                }
                else
                {
                    DTrCODE.Visible = true;
                    MAKE.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load");
            }
        }
        private void clearData()
        {
            cmbRepairer.Items.Clear();
            txtAddress.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtName.Text = string.Empty;

            txtIssueDate.Text = string.Empty;
            txtPONo.Text = string.Empty;
            txtPODate.Text = string.Empty;
            txtInvoiceDate.Text = string.Empty;

            txtRemarks.Text = string.Empty;
            cmblochtlt.Enabled = true;
            cmblochtlt.SelectedIndex = 0;
            if (txtPonum.Text != "")
            {
                cmbRepairer.Enabled = true;
            }
            txtPonum.Text = string.Empty;
            if (cmbGuarantyType.SelectedValue == "WGP")
            {
                grdFaultTC.Columns[9].Visible = false;
                rbdWithOil.Checked = true;
                rbdWithoutOil.Checked = false;
                rbdWithoutOil.Enabled = false;
            }
        }
        protected void cmbGuaranty_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strQry = string.Empty;
            try
            {
                // string GuarantyType = cmbGuarantyType.SelectedValue;
                if (cmbGuarantyType.SelectedIndex > 0)
                {
                    if (cmbGuarantyType.SelectedValue == "WGP")
                    {
                        strQry = "Title=Search and Select Already Entered PO&";
                        strQry += "Query=SELECT UPPER(RSM_PO_NO)RSM_PO_NO FROM TBLREPAIRSENTMASTER WHERE RSM_DIV_CODE = '" + objSession.OfficeCode + "' AND RSM_GUARANTY_TYPE = '" + cmbGuarantyType.SelectedValue + "' AND {0} like %{1}% &";
                        strQry += "DBColName=RSM_PO_NO&";
                        strQry += "ColDisplayName=Purchase Order No&";

                        strQry = strQry.Replace("'", @"\'");

                        cmdSearchPO.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtPonum.ClientID + "&btn=" + cmdSearchPO.ClientID + "',520,520," + txtPonum.ClientID + ")");
                        ddlType.SelectedValue = "1";
                        clearData();

                    }
                    else if (cmbGuarantyType.SelectedValue == "AGP")
                    {
                        strQry = "Title=Search and Select Already Entered PO&";
                        strQry += "Query=SELECT UPPER(RSM_PO_NO)RSM_PO_NO FROM TBLREPAIRSENTMASTER WHERE RSM_DIV_CODE = '" + objSession.OfficeCode + "' AND RSM_GUARANTY_TYPE ='" + cmbGuarantyType.SelectedValue + "' AND {0} like %{1}% &";
                        strQry += "DBColName=RSM_PO_NO&";
                        strQry += "ColDisplayName=Purchase Order No&";

                        strQry = strQry.Replace("'", @"\'");

                        cmdSearchPO.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtPonum.ClientID + "&btn=" + cmdSearchPO.ClientID + "',520,520," + txtPonum.ClientID + ")");
                        ddlType.SelectedValue = "2";
                        clearData();

                    }
                    else if (cmbGuarantyType.SelectedValue == "WRGP")
                    {
                        strQry = "Title=Search and Select Already Entered PO&";
                        strQry += "Query=SELECT UPPER(RSM_PO_NO)RSM_PO_NO FROM TBLREPAIRSENTMASTER WHERE RSM_DIV_CODE = '" + objSession.OfficeCode + "' AND RSM_GUARANTY_TYPE ='" + cmbGuarantyType.SelectedValue + "' AND {0} like %{1}% &";
                        strQry += "DBColName=RSM_PO_NO&";
                        strQry += "ColDisplayName=Purchase Order No&";

                        strQry = strQry.Replace("'", @"\'");

                        cmdSearchPO.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtPonum.ClientID + "&btn=" + cmdSearchPO.ClientID + "',520,520," + txtPonum.ClientID + ")");
                        ddlType.SelectedValue = "2";
                        clearData();


                    }
                }
                else
                {
                    clearData();

                }
                ddlType_SelectedIndexChanged(sender, e);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbGuaranty_SelectedIndexChanged");
            }
        }

        private string GetLastPricingUpdate(string offCode)
        {
            clsCommon objCommon = new clsCommon();
            try
            {
                return objCommon.GetLastPricingDate(offCode);
            }
            catch (Exception ex)
            {

                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetLastPricingUpdate");
                return "Something Went Wrong";
            }
        }

        protected void cmdSearchTC_Click(object sender, EventArgs e)
        {
            try
            {
                clsDtcMaster objDtcMaster = new clsDtcMaster();
                objDtcMaster.sTcCode = txtTcCode.Text;

                objDtcMaster.GetTCDetails(objDtcMaster);

                txtMake.Text = objDtcMaster.sTCMakeName;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdSearchTC_Click");
            }
        }

        protected void cmbRepairer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strQry = string.Empty;
                pnlApproval.Enabled = true;
                cmdSave.Enabled = true;
                if (cmbRepairer.SelectedIndex > 0)
                {
                    if (ddlType.SelectedValue == "2")
                    {
                        clsTransRepairer objRepair = new clsTransRepairer();
                        objRepair.RepairerId = cmbRepairer.SelectedValue;

                        objRepair.GetRepairerDetails(objRepair);

                        txtAddress.Text = objRepair.RegisterAddress;
                        txtName.Text = objRepair.RepairerName;
                        txtPhone.Text = objRepair.RepairerPhoneNo;

                        if (objRepair.sOilType == "1" || (cmbGuarantyType.SelectedValue=="WRGP" && (objRepair.sOilType ?? "").Length >=1))
                        {
                            rbdWithOil.Checked = true;
                            rbdWithoutOil.Checked = false;
                            grdFaultTC.Columns[9].Visible = true;
                            rbdWithoutOil.Enabled = false;
                            SelectedOldPonoIndexChanged(objRepair);
                        }
                        else if (objRepair.sOilType == null || objRepair.sOilType == "")
                        {
                            ShowMsgBox(" There is a problem with selected repairer please contact support team");
                            cmdSave.Enabled = false;
                            pnlApproval.Enabled = false;
                            return;
                        }
                        else
                        {
                            grdFaultTC.Columns[9].Visible = true;
                            rbdWithOil.Checked = false;
                            rbdWithOil.Enabled = false;
                            rbdWithoutOil.Checked = true;
                            rbdWithoutOil.Enabled = true;
                            divOil.Visible = false;
                            SelectedOldPonoIndexChanged(objRepair);
                        }
                    }
                    else
                    {
                        clsTransSupplier objSupplier = new clsTransSupplier();
                        objSupplier.SupplierId = cmbRepairer.SelectedValue;

                        objSupplier.GetSupplierDetails(objSupplier);

                        txtAddress.Text = objSupplier.RegisterAddress;
                        txtName.Text = objSupplier.SupplierName;
                        txtPhone.Text = objSupplier.SupplierPhoneNo;
                    }
                    if (Request.QueryString["ActionType"] != null && Request.QueryString["ActionType"].ToString() != "")
                    {

                    }
                    else
                    {
                        //if (Request.QueryString["TransId"] == null && Request.QueryString["TransId"].ToString() == "")
                        if ((Request.QueryString["TransId"] ?? "").Length == 0)
                        {
                            ResetSelectedPoNoDetails();
                        }
                        else
                        {
                            cmdSave.Enabled = false;
                            pnlApproval.Enabled = false;
                            return;
                        }
                    }
                }
                else
                {
                    txtAddress.Text = string.Empty;
                    txtPhone.Text = string.Empty;
                    txtName.Text = string.Empty;
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbRepairer_SelectedIndexChanged");
            }
        }

        protected void rbdNewPo_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtPONo.Enabled = true;
                txtPODate.Enabled = true;
                cmblochtlt.Enabled = true;
                ResetSelectedPoNoDetails();

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
                txtPONo.Enabled = false;
                txtPODate.Enabled = false;
                // cmblochtlt.Enabled = false;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "rbdOldPo_CheckedChanged");
            }
        }

        public void LoadFaultTc()
        {
            try
            {
                DataTable dt = new DataTable();
                clsDTrRepairActivity objRepair = new clsDTrRepairActivity();
                txtSelectedTcId.Text = txtSelectedTcId.Text.Replace("~", ",");
                if (!txtSelectedTcId.Text.StartsWith(","))
                {
                    txtSelectedTcId.Text = "," + txtSelectedTcId.Text;
                }
                if (!txtSelectedTcId.Text.EndsWith(","))
                {
                    txtSelectedTcId.Text = txtSelectedTcId.Text + ",";
                }

                txtSelectedTcId.Text = txtSelectedTcId.Text.Substring(1, txtSelectedTcId.Text.Length - 1);
                txtSelectedTcId.Text = txtSelectedTcId.Text.Substring(0, txtSelectedTcId.Text.Length - 1);

                objRepair.sTcId = txtSelectedTcId.Text;
                dt = objRepair.LoadFaultTC(objRepair);
                grdFaultTC.DataSource = dt;
                ViewState["FaultTC"] = dt;
                grdFaultTC.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadFaultTc");
            }
        }

        public void LoadRepairSentDTR()
        {
            try
            {
                DataTable dt = new DataTable();
                clsDTrRepairActivity objRepair = new clsDTrRepairActivity();

                dt = objRepair.LoadRepairSentDTR(txtRepairMasterId.Text, hdfNthTime.Value);
                grdFaultTC.DataSource = dt;
                ViewState["FaultTC"] = dt;
                grdFaultTC.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadFaultTc");
            }
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                clsDTrRepairActivity objTcRepair = new clsDTrRepairActivity();
                string[] Arr = new string[2];

                //Check AccessRights
                bool bAccResult = CheckAccessRights("2");
                if (bAccResult == false)
                {
                    return;
                }

                if (ValidateForm() == true)
                {
                    objTcRepair.sSupRepId = cmbRepairer.SelectedValue;
                    objTcRepair.sInvoiceDate = txtInvoiceDate.Text;
                    objTcRepair.sInvoiceNo = txtInvoiceNo.Text;
                    objTcRepair.sManualInvoiceNo = txtInvoiceNo.Text;
                    objTcRepair.sIssueDate = txtIssueDate.Text;
                    objTcRepair.sPurchaseDate = txtPODate.Text;
                    objTcRepair.sPurchaseOrderNo = txtPONo.Text;
                    objTcRepair.sCrby = objSession.UserId;
                    objTcRepair.sStoreId = txtStoreId.Text;
                    objTcRepair.sOfficeCode = objSession.OfficeCode;
                    objTcRepair.sGuarantyType = cmbGuarantyType.SelectedValue;
                    objTcRepair.sOldPONo = txtPonum.Text;
                    objTcRepair.sPORemarks = txtRemarks.Text;
                    if (cmblochtlt.SelectedIndex != 0)
                    {
                        objTcRepair.slochtlt = cmblochtlt.SelectedValue;
                    }
                    else
                    {
                        objTcRepair.slochtlt = objSession.OfficeCode;
                    }
                    if (txtOilQnty.Text != "")
                    {
                        //double num = Convert.ToInt32(txtOilQnty.Text);
                        //num = num / 1000 ;

                        objTcRepair.sOilQty = txtOilQnty.Text;
                    }

                    objTcRepair.sType = ddlType.SelectedValue;

                    if (rbdOldPo.Checked == true)
                    {
                        objTcRepair.sIsOldPo = true;
                    }
                    else
                    {
                        objTcRepair.sIsOldPo = false;
                    }


                    //return;
                    if (txtActiontype.Text == "A" || txtActiontype.Text == "R")
                    {
                        if (hdfWFDataId.Value != "0")
                        {
                            ApproveRejectAction();
                            if (objSession.sTransactionLog == "1")
                            {
                                Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "RepairTransaction");
                            }
                            return;
                        }
                    }

                    //To check Selected Transformers Already Sent for Supplier/Repair and Waiting For Approval
                    clsApproval objApproval = new clsApproval();


                    int i = 0;
                    string[] strQryVallist = new string[grdFaultTC.Rows.Count];
                    bool bDataExist = false;
                    foreach (GridViewRow row in grdFaultTC.Rows)
                    {
                        string sAmount = string.Empty;
                        sAmount = ((Label)row.FindControl("lblAmount")).Text.Trim();
                        if (rbdWithOil.Checked == true)
                        {
                            sAmount = "";
                        }
                        strQryVallist[i] = ((Label)row.FindControl("lblTCCode")).Text.Trim() + "~" + ((Label)row.FindControl("lblGuarantyType")).Text.Trim() + "~" + ((Label)row.FindControl("lblItemId")).Text.Trim() + "~" + sAmount;
                        i++;
                        objTcRepair.sQty = Convert.ToString(grdFaultTC.Rows.Count);
                        string sTCCode = ((Label)row.FindControl("lblTCCode")).Text.Trim();


                        bDataExist = true;
                    }

                    if (bDataExist == false)
                    {
                        ShowMsgBox("No Transformer Exists to Issue for Repairer/Supplier");
                        return;
                    }


                    //Workflow
                    WorkFlowObjects(objTcRepair);



                    Arr = objTcRepair.SaveRepairIssueDetails(strQryVallist, objTcRepair);
                    if (Arr[1].ToString() == "0")
                    {
                        //ShowMsgBox(Arr[0].ToString());

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), UniqueID, "alert('Transformers Issued Sucessfully to Repairer/Supplier'); location.href='FaultTCSearch.aspx';", true);
                        Reset();
                        txtInvoiceNo.Text = string.Empty;
                        txtTcCode.Text = string.Empty;
                        txtMake.Text = string.Empty;
                        grdFaultTC.DataSource = null;
                        grdFaultTC.DataBind();
                        ViewState["FaultTC"] = null;
                        txtSelectedTcId.Text = string.Empty;
                        cmdGatePass.Enabled = true;
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "RepairTransaction");
                        }
                        return;
                    }
                    else if (Arr[1].ToString() == "2")
                    {
                        ShowMsgBox(Arr[0].ToString());
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "RepairTransaction");
                        }
                        return;
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdSave_Click");
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

        public void AddTCtoGrid(string sTcCode)
        {
            try
            {
                clsDTrRepairActivity objTCRepair = new clsDTrRepairActivity();

                if (ValidateGridValue(sTcCode) == true)
                {
                    objTCRepair.sTcCode = sTcCode;
                    objTCRepair.AddFaultTCDetails(objTCRepair);

                    if (ViewState["FaultTC"] != null)
                    {
                        DataTable dtFaultTc = (DataTable)ViewState["FaultTC"];
                        DataRow drow;
                        if (objTCRepair.sTcId != null)
                        {
                            if (dtFaultTc.Rows.Count > 0)
                            {
                                drow = dtFaultTc.NewRow();

                                drow["TC_ID"] = objTCRepair.sTcId;
                                drow["TC_CODE"] = objTCRepair.sTcCode;
                                drow["TC_SLNO"] = objTCRepair.sTcSlno;
                                drow["TM_NAME"] = objTCRepair.sMakeName;
                                drow["TC_CAPACITY"] = objTCRepair.sCapacity;
                                drow["TC_MANF_DATE"] = objTCRepair.sManfDate;
                                drow["TC_PURCHASE_DATE"] = objTCRepair.sPurchaseDate;
                                drow["TC_WARANTY_PERIOD"] = objTCRepair.sWarrantyPeriod;
                                drow["TS_NAME"] = objTCRepair.sSupplierName;
                                drow["TC_GUARANTY_TYPE"] = objTCRepair.sGuarantyType;

                                dtFaultTc.Rows.Add(drow);
                                grdFaultTC.DataSource = dtFaultTc;
                                grdFaultTC.DataBind();
                                ViewState["FaultTC"] = dtFaultTc;
                            }
                        }

                        //ShowMsgBox("TC is not in Store or Good Condition");
                        //txtTcCode.Text = "";
                    }
                    else
                    {
                        DataTable dtFaultTc = new DataTable();
                        DataRow drow;

                        dtFaultTc.Columns.Add(new DataColumn("TC_ID"));
                        dtFaultTc.Columns.Add(new DataColumn("TC_CODE"));
                        dtFaultTc.Columns.Add(new DataColumn("TC_SLNO"));
                        dtFaultTc.Columns.Add(new DataColumn("TM_NAME"));
                        dtFaultTc.Columns.Add(new DataColumn("TC_CAPACITY"));
                        dtFaultTc.Columns.Add(new DataColumn("TC_MANF_DATE"));
                        dtFaultTc.Columns.Add(new DataColumn("TC_PURCHASE_DATE"));
                        dtFaultTc.Columns.Add(new DataColumn("TC_WARANTY_PERIOD"));
                        dtFaultTc.Columns.Add(new DataColumn("TS_NAME"));
                        dtFaultTc.Columns.Add(new DataColumn("TC_GUARANTY_TYPE"));

                        drow = dtFaultTc.NewRow();

                        drow["TC_ID"] = objTCRepair.sTcId;
                        drow["TC_CODE"] = objTCRepair.sTcCode;
                        drow["TC_SLNO"] = objTCRepair.sTcSlno;
                        drow["TM_NAME"] = objTCRepair.sMakeName;
                        drow["TC_CAPACITY"] = objTCRepair.sCapacity;
                        drow["TC_MANF_DATE"] = objTCRepair.sManfDate;
                        drow["TC_PURCHASE_DATE"] = objTCRepair.sPurchaseDate;
                        drow["TC_WARANTY_PERIOD"] = objTCRepair.sWarrantyPeriod;
                        drow["TS_NAME"] = objTCRepair.sSupplierName;
                        drow["TC_GUARANTY_TYPE"] = objTCRepair.sGuarantyType;

                        dtFaultTc.Rows.Add(drow);
                        grdFaultTC.DataSource = dtFaultTc;
                        grdFaultTC.DataBind();
                        ViewState["FaultTC"] = dtFaultTc;

                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "AddTCtoGrid");
            }
        }

        public bool ValidateGridValue(string sTcCode)
        {
            bool bValidate = false;
            try
            {
                ArrayList objArrlist = new ArrayList();

                foreach (GridViewRow row in grdFaultTC.Rows)
                {
                    objArrlist.Add(((Label)row.FindControl("lblTCCode")).Text.Trim());
                }

                if (objArrlist.Contains(sTcCode))
                {
                    ShowMsgBox("Transformer Already Added");
                    DataTable dtFaultTc = (DataTable)ViewState["FaultTC"];
                    grdFaultTC.DataSource = dtFaultTc;
                    grdFaultTC.DataBind();
                    return bValidate;
                }

                bValidate = true;
                return bValidate;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ValidGridValue");
                return bValidate;
            }
        }

        protected void cmdLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtTcCode.Text.Trim() == "")
                {
                    ShowMsgBox("Select Transformer Code");
                    return;
                }
                int flag = CheckRICompletionOfDTR(txtTcCode.Text);
                if (flag == 1)
                {
                    AddTCtoGrid(txtTcCode.Text);
                }
                else
                {
                    ShowMsgBox("DTR may not have completed RI ");
                    return;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdLoad_Click");
            }
        }

        protected int CheckRICompletionOfDTR(string stccode)
        {
            int flag = 0;
            try
            {
                clsTcMaster objTcMaster = new clsTcMaster();
                objTcMaster.sTcCode = stccode;
                flag = objTcMaster.CheckRICompletionOfDTR(objTcMaster);
                return flag;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "CheckRICompetionOfDTR");
                return flag;
            }
        }

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                Reset();
                //txtInvoiceNo.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdReset_Click");
            }
        }
        public void Reset()
        {
            try
            {
                if (cmbRepairer.SelectedIndex > 0)
                {
                    cmbRepairer.SelectedIndex = 0;
                }
                txtAddress.Text = string.Empty;
                txtInvoiceDate.Text = string.Empty;
                //txtInvoiceNo.Text = string.Empty;
                txtIssueDate.Text = string.Empty;
                txtName.Text = string.Empty;
                txtPhone.Text = string.Empty;
                txtPODate.Text = string.Empty;
                txtPONo.Text = string.Empty;
                txtTcCode.Text = string.Empty;
                txtMake.Text = string.Empty;
                hdfTccode.Value = string.Empty;
                cmbGuarantyType.SelectedIndex = 0;
                ddlType.SelectedIndex = 0;
                rbdNewPo.Checked = false;
                rbdOldPo.Checked = false;
                cmblochtlt.SelectedIndex = 0;
                txtPonum.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Reset");
            }
        }

        public bool ValidateForm()
        {
            bool bValidate = false;
            try
            {
                if (divOil.Visible == true)
                {
                    if (txtOilQnty.Text.Trim() == "")
                    {
                        ShowMsgBox("Please Enter the Oil Quantity");
                        txtOilQnty.Focus();
                        return bValidate;
                    }
                    if (!(txtOilQnty.Text.Contains(".")))
                    {
                        ShowMsgBox("Please Enter the oil quantity in KiloLiters Eg :- 63 liters -> .063 klts");
                        return bValidate;
                    }
                }
                if (rbdNewPo.Checked == false && rbdOldPo.Checked == false)
                {
                    ShowMsgBox("Please Select whether its a Old Po or New Po");
                    rbdNewPo.Focus();
                    return bValidate;
                }
                if (cmbGuarantyType.SelectedIndex == 0)
                {
                    ShowMsgBox("Select Guaranty Type");
                    cmbGuarantyType.Focus();
                    return bValidate;
                }
                if (ddlType.SelectedIndex == 0)
                {
                    ShowMsgBox("Select Type(Repairer/Supplier)");
                    ddlType.Focus();
                    return bValidate;
                }

                if (cmbRepairer.SelectedIndex == 0)
                {
                    ShowMsgBox("Select Repairer / Supplier");
                    cmbRepairer.Focus();
                    return bValidate;
                }
                if (txtIssueDate.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Valid Issue Date");
                    txtIssueDate.Focus();
                    return bValidate;
                }
                if (txtInvoiceDate.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Valid Invoice Date");
                    txtInvoiceDate.Focus();
                    return bValidate;
                }
                // if (ddlType.SelectedValue != "1" && cmbGuarantyType.SelectedValue == "WGP")
                //  {
                if (cmblochtlt.SelectedIndex == 0)
                {
                    ShowMsgBox("Select Inspection Done By");
                    cmblochtlt.Focus();
                    return bValidate;
                }

                string sResult = Genaral.DateComparision(txtIssueDate.Text, txtPODate.Text, false, false);
                if (sResult == "2")
                {
                    ShowMsgBox("Issue Date should be greater than PODate");
                    return bValidate;
                }
                sResult = Genaral.DateComparision(txtInvoiceDate.Text, txtPODate.Text, false, false);
                if (sResult == "2")
                {
                    ShowMsgBox("Invoice Date should be greater than PODate");
                    return bValidate;
                }
                if (txtPONo.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Valid Purchase Order No");
                    txtPONo.Focus();
                    return bValidate;
                }
                if (txtPODate.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Valid Purchase Order Date");
                    txtPODate.Focus();
                    return bValidate;
                }

                if (txtInvoiceNo.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Valid Invoice No.");
                    txtInvoiceNo.Focus();
                    return bValidate;
                }
                if (txtInvoiceDate.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Valid Invoice Date");
                    txtInvoiceDate.Focus();
                    return bValidate;
                }
                if (ddlType.SelectedValue == "2")
                {
                    if (rbdWithoutOil.Checked == true)
                    {
                        foreach (GridViewRow row in grdFaultTC.Rows)
                        {
                            string sOilQuantity = ((Label)row.FindControl("lblAmount")).Text;
                            string sDtrcode = ((Label)row.FindControl("lblTCCode")).Text;

                            if (sOilQuantity == "")
                            {
                                ShowMsgBox("Oil Quntity Is Null Cant Send To Repairer Please Contact Support Team.");
                                return bValidate;
                            }
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


        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (ddlType.SelectedIndex > 0)
                {

                    // check for WGP 

                    if (cmbGuarantyType.SelectedValue == "WGP")
                    {
                        if (ddlType.SelectedValue != "1")
                        {
                            ShowMsgBox("Cannot Send WGP transformers to Repairer ");
                            ddlType.SelectedIndex = 0;
                            return;
                        }

                    }
                    //AGP
                    if (cmbGuarantyType.SelectedValue == "AGP")
                    {

                        if (ddlType.SelectedValue != "2")
                        {
                            ShowMsgBox("Cannot Send AGP transformers to Supplier ");
                            ddlType.SelectedIndex = 0;
                            return;
                        }

                    }
                    //WRGP
                    if (cmbGuarantyType.SelectedValue == "WRGP")
                    {
                        if (ddlType.SelectedValue != "2")
                        {
                            ShowMsgBox("Cannot Send WRGP transformers to Supplier ");
                            ddlType.SelectedIndex = 0;
                            return;
                        }

                    }

                    if (ddlType.SelectedValue == "2")
                    {
                        string strQry = string.Empty;

                        // new one  
                        strQry = "SELECT A.TR_ID , (SELECT TR_NAME FROM TBLTRANSREPAIRER WHERE TR_ID = A.TR_TR_ID )  ||'~'|| A.TR_NAME as TR_NAME  FROM  (SELECT ";
                        strQry += " TR_ID  ,TR_TR_ID  ,(SELECT DIV_NAME FROM TBLDIVISION WHERE DIV_CODE = TR_OFFICECODE )   ||  ' DIVISION----'  ||  (SELECT ";
                        strQry += " TQ_NAME FROM TBLTALQ WHERE TQ_SLNO = TR_LOC_CODE AND ROWNUM <= 1) || ' TALUK' TR_NAME  FROM TBLTRANSREPAIRER   WHERE TR_STATUS";
                        strQry += "  = 'A' AND TR_LOC_CODE <> 0  AND  TR_ID  NOT IN  (SELECT TR_ID  FROM TBLTRANSREPAIRER  WHERE TR_BLACK_LISTED  =1 AND  ";
                        strQry += " TR_BLACKED_UPTO >=SYSDATE )  AND TR_OFFICECODE  LIKE '" + objSession.OfficeCode + "%' ORDER BY  TR_OFFICECODE )A ";

                        Genaral.Load_Combo(strQry, "--Select--", cmbRepairer);



                        lblSuppRep.Text = "Repairer";
                        //clearData();
                        txtAddress.Text = string.Empty;
                        txtPhone.Text = string.Empty;
                        txtName.Text = string.Empty;
                    }
                    else if (ddlType.SelectedValue == "1")
                    {
                        Genaral.Load_Combo("SELECT TS_ID,TS_NAME  FROM TBLTRANSSUPPLIER  WHERE TS_STATUS='A' AND TS_ID NOT IN (SELECT TS_ID FROM TBLTRANSSUPPLIER WHERE TS_BLACK_LISTED=1 AND TS_BLACKED_UPTO>=SYSDATE) ORDER BY TS_NAME", "--Select--", cmbRepairer);
                        lblSuppRep.Text = "Supplier";
                        // clearData();
                        txtAddress.Text = string.Empty;
                        txtPhone.Text = string.Empty;
                        txtName.Text = string.Empty;
                    }
                }
                else
                {
                    clearData();

                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbGuarantyType_SelectedIndexChanged");
            }
        }


        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "FaultTCSearch";
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

        protected void grdFaultTC_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Remove")
                {
                    //GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    //int iRowIndex = row.RowIndex;

                    //DataTable dt = (DataTable)ViewState["FaultTC"]; 
                    //dt.Rows[iRowIndex].Delete();
                    //if (dt.Rows.Count == 0)
                    //{
                    //    ViewState["FaultTC"] = null;
                    //}
                    //else
                    //{
                    //    ViewState["FaultTC"] = dt;
                    //}

                    //grdFaultTC.DataSource = dt;
                    //grdFaultTC.DataBind();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    Label sTcId = (Label)row.FindControl("lblTCId");
                    DataTable dt = (DataTable)Session["Table"];

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (sTcId.Text == Convert.ToString(dt.Rows[i]["TC_ID"]))
                        {
                            dt.Rows[i].Delete();
                            dt.AcceptChanges();
                        }
                    }
                    dt.AcceptChanges();
                    grdFaultTC.DataSource = dt;
                    grdFaultTC.DataBind();
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdFaultTC_RowCommand");
            }
        }

        public void GenerateInvoiceNo()
        {
            try
            {
                clsInvoice objInvoice = new clsInvoice();

                txtInvoiceNo.Text = objInvoice.GenerateInvoiceNo(objSession.OfficeCode);
                txtInvoiceNo.ReadOnly = true;
                hdfInvoiceNo.Value = txtInvoiceNo.Text;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GenerateInvoiceNo");
            }
        }

        bool ValidateGatePass()
        {
            bool bValidate = false;

            try
            {

                if (txtVehicleNo.Text.Length == 0)
                {
                    txtVehicleNo.Focus();
                    ShowMsgBox("Enter Vehicle No");
                    return bValidate;
                }
                if (txtChallen.Text.Length == 0)
                {
                    txtChallen.Focus();
                    ShowMsgBox("Enter Challen Number");
                    return bValidate;
                }

                if (txtReciepient.Text.Trim().Length == 0)
                {
                    txtReciepient.Focus();
                    ShowMsgBox("Enter Reciepient Name");
                    return bValidate;
                }



                bValidate = true;
                return bValidate;
            }
            catch (Exception ex)
            {
                bValidate = false;
                return bValidate;
            }
        }

        protected void cmdGatePass_Click(object sender, EventArgs e)
        {
            try
            {
                clsInvoice objInvoice = new clsInvoice();
                string[] Arr = new string[2];
                if (ValidateGatePass() == true)
                {
                    objInvoice.sGatePassId = txtGpId.Text;
                    objInvoice.sVehicleNumber = txtVehicleNo.Text.Replace("'", "");
                    objInvoice.sReceiptientName = txtReciepient.Text.Replace("'", "");
                    objInvoice.sChallenNo = txtChallen.Text.Replace("'", "");
                    objInvoice.sCreatedBy = objSession.UserId;
                    //objInvoice.sTcCode = txtTCCode.Text.Replace("'", "");

                    objInvoice.sInvoiceNo = hdfInvoiceNo.Value.Replace("'", "");

                    Arr = objInvoice.SaveUpdateGatePassDetails(objInvoice);


                    clsDTrRepairActivity objRepair = new clsDTrRepairActivity();
                    objRepair.sPurchaseOrderNo = txtPONo.Text;
                    objRepair.sManualInvoiceNo = txtInvoiceNo.Text;
                    objRepair.sOfficeCode = objSession.OfficeCode;

                    objRepair.UpdateGatePassMMS(objInvoice, objRepair);


                    if (Arr[1].ToString() == "0")
                    {
                        txtGpId.Text = objInvoice.sGatePassId;
                        string strParam = "id=RepairGatepass&InvoiceId=" + hdfInvoiceNo.Value;
                        RegisterStartupScript("PrintD", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

                        return;
                    }
                    if (Arr[1].ToString() == "1")
                    {
                        string strParam = "id=RepairGatepass&InvoiceId=" + txtInvoiceNo.Text;
                        RegisterStartupScript("PrintD", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

                        return;
                    }

                    if (Arr[1].ToString() == "2")
                    {
                        ShowMsgBox(Arr[0]);
                        return;
                    }
                }


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdGatePass_Click");

            }
        }



        public void GetRepairSentDetails()
        {
            try
            {
                clsDTrRepairActivity objRepair = new clsDTrRepairActivity();
                objRepair.sRepairMasterId = txtRepairMasterId.Text;

                objRepair.GetRepairSentDetails(objRepair);

                cmbGuarantyType.SelectedValue = objRepair.sGuarantyType;
                ddlType.SelectedValue = objRepair.sType;

                hdfRepairId.Value = objRepair.sSupRepId;
                hdfNthTime.Value = objRepair.sNthTime;

                txtIssueDate.Text = objRepair.sIssueDate;
                txtPONo.Text = objRepair.sPurchaseOrderNo;
                txtPODate.Text = objRepair.sPurchaseDate;
                txtInvoiceNo.Text = objRepair.sInvoiceNo;
                txtInvoiceDate.Text = objRepair.sInvoiceDate;
                //    txtManualInvoiceNo.Text = objRepair.sManualInvoiceNo;
                txtPonum.Text = objRepair.sOldPONo;
                txtRemarks.Text = objRepair.sPORemarks;
                cmblochtlt.SelectedValue = objRepair.slochtlt;
                hdfInvoiceNo.Value = objRepair.sInvoiceNo;
                if (objRepair.sIsOldPo == true)
                {
                    rbdOldPo.Checked = true;
                }
                else
                {
                    rbdNewPo.Checked = true;
                }
                if (objRepair.sOilQty != "")
                {
                    rbdWithOil.Checked = true;
                    rbdWithoutOil.Checked = false;
                    divOil.Visible = true;
                    txtOilQnty.Text = objRepair.sOilQty;
                    txtOilQnty.Enabled = false;
                }
                rbdNewPo.Enabled = false;
                rbdOldPo.Enabled = false;
                cmdSave.Enabled = false;
                cmdGatePass.Enabled = true;

                cmdReset.Enabled = false;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetRepairSentDetails");

            }
        }

        public void WorkFlowObjects(clsDTrRepairActivity objDTRRepair)
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


                objDTRRepair.sFormName = "TCRepairIssue";
                objDTRRepair.sOfficeCode = objSession.OfficeCode;
                objDTRRepair.sClientIP = sClientIP;
                objDTRRepair.sWFObjectId = hdfWFOId.Value;
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
                    cmdSave.Enabled = true;
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

                if (objSession.RoleId == "2")
                {
                    dvComments.Style.Add("display", "block");
                }
                //cmdReset.Enabled = false;

                if (hdfWFOAutoId.Value != "0")
                {
                    cmdSave.Text = "Save";

                }

                // Check for Creator of Form
                bool bResult = CheckFormCreatorLevel();
                if (bResult == true)
                {
                    cmdSave.Text = "Save";
                    pnlApproval.Enabled = true;
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
            CustOledbConnection objconn = new CustOledbConnection(Constants.Password);

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
                objApproval.sWFDataId = hdfWFDataId.Value;


                bool bResult = objApproval.ApproveWFRequest1(objApproval);
                if (bResult == true)
                {

                    if (objApproval.sApproveStatus == "1")
                    {
                        ShowMsgBox("Approved Successfully");

                        if (objSession.RoleId == "2")
                        {
                            clsRIApproval objRI = new clsRIApproval();
                            // objRI.SendSMStoSectionOfficer(txtDtrCode.Text, txtDecommId.Text, txtFailureId.Text);

                        }


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
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdApprove_Click");
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

                        Session["WFDataId"] = null;
                        Session["WFOId"] = null;
                        Session["WFOAutoId"] = null;
                    }

                    //if (hdfWFDataId.Value != "0")
                    //{
                    //    GetRIDetailsFromXML(hdfWFDataId.Value);
                    //}
                    SetControlText();
                    if (txtActiontype.Text == "A")
                    {
                        cmbGuarantyType.Enabled = false;
                        ddlType.Enabled = false;
                        cmbRepairer.Enabled = false;
                        cmblochtlt.Enabled = false;
                    }
                    if (txtActiontype.Text == "V")
                    {
                        cmdSave.Text = "View";
                        dvComments.Style.Add("display", "none");

                    }
                }
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
                string sResult = objApproval.GetFormCreatorLevel("", objSession.RoleId, "TCRepairIssue");
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
                    Response.Redirect("FaultTCSearch.aspx", false);
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdClose_Click");
            }
        }

        protected void cmdSearchPO_Click(object sender, EventArgs e)
        {
            try
            {
                string[] Arr = new string[5];
                clsDtcMaster objDtcMaster = new clsDtcMaster();
                if (cmbGuarantyType.SelectedIndex == 0)
                {
                    ShowMsgBox("Please select guarantee type");
                    return;
                }
                else if (ddlType.SelectedIndex == 0)
                {
                    txtPonum.Text = string.Empty;
                    ShowMsgBox("Please select type");
                    return;
                }
                //objDtcMaster.sTcCode = txtPonum.Text;
                //String GuarantyType = ViewState["GuarantyType"].ToString();
                Arr = objDtcMaster.GetPONo(txtPonum.Text, objSession.OfficeCode);
                if (Arr[1] != "1")
                {
                    ShowMsgBox(Arr[0]);
                    txtPonum.Text = "";
                    return;
                }
                else
                {
                    if (CheckPoDate(Arr[2]) == 0)
                    {
                        ShowMsgBox("PO DATE  IS OLDER THAN YEAR");
                        Reset();
                    }
                    else
                    {
                        txtPONo.Text = Arr[0];
                        txtPODate.Text = Arr[2];
                        rbdOldPo.Checked = true;
                        rbdNewPo.Checked = false;
                        txtPONo.Enabled = false;
                        cmblochtlt.SelectedValue = Arr[3];
                        cmblochtlt.Enabled = false;
                        cmbRepairer.SelectedValue = Arr[4];
                        cmbRepairer.Enabled = false;

                        clsTransRepairer objRepair = new clsTransRepairer();
                        objRepair.RepairerId = cmbRepairer.SelectedValue;

                        objRepair.GetRepairerDetails(objRepair);

                        txtAddress.Text = objRepair.RegisterAddress;
                        txtName.Text = objRepair.RepairerName;
                        txtPhone.Text = objRepair.RepairerPhoneNo;

                        if (objRepair.sOilType == "1")
                        {
                            rbdWithOil.Checked = true;
                            rbdWithoutOil.Checked = false;
                            grdFaultTC.Columns[9].Visible = true;
                            rbdWithoutOil.Enabled = false;
                            SelectedOldPonoIndexChanged(objRepair);
                        }
                        else
                        {
                            grdFaultTC.Columns[9].Visible = true;
                            rbdWithOil.Checked = false;
                            rbdWithOil.Enabled = false;
                            rbdWithoutOil.Checked = true;
                            rbdWithoutOil.Enabled = true;
                            divOil.Visible = false;
                            SelectedOldPonoIndexChanged(objRepair);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdSearchTC_Click");
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

        protected void rbdWithoutOil_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                divOil.Visible = false;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "rbdWithoutOil_CheckedChanged");

            }

        }

        protected void rbdWithOil_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                divOil.Visible = false;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "rbdWithOil_CheckedChanged");

            }
        }
        /// <summary>
        /// This Method is used for Select old po number
        /// Coded by Sandeep on 26-12-2022
        /// </summary>
        /// <param name="objRepair"></param>
        public void SelectedOldPonoIndexChanged(clsTransRepairer objRepair)
        {
            string strQry = string.Empty;
            try
            {

                if (cmbGuarantyType.SelectedValue == "WGP")
                {
                    strQry = "Title=Search and Select Already Entered PO&";
                    strQry += "Query=SELECT UPPER(RSM_PO_NO)RSM_PO_NO FROM TBLREPAIRSENTMASTER WHERE RSM_DIV_CODE = '" + objSession.OfficeCode + "' AND RSM_GUARANTY_TYPE = '" + cmbGuarantyType.SelectedValue + "' AND {0} like %{1}% &";
                    strQry += "DBColName=RSM_PO_NO&";
                    strQry += "ColDisplayName=Purchase Order No&";

                    strQry = strQry.Replace("'", @"\'");

                    cmdSearchPO.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtPonum.ClientID + "&btn=" + cmdSearchPO.ClientID + "',520,520," + txtPonum.ClientID + ")");
                }
                else if (cmbGuarantyType.SelectedValue == "AGP")
                {
                    strQry = "Title=Search and Select Already Entered PO&";
                    strQry += "Query=SELECT UPPER(RSM_PO_NO)RSM_PO_NO FROM TBLREPAIRSENTMASTER WHERE RSM_DIV_CODE = '" + objSession.OfficeCode + "' AND RSM_GUARANTY_TYPE ='" + cmbGuarantyType.SelectedValue + "' ";
                    if (objRepair.sOilType != "1")
                    {
                        strQry += " AND RSM_OIL_TYPE = '0' ";
                    }
                    else
                    {
                        strQry += "AND (RSM_OIL_TYPE is null or RSM_OIL_TYPE = 1)";
                    }
                    strQry += " AND {0} like %{1}% &";
                    strQry += "DBColName=RSM_PO_NO&";
                    strQry += "ColDisplayName=Purchase Order No&";

                    strQry = strQry.Replace("'", @"\'");

                    cmdSearchPO.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtPonum.ClientID + "&btn=" + cmdSearchPO.ClientID + "',520,520," + txtPonum.ClientID + ")");

                }
                else if (cmbGuarantyType.SelectedValue == "WRGP")
                {
                    strQry = "Title=Search and Select Already Entered PO&";
                    strQry += "Query=SELECT UPPER(RSM_PO_NO)RSM_PO_NO FROM TBLREPAIRSENTMASTER WHERE RSM_DIV_CODE = '" + objSession.OfficeCode + "' AND RSM_GUARANTY_TYPE ='" + cmbGuarantyType.SelectedValue + "'";
                    if (objRepair.sOilType != "1")
                    {
                        strQry += " AND RSM_OIL_TYPE = '0' ";
                    }
                    else
                    {
                        strQry += "AND (RSM_OIL_TYPE is null or RSM_OIL_TYPE = 1)";
                    }
                    strQry += " AND {0} like %{1}% &";
                    strQry += "DBColName=RSM_PO_NO&";
                    strQry += "ColDisplayName=Purchase Order No&";

                    strQry = strQry.Replace("'", @"\'");

                    cmdSearchPO.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtPonum.ClientID + "&btn=" + cmdSearchPO.ClientID + "',520,520," + txtPonum.ClientID + ")");
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "rbdWithOil_CheckedChanged");

            }
        }

        public void ResetSelectedPoNoDetails()
        {
            try
            {
                txtIssueDate.Text = string.Empty;
                txtPONo.Text = string.Empty;
                txtPODate.Text = string.Empty;
                txtInvoiceDate.Text = string.Empty;

                txtRemarks.Text = string.Empty;
                cmblochtlt.Enabled = true;
                cmblochtlt.SelectedIndex = 0;
                if (txtPonum.Text != "")
                {
                    cmbRepairer.SelectedIndex = 0;
                    cmbRepairer.Enabled = true;
                    txtAddress.Text = string.Empty;
                    txtName.Text = string.Empty;
                    txtPhone.Text = string.Empty;
                }
                txtPonum.Text = string.Empty;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ResetSelectedPoNoDetails");
            }
        }



    }
}
