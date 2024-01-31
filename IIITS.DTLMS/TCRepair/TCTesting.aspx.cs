using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Collections;

namespace IIITS.DTLMS.TCRepair
{
    public partial class TCTesting : System.Web.UI.Page
    {

        string strFormCode = "TCTesting";
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
                Form.DefaultButton = cmdSave.UniqueID;
                CalendarExtender1.EndDate = DateTime.Now;
                txtIssueDate.Attributes.Add("readonly", "readonly");
                txtTestedOn.Attributes.Add("readonly", "readonly");

                if (!IsPostBack)
                {

                    // if (Request.QueryString["PoNo"] != null && Request.QueryString["PoNo"].ToString() != "")

                    if ((Convert.ToString(Request.QueryString["PoNo"] ?? "") ?? "").Length > 0)
                    {
                        if (Session["RepairDetailsId"] != null && Session["RepairDetailsId"].ToString() != "")
                        {
                            txtSelectedDetailsId.Text = Session["RepairDetailsId"].ToString();
                        }
                        txtPONo.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["PoNo"]));

                        LoadTestingPendingTC();
                        if (txtrsmoiltype.Text == "0")
                        {
                            divwithoutoil.Style.Add("display", "block");
                            divwithoutoilright.Style.Add("display", "block");
                            LoadOilDetails();
                            cmdOilSupplyingBy.Enabled = true;
                        }
                    }

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
                        Genaral.Load_Combo("SELECT US_ID,US_FULL_NAME FROM TBLUSER WHERE NVL(US_EFFECT_FROM,SYSDATE-1)<=SYSDATE AND US_STATUS='A' AND US_OFFICE_CODE='" + sOfficeCode + "' ORDER BY US_ID", "--Select--", cmbTestedBy);
                  
                    //From DTR Tracker
                    if (Request.QueryString["TransId"] != null && Request.QueryString["TransId"].ToString() != "")
                    {
                        Genaral.Load_Combo("SELECT  TO_CHAR(SM_ID) StoreID,SM_NAME FROM TBLSTOREMAST WHERE SM_STATUS='A' ORDER BY SM_NAME", "--Select--", cmbStore);
                        Genaral.Load_Combo("SELECT US_ID,US_FULL_NAME FROM TBLUSER WHERE NVL(US_EFFECT_FROM,SYSDATE-1)<=SYSDATE AND US_STATUS='A' AND US_OFFICE_CODE like '%' ORDER BY US_ID", "--Select--", cmbTestedBy);
                        string sInspId = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["TransId"]));
                        string sDTrCode = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["DTrCode"]));
                        LoadTestingDoneDTR(sDTrCode, sInspId);
                        cmdSave.Enabled = false;
                    }

                    txtTestedOn.Attributes.Add("onblur", "return ValidateDate(" + txtTestedOn.ClientID + ");");
                    GetStoreId();
                }

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
                lblMessage.Text = ex.Message;
            }
        }


        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                string[] Arr = new string[2];

                //Check AccessRights
                bool bAccResult = CheckAccessRights("2");
                if (bAccResult == false)
                {
                    return;
                }


                if (ValidateForm() == true)
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

                    //added newly by santhosh
                    objTestPending.sOfficeCode = objSession.OfficeCode;
                    objTestPending.RSMOilType = txtrsmoiltype.Text;
                    if (objTestPending.RSMOilType == "0" && txtRepairerSupplyingOilQTY.Text != "")
                    {
                        objTestPending.Pototaloilqty = txtPOTotalOilQTY.Text;
                        objTestPending.POPendingOilQTY = txtPOPendingOilQTY.Text;
                        objTestPending.StoreAlradyIssued = txtStoreAlradyIssued.Text;
                        
                        if (txtRepairerAlreadyIssued.Text!="")
                        {
                            objTestPending.RepairerAlreadyIssued = txtRepairerAlreadyIssued.Text;
                        }
                        else
                        {
                            objTestPending.RepairerAlreadyIssued = "0";
                        }
                     
                        if(Convert.ToDouble(txtRepairerSupplyingOilQTY.Text) > Convert.ToDouble(objTestPending.POPendingOilQTY))
                        {
                            ShowMsgBox("Oil supplying by repairer quantity should be less than or equal to pending oil quantity");
                            return;
                        }
                        objTestPending.OilSupplyingBy = cmdOilSupplyingBy.SelectedValue;
                        objTestPending.RepairerSupplyingOilQTY = txtRepairerSupplyingOilQTY.Text;
                   

                        double QuantityAmt = Convert.ToDouble(objTestPending.RepairerSupplyingOilQTY) + Convert.ToDouble(objTestPending.StoreAlradyIssued) + Convert.ToDouble(objTestPending.RepairerAlreadyIssued);
                        double TotalRPOilQty = (Convert.ToDouble(objTestPending.Pototaloilqty)) - QuantityAmt;
                        objTestPending.TotalRemaingPendingOilQty = Convert.ToString(TotalRPOilQty);

                        double TotalRPoilQtyInKltr = Convert.ToDouble(objTestPending.RepairerSupplyingOilQTY) / 1000;
                        objTestPending.TotalRPoilQtyInKltr = Convert.ToString(TotalRPoilQtyInKltr);
                    }

                    //ends here
                    int i = 0;
                    objTestPending.sTestResult = "0";
                    bool bChecked = false;

                    string[] strQrylist = new string[grdDeliverDetails.Rows.Count];
                    DataTable dtimage = new DataTable();
                    DataColumn dc = new DataColumn("image");
                    dc.DataType = System.Type.GetType("System.Byte[]");
                    dtimage.Columns.Add(dc);
                    dtimage.Columns.Add("RSDID", typeof(string));

                    foreach (GridViewRow row in grdDeliverDetails.Rows)
                    {
                        // For Pass value will be 1 ;   For Fail value will be 0; For Scrap value will be 3; For Send to store (none) value will be 4
                        // For Pass With Copper Loss B.P.L. 5 ; For Pass With Core Loss B.P.L. 6; For Pass With Both Copper And Core Loss B.P.L. 7
                        bool result = ((RadioButton)row.FindControl("rdbPass")).Checked;
                        Byte[] Buffer;
                        bChecked = false;
                        FileUpload fupDoc = (FileUpload)row.FindControl("fupdDoc");
                        if (result)
                        {
                            objTestPending.sTestResult = "1";
                            bChecked = true;
                        }

                        result = ((RadioButton)row.FindControl("rdbFail")).Checked;
                        if (result)
                        {
                            objTestPending.sTestResult = "0";
                            bChecked = true;
                        }

                        //New Code For Scrap
                        result = ((RadioButton)row.FindControl("rdbScrap")).Checked;
                        if (result)
                        {
                            objTestPending.sTestResult = "3";
                            bChecked = true;
                        }

                        result = ((RadioButton)row.FindControl("rdbSendToStore")).Checked;
                        if (result)
                        {
                            objTestPending.sTestResult = "4";
                            bChecked = true;
                        }

                        result = ((RadioButton)row.FindControl("rdbPasswithCopper")).Checked;
                        if (result)
                        {
                            objTestPending.sTestResult = "5";
                            bChecked = true;
                        }
                        result = ((RadioButton)row.FindControl("rdbPassCore")).Checked;
                        if (result)
                        {
                            objTestPending.sTestResult = "6";
                            bChecked = true;
                        }
                        result = ((RadioButton)row.FindControl("rdbPassBoth")).Checked;
                        if (result)
                        {
                            objTestPending.sTestResult = "7";
                            bChecked = true;
                        }

                        string sRemarks = ((TextBox)row.FindControl("txtRemarks")).Text;

                        #region SaveImage
                        if (fupDoc.PostedFile.ContentLength != 0)
                        {
                            string filename = Path.GetFileName(fupDoc.PostedFile.FileName);
                            string strExt = filename.Substring(filename.LastIndexOf('.') + 1);
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

                        strQrylist[i] = ((Label)row.FindControl("lbltransid")).Text.Trim() + "~" + objTestPending.sTestResult + "~" + sRemarks.Replace("'", "`") + "~" + objTestPending.sFileName;
                        i++;



                        if (bChecked == false)
                        {
                            ShowMsgBox("Please select Test Result of all DTr Codes.");
                            return;
                        }
                        if ((sRemarks ?? "").Length == 0)
                        {
                            ShowMsgBox("Please Enter Remarks to Transformers");
                            return;
                        }
                    }
                    Session["fileupload"] = dtimage;
                    //check for image already saved
                    //if (CheckImageAlreaySaved() == false)
                    //{
                    //    ShowMsgBox("Can Not Upload Dublicate Image");
                    //    return;
                    //}
                    Arr = objTestPending.SaveTestingTCDetails(strQrylist, objTestPending, dtimage);
                    if (Arr[1].ToString() == "0")
                    {
                        //ShowMsgBox(Arr[0].ToString());
                        SaveFilePathMTTest();
                        grdDeliverDetails.DataSource = null;
                        grdDeliverDetails.DataBind();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), UniqueID, "alert('" + Arr[0].ToString() + "'); location.href='TestPendingSearch.aspx';", true);
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

                objApproval.sFormName = "TCRepairIssue";
                objApproval.sRoleId = objSession.RoleId;
                objApproval.sAccessType = "1" + "," + sAccessType;
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

        #region SaveImages

        public bool SaveFilePath(clsStoreEnumeration objStoreEnum)
        {
            try
            {
                //FTP Parameter
                string sFTPLink = string.Empty;
                string sFTPUserName = string.Empty;
                string sFTPPassword = string.Empty;

                string sTestFileExt = string.Empty;
                string sTestFileName = string.Empty;
                string sDirectory = string.Empty;

                //  Photo Save DTLMSDocs
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

                // Create Directory

                bool IsExists = objFtp.FtpDirectoryExists(mainfolder + objStoreEnum.sEnumDetailsId + "/");
                if (IsExists == false)
                {

                    objFtp.createDirectory(mainfolder + objStoreEnum.sEnumDetailsId);
                }

                //  Photo Save DTLMSDocs
                string sFileExt = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["NotAllowFileFormat"]);

                if (fupTestDocument.PostedFile.ContentLength != 0)
                {

                    sTestFileExt = System.IO.Path.GetExtension(fupTestDocument.FileName).ToString().ToLower();
                    sTestFileExt = ";" + sTestFileExt.Remove(0, 1) + ";";

                    if (sFileExt.Contains(sTestFileExt))
                    {
                        ShowMsgBox("Invalid File Format");
                        return false;
                    }

                    sTestFileName = System.IO.Path.GetFileName(fupTestDocument.PostedFile.FileName);

                    fupTestDocument.SaveAs(Server.MapPath("~/DTLMSFiles" + "/" + sTestFileName));
                    sDirectory = Server.MapPath("~/DTLMSFiles" + "/" + sTestFileName);

                }


                if (sTestFileName != "")
                {

                    if (File.Exists(sDirectory))
                    {
                        IsExists = objFtp.FtpDirectoryExists(mainfolder + objStoreEnum.sEnumDetailsId + "/" + "TESTING" + "/");
                        if (IsExists == false)
                        {

                            objFtp.createDirectory(mainfolder + objStoreEnum.sEnumDetailsId + "/" + "TESTING");
                        }

                        Isuploaded = objFtp.upload(mainfolder + objStoreEnum.sEnumDetailsId + "/" + "TESTING" + "/", objSession.UserId + "~" + sTestFileName, sDirectory);
                        if (Isuploaded == true & File.Exists(sDirectory))
                        {
                            File.Delete(sDirectory);
                            objStoreEnum.sNamePlatePhotoPath = objStoreEnum.sEnumDetailsId + "/" + "TESTING" + "/" + objSession.UserId + "~" + sTestFileName;

                        }
                    }
                }

                bool bResult;

                return true;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
                lblMessage.Text = clsException.ErrorMsg();
                return false;
            }
        }

        #endregion
        private void LoadTestingPendingTC()
        {
            try
            {
                clsDTrRepairActivity objTestpending = new clsDTrRepairActivity();

                DataTable dt = new DataTable();
                txtSelectedDetailsId.Text = txtSelectedDetailsId.Text.Replace("~", ",");
                if (!txtSelectedDetailsId.Text.StartsWith(","))
                {
                    txtSelectedDetailsId.Text = "," + txtSelectedDetailsId.Text;
                }
                if (!txtSelectedDetailsId.Text.EndsWith(","))
                {
                    txtSelectedDetailsId.Text = txtSelectedDetailsId.Text + ",";
                }


                txtSelectedDetailsId.Text = txtSelectedDetailsId.Text.Substring(1, txtSelectedDetailsId.Text.Length - 1);
                if ((txtSelectedDetailsId.Text ?? "").Length > 0)
                {
                    txtSelectedDetailsId.Text = txtSelectedDetailsId.Text.Substring(0, txtSelectedDetailsId.Text.Length - 1);


                    objTestpending.sRepairDetailsId = txtSelectedDetailsId.Text;
                    objTestpending.sOfficeCode = objSession.OfficeCode;
                    objTestpending.sPurchaseOrderNo = txtPONo.Text;
                    objTestpending.sTestingDone = "0";

                    dt = objTestpending.LoadTestOrDeliverPendingDTR(objTestpending);
                }
                if (dt.Rows.Count > 0)
                {
                    txtPONo.Text = Convert.ToString(dt.Rows[0]["RSM_PO_NO"]);
                    txtIssueDate.Text = Convert.ToString(dt.Rows[0]["RSM_ISSUE_DATE"]);
                    txtOldPONo.Text = Convert.ToString(dt.Rows[0]["RSM_OLD_PO_NO"]);
                    txtPO_Remarks.Text = Convert.ToString(dt.Rows[0]["RSM_REMARKS"]);
                    txtrsmoiltype.Text = Convert.ToString(dt.Rows[0]["RSM_OIL_TYPE"]);
                    hdfrsmoiltype.Value = txtrsmoiltype.Text;
                    DateTime Issuedate = Convert.ToDateTime(txtIssueDate.Text);
                    int Isuedate = Convert.ToInt32((DateTime.Now.AddDays(-1) - Issuedate).TotalDays);
                    string temp = DateTime.Today.AddDays(-Isuedate).ToString("yyyy-MM-dd");
                    hiddenTestedOn.Text = temp;
                }
                grdDeliverDetails.DataSource = dt;
                grdDeliverDetails.DataBind();
                ViewState["TestDTR"] = dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
                lblMessage.Text = clsException.ErrorMsg();
            }

        }

        /// <summary>
        /// newly added by santhosh on 18-01-2023
        /// </summary>
        private void LoadOilDetails()
        {
            try
            {
                clsDTrRepairActivity objLoadOilDetails = new clsDTrRepairActivity();

                DataTable dtLoadOilDetails = new DataTable();

                objLoadOilDetails.sPurchaseOrderNo = txtPONo.Text;
                objLoadOilDetails.sOfficeCode = objSession.OfficeCode;
                objLoadOilDetails.sRoleId = objSession.RoleId;

                dtLoadOilDetails = objLoadOilDetails.LoadOilDetailsOnPONO(objLoadOilDetails);

                if (dtLoadOilDetails.Rows.Count > 0)
                {
                    txtPOTotalOilQTY.Text = Convert.ToString(dtLoadOilDetails.Rows[0]["TOTAL_OIL_AMOUNT"]);
                    txtPOPendingOilQTY.Text = Convert.ToString(dtLoadOilDetails.Rows[0]["ROI_PENDINGQTY"]);
                    txtStoreAlradyIssued.Text = Convert.ToString(dtLoadOilDetails.Rows[0]["STORE_SENT_OIL"]);
                    txtRepairerAlreadyIssued.Text = Convert.ToString(dtLoadOilDetails.Rows[0]["REPAIRER_SENT_OIL"]);
                    if (Convert.ToString(dtLoadOilDetails.Rows[0]["REPAIRER_SENT_OIL"])=="")
                    {
                        txtRepairerAlreadyIssued.Text = "0";
                    }
                    if (Convert.ToString(dtLoadOilDetails.Rows[0]["STORE_SENT_OIL"]) == "")
                    {
                        txtStoreAlradyIssued.Text = "0";
                    }
                    double Sentoil = Convert.ToDouble(txtStoreAlradyIssued.Text) + Convert.ToDouble(txtRepairerAlreadyIssued.Text);
                    string pototaloil=  String.IsNullOrEmpty(txtPOTotalOilQTY.Text) ? "0" : txtPOTotalOilQTY.Text;

                    double TotalOilQty = Convert.ToDouble(pototaloil);
                    double CalculatedPendingQty = TotalOilQty - Sentoil;
                    txtPOPendingOilQTY.Text = CalculatedPendingQty.ToString();
                }

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
                lblMessage.Text = clsException.ErrorMsg();
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

                objTestpending = objTestpending.LoadTestedDTR(objTestpending);

                dt = objTestpending.dtTestDone;

                if (dt.Rows.Count > 10)
                {
                    //div1.Style.Add("overflow", "scroll");
                }
                if (dt.Rows.Count > 0)
                {
                    txtPONo.Text = Convert.ToString(dt.Rows[0]["RSM_PO_NO"]);
                    txtIssueDate.Text = Convert.ToString(dt.Rows[0]["RSM_ISSUE_DATE"]);

                    cmbTestedBy.SelectedValue = objTestpending.sTestedBy;
                    txtTestedOn.Text = objTestpending.sTestedOn;
                    cmbTestLocation.Text = objTestpending.sTestLocation;

                    hdfRemarks.Value = objTestpending.sInspRemarks;
                    hdfResult.Value = objTestpending.sTestResult;
                    txtPO_Remarks.Text= Convert.ToString(dt.Rows[0]["RSM_REMARKS"]);

                }
                grdDeliverDetails.DataSource = dt;
                grdDeliverDetails.DataBind();
                ViewState["TestDTR"] = dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
                lblMessage.Text = clsException.ErrorMsg();
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
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        public bool ValidateForm()
        {
            bool bValidate = false;
            try
            {
                if (cmbStore.SelectedIndex == 0)
                {
                    ShowMsgBox("Please Select the Store");
                    cmbStore.Focus();
                    return bValidate;
                }

                 if (cmbTestedBy.SelectedIndex == 0)
                {
                    ShowMsgBox("Please Enter Tested By");
                    cmbTestedBy.Focus();
                    return bValidate;
                }
                 if (txtrsmoiltype.Text=="0")
                {
                    if (cmdOilSupplyingBy.SelectedIndex == 0)
                    {
                        ShowMsgBox("Please Select Oil Supplying By.");
                        cmdOilSupplyingBy.Focus();
                        return bValidate;
                    }
                }


                 if (cmdOilSupplyingBy.SelectedIndex == 2)
                {

                    if (txtRepairerSupplyingOilQTY.Text == "")
                    {
                        ShowMsgBox("Please Enter Repairer Supplying Oil Quantity.(Ltrs).");
                        txtRepairerSupplyingOilQTY.Focus();
                        return bValidate;
                    }

                    if (txtRepairerSupplyingOilQTY.Text != "")
                    {
                        string oilqty = txtRepairerSupplyingOilQTY.Text;
                        string qty = oilqty.Trim('0').Replace(".", "");

                        if (qty == "")
                        {
                            ShowMsgBox("Enter Valid Oil Quantity");
                            txtRepairerSupplyingOilQTY.Focus();
                            return bValidate;
                        }
                        int dotCount = 0;
                        foreach (char c in txtRepairerSupplyingOilQTY.Text)
                        {
                            if (c == '.')
                            {
                                dotCount++;
                            }
                        }
                        if (dotCount >= 2)
                        {
                            ShowMsgBox("Enter Valid Oil Quantity");
                            txtRepairerSupplyingOilQTY.Focus();
                            return bValidate;
                        }
                    }

                }

                bValidate = true;
                return bValidate;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
                return bValidate;
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
                //cmbStore.SelectedIndex = 0;

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

        protected void grdDeliverDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "Remove")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                    int iRowIndex = row.RowIndex;

                    DataTable dt = (DataTable)ViewState["TestDTR"];
                    dt.Rows[iRowIndex].Delete();
                    if (dt.Rows.Count == 0)
                    {
                        ViewState["TestDTR"] = null;
                    }
                    else
                    {
                        ViewState["TestDTR"] = dt;
                    }

                    grdDeliverDetails.DataSource = dt;
                    grdDeliverDetails.DataBind();
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        protected void grdDeliverDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (cmbTestedBy.SelectedIndex > 0)
                    {
                        RadioButton rdbPass = (RadioButton)e.Row.FindControl("rdbPass");
                        RadioButton rdbFail = (RadioButton)e.Row.FindControl("rdbFail");
                        RadioButton rdbScrap = (RadioButton)e.Row.FindControl("rdbScrap");
                        RadioButton rdbSendToStore = (RadioButton)e.Row.FindControl("rdbSendToStore");

                        RadioButton rdbPasswithCopper = (RadioButton)e.Row.FindControl("rdbPasswithCopper");
                        RadioButton rdbPassCore = (RadioButton)e.Row.FindControl("rdbPassCore");
                        RadioButton rdbPassBoth = (RadioButton)e.Row.FindControl("rdbPassBoth");
                        TextBox txtRemarks = (TextBox)e.Row.FindControl("txtRemarks");

                        if (hdfResult.Value == "0")
                        {
                            rdbFail.Checked = false;
                        }
                        else if (hdfResult.Value == "1")
                        {
                            rdbPass.Checked = true;
                        }
                        else if (hdfResult.Value == "3")
                        {
                            rdbScrap.Checked = true;
                        }
                        else if (hdfResult.Value == "4")
                        {
                            rdbSendToStore.Checked = true;
                        }

                        else if (hdfResult.Value == "5")
                        {
                            rdbPasswithCopper.Checked = true;
                        }
                        else if (hdfResult.Value == "6")
                        {
                            rdbPassCore.Checked = true;
                        }
                        else if (hdfResult.Value == "7")
                        {
                            rdbPassBoth.Checked = true;
                        }

                        txtRemarks.Text = hdfRemarks.Value;

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
                foreach (GridViewRow grdRow in grdDeliverDetails.Rows)
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

        protected void SaveFilePathMTTest()
        {
            try
            {

                foreach (GridViewRow row in grdDeliverDetails.Rows)
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

                        // Create Directory

                        //bool IsExists = objFtp.FtpDirectoryExists(mainfolder + "HT_TESTING"+ "/" + Rsd_id + "/");
                        bool IsExists = objFtp.FtpDirectoryExists(mainfolder + "HT_TESTING");

                        if (IsExists == false)
                        {

                            objFtp.createDirectory(mainfolder + "HT_TESTING");
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
                                IsExists = objFtp.FtpDirectoryExists(mainfolder + "HT_TESTING" + "/" + Rsd_id);
                                if (IsExists == false)
                                {

                                    objFtp.createDirectory(mainfolder + "HT_TESTING" + "/" + Rsd_id);
                                }

                                Isuploaded = objFtp.upload(mainfolder + "HT_TESTING" + "/" + Rsd_id + "/",  sTestFileName, sDirectory);
                                if (Isuploaded == true & File.Exists(sDirectory))
                                {
                                    File.Delete(sDirectory);
                                    clsDTrRepairActivity objinddoc = new clsDTrRepairActivity();

                                    objinddoc.UploadedpathHt = "HT_TESTING" + "/" + Rsd_id + "/" + sTestFileName;

                                    objinddoc.RepairDetailsId = Rsd_id;
                                    objinddoc.UpdateHT_Uploadedpath(objinddoc);

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
