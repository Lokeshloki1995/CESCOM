using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IIITS.DTLMS.BL;
namespace IIITS.DTLMS.DTCFailure
{
    public partial class IndentView : System.Web.UI.Page
    {
        string strFormCode = "IndentView";
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
                        LoadIndentAlreadyCreated("1");
                    }
                    else
                    {
                        LoadAllIndent("1");
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

        public void LoadIndentAlreadyCreated(string sType)
        {

            try
            {
                clsIndent objIndent = new clsIndent();
                objIndent.sTasktype = sType;
                objIndent.sOfficeCode = objSession.OfficeCode;

                DataTable dt = objIndent.LoadAlreadyIndent(objIndent);
                grdIndent.DataSource = dt;
                grdIndent.DataBind();
                ViewState["Indent"] = dt;
                if (sType == "1")
                {
                    lblGridType.Text = "DTC Failure Indent Details :";
                }
                else if (sType == "2")
                {
                    lblGridType.Text = "DTC Enhancement Indent Details :";
                }
                else if (sType == "4")
                {
                    lblGridType.Text = "DTC Failure with Enhancement Indent Details :";
                }

            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadIndentAlreadyCreated");
            }
        }

        public void LoadAllIndent(string sType)
        {

            try
            {
                clsIndent objIndent = new clsIndent();
                string sMsg = string.Empty;

                objIndent.sTasktype = sType;
                objIndent.sOfficeCode = objSession.OfficeCode;
                DataTable dt = objIndent.LoadAllIndent(objIndent);

                //To show the Type of Gridview
                if (sType == "1")
                {
                    //Gridview column visible true/false based on conditions
                    grdIndent.Columns[0].Visible = true;
                    grdIndent.Columns[1].Visible = false;

                    lblGridType.Text = "DTC Failure Indent Details :";
                    sMsg = "Failure";
                }
                else if (sType == "2")
                {
                    //Gridview column visible true/false based on conditions
                    grdIndent.Columns[1].Visible = true;
                    grdIndent.Columns[0].Visible = false;

                    lblGridType.Text = "DTC Enhancement Indent Details :";
                    sMsg = "Enhancement";
                }
                else if (sType == "4")
                {
                    //Gridview column visible true/false based on conditions
                    grdIndent.Columns[1].Visible = true;
                    grdIndent.Columns[0].Visible = false;

                    lblGridType.Text = "DTC Failure with Enhancement Indent Details :";
                    sMsg = "Enhancement";
                }

                if (dt.Rows.Count > 0)
                {
                    grdIndent.DataSource = dt;
                    grdIndent.DataBind();
                    ViewState["Indent"] = dt;
                }
                else
                {
                    lblMessage.Text = "Note : No " + sMsg + " DTC Available Please Declare the DTC " + sMsg + " before creating a Indent";
                    grdIndent.DataSource = dt;
                    grdIndent.DataBind();
                }


            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadAllIndent");
            }
        }
        protected void Export_ClickIndent(object sender, EventArgs e)
        {
            //clsIndent objIndent = new clsIndent();
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


            //objIndent.sTasktype = sType;
            //objIndent.sOfficeCode = objSession.OfficeCode;

            //DataTable dt = objIndent.LoadAlreadyIndent(objIndent);

            DataTable dt = (DataTable)ViewState["Indent"];

            if (dt.Rows.Count > 0)
            {

                dt.Columns["DF_DTC_CODE"].ColumnName = "DTC Code";
                dt.Columns["TC_CODE"].ColumnName = "DTR Code";
                dt.Columns["WO_NO"].ColumnName = "Work Order No.";
                dt.Columns["TI_INDENT_NO"].ColumnName = "Indent NO";

                List<string> listtoRemove = new List<string> { "DF_ID", "TI_ID", "WO_SLNO", "STATUS", "DT_NAME" };
                string filename = "Indent" + DateTime.Now + ".xls";
                string pagetitle="Indent View";

                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No record found");
                ShowEmptyGrid();
            }



        }


        
     

        protected void grdIndent_RowCommand(object sender, GridViewCommandEventArgs e)
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
                    Label lblWOSlNo = (Label)row.FindControl("lblWOSlno");
                    Label lblIndentId = (Label)row.FindControl("lblIndentId");

                    string sReferId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblWOSlNo.Text));
                    string sType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(cmbType.SelectedValue));
                    string sIndentId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblIndentId.Text));

                    Response.Redirect("IndentCreation.aspx?ReferID=" + sReferId + "&TypeValue=" + sType  + "&IndentId=" + sIndentId, false);

                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtFailureId = (TextBox)row.FindControl("txtFailureId");
                    TextBox txtEnhanceId = (TextBox)row.FindControl("txtEnhanceId");
                    TextBox txtDtcCode = (TextBox)row.FindControl("txtDtcCode");
                    TextBox txtDtrCode = (TextBox)row.FindControl("txtDtrCode");
                    TextBox txtWoNo = (TextBox)row.FindControl("txtWoNo");

                    DataTable dt = (DataTable)ViewState["Indent"];
                    dv = dt.DefaultView;
                    if (txtFailureId.Text != "")
                    {
                        sFilter = "DF_ID Like '%" + txtFailureId.Text.Replace("'", "`") + "%' AND";
                    }
                    if (txtEnhanceId.Text != "")
                    {
                        sFilter = "DF_ID Like '%" + txtEnhanceId.Text.Replace("'", "`") + "%' AND";
                    }
                    if (txtDtcCode.Text != "")
                    {
                        sFilter = "DF_DTC_CODE Like '%" + txtDtcCode.Text.Replace("'", "`") + "%' AND";
                    }
                    if (txtDtrCode.Text != "")
                    {
                        sFilter += " TC_CODE Like '%" + txtDtrCode.Text.Replace("'", "`") + "%' AND";
                    }
                    if (txtWoNo.Text != "")
                    {
                        sFilter += " WO_NO Like '%" + txtWoNo.Text.Replace("'", "`") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdIndent.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdIndent.DataSource = dv;
                            ViewState["Indent"] = dv.ToTable();
                            grdIndent.DataBind();

                        }
                        else
                        {
                            ViewState["Indent"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        if (rdbAlready.Checked == true)
                        {

                            LoadIndentAlreadyCreated(cmbType.SelectedValue);
                        }
                        else if (rdbViewAll.Checked == true)
                        {
                            LoadAllIndent(cmbType.SelectedValue);
                        }

                    }
                }
               
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "grdIndent_RowCommand");
               
            }
        }

        protected void rdbAlready_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbType.SelectedValue != "3")
                {
                    LoadIndentAlreadyCreated(cmbType.SelectedValue);
                }
                else
                {
                    LoadNewDTCIndentAlreadyCreated();
                }
              


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
                if (cmbType.SelectedValue != "3")
                {
                    LoadAllIndent(cmbType.SelectedValue);
                }
                else
                {
                    LoadNewDTCAllIndent();
                }
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
                    grdIndent.Visible = true;
                    grdNewDTCIndent.Visible = false;

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
                    cmbType.SelectedIndex = 0;
                    ShowMsgBox("This Functionality had moved to MMS .. Please Login to MMS to create New DTC");
                }
                else if (cmbType.SelectedValue == "2")
                {
                    grdIndent.Visible = true;
                    grdNewDTCIndent.Visible = false;

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
                }
                else if (cmbType.SelectedValue == "4")
                {
                    grdIndent.Visible = true;
                    grdNewDTCIndent.Visible = false;
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
                }
                else
                {
                    grdIndent.Visible = false;
                    grdNewDTCIndent.Visible = true;
                    cmbExport.Visible = false;
                    if (rdbViewAll.Checked == true)
                    {
                        rdbViewAll_CheckedChanged(sender, e);
                    }
                    else
                    {
                        rdbAlready_CheckedChanged(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmbType_SelectedIndexChanged");
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
                Response.Redirect("IndentCreation.aspx?TypeValue=" + sType, false);

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdNew_Click");
            }
        }

        protected void grdIndent_RowDataBound(object sender, GridViewRowEventArgs e)
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
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "grdIndent_RowDataBound");
            }
        }

        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "IndentCreation";
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
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ShowMsgBox");
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
                dt.Columns.Add("TI_ID");
                dt.Columns.Add("WO_SLNO");
                dt.Columns.Add("DF_DTC_CODE");
                dt.Columns.Add("TC_CODE");
                dt.Columns.Add("WO_NO");
                dt.Columns.Add("STATUS");
                dt.Columns.Add("TI_INDENT_NO");

                grdIndent.DataSource = dt;
                grdIndent.DataBind();

                int iColCount = grdIndent.Rows[0].Cells.Count;
                grdIndent.Rows[0].Cells.Clear();
                grdIndent.Rows[0].Cells.Add(new TableCell());
                grdIndent.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdIndent.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ShowEmptyGrid");

            }
        }

        #region NewDTCIndent

        public void LoadNewDTCIndentAlreadyCreated()
        {

            try
            {
                clsIndent objIndent = new clsIndent();
              
                objIndent.sOfficeCode = objSession.OfficeCode;

                DataTable dt = objIndent.LoadAlreadyNewDTCIndent(objIndent);
                grdNewDTCIndent.DataSource = dt;
                grdNewDTCIndent.DataBind();
                ViewState["NewDTCIndent"] = dt;
               
                lblGridType.Text = "New DTC Commission Indent Details :";
               

            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadNewDTCIndentAlreadyCreated");
            }
        }

        public void LoadNewDTCAllIndent()
        {

            try
            {
                clsIndent objIndent = new clsIndent();
                string sMsg = string.Empty;

                objIndent.sOfficeCode = objSession.OfficeCode;
                DataTable dt = objIndent.LoadAllNewDTCIndent(objIndent);

                lblGridType.Text = "New DTC Commission Indent Details :";
                sMsg = "New DTC Commission ";
                if (dt.Rows.Count > 0)
                {
                    grdNewDTCIndent.DataSource = dt;
                    grdNewDTCIndent.DataBind();
                    ViewState["NewDTCIndent"] = dt;
                }
                else
                {
                    lblMessage.Text = "Note : No " + sMsg + " Available Please Declare the  " + sMsg + " Work Order before creating a Indent";
                    grdNewDTCIndent.DataSource = dt;
                    grdNewDTCIndent.DataBind();
                }


            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadNewDTCAllIndent");
            }
        }
        protected void grdNewDTCIndent_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdNewDTCIndent.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["NewDTCIndent"];
                grdNewDTCIndent.DataSource = dt;
                grdNewDTCIndent.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "grdNewDTCIndent_PageIndexChanging");
            }
        }

        protected void grdNewDTCIndent_RowCommand(object sender, GridViewCommandEventArgs e)
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
                    Label lblWOSlNo = (Label)row.FindControl("lblWOSlno1");
                    Label lblIndentId = (Label)row.FindControl("lblIndentId1");

                    string sReferId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblWOSlNo.Text));
                    string sType = HttpUtility.UrlEncode(Genaral.UrlEncrypt(cmbType.SelectedValue));
                    string sIndentId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblIndentId.Text));

                    Response.Redirect("IndentCreation.aspx?ReferID=" + sReferId + "&TypeValue=" + sType + "&IndentId=" + sIndentId, false);

                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtWoNo = (TextBox)row.FindControl("txtWoNo");


                    DataTable dt = (DataTable)ViewState["NewDTCIndent"];
                    dv = dt.DefaultView;
                    if (txtWoNo.Text != "")
                    {
                        sFilter = "WO_NO Like '%" + txtWoNo.Text.Replace("'", "`") + "%' AND";
                    }

                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdNewDTCIndent.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdNewDTCIndent.DataSource = dv;
                            ViewState["NewDTCIndent"] = dv.ToTable();
                            grdNewDTCIndent.DataBind();

                        }
                        else
                        {

                            ShowEmptyGridForNewDtc();
                        }
                    }
                    else
                    {
                        if (rdbAlready.Checked == true)
                        {

                            LoadNewDTCIndentAlreadyCreated();
                        }
                        else if (rdbViewAll.Checked == true)
                        {
                            LoadNewDTCAllIndent();
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "grdNewDTCIndent_RowCommand");

            }
        }

        protected void grdNewDTCIndent_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    LinkButton lnkUpdate = (LinkButton)e.Row.FindControl("lnkUpdate1");
                    LinkButton lnkCreate = (LinkButton)e.Row.FindControl("lnkCreate1");
                    Label lblStatus = (Label)e.Row.FindControl("lblStatus1");


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
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "grdNewDTCIndent_RowDataBound");
            }
        }

        public void ShowEmptyGridForNewDtc()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("TI_ID");
                dt.Columns.Add("WO_SLNO");
                dt.Columns.Add("WO_NO");
                dt.Columns.Add("WO_DATE");
                dt.Columns.Add("STATUS");
                dt.Columns.Add("TI_INDENT_NO");
                dt.Columns.Add("TI_INDENT_DATE");

                grdNewDTCIndent.DataSource = dt;
                grdNewDTCIndent.DataBind();

                int iColCount = grdNewDTCIndent.Rows[0].Cells.Count;
                grdNewDTCIndent.Rows[0].Cells.Clear();
                grdNewDTCIndent.Rows[0].Cells.Add(new TableCell());
                grdNewDTCIndent.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdNewDTCIndent.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ShowEmptyGridForNewDtc");

            }
        }
        #endregion

    

        protected void grdIndent_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdIndent.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Indent"];
                grdIndent.DataSource = SortDataTable(dt as DataTable, true);
                grdIndent.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "grdIndent_PageIndexChanging");
            }
        }

        protected void grdIndent_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdIndent.PageIndex;
            DataTable dt = (DataTable)ViewState["Indent"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {

                grdIndent.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdIndent.DataSource = dt;
            }
            grdIndent.DataBind();
            grdIndent.PageIndex = pageIndex;
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
                        ViewState["Indent"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["Indent"] = dataView.ToTable();

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

        
    }
}