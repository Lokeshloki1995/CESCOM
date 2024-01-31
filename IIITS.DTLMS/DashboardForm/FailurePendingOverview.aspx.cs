using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.IO;


namespace IIITS.DTLMS.DashboardForm
{
    public partial class FailurePendingOverview : System.Web.UI.Page
    {
        string strFormCode = "FailurePendingOverview";
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
                if (!IsPostBack)
                {
                    if (Request.QueryString["OfficeCode"] != null && Request.QueryString["OfficeCode"].ToString() != "")
                    {
                        hdfOffCode.Value = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["OfficeCode"]));
                    }
                    else
                    {
                        hdfOffCode.Value = objSession.OfficeCode;
                    }

                    LoadFailurePendingDetails();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load");
            }
        }

        public void LoadFailurePendingDetails()
        {
            try
            {
                clsDashboard objDashboard = new clsDashboard();
                DataTable dtLoadDetails = new DataTable();
                if (Request.QueryString["value"] == "Failure")
                {
                    dtLoadDetails = objDashboard.LoadFailurePendingDetails(hdfOffCode.Value);
                    grdFailurePending.Visible = true;
                    grdFailurePending.DataSource = dtLoadDetails;
                    grdFailurePending.DataBind();
                    ViewState["FailurePending"] = dtLoadDetails;
                    failure.Text = "Failure Pending Details";
                    failureText.Text = "Failure Pending Details";
                }

                if (Request.QueryString["value"] == "estimation")
                {
                    dtLoadDetails = objDashboard.LoadEstimationPendingDetails(hdfOffCode.Value);
                    grdEstimationPending.Visible = true;
                    grdEstimationPending.DataSource = dtLoadDetails;
                    grdEstimationPending.DataBind();
                    ViewState["estimation"] = dtLoadDetails;
                    failureText.Text = "Estimation Pending Details";
                    failure.Text = "Estimation Pending Details";
                }

                if (Request.QueryString["value"] == "workorder")
                {
                    dtLoadDetails = objDashboard.LoadWorkorderPendingDetails(hdfOffCode.Value);
                    grdWorkorderPending.Visible = true;
                    grdWorkorderPending.DataSource = dtLoadDetails;
                    grdWorkorderPending.DataBind();
                    ViewState["workorder"] = dtLoadDetails;
                    failureText.Text = "Workorder Pending Details";
                    failure.Text = "Workorder Pending Details";
                }

                if (Request.QueryString["value"] == "indent")
                {
                    dtLoadDetails = objDashboard.LoadIndentPendingDetails(hdfOffCode.Value);
                    grdIndentPending.Visible = true;
                    grdIndentPending.DataSource = dtLoadDetails;
                    grdIndentPending.DataBind();
                    ViewState["indent"] = dtLoadDetails;
                    failureText.Text = "Indent Pending Details";
                    failure.Text = "Indent Pending Details";
                }

                if (Request.QueryString["value"] == "invoice")
                {
                    dtLoadDetails = objDashboard.LoadInvoicePendingDetails(hdfOffCode.Value);
                    grdinvoicePending.Visible = true;
                    grdinvoicePending.DataSource = dtLoadDetails;
                    grdinvoicePending.DataBind();
                    ViewState["invoice"] = dtLoadDetails;
                    failureText.Text = "Invoice Pending Details";
                    failure.Text = "Invoice Pending Details";
                }

                if (Request.QueryString["value"] == "DeCommission")
                {
                    dtLoadDetails = objDashboard.LoadDeCommissionPendingDetails(hdfOffCode.Value);
                    grdDecommissionPending.Visible = true;
                    grdDecommissionPending.DataSource = dtLoadDetails;
                    grdDecommissionPending.DataBind();
                    ViewState["invoice"] = dtLoadDetails;
                    failureText.Text = "RI Pending Details";
                    failure.Text = "RI Pending Details";
                }

                if (Request.QueryString["value"] == "RI")
                {
                    dtLoadDetails = objDashboard.LoadRIPendingDetails(hdfOffCode.Value);
                    grdRIPending.Visible = true;
                    grdRIPending.DataSource = dtLoadDetails;
                    grdRIPending.DataBind();
                    ViewState["RI"] = dtLoadDetails;
                    failureText.Text = "RV Pending Details";
                    failure.Text = "RV Pending Details";
                }

                if (Request.QueryString["value"] == "CR")
                {
                    dtLoadDetails = objDashboard.LoadCRPendingDetails(hdfOffCode.Value);
                    grdRIPending.Visible = true;
                    grdRIPending.DataSource = dtLoadDetails;
                    grdRIPending.DataBind();
                    ViewState["CR"] = dtLoadDetails;
                    failureText.Text = "CR Pending Details";
                    failure.Text = "CR Pending Details";
                }
                if (Request.QueryString["value"] == "InvoiceTCDetails")
                {

                    string WOSLNO = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["WOSLNO"]));
                    dtLoadDetails = objDashboard.GetStore_TcDetails(hdfOffCode.Value, WOSLNO);
                    grdInvoiceTCDetails.Visible = true;
                    grdInvoiceTCDetails.DataSource = dtLoadDetails;
                    grdInvoiceTCDetails.DataBind();
                    ViewState["TCdetails"] = dtLoadDetails;
                    failureText.Text = "Transformer Available in Store";
                    failure.Text = "Transformer Available in Store";
                }

            }
            catch (Exception ex)
            {

            }
        }




        protected void grdFailurePending_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDtCode = (TextBox)row.FindControl("txtDtCode");
                    TextBox txtDTCName = (TextBox)row.FindControl("txtDTCName");

                    DataTable dt = (DataTable)ViewState["FailurePending"];
                    dv = dt.DefaultView;

                    if (txtDtCode.Text != "")
                    {
                        sFilter = "DT_CODE Like '%" + txtDtCode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txtDTCName.Text != "")
                    {
                        sFilter += " DT_NAME Like '%" + txtDTCName.Text.Replace("'", "'") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdFailurePending.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdFailurePending.DataSource = dv;
                            ViewState["FailurePending"] = dv.ToTable();
                            grdFailurePending.DataBind();

                        }
                        else
                        {
                            ViewState["FailurePending"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadFailurePendingDetails();
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdFailurePending_RowCommand");
            }
        }

        protected void grdEstimationPending_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDtCode = (TextBox)row.FindControl("txtDtCode");
                    TextBox txtDTCName = (TextBox)row.FindControl("txtDTCName");

                    DataTable dt = (DataTable)ViewState["estimation"];
                    dv = dt.DefaultView;

                    if (txtDtCode.Text != "")
                    {
                        sFilter = "DT_CODE Like '%" + txtDtCode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txtDTCName.Text != "")
                    {
                        sFilter += " DT_NAME Like '%" + txtDTCName.Text.Replace("'", "'") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdEstimationPending.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdEstimationPending.DataSource = dv;
                            ViewState["estimation"] = dv.ToTable();
                            grdEstimationPending.DataBind();

                        }
                        else
                        {
                            ViewState["estimation"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadFailurePendingDetails();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdEstimationPending_RowCommand");
            }
        }

        protected void grdWorkorderPending_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDtCode = (TextBox)row.FindControl("txtDtCode");
                    TextBox txtDTCName = (TextBox)row.FindControl("txtDTCName");

                    DataTable dt = (DataTable)ViewState["workorder"];
                    dv = dt.DefaultView;

                    if (txtDtCode.Text != "")
                    {
                        sFilter = "DT_CODE Like '%" + txtDtCode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txtDTCName.Text != "")
                    {
                        sFilter += " DT_NAME Like '%" + txtDTCName.Text.Replace("'", "'") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdWorkorderPending.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdWorkorderPending.DataSource = dv;
                            ViewState["workorder"] = dv.ToTable();
                            grdWorkorderPending.DataBind();

                        }
                        else
                        {
                            ViewState["workorder"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadFailurePendingDetails();
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdWorkorderPending_RowCommand");
            }
        }

        protected void grdIndentPending_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDtCode = (TextBox)row.FindControl("txtDtCode");
                    TextBox txtDTCName = (TextBox)row.FindControl("txtDTCName");

                    DataTable dt = (DataTable)ViewState["indent"];
                    dv = dt.DefaultView;

                    if (txtDtCode.Text != "")
                    {
                        sFilter = "DT_CODE Like '%" + txtDtCode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txtDTCName.Text != "")
                    {
                        sFilter += " DT_NAME Like '%" + txtDTCName.Text.Replace("'", "'") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdIndentPending.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdIndentPending.DataSource = dv;
                            ViewState["indent"] = dv.ToTable();
                            grdIndentPending.DataBind();

                        }
                        else
                        {
                            ViewState["indent"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadFailurePendingDetails();
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdIndentPending_RowCommand");
            }
        }

        protected void grdinvoicePending_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDtCode = (TextBox)row.FindControl("txtDtCode");
                    TextBox txtDTCName = (TextBox)row.FindControl("txtDTCName");

                    DataTable dt = (DataTable)ViewState["invoice"];
                    dv = dt.DefaultView;

                    if (txtDtCode.Text != "")
                    {
                        sFilter = "DT_CODE Like '%" + txtDtCode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txtDTCName.Text != "")
                    {
                        sFilter += " DT_NAME Like '%" + txtDTCName.Text.Replace("'", "'") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdinvoicePending.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdinvoicePending.DataSource = dv;
                            ViewState["invoice"] = dv.ToTable();
                            grdinvoicePending.DataBind();

                        }
                        else
                        {
                            ViewState["invoice"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadFailurePendingDetails();
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdinvoicePending_RowCommand");
            }
        }

        protected void grdDecommissionPending_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDtCode = (TextBox)row.FindControl("txtDtCode");
                    TextBox txtDTCName = (TextBox)row.FindControl("txtDTCName");

                    DataTable dt = (DataTable)ViewState["invoice"];
                    dv = dt.DefaultView;

                    if (txtDtCode.Text != "")
                    {
                        sFilter = "DT_CODE Like '%" + txtDtCode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txtDTCName.Text != "")
                    {
                        sFilter += " DT_NAME Like '%" + txtDTCName.Text.Replace("'", "'") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdDecommissionPending.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdDecommissionPending.DataSource = dv;
                            ViewState["invoice"] = dv.ToTable();
                            grdDecommissionPending.DataBind();

                        }
                        else
                        {
                            ViewState["invoice"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadFailurePendingDetails();
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdDecommissionPending_RowCommand");
            }
        }

        protected void grdRIPending_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDtCode = (TextBox)row.FindControl("txtDtCode");
                    TextBox txtDTCName = (TextBox)row.FindControl("txtDTCName");

                    DataTable dt = (DataTable)ViewState["RI"];

                    if(dt == null )
                    {
                         dt = (DataTable)ViewState["CR"];
                    }
                    dv = dt.DefaultView;

                    if (txtDtCode.Text != "")
                    {
                        sFilter = "DT_CODE Like '%" + txtDtCode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txtDTCName.Text != "")
                    {
                        sFilter += " DT_NAME Like '%" + txtDTCName.Text.Replace("'", "'") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdRIPending.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdRIPending.DataSource = dv;
                            if (ViewState["RI"] == null)
                            {
                                ViewState["CR"] = dv.ToTable();
                            }
                            else
                            {
                                ViewState["RI"] = dv.ToTable();
                            }
                            grdRIPending.DataBind();

                        }
                        else
                        {
                            if (ViewState["RI"] == null )
                            {
                                ViewState["CR"] = dv.ToTable();
                            }
                            else
                            {
                                ViewState["RI"] = dv.ToTable();
                            }
                           
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadFailurePendingDetails();
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdRIPending_RowCommand");
            }
        }

        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);

                if (Request.QueryString["value"] == "Failure")
                {
                    dt.Columns.Add("DT_CODE");
                    dt.Columns.Add("DT_NAME");
                    dt.Columns.Add("DIV");
                    dt.Columns.Add("SUBDIVSION");
                    dt.Columns.Add("OMSECTION");
                    dt.Columns.Add("DF_ID");
                    dt.Columns.Add("DF_DATE");
                    dt.Columns.Add("WO_NO");
                    dt.Columns.Add("WO_DATE");
                    dt.Columns.Add("GUARANTY_TYPE");
                    dt.Columns.Add("FAIL_CAPACITY");
                    dt.Columns.Add("INV_CAPACITY");
                    dt.Columns.Add("OM_CODE");
                   
                   
                  
                   
                    grdFailurePending.DataSource = dt;
                    grdFailurePending.DataBind();

                    int iColCount = grdFailurePending.Rows[0].Cells.Count;
                    grdFailurePending.Rows[0].Cells.Clear();
                    grdFailurePending.Rows[0].Cells.Add(new TableCell());
                    grdFailurePending.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdFailurePending.Rows[0].Cells[0].Text = "No Records Found";
                }

                if (Request.QueryString["value"] == "estimation")
                {
                    dt.Columns.Add("DT_CODE");
                    dt.Columns.Add("DT_NAME");
                    dt.Columns.Add("DIV");
                    dt.Columns.Add("SUBDIVSION");
                    dt.Columns.Add("OMSECTION");
                    dt.Columns.Add("DTRCODE");
                    dt.Columns.Add("EST_CRON");
                    dt.Columns.Add("FAILURECAPACITY");

                    dt.Columns.Add("INVOICECAPACITY");
                    dt.Columns.Add("FAILURETYPE");
                    dt.Columns.Add("GUARANTY_TYPE");
                    dt.Columns.Add("FL_STATUS");
                    grdEstimationPending.DataSource = dt;
                    grdEstimationPending.DataBind();

                    int iColCount = grdEstimationPending.Rows[0].Cells.Count;
                    grdEstimationPending.Rows[0].Cells.Clear();
                    grdEstimationPending.Rows[0].Cells.Add(new TableCell());
                    grdEstimationPending.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdEstimationPending.Rows[0].Cells[0].Text = "No Records Found";
                }
                if (Request.QueryString["value"] == "workorder")
                {
                    dt.Columns.Add("DT_CODE");
                    dt.Columns.Add("DT_NAME");
                    dt.Columns.Add("DIV");
                    dt.Columns.Add("SUBDIVSION");
                    dt.Columns.Add("OMSECTION");
                    dt.Columns.Add("WO_NO");
                    dt.Columns.Add("WO_DATE");
                    dt.Columns.Add("DECOMCAPACITY");
                    dt.Columns.Add("COMMCAPACITY");
                    dt.Columns.Add("FAILURETYPE");
                    dt.Columns.Add("GUARANTY_TYPE");
                    dt.Columns.Add("WO_STATUS");

                    grdWorkorderPending.DataSource = dt;
                    grdWorkorderPending.DataBind();

                    int iColCount = grdWorkorderPending.Rows[0].Cells.Count;
                    grdWorkorderPending.Rows[0].Cells.Clear();
                    grdWorkorderPending.Rows[0].Cells.Add(new TableCell());
                    grdWorkorderPending.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdWorkorderPending.Rows[0].Cells[0].Text = "No Records Found";
                }
                if (Request.QueryString["value"] == "indent")
                {
                    dt.Columns.Add("DT_CODE");
                    dt.Columns.Add("DT_NAME");
                    dt.Columns.Add("DIV");
                    dt.Columns.Add("SUBDIVSION");
                    dt.Columns.Add("OMSECTION");
                    dt.Columns.Add("WO_NO");
                    dt.Columns.Add("WO_NO_DECOM");
                    
                    dt.Columns.Add("TI_INDENT_DATE");

                    dt.Columns.Add("WO_DATE");
                    dt.Columns.Add("DECOMCAPACITY");
                    dt.Columns.Add("COMMCAPACITY");
                    dt.Columns.Add("FAILURETYPE");
                    dt.Columns.Add("GUARANTY_TYPE");
                    dt.Columns.Add("INDT_STATUS");

                    grdIndentPending.DataSource = dt;
                    grdIndentPending.DataBind();

                    int iColCount = grdIndentPending.Rows[0].Cells.Count;
                    grdIndentPending.Rows[0].Cells.Clear();
                    grdIndentPending.Rows[0].Cells.Add(new TableCell());
                    grdIndentPending.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdIndentPending.Rows[0].Cells[0].Text = "No Records Found";
                }
                if (Request.QueryString["value"] == "invoice")
                {
                    dt.Columns.Add("DT_CODE");
                    dt.Columns.Add("DT_NAME");
                    dt.Columns.Add("DIV");
                    dt.Columns.Add("SUBDIVSION");
                    dt.Columns.Add("OMSECTION");
                    dt.Columns.Add("WO_NO");
                    dt.Columns.Add("WO_NO_DECOM");
                    
                    dt.Columns.Add("WO_DATE");

                    dt.Columns.Add("IN_DATE");
                    dt.Columns.Add("DECOMCAPACITY");
                    dt.Columns.Add("COMMCAPACITY");
                    dt.Columns.Add("FAILURETYPE");
                    dt.Columns.Add("GUARANTY_TYPE");
                    dt.Columns.Add("INV_STATUS");

                    grdinvoicePending.DataSource = dt;
                    grdinvoicePending.DataBind();

                    int iColCount = grdinvoicePending.Rows[0].Cells.Count;
                    grdinvoicePending.Rows[0].Cells.Clear();
                    grdinvoicePending.Rows[0].Cells.Add(new TableCell());
                    grdinvoicePending.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdinvoicePending.Rows[0].Cells[0].Text = "No Records Found";
                }

                if (Request.QueryString["value"] == "DeCommission")
                {
                    dt.Columns.Add("DT_CODE");
                    dt.Columns.Add("DT_NAME");
                    dt.Columns.Add("DTR_CODE"); 
                    dt.Columns.Add("DIV");
                    dt.Columns.Add("SUBDIVSION");
                    dt.Columns.Add("OMSECTION");
                    dt.Columns.Add("WO_NO");
                    dt.Columns.Add("WO_NO_DECOM");
                    dt.Columns.Add("WO_DATE");
                    dt.Columns.Add("DECOMCAPACITY");
                    dt.Columns.Add("COMMCAPACITY");
                    dt.Columns.Add("FAILURETYPE");
                    dt.Columns.Add("GUARANTY_TYPE");
                    dt.Columns.Add("DECOMM_STATUS");

                    grdDecommissionPending.DataSource = dt;
                    grdDecommissionPending.DataBind();

                    int iColCount = grdDecommissionPending.Rows[0].Cells.Count;
                    grdDecommissionPending.Rows[0].Cells.Clear();
                    grdDecommissionPending.Rows[0].Cells.Add(new TableCell());
                    grdDecommissionPending.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdDecommissionPending.Rows[0].Cells[0].Text = "No Records Found";
                }
                if (Request.QueryString["value"] == "RI")
                {
                    dt.Columns.Add("DT_CODE");
                    dt.Columns.Add("DT_NAME");
                    dt.Columns.Add("DTR_CODE");
                    dt.Columns.Add("DIV");
                    dt.Columns.Add("SUBDIVSION");
                    dt.Columns.Add("OMSECTION");
                    dt.Columns.Add("WO_NO");
                    dt.Columns.Add("WO_NO_DECOM");
                    dt.Columns.Add("WO_DATE");
                    dt.Columns.Add("DECOMCAPACITY");
                    dt.Columns.Add("COMMCAPACITY");
                    dt.Columns.Add("TR_RI_DATE");
                    dt.Columns.Add("FAILURETYPE");
                    dt.Columns.Add("GUARANTY_TYPE");
                    dt.Columns.Add("RI_STATUS");
                    dt.Columns.Add("CR_STATUS");

                    grdRIPending.DataSource = dt;
                    grdRIPending.DataBind();

                    int iColCount = grdRIPending.Rows[0].Cells.Count;
                    grdRIPending.Rows[0].Cells.Clear();
                    grdRIPending.Rows[0].Cells.Add(new TableCell());
                    grdRIPending.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdRIPending.Rows[0].Cells[0].Text = "No Records Found";
                }

                if (Request.QueryString["value"] == "CR")
                {
                    dt.Columns.Add("DT_CODE");
                    dt.Columns.Add("DT_NAME");
                    dt.Columns.Add("DTR_CODE");
                    dt.Columns.Add("DIV");
                    dt.Columns.Add("SUBDIVSION");
                    dt.Columns.Add("OMSECTION");
                    dt.Columns.Add("WO_NO");
                    dt.Columns.Add("WO_NO_DECOM");
                    dt.Columns.Add("WO_DATE");
                    dt.Columns.Add("DECOMCAPACITY");
                    dt.Columns.Add("COMMCAPACITY");
                    dt.Columns.Add("TR_RI_DATE");
                    dt.Columns.Add("FAILURETYPE");
                    dt.Columns.Add("GUARANTY_TYPE");
                    dt.Columns.Add("RI_STATUS");
                    dt.Columns.Add("CR_STATUS");

                    grdRIPending.DataSource = dt;
                    grdRIPending.DataBind();

                    int iColCount = grdRIPending.Rows[0].Cells.Count;
                    grdRIPending.Rows[0].Cells.Clear();
                    grdRIPending.Rows[0].Cells.Add(new TableCell());
                    grdRIPending.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdRIPending.Rows[0].Cells[0].Text = "No Records Found";
                }
 


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowEmptyGrid");

            }
        }

        protected void grdFailurePending_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                grdFailurePending.PageIndex = e.NewPageIndex;
                dtComplete = (DataTable)ViewState["FailurePending"];
                grdFailurePending.DataSource = SortDataTablePending(dtComplete as DataTable, true);
                grdFailurePending.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdComplete_PageIndexChanging");
            }
        }

        protected void grdEstimationPending_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                grdEstimationPending.PageIndex = e.NewPageIndex;
                dtComplete = (DataTable)ViewState["estimation"];
                grdEstimationPending.DataSource = SortDataTableEstimation(dtComplete as DataTable, true);
                grdEstimationPending.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdComplete_PageIndexChanging");
            }
        }

        protected void grdWorkorderPending_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                grdWorkorderPending.PageIndex = e.NewPageIndex;
                dtComplete = (DataTable)ViewState["workorder"];
                grdWorkorderPending.DataSource = SortDataTableWorkorder(dtComplete as DataTable, true);
                grdWorkorderPending.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdComplete_PageIndexChanging");
            }
        }

        protected void grdIndentPending_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                grdIndentPending.PageIndex = e.NewPageIndex;
                dtComplete = (DataTable)ViewState["indent"];
                grdIndentPending.DataSource = SortDataTableIndent(dtComplete as DataTable, true);
                grdIndentPending.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdComplete_PageIndexChanging");
            }
        }

        protected void grdinvoicePending_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                grdinvoicePending.PageIndex = e.NewPageIndex;
                dtComplete = (DataTable)ViewState["invoice"];
                grdinvoicePending.DataSource = SortDataTableCommission(dtComplete as DataTable, true);
                grdinvoicePending.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdComplete_PageIndexChanging");
            }
        }

        protected void grdDecommissionPending_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                grdDecommissionPending.PageIndex = e.NewPageIndex;
                dtComplete = (DataTable)ViewState["invoice"];
                grdDecommissionPending.DataSource = SortDataTableDecommission(dtComplete as DataTable, true);
                grdDecommissionPending.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdDecommissionPending_PageIndexChanging");
            }
        }

        protected void grdRIPending_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                grdRIPending.PageIndex = e.NewPageIndex;
                if (ViewState["RI"] != null)
                    dtComplete = (DataTable)ViewState["RI"];
                else
                    dtComplete =(DataTable) ViewState["CR"];

                grdRIPending.DataSource = SortDataTableRi(dtComplete as DataTable, true);
                grdRIPending.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdComplete_PageIndexChanging");
            }
        }

        protected void grdFailurePending_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdFailurePending.PageIndex;
            DataTable dt = (DataTable)ViewState["FailurePending"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdFailurePending.DataSource = SortDataTablePending(dt as DataTable, false);
            }
            else
            {
                grdFailurePending.DataSource = dt;
            }
            grdFailurePending.DataBind();
            grdFailurePending.PageIndex = pageIndex;
        }
        protected void grdEstimationPending_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdEstimationPending.PageIndex;
            DataTable dt = (DataTable)ViewState["estimation"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdEstimationPending.DataSource = SortDataTableEstimation(dt as DataTable, false);
            }

            else
            {
                grdEstimationPending.DataSource = dt;
            }
            grdEstimationPending.DataBind();
            grdEstimationPending.PageIndex = pageIndex;
        }
        protected void grdWorkorderPending_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdWorkorderPending.PageIndex;
            DataTable dt = (DataTable)ViewState["workorder"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdWorkorderPending.DataSource = SortDataTableWorkorder(dt as DataTable, false);
            }
            else
            {
                grdWorkorderPending.DataSource = dt;
            }
            grdWorkorderPending.DataBind();
            grdWorkorderPending.PageIndex = pageIndex;
        }
        protected void grdIndentPending_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdIndentPending.PageIndex;
            DataTable dt = (DataTable)ViewState["indent"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdIndentPending.DataSource = SortDataTableIndent(dt as DataTable, false);
            }
            else
            {
                grdIndentPending.DataSource = dt;
            }

            grdIndentPending.DataBind();
            grdIndentPending.PageIndex = pageIndex;
        }
        protected void grdinvoicePending_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdinvoicePending.PageIndex;
            DataTable dt = (DataTable)ViewState["invoice"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {

                grdinvoicePending.DataSource = SortDataTableCommission(dt as DataTable, false);
            }
            else
            {
                grdinvoicePending.DataSource = dt;
            }
            grdinvoicePending.DataBind();
            grdinvoicePending.PageIndex = pageIndex;
        }
        protected void grdDecommissionPending_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdDecommissionPending.PageIndex;
            DataTable dt = (DataTable)ViewState["invoice"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdDecommissionPending.DataSource = SortDataTableDecommission(dt as DataTable, false);
            }
            else
            {
                grdDecommissionPending.DataSource = dt;
            }
            grdDecommissionPending.DataBind();
            grdDecommissionPending.PageIndex = pageIndex;
        }
        protected void grdRIPending_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdRIPending.PageIndex;
            DataTable dt = (DataTable)ViewState["RI"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdRIPending.DataSource = SortDataTableRi(dt as DataTable, false);
            }

            else
            {
                grdRIPending.DataSource = dt;

            }
            grdRIPending.DataBind();
            grdRIPending.PageIndex = pageIndex;
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
                        ViewState["FailurePending"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["FailurePending"] = dataView.ToTable();


                    }
                }
                return dataView;
            }
            else
            {
                return new DataView();
            }

        }
        protected DataView SortDataTableEstimation(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                        ViewState["estimation"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["estimation"] = dataView.ToTable();


                    }
                }
                return dataView;
            }
            else
            {
                return new DataView();
            }

        }
        protected DataView SortDataTableWorkorder(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                        ViewState["workorder"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["workorder"] = dataView.ToTable();


                    }
                }
                return dataView;
            }
            else
            {
                return new DataView();
            }

        }
        protected DataView SortDataTableIndent(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                        ViewState["indent"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["indent"] = dataView.ToTable();


                    }
                }
                return dataView;
            }
            else
            {
                return new DataView();
            }

        }
        protected DataView SortDataTableCommission(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                        ViewState["invoice"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["invoice"] = dataView.ToTable();


                    }
                }
                return dataView;
            }
            else
            {
                return new DataView();
            }

        }
        protected DataView SortDataTableDecommission(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                        ViewState["invoice"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["invoice"] = dataView.ToTable();


                    }
                }
                return dataView;
            }
            else
            {
                return new DataView();
            }

        }
        protected DataView SortDataTableRi(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                        ViewState["RI"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["RI"] = dataView.ToTable();


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

        protected void Export_clickFailurePendingOverview(object sender, EventArgs e)
        {
            clsDashboard objDashboard = new clsDashboard();
            DataTable dt = new DataTable();
            if (Request.QueryString["value"] == "Failure")
            {
                //dt = objDashboard.LoadFailurePendingDetails(hdfOffCode.Value);
                dt = (DataTable)ViewState["FailurePending"];

                if (dt.Rows.Count > 0)
                {
                    dt.Columns["DIV"].ColumnName = "DIVISION NAME";
                    dt.Columns["SUBDIVSION"].ColumnName = "SUBDIVSION NAME";
                    dt.Columns["OMSECTION"].ColumnName = "SECTION NAME";
                    dt.Columns["DT_CODE"].ColumnName = "DTC CODE";
                    dt.Columns["DT_NAME"].ColumnName = "DTC NAME";
                    dt.Columns["DF_ID"].ColumnName = "FAILURE NO";
                    dt.Columns["DF_DATE"].ColumnName = "FAILURE DATE";
                    dt.Columns["WO_NO"].ColumnName = "WO NUMBER";
                    dt.Columns["WO_DATE"].ColumnName = "WO DATE";
                    dt.Columns["GUARANTY_TYPE"].ColumnName = "Guaranty Type";
                    dt.Columns["FAIL_CAPACITY"].ColumnName = "Failure Capacity";
                    dt.Columns["INV_CAPACITY"].ColumnName = "Invoice Capacity";


                    dt.Columns["DIVISION NAME"].SetOrdinal(0);
                    dt.Columns["SUBDIVSION NAME"].SetOrdinal(1);
                    dt.Columns["SECTION NAME"].SetOrdinal(2);
                    dt.Columns["DTC CODE"].SetOrdinal(3);
                    dt.Columns["DTC NAME"].SetOrdinal(4);
                    dt.Columns["FAILURE NO"].SetOrdinal(5);
                    dt.Columns["FAILURE DATE"].SetOrdinal(6);
                    dt.Columns["WO NUMBER"].SetOrdinal(7);
                    dt.Columns["WO DATE"].SetOrdinal(8);
                    dt.Columns["Guaranty Type"].SetOrdinal(9);




                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "Failure" + DateTime.Now + ".xls";
                    string pagetitle = "Failure Pending Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }

            if (Request.QueryString["value"] == "estimation")
            {
                //dt = objDashboard.LoadEstimationPendingDetails(hdfOffCode.Value);
                dt = (DataTable)ViewState["estimation"];
                if (dt.Rows.Count > 0)
                {

                    dt.Columns["SUBDIVSION"].ColumnName = "SUBDIVSION NAME";
                    dt.Columns["OMSECTION"].ColumnName = "SECTION NAME";
                    dt.Columns["DT_CODE"].ColumnName = "DTC CODE";
                    dt.Columns["DT_NAME"].ColumnName = "DTC NAME";
                   
                   // dt.Columns["OMSECTION"].ColumnName = "SECTION NAME";
                    dt.Columns["EST_CRON"].ColumnName = "ESTIMATION DATE";
                    dt.Columns["FL_STATUS"].ColumnName = "ESTIMATION STATUS";
                    dt.Columns["GUARANTY_TYPE"].ColumnName = "Guaranty Type";


                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "estimation" + DateTime.Now + ".xls";
                    string pagetitle = "Estimation Pending Details";


                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }

            if (Request.QueryString["value"] == "workorder")
            {
                // dt = objDashboard.LoadWorkorderPendingDetails(hdfOffCode.Value);
                dt = (DataTable)ViewState["workorder"];
                if (dt.Rows.Count > 0)
                {

                    dt.Columns["DT_CODE"].ColumnName = "DTC CODE";
                    dt.Columns["DT_NAME"].ColumnName = "DTC NAME";
                    dt.Columns["DIV"].ColumnName = "DIVISION";
                    dt.Columns["SUBDIVSION"].ColumnName = "SUBDIVSION NAME";
                    dt.Columns["OMSECTION"].ColumnName = "SECTION NAME";
                    dt.Columns["WO_DATE"].ColumnName = "WORK ORDER DATE";
                    dt.Columns["WO_STATUS"].ColumnName = "WORK ORDER STATUS";
                    dt.Columns["WO_NO"].ColumnName = "WORK ORDER NO";
                    dt.Columns["GUARANTY_TYPE"].ColumnName = "Guaranty Type";

                    dt.Columns["WORK ORDER NO"].SetOrdinal(6);

                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "workorder" + DateTime.Now + ".xls";
                    string pagetitle = "Workorder Pending Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }

            if (Request.QueryString["value"] == "indent")
            {
                //dt = objDashboard.LoadIndentPendingDetails(hdfOffCode.Value);
                dt = (DataTable)ViewState["indent"];
                if (dt.Rows.Count > 0)
                {

                    dt.Columns["DT_CODE"].ColumnName = "DTC CODE";
                    dt.Columns["DT_NAME"].ColumnName = "DTC NAME";
                    dt.Columns["DIV"].ColumnName = "DIVISION";
                    dt.Columns["SUBDIVSION"].ColumnName = "SUBDIVSION NAME";
                    dt.Columns["OMSECTION"].ColumnName = "SECTION NAME";
                    dt.Columns["TI_INDENT_DATE"].ColumnName = "INDENT DATE";
                    dt.Columns["GUARANTY_TYPE"].ColumnName = "Guaranty Type";
                    dt.Columns["INDT_STATUS"].ColumnName = "INDENT STATUS";



                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "indent" + DateTime.Now + ".xls";
                    string pagetitle = "Indent Pending Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }

            if (Request.QueryString["value"] == "invoice")
            {
                // dt = objDashboard.LoadInvoicePendingDetails(hdfOffCode.Value);
                dt = (DataTable)ViewState["invoice"];
                if (dt.Rows.Count > 0)
                {

                    dt.Columns["DT_CODE"].ColumnName = "DTC CODE";
                    dt.Columns["DT_NAME"].ColumnName = "DTC NAME";
                    dt.Columns["DIV"].ColumnName = "DIVISION";
                    dt.Columns["SUBDIVSION"].ColumnName = "SUBDIVSION NAME";
                    dt.Columns["OMSECTION"].ColumnName = "SECTION NAME";
                    dt.Columns["IN_DATE"].ColumnName = "COMMISSION DATE";
                    dt.Columns["GUARANTY_TYPE"].ColumnName = "Guaranty Type";
                    dt.Columns["INV_STATUS"].ColumnName = "INVOICE STATUS";

                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "invoice" + DateTime.Now + ".xls";
                    string pagetitle = "Invoice Pending Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }

            if (Request.QueryString["value"] == "DeCommission")
            {
                // dt = objDashboard.LoadDeCommissionPendingDetails(hdfOffCode.Value);
                dt = (DataTable)ViewState["invoice"];
                if (dt.Rows.Count > 0)
                {

                    dt.Columns["DT_CODE"].ColumnName = "DTC CODE";
                    dt.Columns["DT_NAME"].ColumnName = "DTC NAME";
                    dt.Columns["DTR_CODE"].ColumnName = "DTR CODE";
                    dt.Columns["DIV"].ColumnName = "DIVISION";
                    dt.Columns["SUBDIVSION"].ColumnName = "SUBDIVSION NAME";
                    dt.Columns["OMSECTION"].ColumnName = "SECTION NAME";
                    dt.Columns["GUARANTY_TYPE"].ColumnName = "Guaranty Type";
                    dt.Columns["DECOMM_STATUS"].ColumnName = "RI STATUS";


                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "RI" + DateTime.Now + ".xls";
                    string pagetitle = "RI Pending Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }

            if (Request.QueryString["value"] == "RI")
            {
                // dt = objDashboard.LoadRIPendingDetails(hdfOffCode.Value);
                dt = (DataTable)ViewState["RI"];
                if (dt.Rows.Count > 0)
                {

                    dt.Columns["DT_CODE"].ColumnName = "DTC CODE";
                    dt.Columns["DT_NAME"].ColumnName = "DTC NAME";
                    dt.Columns["DTR_CODE"].ColumnName = "DTR CODE";
                    dt.Columns["DIV"].ColumnName = "DIVISION";
                    dt.Columns["SUBDIVSION"].ColumnName = "SUBDIVSION NAME";
                    dt.Columns["OMSECTION"].ColumnName = "SECTION NAME";
                    dt.Columns["GUARANTY_TYPE"].ColumnName = "Guaranty Type";
                    dt.Columns["TR_RI_DATE"].ColumnName = "RV DATE";
                    dt.Columns["RI_STATUS"].ColumnName = "RV STATUS";
                    dt.Columns["CR_STATUS"].ColumnName = "CR STATUS";

                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "RV" + DateTime.Now + ".xls";
                    string pagetitle = "RV Pending Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }

            if (Request.QueryString["value"] == "CR")
            {
                // dt = objDashboard.LoadRIPendingDetails(hdfOffCode.Value);
                dt = (DataTable)ViewState["CR"];
                if (dt.Rows.Count > 0)
                {

                    dt.Columns["DT_CODE"].ColumnName = "DTC CODE";
                    dt.Columns["DT_NAME"].ColumnName = "DTC NAME";
                    dt.Columns["DTR_CODE"].ColumnName = "DTR CODE";
                    dt.Columns["DIV"].ColumnName = "DIVISION";
                    dt.Columns["SUBDIVSION"].ColumnName = "SUBDIVSION NAME";
                    dt.Columns["OMSECTION"].ColumnName = "SECTION NAME";
                    dt.Columns["GUARANTY_TYPE"].ColumnName = "Guaranty Type";
                    dt.Columns["TR_RI_DATE"].ColumnName = "RV DATE";
                    dt.Columns["RI_STATUS"].ColumnName = "RV STATUS";
                    dt.Columns["CR_STATUS"].ColumnName = "CR STATUS";

                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "CR" + DateTime.Now + ".xls";
                    string pagetitle = "CR Pending Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }

            if (Request.QueryString["value"] == "InvoiceTCDetails")
            {
                dt = (DataTable)ViewState["TCdetails"];
                string sStoreName = dt.Rows[0]["SM_NAME"].ToString();
                string sCapacity = dt.Rows[0]["TC_CAPACITY"].ToString();
                dt.Columns.Remove("SM_NAME");
                if (dt.Rows.Count > 0)
                {

                    dt.Columns["TC_CODE"].ColumnName = "DTR CODE";
                    dt.Columns["TC_SLNO"].ColumnName = "DTR SLNO";
                    dt.Columns["TC_CAPACITY"].ColumnName = "DTR CAPACITY";
                    dt.Columns["STATUS"].ColumnName = "STATUS";

                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "DTRDetailsInStore" + DateTime.Now + ".xls";
                    string pagetitle = "DTR Details of " + sCapacity + " Capacity in " + sStoreName + " Store";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }
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

            }
        }

        protected void grdInvoiceTCDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txttccode = (TextBox)row.FindControl("TxtTcCode");
                    TextBox txtslno = (TextBox)row.FindControl("txtTCSLNO");

                    DataTable dt = (DataTable)ViewState["TCdetails"];
                    dv = dt.DefaultView;

                    if (txttccode.Text != "")
                    {
                        sFilter = "TC_CODE = '" + txttccode.Text.Replace("'", "'") + "' AND";
                    }
                    if (txtslno.Text != "")
                    {
                        sFilter += " TC_SLNO Like '%" + txtslno.Text.Replace("'", "'") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdInvoiceTCDetails.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdInvoiceTCDetails.DataSource = dv;
                            ViewState["TCdetails"] = dv.ToTable();
                            grdInvoiceTCDetails.DataBind();

                        }
                        else
                        {
                            ViewState["TCdetails"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadFailurePendingDetails();
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdDecommissionPending_RowCommand");
            }
        }

        protected void grdInvoiceTCDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                grdInvoiceTCDetails.PageIndex = e.NewPageIndex;
                dtComplete = (DataTable)ViewState["TCdetails"];
                grdInvoiceTCDetails.DataSource = dtComplete;
                grdInvoiceTCDetails.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdInvoiceTCDetails_PageIndexChanging");
            }
        }
    }
}