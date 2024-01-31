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
    public partial class TransRepairer : System.Web.UI.Page
    {

        string strFormCode = "TransRepairer.aspx";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                Form.DefaultButton = cmdSave.UniqueID;
                objSession = (clsSession)Session["clsSession"];
                lblMessage.Text = string.Empty;
                txtBlackUpto.Attributes.Add("readonly", "readonly");

                if (!IsPostBack)
                {
                    if (Request.QueryString["StrQryId"] != null && Request.QueryString["StrQryId"].ToString() != "")
                    {
                      txtRepairerTalukID.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["StrQryId"]));
                        txtRepairerId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["StrQryId"])); 
                        talqReapirerTable.Visible = false;
                        cmbTalq.Enabled = false;
                        cmbDivision.Enabled = false;

                    }
                    else
                    {
                        LoadMasterRepairer();
                    }
                    Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION ORDER BY DIV_CODE", "-Select-", cmbDivision);



                    cmbIsBlack.SelectedIndex = 2;
                    //txtBlackUpto.Enabled = false;
                 //   txtBlackUpto.Text = string.Empty;
                    txtRepairName.Enabled = false;
                    txtFaxNo.Enabled = false;
                    txtRepairEmailId.Enabled = false;
                    txtRepairAddress.Enabled = false;
                    txtRepairPhnNo.Enabled = false;
                  //  txtBlackUpto.Enabled = false;
                    
                    txtdateExtender.StartDate = DateTime.Now;
                    

                    //if (txtRepairerTalukID.Text != "")
                    //{
                    //    GetRepairerDetails(txtRepairerTalukID.Text);
                    //    cmbIsBlack_SelectedIndexChanged(sender, e);
                    //}
                    if (txtRepairerId.Text != "")
                    {
                        GetRepairerDetails(txtRepairerId.Text);
                        cmbIsBlack_SelectedIndexChanged(sender, e);
                    }

                    txtBlackUpto.Attributes.Add("onblur", "return ValidateDate(" + txtBlackUpto.ClientID + ");");
                   // CheckAccessRights("4");
                    
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "Page_Load");
            }
        }

        public void LoadMasterRepairer()
        {
            clsTransRepairer objrepairer = new clsTransRepairer();
            objrepairer.sRepairOffCode = "";
            DataTable dt = objrepairer.LoadRepairerMasterDetails(objrepairer);
            grdRepairer.DataSource = dt;
            grdRepairer.DataBind();
            ViewState["RepairerTaluk"] = dt; 

        }


        public void GetRepairerDetails(string strRepairerId)
        {
            try
            {
                DataTable dtBillDetails = new DataTable();
                clsTransRepairer objRepair = new clsTransRepairer();

                objRepair.RepairerId = strRepairerId;
              //  objRepair.sRepairerTalukID = txtRepairerTalukID.Text;
                objRepair.GetRepairerDetails(objRepair);

                // here ur not saving the repairer id  but getting the taluk id and hence its  inserting after updating rather than 
               txtRepairerId.Text = objRepair.RepairerId;
                txtRepairerTalukID.Text = objRepair.sRepairerTalukID;
                txtRepairName.Text = objRepair.RepairerName;
                txtRepairAddress.Text = objRepair.RegisterAddress;
                txtRepairPhnNo.Text = objRepair.RepairerPhoneNo;
                txtRepairEmailId.Text = objRepair.RepairerEmail;
                cmbDivision.SelectedValue = objRepair.sOffCode;

                if (cmbDivision.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT TQ_SLNO , TQ_NAME  FROM TBLTALQ WHERE TQ_DIV_CODE like '" + cmbDivision.SelectedValue + "%'", "-Select-", cmbTalq);
                }
                cmbTalq.SelectedValue = objRepair.staluq;
                cmbIsBlack.Text = objRepair.RepairerBlacklisted;
                txtBlackUpto.Text = objRepair.RepairerBlackedupto;
                txtCommAddress.Text = objRepair.CommAddress;
                txtContactPerson.Text = objRepair.sContactPerson;
                txtContractPeriod.Text = objRepair.sContractPeriod;
                txtFaxNo.Text = objRepair.sFax;
                txtMobileNo.Text = objRepair.sMobileNo;
               // txtStatus.Text = objRepair.sStatus;
                cmdSave.Text = "Update";

                txtRepairName.Enabled = false;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "getRepairerDetails");
            }

        }




        protected void cmdSave_Click(object sender, EventArgs e)
        {

            try
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
                    #region repairerMasterData
                   objRepairer.RepairerId = txtRepairerId.Text;
                    objRepairer.RepairerName = txtRepairName.Text.ToUpper().Replace("'", "").Trim();
                    objRepairer.RegisterAddress = txtRepairAddress.Text.Replace("'", "");
                    objRepairer.RepairerPhoneNo = txtRepairPhnNo.Text.Replace("'", "");
                    objRepairer.RepairerEmail = txtRepairEmailId.Text.Replace("'", "").ToLower();
                    objRepairer.sContactPerson = txtContactPerson.Text.Trim().Replace("'", "");
                    objRepairer.sFax = txtFaxNo.Text.Trim();
                    #endregion



                    #region repairerTalukData
                    // objRepairer.RepairerType = cmbType.SelectedValue;
                    if (cmbIsBlack.SelectedIndex > 0)
                    {
                        objRepairer.RepairerBlacklisted = cmbIsBlack.SelectedValue;
                    }
                    objRepairer.sOffCode = cmbDivision.SelectedValue; //division code
                    objRepairer.sRepairerTalukID = txtRepairerTalukID.Text; //tblrepaireraddr id 
                    objRepairer.staluq = cmbTalq.SelectedValue; // taluq id 
                    objRepairer.RepairerBlackedupto = txtBlackUpto.Text;
                    objRepairer.sContractPeriod = txtContractPeriod.Text;
                    objRepairer.CommAddress = txtCommAddress.Text.Replace("'", "");
                    objRepairer.sContactPerson = txtContactPerson.Text;
                    objRepairer.sCrby = objSession.UserId;
                  //  objRepairer.sStatus = txtStatus.Text;
                    
                    objRepairer.sMobileNo = txtMobileNo.Text.Trim();

                    #endregion
                    Arr = objRepairer.SaveRepairerDetails(objRepairer);

                    if (Arr[1].ToString() == "0")
                    {
                        //txtRepairerId.Text = Arr[0].ToString();
                        //cmdSave.Text = "Update";
                        ShowMsgBox(Arr[0]);
                        cmdReset_Click(sender, e);
                    }


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

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdSave_Click");
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
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ShowMsgBox");
            }
        }



        bool ValidateForm()
        {
            bool bValidate = false;
            try
            {

                if (txtContactPerson.Text == "")
                {
                    txtContactPerson.Focus();
                    ShowMsgBox("Please Enter the Contact Person");
                    return bValidate;
                }
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

                if (cmbDivision.SelectedIndex == 0)
                {
                    cmbDivision.Focus();
                    ShowMsgBox("Select Division");
                    return bValidate;
                }

                if (cmbTalq.SelectedIndex == 0)
                {
                    cmbTalq.Focus();
                    ShowMsgBox("Please Select Talq");
                    return bValidate;
                }

                //if (cmbIsBlack.SelectedIndex == 0)
                //{
                //    ShowMsgBox("Please Select the Blacklisted condition");
                //    cmbIsBlack.Focus();
                //    return bValidate;
                //}
                if (cmbIsBlack.SelectedValue == "1")
                {
                    if (txtBlackUpto.Text == "")
                    {
                        ShowMsgBox("Please select the BlockListed Upto Date");
                        txtBlackUpto.Focus();
                        return bValidate;
                    }
                    if (txtBlackUpto.Text != "")
                    {
                        string sRet = Genaral.DateValidation(txtBlackUpto.Text);
                        if (sRet != "")
                        {
                            ShowMsgBox(sRet);
                            return bValidate;
                        }
                    }
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
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ValidateForm");
                return bValidate;
            }
        }

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtRepairerId.Text = string.Empty;
                txtRepairName.Text = string.Empty;
                txtRepairAddress.Text = string.Empty;
                txtRepairPhnNo.Text = string.Empty;
                txtRepairEmailId.Text = string.Empty;
                txtBlackUpto.Text = string.Empty;
                cmbIsBlack.SelectedIndex = 0;
                //cmbType.SelectedIndex = 0;
                cmdSave.Text = "Save";
                txtCommAddress.Text = string.Empty;
                txtContactPerson.Text = string.Empty;
                txtFaxNo.Text = string.Empty;
                txtMobileNo.Text = string.Empty;
                cmbDivision.SelectedIndex = 0;
                Genaral.Load_Combo("SELECT TQ_CODE , TQ_NAME  FROM TBLTALQ WHERE TQ_DIV_CODE like '%'", "-Select-", cmbTalq);
                txtContractPeriod.Text = string.Empty;

             //   txtRepairName.Enabled = true;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdReset_Click");
            }
        }

        

        protected void cmbIsBlack_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbIsBlack.SelectedIndex == 1)
                {
                  //  txtBlackUpto.Enabled = false;

                    blocklist.Visible = true;
                }
                else
                {
                    blocklist.Visible = false;
                   // txtBlackUpto.Enabled = false;
                   // txtBlackUpto.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmbIsBlack_SelectedIndexChanged");
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

        protected void cmbDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDivision.SelectedIndex > 0)
                {
                     Genaral.Load_Combo("SELECT TQ_SLNO , TQ_NAME  FROM TBLTALQ  WHERE TQ_DIV_CODE like '" + cmbDivision.SelectedValue + "%' ORDER BY  TQ_DT_ID", "-Select-", cmbTalq);
                    //Genaral.Load_Combo("SELECT TQ_CODE , TQ_NAME ||  '---'  || (SELECT DIV_NAME  FROM TBLDIVISION WHERE DIV_CODE = TQ_DIV_CODE ) || ' DIVISION'  DIV_NAME   FROM TBLTALQ ORDER BY  TQ_DIV_CODE ", "-Select-", cmbTalq);

                }
                else
                {
                    ShowMsgBox("Please Select the Division");
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbDivision_SelectedIndexChanged");
            }
        }

        protected void grdRepairer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdRepairer.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["RepairerTaluk"];
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
                        ViewState["RepairerTaluk"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["RepairerTaluk"] = dataView.ToTable();

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

                    cmbDivision.SelectedIndex = 0;
                    cmbTalq.SelectedIndex = 0;

                  //txtRepairerId.Text = objRepairer.RepairerId;
                    txtRepairName.Text = objRepairer.RepairerName;
                    txtRepairPhnNo.Text = objRepairer.RepairerPhoneNo;
                    txtRepairEmailId.Text = objRepairer.RepairerEmail;
                    txtFaxNo.Text = objRepairer.sFax;
                    txtRepairAddress.Text = objRepairer.RegisterAddress;
                    txtFaxNo.Text = objRepairer.sFax;
                    txtContactPerson.Text = objRepairer.sContactPerson;
                }
                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtRepairerName = (TextBox)row.FindControl("txtRepairerName");


                    DataTable dt = (DataTable)ViewState["RepairerTaluk"];
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
                            ViewState["RepairerTaluk"] = dv.ToTable();
                            grdRepairer.DataBind();

                        }
                        else
                        {
                            ViewState["RepairerTaluk"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
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
            try
            {
                DataTable dt = new DataTable();
                GridViewSortExpression = e.SortExpression;
                int pageIndex = grdRepairer.PageIndex;
                dt = (DataTable)ViewState["RepairerTaluk"];
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
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowEmptyGrid");
            }
        }

    }
}
   