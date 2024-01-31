using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Configuration;
using System.Web.UI.DataVisualization.Charting;

namespace IIITS.DTLMS
{
    public partial class Dashboard : System.Web.UI.Page
    {
        string strFormCode = "Dashboard";
        clsSession objSession;
        //protected override void OnPreLoad(EventArgs e)
        //{
        //    clsException.LogError(" exceuted  objSession  ", "Office Code OnPreLoad Dashboard " + Session["OFF_CIDES"], strFormCode, "Page_Load");

        //}
        protected void Page_Load(object sender, EventArgs e)

        {
            try
            {

                //clsException.LogError(" exceuted  objSession  ", "Office Code fuccc "+ Session["OFF_CIDES"], strFormCode, "Page_Load");
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")  // clsSession
                {
                    //clsException.LogError(" exceuted  objSession  ", "Office Code GO YO  " + Session["clsSession"], strFormCode, "Page_Load");

                    Response.Redirect("~/Login.aspx", false);
                    return;
                }

                objSession = (clsSession)Session["clsSession"];

                if (!IsPostBack)
                {
                    if (objSession != null)
                    {
                        //clsException.LogError(" objSession != null  ", "objSession != null 2423423" + objSession.OfficeCode, strFormCode, "Page_Load");
                        lblLocation.Text = objSession.OfficeName;
                        hdfLocationCode.Value = objSession.OfficeCode;
                        lnkTotalNumberofDTC.NavigateUrl = "/MasterForms/DTCView.aspx?offCode=" + hdfLocationCode.Value + "";

                    }


                    Genaral.Load_Combo("SELECT MD_NAME,MD_NAME from TBLMASTERDATA WHERE MD_TYPE='C' ORDER BY MD_ID", "-Select-", cmbCapacity);
                    Genaral.Load_Combo("SELECT OM_CODE, OM_CODE || '-' || OM_NAME FROM TBLOMSECMAST WHERE OM_CODE LIKE '" + objSession.OfficeCode + "%' ORDER BY OM_CODE", "--Select--", cmbSection);

                    //DTC Failure Pending Details
                    GetPendingFailureCount();

                    //Faulty DTR Details
                    GetFaultyDTrCount();

                    //DTR At Store Faulty / Repair goog / scrap /  non repairable
                    GetDtrAtStoreCount();

                    //Total DTC Count
                    GetTotalDTCCount();

                    //Approval Inbox Item Count
                    GetInboxStatus();

                    // DTC Failure Abstract Capcitywise
                    LoadDTCFailureAbstract();

                }

                //DTC Failure Chart
                LoadFailureChart();
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message + " Message : " + Session["clsSession"] + "$" + ex.InnerException, strFormCode, "Page_Load");
            }
        }

        public void GetDtrAtStoreCount()
        {
            try
            {

                clsDashboard objDashboard = new clsDashboard();

                objDashboard.sOfficeCode = hdfLocationCode.Value;

                if (objDashboard.sOfficeCode == "0")
                {
                    objDashboard.sOfficeCode = null;
                }

                lblRepairGood.Text = objDashboard.TotalRepairGoodTc(objDashboard); ;
                lblBrandNew.Text = objDashboard.BrandNewDTrCount(objDashboard);
                lblScrap.Text = objDashboard.ScrapDtrCount(objDashboard);
                lblNonRepairable.Text = objDashboard.NonrepairableCount(objDashboard);
                lblReleasedGood.Text = objDashboard.ReleasedGoodCount(objDashboard);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetDtrAtStoreCount");
            }
        }


        public void DashboardFunctions()
        {
            try
            {
                GetPendingFailureCount();
                GetFaultyDTrCount();
                //GetDtrAtStoreCount();
                LoadFailureChart();
                GetTotalDTCCount();
                LoadDTCFailureAbstract();

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "DashboardFunctions");
            }
        }


        /// <summary>
        /// DTC Failure Pending Count
        /// </summary>
        public void GetPendingFailureCount()
        {
            try
            {
                clsDashboard objDashboard = new clsDashboard();

                objDashboard.sOfficeCode = hdfLocationCode.Value;

                if (objDashboard.sOfficeCode == "0")
                {
                    objDashboard.sOfficeCode = null;
                }

                //Total Pending Count
                // lblToatlPending.Text = objDashboard.GetTotalPendingCount(objDashboard);

                //Pending for Estimation
                lblPendingEstimation.Text = objDashboard.GetEstimationPendingCount(objDashboard);

                // Pending for WorkOrder
                //objDashboard.sBOId = "11";
                //lblPendingWO.Text = objDashboard.GetWOPendingCount(objDashboard);

                //// Pending for Indent
                //objDashboard.sBOId = "12";
                //lblPendingIndent.Text = objDashboard.GetIndentPendingCount(objDashboard);


                //// Pending for Commission
                //objDashboard.sBOId = "13,29";
                //lblPendingCommission.Text = objDashboard.GetInvoicePendingCount(objDashboard);

                //objDashboard.sBOId = "14";
                //lblPendingDeCommission.Text = objDashboard.GetDecommissionPendingCount(objDashboard);

                //// Pending for RI             
                //lblPendingRI.Text = objDashboard.GetRIPendingCount(objDashboard);

                ////Pending for CR
                //lblCrPending.Text = objDashboard.GetCRPendingCount(objDashboard);

                DataTable dt = objDashboard.GetFailurePendingCount(objDashboard);

                if (dt.Rows.Count > 0)
                {
                    lblPendingWO.Text = Convert.ToString(dt.Rows[0]["WORKORDER"]);
                    lblPendingIndent.Text = Convert.ToString(dt.Rows[0]["INDENT"]);
                    lblPendingCommission.Text = Convert.ToString(dt.Rows[0]["INVOICE"]);
                    lblPendingDeCommission.Text = Convert.ToString(dt.Rows[0]["DECOMMISION"]);
                    lblPendingRI.Text = Convert.ToString(dt.Rows[0]["RI"]);
                    lblCrPending.Text = Convert.ToString(dt.Rows[0]["CR"]);
                }

                //Total Pending Count
                lblToatlPending.Text = Convert.ToString(Convert.ToInt32(lblPendingEstimation.Text) + Convert.ToInt32(lblPendingWO.Text)
                  + Convert.ToInt32(lblPendingIndent.Text) + Convert.ToInt32(lblPendingCommission.Text));
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetPendingEstimationCount");
            }
        }


        /// <summary>
        /// Faulty DTR Count
        /// </summary>
        public void GetFaultyDTrCount()
        {
            try
            {
                clsDashboard objDashboard = new clsDashboard();

                objDashboard.sOfficeCode = hdfLocationCode.Value;
                if (objDashboard.sOfficeCode == "0")
                {
                    objDashboard.sOfficeCode = null;
                }

                DataTable dt = objDashboard.GetDTRCounts(objDashboard);

                if (dt.Rows.Count > 0)
                {
                    lblRepairGood.Text = Convert.ToString(dt.Rows[0]["REPAIR GOOD"]);
                    lblBrandNew.Text = Convert.ToString(dt.Rows[0]["BRAND NEW"]);
                    lblScrap.Text = Convert.ToString(dt.Rows[0]["SCRAP"]);
                    lblNonRepairable.Text = Convert.ToString(dt.Rows[0]["NON REPAIRABLE"]);
                    lblFaultyStore.Text = Convert.ToString(dt.Rows[0]["FAULTY STORE"]);
                    //lblFaultyRepairer.Text = Convert.ToString(dt.Rows[0]["REPAIRER"]);
                    lblReleasedGood.Text = Convert.ToString(dt.Rows[0]["RELEASED GOOD"]);
                }


                //Total Faulty DTR  no need of this  (get the count )
                // lblTotalFaulty.Text = objDashboard.GetTotalFaultyTC(objDashboard);

                // Faulty DTR at Field needed 
                lblFaultyField.Text = objDashboard.GetFaultyTCField(objDashboard);



                // Faulty DTR at Store merge 
                lblFaultyStore.Text = objDashboard.GetFaultyTCStore(objDashboard);

                // Faulty DTR at Repairer merge 
                lblFaultyRepairer.Text = objDashboard.GetFaultyTCRepair(objDashboard);

                // Faulty DTR at Supplier merge 
                lblFaultySupplier.Text = objDashboard.GetFaultyTCSupplier(objDashboard);

                // total faulty DTR
                lblTotalFaulty.Text = Convert.ToString(Convert.ToInt32(lblFaultyField.Text) + Convert.ToInt32(lblFaultyStore.Text) + Convert.ToInt32(lblFaultyRepairer.Text) + Convert.ToInt32(lblFaultySupplier.Text));

                //TOTAL repair good TC 
                //LabelTcfailed.Text = objDashboard.TotalRepairGoodTc(objDashboard);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetFaultyDTrCount");
            }
        }


        /// <summary>
        /// Inbox Status
        /// </summary>
        public void GetInboxStatus()
        {
            try
            {
                clsDashboard objDashboard = new clsDashboard();

                objDashboard.sOfficeCode = objSession.OfficeCode;
                objDashboard.sRoleId = objSession.RoleId;

                //Total Inbox count
                lblTotalWorkflow.Text = objDashboard.GetTotalWorkflow(objDashboard);

                // Pending
                lblPendingWorkflow.Text = objDashboard.GetPendingWorkflow(objDashboard);

                // Approved
                lblApprovedWorkflow.Text = objDashboard.GetApprovedWorkflow(objDashboard);

                // Rejected
                lblRejectedWorkflow.Text = objDashboard.GetRejectedWorkflow(objDashboard);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetFaultyDTrCount");
            }
        }


        #region Change Location

        protected void lnkChange_Click(object sender, EventArgs e)
        {
            try
            {
                LoadOfficeGrid();
                this.mdlPopup.Show();

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "lnkChange_Click");
            }
        }


        public void LoadOfficeGrid(string sOfficeCode = "", string sOffName = "")
        {
            try
            {

                //  clsFeederMast objFeeder = new clsFeederMast();
                clsDashboard objDashboard = new clsDashboard();
                DataTable dt = new DataTable();

                objDashboard.sOfficeCode = sOfficeCode;
                objDashboard.sOfficeName = sOffName;

                dt = objDashboard.LoadOfficeDet(objDashboard);
                if (dt.Rows.Count > 0)
                {
                    grdOffices.DataSource = dt;
                    grdOffices.DataBind();
                    ViewState["Office"] = dt;
                }
                else
                {
                    ShowEmptyGrid();
                }

            }

            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadOfficeGrid");
            }
        }

        protected void grdOffices_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtOffCode = (TextBox)row.FindControl("txtOffCode");
                    TextBox txtOffName = (TextBox)row.FindControl("txtOffName");

                    LoadOfficeGrid(txtOffCode.Text.Trim().Replace("'", "''"), txtOffName.Text.Trim().Replace("'", "''"));

                    this.mdlPopup.Show();
                    //LoadFailureChart();
                }

                if (e.CommandName == "submit")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    string sOffCode = ((Label)row.FindControl("lblOffCode")).Text;
                    string soffName = ((Label)row.FindControl("lblOffName")).Text;

                    lblLocation.Text = soffName;
                    hdfLocationCode.Value = sOffCode;
                    Session["OffCode"] = sOffCode;
                    ViewState["FailureGraph"] = null;
                    DashboardFunctions();
                    //LoadFailureChart();
                    lnkTotalNumberofDTC.NavigateUrl = "/MasterForms/DTCView.aspx?offCode=" + sOffCode + "";
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdOffices_RowCommand");
            }
        }

        protected void grdOffices_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdOffices.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Office"];
                if (dt.Rows.Count > 0)
                {
                    grdOffices.DataSource = dt;
                    grdOffices.DataBind();
                }

                this.mdlPopup.Show();
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdOffices_PageIndexChanging");
            }
        }

        #endregion

        #region Graph
        public void LoadFailureChart()
        {
            Series series = new Series();
            DataTable dtBarGraph = new DataTable();
            clsDashboard objDashboard = new clsDashboard();

            if (ViewState["FailureGraph"] != null)
            {
                dtBarGraph = (DataTable)ViewState["FailureGraph"];
            }
            else
            {
                objDashboard.sOfficeCode = hdfLocationCode.Value;

                if (objDashboard.sOfficeCode == "0")
                {
                    hdfLocationCode.Value = null;
                }

                dtBarGraph = objDashboard.LoadBarGraph(hdfLocationCode.Value);
                ViewState["FailureGraph"] = dtBarGraph;
            }


            string[] XPointMember = new string[dtBarGraph.Rows.Count];
            string[] XPointMember1 = new string[dtBarGraph.Rows.Count];
            int[] YPointMember = new int[dtBarGraph.Rows.Count];
            int[] YPointMember1 = new int[dtBarGraph.Rows.Count];

            for (int i = 0; i < dtBarGraph.Rows.Count; i++)
            {

                XPointMember[i] = Convert.ToString(dtBarGraph.Rows[i]["PRESENTMONTH"].ToString());

                YPointMember[i] = Convert.ToInt32(dtBarGraph.Rows[i]["PRESENTCOUNT"].ToString());
                XPointMember1[i] = Convert.ToString(dtBarGraph.Rows[i]["PREVIOUSMONTH"].ToString());

                YPointMember1[i] = Convert.ToInt32(dtBarGraph.Rows[i]["PREVIOUSCOUNT"].ToString());
            }

              var  PresentYear = dtBarGraph.Rows[0]["PRESENTYEAR"].ToString();
              var  PreviousYear = dtBarGraph.Rows[0]["PREVIOUSYEAR"].ToString();
       
           
                Chart1.Series[1].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Column;
                Chart1.Series[0].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Column;

                Chart1.Series[1].ToolTip = "(#VALY)Failure on (#VALX " + PresentYear + ")";
                Chart1.Series[0].ToolTip = "(#VALY)Failure on (#VALX " + PreviousYear + ")";
            
            Chart1.Series[1].Points.DataBindXY(XPointMember, YPointMember);
            Chart1.Series[0].Points.DataBindXY(XPointMember1, YPointMember1);


            Chart1.Series[1]["PixelPointWidth"] = "50";
            Chart1.Series[0]["PixelPointWidth"] = "50";

            Chart1.Width = 1000;

            Chart1.Titles["NewTitle"].Text = "DTC Failure/Month";

            Chart1.Series[0].LegendText = "Previous Year";
            Chart1.Series[1].LegendText = "Current Year";


            Axis xaxis = Chart1.ChartAreas[0].AxisX;
            xaxis.Interval = 1;
            Axis yaxis = Chart1.ChartAreas[0].AxisY;
          


            // Maximum value in Series, you may have to change YValues[0] to the appropriate 
            // index for your type of chart (so that it measures the top of your markers)
            double xmaxValue = Chart1.Series[0].Points.Max(x => x.YValues[0]);
            double ymaxValue2 = Chart1.Series[1].Points.Max(y => y.YValues[0]);

            double maxValue = Math.Max(xmaxValue, ymaxValue2);
            // yAxisMax calculated as max value in series + 10 then rounded up to nearest 10
            // but capped at 125 below. You could change this to perhaps 
            // maxValue * 1.1 to always get a 10 % extra above, anything goes
            double yAxisMax = Math.Ceiling((maxValue + 10) / 9) * 10;
          //  double yAxisMax = Math.Ceiling(maxValue + 15);

            //if (yAxisMax > 125)
            //    yAxisMax = 125;

            Chart1.ChartAreas[0].AxisY.Minimum = 0;
            Chart1.ChartAreas[0].AxisY.Maximum = yAxisMax;
            //Chart1.ChartAreas[0].AxisY.Interval = 20;

            Chart1.ChartAreas[0].AxisX.Title = "Month";

            Chart1.ChartAreas[0].AxisY.Title = "No. of DTC Failure";
            Chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
            Chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;

            Chart1.Legends["Legend1"].Docking = Docking.Bottom;
            Chart1.Legends["Legend1"].DockedToChartArea = "ChartArea1";
            Chart1.Legends["Legend1"].IsDockedInsideChartArea = false;
            series.IsValueShownAsLabel = true;

            Chart1.Series[1].IsValueShownAsLabel = true;
            Chart1.Series[0].IsValueShownAsLabel = true;


        }
        #endregion


        #region DTC Failure Abstract

        public void LoadDTCFailureAbstract()
        {
            try
            {
                clsDashboard objDashboard = new clsDashboard();
                objDashboard.sOfficeCode = hdfLocationCode.Value;



                // Genaral.Load_Combo("SELECT OM_CODE, OM_CODE || '-' || OM_NAME FROM TBLOMSECMAST WHERE OM_CODE LIKE '" + objDashboard.sOfficeCode + "%' ORDER BY OM_CODE", "--Select--", cmbSection);

                if (cmbSection.SelectedIndex > 0)
                {
                    objDashboard.sOfficeCode = cmbSection.SelectedValue;

                }
                if (cmbCapacity.SelectedIndex > 0)
                {
                    objDashboard.sCapacity = cmbCapacity.SelectedValue;
                }

                DataTable dt = objDashboard.LoadDTCFailureAbstract(objDashboard);
                if (dt.Rows.Count > 0)
                {
                    grdDTCFailureAbstract.DataSource = dt;
                    grdDTCFailureAbstract.DataBind();
                    ViewState["DTCFailureAbstract"] = dt;
                }
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadDTCFailureAbstract");
            }
        }

        protected void cmbCapacity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadDTCFailureAbstract();
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbCapacity_SelectedIndexChanged");
            }
        }

        protected void cmbSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadDTCFailureAbstract();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbSection_SelectedIndexChanged");
            }

        }

        protected void grdDashboard_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdDTCFailureAbstract.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["DTCFailureAbstract"];
                grdDTCFailureAbstract.DataSource = SortDataTable(dt as DataTable, true);
                grdDTCFailureAbstract.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdDashboard_PageIndexChanging");
            }
        }
        protected void grdDTCFailureAbstract_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdDTCFailureAbstract.PageIndex;
            DataTable dt = (DataTable)ViewState["DTCFailureAbstract"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdDTCFailureAbstract.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdDTCFailureAbstract.DataSource = dt;
            }
            grdDTCFailureAbstract.DataBind();
            grdDTCFailureAbstract.PageIndex = pageIndex;
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
                        dataView.Sort = string.Format("{0} {1} ", GridViewSortExpression, GridViewSortDirection);

                        if (dataView.Sort.ToString() == "TC_CAPACITY ASC ")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["DTCFailureAbstract"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC ")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["DTCFailureAbstract"] = dataView.ToTable();
                        }

                        else
                        {
                            // dataView.Sort = string.Format("{0} {1} ", GridViewSortExpression, GridViewSortDirection);
                            ViewState["DTCFailureAbstract"] = dataView.ToTable();
                        }
                    }
                    else
                    {

                        dataView.Sort = string.Format("{0} {1} ", GridViewSortExpression, GetSortDirection());

                        if (dataView.Sort.ToString() == "TC_CAPACITY ASC ")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["DTCFailureAbstract"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC ")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["DTCFailureAbstract"] = dataView.ToTable();
                        }



                        else
                        {
                            // dataView.Sort = string.Format("{0} {1} ", GridViewSortExpression, GetSortDirection());
                            ViewState["DTCFailureAbstract"] = dataView.ToTable(); ;


                        }


                        //dv.Sort = string.Format("{0} {1} ", GridViewSortExpression, GetSortDirection());

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

        #endregion


        protected void lnkFaultyView_Click(object sender, EventArgs e)
        {
            try
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/DTRRepairDetails.aspx?RefId=Custom&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','_blank');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "lnkFaultyView_Click");
            }
        }


        protected void lnkFailurePend_Click(object sender, EventArgs e)
        {
            try
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/FailurePenddingDetails.aspx?RefId=Custom&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','_blank');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "lnkFailurePend_Click");
            }
        }

        #region DTC Count
        public void GetTotalDTCCount()
        {
            try
            {
                clsDashboard objDashboard = new clsDashboard();

                objDashboard.sOfficeCode = hdfLocationCode.Value;
                if (objDashboard.sOfficeCode == "0")
                {
                    objDashboard.sOfficeCode = null;
                }

                //Total DTC count
                lnkTotalNumberofDTC.Text = objDashboard.GetTotalDTCCount(objDashboard);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetTotalDTCCount");
            }
        }
        #endregion

        protected void Failure_Click(object sender, EventArgs e)
        {
            if (lblToatlPending.Text == "0")
            {
                ShowMsgBox("No Record Found");
            }
            else
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/FailurePendingOverview.aspx?value=Failure&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
            }

        }

        protected void estimation_Click(object sender, EventArgs e)
        {
            if (lblPendingEstimation.Text == "0")
            {
                ShowMsgBox("No Record Found");
            }
            else
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/FailurePendingOverview.aspx?value=estimation&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
            }

        }

        protected void workorder_Click(object sender, EventArgs e)
        {
            if (lblPendingWO.Text == "0")
            {
                ShowMsgBox("No Record Found");
            }
            else
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/FailurePendingOverview.aspx?value=workorder&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
            }

        }

        protected void indent_Click(object sender, EventArgs e)
        {
            if (lblPendingIndent.Text == "0")
            {
                ShowMsgBox("No Record Found");
            }
            else
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/FailurePendingOverview.aspx?value=indent&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
            }

        }

        protected void invoice_Click(object sender, EventArgs e)
        {
            if (lblPendingCommission.Text == "0")
            {
                ShowMsgBox("No Record Found");
            }
            else
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/FailurePendingOverview.aspx?value=invoice&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
            }

        }

        protected void Decommissioning_Click(object sender, EventArgs e)
        {
            if (lblPendingDeCommission.Text == "0")
            {
                ShowMsgBox("No Record Found");
            }
            else
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/FailurePendingOverview.aspx?value=DeCommission&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
            }

        }

        protected void RI_Click(object sender, EventArgs e)
        {
            if (lblPendingRI.Text == "0")
            {
                ShowMsgBox("No Record Found");
            }
            else
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/FailurePendingOverview.aspx?value=RI&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
            }

        }


        protected void CR_Click(object sender, EventArgs e)
        {
            if (lblCrPending.Text == "0")
            {
                ShowMsgBox("No Record Found");
            }
            else
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/FailurePendingOverview.aspx?value=CR&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
            }

        }

        protected void lnkBtnFaildDtrDetails_Click(object sender, EventArgs e)
        {
            string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
            string url = "/DashboardForm/TcFailureDetails.aspx?value=Failure&OfficeCode=" + sOfficeCode;
            string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
            ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);


        }

        //protected void Tcfailed_Click(object sender, EventArgs e)
        //{
        //    if (LabelTcfailed.Text == "0")
        //    {
        //        ShowMsgBox("No Record Found");
        //    }
        //    else
        //    {
        //        string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
        //        string url = "/DashboardForm/TcFailed.aspx?value=TcFailed&OfficeCode=" + sOfficeCode;
        //        string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
        //        ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
        //    }

        //}
        protected void TotalFaulty_Click(object sender, EventArgs e)
        {
            if (lblTotalFaulty.Text == "0")
            {
                ShowMsgBox("No Record Found");
            }
            else
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/TcFailed.aspx?value=TotalFaulty&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
            }

        }
        protected void Faulty_field_Click(object sender, EventArgs e)
        {
            if (lblFaultyField.Text == "0")
            {
                ShowMsgBox("No Record Found");
            }
            else
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/TcFailed.aspx?value=FaultyField&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
            }

        }
        protected void Faulty_Store_Click(object sender, EventArgs e)
        {
            if (lblFaultyStore.Text == "0")
            {
                ShowMsgBox("No Record Found");
            }
            else
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/TcFailed.aspx?value=FaultyStore&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
            }

        }
        protected void Faulty_Repairer_Click(object sender, EventArgs e)
        {
            if (lblFaultyRepairer.Text == "0")
            {
                ShowMsgBox("No Record Found");
            }
            else
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/TcFailed.aspx?value=FaultyRepairer&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
            }

        }

        protected void Faulty_Supplier_Click(object sender, EventArgs e)
        {
            if (lblFaultyRepairer.Text == "0")
            {
                ShowMsgBox("No Record Found");
            }
            else
            {
                string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                string url = "/DashboardForm/TcFailed.aspx?value=FaultySupplier&OfficeCode=" + sOfficeCode;
                string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
            }

        }
        protected void Export_clickDTCFailureAbstract(object sender, EventArgs e)
        {
            //clsDashboard objDashboard = new clsDashboard();

            //objDashboard.sOfficeCode = hdfLocationCode.Value;
            //if (cmbSection.SelectedIndex > 0)
            //{
            //    objDashboard.sOfficeCode = cmbSection.SelectedValue;
            //}
            //if (cmbCapacity.SelectedIndex > 0)
            //{
            //    objDashboard.sCapacity = cmbCapacity.SelectedValue;
            //}

            //DataTable dt = objDashboard.LoadDTCFailureAbstract(objDashboard);
            DataTable dt = (DataTable)ViewState["DTCFailureAbstract"];
            if (dt.Rows.Count > 0)
            {

                dt.Columns["TC_CAPACITY"].ColumnName = "Capacity(in KVA)";
                dt.Columns["DF_LOC_CODE"].ColumnName = "Section Code";
                dt.Columns["SECTION"].ColumnName = "Section Name";
                dt.Columns["CURRENTMONTH"].ColumnName = "Current Month";
                dt.Columns["PREVIOUSMONTH"].ColumnName = "Previous Month";
                dt.Columns["CURRENTQUARTER"].ColumnName = "Current Quarter";
                dt.Columns["FAILURECOUNTOFYEAR"].ColumnName = "Current Financial Year";

                List<string> listtoRemove = new List<string> { "" };
                string filename = "DTCFailureAbstract" + DateTime.Now + ".xls";
                string pagetitle = "DTC Failure Abstract";

                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No record found");

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

        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("OFF_CODE");
                dt.Columns.Add("OFF_NAME");

                grdOffices.DataSource = dt;
                grdOffices.DataBind();

                int iColCount = grdOffices.Rows[0].Cells.Count;
                grdOffices.Rows[0].Cells.Clear();
                grdOffices.Rows[0].Cells.Add(new TableCell());
                grdOffices.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdOffices.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowEmptyGrid");

            }
        }

        public void TCDetails_Click(object sender, EventArgs e)
        {
            string temp = string.Empty;
            try
            {
                LinkButton lnk = (LinkButton)sender;
                temp = lnk.ID;
                switch (temp)
                {
                    case "lnkRepairGood":
                        if (lnkRepairGood.Text == "0")
                        {
                            ShowMsgBox("No Record Found");
                            return;
                        }
                        else
                        {
                            string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                            string url = "/DashboardForm/TcFailed.aspx?value=TcFailed&OfficeCode=" + sOfficeCode;
                            string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                            ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
                        }
                        break;
                    case "lnkBrandNew":
                        if (lnkBrandNew.Text == "0")
                        {
                            ShowMsgBox("No Record Found");
                            return;
                        }
                        else
                        {
                            string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                            string url = "/DashboardForm/TcFailed.aspx?value=BrandNew&OfficeCode=" + sOfficeCode;
                            string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                            ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
                        }
                        break;
                    case "lnkScrap":
                        if (lnkScrap.Text == "0")
                        {
                            ShowMsgBox("No Record Found");
                            return;
                        }
                        else
                        {
                            string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                            string url = "/DashboardForm/TcFailed.aspx?value=ScrapStore&OfficeCode=" + sOfficeCode;
                            string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                            ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
                        }
                        break;
                    case "lnkNotRepairable":
                        if (lnkNotRepairable.Text == "0")
                        {
                            ShowMsgBox("No Record Found");
                            return;
                        }
                        else
                        {
                            string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                            string url = "/DashboardForm/TcFailed.aspx?value=NonRepairable&OfficeCode=" + sOfficeCode;
                            string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                            ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
                        }
                        break;
                    case "lnkReleasedGood":
                        if (lnkReleasedGood.Text == "0")
                        {
                            ShowMsgBox("No Record Found");
                            return;
                        }
                        else
                        {
                            string sOfficeCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfLocationCode.Value));
                            string url = "/DashboardForm/TcFailed.aspx?value=ReleasedGood&OfficeCode=" + sOfficeCode;
                            string s = "window.open('" + url + "','mypopup','width=1100,height=800');"; // '_newtab'
                            ClientScript.RegisterStartupScript(this.GetType(), "script1", s, true);
                        }
                        break;

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "TCDetails_Click");
            }
        }


    }
}