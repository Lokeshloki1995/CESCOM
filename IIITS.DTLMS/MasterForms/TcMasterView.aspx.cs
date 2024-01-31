using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;

namespace IIITS.DTLMS.MasterForms
{
    public partial class TcMasterView : System.Web.UI.Page
    {
        string strFormCode = "TcMasterView";
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
                    LoadTcMasterDetails();

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

                Genaral.Load_Combo("SELECT TM_ID,TM_NAME FROM TBLTRANSMAKES ORDER BY TM_NAME", "-Select-", cmbMake);
                Genaral.Load_Combo("SELECT MD_NAME,MD_NAME from TBLMASTERDATA WHERE MD_TYPE='C' ORDER BY MD_ID", "-Select-", cmbCapacity);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadCombo");
            }
        }

        public void LoadTcMasterDetails()
        {
            try
            {
                clsTcMaster objTCmaster = new clsTcMaster();
                objTCmaster.sOfficeCode = objSession.OfficeCode;

                if (cmbMake.SelectedIndex > 0)
                {
                    objTCmaster.sTcMakeId = cmbMake.SelectedValue;
                }
                if (cmbCapacity.SelectedIndex > 0)
                {
                    objTCmaster.sTcCapacity = cmbCapacity.SelectedValue;
                }

                DataTable dt = objTCmaster.LoadTcMaster(objTCmaster);
                if (dt.Rows.Count == 0)
                {
                    ShowEmptyGrid();
                    ViewState["TC"] = dt;
                }
                else
                {
                    grdTcMaster.DataSource = dt;
                    grdTcMaster.DataBind();
                    ViewState["TC"] = dt;
                }

                lblTotalDTr.Text = "Total Transformer Count : " + objTCmaster.GetDTRCount(objTCmaster);

            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadTcMasterDetails");
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
                Response.Redirect("TcMaster.aspx", false);

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

        protected void grdTcMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                //grdTcMaster.PageIndex = e.NewPageIndex;
                //DataTable dt = (DataTable)ViewState["TC"];
                //grdTcMaster.DataSource = SortDataTable(dt as DataTable, true);
                //grdTcMaster.DataBind();
                grdTcMaster.PageIndex = e.NewPageIndex;
                //Bind the results back
                DataTable dt = (DataTable)ViewState["TC"];
                grdTcMaster.DataSource = SortDataTable(dt as DataTable, true);
                grdTcMaster.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdTcMaster_PageIndexChanging");
            }
        }

        protected void grdTcMaster_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "create")
                {
                    bool bAccResult = CheckAccessRights("3");
                    if (bAccResult == false)
                    {
                        return;
                    }
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                    string strTcId = ((Label)row.FindControl("lblTcId")).Text;
                    strTcId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(strTcId));
                    Response.Redirect("TcMaster.aspx?TCId=" + strTcId + "", false);
                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtTCCode = (TextBox)row.FindControl("txtTCCode");
                    TextBox txtTCSlno = (TextBox)row.FindControl("txtTCSlno");
                    TextBox txtMake = (TextBox)row.FindControl("txtMake");

                    DataTable dt = (DataTable)ViewState["TC"];
                    dv = dt.DefaultView;

                    if (txtTCCode.Text != "")
                    {
                        sFilter = "TC_CODE Like '%" + txtTCCode.Text.Replace("'", "'") + "%' AND";
                    }

                    if (txtTCSlno.Text != "")
                    {
                        sFilter += " TC_SLNO Like '%" + txtTCSlno.Text.Replace("'", "'") + "%' AND";
                    }

                    if (txtMake.Text != "")
                    {
                        //sFilter += " TC_MAKE_ID Like '%" + txtMake.Text.Replace("'", "'") + "%' AND";
                        sFilter += " TM_NAME Like '%" + txtMake.Text.Replace("'", "'") + "%' AND";
                    }

                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdTcMaster.PageIndex = 0;
                        dv.RowFilter = sFilter;

                        if (dv.Count > 0)
                        {
                            grdTcMaster.DataSource = dv;
                            ViewState["TC"] = dv.ToTable();
                            grdTcMaster.DataBind();
                        }
                        else
                        {
                            clsTcMaster objtcmaster = new clsTcMaster();
                            if (txtTCCode.Text != "")
                            {
                                objtcmaster.sTcCode = txtTCCode.Text.Trim();
                            }
                            if (txtTCSlno.Text != "")
                            {
                                objtcmaster.sTcSlNo = txtTCSlno.Text.Trim();
                            }
                            if (txtMake.Text != "")
                            {
                                objtcmaster.sTcMakeId = txtMake.Text.Trim();
                            }

                            DataTable dtSingleDTC = objtcmaster.GetTCDetailsForSearch(objtcmaster);

                            if (dtSingleDTC.Rows.Count > 0)
                            {
                                grdTcMaster.DataSource = dtSingleDTC;
                                grdTcMaster.DataBind();
                                ViewState["TC"] = dtSingleDTC;
                            }
                            else
                            {
                                //LoadTcMasterDetails();
                                ShowEmptyGrid();
                            }
                        }
                    }
                    else
                    {
                        LoadTcMasterDetails();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdTcMaster_RowCommand");

            }
        }

        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("TC_ID");
                dt.Columns.Add("TC_CODE");
                dt.Columns.Add("TC_SLNO");
                //dt.Columns.Add("TC_MAKE_ID");
                dt.Columns.Add("TM_NAME");
                dt.Columns.Add("TC_CAPACITY");
                dt.Columns.Add("TC_LIFE_SPAN");


                grdTcMaster.DataSource = dt;
                grdTcMaster.DataBind();

                int iColCount = grdTcMaster.Rows[0].Cells.Count;
                grdTcMaster.Rows[0].Cells.Clear();
                grdTcMaster.Rows[0].Cells.Add(new TableCell());
                grdTcMaster.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdTcMaster.Rows[0].Cells[0].Text = "No Records Found";

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

                objApproval.sFormName = "TcMaster";
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

        protected void cmbMake_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadTcMasterDetails();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbMake_SelectedIndexChanged");
            }
        }

        protected void cmbCapacity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadTcMasterDetails();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbCapacity_SelectedIndexChanged");
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
                        ViewState["TC"] = dataView.ToTable();


                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["TC"] = dataView.ToTable();

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
        protected void grdTcMaster_Sorting(object sender, GridViewSortEventArgs e)
        {

            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdTcMaster.PageIndex;
            DataTable dt = (DataTable)ViewState["TC"];
            string sortingDirection = string.Empty;

            //DataView sortedView = new DataView(dt);
            //sortedView.Sort = e.SortExpression + " " + sortingDirection;
            if (dt.Rows.Count > 0)
            {
                grdTcMaster.DataSource = SortDataTable(dt as DataTable, false);
                //grdTcMaster.DataSource = dt.AsEnumerable().OrderBy(x => x[sortExpression]);
            }
            else
            {
                grdTcMaster.DataSource = dt;
            }

            grdTcMaster.DataBind();


            grdTcMaster.PageIndex = pageIndex;

            // DataTable dataTable = ViewState["TC"] as DataTable;
            // string sortingDirection = string.Empty;
            // if (direction == SortDirection.Ascending)
            // {
            //     direction = SortDirection.Descending;
            //     sortingDirection = "Desc";
            //     grdTcMaster.HeaderStyle.CssClass = "descending";

            // }
            // else
            // {
            //     direction = SortDirection.Ascending;
            //     sortingDirection = "Asc";
            //     grdTcMaster.HeaderStyle.CssClass = "ascending";

            // }
            // DataView sortedView = new DataView(dataTable);
            // sortedView.Sort = e.SortExpression + " " + sortingDirection;
            // Session["SortedView"] = sortedView;
            //// ViewState["SortedView"] = sortedView;
            // grdTcMaster.DataSource = sortedView;
            // grdTcMaster.DataBind();
        }

        //public SortDirection direction
        //{
        //    get
        //    {
        //        if (ViewState["directionState"] == null)
        //        {
        //            ViewState["directionState"] = SortDirection.Ascending;
        //        }
        //        return (SortDirection)ViewState["directionState"];
        //    }
        //    set
        //    {
        //        ViewState["directionState"] = value;
        //    }
        //}

        private string GetSortDirection()
        {
            switch (GridViewSortDirection)
            {
                case "ASC":
                    GridViewSortDirection = "DESC";
                    // grdTcMaster.HeaderStyle.CssClass = "descending";
                    break;
                case "DESC":
                    GridViewSortDirection = "ASC";
                    //grdTcMaster.HeaderStyle.CssClass = "ascending";
                    break;
            }


            return GridViewSortDirection;
        }

        protected void Export_clickTCMaster(object sender, EventArgs e)
        {

            clsTcMaster objTCmaster = new clsTcMaster();
           // DataTable dt = new DataTable();
            objTCmaster.sOfficeCode = objSession.OfficeCode;
            try
            {
                if (cmbMake.SelectedIndex > 0)
                {
                    objTCmaster.sTcMakeId = cmbMake.SelectedValue;
                }
                if (cmbCapacity.SelectedIndex > 0)
                {
                    objTCmaster.sTcCapacity = cmbCapacity.SelectedValue;
                }

               DataTable dt = objTCmaster.LoadTcMaster(objTCmaster);

               // DataTable dt = (DataTable)ViewState["TC"];

                if (dt.Rows.Count > 0)
                {
                    dt.Columns["TC_CODE"].ColumnName = "DTR CODE";
                    dt.Columns["TC_SLNO"].ColumnName = "DTR SLNO  ";
                  
                    dt.Columns["TC_MAKE_ID"].ColumnName = "MAKE NAME";
                    dt.Columns["TC_CAPACITY"].ColumnName = "CAPACITY(IN KVA)";
                    dt.Columns["TC_LIFE_SPAN"].ColumnName = "LIFE SPAN";
                    dt.Columns["MAKE NAME"].SetOrdinal(3);
                    List<string> listtoRemove = new List<string> { "TC_ID" };
                    string filename = "TCDetails" + DateTime.Now + ".xls";
                    string HeadTitle = "TC Details";

                    Genaral.getexcel(dt, listtoRemove, filename, HeadTitle);
                }
                else
                {
                    ShowMsgBox("No record found");
                    ShowEmptyGrid();
                }
                
            }
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Export_clickTCMaster");
            }



        }
    }
}