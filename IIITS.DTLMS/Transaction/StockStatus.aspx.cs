using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IIITS.DTLMS.BL;

namespace IIITS.DTLMS.Transaction
{
    public partial class StockStatus : System.Web.UI.Page
    {
        string strFormCode = "StockStatus";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }

                if (!IsPostBack)
                {
                  
                    Genaral.Load_Combo("SELECT MD_NAME,MD_NAME from TBLMASTERDATA WHERE MD_TYPE='C' ORDER BY MD_ID", "-Select-", cmbCapacity);
                    Genaral.Load_Combo("SELECT SM_NAME,SM_NAME from TBLSTOREMAST ORDER BY SM_ID", "-Select-", cmdStore);
                    LoadStockDetails();

                }
               
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "Page_Load");
            }
             
        }

       

        public void LoadStockDetails(string sStoreName = "",string sLocation = "")
        {
           try
            {
                clsStockStatus ObjStore = new clsStockStatus();
                ObjStore.sStoreName = sStoreName;
                ObjStore.sStorelocation = sLocation;

                if (cmbCapacity.SelectedItem.Text.Trim() == "-Select-")
                {
                    ObjStore.sCapacity = string.Empty;

                }
                else
                {
                    ObjStore.sCapacity = cmbCapacity.SelectedItem.Text.Trim();
                }

                if (cmdStore.SelectedItem.Text.Trim() == "-Select-")
                {
                    ObjStore.sStoreName = string.Empty;

                }
                else
                {
                    ObjStore.sStoreName = cmdStore.SelectedItem.Text.Trim();
                }

                DataTable dtStoreDetails = new DataTable();
                dtStoreDetails = ObjStore.LoadStockGrid(ObjStore);

                if (dtStoreDetails.Rows.Count <= 0)
                {
                    DataTable dtFeederDetails = new DataTable();
                    DataRow newRow = dtFeederDetails.NewRow();
                    dtFeederDetails.Rows.Add(newRow);
                    dtFeederDetails.Columns.Add("SM_ID");
                    dtFeederDetails.Columns.Add("SM_NAME");
                   // dtFeederDetails.Columns.Add("SM_OFF_CODE");
                    dtFeederDetails.Columns.Add("TC_CAPACITY");
                    dtFeederDetails.Columns.Add("TC_CODE");

                    grdStockStatus.DataSource = dtFeederDetails;
                    grdStockStatus.DataBind();
                    int iColCount = grdStockStatus.Rows[0].Cells.Count;
                    grdStockStatus.Rows[0].Cells.Clear();
                    grdStockStatus.Rows[0].Cells.Add(new TableCell());
                    grdStockStatus.Rows[0].Cells[0].ColumnSpan = iColCount;
                    grdStockStatus.Rows[0].Cells[0].Text = "No Records Found";

                    ViewState["Stock"] = dtStoreDetails;

                }
                else
                {
                    dtStoreDetails.Rows.RemoveAt(dtStoreDetails.Rows.Count - 1);
                    grdStockStatus.DataSource = dtStoreDetails;
                    grdStockStatus.DataBind();
                    ViewState["Stock"] = dtStoreDetails;
                }
            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadStockDetails");
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
                        ViewState["Stock"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["Stock"] = dataView.ToTable();

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

        protected void grdStockStatus_Sorting(object sender, GridViewSortEventArgs e)
        {
         
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdStockStatus.PageIndex;
            DataTable dt = (DataTable)ViewState["Stock"];
            string sortingDirection = string.Empty;

            

            if (dt.Rows.Count > 0)
            {

                grdStockStatus.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdStockStatus.DataSource = dt;
            }
            grdStockStatus.DataBind();
            grdStockStatus.PageIndex = pageIndex;
        }

        //protected void grdStockStatus_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    try
        //    {

        //        if (e.CommandName == "search")
        //        {
        //            GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
        //            TextBox txtSearchName = (TextBox)row.FindControl("txtstoreName");
        //            TextBox txtLocation = (TextBox)row.FindControl("txtLocation");
        //            LoadStockDetails(txtSearchName.Text.Trim().Replace("'", ""), txtLocation.Text.Trim().Replace("'", ""));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdUser_RowCommand");
        //    }
        //}
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

        protected void Export_clickStockStatus(object sender, EventArgs e)
        {


            DataTable dt = (DataTable)ViewState["Stock"];

            if (dt.Rows.Count > 0)
            {


                dt.Columns["SM_NAME"].ColumnName = "Store Name";
                //dt.Columns["SM_OFF_CODE"].ColumnName = "Location";
                dt.Columns["TC_CAPACITY"].ColumnName = "Capacity(in KVA)";
                dt.Columns["TC_CODE"].ColumnName = "StockCount";

               // dt.Columns["SUPPLIER NAME"].SetOrdinal(3);
                List<string> listtoRemove = new List<string> { "SM_ID" };
                string filename = "Stock" + DateTime.Now + ".xls";
                string pagetitle = "Stock Status Details";

                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);


            }
            else
            {
                ShowMsgBox("No record found");

                DataTable dtFeederDetails = new DataTable();
                DataRow newRow = dtFeederDetails.NewRow();
                dtFeederDetails.Rows.Add(newRow);
                //dtFeederDetails.Columns.Add("SM_ID");
                dtFeederDetails.Columns.Add("SM_NAME");
               // dtFeederDetails.Columns.Add("SM_OFF_CODE");
                dtFeederDetails.Columns.Add("TC_CAPACITY");
                dtFeederDetails.Columns.Add("TC_CODE");

                grdStockStatus.DataSource = dtFeederDetails;
                grdStockStatus.DataBind();
                int iColCount = grdStockStatus.Rows[0].Cells.Count;
                grdStockStatus.Rows[0].Cells.Clear();
                grdStockStatus.Rows[0].Cells.Add(new TableCell());
                grdStockStatus.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdStockStatus.Rows[0].Cells[0].Text = "No Records Found";

            }
            


        }


        protected void grdStockStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdStockStatus.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Stock"];
                dt.Columns["SM_NAME"].AllowDBNull = true;
                dt.Columns["SM_OFF_CODE"].AllowDBNull = true;
                grdStockStatus.DataSource = SortDataTable(dt as DataTable, true);
                grdStockStatus.DataBind();

            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdStockStatus_PageIndexChanging");

            }
        }

        protected void grdStockStatus_DataBound1(object sender, EventArgs e)
        {
            for (int i = grdStockStatus.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = grdStockStatus.Rows[i];
                GridViewRow previousRow = grdStockStatus.Rows[i - 1];
                for (int j = 0; j < row.Cells.Count-1; j++)
                {
                    if (row.Cells[j].Text == previousRow.Cells[j].Text)
                    {
                        if (previousRow.Cells[j].RowSpan == 0)
                        {
                            if (row.Cells[j].RowSpan == 0)
                            {
                                previousRow.Cells[j].RowSpan += 2;
                            }
                            else
                            {
                                previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                            }
                            row.Cells[j].Visible = false;
                        }
                    }
                }
            }
        }

        protected void cmdStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStockDetails();
        }

        protected void cmbCapacity_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStockDetails();
        }
    }
}