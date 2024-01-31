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
    public partial class WebForm1 : System.Web.UI.Page
    {
        string strFormCode = "TransRepairMaster";
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
                Form.DefaultButton = cmdSave.UniqueID;

                if (!IsPostBack)
                {
                    LoadMasterRepairer();
                    CheckAccessRights("4");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load");
            }
        }

        public void LoadMasterRepairer()
        {
            clsTransRepairer objrepairer = new clsTransRepairer();
            objrepairer.sRepairOffCode = "";
            DataTable dt = objrepairer.LoadRepairerMasterDetails(objrepairer);
            if (dt.Rows.Count > 0)
            {
                grdRepairer.DataSource = dt;
                grdRepairer.DataBind();
                ViewState["RepairerMaster"] = dt;
            }
            

        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            clsTransRepairer objRepairer = new clsTransRepairer();
            string[] Arr = new string[2];

            //Check AccessRights
            bool bAccResult;
            if (cmdSave.Text == "Update")
            {
                bAccResult = CheckAccessRights("3");
            }
            else
            {
                bAccResult = CheckAccessRights("2");
            }

            if (bAccResult == false)
            {
                return;
            }

            if (ValidateForm() == true)
            {
                objRepairer.RepairerId = txtRepairerId.Text;
                objRepairer.RepairerName = txtRepairName.Text.ToUpper().Replace("'", "").Trim();
                objRepairer.RegisterAddress = txtRepairAddress.Text.Replace("'", "");
                objRepairer.RepairerPhoneNo = txtRepairPhnNo.Text.Replace("'", "");
                objRepairer.RepairerEmail = txtRepairEmailId.Text.Replace("'", "").ToLower();
                // objRepairer.RepairerType = cmbType.SelectedValue;
               

                objRepairer.sCrby = objSession.UserId;
              // objRepairer.sContactPerson = txtContactPerson.Text.Trim().Replace("'", "");
                objRepairer.sFax = txtFaxNo.Text.Trim();

                Arr = objRepairer.SaveRepairerMasterDetails(objRepairer);

                if (Arr[1].ToString() == "0")
                {
                    //txtRepairerId.Text = Arr[0].ToString();
                    //cmdSave.Text = "Update";
                    ShowMsgBox("Saved Successfully");
                    cmdReset_Click(sender, e);
                }

                LoadMasterRepairer();

                if (Arr[1].ToString() == "1")
                {
                    ShowMsgBox(Arr[0]);
                    return;
                }

                if (Arr[1].ToString() == "4")
                {
                    ShowMsgBox(Arr[0]);
                    return;
                }
            }
        }
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

        private bool ValidateForm()
        {
            bool bValidate = false;
            try
            {


                if (txtRepairName.Text == "")
                {
                    txtRepairName.Focus();
                    ShowMsgBox("Please Enter the Name of Repairer");
                    return bValidate;
                }
                if (txtRepairAddress.Text == "")
                {
                    txtRepairAddress.Focus();
                    ShowMsgBox("Please Enter Valid Register Address");
                    return bValidate;
                }
                if (txtRepairPhnNo.Text == "")
                {
                    txtRepairPhnNo.Focus();
                    ShowMsgBox("Please Enter Valid Phone No");
                    return bValidate;
                }

                if (txtRepairPhnNo.Text.Length < 10)
                {
                    ShowMsgBox("Enter valid  Phone no");
                    txtRepairPhnNo.Focus();
                    return bValidate;
                }

                if (txtRepairEmailId.Text == "")
                {
                    txtRepairEmailId.Focus();
                    ShowMsgBox("Please Enter the EmailId");
                    return bValidate;
                }

                if (txtFaxNo.Text.Trim() != "")
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(txtFaxNo.Text, "^[0-9 \\-\\s \\( \\)]*$"))
                    {
                        ShowMsgBox("Please Enter Valid Fax No (Eg:865-934-1234)");
                        return bValidate;
                    }
                }

                if (txtRepairPhnNo.Text != "")
                {
                    if ((txtRepairPhnNo.Text.Length - txtRepairPhnNo.Text.Replace("-", "").Length) >= 2)
                    {
                        txtRepairPhnNo.Focus();
                        ShowMsgBox("You cannot use more than one hyphen (-)");
                        return bValidate;
                    }

                    if (txtRepairPhnNo.Text.Contains(".") == true)
                    {
                        txtRepairPhnNo.Focus();
                        ShowMsgBox("You cannot enter (.) in Phone Number");
                        return bValidate;
                    }
                }
                bValidate = true;
                return bValidate;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ValidateForm");
                return bValidate;
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
            txtRepairName.Text = "";
            //txtContactPerson.Text = "";
            txtFaxNo.Text = "";
            txtRepairEmailId.Text = "";
            txtRepairName.Text = "";
            txtRepairerId.Text = "";
            txtRepairAddress.Text = "";
            txtRepairPhnNo.Text = "";

        }

        protected void grdRepairer_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdRepairer_PageIndexChanging");
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
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                    string sRepairerId = ((Label)row.FindControl("lblRepairId")).Text;
                    clsTransRepairer objRepairer = new clsTransRepairer();
                    objRepairer.RepairerId = sRepairerId;
                    objRepairer.GetRepairerDetails(objRepairer);

                    txtRepairerId.Text = objRepairer.RepairerId;
                    txtRepairName.Text = objRepairer.RepairerName;
                    txtRepairPhnNo.Text = objRepairer.RepairerPhoneNo;
                    txtRepairEmailId.Text = objRepairer.RepairerEmail;
                    txtFaxNo.Text = objRepairer.sFax;
                    txtRepairAddress.Text = objRepairer.RegisterAddress;

                    cmdSave.Text = "Update";
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
                        LoadMasterRepairer();
                    }

                }

                if (e.CommandName == "status")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                    string sRepairerId = ((Label)row.FindControl("lblRepairId")).Text;
                    clsTransRepairer objRepairer = new clsTransRepairer();
                    objRepairer.RepairerId = sRepairerId;
                    string status = ((Label)row.FindControl("lblStatus")).Text;

                    if(status != "" && status == "A" )
                    {
                        objRepairer.sMasterStatus = "D";
                        bool updatedStatus = objRepairer.ActiveDeactiveMasterRepairer(objRepairer);
                        if (updatedStatus)
                        {
                            ShowMsgBox("Repairer Deactivated Successfully");
                        }
                        else
                        {
                            ShowMsgBox("Something Went Wrong");
                        }
                        LoadMasterRepairer();
                        
                    }
                    if(status != "" && status == "D" )
                    {
                        objRepairer.sMasterStatus = "A";
                        bool updatedStatus = objRepairer.ActiveDeactiveMasterRepairer(objRepairer);
                        if (updatedStatus)
                        {
                            ShowMsgBox("Repairer Deactivated Successfully");
                        }
                        else
                        {
                            ShowMsgBox("Something Went Wrong");
                        }
                        LoadMasterRepairer();
                    }
                    
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
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
                dt.Columns.Add("TR_FAX");
                dt.Columns.Add("TR_MASTER_STATUS");

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