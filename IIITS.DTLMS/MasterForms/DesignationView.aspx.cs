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
    public partial class DesignationView : System.Web.UI.Page
    {       
        string strFormCode = "DesignationView.aspx";
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
                    LoadDesignationGrid();
                   

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
                Response.Redirect("Designation.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdNew_Click");
            }
        }


        public void LoadDesignationGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                clsDesignation objDesgn = new clsDesignation();

                dt = objDesgn.LoadDetails();
                ViewState["Designationdetails"] = dt;
                grdDesignationDetails.DataSource = dt;
                grdDesignationDetails.DataBind();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadDesignationGrid");
            }
        }

        protected void grdDesignationDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdDesignationDetails.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Designationdetails"];

                grdDesignationDetails.DataSource = SortDataTable(dt as DataTable, true);
                grdDesignationDetails.DataBind();
               // LoadDesignationGrid();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "grdDesignationDetails_PageIndexChanging");
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

                        

                        dataView.Sort = string.Format("{0} {1}  ", GridViewSortExpression, GridViewSortDirection);
                        ViewState["Designationdetails"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["Designationdetails"] = dataView.ToTable();

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

        protected void grdDesignationDetails_Sorting(object sender, GridViewSortEventArgs e)
        {
            //int columnIndex = 0;
            //foreach (DataControlFieldHeaderCell headerCell in grdDesignationDetails.HeaderRow.Cells)
            //{
            //    if (headerCell.ContainingField.SortExpression == e.SortExpression)
            //    {
            //        columnIndex = grdDesignationDetails.HeaderRow.Cells.GetCellIndex(headerCell);
            //    }
            //}



            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdDesignationDetails.PageIndex;
            DataTable dt = (DataTable)ViewState["Designationdetails"];
            string sortingDirection = string.Empty;

            //Image sortImage = new Image();
            //Image sortImageboth = new Image();

            //if (direction == SortDirection.Ascending)
            //{
            //    direction = SortDirection.Descending;
            //    sortingDirection = "Desc";
            //    sortImage.ImageUrl = "img/sort_desc.png";
            //    sortImage.CssClass = "asc";
            //    sortImage.Visible = true;



            //}
            //else
            //{
            //    direction = SortDirection.Ascending;
            //    sortingDirection = "Asc";
            //    sortImage.ImageUrl = "img/sort_asc.png";
            //    sortImage.Visible = true;


            //}

            if (dt.Rows.Count > 0)
            {

                grdDesignationDetails.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdDesignationDetails.DataSource = dt;
            }
            grdDesignationDetails.DataBind();
            grdDesignationDetails.PageIndex = pageIndex;
        }

        protected void imgBtnEdit_Click(object sender, EventArgs e)
        {
            try
            {

                ImageButton imgEdit = (ImageButton)sender;
                GridViewRow rw = (GridViewRow)imgEdit.NamingContainer;
                String DesignationId = ((Label)rw.FindControl("lblId")).Text;
                DesignationId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(DesignationId));
                Response.Redirect("Designation.aspx?StrQryId=" + DesignationId + "", false);
            }

            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "imgBtnEdit_Click");

            }

        }

        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "Designation";
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

       

        public SortDirection direction
        {
            get
            {
                if (ViewState["directionState"] == null)
                {
                    ViewState["directionState"] = SortDirection.Ascending;
                }
                return (SortDirection)ViewState["directionState"];
            }
            set
            {
                ViewState["directionState"] = value;
            }
        }

        protected void Export_clickDest(object sender, EventArgs e)
        {
            //    DataTable dt1 = new DataTable();
            //    clsDesignation objDesgn = new clsDesignation();

            //    dt1 = objDesgn.LoadDetails();

            //    ViewState["Designationdetails"] = dt1;
            //    DataTable dt = (DataTable)ViewState["Designationdetails"];



            DataTable dt = (DataTable)ViewState["Designationdetails"];

            dt.Columns["DM_NAME"].ColumnName = "DESIGNATION ";
            dt.Columns["DM_DESC"].ColumnName = "DESIGNATION DESCRIPTION";

            List<string> listtoRemove = new List<string> { "DM_DESGN_ID" };
            string filename = "DesignationDetails" + DateTime.Now + ".xls";
             string pagetitle="Designation View";

             Genaral.getexcel(dt, listtoRemove, filename, pagetitle);

        }
    }
}