using IIITS.DTLMS.BL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.TCRepair
{
    public partial class Reclassification : System.Web.UI.Page
    {
        clsSession objSession;
        string UserName = Convert.ToString(ConfigurationManager.AppSettings["FTP_USER"]);
        string Password = Convert.ToString(ConfigurationManager.AppSettings["FTP_PASS"]);
        string SFTPPath = Convert.ToString(ConfigurationManager.AppSettings["SFTPPath"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToString(Session["clsSession"] ?? "").Length == 0)
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                objSession = (clsSession)Session["clsSession"];
                if (CheckAccessRights(Constants.CheckAccessRights.CheckAccessRightsAll))
                {
                    lblMessage.Text = string.Empty;
                    Form.DefaultButton = cmdSave.UniqueID;
                    CalendarExtender1.EndDate = DateTime.Now;
                    txtIssueDate.Attributes.Add("readonly", "readonly");
                    txtTestedOn.Attributes.Add("readonly", "readonly");

                    cmbTestLocation.Attributes.Add("readonly", "readonly");
                    txtTestedOn.Attributes.Add("readonly", "readonly");
                    cmbTestedBy.Attributes.Add("readonly", "readonly");

                    cmbTestLocation.Enabled = false;
                    txtTestedOn.Enabled = false;
                    cmbTestedBy.Enabled = false;

                    if (!IsPostBack)
                    {
                        string sOfficeCode = string.Empty;
                        if (objSession.OfficeCode.Length > 2)
                        {
                            sOfficeCode = objSession.OfficeCode.Substring(0, 2);
                        }
                        else
                        {
                            sOfficeCode = objSession.OfficeCode;
                        }

                        Genaral.Load_Combo("SELECT  TO_CHAR(SM_ID) StoreID,SM_NAME FROM TBLSTOREMAST WHERE SM_STATUS='A' ORDER BY SM_NAME", "--Select--", cmbStore);


                        if (Convert.ToString(Request.QueryString["RepairMasterId"] ?? "").Length > 0 && Convert.ToString(Request.QueryString["PoNo"] ?? "").Length > 0)
                        {
                            string sRepairMasterId = HttpUtility.HtmlDecode(Genaral.UrlDecrypt(Convert.ToString(Request.QueryString["RepairMasterId"])));
                            string PoNo = HttpUtility.HtmlDecode(Genaral.UrlDecrypt(Convert.ToString(Request.QueryString["PoNo"])));
                            LoadRepairMasterDetaisl(sRepairMasterId, PoNo);
                        }

                        //From DTR Tracker
                        if (Request.QueryString["TransId"] != null && Request.QueryString["TransId"].ToString() != "")
                        {
                            Genaral.Load_Combo("SELECT  TO_CHAR(SM_ID) StoreID,SM_NAME FROM TBLSTOREMAST WHERE SM_STATUS='A' ORDER BY SM_NAME", "--Select--", cmbStore);
                            Genaral.Load_Combo("SELECT US_ID,US_FULL_NAME FROM TBLUSER WHERE NVL(US_EFFECT_FROM,SYSDATE-1)<=SYSDATE AND US_STATUS='A' AND US_OFFICE_CODE like '%' ORDER BY US_ID", "--Select--", cmbTestedBy);
                            string sInspId = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["TransId"]));
                            string sDTrCode = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["DTrCode"]));
                            hdfTransId.Value = sInspId;
                            LoadTestingDoneDTR(sDTrCode, sInspId);
                            cmdSave.Enabled = false;
                        }

                        txtTestedOn.Attributes.Add("onblur", "return ValidateDate(" + txtTestedOn.ClientID + ");");
                        GetStoreId();
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
        private void LoadTestingDoneDTR(string sDTrCode, string sInspectId)
        {
            try
            {
                clsDTrRepairActivity objTestpending = new clsDTrRepairActivity();

                DataTable dt = new DataTable();

                objTestpending.sTcCode = sDTrCode;
                objTestpending.sTestInspectionId = sInspectId;

                objTestpending = objTestpending.LoadEETestedDTR(objTestpending);

                dt = objTestpending.dtTestDone;

                if (dt.Rows.Count > 0)
                {
                    txtPONo.Text = Convert.ToString(dt.Rows[0]["RSM_PO_NO"]);
                    txtIssueDate.Text = Convert.ToString(dt.Rows[0]["RSM_ISSUE_DATE"]);

                    cmbTestedBy.SelectedValue = objTestpending.sTestedBy;
                    txtTestedOn.Text = objTestpending.sTestedOn;
                    cmbTestLocation.Text = objTestpending.sTestLocation;

                    hdfRemarks.Value = objTestpending.sInspRemarks;
                    hdfResult.Value = objTestpending.sTestResult;

                    hdfRemarksEE.Value = Convert.ToString(dt.Rows[0]["IND_EE_REMARKS"]);
                    hdfOMNum.Value = Convert.ToString(dt.Rows[0]["IND_EE_OMNO"]);
                    hdfOMDate.Value = Convert.ToString(dt.Rows[0]["IND_EE_OM_DATE"]);
                    txtPO_Remarks.Text = Convert.ToString(dt.Rows[0]["RSM_REMARKS"]);
                }
                ViewState["Reclassification"] = dt;
                grdReclassificaDetails.DataSource = dt;
                grdReclassificaDetails.DataBind();

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
                lblMessage.Text = clsException.ErrorMsg();
            }

        }
        protected void LoadRepairMasterDetaisl(string sRepairMasterId, string PoNo)
        {
            DataTable DtRepMastDetaisl = new DataTable();
            try
            {
                clsDTrRepairActivity Obj = new clsDTrRepairActivity();
                Obj.RepairDetailsId = sRepairMasterId;
                Obj.sPurchaseOrderNo = PoNo;
                Obj.sOfficeCode = objSession.OfficeCode;
                DtRepMastDetaisl = Obj.GetRepairMasterDetaisl(Obj);
                if (DtRepMastDetaisl.Rows.Count > 0)
                {
                    txtPONo.Text = Convert.ToString(DtRepMastDetaisl.Rows[0]["RSM_PO_NO"]);
                    txtIssueDate.Text = Convert.ToString(DtRepMastDetaisl.Rows[0]["RSM_ISSUE_DATE"]);
                    txtOldPONo.Text = Convert.ToString(DtRepMastDetaisl.Rows[0]["RSM_OLD_PO_NO"]);
                    txtPO_Remarks.Text = Convert.ToString(DtRepMastDetaisl.Rows[0]["RSM_REMARKS"]);
                    txtrsmoiltype.Text = Convert.ToString(DtRepMastDetaisl.Rows[0]["RSM_OIL_TYPE"]);

                    cmbTestLocation.SelectedValue = Convert.ToString(DtRepMastDetaisl.Rows[0]["IND_TEST_LOCATION"]);
                    txtTestedOn.Text = Convert.ToString(DtRepMastDetaisl.Rows[0]["IND_INSP_DATE"]);

                    Genaral.Load_Combo("SELECT US_ID,US_FULL_NAME FROM TBLUSER WHERE NVL(US_EFFECT_FROM,SYSDATE-1)<=SYSDATE " +
                            " AND US_STATUS='A' AND US_ID='" + Convert.ToString(DtRepMastDetaisl.Rows[0]["IND_INSP_BY"]) + "' ORDER BY US_ID", "", cmbTestedBy);

                    //cmbTestedBy.SelectedValue = Convert.ToString(DtRepMastDetaisl.Rows[0]["IND_INSP_BY"]);

                    hdfrsmoiltype.Value = txtrsmoiltype.Text;
                    DateTime Issuedate = Convert.ToDateTime(txtIssueDate.Text);
                    int Isuedate = Convert.ToInt32((DateTime.Now.AddDays(-1) - Issuedate).TotalDays);
                    string temp = DateTime.Today.AddDays(-Isuedate).ToString("yyyy-MM-dd");
                    hiddenTestedOn.Text = temp;
                }
                ViewState["Reclassification"] = DtRepMastDetaisl;
                grdReclassificaDetails.DataSource = DtRepMastDetaisl;
                grdReclassificaDetails.DataBind();
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                       MethodBase.GetCurrentMethod().DeclaringType.Name,
                       MethodBase.GetCurrentMethod().Name);
            }
        }


        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                string[] Arr = new string[2];

                //Check AccessRights
                bool bAccResult = CheckAccessRights(Constants.CheckAccessRights.CheckAccessRightsCreate);
                if (bAccResult == false)
                {
                    return;
                }

                if (grdReclassificaDetails.Rows.Count > 0)
                {
                    if (CheckDuplicateImage() == false)
                    {
                        ShowMsgBox("Dublicate Image Not Allowed");
                        return;
                    }
                    clsDTrRepairActivity objTestPending = new clsDTrRepairActivity();
                    objTestPending.sTestedBy = cmbTestedBy.SelectedValue;
                    objTestPending.sStoreId = cmbStore.SelectedValue;
                    objTestPending.sTestedOn = txtTestedOn.Text;
                    objTestPending.sPurchaseOrderNo = txtPONo.Text;
                    objTestPending.sIssueDate = txtIssueDate.Text;
                    objTestPending.sCrby = objSession.UserId;
                    objTestPending.sTestLocation = cmbTestLocation.SelectedValue;
                    objTestPending.sOfficeCode = objSession.OfficeCode;
                    objTestPending.RSMOilType = txtrsmoiltype.Text;
                    if (objTestPending.RSMOilType == "0" && txtRepairerSupplyingOilQTY.Text != "")
                    {
                        objTestPending.Pototaloilqty = txtPOTotalOilQTY.Text;
                        objTestPending.POPendingOilQTY = txtPOPendingOilQTY.Text;
                        objTestPending.StoreAlradyIssued = txtStoreAlradyIssued.Text;

                        if (txtRepairerAlreadyIssued.Text != "")
                        {
                            objTestPending.RepairerAlreadyIssued = txtRepairerAlreadyIssued.Text;
                        }
                        else
                        {
                            objTestPending.RepairerAlreadyIssued = "0";
                        }

                        if (Convert.ToDouble(txtRepairerSupplyingOilQTY.Text) > Convert.ToDouble(objTestPending.POPendingOilQTY))
                        {
                            ShowMsgBox("Oil supplying by repairer quantity should be less than or equal to pending oil quantity");
                            return;
                        }
                        objTestPending.OilSupplyingBy = cmdOilSupplyingBy.SelectedValue;
                        objTestPending.RepairerSupplyingOilQTY = txtRepairerSupplyingOilQTY.Text;

                        double QuantityAmt = Convert.ToDouble(objTestPending.RepairerSupplyingOilQTY) +
                            Convert.ToDouble(objTestPending.StoreAlradyIssued) + Convert.ToDouble(objTestPending.RepairerAlreadyIssued);
                        double TotalRPOilQty = (Convert.ToDouble(objTestPending.Pototaloilqty)) - QuantityAmt;
                        objTestPending.TotalRemaingPendingOilQty = Convert.ToString(TotalRPOilQty);

                        double TotalRPoilQtyInKltr = Convert.ToDouble(objTestPending.RepairerSupplyingOilQTY) / 1000;
                        objTestPending.TotalRPoilQtyInKltr = Convert.ToString(TotalRPoilQtyInKltr);
                    }

                    int i = 0;
                    objTestPending.sTestResult = "0";
                    bool bChecked = false;

                    string[] strQrylist = new string[grdReclassificaDetails.Rows.Count];
                    DataTable dtimage = new DataTable();
                    DataColumn dc = new DataColumn("image");
                    dc.DataType = System.Type.GetType("System.Byte[]");
                    dtimage.Columns.Add(dc);
                    dtimage.Columns.Add("RSDID", typeof(string));

                    foreach (GridViewRow row in grdReclassificaDetails.Rows)
                    {
                        //Repair Good
                        bool result = ((RadioButton)row.FindControl("rdbRepairGood")).Checked;
                        Byte[] Buffer;
                        bChecked = false;
                        if (result)
                        {
                            objTestPending.sTestResult = "1";
                            bChecked = true;
                        }
                        //Faulty
                        result = ((RadioButton)row.FindControl("rdbFaulty")).Checked;
                        if (result)
                        {
                            objTestPending.sTestResult = "2";
                            bChecked = true;
                        }
                        //Not Repairable
                        result = ((RadioButton)row.FindControl("rdbNotRepairable")).Checked;
                        if (result)
                        {
                            objTestPending.sTestResult = "3";
                            bChecked = true;
                        }
                        if (bChecked == false)
                        {
                            ShowMsgBox("Please select Item type of all DTr Codes.");
                            return;
                        }

                        string OMNumber = ((TextBox)row.FindControl("txtOMNum")).Text.Trim();
                        if ((OMNumber ?? "").Length == 0)
                        {
                            ((TextBox)row.FindControl("txtOMNum")).Focus();
                            ShowMsgBox("Please Enter OM Number");
                            return;
                        }
                        // to check OMNumber contains at least one number.
                        Regex regex = new Regex(@"\d+");
                        Match match = regex.Match(OMNumber);
                        if (!match.Success)
                        {
                            ((TextBox)row.FindControl("txtOMNum")).Text = "";
                            ((TextBox)row.FindControl("txtOMNum")).Focus();
                            ShowMsgBox("Please Enter valid OM Number");
                            return;
                        }

                        string OMDate = ((TextBox)row.FindControl("txtOMDate")).Text;
                        if ((OMDate ?? "").Length == 0)
                        {
                            ((TextBox)row.FindControl("txtOMDate")).Focus();
                            ShowMsgBox("Please Select OM Date");
                            return;
                        }
                        string sResult = Genaral.DateComparision(OMDate, "", true, false);
                        if (sResult == "1")
                        {
                            ((TextBox)row.FindControl("txtOMDate")).Text = "";
                            ((TextBox)row.FindControl("txtOMDate")).Focus();
                            ShowMsgBox("OM Date should be Less than or Equal to Current Date");
                            return;
                        }
                        string EERemarks = ((TextBox)row.FindControl("txtRemarksEE")).Text.Trim();
                        //if ((EERemarks ?? "").Length == 0)
                        //{
                        //    ((TextBox)row.FindControl("txtRemarksEE")).Focus();
                        //    ShowMsgBox("Please Enter Remarks to Transformers");
                        //    return;
                        //}

                        string RSMID = ((Label)row.FindControl("lbltransid")).Text.Trim();

                        FileUpload fupDoc = (FileUpload)row.FindControl("fupdDoc");
                        string EE_Upload_filename = Path.GetFileName(fupDoc.PostedFile.FileName);

                        #region SaveImage
                        if (fupDoc.PostedFile.ContentLength != 0)
                        {
                            string strExt = EE_Upload_filename.Substring(EE_Upload_filename.LastIndexOf('.') + 1);
                            if (strExt.ToLower().Equals("jpg") || strExt.ToLower().Equals("jpeg") || strExt.ToLower().Equals("png") || strExt.ToLower().Equals("gif") || strExt.ToLower().Equals("pdf"))
                            {
                                Stream strm = fupDoc.PostedFile.InputStream;
                                Buffer = new byte[strm.Length];
                                strm.Read(Buffer, 0, (int)strm.Length);

                                dtimage.Rows.Add(Buffer, ((Label)row.FindControl("lbltransid")).Text.Trim());

                            }
                            else
                            {
                                ShowMsgBox("Invalid File");
                                return;
                            }
                        }
                        #endregion

                        strQrylist[i] = RSMID + "~" + objTestPending.sTestResult + "~" + EERemarks.Replace("'", "`") + "~" + OMNumber + "~" + OMDate;
                        i++;


                    }
                    Session["fileupload"] = dtimage;
                    Arr = objTestPending.SaveEETestingDetails(strQrylist, objTestPending, dtimage);
                    if (Arr[1].ToString() == "0")
                    {
                        SaveFilePathEETest();
                        grdReclassificaDetails.DataSource = null;
                        grdReclassificaDetails.DataBind();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), UniqueID, "alert('" + Arr[0].ToString() + "'); location.href='ReclassificationView.aspx';", true);
                        Reset();
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "RepairTrasnaction-Testing");
                        }
                        return;
                    }
                    else
                    {
                        ShowMsgBox("No Transformer Exists to Inspect");
                    }
                }
                else
                {
                    ShowMsgBox("At least one DTR is requeired for reclassification to proceed further.");
                }


            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
                lblMessage.Text = clsException.ErrorMsg();
            }
        }
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "Reclassification";
                objApproval.sRoleId = objSession.RoleId;
                objApproval.sAccessType = Constants.CheckAccessRights.CheckAccessRightsAll + "," + sAccessType;
                bool bResult = objApproval.CheckAccessRights(objApproval);
                if (bResult == false)
                {
                    Response.Redirect("~/UserRestrict.aspx", false);
                }
                return bResult;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
                return false;

            }
        }
        #region not used code.
        //public bool SaveFilePath(clsStoreEnumeration objStoreEnum)
        //{
        //    try
        //    {
        //        //FTP Parameter
        //        string sFTPLink = string.Empty;
        //        string sFTPUserName = string.Empty;
        //        string sFTPPassword = string.Empty;

        //        string sTestFileExt = string.Empty;
        //        string sTestFileName = string.Empty;
        //        string sDirectory = string.Empty;

        //        //  Photo Save DTLMSDocs
        //        string mainfolder = Convert.ToString(ConfigurationManager.AppSettings["SFTPmainfolder1"]);

        //        clsCommon objComm = new clsCommon();
        //        DataTable dt = objComm.GetAppSettings();
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            if (Convert.ToString(dt.Rows[i]["AP_KEY"]) == "FTPLINK")
        //            {
        //                sFTPLink = Convert.ToString(dt.Rows[i]["AP_VALUE"]);
        //            }
        //            else if (Convert.ToString(dt.Rows[i]["AP_KEY"]) == "FTPUSERNAME")
        //            {
        //                sFTPUserName = Convert.ToString(dt.Rows[i]["AP_VALUE"]);
        //            }
        //            else if (Convert.ToString(dt.Rows[i]["AP_KEY"]) == "FTPPASSWORD")
        //            {
        //                sFTPPassword = Convert.ToString(dt.Rows[i]["AP_VALUE"]);
        //            }
        //        }
        //        sFTPLink = Convert.ToString(ConfigurationManager.AppSettings["SFTPPath"].ToUpper());
        //        clsSFTP objFtp = new clsSFTP(sFTPLink, sFTPUserName, sFTPPassword);
        //        bool Isuploaded;

        //        // Create Directory

        //        bool IsExists = objFtp.FtpDirectoryExists(mainfolder + objStoreEnum.sEnumDetailsId + "/");
        //        if (IsExists == false)
        //        {

        //            objFtp.createDirectory(mainfolder + objStoreEnum.sEnumDetailsId);
        //        }

        //        //  Photo Save DTLMSDocs
        //        string sFileExt = Convert.ToString(ConfigurationManager.AppSettings["NotAllowFileFormat"]);

        //        if (fupTestDocument.PostedFile.ContentLength != 0)
        //        {

        //            sTestFileExt = System.IO.Path.GetExtension(fupTestDocument.FileName).ToString().ToLower();
        //            sTestFileExt = ";" + sTestFileExt.Remove(0, 1) + ";";

        //            if (sFileExt.Contains(sTestFileExt))
        //            {
        //                ShowMsgBox("Invalid File Format");
        //                return false;
        //            }

        //            sTestFileName = System.IO.Path.GetFileName(fupTestDocument.PostedFile.FileName);

        //            fupTestDocument.SaveAs(Server.MapPath("~/DTLMSFiles" + "/" + sTestFileName));
        //            sDirectory = Server.MapPath("~/DTLMSFiles" + "/" + sTestFileName);

        //        }


        //        if (sTestFileName != "")
        //        {

        //            if (File.Exists(sDirectory))
        //            {
        //                IsExists = objFtp.FtpDirectoryExists(mainfolder + objStoreEnum.sEnumDetailsId + "/" + "TESTING" + "/");
        //                if (IsExists == false)
        //                {

        //                    objFtp.createDirectory(mainfolder + objStoreEnum.sEnumDetailsId + "/" + "TESTING");
        //                }

        //                Isuploaded = objFtp.upload(mainfolder + objStoreEnum.sEnumDetailsId + "/" + "TESTING" + "/", objSession.UserId + "~" + sTestFileName, sDirectory);
        //                if (Isuploaded == true & File.Exists(sDirectory))
        //                {
        //                    File.Delete(sDirectory);
        //                    objStoreEnum.sNamePlatePhotoPath = objStoreEnum.sEnumDetailsId + "/" + "TESTING" + "/" + objSession.UserId + "~" + sTestFileName;

        //                }
        //            }
        //        }

        //        bool bResult;

        //        return true;

        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
        //            System.Reflection.MethodBase.GetCurrentMethod().Name);
        //        lblMessage.Text = clsException.ErrorMsg();
        //        return false;
        //    }
        //}

        //private void LoadTestingPendingTC()
        //{
        //    try
        //    {
        //        clsDTrRepairActivity objTestpending = new clsDTrRepairActivity();

        //        DataTable dt = new DataTable();
        //        txtSelectedDetailsId.Text = txtSelectedDetailsId.Text.Replace("~", ",");
        //        if (!txtSelectedDetailsId.Text.StartsWith(","))
        //        {
        //            txtSelectedDetailsId.Text = "," + txtSelectedDetailsId.Text;
        //        }
        //        if (!txtSelectedDetailsId.Text.EndsWith(","))
        //        {
        //            txtSelectedDetailsId.Text = txtSelectedDetailsId.Text + ",";
        //        }

        //        txtSelectedDetailsId.Text = txtSelectedDetailsId.Text.Substring(1, txtSelectedDetailsId.Text.Length - 1);
        //        txtSelectedDetailsId.Text = txtSelectedDetailsId.Text.Substring(0, txtSelectedDetailsId.Text.Length - 1);

        //        objTestpending.sRepairDetailsId = txtSelectedDetailsId.Text;
        //        objTestpending.sOfficeCode = objSession.OfficeCode;
        //        objTestpending.sPurchaseOrderNo = txtPONo.Text;
        //        objTestpending.sTestingDone = "0";

        //        dt = objTestpending.LoadTestOrDeliverPendingDTR(objTestpending);
        //        if (dt.Rows.Count > 0)
        //        {
        //            txtPONo.Text = Convert.ToString(dt.Rows[0]["RSM_PO_NO"]);
        //            txtIssueDate.Text = Convert.ToString(dt.Rows[0]["RSM_ISSUE_DATE"]);
        //            txtOldPONo.Text = Convert.ToString(dt.Rows[0]["RSM_OLD_PO_NO"]);
        //            txtPO_Remarks.Text = Convert.ToString(dt.Rows[0]["RSM_REMARKS"]);
        //            txtrsmoiltype.Text = Convert.ToString(dt.Rows[0]["RSM_OIL_TYPE"]);
        //            hdfrsmoiltype.Value = txtrsmoiltype.Text;
        //            DateTime Issuedate = Convert.ToDateTime(txtIssueDate.Text);
        //            int Isuedate = Convert.ToInt32((DateTime.Now.AddDays(-1) - Issuedate).TotalDays);
        //            string temp = DateTime.Today.AddDays(-Isuedate).ToString("yyyy-MM-dd");
        //            hiddenTestedOn.Text = temp;
        //        }
        //        grdReclassificaDetails.DataSource = dt;
        //        grdReclassificaDetails.DataBind();
        //        ViewState["TestDTR"] = dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
        //            System.Reflection.MethodBase.GetCurrentMethod().Name);
        //        lblMessage.Text = clsException.ErrorMsg();
        //    }

        //}       
        //private void LoadOilDetails()
        //{
        //    try
        //    {
        //        clsDTrRepairActivity objLoadOilDetails = new clsDTrRepairActivity();
        //        DataTable dtLoadOilDetails = new DataTable();

        //        objLoadOilDetails.sPurchaseOrderNo = txtPONo.Text;
        //        objLoadOilDetails.sOfficeCode = objSession.OfficeCode;
        //        objLoadOilDetails.sRoleId = objSession.RoleId;

        //        dtLoadOilDetails = objLoadOilDetails.LoadOilDetailsOnPONO(objLoadOilDetails);

        //        if (dtLoadOilDetails.Rows.Count > 0)
        //        {
        //            txtPOTotalOilQTY.Text = Convert.ToString(dtLoadOilDetails.Rows[0]["TOTAL_OIL_AMOUNT"]);
        //            txtPOPendingOilQTY.Text = Convert.ToString(dtLoadOilDetails.Rows[0]["ROI_PENDINGQTY"]);
        //            txtStoreAlradyIssued.Text = Convert.ToString(dtLoadOilDetails.Rows[0]["STORE_SENT_OIL"]);
        //            txtRepairerAlreadyIssued.Text = Convert.ToString(dtLoadOilDetails.Rows[0]["REPAIRER_SENT_OIL"]);
        //            if (Convert.ToString(dtLoadOilDetails.Rows[0]["REPAIRER_SENT_OIL"]) == "")
        //            {
        //                txtRepairerAlreadyIssued.Text = "0";
        //            }
        //            if (Convert.ToString(dtLoadOilDetails.Rows[0]["STORE_SENT_OIL"]) == "")
        //            {
        //                txtStoreAlradyIssued.Text = "0";
        //            }
        //            double Sentoil = Convert.ToDouble(txtStoreAlradyIssued.Text) + Convert.ToDouble(txtRepairerAlreadyIssued.Text);
        //            double TotalOilQty = Convert.ToDouble(txtPOTotalOilQTY.Text);
        //            double CalculatedPendingQty = TotalOilQty - Sentoil;
        //            txtPOPendingOilQTY.Text = CalculatedPendingQty.ToString();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
        //            System.Reflection.MethodBase.GetCurrentMethod().Name);
        //        lblMessage.Text = clsException.ErrorMsg();
        //    }
        //}
        //private void LoadTestingDoneDTR(string sDTrCode, string sInspectId)
        //{
        //    try
        //    {
        //        clsDTrRepairActivity objTestpending = new clsDTrRepairActivity();
        //        DataTable dt = new DataTable();

        //        objTestpending.sTcCode = sDTrCode;
        //        objTestpending.sTestInspectionId = sInspectId;

        //        objTestpending = objTestpending.LoadTestedDTR(objTestpending);

        //        dt = objTestpending.dtTestDone;

        //        if (dt.Rows.Count > 0)
        //        {
        //            txtPONo.Text = Convert.ToString(dt.Rows[0]["RSM_PO_NO"]);
        //            txtIssueDate.Text = Convert.ToString(dt.Rows[0]["RSM_ISSUE_DATE"]);

        //            cmbTestedBy.SelectedValue = objTestpending.sTestedBy;
        //            txtTestedOn.Text = objTestpending.sTestedOn;
        //            cmbTestLocation.Text = objTestpending.sTestLocation;

        //            hdfRemarks.Value = objTestpending.sInspRemarks;
        //            hdfResult.Value = objTestpending.sTestResult;

        //        }
        //        grdReclassificaDetails.DataSource = dt;
        //        grdReclassificaDetails.DataBind();
        //        ViewState["TestDTR"] = dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
        //            System.Reflection.MethodBase.GetCurrentMethod().Name);
        //        lblMessage.Text = clsException.ErrorMsg();
        //    }
        //}
        //public bool ValidateForm()
        //{
        //    bool bValidate = false;
        //    try
        //    {
        //        bValidate = true;
        //        return bValidate;
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
        //            System.Reflection.MethodBase.GetCurrentMethod().Name);
        //        return bValidate;
        //    }
        //}
        #endregion
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
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
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
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        void Reset()
        {
            try
            {
                cmbTestedBy.SelectedIndex = 0;
                txtTestedOn.Text = string.Empty;
                cmbTestLocation.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        public void GetStoreId()
        {
            try
            {
                clsTcMaster objTcMaster = new clsTcMaster();
                cmbStore.SelectedValue = objTcMaster.GetStoreId(objSession.OfficeCode);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        protected void grdReclassificaDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "Remove")
                {
                    DataTable dt = (DataTable)ViewState["Reclassification"];
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    try
                    {

                        if (dt.Rows.Count > 1)
                        {
                            int iRowIndex = row.RowIndex;
                            dt.AcceptChanges();
                            dt.Rows[iRowIndex].Delete();
                            dt.AcceptChanges();
                            if (dt.Rows.Count == 0)
                            {
                                ViewState["Reclassification"] = null;
                            }
                            else
                            {
                                ViewState["Reclassification"] = dt;
                            }
                        }
                        else
                        {
                            ShowMsgBox("At least one DTR is requeired for reclassification to proceed further.");
                        }
                        grdReclassificaDetails.DataSource = dt;
                        grdReclassificaDetails.DataBind();
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        protected void grdReclassificaDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DataTable dt = (DataTable)ViewState["Reclassification"];

                    string sFilter = string.Empty;
                    string sTcCode = ((Label)e.Row.FindControl("lblTcCode")).Text;

                    TextBox txtRemarksEE = (TextBox)e.Row.FindControl("txtRemarksEE");
                    TextBox txtOMNum = (TextBox)e.Row.FindControl("txtOMNum");
                    TextBox txtOMDate = (TextBox)e.Row.FindControl("txtOMDate");

                    if (cmbTestedBy.SelectedIndex > 0 && (hdfTransId.Value ?? "").Length > 0)
                    {
                        RadioButton rdbRepairGood = (RadioButton)e.Row.FindControl("rdbRepairGood");
                        RadioButton rdbFaulty = (RadioButton)e.Row.FindControl("rdbFaulty");
                        RadioButton rdbNotRepairable = (RadioButton)e.Row.FindControl("rdbNotRepairable");

                        if (hdfResult.Value == "1")
                        {
                            rdbRepairGood.Checked = true;                            
                        }
                        else if (hdfResult.Value == "2")
                        {                           
                            rdbFaulty.Checked = true;
                            
                        }
                        else
                        {                           
                            rdbNotRepairable.Checked = true;
                        }

                        txtOMNum.Text = hdfOMNum.Value;
                        txtOMNum.ReadOnly = true;
                        txtOMDate.Text = hdfOMDate.Value;
                        txtOMDate.ReadOnly = true;

                        if (dt.Rows.Count > 0)
                        {
                            if (Convert.ToString(dt.Rows[0]["IND_EE_REMARKS"] ?? "").Length > 0)
                            {
                                txtRemarksEE.Text = Convert.ToString(dt.Rows[0]["IND_EE_REMARKS"] ?? "");
                            }else
                            {
                                txtRemarksEE.Text = string.Empty;
                            }
                            txtRemarksEE.ReadOnly = true;
                        }                       
                    }


                    if (dt.Rows.Count > 0)
                    {
                        DataRow[] dtrow = dt.Select(string.Format("CONVERT({0}, System.String) like '%{1}%'",
                             "TC_CODE", sTcCode.Trim()));

                        LinkButton lnkDownload = ((LinkButton)e.Row.FindControl("lnkDownload"));
                        LinkButton lnkNoDnlHT_Doc = ((LinkButton)e.Row.FindControl("lnkNoDnlHT_Doc"));
                        if (dtrow[0]["HT_uplodede_doc"].ToString() == null || dtrow[0]["HT_uplodede_doc"].ToString() == "")
                        {
                            lnkDownload.Visible = false;
                            lnkNoDnlHT_Doc.Visible = true;
                            lnkNoDnlHT_Doc.CssClass = "blockpointer";
                        }
                        else
                        {
                            lnkDownload.Enabled = true;
                            lnkNoDnlHT_Doc.Visible = false;
                            lnkDownload.CssClass = "handPointer";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        public bool CheckDuplicateImage()
        {
            try
            {
                ArrayList TcDetails = new ArrayList();
                bool result = true;
                string filename = string.Empty;
                foreach (GridViewRow grdRow in grdReclassificaDetails.Rows)
                {
                    bool IsFile = ((FileUpload)grdRow.FindControl("fupdDoc")).HasFile;

                    if (IsFile)
                    {
                        filename = ((FileUpload)(grdRow.FindControl("fupdDoc"))).FileName;

                        if (TcDetails.Contains(filename))
                            result = false;
                        else
                            result = true;
                        TcDetails.Add(filename);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }
        public bool CheckImageAlreaySaved()
        {
            try
            {
                clsDTrRepairActivity objTcActivity = new clsDTrRepairActivity();
                DataTable dbImage = objTcActivity.GetAllImages();
                DataTable UpldingImage = (DataTable)Session["fileupload"];
                Session["fileupload"] = null;
                for (int i = 0; i < dbImage.Rows.Count; i++)
                {
                    for (int j = 0; j < UpldingImage.Rows.Count; j++)
                    {
                        if ((dbImage.Rows[i][0] != DBNull.Value) && (UpldingImage.Rows[j][0] != DBNull.Value))
                        {
                            byte[] dbimg = (byte[])dbImage.Rows[i][0];
                            byte[] upimg = (byte[])UpldingImage.Rows[j][0];

                            if (dbimg.SequenceEqual(upimg))
                                return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        public void cmdOilSupplyingby_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmdOilSupplyingBy.Text == "2")
                {
                    txtRepairerSupplyingOilQTY.Enabled = true;
                }
                else
                {
                    txtRepairerSupplyingOilQTY.Enabled = false;
                    txtRepairerSupplyingOilQTY.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        // to download the file
        protected void DownloadFile(object sender, EventArgs e)
        {

            string fileName = (sender as LinkButton).CommandArgument;

            try
            {
                if ((fileName ?? "").Length > 0)
                {
                    string SFTPmainfolderpath = Convert.ToString(ConfigurationManager.AppSettings["VirtualDirectoryPath"]);
                    string PoNo = Regex.Replace(txtPONo.Text, @"[^0-9a-zA-Z]+", "");
                    clsSFTP objFtp = new clsSFTP(SFTPPath, UserName, Password);

                    string path = SFTPmainfolderpath + fileName;
                    ClientScript.RegisterStartupScript(this.GetType(), "Print", "<script>window.open('" + path + "','_blank')</script>");
                }
                else
                {
                    ShowMsgBox("HT as Not Uploaded the Documents.");
                }
            }
            catch (WebException ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                 MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }
        protected void DownloadFiledwnld(object sender, EventArgs e)
        {
            string fileName1 = (sender as LinkButton).CommandArgument;
            try
            {
                //Create a stream for the file
                Stream stream = null;
                string fileName = string.Empty;
                //This controls how many bytes to read at a time and send to the client
                int bytesToRead = 10000;

                // Buffer to read bytes in chunk size specified above
                byte[] buffer = new Byte[bytesToRead];

                // The number of bytes read
                try
                {
                    string SFTPmainfolder = Convert.ToString(ConfigurationManager.AppSettings["VirtualDirectoryPMCDocs"]);

                    string PoNo = Regex.Replace(txtPONo.Text, @"[^0-9a-zA-Z]+", "");
                    clsSFTP objFtp = new clsSFTP(SFTPPath, UserName, Password);

                    string url = SFTPmainfolder + "PMC_PO_DOCS/" + PoNo + "/" + fileName1;
                    //fileName = getFilename(url); need to remove it  ==>

                    //Create a WebRequest to get the file
                    HttpWebRequest fileReq = (HttpWebRequest)HttpWebRequest.Create(url);

                    //Create a response for this request
                    HttpWebResponse fileResp = (HttpWebResponse)fileReq.GetResponse();

                    if (fileReq.ContentLength > 0)
                        fileResp.ContentLength = fileReq.ContentLength;

                    //Get the Stream returned from the response
                    stream = fileResp.GetResponseStream();

                    // prepare the response to the client. resp is the client Response
                    var resp = HttpContext.Current.Response;

                    //Indicate the type of data being sent
                    resp.ContentType = "application/octet-stream";

                    //Name the file 
                    resp.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
                    resp.AddHeader("Content-Length", Convert.ToString(fileResp.ContentLength));

                    int length;
                    do
                    {
                        // Verify that the client is connected.
                        if (resp.IsClientConnected)
                        {
                            // Read data into the buffer.
                            length = stream.Read(buffer, 0, bytesToRead);

                            // and write it out to the response's output stream
                            resp.OutputStream.Write(buffer, 0, length);

                            // Flush the data
                            resp.Flush();

                            //Clear the buffer
                            buffer = new Byte[bytesToRead];
                        }
                        else
                        {
                            // cancel the download if client has disconnected
                            length = -1;
                        }
                    } while (length > 0); //Repeat until no data is read
                }
                finally
                {
                    if (stream != null)
                    {
                        //Close the input stream
                        stream.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("(404) Not Found"))
                {
                    ShowMsgBox("File Not Found");
                }
                else
                {
                    lblMessage.Text = clsException.ErrorMsg();
                    clsException.LogError(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                     System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                }
            }
        }
        protected void SaveFilePathEETest()
        {
            try
            {
                foreach (GridViewRow row in grdReclassificaDetails.Rows)
                {
                    FileUpload fupDoc = (FileUpload)row.FindControl("fupdDoc");
                    string Rsd_id = (((Label)row.FindControl("lbltransid")).Text.Trim());
                    if (fupDoc.PostedFile.ContentLength != 0)
                    {
                        string filename = Path.GetFileName(fupDoc.PostedFile.FileName);
                        string sFTPLink = string.Empty;
                        string sFTPUserName = string.Empty;
                        string sFTPPassword = string.Empty;
                        string sTestFileExt = string.Empty;
                        string sTestFileName = string.Empty;
                        string sDirectory = string.Empty;

                        //  Photo Save DTLMSDocs
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
                        sFTPLink = Convert.ToString(ConfigurationManager.AppSettings["SFTPPath"].ToUpper());
                        clsSFTP objFtp = new clsSFTP(sFTPLink, sFTPUserName, sFTPPassword);
                        bool Isuploaded;

                        // Create Directory
                        //bool IsExists = objFtp.FtpDirectoryExists(mainfolder + "HT_TESTING"+ "/" + Rsd_id + "/");
                        bool IsExists = objFtp.FtpDirectoryExists(mainfolder + "EE_TESTING");

                        if (IsExists == false)
                        {

                            objFtp.createDirectory(mainfolder + "EE_TESTING");
                        }
                        if (fupDoc.PostedFile.ContentLength != 0)
                        {

                            sTestFileExt = System.IO.Path.GetExtension(fupDoc.FileName).ToString().ToLower();
                            sTestFileExt = ";" + sTestFileExt.Remove(0, 1) + ";";

                            // sTestFileName = System.IO.Path.GetFileName(fupTestDocument.PostedFile.FileName);
                            sTestFileName = filename;

                            fupDoc.SaveAs(Server.MapPath("~/DTLMSFiles" + "/" + sTestFileName));
                            sDirectory = Server.MapPath("~/DTLMSFiles" + "/" + sTestFileName);
                        }

                        if (sTestFileName != "")
                        {
                            if (File.Exists(sDirectory))
                            {
                                IsExists = objFtp.FtpDirectoryExists(mainfolder + "EE_TESTING" + "/" + Rsd_id);
                                if (IsExists == false)
                                {
                                    objFtp.createDirectory(mainfolder + "EE_TESTING" + "/" + Rsd_id);
                                }
                                Isuploaded = objFtp.upload(mainfolder + "EE_TESTING" + "/" + Rsd_id + "/", sTestFileName, sDirectory);
                                if (Isuploaded == true & File.Exists(sDirectory))
                                {
                                    File.Delete(sDirectory);
                                    clsDTrRepairActivity objinddoc = new clsDTrRepairActivity();
                                    objinddoc.UploadedpathHt = "EE_TESTING" + "/" + Rsd_id + "/" + sTestFileName;
                                    objinddoc.RepairDetailsId = Rsd_id;
                                    objinddoc.UpdateEE_Uploadedpath(objinddoc);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
    }
}