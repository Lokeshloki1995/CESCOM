using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Globalization;

namespace IIITS.DTLMS.DTCFailure
{
    public partial class WorkOrderView : System.Web.UI.Page
    {
        string strFormCode = "WorkOrderView";
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

                if (!IsPostBack)
                {
                    if (rdbAlready.Checked == true)
                    {
                        LoadWOAlreadyCreated("1");
                    }
                    else
                    {
                        LoadAllWorkOrder("1");
                    }
                    CheckAccessRights("4");
                }
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "Page_Load");

            }
        }

        protected void cmdNew_Click(object sender, EventArgs e)
        {
            try
            {
                //Check AccessRights
                bool bAccResult = CheckAccessRights("2");
                if (bAccResult == false)
                {
                    return;
                }
                string sType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(cmbType.SelectedValue));
                Response.Redirect("WorkOrder.aspx?TypeValue=" + sType, false);

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdNew_Click");
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
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ShowMsgBox");
            }
        }

        public void LoadWOAlreadyCreated(string sType)
        {

            try
            {
                clsWorkOrder objWO= new clsWorkOrder();

                objWO.sTaskType = sType;
                objWO.sOfficeCode = objSession.OfficeCode;

                DataTable dt = objWO.LoadAlreadyWorkOrder(objWO);
                grdWorkOrder.DataSource = dt;
                grdWorkOrder.DataBind();
                ViewState["Workorder"] = dt;
                if (sType == "1")
                {
                    lblGridType.Text = "DTC Failure WorkOrder Details :";
                }
                else if (sType == "2")
                {
                    lblGridType.Text = "DTC Enhancement WorkOrder Details :";
                }
                else if (sType == "4")
                {
                    lblGridType.Text = "DTC Failure With Enhancement WorkOrder Details :";
                }
              
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadWOAlreadyCreated");
            }
        }

        protected void Export_ClickWorkorder(object sender, EventArgs e)
        {
            //clsWorkOrder objWO = new clsWorkOrder();
            //string sType = "";

            //if (cmbType.SelectedValue == "1")
            //{
            //    sType = "1";
            //}
            // if (cmbType.SelectedValue == "2")
            //{
            //    sType = "2";
            //}
            // if (cmbType.SelectedValue == "4")
            //{
            //    sType = "4";
            //}
            //else
            //{
            //    cmbExport.Visible = false;
            //}

            //objWO.sTaskType = sType;
            //objWO.sOfficeCode = objSession.OfficeCode;

            //DataTable dt = objWO.LoadAlreadyWorkOrder(objWO);

            DataTable dt = (DataTable)ViewState["Workorder"];

            if (dt.Rows.Count > 0)
            {

                dt.Columns["DT_CODE"].ColumnName = "DTC Code";
                dt.Columns["DT_NAME"].ColumnName = "DTC Name";
                dt.Columns["TC_CODE"].ColumnName = "DTR Code";
                dt.Columns["WO_NO"].ColumnName = "WO No";

                List<string> listtoRemove = new List<string> { "DF_ID", "STATUS" };
                string filename = "WorkOrder" + DateTime.Now + ".xls";
                string pagetitle = " Work Order View";

                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No record found");
                ShowEmptyGrid();
            }



        }

        public void LoadAllWorkOrder(string sType)
        {

            try
            {
                clsWorkOrder objWO = new clsWorkOrder();
                string sMsg = string.Empty;

                objWO.sTaskType = sType;
                objWO.sOfficeCode = objSession.OfficeCode;

                DataTable dt = objWO.LoadAllWorkOrder(objWO);

                //To show the Type of Gridview
                if (sType == "1")
                {
                    //Gridview column visible true/false based on conditions
                    grdWorkOrder.Columns[0].Visible = true;
                    grdWorkOrder.Columns[1].Visible = false;

                    lblGridType.Text = "DTC Failure WorkOrder Details :";
                    sMsg = "Failure";
                }
                else if (sType == "2")
                {
                    //Gridview column visible true/false based on conditions
                    grdWorkOrder.Columns[1].Visible = true;
                    grdWorkOrder.Columns[0].Visible = false;

                    lblGridType.Text = "DTC Enhancement WorkOrder Details :";
                    sMsg = "Enhancement";
                }
                else if (sType == "4")
                {
                    //Gridview column visible true/false based on conditions
                    grdWorkOrder.Columns[1].Visible = true;
                    grdWorkOrder.Columns[0].Visible = false;

                    lblGridType.Text = "DTC Failure with Enhancement WorkOrder Details :";
                    sMsg = "Enhancement";
                }
                

                if (dt.Rows.Count > 0)
                {
                    grdWorkOrder.DataSource = dt;
                    grdWorkOrder.DataBind();
                    ViewState["Workorder"] = dt;
                }
                else
                {
                    
                    lblMessage.Text = "Note : No " + sMsg + " DTC Available Please Declare the DTC " + sMsg + " before creating a Work Order";
                    cmdNew.Visible = false;
                    grdWorkOrder.DataSource = dt;
                    grdWorkOrder.DataBind();
                }

                
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadAllWorkOrder");
            }
        }

       

       

        protected void grdWorkOrder_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Create" || e.CommandName == "CreateNew")
                {

                    if (e.CommandName == "CreateNew")
                    {
                        //Check AccessRights
                        bool bAccResult = CheckAccessRights("2");
                        if (bAccResult == false)
                        {
                            return;
                        }
                    }

                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    //It should be Either Failure or Enhancement Id
                    Label lblFailureId = (Label)row.FindControl("lblFailureId");

                    string sReferId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblFailureId.Text));
                    string sType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(cmbType.SelectedValue));

                    Response.Redirect("WorkOrder.aspx?ReferID=" + sReferId + "&TypeValue=" + sType, false);

                }


                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtFailureId = (TextBox)row.FindControl("txtFailureId");
                    TextBox txtEnhanceId = (TextBox)row.FindControl("txtEnhanceId");
                    TextBox txtDtCode = (TextBox)row.FindControl("txtDtcCode");
                    TextBox txtDtName = (TextBox)row.FindControl("txtdtcName");
                    TextBox txtDtrCode = (TextBox)row.FindControl("txtDtrCode");

                    DataTable dt = (DataTable)ViewState["Workorder"];
                    dv = dt.DefaultView;
                    if (txtFailureId.Text != "")
                    {
                        sFilter = "DF_ID Like '%" + txtFailureId.Text.Replace("'", "`") + "%' AND";
                    }
                    if (txtEnhanceId.Text != "")
                    {
                        sFilter = "DF_ID Like '%" + txtEnhanceId.Text.Replace("'", "`") + "%' AND";
                    }
                    if (txtDtCode.Text != "")
                    {
                        sFilter = "DT_CODE Like '%" + txtDtCode.Text.Replace("'", "`") + "%' AND";
                    }
                    if (txtDtName.Text != "")
                    {
                        sFilter += " DT_NAME Like '%" + txtDtName.Text.Replace("'", "`") + "%' AND";
                    }
                    if (txtDtrCode.Text != "")
                    {
                        sFilter += " TC_CODE Like '%" + txtDtrCode.Text.Replace("'", "`") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdWorkOrder.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdWorkOrder.DataSource = dv;
                            ViewState["Workorder"] = dv.ToTable();
                            grdWorkOrder.DataBind();

                        }
                        else
                        {
                            ViewState["Workorder"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        if (rdbAlready.Checked == true)
                        {

                            LoadWOAlreadyCreated(cmbType.SelectedValue);
                        }
                        else if (rdbViewAll.Checked == true)
                        {
                            LoadAllWorkOrder(cmbType.SelectedValue);
                        }

                    }
                }

            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "grdWorkOrder_RowCommand");
            }
        }

        protected void grdWorkOrder_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdWorkOrder.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Workorder"];
                grdWorkOrder.DataSource = SortDataTable(dt as DataTable, true);
                grdWorkOrder.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "grdWorkOrder_PageIndexChanging");
            }
        }

        protected void grdWorkOrder_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdWorkOrder.PageIndex;
            DataTable dt = (DataTable)ViewState["Workorder"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {
                grdWorkOrder.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdWorkOrder.DataSource = dt;

            }
            grdWorkOrder.DataBind();
            grdWorkOrder.PageIndex = pageIndex;
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
                        ViewState["Workorder"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["Workorder"] = dataView.ToTable();

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

        protected void rdbAlready_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                LoadWOAlreadyCreated(cmbType.SelectedValue);
               
               
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "rdbAlready_CheckedChanged");
            }
        }

        protected void rdbViewAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                LoadAllWorkOrder(cmbType.SelectedValue);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "rdbViewAll_CheckedChanged");
            }
        }

        protected void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbType.SelectedValue == "1")
                {
                    rdbAlready.Visible = true;
                    grdWorkOrder.Visible = true;
                    grdNewDTC.Visible = false;

                    //Temp
                    rdbAlready.Checked = true;
                    rdbViewAll.Checked = false;
                    cmbExport.Visible = true;

                    if (rdbViewAll.Checked == true)
                    {
                        rdbViewAll_CheckedChanged(sender, e);
                    }
                    else
                    {
                        rdbAlready_CheckedChanged(sender, e);
                    }
                    cmdNew.Visible = false;
                }
                else if (cmbType.SelectedValue == "2")
                {
                    rdbAlready.Visible = true;
                    grdWorkOrder.Visible = true;
                    grdNewDTC.Visible = false;
                    cmbExport.Visible = true;

                    //Temp
                    rdbAlready.Checked = true;
                    rdbViewAll.Checked = false;

                    if (rdbViewAll.Checked == true)
                    {
                        rdbViewAll_CheckedChanged(sender, e);
                    }
                    else
                    {
                        rdbAlready_CheckedChanged(sender, e);
                    }
                    cmdNew.Visible = false;
                }
                else if (cmbType.SelectedValue == "4")
                {
                    rdbAlready.Visible = true;
                    grdWorkOrder.Visible = true;
                    grdNewDTC.Visible = false;
                    cmbExport.Visible = true;

                    //Temp
                    rdbAlready.Checked = true;
                    rdbViewAll.Checked = false;

                    if (rdbViewAll.Checked == true)
                    {
                        rdbViewAll_CheckedChanged(sender, e);
                    }
                    else
                    {
                        rdbAlready_CheckedChanged(sender, e);
                    }
                    cmdNew.Visible = false;
                }
                else
                {
                    cmbType.SelectedIndex = 0;
                    rdbAlready.Visible = false;
                    grdWorkOrder.Visible = false;
                    grdNewDTC.Visible = true;
                    rdbViewAll.Checked = true;
                    LoadNewDTCWorkOrder();
                    cmdNew.Visible = false;
                    cmbExport.Visible = false;
                    ShowMsgBox("This Functionality had moved to MMS .. Please Login to MMS to create New DTC");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmbType_SelectedIndexChanged");
            }
        }

        protected void grdWorkOrder_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    LinkButton lnkUpdate = (LinkButton)e.Row.FindControl("lnkUpdate");
                    LinkButton lnkCreate = (LinkButton)e.Row.FindControl("lnkCreate");
                    Label lblStatus = (Label)e.Row.FindControl("lblStatus");


                    if (lblStatus.Text == "YES")
                    {
                        lnkUpdate.Visible = true;
                        lnkCreate.Visible = false;
                    }
                    else
                    {
                        lnkUpdate.Visible = false;
                        lnkCreate.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "grdWorkOrder_RowDataBound");
            }
        }

        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("DF_ID");
                dt.Columns.Add("DT_CODE");
                dt.Columns.Add("DT_NAME");
                dt.Columns.Add("TC_CODE");
                dt.Columns.Add("STATUS");
                dt.Columns.Add("WO_NO");


                grdWorkOrder.DataSource = dt;
                grdWorkOrder.DataBind();

                int iColCount = grdWorkOrder.Rows[0].Cells.Count;
                grdWorkOrder.Rows[0].Cells.Clear();
                grdWorkOrder.Rows[0].Cells.Add(new TableCell());
                grdWorkOrder.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdWorkOrder.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ShowEmptyGrid");

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
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "CheckAccessRights");
                return false;

            }
        }

        #endregion

        #region NewDTC
     
        public void LoadNewDTCWorkOrder()
        {

            try
            {
                clsWorkOrder objWO = new clsWorkOrder();
                string sMsg = string.Empty;

                objWO.sOfficeCode = objSession.OfficeCode;
                DataTable dt = objWO.LoadNewDTCWO(objWO);
                lblGridType.Text = "New DTC Commission WorkOrder Details :";
                sMsg = "New DTC Commission WorkOrder Details ";
                if (dt.Rows.Count > 0)
                {
                    grdNewDTC.DataSource = dt;
                    grdNewDTC.DataBind();
                    ViewState["NewDTC"] = dt;
                }
                else
                {

                    lblMessage.Text = "Note : No " + sMsg  + " Available";
                    grdNewDTC.DataSource = dt;
                    grdNewDTC.DataBind();
                }


            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadNewDTCWorkOrder");
            }
        }
        protected void grdNewDTC_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdNewDTC.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["NewDTC"];
                grdNewDTC.DataSource = SortDataTable(dt as DataTable, true);
                grdNewDTC.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "grdNewDTC_PageIndexChanging");
            }
        }

        protected void grdNewDTC_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdNewDTC.PageIndex;
            DataTable dt = (DataTable)ViewState["NewDTC"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {
                grdNewDTC.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdNewDTC.DataSource = dt;

            }
            grdNewDTC.DataBind();
            grdNewDTC.PageIndex = pageIndex;
        }



        protected void grdNewDTC_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Create")
                {


                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    //It should be Either Failure or Enhancement Id
                    Label lblWOSlno = (Label)row.FindControl("lblWOSlno");

                    string sReferId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblWOSlno.Text));
                    string sType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(cmbType.SelectedValue));

                    Response.Redirect("WorkOrder.aspx?ReferID=" + sReferId + "&TypeValue=" + sType, false);

                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtWoNo = (TextBox)row.FindControl("txtWoNo");


                    DataTable dt = (DataTable)ViewState["NewDTC"];
                    dv = dt.DefaultView;
                    if (txtWoNo.Text != "")
                    {
                        sFilter = "WO_NO Like '%" + txtWoNo.Text.Replace("'", "`") + "%' AND";
                    }

                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdNewDTC.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdNewDTC.DataSource = dv;
                            ViewState["NewDTC"] = dv.ToTable();
                            grdNewDTC.DataBind();

                        }
                        else
                        {

                            ShowEmptyGridForNewDtc();
                        }
                    }
                    else
                    {
                        LoadNewDTCWorkOrder();

                    }

                }
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "grdNewDTC_RowCommand");
            }
        }

        public void ShowEmptyGridForNewDtc()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("WO_SLNO");
                dt.Columns.Add("WO_NO");
                dt.Columns.Add("WO_DATE");
                dt.Columns.Add("WO_ACC_CODE");



                grdNewDTC.DataSource = dt;
                grdNewDTC.DataBind();

                int iColCount = grdNewDTC.Rows[0].Cells.Count;
                grdNewDTC.Rows[0].Cells.Clear();
                grdNewDTC.Rows[0].Cells.Add(new TableCell());
                grdNewDTC.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdNewDTC.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ShowEmptyGridForNewDtc");

            }
        }

        #endregion

    
    }
}