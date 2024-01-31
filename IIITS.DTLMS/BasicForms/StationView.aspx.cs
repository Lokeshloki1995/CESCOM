using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;

namespace IIITS.DTLMS.BasicForms
{
    public partial class StationView : System.Web.UI.Page
    {
        static string strStationId = "0";
        string sFormCode = "StationView";
        string stroffCode = string.Empty;
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
                    Genaral.Load_Combo("SELECT \"CM_CIRCLE_CODE\",\"CM_CIRCLE_NAME\" FROM \"TBLCIRCLE\" ORDER BY \"CM_CIRCLE_CODE\" ", "--Select--", cmbCircle);
                    CheckAccessRights("4");
                    LoadStation();
                }
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, sFormCode, "Page_Load");
            }
        }

        public void LoadStation()
        {
            try
            {
                clsStation objStation = new clsStation();
                DataTable dt = new DataTable();
                string sLocation = string.Empty;

               clsSession obj = (clsSession) Session["clsSession"];
                stroffCode = obj.OfficeCode;

                if (stroffCode.Length >= 1)
                {
                    //Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME FROM TBLCIRCLE ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle);
                    cmbCircle.Items.FindByValue(stroffCode.Substring(0,1)).Selected = true;
                    stroffCode = string.Empty;
                    stroffCode = obj.OfficeCode;
                }

                if (stroffCode.Length >= 1)
                {
                    Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION WHERE DIV_CICLE_CODE='" + cmbCircle.SelectedValue + "'", "--Select--", cmbDivision);
                    if (stroffCode.Length >= 2)
                    {
                        stroffCode = objSession.OfficeCode.Substring(0, 2);
                        cmbDivision.Items.FindByValue(stroffCode).Selected = true;
                        stroffCode = string.Empty;
                        stroffCode = objSession.OfficeCode;
                    }
                }
                
                //cmbDivision.SelectedValue =  ((obj.OfficeCode.Length > 2) || (obj.OfficeCode.Length == 2)) ?  ( obj.OfficeCode.Substring(0, 1)) : ( obj.OfficeCode.Substring(0, 1) );
                //if ((obj.OfficeCode.Length) >  2 || (obj.OfficeCode.Length == 2) )
                //{
                //    cmbDivision.Items.FindByValue(obj.OfficeCode.Substring(0, 2)).Selected = true;
                //   // cmbDivision.SelectedValue = obj.OfficeCode.Substring(0,2)  ;
                //}
                if (cmbDivision.SelectedIndex > 0)
                {
                    sLocation = cmbDivision.SelectedValue;
                }
                else if (cmbCircle.SelectedIndex > 0)
                {
                    sLocation = cmbCircle.SelectedValue;
                }

                dt = objStation.LoadStationDet("", sLocation);
              //  dt = objStation.LoadStationDet();
               // dt = objStation.LoadStationDet("", sLocation);
                grdStation.DataSource = dt;
                grdStation.DataBind();
                ViewState["Station"] = dt;
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, sFormCode, "LoadStation");
            }
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

                strStationId = (((HiddenField)rw.FindControl("hfID")).Value.ToString());
                strStationId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(strStationId));
                Response.Redirect("Station.aspx?StationId=" + strStationId + "", false);

            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, sFormCode, "imgBtnEdit_Click");
            }

        }

        /// <summary>
        /// protected void imbBtnDelete_Click(object sender, ImageClickEventArgs e)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbBtnDelete_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton imgDel = (ImageButton)sender;
            GridViewRow rw = (GridViewRow)imgDel.NamingContainer;


        }
        protected void cmbCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCircle.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION WHERE DIV_CICLE_CODE ='" + cmbCircle.SelectedValue + "'", "--Select--", cmbDivision);
                   
                }

                else
                {
                    cmbDivision.Items.Clear();
                   

                }
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, sFormCode, "cmbCircle_SelectedIndexChanged");
            }
        }


        protected void cmdLoad_Click(object sender, EventArgs e)
        {
            try
            {
                LoadStation();
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, sFormCode, "cmdLoad_Click");
            }
        }

        protected void grdStation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdStation.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Station"];
                grdStation.DataSource = SortDataTable(dt as DataTable, true);
                grdStation.DataBind();
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, sFormCode, "grdStation_PageIndexChanging");
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
                        
                            ViewState["Station"] = dataView.ToTable();
                        

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        
                            ViewState["Station"] = dataView.ToTable();
                        

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

        protected void grdStation_Sorting(object sender, GridViewSortEventArgs e)
        {

            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdStation.PageIndex;
            DataTable dt = (DataTable)ViewState["Station"];
            string sortingDirection = string.Empty;

            if (dt.Rows.Count > 0)
            {
                grdStation.DataSource = SortDataTable(dt as DataTable, false);
            }

            else
            {
                grdStation.DataSource = dt;
            }

            grdStation.DataBind();
            grdStation.PageIndex = pageIndex;
            



        }

        protected void grdStation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
               //LoadStation();
                if (e.CommandName == "search")
                {
                    GridViewRow row = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;

                    TextBox txtStationCode = (TextBox)row.FindControl("txtStationCode");
                    TextBox txtStationName = (TextBox)row.FindControl("txtStationName");
                   

                    DataTable dt = (DataTable)ViewState["Station"];
                    DataView dv = new DataView();
                    dv.Table = dt;
                    string sFilter = string.Empty;
                    if (txtStationCode.Text != "")
                    {
                        sFilter = " ST_STATION_CODE like '" + txtStationCode.Text.Trim().ToUpper() + "%' AND";
                    }
                    if (txtStationName.Text != "")
                    {
                        sFilter = " ST_NAME like '" + txtStationName.Text.Trim().ToUpper() + "%' AND";
                    }

                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdStation.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdStation.DataSource = dv;
                            ViewState["Station"] = dv.ToTable();
                            grdStation.DataBind();

                        }
                        else
                        {
                            ViewState["Station"] = dv.ToTable();

                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                       // grdStation.DataSource = dv;
                        //grdStation.DataBind();
                        LoadStation();
                    }


                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, sFormCode, "grdStation_RowCommand");
            }
        }


        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("ST_ID");
                dt.Columns.Add("ST_NAME");
                dt.Columns.Add("ST_STATION_CODE");
                dt.Columns.Add("STC_CAP_VALUE");
                dt.Columns.Add("OFFNAME");
                dt.Columns.Add("ST_DESCRIPTION");
                dt.Columns.Add("CIRCLE");
                dt.Columns.Add("DIVISION");
                dt.Columns.Add("DT_NAME");
                dt.Columns.Add("TQ_NAME");
                grdStation.DataSource = dt;
                grdStation.DataBind();

                int iColCount = grdStation.Rows[0].Cells.Count;
                grdStation.Rows[0].Cells.Clear();
                grdStation.Rows[0].Cells.Add(new TableCell());
                grdStation.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdStation.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {

                clsException.LogError(ex.StackTrace, ex.Message, sFormCode, "ShowEmptyGrid");

            }
        }

        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "Station";
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

        protected void cmdNewStation_Click(object sender, EventArgs e)
        {
            try
            {
                //Check AccessRights
                bool bAccResult = CheckAccessRights("2");
                if (bAccResult == false)
                {
                    return;
                }
                Response.Redirect("Station.aspx", false);

            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, sFormCode, "cmdNewStation_Click");
            }
        }

        protected void Export_clickStation(object sender, EventArgs e)
        {
            //clsStation objStation = new clsStation();
            //DataTable dt = new DataTable();
            //dt = objStation.LoadStationDet();

            DataTable dt = (DataTable)ViewState["Station"];


            if (dt.Rows.Count > 0)
            {
                dt.Columns["CIRCLE"].ColumnName = "Circle name";
                dt.Columns["DIVISION"].ColumnName = "Divivsion";
                dt.Columns["ST_STATION_CODE"].ColumnName = "Station code";

                dt.Columns["ST_NAME"].ColumnName = "Station name";
                dt.Columns["STC_CAP_VALUE"].ColumnName = "Voltage Class";
                dt.Columns["ST_DESCRIPTION"].ColumnName = "Description";

                List<string> listtoRemove = new List<string> { "ST_ID", "ST_OFF_CODE", "ST_PARENT_STATID" };
                string filename = "StationDetails" + DateTime.Now + ".xls";
                string pagetitle = "Station Details";

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