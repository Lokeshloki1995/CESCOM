using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IIITS.DTLMS.BL;
using System.Collections;

namespace IIITS.DTLMS.MasterForms
{
    public partial class AgencyMaster : System.Web.UI.Page
    {
        string strFormCode = "AgencyMaster";
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
                    //LoadAgencyMasterRepairer();
                    CheckAccessRights("4");
                    if (Request.QueryString["StrQryId"] != null && Request.QueryString["StrQryId"].ToString() != "")
                    {
                        txtRepairerId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["StrQryId"]));
                    }
                    Genaral.Load_Combo("SELECT \"DIV_CODE\",\"DIV_NAME\" FROM \"TBLDIVISION\"", "-Select-", cmbDivision);

                    //txtRepairerId.Enabled = false;
                    //txtRepairName.Enabled = false;
                    //txtRepairPhnNo.Enabled = false;
                    //txtRepairAddress.Enabled = false;
                    //txtRepairEmailId.Enabled = false;

                    if (txtRepairerId.Text != "")
                    {
                        GetAgencyRepairerDetails(txtRepairerId.Text);

                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load");
            }
        }

        //public void LoadAgencyMasterRepairer()
        //{
        //    clsAgency objAgency = new clsAgency();
        //    objAgency.sRepairOffCode = "";
        //    DataTable dt = objAgency.LoadAgencyRepairerMasterDetails(objAgency);
        //    if (dt.Rows.Count > 0)
        //    {
        //        grdRepairer.DataSource = dt;
        //        grdRepairer.DataBind();
        //        ViewState["RepairerMaster"] = dt;
        //    }


        //}
        public void GetAgencyRepairerDetails(string sAgencyId)
        {
            try
            {
                string[] strQryVallist = null;
                clsAgency objAgency = new clsAgency();
                objAgency.AgencyId = sAgencyId;
                objAgency.GetAgencyRepairerDetails(objAgency);
                txtRepairName.Enabled = false;
                txtRepairerId.Text = objAgency.AgencyId;
                txtRepairName.Text = objAgency.AgencyName;
                txtRepairPhnNo.Text = objAgency.AgencyPhoneNo;
                txtRepairEmailId.Text = objAgency.AgencyEmail;
                txtOfficeCode.Text = objAgency.sOffCode;
                txtOfficeCode.ReadOnly = true;
                btnSearch.Visible = true;
                //txtFaxNo.Text = objAgency.sFax;
                txtRepairAddress.Text = objAgency.AgencyAddress;
                ArrayList OffCode = new ArrayList();
                if(objAgency.sOffCode!="" && objAgency.sOffCode != null)
                {
                    strQryVallist = objAgency.sOffCode.Split(',');
                    foreach (string sOffCode in strQryVallist)
                    {
                        if (sOffCode != "")
                        {
                            OffCode.Add(sOffCode);
                        }

                    }
                }

                ViewState["Existed_Offcode"] = OffCode;
                cmdSave.Text = "Update";
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            clsAgency objAgency = new clsAgency();
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
                objAgency.AgencyId = txtRepairerId.Text;
                objAgency.AgencyName = txtRepairName.Text.ToUpper().Replace("'", "").Trim();
                objAgency.AgencyAddress = txtRepairAddress.Text.Replace("'", "");
                objAgency.AgencyPhoneNo = txtRepairPhnNo.Text.Replace("'", "");
                objAgency.AgencyEmail = txtRepairEmailId.Text.Replace("'", "").ToLower();
                objAgency.sOffCode = txtOfficeCode.Text;
                if (ViewState["Existed_Offcode"] != null)
                {
                    objAgency.sExistedOffCode = (ArrayList)ViewState["Existed_Offcode"];
                }

                objAgency.sCrby = objSession.UserId;
                Arr = objAgency.SaveAgencyMasterDetails(objAgency);

                if (Arr[1].ToString() == "0")
                {
                    ShowMsgBox("Agency Created Successfully");
                   // cmdReset_Click(sender, e);
                    cmdSave.Enabled = false;
                }

                //LoadAgencyMasterRepairer();

                if (Arr[1].ToString() == "1")
                {
                    ShowMsgBox(Arr[0]);
                    cmdSave.Enabled = false;
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

                objApproval.sFormName = "AgencyCreate";
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
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.mdlPopup.Show();
                if (ViewState["Existed_Offcode"] != null || ViewState["CHECKED_ITEMS"] != null)
                {
                    if (ViewState["Existed_Offcode"] != null && ViewState["CHECKED_ITEMS"] == null)
                    {
                        pnlControls.Visible = true;
                        LoadOffice(objSession.OfficeCode);
                        SaveCheckedValuesexist();
                        PopulateCheckedValuesoffcode();
                    }

                    if (ViewState["CHECKED_ITEMS"] != null)
                    {
                        pnlControls.Visible = true;
                        SaveCheckedValues();
                        LoadOffice(objSession.OfficeCode);
                        PopulateCheckedValues();
                    }
                }
                else
                {
                    pnlControls.Visible = true;
                    LoadOffice(objSession.OfficeCode);
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        private void SaveCheckedValues()
        {
            try
            {
                ArrayList userdetails = new ArrayList();
                ArrayList tmpArrayList = new ArrayList();

                int index = -1;
                string strIndex = string.Empty;
                string strOk1 = string.Empty;
                foreach (GridViewRow gvrow in GrdOffices.Rows)
                {
                    index = Convert.ToInt32(GrdOffices.DataKeys[gvrow.RowIndex].Value);
                    CheckBox result = ((CheckBox)gvrow.FindControl("cbSelect"));
                    // Check in the Session
                    if ((ArrayList)ViewState["CHECKED_ITEMS"] != null)
                        userdetails = (ArrayList)ViewState["CHECKED_ITEMS"];


                    Label lblOff = (Label)gvrow.FindControl("lblOffCode");

                    if (result.Checked == true)
                    {
                        if (!userdetails.Contains(index))
                        {
                            userdetails.Add(index);
                        }
                    }

                    else
                    {
                        if (userdetails.Contains(index))
                        {
                            userdetails.Remove(index);
                        }
                    }
                }
                if (userdetails != null && userdetails.Count > 0)
                    ViewState["CHECKED_ITEMS"] = userdetails;

            }
            catch (Exception ex)
            {

                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }


        private void SaveCheckedValuesexist()
        {
            try
            {
                ArrayList userdetails = new ArrayList();
                ArrayList tmpArrayList = new ArrayList();

                int index = -1;
                string strIndex = string.Empty;
                string strOk1 = string.Empty;
                foreach (GridViewRow gvrow in GrdOffices.Rows)
                {
                    index = Convert.ToInt32(GrdOffices.DataKeys[gvrow.RowIndex].Value);
                    CheckBox result = ((CheckBox)gvrow.FindControl("cbSelect"));
                    // Check in the Session
                    if ((ArrayList)ViewState["Existed_Offcode"] != null)
                        userdetails = (ArrayList)ViewState["Existed_Offcode"];


                    Label lblOff = (Label)gvrow.FindControl("lblOffCode");

                    if (result.Checked == true)
                    {
                        if (!userdetails.Contains(Convert.ToString(index)))
                        {
                            userdetails.Add(index);
                        }
                    }

                    else
                    {
                        if (userdetails.Contains(Convert.ToString(index)))
                        {
                            userdetails.Remove(index);
                        }
                    }
                }
                if (userdetails != null && userdetails.Count > 0)
                    ViewState["Existed_Offcode"] = userdetails;

            }
            catch (Exception ex)
            {

                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }
        public void LoadOffice(string sDivisionCode = "", string sDivisionName = "")
        {
            try
            {
                DataTable dtPageDetaiils = new DataTable();
                clsDivision objDivision = new clsDivision();
                objDivision.sDivisionCode = sDivisionCode;
                objDivision.sDivisionName = sDivisionName;
                //if (objDivision.sUserId == "2")
                //{
                //    string officecode = clsDivision.getDivisionDetails(sDivisionCode, objDivision.sUserId);

                //    objDivision.sDivisionCode = officecode.Substring(0, 2);
                //}
                //else
                //{
                string officecode = sDivisionCode;
                if (officecode == "")
                {
                    objDivision.sDivisionCode = officecode;
                }
                else
                {
                    objDivision.sDivisionCode = officecode.Substring(0, 2);
                }

                dtPageDetaiils = objDivision.LoadAllDivisionDetails();


                //    dtPageDetaiils = objDivision.LoadAllDivisionDetailsSearch(objDivision.sDivisionCode);
                //if (dtPageDetaiils.Rows.Count > 0)
                //{
                GrdOffices.DataSource = dtPageDetaiils;
                GrdOffices.DataBind();
            }

            catch (Exception ex)
            {

                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void LoadOfficesearch(string sDivisionCode = "", string sDivisionName = "")
        {
            try
            {
                DataTable dtPageDetaiils = new DataTable();
                clsDivision objDivision = new clsDivision();
                objDivision.sDivisionCode = sDivisionCode;
                objDivision.sDivisionName = sDivisionName;
                //if (objDivision.sUserId == "2")
                //{
                //    string officecode = clsDivision.getDivisionDetails(sDivisionCode, objDivision.sUserId);

                //    objDivision.sDivisionCode = officecode.Substring(0, 2);
                //}
                //else
                //{
                string officecode = sDivisionCode;
                string officename = sDivisionName;
                if (officecode == "" && officename == "")
                {
                    objDivision.sDivisionCode = officecode;
                    objDivision.sDivisionName = officename;
                }
                else
                {
                    // objDivision.sDivisionCode = officecode.Substring(0, 2);
                    objDivision.sDivisionCode = officecode;
                    objDivision.sDivisionName = officename;
                }

                //dtPageDetaiils = objDivision.LoadAllDivisionDetails();


                dtPageDetaiils = objDivision.LoadAllDivisionDetails();
                //if (dtPageDetaiils.Rows.Count > 0)
                //{
                GrdOffices.DataSource = dtPageDetaiils;
                GrdOffices.DataBind();
            }

            catch (Exception ex)
            {

                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }


        private void PopulateCheckedValues()
        {
            try
            {
                ArrayList userdetails = (ArrayList)ViewState["CHECKED_ITEMS"];

                if (userdetails != null && userdetails.Count > 0)
                {
                    foreach (GridViewRow gvrow in GrdOffices.Rows)
                    {
                        int index = Convert.ToInt32(GrdOffices.DataKeys[gvrow.RowIndex].Value);
                        if (userdetails.Contains(index))
                        {
                            CheckBox myCheckBox = (CheckBox)gvrow.FindControl("cbSelect");
                            myCheckBox.Checked = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        private void PopulateCheckedValuesoffcode()
        {
            try
            {
                ArrayList arrCheckedValues = (ArrayList)ViewState["Existed_Offcode"];
                if (arrCheckedValues != null && arrCheckedValues.Count > 0)
                {
                    foreach (GridViewRow gvrow in GrdOffices.Rows)
                    {
                        int index = Convert.ToInt32(GrdOffices.DataKeys[gvrow.RowIndex].Values[0]);
                        if (arrCheckedValues.Contains(Convert.ToString(index)))
                        {
                            CheckBox myCheckBox = (CheckBox)gvrow.FindControl("cbSelect");
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
        protected void GrdOffices_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                SaveCheckedValues();
                GrdOffices.PageIndex = 0;
                GrdOffices.PageIndex = e.NewPageIndex;
                LoadOffice(objSession.OfficeCode);
                PopulateCheckedValues();

                if (ViewState["Existed_Offcode"] != null && ViewState["CHECKED_ITEMS"] == null)
                {
                    PopulateCheckedValuesoffcode();
                }
                this.mdlPopup.Show();
            }
            catch (Exception ex)
            {

                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void GrdOffices_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {
                    //GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    //TextBox txtdivcode = (TextBox)row.FindControl("txtdivcode");
                    //TextBox txtdivName = (TextBox)row.FindControl("txtdivName");

                    //LoadOffice(txtdivcode.Text.Trim().Replace("'", "''"), txtdivName.Text.Trim().Replace("'", "''"));
                    //LoadOffice(txtdivcode.Text.Trim().Replace("'", "''"), txtdivName.Text.Trim().Replace("'", "''"));

                    //LoadOfficesearch(txtdivcode.Text.Trim().Replace("'", "''"), txtdivName.Text.Trim().Replace("'", "''"));
                    {
                        string sFilter = string.Empty;
                        DataView dv = new DataView();
                        DataTable dt = new DataTable();
                        clsDivision objDivision = new clsDivision();
                        dt = objDivision.LoadAllDivisionDetails();
                        //ViewState["DIVISION"] = dt;

                        GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                        TextBox txtDivCode = (TextBox)row.FindControl("txtdivcode");
                        TextBox txtDivName = (TextBox)row.FindControl("txtdivName");

                        //DataTable dt = (DataTable)ViewState["DIVISION"];
                        dv = dt.DefaultView;

                        if (txtDivCode.Text != "")
                        {
                            sFilter = "DIV_CODE Like '%" + txtDivCode.Text.Replace("'", "'") + "%' AND";
                        }
                        if (txtDivName.Text != "")
                        {
                            sFilter += " DIV_NAME Like '%" + txtDivName.Text.Replace("'", "'") + "%' AND";
                        }

                        if (sFilter.Length > 0)
                        {
                            sFilter = sFilter.Remove(sFilter.Length - 3);
                            GrdOffices.PageIndex = 0;
                            dv.RowFilter = sFilter;
                            if (dv.Count > 0)
                            {
                                GrdOffices.DataSource = dv;
                                ViewState["DIVISION"] = dv.ToTable();
                                GrdOffices.DataBind();

                            }
                            else
                            {
                                ViewState["DIVISION"] = dv.ToTable();
                                ShowEmptyGrid();
                            }
                        }
                        else
                        {
                            LoadOfficesearch(txtDivCode.Text.Trim().Replace("'", "''"), txtDivName.Text.Trim().Replace("'", "''"));
                        }


                    }

                    this.mdlPopup.Show();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                //dt.Columns.Add("DIV_ID");
                dt.Columns.Add("DIV_CODE");
                dt.Columns.Add("DIV_NAME");


                GrdOffices.DataSource = dt;
                GrdOffices.DataBind();

                int iColCount = GrdOffices.Rows[0].Cells.Count;
                GrdOffices.Rows[0].Cells.Clear();
                GrdOffices.Rows[0].Cells.Add(new TableCell());
                GrdOffices.Rows[0].Cells[0].ColumnSpan = iColCount;
                GrdOffices.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowEmptyGrid");

            }
        }
        protected void GrdOffices_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                foreach (GridViewRow Row in GrdOffices.Rows)
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)//except header and footer
                    {
                        TextBox txtOff = new TextBox();
                        CheckBox cbSelect = new CheckBox();
                        ArrayList arroffcode = new ArrayList();

                        cbSelect = (CheckBox)e.Row.FindControl("cbSelect");
                        Label lblOff = new Label();

                        lblOff = (Label)Row.FindControl("lblOffCode");
                    }
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

        private bool ValidateForm()
        {
            bool bValidate = false;
            try
            {


                if (txtRepairName.Text.Trim().Length == 0)
                {
                    txtRepairName.Focus();
                    ShowMsgBox("Please Enter the Name of Agency");
                    return bValidate;
                }
                if (txtRepairAddress.Text.Trim().Length == 0)
                {
                    txtRepairAddress.Focus();
                    ShowMsgBox("Please Enter Valid Register Address");
                    return bValidate;
                }
                //if (txtRepairAddress.Text != "")
                //{
                //    if (!System.Text.RegularExpressions.Regex.IsMatch(txtRepairAddress.Text, "^[0-9 a-z A-Z ]+$")) 
                //    {

                //        txtRepairAddress.Focus();
                //        ShowMsgBox("Please Enter Valid Address ");
                //        return bValidate;
                //    }
                //}

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
                if (txtRepairPhnNo.Text[0] == '0' || txtRepairPhnNo.Text[0] == '1' || txtRepairPhnNo.Text[0] == '2' || txtRepairPhnNo.Text[0] == '3' || txtRepairPhnNo.Text[0] == '4' || txtRepairPhnNo.Text[0] == '5')
                {
                    ShowMsgBox("Please Enter Valid Phone No");
                    txtRepairPhnNo.Focus();
                    return false;
                }

                if (txtRepairEmailId.Text == "")
                {
                    txtRepairEmailId.Focus();
                    ShowMsgBox("Please Enter the EmailId");
                    return bValidate;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(txtRepairEmailId.Text, "^\\s*[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+[a-zA-Z0-9]\\s*$"))
                {
                    ShowMsgBox("Please enter Valid Email (xyz@aaa.com)");
                    return bValidate;
                }

                if (txtOfficeCode.Text.Trim().Length == 0)
                {
                    txtOfficeCode.Focus();
                    ShowMsgBox("Please select The Division code");
                    return bValidate;
                }

                //if (txtFaxNo.Text.Trim() != "")
                //{
                //    if (!System.Text.RegularExpressions.Regex.IsMatch(txtFaxNo.Text, "^[0-9 \\-\\s \\( \\)]*$"))
                //    {
                //        ShowMsgBox("Please Enter Valid Fax No (Eg:865-934-1234)");
                //        return bValidate;
                //    }
                //}

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
            txtRepairEmailId.Text = "";      
            txtRepairerId.Text = "";
            txtRepairAddress.Text = "";
            txtRepairPhnNo.Text = "";
            if (cmdSave.Text != "Update")
            {
                txtRepairName.Text = "";
                txtOfficeCode.Text = "";
                ViewState["Existed_Offcode"] = null;
                ViewState["CHECKED_ITEMS"] = null;
                GrdOffices.AllowPaging = true;
                GrdOffices.PageIndex = 0;
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
                Response.Redirect("", false);
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdNew_Click");
            }
        }
        protected void btnOK_Click1(object sender, EventArgs e)
        {
            try
            {
                ArrayList arrChecked = new ArrayList();

                GrdOffices.AllowPaging = false;
                SaveCheckedValues();
                LoadOffice(objSession.OfficeCode);
                PopulateCheckedValues();

                foreach (GridViewRow Row in GrdOffices.Rows)
                {
                    bool result = ((CheckBox)Row.FindControl("cbSelect")).Checked;

                    if (result == true)
                    {
                        arrChecked.Add(((Label)Row.FindControl("lblOffCode")).Text);
                    }
                }

                GrdOffices.AllowPaging = true;
                SaveCheckedValues();
                LoadOffice(objSession.OfficeCode);
                PopulateCheckedValues();


                string sOfficeCode = string.Empty;

                for (int i = 0; i < arrChecked.Count; i++)
                {
                    sOfficeCode += arrChecked[i];
                    if (sOfficeCode.StartsWith(",") == false)
                    {
                        //sOfficeCode =  sOfficeCode;
                    }
                    if (sOfficeCode.EndsWith(",") == false)
                    {
                        sOfficeCode = sOfficeCode + ",";
                    }
                }

                //txtOfficeCode.Text = strOk;
                if (sOfficeCode.EndsWith(",") == true)
                {
                    sOfficeCode = sOfficeCode.Remove(sOfficeCode.Length - 1);
                }

                txtOfficeCode.Text = sOfficeCode;
                //txtOfficeCode.Enabled = false;
                pnlControls.Visible = false;
            }
            catch (Exception ex)
            {

                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }

        }

        //protected void grdAgency_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    try
        //    {
        //        grdRepairer.PageIndex = e.NewPageIndex;
        //        DataTable dt = (DataTable)ViewState["RepairerMaster"];
        //        grdRepairer.DataSource = SortDataTable(dt as DataTable, true);
        //        grdRepairer.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdAgency_PageIndexChanging");
        //    }
        //}
        //private string GridViewSortDirection
        //{
        //    get { return ViewState["SortDirection"] as string ?? "ASC"; }
        //    set { ViewState["SortDirection"] = value; }


        //}
        //private string GridViewSortExpression
        //{
        //    get { return ViewState["SortExpression"] as string ?? string.Empty; }
        //    set { ViewState["SortExpression"] = value; }
        //}
        //protected DataView SortDataTable(DataTable dataTable, bool isPageIndexChanging)
        //{
        //    if (dataTable != null)
        //    {
        //        DataView dataView = new DataView(dataTable);
        //        if (GridViewSortExpression != string.Empty)
        //        {
        //            if (isPageIndexChanging)
        //            {
        //                dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
        //                ViewState["RepairerMaster"] = dataView.ToTable();

        //            }
        //            else
        //            {
        //                dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
        //                ViewState["RepairerMaster"] = dataView.ToTable();
        //            }
        //        }
        //        return dataView;
        //    }
        //    else
        //    {
        //        return new DataView();
        //    }

        //}
        //private string GetSortDirection()
        //{
        //    switch (GridViewSortDirection)
        //    {
        //        case "ASC":
        //            GridViewSortDirection = "DESC";

        //            break;
        //        case "DESC":
        //            GridViewSortDirection = "ASC";

        //            break;
        //    }


        //    return GridViewSortDirection;
        //}
        //protected void grdRepairer_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    try
        //    {
        //        if (e.CommandName == "upload")
        //        {
        //            GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

        //            string sAgencyId = ((Label)row.FindControl("lblRepairId")).Text;
        //            clsAgency objAgency = new clsAgency();
        //            objAgency.AgencyId = sAgencyId;
        //            objAgency.GetAgencyRepairerDetails(objAgency);

        //            txtRepairerId.Text = objAgency.AgencyId;
        //            txtRepairName.Text = objAgency.AgencyName;
        //            txtRepairPhnNo.Text = objAgency.AgencyPhoneNo;
        //            txtRepairEmailId.Text = objAgency.AgencyEmail;
        //            //txtFaxNo.Text = objAgency.sFax;
        //            txtRepairAddress.Text = objAgency.AgencyAddress;

        //            cmdSave.Text = "Update";
        //        }
        //        if (e.CommandName == "search")
        //        {
        //            string sFilter = string.Empty;
        //            DataView dv = new DataView();

        //            GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
        //            TextBox txtRepairerName = (TextBox)row.FindControl("txtRepairerName");


        //            DataTable dt = (DataTable)ViewState["RepairerMaster"];
        //            dv = dt.DefaultView;

        //            if (txtRepairerName.Text != "")
        //            {
        //                sFilter = "RA_NAME Like '%" + txtRepairerName.Text.Replace("'", "") + "%' AND";
        //            }

        //            if (sFilter.Length > 0)
        //            {
        //                sFilter = sFilter.Remove(sFilter.Length - 3);
        //                grdRepairer.PageIndex = 0;
        //                dv.RowFilter = sFilter;
        //                if (dv.Count > 0)
        //                {
        //                    grdRepairer.DataSource = dv;
        //                    ViewState["RepairerMaster"] = dv.ToTable();
        //                    grdRepairer.DataBind();

        //                }
        //                else
        //                {
        //                    ViewState["RepairerMaster"] = dv.ToTable();
        //                    ShowEmptyGrid();
        //                }
        //            }
        //            else
        //            {
        //                LoadAgencyMasterRepairer();
        //            }

        //        }

        //        if (e.CommandName == "status")
        //        {
        //            GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

        //            string sAgencyId = ((Label)row.FindControl("lblRepairId")).Text;
        //            clsAgency objAgency = new clsAgency();
        //            objAgency.AgencyId = sAgencyId;
        //            string status = ((Label)row.FindControl("lblStatus")).Text;

        //            if (status != "" && status == "A")
        //            {
        //                objAgency.sMasterStatus = "D";
        //                bool updatedStatus = objAgency.ActiveDeactiveMasterAgency(objAgency);
        //                if (updatedStatus)
        //                {
        //                    ShowMsgBox("Agency Deactivated Successfully");
        //                }
        //                else
        //                {
        //                    ShowMsgBox("Something Went Wrong");
        //                }
        //                LoadAgencyMasterRepairer();

        //            }
        //            if (status != "" && status == "D")
        //            {
        //                objAgency.sMasterStatus = "A";
        //                bool updatedStatus = objAgency.ActiveDeactiveMasterAgency(objAgency);
        //                if (updatedStatus)
        //                {
        //                    ShowMsgBox("Agency Activated Successfully");
        //                }
        //                else
        //                {
        //                    ShowMsgBox("Something Went Wrong");
        //                }
        //                LoadAgencyMasterRepairer();
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdRepairer_RowCommand");
        //    }
        //}

        //protected void grdRepairer_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Row.RowType == DataControlRowType.DataRow)
        //        {
        //            Label lblStatus;
        //            lblStatus = (Label)e.Row.FindControl("lblStatus");
        //            ImageButton imgBtnEdit;
        //            //  imgBtnEdit = (ImageButton)e.Row.FindControl("imgBtnEdit");

        //            if (lblStatus.Text == "A")
        //            {
        //                ImageButton imgActive;
        //                imgActive = (ImageButton)e.Row.FindControl("imgActive");
        //                imgActive.Visible = true;
        //                //   imgBtnEdit.Enabled = true;
        //                // imgBtnEdit.ToolTip = "";
        //            }
        //            else
        //            {
        //                ImageButton imgDeActive;
        //                imgDeActive = (ImageButton)e.Row.FindControl("imgDeActive");
        //                imgDeActive.Visible = true;
        //                //imgBtnEdit.Enabled = false;
        //                //imgBtnEdit.ToolTip = "Repairer is DeActivated,You cannot Edit";
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdRepairer_RowDataBound");


        //    }
        //}


        //public void ShowEmptyGrid()
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        DataRow newRow = dt.NewRow();
        //        dt.Rows.Add(newRow);
        //        dt.Columns.Add("RA_ID");
        //        dt.Columns.Add("RA_NAME");
        //        dt.Columns.Add("RA_ADDRESS");
        //        dt.Columns.Add("RA_PHNO");
        //        dt.Columns.Add("RA_MAIL");
        //        dt.Columns.Add("RA_STATUS");


        //        grdRepairer.DataSource = dt;
        //        grdRepairer.DataBind();

        //        int iColCount = grdRepairer.Rows[0].Cells.Count;
        //        grdRepairer.Rows[0].Cells.Clear();
        //        grdRepairer.Rows[0].Cells.Add(new TableCell());
        //        grdRepairer.Rows[0].Cells[0].ColumnSpan = iColCount;
        //        grdRepairer.Rows[0].Cells[0].Text = "No Records Found";

        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowEmptyGrid");

        //    }
        //}


        //protected void grdRepairer_Sorting(object sender, GridViewSortEventArgs e)
        //{
        //    GridViewSortExpression = e.SortExpression;
        //    int pageIndex = grdRepairer.PageIndex;
        //    DataTable dt = (DataTable)ViewState["RepairerMaster"];
        //    string sortingDirection = string.Empty;
        //    if (dt.Rows.Count > 0)
        //    {
        //        grdRepairer.DataSource = SortDataTable(dt as DataTable, false);
        //    }
        //    else
        //    {
        //        grdRepairer.DataSource = dt;
        //    }
        //    grdRepairer.DataBind();
        //    grdRepairer.PageIndex = pageIndex;
        //}
    }
}