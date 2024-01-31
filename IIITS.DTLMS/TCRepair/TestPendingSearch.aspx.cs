using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Collections;

namespace IIITS.DTLMS.TCRepair
{
    public partial class TestPendingSearch : System.Web.UI.Page
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
                    {
                        LoadComboFiled();
                        LoadTestingPendingDetails();

                        clsDTrRepairActivity objDeliverPending = new clsDTrRepairActivity();

                        string OilStatus = objDeliverPending.CheckwithOrwithoutOil();

                        string strQry = string.Empty;


                        strQry = "Title=Search and Select Purchase Order Details&";
                        strQry += "Query=SELECT DISTINCT RSM_PO_NO,RSM_INV_NO FROM TBLREPAIRSENTMASTER,TBLREPAIRSENTDETAILS,TBLTCMASTER WHERE RSM_HTLT_DIV_CODE ";
                        strQry += "LIKE '" + objSession.OfficeCode + "%' AND RSM_ID=RSD_RSM_ID AND RSD_TC_CODE = TC_CODE AND TC_CURRENT_LOCATION=3 AND";
                        if (objSession.RoleId == "2")
                        {
                            strQry += " RSM_STO_STATUS_FLAG ='1' AND ";
                        }
                        if (objSession.RoleId == "10")
                        {
                            // strQry += " RSM_STO_STATUS_FLAG is null ";
                            strQry += " RSM_STO_STATUS_FLAG is null AND {0} like %{1}% &";
                        }
                        // for oil invoice details
                        strQry += "union all SELECT DISTINCT RSM_PO_NO,RSM_INV_NO FROM TBLREPAIRSENTMASTER,TBLREPAIRSENTDETAILS where  RSM_ID = RSD_RSM_ID and RSM_OIL_TYPE = '0' and RSD_OIL_APPROVE_STATUS = '1' and RSM_HTLT_DIV_CODE LIKE '" + objSession.OfficeCode + "%' and ";

                        strQry += "  RSD_DELIVARY_DATE IS NULL and {0} like %{1}% &";
                        strQry += "DBColName=RSM_PO_NO~RSM_INV_NO&";
                        strQry += "ColDisplayName=PO No~Invoice No&";

                        strQry = strQry.Replace("'", @"\'");

                        cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtPONo.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + txtPONo.ClientID + ")");
                    }
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

                //Genaral.Load_Combo("SELECT TR_ID,TR_NAME  FROM TBLTRANSREPAIRER WHERE TR_STATUS='A' AND TR_ID NOT IN (SELECT TR_ID FROM TBLTRANSREPAIRER WHERE TR_BLACK_LISTED=1 AND TR_BLACKED_UPTO>=SYSDATE) ORDER BY TR_NAME", "--Select--", cmbRepairer);
                //Genaral.Load_Combo("SELECT  TS_ID, TS_NAME  FROM TBLTRANSSUPPLIER  WHERE TS_STATUS='A' AND TS_ID NOT IN (SELECT TS_ID FROM TBLTRANSSUPPLIER WHERE TS_BLACK_LISTED=1 AND TS_BLACKED_UPTO>=SYSDATE) ORDER BY TS_NAME", "--SELECT--", cmbSupplier);
                Genaral.Load_Combo(strQry, "--SELECT--", cmbMake);
                Genaral.Load_Combo("SELECT MD_NAME,MD_NAME from TBLMASTERDATA WHERE MD_TYPE='C' ORDER BY MD_ID", "--Select--", cmbCapacity);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadComboFiled");
            }
        }

        protected void cmdLoad_Click(object sender, EventArgs e)
        {
            try
            {
                LoadTestingPendingTCDetails();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdLoad_Click");
            }
        }

        private void LoadTestingPendingDetails()
        {
            try
            {
                clsDTrRepairActivity objDeliverPending = new clsDTrRepairActivity();

                objDeliverPending.sOfficeCode = objSession.OfficeCode;
                objDeliverPending.sRoleId = objSession.RoleId;
                objDeliverPending.sTestingDone = "1";

                objDeliverPending.sPurchaseOrderNo = "";
                DataTable dt = new DataTable();
                dt = objDeliverPending.LoadPendingForTestingDetails(objDeliverPending);

                grdTestPending.DataSource = dt;
                grdTestPending.DataBind();
                ViewState["TestPending"] = dt;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadTestingPendingDetails");
            }
        }

        private void LoadTestingPendingTCDetails()
        {
            try
            {
                clsDTrRepairActivity objTestpending = new clsDTrRepairActivity();

                if (cmbCapacity.SelectedIndex > 0)
                {
                    objTestpending.sCapacity = cmbCapacity.SelectedValue;
                }
                if (cmbMake.SelectedIndex > 0)
                {
                    objTestpending.sMakeId = cmbMake.SelectedValue;
                }


                objTestpending.sPurchaseOrderNo = txtPONo.Text.Trim();
                //objpending.sPendingDays = txtNoofDays.Text.Trim();
                objTestpending.sOfficeCode = objSession.OfficeCode;
                objTestpending.sRoleId = objSession.RoleId;
                objTestpending.sTestingDone = "0";

                DataTable dt = new DataTable();
                dt = objTestpending.LoadTestOrDeliverPendingDTR(objTestpending);
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
        protected void Export_ClickTestPendingSearch(object sender, EventArgs e)
        {
            //try
            //{
            clsDTrRepairActivity objTestpending = new clsDTrRepairActivity();
            DataTable dt = new DataTable();

            if ((ViewState["testpending"])!= null)
            {
                dt = (DataTable)ViewState["testpending"];
            }
            else
            {
                objTestpending.sPurchaseOrderNo = txtPONo.Text.Trim();
                //objpending.sPendingDays = txtNoofDays.Text.Trim();
                objTestpending.sOfficeCode = objSession.OfficeCode;
                objTestpending.sTestingDone = "0";

                if (cmbCapacity.SelectedIndex > 0)
                {
                    objTestpending.sCapacity = cmbCapacity.SelectedValue;
                }
                if (cmbMake.SelectedIndex > 0)
                {
                    objTestpending.sMakeId = cmbMake.SelectedValue;
                }
                //if (cmbRepairer.SelectedIndex > 0)
                //{
                //    objTestpending.sRepairerId = cmbRepairer.SelectedValue;
                //}
                //if (cmbSupplier.SelectedIndex > 0)
                //{
                //    objTestpending.sSupplierId = cmbSupplier.SelectedValue;
                //}
               
                dt = objTestpending.LoadTestOrDeliverPendingDTR(objTestpending);
            }

            if (dt.Rows.Count > 0 || dt == null)
            {
                dt.Columns["TC_CODE"].ColumnName = "DTR CODE";
                dt.Columns["TC_SLNO"].ColumnName = "DTR SLNO";
                dt.Columns["TM_NAME"].ColumnName = "MAKE NAME";
                dt.Columns["CAPACITY"].ColumnName = "CAPACITY(IN KVA)";
                dt.Columns["RSM_ISSUE_DATE"].ColumnName = "ISSUE DATE";
                dt.Columns["RSM_PO_NO"].ColumnName = "PO NO";

                dt.Columns["TC_MANF_DATE"].ColumnName = "MANUFACTURE DATE";
                dt.Columns["RSM_OLD_PO_NO"].ColumnName = "OLD PO NO";
                dt.Columns["RSM_REMARKS"].ColumnName = "REMARKS";
                dt.Columns["RSM_OIL_TYPE"].ColumnName = "OIL TYPE";
                //dtLoadDetails.Columns["FEEDER NAME"].SetOrdinal(0);

                List<string> listtoRemove = new List<string> { "RSD_ID", "SUP_REPNAME" };
                string filename = "TestPendingDetails" + DateTime.Now + ".xls";
                string pagetitle = "Test Pending Details";

                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No record found");
            }
            //}
            //catch (Exception ex)
            //{
            //    //lblMessage.Text = clsException.ErrorMsg();
            //    //clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Export_ClickTcsearch");
            //}
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

        protected void Export_ClickPendingTesting(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            if (ViewState["TestPending"] != null)
            {
                dt = (DataTable)ViewState["TestPending"];
            }
            else
            {
                clsDTrRepairActivity objDeliverPending = new clsDTrRepairActivity();

                objDeliverPending.sOfficeCode = objSession.OfficeCode;
                objDeliverPending.sTestingDone = "1";

                objDeliverPending.sPurchaseOrderNo = "";

                dt = objDeliverPending.LoadPendingForTestingDetails(objDeliverPending);
            }
            if (dt.Rows.Count > 0)
            {

                dt.Columns["RSM_PO_NO"].ColumnName = "PO No";
                dt.Columns["RSM_DIV_CODE"].ColumnName = "Division Code";
                dt.Columns["PODATE"].ColumnName = "PO Date  ";
                dt.Columns["ISSUEDATE"].ColumnName = "Issue Date";
                dt.Columns["SUP_REPNAME"].ColumnName = "Supplier/Repairer";
                dt.Columns["PO_QUANTITY"].ColumnName = "Total Quantity";
                dt.Columns["PENDING_QNTY"].ColumnName = "Pending Qty for Testing";

                //dt.Columns["PENDING_QTY_EE"].ColumnName = "Pending Quantity with EE ";
                // dt.Columns["DELIVERED_QNTY"].ColumnName = "Recieved Quantity";

                //dt.Columns["PO No"].ColumnName = "RSM_PO_NO";
                //dt.Columns["PO Date"].ColumnName = "PODATE";
                //dt.Columns["Issue Date"].ColumnName = "ISSUEDATE";
                //dt.Columns["Supplier/Repairer"].ColumnName = "SUP_REPNAME";
                //dt.Columns["Total Quantity"].ColumnName = "PO_QUANTITY";
                //dt.Columns["Pending Qty for Testing"].ColumnName = "PENDING_QNTY";


                //dttesting.Columns["MAKE NAME"].SetOrdinal(3);
                List<string> listtoRemove = new List<string> { "DELIVERED_QNTY", "PENDING_QTY_EE" };
                // List<string> listtoRemove = new List<string>() ; 
                string filename = "PendingForTestingDetails" + DateTime.Now + ".xls";
                string pagetitle = "Pending For Testing Details";

                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No record found");
            }


        }

        protected void cmdDeliver_Click(object sender, EventArgs e)
        {
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
                foreach (GridViewRow row in grdPendingTc.Rows)
                {

                    if (((CheckBox)row.FindControl("chkSelect")).Checked == true)
                    {
                        strQryVallist[i] = ((Label)row.FindControl("lblRepairDetailsId")).Text.Trim();
                        AtleastOneApp = true;
                        txtPONo.Text = ((Label)row.FindControl("lblPONo")).Text.Trim();
                    }
                    i++;

                }

                grdPendingTc.AllowPaging = true;
                SaveCheckedValues();
                LoadTestingPendingTCDetails();
                PopulateCheckedValues();

                if (!AtleastOneApp)
                {
                    ShowMsgBox("Please Select DTr to Inspect");
                    SaveCheckedValues();
                    LoadTestingPendingTCDetails();
                    PopulateCheckedValues();
                    return;
                }

                string sSelectedValue = string.Empty;
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
                Response.Redirect("TCTesting.aspx?PoNo=" + sPONo, false);

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
                cmbCapacity.SelectedIndex = 0;
                //txtNoofDays.Text = string.Empty;
                txtPONo.Text = string.Empty;
                //cmbRepairer.SelectedIndex = 0;
                cmbMake.SelectedIndex = 0;
                //LoadPendingDetails();
                grdPendingTc.DataSource = null;
                grdPendingTc.DataBind();
                //cmbSupplier.SelectedIndex = 0;
                grdPendingTc.Visible = false;
                cmdDeliver.Visible = false;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdReset_Click");
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