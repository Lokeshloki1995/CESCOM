using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Globalization;

namespace IIITS.DTLMS.MasterForms
{
    public partial class DTCDetailsApproveView : System.Web.UI.Page
    {
        clsSession objSession;
        /// <summary>
        /// This Method used for page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

                if (!IsPostBack)
                {
                    LoadApprovePendingDtcDetails();
                    CheckAccessRights("4");
                    SetContralText();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        /// <summary>
        /// This Method used to fetch All the Dtc Details records for lt approve 
        /// </summary>
        public void LoadApprovePendingDtcDetails()
        {
            try
            {
                clsDtcDetailsApprove objDtcdetails = new clsDtcDetailsApprove();
                objDtcdetails.OfficeCode = objSession.OfficeCode;
                objDtcdetails.Status = "0,1";
                DataTable dtDetails = objDtcdetails.LoadApprovePendingDtcDetails(objDtcdetails);

                grdDTCDetailsApprove.DataSource = dtDetails;
                grdDTCDetailsApprove.DataBind();
                ViewState["DtcDetails"] = dtDetails;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                      System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }


        /// <summary>
        /// This method used to bind the LT Approved dtc details to grid
        /// </summary>
        public void LoadApprovedDtcDetails()
        {
            try
            {
                clsDtcDetailsApprove objapproveddetails = new clsDtcDetailsApprove();
                objapproveddetails.OfficeCode = objSession.OfficeCode;
                objapproveddetails.Status = "1";

                DataTable dtDetails = objapproveddetails.LoadApprovePendingDtcDetails(objapproveddetails);
                grdDTCDetailsApprove.DataSource = dtDetails;
                grdDTCDetailsApprove.DataBind();
                ViewState["DtcDetails"] = dtDetails;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }
        /// <summary>
        /// This Method Used to bind the pening dtc details for LT Approve
        /// </summary>
        public void LoadPendingDtcDetails()
        {
            try
            {
                clsDtcDetailsApprove objapproveddetails = new clsDtcDetailsApprove();
                objapproveddetails.OfficeCode = objSession.OfficeCode;
                objapproveddetails.Status = "0";
                DataTable dtDetails = objapproveddetails.LoadApprovePendingDtcDetails(objapproveddetails);
                grdDTCDetailsApprove.DataSource = dtDetails;
                grdDTCDetailsApprove.DataBind();
                ViewState["DtcDetails"] = dtDetails;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                      System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }


        /// <summary>
        /// This method used for generate the excel for dtc details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Export_ClickFailureEntry(object sender, EventArgs e)
        {
            try
            {

                DataTable dtDetails = (DataTable)ViewState["DtcDetails"];

                if (dtDetails.Rows.Count > 0)
                {

                    dtDetails.Columns["DT_CODE"].ColumnName = "DTC CODE";
                    dtDetails.Columns["DT_NAME"].ColumnName = "DTC NAME";
                    dtDetails.Columns["DT_TC_ID"].ColumnName = "DTR CODE";
                    dtDetails.Columns["TC_CAPACITY"].ColumnName = "DTR CAPACITY";
                    dtDetails.Columns["DT_CRON"].ColumnName = "CREATED ON";
                    dtDetails.Columns["METERS_AVAILABLE"].ColumnName = "DTC METERING";
                    dtDetails.Columns["METERS_STATUS"].ColumnName = "METER STATUS";
                    dtDetails.Columns["CT_RATIO"].ColumnName = "CT RATIO";
                    dtDetails.Columns["WIRING"].ColumnName = "WIRING";
                    dtDetails.Columns["MODEM"].ColumnName = "MODEM";
                    dtDetails.Columns["APPROVE_STATUS"].ColumnName = "STATUS";
                    dtDetails.Columns["METER_MAKE"].ColumnName = "METER MAKE";
                    dtDetails.Columns["DT_METER_SLNO"].ColumnName = "SL NO";
                    dtDetails.Columns["METER_RECORDING"].ColumnName = "METER RECORDING";
                    dtDetails.Columns["DT_METER_REMARKS"].ColumnName = "REMARKS";


                    List<string> listtoRemove = new List<string> { "DT_ID", "DT_DTCMETERS", "DT_METER_STATUS",
                    "DT_CT_RATIO", "DT_WIRING", "DT_MODEM", "DT_LTSTATUS","DT_METER_MAKE","DT_METER_RECORDING" };
                    string filename = "DTCDetails" + DateTime.Now + ".xls";
                    string pagetitle = "DTC Details View";

                    Genaral.getexcel(dtDetails, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                      System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }



        /// <summary>
        /// This method used for grid page indexing 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdDTCDetailsApprove_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdDTCDetailsApprove.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["DtcDetails"];
                grdDTCDetailsApprove.DataSource = SortDataTable(dt as DataTable, true);
                grdDTCDetailsApprove.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                      System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        /// <summary>
        /// This method used for grid sorting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdDTCDetailsApprove_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdDTCDetailsApprove.PageIndex;
            DataTable dt = (DataTable)ViewState["DtcDetails"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {
                grdDTCDetailsApprove.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdDTCDetailsApprove.DataSource = dt;
            }
            grdDTCDetailsApprove.DataBind();
            grdDTCDetailsApprove.PageIndex = pageIndex;
        }
        /// <summary>
        /// This method used to sort the data table
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="isPageIndexChanging"></param>
        /// <returns></returns>
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
                        ViewState["DtcDetails"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["DtcDetails"] = dataView.ToTable();

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
        /// <summary>
        /// This method used for grid row command 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdDTCDetailsApprove_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "CreateNew")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    Label lblDtcCode = (Label)row.FindControl("lblDtcCode");
                    Label lbldtid = (Label)row.FindControl("lbldtid");

                    string DtcCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblDtcCode.Text));
                    string Dtid = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lbldtid.Text));

                    Response.Redirect("~/MasterForms/DTCCommision.aspx?DtcCode=" + DtcCode + "&Dtid=" + Dtid, false);
                }
                if (e.CommandName == "View")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    Label lblDtcCode = (Label)row.FindControl("lblDtcCode");
                    Label lbldtid = (Label)row.FindControl("lbldtid");
                    Label lblStatus = (Label)row.FindControl("lblApprovestatus");
                    string DtcCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblDtcCode.Text));
                    string Dtid = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lbldtid.Text));
                    string Status = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblStatus.Text));

                    Response.Redirect("~/MasterForms/DTCCommision.aspx?DtcCode="
                        + DtcCode + "&Dtid=" + Dtid + "&Status=" + Status, false);
                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDtCode = (TextBox)row.FindControl("txtDtCode");
                    TextBox txtDtName = (TextBox)row.FindControl("txtDtName");

                    DataTable dt = (DataTable)ViewState["DtcDetails"];
                    dv = dt.DefaultView;

                    if (txtDtCode.Text != "")
                    {
                        //code to fetch if the  dtc has been bifurcated .
                        clsDtcMaster objDtc = new clsDtcMaster();
                        objDtc.sDtcCode = txtDtCode.Text;
                        objDtc.sOfficeCode = objSession.OfficeCode;
                        string sDTCCode = objDtc.GetNewDTCCode(objDtc);
                        if (!(sDTCCode == "" || sDTCCode == null))
                        {
                            string msg = "OLD DTC Code =  " + txtDtCode.Text + "  NEW DTC Code =  " + sDTCCode + " ";
                            ShowMsgBox(msg);
                            sFilter = "DT_CODE Like '%" + sDTCCode.Replace("'", "'") + "%' AND";
                        }
                        else
                        {
                            sFilter = "DT_CODE Like '%" + txtDtCode.Text.Replace("'", "'") + "%' AND";
                        }
                    }
                    if (txtDtName.Text != "")
                    {
                        sFilter += " DT_NAME Like '%" + txtDtName.Text.Replace("'", "`") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdDTCDetailsApprove.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdDTCDetailsApprove.DataSource = dv;
                            ViewState["DtcDetails"] = dv.ToTable();
                            grdDTCDetailsApprove.DataBind();

                        }
                        else
                        {
                            ViewState["DtcDetails"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        if (rdbAlready.Checked == true)
                        {
                            LoadApprovedDtcDetails();
                        }
                        else if (rdbViewAll.Checked == true)
                        {
                            LoadApprovePendingDtcDetails();
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
        /// this method used for get all dtc details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdbViewAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                LoadApprovePendingDtcDetails();
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
        /// this method used for approved dtc details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdbAlready_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                LoadApprovedDtcDetails();
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
        /// this method used for calling pending dtc details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdbPending_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                LoadPendingDtcDetails();
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
        /// this method used for grid  row data bound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdDTCDetailsApprove_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    LinkButton lnkUpdate = (LinkButton)e.Row.FindControl("lnkUpdate");
                    LinkButton lnkCreate = (LinkButton)e.Row.FindControl("lnkCreate");
                    Label lblStatus = (Label)e.Row.FindControl("lblApprovestatus");
                    Label lblDtcCode = (Label)e.Row.FindControl("lblDtcCode");
                    LinkButton lnkView = (LinkButton)e.Row.FindControl("lnkView");


                    if (lblStatus.Text == "APPROVED")
                    {
                        lnkView.Visible = true;
                        lnkCreate.Visible = false;
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
        /// This method used for check the access rights
        /// </summary>
        /// <param name="sAccessType"></param>
        /// <returns></returns>
        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "DTCDetailsApproveView";
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
                clsException.LogError(ex.StackTrace, ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                     System.Reflection.MethodBase.GetCurrentMethod().Name);
                return false;

            }
        }

        #endregion
        /// <summary>
        /// This method used to show pop up message
        /// </summary>
        /// <param name="sMsg"></param>
        private void ShowMsgBox(string sMsg)
        {
            try
            {
                String sShowMsg = string.Empty;
                sShowMsg = "<script language=javascript> alert ('" + sMsg + "')</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "Msg", sShowMsg);
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
        /// This method used to show empty grid
        /// </summary>
        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("DT_ID");
                dt.Columns.Add("DT_CODE");
                dt.Columns.Add("DT_NAME");
                dt.Columns.Add("APPROVE_STATUS");
                dt.Columns.Add("DT_CRON");

                grdDTCDetailsApprove.DataSource = dt;
                grdDTCDetailsApprove.DataBind();

                int iColCount = grdDTCDetailsApprove.Rows[0].Cells.Count;
                grdDTCDetailsApprove.Rows[0].Cells.Clear();
                grdDTCDetailsApprove.Rows[0].Cells.Add(new TableCell());
                grdDTCDetailsApprove.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdDTCDetailsApprove.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);

            }
        }
        public void SetContralText()
        {
            try
            {
                if(objSession.RoleId=="4")
                {
                    grdDTCDetailsApprove.Columns[6].Visible = false;
                }
            }
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

    }
}