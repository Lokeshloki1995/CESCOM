using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;

namespace IIITS.DTLMS.Internal
{
    public partial class StatusReport : System.Web.UI.Page
    {
        string strFormCode = "StatusReport";
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
                    if (Request.QueryString["RefId"] != null && Request.QueryString["RefId"].ToString() != "")
                    {
                        hdfRefId.Value = Request.QueryString["RefId"].ToString();
                    }
                    if (hdfRefId.Value == "Custom")
                    {
                        hdfFromDate.Value = Request.QueryString["FromDate"].ToString();
                        hdfToDate.Value = Request.QueryString["ToDate"].ToString();
                        hdfOfficeCode.Value = Request.QueryString["OffCode"].ToString();
                        hdfFeederCode.Value = Request.QueryString["FeederCode"].ToString();
                    }

                    if (hdfFeederCode.Value == "")
                    {
                        LoadStatusReportLocationWise(hdfOfficeCode.Value);
                    }
                    else
                    {
                        LoadStatusReportFeederWise();
                        cmdBack.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "Page_Load");
            }

        }
        public void LoadStatusReportLocationWise(string sOfficeCode="")
        {

            try
            {
                clsInterDashboard objDashboard = new clsInterDashboard();
                DataTable dt = new DataTable();
                bool bCurrentDate = false;

                if(hdfRefId.Value=="true")
                {
                    bCurrentDate = true;
                    lblEnumStatus.Text = "Enumeration Status Report as On " + System.DateTime.Now.ToString("dd-MMM-yyyy"); 
                }

                objDashboard.sOfficeCode = sOfficeCode;
                objDashboard.bCurrentDate = bCurrentDate;
                objDashboard.sFromDate = hdfFromDate.Value;
                objDashboard.sToDate = hdfToDate.Value;
                //objDashboard.sFeederCode = hdfFeederCode.Value;

                dt = objDashboard.LoadStatusReportLocationWise(objDashboard);
                if (dt.Rows.Count == 0)
                {

                    ShowEmptyGrid();
                    ViewState["StatusReport"] = dt;
                    grdStatusReport.Columns[2].Visible = false;
                    grdStatusReport.Columns[1].Visible = true;
                    grdStatusReport.Columns[8].Visible = true;
                }
                else
                {
                    grdStatusReport.DataSource = dt;
                    grdStatusReport.DataBind();
                    ViewState["StatusReport"] = dt;

                    grdStatusReport.Columns[2].Visible = false;
                    grdStatusReport.Columns[1].Visible = true;

                    if (sOfficeCode.Length == 2)
                    {
                        grdStatusReport.Columns[8].Visible = true;
                    }
                    else
                    {
                        grdStatusReport.Columns[8].Visible = false;
                    }
                }      
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadStatusReportLocationWise");
            }
        }

        public void LoadStatusReportFeederWise(string sOfficeCode = "")
        {

            try
            {
                clsInterDashboard objDashboard = new clsInterDashboard();
                DataTable dt = new DataTable();
                bool bCurrentDate = false;

                if (hdfRefId.Value == "true")
                {
                    bCurrentDate = true;
                }

                objDashboard.sOfficeCode = sOfficeCode;
                objDashboard.bCurrentDate = bCurrentDate;
                objDashboard.sFromDate = hdfFromDate.Value;
                objDashboard.sToDate = hdfToDate.Value;
                objDashboard.sFeederCode = hdfFeederCode.Value;

                dt = objDashboard.LoadStatusReportFeederWise(objDashboard);
                if (dt.Rows.Count == 0)
                {

                    ShowEmptyGrid();
                    ViewState["StatusReport"] = dt;
                    grdStatusReport.Columns[2].Visible = true;
                    grdStatusReport.Columns[1].Visible = false;
                    grdStatusReport.Columns[8].Visible = false;
                }
                else
                {
                    grdStatusReport.DataSource = dt;
                    grdStatusReport.DataBind();
                    ViewState["StatusReport"] = dt;
                    grdStatusReport.Columns[2].Visible = true;
                    grdStatusReport.Columns[1].Visible = false;
                    grdStatusReport.Columns[8].Visible = false;
                }
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadStatusReportFeederWise");
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
       
      
        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("OFFICECODE");
                dt.Columns.Add("LOCATION");
                dt.Columns.Add("TOTAL");
                dt.Columns.Add("QCPENDING");
                dt.Columns.Add("QCDONE");
                dt.Columns.Add("QCREJECT");
                dt.Columns.Add("PENDING_CLAR");
                dt.Columns.Add("FD_FEEDER_NAME");
                dt.Columns.Add("ENUMTYPE");

                grdStatusReport.DataSource = dt;
                grdStatusReport.DataBind();

                int iColCount = grdStatusReport.Rows[0].Cells.Count;
                grdStatusReport.Rows[0].Cells.Clear();
                grdStatusReport.Rows[0].Cells.Add(new TableCell());
                grdStatusReport.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdStatusReport.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ShowEmptyGrid");

            }
        }

        protected void grdStatusReport_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "view")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    string sOfficeCode = ((Label)row.FindControl("lblOffCode")).Text;
                    hdfOfficeCode.Value = sOfficeCode;

                    if (sOfficeCode.Length == 2)
                    {
                        LoadStatusReportLocationWise(sOfficeCode);
                      
                     
                    }
                    if (sOfficeCode.Length == 3)
                    {
                        LoadStatusReportFeederWise(sOfficeCode);

                    }

                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtLocName = (TextBox)row.FindControl("txtLocName");
                    TextBox txtFeederName = (TextBox)row.FindControl("txtFeederName");
                   
                    DataTable dt = (DataTable)ViewState["StatusReport"];
                    dv = dt.DefaultView;

                    if (txtLocName.Text != "")
                    {
                        sFilter = "LOCATION Like '%" + txtLocName.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txtFeederName.Text != "")
                    {
                        sFilter += " FD_FEEDER_NAME Like '%" + txtFeederName.Text.Replace("'", "'") + "%' AND";
                    }

                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdStatusReport.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdStatusReport.DataSource = dv;
                            ViewState["StatusReport"] = dv.ToTable();
                            grdStatusReport.DataBind();

                        }
                        else
                        {

                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        if (hdfOfficeCode.Value.Length == 2)
                        {                           
                            LoadStatusReportLocationWise(hdfOfficeCode.Value.Substring(0, 2));
                        }
                        else if (hdfOfficeCode.Value.Length == 3)
                        {
                            LoadStatusReportFeederWise(hdfOfficeCode.Value.Substring(0, 3));
                        }
                        else
                        {
                            LoadStatusReportLocationWise();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdStatusReport_RowCommand");

            }
        }

        protected void cmdBack_Click(object sender, EventArgs e)
        {
            try
            {
                if (hdfOfficeCode.Value.Length > 2)
                {
                    LoadStatusReportLocationWise(hdfOfficeCode.Value.Substring(0, 2));

                
                    hdfOfficeCode.Value = "";
                }
                else
                {
                    LoadStatusReportLocationWise();
                  
                }
             
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdBack_Click");

            }
        }

        protected void grdStatusReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdStatusReport.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["StatusReport"];
                grdStatusReport.DataSource = dt;
                grdStatusReport.DataBind();
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdStatusReport_PageIndexChanging");

            }
        }
       
    }
}