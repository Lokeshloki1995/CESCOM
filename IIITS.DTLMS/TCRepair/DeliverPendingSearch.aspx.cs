using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using ClosedXML.Excel;
using System.IO;

namespace IIITS.DTLMS.TCRepair
{
    public partial class DeliverPendingSearch : System.Web.UI.Page
    {
        string strFormCode = "DeliverPendingSearch";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["UserId"] == null || Request.QueryString["UserId"].ToString() == "")
                {
                    if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                    {
                        Response.Redirect("~/Login.aspx", false);
                    }
                }

                if (Request.QueryString["UserId"] != null && Request.QueryString["UserId"].ToString() != "")
                {
                    string PId = Genaral.Decrypt(Request.QueryString["UserId"].ToString());
                    if (PId != null)
                    {
                        clsLogin objLogin = new clsLogin();
                        objSession = new clsSession();
                        objLogin.sUserId = PId;
                        objLogin.MMSUserLogin(objLogin);

                        if (objLogin.sMessage == null)
                        {

                            Session["FullName"] = objLogin.sFullName;
                            Session["ChangPwd"] = objLogin.sChangePwd;

                            if (objLogin.sOfficeCode == "0")
                            {
                                objLogin.sOfficeCode = "";
                            }

                            objSession.UserId = objLogin.sUserId;
                            objSession.FullName = objLogin.sFullName;
                            objSession.RoleId = objLogin.sRoleId;
                            objSession.OfficeCode = objLogin.sOfficeCode;
                            objSession.OfficeName = objLogin.sOfficeName;
                            objSession.Designation = objLogin.sDesignation;
                            objSession.OfficeNameWithType = objLogin.sOfficeNamewithType;
                            Session["ProjectType"] = "MMS";
                            Session["clsSession"] = objSession;
                            if (Session["ChangPwd"] == null || Session["ChangPwd"].ToString() == "")
                            {
                                Response.Redirect("~/MasterForms/ChangePassword.aspx", false);
                            }
                            //else
                            //{
                            //    Response.Redirect("Dashboard.aspx", false);
                            //}

                            //HttpUtility.UrlEncode(General.Encrypt(e.Item.Cells(0).Text))
                            //General.Decrypt(HttpUtility.UrlDecode(Request.QueryString("EmpId")))
                        }
                        //else
                        //{
                        //    lblMsg.Text = objLogin.sMessage;
                        //}

                    }
                }

                lblMessage.Text = string.Empty;
                objSession = (clsSession)Session["clsSession"];
                if (!IsPostBack)
                {
                    if (CheckAccessRights("4"))
                    {
                        Genaral.Load_Combo("SELECT  TR_ID, TR_NAME  FROM TBLTRANSREPAIRER WHERE TR_STATUS='A' AND TR_ID NOT IN (SELECT TR_ID FROM TBLTRANSREPAIRER WHERE TR_BLACK_LISTED=1 AND TR_BLACKED_UPTO>=SYSDATE) AND  TR_OFFICECODE LIKE '" + objSession.OfficeCode + "%' ORDER BY TR_NAME", "--SELECT--", cmbRepairer);

                        Genaral.Load_Combo("SELECT  TS_ID, TS_NAME  FROM TBLTRANSSUPPLIER  WHERE TS_STATUS='A' AND TS_ID NOT IN (SELECT TS_ID FROM TBLTRANSSUPPLIER WHERE TS_BLACK_LISTED=1 AND TS_BLACKED_UPTO>=SYSDATE) ORDER BY TS_NAME", "--SELECT--", cmbSupplier);

                        string strQry = string.Empty;

                        LoadTestingPassedDetails();
                        LoadTestingPendingDetails();

                        strQry = "Title=Search and Select Purchase Order Details&";
                        strQry += "Query=SELECT DISTINCT RSM_PO_NO,RSM_INV_NO FROM TBLREPAIRSENTMASTER,TBLREPAIRSENTDETAILS,TBLINSPECTIONDETAILS WHERE RSM_DIV_CODE LIKE '" + objSession.OfficeCode + "%' AND RSM_ID=RSD_RSM_ID ";
                        strQry += " AND RSD_DELIVARY_DATE IS NULL AND IND_RSD_ID=RSD_ID and {0} like %{1}% &";
                        strQry += "DBColName=RSM_PO_NO~RSM_INV_NO&";
                        strQry += "ColDisplayName=PO No~Invoice No&";

                        strQry = strQry.Replace("'", @"\'");

                        cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtWoNo.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + txtWoNo.ClientID + ")");
                    }

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load");
            }
        }


        private void LoadTestingPassedDetails()
        {
            try
            {
                clsDTrRepairActivity objDeliverPending = new clsDTrRepairActivity();

                if (cmbRepairer.SelectedIndex > 0)
                {
                    objDeliverPending.sRepairerId = cmbRepairer.SelectedValue;
                }
                if (cmbSupplier.SelectedIndex > 0)
                {
                    objDeliverPending.sSupplierId = cmbSupplier.SelectedValue;
                }

                objDeliverPending.sPurchaseOrderNo = txtWoNo.Text.Trim();
                if (objDeliverPending.sPurchaseOrderNo == "")
                {
                    grdTestingPass.Columns[8].Visible = false;
                }
                else
                {
                    grdTestingPass.Columns[8].Visible = true;
                }

                objDeliverPending.sOfficeCode = objSession.OfficeCode;
                objDeliverPending.sTestingDone = "1";

                DataTable dt = new DataTable();
                dt = objDeliverPending.LoadTestingPassedDetails(objDeliverPending);

                grdTestingPass.DataSource = dt;
                grdTestingPass.DataBind();
                ViewState["TestPass"] = dt;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadTestingPassedDetails");
            }
        }

        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "ReceivePending";
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

        private void LoadTestingPendingDetails()
        {
            try
            {
                clsDTrRepairActivity objDeliverPending = new clsDTrRepairActivity();

                if (cmbRepairer.SelectedIndex > 0)
                {
                    objDeliverPending.sRepairerId = cmbRepairer.SelectedValue;
                }
                if (cmbSupplier.SelectedIndex > 0)
                {
                    objDeliverPending.sSupplierId = cmbSupplier.SelectedValue;
                }

                objDeliverPending.sPurchaseOrderNo = txtWoNo.Text.Trim();

                objDeliverPending.sOfficeCode = objSession.OfficeCode;
                objDeliverPending.sTestingDone = "1";

                DataTable dt = new DataTable();
                dt = objDeliverPending.LoadPendingForTestingDetails(objDeliverPending);

                grdTestPending.DataSource = dt;
                grdTestPending.DataBind();
                ViewState["TestPending"] = dt;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadTestingPendingDetails");
            }
        }
        public DataSet getDataSetExportToExcel()
        {
            DataSet ds = new DataSet();
            try
            {

                clsDTrRepairActivity objDeliverPending = new clsDTrRepairActivity();

                if (cmbRepairer.SelectedIndex > 0)
                {
                    objDeliverPending.sRepairerId = cmbRepairer.SelectedValue;
                }
                if (cmbSupplier.SelectedIndex > 0)
                {
                    objDeliverPending.sSupplierId = cmbSupplier.SelectedValue;
                }

                objDeliverPending.sPurchaseOrderNo = txtWoNo.Text.Trim();

                objDeliverPending.sOfficeCode = objSession.OfficeCode;
                objDeliverPending.sTestingDone = "1";

                DataTable dttesting = new DataTable();
                dttesting = objDeliverPending.LoadPendingForTestingDetails(objDeliverPending);



                if (cmbRepairer.SelectedIndex > 0)
                {
                    objDeliverPending.sRepairerId = cmbRepairer.SelectedValue;
                }
                if (cmbSupplier.SelectedIndex > 0)
                {
                    objDeliverPending.sSupplierId = cmbSupplier.SelectedValue;
                }

                objDeliverPending.sPurchaseOrderNo = txtWoNo.Text.Trim();


                objDeliverPending.sOfficeCode = objSession.OfficeCode;
                objDeliverPending.sTestingDone = "1";

                DataTable dttestingpassed = new DataTable();
                dttestingpassed = objDeliverPending.LoadTestingPassedDetails(objDeliverPending);

                DataTable dt = dttesting.Copy();
                DataTable dt1 = dttestingpassed.Copy();
                dt.TableName = "PendingForTesting";
                dt1.TableName = "TestingPassedDetails";

                ds.Tables.Add(dt);

                ds.Tables.Add(dt1);


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Export_ClickTcsearch");
            }
            return ds;


        }

        protected void Export_ClickPendingTesting(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            if (ViewState["TestPending"] != null)
            {
                dt = (DataTable)ViewState["TestPending"];
            }
            else
            {
                clsDTrRepairActivity objDeliverPending = new clsDTrRepairActivity();

                objDeliverPending.sOfficeCode = objSession.OfficeCode;
                objDeliverPending.sTestingDone = "1";
                objDeliverPending.sPurchaseOrderNo = "";
                dt = objDeliverPending.LoadPendingForTestingDetails(objDeliverPending);
            }
            if (dt.Rows.Count > 0)
            {

                dt.Columns["RSM_PO_NO"].ColumnName = "PO No";
                dt.Columns["RSM_DIV_CODE"].ColumnName = "Division Code";
                dt.Columns["PODATE"].ColumnName = "PO Date";
                dt.Columns["ISSUEDATE"].ColumnName = "Issue Date";
                dt.Columns["SUP_REPNAME"].ColumnName = "Supplier/Repairer";
                dt.Columns["PO_QUANTITY"].ColumnName = "Total Quantity";
                dt.Columns["PENDING_QNTY"].ColumnName = "Pending Qty With HT";
                dt.Columns["PENDING_QTY_EE"].ColumnName = "Pending Quantity with EE";

                dt.Columns["PO No"].SetOrdinal(0);
                dt.Columns["Division Code"].SetOrdinal(1);
                dt.Columns["PO Date"].SetOrdinal(2);
                dt.Columns["Issue Date"].SetOrdinal(3);
                dt.Columns["Supplier/Repairer"].SetOrdinal(4);
                dt.Columns["Total Quantity"].SetOrdinal(5);
                dt.Columns["Pending Qty for Testing"].SetOrdinal(6);
                dt.Columns["Pending Quantity with EE"].SetOrdinal(7);

                List<string> listtoRemove = new List<string> { "DELIVERED_QNTY" };
               
                string filename = "PendingForTestingDetails" + DateTime.Now + ".xls";
                string pagetitle = "Pending For Testing Details";

                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No record found");
            }
        }

        protected void Export_ClickDeliverPendingSearch(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["TestPass"];

            if (dt.Rows.Count > 0)
            {

                dt.Columns["RSM_PO_NO"].ColumnName = "PO No";
                dt.Columns["PODATE"].ColumnName = "PO Date";
                dt.Columns["ISSUEDATE"].ColumnName = "Issue Date";
                dt.Columns["SUP_REPNAME"].ColumnName = "Supplier/Repairer";
                dt.Columns["PO_QUANTITY"].ColumnName = "Total Quantity";
                dt.Columns["PENDING_QNTY"].ColumnName = "Pending Qty for Recieve";
                dt.Columns["DELIVERED_QNTY"].ColumnName = "Recieved Quantity";
                dt.Columns["REPAIR_SENT_OIL"].ColumnName = "Repairer Sent Oil";


                // dttestingpassed.Columns["MAKE NAME"].SetOrdinal(3);
                List<string> listtoRemove = new List<string> { "RSM_ID" };
                if (txtWoNo.Text == "")
                {
                    listtoRemove.Add("REPAIR_SENT_OIL");
                }
                string filename = "TestingPassedDetails" + DateTime.Now + ".xls";
                string pagetitle = "Testing Passed Details";


                Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No record found");
            }


        }
        //protected void Export_ClickDeliverPendingSearch(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        DataSet ds = getDataSetExportToExcel();

        //        if (ds.Tables[0].Rows.Count > 0 || ds.Tables[1].Rows.Count > 0)
        //        {

        //            //foreach (DataTable dt in ds.Tables)
        //            //{
        //                DataTable dt = new DataTable();
        //                DataRow newRow = dt.NewRow();
        //                dt.Rows.Add(newRow);

        //                dt.Columns.Add("RSM_PO_NO");
        //                dt.Columns.Add("PODATE");
        //                dt.Columns.Add("ISSUEDATE");
        //                dt.Columns.Add("SUP_REPNAME");
        //                dt.Columns.Add("PO_QUANTITY");
        //                dt.Columns.Add("PENDING_QNTY");
        //                dt.Columns.Add("DELIVERED_QNTY");
        //            //}



        //        }




        //        if (ds.Tables[0].Rows.Count > 0 || ds.Tables[1].Rows.Count > 0)
        //        {
        //            using (XLWorkbook wb = new XLWorkbook())
        //            {
        //                 //DataTable dt = new DataTable();

        //                //int Count = 1;
        //                 foreach (DataTable dt in ds.Tables)
        //                {

        //                   // dt.Columns.Remove("RSM_ID");

        //                    wb.Worksheets.Add(dt);

        //                    //wb.Worksheet(1).Row(1).InsertRowsAbove(2);


        //                }

        //                 HttpContext.Current.Response.Clear();
        //                 HttpContext.Current.Response.Buffer = true;
        //                 HttpContext.Current.Response.Charset = "";
        //                 string FileName = "DeliverPendingSearch" + DateTime.Now + ".xls";
        //                 HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //                 HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + FileName);

        //                 using (MemoryStream MyMemoryStream = new MemoryStream())
        //                 {
        //                     wb.SaveAs(MyMemoryStream);
        //                     MyMemoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
        //                     HttpContext.Current.Response.Flush();
        //                     HttpContext.Current.Response.End();
        //                 }

        //                //Export the Excel file.

        //            }
        //        }

        //        else
        //        {
        //            ShowMsgBox("no records found");
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}




        //protected void Export_ClickDeliverPendingSearch(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        DataSet ds = getDataSetExportToExcel();

        //        if (ds.Tables[0].Rows.Count > 0 || ds.Tables[1].Rows.Count > 0)
        //        {
        //            string filename = "DeliverPending";
        //            Genaral.getexcelds(ds, filename);
        //        }
        //        else
        //        {
        //            ShowMsgBox("No record found");
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Export_ClickTcsearch");
        //    }


        //}

        protected void imgBtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                ImageButton imgEdit = (ImageButton)sender;
                GridViewRow rw = (GridViewRow)imgEdit.NamingContainer;

                String strTCcode = ((Label)rw.FindControl("lblTcCode")).Text;
                strTCcode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(strTCcode));
                Response.Redirect("DeliverTC.aspx?QryTccode=" + strTCcode + "");
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "imgBtnEdit_Click");
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

        protected void cmdReset_Click(object sender, EventArgs e)
        {

            txtWoNo.Text = string.Empty;
            cmbRepairer.SelectedIndex = 0;

            //grdTestingPass.DataSource = null;
            //grdTestingPass.DataBind();
            cmbSupplier.SelectedIndex = 0;
        }

        protected void cmdLoad_Click(object sender, EventArgs e)
        {
            try
            {
                LoadTestingPassedDetails();
                LoadTestingPendingDetails();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdLoad_Click");
            }
        }


        protected void grdTestingPass_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Recieve")
                {

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    Label lblRepairMasterId = (Label)row.FindControl("lblRepairMasterId");
                    Label lblInsResultId = (Label)row.FindControl("lblInsResult");


                    string sRepairMasterId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblRepairMasterId.Text));
                    string sInsResultId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblRepairMasterId.Text));
                    Response.Redirect("DeliverTC.aspx?InsResult=" + sInsResultId + "&RepairMasterId=" + sRepairMasterId, false);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdTestingPass_RowCommand");
            }
        }

        private void PopulateCheckedValues()
        {
            try
            {
                ArrayList arrCheckedValues = (ArrayList)ViewState["CHECKED_ITEMS"];
                if (arrCheckedValues != null && arrCheckedValues.Count > 0)
                {
                    foreach (GridViewRow gvrow in grdTestPending.Rows)
                    {
                        int index = Convert.ToInt32(grdTestPending.DataKeys[gvrow.RowIndex].Values[0]);
                        if (arrCheckedValues.Contains(index))
                        {
                            CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkSelect");
                            myCheckBox.Checked = true;
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "PopulateCheckedValues");

            }
        }



        protected void grdTestPending_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdTestPending.PageIndex = e.NewPageIndex;
                PopulateCheckedValues();
                DataTable dt = (DataTable)ViewState["TestPending"];
                grdTestPending.DataSource = SortDataTablePending(dt as DataTable, true);
                grdTestPending.DataBind();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdTestPending_PageIndexChanging");
            }
        }
        protected void grdTestingPass_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            try
            {
                grdTestingPass.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["TestPass"];
                grdTestingPass.DataSource = SortDataTablePassed(dt as DataTable, true);
                grdTestingPass.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdTestingPass_PageIndexChanging");
            }
        }
        protected void grdTestingPass_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdTestingPass.PageIndex;
            DataTable dt = (DataTable)ViewState["TestPass"];
            string sortingDirection = string.Empty;

            grdTestingPass.DataSource = SortDataTablePassed(dt as DataTable, false);
            grdTestingPass.DataBind();
            grdTestingPass.PageIndex = pageIndex;
        }
        protected void grdTestPending_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdTestPending.PageIndex;
            DataTable dt = (DataTable)ViewState["TestPending"];
            string sortingDirection = string.Empty;

            grdTestPending.DataSource = SortDataTablePending(dt as DataTable, false);
            grdTestPending.DataBind();
            grdTestPending.PageIndex = pageIndex;
        }

        protected DataView SortDataTablePassed(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);

                        ViewState["TestPass"] = dataView.ToTable();
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());

                        ViewState["TestPass"] = dataView.ToTable();
                    }


                }
                return dataView;
            }
            else
            {
                return new DataView();
            }

        }
        protected DataView SortDataTablePending(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                        ViewState["TestPending"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["TestPending"] = dataView.ToTable();

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


    }
}