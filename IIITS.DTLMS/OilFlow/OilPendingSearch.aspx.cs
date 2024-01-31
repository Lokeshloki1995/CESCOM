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
using IIITS.DTLMS.BL.OilFlow;

namespace IIITS.DTLMS.OilFlow
{
    public partial class OilPendingSearch : System.Web.UI.Page
    {
        string strFormCode = "DeliverPendingSearch";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                lblMessage.Text = string.Empty;
                objSession = (clsSession)Session["clsSession"];
                if (!IsPostBack)
                {
                    if (CheckAccessRights("4"))
                    {
                        //LoadComboFiled();
                        string Pono = LoadReceivePendingDetails();
                        //LoadTestingPendingDetails();

                        string strQry = string.Empty;

                        if (Pono == "")
                        {
                            strQry = "Title=Search and Select Oil Purchase Order Details&";
                            strQry += "Query=Select * from(SELECT DISTINCT OSD_PO_NO FROM TBLOILSENTMASTER  WHERE OSD_OFFICE_CODE LIKE '" + objSession.OfficeCode + "%' AND OSD_STATUS='0' and OSD_INSP_RESULT='1' union all "; 
                            strQry += "SELECT DISTINCT OSD_PO_NO FROM TBLOILSENTMASTER inner join TBLAGENCYINSPECTIONDETAILS on OSD_ID=AIN_OSD_ID WHERE  OSD_STATUS = '1' and  OSD_OFFICE_CODE LIKE '" + objSession.OfficeCode + "%' AND AIN_STATUS_FLAG = '0' ORDER BY OSD_PO_NO DESC)  ";
                            strQry += "where {0} like %{1}% &DBColName=UPPER(OSD_PO_NO)&";
                            strQry += "ColDisplayName=PO No&";
                        }
                        else
                        {

                            strQry = "Title=Search and Select Oil Purchase Order Details&";
                            strQry += "Query=Select * from(SELECT DISTINCT OSD_PO_NO FROM TBLOILSENTMASTER INNER JOIN TBLAGENCYINSPECTIONDETAILS ON OSD_ID=AIN_OSD_ID WHERE OSD_OFFICE_CODE LIKE '" + objSession.OfficeCode + "%' AND AIN_STATUS_FLAG = '0' ORDER BY OSD_PO_NO DESC)  ";
                            strQry += "where {0} like %{1}% &DBColName=UPPER(OSD_PO_NO)&";
                            strQry += "ColDisplayName=PO No&";
                        }

                        strQry = strQry.Replace("'", @"\'");

                        cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtPONo.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + txtPONo.ClientID + ")");
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
                // clsOilTest objDeliverPending = new clsOilTest();
                clsOilTest objoiltest = new clsOilTest();

                //}

                objoiltest.sPurchaseOrderNo = txtPONo.Text.Trim();
                //objpending.sPendingDays = txtNoofDays.Text.Trim();
                objoiltest.sOfficeCode = objSession.OfficeCode;
                objoiltest.sRoleId = objSession.RoleId;
                objoiltest.sTestingDone = "0";


                objoiltest.sOfficeCode = objSession.OfficeCode;
                objoiltest.sTestingDone = "1";

                DataTable dt = new DataTable();
                dt = objoiltest.LoadTestingPassedDetails(objoiltest);

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
                clsOilTest objoiltest = new clsOilTest();

                objoiltest.sPurchaseOrderNo = txtPONo.Text.Trim();

                objoiltest.sOfficeCode = objSession.OfficeCode;
                objoiltest.sTestingDone = "1";

                DataTable dt = new DataTable();
                //dt = objoiltest.LoadPendingToRecieveDetails(objoiltest); 
                dt = objoiltest.LoadPendingForTestingDetails(objoiltest);

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
        private string LoadReceivePendingDetails()
        {
            string Pono = string.Empty;
            try
            {
                clsOilTest objoiltest = new clsOilTest();

                objoiltest.sPurchaseOrderNo = txtPONo.Text.Trim();

                objoiltest.sOfficeCode = objSession.OfficeCode;
                objoiltest.sTestingDone = "1";

                DataTable dt = new DataTable();
                Pono = objoiltest.GetPono(objoiltest);
                dt = objoiltest.LoadPendingForTestingDetails(objoiltest);

                grdTestPending.DataSource = dt;
                grdTestPending.DataBind();
                ViewState["TestPending"] = dt;
                 return Pono;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadTestingPendingDetails");
                return Pono;
            }
        }
        
        public DataSet getDataSetExportToExcel()
        {
            DataSet ds = new DataSet();
            try
            {

                clsDTrRepairActivity objDeliverPending = new clsDTrRepairActivity();


                objDeliverPending.sPurchaseOrderNo = txtPONo.Text.Trim();

                objDeliverPending.sOfficeCode = objSession.OfficeCode;
                objDeliverPending.sTestingDone = "1";

                DataTable dttesting = new DataTable();
                dttesting = objDeliverPending.LoadPendingForTestingDetails(objDeliverPending);


                objDeliverPending.sPurchaseOrderNo = txtPONo.Text.Trim();


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

            txtPONo.Text = string.Empty;
            //cmbRepairer.SelectedIndex = 0;

            ////grdTestingPass.DataSource = null;
            ////grdTestingPass.DataBind();
            //cmbSupplier.SelectedIndex = 0;
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
                    Label lblPoNo = (Label)row.FindControl("lblPoNo");
                    Label lblInsResultId = (Label)row.FindControl("lblInsResult");
                    Label lblTotalQuantity = (Label)row.FindControl("lblTotalQuantity");


                    string sPurchaseOrderNo = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtPONo.Text));
                    string sInsResultId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtPONo.Text));
                    string sQty = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblTotalQuantity.Text));
                    Response.Redirect("DeliverOil.aspx?InsResult=" + sInsResultId + "&PoNo=" + sPurchaseOrderNo + "&Qty=" + sQty, false);
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