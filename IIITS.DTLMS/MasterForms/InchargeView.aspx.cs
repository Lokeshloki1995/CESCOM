using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using IIITS.DTLMS.BL;
using IIITS.DTLMS.BL.MasterForms;

namespace IIITS.DTLMS.MasterForms
{
    public partial class InchargeView : System.Web.UI.Page
    {
        clsSession objSession = new clsSession();
        string strFormCode = "InchargeView";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }

                objSession = (clsSession)Session["clsSession"];
                string stroffCode = string.Empty;
                cmbCircle.Attributes.Add("readonly", "readonly");
                cmbDivision.Attributes.Add("readonly", "readonly");
                cmbsubdivision.Attributes.Add("readonly", "readonly");
                cmbSection.Attributes.Add("readonly", "readonly");
                cmbCircle.Enabled = false;
                cmbDivision.Enabled = false;
                cmbsubdivision.Enabled = false;
                cmbSection.Enabled = false;

                if (objSession.OfficeCode.Length > 1)
                {
                    stroffCode = objSession.OfficeCode.Substring(0, 1);
                }
                else
                {
                    stroffCode = objSession.OfficeCode;
                }

                if (!IsPostBack)
                {
                    if (stroffCode == null || stroffCode == "")
                    {
                        Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME FROM TBLCIRCLE ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle);

                    }

                    if (stroffCode.Length >= 1)
                    {
                        Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME FROM TBLCIRCLE ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle);


                        cmbCircle.Items.FindByValue(stroffCode).Selected = true;
                        stroffCode = string.Empty;
                        stroffCode = objSession.OfficeCode;
                    }

                    if (stroffCode.Length >= 1)
                    {
                        Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION WHERE DIV_CICLE_CODE='" + cmbCircle.SelectedValue + "'", "--Select--", cmbDivision);
                        if (stroffCode.Length >= 2)
                        {
                            stroffCode = objSession.OfficeCode.Substring(0, 2);
                            cmbDivision.Items.FindByValue(stroffCode).Selected = true;
                            stroffCode = string.Empty;
                            stroffCode = objSession.OfficeCode;
                        }
                    }

                    if (stroffCode.Length >= 2)
                    {
                        Genaral.Load_Combo("SELECT SD_SUBDIV_CODE,SD_SUBDIV_NAME from TBLSUBDIVMAST WHERE SD_DIV_CODE='" + cmbDivision.SelectedValue + "'", "--Select--", cmbsubdivision);
                        if (stroffCode.Length >= 3)
                        {
                            stroffCode = objSession.OfficeCode.Substring(0, 3);
                            cmbsubdivision.Items.FindByValue(stroffCode).Selected = true;
                            stroffCode = string.Empty;
                            stroffCode = objSession.OfficeCode;
                        }
                    }
                    if (stroffCode.Length >= 3)
                    {
                        Genaral.Load_Combo("SELECT OM_CODE,OM_NAME FROM TBLOMSECMAST WHERE OM_SUBDIV_CODE='" + cmbsubdivision.SelectedValue + "'", "--Select--", cmbSection);
                        if (stroffCode.Length >= 4)
                        {
                            stroffCode = objSession.OfficeCode.Substring(0, 4);
                            cmbSection.Items.FindByValue(stroffCode).Selected = true;
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


        protected void cmdLoad_click(object sender,EventArgs e)
        {
            try
            {
                LoadUserDetails();
            }
            
           catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load");
            }
        }

        protected void cmdNew_Click(object sender, EventArgs e)
        {
            try
            {
                //Check AccessRights
                //bool bAccResult = CheckAccessRights("2");
                //if (bAccResult == false)
                //{
                //    return;
                //}
                Response.Redirect("Incharge.aspx", false);

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdNew_Click");
            }
        }
        public void LoadUserDetails()
        {
            try
            {
                clsIncharge objInchargeUser = new clsIncharge();
                DataTable dtInchargeUserDetails = new DataTable();
                objInchargeUser.sOfficeCode = objSession.OfficeCode;
                
                dtInchargeUserDetails = objInchargeUser.LoadUserGrid(objInchargeUser);
                if (dtInchargeUserDetails.Rows.Count == 0)
                {
                    ShowEmptyGrid();
                    ViewState["USER"] = dtInchargeUserDetails;

                }
                else
                {
                    grdUser.DataSource = dtInchargeUserDetails;
                    grdUser.DataBind();
                    ViewState["USER"] = dtInchargeUserDetails;
                }

            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadUserDetails");
            }
        }
        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("IOMD_AUTOGEN_OM_NUMBER");
                dt.Columns.Add("IOMD_MAN_OM_NUMBER");
                dt.Columns.Add("IOMD_OM_DATE");
                dt.Columns.Add("OFF_NAME");
                dt.Columns.Add("IOML_CHARGE_FROM_DATE");
                dt.Columns.Add("IOML_CHARGE_TO_DATE");
                dt.Columns.Add("ACTUAL_USER");

                dt.Columns.Add("HANDOVER_USER");

                grdUser.DataSource = dt;
                grdUser.DataBind();

                int iColCount = grdUser.Rows[0].Cells.Count;
                grdUser.Rows[0].Cells.Clear();
                grdUser.Rows[0].Cells.Add(new TableCell());
                grdUser.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdUser.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowEmptyGrid");

            }
        }
        protected void grdUser_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {



            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdUser_RowDataBound");
            }
        }

        protected void grdUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdUser.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["USER"];

                grdUser.DataSource = SortDataTable(dt as DataTable, true);
                grdUser.DataBind();
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdUser_PageIndexChanging");

            }
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
                        ViewState["USER"] = dataView.ToTable();


                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["USER"] = dataView.ToTable();

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

        protected void grdUser_Sorting(object sender, GridViewSortEventArgs e)
        {


            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdUser.PageIndex;
            DataTable dt = (DataTable)ViewState["USER"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {
                grdUser.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdUser.DataSource = dt;
            }
            grdUser.DataBind();
            grdUser.PageIndex = pageIndex;
        }


        protected void grdUser_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "create")
                {

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    string strUserId = ((Label)row.FindControl("lblUserId")).Text;
                    strUserId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(strUserId));
                    Response.Redirect("Incharge.aspx?QryUserId=" + strUserId + "", false);

                }


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdUser_RowCommand");
            }
        }

    }
}