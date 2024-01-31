
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Collections;
using IIITS.DTLMS.BL.OilFlow;

namespace IIITS.DTLMS.OilFlow
{
    public partial class PendingAgencyTest : System.Web.UI.Page
    {
        string strFormCode = "TestPendingSearch";
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
                if (!IsPostBack)
                {
                    if (CheckAccessRights("4"))
                    //{

                    if (objSession.RoleId == "31")
                    {
                        Genaral.Load_Combo("select CM_CIRCLE_CODE, CM_CIRCLE_NAME from TBLCIRCLE WHERE CM_CIRCLE_CODE IN (SELECT TRM_CM_ID FROM TBLTAQCROLEMAPPING WHERE TRM_US_ID='"+objSession.UserId+"') ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle);
                    }
                    else
                    {
                        Genaral.Load_Combo("select CM_CIRCLE_CODE, CM_CIRCLE_NAME from TBLCIRCLE ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle);
                    }
                    LoadComboFiled();
                        string Pono=LoadTestingPendingDetails();

                        string strQry = string.Empty;
                    string off = string.Empty;
                    if (objSession.RoleId == "31")
                    {
                        off = cmbDivision.SelectedValue;
                    }

                    if (Pono=="")
                    {
                        //strQry = "Title=Search and Select Oil Purchase Order Details&";
                        //strQry += "Query=SELECT DISTINCT OSD_PO_NO,OSD_INVOICE_NO FROM TBLOILSENTMASTER  WHERE OSD_OFFICE_CODE LIKE '" + objSession.OfficeCode + "%' AND OSD_STATUS='0' and (OSD_INSP_RESULT is null or OSD_INSP_RESULT ='0') and OSD_STATUS_FLAG='1' ";
                        //strQry += "&DBColName=OSD_PO_NO~OSD_INVOICE_NO&";
                        //strQry += "ColDisplayName=PO No~Invoice No&";
                        if (objSession.RoleId == "31")
                        {
                            strQry = "Title=Search and Select Oil Purchase Order Details&";
                            strQry += "Query=select * from(SELECT DISTINCT OSD_PO_NO,OSD_INVOICE_NO,OSD_ID FROM TBLOILSENTMASTER  WHERE OSD_OFFICE_CODE LIKE '" + off + "%' AND OSD_STATUS='0' and (OSD_INSP_RESULT is null or OSD_INSP_RESULT ='0') and OSD_STATUS_FLAG='1' UNION ALL ";
                            strQry += " SELECT DISTINCT OSD_PO_NO,OSD_INVOICE_NO,OSD_ID FROM TBLOILSENTMASTER inner join TBLOILSENTDETAILS on UPPER(OSD_PO_NO)=UPPER(OS_PO_NO)  WHERE OSD_OFFICE_CODE LIKE '" + off + "%' AND (OS_PENDING_QTY <>'0') and OSD_STATUS_FLAG='1' order by OSD_ID desc) ";
                            strQry += " where {0} like %{1}% &DBColName=UPPER(OSD_PO_NO)~OSD_INVOICE_NO&";
                            strQry += "ColDisplayName=PO No~Invoice No&";
                        }
                        else
                        {
                            strQry = "Title=Search and Select Oil Purchase Order Details&";
                            strQry += "Query=select * from(SELECT DISTINCT OSD_PO_NO,OSD_INVOICE_NO,OSD_ID FROM TBLOILSENTMASTER  WHERE OSD_OFFICE_CODE LIKE '" + objSession.OfficeCode + "%' AND OSD_STATUS='0' and (OSD_INSP_RESULT is null or OSD_INSP_RESULT ='0') and OSD_STATUS_FLAG='1' UNION ALL ";
                            strQry += " SELECT DISTINCT OSD_PO_NO,OSD_INVOICE_NO,OSD_ID FROM TBLOILSENTMASTER inner join TBLOILSENTDETAILS on UPPER(OSD_PO_NO)=UPPER(OS_PO_NO)  WHERE OSD_OFFICE_CODE LIKE '" + objSession.OfficeCode + "%' AND (OS_PENDING_QTY <>'0') and OSD_STATUS_FLAG='1' order by OSD_ID desc) ";
                            strQry += " where {0} like %{1}% &DBColName=UPPER(OSD_PO_NO)~OSD_INVOICE_NO&";
                            strQry += "ColDisplayName=PO No~Invoice No&";
                        }
                    }
                    else
                    {
                        if (objSession.RoleId == "31")
                        {
                            strQry = "Title=Search and Select Oil Purchase Order Details&";
                            strQry += "Query=select * from(SELECT DISTINCT OSD_PO_NO,OSD_INVOICE_NO FROM TBLOILSENTMASTER inner join TBLOILSENTDETAILS on UPPER(OSD_PO_NO)=UPPER(OS_PO_NO)  WHERE OSD_OFFICE_CODE LIKE '" + off + "%' AND (OS_PENDING_QTY <>'0') and OSD_STATUS_FLAG='1') ";
                            strQry += "where {0} like %{1}% &DBColName=UPPER(OSD_PO_NO)~OSD_INVOICE_NO&";
                            strQry += "ColDisplayName=PO No~Invoice No&";
                        }
                        else
                        {
                            strQry = "Title=Search and Select Oil Purchase Order Details&";
                            strQry += "Query=select * from(SELECT DISTINCT OSD_PO_NO,OSD_INVOICE_NO FROM TBLOILSENTMASTER inner join TBLOILSENTDETAILS on UPPER(OSD_PO_NO)=UPPER(OS_PO_NO)  WHERE OSD_OFFICE_CODE LIKE '" + objSession.OfficeCode + "%' AND (OS_PENDING_QTY <>'0') and OSD_STATUS_FLAG='1') ";
                            strQry += "where {0} like %{1}% &DBColName=UPPER(OSD_PO_NO)~OSD_INVOICE_NO&";
                            strQry += "ColDisplayName=PO No~Invoice No&";
                        }
                    }
                    strQry = strQry.Replace("'", @"\'");

                        cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtPONo.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + txtPONo.ClientID + ")");
                    //}

                   // ShowMsgBox("Please Select Circle and Division then Proceed");
                    txtPONo.Enabled = false;
                    cmdSearch.Enabled = false;
                    cmdLoad.Enabled = false;
                    cmdReset.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load");
            }
        }

        public void LoadComboFiled()
        {
            try
            {
                string strQry = string.Empty;
                strQry = "SELECT TM_ID,TM_NAME FROM TBLTRANSMAKES ORDER BY TM_NAME";
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadComboFiled");
            }
        }
        protected void cmbCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Genaral.Load_Combo("select DIV_CODE,DIV_CODE || '-' || DIV_NAME FROM TBLDIVISION  WHERE DIV_CICLE_CODE LIKE  '" + cmbCircle.SelectedValue + "%' ORDER BY DIV_CODE ", "--Select--", cmbDivision);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbCircle_SelectedIndexChanged");
            }
        }
        protected void cmdSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string Pono = LoadTestingPendingDetails();
                string off = string.Empty;
                if (objSession.RoleId=="31")
                {
                    off= cmbDivision.SelectedValue;
                }
                string strQry = string.Empty;

                if (Pono == "")
                {
                    if (objSession.RoleId == "31")
                    {
                        strQry = "Title=Search and Select Oil Purchase Order Details&";
                        strQry += "Query=select * from(SELECT DISTINCT OSD_PO_NO,OSD_INVOICE_NO,OSD_ID FROM TBLOILSENTMASTER  WHERE OSD_OFFICE_CODE LIKE '" + off + "%' AND OSD_STATUS='0' and (OSD_INSP_RESULT is null or OSD_INSP_RESULT ='0') and OSD_STATUS_FLAG='1' UNION ALL ";
                        strQry += " SELECT DISTINCT OSD_PO_NO,OSD_INVOICE_NO,OSD_ID FROM TBLOILSENTMASTER inner join TBLOILSENTDETAILS on UPPER(OSD_PO_NO)=UPPER(OS_PO_NO)  WHERE OSD_OFFICE_CODE LIKE '" + off + "%' AND (OS_PENDING_QTY <>'0') and OSD_STATUS_FLAG='1' order by OSD_ID desc) ";
                        strQry += " where {0} like %{1}% &DBColName=UPPER(OSD_PO_NO)~OSD_INVOICE_NO&";
                        strQry += "ColDisplayName=PO No~Invoice No&";
                    }
                    else
                    {
                        strQry = "Title=Search and Select Oil Purchase Order Details&";
                        strQry += "Query=select * from(SELECT DISTINCT OSD_PO_NO,OSD_INVOICE_NO,OSD_ID FROM TBLOILSENTMASTER  WHERE OSD_OFFICE_CODE LIKE '" + objSession.OfficeCode + "%' AND OSD_STATUS='0' and (OSD_INSP_RESULT is null or OSD_INSP_RESULT ='0') and OSD_STATUS_FLAG='1' UNION ALL ";
                        strQry += " SELECT DISTINCT OSD_PO_NO,OSD_INVOICE_NO,OSD_ID FROM TBLOILSENTMASTER inner join TBLOILSENTDETAILS on UPPER(OSD_PO_NO)=UPPER(OS_PO_NO)  WHERE OSD_OFFICE_CODE LIKE '" + objSession.OfficeCode + "%' AND (OS_PENDING_QTY <>'0') and OSD_STATUS_FLAG='1' order by OSD_ID desc) ";
                        strQry += " where {0} like %{1}% &DBColName=UPPER(OSD_PO_NO)~OSD_INVOICE_NO&";
                        strQry += "ColDisplayName=PO No~Invoice No&";
                    }
                }
                else
                {
                    if (objSession.RoleId == "31")
                    {
                        strQry = "Title=Search and Select Oil Purchase Order Details&";
                        strQry += "Query=select * from(SELECT DISTINCT OSD_PO_NO,OSD_INVOICE_NO FROM TBLOILSENTMASTER inner join TBLOILSENTDETAILS on UPPER(OSD_PO_NO)=UPPER(OS_PO_NO)  WHERE OSD_OFFICE_CODE LIKE '" + off + "%' AND (OS_PENDING_QTY <>'0') and OSD_STATUS_FLAG='1') ";
                        strQry += "where {0} like %{1}% &DBColName=UPPER(OSD_PO_NO)~OSD_INVOICE_NO&";
                        strQry += "ColDisplayName=PO No~Invoice No&";
                    }
                    else
                    {
                        strQry = "Title=Search and Select Oil Purchase Order Details&";
                        strQry += "Query=select * from(SELECT DISTINCT OSD_PO_NO,OSD_INVOICE_NO FROM TBLOILSENTMASTER inner join TBLOILSENTDETAILS on UPPER(OSD_PO_NO)=UPPER(OS_PO_NO)  WHERE OSD_OFFICE_CODE LIKE '" + objSession.OfficeCode + "%' AND (OS_PENDING_QTY <>'0') and OSD_STATUS_FLAG='1') ";
                        strQry += "where {0} like %{1}% &DBColName=UPPER(OSD_PO_NO)~OSD_INVOICE_NO&";
                        strQry += "ColDisplayName=PO No~Invoice No&";
                    }
                }
                strQry = strQry.Replace("'", @"\'");

                cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtPONo.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + txtPONo.ClientID + ")");

                //ShowMsgBox("Please Select Circle and Division then Proceed");

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbCircle_SelectedIndexChanged");
            }
        }
        protected void cmdLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbCircle.SelectedIndex == 0)
                {
                    cmbCircle.Focus();
                    ShowMsgBox("Please Select Circle");
                    return;
                }
                if (cmbDivision.SelectedIndex == 0)
                {
                    cmbDivision.Focus();
                    ShowMsgBox("Please Select Division");
                    return;
                }
                cmbCircle.Enabled = false;
                cmbDivision.Enabled = false;

                LoadTestingPendingTCDetails();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdLoad_Click");
            }
        }

        protected void btnProceed_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbCircle.SelectedIndex == 0)
                {
                    cmbCircle.Focus();
                    ShowMsgBox("Please Select Circle");
                    txtPONo.Text = string.Empty;
                    return;
                }
                if (cmbDivision.SelectedIndex == 0)
                {
                    cmbDivision.Focus();
                    ShowMsgBox("Please Select Division");
                    txtPONo.Text = string.Empty;
                    return;
                }

                txtPONo.Enabled = true;
                cmdSearch.Enabled = true;
                cmdLoad.Enabled = true;
                cmdReset.Enabled = true;
                cmdSearch_SelectedIndexChanged(sender, e);

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdReset_Click");
            }
        }

        private string LoadTestingPendingDetails()
        {
            string Pono = string.Empty;
            try
            {
                clsOilTest objDeliverPending = new clsOilTest();

                objDeliverPending.sOfficeCode = objSession.OfficeCode;
                objDeliverPending.sRoleId = objSession.RoleId;
                objDeliverPending.sTestingDone = "1";
                objDeliverPending.sOfficeCode = objSession.OfficeCode; 

                if(objSession.RoleId=="31")
                {
                    objDeliverPending.sOfficeCode = cmbDivision.SelectedValue;
                }
                objDeliverPending.sPurchaseOrderNo = "";
                Pono = objDeliverPending.GetPono(objDeliverPending);
                DataTable dt = new DataTable();
                dt = objDeliverPending.LoadPendingToRecieveDetails(objDeliverPending);

                grdTestPending.DataSource = dt;
                grdTestPending.DataBind();
                ViewState["TestPending"] = dt;
                return Pono;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadTestingPendingDetails");
                return Pono;
            }
        }

        private void LoadTestingPendingTCDetails()
        {
            try
            {
                clsOilTest objoiltest = new clsOilTest();

                objoiltest.sPurchaseOrderNo = txtPONo.Text.Trim();
                //objpending.sPendingDays = txtNoofDays.Text.Trim();
                //objoiltest.sOfficeCode = objSession.OfficeCode;
                objoiltest.sRoleId = objSession.RoleId;
                objoiltest.sTestingDone = "0";


                if(objSession.RoleId=="31")
                {
                    objoiltest.sOfficeCode = cmbDivision.SelectedValue;
                }
                else
                {
                    objoiltest.sOfficeCode = objSession.OfficeCode;
                }
                DataTable dt = new DataTable();
                dt = objoiltest.LoadTestOrDeliverPendingDTR(objoiltest);
                if (dt.Rows.Count > 0)
                {
                    ViewState["testpending"] = dt;
                    grdPendingTc.DataSource = SortDataTable(dt as DataTable, false);
                    grdPendingTc.DataBind();
                    cmdDeliver.Visible = true;
                    grdPendingTc.Visible = true;
                }
                else
                {
                    ViewState["testpending"] = dt;
                    cmdDeliver.Visible = false;
                    grdPendingTc.DataSource = dt;//sort;
                    grdPendingTc.DataBind();
                }


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadPendingTCDetails");
            }
        }

        protected void grdTestPending_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdTestPending.PageIndex = e.NewPageIndex;
                PopulateCheckedValues();
                DataTable dt = (DataTable)ViewState["TestPending"];
                grdTestPending.DataSource = SortDataTablePending(dt as DataTable, true);
                grdTestPending.DataBind();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdTestPending_PageIndexChanging");
            }
        }

        protected void grdTestPending_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdTestPending.PageIndex;
            DataTable dt = (DataTable)ViewState["TestPending"];
            string sortingDirection = string.Empty;

            grdTestPending.DataSource = SortDataTablePending(dt as DataTable, false);
            grdTestPending.DataBind();
            grdTestPending.PageIndex = pageIndex;
        }

        protected DataView SortDataTablePending(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                        ViewState["TestPending"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["TestPending"] = dataView.ToTable();

                    }

                }
                return dataView;
            }
            else
            {
                return new DataView();
            }

        }


        protected void cmdDeliver_Click(object sender, EventArgs e)
        {
            string Invoice = string.Empty;
            try
            {

                //Check AccessRights
                //bool bAccResult = CheckAccessRights("2");
                //if (bAccResult == false)
                //{
                //    return;
                //}

                bool AtleastOneApp = false;
                int i = 0;
                string[] Arr = new string[3];
                grdPendingTc.AllowPaging = false;
                SaveCheckedValues();
                LoadTestingPendingTCDetails();

                PopulateCheckedValues();
                string[] strQryVallist = new string[grdPendingTc.Rows.Count];
                string[] strinvoice = new string[grdPendingTc.Rows.Count];
                foreach (GridViewRow row in grdPendingTc.Rows)
                {

                    if (((CheckBox)row.FindControl("chkSelect")).Checked == true)
                    {
                        strQryVallist[i] = ((Label)row.FindControl("lblPoId")).Text.Trim();
                        AtleastOneApp = true;
                        txtPONo.Text = ((Label)row.FindControl("lblPONo")).Text.Trim();
                        strinvoice[i] = ((Label)row.FindControl("lblInvoice")).Text.Trim();

                    }
                    i++;

                }

                grdPendingTc.AllowPaging = true;
                SaveCheckedValues();
                LoadTestingPendingTCDetails();
                PopulateCheckedValues();

                if (!AtleastOneApp)
                {
                    ShowMsgBox("Please Select Po No");
                    SaveCheckedValues();
                    LoadTestingPendingTCDetails();
                    PopulateCheckedValues();
                    return;
                }

                string sSelectedValue = string.Empty;
                string sInvoiceNo = string.Empty;
                for (int j = 0; j < strQryVallist.Length; j++)
                {
                    if (strQryVallist[j] != null)
                    {
                        sSelectedValue += strQryVallist[j].ToString() + "~";
                       
                    }
                }
                string sRepairDetailsId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sSelectedValue));
                Session["RepairDetailsId"] = sSelectedValue;
                string sPONo = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtPONo.Text));
                Response.Redirect("OilTesting.aspx?PoNo=" + sPONo, false);

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdDeliver_Click");
            }
        }

        protected void grdPendingTc_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                SaveCheckedValues();
                grdPendingTc.PageIndex = e.NewPageIndex;
                LoadTestingPendingTCDetails();
                PopulateCheckedValues();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdPendingTc_PageIndexChanging");
            }
        }

        protected void grdPendingTc_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdPendingTc.PageIndex;
            DataTable dt = (DataTable)ViewState["testpending"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {

                grdPendingTc.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdPendingTc.DataSource = dt;
            }
            grdPendingTc.DataBind();
            grdPendingTc.PageIndex = pageIndex;
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

        private void PopulateCheckedValues()
        {
            try
            {
                ArrayList arrCheckedValues = (ArrayList)ViewState["CHECKED_ITEMS"];
                if (arrCheckedValues != null && arrCheckedValues.Count > 0)
                {
                    foreach (GridViewRow gvrow in grdPendingTc.Rows)
                    {
                        int index = Convert.ToInt32(grdPendingTc.DataKeys[gvrow.RowIndex].Values[0]);
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


        //This method is used to save the checkedstate of values
        private void SaveCheckedValues()
        {
            try
            {
                ArrayList arrCheckedValues = new ArrayList();
                int index = -1;
                foreach (GridViewRow gvrow in grdPendingTc.Rows)
                {
                    index = Convert.ToInt32(grdPendingTc.DataKeys[gvrow.RowIndex].Values[0]); ;

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

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtPONo.Text = string.Empty;
                grdPendingTc.DataSource = null;
                grdPendingTc.DataBind();
                grdPendingTc.Visible = false;
                cmdDeliver.Visible = false;

                cmbCircle.SelectedIndex = 0;
                cmbDivision.SelectedIndex = 0;

                cmbCircle.Enabled = true;
                cmbDivision.Enabled = true;

                txtPONo.Enabled = false;
                cmdSearch.Enabled = false;
                cmdLoad.Enabled = false;

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
                    Response.Redirect("~/Dashboard.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdClose_Click");
            }
        }



        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "TestPendingSearch";
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
    }
}