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
using System.IO;

namespace IIITS.DTLMS.Internal
{
    public partial class StoreEnumeration : System.Web.UI.Page
    {
        string strFormCode = "StoreEnumeration";
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
                lblmessage.Text = string.Empty;

                RetainImageOnPostback();

                if (!IsPostBack)
                {

                    Genaral.Load_Combo("SELECT TM_ID, TM_NAME FROM TBLTRANSMAKES ORDER BY TM_ID", "--Select--", cmbMake);
                    Genaral.Load_Combo("SELECT MD_NAME, MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'C'", "--Select--", cmbCapacity);
                    Genaral.Load_Combo("SELECT IU_ID, IU_FULLNAME FROM TBLINTERNALUSERS WHERE IU_USERTYPE = 1", "--Select--", cmboperator1);

                    Genaral.Load_Combo("SELECT MD_ID, MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'TCL' and MD_ID <> 2 ORDER BY MD_NAME", "--Select--", cmbLocationType);
                    Genaral.Load_Combo("SELECT MD_ID, MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'TCT' ORDER BY MD_NAME", "--Select--", cmbTranstype);
                    Genaral.Load_Combo("SELECT MD_ID, MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'SR'", "--Select--", cmbRating);

                    if (Request.QueryString["QryEnumId"] != null && Request.QueryString["QryEnumId"].ToString() != "")
                    {
                        txtEnumDetailsId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["QryEnumId"]));
                        txtStatus.Text  = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["Status"]));

                        RestrictUpdate(txtStatus.Text);

                        GetStoreEnumeration(txtEnumDetailsId.Text);
                      
                        cmboperator1_SelectedIndexChanged(sender, e);
                        cmboperator2.SelectedValue = hdfOperaator2.Value;
                        cmbLocationType_SelectedIndexChanged(sender, e);
                        cmbLocationName.SelectedValue = hdfLocationName.Value;
                        txtAddress.Text = txtAddress1.Text;

                        if (cmbRating.SelectedValue == "1")
                        {
                            cmbRating_SelectedIndexChanged(sender, e);
                            cmbStarRated.SelectedValue = hdfStarRate.Value;
                        }

                       

                        if (cmbMake.SelectedValue == "1")
                        {
                            string sTCSlno = txtTcslno.Text;
                            cmbMake_SelectedIndexChanged(sender, e);
                            txtTcslno.Text = sTCSlno;
                        }
                    }
                    txtwelddate.Attributes.Add("onblur", "return ValidateDate(" + txtwelddate.ClientID + ");");
                    //txtManufactureDate.Attributes.Add("onblur", "return ValidateDate(" + txtManufactureDate.ClientID + ");");

                }

                //RetainImageinPostback();
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "Page_Load");
            }
        }

        protected void GetStoreEnumeration(string strRecordId)
        {
            try
            {
                clsStoreEnumeration objstore = new clsStoreEnumeration();

                objstore.sEnumDetailsId = strRecordId;

                objstore.GetStoreEnumerationDetails(objstore);
                             
                cmbLocationType.SelectedValue = objstore.sLoctype;
                txtAddress1.Text = objstore.sLocAdd;
                txtwelddate.Text = objstore.sWeldDate;
                cmboperator1.SelectedValue = objstore.sOperator1;

                txtTcCode.Text = objstore.sTcCode;
                txtTcslno.Text = objstore.sTCSlno;
                txtManufactureDate.Text = objstore.sTCManfDate;               
                hdfOperaator2.Value = objstore.sOperator2;
                hdfLocationName.Value = objstore.sLocName + "~" + objstore.sDivCode;
                txtAddress.Text = objstore.sLocAdd;
                if (objstore.sTCCapacity != "")
                {
                    cmbCapacity.SelectedValue = objstore.sTCCapacity;
                }
                cmbMake.SelectedValue = objstore.sTCMake;
                cmbTranstype.SelectedValue = objstore.sTCType;

                txtTankCapacity.Text = objstore.sTankCapacity;
                txtWeight.Text = objstore.sTCWeight;

                cmbRating.SelectedValue = objstore.sRating;
                hdfStarRate.Value = objstore.sStarRate;

                txtSSPlatePath.Text = objstore.sSSPlatePhotoPath;
                txtNamePlatePhotoPath.Text = objstore.sNamePlatePhotoPath;

                ShowUploadedImages();

                cmdSave.Text = "Update";

            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadStoreEnum");
            }
        }


        public void Reset()
        {
            try
            {                
                txtwelddate.Text = string.Empty;
                txtTcCode.Text = string.Empty;
                txtTcslno.Text = string.Empty;
                txtManufactureDate.Text = string.Empty;
            
                cmbCapacity.SelectedIndex = 0;
                cmbMake.SelectedIndex = 0;
                cmbTranstype.SelectedIndex = 0;
                cmboperator1.SelectedIndex = 0;
                if (cmboperator2.SelectedIndex > 0)
                {
                    cmboperator2.SelectedIndex = 0;
                }
                if (cmbLocationName.SelectedIndex > 0)
                {
                    cmbLocationName.SelectedIndex = 0;
                }
                cmbLocationType.SelectedIndex = 0;
                txtAddress.Text = string.Empty;
                txtTcslno.Enabled = true;

                cmbRating.SelectedIndex = 0;
                cmbStarRated.Items.Clear();
                dvStar.Style.Add("display", "none");
                txtWeight.Text = string.Empty;
                txtTankCapacity.Text = string.Empty;

                cmdSave.Text = "Save";

                dvSSPlate.Style.Add("display", "none");
                dvNamePlate.Style.Add("display", "none");

            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "Reset");
            }
        }

        private void ShowMsgBox(string sMsg)
        {
            try
            {
                String sShowMsg = string.Empty;
                sShowMsg = "<script language=javascript> alert ('" + sMsg + "')</script>";
                this.Page.RegisterStartupScript("Msg", sShowMsg);
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ShowMsgBox");
            }
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                string[] Arr = new string[2];
                clsStoreEnumeration objEnumeration = new clsStoreEnumeration();
                if (ValidateForm() == true)
                {
                    objEnumeration.sEnumDetailsId = txtEnumDetailsId.Text;
                    objEnumeration.sLoctype = cmbLocationType.SelectedValue;
                    objEnumeration.sLocName = cmbLocationName.SelectedValue.Split('~').GetValue(0).ToString();
                    objEnumeration.sLocAdd = txtAddress.Text.Trim().Replace("'", ""); ;
                    objEnumeration.sDivCode = cmbLocationName.SelectedValue.Split('~').GetValue(1).ToString();
                    objEnumeration.sOperator1 = cmboperator1.SelectedValue;
                    objEnumeration.sOperator2 = cmboperator2.SelectedValue;
                    objEnumeration.sWeldDate = txtwelddate.Text;
                    objEnumeration.sTcCode = txtTcCode.Text;
                    objEnumeration.sTCMake = cmbMake.SelectedValue;
                    if (cmbCapacity.SelectedIndex > 0)
                    {
                        objEnumeration.sTCCapacity = cmbCapacity.SelectedValue;
                    }
                    objEnumeration.sTCType = cmbTranstype.SelectedValue;
                    objEnumeration.sTCSlno = txtTcslno.Text;
                    objEnumeration.sTCManfDate = txtManufactureDate.Text;
                    objEnumeration.sCrBy = objSession.UserId;
                    objEnumeration.sStatus = txtStatus.Text;
                    objEnumeration.sMakeName = cmbMake.SelectedItem.ToString();

                    objEnumeration.sTankCapacity = txtTankCapacity.Text;
                    objEnumeration.sTCWeight = txtWeight.Text;


                    if (cmbRating.SelectedIndex > 0)
                    {
                        objEnumeration.sRating = cmbRating.SelectedValue;
                    }
                    if (cmbStarRated.SelectedIndex > 0)
                    {
                        objEnumeration.sStarRate = cmbStarRated.SelectedValue;
                    }
                    //if (objEnumeration.sLoctype == "3")
                    //{
                    //    objEnumeration.sEnumType = "3";
                    //    Arr = objEnumeration.SaveStoreEnumerationDetails(objEnumeration);
                    //}
                    //else
                    //{
                    //    objEnumeration.sEnumType = "1";
                      
                    //}

                    Arr = objEnumeration.SaveStoreEnumerationDetails(objEnumeration);

                    if (Arr[1].ToString() == "0")
                    {
                       
                        bool bResult = SaveImagesPath(objEnumeration);
                        txtEnumDetailsId.Text = objEnumeration.sEnumDetailsId;
                        cmdSave.Text = "Update";
                        if (bResult == true)
                        {
                            ShowMsgBox(Arr[0].ToString());
                            ShowUploadedImages();
                            LoadStoreEnumeration();
                           
                        }
                        return;
                    }
                    if (Arr[1].ToString() == "1")
                    {
                       
                        SaveImagesPath(objEnumeration);
                        ShowMsgBox(Arr[0]);
                        ShowUploadedImages();
                        LoadStoreEnumeration();
                        return;
                    }
                    if (Arr[1].ToString() == "2")
                    {
                        ShowMsgBox(Arr[0]);
                        return;
                    }
                }


            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdSave_Click");
            }
        }

        bool ValidateForm()
        {
            bool bValidate = false;


            if (cmbLocationType.SelectedIndex == 0)
            {
                cmbLocationType.Focus();
                ShowMsgBox("Please Select the Location Type");
                return false;
            }

            if (cmbLocationName.SelectedIndex == 0)
            {
                if (cmbLocationType.SelectedValue == "1" || cmbLocationType.SelectedValue == "5")
                {
                    ShowMsgBox("Please Select the Store Name");
                }
                else if(cmbLocationType.SelectedValue == "3")
                {
                    ShowMsgBox("Please Select the Repairer Name");
                }
                cmbLocationName.Focus();              
                return false;
            }

            if (txtAddress.Text.Trim().Length == 0)
            {
                txtAddress.Focus();
                ShowMsgBox("Enter Address");
                return false;
            }
           
            if (txtwelddate.Text.Trim().Length == 0)
            {
                txtwelddate.Focus();
                ShowMsgBox("Enter Date of Fixing");
                return false;
            }
            string sResult = Genaral.DateValidation(txtwelddate.Text);
            if (sResult != "")
            {
                ShowMsgBox(sResult);
                return bValidate;
            }
            sResult = Genaral.DateComparision(txtwelddate.Text, "", true, false);
            if (sResult == "1")
            {
                ShowMsgBox("Date of Fixing should be Less than Current Date");
                return bValidate;
            }

            if (cmboperator1.SelectedIndex == 0)
            {
                cmboperator1.Focus();
                ShowMsgBox("Please Select the Operator1 Name ");
                return false;
            }
            if (cmboperator2.SelectedIndex == 0)
            {
                cmboperator2.Focus();
                ShowMsgBox("Please Select the Operator2 name");
                return false;
            }

            if (txtTcCode.Text.Trim().Length == 0)
            {
                txtTcCode.Focus();
                ShowMsgBox("Enter SS Plate Number");
                return false;
            }

            if (cmbMake.SelectedIndex == 0)
            {
                cmbMake.Focus();
                ShowMsgBox("Please Select the DTr Make");
                return false;
            }

            if (cmbMake.SelectedValue != "1")
            {

                //if (cmbCapacity.SelectedIndex == 0)
                //{
                //    cmbCapacity.Focus();
                //    ShowMsgBox("Select DTr Capacity");
                //    return false;
                //}

                //if (txtTcslno.Text.Trim().Length == 0)
                //{
                //    txtTcslno.Focus();
                //    ShowMsgBox("Enter DTr SLNO");
                //    return false;
                //}
            }
           
            //if (txtManufactureDate.Text.Trim().Length == 0)
            //{
            //    txtManufactureDate.Focus();
            //    ShowMsgBox("Enter Manufacture Date");
            //    return false;
            //}

            if (txtManufactureDate.Text.Trim() != "")
            {
                //sResult = Genaral.DateValidation(txtManufactureDate.Text);
                //if (sResult != "")
                //{
                //    ShowMsgBox(sResult);
                //    return bValidate;
                //}
                //sResult = Genaral.DateComparision(txtManufactureDate.Text, "", true, false);
                //if (sResult == "1")
                //{
                //    ShowMsgBox("Manufacture Date should be Less than Current Date");
                //    return bValidate;
                //}

                string Date = @"^([1-9]|0[1-9]|1[0-2])[- / .](1[9][0-9][0-9]|2[0][0-9][0-9])$";
                System.Text.RegularExpressions.Regex r1 = new System.Text.RegularExpressions.Regex(Date);
                if (!r1.IsMatch(txtManufactureDate.Text))
                {
                    ShowMsgBox("Please Enter a valid Maufacture date in the format (MM/yyyy)");
                    return bValidate;

                }
                string DatePattern = @"^[a-zA-Z0-9 !@#$%^&*)(]{1,20}$";
                System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(DatePattern);
                if (r.IsMatch(txtManufactureDate.Text))
                {
                    ShowMsgBox("Please Enter a valid Maufacture date in the format (dd/mm/yyyy)");
                    return bValidate;

                }
            }

            if (cmbTranstype.SelectedIndex == 0)
            {
                cmbTranstype.Focus();
                ShowMsgBox("Please Select the Transaformer Type");
                return false;
            }

            if (txtEnumDetailsId.Text == "")
            {
                if (fupNamePlate.PostedFile.ContentLength == 0 && txtNamePlatePhotoPath.Text.Trim()=="")
                {
                    fupNamePlate.Focus();
                    ShowMsgBox("Select Name Plate Image to Upload");
                    return false;
                }

                if (fupSSPlate.PostedFile.ContentLength == 0 && txtSSPlatePath.Text.Trim() == "")
                {
                    fupSSPlate.Focus();
                    ShowMsgBox("Select SS Plate Image to Upload");
                    return false;
                }
            }
            if (cmbRating.SelectedValue == "1")
            {
                if (cmbStarRated.SelectedIndex == 0)
                {
                    cmbStarRated.Focus();
                    ShowMsgBox("Select Star Rating");
                    return false;
                }
            }

            string sValidateResult = ValidateImages();
            if (sValidateResult != "")
            {
                ShowMsgBox(sValidateResult);
                return false;
            }

            bValidate = true;
            return bValidate;
        }

        protected void cmboperator1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmboperator1.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT IU_ID, IU_FULLNAME FROM TBLINTERNALUSERS WHERE IU_USERTYPE = 1 AND IU_ID <> '" + cmboperator1.SelectedValue + "'", "--Select--", cmboperator2);
                   // LoadStoreEnumeration(cmboperator1.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmboperator1_SelectedIndexChanged");
            }
        }



        protected void GridStoreEnumDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GridStoreEnumDetails.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["EnumStore"];
                GridStoreEnumDetails.DataSource = dt;
                GridStoreEnumDetails.DataBind();

            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GridTCDetails_PageIndexChanging");
            }
        }

        protected void GridStoreEnumDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "remove")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    Label lblEnumDetailsId = (Label)row.FindControl("lblEdID");

                    clsStoreEnumeration objFieldEnum = new clsStoreEnumeration();
                    objFieldEnum.sEnumDetailsId = lblEnumDetailsId.Text;
                    bool bResult = objFieldEnum.DeleteEnumerationDetails(objFieldEnum);
                    if (bResult == true)
                    {
                        ShowMsgBox("Removed Successfully");
                        LoadStoreEnumeration("");
                        return;
                    }
                }
                #region Edit
              
                if (e.CommandName == "Modify")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    int iRowIndex = row.RowIndex;

                    Label lblOffcode = (Label)row.FindControl("lblOffcode");
                    Label lblLocationName = (Label)row.FindControl("lblLocationName");
                    Label lblLocationAdd = (Label)row.FindControl("lblLocationAdd");
                    Label lblWelddate = (Label)row.FindControl("lblWelddate");
                    Label lbloper1 = (Label)row.FindControl("lbloper1");
                    Label lbloper2 = (Label)row.FindControl("lbloper2");
                    Label lblTcCode = (Label)row.FindControl("lblTcCode");
                    Label lblmake = (Label)row.FindControl("lblmake");
                    Label lblTcslno = (Label)row.FindControl("lblTcslno");
                    Label lblManfDate = (Label)row.FindControl("lblManfDate");
                    Label lblCapacity = (Label)row.FindControl("lblCapacity");
                    Label lblPhotopath = (Label)row.FindControl("lblPhotopath");
                    Label lblDTCCode = (Label)row.FindControl("lblDTCCode");
                    Label lblCescDTCCode = (Label)row.FindControl("lblCescDTCCode");
                    Label lblIpDTCCode = (Label)row.FindControl("lblIpDTCCode");
                    Label lblEnumerationDate = (Label)row.FindControl("lblEnumerationDate");
                    Label lblOldPhoto = (Label)row.FindControl("lblOldPhoto");
                    Label lblNewPhoto = (Label)row.FindControl("lblNewPhoto");
                    Label lblLocationTypeID = (Label)row.FindControl("lblLocationTypeID");
                    Label lblTcType = (Label)row.FindControl("lblTcType");


                  
                    txtAddress.Text = lblLocationAdd.Text;
                    cmbLocationType.SelectedValue = lblLocationTypeID.Text;
                    txtwelddate.Text = lblWelddate.Text;
                    txtTcCode.Text = lblTcCode.Text;
                    txtTcslno.Text = lblTcslno.Text;
                    txtManufactureDate.Text = lblManfDate.Text;
                  
                    cmbCapacity.SelectedValue = lblCapacity.Text;
                    cmbMake.SelectedValue = lblmake.Text;
                    cmboperator1.SelectedValue = lbloper1.Text;
                    cmboperator2.SelectedValue = lbloper2.Text;
                    cmbTranstype.SelectedValue = lblTcType.Text;

                    DataTable dt = (DataTable)ViewState["TC"];
                    dt.Rows[iRowIndex].Delete();
                    if (dt.Rows.Count == 0)
                    {
                        ViewState["TC"] = null;
                    }
                    else
                    {
                        ViewState["TC"] = dt;
                    }

                    GridStoreEnumDetails.DataSource = dt;
                    GridStoreEnumDetails.DataBind();
                }
                #endregion

            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GridTCDetails_RowCommand");
            }
        }

        public void LoadStoreEnumeration(string strOperator = "")
        {
            try
            {
                clsStoreEnumeration objFieldEnum = new clsStoreEnumeration();
                DataTable dt = new DataTable();
                dt = objFieldEnum.LoadStoreEnumeration();
                GridStoreEnumDetails.DataSource = dt;
                GridStoreEnumDetails.DataBind();
                ViewState["EnumStore"] = dt;
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadStoreEnumeration");
            }
        }
        protected void cmbLocationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbLocationType.SelectedValue == "1" || cmbLocationType.SelectedValue == "5")
                {
                    Genaral.Load_Combo("SELECT SM_ID || '~' || SM_OFF_CODE,SM_NAME FROM TBLSTOREMAST", "--Select--", cmbLocationName);
                    lblRepairerName.Text = "Store Name";
                    lblAddress.Text = "Store Address";
                    txtAddress.Text = string.Empty;

                }
                else if (cmbLocationType.SelectedValue == "3")
                {
                    Genaral.Load_Combo("SELECT TR_ID || '~' || TR_OFFICECODE,TR_NAME FROM TBLTRANSREPAIRER", "--Select--", cmbLocationName);
                    lblRepairerName.Text = "Repairer Name";
                    txtAddress.Text = string.Empty;
                    lblAddress.Text = "Repairer Address";
                }
                if (cmbLocationType.SelectedIndex == 0)
                {
                    txtAddress.Text = string.Empty;
                    cmbLocationName.Items.Clear();
                }

  
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmbRepairerName_SelectedIndexChanged");
            }
        }

        protected void cmbLocationName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                clsStoreEnumeration objenumeration = new clsStoreEnumeration();
                if (cmbLocationName.SelectedIndex > 0)
                {
                    if (cmbLocationType.SelectedValue == "1" || cmbLocationType.SelectedValue == "5")
                    {
                        objenumeration.sValue = "1";
                        objenumeration.sSelectedValue = cmbLocationName.SelectedValue.Split('~').GetValue(0).ToString();
                        //objenumeration.GetAddress(objenumeration);
                        txtAddress.Text = objenumeration.GetAddress(objenumeration);

                    }
                    if (cmbLocationType.SelectedValue == "3")
                    {
                        objenumeration.sValue = "3";
                        objenumeration.sSelectedValue = cmbLocationName.SelectedValue.Split('~').GetValue(0).ToString();
                        //objenumeration.GetAddress(objenumeration);
                        txtAddress.Text = objenumeration.GetAddress(objenumeration);
                    }

                }
                else
                {
                    txtAddress.Text = string.Empty;
                }

            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmbLocationName_SelectedIndexChanged");
            }



        }

        protected void cmboperator2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //LoadStoreEnumeration(cmboperator2.SelectedValue);
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmboperator2_SelectedIndexChanged");
            }

        }

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                Reset();
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdReset_Click");
            }
        }

        public bool SaveImagesPath(clsStoreEnumeration objStoreEnum)
        {
            try
            {

                //FTP Parameter
                string sFTPLink = string.Empty;
                string sFTPUserName = string.Empty;
                string sFTPPassword = string.Empty;

                // File Name Parameter
                string sDirectory = string.Empty;
                string sPlateFileName = string.Empty;
                string sSSPlateFileName = string.Empty;

                // File Path Parameter
                string sSavePlateFilePath = string.Empty;
                string sSaveSSPlateFilePath = string.Empty;

                //FileType Parameter
                string sPlatePhotoExtension = string.Empty;
                string sSSPlatePhotoExtension = string.Empty;

 
                //  Photo Save DTLMSDocs
                string sFileExt = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FileFormat"]);

                string mainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder1"]);

                //  Photo Save DTLMSDocs

                clsCommon objComm = new clsCommon();
                DataTable dt = objComm.GetAppSettings();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToString(dt.Rows[i]["AP_KEY"]) == "FTPLINK")
                    {
                        sFTPLink = Convert.ToString(dt.Rows[i]["AP_VALUE"]);
                    }
                    else if (Convert.ToString(dt.Rows[i]["AP_KEY"]) == "FTPUSERNAME")
                    {
                        sFTPUserName = Convert.ToString(dt.Rows[i]["AP_VALUE"]);
                    }
                    else if (Convert.ToString(dt.Rows[i]["AP_KEY"]) == "FTPPASSWORD")
                    {
                        sFTPPassword = Convert.ToString(dt.Rows[i]["AP_VALUE"]);
                    }
                }

                sFTPLink = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPPath"].ToUpper());
                clsSFTP objFtp = new clsSFTP(sFTPLink, sFTPUserName, sFTPPassword);
                bool Isuploaded;

                string sNamePlateFolderName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["NamePlateFolder"].ToUpper());
                string sSSPlateFolderName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SSPlateFolder"].ToUpper());

                // Create Directory

                bool IsExists = objFtp.FtpDirectoryExists(mainfolder + objStoreEnum.sEnumDetailsId + "/" );
                if (IsExists == false)
                {

                    objFtp.createDirectory(mainfolder + objStoreEnum.sEnumDetailsId);
                }


                // Name Plate Photo Save

                if (txtNamePlatePhotoPath.Text.Trim() != "")
                {
                    sPlatePhotoExtension = System.IO.Path.GetExtension(txtNamePlatePhotoPath.Text).ToString().ToLower();
                    sPlatePhotoExtension = ";" + sPlatePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sPlatePhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sPlateFileName = Path.GetFileName(txtNamePlatePhotoPath.Text);
                    sDirectory = txtNamePlatePhotoPath.Text;
                    objStoreEnum.sNamePlatePhotoPath = txtNamePlatePhotoPath.Text;
                }
                else if (fupNamePlate.PostedFile.ContentLength != 0)
                {

                    sPlatePhotoExtension = System.IO.Path.GetExtension(fupNamePlate.FileName).ToString().ToLower();
                    sPlatePhotoExtension = ";" + sPlatePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sPlatePhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sPlateFileName = Path.GetFileName(fupNamePlate.PostedFile.FileName);

                    fupNamePlate.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sPlateFileName));
                    sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sPlateFileName);
                   
                }

                if (sPlateFileName != "")
                {

                    if (File.Exists(sDirectory))
                    {
                        IsExists = objFtp.FtpDirectoryExists(mainfolder + objStoreEnum.sEnumDetailsId + "/" + sNamePlateFolderName + "/" );
                        if (IsExists == false)
                        {

                            objFtp.createDirectory(mainfolder + objStoreEnum.sEnumDetailsId + "/" + sNamePlateFolderName);
                        }

                        Isuploaded = objFtp.upload(mainfolder + objStoreEnum.sEnumDetailsId + "/" + sNamePlateFolderName + "/" , objSession.UserId + "~" + sPlateFileName, sDirectory);
                        if (Isuploaded == true & File.Exists(sDirectory))
                        {
                            File.Delete(sDirectory);
                            objStoreEnum.sNamePlatePhotoPath = objStoreEnum.sEnumDetailsId + "/" + sNamePlateFolderName + "/" + objSession.UserId + "~" + sPlateFileName;
                            txtNamePlatePhotoPath.Text = objStoreEnum.sNamePlatePhotoPath;
                        }
                    }
                }

                // SS Plate Photo Save
                if (txtSSPlatePath.Text.Trim() != "")
                {
                    sSSPlatePhotoExtension = System.IO.Path.GetExtension(txtSSPlatePath.Text).ToString().ToLower();
                    sSSPlatePhotoExtension = ";" + sSSPlatePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sSSPlatePhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sSSPlateFileName = Path.GetFileName(txtSSPlatePath.Text);
                    sDirectory = txtSSPlatePath.Text;
                    objStoreEnum.sSSPlatePhotoPath = txtSSPlatePath.Text;
                }

                else if (fupSSPlate.PostedFile.ContentLength != 0)
                {

                    sSSPlatePhotoExtension = System.IO.Path.GetExtension(fupSSPlate.FileName).ToString().ToLower();
                    sSSPlatePhotoExtension = ";" + sSSPlatePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sSSPlatePhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sSSPlateFileName = Path.GetFileName(fupSSPlate.PostedFile.FileName);

                    fupSSPlate.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sSSPlateFileName));
                    sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sSSPlateFileName);
                }

                if(sSSPlateFileName!="")
                {
                    if (File.Exists(sDirectory))
                    {
                        IsExists = objFtp.FtpDirectoryExists(mainfolder + objStoreEnum.sEnumDetailsId + "/" + sSSPlateFolderName + "/");
                        if (IsExists == false)
                        {

                            objFtp.createDirectory(mainfolder + objStoreEnum.sEnumDetailsId + "/" + sSSPlateFolderName);
                        }

                        Isuploaded = objFtp.upload(mainfolder + objStoreEnum.sEnumDetailsId + "/" + sSSPlateFolderName + "/" , objSession.UserId + "~" + sSSPlateFileName, sDirectory);
                        if (Isuploaded == true & File.Exists(sDirectory))
                        {
                            File.Delete(sDirectory);
                            objStoreEnum.sSSPlatePhotoPath = objStoreEnum.sEnumDetailsId + "/" + sSSPlateFolderName + "/" + objSession.UserId + "~" + sSSPlateFileName;
                            txtSSPlatePath.Text = objStoreEnum.sSSPlatePhotoPath;
                        }
                    }
                }

                bool bResult;
                if (txtEnumDetailsId.Text.Trim() == "")
                {
                     bResult = objStoreEnum.SaveImagePathDetails(objStoreEnum);
                }
                else
                {
                    bResult = objStoreEnum.UpdateImagePathDetails(objStoreEnum);
                }
               
                return bResult;

            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "SaveImages");
                return false;
            }
        }

        public void RestrictUpdate(string sStatusFlag)
        {
            try
            {
                if (sStatusFlag == "1")
                {
                    cmdSave.Enabled = false;
                    cmdReset.Enabled = false;
                    GridStoreEnumDetails.Columns[5].Visible = false;
                }
                clsQCApproval objQC = new clsQCApproval();

                bool bResult = objQC.CheckEnumerationUpdateAuthority(objSession.UserId);
                if (bResult == false)
                {
                    cmdSave.Enabled = false;
                    cmdReset.Enabled = false;
                    GridStoreEnumDetails.Columns[5].Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "RestrictUpdate");
            }
        }

      

        public void RetainImageinPostbackSession()
        {
            try
            {
                //Name Plate Photo

                //Case: 1 When the page is submitted for the first time(First PostBack) and there is file 
                // in FileUpload control but session is Null then Store the values to Session Object as:
                if (Session["fupNamePlate"] == null && fupNamePlate.HasFile)
                {
                    Session["fupNamePlate"] = fupNamePlate;
                    //lblImageName.Text = FileUpload1.FileName;
                }
                // Case 2: On Next PostBack Session has value but FileUpload control is
                // Blank due to PostBack then return the values from session to FileUpload as:
                else if (Session["fupNamePlate"] != null && (!fupNamePlate.HasFile))
                {
                    fupNamePlate = (FileUpload)Session["fupNamePlate"];
                    //lblImageName.Text = FileUpload1.FileName;
                }
                // Case 3: When there is value in Session but user want to change the file then
                // In this case we need to change the file in session object also as:
                else if (fupNamePlate.HasFile)
                {
                    Session["fupNamePlate"] = fupNamePlate;
                    //lblImageName.Text = FileUpload1.FileName;
                }

            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "RetainImageinPostback");
            }
        }


        protected void cmbRating_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbRating.SelectedValue == "1")
                {
                    dvStar.Style.Add("display", "block");
                    Genaral.Load_Combo("SELECT MD_ID, MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'SRT'", "--Select--", cmbStarRated);
                }
                else
                {
                    dvStar.Style.Add("display", "none");
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbRating_SelectedIndexChanged");
            }
        }

        protected void cmbMake_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbMake.SelectedValue == "1")
                {
                    
                    if (txtTcCode.Text.Trim() == "")
                    {
                        ShowMsgBox("Enter SS Plate Number");
                        cmbMake.SelectedIndex = 0;
                        return;
                    }
                    txtTcslno.Enabled = false;
                    txtTcslno.Text = "NNP" + txtTcCode.Text;
                }
                else
                {
                    txtTcslno.Enabled = true;
                    txtTcslno.Text = "";
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmbMake_SelectedIndexChanged");
            }
        }

        public void RetainImageOnPostback()
        {
            try
            {
                string sDirectory = string.Empty;
                string sSSPlateFileName = string.Empty;
                string sNamePlateFileName = string.Empty;
              
                if (fupSSPlate.HasFile)
                {
                    sSSPlateFileName = Path.GetFileName(fupSSPlate.PostedFile.FileName);
                    fupSSPlate.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sSSPlateFileName));
                    txtSSPlatePath.Text = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sSSPlateFileName);

                }

                if (fupNamePlate.HasFile)
                {
                    sNamePlateFileName = Path.GetFileName(fupNamePlate.PostedFile.FileName);
                    fupNamePlate.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sNamePlateFileName));
                    txtNamePlatePhotoPath.Text = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sNamePlateFileName);
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "RetainImageOnPostback");
            }
        }

        public string ValidateImages()
        {
            string svalidate = string.Empty;
            try
            {
                //FileType Parameter
                string sPlatePhotoExtension = string.Empty;
                string sSSPlatePhotoExtension = string.Empty;
              

                //  Photo Save DTLMSDocs
                string sFileExt = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FileFormat"]);

                //Name Plate Photo
                if (txtNamePlatePhotoPath.Text.Trim() != "")
                {
                    sPlatePhotoExtension = System.IO.Path.GetExtension(txtNamePlatePhotoPath.Text).ToString().ToLower();
                    sPlatePhotoExtension = ";" + sPlatePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sPlatePhotoExtension))
                    {
                        //ShowMsgBox("Invalid Image Format");
                        return "Invalid Image Format in Name Plate Photo";
                    }

                    //string sFileName = Path.GetFileName(txtNamePlatePhotoPath.Text);
                    //if (File.Exists(txtNamePlatePhotoPath.Text))
                    //{
                    //    return sFileName + " File Already Exists";
                    //}

                }
                else if (fupNamePlate.PostedFile.ContentLength != 0)
                {
                    sPlatePhotoExtension = System.IO.Path.GetExtension(fupNamePlate.FileName).ToString().ToLower();
                    sPlatePhotoExtension = ";" + sPlatePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sPlatePhotoExtension))
                    {
                        //ShowMsgBox("Invalid Image Format");
                        return "Invalid Image Format in Name Plate Photo";
                    }


                }

                if (txtSSPlatePath.Text.Trim() != "")
                {
                    sSSPlatePhotoExtension = System.IO.Path.GetExtension(txtSSPlatePath.Text).ToString().ToLower();
                    sSSPlatePhotoExtension = ";" + sSSPlatePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sSSPlatePhotoExtension))
                    {
                        //ShowMsgBox("Invalid Image Format");
                        return "Invalid Image Format in SS plate photo";
                    }

                }

                else if (fupSSPlate.PostedFile.ContentLength != 0)
                {

                    sSSPlatePhotoExtension = System.IO.Path.GetExtension(fupSSPlate.FileName).ToString().ToLower();
                    sSSPlatePhotoExtension = ";" + sSSPlatePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sSSPlatePhotoExtension))
                    {
                        //ShowMsgBox("Invalid Image Format");
                        return "Invalid Image Format in SS plate photo";
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ValidateImages");
                return ex.Message;
            }
        }

        public void ShowUploadedImages()
        {
            try
            {
                clsCommon objComm = new clsCommon();

                //FTP Parameter
                string sFTPLink = string.Empty;
                string sFTPUserName = string.Empty;
                string sFTPPassword = string.Empty;

                // To bind the Images from Ftp Path to Image Control
                string mainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder1"]);
                DataTable dt = objComm.GetAppSettings();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToString(dt.Rows[i]["AP_KEY"]) == "FTPLINK")
                    {
                        sFTPLink = Convert.ToString(dt.Rows[i]["AP_VALUE"]);
                    }
                    else if (Convert.ToString(dt.Rows[i]["AP_KEY"]) == "FTPUSERNAME")
                    {
                        sFTPUserName = Convert.ToString(dt.Rows[i]["AP_VALUE"]);
                    }
                    else if (Convert.ToString(dt.Rows[i]["AP_KEY"]) == "FTPPASSWORD")
                    {
                        sFTPPassword = Convert.ToString(dt.Rows[i]["AP_VALUE"]);
                    }
                }
                
                clsSFTP objFtp = new clsSFTP(sFTPLink, sFTPUserName, sFTPPassword);

                if (txtNamePlatePhotoPath.Text != "")
                {
                    dvNamePlate.Style.Add("display", "block");
                    imgNamePlate.ImageUrl = "ftp://" + sFTPUserName + ":" + sFTPPassword + "@" + sFTPLink.Remove(0, 6) + txtNamePlatePhotoPath.Text;
                }
                if (txtSSPlatePath.Text != "")
                {
                    dvSSPlate.Style.Add("display", "block");
                    imgSSPlate.ImageUrl = "ftp://" + sFTPUserName + ":" + sFTPPassword + "@" + sFTPLink.Remove(0, 6) + txtSSPlatePath.Text;
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ShowUploadedImages");

            }
        }
       
    }
}