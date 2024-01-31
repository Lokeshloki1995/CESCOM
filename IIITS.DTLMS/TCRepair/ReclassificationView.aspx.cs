using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Collections;

namespace IIITS.DTLMS.TCRepair
{
    public partial class ReclassificationView : System.Web.UI.Page
    {
        clsSession objSession;
        /// <summary>
        /// This function used for page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["UserId"] == null || Request.QueryString["UserId"].ToString() == "")
                {
                    if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                    {
                        Response.Redirect("~/Login.aspx", false);
                    }
                }

                if (Request.QueryString["UserId"] != null && Request.QueryString["UserId"].ToString() != "")
                {
                    string PId = Genaral.Decrypt(Request.QueryString["UserId"].ToString());
                    if (PId != null)
                    {
                        clsLogin objLogin = new clsLogin();
                        objSession = new clsSession();
                        objLogin.sUserId = PId;
                        objLogin.MMSUserLogin(objLogin);

                        if (objLogin.sMessage == null)
                        {

                            Session["FullName"] = objLogin.sFullName;
                            Session["ChangPwd"] = objLogin.sChangePwd;

                            if (objLogin.sOfficeCode == "0")
                            {
                                objLogin.sOfficeCode = "";
                            }

                            objSession.UserId = objLogin.sUserId;
                            objSession.FullName = objLogin.sFullName;
                            objSession.RoleId = objLogin.sRoleId;
                            objSession.OfficeCode = objLogin.sOfficeCode;
                            objSession.OfficeName = objLogin.sOfficeName;
                            objSession.Designation = objLogin.sDesignation;
                            objSession.OfficeNameWithType = objLogin.sOfficeNamewithType;
                            Session["ProjectType"] = "MMS";
                            Session["clsSession"] = objSession;
                            if (Session["ChangPwd"] == null || Session["ChangPwd"].ToString() == "")
                            {
                                Response.Redirect("~/MasterForms/ChangePassword.aspx", false);
                            }
                        }
                    }
                }

                lblMessage.Text = string.Empty;
                objSession = (clsSession)Session["clsSession"];
                if (!IsPostBack)
                {
                    if (CheckAccessRights("4"))
                    {
                        string strQry = string.Empty;
                        LoadTestingPendingDetails();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        
        /// <summary>
        /// This functon used for check access rights
        /// </summary>
        /// <param name="sAccessType"></param>
        /// <returns></returns>
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "Reclassification";
                objApproval.sRoleId = objSession.RoleId;
                objApproval.sAccessType = Constants.CheckAccessRights.CheckAccessRightsAll + "," + sAccessType;
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
                clsException.LogError(ex.StackTrace, ex.Message,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                return false;

            }
        }
        /// <summary>
        /// This function used to load the HT TEsting passed details
        /// </summary>
        private void LoadTestingPendingDetails()
        {
            try
            {
                clsDTrRepairActivity objDeliverPending = new clsDTrRepairActivity();
                objDeliverPending.sPurchaseOrderNo = "";

                objDeliverPending.sOfficeCode = objSession.OfficeCode;

                DataTable dt = new DataTable();
                DataTable dtpass = new DataTable();

                dt = objDeliverPending.LoadPendingForEETestingDetails(objDeliverPending);
                dtpass = objDeliverPending.LoadEETestingPassDetails(objDeliverPending);

                grdTestPending.DataSource = dt;
                grdTestPending.DataBind();
                ViewState["TestPending"] = dt;


                grdTestingPassEE.DataSource = dtpass;
                grdTestingPassEE.DataBind();
                ViewState["TestPass"] = dtpass;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        /// <summary>
        /// This function used to export excel the pending EE records
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                dt = objDeliverPending.LoadPendingForEETestingDetails(objDeliverPending);
            }
            if (dt.Rows.Count > 0)
            {

                dt.Columns["RSM_PO_NO"].ColumnName = "PO No";
                dt.Columns["PODATE"].ColumnName = "PO Date";
                dt.Columns["ISSUEDATE"].ColumnName = "Issue Date";
                dt.Columns["SUP_REPNAME"].ColumnName = "Supplier/Repairer";
                dt.Columns["PO_QUANTITY"].ColumnName = "Total Quantity";
                dt.Columns["INSPECTED_QNTY_HT"].ColumnName = "Inspected Qty By HT";
                dt.Columns["PENDING_QNTY"].ColumnName = "Qty Pending With EE For Type Reclassification";

                dt.Columns["PO No"].SetOrdinal(0);
                dt.Columns["PO Date"].SetOrdinal(1);
                dt.Columns["Issue Date"].SetOrdinal(2);
                dt.Columns["Supplier/Repairer"].SetOrdinal(3);
                dt.Columns["Total Quantity"].SetOrdinal(4);
                dt.Columns["Inspected Qty By HT"].SetOrdinal(5);
                dt.Columns["Qty Pending With EE For Type Reclassification"].SetOrdinal(6);

                List<string> listtoRemove = new List<string> { "RSM_ID", "EE_INS_QTY" };
                string filename = "PendingForEEVerificationTestingDetails" + DateTime.Now + ".xls";
                string pagetitle = "Pending for Item Type Reclassification Details";

                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No record found");
            }
        }
        /// <summary>
        /// This function used to export excel the EE testing done records
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Export_ClickPassedTesting(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            if (ViewState["TestPass"] != null)
            {
                dt = (DataTable)ViewState["TestPass"];
            }
            else
            {
                clsDTrRepairActivity objDeliverPending = new clsDTrRepairActivity();

                objDeliverPending.sOfficeCode = objSession.OfficeCode;
                objDeliverPending.sTestingDone = "1";
                objDeliverPending.sPurchaseOrderNo = "";
                dt = objDeliverPending.LoadPendingForEETestingDetails(objDeliverPending);
            }
            if (dt.Rows.Count > 0)
            {

                dt.Columns["RSM_PO_NO"].ColumnName = "PO No";
                dt.Columns["PODATE"].ColumnName = "PO Date  ";
                dt.Columns["ISSUEDATE"].ColumnName = "Issue Date";
                dt.Columns["SUP_REPNAME"].ColumnName = "Supplier/Repairer";
                dt.Columns["PO_QUANTITY"].ColumnName = "Total Quantity";
                dt.Columns["EE_INS_QTY"].ColumnName = "Inspected By EE";

                List<string> listtoRemove = new List<string> { "RSM_ID", "INSPECTED_QNTY_HT" };
                string filename = "PendingForEEVerificationTestingDetails" + DateTime.Now + ".xls";
                string pagetitle = "Pending For EE Verification Testing Details";

                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No record found");
            }
        }
       
        /// <summary>
        /// This function used for show the pop up message
        /// </summary>
        /// <param name="sMsg"></param>
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
                clsException.LogError(ex.StackTrace, ex.Message,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        
        /// <summary>
        /// Rowcommand of click on edit button in view page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdTestingPass_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Recieve")
                {

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    Label lblRepairMasterId = (Label)row.FindControl("lblRepairMasterId");
                    Label lblpono = (Label)row.FindControl("lblpono");
                    
                    string sRepairMasterId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblRepairMasterId.Text));
                    string PoNo = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblpono.Text));
                    Response.Redirect("Reclassification.aspx?RepairMasterId=" + sRepairMasterId+ "&PoNo=" + PoNo, false);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        /// <summary>
        /// Used for populate the selected values
        /// </summary>
        private void PopulateCheckedValues()
        {
            try
            {
                ArrayList arrCheckedValues = (ArrayList)ViewState["CHECKED_ITEMS"];
                if (arrCheckedValues != null && arrCheckedValues.Count > 0)
                {
                    foreach (GridViewRow gvrow in grdTestPending.Rows)
                    {
                        int index = Convert.ToInt32(grdTestPending.DataKeys[gvrow.RowIndex].Values[0]);
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
                clsException.LogError(ex.StackTrace, ex.Message,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name);

            }
        }

        /// <summary>
        /// this function used for page indexing pending records for ee
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                clsException.LogError(ex.StackTrace, ex.Message,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        /// <summary>
        /// this function used for page indexing of ee approved records
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdTestingPassEE_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdTestingPassEE.PageIndex = e.NewPageIndex;
                PopulateCheckedValues();
                DataTable dt = (DataTable)ViewState["TestPass"];
                grdTestingPassEE.DataSource = SortDataTablePending(dt as DataTable, true);
                grdTestingPassEE.DataBind();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        /// <summary>
        /// this function used for grid sorting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// this function used for grid sorting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdTestingPassEE_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdTestPending.PageIndex;
            DataTable dt = (DataTable)ViewState["TestPass"];
            string sortingDirection = string.Empty;

            grdTestPending.DataSource = SortDataTablePending(dt as DataTable, false);
            grdTestPending.DataBind();
            grdTestPending.PageIndex = pageIndex;
        }
        /// <summary>
        /// this function used for grid sorting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected DataView SortDataTablePassed(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);

                        ViewState["TestPass"] = dataView.ToTable();
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());

                        ViewState["TestPass"] = dataView.ToTable();
                    }


                }
                return dataView;
            }
            else
            {
                return new DataView();
            }

        }  
        /// <summary>
           /// this function used for grid sorting
           /// </summary>
           /// <param name="sender"></param>
           /// <param name="e"></param>
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
        /// <summary>
        /// this function used for sort in ascending order
        /// </summary>
        private string GridViewSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "ASC"; }
            set { ViewState["SortDirection"] = value; }


        }
        /// <summary>
        /// this function used for sort in ascending AND descending order
        /// </summary>
        private string GridViewSortExpression
        {
            get { return ViewState["SortExpression"] as string ?? string.Empty; }
            set { ViewState["SortExpression"] = value; }
        }

        /// <summary>
        /// this function used for sort in descending order
        /// </summary>
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


    }
}