using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IIITS.DTLMS.BL;
using System.Web.Services;
using System.IO;
using ClosedXML.Excel;
using System.Configuration;

namespace IIITS.DTLMS.MasterForms
{
    public partial class UserView : System.Web.UI.Page
    {
        string strFormCode = "UserView";
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
                lblMessage.Text = string.Empty;
                lblMsg.Text = string.Empty;
                if (!IsPostBack)
                {
                  //  ClientScript.RegisterHiddenField("isPostBack", "1");

                      //  officeCode.Attributes.Add("style", "display:block");
                    Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME FROM TBLCIRCLE ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle);
                    if (Convert.ToString(ConfigurationSettings.AppSettings["CommonLogin"]).ToUpper().Equals("ON"))
                    {
                        cmdNew.Visible = false;
                    }

                    CheckAccessRights("4");
                    LoadUserDetails();
                    if (Convert.ToString(ConfigurationSettings.AppSettings["CommonLogin"]).ToUpper().Equals("ON"))
                    {
                        grdUser.Columns[7].Visible = false;

                    }
                    cmbCircle_SelectedIndexChanged(sender, e);
                    cmbDivision.SelectedValue = hdfDivision.Value;
                    cmbDivision_SelectedIndexChanged(sender, e);
                    cmbsubdivision.SelectedValue = hdfSubdivision.Value;
                    cmbsubdivision_SelectedIndexChanged(sender, e);
                   
                    cmbSection.SelectedValue = hdfSection.Value;
                    cmbSection_SelectedIndexChanged(sender, e);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load");
            }
        }

        protected void cmb_OfficeIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //if(cmbOfficeType.SelectedValue == "0" || cmbOfficeType.SelectedValue == "1")
                //{
                //    CorporateOffice();
                //}
                if(cmbOfficeType.SelectedIndex>0)
                {
                    CorporateOffice();
                }
                else
                {
                    LoadUserDetails();
                }
                
            }
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmb_OfficeIndexChanged");
            }

           }


        protected void cmbCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCircle.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION  WHERE DIV_CICLE_CODE='" + cmbCircle.SelectedValue + "' ORDER BY DIV_CODE", "--Select--", cmbDivision);
                    clsUser objUser = new clsUser();
                    DataTable dtUserDetails = new DataTable();
                    objUser.sOffCode = cmbCircle.SelectedValue;
                    dtUserDetails = objUser.LoadUserGrid(objUser);
                    if (dtUserDetails.Rows.Count == 0)
                    {
                        ShowEmptyGrid();
                        ViewState["USER"] = dtUserDetails;
                    }
                    else
                    {
                        grdUser.DataSource = dtUserDetails;
                        grdUser.DataBind();
                        ViewState["USER"] = dtUserDetails;
                    }
                    cmbOfficeType.SelectedIndex = 2;
                }
                else
                {

                    clsUser objUser = new clsUser();
                    DataTable dtUserDetails = new DataTable();
                    objUser.sOffCode = cmbCircle.SelectedValue;
                    dtUserDetails = objUser.LoadUserGrid(objUser);
                    if (dtUserDetails.Rows.Count == 0)
                    {
                        ShowEmptyGrid();
                        ViewState["USER"] = dtUserDetails;
                    }
                    else
                    {
                        grdUser.DataSource = dtUserDetails;
                        grdUser.DataBind();
                        ViewState["USER"] = dtUserDetails;
                    }

                    cmbDivision.Items.Clear();
                    cmbsubdivision.Items.Clear();
                    cmbSection.Items.Clear();

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbCircle_SelectedIndexChanged");
            }

        }

        protected void cmbDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDivision.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT SD_SUBDIV_CODE, SD_SUBDIV_NAME FROM TBLSUBDIVMAST  WHERE SD_DIV_CODE='" + cmbDivision.SelectedValue + "' ORDER BY SD_SUBDIV_CODE", "--Select--", cmbsubdivision);
                    clsUser objUser = new clsUser();
                    DataTable dtUserDetails = new DataTable();
                    objUser.sOffCode = cmbDivision.SelectedValue;
                    dtUserDetails = objUser.LoadUserGrid(objUser);
                    if (dtUserDetails.Rows.Count == 0)
                    {
                        ShowEmptyGrid();
                        ViewState["USER"] = dtUserDetails;
                    }
                    else
                    {
                        grdUser.DataSource = dtUserDetails;
                        grdUser.DataBind();
                        ViewState["USER"] = dtUserDetails;
                    }
                    cmbOfficeType.SelectedIndex = 2;
                }
                else
                {
                    clsUser objUser = new clsUser();
                    DataTable dtUserDetails = new DataTable();
                    objUser.sOffCode = cmbCircle.SelectedValue;
                    dtUserDetails = objUser.LoadUserGrid(objUser);
                    if (dtUserDetails.Rows.Count == 0)
                    {
                        ShowEmptyGrid();
                        ViewState["USER"] = dtUserDetails;
                    }
                    else
                    {
                        grdUser.DataSource = dtUserDetails;
                        grdUser.DataBind();
                        ViewState["USER"] = dtUserDetails;
                    }

                    cmbsubdivision.Items.Clear();
                    cmbSection.Items.Clear();

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbCircle_SelectedIndexChanged");
            }
        }

        protected void cmbsubdivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbsubdivision.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT OM_CODE, OM_NAME FROM TBLOMSECMAST WHERE OM_SUBDIV_CODE = '" + cmbsubdivision.SelectedValue + "' ORDER BY OM_CODE", "--Select--", cmbSection);
                    clsUser objUser = new clsUser();
                    DataTable dtUserDetails = new DataTable();
                    objUser.sOffCode = cmbsubdivision.SelectedValue;
                    dtUserDetails = objUser.LoadUserGrid(objUser);
                    if (dtUserDetails.Rows.Count == 0)
                    {
                        ShowEmptyGrid();
                        ViewState["USER"] = dtUserDetails;
                    }
                    else
                    {
                        grdUser.DataSource = dtUserDetails;
                        grdUser.DataBind();
                        ViewState["USER"] = dtUserDetails;
                    }
                    cmbOfficeType.SelectedIndex = 2;
                }
                else
                {

                    clsUser objUser = new clsUser();
                    DataTable dtUserDetails = new DataTable();
                    objUser.sOffCode = cmbDivision.SelectedValue==""?null:cmbDivision.SelectedValue;

                    dtUserDetails = objUser.LoadUserGrid(objUser);
                    if (dtUserDetails.Rows.Count == 0)
                    {
                        ShowEmptyGrid();
                        ViewState["USER"] = dtUserDetails;
                    }
                    else
                    {
                        grdUser.DataSource = dtUserDetails;
                        grdUser.DataBind();
                        ViewState["USER"] = dtUserDetails;
                    }

                    cmbSection.Items.Clear();

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbsubdivision_SelectedIndexChanged");
            }
        }

        protected void cmbSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSection.SelectedIndex > 0)
                {
                    clsUser objUser = new clsUser();
                    DataTable dtUserDetails = new DataTable();
                    objUser.sOffCode = cmbSection.SelectedValue;
                    dtUserDetails = objUser.LoadUserGrid(objUser);
                    if (dtUserDetails.Rows.Count == 0)
                    {
                        ShowEmptyGrid();
                        ViewState["USER"] = dtUserDetails;
                    }
                    else
                    {
                        grdUser.DataSource = dtUserDetails;
                        grdUser.DataBind();
                        ViewState["USER"] = dtUserDetails;
                    }
                    cmbOfficeType.SelectedIndex = 2;
                }
                else
                {
                    clsUser objUser = new clsUser();
                    DataTable dtUserDetails = new DataTable();
                    objUser.sOffCode = cmbsubdivision.SelectedValue==""?null:cmbsubdivision.SelectedValue;
                    dtUserDetails = objUser.LoadUserGrid(objUser);
                    if (dtUserDetails.Rows.Count == 0)
                    {
                        ShowEmptyGrid();
                        ViewState["USER"] = dtUserDetails;
                    }
                    else
                    {
                        grdUser.DataSource = dtUserDetails;
                        grdUser.DataBind();
                        ViewState["USER"] = dtUserDetails;
                    }
                  //  cmbSection.Items.Clear();

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbSection_SelectedIndexChanged");
            }
        }


        public void LoadUserDetails()
        {
            try
            {
                clsUser objUser = new clsUser();
                DataTable dtUserDetails = new DataTable();
                dtUserDetails = objUser.LoadUserGrid(objUser);
                if (dtUserDetails.Rows.Count == 0)
                {
                    ShowEmptyGrid();
                    ViewState["USER"] = dtUserDetails;

                }
                else
                {
                    grdUser.DataSource = dtUserDetails;
                    grdUser.DataBind();
                    ViewState["USER"] = dtUserDetails;

                }

            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadUserDetails");
            }
        }

        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("US_ID");
                dt.Columns.Add("US_FULL_NAME");
                dt.Columns.Add("US_EMAIL");
                dt.Columns.Add("US_MOBILE_NO");
                dt.Columns.Add("RO_NAME");
                dt.Columns.Add("US_DESG_ID");
                dt.Columns.Add("OFF_NAME");

                dt.Columns.Add("US_STATUS1");

                grdUser.DataSource = dt;
                grdUser.DataBind();

                int iColCount = grdUser.Rows[0].Cells.Count;
                grdUser.Rows[0].Cells.Clear();
                grdUser.Rows[0].Cells.Add(new TableCell());
                grdUser.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdUser.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowEmptyGrid");

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
                Response.Redirect("UserCreate.aspx", false);

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdNew_Click");
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
        protected void grdUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdUser.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["USER"];

                grdUser.DataSource = SortDataTable(dt as DataTable, true);
                grdUser.DataBind();
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdUser_PageIndexChanging");

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
                        ViewState["USER"] = dataView.ToTable();


                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["USER"] = dataView.ToTable();

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

        protected void grdUser_Sorting(object sender, GridViewSortEventArgs e)
        {
     

            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdUser.PageIndex;
            DataTable dt = (DataTable)ViewState["USER"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {
                grdUser.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdUser.DataSource = dt;
            }
            grdUser.DataBind();
            grdUser.PageIndex = pageIndex;
        }


        protected void grdUser_RowCommand(object sender, GridViewCommandEventArgs e)
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
                    string strUserId = ((Label)row.FindControl("lblUserId")).Text;
                    strUserId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(strUserId));
                    Response.Redirect("UserCreate.aspx?QryUserId=" + strUserId + "", false);

                }
                if (e.CommandName == "status")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    string sUserId = ((Label)row.FindControl("lblUserId")).Text;
                    string sStatus = ((Label)row.FindControl("lblStatus")).Text;

                    ImageButton imgDeactive = new ImageButton();
                    ImageButton imgActive = new ImageButton();
                    imgDeactive = (ImageButton)row.FindControl("imgDeactive");
                    imgActive = (ImageButton)row.FindControl("imgActive");

                    clsUser objUser = new clsUser();
                    objUser.lSlNo = sUserId;
                    ViewState["ID"] = sUserId;
                    ViewState["US_STATUS1"] = sStatus;

                    txtEffectFrom.Text = string.Empty;
                    txtReason.Text = string.Empty;

                    this.mdlPopup.Show();
                }


                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtSearchName = (TextBox)row.FindControl("txtsFullName");
                    TextBox txtSearchDesignation = (TextBox)row.FindControl("txtsDesignation");
                    TextBox txtMobileNumber = (TextBox)row.FindControl("txtMobileNumber");
                    DataTable dt = (DataTable)ViewState["USER"];
                    dv = dt.DefaultView;

                    if (txtSearchName.Text != "")
                    {
                        sFilter = "US_FULL_NAME Like '%" + txtSearchName.Text.Replace("'", "'") + "%' AND";
                    }
                    if (txtMobileNumber.Text != "")
                    {
                        sFilter = "US_MOBILE_NO = '" + txtMobileNumber.Text + "' AND";
                    }

                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdUser.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdUser.DataSource = dv;
                            ViewState["USER"] = dv.ToTable();
                            grdUser.DataBind();
                        }
                        else
                        {
                            ViewState["USER"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        LoadUserDetails();
                    }


                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdUser_RowCommand");
            }
        }

        protected void grdUser_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblStatus;
                    lblStatus = (Label)e.Row.FindControl("lblStatus");
                    ImageButton imgBtnEdit;
                    ImageButton imgDeActive;
                    ImageButton imgActive;
                    imgBtnEdit = (ImageButton)e.Row.FindControl("imgBtnEdit");

                    if (lblStatus.Text == "A")
                    {

                        imgActive = (ImageButton)e.Row.FindControl("imgActive");
                        imgDeActive = (ImageButton)e.Row.FindControl("imgDeActive");
                        imgActive.Visible = true;
                        imgDeActive.Visible = false;
                        imgBtnEdit.Enabled = true;
                        imgBtnEdit.ToolTip = "";
                    }
                    else
                    {

                        imgDeActive = (ImageButton)e.Row.FindControl("imgDeActive");
                        imgActive = (ImageButton)e.Row.FindControl("imgActive");
                        imgDeActive.Visible = true;
                        imgActive.Visible = false;
                        imgBtnEdit.Enabled = false;
                        imgBtnEdit.ToolTip = "User is DeActivated,You Cannot Edit";
                    }
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdUser_RowDataBound");


            }
        }

        protected void cmdSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateEnableDisable() == true)
                {
                    clsUser objUser = new clsUser();
                    objUser.sReason = txtReason.Text;
                    objUser.sEffectFrom = txtEffectFrom.Text;
                    objUser.lSlNo = Convert.ToString(ViewState["ID"]);
                    objUser.sStatus = Convert.ToString(ViewState["US_STATUS1"]);
                    ImageButton imgDeactive = new ImageButton();
                    ImageButton imgActive = new ImageButton();
                    if (objUser.sStatus == "A")
                    {
                        objUser.sStatus = "D";
                        bool bResult = objUser.ActiveDeactiveUser(objUser);
                        if (bResult == true)
                        {
                            imgDeactive.Visible = true;
                            imgActive.Visible = false;
                            ShowMsgBox("User Deactivated Successfully");
                            LoadUserDetails();
                            txtEffectFrom.Text = "";
                            txtReason.Text = "";

                        }
                    }
                    else
                    {
                        objUser.sStatus = "A";
                        bool bResult = objUser.ActiveDeactiveUser(objUser);
                        if (bResult == true)
                        {
                            imgDeactive.Visible = false;
                            imgActive.Visible = true;
                            ShowMsgBox("User Activated Successfully");
                            LoadUserDetails();
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
                if (txtEffectFrom.Text.Trim() == "")
                {
                    lblMsg.Text = "Enter Effect From";
                    txtEffectFrom.Focus();
                    mdlPopup.Show();
                    return bValidate;
                }
                if (txtReason.Text.Trim() == "")
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


        #region Access Rights

        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "UserCreate";
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

       


        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                LoadUserDetails();
                cmbCircle.SelectedIndex = 0;
                cmbDivision.Items.Clear();
                cmbsubdivision.Items.Clear();
                cmbSection.Items.Clear();
                cmbOfficeType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdReset_Click");
            }
        }

       

        public SortDirection direction
        {
            get
            {
                if (ViewState["directionState"] == null)
                {
                    ViewState["directionState"] = SortDirection.Ascending;
                }
                return (SortDirection)ViewState["directionState"];
            }
            set
            {
                ViewState["directionState"] = value;
            }
        }

        //[WebMethod]
        //public static void btnCorporateOffice_click()
        //{
        //    UserView obj = new UserView();
        //    obj.CorporateOffice();


        //}

        public  void CorporateOffice()
        {
            try
            {
                clsUser objUser = new clsUser();
                DataTable dtUserDetails = new DataTable();
                objUser.sOffCode = "";
                objUser.dsgnt=cmbOfficeType.SelectedIndex;
                dtUserDetails = objUser.LoadUserGrid(objUser);
                if (dtUserDetails.Rows.Count == 0)
                {
                    ShowEmptyGrid();
                    ViewState["USER"] = null;
                }
                else
                {
                    grdUser.DataSource = dtUserDetails;
                    grdUser.DataBind();
                    ViewState["USER"] = dtUserDetails;
                    cmbCircle.SelectedIndex = 0;
                    cmbDivision.Items.Clear();
                    cmbsubdivision.Items.Clear();
                    cmbSection.Items.Clear();
                }

            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "CorporateOffice");
            }

        }


        protected void Export_click(object sender, EventArgs e)
        {
            //clsUser objUser = new clsUser();
            //DataTable dtUserDetails1 = new DataTable();
            //objUser.sOffCode = cmbCircle.SelectedValue;
            //objUser.sOffCode = cmbDivision.SelectedValue;
            //objUser.sOffCode = cmbsubdivision.SelectedValue;           
            //dtUserDetails1 = objUser.LoadUserGrid(objUser);

            

            //ViewState["USER"] = dtUserDetails1;
            //DataTable dtUserDetails = (DataTable)ViewState["USER"];


            DataTable dtUserDetails = (DataTable)ViewState["USER"];

            if (dtUserDetails.Rows.Count > 0)
            {

                dtUserDetails.Columns["US_FULL_NAME"].ColumnName = "USER NAME";
                dtUserDetails.Columns["US_DESG_ID"].ColumnName = "DESIGNATION ";
                dtUserDetails.Columns["US_EMAIL"].ColumnName = "EMAIL ID";
                dtUserDetails.Columns["US_MOBILE_NO"].ColumnName = "MOBILE NO";
                dtUserDetails.Columns["RO_NAME"].ColumnName = "ROLE NAME";
                dtUserDetails.Columns["OFF_NAME"].ColumnName = "LOCATION";

                dtUserDetails.Columns["ROLE NAME"].SetOrdinal(2);

                //List<int> listtoRemove = new List<int> { dtUserDetails.Columns["US_ID"].Ordinal, dtUserDetails.Columns["US_STATUS"].Ordinal,
                //    dtUserDetails.Columns["US_STATUS1"].Ordinal };
                List<string> listtoRemove = new List<string> { "US_ID", "US_STATUS",
                "US_STATUS1" };
                string filename = "UsersDetails" + DateTime.Now + ".xls";
                string pagetitle = "Users Details";

                Genaral.getexcel(dtUserDetails, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No Records Found");
            }

          
        }
    }
}
    
