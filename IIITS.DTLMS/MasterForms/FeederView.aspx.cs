using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Drawing;
using System.Configuration;

namespace IIITS.DTLMS.MasterForms
{
    public partial class FeederView : System.Web.UI.Page
    {
        string sFormCode = "FeederView";
        clsSession objSession;
        string stroffCode = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                objSession = (clsSession)Session["clsSession"];
                lblErrormsg.Text = string.Empty;
                if (!IsPostBack)
                {
                    CheckAccessRights("4");
                    LoadFeederGrid(objSession.OfficeCode, "","");
                }
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, sFormCode, "Page_Load");
            }
        }

        public void LoadFeederGrid(string sOfficeCode,string strFeederName="",string strFeederCode="",string sStationName="",string sStationCode="")
        {
            try
            {

                DropDownList cmbCircle = (DropDownList)ReportFilterControl1.FindControl("cmbCircle");
                DropDownList cmbDiv = (DropDownList)ReportFilterControl1.FindControl("cmbDivision");
                DropDownList cmbSubdivision = (DropDownList)ReportFilterControl1.FindControl("cmbSubDiv");
                stroffCode = sOfficeCode;
                if (stroffCode.Length >= 1)
                {
                    Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME FROM TBLCIRCLE ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle);

                    stroffCode = sOfficeCode.Substring(0, 1);
                    cmbCircle.Items.FindByValue(stroffCode).Selected = true;
                    stroffCode = string.Empty;
                    stroffCode = sOfficeCode;
                    //cmbCircle.Items.FindByValue(stroffCode.Substring(0, 1)).Selected = true;
                    //stroffCode = string.Empty;
                    //stroffCode = sOfficeCode;
                }

                if (stroffCode.Length >= 1)
                {
                    Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION WHERE DIV_CICLE_CODE='" + cmbCircle.SelectedValue + "'", "--Select--", cmbDiv);
                    if (stroffCode.Length >= 2)
                    {
                        stroffCode = sOfficeCode.Substring(0, 2);
                        cmbCircle.Items.FindByValue(stroffCode.Substring(0, 1)).Selected = true;
                        cmbDiv.Items.FindByValue(stroffCode).Selected = true;
                        stroffCode = string.Empty;
                        stroffCode = sOfficeCode;
                    }
                }

                if (stroffCode.Length >= 2)
                {
                    Genaral.Load_Combo("SELECT SD_SUBDIV_CODE,SD_SUBDIV_NAME from TBLSUBDIVMAST WHERE SD_DIV_CODE='" + cmbDiv.SelectedValue + "'", "--Select--", cmbSubdivision);

                    if (stroffCode.Length >= 3)
                    {
                        stroffCode = sOfficeCode.Substring(0, 3);
                        cmbSubdivision.Items.FindByValue(stroffCode).Selected = true;
                        stroffCode = string.Empty;
                        stroffCode = sOfficeCode;
                    }
                }

            
                clsFeederView ObjFeeder = new clsFeederView();
               
                DataTable dt = new DataTable();
                if (sOfficeCode == "")
                {
                    sOfficeCode= (sOfficeCode.Length > 3) ? sOfficeCode.Substring(0,3) : sOfficeCode;
                }




                //if (stroffCode.Length >= 1)
                //{
                //    //Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME FROM TBLCIRCLE ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle);
                //   // .Items.FindByValue(stroffCode.Substring(0, 1)).Selected = true;
                //    stroffCode = string.Empty;
                //    stroffCode = obj.OfficeCode;
                //}

                //if (stroffCode.Length >= 1)
                //{
                //    Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION WHERE DIV_CICLE_CODE='" + cmbCircle.SelectedValue + "'", "--Select--", cmbDivision);
                //    if (stroffCode.Length >= 2)
                //    {
                //        stroffCode = objSession.OfficeCode.Substring(0, 2);
                //        cmbDivision.Items.FindByValue(stroffCode).Selected = true;
                //        stroffCode = string.Empty;
                //        stroffCode = objSession.OfficeCode;
                //    }
                //}

                dt = ObjFeeder.LoadFeederMastDet(sOfficeCode,strFeederName, strFeederCode,sStationName, sStationCode);

                if (dt.Rows.Count <= 0)
                {
                    DataTable dtFeederDetails = new DataTable();
                    DataRow newRow = dtFeederDetails.NewRow();
                    dtFeederDetails.Rows.Add(newRow);
                    dtFeederDetails.Columns.Add("FD_FEEDER_ID");
                    dtFeederDetails.Columns.Add("FD_FEEDER_NAME");
                    dtFeederDetails.Columns.Add("FD_FEEDER_CODE");
                    dtFeederDetails.Columns.Add("DIV_NAME");
                    dtFeederDetails.Columns.Add("OFF_NAME");

                    dtFeederDetails.Columns.Add("ST_NAME");
                    dtFeederDetails.Columns.Add("FD_TYPE");
                    dtFeederDetails.Columns.Add("ST_STATION_CODE");
                    grdFeeder.DataSource = dtFeederDetails;
                    grdFeeder.DataBind();

                    int iColCount = grdFeeder.Rows[0].Cells.Count;
                    grdFeeder.Rows[0].Cells.Clear();
                    grdFeeder.Rows[0].Cells.Add(new TableCell());
                    grdFeeder.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdFeeder.Rows[0].Cells[0].Text = "No Records Found";

                    ViewState["Feeder"] = dt;
                    

                }

                else
                {

                    grdFeeder.DataSource = dt;
                    grdFeeder.DataBind();
                    ViewState["Feeder"] = dt;
                }

              
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, sFormCode, "LoadFeederGrid");
            }

        }

        protected void grdFeeder_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdFeeder.PageIndex = e.NewPageIndex;
                // LoadFeederGrid(strSearchFeederName, strSearchFeederCode);
             
                DataTable dt = (DataTable)ViewState["Feeder"];
                dt.Columns["FD_FEEDER_NAME"].AllowDBNull = true;
                dt.Columns["FD_FEEDER_CODE"].AllowDBNull = true;
                dt.Columns["OFF_NAME"].AllowDBNull = true;
                grdFeeder.DataSource = SortDataTable(dt as DataTable, true);
                grdFeeder.DataBind();
              
               
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, sFormCode, "grdFeeder_PageIndexChanging");
            }
        }

        protected void grdFeeder_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdFeeder.PageIndex;
            DataTable dt = (DataTable)ViewState["Feeder"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {

                grdFeeder.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdFeeder.DataSource = dt;
            }
            grdFeeder.DataBind();
            grdFeeder.PageIndex = pageIndex;
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
                        ViewState["Feeder"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["Feeder"] = dataView.ToTable();

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

        protected void grdFeeder_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                    TextBox txtFeederName = (TextBox)row.FindControl("txtFeederName");
                    TextBox txtFeederCode = (TextBox)row.FindControl("txtFeederCode");
                    TextBox txtStation = (TextBox)row.FindControl("txtStation");
                    TextBox txtStationCode = (TextBox)row.FindControl("txtStationCode");

                    LoadFeederGrid("",txtFeederName.Text.Trim(), txtFeederCode.Text.Trim(),txtStation.Text.Trim(), txtStationCode.Text.Trim());
                }

                if (e.CommandName == "create")
                {

                    //Check AccessRights
                    bool bAccResult = CheckAccessRights("3");
                    if (bAccResult == false)
                    {
                        return;
                    }

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                    string sFeederId = ((Label)row.FindControl("lblFeederId")).Text;
                    sFeederId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sFeederId));
                    Response.Redirect("FeederMast.aspx?FeederId=" + sFeederId + "", false);


                }
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, sFormCode, "grdFeeder_RowCommand");
            }
        }

        protected void cmdLoad_Click(object sender, EventArgs e)
        {
            try
            {
              string sOfficeCode=  ReportFilterControl1.GetOfficeID();
              LoadFeederGrid(sOfficeCode);
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, sFormCode, "cmdLoad_Click");
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
                Response.Redirect("FeederMast.aspx", false);

            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, sFormCode, "cmdNew_Click");
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
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, sFormCode, "ShowMsgBox");
            }
        }

        protected void Export_clickFeeder(object sender, EventArgs e)
        {

            //clsFeederView ObjFeeder = new clsFeederView();
            //string sOfficeCode = ReportFilterControl1.GetOfficeID();
            //string strFeederName="";
            //string strFeederCode="";
            //string sStationName = "";
            //DataTable dt = new DataTable();
            //if (sOfficeCode == "")
            //{
            //    sOfficeCode = objSession.OfficeCode;
            //}
            //else
            //{
            //     sOfficeCode = ReportFilterControl1.GetOfficeID();
            //}
            //dt = ObjFeeder.LoadFeederMastDet(sOfficeCode, strFeederName, strFeederCode, sStationName);


            DataTable dt = (DataTable)ViewState["Feeder"];


            if (dt.Rows.Count > 0)
            {

                dt.Columns["ST_NAME"].ColumnName = "STATION NAME";
                dt.Columns["FD_FEEDER_NAME"].ColumnName = "FEEDER NAME";
                dt.Columns["FD_FEEDER_CODE"].ColumnName = "FEEDER CODE";
                dt.Columns["OFF_NAME"].ColumnName = "SUB-DIVISION NAME";
                dt.Columns["FD_TYPE"].ColumnName = "FEEDER TYPE";
                dt.Columns["ST_STATION_CODE"].ColumnName = "STATION CODE";

                dt.Columns["STATION CODE"].SetOrdinal(0);
                dt.Columns["STATION NAME"].SetOrdinal(1);

                List<string> listtoRemove = new List<string> { "FD_FEEDER_ID" };
                string filename = "FeederDetails" + DateTime.Now + ".xls";
                string pagetitle = "Feeder Details";

                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No record found");


                DataTable dtFeederDetails = new DataTable();
                DataRow newRow = dtFeederDetails.NewRow();
                dtFeederDetails.Rows.Add(newRow);
                dtFeederDetails.Columns.Add("FD_FEEDER_ID");
                dtFeederDetails.Columns.Add("FD_FEEDER_NAME");
                dtFeederDetails.Columns.Add("FD_FEEDER_CODE");
                dtFeederDetails.Columns.Add("OFF_NAME");
                dtFeederDetails.Columns.Add("ST_NAME");
                dtFeederDetails.Columns.Add("FD_TYPE");

                grdFeeder.DataSource = dtFeederDetails;
                grdFeeder.DataBind();

                int iColCount = grdFeeder.Rows[0].Cells.Count;
                grdFeeder.Rows[0].Cells.Clear();
                grdFeeder.Rows[0].Cells.Add(new TableCell());
                grdFeeder.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdFeeder.Rows[0].Cells[0].Text = "No Records Found";
            }



        }

        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "FeederMast";
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
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, sFormCode, "CheckAccessRights");
                return false;

            }
        }

        #endregion

    }
}