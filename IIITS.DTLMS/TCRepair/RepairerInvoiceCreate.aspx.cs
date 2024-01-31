using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using IIITS.DTLMS.BL;
using System.Collections;
using System.Configuration;

namespace IIITS.DTLMS.TCRepair
{
    public partial class RepairerInvoiceCreate : System.Web.UI.Page
    {
        string strFormCode = "RepairerInvoiceCreate";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)

        {
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                lblMessage.Text = string.Empty;
                objSession = (clsSession)Session["clsSession"];
                txtInvoiceDate.Attributes.Add("readonly", "readonly");
                //txtPONo.Attributes.Add("readonly", "readonly");
                CalendarExtender1.EndDate = DateTime.Now;
                LoadInvoicedOilDetails();

                if (!IsPostBack)
                {


                    if (CheckAccessRights("4"))
                    {
                        cmbRepairer.Enabled = false;
                        string strQry = string.Empty;
                        strQry = "SELECT TR_ID,TR_NAME FROM TBLTRANSREPAIRER WHERE TR_STATUS = 'A' AND TR_ID NOT IN (SELECT TR_ID FROM TBLTRANSREPAIRER ";
                        strQry += " WHERE TR_BLACK_LISTED = 1 AND TR_BLACKED_UPTO >= SYSDATE) ORDER BY TR_NAME ";
                        Genaral.Load_Combo(strQry, "--Select--", cmbRepairer);
                        if (Request.QueryString["ActionType"] != null && Request.QueryString["ActionType"].ToString() != "")
                        {
                            txtActiontype.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));
                            //Genaral.Load_Combo("SELECT IT_ID, IT_CODE || '-' ||IT_NAME from TBLMMSITEMMASTER  Where IT_UOM='kLtr' and IT_CODE='601410' ", cmbItemCode);
                            //string Reclaimed_oil_itemcode = ConfigurationManager.AppSettings["Reclaimed_oil_itemcode"];
                            //string EHV_Grade_Transformer = ConfigurationManager.AppSettings["EHV_Grade_Transformer_oil_itemcode"];

                            //string Qry = "SELECT IT_ID, IT_CODE || '-' ||IT_NAME from TBLMMSITEMMASTER  Where IT_UOM='kLtr' ";
                            //Qry += " AND IT_CODE IN('" + Reclaimed_oil_itemcode + "','" + EHV_Grade_Transformer + "') ";
                            //Genaral.Load_Combo(Qry, "--Select--", cmbItemCode);

                            string Reclaimed_oil_itemcode = ConfigurationManager.AppSettings["Reclaimed_oil_itemcode"];
                            string EHV_Grade_Transformer = ConfigurationManager.AppSettings["EHV_Grade_Transformer_oil_itemcode"];

                            string Qry = "SELECT IT_ID, IT_CODE || '-' ||IT_NAME from TBLMMSITEMMASTER  Where IT_UOM='kLtr' ";
                            Qry += " AND IT_CODE IN('" + Reclaimed_oil_itemcode + "','" + EHV_Grade_Transformer + "') ";
                            Genaral.Load_Combo(Qry, "--Select--", cmbItemCode);
                            WorkFlowConfig();


                        }
                        if (txtActiontype.Text == "A" && objSession.RoleId == "2")
                        {
                            strQry = "Title=Search and Select Purchase Order Details&";

                            strQry += "Query=SELECT RSM_PO_NO, ROI_INVOICE_NO from TBLREPAIRSENTMASTER, TBL_REPAIREROIL_INVOICE where ROI_OFFICECODE LIKE '" + objSession.OfficeCode + "%' AND RSM_ID = ROI_RSM_ID AND ROI_APPROVE_STATUS = '0'";
                            strQry += " AND {0} like %{1}% &";
                            strQry += "DBColName=RSM_PO_NO~ROI_INVOICE_NO&";
                            strQry += "ColDisplayName=PO No~Invoice No&";
                        }

                        else
                        {
                            strQry = "Title=Search and Select Purchase Order Details&";

                            strQry += "Query=SELECT DISTINCT RSM_PO_NO,RSM_INV_NO FROM TBLREPAIRSENTMASTER WHERE RSM_DIV_CODE LIKE '" + objSession.OfficeCode + "%' AND RSM_OIL_TYPE = '0' ";
                            strQry += " and {0} like %{1}% ORDER BY RSM_INV_NO desc &";
                            strQry += "DBColName=RSM_PO_NO~RSM_INV_NO&";
                            strQry += "ColDisplayName=PO No~Invoice No&";
                        }
                        strQry = strQry.Replace("'", @"\'");

                        cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtPONo.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + txtPONo.ClientID + ")");

                        if (txtActiontype.Text == "V")
                        {
                            cmdSave.Enabled = false;
                            dvComments.Style.Add("display", "none");
                        }
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
            clsDTrRepairActivity objoilinvoice = new clsDTrRepairActivity();
            string[] Arr = new string[2];

            if (ValidateForm() == true)
            {
                objoilinvoice.sPurchaseOrderNo = txtPONo.Text;
                objoilinvoice.sInvoiceNo = txtInvoiceNo.Text;
                objoilinvoice.sInvoiceDate = txtInvoiceDate.Text;
                objoilinvoice.sItemCode = cmbItemCode.SelectedValue;
                objoilinvoice.sInvoicedQty = txtEnterInvoiceOilQty.Text;
                objoilinvoice.sCrby = objSession.UserId;
                objoilinvoice.sOfficeCode = objSession.OfficeCode;
                objoilinvoice.sRepairerId = cmbRepairer.SelectedValue;
                objoilinvoice.sOilQtyInKltr = txtquantity.Text;
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

                objoilinvoice.sClientIP = sClientIP;

                bool flag = false;

                if (txtActiontype.Text == "A")
                {
                    ApproveAction();

                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "AgentpendingSearch");

                    }
                    return;
                }
                foreach (GridViewRow row in grdOilInvioce.Rows)
                {
                    if (((CheckBox)row.FindControl("chkSelect")).Checked == true)
                    {
                        objoilinvoice.RepairDetailsId = ((Label)row.FindControl("lblRepairDetailsId")).Text;
                        objoilinvoice.sTcQuantity = ((Label)row.FindControl("lbltcquantity")).Text;
                        objoilinvoice.sTotalOilQty = ((Label)row.FindControl("lbloiltotalqty")).Text;
                        objoilinvoice.sSentOilQty = ((Label)row.FindControl("lblsentoil")).Text;
                        objoilinvoice.sPendingOilQty = ((Label)row.FindControl("lblpendingoil")).Text;
                        objoilinvoice.sRepairerInvoiceNO = ((Label)row.FindControl("lblinvoiceno")).Text;
                        objoilinvoice.Repairsentoil = ((Label)row.FindControl("lblrepairsentoil")).Text;
                        flag = true;
                    }
                }

                if (!flag)
                {
                    ShowMsgBox("Please Select Po No");
                    return;
                }

                if (!(Convert.ToDouble(txtEnterInvoiceOilQty.Text) <= Convert.ToDouble(objoilinvoice.sTotalOilQty)))
                {
                    ShowMsgBox("Enter Invoice Oil Qty Should be less than or equals to Total Po Oil Qty");
                    return;
                }
                else if (!(Convert.ToDouble(txtEnterInvoiceOilQty.Text) <= Convert.ToDouble(objoilinvoice.sPendingOilQty)))
                {
                    ShowMsgBox("Enter Invoice Oil Qty Should be less than or equals to Pending Oil");
                    return;
                }

                if (Convert.ToDouble(txtquantity.Text) > Convert.ToDouble(hdftotalqty.Value))
                {
                    ShowMsgBox("Invoice Oil Qty Should be less than or equals to Total  Availbale Oil Quantity");
                    return;
                }


                Arr = objoilinvoice.SaveRepairerOilInvoice(objoilinvoice);
                if (Arr[1].ToString() == "1")
                {
                    ShowMsgBox(Arr[0].ToString());
                    cmdSave.Enabled = false;
                }
                else if ((Arr[0] ?? "").Length > 0)
                {
                    ShowMsgBox(Arr[0].ToString());
                }

            }
        }


        public bool ValidateForm()
        {
            bool bValidate = false;
            try
            {
                if (txtInvoiceDate.Text == "")
                {
                    ShowMsgBox("Enter Invoice Date");
                    txtInvoiceDate.Focus();
                    return false;

                }

                if (cmbItemCode.SelectedIndex == 0)
                {
                    ShowMsgBox("Please Select Item Code");
                    cmbItemCode.Focus();
                    return false;

                }

                if (txtEnterInvoiceOilQty.Text != "" && txtEnterInvoiceOilQty.Text.StartsWith("."))
                {
                    ShowMsgBox("Please Enter Valid Oil Quantity ");
                    txtEnterInvoiceOilQty.Focus();
                    return bValidate;
                }

                if (txtEnterInvoiceOilQty.Text == "" )
                {
                    ShowMsgBox("Enter Invoice Oil Qty");
                    txtEnterInvoiceOilQty.Focus();
                    return bValidate;
                }

               


                if (txtEnterInvoiceOilQty.Text != "")
                {
                    string oilqty = txtEnterInvoiceOilQty.Text;
                    string qty = oilqty.Trim('0').Replace(".", "");

                    if (qty == "")
                    {
                        ShowMsgBox("Enter Valid Invoice Oil Qty");
                        txtEnterInvoiceOilQty.Focus();
                        return bValidate;
                    }
                    if (txtEnterInvoiceOilQty.Text.Contains(".") && txtEnterInvoiceOilQty.Text.IndexOf(".") != txtEnterInvoiceOilQty.Text.LastIndexOf("."))
                    {
                        ShowMsgBox("Enter Valid Invoice Oil Qty");
                        txtEnterInvoiceOilQty.Focus();
                        return bValidate;
                    }
                    int Dot = 0;
                    foreach (char c in txtEnterInvoiceOilQty.Text)
                    {
                        if (c == '.')
                        {
                            Dot++;
                        }

                        if (Dot > 1)
                        {
                            ShowMsgBox("Enter Valid Invoice Oil Qty");
                            txtEnterInvoiceOilQty.Focus();
                            return bValidate;
                        }
                    }
                                          
                    if (txtEnterInvoiceOilQty.Text.Contains('-'))
                    {
                        ShowMsgBox("Enter Valid Invoice Oil Qty");
                        txtEnterInvoiceOilQty.Focus();
                        return bValidate;
                    }

                    if (hdftotalqty.Value != "")
                    {
                        if (Convert.ToDouble(hdftotalqty.Value) <= 0)
                        {
                            ShowMsgBox("Total Available Oil Qty can not be less than or equals to 0");
                            return false;
                        }
                        if (Convert.ToDouble(hdftotalqty.Value) < Convert.ToDouble(txtquantity.Text))
                        {
                            ShowMsgBox("Quantity(in kltr) Should be less than or equals to Total  Availbale Quantity");
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
  
        protected void cmdLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPONo.Text != "")
                {
                    LoadTestingPendingTCDetails();
                }
                else
                {
                    ShowMsgBox("Please Select Purchase Order NO");
                    return;
                }


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdLoad_Click");
            }
        }
        protected void cmd_LoadTextFields(object sender, EventArgs e)
        {
            try
            {
                SaveCheckedValues();
                if (ViewState["CHECKED_ITEMS"] != null)
                {
                    string OffCode = objSession.OfficeCode;
                    string Reclaimed_oil_itemcode = ConfigurationManager.AppSettings["Reclaimed_oil_itemcode"];
                    string EHV_Grade_Transformer = ConfigurationManager.AppSettings["EHV_Grade_Transformer_oil_itemcode"];

                    string Qry = "SELECT IT_ID, IT_CODE || '-' ||IT_NAME from TBLMMSITEMMASTER  Where IT_UOM='kLtr' ";
                    Qry += " AND IT_CODE IN('" + Reclaimed_oil_itemcode + "','" + EHV_Grade_Transformer + "') ";
                    Genaral.Load_Combo(Qry, "--Select--", cmbItemCode);

                    //objRepair.sItemCode = cmbItemCode.SelectedValue;
                    //objRepair.sOfficeCode = objSession.OfficeCode;

                    //lblAvailableQty.Text = "Total Available Oil Qty " + objRepair.getTotalItemQnty(objRepair) + " Kltr";
                    //hdftotalqty.Value = objRepair.getTotalItemQnty(objRepair);
                    GenerateInvoiceNo();
                    divFormDetails.Visible = true;
                    btnloadfilldetails.Visible = true;
                    string Podate = hdfPodate.Value;
                    DateTime DToDate = DateTime.ParseExact(Podate, "d-M-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string sFromDate = DToDate.ToString("yyyy/MM/dd");
                    CalendarExtender1.StartDate = Convert.ToDateTime(sFromDate);

                }
                else
                {
                    ShowMsgBox("Please Select Purchase Order NO");
                }


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmd_LoadTextFields");
            }
        }
        protected void cmdReset_Click(object sender, EventArgs e)
        {

            //txtPONo.Text = string.Empty;
            //cmbRepairer.SelectedIndex = 0;
            Response.Redirect("RepairerInvoiceCreate.aspx");

        }

        private void LoadTestingPendingTCDetails()
        {
            try
            {
                clsDTrRepairActivity objTestpending = new clsDTrRepairActivity();
                objTestpending.sPurchaseOrderNo = txtPONo.Text.Trim();
                objTestpending.sOfficeCode = objSession.OfficeCode;
                objTestpending.sRoleId = objSession.RoleId;
                objTestpending.sTestingDone = "0";

                DataTable dt = new DataTable();
                dt = objTestpending.LoadTestOrDeliverPendingDTRForRIC(objTestpending);
                if (dt.Rows.Count > 0)
                {
                    ViewState["testpending"] = dt;

                    cmbRepairer.SelectedValue = dt.Rows[0]["RSM_SUPREP_ID"].ToString();
                    if (dt.Rows[0]["ROI_PENDINGQTY"].ToString() == "")
                    {
                        dt.Rows[0]["ROI_PENDINGQTY"] = dt.Rows[0]["TOTAL_OIL_AMOUNT"].ToString();
                        dt.Rows[0]["SENT_OIL"] = "0";
                    }
                    if (dt.Rows[0]["REPAIRER_SENT_OIL"].ToString() == "")
                    {
                        dt.Rows[0]["REPAIRER_SENT_OIL"] = "0";
                    }
                    if (dt.Rows[0]["SENT_OIL"].ToString() == "")
                    {
                        dt.Rows[0]["SENT_OIL"] = "0";
                    }
                    double Sentoil = Convert.ToDouble(dt.Rows[0]["SENT_OIL"]) + Convert.ToDouble(dt.Rows[0]["REPAIRER_SENT_OIL"]);
                    double TotalOilQty = Convert.ToDouble(dt.Rows[0]["TOTAL_OIL_AMOUNT"]);
                    double CalculatedPendingQty = TotalOilQty - Sentoil;
                    dt.Rows[0]["ROI_PENDINGQTY"] = CalculatedPendingQty.ToString();
                    hdfPodate.Value = dt.Rows[0]["RSM_PO_DATE"].ToString();
                    grdOilInvioce.DataSource = SortDataTable(dt as DataTable, false);
                    grdOilInvioce.DataBind();
                    grdOilInvioce.Visible = true;
                    divgrd.Visible = true;
                    btnloadfilldetails.Visible = true;
                }
                else
                {
                    ShowEmptyGrid();
                    ViewState["testpending"] = dt;
                    divgrd.Visible = true;
                }


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadPendingTCDetails");
            }
        }
        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("RSD_ID");
                dt.Columns.Add("RSM_PO_NO");
                dt.Columns.Add("TC_QUANTITY");
                dt.Columns.Add("RSM_INV_NO");
                dt.Columns.Add("TOTAL_OIL_AMOUNT");
                dt.Columns.Add("SENT_OIL");
                dt.Columns.Add("ROI_PENDINGQTY");
                dt.Columns.Add("TC_CODE");
                dt.Columns.Add("RSM_ISSUE_DATE");
                dt.Columns.Add("RSD_OIL_QUANTITY");
                dt.Columns.Add("SUP_REPNAME");
                dt.Columns.Add("REPAIRER_SENT_OIL");

                grdOilInvioce.DataSource = dt;
                grdOilInvioce.DataBind();
                int iColCount = grdOilInvioce.Rows[0].Cells.Count;
                grdOilInvioce.Rows[0].Cells.Clear();
                grdOilInvioce.Rows[0].Cells.Add(new TableCell());
                grdOilInvioce.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdOilInvioce.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowEmptyGrid");

            }
        }

        public void GenerateInvoiceNo()
        {
            try
            {
                clsInvoice objInvoice = new clsInvoice();

                txtInvoiceNo.Text = objInvoice.GenerateInvoiceNo(objSession.OfficeCode);
                txtInvoiceNo.ReadOnly = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GenerateInvoiceNo");
            }
        }

        protected void grdOilInvioce_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                SaveCheckedValues();
                grdOilInvioce.PageIndex = e.NewPageIndex;
                LoadTestingPendingTCDetails();
                PopulateCheckedValues();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdOilInvioce_PageIndexChanging");
            }
        }

        protected void grdOilInvioce_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdOilInvioce.PageIndex;
            DataTable dt = (DataTable)ViewState["testpending"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {

                grdOilInvioce.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdOilInvioce.DataSource = dt;
            }
            grdOilInvioce.DataBind();
            grdOilInvioce.PageIndex = pageIndex;
        }

        private void PopulateCheckedValues()
        {
            try
            {
                ArrayList arrCheckedValues = (ArrayList)ViewState["CHECKED_ITEMS"];
                if (arrCheckedValues != null && arrCheckedValues.Count > 0)
                {
                    foreach (GridViewRow gvrow in grdOilInvioce.Rows)
                    {
                        int index = Convert.ToInt32(grdOilInvioce.DataKeys[gvrow.RowIndex].Values[0]);
                        if (arrCheckedValues.Contains(index))
                        {
                            CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkSelect");
                            myCheckBox.Checked = true;
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "PopulateCheckedValues");

            }
        }

        private void SaveCheckedValues()
        {
            try
            {
                ArrayList arrCheckedValues = new ArrayList();
                int index = -1;
                foreach (GridViewRow gvrow in grdOilInvioce.Rows)
                {
                    index = Convert.ToInt32(grdOilInvioce.DataKeys[gvrow.RowIndex].Values[0]); ;

                    bool result = ((CheckBox)gvrow.FindControl("chkSelect")).Checked;

                    // Check in the viewstate
                    if (ViewState["CHECKED_ITEMS"] != null)
                        arrCheckedValues = (ArrayList)ViewState["CHECKED_ITEMS"];
                    if (result)
                    {
                        if (!arrCheckedValues.Contains(index))
                            arrCheckedValues.Add(index);
                    }
                    else
                        arrCheckedValues.Remove(index);
                }
                if (arrCheckedValues != null && arrCheckedValues.Count > 0)
                    ViewState["CHECKED_ITEMS"] = arrCheckedValues;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveCheckedValues");
            }
        }

        private string GetSortDirection()
        {
            switch (GridViewSortDirection)
            {
                case "ASC":
                    GridViewSortDirection = "DESC";

                    break;
                case "DESC":
                    GridViewSortDirection = "ASC";

                    break;
            }


            return GridViewSortDirection;
        }

        private string GridViewSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "ASC"; }
            set { ViewState["SortDirection"] = value; }


        }
        private string GridViewSortExpression
        {
            get { return ViewState["SortExpression"] as string ?? string.Empty; }
            set { ViewState["SortExpression"] = value; }
        }
        protected DataView SortDataTable(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                        ViewState["testpending"] = dataView.ToTable();


                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["testpending"] = dataView.ToTable();


                    }


                }
                return dataView;
            }
            else
            {
                return new DataView();
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

        protected void imgBtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                ImageButton imgEdit = (ImageButton)sender;
                GridViewRow rw = (GridViewRow)imgEdit.NamingContainer;

                String strTCcode = ((Label)rw.FindControl("lblTcCode")).Text;
                strTCcode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(strTCcode));
                Response.Redirect("DeliverTC.aspx?QryTccode=" + strTCcode + "");
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "imgBtnEdit_Click");
            }
        }

        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "RepairerInvoiceCreate";
                objApproval.sRoleId = objSession.RoleId;
                objApproval.sAccessType = "1" + "," + sAccessType;
                bool bResult = objApproval.CheckAccessRights(objApproval);

                if (Request.QueryString["ActionType"] != null && Request.QueryString["ActionType"].ToString() != "")
                {
                    txtActiontype.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));
                }
                if (objSession.RoleId == "2" && txtActiontype.Text == "")
                {
                    bResult = false;
                }
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

        protected void cmdResetDetails_Click(object sender, EventArgs e)
        {
            txtInvoiceDate.Text = string.Empty;
            txtquantity.Text = string.Empty;
            txtEnterInvoiceOilQty.Text = string.Empty;
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
                        hdfPono.Value = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["Wodataid"]));
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
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                // clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "WorkFlowConfig");
            }
        }


        public void GetWODetailsFromXML(string sWFDataId)
        {
            try
            {
                clsDTrRepairActivity objRepairOilInvoice = new clsDTrRepairActivity();
                objRepairOilInvoice.sWFDataId = sWFDataId;
                objRepairOilInvoice.sPurchaseOrderNo = hdfPono.Value;
                DataTable DtForGrid = objRepairOilInvoice.GetRepairerOilDetailsFromXML(objRepairOilInvoice);

                if (DtForGrid.Rows.Count > 0)
                {

                    txtPONo.Text = objRepairOilInvoice.sPurchaseOrderNo;
                    txtInvoiceNo.Text = Convert.ToString(DtForGrid.Rows[0]["RSM_INV_NO"]).Trim();
                    txtEnterInvoiceOilQty.Text = Convert.ToString(DtForGrid.Rows[0]["ROI_INVOICED_OILQTY"]).Trim();
                    txtInvoiceDate.Text = Convert.ToString(DtForGrid.Rows[0]["ROI_INVOICED_DATE"]).Trim();
                    txtquantity.Text = Convert.ToString(DtForGrid.Rows[0]["ROI_INVOICE_QTY_KLTR"]).Trim();
                    cmbRepairer.SelectedItem.Text = Convert.ToString(DtForGrid.Rows[0]["SUP_REPNAME"]).Trim();
                    cmbItemCode.SelectedValue = Convert.ToString(DtForGrid.Rows[0]["ROI_ITEM_ID"]).Trim();

                    grdOilInvioce.DataSource = SortDataTable(DtForGrid as DataTable, false);
                    grdOilInvioce.DataBind();
                    foreach (GridViewRow row in grdOilInvioce.Rows)
                    {
                        CheckBox myCheckBox = (CheckBox)row.FindControl("chkSelect");
                        myCheckBox.Checked = true;
                        myCheckBox.Enabled = false;
                    }
                }
                else
                {
                    ShowEmptyGrid();
                    ViewState["testpending"] = DtForGrid;
                    divgrd.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "GetFailureDetailsFromXML");
            }

        }
        public void SetControlText()
        {
            grdOilInvioce.Visible = true;
            divgrd.Visible = true;
            divFormDetails.Visible = true;
            btnloadfilldetails.Visible = true;
            txtPONo.Enabled = true;
            cmdSearch.Enabled = false;
            cmdLoad.Visible = false;
            cmdReset.Visible = false;
            btnloadfilldetails.Visible = false;
            cmdresetdetails.Visible = false;
            cmdSave.Text = "Approve";
            cmbItemCode.Enabled = false;
            txtInvoiceNo.Enabled = false;
            txtInvoiceDate.Enabled = false;
            txtEnterInvoiceOilQty.Enabled = false;
            dvComments.Style.Add("display", "block");
        }

        public void ApproveAction()
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
                objApproval.sWFObjectId = hdfWFOId.Value;
                objApproval.sWFAutoId = hdfWFOAutoId.Value;
                objApproval.sApproveComments = txtComment.Text.Trim();
                if (txtActiontype.Text == "A")
                {
                    objApproval.sApproveStatus = "1";
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

                bResult = objApproval.ApproveWFRequest(objApproval);


                if (bResult == true)
                {

                    if (objApproval.sApproveStatus == "1")
                    {

                        ShowMsgBox("Approved Successfully");
                        cmdSave.Enabled = false;

                    }

                }
                else
                {
                    ShowMsgBox("Something Went Wrong");
                    return;
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "cmdApprove_Click");
            }
        }

        protected void grdInvoicedPo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdInvoicedPo.PageIndex = e.NewPageIndex;
                PopulateCheckedValues();
                DataTable dt = (DataTable)ViewState["InvoicedDetails"];
                grdInvoicedPo.DataSource = SortDataTable(dt as DataTable, true);
                grdInvoicedPo.DataBind();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdInvoicedPo_PageIndexChanging");
            }
        }

        protected void grdInvoicedPo_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                GridViewSortExpression = e.SortExpression;
                int pageIndex = grdInvoicedPo.PageIndex;
                DataTable dt = (DataTable)ViewState["InvoicedDetails"];
                string sortingDirection = string.Empty;

                grdInvoicedPo.DataSource = SortDataTable(dt as DataTable, false);
                grdInvoicedPo.DataBind();
                grdInvoicedPo.PageIndex = pageIndex;
            }


            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdInvoicedPo_PageIndexChanging");
            }
        }

        protected void txtqty_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtEnterInvoiceOilQty.Text != "" && txtEnterInvoiceOilQty.Text.StartsWith("."))
                {
                    ShowMsgBox("Please Enter Valid Oil Quantity ");
                    txtEnterInvoiceOilQty.Focus();
                    return;
                }
                string Quantity = txtEnterInvoiceOilQty.Text;
                if (Quantity != "")
                {
                    double Qtyltr = 1000;
                    double TotalOilQuantityinLtr = Convert.ToDouble(Quantity) / Qtyltr;
                    txtquantity.Text = Convert.ToString(TotalOilQuantityinLtr);
                }
                else
                {
                    txtquantity.Text = string.Empty;
                    txtquantity.Enabled = true;
                }


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "txtqty_SelectedIndexChanged");
            }
        }

        public void LoadInvoicedOilDetails()
        {
            DataTable dtInvoicedDetails = new DataTable();
            try
            {
                clsDTrRepairActivity objinvoiceoildetails = new clsDTrRepairActivity();
                objinvoiceoildetails.sOfficeCode = objSession.OfficeCode;
                dtInvoicedDetails = objinvoiceoildetails.LoadInvoicedOilDetails(objinvoiceoildetails);
                if (dtInvoicedDetails.Rows.Count > 0)
                {
                    grdInvoicedPo.DataSource = SortDataTable(dtInvoicedDetails as DataTable, false);
                    grdInvoicedPo.DataBind();
                    ViewState["InvoicedDetails"] = dtInvoicedDetails;
                }
                else
                {
                    DataTable dt = new DataTable();
                    DataRow newRow = dt.NewRow();
                    dt.Rows.Add(newRow);
                    dt.Columns.Add("RSM_PO_NO");
                    dt.Columns.Add("ROI_INVOICE_NO");
                    dt.Columns.Add("ROI_INVOICE_DATE");
                    dt.Columns.Add("ROI_MMS_INVOICE_NO");
                    dt.Columns.Add("ROI_TOTAL_OILQTY");
                    dt.Columns.Add("ROI_INVOICED_OILQTY");

                    grdInvoicedPo.DataSource = dt;
                    grdInvoicedPo.DataBind();
                    int iColCount = grdInvoicedPo.Rows[0].Cells.Count;
                    grdInvoicedPo.Rows[0].Cells.Clear();
                    grdInvoicedPo.Rows[0].Cells.Add(new TableCell());
                    grdInvoicedPo.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdInvoicedPo.Rows[0].Cells[0].Text = "No Records Found";
                    ViewState["InvoicedDetails"] = dt;
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadInvoicedOilDetails");
            }
        }

        protected void cmdClose_click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/Dashboard.aspx", false);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                lblMessage.Text = ex.Message;
            }
        }
        /// <summary>
        /// This function used for get the total oil quantity from mms based on selected item code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cmbItemCode_SelectedIndexChanged(object sender ,EventArgs e)
        {
            try
            {
                clsDTrRepairActivity objRepair = new clsDTrRepairActivity();
                objRepair.sItemCode = cmbItemCode.SelectedValue;
                objRepair.sOfficeCode = objSession.OfficeCode;

                lblAvailableQty.Text = "Total Available Oil Qty " + objRepair.getTotalItemQnty(objRepair) + " Kltr";
                hdftotalqty.Value = objRepair.getTotalItemQnty(objRepair);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                lblMessage.Text = ex.Message;
            }
        }



    }
}