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
    public partial class FieldEnum : System.Web.UI.Page
    {
        string strFormCode = "FieldEnumeration";
        clsSession objSession;

        protected void Page_Init(object sender, EventArgs e)
        {
            clsException.SaveFunctionExecLog("Page_Init --- START");

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                clsException.SaveFunctionExecLog("PageLoad --- START");

                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/InternalLogin.aspx", false);
                }
                objSession = (clsSession)Session["clsSession"];
                lblmessage.Text = string.Empty;
                RetainImageOnPostback();
                if (!IsPostBack)
                {
                  
                    Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME FROM TBLCIRCLE ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle);
                                                         
                    Genaral.Load_Combo("SELECT TM_ID,TM_NAME FROM TBLTRANSMAKES ORDER BY TM_ID", "--Select--", cmbMake);
                    Genaral.Load_Combo("SELECT MD_NAME,MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'C'", "--Select--", cmbCapacity);
                    Genaral.Load_Combo("SELECT IU_ID, IU_FULLNAME FROM TBLINTERNALUSERS WHERE IU_USERTYPE = 1", "--Select--", cmboperator1);
                    Genaral.Load_Combo("SELECT MD_ID, MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'PT'", "--Select--", cmbProjecttype);
                    Genaral.Load_Combo("SELECT MD_ID, MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'LT'", "--Select--", cmbLoadtype);
                    Genaral.Load_Combo("SELECT MD_ID, MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'PLT'", "--Select--", cmbPlatformType);
                    Genaral.Load_Combo("SELECT MD_ID, MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'BT'", "--Select--", cmbBreakerType);
                    Genaral.Load_Combo("SELECT MD_ID, MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'SR'", "--Select--", cmbRating);

                    if (Request.QueryString["QryEnumId"] != null && Request.QueryString["QryEnumId"].ToString() != "")
                    {
                        txtEnumDetailsId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["QryEnumId"]));
                        txtStatus.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["Status"]));

                        RestrictUpdate(txtStatus.Text);

                        GetEnumerationDetails(txtEnumDetailsId.Text);

                        cmbCircle_SelectedIndexChanged(sender, e);
                        cmbDivision.SelectedValue = hdfDivision.Value;
                        cmbDivision_SelectedIndexChanged(sender, e);
                        cmbsubdivision.SelectedValue = hdfSubdivision.Value;
                        cmbsubdivision_SelectedIndexChanged(sender, e);
                        cmbSection.SelectedValue = hdfSection.Value;
                        cmbFeeder.SelectedValue = hdfFeeder.Value;
                        cmboperator1_SelectedIndexChanged(sender, e);
                        cmboperator2.SelectedValue = hdfOperator.Value;
                        cmboperator2_SelectedIndexChanged(sender, e);

                        if (cmbMake.SelectedValue == "1")
                        {
                            string sTCSlno = txtTcslno.Text;
                            cmbMake_SelectedIndexChanged(sender, e);
                            txtTcslno.Text = sTCSlno;
                        }

                        if (cmbRating.SelectedValue == "1")
                        {
                            cmbRating_SelectedIndexChanged(sender, e);
                            cmbStarRated.SelectedValue = hdfStarRate.Value;
                        }

                    }

                    txtwelddate.Attributes.Add("onblur", "return ValidateDate(" + txtwelddate.ClientID + ");");
                    //txtManufactureDate.Attributes.Add("onblur", "return ValidateDate(" + txtManufactureDate.ClientID + ");");
                    txtEnumerationdate.Attributes.Add("onblur", "return ValidateDate(" + txtEnumerationdate.ClientID + ");");
                }

               // RetainImageinPostback();

                clsException.SaveFunctionExecLog("PageLoad --- END");
            }
            catch (Exception ex)
            {               
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "Page_Load");
            }
        }



        protected void GetEnumerationDetails(string strRecordId)
        {
            try
            {
                clsFieldEnumeration objfield = new clsFieldEnumeration();


                objfield.sEnumDetailsID = strRecordId;

                objfield.GetEnumerationDetails(objfield);

                cmbCircle.SelectedValue = objfield.sOfficeCode.Substring(0, 1);
                hdfDivision.Value = objfield.sOfficeCode.Substring(0, 2);
                hdfSubdivision.Value = objfield.sOfficeCode.Substring(0, 3);
                hdfSection.Value = objfield.sOfficeCode.Substring(0, 4);
                hdfFeeder.Value = objfield.sFeederCode;

                cmboperator1.SelectedValue = objfield.sOperator1;
                hdfOperator.Value = objfield.sOperator2;
                txtDTCName.Text = objfield.sDTCName;
                txtwelddate.Text = objfield.sWeldDate;
                txtTcCode.Text = objfield.sTcCode;
                txtTcslno.Text = objfield.sTCSlno;
                txtManufactureDate.Text = objfield.sTCManfDate;
                txtDTCCode.Text = objfield.sDTCCode;
                txtoldDTCCode.Text = objfield.sOldDTCCode;
                txtIPDTCCode.Text = objfield.sIPDTCCode;
                txtEnumerationdate.Text = objfield.sEnumDate;
                if (objfield.sTCCapacity != "")
                {
                    cmbCapacity.SelectedValue = objfield.sTCCapacity;
                }
                cmbMake.SelectedValue = objfield.sTCMake;

                txtIPDTCCode.Text = objfield.sIPDTCCode;
                txtInternalCode.Text = objfield.sInternalCode;
                txtConnectedKW.Text = objfield.sConnectedKW;
                txtConnectedHP.Text = objfield.sConnectedHP;
                txtKWHReading.Text = objfield.sKWHReading;
                txtCommisionDate.Text = objfield.sCommisionDate;
                txtServiceDate.Text = objfield.sLastServiceDate;

                cmbPlatformType.SelectedValue = objfield.sPlatformType;
                cmbBreakerType.SelectedValue = objfield.sBreakertype;
                cmbDTCMetered.SelectedValue = objfield.sDTCMeters;
                cmbHTProtection.SelectedValue = objfield.sHTProtect;
                cmbLTProtection.SelectedValue = objfield.sLTProtect;
                cmbGrounding.SelectedValue = objfield.sGrounding;
                cmbLightArrester.SelectedValue = objfield.sArresters;
                cmbLoadtype.SelectedValue = objfield.sLoadtype;
                cmbProjecttype.SelectedValue = objfield.sProjecttype;
                txtltLine.Text = objfield.sLTlinelength;
                txtDepreciation.Text = objfield.sDepreciation;
                txtLatitude.Text = objfield.sLatitude;
                txtLongitude.Text = objfield.sLongitude;
                txtEnumDetailsId.Text = objfield.sEnumDetailsID;
                

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

                txtTankCapacity.Text = objfield.sTankCapacity;
                txtWeight.Text = objfield.sTCWeight;
                txtInfosysAsset.Text = objfield.sInfosysAsset;

                cmbRating.SelectedValue = objfield.sRating;
                hdfStarRate.Value = objfield.sStarRate;


                txtDTLMSDTCPath.Text = objfield.sDTLMSCodePhotoPath;
                txtOLDDTCPath.Text = objfield.sOldCodePhotoPath;
                txtIPDTCPath.Text = objfield.sIPEnumCodePhotoPath;
                txtInfosysPath.Text = objfield.sInfosysCodePhotoPath;
                txtDTCPath.Text = objfield.sDTCPhotoPath;

                txtSSPlatePath.Text = objfield.sSSPlatePhotoPath;
                txtNamePlatePhotoPath.Text = objfield.sNamePlatePhotoPath;


                
                //cmdLoadImage.Visible = true;

                if (objfield.sIsIPEnumDone == "1")
                {
                    chkIsIPEnum.Checked = true;
                }
                cmdSave.Text = "Update";

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "GetEnumerationDetails");
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
                txtConnectedHP.Text = string.Empty;
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

                txtEnumDetailsId.Text = string.Empty;

                txtWeight.Text = string.Empty;
                txtTankCapacity.Text = string.Empty;
                txtInfosysAsset.Text = string.Empty;
                cmbRating.SelectedIndex = 0;
                cmbStarRated.Items.Clear();
                dvStar.Style.Add("display", "none");

                txtTcslno.Enabled = true;
                
                cmdSave.Text = "Save";

              
                txtSSPlatePath.Text = string.Empty;
                txtNamePlatePhotoPath.Text = string.Empty;
                txtDTLMSDTCPath.Text = string.Empty;
                txtDTCPath.Text = string.Empty;
                txtOLDDTCPath.Text = string.Empty;
                txtIPDTCPath.Text = string.Empty;
                txtInfosysPath.Text = string.Empty;

               

              
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "Reset");
            }
        }

        public void SaveEnumerationDetails()
        {
            try
            {
                clsFieldEnumeration objFieldEnum = new clsFieldEnumeration();
                string[] Arr = new string[2];

                objFieldEnum.sEnumDetailsID = txtEnumDetailsId.Text;

                objFieldEnum.sOfficeCode = cmbSection.SelectedValue;
                objFieldEnum.sFeederCode = cmbFeeder.SelectedValue;
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
              
                objFieldEnum.sMakeName = cmbMake.SelectedItem.ToString();

                // DTC Details
                objFieldEnum.sDTCName = txtDTCName.Text;
                objFieldEnum.sDTCCode = txtDTCCode.Text;
                objFieldEnum.sOldDTCCode = txtoldDTCCode.Text;
                objFieldEnum.sIPDTCCode = txtIPDTCCode.Text;
                objFieldEnum.sEnumDate = txtEnumerationdate.Text;
                objFieldEnum.sInfosysAsset = txtInfosysAsset.Text;

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

                if (rdbDTLMS.Checked == true)
                {
                    objFieldEnum.bIsDTLMSDetails = true;
                }
                if (rdbIPEnum.Checked == true)
                {
                    objFieldEnum.bIsIPDetails = true;
                }
                if (rdbOldDtc.Checked == true)
                {
                    objFieldEnum.bIsCESCDetails = true;
                }

                objFieldEnum.sCrBy = objSession.UserId;
                objFieldEnum.sStatus = txtStatus.Text;

                objFieldEnum.sTankCapacity = txtTankCapacity.Text;
                objFieldEnum.sTCWeight = txtWeight.Text;

                if (cmbRating.SelectedIndex > 0)
                {
                    objFieldEnum.sRating = cmbRating.SelectedValue;
                }
                if (cmbStarRated.SelectedIndex > 0)
                {
                    objFieldEnum.sStarRate = cmbStarRated.SelectedValue;
                }

                if (chkIsIPEnum.Checked == true)
                {
                    objFieldEnum.sIsIPEnumDone = "1";
                }
                else
                {
                    objFieldEnum.sIsIPEnumDone = "0";
                }

                clsException.SaveFunctionExecLog("SaveEnumerationDetails --- START");

                Arr = objFieldEnum.SaveFieldEnumerationDetails(objFieldEnum);

                clsException.SaveFunctionExecLog("SaveEnumerationDetails --- END");

                if (Arr[1].ToString() == "0")
                {
                    bool bResult = true;
                    //bool bResult = SaveImagesPath(objFieldEnum);
                    txtEnumDetailsId.Text = objFieldEnum.sEnumDetailsID;
                    cmdSave.Text = "Update";
                    ShowMsgBox(Arr[0].ToString());     
                    if (bResult == true)
                    {
                        ShowMsgBox(Arr[0].ToString());                      
                        LoadFieldEnumeration();
                       
                    }
                    else
                    {
                        //ShowMsgBox("Error Occured While Uploading Image");
                    }
                    return;
                }
                if (Arr[1].ToString() == "1")
                {
                    bool bResult = true;
                    //bool bResult = SaveImagesPath(objFieldEnum);
                    if (bResult == true)
                    {
                        clsException.SaveFunctionExecLog("GeneateSuccessMessage --- START");
                        ShowMsgBox(Arr[0]);
                        LoadFieldEnumeration();
                    }                  
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

        bool ValidateForm()
        {
            bool bValidate = false;
            clsException.SaveFunctionExecLog("ValidateForm --- START");

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
                    ShowMsgBox("Please Enter a valid Maufacture date in the format (mm/yyyy)");
                    return bValidate;

                }

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

           
            if (txtDTCCode.Text.Trim().Length == 0)
            {
                txtDTCCode.Focus();
                ShowMsgBox("Enter 6 digit DTC Code");

                DTCDetails.Attributes.Add("class", "tab-pane fade in active");
                liDTCDetails.Attributes.Add("class", "active");
                liTCDetails.Attributes.Add("class", "");

                return false;
            }
            if (txtDTCCode.Text.Trim().Length < 6)
            {
                txtDTCCode.Focus();
                ShowMsgBox("Enter 6 digit DTC Code");
                DTCDetails.Attributes.Add("class", "tab-pane fade in active");
                liDTCDetails.Attributes.Add("class", "active");
                liTCDetails.Attributes.Add("class", "");
                return false;
            }

            if (txtDTCCode.Text.Substring(0, 4) != cmbFeeder.SelectedValue)
            {
                txtDTCCode.Focus();
                ShowMsgBox("Enter Feeder code not Matching with DTC Code");
                DTCDetails.Attributes.Add("class", "tab-pane fade in active");
                liDTCDetails.Attributes.Add("class", "active");
                liTCDetails.Attributes.Add("class", "");
                return false;
            }

            if(txtoldDTCCode.Text.Trim()!="")
            {
                if (txtoldDTCCode.Text.Trim().Length < 6)
                {
                    txtoldDTCCode.Focus();
                    ShowMsgBox("Enter 6 digit  OLD DTC Code(CESC)");
                    DTCDetails.Attributes.Add("class", "tab-pane fade in active");
                    liDTCDetails.Attributes.Add("class", "active");
                    liTCDetails.Attributes.Add("class", "");
                    return false;
                }
            }

            if (txtIPDTCCode.Text.Trim() != "")
            {
                if (txtIPDTCCode.Text.Trim().Length < 6)
                {
                    txtIPDTCCode.Focus();
                    ShowMsgBox("Enter 6 digit  IP DTC Code(IP Enum)");
                    DTCDetails.Attributes.Add("class", "tab-pane fade in active");
                    liDTCDetails.Attributes.Add("class", "active");
                    liTCDetails.Attributes.Add("class", "");
                    return false;
                }
            }

            //if (txtoldDTCCode.Text.Trim().Length == 0)
            //{
            //    txtoldDTCCode.Focus();
            //    ShowMsgBox("Enter OLD DTC Code");
            //    DTCDetails.Attributes.Add("class", "tab-pane fade in active");
            //    liDTCDetails.Attributes.Add("class", "active");
            //    liTCDetails.Attributes.Add("class", "");
            //    return false;
            //}
            //if (txtIPDTCCode.Text.Trim().Length == 0)
            //{
            //    txtIPDTCCode.Focus();
            //    ShowMsgBox("Enter IP DTC Code");
            //    DTCDetails.Attributes.Add("class", "tab-pane fade in active");
            //    liDTCDetails.Attributes.Add("class", "active");
            //    liTCDetails.Attributes.Add("class", "");
            //    return false;
            //}

            if (txtDTCName.Text.Trim().Length == 0)
            {
                txtDTCName.Focus();
                ShowMsgBox("Enter DTC Name");


                DTCDetails.Attributes.Add("class", "tab-pane fade in active");
                TCDetails.Attributes.Add("class", "tab-pane fade");
                liDTCDetails.Attributes.Add("class", "active");
                liTCDetails.Attributes.Add("class", "");

                return false;
            }

            if (txtEnumerationdate.Text.Trim()!="")
            {

                sResult = Genaral.DateValidation(txtEnumerationdate.Text);
                if (sResult != "")
                {
                    ShowMsgBox(sResult);
                    txtEnumerationdate.Focus();

                    DTCDetails.Attributes.Add("class", "tab-pane fade in active");
                    TCDetails.Attributes.Add("class", "tab-pane fade");
                    liDTCDetails.Attributes.Add("class", "active");
                    liTCDetails.Attributes.Add("class", "");
                    return bValidate;
                }

            }
           
            if (txtEnumDetailsId.Text == "")
            {
                if (txtoldDTCCode.Text.Trim() != "")
                {

                    if (fupOldCodePhoto.PostedFile.ContentLength == 0 && txtOLDDTCPath.Text.Trim()=="")
                    {
                        fupOldCodePhoto.Focus();
                        ShowMsgBox("Select Old Code Photo to Upload");

                        DTCDetails.Attributes.Add("class", "tab-pane fade in active");
                        TCDetails.Attributes.Add("class", "tab-pane fade");
                        liDTCDetails.Attributes.Add("class", "active");
                        liTCDetails.Attributes.Add("class", "");
                        return false;
                    }
                }
                if (txtIPDTCCode.Text.Trim() != "")
                {
                    if (fupIPEnum.PostedFile.ContentLength == 0 && txtIPDTCPath.Text.Trim() == "")
                    {
                        fupIPEnum.Focus();
                        ShowMsgBox("Select IP Enumeration Code Photo to Upload");

                        DTCDetails.Attributes.Add("class", "tab-pane fade in active");
                        TCDetails.Attributes.Add("class", "tab-pane fade");
                        liDTCDetails.Attributes.Add("class", "active");
                        liTCDetails.Attributes.Add("class", "");
                        return false;
                    }
                }

                if (fupDTLMSCodePhoto.PostedFile.ContentLength == 0 && txtDTLMSDTCPath.Text.Trim() == "")
                {
                    fupDTLMSCodePhoto.Focus();
                    ShowMsgBox("Select DTLMS Code Photo to Upload");

                    DTCDetails.Attributes.Add("class", "tab-pane fade in active");
                    TCDetails.Attributes.Add("class", "tab-pane fade");
                    liDTCDetails.Attributes.Add("class", "active");
                    liTCDetails.Attributes.Add("class", "");
                    return false;
                }
                if (txtInfosysAsset.Text.Trim() != "")
                {

                    if (fupInfosys.PostedFile.ContentLength == 0 && txtInfosysPath.Text.Trim() == "")
                    {
                        fupInfosys.Focus();
                        ShowMsgBox("Select Infosys Asset Photo to Upload");

                        DTCDetails.Attributes.Add("class", "tab-pane fade in active");
                        TCDetails.Attributes.Add("class", "tab-pane fade");
                        liDTCDetails.Attributes.Add("class", "active");
                        liTCDetails.Attributes.Add("class", "");
                        return false;
                    }
                }

                if (fupDTCPhoto.PostedFile.ContentLength == 0 && txtDTCPath.Text.Trim() == "")
                {
                    fupDTCPhoto.Focus();
                    ShowMsgBox("Select DTC Photo to Upload");

                    DTCDetails.Attributes.Add("class", "tab-pane fade in active");
                    TCDetails.Attributes.Add("class", "tab-pane fade");
                    liDTCDetails.Attributes.Add("class", "active");
                    liTCDetails.Attributes.Add("class", "");
                    return false;
                }
            }

           string sValidateResult= ValidateImages();
           if (sValidateResult !="")
           {
               ShowMsgBox(sValidateResult);
               return false;
           }

           clsException.SaveFunctionExecLog("ValidateForm --- END");
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

        protected void grdFieldEnumDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "remove")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    Label lblEnumDetailsId = (Label)row.FindControl("lblEnumId");

                    clsFieldEnumeration objFieldEnum = new clsFieldEnumeration();
                    objFieldEnum.sEnumDetailsID = lblEnumDetailsId.Text;
                    bool bResult = objFieldEnum.DeleteEnumerationDetails(objFieldEnum);
                    if (bResult == true)
                    {
                        ShowMsgBox("Removed Successfully");
                        LoadFieldEnumeration();
                        return;
                    }

                }
                #region edit
               
                if (e.CommandName == "Modify")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    int iRowIndex = row.RowIndex;

                    Label lblOffcode = (Label)row.FindControl("lblOffcode");
                    Label lblfeedercode = (Label)row.FindControl("lblfeedercode");
                    Label lblDTCName = (Label)row.FindControl("lblDTCName");
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


                    cmbsubdivision.SelectedValue = lblOffcode.Text.Substring(1,3);
                    cmbSection.SelectedValue = lblOffcode.Text;
                    cmbFeeder.SelectedValue = lblfeedercode.Text;
                    txtDTCName.Text = lblDTCName.Text;
                    txtwelddate.Text = lblWelddate.Text;
                    txtTcCode.Text = lblTcCode.Text;
                    txtTcslno.Text = lblTcslno.Text;
                    txtManufactureDate.Text = lblManfDate.Text;
                    //txtplatephoto.Text = lblPhotopath.Text;
                    txtDTCCode.Text = lblDTCCode.Text;
                    txtoldDTCCode.Text = lblCescDTCCode.Text;
                    txtIPDTCCode.Text = lblIpDTCCode.Text;
                    txtEnumerationdate.Text = lblEnumerationDate.Text;
                    //txtoldCoding.Text = lblOldPhoto.Text;
                   // txtAfterCoding.Text = lblNewPhoto.Text;
                    cmbCapacity.SelectedValue = lblCapacity.Text;
                    cmbMake.SelectedValue = lblmake.Text;
                    cmboperator1.SelectedValue = lbloper1.Text;
                    cmboperator2.SelectedValue = lbloper2.Text;
                    
                    DataTable dt = (DataTable)ViewState["Enum"];
                    dt.Rows[iRowIndex].Delete();
                    if (dt.Rows.Count == 0)
                    {
                        ViewState["Enum"] = null;
                    }
                    else
                    {
                        ViewState["Enum"] = dt;
                    }

                    grdFieldEnumDetails.DataSource = dt;
                    grdFieldEnumDetails.DataBind();
                }

                #endregion
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "grdFieldEnumDetails_RowCommand");
            }
        }

        protected void grdFieldEnumDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdFieldEnumDetails.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Enum"];
                grdFieldEnumDetails.DataSource = dt;
                grdFieldEnumDetails.DataBind();

            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "grdFieldEnumDetails_PageIndexChanging");
            }
        }

        protected void cmboperator1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try 
            {
                if (cmboperator1.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT IU_ID, IU_FULLNAME FROM TBLINTERNALUSERS WHERE IU_USERTYPE = 1 AND IU_ID <> '" + cmboperator1.SelectedValue + "'", "--Select--", cmboperator2);
                    LoadFieldEnumeration(cmboperator1.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmboperator1_SelectedIndexChanged");
            }
        }

    

        public bool SaveImagesPath(clsFieldEnumeration objFieldEnum)
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
                string sOldCodeFileName = string.Empty;
                string sIPEnumCodeFileName = string.Empty;
                string sDTLMSCodeFileName = string.Empty;
                string sDTCFileName = string.Empty;
                string sInfosysCodeFileName = string.Empty;

                // File Path Parameter
                string sSavePlateFilePath = string.Empty;
                string sSaveSSPlateFilePath = string.Empty;
                string sSaveOldCodeFilePath = string.Empty;
                string sSaveIPEnumCodeFilePath = string.Empty;
                string sSaveDTLMSCodeFilePath = string.Empty;
                string sSaveDTCFilePath = string.Empty;
                string sSaveInfosysCodeFilePath = string.Empty;

                //FileType Parameter
                string sPlatePhotoExtension = string.Empty;
                string sSSPlatePhotoExtension = string.Empty;
                string sOldCodePhotoExtension = string.Empty;
                string sIPEnumCodePhotoExtension = string.Empty;
                string sDTLMSCodePhotoExtension = string.Empty;
                string sDTCPhotoExtension = string.Empty;
                string sInfosysCodePhotoExtension = string.Empty;

                clsException.SaveFunctionExecLog("SaveImagesPath --- START");
    
                //  Photo Save DTLMSDocs
                string sFileExt = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FileFormat"]);
                string mainfolder = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SFTPmainfolder1"]);
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
                string sOldCodeFolderName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["OldCodeFolder"].ToUpper());
                string sIPEnumFolderName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["IPEnumCodeFolder"].ToUpper());
                string sDTLMSFolderName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["DTLMSCodeFolder"].ToUpper());
                string sDTCFolderName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["DTCPhoto"].ToUpper());
                string sInfosysFolderName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["InfosysCodeFolder"].ToUpper());

                // Create Directory
              
                bool IsExists = objFtp.FtpDirectoryExists(mainfolder + objFieldEnum.sEnumDetailsID + "/");
                if (IsExists == false)
                {

                    objFtp.createDirectory(mainfolder + objFieldEnum.sEnumDetailsID);
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
                    objFieldEnum.sNamePlatePhotoPath = txtNamePlatePhotoPath.Text;
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
                        IsExists = objFtp.FtpDirectoryExists(mainfolder + objFieldEnum.sEnumDetailsID + "/" + sNamePlateFolderName + "/");
                        if (IsExists == false)
                        {

                            objFtp.createDirectory(mainfolder + objFieldEnum.sEnumDetailsID + "/" + sNamePlateFolderName);
                        }

                        Isuploaded = objFtp.upload(mainfolder + objFieldEnum.sEnumDetailsID + "/" + sNamePlateFolderName + "/" , sPlateFileName, sDirectory);
                        if (Isuploaded == true & File.Exists(sDirectory))
                        {
                            File.Delete(sDirectory);
                            objFieldEnum.sNamePlatePhotoPath = objFieldEnum.sEnumDetailsID + "/" + sNamePlateFolderName + "/" + sPlateFileName;
                            txtNamePlatePhotoPath.Text = objFieldEnum.sNamePlatePhotoPath;
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
                    objFieldEnum.sSSPlatePhotoPath = txtSSPlatePath.Text;
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
                if (sSSPlateFileName != "")
                {
                    if (File.Exists(sDirectory))
                    {
                        IsExists = objFtp.FtpDirectoryExists(mainfolder + objFieldEnum.sEnumDetailsID + "/" + sSSPlateFolderName + "/" );
                        if (IsExists == false)
                        {

                            objFtp.createDirectory(mainfolder+ objFieldEnum.sEnumDetailsID + "/" + sSSPlateFolderName);
                        }

                        Isuploaded = objFtp.upload(mainfolder + objFieldEnum.sEnumDetailsID + "/" + sSSPlateFolderName + "/" , sSSPlateFileName, sDirectory);
                        if (Isuploaded == true & File.Exists(sDirectory))
                        {
                            File.Delete(sDirectory);
                            objFieldEnum.sSSPlatePhotoPath = objFieldEnum.sEnumDetailsID + "/" + sSSPlateFolderName + "/" + sSSPlateFileName;
                            txtSSPlatePath.Text = objFieldEnum.sSSPlatePhotoPath;
                        }
                    }
                }

                          
                // Old Code Photo Save

                if (txtOLDDTCPath.Text.Trim() != "")
                {
                    sOldCodePhotoExtension = System.IO.Path.GetExtension(txtOLDDTCPath.Text).ToString().ToLower();
                    sOldCodePhotoExtension = ";" + sOldCodePhotoExtension.Remove(0, 1) + ";";


                    if (!sFileExt.Contains(sOldCodePhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sOldCodeFileName = Path.GetFileName(txtOLDDTCPath.Text);
                    sDirectory = txtOLDDTCPath.Text;
                    objFieldEnum.sOldCodePhotoPath = txtOLDDTCPath.Text;
                }

                else if (fupOldCodePhoto.PostedFile.ContentLength != 0)
                {

                    sOldCodePhotoExtension = System.IO.Path.GetExtension(fupOldCodePhoto.FileName).ToString().ToLower();
                    sOldCodePhotoExtension = ";" + sOldCodePhotoExtension.Remove(0, 1) + ";";


                    if (!sFileExt.Contains(sOldCodePhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sOldCodeFileName = Path.GetFileName(fupOldCodePhoto.PostedFile.FileName);

                    fupOldCodePhoto.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sOldCodeFileName));
                    sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sOldCodeFileName);                   
                }
                if (sOldCodeFileName != "")
                {
                    if (File.Exists(sDirectory))
                    {
                        IsExists = objFtp.FtpDirectoryExists(mainfolder + objFieldEnum.sEnumDetailsID + "/" + sOldCodeFolderName + "/");
                        if (IsExists == false)
                        {

                            objFtp.createDirectory(mainfolder + objFieldEnum.sEnumDetailsID + "/" + sOldCodeFolderName);
                        }

                        Isuploaded = objFtp.upload(mainfolder  +  objFieldEnum.sEnumDetailsID + "/" + sOldCodeFolderName + "/" , sOldCodeFileName, sDirectory);
                        if (Isuploaded == true & File.Exists(sDirectory))
                        {
                            File.Delete(sDirectory);
                            objFieldEnum.sOldCodePhotoPath = objFieldEnum.sEnumDetailsID + "/" + sOldCodeFolderName + "/" + sOldCodeFileName;
                            txtOLDDTCPath.Text = objFieldEnum.sOldCodePhotoPath;
                        }
                    }
                }

                // IP Enum Code Photo Save
                if (txtIPDTCPath.Text.Trim() != "")
                {
                    sIPEnumCodePhotoExtension = System.IO.Path.GetExtension(txtIPDTCPath.Text).ToString().ToLower();
                    sIPEnumCodePhotoExtension = ";" + sIPEnumCodePhotoExtension.Remove(0, 1) + ";";


                    if (!sFileExt.Contains(sIPEnumCodePhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sIPEnumCodeFileName = Path.GetFileName(txtIPDTCPath.Text);
                    sDirectory = txtIPDTCPath.Text;
                    objFieldEnum.sIPEnumCodePhotoPath = txtIPDTCPath.Text;
                }
               else  if (fupIPEnum.PostedFile.ContentLength != 0)
                {

                    sIPEnumCodePhotoExtension = System.IO.Path.GetExtension(fupIPEnum.FileName).ToString().ToLower();
                    sIPEnumCodePhotoExtension = ";" + sIPEnumCodePhotoExtension.Remove(0, 1) + ";";


                    if (!sFileExt.Contains(sIPEnumCodePhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sIPEnumCodeFileName = Path.GetFileName(fupIPEnum.PostedFile.FileName);

                    fupIPEnum.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sIPEnumCodeFileName));
                    sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sIPEnumCodeFileName);                   
                }
                if (sIPEnumCodeFileName != "")
                {
                    if (File.Exists(sDirectory))
                    {
                        IsExists = objFtp.FtpDirectoryExists(mainfolder + objFieldEnum.sEnumDetailsID + "/" + sIPEnumFolderName + "/");
                        if (IsExists == false)
                        {

                            objFtp.createDirectory(mainfolder + objFieldEnum.sEnumDetailsID + "/" + sIPEnumFolderName);
                        }

                        Isuploaded = objFtp.upload(mainfolder + objFieldEnum.sEnumDetailsID + "/" + sIPEnumFolderName + "/" , sIPEnumCodeFileName, sDirectory);
                        if (Isuploaded == true & File.Exists(sDirectory))
                        {
                            File.Delete(sDirectory);
                            objFieldEnum.sIPEnumCodePhotoPath = objFieldEnum.sEnumDetailsID + "/" + sIPEnumFolderName + "/" + sIPEnumCodeFileName;
                            txtIPDTCPath.Text = objFieldEnum.sIPEnumCodePhotoPath;
                        }
                    }
                }

                // DTLMS Code Photo Save

                if (txtDTLMSDTCPath.Text.Trim() != "")
                {
                    sDTLMSCodePhotoExtension = System.IO.Path.GetExtension(txtDTLMSDTCPath.Text).ToString().ToLower();
                    sDTLMSCodePhotoExtension = ";" + sDTLMSCodePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sDTLMSCodePhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sDTLMSCodeFileName = Path.GetFileName(txtDTLMSDTCPath.Text);
                    sDirectory = txtDTLMSDTCPath.Text;
                    objFieldEnum.sDTLMSCodePhotoPath = txtDTLMSDTCPath.Text;
                }

                if (fupDTLMSCodePhoto.PostedFile.ContentLength != 0)
                {

                    sDTLMSCodePhotoExtension = System.IO.Path.GetExtension(fupDTLMSCodePhoto.FileName).ToString().ToLower();
                    sDTLMSCodePhotoExtension = ";" + sDTLMSCodePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sDTLMSCodePhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sDTLMSCodeFileName = Path.GetFileName(fupDTLMSCodePhoto.PostedFile.FileName);

                    fupDTLMSCodePhoto.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sDTLMSCodeFileName));
                    sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sDTLMSCodeFileName);
                  
                }
                if (sDTLMSCodeFileName != "")
                {
                    if (File.Exists(sDirectory))
                    {
                        IsExists = objFtp.FtpDirectoryExists(mainfolder + objFieldEnum.sEnumDetailsID + "/" + sDTLMSFolderName + "/");
                        if (IsExists == false)
                        {

                            objFtp.createDirectory(mainfolder + objFieldEnum.sEnumDetailsID + "/" + sDTLMSFolderName);
                        }

                        Isuploaded = objFtp.upload(mainfolder + objFieldEnum.sEnumDetailsID + "/" + sDTLMSFolderName + "/" , sDTLMSCodeFileName, sDirectory);
                        if (Isuploaded == true & File.Exists(sDirectory))
                        {
                            File.Delete(sDirectory);
                            objFieldEnum.sDTLMSCodePhotoPath = objFieldEnum.sEnumDetailsID + "/" + sDTLMSFolderName + "/" + sDTLMSCodeFileName;
                            txtDTLMSDTCPath.Text = objFieldEnum.sDTLMSCodePhotoPath;
                        }
                    }
                }


                // DTC Photo Save
                if (txtDTCPath.Text.Trim() != "")
                {
                    sDTCPhotoExtension = System.IO.Path.GetExtension(txtDTCPath.Text).ToString().ToLower();
                    sDTCPhotoExtension = ";" + sDTCPhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sDTCPhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sDTCFileName = Path.GetFileName(txtDTCPath.Text);
                    sDirectory = txtDTCPath.Text;
                    objFieldEnum.sDTCPhotoPath = txtDTCPath.Text;
                }
                else if (fupDTCPhoto.PostedFile.ContentLength != 0)
                {

                    sDTCPhotoExtension = System.IO.Path.GetExtension(fupDTCPhoto.FileName).ToString().ToLower();
                    sDTCPhotoExtension = ";" + sDTCPhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sDTCPhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sDTCFileName = Path.GetFileName(fupDTCPhoto.PostedFile.FileName);
                   

                    fupDTCPhoto.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sDTCFileName));
                    sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sDTCFileName);
                }
                if (sDTCFileName != "")
                {
                    if (File.Exists(sDirectory))
                    {
                        IsExists = objFtp.FtpDirectoryExists(mainfolder + objFieldEnum.sEnumDetailsID + "/" + sDTCFolderName + "/");
                        if (IsExists == false)
                        {

                            objFtp.createDirectory(mainfolder + objFieldEnum.sEnumDetailsID + "/" + sDTCFolderName);
                        }

                        Isuploaded = objFtp.upload(mainfolder + objFieldEnum.sEnumDetailsID + "/" + sDTCFolderName + "/" , sDTCFileName, sDirectory);
                        if (Isuploaded == true & File.Exists(sDirectory))
                        {
                            File.Delete(sDirectory);
                            objFieldEnum.sDTCPhotoPath = objFieldEnum.sEnumDetailsID + "/" + sDTCFolderName + "/" + sDTCFileName;
                            txtDTCPath.Text = objFieldEnum.sDTCPhotoPath;
                        }
                    }
                }

                // Infosys Photo Save
                if (txtInfosysPath.Text.Trim() != "")
                {
                    sInfosysCodePhotoExtension = System.IO.Path.GetExtension(txtInfosysPath.Text).ToString().ToLower();
                    sInfosysCodePhotoExtension = ";" + sInfosysCodePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sInfosysCodePhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sInfosysCodeFileName = Path.GetFileName(txtInfosysPath.Text);
                    sDirectory = txtInfosysPath.Text;
                    objFieldEnum.sInfosysCodePhotoPath = txtInfosysPath.Text;
                }
                else if (fupInfosys.PostedFile.ContentLength != 0)
                {
                    sInfosysCodePhotoExtension = System.IO.Path.GetExtension(fupInfosys.FileName).ToString().ToLower();
                    sInfosysCodePhotoExtension = ";" + sInfosysCodePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sInfosysCodePhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sInfosysCodeFileName = Path.GetFileName(fupInfosys.PostedFile.FileName);

                    fupInfosys.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sInfosysCodeFileName));
                    sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sInfosysCodeFileName);                  
                }

                if (sInfosysCodeFileName != "")
                {
                    if (File.Exists(sDirectory))
                    {
                        IsExists = objFtp.FtpDirectoryExists(mainfolder + objFieldEnum.sEnumDetailsID + "/" + sInfosysFolderName + "/");
                        if (IsExists == false)
                        {

                            objFtp.createDirectory(mainfolder + objFieldEnum.sEnumDetailsID + "/" + sInfosysFolderName);
                        }

                        Isuploaded = objFtp.upload(mainfolder + objFieldEnum.sEnumDetailsID + "/" + sInfosysFolderName + "/" , sInfosysCodeFileName, sDirectory);
                        if (Isuploaded == true & File.Exists(sDirectory))
                        {
                            File.Delete(sDirectory);
                            objFieldEnum.sInfosysCodePhotoPath = objFieldEnum.sEnumDetailsID + "/" + sInfosysFolderName + "/" + sInfosysCodeFileName;
                            txtInfosysPath.Text = objFieldEnum.sInfosysCodePhotoPath;
                        }
                    }
                }

                clsException.SaveFunctionExecLog("SaveImagesPath --- END");

                bool bResult;

                clsException.SaveFunctionExecLog("SaveImagePathDetails --- START");

                if (txtEnumDetailsId.Text.Trim() == "")
                {
                     bResult = objFieldEnum.SaveImagePathDetails(objFieldEnum);
                }
                else
                {
                     bResult = objFieldEnum.UpdateImagePathDetails(objFieldEnum);
                }

                clsException.SaveFunctionExecLog("SaveImagePathDetails --- END");
                clsException.SaveFunctionExecLog("ENDImageSaveFunction --- END");
                return bResult;

            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "SaveImages");
                return false;
            }
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                clsException.SaveFunctionExecLog("cmdSave_Click --- START");
                if (ValidateForm() == true)
                {
                    SaveEnumerationDetails();
                    
                    LoadFieldEnumeration();
                }
                clsException.SaveFunctionExecLog("cmdSave_Click --- END");
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdSave_Click");
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
                else
                {
                    cmbDivision.Items.Clear();
                    cmbsubdivision.Items.Clear(); 
                    cmbSection.Items.Clear();
                    cmbFeeder.Items.Clear();
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
                else
                {
                  
                    cmbsubdivision.Items.Clear();
                    cmbSection.Items.Clear();
                    cmbFeeder.Items.Clear();
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
                else
                {
                  
                    cmbSection.Items.Clear();
                    cmbFeeder.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmbsubdivision_SelectedIndexChanged");
            }
        }

        public void LoadFieldEnumeration(string sOperator="")
        {
            try
            {
                clsFieldEnumeration objFieldEnum = new clsFieldEnumeration();
                DataTable dt = new DataTable();
                dt = objFieldEnum.LoadFieldEnumeration(sOperator);
                grdFieldEnumDetails.DataSource = dt;
                grdFieldEnumDetails.DataBind();
                ViewState["Enum"] = dt;
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadFieldEnumeration");
            }
        }

        protected void cmboperator2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmboperator2.SelectedIndex > 0)
                {
                    //LoadFieldEnumeration(cmboperator2.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmboperator2_SelectedIndexChanged");
            }
        }

        protected void rdbOldDtc_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                otherDetails.Attributes.Add("class", "tab-pane fade in active");
                TCDetails.Attributes.Add("class", "tab-pane fade");
                DTCDetails.Attributes.Add("class", "tab-pane fade");
             
                liTCDetails.Attributes.Add("class", "");
                liDTCDetails.Attributes.Add("class", "");
                liOtherDetails.Attributes.Add("class", "active");

                if (txtoldDTCCode.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Old DTC Code(CESC)");
                    return;
                }

               // ResetOtherDetails();

                clsFieldEnumeration objFieldEnum = new clsFieldEnumeration();

                objFieldEnum.sOldDTCCode = txtoldDTCCode.Text;

                objFieldEnum.GetCESCOldData(objFieldEnum);

                txtLatitude.Text = objFieldEnum.sLatitude;
                txtLongitude.Text = objFieldEnum.sLongitude;
                cmbLightArrester.SelectedValue = objFieldEnum.sArresters;
                cmbBreakerType.SelectedValue = objFieldEnum.sBreakertype;
                cmbHTProtection.SelectedValue = objFieldEnum.sHTProtect;
                cmbLTProtection.SelectedValue = objFieldEnum.sLTProtect;
                cmbDTCMetered.SelectedValue = objFieldEnum.sDTCMeters;
                cmbGrounding.SelectedValue = objFieldEnum.sGrounding;
                txtConnectedHP.Text = objFieldEnum.sConnectedHP;
                txtConnectedKW.Text = objFieldEnum.sConnectedKW;

                txtInternalCode.Text = objFieldEnum.sInternalCode;
               // cmbLoadtype.SelectedValue = objFieldEnum.sLoadtype;
               // cmbProjecttype.SelectedValue = objFieldEnum.sProjecttype;
             

            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "rdbOldDtc_CheckedChanged");
            }
        }

        protected void rdbIPEnum_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                otherDetails.Attributes.Add("class", "tab-pane fade in active");
                TCDetails.Attributes.Add("class", "tab-pane fade");
                DTCDetails.Attributes.Add("class", "tab-pane fade");

                liTCDetails.Attributes.Add("class", "");
                liDTCDetails.Attributes.Add("class", "");
                liOtherDetails.Attributes.Add("class", "active");

                if (txtIPDTCCode.Text.Trim() == "")
                {
                    ShowMsgBox("Enter IP Enumeration DTC Code");
                    return;
                }


                clsFieldEnumeration objFieldEnum = new clsFieldEnumeration();

                objFieldEnum.sIPDTCCode = txtIPDTCCode.Text;

                objFieldEnum.GetIPEnumerationData(objFieldEnum);

                txtLatitude.Text = objFieldEnum.sLatitude;
                txtLongitude.Text = objFieldEnum.sLongitude;
                cmbLightArrester.SelectedValue = objFieldEnum.sArresters;
                cmbBreakerType.SelectedValue = objFieldEnum.sBreakertype;
                cmbHTProtection.SelectedValue = objFieldEnum.sHTProtect;
                cmbLTProtection.SelectedValue = objFieldEnum.sLTProtect;
                cmbDTCMetered.SelectedValue = objFieldEnum.sDTCMeters;
                cmbGrounding.SelectedValue = objFieldEnum.sGrounding;
                txtConnectedHP.Text = objFieldEnum.sConnectedHP;
                txtConnectedKW.Text = objFieldEnum.sConnectedKW;

            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "rdbIPEnum_CheckedChanged");
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

        protected void rdbDTLMS_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                otherDetails.Attributes.Add("class", "tab-pane fade in active");
                TCDetails.Attributes.Add("class", "tab-pane fade");

                liTCDetails.Attributes.Add("class", "");
                liOtherDetails.Attributes.Add("class", "active");


                ResetOtherDetails();

            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "rdbDTLMS_CheckedChanged");
            }
        }

        public void RestrictUpdate(string sStatusFlag)
        {
            try
            {
                if (sStatusFlag == "1" || sStatusFlag == "3")
                {
                    cmdSave.Enabled = false;
                    cmdReset.Enabled = false;
                    grdFieldEnumDetails.Columns[10].Visible = false;
                }
                clsQCApproval objQC = new clsQCApproval();

                bool bResult = objQC.CheckEnumerationUpdateAuthority(objSession.UserId);
                if (bResult == false)
                {
                    cmdSave.Enabled = false;
                    cmdReset.Enabled = false;
                    grdFieldEnumDetails.Columns[10].Visible = false;
                }
                

            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message + "-" + objSession.UserId, strFormCode, "RestrictUpdate");
            }
        }

        public void ResetOtherDetails()
        {
            try
            {
                txtInternalCode.Text = string.Empty;
                txtConnectedHP.Text = string.Empty;
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
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ResetOtherDetails");
            }
        }

    
        public void RetainImageinPostback()
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


                //SS Plate Photo

                //Case: 1 When the page is submitted for the first time(First PostBack) and there is file 
                // in FileUpload control but session is Null then Store the values to Session Object as:
                if (Session["fupSSPlate"] == null && fupSSPlate.HasFile)
                {
                    Session["fupSSPlate"] = fupSSPlate;
                    //lblImageName.Text = FileUpload1.FileName;
                }
                // Case 2: On Next PostBack Session has value but FileUpload control is
                // Blank due to PostBack then return the values from session to FileUpload as:
                else if (Session["fupSSPlate"] != null && (!fupSSPlate.HasFile))
                {
                    fupSSPlate = (FileUpload)Session["fupSSPlate"];
                    //lblImageName.Text = FileUpload1.FileName;
                }
                // Case 3: When there is value in Session but user want to change the file then
                // In this case we need to change the file in session object also as:
                else if (fupSSPlate.HasFile)
                {
                    Session["fupSSPlate"] = fupSSPlate;
                    //lblImageName.Text = FileUpload1.FileName;
                }


                // Old Code Photo

                //Case: 1 When the page is submitted for the first time(First PostBack) and there is file 
                // in FileUpload control but session is Null then Store the values to Session Object as:
                if (Session["fupOldCodePhoto"] == null && fupOldCodePhoto.HasFile)
                {
                    Session["fupOldCodePhoto"] = fupOldCodePhoto;
                    //lblImageName.Text = FileUpload1.FileName;
                }
                // Case 2: On Next PostBack Session has value but FileUpload control is
                // Blank due to PostBack then return the values from session to FileUpload as:
                else if (Session["fupOldCodePhoto"] != null && (!fupOldCodePhoto.HasFile))
                {
                    fupOldCodePhoto = (FileUpload)Session["fupOldCodePhoto"];
                    //lblImageName.Text = FileUpload1.FileName;
                }
                // Case 3: When there is value in Session but user want to change the file then
                // In this case we need to change the file in session object also as:
                else if (fupOldCodePhoto.HasFile)
                {
                    Session["fupOldCodePhoto"] = fupOldCodePhoto;
                    //lblImageName.Text = FileUpload1.FileName;
                }


                // IP Enumeration Code Photo

                //Case: 1 When the page is submitted for the first time(First PostBack) and there is file 
                // in FileUpload control but session is Null then Store the values to Session Object as:
                if (Session["fupIPEnum"] == null && fupIPEnum.HasFile)
                {
                    Session["fupIPEnum"] = fupIPEnum;
                    //lblImageName.Text = FileUpload1.FileName;
                }
                // Case 2: On Next PostBack Session has value but FileUpload control is
                // Blank due to PostBack then return the values from session to FileUpload as:
                else if (Session["fupIPEnum"] != null && (!fupIPEnum.HasFile))
                {
                    fupIPEnum = (FileUpload)Session["fupIPEnum"];
                    //lblImageName.Text = FileUpload1.FileName;
                }
                // Case 3: When there is value in Session but user want to change the file then
                // In this case we need to change the file in session object also as:
                else if (fupIPEnum.HasFile)
                {
                    Session["fupIPEnum"] = fupIPEnum;
                    //lblImageName.Text = FileUpload1.FileName;
                }


                // DTLMS Code Photo

                //Case: 1 When the page is submitted for the first time(First PostBack) and there is file 
                // in FileUpload control but session is Null then Store the values to Session Object as:
                if (Session["fupDTLMSCodePhoto"] == null && fupDTLMSCodePhoto.HasFile)
                {
                    Session["fupDTLMSCodePhoto"] = fupDTLMSCodePhoto;
                    //lblImageName.Text = FileUpload1.FileName;
                }
                // Case 2: On Next PostBack Session has value but FileUpload control is
                // Blank due to PostBack then return the values from session to FileUpload as:
                else if (Session["fupDTLMSCodePhoto"] != null && (!fupDTLMSCodePhoto.HasFile))
                {
                    fupDTLMSCodePhoto = (FileUpload)Session["fupDTLMSCodePhoto"];
                    //lblImageName.Text = FileUpload1.FileName;
                }
                // Case 3: When there is value in Session but user want to change the file then
                // In this case we need to change the file in session object also as:
                else if (fupDTLMSCodePhoto.HasFile)
                {
                    Session["fupDTLMSCodePhoto"] = fupDTLMSCodePhoto;
                    //lblImageName.Text = FileUpload1.FileName;
                }


                // DTC Photo

                //Case: 1 When the page is submitted for the first time(First PostBack) and there is file 
                // in FileUpload control but session is Null then Store the values to Session Object as:
                if (Session["fupDTCPhoto"] == null && fupDTCPhoto.HasFile)
                {
                    Session["fupDTCPhoto"] = fupDTCPhoto;
                    //lblImageName.Text = FileUpload1.FileName;
                }
                // Case 2: On Next PostBack Session has value but FileUpload control is
                // Blank due to PostBack then return the values from session to FileUpload as:
                else if (Session["fupDTCPhoto"] != null && (!fupDTCPhoto.HasFile))
                {
                    fupDTCPhoto = (FileUpload)Session["fupDTCPhoto"];
                    //lblImageName.Text = FileUpload1.FileName;
                }
                // Case 3: When there is value in Session but user want to change the file then
                // In this case we need to change the file in session object also as:
                else if (fupDTCPhoto.HasFile)
                {
                    Session["fupDTCPhoto"] = fupDTCPhoto;
                    //lblImageName.Text = FileUpload1.FileName;
                }



                // Infosys Photo

                //Case: 1 When the page is submitted for the first time(First PostBack) and there is file 
                // in FileUpload control but session is Null then Store the values to Session Object as:
                if (Session["fupInfosys"] == null && fupInfosys.HasFile)
                {
                    Session["fupInfosys"] = fupInfosys;
                    //lblImageName.Text = FileUpload1.FileName;
                }
                // Case 2: On Next PostBack Session has value but FileUpload control is
                // Blank due to PostBack then return the values from session to FileUpload as:
                else if (Session["fupInfosys"] != null && (!fupInfosys.HasFile))
                {
                    fupInfosys = (FileUpload)Session["fupInfosys"];
                    //lblImageName.Text = FileUpload1.FileName;
                }
                // Case 3: When there is value in Session but user want to change the file then
                // In this case we need to change the file in session object also as:
                else if (fupInfosys.HasFile)
                {
                    Session["fupInfosys"] = fupInfosys;
                    //lblImageName.Text = FileUpload1.FileName;
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "RetainImageinPostback");
            }
        }

        protected void txtIPDTCCode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                clsFieldEnumeration objFieldEnum = new clsFieldEnumeration();
                objFieldEnum.sIPDTCCode = txtIPDTCCode.Text;
                objFieldEnum.GetIPEnumerationData(objFieldEnum);
                
                txtDTCName.Text = objFieldEnum.sDTCName;
                

                DTCDetails.Attributes.Add("class", "tab-pane fade in active");
                liDTCDetails.Attributes.Add("class", "active");
                liTCDetails.Attributes.Add("class", "");
                liOtherDetails.Attributes.Add("class", "");
                TCDetails.Attributes.Add("class", "tab-pane fade");
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "txtIPDTCCode_TextChanged");
            }
        }

        protected void txtoldDTCCode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                clsFieldEnumeration objFieldEnum = new clsFieldEnumeration();
                objFieldEnum.sOldDTCCode = txtoldDTCCode.Text;
                objFieldEnum.GetCESCOldData(objFieldEnum);

                if (txtDTCName.Text == "")
                {
                    txtDTCName.Text = objFieldEnum.sDTCName;
                }


                DTCDetails.Attributes.Add("class", "tab-pane fade in active");
                liDTCDetails.Attributes.Add("class", "active");
                liTCDetails.Attributes.Add("class", "");
                liOtherDetails.Attributes.Add("class", "");
                TCDetails.Attributes.Add("class", "tab-pane fade");
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
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbMake_SelectedIndexChanged");
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

        public void RetainImageOnPostback()
        {
            try
            {
                clsException.SaveFunctionExecLog("RetainImageOnPostback --- START");

                string sDirectory = string.Empty;

                string sNamePlateFileName = string.Empty;
                string sSSPlateFileName = string.Empty;
                string sOldCodeFileName = string.Empty;
                string sIPEnumCodeFileName = string.Empty;
                string sDTLMSCodeFileName = string.Empty;
                string sDTCFileName = string.Empty;
                string sInfosysCodeFileName = string.Empty;

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

                if (fupDTLMSCodePhoto.HasFile)
                {
                    sDTLMSCodeFileName = Path.GetFileName(fupDTLMSCodePhoto.PostedFile.FileName);
                    fupDTLMSCodePhoto.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sDTLMSCodeFileName));
                    txtDTLMSDTCPath.Text = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sDTLMSCodeFileName);
                }

                if (fupOldCodePhoto.HasFile)
                {
                    sOldCodeFileName = Path.GetFileName(fupOldCodePhoto.PostedFile.FileName);
                    fupOldCodePhoto.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sOldCodeFileName));
                    txtOLDDTCPath.Text = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sOldCodeFileName);
                }

                if (fupIPEnum.HasFile)
                {
                    sIPEnumCodeFileName = Path.GetFileName(fupIPEnum.PostedFile.FileName);
                    fupIPEnum.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sIPEnumCodeFileName));
                    txtIPDTCPath.Text = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sIPEnumCodeFileName);
                }

                if (fupInfosys.HasFile)
                {
                    sInfosysCodeFileName = Path.GetFileName(fupInfosys.PostedFile.FileName);
                    fupInfosys.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sInfosysCodeFileName));
                    txtInfosysPath.Text = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sInfosysCodeFileName);
                }

                if (fupDTCPhoto.HasFile)
                {
                    sDTCFileName = Path.GetFileName(fupDTCPhoto.PostedFile.FileName);
                    fupDTCPhoto.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sDTCFileName));
                    txtDTCPath.Text = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sDTCFileName);
                }

                clsException.SaveFunctionExecLog("RetainImageOnPostback --- END");
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "RetainImageOnPostback");
            }
        }

        public string  ValidateImages()
        {
            clsException.SaveFunctionExecLog("ValidateImages --- START");
            string svalidate = string.Empty;
            try
            {
                //FileType Parameter
                string sPlatePhotoExtension = string.Empty;
                string sSSPlatePhotoExtension = string.Empty;
                string sOldCodePhotoExtension = string.Empty;
                string sIPEnumCodePhotoExtension = string.Empty;
                string sDTLMSCodePhotoExtension = string.Empty;
                string sDTCPhotoExtension = string.Empty;
                string sInfosysCodePhotoExtension = string.Empty;

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

                // Old Code Photo Save

                if (txtOLDDTCPath.Text.Trim() != "")
                {
                    sOldCodePhotoExtension = System.IO.Path.GetExtension(txtOLDDTCPath.Text).ToString().ToLower();
                    sOldCodePhotoExtension = ";" + sOldCodePhotoExtension.Remove(0, 1) + ";";


                    if (!sFileExt.Contains(sOldCodePhotoExtension))
                    {
                        //ShowMsgBox("Invalid Image Format");
                        return "Invalid Image Format in OLD DTC Code(CESC) Photo";
                    }

                }

                else if (fupOldCodePhoto.PostedFile.ContentLength != 0)
                {

                    sOldCodePhotoExtension = System.IO.Path.GetExtension(fupOldCodePhoto.FileName).ToString().ToLower();
                    sOldCodePhotoExtension = ";" + sOldCodePhotoExtension.Remove(0, 1) + ";";


                    if (!sFileExt.Contains(sOldCodePhotoExtension))
                    {
                        //ShowMsgBox("Invalid Image Format");
                        return "Invalid Image Format in OLD DTC Code(CESC) Photo";
                    }

                }

                // IP Enum Code Photo Save
                if (txtIPDTCPath.Text.Trim() != "")
                {
                    sIPEnumCodePhotoExtension = System.IO.Path.GetExtension(txtIPDTCPath.Text).ToString().ToLower();
                    sIPEnumCodePhotoExtension = ";" + sIPEnumCodePhotoExtension.Remove(0, 1) + ";";


                    if (!sFileExt.Contains(sIPEnumCodePhotoExtension))
                    {
                        //ShowMsgBox("Invalid Image Format");
                        return "Invalid Image Format in  DTC Code(IP Enum) Photo";
                    }

                }
                else if (fupIPEnum.PostedFile.ContentLength != 0)
                {

                    sIPEnumCodePhotoExtension = System.IO.Path.GetExtension(fupIPEnum.FileName).ToString().ToLower();
                    sIPEnumCodePhotoExtension = ";" + sIPEnumCodePhotoExtension.Remove(0, 1) + ";";


                    if (!sFileExt.Contains(sIPEnumCodePhotoExtension))
                    {
                        //ShowMsgBox("Invalid Image Format");
                        return "Invalid Image Format in DTC Code(IP Enum) Photo";
                    }

                 }

                  // DTLMS Code Photo Save

                    if (txtDTLMSDTCPath.Text.Trim() != "")
                    {
                        sDTLMSCodePhotoExtension = System.IO.Path.GetExtension(txtDTLMSDTCPath.Text).ToString().ToLower();
                        sDTLMSCodePhotoExtension = ";" + sDTLMSCodePhotoExtension.Remove(0, 1) + ";";

                        if (!sFileExt.Contains(sDTLMSCodePhotoExtension))
                        {
                           // ShowMsgBox("Invalid Image Format");
                            return "Invalid Image Format  in DTC Code(DTLMS) Photo";
                        }
                    }


                    else if (fupDTLMSCodePhoto.PostedFile.ContentLength != 0)
                    {

                        sDTLMSCodePhotoExtension = System.IO.Path.GetExtension(fupDTLMSCodePhoto.FileName).ToString().ToLower();
                        sDTLMSCodePhotoExtension = ";" + sDTLMSCodePhotoExtension.Remove(0, 1) + ";";

                        if (!sFileExt.Contains(sDTLMSCodePhotoExtension))
                        {
                            //ShowMsgBox("Invalid Image Format");
                            return "Invalid Image Format in DTC Code(DTLMS) Photo";
                        }


                   }

                    // DTC Photo Save
                    if (txtDTCPath.Text.Trim() != "")
                    {
                        sDTCPhotoExtension = System.IO.Path.GetExtension(txtDTCPath.Text).ToString().ToLower();
                        sDTCPhotoExtension = ";" + sDTCPhotoExtension.Remove(0, 1) + ";";

                        if (!sFileExt.Contains(sDTCPhotoExtension))
                        {
                            //ShowMsgBox("Invalid Image Format");
                            return "Invalid Image Format in DTC Photo";
                        }

                    }
                    else if (fupDTCPhoto.PostedFile.ContentLength != 0)
                    {

                        sDTCPhotoExtension = System.IO.Path.GetExtension(fupDTCPhoto.FileName).ToString().ToLower();
                        sDTCPhotoExtension = ";" + sDTCPhotoExtension.Remove(0, 1) + ";";

                        if (!sFileExt.Contains(sDTCPhotoExtension))
                        {
                            //ShowMsgBox("Invalid Image Format");
                            return "Invalid Image Format in DTC Photo";
                        }

                    }


                    // Infosys Photo Save
                    if (txtInfosysPath.Text.Trim() != "")
                    {
                        sInfosysCodePhotoExtension = System.IO.Path.GetExtension(txtInfosysPath.Text).ToString().ToLower();
                        sInfosysCodePhotoExtension = ";" + sInfosysCodePhotoExtension.Remove(0, 1) + ";";

                        if (!sFileExt.Contains(sInfosysCodePhotoExtension))
                        {
                            //ShowMsgBox("Invalid Image Format");
                            return "Invalid Image Format in Infosys Asset ID photo";
                        }

                    }
                    else if (fupInfosys.PostedFile.ContentLength != 0)
                    {
                        sInfosysCodePhotoExtension = System.IO.Path.GetExtension(fupInfosys.FileName).ToString().ToLower();
                        sInfosysCodePhotoExtension = ";" + sInfosysCodePhotoExtension.Remove(0, 1) + ";";

                        if (!sFileExt.Contains(sInfosysCodePhotoExtension))
                        {
                           //ShowMsgBox("Invalid Image Format");
                            return "Invalid Image Format in Infosys Asset ID photo";
                        }

                    }

                    clsException.SaveFunctionExecLog("ValidateImages --- END");

                    return "";
            }

            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ValidateImages");
                return ex.Message;
            }
        }

       

        protected void cmdLoadImage_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdLoadImage_Click");

            }
        }

    }
}