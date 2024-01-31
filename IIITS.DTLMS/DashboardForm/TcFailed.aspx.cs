using IIITS.DTLMS.BL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.DashboardForm
{
    public partial class TcFailed : System.Web.UI.Page
    {
        string strFormCode = "FailureTc";
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

                    LoadFailureDetails();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load");
            }
        }
        public void LoadFailureDetails()
        {
            try
            {
                clsDashboard objDashboard = new clsDashboard();
                DataTable dtLoadDetails = new DataTable();


                objDashboard.sOfficeCode = hdfOffCode.Value;

                // repair good dtr 
                if (Request.QueryString["value"] == "TcFailed")
                {
                    dtLoadDetails = objDashboard.TotalTcfailedview(hdfOffCode.Value);
                    grdFailuretc.Visible = true;
                    grdFailuretc.DataSource = dtLoadDetails;
                    grdFailuretc.DataBind();
                    ViewState["FailureTc"] = dtLoadDetails;
                    failureText.Text = "Repair Good TC Details";
                    failure.Text = "Repair Good TC Details";
                }
                if (Request.QueryString["value"] == "TotalFaulty")
                {

                    dtLoadDetails = objDashboard.GetTotalFaultyTCview(hdfOffCode.Value); ;
                    grdtotaldtr.Visible = true;
                    grdtotaldtr.DataSource = dtLoadDetails;
                    grdtotaldtr.DataBind();
                    ViewState["TotalFaulty"] = dtLoadDetails;
                    failureText.Text = "TotalFaulty DTR Details";
                    failure.Text = "TotalFaulty DTR Details";
                }

                if (Request.QueryString["value"] == "FaultyField")
                {                    
                    dtLoadDetails = objDashboard.GetFaultyTCFieldview(hdfOffCode.Value);
                    grdfaultyfield.Visible = true;
                    grdfaultyfield.DataSource = dtLoadDetails;
                    grdfaultyfield.DataBind();
                    ViewState["FaultyField"] = dtLoadDetails;
                    failureText.Text = "Faulty DTr at Field Details";
                    failure.Text = "Faulty DTr at Field Details";
                }
                if (Request.QueryString["value"] == "FaultyStore")
                {
                    
                       dtLoadDetails = objDashboard.GetFaultyTCStoreview(hdfOffCode.Value);
                    grdfaultystore.Visible = true;
                    grdfaultystore.DataSource = dtLoadDetails;
                    grdfaultystore.DataBind();
                    ViewState["FaultyStore"] = dtLoadDetails;
                    failureText.Text = "Faulty DTr at Store Details";
                    failure.Text = "Faulty DTr at Store Details";
                }
                if (Request.QueryString["value"] == "FaultyRepairer")
                {
                    dtLoadDetails = objDashboard.GetFaultyTCRepairview(hdfOffCode.Value,"2");
                    grdfaultyrepairer.Visible = true;
                    grdfaultyrepairer.DataSource = dtLoadDetails;
                    grdfaultyrepairer.DataBind();
                    ViewState["FaultyRepairer"] = dtLoadDetails;
                    failureText.Text = "Faulty DTr at Repairer Details";
                    failure.Text = "Faulty DTr at Repairer Details";
                }

                if (Request.QueryString["value"] == "FaultySupplier")
                {
                    dtLoadDetails = objDashboard.GetFaultyTCRepairview(hdfOffCode.Value,"1");
                    grdSupplier.Visible = true;
                    grdSupplier.DataSource = dtLoadDetails;
                    grdSupplier.DataBind();
                    ViewState["FaultySupplier"] = dtLoadDetails;
                    failureText.Text = "Faulty DTr at Supplier Details";
                    failure.Text = "Faulty DTr at Supplier Details";
                }

                // DTR Details at store 

                if (Request.QueryString["value"] == "BrandNew")
                {
                    dtLoadDetails = objDashboard.GetBrandNew(hdfOffCode.Value);
                    grdBrandNewDtr.Visible = true;
                    grdBrandNewDtr.DataSource = dtLoadDetails;
                    grdBrandNewDtr.DataBind();
                    ViewState["BrandNew"] = dtLoadDetails;
                    failureText.Text = "BrandNew DTr ";
                    failure.Text = "BrandNew DTr ";
                }

                if (Request.QueryString["value"] == "ScrapStore")
                {
                    dtLoadDetails = objDashboard.GetScrapStore(hdfOffCode.Value);
                    grdScrapdtr.Visible = true;
                    grdScrapdtr.DataSource = dtLoadDetails;
                    grdScrapdtr.DataBind();
                    ViewState["ScrapStore"] = dtLoadDetails;
                    failureText.Text = "Scrap Dtr";
                    failure.Text = "Scrap Dtr";
                }

                if (Request.QueryString["value"] == "NonRepairable")
                {
                    dtLoadDetails = objDashboard.GetNonRepairable(hdfOffCode.Value);
                    grdNonRepairable.Visible = true;
                    grdNonRepairable.DataSource = dtLoadDetails;
                    grdNonRepairable.DataBind();
                    ViewState["NonRepairable"] = dtLoadDetails;
                    failureText.Text = "Non Repairable DTR";
                    failure.Text = "Non Repairable DTR";
                }

                if (Request.QueryString["value"] == "ReleasedGood")
                {
                    dtLoadDetails = objDashboard.GetReleasedGood(hdfOffCode.Value);
                    grdNonRepairable.Visible = true;
                    grdNonRepairable.DataSource = dtLoadDetails;
                    grdNonRepairable.DataBind();
                    ViewState["ReleasedGood"] = dtLoadDetails;
                    failureText.Text = "Released Good DTR";
                    failure.Text = "Released Good  DTR";
                }


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadFailureDetails");
            }
        }

        #region for pageindexing and sort
        protected void grdFailuretc_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                grdFailuretc.PageIndex = e.NewPageIndex;
                dtComplete = (DataTable)ViewState["FailureTc"];
                grdFailuretc.DataSource = SortDataTableRepaireGood(dtComplete as DataTable, true);
                grdFailuretc.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdFailuretc_PageIndexChanging");
            }
        }
        protected void grdtotaldtr_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                grdtotaldtr.PageIndex = e.NewPageIndex;
                dtComplete = (DataTable)ViewState["TotalFaulty"];
                grdtotaldtr.DataSource = SortDataTableTotal(dtComplete as DataTable, true);
                grdtotaldtr.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdtotaldtr_PageIndexChanging");
            }
        }
        protected void grdfaultyfield_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                grdfaultyfield.PageIndex = e.NewPageIndex;
                dtComplete = (DataTable)ViewState["FaultyField"];
                grdfaultyfield.DataSource = SortDataTableField(dtComplete as DataTable, true);
                grdfaultyfield.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdfaultyfield_PageIndexChanging");
            }
        }
        protected void grdfaultystore_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                grdfaultystore.PageIndex = e.NewPageIndex;
                dtComplete = (DataTable)ViewState["FaultyStore"];
                grdfaultystore.DataSource = SortDataTableStore(dtComplete as DataTable, true);
                grdfaultystore.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdfaultystore_PageIndexChanging");
            }
        }
        protected void grdBrandNewDtr_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                grdBrandNewDtr.PageIndex = e.NewPageIndex;
                dtComplete = (DataTable)ViewState["BrandNew"];
                grdBrandNewDtr.DataSource = SortDataTableBrandNew(dtComplete as DataTable, true);
                grdBrandNewDtr.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdBrandNewDtr_PageIndexChanging");
            }
        }


        protected void grdNonRepairable_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                grdNonRepairable.PageIndex = e.NewPageIndex;
                dtComplete = (DataTable)ViewState["NonRepairable"];
                grdNonRepairable.DataSource = SortDataTableNonRepairable(dtComplete as DataTable, true);
                grdNonRepairable.DataBind();

                if (Request.QueryString["value"] == "ReleasedGood")
                {
                    DataTable dtreleasegood = new DataTable();
                    grdNonRepairable.PageIndex = e.NewPageIndex;
                    dtreleasegood = (DataTable)ViewState["ReleasedGood"];
                    grdNonRepairable.DataSource = SortDataTableNonRepairable(dtreleasegood as DataTable, true);
                    grdNonRepairable.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdBrandNewDtr_PageIndexChanging");
            }
        }

        protected void grdScrapdtr_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                grdScrapdtr.PageIndex = e.NewPageIndex;
                dtComplete = (DataTable)ViewState["ScrapStore"];
                grdScrapdtr.DataSource = SortDataTableScrap(dtComplete as DataTable, true);
                grdScrapdtr.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdScrapdtr_PageIndexChanging");
            }
        }


        protected void grdfaultyrepairer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtComplete = new DataTable();
                grdfaultyrepairer.PageIndex = e.NewPageIndex;

                if (ViewState["FaultyRepairer"] != null)
                {
                    dtComplete = (DataTable)ViewState["FaultyRepairer"];
                    grdfaultyrepairer.DataSource = SortDataTableRepairer(dtComplete as DataTable, true);
                    grdfaultyrepairer.DataBind();
                }
                else
                {
                    dtComplete = (DataTable)ViewState["FaultySupplier"];
                    grdSupplier.DataSource = SortDataTableRepairer(dtComplete as DataTable, true);
                    grdSupplier.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdfaultyrepairer_PageIndexChanging");
            }
        }

        protected void grdfaultyrepairer_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdfaultyrepairer.PageIndex;
            DataTable dt;
            if (ViewState["FaultyRepairer"] != null)
            {
                dt = (DataTable)ViewState["FaultyRepairer"];
                if (dt.Rows.Count > 0)
                {
                    grdfaultyrepairer.DataSource = SortDataTableRepairer(dt as DataTable, false);
                }
                else
                {
                    grdfaultyrepairer.DataSource = dt;
                }
                grdfaultyrepairer.DataBind();
                grdfaultyrepairer.PageIndex = pageIndex;
            }
            else
            {
                dt = (DataTable)ViewState["FaultySupplier"];
                string sortingDirection = string.Empty;
                if (dt.Rows.Count > 0)
                {
                    grdSupplier.DataSource = SortDataTableRepairer(dt as DataTable, false);
                }
                else
                {
                    grdSupplier.DataSource = dt;
                }
                grdSupplier.DataBind();
                grdSupplier.PageIndex = pageIndex;
            }
           }
        protected void grdfaultystore_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdfaultystore.PageIndex;
            DataTable dt = (DataTable)ViewState["FaultyStore"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {
                grdfaultystore.DataSource = SortDataTableStore(dt as DataTable, false);
            }
            else
            {
                grdfaultystore.DataSource = dt;
            }
            grdfaultystore.DataBind();
            grdfaultystore.PageIndex = pageIndex;
        }

        protected void grdBrandNewDtr_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdBrandNewDtr.PageIndex;
            DataTable dt = (DataTable)ViewState["BrandNew"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {
                grdBrandNewDtr.DataSource = SortDataTableBrandNew(dt as DataTable, false);
            }
            else
            {
                grdBrandNewDtr.DataSource = dt;
            }
            grdBrandNewDtr.DataBind();
            grdBrandNewDtr.PageIndex = pageIndex;
        }

        protected void grdNonRepairable_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdNonRepairable.PageIndex;
            DataTable dt = (DataTable)ViewState["NonRepairable"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {
                grdNonRepairable.DataSource = SortDataTableNonRepairable(dt as DataTable, false);
            }
            else
            {
                grdNonRepairable.DataSource = dt;
            }
            grdNonRepairable.DataBind();
            grdNonRepairable.PageIndex = pageIndex;
        }



        protected void grdfaultyfield_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdfaultyfield.PageIndex;
            DataTable dt = (DataTable)ViewState["FaultyField"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdfaultyfield.DataSource = SortDataTableField(dt as DataTable, false);
            }
            else
            {
                grdfaultyfield.DataSource = dt;

            }
            grdfaultyfield.DataBind();
            grdfaultyfield.PageIndex = pageIndex;
        }
        protected void grdtotaldtr_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdtotaldtr.PageIndex;
            DataTable dt = (DataTable)ViewState["TotalFaulty"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {
                grdtotaldtr.DataSource = SortDataTableTotal(dt as DataTable, false);
            }
            else
            {
                grdtotaldtr.DataSource = dt;
            }
            grdtotaldtr.DataBind();
            grdtotaldtr.PageIndex = pageIndex;
        }
        protected void grdFailuretc_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdFailuretc.PageIndex;
            DataTable dt = (DataTable)ViewState["FailureTc"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {
                grdFailuretc.DataSource = SortDataTableRepaireGood(dt as DataTable, false);
            }
            else
            {
                grdFailuretc.DataSource = dt;
            }
            grdFailuretc.DataBind();
            grdFailuretc.PageIndex = pageIndex;
        }


        protected void grdScrapdtr_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdScrapdtr.PageIndex;
            DataTable dt = (DataTable)ViewState["ScrapStore"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {
                grdScrapdtr.DataSource = SortDataTableScrap(dt as DataTable, false);
            }
            else
            {
                grdScrapdtr.DataSource = dt;
            }
            grdScrapdtr.DataBind();
            grdScrapdtr.PageIndex = pageIndex;
        }

        protected DataView SortDataTableTotal(DataTable dataTable, bool isPageIndexChanging)

        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);

                        if (dataView.Sort.ToString() == "TC_CAPACITY ASC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["TotalFaulty"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["TotalFaulty"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["TotalFaulty"] = dataView.ToTable();
                        }

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        if (dataView.Sort.ToString() == "TC_CAPACITY ASC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["TotalFaulty"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["TotalFaulty"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["TotalFaulty"] = dataView.ToTable();
                        }

                    }
                }
                return dataView;
            }
            else
            {
                return new DataView();
            }

        }
        protected DataView SortDataTableField(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                        if (dataView.Sort.ToString() == "TC_CAPACITY ASC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["FaultyField"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["FaultyField"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["FaultyField"] = dataView.ToTable();
                        }


                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());

                        if (dataView.Sort.ToString() == "TC_CAPACITY ASC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["FaultyField"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["FaultyField"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["FaultyField"] = dataView.ToTable();
                        }

                    }
                }
                return dataView;
            }
            else
            {
                return new DataView();
            }

        }
        protected DataView SortDataTableStore(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                        if (dataView.Sort.ToString() == "TC_CAPACITY ASC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["FaultyStore"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["FaultyStore"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["FaultyStore"] = dataView.ToTable();
                        }
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        if (dataView.Sort.ToString() == "TC_CAPACITY ASC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["FaultyStore"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["FaultyStore"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["FaultyStore"] = dataView.ToTable();
                        }

                    }
                }
                return dataView;
            }
            else
            {
                return new DataView();
            }

        }
        protected DataView SortDataTableRepairer(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                        if (dataView.Sort.ToString() == "TC_CAPACITY ASC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);

                            if(ViewState["FaultyRepairer"]!=null)
                            ViewState["FaultyRepairer"] = dataView.ToTable();
                            else
                                ViewState["FaultySupplier"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            if (ViewState["FaultyRepairer"] != null)
                                ViewState["FaultyRepairer"] = dataView.ToTable();
                            else
                                ViewState["FaultySupplier"] = dataView.ToTable();
                        }
                        else
                        {
                            if (ViewState["FaultyRepairer"] != null)
                                ViewState["FaultyRepairer"] = dataView.ToTable();
                            else
                                ViewState["FaultySupplier"] = dataView.ToTable();
                        }
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        if (dataView.Sort.ToString() == "TC_CAPACITY ASC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            if (ViewState["FaultyRepairer"] != null)
                                ViewState["FaultyRepairer"] = dataView.ToTable();
                            else
                                ViewState["FaultySupplier"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            if (ViewState["FaultyRepairer"] != null)
                                ViewState["FaultyRepairer"] = dataView.ToTable();
                            else
                                ViewState["FaultySupplier"] = dataView.ToTable();
                        }
                        else
                        {
                            if (ViewState["FaultyRepairer"] != null)
                                ViewState["FaultyRepairer"] = dataView.ToTable();
                            else
                                ViewState["FaultySupplier"] = dataView.ToTable();
                        }

                    }
                }
                return dataView;
            }
            else
            {
                return new DataView();
            }

        }
        //  brand new
        protected DataView SortDataTableBrandNew(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                        if (dataView.Sort.ToString() == "TC_CAPACITY ASC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["BrandNew"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["BrandNew"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["BrandNew"] = dataView.ToTable();
                        }
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        if (dataView.Sort.ToString() == "TC_CAPACITY ASC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["BrandNew"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["BrandNew"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["BrandNew"] = dataView.ToTable();
                        }

                    }
                }
                return dataView;
            }
            else
            {
                return new DataView();
            }

        }

        //scrap 
        protected DataView SortDataTableScrap(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                        if (dataView.Sort.ToString() == "TC_CAPACITY ASC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["ScrapStore"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["ScrapStore"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["ScrapStore"] = dataView.ToTable();
                        }
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        if (dataView.Sort.ToString() == "TC_CAPACITY ASC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["ScrapStore"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["ScrapStore"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["ScrapStore"] = dataView.ToTable();
                        }

                    }
                }
                return dataView;
            }
            else
            {
                return new DataView();
            }

        }

        //non repairable 
        protected DataView SortDataTableNonRepairable(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                        if (dataView.Sort.ToString() == "TC_CAPACITY ASC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["NonRepairable"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["NonRepairable"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["NonRepairable"] = dataView.ToTable();
                        }
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        if (dataView.Sort.ToString() == "TC_CAPACITY ASC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["NonRepairable"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["NonRepairable"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["NonRepairable"] = dataView.ToTable();
                        }

                    }
                }
                return dataView;
            }
            else
            {
                return new DataView();
            }

        }

        protected DataView SortDataTableRepaireGood(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                        if (dataView.Sort.ToString() == "TC_CAPACITY ASC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["FailureTc"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["FailureTc"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["FailureTc"] = dataView.ToTable();
                        }
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        if (dataView.Sort.ToString() == "TC_CAPACITY ASC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["FailureTc"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["FailureTc"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["FailureTc"] = dataView.ToTable();
                        }

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

        #region for searching 

        protected void grdFailuretc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txttCode = (TextBox)row.FindControl("txttCode");
                    TextBox txttcCapacity = (TextBox)row.FindControl("txttcCapacity");

                    DataTable dt = (DataTable)ViewState["FailureTc"];
                    dv = dt.DefaultView;

                    if (txttCode.Text != "")
                    {
                        sFilter = "Convert(TC_CODE, 'System.String') Like '%" + txttCode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txttcCapacity.Text != "")
                    {
                        sFilter += " Convert(TC_CAPACITY, 'System.String') Like '%" + txttcCapacity.Text.Replace("'", "'") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdFailuretc.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdFailuretc.DataSource = dv;
                            ViewState["FailureTc"] = dv.ToTable();
                            grdFailuretc.DataBind();

                        }
                        else
                        {
                            ViewState["FailureTc"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadFailureDetails();
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdFailurePending_RowCommand");
            }
        }
        protected void grdtotaldtr_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txttCode = (TextBox)row.FindControl("txttCode");
                    TextBox txttcCapacity = (TextBox)row.FindControl("txttcCapacity");

                    DataTable dt = (DataTable)ViewState["TotalFaulty"];
                    dv = dt.DefaultView;

                    if (txttCode.Text != "")
                    {
                        sFilter = "Convert(TC_CODE, 'System.String') Like '%" + txttCode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txttcCapacity.Text != "")
                    {
                        sFilter += " Convert(TC_CAPACITY, 'System.String') Like '%" + txttcCapacity.Text.Replace("'", "'") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdtotaldtr.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdtotaldtr.DataSource = dv;
                            ViewState["TotalFaulty"] = dv.ToTable();
                            grdtotaldtr.DataBind();

                        }
                        else
                        {
                            ViewState["TotalFaulty"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadFailureDetails();
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdtotaldtr_RowCommand");
            }
        }
        protected void grdfaultyfield_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txttCode = (TextBox)row.FindControl("txttCode");
                    TextBox txttcCapacity = (TextBox)row.FindControl("txttcCapacity");

                    DataTable dt = (DataTable)ViewState["FaultyField"];
                    dv = dt.DefaultView;

                    if (txttCode.Text != "")
                    {
                        sFilter = "Convert(TC_CODE, 'System.String') Like '%" + txttCode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txttcCapacity.Text != "")
                    {
                        sFilter += " Convert(TC_CAPACITY, 'System.String') Like '%" + txttcCapacity.Text.Replace("'", "'") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdfaultyfield.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdfaultyfield.DataSource = dv;
                            ViewState["FaultyField"] = dv.ToTable();
                            grdfaultyfield.DataBind();

                        }
                        else
                        {
                            ViewState["FaultyField"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadFailureDetails();
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdfaultyfield_RowCommand");
            }
        }
        protected void grdfaultystore_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txttCode = (TextBox)row.FindControl("txttCode");
                    TextBox txttcCapacity = (TextBox)row.FindControl("txttcCapacity");

                    DataTable dt = (DataTable)ViewState["FaultyStore"];
                    dv = dt.DefaultView;

                    if (txttCode.Text != "")
                    {
                        sFilter = "Convert(TC_CODE, 'System.String') Like '%" + txttCode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txttcCapacity.Text != "")
                    {
                        sFilter += " Convert(TC_CAPACITY, 'System.String') Like '%" + txttcCapacity.Text.Replace("'", "'") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdfaultystore.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdfaultystore.DataSource = dv;
                            ViewState["FaultyStore"] = dv.ToTable();
                            grdfaultystore.DataBind();

                        }
                        else
                        {
                            ViewState["FaultyStore"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadFailureDetails();
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdfaultystore_RowCommand");
            }
        }


        protected void grdBrandNewDtr_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txttCode = (TextBox)row.FindControl("txttCode");
                    TextBox txttcCapacity = (TextBox)row.FindControl("txttcCapacity");

                    DataTable dt = (DataTable)ViewState["BrandNew"];
                    dv = dt.DefaultView;

                    if (txttCode.Text != "")
                    {
                        sFilter = "Convert(TC_CODE, 'System.String') Like '%" + txttCode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txttcCapacity.Text != "")
                    {
                        sFilter += " Convert(TC_CAPACITY, 'System.String') Like '%" + txttcCapacity.Text.Replace("'", "'") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdBrandNewDtr.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdBrandNewDtr.DataSource = dv;
                            ViewState["BrandNew"] = dv.ToTable();
                            grdBrandNewDtr.DataBind();

                        }
                        else
                        {
                            ViewState["BrandNew"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadFailureDetails();
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdBrandNewDtr_RowCommand");
            }
        }



        protected void grdNonRepairabler_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();
                    DataTable dt = new DataTable();
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txttCode = (TextBox)row.FindControl("txttCode");
                    TextBox txttcCapacity = (TextBox)row.FindControl("txttcCapacity");

                    if (Request.QueryString["value"] == "ReleasedGood")
                    {
                        dt = (DataTable)ViewState["ReleasedGood"];
                        dv = dt.DefaultView;
                    }
                    else
                    {
                        dt = (DataTable)ViewState["NonRepairable"];
                        dv = dt.DefaultView;
                    }




                    if (txttCode.Text != "")
                    {
                        sFilter = "Convert(TC_CODE, 'System.String') Like '%" + txttCode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txttcCapacity.Text != "")
                    {
                        sFilter += " Convert(TC_CAPACITY, 'System.String') Like '%" + txttcCapacity.Text.Replace("'", "'") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdNonRepairable.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdNonRepairable.DataSource = dv;
                            ViewState["NonRepairable"] = dv.ToTable();
                            grdNonRepairable.DataBind();

                        }
                        else
                        {
                            ViewState["NonRepairable"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadFailureDetails();
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdBrandNewDtr_RowCommand");
            }
        }

       

        protected void grdScrapdtr_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txttCode = (TextBox)row.FindControl("txttCode");
                    TextBox txttcCapacity = (TextBox)row.FindControl("txttcCapacity");

                    DataTable dt = (DataTable)ViewState["ScrapStore"];
                    dv = dt.DefaultView;

                    if (txttCode.Text != "")
                    {
                        sFilter = "Convert(TC_CODE, 'System.String') Like '%" + txttCode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txttcCapacity.Text != "")
                    {
                        sFilter += " Convert(TC_CAPACITY, 'System.String') Like '%" + txttcCapacity.Text.Replace("'", "'") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdScrapdtr.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdScrapdtr.DataSource = dv;
                            ViewState["ScrapStore"] = dv.ToTable();
                            grdScrapdtr.DataBind();

                        }
                        else
                        {
                            ViewState["ScrapStore"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadFailureDetails();
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdBrandNewDtr_RowCommand");
            }
        }


        protected void grdfaultyrepairer_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txttCode = (TextBox)row.FindControl("txttCode");
                    TextBox txttcCapacity = (TextBox)row.FindControl("txttcCapacity");

                    DataTable dt;
                    if(ViewState["FaultyRepairer"] != null )
                        dt = (DataTable)ViewState["FaultyRepairer"];
                    else
                        dt = (DataTable)ViewState["FaultySupplier"];
                    dv = dt.DefaultView;

                    if (txttCode.Text != "")
                    {
                        sFilter = "Convert(TC_CODE, 'System.String') Like '%" + txttCode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txttcCapacity.Text != "")
                    {
                        sFilter += " Convert(TC_CAPACITY, 'System.String') Like '%" + txttcCapacity.Text.Replace("'", "'") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdfaultyrepairer.PageIndex = 0;
                        grdSupplier.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                           
                            if (ViewState["FaultyRepairer"] != null)
                            {
                                ViewState["FaultyRepairer"] = dv.ToTable();
                                grdfaultyrepairer.DataSource = dv;
                                grdfaultyrepairer.DataBind();
                            }
                            else
                            {
                                ViewState["FaultySupplier"] = dv.ToTable();
                                grdSupplier.DataSource = dv;
                                grdSupplier.DataBind();
                            }
                        }
                        else
                        {
                            if (ViewState["FaultyRepairer"] != null)
                                ViewState["FaultyRepairer"] = dv.ToTable();
                            else
                                ViewState["FaultySupplier"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadFailureDetails();
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdfaultyrepairer_RowCommand");
            }
        }
        #endregion

        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);

                if (Request.QueryString["value"] == "TcFailed")
                {
                    dt.Columns.Add("DIVISION");
                    dt.Columns.Add("TC_CODE");
                    dt.Columns.Add("TC_CAPACITY");
                    dt.Columns.Add("TM_NAME");
                    dt.Columns.Add("TC_SLNO");
                    dt.Columns.Add("TC_MANF_DATE");

                    grdFailuretc.DataSource = dt;
                    grdFailuretc.DataBind();

                    int iColCount = grdFailuretc.Rows[0].Cells.Count;
                    grdFailuretc.Rows[0].Cells.Clear();
                    grdFailuretc.Rows[0].Cells.Add(new TableCell());
                    grdFailuretc.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdFailuretc.Rows[0].Cells[0].Text = "No Records Found";
                }
                if (Request.QueryString["value"] == "TotalFaulty")
                {
                    dt.Columns.Add("DIVISION");
                    dt.Columns.Add("TC_CODE");
                    dt.Columns.Add("TC_CAPACITY");
                    dt.Columns.Add("TM_NAME");
                    dt.Columns.Add("TC_SLNO");
                    dt.Columns.Add("TC_MANF_DATE");

                    grdtotaldtr.DataSource = dt;
                    grdtotaldtr.DataBind();

                    int iColCount = grdtotaldtr.Rows[0].Cells.Count;
                    grdtotaldtr.Rows[0].Cells.Clear();
                    grdtotaldtr.Rows[0].Cells.Add(new TableCell());
                    grdtotaldtr.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdtotaldtr.Rows[0].Cells[0].Text = "No Records Found";
                }
                if (Request.QueryString["value"] == "FaultyField")
                {
                    dt.Columns.Add("DIVISION");
                    dt.Columns.Add("SUBDIVISION");
                    dt.Columns.Add("SECTION");
                    dt.Columns.Add("TC_CODE");
                    dt.Columns.Add("TC_CAPACITY");
                    dt.Columns.Add("TC_SLNO");
                    dt.Columns.Add("DF_GUARANTY_TYPE");
                    dt.Columns.Add("TC_MANF_DATE");
                    dt.Columns.Add("TM_NAME");
                    dt.Columns.Add("TR_RI_DATE");
                    dt.Columns.Add("IN_MANUAL_INVNO");
                    dt.Columns.Add("IN_DATE");



                    grdfaultyfield.DataSource = dt;
                    grdfaultyfield.DataBind();

                    int iColCount = grdfaultyfield.Rows[0].Cells.Count;
                    grdfaultyfield.Rows[0].Cells.Clear();
                    grdfaultyfield.Rows[0].Cells.Add(new TableCell());
                    grdfaultyfield.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdfaultyfield.Rows[0].Cells[0].Text = "No Records Found";
                }
                if (Request.QueryString["value"] == "FaultyStore")
                {
                    dt.Columns.Add("DIVISION");
                    dt.Columns.Add("TC_CODE");
                    dt.Columns.Add("TC_CAPACITY");
                    dt.Columns.Add("TM_NAME");
                    dt.Columns.Add("TC_SLNO");
                    dt.Columns.Add("TC_MANF_DATE");
                    dt.Columns.Add("DF_GUARANTY_TYPE");
                   

                    grdfaultystore.DataSource = dt;
                    grdfaultystore.DataBind();

                    int iColCount = grdfaultystore.Rows[0].Cells.Count;
                    grdfaultystore.Rows[0].Cells.Clear();
                    grdfaultystore.Rows[0].Cells.Add(new TableCell());
                    grdfaultystore.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdfaultystore.Rows[0].Cells[0].Text = "No Records Found";
                }
                if (Request.QueryString["value"] == "FaultyRepairer")
                {
                    dt.Columns.Add("DIVISION");
                    dt.Columns.Add("TC_CODE");
                    dt.Columns.Add("TC_CAPACITY");
                    dt.Columns.Add("TM_NAME");
                    dt.Columns.Add("TC_SLNO");
                    dt.Columns.Add("TC_MANF_DATE");
                    dt.Columns.Add("RSM_PO_NO");
                    dt.Columns.Add("RSM_PO_DATE");
                    dt.Columns.Add("SUPPLIER");

                    grdfaultyrepairer.DataSource = dt;
                    grdfaultyrepairer.DataBind();

                    int iColCount = grdfaultyrepairer.Rows[0].Cells.Count;
                    grdfaultyrepairer.Rows[0].Cells.Clear();
                    grdfaultyrepairer.Rows[0].Cells.Add(new TableCell());
                    grdfaultyrepairer.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdfaultyrepairer.Rows[0].Cells[0].Text = "No Records Found";
                }
                if (Request.QueryString["value"] == "FaultySupplier")
                {
                    dt.Columns.Add("DIVISION");
                    dt.Columns.Add("TC_CODE");
                    dt.Columns.Add("TC_CAPACITY");
                    dt.Columns.Add("TM_NAME");
                    dt.Columns.Add("TC_SLNO");
                    dt.Columns.Add("TC_MANF_DATE");
                    dt.Columns.Add("RSM_PO_NO");
                    dt.Columns.Add("RSM_PO_DATE");
                    dt.Columns.Add("SUPPLIER");

                    grdfaultyrepairer.DataSource = dt;
                    grdfaultyrepairer.DataBind();

                    int iColCount = grdfaultyrepairer.Rows[0].Cells.Count;
                    grdfaultyrepairer.Rows[0].Cells.Clear();
                    grdfaultyrepairer.Rows[0].Cells.Add(new TableCell());
                    grdfaultyrepairer.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdfaultyrepairer.Rows[0].Cells[0].Text = "No Records Found";
                }


                if (Request.QueryString["value"] == "BrandNew")
                {
                    dt.Columns.Add("DIVISION");
                    dt.Columns.Add("TC_CODE");
                    dt.Columns.Add("TC_CAPACITY");
                    dt.Columns.Add("TM_NAME");
                    dt.Columns.Add("TC_SLNO");
                    dt.Columns.Add("TC_MANF_DATE");
                    dt.Columns.Add("PO_NO");
                    dt.Columns.Add("PO_DATE");

                    dt.Columns.Add("VM_NAME");

                    grdBrandNewDtr.DataSource = dt;
                    grdBrandNewDtr.DataBind();

                    int iColCount = grdBrandNewDtr.Rows[0].Cells.Count;
                    grdBrandNewDtr.Rows[0].Cells.Clear();
                    grdBrandNewDtr.Rows[0].Cells.Add(new TableCell());
                    grdBrandNewDtr.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdBrandNewDtr.Rows[0].Cells[0].Text = "No Records Found";
                }

                if (Request.QueryString["value"] == "ScrapStore")
                {
                    dt.Columns.Add("DIVISION");
                    dt.Columns.Add("TC_CODE");
                    dt.Columns.Add("TC_CAPACITY");
                    dt.Columns.Add("TM_NAME");
                    dt.Columns.Add("TC_SLNO");
                    dt.Columns.Add("TC_MANF_DATE");

                    grdScrapdtr.DataSource = dt;
                    grdScrapdtr.DataBind();

                    int iColCount = grdScrapdtr.Rows[0].Cells.Count;
                    grdScrapdtr.Rows[0].Cells.Clear();
                    grdScrapdtr.Rows[0].Cells.Add(new TableCell());
                    grdScrapdtr.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdScrapdtr.Rows[0].Cells[0].Text = "No Records Found";
                }
                if (Request.QueryString["value"] == "NonRepairable")
                {
                    dt.Columns.Add("DIVISION");
                    dt.Columns.Add("TC_CODE");
                    dt.Columns.Add("TC_CAPACITY");
                    dt.Columns.Add("TM_NAME");
                    dt.Columns.Add("TC_SLNO");
                    dt.Columns.Add("TC_MANF_DATE");

                    grdNonRepairable.DataSource = dt;
                    grdNonRepairable.DataBind();

                    int iColCount = grdNonRepairable.Rows[0].Cells.Count;
                    grdNonRepairable.Rows[0].Cells.Clear();
                    grdNonRepairable.Rows[0].Cells.Add(new TableCell());
                    grdNonRepairable.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdNonRepairable.Rows[0].Cells[0].Text = "No Records Found";
                }

                if (Request.QueryString["value"] == "ReleasedGood")
                {
                    dt.Columns.Add("DIVISION");
                    dt.Columns.Add("TC_CODE");
                    dt.Columns.Add("TC_CAPACITY");
                    dt.Columns.Add("TM_NAME");
                    dt.Columns.Add("TC_SLNO");
                    dt.Columns.Add("TC_MANF_DATE");

                    grdNonRepairable.DataSource = dt;
                    grdNonRepairable.DataBind();

                    int iColCount = grdNonRepairable.Rows[0].Cells.Count;
                    grdNonRepairable.Rows[0].Cells.Clear();
                    grdNonRepairable.Rows[0].Cells.Add(new TableCell());
                    grdNonRepairable.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdNonRepairable.Rows[0].Cells[0].Text = "No Records Found";
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadFailureDetails");
            }
        }

        protected void Export_clickTcFailure(object sender, EventArgs e)
        {
            //clsDashboard objDashboard = new clsDashboard();
            DataTable dt = new DataTable();


            //objDashboard.sOfficeCode = hdfOffCode.Value;


            if (Request.QueryString["value"] == "TcFailed")
            {
                //dt = objDashboard.TotalTcfailedview(hdfOffCode.Value);
                dt = (DataTable)ViewState["FailureTc"];

                if (dt.Rows.Count > 0)
                {

                    dt.Columns["TC_CODE"].ColumnName = "DTR CODE";
                    dt.Columns["TC_CAPACITY"].ColumnName = "DTR CAPACITY";
                    dt.Columns["TC_SLNO"].ColumnName = "DTR SLNO";
                    dt.Columns["TC_MANF_DATE"].ColumnName = "DTR MANUFACTURE DATE";
                    dt.Columns["TM_NAME"].ColumnName = "Make Name";
                    dt.Columns["DIVISION"].ColumnName = "Division";

                    dt.Columns["Division"].SetOrdinal(0);
                    dt.Columns["DTR CODE"].SetOrdinal(1);
                    dt.Columns["DTR CAPACITY"].SetOrdinal(2);
                    dt.Columns["Make Name"].SetOrdinal(3);
                    dt.Columns["DTR SLNO"].SetOrdinal(4);
                    dt.Columns["DTR MANUFACTURE DATE"].SetOrdinal(5);

                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "TcFailed" + DateTime.Now + ".xls";
                    string pagetitle = "Repair Good DTR Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }
            if (Request.QueryString["value"] == "TotalFaulty")
            {

                // dt = objDashboard.GetTotalFaultyTCview(hdfOffCode.Value);
                dt = (DataTable)ViewState["TotalFaulty"];
                if (dt.Rows.Count > 0)
                {

                   
                    dt.Columns["TC_CODE"].ColumnName = "DTR CODE";
                    dt.Columns["TC_CAPACITY"].ColumnName = "DTR CAPACITY";
                    dt.Columns["TC_SLNO"].ColumnName = "DTR SLNO";
                    dt.Columns["TC_MANF_DATE"].ColumnName = "DTR MANUFACTURE DATE";
                    dt.Columns["TM_NAME"].ColumnName = "Make Name";
                    dt.Columns["DIVISION"].ColumnName = "Division";

                    dt.Columns["Division"].SetOrdinal(0);
                    dt.Columns["DTR CODE"].SetOrdinal(1);
                    dt.Columns["DTR CAPACITY"].SetOrdinal(2);
                    dt.Columns["Make Name"].SetOrdinal(3);
                    dt.Columns["DTR SLNO"].SetOrdinal(4);
                    dt.Columns["DTR MANUFACTURE DATE"].SetOrdinal(5);

                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "TotalFaulty" + DateTime.Now + ".xls";
                    string pagetitle = "Total Faulty DTR Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }

            if (Request.QueryString["value"] == "FaultyField")
            {
                // dt = objDashboard.GetFaultyTCFieldview(hdfOffCode.Value);
                dt = (DataTable)ViewState["FaultyField"];
                if (dt.Rows.Count > 0)
                {

                    dt.Columns["TC_CODE"].ColumnName = "DTr Code";
                    dt.Columns["TC_CAPACITY"].ColumnName = "DTr Capacity";
                    dt.Columns["TC_SLNO"].ColumnName = "DTr Slno";
                    dt.Columns["TC_MANF_DATE"].ColumnName = "DTr Manufacture Date";
                    dt.Columns["DIVISION"].ColumnName = "Division";
                    dt.Columns["SUBDIVISION"].ColumnName = "Subdivision";
                    dt.Columns["SECTION"].ColumnName = "Section";
                    dt.Columns["TM_NAME"].ColumnName = "Make Name";
                    dt.Columns["IN_MANUAL_INVNO"].ColumnName = "Invoice No";
                    dt.Columns["IN_DATE"].ColumnName = "Invoice Date";
                    dt.Columns["TR_RI_DATE"].ColumnName = "Decommissioning Date";
                    dt.Columns["DF_GUARANTY_TYPE"].ColumnName = "Guaranty Type";

                    dt.Columns["Division"].SetOrdinal(0);
                    dt.Columns["Subdivision"].SetOrdinal(1);
                    dt.Columns["Section"].SetOrdinal(2);
                    dt.Columns["DTr Code"].SetOrdinal(3);
                    dt.Columns["DTr Capacity"].SetOrdinal(4);
                    dt.Columns["Make Name"].SetOrdinal(5);
                    dt.Columns["DTr Slno"].SetOrdinal(6);
                    dt.Columns["DTr Manufacture Date"].SetOrdinal(7);
                    dt.Columns["Guaranty Type"].SetOrdinal(8);

                    dt.Columns["Invoice No"].SetOrdinal(9);
                    dt.Columns["Invoice Date"].SetOrdinal(10);
                    dt.Columns["Decommissioning Date"].SetOrdinal(11);
                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "FaultyDTratField" + DateTime.Now + ".xls";
                    string pagetitle = " Faulty DTR at Field Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }
            if (Request.QueryString["value"] == "FaultyStore")
            {
                //  dt = objDashboard.GetFaultyTCStoreview(hdfOffCode.Value);
                dt = (DataTable)ViewState["FaultyStore"];
                if (dt.Rows.Count > 0)
                {

                    dt.Columns["TC_CODE"].ColumnName = "DTR CODE";
                    dt.Columns["TC_CAPACITY"].ColumnName = "DTR CAPACITY";
                    dt.Columns["TC_SLNO"].ColumnName = "DTR SLNO";
                    dt.Columns["TC_MANF_DATE"].ColumnName = "DTR MANUFACTURE DATE";
                    dt.Columns["TM_NAME"].ColumnName = "Make Name";
                    dt.Columns["DIVISION"].ColumnName = "Division";
                   

                    dt.Columns["Division"].SetOrdinal(0);
                    dt.Columns["DTR CODE"].SetOrdinal(1);
                    dt.Columns["DTR CAPACITY"].SetOrdinal(2);
                    dt.Columns["Make Name"].SetOrdinal(3);
                    dt.Columns["DTR SLNO"].SetOrdinal(4);
                    dt.Columns["DTR MANUFACTURE DATE"].SetOrdinal(5);

                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "FaultyDtratStore" + DateTime.Now + ".xls";
                    string pagetitle = "Faulty DTR at Store Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }
            if (Request.QueryString["value"] == "FaultyRepairer")
            {
                // dt = objDashboard.GetFaultyTCRepairview(hdfOffCode.Value);
                dt = (DataTable)ViewState["FaultyRepairer"];


                if (dt.Rows.Count > 0)
                {

                    dt.Columns["TC_CODE"].ColumnName = "DTR CODE";
                    dt.Columns["TC_CAPACITY"].ColumnName = "DTR CAPACITY";
                    dt.Columns["TC_SLNO"].ColumnName = "DTR SLNO";
                    dt.Columns["TC_MANF_DATE"].ColumnName = "DTR MANUFACTURE DATE";
                    dt.Columns["TM_NAME"].ColumnName = "Make Name";
                    dt.Columns["DIVISION"].ColumnName = "Division";
                    dt.Columns["SUPPLIER"].ColumnName = "Repairer Name";

                    dt.Columns["Division"].SetOrdinal(0);
                    dt.Columns["DTR CODE"].SetOrdinal(1);
                    dt.Columns["DTR CAPACITY"].SetOrdinal(2);
                    dt.Columns["Make Name"].SetOrdinal(3);
                    dt.Columns["DTR SLNO"].SetOrdinal(4);
                    dt.Columns["DTR MANUFACTURE DATE"].SetOrdinal(5);

                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "FaultyDtratRepairer" + DateTime.Now + ".xls";
                    string pagetitle = "Faulty DTR at Repairer Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }

            // faulty supplier  

            if (Request.QueryString["value"] == "FaultySupplier")
            {
                // dt = objDashboard.GetFaultyTCRepairview(hdfOffCode.Value);
                dt = (DataTable)ViewState["FaultySupplier"];


                if (dt.Rows.Count > 0)
                {

                    dt.Columns["TC_CODE"].ColumnName = "DTR CODE";
                    dt.Columns["TC_CAPACITY"].ColumnName = "DTR CAPACITY";
                    dt.Columns["TC_SLNO"].ColumnName = "DTR SLNO";
                    dt.Columns["TC_MANF_DATE"].ColumnName = "DTR MANUFACTURE DATE";
                    dt.Columns["TM_NAME"].ColumnName = "Make Name";
                    dt.Columns["DIVISION"].ColumnName = "Division";
                    dt.Columns["SUPPLIER"].ColumnName = "Supplier Name";

                    dt.Columns["Division"].SetOrdinal(0);
                    dt.Columns["DTR CODE"].SetOrdinal(1);
                    dt.Columns["DTR CAPACITY"].SetOrdinal(2);
                    dt.Columns["Make Name"].SetOrdinal(3);
                    dt.Columns["DTR SLNO"].SetOrdinal(4);
                    dt.Columns["DTR MANUFACTURE DATE"].SetOrdinal(5);

                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "FaultyDtratSupplier" + DateTime.Now + ".xls";
                    string pagetitle = "Faulty DTR at Repairer Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }




            // brand new , faulty and repair good 
            //BrandNew // ScrapStore  //NonRepairable
            if (Request.QueryString["value"] == "BrandNew")
            {
                // dt = objDashboard.GetFaultyTCRepairview(hdfOffCode.Value);
                dt = (DataTable)ViewState["BrandNew"];


                if (dt.Rows.Count > 0)
                {

                    dt.Columns["TC_CODE"].ColumnName = "DTR CODE";
                    dt.Columns["TC_CAPACITY"].ColumnName = "DTR CAPACITY";
                    dt.Columns["TC_SLNO"].ColumnName = "DTR SLNO";
                    dt.Columns["TC_MANF_DATE"].ColumnName = "DTR MANUFACTURE DATE";
                    dt.Columns["TM_NAME"].ColumnName = "Make Name";
                    dt.Columns["DIVISION"].ColumnName = "Division";

                    dt.Columns["Division"].SetOrdinal(0);
                    dt.Columns["DTR CODE"].SetOrdinal(1);
                    dt.Columns["DTR CAPACITY"].SetOrdinal(2);
                    dt.Columns["Make Name"].SetOrdinal(3);
                    dt.Columns["DTR SLNO"].SetOrdinal(4);
                    dt.Columns["DTR MANUFACTURE DATE"].SetOrdinal(5);

                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "BrandNew" + DateTime.Now + ".xls";
                    string pagetitle = "Brand New Dtr Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }

            if (Request.QueryString["value"] == "ScrapStore")
            {
                // dt = objDashboard.GetFaultyTCRepairview(hdfOffCode.Value);
                dt = (DataTable)ViewState["ScrapStore"];


                if (dt.Rows.Count > 0)
                {

                    dt.Columns["TC_CODE"].ColumnName = "DTR CODE";
                    dt.Columns["TC_CAPACITY"].ColumnName = "DTR CAPACITY";
                    dt.Columns["TC_SLNO"].ColumnName = "DTR SLNO";
                    dt.Columns["TC_MANF_DATE"].ColumnName = "DTR MANUFACTURE DATE";
                    dt.Columns["TM_NAME"].ColumnName = "Make Name";
                    dt.Columns["DIVISION"].ColumnName = "Division";

                    dt.Columns["Division"].SetOrdinal(0);
                    dt.Columns["DTR CODE"].SetOrdinal(1);
                    dt.Columns["DTR CAPACITY"].SetOrdinal(2);
                    dt.Columns["Make Name"].SetOrdinal(3);
                    dt.Columns["DTR SLNO"].SetOrdinal(4);
                    dt.Columns["DTR MANUFACTURE DATE"].SetOrdinal(5);

                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "ScrapStore" + DateTime.Now + ".xls";
                    string pagetitle = "Scrap DTR Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }

            if (Request.QueryString["value"] == "NonRepairable")
            {
                // dt = objDashboard.GetFaultyTCRepairview(hdfOffCode.Value);
                dt = (DataTable)ViewState["NonRepairable"];


                if (dt.Rows.Count > 0)
                {

                    dt.Columns["TC_CODE"].ColumnName = "DTR CODE";
                    dt.Columns["TC_CAPACITY"].ColumnName = "DTR CAPACITY";
                    dt.Columns["TC_SLNO"].ColumnName = "DTR SLNO";
                    dt.Columns["TC_MANF_DATE"].ColumnName = "DTR MANUFACTURE DATE";
                    dt.Columns["TM_NAME"].ColumnName = "Make Name";
                    dt.Columns["DIVISION"].ColumnName = "Division";

                    dt.Columns["Division"].SetOrdinal(0);
                    dt.Columns["DTR CODE"].SetOrdinal(1);
                    dt.Columns["DTR CAPACITY"].SetOrdinal(2);
                    dt.Columns["Make Name"].SetOrdinal(3);
                    dt.Columns["DTR SLNO"].SetOrdinal(4);
                    dt.Columns["DTR MANUFACTURE DATE"].SetOrdinal(5);

                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "NonRepairable" + DateTime.Now + ".xls";
                    string pagetitle = "NonRepairable DTR Details";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }

            }
            
            if (Request.QueryString["value"] == "ReleasedGood")
            {
                // dt = objDashboard.GetFaultyTCRepairview(hdfOffCode.Value);
                dt = (DataTable)ViewState["ReleasedGood"];


                if (dt.Rows.Count > 0)
                {

                    dt.Columns["TC_CODE"].ColumnName = "DTR CODE";
                    dt.Columns["TC_CAPACITY"].ColumnName = "DTR CAPACITY";
                    dt.Columns["TC_SLNO"].ColumnName = "DTR SLNO";
                    dt.Columns["TC_MANF_DATE"].ColumnName = "DTR MANUFACTURE DATE";
                    dt.Columns["TM_NAME"].ColumnName = "Make Name";
                    dt.Columns["DIVISION"].ColumnName = "Division";

                    dt.Columns["Division"].SetOrdinal(0);
                    dt.Columns["DTR CODE"].SetOrdinal(1);
                    dt.Columns["DTR CAPACITY"].SetOrdinal(2);
                    dt.Columns["Make Name"].SetOrdinal(3);
                    dt.Columns["DTR SLNO"].SetOrdinal(4);
                    dt.Columns["DTR MANUFACTURE DATE"].SetOrdinal(5);

                    List<string> listtoRemove = new List<string> { "" };
                    string filename = "Released Good DTR" + DateTime.Now + ".xls";
                    string pagetitle = "Released Good DTR  Details";

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
    }
}