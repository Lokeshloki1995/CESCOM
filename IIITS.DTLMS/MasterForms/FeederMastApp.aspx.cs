using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Text.RegularExpressions;
using System.Collections;


namespace IIITS.DTLMS.MasterForms
{
    public partial class FeederMastApp : System.Web.UI.Page
    {
      
        string strUserLogged = string.Empty;
        string Officecode = string.Empty;
        ArrayList userdetails = new ArrayList();
        string sFormCode = "FeederMast";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/InternalLogin.aspx", false);
                }
               
                objSession = (clsSession)Session["clsSession"];
                lblErrormsg.Text = string.Empty;
                txtOfficeCode.Enabled = false;
                if (!IsPostBack)
                {
                    if (objSession.UserType != "4")
                    {
                        Response.Redirect("~/UserRestrict.aspx", false);
                    }

                    Genaral.Load_Combo("SELECT ST_ID,ST_NAME from TBLSTATION ORDER BY ST_ID ", "--Select--", cmbStation);
                    Genaral.Load_Combo("SELECT FC_ID,FC_NAME from TBLFEEDERCATEGORY ORDER BY FC_ID asc", "--Select--", cmbCat);              
                    Genaral.Load_Combo("SELECT FT_ID,FT_NAME FROM TBLFDRTYPE", "--Select--", cmbType);


                    if (Request.QueryString["FeederId"] != null && Request.QueryString["FeederId"].ToString() != "")
                    {
                        txtFeederId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["FeederId"]));
                        GetFeederDetails();
                        cmbStation_SelectedIndexChanged(sender, e);
                        cmbBank.SelectedValue = hdfBank.Value;
                        cmbBank_SelectedIndexChanged(sender, e);
                        cmbbus.SelectedValue = hdfBus.Value;
                    }

    
                  
                }
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, sFormCode, "Page_Load");
            }
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                string[] Arrmsg = new string[2];
                

                //Check AccessRights
                //bool bAccResult;
                //if (cmdSave.Text == "Update")
                //{
                //    bAccResult = CheckAccessRights("3");
                //}
                //else
                //{
                //    bAccResult = CheckAccessRights("2");
                //}

                //if (bAccResult == false)
                //{
                //    return;
                //}


                if (cmbStation.SelectedIndex == 0)
                {
                    ShowMsgBox("Select the Station");
                    cmbStation.Focus();
                    return;
                }
                if (cmbBank.SelectedIndex == 0)
                {
                   ShowMsgBox("Select the Bank");
                    cmbBank.Focus();
                    return;
                }

                if (cmbbus.SelectedIndex == 0)
                {
                   ShowMsgBox("Select the Bus");
                    cmbbus.Focus();
                    return;
                }

                if (txtOfficeCode.Text.Trim().Length == 0)
                {
                   ShowMsgBox("Office Code length must not be empty");
                    txtOfficeCode.Focus();
                    return;
                }

                if (txtFeederCode.Text.Trim().Length != 4)
                {
                   ShowMsgBox("Feeder Code Must Be 4 Digit Number");
                    txtFeederCode.Focus();
                    return;
                }
                if (txtFeederName.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Enter the Feeder Name");
                    txtFeederName.Focus();
                    return;
                }   

             
                if (cmbCat.SelectedIndex <= 0)
                {
                    ShowMsgBox("Select the Feeder Type");
                    cmbCat.Focus();
                    return;
                }
                if (cmbInt.SelectedIndex == 0)
                {
                    ShowMsgBox("Select Interflow");
                    cmbInt.Focus();
                    return;
                }
                if (txtDTC.Text == "")
                {
                    ShowMsgBox("Enter Valid DTC Capacity");
                    txtDTC.Focus();
                    return;
                }

                string strFeederType = string.Empty;
                string StrChangeDate = string.Empty;
               
                clsFeederMast objFeederMaster = new clsFeederMast();
                objFeederMaster.FeederCode = Convert.ToString(txtFeederCode.Text.Trim().ToUpper());
                objFeederMaster.FeederName = txtFeederName.Text.Trim().ToUpper();
                objFeederMaster.Stationid = Convert.ToInt64(cmbStation.SelectedValue);
                objFeederMaster.BankId = Convert.ToInt64(cmbBank.SelectedValue);
                objFeederMaster.BusId = Convert.ToInt64(cmbbus.SelectedValue);
                objFeederMaster.FeederType = cmbType.SelectedValue.Trim().ToUpper();
                objFeederMaster.FeederCategory= cmbCat.SelectedValue.Trim().ToUpper();
                objFeederMaster.FeederInterflow=cmbInt.SelectedValue.Trim().ToUpper();
                objFeederMaster.FeederDCC = txtDTC.Text.Trim().ToUpper();


                string strOfficeCode = txtOfficeCode.Text;

                objFeederMaster.OfficeCode = txtOfficeCode.Text;

                objFeederMaster.UserLogged = objSession.UserId;
                objFeederMaster.FeederID = Convert.ToInt64(txtFeederId.Text);

                if (txtFeederId.Text.Trim() == "0")
                {
                    
                    objFeederMaster.IsSave = true;

                    Arrmsg = objFeederMaster.FeederMaster(objFeederMaster);

                    if (Arrmsg[1].ToString() == "0")
                    {
                        bool bResult = objFeederMaster.SyncFeederDetailstoApp(objFeederMaster);
                        if (bResult == false)
                        {
                            ShowMsgBox("Error Occurred While Syncing in APP DB");
                            return;
                        }
                        else
                        {
                            ShowMsgBox("Feeder Details Saved Successfully");
                        }
                        Reset();
                    }
                    if (Arrmsg[1].ToString() == "4")
                    {
                        ShowMsgBox(Arrmsg[0].ToString());
                        mdlPopup.Hide();
                    }
                                   
                }
                else
                {
                    objFeederMaster.IsSave = false;
                    Arrmsg = objFeederMaster.FeederMaster(objFeederMaster);
                    if (Arrmsg[1].ToString() == "0")
                    {
                        bool bResult = objFeederMaster.SyncFeederDetailstoApp(objFeederMaster);
                        if (bResult == false)
                        {
                            ShowMsgBox("Error Occurred While Syncing in APP DB");
                            return;
                        }
                        else
                        {
                            ShowMsgBox("Feeder Details Saved Successfully");
                        }
                        Reset();
                    }
                    if (Arrmsg[1].ToString() == "4")
                    {
                        ShowMsgBox(Arrmsg[0].ToString());
                    }
                }
            }
            catch(Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, sFormCode, "cmdSave_Click");
               
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
                clsException.LogError(ex.StackTrace,ex.Message, sFormCode, "ShowMsgBox");
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
                clsException.LogError(ex.StackTrace,ex.Message, sFormCode, "PopulateCheckedValues");
            }
        }


        //This method is used to save the checkedstate of values
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
               
                clsException.LogError(ex.StackTrace,ex.Message, sFormCode, "SaveCheckedValues");
            }

        }


        public void LoadOffice(string sOfficeCode = "", string sOffName = "")
        {
            try
            {
                DataTable dtPageDetaiils = new DataTable();
                clsFeederMast objStation = new clsFeederMast();
                objStation.OfficeCode = sOfficeCode;
                objStation.OfficeName = sOffName;
                dtPageDetaiils = objStation.LoadOfficeDet(objStation);
                //if (dtPageDetaiils.Rows.Count > 0)
                //{
                GrdOffices.DataSource = dtPageDetaiils;
                GrdOffices.DataBind();
            }

            catch (Exception ex)
            {

                clsException.LogError(ex.StackTrace, ex.Message, sFormCode, "LoadOffice");
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
                this.mdlPopup.Show();
            }
            catch (Exception ex)
            {
               
                clsException.LogError(ex.StackTrace,ex.Message, sFormCode, "GrdOffices_PageIndexChanging");
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
               
                clsException.LogError(ex.StackTrace,ex.Message, sFormCode, "GrdOffices_RowDataBound");
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
                txtOfficeCode.Enabled = false;
            }
            catch (Exception ex)
            {
                
                clsException.LogError(ex.StackTrace,ex.Message, sFormCode, "btnOK_Click1");
            }

        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            //statement used to hide the modal pop up.
            this.mdlPopup.Hide();
            
        }


        protected void btnPopByID_Click(object sender, EventArgs e)
        {
            try
            {
                //CustOledbConnection objCon = new CustOledbConnection(Constants.Password);
                //string strOfficeID = txtOffID.Text.Trim();
                //txtOffID.Enabled = false;

            }
            catch (Exception ex)
            {

            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.mdlPopup.Show();               
                //userdetails.Clear();     

                if (ViewState["CHECKED_ITEMS"] != null)
                {
                    SaveCheckedValues();
                    LoadOffice(objSession.OfficeCode);
                    PopulateCheckedValues();
                }
                else
                {
                    LoadOffice(objSession.OfficeCode);
                }
                
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, sFormCode, "btnSearch_Click");
            }
        }

       

        public void Reset()
        {
            try
            {
                cmbStation.SelectedIndex = 0;
                txtOfficeCode.Text = string.Empty;                
                txtFeederCode.Text = string.Empty;
                txtFeederName.Text = string.Empty;
                cmbCat.SelectedIndex = 0;
                if (cmbBank.SelectedIndex > 0)
                {
                    cmbBank.SelectedIndex = 0;
                }
                if (cmbbus.SelectedIndex > 0)
                {
                    cmbbus.SelectedIndex = 0;
                }
              //  this.mdlPopup.Show();
                txtOfficeCode.Text = "";
                userdetails.Clear();
                
                btnSearch.Enabled = true;
                cmdSave.Text = "Save";
                cmbType.SelectedIndex = 0;
                cmbCat.SelectedIndex = 0;
                cmbInt.SelectedIndex = 0;
                txtDTC.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, sFormCode, "Reset");
            }
        }

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        public void GetFeederDetails()
        {
            try
            {
                clsFeederMast objFeeder = new clsFeederMast();
                ArrayList arrOffCode = new ArrayList();
                ArrayList arrOffCodeValue = new ArrayList();

                DataTable DtFeederDet = objFeeder.GetFeederDetails(txtFeederId.Text);
                if (DtFeederDet.Rows.Count > 0)
                {
                    txtOfficeCode.Text = Convert.ToString(DtFeederDet.Rows[0]["OFFCODE"]);
                    txtFeederCode.Text = Convert.ToString(DtFeederDet.Rows[0]["FD_FEEDER_CODE"]);
                    txtFeederName.Text = Convert.ToString(DtFeederDet.Rows[0]["FD_FEEDER_NAME"]);
                    cmbInt.SelectedValue = Convert.ToString(DtFeederDet.Rows[0]["FD_IS_INTERFLOW"]);
                    txtDTC.Text = Convert.ToString(DtFeederDet.Rows[0]["FD_DTC_CAPACITY"]);
                    cmbStation.SelectedValue = Convert.ToString(DtFeederDet.Rows[0]["ST_ID"]);
                    cmbCat.SelectedValue = Convert.ToString(DtFeederDet.Rows[0]["FC_ID"]);
                    hdfBank.Value = Convert.ToString(DtFeederDet.Rows[0]["BN_ID"]);
                    hdfBus.Value = Convert.ToString(DtFeederDet.Rows[0]["BS_ID"]);
                    cmbType.SelectedValue = Convert.ToString(DtFeederDet.Rows[0]["FC_FT_ID"]);
                    
               

                    if (txtOfficeCode.Text.StartsWith(";"))
                    {
                        txtOfficeCode.Text = txtOfficeCode.Text.Substring(1, txtOfficeCode.Text.Length - 1);
                    }
                    if (txtOfficeCode.Text.EndsWith(";"))
                    {
                        txtOfficeCode.Text = txtOfficeCode.Text.Substring(0, txtOfficeCode.Text.Length - 1);
                    }

                    txtOfficeCode.Text = txtOfficeCode.Text.Replace(";", ",");

                    arrOffCode.AddRange(txtOfficeCode.Text.Split(','));


                    for (int i = 0; i < arrOffCode.Count; i++)
                    {
                        arrOffCodeValue.Add(Convert.ToInt32(arrOffCode[i]));
                    }

                    ViewState["CHECKED_ITEMS"] = arrOffCodeValue;

                    LoadOffice();
                    PopulateCheckedValues();
                }


                cmdSave.Text = "Update";
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, sFormCode, "GetFeederDetails");
            }
        }

        protected void cmbStation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbStation.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT BN_ID,BN_NAME FROM TBLBANK WHERE BN_ST_ID='" + cmbStation.SelectedValue + "'", "--Select--", cmbBank);
                }
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, sFormCode, "cmbStation_SelectedIndexChanged");
            }
        }

        protected void cmbBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbBank.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT BS_ID,BS_NAME FROM TBLBUS WHERE BS_BN_ID='" + cmbBank.SelectedValue + "'", "--Select--", cmbbus);
                }
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, sFormCode, "cmbBank_SelectedIndexChanged");
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

        protected void GrdOffices_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "search")
                {

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtOffCode = (TextBox)row.FindControl("txtOffCode");
                    TextBox txtOffName = (TextBox)row.FindControl("txtOffName");

                    LoadOffice(txtOffCode.Text.Trim().Replace("'", "''"), txtOffName.Text.Trim().Replace("'", "''"));

                    this.mdlPopup.Show();
                    
                }
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, sFormCode, "cmbBank_SelectedIndexChanged");
            }
        }
        
    }
}