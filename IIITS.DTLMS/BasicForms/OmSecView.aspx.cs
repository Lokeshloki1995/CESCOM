using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Text.RegularExpressions;

namespace IIITS.DTLMS.BasicForms
{
    public partial class OmSecView : System.Web.UI.Page
    {
        string strFormCode = "OmSecView.aspx";
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
                    CheckAccessRights("4");
                    LoadOmSecDet();
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "Page_Load");
            }

        }

        public void LoadOmSecDet()
        {
            try
            {
                clsOmSecMast ObjOmSec = new clsOmSecMast();
                DataTable dt = new DataTable();
                dt = ObjOmSec.LoadOmSecvOffDet();
                grdOmSection.DataSource = dt;
                grdOmSection.DataBind();
                ViewState["OMSection"] = dt;
            }
            catch (Exception ex)

            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadOmSecDet");

            }
        }

        protected void grdOmSection_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdOmSection.PageIndex = e.NewPageIndex;
                clsOmSecMast ObjOmSec = new clsOmSecMast();
                DataTable dt = new DataTable();
                dt = ObjOmSec.LoadOmSecvOffDet();
                ViewState["OMSection"] = dt;
                grdOmSection.DataSource = SortDataTable(dt as DataTable, true);
                grdOmSection.DataBind();
                

               // LoadOmSecDet();
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "grdOmSection_PageIndexChanging");
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
                        ViewState["OMSection"] = dataView.ToTable();
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["OMSection"] = dataView.ToTable();

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

        protected void grdOmSection_Sorting(object sender, GridViewSortEventArgs e)
        {


            int columnIndex = 0;
            foreach (DataControlFieldHeaderCell headerCell in grdOmSection.HeaderRow.Cells)
            {
                if (headerCell.ContainingField.SortExpression == e.SortExpression)
                {
                    columnIndex = grdOmSection.HeaderRow.Cells.GetCellIndex(headerCell);
                }
            }

            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdOmSection.PageIndex;
            DataTable dt = (DataTable)ViewState["OMSection"];
            string sortingDirection = string.Empty;

            Image sortImage = new Image();
            Image sortImageboth = new Image();


            if (dt.Rows.Count > 0)
            {

                grdOmSection.DataSource = SortDataTable(dt as DataTable, false);
            }

            else
            {
                grdOmSection.DataSource = dt;
            }
            grdOmSection.DataBind();
            grdOmSection.PageIndex = pageIndex;
        }


        protected void imgBtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                //Check AccessRights
                bool bAccResult = CheckAccessRights("3");
                if (bAccResult == false)
                {
                    return;
                }

                ImageButton imgEdit = (ImageButton)sender;
                GridViewRow rw = (GridViewRow)imgEdit.NamingContainer;

                String strOmSecId = (((HiddenField)rw.FindControl("hfID")).Value.ToString());
                strOmSecId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(strOmSecId));
                Response.Redirect("OmSecMast.aspx?OmSecId=" + strOmSecId + "", false);
            }
            catch (Exception ex)
            {

                //.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "imgBtnEdit_Click");
            }
        }



        protected void imbBtnDelete_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton imgDel = (ImageButton)sender;
            GridViewRow rw = (GridViewRow)imgDel.NamingContainer;
        }


        protected void cmdNewOmSec_Click(object sender, EventArgs e)
        {
            try
            {
                //Check AccessRights
                bool bAccResult = CheckAccessRights("2");
                if (bAccResult == false)
                {
                    return;
                }

                Response.Redirect("OmSecMast.aspx", false);

            }
            catch (Exception ex)
            {
                 lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdNewOmSec_Click");
            }
        }

        protected void grdOmSection_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtSubDivName = (TextBox)row.FindControl("txtsubDivisionName");
                    TextBox txtOmName = (TextBox)row.FindControl("txtOmName");
                    TextBox txtOmCode = (TextBox)row.FindControl("txtOmCode");


                    DataTable dt = (DataTable)ViewState["OMSection"];
                    dv = dt.DefaultView;

                    if (txtSubDivName.Text != "")
                    {
                        sFilter = "SD_SUBDIV_NAME Like '%" + txtSubDivName.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txtOmName.Text != "")
                    {
                        sFilter += " OM_NAME Like '%" + txtOmName.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txtOmCode.Text != "")
                    {
                        sFilter += " OM_CODE Like '%" + txtOmCode.Text.Replace("'", "'") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdOmSection.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdOmSection.DataSource = dv;
                            ViewState["OMSection"] = dv.ToTable();
                            grdOmSection.DataBind();

                        }
                        else
                        {
                            ViewState["OMSection"] = dv.ToTable();

                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadOmSecDet();
                    }


                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "grdOmSection_RowCommand");
            }
        }

        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("OM_SLNO");
                dt.Columns.Add("SD_SUBDIV_NAME");
                dt.Columns.Add("OM_NAME");
                dt.Columns.Add("OM_CODE");
                dt.Columns.Add("OM_MOBILE_NO");
                dt.Columns.Add("OM_HEAD_EMP");
               

                grdOmSection.DataSource = dt;
                grdOmSection.DataBind();

                int iColCount = grdOmSection.Rows[0].Cells.Count;
                grdOmSection.Rows[0].Cells.Clear();
                grdOmSection.Rows[0].Cells.Add(new TableCell());
                grdOmSection.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdOmSection.Rows[0].Cells[0].Text = "No Records Found";

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

                objApproval.sFormName = "OmSecMast";
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

        protected void Export_clicksection(object sender, EventArgs e)
        {
            //clsOmSecMast ObjOmSec = new clsOmSecMast();
            //DataTable dt = new DataTable();
            //dt = ObjOmSec.LoadOmSecvOffDet();

            DataTable dt = (DataTable)ViewState["OMSection"];

            if (dt.Rows.Count > 0)
            {

                dt.Columns["SD_SUBDIV_NAME"].ColumnName = "SubDivision Name";
                dt.Columns["OM_NAME"].ColumnName = "OmSection Name";
                dt.Columns["OM_CODE"].ColumnName = "OmSection Code";
                dt.Columns["OM_MOBILE_NO"].ColumnName = "Mobile No";
                dt.Columns["OM_HEAD_EMP"].ColumnName = "Office Head";


                dt.Columns["SubDivision Name"].SetOrdinal(0);
                dt.Columns["OmSection Name"].SetOrdinal(1);
                dt.Columns["OmSection Code"].SetOrdinal(2);


                List<string> listtoRemove = new List<string> { "OM_SLNO", "OM_SUBDIV_CODE" };
                string filename = "SectionDetails" + DateTime.Now + ".xls";
                string pagetitle = "Section Details";

                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No record found");
                ShowEmptyGrid();
            }

        }
    }
}