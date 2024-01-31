using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Collections;
using System.Data;
using System.IO;

namespace IIITS.DTLMS.Internal
{
    public partial class QCEnumApprove : System.Web.UI.Page
    {
        string strFormCode = "QCEnumApprove";
        clsSession objSession;
        static int iIncrment = 2;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/InternalLogin.aspx", false);
                }
                objSession = (clsSession)Session["clsSession"];

                if (!IsPostBack)
                {
                    
                    Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME FROM TBLCIRCLE ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle);


                    Genaral.Load_Combo("SELECT TM_ID,TM_NAME FROM TBLTRANSMAKES ORDER BY TM_ID", "--Select--", cmbMake);
                    Genaral.Load_Combo("SELECT MD_NAME,MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'C'", "--Select--", cmbCapacity);
                    Genaral.Load_Combo("SELECT IU_ID, IU_FULLNAME FROM TBLINTERNALUSERS WHERE IU_USERTYPE = 1", "--Select--", cmboperator1);
                    Genaral.Load_Combo("SELECT MD_ID, MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'PT'", "--Select--", cmbProjecttype);
                    Genaral.Load_Combo("SELECT MD_ID, MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'LT'", "--Select--", cmbLoadtype);
                    Genaral.Load_Combo("SELECT MD_ID, MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'SR'", "--Select--", cmbRating);

                    if (Request.QueryString["QryEnumId"] != null && Request.QueryString["QryEnumId"].ToString() != "")
                    {
                        //
                        txtEnumDetailsId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["QryEnumId"]));
                        txtEnumType.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["EnumType"]));
                        string sStatus = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["Status"]));

                        RestrictUpdate(sStatus);

                        LoadEnumerationDetails(txtEnumDetailsId.Text);

                        if (txtEnumType.Text == "2")
                        {
                            cmbCircle_SelectedIndexChanged(sender, e);
                            cmbDivision.SelectedValue = hdfDivision.Value;
                            cmbDivision_SelectedIndexChanged(sender, e);
                            cmbsubdivision.SelectedValue = hdfSubdivision.Value;
                            cmbsubdivision_SelectedIndexChanged(sender, e);
                            cmbSection.SelectedValue = hdfSection.Value;
                            cmbFeeder.SelectedValue = hdfFeeder.Value;
                        }
                        else if (txtEnumType.Text == "1" || txtEnumType.Text == "3" || txtEnumType.Text == "5")
                        {
                            cmbLocationType_SelectedIndexChanged(sender, e);
                            cmbLocationName.SelectedValue = hdfLocName.Value;
                        }
                        cmboperator1_SelectedIndexChanged(sender, e);
                        cmboperator2.SelectedValue = hdfoperator.Value;

                        if (cmbRating.SelectedValue == "1")
                        {
                            cmbRating_SelectedIndexChanged(sender, e);
                            cmbRating.SelectedValue = hdfStarRate.Value;
                        }


                        if (cmbMake.SelectedValue == "1")
                        {
                            cmbMake_SelectedIndexChanged(sender, e);
                        }

                        VisibilityEnumType();

                    }

                    txtwelddate.Attributes.Add("onblur", "return ValidateDate(" + txtwelddate.ClientID + ");");
                  
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "Page_Load");
            }
        }

        protected void LoadEnumerationDetails(string strRecordId)
        {
            try
            {
                clsFieldEnumeration objfield = new clsFieldEnumeration();
                objfield.sEnumDetailsID = strRecordId;

                objfield.GetEnumerationDetails(objfield);


                clsCommon objComm = new clsCommon();

                //FTP Parameter
                string sFTPLink = string.Empty;
                string sFTPUserName = string.Empty;
                string sFTPPassword = string.Empty;
                string mainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder1"]);
                // To bind the Images from Ftp Path to Image Control

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

                if (txtEnumType.Text == "2")
                {
                    // DTC Details

                    cmbCircle.SelectedValue = objfield.sOfficeCode.Substring(0, 1);
                    hdfDivision.Value = objfield.sOfficeCode.Substring(0, 2);
                    hdfSubdivision.Value = objfield.sOfficeCode.Substring(0, 3);
                    hdfSection.Value = objfield.sOfficeCode.Substring(0, 4);
                    hdfFeeder.Value = objfield.sFeederCode;

                    txtDTCName.Text = objfield.sDTCName;
                    txtDTCCode.Text = objfield.sDTCCode;
                    txtoldDTCCode.Text = objfield.sOldDTCCode;
                    txtIPDTCCode.Text = objfield.sIPDTCCode;
                    txtEnumerationdate.Text = objfield.sEnumDate;

                    txtIPDTCCode.Text = objfield.sIPDTCCode;
                    txtInternalCode.Text = objfield.sInternalCode;
                    txtConnectedKW.Text = objfield.sConnectedKW;
                    txtConnectedHP.Text = objfield.sConnectedHP;
                    txtKWHReading.Text = objfield.sKWHReading;
                    txtCommisionDate.Text = objfield.sCommisionDate;
                    txtServiceDate.Text = objfield.sLastServiceDate;

                    if (objfield.sPlatformType != "")
                    {
                        cmbPlatformType.SelectedValue = objfield.sPlatformType;
                    }
                    if (objfield.sBreakertype != "")
                    {
                        cmbBreakerType.SelectedValue = objfield.sBreakertype;
                    }
                    if (objfield.sDTCMeters != "")
                    {
                        cmbDTCMetered.SelectedValue = objfield.sDTCMeters;
                    }
                    if (objfield.sHTProtect != "")
                    {
                        cmbHTProtection.SelectedValue = objfield.sHTProtect;
                    }
                    if (objfield.sLTProtect != "")
                    {
                        cmbLTProtection.SelectedValue = objfield.sLTProtect;
                    }
                    if (objfield.sGrounding != "")
                    {
                        cmbGrounding.SelectedValue = objfield.sGrounding;
                    }
                    if (objfield.sArresters != "")
                    {
                        cmbLightArrester.SelectedValue = objfield.sArresters;
                    }
                    if (objfield.sLoadtype != "")
                    {
                        cmbLoadtype.SelectedValue = objfield.sLoadtype;
                    }
                    if (objfield.sProjecttype != "")
                    {
                        cmbProjecttype.SelectedValue = objfield.sProjecttype;
                    }
                   
                    txtltLine.Text = objfield.sLTlinelength;
                    txtDepreciation.Text = objfield.sDepreciation;
                    txtLatitude.Text = objfield.sLatitude;
                    txtLongitude.Text = objfield.sLongitude;

                    if (objfield.sIsIPEnumDone == "1")
                    {
                        chkIsIPEnum.Checked = true;
                    }

                    txtInfosysAsset.Text = objfield.sInfosysAsset;

                    if (objfield.sIPCESCValue == "1")
                    {
                        rdbDTLMS.Checked = true;

                        rdbOldDtc.Enabled = false;
                        rdbIPEnum.Enabled = false;
                    }
                    else if (objfield.sIPCESCValue == "2")
                    {
                        rdbOldDtc.Checked = true;

                        rdbDTLMS.Enabled = false;
                        rdbIPEnum.Enabled = false;
                    }
                    else if (objfield.sIPCESCValue == "3")
                    {
                        rdbIPEnum.Checked = true;

                        rdbOldDtc.Enabled = false;
                        rdbDTLMS.Enabled = false;
                    }

                    imgOldCode.ImageUrl = "ftp://" + sFTPUserName + ":" + sFTPPassword + "@" + sFTPLink.Remove(0, 6) + objfield.sOldCodePhotoPath;
                    imgDTLMS.ImageUrl = "ftp://" + sFTPUserName + ":" + sFTPPassword + "@" + sFTPLink.Remove(0, 6) + objfield.sDTLMSCodePhotoPath;
                    imgIPEnum.ImageUrl = "ftp://" + sFTPUserName + ":" + sFTPPassword + "@" + sFTPLink.Remove(0, 6) + objfield.sIPEnumCodePhotoPath;
                    imgInfosys.ImageUrl = "ftp://" + sFTPUserName + ":" + sFTPPassword + "@" + sFTPLink.Remove(0, 6) + objfield.sInfosysCodePhotoPath;
                    imgDTCPhoto.ImageUrl = "ftp://" + sFTPUserName + ":" + sFTPPassword + "@" + sFTPLink.Remove(0, 6) + objfield.sDTCPhotoPath;
                }

                else if (txtEnumType.Text == "1" || txtEnumType.Text == "3" || txtEnumType.Text == "5")
                {
                    Genaral.Load_Combo("SELECT MD_ID, MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'TCL' and MD_ID <> 2 ORDER BY MD_NAME", "--Select--", cmbLocationType);
                    Genaral.Load_Combo("SELECT MD_ID, MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'TCT' ORDER BY MD_NAME", "--Select--", cmbTranstype);

                    cmbLocationType.SelectedValue = objfield.sLocType;
                    hdfLocName.Value = objfield.sLocName + "~" + objfield.sOfficeCode;
                    txtLocAddress.Text = objfield.sLocAddress;
                    cmbTranstype.SelectedValue = objfield.sTCType;
                }

             
                // Transformer Details
                txtTcCode.Text = objfield.sTcCode;
                txtTcslno.Text = objfield.sTCSlno;
                txtManufactureDate.Text = objfield.sTCManfDate;
                if (objfield.sTCCapacity != "")
                {
                    cmbCapacity.SelectedValue = objfield.sTCCapacity;
                }
                cmbMake.SelectedValue = objfield.sTCMake;
                txtwelddate.Text = objfield.sWeldDate;
                cmboperator1.SelectedValue = objfield.sOperator1;
                hdfoperator.Value = objfield.sOperator2;

                if (objfield.sRating != "")
                {
                    cmbRating.SelectedValue = objfield.sRating;
                }
                if (objfield.sStarRate != "")
                {
                    hdfStarRate.Value = objfield.sStarRate;
                }
                txtTankCapacity.Text = objfield.sTankCapacity;
                txtWeight.Text = objfield.sTCWeight;

                            
                txtEnumDetailsId.Text = objfield.sEnumDetailsID;

                if (txtTcslno.Text == objfield.sEnumDTCID)
                {
                    chkSlnoNotExist.Checked = true;
                }


                imgNamePlate.ImageUrl = "ftp://"+sFTPUserName+":" + sFTPPassword +"@" + sFTPLink.Remove(0,6) + objfield.sNamePlatePhotoPath;
                imgSSPlate.ImageUrl = "ftp://" + sFTPUserName + ":" + sFTPPassword + "@" + sFTPLink.Remove(0, 6) + objfield.sSSPlatePhotoPath;

            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadTCDetails");
            }
        }

     

        public void Reset()
        {
            try
            {

                txtTcCode.Text = string.Empty;
                txtTcslno.Text = string.Empty;
                cmbCapacity.SelectedIndex = 0;
                cmbMake.SelectedIndex = 0;
                txtManufactureDate.Text = string.Empty;

                txtDTCName.Text = string.Empty;
                txtDTCCode.Text = string.Empty;
                txtoldDTCCode.Text = string.Empty;
                txtIPDTCCode.Text = string.Empty;
                txtEnumerationdate.Text = string.Empty;

                txtInternalCode.Text = string.Empty;
                txtConnectedKW.Text = string.Empty;
                txtConnectedKW.Text = string.Empty;
                txtKWHReading.Text = string.Empty;
                txtCommisionDate.Text = string.Empty;
                txtServiceDate.Text = string.Empty;
                cmbPlatformType.SelectedIndex = 0;
                cmbBreakerType.SelectedIndex = 0;
                cmbDTCMetered.SelectedIndex = 0;
                cmbHTProtection.SelectedIndex = 0;
                cmbLTProtection.SelectedIndex = 0;
                cmbGrounding.SelectedIndex = 0;
                cmbLightArrester.SelectedIndex = 0;
                cmbLoadtype.SelectedIndex = 0;
                cmbProjecttype.SelectedIndex = 0;
                txtltLine.Text = string.Empty; 
                txtDepreciation.Text = string.Empty;
                txtLatitude.Text = string.Empty;
                txtLongitude.Text = string.Empty;

              
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "Reset");
            }
        }

        bool ValidateForm()
        {
            bool bValidate = false;

            if (txtEnumType.Text == "2")
            {
                if (cmbCircle.SelectedIndex == 0)
                {
                    cmbCircle.Focus();
                    ShowMsgBox("Please Select the Circle");
                    return false;
                }
                if (cmbDivision.SelectedIndex == 0)
                {
                    cmbDivision.Focus();
                    ShowMsgBox("Please Select the Division");
                    return false;
                }
                if (cmbsubdivision.SelectedIndex == 0)
                {
                    cmbsubdivision.Focus();
                    ShowMsgBox("Please Select the Subdivision");
                    return false;
                }
                if (cmbSection.SelectedIndex == 0)
                {
                    cmbSection.Focus();
                    ShowMsgBox("Please Select the Section");
                    return false;
                }
                if (cmbFeeder.SelectedIndex == 0)
                {
                    cmbFeeder.Focus();
                    ShowMsgBox("Please Select the Feeder");
                    return false;
                }
            }

         
            if (txtwelddate.Text.Trim().Length == 0)
            {
                txtwelddate.Focus();
                ShowMsgBox("Enter Date of Fixing");
                return false;
            }
            if (cmboperator1.SelectedIndex == 0)
            {
                cmboperator1.Focus();
                ShowMsgBox("Please Select the Operator1");
                return false;
            }
            if (cmboperator2.SelectedIndex == 0)
            {
                cmboperator2.Focus();
                ShowMsgBox("Please Select the Operator2");
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

                //if (txtManufactureDate.Text.Trim().Length == 0)
                //{
                //    txtManufactureDate.Focus();
                //    ShowMsgBox("Enter Manufacture Date");
                //    return false;
                //}
            }
            if (txtManufactureDate.Text.Trim() != "")
            {
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

            if (txtEnumType.Text == "2")
            {
                if (txtDTCName.Text.Trim().Length == 0)
                {
                    txtDTCName.Focus();
                    ShowMsgBox("Enter DTC Name");
                    return false;
                }
                if (txtDTCCode.Text.Trim().Length == 0)
                {
                    txtDTCCode.Focus();
                    ShowMsgBox("Enter DTC Code");
                    return false;
                }

                //if (txtoldDTCCode.Text.Trim().Length == 0)
                //{
                //    txtoldDTCCode.Focus();
                //    ShowMsgBox("Enter OLD DTC Code");
                //    return false;
                //}
                //if (txtIPDTCCode.Text.Trim().Length == 0)
                //{
                //    txtIPDTCCode.Focus();
                //    ShowMsgBox("Enter IP DTC Code");
                //    return false;
                //}

                //if (txtEnumerationdate.Text.Trim().Length == 0)
                //{
                //    txtEnumerationdate.Focus();
                //    ShowMsgBox("Enter Enumeration Date");
                //    return false;
                //} 

                if (txtEnumerationdate.Text.Trim() != "")
                {

                   string  sResult = Genaral.DateValidation(txtEnumerationdate.Text);
                    if (sResult != "")
                    {
                        ShowMsgBox(sResult);
                        txtEnumerationdate.Focus();
                        return bValidate;
                    }

                }
            }

            bValidate = true;
            return bValidate;
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

        protected void cmboperator1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try 
            {
                if (cmboperator1.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT IU_ID, IU_FULLNAME FROM TBLINTERNALUSERS WHERE IU_USERTYPE = 1 AND IU_ID <> '" + cmboperator1.SelectedValue + "'", "--Select--", cmboperator2);
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmboperator1_SelectedIndexChanged");
            }
        }

     

        protected void cmbCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCircle.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION  WHERE DIV_CICLE_CODE='" + cmbCircle.SelectedValue + "' ORDER BY DIV_CODE", "--Select--", cmbDivision);
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmbCircle_SelectedIndexChanged");
            }

        }

        protected void cmbDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDivision.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT SD_SUBDIV_CODE, SD_SUBDIV_NAME FROM TBLSUBDIVMAST  WHERE SD_DIV_CODE='" + cmbDivision.SelectedValue + "' ORDER BY SD_SUBDIV_CODE", "--Select--", cmbsubdivision);
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmbCircle_SelectedIndexChanged");
            }
        }

        protected void cmbsubdivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbsubdivision.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT OM_CODE, OM_NAME FROM TBLOMSECMAST WHERE OM_SUBDIV_CODE = '" + cmbsubdivision.SelectedValue + "' ORDER BY OM_CODE", "--Select--", cmbSection);
                    string strQry = "SELECT FD_FEEDER_CODE,FD_FEEDER_CODE || '-' || FD_FEEDER_NAME FROM TBLFEEDERMAST, TBLFEEDEROFFCODE WHERE  FD_FEEDER_ID = FDO_FEEDER_ID AND";
                    strQry += " FDO_OFFICE_CODE LIKE '" + cmbsubdivision.SelectedValue + "%' ORDER BY FD_FEEDER_CODE";
                    Genaral.Load_Combo(strQry, "--Select--", cmbFeeder);
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmbsubdivision_SelectedIndexChanged");
            }
        }

      

        protected void btnApproval_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateForm() == true)
                {
                   ApproveEnumerationDetails();
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdSave_Click");
            }

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {

                txtTcCode.Text = string.Empty;
                txtTcslno.Text = string.Empty;
                cmbCapacity.SelectedIndex = 0;
                cmbMake.SelectedIndex = 0;
                txtManufactureDate.Text = string.Empty;

                txtDTCName.Text = string.Empty;
                txtDTCCode.Text = string.Empty;
                txtoldDTCCode.Text = string.Empty;
                txtIPDTCCode.Text = string.Empty;
                txtEnumerationdate.Text = string.Empty;

                txtInternalCode.Text = string.Empty;
                txtConnectedKW.Text = string.Empty;
                txtConnectedKW.Text = string.Empty;
                txtKWHReading.Text = string.Empty;
                txtCommisionDate.Text = string.Empty;
                txtServiceDate.Text = string.Empty;
                cmbPlatformType.SelectedIndex = 0;
                cmbBreakerType.SelectedIndex = 0;
                cmbDTCMetered.SelectedIndex = 0;
                cmbHTProtection.SelectedIndex = 0;
                cmbLTProtection.SelectedIndex = 0;
                cmbGrounding.SelectedIndex = 0;
                cmbLightArrester.SelectedIndex = 0;
                cmbLoadtype.SelectedIndex = 0;
                cmbProjecttype.SelectedIndex = 0;
                txtltLine.Text = string.Empty;
                txtDepreciation.Text = string.Empty;
                txtLatitude.Text = string.Empty;
                txtLongitude.Text = string.Empty;
                txtConnectedKW.Text = string.Empty;
                cmbCircle.SelectedIndex = 0;
                cmbDivision.SelectedIndex = 0;
                cmbsubdivision.SelectedIndex = 0;
                cmbSection.SelectedIndex = 0;
                cmbFeeder.SelectedIndex = 0;
                txtwelddate.Text = string.Empty;
                cmboperator1.SelectedIndex = 0;
                cmboperator2.SelectedIndex = 0;



            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "Reset");
            }
        }

        public void ApproveEnumerationDetails()
        {
            try
            {
                clsFieldEnumeration objFieldEnum = new clsFieldEnumeration();
                string[] Arr = new string[2];

                if (txtRemark.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Comments");
                    return;

                }

                objFieldEnum.sQCApprovalId = txtApproveId.Text;
                objFieldEnum.sEnumDetailsID = txtEnumDetailsId.Text;

                objFieldEnum.sWeldDate = txtwelddate.Text;
                objFieldEnum.sOperator1 = cmboperator1.SelectedValue;
                objFieldEnum.sOperator2 = cmboperator2.SelectedValue;

                //TC Details

                objFieldEnum.sTcCode = txtTcCode.Text;
                objFieldEnum.sTCMake = cmbMake.SelectedValue;
                if (cmbCapacity.SelectedIndex > 0)
                {
                    objFieldEnum.sTCCapacity = cmbCapacity.SelectedValue;
                }
              
                objFieldEnum.sTCSlno = txtTcslno.Text;
                objFieldEnum.sTCManfDate = txtManufactureDate.Text;

                if (txtEnumType.Text == "2")
                {

                    objFieldEnum.sOfficeCode = cmbSection.SelectedValue;
                    objFieldEnum.sFeederCode = cmbFeeder.SelectedValue;

                    // DTC Details
                    objFieldEnum.sDTCName = txtDTCName.Text;
                    objFieldEnum.sDTCCode = txtDTCCode.Text;
                    objFieldEnum.sOldDTCCode = txtoldDTCCode.Text;
                    objFieldEnum.sIPDTCCode = txtIPDTCCode.Text;
                    objFieldEnum.sEnumDate = txtEnumerationdate.Text;


                    // DTC Other Details

                    objFieldEnum.sInternalCode = txtInternalCode.Text;
                    objFieldEnum.sConnectedKW = txtConnectedKW.Text;
                    objFieldEnum.sConnectedHP = txtConnectedHP.Text;
                    objFieldEnum.sKWHReading = txtKWHReading.Text;
                    objFieldEnum.sCommisionDate = txtCommisionDate.Text;
                    objFieldEnum.sLastServiceDate = txtServiceDate.Text;
                    if (cmbPlatformType.SelectedIndex > 0)
                    {
                        objFieldEnum.sPlatformType = cmbPlatformType.SelectedValue;
                    }
                    if (cmbBreakerType.SelectedIndex > 0)
                    {
                        objFieldEnum.sBreakertype = cmbBreakerType.SelectedValue;
                    }
                    if (cmbDTCMetered.SelectedIndex > 0)
                    {
                        objFieldEnum.sDTCMeters = cmbDTCMetered.SelectedValue;
                    }
                    if (cmbHTProtection.SelectedIndex > 0)
                    {
                        objFieldEnum.sHTProtect = cmbHTProtection.SelectedValue;
                    }
                    if (cmbLTProtection.SelectedIndex > 0)
                    {
                        objFieldEnum.sLTProtect = cmbLTProtection.SelectedValue;
                    }
                    if (cmbGrounding.SelectedIndex > 0)
                    {
                        objFieldEnum.sGrounding = cmbGrounding.SelectedValue;
                    }
                    if (cmbLightArrester.SelectedIndex > 0)
                    {
                        objFieldEnum.sArresters = cmbLightArrester.SelectedValue;
                    }
                    if (cmbLoadtype.SelectedIndex > 0)
                    {
                        objFieldEnum.sLoadtype = cmbLoadtype.SelectedValue;
                    }
                    if (cmbProjecttype.SelectedIndex > 0)
                    {
                        objFieldEnum.sProjecttype = cmbProjecttype.SelectedValue;
                    }
                    objFieldEnum.sLTlinelength = txtltLine.Text;
                    objFieldEnum.sDepreciation = txtDepreciation.Text;
                    objFieldEnum.sLatitude = txtLatitude.Text;
                    objFieldEnum.sLongitude = txtLongitude.Text;


                    if (chkIsIPEnum.Checked == true)
                    {
                        objFieldEnum.sIsIPEnumDone = "1";
                    }
                    else
                    {
                        objFieldEnum.sIsIPEnumDone = "0";
                    }

                }
                else if (txtEnumType.Text == "1" || txtEnumType.Text == "3" || txtEnumType.Text == "5")
                {
                    objFieldEnum.sOfficeCode = cmbLocationName.SelectedValue.Split('~').GetValue(1).ToString();
                    objFieldEnum.sLocName = cmbLocationName.SelectedValue.Split('~').GetValue(0).ToString();
                    objFieldEnum.sLocAddress = txtLocAddress.Text.Trim().Replace("'", "");
                    objFieldEnum.sTCType = cmbTranstype.SelectedValue;
                }

                objFieldEnum.sCrBy = objSession.UserId;
                objFieldEnum.sEnumType = txtEnumType.Text;

                objFieldEnum.sInfosysAsset = txtInfosysAsset.Text;
                objFieldEnum.sTCWeight = txtWeight.Text;
                objFieldEnum.sTankCapacity = txtTankCapacity.Text;

                if (cmbRating.SelectedIndex > 0)
                {
                    objFieldEnum.sRating = cmbRating.SelectedValue;
                }
                if (cmbStarRated.SelectedIndex > 0)
                {
                    objFieldEnum.sStarRate = cmbStarRated.SelectedValue;
                }

                Arr = objFieldEnum.ApproveQCEnumerationDetails(objFieldEnum);

                if (Arr[1].ToString() == "0")
                {

                    //SaveImagesPath(objFieldEnum);
                    ShowMsgBox(Arr[0].ToString());
                    //txtApproveId.Text = objFieldEnum.sQCApprovalId;

                    btnPending.Enabled = false;
                    btnReject.Enabled = false;
                    btnApproval.Enabled = false;
                    return;
                }

                if (Arr[1].ToString() == "2")
                {
                    ShowMsgBox(Arr[0]);
                    return;
                }

            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "SaveEnumerationDetails");
            }
        }


        protected void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                clsFieldEnumeration objFieldEnum = new clsFieldEnumeration();

                if (txtRemark.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Comments");
                    return;

                }

                objFieldEnum.sRemark = txtRemark.Text;
                objFieldEnum.sEnumDetailsID = txtEnumDetailsId.Text;
                objFieldEnum.sCrBy = objSession.UserId;

                bool bResult = objFieldEnum.RejectEnumerationDetails(objFieldEnum);

                if (bResult == true)
                {
                    ShowMsgBox("Enumeration Details Rejected Successfully");

                    btnApproval.Enabled = false;
                    btnPending.Enabled = false;
                    btnReject.Enabled = false;
                }
                else
                {
                    ShowMsgBox("Reject Failed");
                    return;
                }

            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "btnReject_Click");
            }
        }

        protected void btnPending_Click(object sender, EventArgs e)
        {
            try
            {
                clsFieldEnumeration objFieldEnum = new clsFieldEnumeration();

                if (txtRemark.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Comments");
                    return;

                }

                objFieldEnum.sRemark = txtRemark.Text;
                objFieldEnum.sEnumDetailsID = txtEnumDetailsId.Text;
                objFieldEnum.sCrBy = objSession.UserId;

                bool bResult = objFieldEnum.PendingForClarification(objFieldEnum);

                if (bResult == true)
                {
                    ShowMsgBox("Enumeration Details Sent for Clarification");

                    btnApproval.Enabled = false;
                    btnReject.Enabled = false;
                    btnPending.Enabled = false;
                }
                else
                {
                    ShowMsgBox("Sending Failed");
                    return;
                }
              
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "btnPending_Click");
            }

        }

        public void VisibilityEnumType()
        {
            try
            {
                if (txtEnumType.Text == "1" || txtEnumType.Text == "3" || txtEnumType.Text == "5")
                {
                    dvCircle.Style.Add("display", "none");
                    dvDiv.Style.Add("display", "none");
                    dvSub.Style.Add("display", "none");
                    dvSection.Style.Add("display", "none");
                    dvFeeder.Style.Add("display", "none");

                    dvLocAddress.Style.Add("display", "block");
                    dvLocName.Style.Add("display", "block");
                    dvLocType.Style.Add("display", "block");
                    dvTransType.Style.Add("display", "block");

                    liDTCDetails.Style.Add("display", "none");
                    liOtherDetails.Style.Add("display", "none");

                }
                else
                {
                    dvCircle.Style.Add("display", "block");
                    dvDiv.Style.Add("display", "block");
                    dvSub.Style.Add("display", "block");
                    dvSection.Style.Add("display", "block");
                    dvFeeder.Style.Add("display", "block");

                    dvLocAddress.Style.Add("display", "none");
                    dvLocName.Style.Add("display", "none");
                    dvLocType.Style.Add("display", "none");
                    dvTransType.Style.Add("display", "none");

                    liDTCDetails.Style.Add("display", "block");
                    liOtherDetails.Style.Add("display", "block");
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "VisibilityEnumType");
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

                }
                else if (cmbLocationType.SelectedValue == "3")
                {
                    Genaral.Load_Combo("SELECT TR_ID || '~' || TR_OFFICECODE,TR_NAME FROM TBLTRANSREPAIRER", "--Select--", cmbLocationName);
                    lblRepairerName.Text = "Repairer Name";
                    lblAddress.Text = "Repairer Address";
                }

            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmbLocationType_SelectedIndexChanged");
            }
        }

        protected void cmdNextDetails_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnApproval.Enabled == true || btnPending.Enabled == true || btnReject.Enabled == true)
                {
                    ShowMsgBox("Please take Action for current Record/Details");
                    return;
                }

                GetEnumerationId();

                LoadEnumerationDetails(txtEnumDetailsId.Text);

                if (txtEnumType.Text == "2")
                {
                    cmbCircle_SelectedIndexChanged(sender, e);
                    cmbDivision.SelectedValue = hdfDivision.Value;
                    cmbDivision_SelectedIndexChanged(sender, e);
                    cmbsubdivision.SelectedValue = hdfSubdivision.Value;
                    cmbsubdivision_SelectedIndexChanged(sender, e);
                    cmbSection.SelectedValue = hdfSection.Value;
                    cmbFeeder.SelectedValue = hdfFeeder.Value;
                }
                else if (txtEnumType.Text == "1" || txtEnumType.Text == "3" || txtEnumType.Text == "5")
                {
                    cmbLocationType_SelectedIndexChanged(sender, e);
                    cmbLocationName.SelectedValue = hdfLocName.Value;
                }
                cmboperator1_SelectedIndexChanged(sender, e);
                cmboperator2.SelectedValue = hdfoperator.Value;

                VisibilityEnumType();
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdNextDetails_Click");
            }
        }

        public void GetEnumerationId()
        {
            try
            {
                // AllEnumID

                if (Session["AllEnumID"] != null && Session["AllEnumID"].ToString() != "")
                {
                    //string sEnumerationId = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["AllEnumID"])); 
                    string sEnumerationId = Session["AllEnumID"].ToString();
                    Session["AllEnumID"] = null;
                    if (!sEnumerationId.StartsWith("~"))
                    {
                        sEnumerationId = "~" + sEnumerationId;
                    }
                    if (!sEnumerationId.EndsWith("~"))
                    {
                        sEnumerationId = sEnumerationId + "~";
                    }

                    string[] strarrEnum = sEnumerationId.Split('~');

                    string[] strDetailVal = strarrEnum.ToArray();
                    if (iIncrment > 1)
                    {

                        if (strDetailVal[iIncrment] != "")
                        {
                            txtEnumDetailsId.Text = strDetailVal[iIncrment].Split('`').GetValue(0).ToString();
                            txtEnumType.Text = strDetailVal[iIncrment].Split('`').GetValue(1).ToString();

                        }
                        iIncrment++;
                    }
                    

                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetEnumerationId");
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
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "txtIPDTCCode_TextChanged");
            }
        }

        protected void cmbMake_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbMake.SelectedValue == "1")
                {
                    txtTcslno.Enabled = false;
                }
                else
                {
                    txtTcslno.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmbMake_SelectedIndexChanged");
            }
        }


        public void RestrictUpdate(string sStatusFlag)
        {
            try
            {
                if (sStatusFlag == "2" || sStatusFlag == "3")
                {
                    btnApproval.Enabled = false;
                    btnPending.Enabled = false;
                    btnReject.Enabled = false;
                }
              
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "RestrictUpdate");
            }
        }
    }
}