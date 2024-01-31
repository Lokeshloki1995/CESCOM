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
using IIITS.DTLMS.BL.OilFlow;
using System.Configuration;
using System.Globalization;

namespace IIITS.DTLMS.OilFlow
{
  
    public partial class OilTesting : System.Web.UI.Page
    {

        string strFormCode = "OilTesting";
        string flag;
        string invoice;
        clsSession objSession;
        string Percentage = ConfigurationManager.AppSettings["Percentage"].ToString();
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
                txtpodate.Enabled = false;
                CalendarExtender1.EndDate = DateTime.Now;
                //CalenderTestedOn.EndDate = DateTime.Now;

                //txtIssueDate.Attributes.Add("readonly", "readonly");
                txtTestedOn.Attributes.Add("readonly", "readonly");

                if (!IsPostBack)
                {
                    //CalendarExtender1.EndDate = System.DateTime.Now;
                    if (Request.QueryString["PoNo"] != null && Request.QueryString["PoNo"].ToString() != "")
                    {
                        if (Session["RepairDetailsId"] != null && Session["RepairDetailsId"].ToString() != "")
                        {
                            txtSelectedDetailsId.Text = Session["RepairDetailsId"].ToString();
                            // Session["RepairDetailsId"] = null;
                        }
                        txtPONo.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["PoNo"]));

                        //invoice = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["InvoiceNo"]));

                        //string sItemCode = cmbItemCode.SelectedItem.Text.Split('-').GetValue(0).ToString();
                        //string sItemName = cmbItemCode.SelectedItem.Text.Split('-').GetValue(1).ToString();

                        LoadTestingPendingTC();
                    }
                    //DateTime testedon = DateTime.Now;
                    //testedon = Convert.ToDateTime(txtpodate.Text);
                    //CalendarExtender1.StartDate = testedon;
                   // DateTime testedon = DateTime.Now;
                   string testedon = txtpodate.Text;
                    DateTime DToDate = DateTime.ParseExact(testedon, "d-M-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string sFromDate = DToDate.ToString("yyyy/MM/dd");
                    CalendarExtender1.StartDate = Convert.ToDateTime(sFromDate);




                    string sOfficeCode = string.Empty;
                    if (objSession.OfficeCode.Length > 2)
                    {
                        sOfficeCode = objSession.OfficeCode.Substring(0, 2);
                    }
                    else
                    {
                        sOfficeCode = objSession.OfficeCode;
                    }
                    //Genaral.Load_Combo("SELECT US_ID,US_FULL_NAME FROM TBLUSER WHERE NVL(US_EFFECT_FROM,SYSDATE-1)<=SYSDATE AND US_STATUS='A' AND US_ROLE_ID='31' AND US_OFFICE_CODE LIKE '" + sOfficeCode + "%'", "--Select--", cmbTestedBy);
                    Genaral.Load_Combo("SELECT US_ID,US_FULL_NAME FROM TBLUSER WHERE NVL(US_EFFECT_FROM,SYSDATE-1)<=SYSDATE AND US_STATUS='A' AND US_ID='"+objSession.UserId+"' ", cmbTestedBy);


                    if (Request.QueryString["WFOID"] != null && Request.QueryString["WFOID"].ToString() != "")
                    {
                        txtWOno.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["WFOID"]));
                    }
                    if (Request.QueryString["ActionType"] != null && Request.QueryString["ActionType"].ToString() != "")
                    {
                        txtActiontype.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));
                    }

            
                    //From DTR Tracker
                    if (Request.QueryString["Recordid"] != null && Request.QueryString["Recordid"].ToString() != "")
                    {
                        string sRecordid = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["Recordid"]));
                        WorkFlowConfig();
                    }
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load");
                lblMessage.Text = ex.Message;
            }
        }


        public void WorkFlowConfig()
        {
            try
            {
                if (Request.QueryString["ActionType"] != null && Request.QueryString["ActionType"].ToString() != "")
                {
                    //WFDataId

                    if (Session["WFOId"] != null && Session["WFOId"].ToString() != "")
                    {
                        hdfWFDataId.Value = Session["WFDataId"].ToString();
                        //hdfWFOId.Value = Convert.ToString(Session["WFOId"]);
                        hdfWFOId.Value = Session["WFOId"].ToString();
                        hdfApproveStatus.Value = Convert.ToString(Session["ApproveStatus"]);
                        hdfWFOAutoId.Value = Convert.ToString(Session["WFOAutoId"]);


                        Session["WFDataId"] = null;
                        Session["WFOId"] = null;
                        Session["ApproveStatus"] = null;
                        Session["WFOAutoId"] = null;
                    }


                    GetFailureDetailsFromXML(hdfWFDataId.Value);

                    if (txtActiontype.Text == "M")
                    {
                        cmdSave.Text = "Modify and Approve";
                        cmdReset.Visible = false;
                    }


                    if (txtActiontype.Text == "A")
                    {
                        cmdSave.Text = "Approve";
                        cmdReset.Visible = false;
                    }

                    if (txtActiontype.Text == "R")
                    {
                        cmdSave.Text = "Reject";
                        cmdReset.Visible = false;
                    }
                }
                else
                {
                    if (cmdSave.Text != "Save" && cmdSave.Text != "View")
                    {
                        cmdSave.Enabled = false;

                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
            }
        }
        private string GetLastPricingUpdate(string offCode)
        {
            clsCommon objCommon = new clsCommon();
            try
            {
                return null;
                //return objCommon.GetLastPricingDate(offCode);
            }
            catch (Exception ex)
            {

                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetLastPricingUpdate");
                return "Something Went Wrong";
            }
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                string[] Arr = new string[2];
                double Temp = 0;

                //Check AccessRights
                //bool bAccResult = CheckAccessRights("1");
                // if (bAccResult == false)
                // {
                //     return;
                // }


                if (ValidateForm() == true)
                {

                    clsOilTest objoiltest = new clsOilTest();

                    //objoiltest.sTestedBy = cmbTestedBy.SelectedValue;
                    objoiltest.sStoreId = txtDivision.Text;
                    //objoiltest.sDesignation = cmbDesignation.SelectedValue;
                    objoiltest.sTestedOn = txtTestedOn.Text;
                    objoiltest.sPurchaseOrderNo = txtPONo.Text;
                    //objoiltest.sIssueDate = txtIssueDate.Text;
                    objoiltest.sCrby = objSession.UserId;
                    objoiltest.sTestLocation = cmbTestLocation.SelectedValue;
                    objoiltest.sTestedBy = cmbTestedBy.SelectedValue;
                    objoiltest.sOfficeCode = objSession.OfficeCode;
                    //objoiltest.sInsQty  = txtInspection.Text;
           




                    //objoiltest.sWOId = txtWOno.Text;

                    if (txtActiontype.Text == "A" || txtActiontype.Text == "R")
                    {
                        ApproveRejectAction();
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "Failure");
                        }
                        return;
                    }

                    WorkFlowObjects(objoiltest);



                    int i = 0;
                    objoiltest.sTestResult = "0";
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
                        //bool result = ((RadioButton)row.FindControl("rdbPass")).Checked;
                        //Byte[] Buffer;
                        //bChecked = false;
                        //FileUpload fupDoc = (FileUpload)row.FindControl("fupdDoc");
                        //if (result)
                        //{
                        //    objoiltest.sTestResult = "1";
                        //    bChecked = true;
                        //}

                        //result = ((RadioButton)row.FindControl("rdbFail")).Checked;
                        //if (result)
                        //{
                        //    objoiltest.sTestResult = "0";
                        //    bChecked = true;
                        //}

                        string sRemarks = ((TextBox)row.FindControl("txtRemarks")).Text;
                        string sInsQty = ((TextBox)row.FindControl("txtInspection")).Text;
                        string TotalQuantity = ((Label)row.FindControl("lblQuantity")).Text;

                        #region SaveImage
                        //if (fupDoc.PostedFile.ContentLength != 0)
                        //{
                        //    string filename = Path.GetFileName(fupDoc.PostedFile.FileName);
                        //    string strExt = filename.Substring(filename.LastIndexOf('.') + 1);
                        //    if (strExt.ToLower().Equals("jpg") || strExt.ToLower().Equals("jpeg") || strExt.ToLower().Equals("png") || strExt.ToLower().Equals("gif") || strExt.ToLower().Equals("pdf"))
                        //    {
                        //        Stream strm = fupDoc.PostedFile.InputStream;
                        //        Buffer = new byte[strm.Length];
                        //        strm.Read(Buffer, 0, (int)strm.Length);

                        //        dtimage.Rows.Add(Buffer, ((Label)row.FindControl("lblTcCode")).Text.Trim());

                        //    }
                        //    else
                        //    {
                        //        ShowMsgBox("Invalid File");
                        //        return;
                        //    }
                        //}
                        #endregion

                        strQrylist[i] = ((Label)row.FindControl("lblPONO")).Text.Trim() + "~" + objoiltest.sTestResult + "~" + sRemarks.Replace("'", "`") + "~" + objoiltest.sFileName +"~" + ((TextBox)row.FindControl("txtInspection")).Text + "~" + ((Label)row.FindControl("lblQuantity")).Text;
                        i++;

                        if(sInsQty=="")
                        {
                            ShowMsgBox("Please Enter Inspection Quantity");
                            return;
                        }

                        if (sInsQty == "0"|| sInsQty == "00"|| sInsQty == "000"|| sInsQty == "0000")
                        {
                            ShowMsgBox("Please Enter Valid Inspection Quantity");
                            return;
                        }

                        if (hdfpendingqty.Value!="")
                        {
                            Temp = Convert.ToDouble(hdfpendingqty.Value);
                        }
                        else
                        {
                            Temp = Convert.ToDouble(TotalQuantity) * Convert.ToDouble(Percentage) / 100;
                        }
                     
                       
                      


                        if (Temp < Convert.ToDouble(sInsQty))
                        {
                            ShowMsgBox("Inspection Quantity Should Be Less Than Or Equals To Available Quantity");
                            return;
                        }


                        //if (bChecked == false)
                        //{
                        //    ShowMsgBox("Please Add Testing Result");
                        //    return;
                        //}
                    }

                    if (txtActiontype.Text == "M")
                    {

                        objoiltest.sPurchaseOrderNo = txtPONo.Text;
                        objoiltest.sActionType = txtActiontype.Text;
                        objoiltest.sOfficeCode = objSession.OfficeCode;
                        objoiltest.sCrby = objSession.UserId;
                        Arr = objoiltest.SaveOiltestapprovalDetails(strQrylist, objoiltest);
                        if (Arr[1].ToString() == "0")
                        {
                            hdfWFDataId.Value = objoiltest.sWFDataId;
                            ApproveRejectAction();
                            if (objSession.sTransactionLog == "1")
                            {
                                Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "Failure");
                            }

                            return;
                        }
                        if (Arr[1].ToString() == "2")
                        {
                            ShowMsgBox(Arr[0]);
                            if (objSession.sTransactionLog == "1")
                            {
                                Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "Failure");
                            }
                            return;
                        }

                    }

                    if (fupTestDocument.PostedFile.ContentLength != 0)
                    {

                        objoiltest.sOilTestFileName = System.IO.Path.GetFileName(fupTestDocument.PostedFile.FileName);

                        fupTestDocument.SaveAs(Server.MapPath("~/DTLMSFiles" + "/OlL_TESTING_DOCS" + "/" + objoiltest.sOilTestFileName));
                        string sDirectory = Server.MapPath("~/DTLMSFiles" + "/OlL_TESTING_DOCS" + "/" + objoiltest.sOilTestFileName);
                        objoiltest.sOilFilePath = sDirectory;
                    }



                    Session["fileupload"] = dtimage;
                    //check for image already saved
                    //if (CheckImageAlreaySaved() == false)
                    //{
                    //    ShowMsgBox("Can Not Upload Dublicate Image");
                    //    return;
                    //}
                    //Arr = objoiltest.SaveOiltestapprovalDetails(strQrylist, objoiltest);
                    Arr = objoiltest.SaveOilDetails(strQrylist, objoiltest); 

                    if (Arr[1].ToString() == "0")
                    {
                        //ShowMsgBox(Arr[0].ToString());
                        hdfWFDataId.Value = objoiltest.sWFDataId;
                        grdDeliverDetails.DataSource = null;
                        grdDeliverDetails.DataBind();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), UniqueID, "alert('" + Arr[0].ToString() + "'); location.href='PendingAgencyTest.aspx';", true);
                        Reset();
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "RepairTrasnaction-Testing");
                        }

                        return;
                    }
                    else
                    {
                        ShowMsgBox("No Oils Exists to Inspect");
                    }
                }


            }

            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdSave_Click");
                lblMessage.Text = clsException.ErrorMsg();
            }
        }
        public void WorkFlowObjects(clsOilTest objoiltest)
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


                objoiltest.sFormName = "OilTesting";
                objoiltest.sOfficeCode = objSession.OfficeCode;
                objoiltest.sClientIP = sClientIP;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "WorkFlowObjects");
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
                //if (txtEnumDetailsId.Text.Trim() == "")
                //{
                //    bResult = objStoreEnum.SaveImagePathDetails(objStoreEnum);
                //}
                //else
                //{
                //    bResult = objStoreEnum.UpdateImagePathDetails(objStoreEnum);
                //}

                return true;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdSave_Click");
                lblMessage.Text = clsException.ErrorMsg();
                return false;
            }
        }

        #endregion
        private void LoadTestingPendingTC()
        {
            try
            {
                clsOilTest objoiltest = new clsOilTest();

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
                txtSelectedDetailsId.Text = txtSelectedDetailsId.Text.Substring(0, txtSelectedDetailsId.Text.Length - 1);

                objoiltest.sRepairDetailsId = txtSelectedDetailsId.Text;

                objoiltest.sOfficeCode = objSession.OfficeCode;
                objoiltest.sPurchaseOrderNo = txtPONo.Text;
                objoiltest.sInvoiceNo = invoice;
                objoiltest.sTestingDone = "0";

                dt = objoiltest.LoadTestDeliverPendingoil(objoiltest);
                objoiltest.sInspectedQty = objoiltest.LoadInspectedoil(objoiltest);

                if (dt.Rows.Count > 0)
                {
                    txtPONo.Text = Convert.ToString(dt.Rows[0]["OSD_PO_NO"]);
                    //txtIssueDate.Text = Convert.ToString(dt.Rows[0]["ASD_PO_DATE"]);
                    txtDivision.Text = Convert.ToString(dt.Rows[0]["OSD_OFFICE_CODE"]);
                    string Invoiceqty = Convert.ToString(dt.Rows[0]["OSD_QUANTITY"]);
                    txtpodate.Text = Convert.ToString(dt.Rows[0]["OSD_PO_DATE"]);
                    string QtyPercentageValue = Convert.ToString(dt.Rows[0]["OSD_PERCENTAGE_VALUE"]);
                    

                    if (objoiltest.sInspectedQty != "" && objoiltest.sInspectedQty != null)
                    {
                        //objoiltest.sPendingQty = (Convert.ToInt32(Invoiceqty) - Convert.ToInt32(objoiltest.sInspectedQty)) * Convert.ToInt32(Percentage) / 100;
                        objoiltest.sPendingQty = (Convert.ToDouble(QtyPercentageValue) - Convert.ToDouble(objoiltest.sInspectedQty));
                        lblInspqty.Text = "Available Quantity To Inspect : " + objoiltest.sPendingQty;
                        hdfpendingqty.Value = Convert.ToString(objoiltest.sPendingQty);
                    }
                    else
                    {
                        objoiltest.sPendingQty = Convert.ToDouble(Invoiceqty) * Convert.ToDouble(Percentage) / 100;
                        lblInspqty.Text = "Available Quantity To Inspect : " + objoiltest.sPendingQty;
                    }
                }
                grdDeliverDetails.DataSource = dt;
                grdDeliverDetails.DataBind();
                ViewState["TestDTR"] = dt;
                
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadTestingPendingTC");
                lblMessage.Text = clsException.ErrorMsg();
            }

        }
        public void LoadPendingApprovalInbox(string sFormName = "", string sDesc = "")
        {
            try
            {
                string sFilter = string.Empty;
                DataTable dtView = new DataTable();
                DataView dv = new DataView();
                clsOilTest objoiltest = new clsOilTest();
                objoiltest.sRoleId = objSession.RoleId;
                objoiltest.sOfficeCode = objSession.OfficeCode;

                objoiltest.sWOId = txtWOno.Text;


                //objApproval.sToDate = txtToDate.Text;

                objoiltest.sFormName = sFormName;
                objoiltest.sDescription = sDesc;

                if (objoiltest.sBOId == "7")
                {
                    dtView = (DataTable)ViewState["TestDTR"];
                    dv = dtView.DefaultView;
                    sFilter = "BO_NAME ='DeCommission Entry' AND WO_DESCRIPTION LIKE 'Commissioning of DTC%' ";
                    dv.RowFilter = sFilter;
                    if (dv.Count > 0)
                    {
                        grdDeliverDetails.DataSource = dv;
                        ViewState["TestDTR"] = dv.ToTable();
                        grdDeliverDetails.DataBind();

                    }
                    else
                    {

                        //ShowEmptyGrid();
                    }

                }
                else
                {

                    DataTable dt = objoiltest.GetWOBasicDetails(objoiltest);
                    //if (dt.Rows.Count == 0)
                    //{
                    //    //ShowEmptyGrid();
                    //    ViewState["TestDTR"] = dt;
                    //}
                    //else
                    {
                        grdDeliverDetails.DataSource = dt;
                        grdDeliverDetails.DataBind();
                        ViewState["TestDTR"] = dt;
                    }
                }

            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadApprovalInbox");
            }
        }


        private void LoadTestingDoneDTR(string sRecordid)
        {
            try
            {
                clsOilTest objoiltest = new clsOilTest();

                DataTable dt = new DataTable();

                //objoiltest.sTcCode = sDTrCode;
                objoiltest.sTestInspectionId = sRecordid;

                objoiltest = objoiltest.LoadTestedDTR(objoiltest);

                dt = objoiltest.dtTestDone;

                if (dt.Rows.Count > 10)
                {
                    //div1.Style.Add("overflow", "scroll");
                }
                if (dt.Rows.Count > 0)
                {
                    txtTestedOn.Text = objoiltest.sTestedOn;
                    cmbTestLocation.Text = objoiltest.sTestLocation;

                    hdfRemarks.Value = objoiltest.sInspRemarks;
                    hdfResult.Value = objoiltest.sTestResult;

                }
                grdDeliverDetails.DataSource = dt;
                grdDeliverDetails.DataBind();
                ViewState["TestDTR"] = dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadTestingPendingTC");
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
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowMsgBox");
            }
        }

        public bool ValidateForm()
        {
            bool bValidate = false;
            try
            {

                if (cmbTestLocation.SelectedIndex == 0)
                {
                    ShowMsgBox("Please Select Testing Location");
                    cmbTestLocation.Focus();
                    return bValidate;
                }
                //if (cmbTestedBy.SelectedIndex == 0)
                if (cmbTestedBy.Text == "")
                {
                    ShowMsgBox("Please Select Tested By");
                    cmbTestLocation.Focus();
                    return bValidate;
                }
                if (txtTestedOn.Text == "")
                {
                    ShowMsgBox("Please Select Tested On Date");
                    txtTestedOn.Focus();
                    return bValidate;
                }
                string sFileExt = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["NotAllowFileFormat"]);
                if (fupTestDocument.PostedFile.ContentLength != 0)
                {

                    string sWOFileExt = string.Empty;
                    string sWOFileName = string.Empty;
                    string sDirectory = string.Empty;


                    sWOFileExt = System.IO.Path.GetExtension(fupTestDocument.FileName).ToString().ToLower();
                    sWOFileExt = ";" + sWOFileExt.Remove(0, 1) + ";";

                    if (sFileExt.Contains(sWOFileExt))
                    {
                        ShowMsgBox("Invalid File Format");
                        return false;
                    }

                }

                if (fupTestDocument.PostedFile.ContentLength == 0)
                {
                    ShowMsgBox("Please Select The Inspection Report File");
                    fupTestDocument.Focus();
                    return false;
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

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                Reset();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdReset_Click");
            }
        }
        void Reset()
        {
            try
            {
                //cmbStore.SelectedIndex = 0;

                //cmbTestedBy.SelectedIndex = 0;
                txtTestedOn.Text = string.Empty;

                cmbTestedBy.SelectedIndex = 0;
                cmbTestLocation.SelectedIndex = 0;
                bool bChecked = false;
                foreach (GridViewRow row in grdDeliverDetails.Rows)
                {
                    // For Pass value will be 1 ;   For Fail value will be 0; For Scrap value will be 3; For Send to store (none) value will be 4
                    ((TextBox)row.FindControl("txtInspection")).Text = "";
                    ((TextBox)row.FindControl("txtRemarks")).Text = "";

                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Reset");
            }
        }

        public void GetStoreId()
        {
            try
            {
                clsOilTest objoiltest = new clsOilTest();
                objoiltest.getdivisionname(objoiltest);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetStoreId");
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
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdDeliverDetails_RowCommand");
            }
        }

        //protected void grdDeliverDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        protected void grdDeliverDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (cmbTestLocation.SelectedIndex >= 0)
                    {
                        //txtActiontype.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));

                        RadioButton rdbPass = (RadioButton)e.Row.FindControl("rdbPass");
                        RadioButton rdbFail = (RadioButton)e.Row.FindControl("rdbFail");
                        TextBox txtRemarks = (TextBox)e.Row.FindControl("txtRemarks");


                        if (txtActiontype.Text == "M")
                        {
                            if (hdfResult.Value == "0")
                            {

                                //rdbPass.Enabled = false;
                                rdbFail.Checked = true;


                            }
                            if (hdfResult.Value == "1")
                            {
                                //rdbFail.Enabled = false;
                                rdbPass.Checked = true;

                            }
                            txtRemarks.Text = hdfRemarks.Value;   
                        }
                        if (txtActiontype.Text == "A" || txtActiontype.Text == "R")
                        {
                            if (hdfResult.Value == "0")
                            {
                                rdbPass.Enabled = false;
                                rdbFail.Checked = true;
                            }
                            if (hdfResult.Value == "1")
                            {
                                rdbFail.Enabled = false;
                                rdbPass.Checked = true;
                            }
                            txtRemarks.Enabled = false;
                            txtRemarks.Text = hdfRemarks.Value;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdDeliverDetails_RowDataBound");
            }
        }


        public bool CheckDublicateImage()
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
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "CheckDublicateImage");
                return false;
            }



        }

        public void GetFailureDetailsFromXML(string sWFDataId)
        {
            try
            {



                clsOilTest objoiltest = new clsOilTest();
                //DataTable dt = new DataTable();
                objoiltest.sRoleId = objSession.RoleId;
                objoiltest.sOfficeCode = objSession.OfficeCode;
                objoiltest.sWOId = txtWOno.Text;

                objoiltest.sWFDataId = sWFDataId;
                objoiltest.GetFailureDetailsFromXML(objoiltest);
                DataTable dt = objoiltest.GetWOBasicDetails(objoiltest);

                txtPONo.Text = objoiltest.sPurchaseOrderNo;
                //txtIssueDate.Text = objoiltest.sIssueDate;
                txtDivision.Text = objoiltest.sCrby;
                txtPO_Remarks.Text = objoiltest.sAinRemarks;
                //txtRemarks.text = objoiltest.sAinRemarks;
                cmbTestLocation.Enabled = false;
                cmbTestLocation.SelectedValue = objoiltest.sAINTestLocation;

                hdfRemarks.Value = objoiltest.sAinRemarks;
                hdfResult.Value = objoiltest.sAinInspResult;
                grdDeliverDetails.DataSource = dt;
                grdDeliverDetails.DataBind();
                ViewState["TestDTR"] = dt;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "GetFailureDetailsFromXML");
            }

        }

        public void ApproveRejectAction()
        {
            try
            {
                clsApproval objApproval = new clsApproval();



                objApproval.sCrby = objSession.UserId;
                objApproval.sOfficeCode = objSession.OfficeCode;
                objApproval.sApproveComments = txtComment.Text.Trim();
                //objApproval.sWFObjectId = hdfWFOId.Value;
                objApproval.sWFObjectId = txtWOno.Text;
                objApproval.sWFAutoId = hdfWFOAutoId.Value;

                if (txtActiontype.Text == "A")
                {
                    objApproval.sApproveStatus = "1";
                }
                if (txtActiontype.Text == "R")
                {
                    objApproval.sApproveStatus = "3";
                }
                if (txtActiontype.Text == "M")
                {
                    objApproval.sApproveStatus = "2";
                }

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

                objApproval.sClientIp = sClientIP;

                bool bResult = false;
                if (txtActiontype.Text == "M")
                {
                    objApproval.sWFDataId = hdfWFDataId.Value;
                    if (hdfRejectApproveRef.Value == "RA")
                    {
                        objApproval.sApproveStatus = "1";
                    }
                    bResult = objApproval.ModifyApproveWFRequest(objApproval);
                }
                else
                {
                    bResult = objApproval.ApproveWFRequest(objApproval);
                }

                if (bResult == true)
                {

                    if (objApproval.sApproveStatus == "1")
                    {
                        //  ShowMsgBox("Approved Successfully");
                        txtApproveId.Text = objApproval.sNewRecordId;

                        cmdSave.Enabled = false;

                        if (objSession.RoleId == "5")
                        {
                            clsFailureEntry objFailure = new clsFailureEntry();
                            //objFailure.SendSMStoSectionOfficer(txtFailureOfficCode.Text, txtDTCCode.Text);
                            string status = EstimationReport(flag);
                            if (status != "SUCCESS")
                            {
                                ShowMsgBox("OOPS Estimation couldnt be approved please contact support team.");
                            }
                            else
                            {
                                ShowMsgBox("Approved Successfully");
                            }

                        }
                        if (objSession.RoleId == "4")
                        {
                            string sWoID = string.Empty;
                            string sOffCode = objSession.OfficeCode;
                            clsFailureEntry ObjFailure = new clsFailureEntry();
                            sWoID = ObjFailure.getWoIDforEstimation(sOffCode, txtPONo.Text);
                            //EstimationReportSO(sWoID);
                        }

                    }
                    else if (objApproval.sApproveStatus == "3")
                    {
                        ShowMsgBox("Rejected Successfully");
                        cmdSave.Enabled = false;
                    }
                    else if (objApproval.sApproveStatus == "2")
                    {
                        //  ShowMsgBox("Modified and Approved Successfully");
                        txtApproveId.Text = objApproval.sNewRecordId;
                        cmdSave.Enabled = false;

                        if (objSession.RoleId == "5")
                        {
                            clsFailureEntry objFailure = new clsFailureEntry();
                            string status = EstimationReport(flag);
                            if (status != "SUCCESS")
                            {
                                ShowMsgBox("OOPS Estimation couldnt be approved please contact support team.");
                            }
                            else
                            {
                                ShowMsgBox("Modified and Approved Successfully");
                            }
                        }
                    }
                }
                else
                {
                    ShowMsgBox("Selected Record Already Approved");
                    return;
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "cmdApprove_Click");
            }
        }

        public string EstimationReport(string flag)
        {
            try
            {
                if (txtApproveId.Text.Contains("-"))
                {
                    return "FAILURE";
                }
                //if (cmdSave.Text == "Save")
                //{
                clsEstimation objEst = new clsEstimation();
                //objEst.sOfficeCode = txtFailureOfficCode.Text;
                objEst.sFailureId = txtApproveId.Text;
                //objEst.sLastRepair = txtLastRepairer.Text;
                objEst.sCrby = objSession.UserId;

                string status = objEst.SaveEstimationDetails(objEst);
                if (status != "SUCCESS")
                {
                    ShowMsgBox("OOPS Estimation report cannot be generated please contact support team ");
                    return "FAILURE";
                }
                return "SUCCESS";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "EstimationReport");
                return "FAILURE";
            }
        }

    }

}
