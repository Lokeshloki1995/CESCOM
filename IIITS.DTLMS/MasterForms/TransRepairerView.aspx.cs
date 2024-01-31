﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;

namespace IIITS.DTLMS.MasterForms
{
    public partial class TransRepairerView : System.Web.UI.Page
    {

        clsSession objSession;
        string strFormCode = "TransRepairerView.aspx";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                objSession = (clsSession)Session["clsSession"];
                CalendarExtender1.EndDate = DateTime.Now;
                lblMessage.Text = string.Empty;
                if (!IsPostBack)
                {
                    CheckAccessRights("4");
                    Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION ORDER BY DIV_CODE", "-Select-", cmbdiv);
                    LoadRepairerGrid(cmbdiv.SelectedValue);

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load");
            }

        }
        protected void cmbdiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbdiv.SelectedIndex > 0)
                {
                    LoadRepairerGrid(cmbdiv.SelectedValue);

                }
                else
                {
                    LoadRepairerGrid(cmbdiv.SelectedValue);
                    //ShowMsgBox("Please Select the Taluq");
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbDivision_SelectedIndexChanged");
            }
        }

        public void LoadRepairerGrid(string divcode = "")
        {
            try
            {
                clsTransRepairer objRepairer = new clsTransRepairer();
                if (objSession.OfficeCode.Length > 2)
                {
                    objRepairer.sRepairOffCode = objSession.OfficeCode.Substring(0, 2);
                }
                else
                {
                    objRepairer.sRepairOffCode = objSession.OfficeCode;
                }

                DataTable dt = new DataTable();
                if (divcode == "-Select-")
                {
                    divcode = "";
                    objRepairer.sRepairOffCode = divcode;
                }
                else
                {
                    objRepairer.sRepairOffCode = divcode;
                }

                dt = objRepairer.LoadRepairerDetails(objRepairer);
                grdRepairer.DataSource = dt;
                grdRepairer.DataBind();
                ViewState["Repairer"] = dt;


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadSupplierGrid");
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
                Response.Redirect("TransRepairer.aspx", false);
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdNew_Click");
            }
        }


        protected void grdRepairer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdRepairer.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Repairer"];
                grdRepairer.DataSource = SortDataTable(dt as DataTable, true);
                grdRepairer.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdRepairer_PageIndexChanging");
            }
        }

        protected void grdRepairer_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdRepairer.PageIndex;
            DataTable dt = (DataTable)ViewState["Repairer"];
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
                        ViewState["Repairer"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["Repairer"] = dataView.ToTable();

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

        public void AdminAccess()
        {
            try
            {
                if (objSession.UserType != "1")
                {
                    grdRepairer.Columns[8].Visible = false;
                    cmdNew.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "AdminAccess");
            }
        }
        #region Access Rights

        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "TransRepairer";
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

        protected void grdRepairer_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "create")
                {
                    //Check AccessRights
                    bool bAccResult = CheckAccessRights("3");
                    if (bAccResult == false)
                    {
                        return;
                    }

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                    string sRepairerId = ((Label)row.FindControl("lblRepairId")).Text;
                    sRepairerId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(sRepairerId));
                    Response.Redirect("TransRepairer.aspx?StrQryId=" + sRepairerId + "");
                }


                //if (e.CommandName == "status")
                //{

                //    //Check AccessRights
                //    bool bAccResult = CheckAccessRights("3");
                //    if (bAccResult == false)
                //    {
                //        return;
                //    }

                //    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                //    string sRepairerId = ((Label)row.FindControl("lblRepairId")).Text;
                //    string sStatus = ((Label)row.FindControl("lblStatus")).Text;

                //    ImageButton imgDeactive;
                //    ImageButton imgActive;

                //    imgDeactive = (ImageButton)row.FindControl("imgDeactive");
                //    imgActive = (ImageButton)row.FindControl("imgActive");


                //    clsTransRepairer objRepairer = new clsTransRepairer();
                //    objRepairer.RepairerId = sRepairerId;

                //    if (sStatus == "A")
                //    {
                //        objRepairer.sStatus = "D";
                //        bool bResult = objRepairer.ActiveDeactiveRepairer(objRepairer);
                //        if (bResult == true)
                //        {
                //            imgDeactive.Visible = true;
                //            imgActive.Visible = false;

                //            ShowMsgBox("Repairer Deactivated Successfully");
                //            LoadRepairerGrid();
                //            return;
                //        }
                //    }
                //    if (sStatus == "D")
                //    {
                //        objRepairer.sStatus = "A";
                //        bool bResult = objRepairer.ActiveDeactiveRepairer(objRepairer);
                //        if (bResult == true)
                //        {
                //            imgDeactive.Visible = false;
                //            imgActive.Visible = true;

                //            ShowMsgBox("Repairer Activated Successfully");
                //            LoadRepairerGrid();
                //            return;
                //        }
                //    }
                //}


                // activate or deactivate the repairer
                if (e.CommandName == "status")
                {
                    //  Check AccessRights
                    bool bAccResult = CheckAccessRights("3");
                    if (bAccResult == false)
                    {
                        return;
                    }
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    string sUserId = ((Label)row.FindControl("lblRepairId")).Text;
                    string sStatus = ((Label)row.FindControl("lblStatus")).Text;

                    ImageButton imgDeactive = new ImageButton();
                    ImageButton imgActive = new ImageButton();
                    imgDeactive = (ImageButton)row.FindControl("imgDeactive");
                    imgActive = (ImageButton)row.FindControl("imgActive");

                    clsTransRepairer objRepairer = new clsTransRepairer();
                    objRepairer.sRepairerTalukID = sUserId;
                    ViewState["TALUKID"] = sUserId;
                    ViewState["STATUS"] = sStatus;

                    txtEffectFrom.Text = string.Empty;
                    txtReason.Text = string.Empty;

                    this.mdlPopup.Show();
                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtRepairerName = (TextBox)row.FindControl("txtRepairerName");


                    DataTable dt = (DataTable)ViewState["Repairer"];
                    dv = dt.DefaultView;

                    if (txtRepairerName.Text != "")
                    {
                        sFilter = "TR_NAME Like '%" + txtRepairerName.Text.Replace("'", "") + "%' AND";
                    }

                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdRepairer.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdRepairer.DataSource = dv;
                            ViewState["Repairer"] = dv.ToTable();
                            grdRepairer.DataBind();

                        }
                        else
                        {
                            ViewState["Repairer"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadRepairerGrid();
                    }


                }


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdSupplierDetails_RowCommand");


            }
        }

        protected void cmdSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateEnableDisable() == true)
                {
                    clsTransRepairer objrepairer = new clsTransRepairer();
                    objrepairer.sDeactivateReason = txtReason.Text;
                    objrepairer.sDeactivateFrom = txtEffectFrom.Text;
                    // objrepairer.sDeactivateTo = null;
                    objrepairer.RepairerId = Convert.ToString(ViewState["TALUKID"]);
                    objrepairer.sStatus = Convert.ToString(ViewState["STATUS"]);
                    ImageButton imgDeactive = new ImageButton();
                    ImageButton imgActive = new ImageButton();
                    if (objrepairer.sStatus == "A")
                    {
                        objrepairer.sStatus = "D";
                        bool bResult = objrepairer.ActiveDeactiveRepairer(objrepairer);
                        if (bResult == true)
                        {
                            imgDeactive.Visible = true;
                            imgActive.Visible = false;
                            ShowMsgBox("Repairer Deactivated Successfully");
                            LoadRepairerGrid();
                            txtEffectFrom.Text = "";
                            txtReason.Text = "";

                        }
                    }
                    else
                    {
                        objrepairer.sStatus = "A";
                        bool bResult = objrepairer.ActiveDeactiveRepairer(objrepairer);
                        if (bResult == true)
                        {
                            imgDeactive.Visible = false;
                            imgActive.Visible = true;
                            ShowMsgBox("Repairer Activated Successfully");
                            LoadRepairerGrid();
                            txtEffectFrom.Text = "";
                            txtReason.Text = "";

                        }
                    }
                }


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdSubmit_Click");
            }
        }

        public bool ValidateEnableDisable()
        {
            bool bValidate = false;
            try
            {
                if (txtEffectFrom.Text.Trim() == "" || txtEffectFrom.Text.Trim() == null)
                {
                    lblMsg.Text = "Enter Effect From";
                    txtEffectFrom.Focus();
                    mdlPopup.Show();
                    return bValidate;
                }
                //if (txtEffectTill.Text.Trim() == "" || txtEffectTill.Text.Trim() == null)
                //{
                //    lblMsg.Text = "Enter Effect Till";
                //    txtEffectTill.Focus();
                //    mdlPopup.Show();
                //    return bValidate;
                //}
                if (txtReason.Text.Trim() == "" || txtReason.Text.Trim() == null)
                {
                    lblMsg.Text = "Enter Reason";
                    txtReason.Focus();
                    mdlPopup.Show();
                    return bValidate;
                }
                if (txtReason.Text.Length > 500)
                {
                    lblMsg.Text = "Enter Below 500 charecters";
                    txtReason.Focus();
                    mdlPopup.Show();
                    return bValidate;
                }

                string sResult = Genaral.DateComparision(txtEffectFrom.Text, "", true, false);
                if (sResult == "2")
                {
                    ShowMsgBox("Effect From Date should be Greater than Current Date");
                    txtEffectFrom.Focus();
                    mdlPopup.Show();
                    return bValidate;
                }
                //sResult = Genaral.DateComparision(txtEffectTill.Text, txtEffectFrom.Text, false, false);
                //if (sResult == "2")
                //{
                //    ShowMsgBox("Effect From Date should be Greater than Effect Till Date");
                //    txtEffectTill.Focus();
                //    mdlPopup.Show();
                //    return bValidate;
                //}

                bValidate = true;
                return bValidate;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, " ValidateEnableDisable");
                return bValidate;
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
                    imgBtnEdit = (ImageButton)e.Row.FindControl("imgBtnEdit");

                    if (lblStatus.Text == "A")
                    {
                        ImageButton imgActive;
                        imgActive = (ImageButton)e.Row.FindControl("imgActive");
                        imgActive.Visible = true;
                        imgBtnEdit.Enabled = true;
                        imgBtnEdit.ToolTip = "";
                    }
                    else
                    {
                        ImageButton imgDeActive;
                        imgDeActive = (ImageButton)e.Row.FindControl("imgDeActive");
                        imgDeActive.Visible = true;
                        imgBtnEdit.Enabled = false;
                        imgBtnEdit.ToolTip = "Repairer is DeActivated,You cannot Edit";
                    }
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
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
                dt.Columns.Add("TR_ID");
                dt.Columns.Add("TR_NAME");
                dt.Columns.Add("TR_ADDRESS");
                dt.Columns.Add("TR_PHONE");
                dt.Columns.Add("TR_EMAIL");
                dt.Columns.Add("TR_BLACKED_UPTO");
                dt.Columns.Add("TR_BLACK_LISTED");
                dt.Columns.Add("TR_STATUS");
                dt.Columns.Add("TR_OFFICECODE");
                dt.Columns.Add("RA_TALQ_ADDR");
                dt.Columns.Add("RA_CONTACT_NO");
                dt.Columns.Add("DIVISION");
                dt.Columns.Add("TALUK");
                dt.Columns.Add("RA_BLACK_LISTED");
                dt.Columns.Add("RA_BLACK_LISTED_UPTO");
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
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowEmptyGrid");

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

        protected void Export_clickRepairer(object sender, EventArgs e)
        {


            try
            {
                //clsTransRepairer objRepairer = new clsTransRepairer();
                //if (objSession.OfficeCode.Length > 2)
                //{
                //    objRepairer.sRepairOffCode = objSession.OfficeCode.Substring(0, 2);
                //}
                //else
                //{
                //    objRepairer.sRepairOffCode = objSession.OfficeCode;
                //}

                //DataTable dt = new DataTable();
                //dt = objRepairer.LoadRepairerDetails(objRepairer);

                DataTable dt = (DataTable)ViewState["Repairer"];

                if (dt.Rows.Count > 0)
                {
                    dt.Columns["TR_NAME"].ColumnName = "Repairer Name";
                    dt.Columns["TR_PHONE"].ColumnName = "Repairer Contact Number";
                    dt.Columns["TR_EMAIL"].ColumnName = "Email Id";
                    dt.Columns["TR_ADDRESS"].ColumnName = "Registered Address";

                    // dt.Columns["TR_BLACK_LISTED"].ColumnName = "Black listed";
                    dt.Columns["RA_TALQ_ADDR"].ColumnName = "Taluk Address";
                    dt.Columns["RA_CONTACT_NO"].ColumnName = "Taluk Contact No";
                    dt.Columns["RA_BLACK_LISTED"].ColumnName = "Black Listed";
                    dt.Columns["RA_BLACK_LISTED_UPTO"].ColumnName = "Black Listed Upto";
                    dt.Columns["DIVISION"].ColumnName = "Division";
                    dt.Columns["TALUK"].ColumnName = "Taluk";
                    dt.Columns["RA_STATUS"].ColumnName = "Active/Deactive";


                    List<string> listtoRemove = new List<string> { "TR_ID", "RA_ID" };
                    string filename = "RepairerDetails" + DateTime.Now + ".xls";
                    string pagetitle = "RepairerDetails";

                    Genaral.getexcel(dt, listtoRemove, filename, pagetitle);
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
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Export_clickRepairer");
            }

        }

    }
}