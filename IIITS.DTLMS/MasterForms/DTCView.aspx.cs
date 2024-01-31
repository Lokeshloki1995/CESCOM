using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Configuration;

namespace IIITS.DTLMS.MasterForms
{
    public partial class DTCView : System.Web.UI.Page
    {
        string strFormCode = "DTCView";
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
                    CheckAccessRights("4");
                    LoadCombo();
                    LoadDtcDetails();


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load");
            }

        }

        public void LoadCombo()
        {
            try
            {
                string sOfficeCode = string.Empty;
                if (objSession.OfficeCode.Length > 3)
                {
                    sOfficeCode = objSession.OfficeCode.Substring(0, 3);
                }
                string strQry = "SELECT DISTINCT FD_FEEDER_CODE,FD_FEEDER_CODE || '-' || FD_FEEDER_NAME FROM TBLFEEDERMAST, TBLFEEDEROFFCODE WHERE  FD_FEEDER_ID = FDO_FEEDER_ID AND";
                strQry += " FDO_OFFICE_CODE LIKE '" + sOfficeCode + "%' ORDER BY FD_FEEDER_CODE";
                Genaral.Load_Combo(strQry, "--Select--", cmbFeeder);

                Genaral.Load_Combo("SELECT MD_ID, MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'PT'", "--Select--", cmbProjectType);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadCombo");
            }

        }

        public void LoadDtcDetails()
        {

            try
            {
                clsDtcMaster ObjDtcMaster = new clsDtcMaster();


                if (Request.QueryString["offCode"] != null && Request.QueryString["offCode"].ToString() != "")
                {
                    ObjDtcMaster.sOfficeCode = Request.QueryString["offCode"];
                }
                else
                {
                    ObjDtcMaster.sOfficeCode = objSession.OfficeCode;
                }


                if (cmbFeeder.SelectedIndex > 0)
                {
                    ObjDtcMaster.sFeederCode = cmbFeeder.SelectedValue;
                }
                if (cmbProjectType.SelectedIndex > 0)
                {
                    ObjDtcMaster.sProjectType = cmbProjectType.SelectedValue;
                }

                DataTable dtDtcDetails = ObjDtcMaster.LoadDtcGrid(ObjDtcMaster, "GRID");
                if (dtDtcDetails.Rows.Count == 0)
                {

                    ShowEmptyGrid();
                    ViewState["DTC"] = dtDtcDetails;
                }
                else
                {
                    grdDtc.DataSource = dtDtcDetails;
                    grdDtc.DataBind();
                    ViewState["DTC"] = dtDtcDetails;
                }
                lblTotalDTC.Text = "Total DTC Count : " + ObjDtcMaster.GetDTCCount(ObjDtcMaster);
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadDtcDetails");
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
                Response.Redirect("DTCCommision.aspx", false);

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdNew_Click");
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
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowMsgBox");
            }
        }
        protected void grdDtc_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdDtc.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["DTC"];
                grdDtc.DataSource = SortDataTable(dt as DataTable, true);
                grdDtc.DataBind();
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdDtc_PageIndexChanging");

            }
        }

        protected void grdDtc_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdDtc.PageIndex;
            DataTable dt = (DataTable)ViewState["DTC"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {
                grdDtc.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdDtc.DataSource = dt;
            }
            grdDtc.DataBind();
            grdDtc.PageIndex = pageIndex;
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

                        if (dataView.Sort.ToString() == "TC_CAPACITY ASC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderBy(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["DTC"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["DTC"] = dataView.ToTable();
                        }

                        else
                        {
                            ViewState["DTC"] = dataView.ToTable();
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
                            ViewState["DTC"] = dataView.ToTable();
                        }
                        else if (dataView.Sort.ToString() == "TC_CAPACITY DESC")
                        {

                            DataTable dt = dataTable.AsEnumerable()
                           .OrderByDescending(r => int.Parse(r.Field<String>("TC_CAPACITY")))
                          .CopyToDataTable();

                            dataView = new DataView(dt);
                            ViewState["DTC"] = dataView.ToTable();
                        }
                        else
                        {
                            ViewState["DTC"] = dataView.ToTable();
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

        /// <summary>
        /// this method used for grid  row data bound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdDtc_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    ImageButton lnkEdit = (ImageButton)e.Row.FindControl("imgBtnEdit");

                    if (objSession.RoleId == ConfigurationManager.AppSettings["LTROLEID"])
                    {
                        lnkEdit.Enabled = false;
                    }
                    else
                    {
                        lnkEdit.Enabled = true;
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
        protected void grdDtc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "create")
                {
                    bool bAccResult = CheckAccessRights("2");
                    if (bAccResult == false)
                    {
                        return;
                    }
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                    string strDtcId = ((Label)row.FindControl("lblDtcId")).Text;
                    strDtcId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(strDtcId));
                    Response.Redirect("DTCCommision.aspx?QryDtcId=" + strDtcId + "", false);
                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDTCCode = (TextBox)row.FindControl("txtDTCCode");
                    TextBox txtDTCName = (TextBox)row.FindControl("txtDTCName");
                    TextBox txtTCCode = (TextBox)row.FindControl("txtTCCode");
                    TextBox txtFeederName = (TextBox)row.FindControl("txtFeederName");


                    DataTable dt = (DataTable)ViewState["DTC"];
                    if (dt.Rows.Count > 0)
                    {
                        dv = dt.DefaultView;

                        if (txtFeederName.Text != "")
                        {
                            sFilter += " FEEDER_NAME Like '%" + txtFeederName.Text.Replace("'", "'") + "%' AND";
                        }

                        if (txtDTCCode.Text != "")
                        {
                            //code to fetch if the  dtc has been bifurcated .
                            clsDtcMaster objDtc = new clsDtcMaster();
                            objDtc.sDtcCode = txtDTCCode.Text;
                            objDtc.sOfficeCode = objSession.OfficeCode;
                            string sDTCCode = objDtc.GetNewDTCCode(objDtc);
                            if (!(sDTCCode == "" || sDTCCode == null))
                            {
                                string msg = "OLD DTC Code =  " + txtDTCCode.Text + "  NEW DTC Code =  " + sDTCCode + " ";
                                ShowMsgBox(msg);
                                sFilter = "DT_CODE Like '%" + sDTCCode.Replace("'", "'") + "%' AND";

                                sFilter = "DT_CODE Like '%" + sDTCCode.Replace("'", "'") + "%' AND";
                            }
                            else
                            {
                                sFilter = "DT_CODE Like '%" + txtDTCCode.Text.Replace("'", "'") + "%' AND";
                            }
                        }

                        if (txtDTCName.Text != "")
                        {
                            sFilter += " DT_NAME Like '%" + txtDTCName.Text.Replace("'", "'") + "%' AND";
                        }

                        if (txtTCCode.Text != "")
                        {
                            sFilter += " TC_CODE Like '%" + txtTCCode.Text.Replace("'", "'") + "%' AND";
                        }
                        if (sFilter.Length > 0)
                        {
                            sFilter = sFilter.Remove(sFilter.Length - 3);
                            grdDtc.PageIndex = 0;
                            dv.RowFilter = sFilter;
                            #region old
                            //if (dv.Count > 0)
                            //{
                            //    dv.RowFilter = sFilter;
                            //}

                            //if (sFilter.Contains("FEEDER_NAME"))
                            //{
                            //    clsDtcMaster objDTCMaster = new clsDtcMaster();

                            //    if (txtFeederName.Text != "")
                            //    {
                            //        objDTCMaster.sFeederCode = txtFeederName.Text.Trim();
                            //    }

                            //    DataTable dtSingleDTC = objDTCMaster.GetDTCDetailsForSearch(objDTCMaster);

                            //    if (dtSingleDTC.Rows.Count > 0)
                            //    {
                            //        grdDtc.DataSource = dtSingleDTC;
                            //        grdDtc.DataBind();
                            //        ViewState["DTC"] = dtSingleDTC;
                            //    }
                            //    else
                            //    {
                            //        //LoadDtcDetails();
                            //        ShowEmptyGrid();
                            //    }
                            //    return;
                            //}


                            //if (dv.Count > 0)
                            //{
                            //    grdDtc.DataSource = dv;
                            //    ViewState["DTC"] = dv.ToTable();
                            //    grdDtc.DataBind();

                            //}
                            //else
                            //{
                            //    clsDtcMaster objDTCMaster = new clsDtcMaster();
                            //    if (txtDTCCode.Text != "")
                            //    {
                            //        objDTCMaster.sDtcCode = txtDTCCode.Text.Trim();
                            //    }
                            //    if (txtDTCName.Text != "")
                            //    {
                            //        objDTCMaster.sDtcName = txtDTCName.Text.Trim();

                            //    }
                            //    if (txtTCCode.Text != "")
                            //    {
                            //        objDTCMaster.sTcCode = txtTCCode.Text.Trim();

                            //    }
                            //    if (txtFeederName.Text != "")
                            //    {
                            //        objDTCMaster.sFeederCode = txtFeederName.Text.Trim();
                            //    }
                            //    DataTable dtSingleDTC = objDTCMaster.GetDTCDetailsForSearch(objDTCMaster);

                            //    if (dtSingleDTC.Rows.Count > 0)
                            //    {
                            //        grdDtc.DataSource = dtSingleDTC;
                            //        grdDtc.DataBind();
                            //        ViewState["DTC"] = dtSingleDTC;
                            //    }
                            //    else
                            //    {
                            //        //LoadDtcDetails();
                            //        ShowEmptyGrid();
                            //    }
                            //}
                            #endregion
                            if (dv.Count > 0)
                            {
                                grdDtc.DataSource = dv;
                                ViewState["DTC"] = dv.ToTable();
                                grdDtc.DataBind();
                            }
                            else
                            {
                                clsDtcMaster objDTCMaster = new clsDtcMaster();
                                if (txtDTCCode.Text != "")
                                {
                                    objDTCMaster.sDtcCode = txtDTCCode.Text.Trim();
                                }
                                if (txtDTCName.Text != "")
                                {
                                    objDTCMaster.sDtcName = txtDTCName.Text.Trim();

                                }
                                if (txtTCCode.Text != "")
                                {
                                    objDTCMaster.sTcCode = txtTCCode.Text.Trim();

                                }
                                if (txtFeederName.Text != "")
                                {
                                    objDTCMaster.sFeederCode = txtFeederName.Text.Trim();
                                }
                                DataTable dtSingleDTC = objDTCMaster.GetDTCDetailsForSearch(objDTCMaster);

                                if (dtSingleDTC.Rows.Count > 0)
                                {
                                    grdDtc.DataSource = dtSingleDTC;
                                    grdDtc.DataBind();
                                    ViewState["DTC"] = dtSingleDTC;
                                }
                                else
                                {
                                    //LoadDtcDetails();
                                    ShowEmptyGrid();
                                }
                            }
                        }
                        else
                        {
                            LoadDtcDetails();
                        }
                    }
                    else
                    {
                        ShowEmptyGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdDtc_RowCommand");

            }
        }

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
                dt.Columns.Add("TC_CODE");
                dt.Columns.Add("DT_TOTAL_CON_KW");
                dt.Columns.Add("DT_TOTAL_CON_HP");
                dt.Columns.Add("TC_CAPACITY");
                dt.Columns.Add("DT_LAST_SERVICE_DATE");
                dt.Columns.Add("FEEDER_NAME");
                dt.Columns.Add("DT_PROJECTTYPE");
                dt.Columns.Add("DIVISION");
                dt.Columns.Add("SUBDIVISION");
                dt.Columns.Add("SECTION");

                grdDtc.DataSource = dt;
                grdDtc.DataBind();

                int iColCount = grdDtc.Rows[0].Cells.Count;
                grdDtc.Rows[0].Cells.Clear();
                grdDtc.Rows[0].Cells.Add(new TableCell());
                grdDtc.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdDtc.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowEmptyGrid");

            }
        }

        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "DTCCommision";
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

        protected void cmbFeeder_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadDtcDetails();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbFeeder_SelectedIndexChanged");
            }
        }
        protected void cmbProjectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadDtcDetails();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbProjectType_SelectedIndexChanged");
            }
        }

        protected void Export_clickDTCMaster(object sender, EventArgs e)
        {
            try
            {

                clsDtcMaster ObjDtcMaster = new clsDtcMaster();
                if (Request.QueryString["offCode"] != null && Request.QueryString["offCode"].ToString() != "")
                {
                    ObjDtcMaster.sOfficeCode = Request.QueryString["offCode"];
                }
                else
                {
                    ObjDtcMaster.sOfficeCode = objSession.OfficeCode;
                }
                if (cmbFeeder.SelectedIndex > 0)
                {
                    ObjDtcMaster.sFeederCode = cmbFeeder.SelectedValue;
                }
                if (cmbProjectType.SelectedIndex > 0)
                {
                    ObjDtcMaster.sProjectType = cmbProjectType.SelectedValue;
                }

                DataTable dtDtcDetails = ObjDtcMaster.LoadDtcGrid(ObjDtcMaster, "EXCEL");

                //dtDtcDetails = (DataTable)ViewState["DTC"];
                if (dtDtcDetails.Rows.Count > 0)
                {

                    dtDtcDetails.Columns["FEEDER_NAME"].ColumnName = "FEEDER NAME";
                    dtDtcDetails.Columns["DT_CODE"].ColumnName = "DTC CODE";
                    dtDtcDetails.Columns["DT_NAME"].ColumnName = "DTC NAME";
                    dtDtcDetails.Columns["TC_CODE"].ColumnName = "DTR CODE(SS PLATE NO)";
                    dtDtcDetails.Columns["TC_CAPACITY"].ColumnName = "DTR CAPACITY(IN KVA)";

                    dtDtcDetails.Columns["FEEDER NAME"].SetOrdinal(0);

                    List<string> listtoRemove = new List<string> { "DT_ID", "DT_TOTAL_CON_KW", "DT_TOTAL_CON_HP", "DT_LAST_SERVICE_DATE", "DT_PROJECTTYPE" };
                    string filename = "DTCDetails" + DateTime.Now + ".xls";
                    string pagetitle = "DTC Details";

                    Genaral.getexcel(dtDtcDetails, listtoRemove, filename, pagetitle);
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
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Export_clickDTCMaster");
            }

        }

    }
}