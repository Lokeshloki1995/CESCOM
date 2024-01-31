using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IIITS.DTLMS.BL;

namespace IIITS.DTLMS.MasterForms
{
    public partial class AgencyMasterView : System.Web.UI.Page
    {
        string strFormCode = "AgencyMaster";
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
                    LoadAgencyMasterRepairer();
                    CheckAccessRights("4");
                }
            }
            catch (Exception ex)
            {

                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load");
            }
        }

        public void LoadAgencyMasterRepairer()
        {
            clsAgency objAgency = new clsAgency();
            objAgency.sRepairOffCode = "";
            DataTable dt = objAgency.LoadAgencyRepairerMasterDetails(objAgency);
            if (dt.Rows.Count > 0)
            {
                grdRepairer.DataSource = dt;
                grdRepairer.DataBind();
                ViewState["RepairerMaster"] = dt;
                //updatepanel.update();
                //modalPopup.show();
            }


        }


        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "AgencyCreate";
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
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "CheckAccessRights");
                return false;
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
                //lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowMsgBox");
            }
        }

    
        protected void cmdNew_Click(object sender, EventArgs e)
        {
            try
            {
                ///Check AccessRights
                bool bAccResult = CheckAccessRights("2");
                if (bAccResult == false)
                {
                    return;
                }
                Response.Redirect("AgencyMaster.aspx", false);
            }

            catch (Exception ex)
            {
                //lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdNew_Click");
            }
        }

        protected void grdAgency_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdRepairer.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["RepairerMaster"];
                grdRepairer.DataSource = SortDataTable(dt as DataTable, true);
                grdRepairer.DataBind();
            }
            catch (Exception ex)
            {
                //lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdAgency_PageIndexChanging");
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
                        ViewState["RepairerMaster"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["RepairerMaster"] = dataView.ToTable();
                    }
                }
                return dataView;
            }
            else
            {
                return new DataView();
            }

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
        protected void grdRepairer_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
               

                if (e.CommandName == "upload")
                {
                    //Check AccessRights
                    bool bAccResult = CheckAccessRights("3");
                    if (bAccResult == false)
                    {
                        return;
                    }

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                    //string sAgencyId = ((Label)row.FindControl("lblRepairId")).Text;
                    //sAgencyId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sAgencyId));
                    //Response.Redirect("AgencyMaster.aspx?StrQryId=" + sAgencyId + "", false);
                    string MappingId = ((Label)row.FindControl("lblMappingid")).Text;
                    MappingId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(MappingId));
                    Response.Redirect("AgencyMaster.aspx?StrQryId=" + MappingId + "", false);
                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtRepairerName = (TextBox)row.FindControl("txtRepairerName");


                    DataTable dt = (DataTable)ViewState["RepairerMaster"];
                    dv = dt.DefaultView;

                    if (txtRepairerName.Text != "")
                    {
                        sFilter = "RA_NAME Like '%" + txtRepairerName.Text.Replace("'", "") + "%' AND";
                    }

                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdRepairer.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdRepairer.DataSource = dv;
                            ViewState["RepairerMaster"] = dv.ToTable();
                            grdRepairer.DataBind();

                        }
                        else
                        {
                            ViewState["RepairerMaster"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadAgencyMasterRepairer();
                    }

                }

               

                if (e.CommandName == "status")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                    string sAgencyId = ((Label)row.FindControl("lblRepairId")).Text;
                    string sMappingid = ((Label)row.FindControl("lblMappingid")).Text;
                    clsAgency objAgency = new clsAgency();
                    objAgency.AgencyId = sAgencyId;
                    objAgency.sMappingId = sMappingid;
                    string status = ((Label)row.FindControl("lblStatus")).Text;

                    if (status != "" && status == "A")
                    {
                        objAgency.sMasterStatus = "D";
                        bool updatedStatus = objAgency.ActiveDeactiveMasterAgency(objAgency);
                        if (updatedStatus)
                        {
                            ShowMsgBox("Agency Deactivated Successfully");
                        }
                        else
                        {
                            ShowMsgBox("Something Went Wrong");
                        }
                        LoadAgencyMasterRepairer();

                    }
                    if (status != "" && status == "D")
                    {
                        objAgency.sMasterStatus = "A";
                        bool updatedStatus = objAgency.ActiveDeactiveMasterAgency(objAgency);
                        if (updatedStatus)
                        {
                            ShowMsgBox("Agency Activated Successfully");
                        }
                        else
                        {
                            ShowMsgBox("Something Went Wrong");
                        }
                        LoadAgencyMasterRepairer();
                    }

                }
            }
            catch (Exception ex)
            {
                //lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdRepairer_RowCommand");
            }
        }

        protected void grdRepairer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblStatus;
                    lblStatus = (Label)e.Row.FindControl("lblStatus");
                    ImageButton imgBtnEdit;
                    //  imgBtnEdit = (ImageButton)e.Row.FindControl("imgBtnEdit");

                    if (lblStatus.Text == "A")
                    {
                        ImageButton imgActive;
                        imgActive = (ImageButton)e.Row.FindControl("imgActive");
                        imgActive.Visible = true;
                        //   imgBtnEdit.Enabled = true;
                        // imgBtnEdit.ToolTip = "";
                    }
                    else
                    {
                        ImageButton imgDeActive;
                        imgDeActive = (ImageButton)e.Row.FindControl("imgDeActive");
                        imgDeActive.Visible = true;
                        //imgBtnEdit.Enabled = false;
                        //imgBtnEdit.ToolTip = "Repairer is DeActivated,You cannot Edit";
                    }
                }

            }
            catch (Exception ex)
            {
                //lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdRepairer_RowDataBound");


            }
        }


        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("RA_ID");
                dt.Columns.Add("RA_NAME");
                dt.Columns.Add("RA_ADDRESS");
                dt.Columns.Add("RA_PHNO");
                dt.Columns.Add("RA_MAIL");
                dt.Columns.Add("RA_STATUS");


                grdRepairer.DataSource = dt;
                grdRepairer.DataBind();

                int iColCount = grdRepairer.Rows[0].Cells.Count;
                grdRepairer.Rows[0].Cells.Clear();
                grdRepairer.Rows[0].Cells.Add(new TableCell());
                grdRepairer.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdRepairer.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                //lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowEmptyGrid");

            }
        }


        protected void grdRepairer_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdRepairer.PageIndex;
            DataTable dt = (DataTable)ViewState["RepairerMaster"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {
                grdRepairer.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdRepairer.DataSource = dt;
            }
            grdRepairer.DataBind();
            grdRepairer.PageIndex = pageIndex;
        }
    }
}