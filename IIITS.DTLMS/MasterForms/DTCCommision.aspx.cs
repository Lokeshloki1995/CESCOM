using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Configuration;
using System.IO;
using System.Data;

namespace IIITS.DTLMS.MasterForms
{
    public partial class DTCCommision : System.Web.UI.Page
    {
        string strFormCode = "DTCCommision";
        clsSession objSession;
        string soffcode;
        clsDTCCommision objDtcMaster = new clsDTCCommision();
        string sDTCXmlData = string.Empty;

        /// <summary>
        /// This method used for page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
            {
                Response.Redirect("~/Login.aspx", false);
            }
            Form.DefaultButton = cmdSave.UniqueID;
            objSession = (clsSession)Session["clsSession"];
            lblMessage.Text = string.Empty;
            RetainImageOnPostback();
            txtCommisionDate_CalendarExtender1.EndDate = System.DateTime.Now.AddDays(0);
            txtCommisionDate.Attributes.Add("readonly", "readonly");
            txtServiceDate.Attributes.Add("readonly", "readonly");
            if (!IsPostBack)
            {
                if (objSession.OfficeCode.Length > 2)
                {
                    soffcode = objSession.OfficeCode.Substring(0, 3);
                }
                else
                    soffcode = objSession.OfficeCode;

                string Qry1 = "SELECT FD_FEEDER_CODE ,FD_FEEDER_CODE ||'-'|| FD_FEEDER_NAME FROM TBLFEEDERMAST, ";
                Qry1 += " TBLFEEDEROFFCODE WHERE FD_FEEDER_ID=FDO_FEEDER_ID AND FDO_OFFICE_CODE LIKE'" + soffcode + "%'";
                Genaral.Load_Combo(Qry1, "--Select--", cmbFeeder);
                string Qry2 = "SELECT MD_ID, MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'PT'";
                Genaral.Load_Combo(Qry2, "--Select--", cmbprojecttype);

                Loadmeterdetails();

                if (Request.QueryString["QryDtcId"] != null && Request.QueryString["QryDtcId"].ToString() != "")
                {
                    txtDTCId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["QryDtcId"]));
                    DisablePhotoUpload();
                }

                if (Request.QueryString["DtcCode"] != null && Request.QueryString["DtcCode"].ToString() != "")
                {

                    DtcCodeApprove.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["DtcCode"]));
                    Dtid.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["Dtid"]));
                    if (Request.QueryString["Status"] != null && Request.QueryString["Status"].ToString() != "")
                    {
                        Status.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["Status"]));
                    }
                    DisablePhotoUpload();
                    LoadDtcDetails(Dtid.Text);


                    objDtcMaster.lDtcId = Convert.ToString(txtDTCId.Text);
                    objDtcMaster.GetDTCDetails(objDtcMaster);
                    sDTCXmlData = objDtcMaster.SaveXmlData(objDtcMaster);
                    ViewState["sDTCXmlData"] = sDTCXmlData;

                    SetContralText();
                }

                if (Request.QueryString["WOSlno"] != null && Request.QueryString["WOSlno"].ToString() != "")
                {
                    txtWOslno.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["WOSlno"]));
                    txtTCCode.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["TCCode"]));
                    GetTCDetails();

                }

                if (txtDTCId.Text != "")
                {
                    LoadDtcDetails(txtDTCId.Text);

                    objDtcMaster.lDtcId = Convert.ToString(txtDTCId.Text);
                    objDtcMaster.GetDTCDetails(objDtcMaster);
                    sDTCXmlData = objDtcMaster.SaveXmlData(objDtcMaster);
                    ViewState["sDTCXmlData"] = sDTCXmlData;

                }
                if (cmbDTCMetered.SelectedIndex == 2)
                {
                    cmbMeterstatus.Enabled = false;
                    cmbCtratio.Enabled = false;
                    cmdmake.Enabled = false;
                    txtslno.Enabled = false;
                    cmbWiring.Enabled = false;
                    cmbModem.Enabled = false;
                    Manufactureyear.Enabled = false;
                }
                if (cmbMeterstatus.SelectedIndex == 3)
                {
                    cmbCtratio.Enabled = false;
                    cmdmake.Enabled = false;
                    txtslno.Enabled = false;
                    Manufactureyear.Enabled = false;
                }
                if (cmdmake.SelectedIndex == Convert.ToInt32(ConfigurationManager.AppSettings["Securethreadthrough"]))
                {
                    cmbCtratio.Enabled = false;
                }
                else if(cmbDTCMetered.SelectedIndex != 2 && cmbMeterstatus.SelectedIndex != 3)
                {
                    cmbCtratio.Enabled = true;
                }
                string strQry = string.Empty;
                FilterLocation();

                strQry = "Title=Search and Select TC Details&";
                strQry += "Query=SELECT TC_CODE,TC_SLNO,TM_NAME,TO_CHAR(TC_CAPACITY) TC_CAPACITY ";
                strQry += " FROM TBLTCMASTER,TBLTRANSMAKES  WHERE TC_LOCATION_ID ";
                strQry += " LIKE '" + objSession.OfficeCode + "%' AND TC_MAKE_ID= TM_ID and ";
                strQry += " TC_STATUS in (1,2) AND TC_CURRENT_LOCATION=1 and {0} like %{1}% order by TC_CODE&";
                strQry += "DBColName=TC_CODE~TC_SLNO~TM_NAME&";
                strQry += "ColDisplayName=DTr Code~DTr SlNo~Make Name&";

                strQry = strQry.Replace("'", @"\'");

                cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry
                    + "tb=" + hdfTcCode.ClientID + "&btn=" + cmdSearch.ClientID
                    + "',520,520," + hdfTcCode.ClientID + ")");

                txtCommisionDate.Attributes.Add("onblur", "return ValidateDate("
                    + txtCommisionDate.ClientID + ");");
                txtFeederChngDate.Attributes.Add("onblur", "return ValidateDate("
                    + txtFeederChngDate.ClientID + ");");
                txtServiceDate.Attributes.Add("onblur", "return ValidateDate("
                    + txtServiceDate.ClientID + ");");

                txtDTCCode.Attributes.Add("readonly", "readonly");

                //WorkFlow / Approval
                WorkFlowConfig();
            }

        }
        /// <summary>
        /// This method used to handle visible false
        /// photoUpload,latlong,chkRequiresPainting
        /// </summary>
        private void DisablePhotoUpload()
        {
            try
            {
                photoUpload.Visible = false;
                latlong.Visible = false;
                chkRequiresPainting.Visible = false;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        /// <summary>
        /// This method used to assign photo uploaded values
        /// </summary>
        private void RetainImageOnPostback()
        {
            string sDirectory = string.Empty;
            string sDTLMSCodeFileName = string.Empty; // dtc code photo 
            string sDTCFileName = string.Empty; // entire structure photo 
            string sNamePlateFileName = string.Empty; //name plate photo
            string sSSPlateFileName = string.Empty; // ss plate photo 
            string sOldCodeFileName = string.Empty;
            string sIPEnumCodeFileName = string.Empty;

            string stempDTCCodePhoto1 = string.Empty;
            string stempDTCCodePhoto2 = string.Empty;

            string sInfosysCodeFileName = string.Empty;
            try
            {
                // structure photo  
                if (fupDTCStructure.HasFile)
                {
                    sDTCFileName = Path.GetFileName(fupDTCStructure.PostedFile.FileName);
                    fupDTCStructure.SaveAs(Server.MapPath("~/DTLMSDocs" + "/"
                        + objSession.UserId + "~" + sDTCFileName));
                    txtDTCPath.Text = Server.MapPath("~/DTLMSDocs" + "/"
                        + objSession.UserId + "~" + sDTCFileName);
                }
                //dtc code photo 
                if (fupDTCCode.HasFile)
                {
                    sDTLMSCodeFileName = Path.GetFileName(fupDTCCode.PostedFile.FileName);
                    fupDTCStructure.SaveAs(Server.MapPath("~/DTLMSDocs" + "/"
                        + objSession.UserId + "~" + sDTLMSCodeFileName));
                    txtDTLMSDTCPath.Text = Server.MapPath("~/DTLMSDocs" + "/"
                        + objSession.UserId + "~" + sDTLMSCodeFileName);
                }
                // ss plate 
                if (fupSSplatePhoto.HasFile)
                {
                    sSSPlateFileName = Path.GetFileName(fupSSplatePhoto.PostedFile.FileName);
                    fupSSplatePhoto.SaveAs(Server.MapPath("~/DTLMSDocs" + "/"
                        + objSession.UserId + "~" + sSSPlateFileName));
                    txtSSPlatePath.Text = Server.MapPath("~/DTLMSDocs" + "/"
                        + objSession.UserId + "~" + sSSPlateFileName);
                }
                // name plate photo 
                if (fupNamePlatePhoto.HasFile)
                {
                    sNamePlateFileName = Path.GetFileName(fupNamePlatePhoto.PostedFile.FileName);
                    fupNamePlatePhoto.SaveAs(Server.MapPath("~/DTLMSDocs" + "/"
                        + objSession.UserId + "~" + sNamePlateFileName));
                    txtNamePlatePhotoPath.Text = Server.MapPath("~/DTLMSDocs"
                        + "/" + objSession.UserId + "~" + sNamePlateFileName);
                }
                //tempDTCCode
                if (fupTempDTCCode1.HasFile)
                {
                    stempDTCCodePhoto1 = Path.GetFileName(fupTempDTCCode1.PostedFile.FileName);
                    fupTempDTCCode1.SaveAs(Server.MapPath("~/DTLMSDocs" + "/"
                        + objSession.UserId + "~" + stempDTCCodePhoto1));
                    txtTempDTCCode1Path.Text = Server.MapPath("~/DTLMSDocs" + "/"
                        + objSession.UserId + "~" + stempDTCCodePhoto1);
                }
                if (fupTempDTCCode2.HasFile)
                {
                    stempDTCCodePhoto2 = Path.GetFileName(fupTempDTCCode2.PostedFile.FileName);
                    fupTempDTCCode2.SaveAs(Server.MapPath("~/DTLMSDocs" + "/"
                        + objSession.UserId + "~" + stempDTCCodePhoto2));
                    txtTempDTCCode2Path.Text = Server.MapPath("~/DTLMSDocs" + "/"
                        + objSession.UserId + "~" + stempDTCCodePhoto2);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                  System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        /// <summary>
        /// this method  used to handle the visile false
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkRequiresPainting_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkRequiresPainting.Checked == true)
                {
                    tempDTCCode.Visible = true;
                    finalDTCCode.Visible = false;
                }
                else
                {
                    tempDTCCode.Visible = false;
                    finalDTCCode.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                     System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        /// <summary>
        /// this method used to fetch dtc details 
        /// </summary>
        /// <param name="strId"></param>
        public void LoadDtcDetails(string strId)
        {
            try
            {
                clsDTCCommision objDtcMaster = new clsDTCCommision();
                objDtcMaster.lDtcId = Convert.ToString(strId);

                objDtcMaster.GetDTCDetails(objDtcMaster);

                txtDTCId.Text = Convert.ToString(objDtcMaster.lDtcId);
                txtDTCName.Text = objDtcMaster.sDtcName;
                txtCommisionDate.Text = objDtcMaster.sCommisionDate;
                txtConnectedHP.Text = objDtcMaster.iConnectedHP;
                txtConnectedKW.Text = objDtcMaster.iConnectedKW;
                txtDTCCode.Text = Convert.ToString(objDtcMaster.sDtcCode);
                txtFeederChngDate.Text = objDtcMaster.sFeederChangeDate;
                txtInternalCode.Text = objDtcMaster.sInternalCode;
                txtKWHReading.Text = objDtcMaster.iKWHReading;
                txtOMSection.Text = objDtcMaster.sOMSectionName;
                txtServiceDate.Text = objDtcMaster.sServiceDate;
                cmbprojecttype.SelectedValue = objDtcMaster.sProjecttype;
                cmbFeeder.SelectedValue = objDtcMaster.sFeedercode.Split('-')[0];

                txtCapacity.Text = objDtcMaster.sTCCapacity;
                txtTCMake.Text = objDtcMaster.sTCMakeName;
                txtTCCode.Text = objDtcMaster.sTcCode;
                txtOldTCCode.Text = objDtcMaster.sTcCode;

                cmbDTCMetered.SelectedValue = objDtcMaster.sDTCMeters;
                cmbMeterstatus.SelectedValue = objDtcMaster.MeterStatus;
                cmbCtratio.SelectedValue = objDtcMaster.Ctratio;
                cmbWiring.SelectedValue = objDtcMaster.Wiring;
                cmbModem.SelectedValue = objDtcMaster.Modem;
                Ltstatus.Text = objDtcMaster.Ltstatus;
                txtslno.Text = objDtcMaster.Meterslno;
                cmdmake.SelectedValue = objDtcMaster.Metermake;
                cmbMeterRecording.SelectedValue = objDtcMaster.Meterrecording;
                Manufactureyear.SelectedValue = objDtcMaster.Manufactureyear;
                Remarks.Text = objDtcMaster.Remarks;
                if (Ltstatus.Text == "0")
                {
                    cmdSave.Text = "Update";
                }
                else
                {
                    cmdSave.Text = "Update & Continue";
                }
                cmdNext.Visible = true;
                txtDTCCode.Enabled = false;
                cmbFeeder.Enabled = false;


                hdfDTCPath.Value = objDtcMaster.sDTCPath;
                hdfDTCImagePath.Value = objDtcMaster.sDTCImagePath;
                hdfDTRImagePath.Value = objDtcMaster.sDTrImagePath;
                hdfDTRNamePlatePath.Value = objDtcMaster.sDTrNamePlate;

                ShowUploadedImages();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                     System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        /// <summary>
        /// this method used for save the details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdSave_Click(object sender, EventArgs e)
        {
            clsDTCCommision objDtcCommision = new clsDTCCommision();
            clsFieldEnumeration objfieldEnumeration;
            try
            {
                //Check AccessRights
                bool bAccResult;
                if (cmdSave.Text == "Update")
                {
                    if (Ltstatus.Text == "0")
                    {
                        bAccResult = CheckAccessRights("2");
                    }
                    else
                    {
                        bAccResult = CheckAccessRights("3");
                    }
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
                    string[] Arr = new string[3];
                    objDtcCommision.lDtcId = Convert.ToString(txtDTCId.Text);
                    objDtcCommision.sDtcName = txtDTCName.Text;
                    objDtcCommision.iConnectedHP = Convert.ToString(txtConnectedHP.Text);
                    objDtcCommision.iConnectedKW = Convert.ToString(txtConnectedKW.Text);
                    objDtcCommision.sInternalCode = txtInternalCode.Text;
                    objDtcCommision.sFeederChangeDate = txtFeederChngDate.Text;
                    objDtcCommision.sServiceDate = txtServiceDate.Text;
                    objDtcCommision.sOMSectionName = txtOMSection.Text;
                    objDtcCommision.sDtcCode = txtDTCCode.Text;
                    objDtcCommision.iKWHReading = txtKWHReading.Text;
                    objDtcCommision.sCommisionDate = txtCommisionDate.Text;
                    objDtcCommision.sTcCode = txtTCCode.Text;
                    objDtcCommision.sCrBy = objSession.UserId;
                    objDtcCommision.sOldTcCode = txtOldTCCode.Text;

                    objDtcCommision.sWOslno = txtWOslno.Text;
                    objDtcCommision.sOfficeCode = objSession.OfficeCode;

                    if (cmbprojecttype.SelectedIndex > 0)
                    {
                        objDtcCommision.sProjecttype = cmbprojecttype.SelectedValue;
                    }
                    else
                    {
                        objDtcCommision.sProjecttype = "0";
                    }
                    objDtcCommision.sDTCMeters = cmbDTCMetered.SelectedValue;
                    if (cmbDTCMetered.SelectedIndex == 1)
                    {
                        objDtcCommision.MeterStatus = cmbMeterstatus.SelectedValue;
                        objDtcCommision.Wiring = cmbWiring.SelectedValue;
                        objDtcCommision.Modem = cmbModem.SelectedValue;

                        if (cmbMeterstatus.SelectedIndex != 3)
                        {
                            objDtcCommision.Ctratio = cmbCtratio.SelectedValue;
                            objDtcCommision.Metermake = cmdmake.SelectedValue;
                            objDtcCommision.Meterslno = txtslno.Text;
                            objDtcCommision.Manufactureyear = Manufactureyear.SelectedValue;
                        }
                    }
                    objDtcCommision.RoleId = objSession.RoleId;
                    if (Ltstatus.Text == "0" && objDtcCommision.RoleId ==
                        ConfigurationManager.AppSettings["LTROLEID"])
                    {
                        objDtcCommision.Ltstatus = "1";
                        objDtcCommision.Remarks = Remarks.Text;
                        objDtcCommision.Meterrecording = cmbMeterRecording.SelectedValue;

                    }
                    else
                    {
                        objDtcCommision.Ltstatus = "0";
                    }
                    // if its null then its new DTC commissioning so first save it 
                    //in the enumeration details and in the enumeration and then insert into
                    // the dtc master table .
                    if (objDtcCommision.lDtcId == "" || objDtcCommision.lDtcId == null)
                    {
                        objfieldEnumeration = new clsFieldEnumeration();
                        objfieldEnumeration.sDTCCode = txtDTCCode.Text.Trim();
                        objfieldEnumeration.sDTCName = txtDTCName.Text.Trim().ToUpper();
                        objfieldEnumeration.sFeederCode = cmbFeeder.SelectedValue;
                        objfieldEnumeration.sConnectedHP = Convert.ToString(txtConnectedHP.Text);
                        objfieldEnumeration.sConnectedKW = Convert.ToString(txtConnectedKW.Text);
                        objfieldEnumeration.sInternalCode = txtInternalCode.Text;
                        objfieldEnumeration.sTcCode = txtTCCode.Text;
                        objfieldEnumeration.sTCMake = txtMakeId.Text;
                        objfieldEnumeration.sTCCapacity = txtCapacity.Text;
                        objfieldEnumeration.sCrBy = objfieldEnumeration.sOperator1
                            = objfieldEnumeration.sOperator2 = objSession.UserId;

                        objfieldEnumeration.sOfficeCode = objSession.OfficeCode;

                        if (chkRequiresPainting.Checked == true)
                        {

                            objfieldEnumeration.sIsTempDTCCodePhoto = "TRUE";
                        }
                        else
                        {
                            objfieldEnumeration.sIsTempDTCCodePhoto = "FALSE";
                        }


                        if (cmbprojecttype.SelectedIndex > 0)
                        {
                            objfieldEnumeration.sProjecttype = cmbprojecttype.SelectedValue;
                        }
                        else
                        {
                            objfieldEnumeration.sProjecttype = "0";
                        }
                        //insert into the enumerationdetails
                        Arr = objfieldEnumeration.SaveFieldEnumerationDetails(objfieldEnumeration);

                        if (Arr[1] == "0")
                        {
                            //save the immages 
                            SaveImagesPath(objfieldEnumeration);

                        }
                        if (Arr[1] == "-1")
                        {
                            ShowMsgBox("Exception Occurred");
                            return;
                        }
                        if (Arr[1] == "2")
                        {
                            ShowMsgBox(Arr[0]);
                            return;
                        }
                    }

                    //Workflow
                    WorkFlowObjects(objDtcCommision);

                    objDtcCommision.sFormName = strFormCode;
                    objDtcCommision.sDescription = "DATA MODIFIED BY " + Session["FullName"];
                    objDtcCommision.sXmlData = Convert.ToString(ViewState["sDTCXmlData"]);

                    Arr = objDtcCommision.SaveUpdateDtcDetails(objDtcCommision);

                    if (Arr[1].ToString() == "0")
                    {
                        if (objDtcCommision.Ltstatus == "1" && objDtcCommision.RoleId ==
                            ConfigurationManager.AppSettings["LTROLEID"])
                        {
                            cmdSave.Enabled = false;
                            ShowMsgBox(Arr[0]);
                            return;
                        }

                        txtDTCId.Text = objDtcCommision.lDtcId;
                        cmdSave.Text = "Update";
                        string strDtcId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtDTCId.Text));
                        if (photoUpload.Visible == true)
                        {
                            Response.Redirect("DTCDetails.aspx?QryDtcId=" + strDtcId, false);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), UniqueID,
                                "alert('Saved Successfully'); location.href='DTCDetails.aspx?QryDtcId="
                                + strDtcId + "';", true);

                        }
                    }

                    if (Arr[1].ToString() == "1")
                    {

                        cmdSave.Text = "Update";

                        if (objDtcCommision.Ltstatus == "1" && objDtcCommision.RoleId ==
                            ConfigurationManager.AppSettings["LTROLEID"])
                        {
                            cmdSave.Enabled = false;
                            ShowMsgBox(Arr[0]);
                            return;
                        }

                        //Save data in TBLWFODATA And TBLWORKFLOWOBJECTS 
                        objDtcCommision.GetDTCDetails(objDtcMaster);
                        sDTCXmlData = objDtcCommision.SaveXmlData(objDtcCommision);
                        bool sResult;
                        sResult = objDtcCommision.SaveWorkFlowData(objDtcCommision);

                        objDtcCommision.GetDTCDetails(objDtcMaster);
                        sDTCXmlData = objDtcCommision.SaveXmlData(objDtcCommision);
                        //To get last data
                        ViewState["sDTCXmlData"] = sDTCXmlData;

                        string strDtcId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtDTCId.Text));
                        string sReference = HttpUtility.UrlEncode(Genaral.UrlEncrypt("Update"));
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), UniqueID,
                            "alert('Updated Successfully'); location.href='DTCDetails.aspx?QryDtcId="
                            + strDtcId + "&Ref=" + sReference + "';", true);
                    }

                    if (Arr[1].ToString() == "2")
                    {
                        ShowMsgBox(Arr[0]);
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "DTCMaster");
                        }
                        return;
                    }
                    if (objSession.sTransactionLog == "1")
                    {
                        Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "DTCMaster");
                    }

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                         System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                           System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        /// <summary>
        /// this method used for validation purpose
        /// </summary>
        /// <returns></returns>
        bool ValidateForm()
        {
            bool bValidate = false;
            try
            {

                if (txtDTCCode.Text.Trim().Length < 6)
                {
                    txtDTCCode.Focus();
                    ShowMsgBox("Enter 6 digit DTC Code");
                    return bValidate;

                }
                if (txtDTCCode.Text.Trim().Length == 0)
                {
                    txtDTCCode.Focus();
                    ShowMsgBox("Enter DTC Code");
                    return bValidate;
                }
                if (txtDTCName.Text.Trim().Length == 0)
                {
                    txtDTCName.Focus();
                    ShowMsgBox("Enter DTC Name");
                    return bValidate;
                }
                if (txtOMSection.Text.Trim().Length == 0)
                {
                    txtOMSection.Focus();
                    ShowMsgBox("Enter O & M Section");
                    return bValidate;
                }

                if (txtConnectedKW.Text.Trim() != "")
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(txtConnectedKW.Text,
                        "^(\\d{1,4})?(\\.\\d{1,2})?$"))
                    {
                        ShowMsgBox("Enter valid Connected KW (eg:1234.11)");
                        return false;
                    }

                    if (!System.Text.RegularExpressions.Regex.IsMatch(txtConnectedKW.Text,
                        "[-+]?[0-9]{0,3}\\.?[0-9]{1,2}"))
                    {
                        ShowMsgBox("Enter valid Connected KW (eg:1234.11)");
                        return false;
                    }
                }

                if (txtConnectedHP.Text.Trim() != "")
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(txtConnectedHP.Text,
                        "^(\\d{1,4})?(\\.\\d{1,2})?$"))
                    {
                        ShowMsgBox("Enter valid Connected HP (eg:1234.11)");
                        return false;
                    }

                    if (!System.Text.RegularExpressions.Regex.IsMatch(txtConnectedHP.Text,
                        "[-+]?[0-9]{0,3}\\.?[0-9]{1,2}"))
                    {
                        ShowMsgBox("Enter valid Connected HP (eg:1234.11)");
                        return false;
                    }
                }

                if (txtKWHReading.Text.Trim() != "")
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(txtKWHReading.Text,
                        "^(\\d{1,6})?(\\.\\d{1,2})?$"))
                    {
                        ShowMsgBox("Enter Valid KWH Reading (eg:1234.11)");
                        return false;
                    }

                    if (!System.Text.RegularExpressions.Regex.IsMatch(txtKWHReading.Text,
                        "[-+]?[0-9]{0,5}\\.?[0-9]{0,2}"))
                    {
                        ShowMsgBox("Enter Valid KWH Reading (eg:1234.11)");
                        return false;
                    }
                }
                if (txtTCCode.Text.Trim().Length == 0)
                {
                    txtTCCode.Focus();
                    ShowMsgBox("Enter Valid TC Code");
                    return bValidate;
                }


                if (cmbprojecttype.SelectedIndex > 0)
                {
                    if (cmbprojecttype.SelectedValue == "9" || cmbprojecttype.SelectedValue == "10")
                    {
                        if (txtCommisionDate.Text == "")
                        {
                            txtCommisionDate.Focus();
                            ShowMsgBox("Enter Commission Date");
                            return bValidate;
                        }
                    }
                }

                if (cmbDTCMetered.SelectedIndex == 0)
                {
                    cmbDTCMetered.Focus();
                    ShowMsgBox("Please Select DTC Metering");
                    return false;
                }
                else if (cmbDTCMetered.SelectedValue == "1")
                {
                    if (cmbMeterstatus.SelectedIndex == 0)
                    {
                        cmbMeterstatus.Focus();
                        ShowMsgBox("Please Select Meter Status");
                        return false;
                    }

                    else if (cmbWiring.SelectedIndex == 0)
                    {
                        cmbWiring.Focus();
                        ShowMsgBox("Please Select Wiring");
                        return false;
                    }
                    else if (cmbModem.SelectedIndex == 0)
                    {
                        cmbModem.Focus();
                        ShowMsgBox("Please Select Modem");
                        return false;
                    }
                }

                if (cmbDTCMetered.SelectedValue == "1" && cmbMeterstatus.SelectedValue != "3")
                {

                    if (cmbCtratio.SelectedIndex == 0)
                    {
                        cmbCtratio.Focus();
                        ShowMsgBox("Please Select CT Ratio");
                        return false;
                    }

                    else if (txtslno.Text.Trim().Length == 0)
                    {
                        txtslno.Focus();
                        ShowMsgBox("Please Enter Meter SL No");
                        return bValidate;
                    }

                    else if (cmdmake.SelectedIndex == 0)
                    {
                        cmdmake.Focus();
                        ShowMsgBox("Please Select Meter Make");
                        return false;
                    }

                    else if (Manufactureyear.SelectedIndex == 0)
                    {
                        Manufactureyear.Focus();
                        ShowMsgBox("Please Select Year Of Manufacture");
                        return false;
                    }
                }

                if (objSession.RoleId == ConfigurationManager.AppSettings["LTROLEID"])
                {
                    if (cmbMeterRecording.SelectedIndex == 0)
                    {
                        cmbMeterRecording.Focus();
                        ShowMsgBox("Please Select Meter Recording");
                        return false;
                    }

                    else if (Remarks.Text.Trim().Length == 0)
                    {
                        Remarks.Focus();
                        ShowMsgBox("Please Enter Remarks");
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
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                return bValidate;
            }
        }
        /// <summary>
        /// this method used for validate images
        /// </summary>
        /// <returns></returns>
        private string ValidateImages()
        {
            string svalidate = string.Empty;
            try
            {
                if (latlong.Visible == false)
                {
                    return "";
                }
                //FileType Parameter
                string sDTCStructurePhotoExtension = string.Empty;
                string sDTCCodePhotoExtension = string.Empty;
                string sDtrCodePhotoExtension = string.Empty;
                string sDtrNamePlatePhotoExtension = string.Empty;
                string sTempDTCCodePhoto1 = string.Empty;
                string sTempDTCCodePhoto2 = string.Empty;


                //  Photo Save DTLMSDocs
                string sFileExt = Convert.ToString(ConfigurationManager.AppSettings["FileFormat"]);

                // if checkbox selected then its for tempDTCCcode phot upload
                if (chkRequiresPainting.Checked == true)
                {
                    if (!fupTempDTCCode1.HasFile)
                        return "Please upload DTC(DTLMS) Code Photo";
                    if (!fupTempDTCCode2.HasFile)
                        return "Please upload DTC Structure Photo";
                    if (txtTempDTCCode1Path.Text.Trim() != "")
                    {
                        sTempDTCCodePhoto1 = Path.GetExtension(txtTempDTCCode1Path.Text).ToString().ToLower();
                        sTempDTCCodePhoto1 = ";" + sTempDTCCodePhoto1.Remove(0, 1) + ";";

                        if (!sFileExt.Contains(sTempDTCCodePhoto1))
                        {
                            //ShowMsgBox("Invalid Image Format");
                            return "Invalid Image Format in Temp DTCCode1";
                        }
                    }
                    else if (fupTempDTCCode1.PostedFile.ContentLength != 0)
                    {
                        sTempDTCCodePhoto1 = Path.GetExtension(fupTempDTCCode1.FileName).ToString().ToLower();
                        sTempDTCCodePhoto1 = ";" + sTempDTCCodePhoto1.Remove(0, 1) + ";";
                        if (!sFileExt.Contains(sTempDTCCodePhoto1))
                        {
                            //ShowMsgBox("Invalid Image Format");
                            return "Invalid Image Format in DTLMS Structure Photo";
                        }
                    }
                    //tmepDTCCode2
                    if (txtTempDTCCode2Path.Text.Trim() != "")
                    {
                        sTempDTCCodePhoto1 = Path.GetExtension(txtTempDTCCode2Path.Text).ToString().ToLower();
                        sTempDTCCodePhoto1 = ";" + sTempDTCCodePhoto1.Remove(0, 1) + ";";

                        if (!sFileExt.Contains(sTempDTCCodePhoto1))
                        {
                            //ShowMsgBox("Invalid Image Format");
                            return "Invalid Image Format in Temp DTCCode1";
                        }
                    }
                    else if (fupTempDTCCode2.PostedFile.ContentLength != 0)
                    {
                        sTempDTCCodePhoto2 = Path.GetExtension(fupTempDTCCode2.FileName).ToString().ToLower();
                        sTempDTCCodePhoto2 = ";" + sTempDTCCodePhoto2.Remove(0, 1) + ";";
                        if (!sFileExt.Contains(sTempDTCCodePhoto1))
                        {
                            //ShowMsgBox("Invalid Image Format");
                            return "Invalid Image Format in DTLMS Structure Photo";
                        }
                    }
                }

                else
                {
                    if (!fupDTCCode.HasFile)
                        return "Please upload DTC(DTLMS) Code Photo";
                    if (!fupDTCStructure.HasFile)
                        return "Please upload DTC Structure Photo";
                    if (!fupNamePlatePhoto.HasFile)
                        return "Please Upload Name Plate Photo";
                    if (!fupSSplatePhoto.HasFile)
                        return "Please Upload SS Plate Photo";

                    //Structure  Photo

                    if (txtDTCPath.Text.Trim() != "")
                    {
                        sDTCStructurePhotoExtension = Path.GetExtension(txtDTCPath.Text).ToString().ToLower();
                        sDTCStructurePhotoExtension = ";" + sDTCStructurePhotoExtension.Remove(0, 1) + ";";

                        if (!sFileExt.Contains(sDTCStructurePhotoExtension))
                        {
                            //ShowMsgBox("Invalid Image Format");
                            return "Invalid Image Format in DTLMS Structure Photo";
                        }
                    }
                    else if (fupDTCStructure.PostedFile.ContentLength != 0)
                    {
                        sDTCStructurePhotoExtension = Path.GetExtension(fupDTCStructure.FileName).ToString().ToLower();
                        sDTCStructurePhotoExtension = ";" + sDTCStructurePhotoExtension.Remove(0, 1) + ";";
                        if (!sFileExt.Contains(sDTCStructurePhotoExtension))
                        {
                            //ShowMsgBox("Invalid Image Format");
                            return "Invalid Image Format in DTLMS Structure Photo";
                        }
                    }

                    // DTLMS Code Photo 
                    if (txtDTLMSDTCPath.Text.Trim() != "")
                    {
                        sDTCCodePhotoExtension = Path.GetExtension(txtDTLMSDTCPath.Text).ToString().ToLower();
                        sDTCCodePhotoExtension = ";" + sDTCCodePhotoExtension.Remove(0, 1) + ";";

                        if (!sFileExt.Contains(sDTCCodePhotoExtension))
                        {
                            //ShowMsgBox("Invalid Image Format");
                            return "Invalid Image Format in DTLMS Code Photo";
                        }
                    }
                    else if (fupDTCCode.PostedFile.ContentLength != 0)
                    {
                        sDTCCodePhotoExtension = Path.GetExtension(fupDTCCode.FileName).ToString().ToLower();
                        sDTCCodePhotoExtension = ";" + sDTCCodePhotoExtension.Remove(0, 1) + ";";
                        if (!sFileExt.Contains(sDTCCodePhotoExtension))
                        {
                            //ShowMsgBox("Invalid Image Format");
                            return "Invalid Image Format in DTLMS Structure Photo";
                        }
                    }

                    // ss  plate photo 
                    if (txtSSPlatePath.Text.Trim() != "")
                    {
                        sDtrCodePhotoExtension = Path.GetExtension(txtSSPlatePath.Text).ToString().ToLower();
                        sDtrCodePhotoExtension = ";" + sDtrCodePhotoExtension.Remove(0, 1) + ";";

                        if (!sFileExt.Contains(sDtrCodePhotoExtension))
                        {
                            //ShowMsgBox("Invalid Image Format");
                            return "Invalid Image Format in SS plate photo";
                        }

                    }

                    else if (fupSSplatePhoto.PostedFile.ContentLength != 0)
                    {

                        sDtrCodePhotoExtension = Path.GetExtension(fupSSplatePhoto.FileName).ToString().ToLower();
                        sDtrCodePhotoExtension = ";" + sDtrCodePhotoExtension.Remove(0, 1) + ";";

                        if (!sFileExt.Contains(sDtrCodePhotoExtension))
                        {
                            //ShowMsgBox("Invalid Image Format");
                            return "Invalid Image Format in SS plate photo";
                        }
                    }

                    // name   plate photo 
                    if (txtNamePlatePhotoPath.Text.Trim() != "")
                    {
                        sDtrNamePlatePhotoExtension = Path.GetExtension(txtNamePlatePhotoPath.Text).ToString().ToLower();
                        sDtrNamePlatePhotoExtension = ";" + sDtrNamePlatePhotoExtension.Remove(0, 1) + ";";

                        if (!sFileExt.Contains(sDtrNamePlatePhotoExtension))
                        {
                            //ShowMsgBox("Invalid Image Format");
                            return "Invalid Image Format in SS plate photo";
                        }

                    }
                    else if (fupNamePlatePhoto.PostedFile.ContentLength != 0)
                    {

                        sDtrNamePlatePhotoExtension = Path.GetExtension(fupNamePlatePhoto.FileName).ToString().ToLower();
                        sDtrNamePlatePhotoExtension = ";" + sDtrNamePlatePhotoExtension.Remove(0, 1) + ";";

                        if (!sFileExt.Contains(sDtrNamePlatePhotoExtension))
                        {
                            //ShowMsgBox("Invalid Image Format");
                            return "Invalid Image Format in SS plate photo";
                        }
                    }
                }


                return "";
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                     System.Reflection.MethodBase.GetCurrentMethod().Name);
                return svalidate;
            }
        }
        /// <summary>
        /// this method used for save the images path
        /// </summary>
        /// <param name="objFieldEnum"></param>
        /// <returns></returns>
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

                string sDTLMSCodeFileName = string.Empty;
                string sDTCFileName = string.Empty;

                string sTempDTCCode1Filename = string.Empty;
                string sTempDTCCode2Filename = string.Empty;

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

                string sDTLMSCodePhotoExtension = string.Empty;
                string sDTCPhotoExtension = string.Empty;

                string stempDTCCode1PhotoExtension = string.Empty;
                string stempDTCCode2PhotoExtension = string.Empty;

                clsException.SaveFunctionExecLog("SaveImagesPath --- START");

                //  Photo Save DTLMSDocs
                string sFileExt = Convert.ToString(ConfigurationManager.AppSettings["FileFormat"]);
                string mainfolder = Convert.ToString(ConfigurationManager.AppSettings["SFTPmainfolder1"]);

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

                //sFTPLink = "ftp://192.168.4.123/DTLMSIMAGES/";
                //sFTPUserName = "FTP_DTLMS";
                //sFTPPassword = "cesc123+";
                sFTPLink = Convert.ToString(ConfigurationManager.AppSettings["SFTPPath"].ToUpper());
                clsSFTP objFtp = new clsSFTP(sFTPLink, sFTPUserName, sFTPPassword);
                bool Isuploaded;

                string sNamePlateFolderName = Convert.ToString(ConfigurationManager.
                    AppSettings["NamePlateFolder"].ToUpper());
                string sSSPlateFolderName = Convert.ToString(ConfigurationManager.
                    AppSettings["SSPlateFolder"].ToUpper());
                string sDTLMSFolderName = Convert.ToString(ConfigurationManager.
                    AppSettings["DTLMSCodeFolder"].ToUpper());
                string sDTCFolderName = Convert.ToString(ConfigurationManager.
                    AppSettings["DTCPhoto"].ToUpper());
                string sTempDTCCode1FolderName = Convert.ToString(ConfigurationManager.
                    AppSettings["TempDTCCodePhoto1"].ToUpper());


                // Create Directory
                bool IsExists = objFtp.FtpDirectoryExists(mainfolder);
                if (IsExists == false)
                {

                    objFtp.createDirectory(mainfolder);
                }


                IsExists = objFtp.FtpDirectoryExists(mainfolder + objFieldEnum.sEnumDetailsID + "/");
                if (IsExists == false)
                {

                    objFtp.createDirectory(mainfolder + objFieldEnum.sEnumDetailsID);
                }


                // Name Plate Photo Save
                if (txtNamePlatePhotoPath.Text.Trim() != "")
                {
                    sPlatePhotoExtension = Path.GetExtension(txtNamePlatePhotoPath.Text).ToString().ToLower();
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
                //else if (fupNamePlatePhoto.PostedFile.ContentLength != 0)
                else if (fupNamePlatePhoto.HasFile)
                {

                    sPlatePhotoExtension = Path.GetExtension(fupNamePlatePhoto.FileName).ToString().ToLower();
                    sPlatePhotoExtension = ";" + sPlatePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sPlatePhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sPlateFileName = Path.GetFileName(fupNamePlatePhoto.PostedFile.FileName);

                    fupNamePlatePhoto.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId
                        + "~" + sPlateFileName));
                    sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~"
                        + sPlateFileName);

                }

                if (sPlateFileName != "")
                {
                    if (File.Exists(sDirectory))
                    {
                        IsExists = objFtp.FtpDirectoryExists(mainfolder
                            + objFieldEnum.sEnumDetailsID + "/" + sNamePlateFolderName + "/");
                        if (IsExists == false)
                        {

                            objFtp.createDirectory(mainfolder + objFieldEnum.sEnumDetailsID + "/"
                                + sNamePlateFolderName);
                        }

                        Isuploaded = objFtp.upload(mainfolder + objFieldEnum.sEnumDetailsID
                            + "/" + sNamePlateFolderName + "/", sPlateFileName, sDirectory);
                        if (Isuploaded == true & File.Exists(sDirectory))
                        {
                            File.Delete(sDirectory);
                            objFieldEnum.sNamePlatePhotoPath = objFieldEnum.sEnumDetailsID
                                + "/" + sNamePlateFolderName + "/" + sPlateFileName;
                            txtNamePlatePhotoPath.Text = objFieldEnum.sNamePlatePhotoPath;
                        }
                    }
                }

                //return true;
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

                // else if (fupSSplatePhoto.PostedFile.ContentLength != 0)
                else if (fupSSplatePhoto.HasFile)
                {

                    sSSPlatePhotoExtension = System.IO.Path.GetExtension(fupSSplatePhoto.FileName).ToString().ToLower();
                    sSSPlatePhotoExtension = ";" + sSSPlatePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sSSPlatePhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sSSPlateFileName = Path.GetFileName(fupSSplatePhoto.PostedFile.FileName);

                    fupSSplatePhoto.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sSSPlateFileName));
                    sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sSSPlateFileName);

                }
                if (sSSPlateFileName != "")
                {
                    if (File.Exists(sDirectory))
                    {
                        IsExists = objFtp.FtpDirectoryExists(mainfolder + objFieldEnum.sEnumDetailsID
                            + "/" + sSSPlateFolderName + "/");
                        if (IsExists == false)
                        {

                            objFtp.createDirectory(mainfolder + objFieldEnum.sEnumDetailsID + "/"
                                + sSSPlateFolderName);
                        }

                        Isuploaded = objFtp.upload(mainfolder + objFieldEnum.sEnumDetailsID + "/"
                            + sSSPlateFolderName + "/", sSSPlateFileName, sDirectory);
                        if (Isuploaded == true & File.Exists(sDirectory))
                        {
                            File.Delete(sDirectory);
                            objFieldEnum.sSSPlatePhotoPath = objFieldEnum.sEnumDetailsID
                                + "/" + sSSPlateFolderName + "/" + sSSPlateFileName;
                            txtSSPlatePath.Text = objFieldEnum.sSSPlatePhotoPath;
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

                //if (fupDTCCode.PostedFile.ContentLength != 0)
                if (fupDTCCode.HasFile)
                {

                    sDTLMSCodePhotoExtension = System.IO.Path.GetExtension(fupDTCCode.FileName).ToString().ToLower();
                    sDTLMSCodePhotoExtension = ";" + sDTLMSCodePhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sDTLMSCodePhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sDTLMSCodeFileName = Path.GetFileName(fupDTCCode.PostedFile.FileName);

                    fupDTCCode.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sDTLMSCodeFileName));
                    sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sDTLMSCodeFileName);

                }
                if (sDTLMSCodeFileName != "")
                {
                    if (File.Exists(sDirectory))
                    {
                        IsExists = objFtp.FtpDirectoryExists(mainfolder + objFieldEnum.sEnumDetailsID
                            + "/" + sDTLMSFolderName + "/");
                        if (IsExists == false)
                        {

                            objFtp.createDirectory(mainfolder + objFieldEnum.sEnumDetailsID + "/"
                                + sDTLMSFolderName);
                        }

                        Isuploaded = objFtp.upload(mainfolder + objFieldEnum.sEnumDetailsID + "/"
                            + sDTLMSFolderName + "/", sDTLMSCodeFileName, sDirectory);
                        if (Isuploaded == true & File.Exists(sDirectory))
                        {
                            File.Delete(sDirectory);
                            objFieldEnum.sDTLMSCodePhotoPath = objFieldEnum.sEnumDetailsID + "/"
                                + sDTLMSFolderName + "/" + sDTLMSCodeFileName;
                            txtDTLMSDTCPath.Text = objFieldEnum.sDTLMSCodePhotoPath;
                        }
                    }
                }


                // DTC Photo Save or structure 
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
                if (fupDTCStructure.HasFile)
                {

                    sDTCPhotoExtension = System.IO.Path.GetExtension(fupDTCStructure.FileName).ToString().ToLower();
                    sDTCPhotoExtension = ";" + sDTCPhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(sDTCPhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sDTCFileName = Path.GetFileName(fupDTCStructure.PostedFile.FileName);


                    fupDTCStructure.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sDTCFileName));
                    sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~" + sDTCFileName);
                }
                if (sDTCFileName != "")
                {
                    if (File.Exists(sDirectory))
                    {
                        IsExists = objFtp.FtpDirectoryExists(mainfolder + objFieldEnum.sEnumDetailsID + "/"
                            + sDTCFolderName + "/");
                        if (IsExists == false)
                        {

                            objFtp.createDirectory(mainfolder + objFieldEnum.sEnumDetailsID + "/" + sDTCFolderName);
                        }

                        Isuploaded = objFtp.upload(mainfolder + objFieldEnum.sEnumDetailsID + "/" + sDTCFolderName
                            + "/", sDTCFileName, sDirectory);
                        if (Isuploaded == true & File.Exists(sDirectory))
                        {
                            File.Delete(sDirectory);
                            objFieldEnum.sDTCPhotoPath = objFieldEnum.sEnumDetailsID + "/" + sDTCFolderName
                                + "/" + sDTCFileName;
                            txtDTCPath.Text = objFieldEnum.sDTCPhotoPath;
                        }
                    }
                }

                // temp DTCCode Photo Save
                if (txtTempDTCCode1Path.Text.Trim() != "")
                {
                    stempDTCCode1PhotoExtension = Path.GetExtension(txtTempDTCCode1Path.Text).ToString().ToLower();
                    stempDTCCode1PhotoExtension = ";" + stempDTCCode1PhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(stempDTCCode1PhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sTempDTCCode1Filename = Path.GetFileName(txtTempDTCCode1Path.Text);
                    sDirectory = txtTempDTCCode1Path.Text;
                    objFieldEnum.sTempDTCCode1PhotoPath = txtTempDTCCode1Path.Text;
                }

                //else if (fupTempDTCCode1.PostedFile.ContentLength != 0)
                else if (fupTempDTCCode1.HasFile)
                {

                    stempDTCCode1PhotoExtension = Path.GetExtension(fupTempDTCCode1.FileName).ToString().ToLower();
                    stempDTCCode1PhotoExtension = ";" + stempDTCCode1PhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(stempDTCCode1PhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sTempDTCCode1Filename = Path.GetFileName(fupTempDTCCode1.PostedFile.FileName);


                    fupTempDTCCode1.SaveAs(Server.MapPath("~/DTLMSDocs" + "/"
                        + objSession.UserId + "~" + sTempDTCCode1Filename));
                    sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId
                        + "~" + sTempDTCCode1Filename);
                }
                if (sTempDTCCode1Filename != "")
                {
                    if (File.Exists(sDirectory))
                    {
                        IsExists = objFtp.FtpDirectoryExists(mainfolder + objFieldEnum.sEnumDetailsID + "/"
                            + sTempDTCCode1FolderName + "/");
                        if (IsExists == false)
                        {

                            objFtp.createDirectory(mainfolder + objFieldEnum.sEnumDetailsID
                                + "/" + sTempDTCCode1FolderName);
                        }

                        Isuploaded = objFtp.upload(mainfolder + objFieldEnum.sEnumDetailsID
                            + "/" + sTempDTCCode1FolderName
                            + "/", sTempDTCCode1Filename, sDirectory);
                        if (Isuploaded == true & File.Exists(sDirectory))
                        {
                            File.Delete(sDirectory);
                            objFieldEnum.sTempDTCCode1PhotoPath = objFieldEnum.sEnumDetailsID + "/"
                                + sTempDTCCode1FolderName + "/" + sTempDTCCode1Filename;
                            txtTempDTCCode1Path.Text = objFieldEnum.sTempDTCCode1PhotoPath;
                        }
                    }
                }

                // for another temp DTC Code 

                if (txtTempDTCCode2Path.Text.Trim() != "")
                {
                    stempDTCCode2PhotoExtension = Path.GetExtension(txtTempDTCCode2Path.Text).ToString().ToLower();
                    stempDTCCode2PhotoExtension = ";" + stempDTCCode2PhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(stempDTCCode2PhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sTempDTCCode2Filename = Path.GetFileName(txtTempDTCCode2Path.Text);
                    sDirectory = txtTempDTCCode2Path.Text;
                    objFieldEnum.stempDTCCode2PhotoPath = txtTempDTCCode2Path.Text;
                }

                //  else if (fupTempDTCCode2.PostedFile.ContentLength != 0)
                else if (fupTempDTCCode2.HasFile)
                {

                    stempDTCCode2PhotoExtension = Path.GetExtension(fupTempDTCCode2.FileName).ToString().ToLower();
                    stempDTCCode2PhotoExtension = ";" + stempDTCCode2PhotoExtension.Remove(0, 1) + ";";

                    if (!sFileExt.Contains(stempDTCCode2PhotoExtension))
                    {
                        ShowMsgBox("Invalid Image Format");
                        return false;
                    }

                    sTempDTCCode2Filename = Path.GetFileName(fupTempDTCCode2.PostedFile.FileName);


                    fupTempDTCCode2.SaveAs(Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId
                        + "~" + sTempDTCCode2Filename));
                    sDirectory = Server.MapPath("~/DTLMSDocs" + "/" + objSession.UserId + "~"
                        + sTempDTCCode2Filename);
                }
                if (sTempDTCCode2Filename != "")
                {
                    if (File.Exists(sDirectory))
                    {
                        IsExists = objFtp.FtpDirectoryExists(mainfolder + objFieldEnum.sEnumDetailsID
                            + "/" + sTempDTCCode1FolderName + "/");
                        if (IsExists == false)
                        {

                            objFtp.createDirectory(mainfolder + objFieldEnum.sEnumDetailsID + "/"
                                + sTempDTCCode1FolderName);
                        }

                        Isuploaded = objFtp.upload(mainfolder + objFieldEnum.sEnumDetailsID + "/"
                            + sTempDTCCode1FolderName
                            + "/", sTempDTCCode2Filename, sDirectory);
                        if (Isuploaded == true & File.Exists(sDirectory))
                        {
                            File.Delete(sDirectory);
                            objFieldEnum.stempDTCCode2PhotoPath = objFieldEnum.sEnumDetailsID + "/"
                                + sTempDTCCode1FolderName
                                + "/" + sTempDTCCode2Filename;
                            txtTempDTCCode2Path.Text = objFieldEnum.stempDTCCode2PhotoPath;
                        }
                    }
                }


                bool bResult;
                bResult = objFieldEnum.SaveImagePathDetails(objFieldEnum);



                return bResult;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveImages");
                return false;
            }
        }
        /// <summary>
        /// this method used to show pop up message
        /// </summary>
        /// <param name="sMsg"></param>
        private void ShowMsgBox(string sMsg)
        {
            try
            {
                string sShowMsg = string.Empty;
                sShowMsg = "<script language=javascript> alert ('" + sMsg + "')</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "Msg", sShowMsg);


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowMsgBox");
            }
        }
        /// <summary>
        /// this method used to reset the fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtTCCode.Text = string.Empty;
                hdfTcCode.Value = string.Empty;
                txtServiceDate.Text = string.Empty;
                txtKWHReading.Text = string.Empty;
                txtInternalCode.Text = string.Empty;
                txtFeederChngDate.Text = string.Empty;
                txtDTCName.Text = string.Empty;
                txtDTCId.Text = string.Empty;
                txtDTCCode.Text = string.Empty;
                txtConnectedKW.Text = string.Empty;
                txtConnectedHP.Text = string.Empty;
                txtCommisionDate.Text = string.Empty;
                txtTCMake.Text = string.Empty;
                txtCapacity.Text = string.Empty;

                txtDTCCode.Enabled = true;
                txtDTCId.Text = string.Empty;
                cmbFeeder.SelectedValue = "--Select--";

                cmdNext.Visible = false;
                FilterLocation();

                if (txtOMSection.Enabled == true)
                {
                    txtOMSection.Text = string.Empty;
                }
                Resetmeterdetails();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdReset_Click");
            }
        }
        /// <summary>
        /// this method used to close the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmdClose.Text == "Close")
                {
                    Response.Redirect("DTCDetailsApproveView.aspx", false);
                }
                else
                {
                    Response.Redirect("DTCView.aspx", false);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdClose_Click");
            }
        }
        /// <summary>
        /// This method used for search click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                clsDtcMaster objDTcMaster = new clsDtcMaster();

                objDTcMaster.sTcCode = hdfTcCode.Value;

                objDTcMaster.GetTCDetails(objDTcMaster);

                txtTCMake.Text = objDTcMaster.sTCMakeName;
                txtCapacity.Text = objDTcMaster.sTCCapacity;
                txtTCCode.Text = objDTcMaster.sTcCode;


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdSearch_Click");
            }
        }
        /// <summary>
        /// This  method used to redirect for dtcdetails form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdNext_Click(object sender, EventArgs e)
        {
            try
            {
                string strDtcId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtDTCId.Text));
                Response.Redirect("DTCDetails.aspx?QryDtcId=" + strDtcId + "", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdNext_Click");
            }

        }
        /// <summary>
        /// this method used for filter location
        /// </summary>
        public void FilterLocation()
        {
            try
            {
                string strQry = string.Empty;

                if (objSession.OfficeCode.Trim().Length == 4)
                {
                    txtOMSection.Text = objSession.OfficeCode;
                    btnOmSearch.Visible = false;
                    txtOMSection.Enabled = false;
                }
                else
                {
                    strQry = "Title=Search and Select Location Details&";
                    strQry += "Query=select OM_CODE,OM_NAME FROM TBLOMSECMAST  WHERE OM_CODE LIKE '"
                        + objSession.OfficeCode + "%' AND {0} like %{1}% order by OM_NAME&";
                    strQry += "DBColName=OM_NAME~OM_CODE&";
                    strQry += "ColDisplayName=OM_NAME~OM_CODE&";

                    strQry = strQry.Replace("'", @"\'");

                    btnOmSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?"
                        + strQry + "tb=" + txtOMSection.ClientID + "&btn="
                        + btnOmSearch.ClientID + "',520,520," + txtOMSection.ClientID + ")");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "FilterLocation");
            }
        }
        /// <summary>
        /// this method used to check the access rights
        /// </summary>
        /// <param name="sAccessType"></param>
        /// <returns></returns>
        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "DTCCommision";
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

        public void GetTCDetails()
        {
            try
            {
                clsInvoice objInvoice = new clsInvoice();
                objInvoice.sTcCode = txtTCCode.Text;

                objInvoice.GetTCDetails(objInvoice);

                txtMakeId.Text = objInvoice.sTcMake.Split('~')[0];
                txtTCMake.Text = objInvoice.sTcMake.Split('~')[1];
                txtCapacity.Text = objInvoice.sTcCapacity;

                cmdSearch.Visible = false;
                txtTCCode.Enabled = false;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                      System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }


        public void WorkFlowObjects(clsDTCCommision objDTCComm)
        {
            try
            {
                string sClientIP = string.Empty;

                string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(ip))
                {
                    string[] ipRange = ip.Split(',');
                    int le = ipRange.Length - 1;
                    sClientIP = ipRange[0];
                }
                else
                {
                    sClientIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }


                objDTCComm.sFormName = "DTCCommision";
                objDTCComm.sOfficeCode = objSession.OfficeCode;
                objDTCComm.sClientIP = sClientIP;
                objDTCComm.sWFOId = txtWFOId.Text;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                      System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        public void WorkFlowConfig()
        {
            try
            {
                if (Request.QueryString["ActionType"] != null && Request.QueryString["ActionType"].ToString() != "")
                {


                    if (Session["WFOId"] != null && Session["WFOId"].ToString() != "")
                    {

                        txtWFOId.Text = Convert.ToString(Session["WFOId"]);


                        Session["WFOId"] = null;
                        Session["WFDataId"] = null;
                        Session["WFOAutoId"] = null;
                    }

                    //SetControlText();
                    if (txtActiontype.Text == "V")
                    {
                        cmdSave.Text = "View";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                      System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        protected void lnkDTCHistory_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtDTCId.Text.Trim() != "")
                {
                    string sDTCCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtDTCCode.Text));
                    Response.Redirect("/Transaction/DTCTracker.aspx?DTCCode=" + sDTCCode, false);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                      System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }


        public void ShowUploadedImages()
        {
            try
            {
                string ftpLink = string.Empty;
                ftpLink = ConfigurationManager.AppSettings["VirtualDirectoryPath"].ToString();

                if (hdfDTCImagePath.Value != "")
                {
                    dvDTCCode.Style.Add("display", "block");
                    imgDTCCode.ImageUrl = ftpLink + hdfDTCImagePath.Value;
                }
                if (hdfDTCPath.Value != "")
                {
                    dvDTCPhoto.Style.Add("display", "block");
                    imgDTCPhoto.ImageUrl = ftpLink + hdfDTCPath.Value;
                }
                if (hdfDTRImagePath.Value != "")
                {
                    dvDTrCode.Style.Add("display", "block");
                    imgDTrCode.ImageUrl = ftpLink + hdfDTRImagePath.Value;
                }
                if (hdfDTRNamePlatePath.Value != "")
                {
                    divDTRNamePlate.Style.Add("display", "block");
                    imgDTRNamePlate.ImageUrl = ftpLink + hdfDTRNamePlatePath.Value;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                      System.Reflection.MethodBase.GetCurrentMethod().Name);

            }
        }

        /// <summary>
        /// this method used to call the autogenerate dtc code function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbFeeder_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string feeder = cmbFeeder.SelectedValue.Substring(0, 4);

                string autoDTCCode = AutoGenerateDTCCode(feeder);
                if (autoDTCCode.Length > 0)
                {
                    txtDTCCode.Text = autoDTCCode;
                    lblAutoDTCCode.Text = "Auto Generated DTC Code";
                }
                txtDTCCode.Focus();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                      System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        /// <summary>
        /// this method used to fetch the autogenerate dtc code
        /// </summary>
        /// <param name="feeder"></param>
        /// <returns></returns>
        private string AutoGenerateDTCCode(string feeder)
        {
            string autoDTCCode = string.Empty;
            try
            {
                clsDTCCommision objDtccommission = new clsDTCCommision();
                objDtccommission.sFeedercode = feeder;
                autoDTCCode = objDtccommission.AutoGenerateDTCCode(objDtccommission);
                if (autoDTCCode.Contains("Invalid"))
                {
                    ShowMsgBox(autoDTCCode);
                    return "";
                }
                return autoDTCCode;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                      System.Reflection.MethodBase.GetCurrentMethod().Name);
                return "";
            }
        }
        /// <summary>
        /// this method used to make the meter fields visible true and false based on dtc metering
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbDTCMetered_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDTCMetered.SelectedIndex == 2)
                {
                    cmbMeterstatus.Enabled = false;
                    cmbCtratio.Enabled = false;
                    cmdmake.Enabled = false;
                    cmbWiring.Enabled = false;
                    cmbModem.Enabled = false;
                    txtslno.Enabled = false;
                    Manufactureyear.Enabled = false;

                    cmbMeterstatus.SelectedIndex = 0;
                    cmbCtratio.SelectedIndex = 0;
                    cmdmake.SelectedIndex = 0;
                    cmbWiring.SelectedIndex = 0;
                    cmbModem.SelectedIndex = 0;
                    Manufactureyear.SelectedIndex = 0;
                    txtslno.Text = string.Empty;
                }
                else if (cmbDTCMetered.SelectedIndex == 1)
                {
                    cmbMeterstatus.Enabled = true;
                    cmbCtratio.Enabled = true;
                    cmdmake.Enabled = true;
                    cmbWiring.Enabled = true;
                    cmbModem.Enabled = true;
                    txtslno.Enabled = true;
                    Manufactureyear.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                      System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }
        /// <summary>
        /// this method used to make the meter fields visible true and false based on meter status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Meterstatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbMeterstatus.SelectedIndex == 3)
                {

                    cmbCtratio.Enabled = false;
                    cmdmake.Enabled = false;
                    txtslno.Enabled = false;
                    Manufactureyear.Enabled = false;

                    cmbCtratio.SelectedIndex = 0;
                    cmdmake.SelectedIndex = 0;
                    txtslno.Text = string.Empty;
                    Manufactureyear.SelectedIndex = 0;
                }
                else
                {
                    cmbCtratio.Enabled = true;
                    cmdmake.Enabled = true;
                    txtslno.Enabled = true;
                    Manufactureyear.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                      System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        /// <summary>
        /// This method used to bind the ct ratio value as inbuilt ct if meter make selected as securethread through
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Metermake_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmdmake.SelectedIndex == Convert.ToInt32(ConfigurationManager.AppSettings["Securethreadthrough"]))
                {
                    cmbCtratio.SelectedIndex = Convert.ToInt32(ConfigurationManager.AppSettings["Inbuiltct"]);
                    cmbCtratio.Enabled = false;
                }
                else
                {
                    cmbCtratio.SelectedIndex = 0;
                    cmbCtratio.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                      System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        /// <summary>
        /// this method used for set the control texts
        /// </summary>
        public void SetContralText()
        {
            try
            {
                if (objSession.RoleId == ConfigurationManager.AppSettings["LTROLEID"])
                {
                    cmbFeeder.Enabled = false;
                    txtDTCCode.Enabled = false;
                    txtDTCName.Enabled = false;
                    txtOMSection.Enabled = false;
                    txtInternalCode.Enabled = false;
                    txtConnectedKW.Enabled = false;
                    txtConnectedHP.Enabled = false;
                    txtKWHReading.Enabled = false;
                    txtLatitude.Enabled = false;
                    txtlongitude.Enabled = false;
                    txtTCCode.Enabled = false;
                    txtTCMake.Enabled = false;
                    txtCapacity.Enabled = false;
                    txtCommisionDate.Enabled = false;
                    txtServiceDate.Enabled = false;
                    cmbprojecttype.Enabled = false;
                    txtFeederChngDate.Enabled = false;
                    cmdClose.Text = "Close";
                    cmdSave.Text = "APPROVE";

                    Ltdiv.Style.Add("display", "block");
                    if (Ltstatus.Text == "0")
                    {
                        cmdNext.Enabled = false;
                        cmdReset.Enabled = false;
                    }
                    if (Status.Text == "APPROVED")
                    {
                        cmdSave.Enabled = false;
                        cmdReset.Enabled = false;
                        cmdNext.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        /// <summary>
        /// this method used to load the dropdown values of meters
        /// </summary>
        public void Loadmeterdetails()
        {
            try
            {
                string Qry3 = "SELECT MD_ID, MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'MTR' ORDER BY MD_ID";
                Genaral.Load_Combo(Qry3, "--Select--", cmbDTCMetered);
                string Qry4 = "SELECT MD_ID, MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'MTRS' ORDER BY MD_ID";
                Genaral.Load_Combo(Qry4, "--Select--", cmbMeterstatus);
                string Qry5 = "SELECT MD_ID, MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'CTR' ORDER BY MD_ID";
                Genaral.Load_Combo(Qry5, "--Select--", cmbCtratio);
                string Qry6 = "SELECT MD_ID, MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'WIR' ORDER BY MD_ID";
                Genaral.Load_Combo(Qry6, "--Select--", cmbWiring);
                string Qry7 = "SELECT MD_ID, MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'MODEM' ORDER BY MD_ID";
                Genaral.Load_Combo(Qry7, "--Select--", cmbModem);
                string Qry8 = "SELECT MD_ID, MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'MAKE' ORDER BY MD_ID";
                Genaral.Load_Combo(Qry8, "--Select--", cmdmake);
                string Qry9 = "SELECT MD_ID, MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'MRECORDING' ORDER BY MD_ID";
                Genaral.Load_Combo(Qry9, "--Select--", cmbMeterRecording);

                Manufactureyear.Items.Clear();
                Manufactureyear.Items.Add("--Select--");


                var currentYear = DateTime.Today.Year;
                var startyear = currentYear - Convert.ToInt32(ConfigurationManager.AppSettings["MtrManufacturestartyear"]);

                for (int i = startyear; i >= 0; i--)
                {
                    Manufactureyear.Items.Add((currentYear - i).ToString());
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                 System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        /// <summary>
        /// This method used to reset the dtc metering details
        /// </summary>
        protected void Resetmeterdetails()
        {
            try
            {
                cmbDTCMetered.Enabled = true;
                cmbMeterstatus.Enabled = true;
                cmbModem.Enabled = true;
                cmbWiring.Enabled = true;
                cmbCtratio.Enabled = true;
                cmdmake.Enabled = true;
                txtslno.Enabled = true;
                Manufactureyear.Enabled = true;

                cmbDTCMetered.SelectedIndex = 0;
                cmbMeterstatus.SelectedIndex = 0;
                cmbModem.SelectedIndex = 0;
                cmbWiring.SelectedIndex = 0;
                cmbCtratio.SelectedIndex = 0;
                cmdmake.SelectedIndex = 0;
                Manufactureyear.SelectedIndex = 0;
                txtslno.Text = string.Empty;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                      System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }
    }
}