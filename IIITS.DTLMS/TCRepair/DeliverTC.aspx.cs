using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Data.OleDb;
using System.Threading;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.Reflection;

namespace IIITS.DTLMS.TCRepair
{
    public partial class DeliverTC : System.Web.UI.Page
    {

        string strFormCode = "DeliverTC";
        clsSession objSession;
        string SDtrCode;

        string UserName = Convert.ToString(ConfigurationManager.AppSettings["FTP_USER"]);
        string Password = Convert.ToString(ConfigurationManager.AppSettings["FTP_PASS"]);
        string SFTPPath = Convert.ToString(ConfigurationManager.AppSettings["SFTPPath"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            string sFolderPath = Convert.ToString(ConfigurationManager.AppSettings["LOGFILEPATH"]) + DateTime.Now.ToString("yyyyMM");
            string sPath = sFolderPath + "//" + DateTime.Now.ToString("yyyyMMdd") + "-ErrorLog.txt";
            try
            {

                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                Form.DefaultButton = cmdSave.UniqueID;
                objSession = (clsSession)Session["clsSession"];
                lblMessage.Text = string.Empty;

                txtDeliverdate.Attributes.Add("readonly", "readonly");
                txtRVDate.Attributes.Add("readonly", "readonly");
                OMDateCalender.EndDate = DateTime.Now;
                txtOMDate.Attributes.Add("readonly", "readonly");
                if (!IsPostBack)
                {

                    if (Request.QueryString["RepairMasterId"] != null && Request.QueryString["RepairMasterId"].ToString() != "")
                    {
                        txtRepairMasterId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["RepairMasterId"]));
                        txtInsResultId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["InsResult"]));
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
                    // File.AppendAllText(sPath, "\n \n \n DTLMS \n DateTime    : " + System.DateTime.Now + " \n Date from storename : " + cmbStore.SelectedValue + " \n ############################################################## \n ");

                    Genaral.Load_Combo("SELECT US_ID,US_FULL_NAME FROM TBLUSER WHERE NVL(US_EFFECT_FROM,SYSDATE-1)<=SYSDATE AND US_STATUS='A' AND US_OFFICE_CODE LIKE '" + sOfficeCode + "%'", "--Select--", cmbVerifiedBy);
                    Genaral.Load_Combo("SELECT IT_ID,IT_CODE || '-' ||IT_NAME from TBLMMSITEMMASTER  ORDER BY IT_CODE", "--Select--", cmbItemCode);
                    LoadRecievePending();
                    //From DTR Tracker
                    if (Request.QueryString["TransId"] != null && Request.QueryString["TransId"].ToString() != "")
                    {
                        string sRepairDetailsId = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["TransId"]));
                        GetRepairRecieveDetails(sRepairDetailsId);
                        cmdSave.Enabled = false;
                    }
                    else
                    {
                        string date = GetLastPricingUpdate(objSession.OfficeCode);
                        if (date.Length != 0)
                        {
                            //DateTime DToDate = DateTime.ParseExact(date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            //string sFromDate = DToDate.ToString("yyyy/MM/dd");
                            //RVDateCalender.StartDate = Convert.ToDateTime(sFromDate);

                            //DateTime DToDate = Convert.ToDateTime(date, System.Globalization.CultureInfo.InvariantCulture).AddDays(1);
                            //  RVDateCalender.StartDate = Convert.ToDateTime(date, System.Globalization.CultureInfo.InvariantCulture).AddDays(1);

                            // File.AppendAllText(sPath, "\n \n \n DTLMS \n DateTime    : " + System.DateTime.Now + " \n Date from GetLastPricingUpdate : " + RVDateCalender.StartDate + "Date=" + date + " \n ############################################################## \n ");

                        }
                    }

                    LoadRecievedDTr();

                    txtDeliverdate.Attributes.Add("onblur", "return ValidateDate(" + txtDeliverdate.ClientID + ");");
                    //   DeliverCalender.StartDate = System.DateTime.Now;
                    // txtRIDate.Attributes.Add("onblur", "return ValidateDate(" + txtRIDate.ClientID + ");");
                    clsDTrRepairActivity objDeliver = new clsDTrRepairActivity();

                    //string res = objDeliver.getWarentyStatus(SDtrCode);

                    //if (res == "" || res == null)
                    //{
                    //    txtWarrentyPeriod.ReadOnly = true;
                    //    cmbGuarantyType.Enabled = true;
                    //}
                    //else
                    //{
                    //    txtWarrentyPeriod.ReadOnly = false;
                    //    txtWarrentyPeriod.Text = res;
                    //    cmbGuarantyType.Enabled = false;
                    //    cmbGuarantyType.SelectedValue = "WRGP";
                    //}

                    GetStoreId();
                    AutoGenerateRvNumber(sOfficeCode);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load");
                lblMessage.Text = clsException.ErrorMsg();
            }
        }

        private string GetLastPricingUpdate(string offCode)
        {
            clsCommon objCommon = new clsCommon();
            try
            {
                return objCommon.GetLastPricingDate(offCode);
            }
            catch (Exception ex)
            {

                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetLastPricingUpdate");
                return "Something Went Wrong";
            }
        }

        private void AutoGenerateRvNumber(string sOfficeLoc)
        {
            try
            {
                clsDTrRepairActivity objRepairer = new clsDTrRepairActivity();
                txtRVNo.Text = objRepairer.GenerateAutoRVNumber(sOfficeLoc);
                txtRVNo.ReadOnly = true;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "AutoGenerateRvNumber");
                lblMessage.Text = clsException.ErrorMsg();
            }
        }
        //method to load the updated tc with there item code to the grdUpdatedTcItemCode 
        protected void cmdLoad_Click(object sender, EventArgs e)
        {
            try
            {
                string strvalue = string.Empty;
                string selecteditem = string.Empty;
                string txtPeriod = string.Empty;

                string tempTC = string.Empty;

                bool flag = false;

                List<String> tempTcCodes = new List<string>();
                List<String> TcState = new List<string>();

                if (cmbItemCode.SelectedValue == "--Select--")
                {
                    ShowMsgBox("Please select Item code");
                    cmbItemCode.Focus();
                    if (ViewState["RecvPending"] == null)
                    {
                        ShowEmptyGrid();
                    }
                    return;
                }

                foreach (GridViewRow row in grdReceivePending.Rows)
                {
                    if (((CheckBox)row.FindControl("chkSelect")).Checked == true)
                    {
                        tempTC = ((Label)row.FindControl("lblTcCode")).Text;
                        tempTcCodes.Add(tempTC);
                        flag = true;

                        strvalue += ((Label)row.FindControl("lblInsResult")).Text + ' ';
                    }
                }
                if ((strvalue.Contains("FAULTY FOR RECLASSIFICATION ") && strvalue.Contains("PASS ")) ||
                    (strvalue.Contains("FAULTY FOR RECLASSIFICATION ") && strvalue.Contains("NOT REPAIRABLE ")) ||
                    (strvalue.Contains("FAULTY FOR RECLASSIFICATION ") && strvalue.Contains("PASS ") && strvalue.Contains("NOT REPAIRABLE ")) ||
                    (strvalue.Contains("FAULTY FOR RECLASSIFICATION ") && strvalue.Contains("ALL TEST OK EXPT CU & IRON LOSS ")) ||
                    (strvalue.Contains("FAULTY FOR RECLASSIFICATION ") && strvalue.Contains("ALL TEST OK EXPT CU LOSS ")) ||
                    (strvalue.Contains("FAULTY FOR RECLASSIFICATION ") && strvalue.Contains("ALL TEST OK EXPT IRON LOSS ")))
                {
                    divOmNo.Visible = false;
                    divOmDate.Visible = false;
                    ShowMsgBox("All DTr Code Should Be Faulty For Reclassification Or Other Than Faulty For Reclassification");
                    return;
                }
                else if (strvalue.Contains("PASS ") || strvalue.Contains("NOT REPAIRABLE ") || strvalue.Contains("ALL TEST OK EXPT CU LOSS ") || strvalue.Contains("ALL TEST OK EXPT IRON LOSS ") || strvalue.Contains("ALL TEST OK EXPT CU & IRON LOSS "))
                {
                    divOmNo.Visible = false;
                    divOmDate.Visible = false;
                }
                else if (strvalue.Contains("FAULTY FOR RECLASSIFICATION ") && !(strvalue.Contains("PASS ") && strvalue.Contains("NOT REPAIRABLE ")))
                {
                    divOmNo.Visible = true;
                    divOmDate.Visible = true;
                }

                if (!flag)
                {
                    ShowMsgBox("Please select DTr code.");
                    if (ViewState["RecvPending"] == null)
                    {
                        ShowEmptyGrid();
                    }
                    return;
                }

                LoadUpdatedItemCode(tempTcCodes);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdLoad_Click");
                lblMessage.Text = clsException.ErrorMsg();
            }
        }

        protected void EnableDisableSendButton()
        {
            try
            {
                if (grdReceivePending.Rows.Count == grdUpdatedTcItemCode.Rows.Count)
                {
                    cmdSave.Visible = true;
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "EnableDisableSendButton");
                lblMessage.Text = clsException.ErrorMsg();
            }
        }

        protected void DisableCheckBox(List<string> tempTcCodes)
        {
            try
            {
                if (ViewState["TcCodes"] != null)
                {
                    List<string> tmpstr = (List<string>)ViewState["TcCodes"];
                    tempTcCodes.AddRange(tmpstr);
                }
                foreach (GridViewRow rows in grdReceivePending.Rows)
                {
                    Label lblTCCode = (Label)rows.FindControl("lblTcCode");
                    foreach (var row in tempTcCodes)
                    {
                        if (Convert.ToString(row) == lblTCCode.Text)
                        {
                            ((CheckBox)rows.FindControl("chkSelect")).Enabled = false;
                            ((CheckBox)rows.FindControl("chkSelect")).Checked = false;
                            //((CheckBox)rows.FindControl("chkSelect")).Checked = true;
                        }
                    }
                }
                ViewState["TcCodes"] = tempTcCodes;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "DisableCheckBox");
                lblMessage.Text = clsException.ErrorMsg();
            }
        }
        // this is the method which will load the selected DTR codes into the grd 'grdUpdatedTcItemCode' 
        protected void LoadUpdatedItemCode(List<string> tempTcCodes)
        {
            try
            {
                string state = string.Empty;
                string sItemId = cmbItemCode.SelectedValue;
                string sItemCode = cmbItemCode.SelectedItem.Text.Split('-').GetValue(0).ToString();
                string sItemName = cmbItemCode.SelectedItem.Text.Split('-').GetValue(1).ToString();
                DataTable dtTcItCode = new DataTable();
                DataTable tempDt = new DataTable();
                DataTable dtgoodTc = (DataTable)ViewState["RecvPending"];
                if (ViewState["RecvPending"] != null)
                {
                    dtTcItCode = ((DataTable)ViewState["RecvPending"]).Clone();
                    dtTcItCode.Columns.Add("IT_CODE", typeof(string));
                    dtTcItCode.Columns.Add("IT_ID", typeof(string));
                    dtTcItCode.Columns.Add("IT_NAME", typeof(string));
                }
                foreach (string s in tempTcCodes)
                {
                    var filteredMRList = dtgoodTc.AsEnumerable().Where(r => r.Field<String>("TC_CODE") == s);
                    if (filteredMRList.Any())
                    {
                        //tempDt = filteredMRList.CopyToDataTable();
                        //dtTcItCode.ImportRow(tempDt.Rows[0]);
                        dtTcItCode.ImportRow(filteredMRList.CopyToDataTable().Rows[0]);
                    }
                    //getting null here
                }
                for (int i = 0; i < dtTcItCode.Rows.Count; i++)
                {
                    dtTcItCode.Rows[i]["IT_ID"] = sItemId;
                    dtTcItCode.Rows[i]["IT_CODE"] = sItemCode;
                    dtTcItCode.Rows[i]["IT_NAME"] = sItemName;
                }
                if (ViewState["dtUpdatedDtrs"] != null)
                {
                    tempDt = (DataTable)ViewState["dtUpdatedDtrs"];

                    for (int i = 0; i < tempDt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dtTcItCode.Rows.Count; j++)
                        {
                            if (tempDt.Rows[i]["STATE"].ToString() == "FAULTY FOR RECLASSIFICATION" && dtTcItCode.Rows[j]["STATE"].ToString() == "FAULTY FOR RECLASSIFICATION")
                            {
                                dtTcItCode.Merge(tempDt);
                                dtTcItCode = dtTcItCode.DefaultView.ToTable(true, "STATUS", "RSD_ID", "RSM_PO_NO", "RSM_ISSUE_DATE", "TC_CODE", "TC_SLNO", "IND_DOC", "CAPACITY", "IT_ID", "IT_CODE", "IT_NAME", "TC_WARANTY_PERIOD", "WARRENTY_TYPE", "STATE", "TC_MANF_DATE", "MAKE", "SUP_REPNAME", "TC_WARRENTY");
                                grdUpdatedTcItemCode.DataSource = dtTcItCode;
                                grdUpdatedTcItemCode.DataBind();
                                ViewState["dtUpdatedDtrs"] = dtTcItCode;
                            }
                            else if ((tempDt.Rows[i]["STATE"].ToString() == "FAULTY FOR RECLASSIFICATION" && dtTcItCode.Rows[j]["STATE"].ToString() == "NOT REPAIRABLE") ||
                                (tempDt.Rows[i]["STATE"].ToString() == "FAULTY FOR RECLASSIFICATION" && dtTcItCode.Rows[j]["STATE"].ToString() == "PASS") ||
                                (tempDt.Rows[i]["STATE"].ToString() == "FAULTY FOR RECLASSIFICATION" && dtTcItCode.Rows[j]["STATE"].ToString() == "ALL TEST OK EXPT CU LOSS")||
                                (tempDt.Rows[i]["STATE"].ToString() == "FAULTY FOR RECLASSIFICATION" && dtTcItCode.Rows[j]["STATE"].ToString() == "ALL TEST OK EXPT IRON LOSS") ||
                                (tempDt.Rows[i]["STATE"].ToString() == "FAULTY FOR RECLASSIFICATION" && dtTcItCode.Rows[j]["STATE"].ToString() == "ALL TEST OK EXPT CU & IRON LOSS"))
                            {
                                divOmNo.Visible = true;
                                divOmDate.Visible = true;
                                ShowMsgBox(" ALL DTR Code Should Be FAULTY FOR RECLASSIFICATION Or Other Than FAULTY FOR RECLASSIFICATION");
                                return;
                            }
                            else if ((tempDt.Rows[i]["STATE"].ToString() == "NOT REPAIRABLE" && dtTcItCode.Rows[j]["STATE"].ToString() == "FAULTY FOR RECLASSIFICATION") || 
                                (tempDt.Rows[i]["STATE"].ToString() == "PASS" && dtTcItCode.Rows[j]["STATE"].ToString() == "FAULTY FOR RECLASSIFICATION") ||
                                (tempDt.Rows[i]["STATE"].ToString() == "ALL TEST OK EXPT CU LOSS" && dtTcItCode.Rows[j]["STATE"].ToString() == "FAULTY FOR RECLASSIFICATION") ||
                                (tempDt.Rows[i]["STATE"].ToString() == "ALL TEST OK EXPT IRON LOSS" && dtTcItCode.Rows[j]["STATE"].ToString() == "FAULTY FOR RECLASSIFICATION") ||
                                (tempDt.Rows[i]["STATE"].ToString() == "ALL TEST OK EXPT CU & IRON LOSS" && dtTcItCode.Rows[j]["STATE"].ToString() == "FAULTY FOR RECLASSIFICATION"))
                            {
                                divOmNo.Visible = false;
                                divOmDate.Visible = false;
                                ShowMsgBox(" ALL DTR Code Should Be FAULTY FOR RECLASSIFICATION Or Other Than FAULTY FOR RECLASSIFICATION");
                                return;
                            }
                            else if ((tempDt.Rows[i]["STATE"].ToString() == "NOT REPAIRABLE" || dtTcItCode.Rows[j]["STATE"].ToString() == "NOT REPAIRABLE") ||
                                (tempDt.Rows[i]["STATE"].ToString() == "PASS" || dtTcItCode.Rows[j]["STATE"].ToString() == "PASS") ||
                                (tempDt.Rows[i]["STATE"].ToString() == "NOT REPAIRABLE" || dtTcItCode.Rows[j]["STATE"].ToString() == "PASS") ||
                                (tempDt.Rows[i]["STATE"].ToString() == "PASS" || dtTcItCode.Rows[j]["STATE"].ToString() == "NOT REPAIRABLE") ||
                                (tempDt.Rows[i]["STATE"].ToString() == "ALL TEST OK EXPT CU LOSS" || dtTcItCode.Rows[j]["STATE"].ToString() == "ALL TEST OK EXPT CU LOSS") ||
                                (tempDt.Rows[i]["STATE"].ToString() == "ALL TEST OK EXPT IRON LOSS" || dtTcItCode.Rows[j]["STATE"].ToString() == "ALL TEST OK EXPT IRON LOSS") ||
                                (tempDt.Rows[i]["STATE"].ToString() == "ALL TEST OK EXPT CU & IRON LOSS" || dtTcItCode.Rows[j]["STATE"].ToString() == "ALL TEST OK EXPT CU & IRON LOSS"))
                            {
                                dtTcItCode.Merge(tempDt);
                                dtTcItCode = dtTcItCode.DefaultView.ToTable(true, "STATUS", "RSD_ID", "RSM_PO_NO", "RSM_ISSUE_DATE", "TC_CODE", "TC_SLNO", "IND_DOC", "CAPACITY", "IT_ID", "IT_CODE", "IT_NAME", "TC_WARANTY_PERIOD", "WARRENTY_TYPE", "STATE", "TC_MANF_DATE", "MAKE", "SUP_REPNAME", "TC_WARRENTY");
                                grdUpdatedTcItemCode.DataSource = dtTcItCode;
                                grdUpdatedTcItemCode.DataBind();
                                ViewState["dtUpdatedDtrs"] = dtTcItCode;
                            }
                        }
                    }
                    //dtTcItCode.Merge(tempDt);
                }
                dtTcItCode = dtTcItCode.DefaultView.ToTable(true, "STATUS", "RSD_ID", "RSM_PO_NO", "RSM_ISSUE_DATE", "TC_CODE", "TC_SLNO", "IND_DOC", "CAPACITY", "IT_ID", "IT_CODE", "IT_NAME", "TC_WARANTY_PERIOD", "WARRENTY_TYPE", "STATE", "TC_MANF_DATE", "MAKE", "SUP_REPNAME", "TC_WARRENTY");

                if (ViewState["dtUpdatedDtrs"] == null)
                {

                    grdUpdatedTcItemCode.DataSource = dtTcItCode;
                    grdUpdatedTcItemCode.DataBind();

                    ViewState["dtUpdatedDtrs"] = dtTcItCode;



                }
                DisableCheckBox(tempTcCodes);


            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadUpdatedItemCode");
                lblMessage.Text = clsException.ErrorMsg();
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

                    clsDTrRepairActivity objDeliver = new clsDTrRepairActivity();
                    objDeliver.sDeliverDate = txtDeliverdate.Text;
                    objDeliver.sDeliverChallenNo = txtChallenNo.Text;
                    objDeliver.sVerifiedby = cmbVerifiedBy.SelectedValue;
                    objDeliver.sStoreId = cmbStore.SelectedValue;
                    objDeliver.sCrby = objSession.UserId;
                    objDeliver.sRVNo = txtRVNo.Text;
                    objDeliver.sRVDate = txtRVDate.Text;
                    objDeliver.sItemCode = cmbItemCode.SelectedValue;
                    objDeliver.sRepairMasterId = txtRepairMasterId.Text;
                    if (txtOMNo.Text != "" && txtOMNo.Text != null)
                    {
                        objDeliver.sOMNo = txtOMNo.Text;
                    }
                    if (txtOMDate.Text != "" && txtOMDate.Text != null)
                    {
                        objDeliver.sOMDate = txtOMDate.Text;
                    }




                    //objDeliver.sWarrantyPeriod = ConfigurationManager.AppSettings["WarrentyPeriod"].ToString();
                    //objDeliver.sGuarantyType = ConfigurationManager.AppSettings["WarrentyTypeWRGP"].ToString();
                    //objDeliver.sStatus = "1";



                    if (grdReceivePending.Rows.Count == 0)
                    {
                        ShowMsgBox("No Transformer Exists to Recieve");
                        return;
                    }
                    if (grdUpdatedTcItemCode.Rows.Count == 0)
                    {
                        ShowMsgBox("Please Select atleast ONE DTR to Receive");
                        return;
                    }

                    int i = 0;
                    bool bChecked = false;
                    string[] strQryVallist = new string[grdUpdatedTcItemCode.Rows.Count];
                    string sInsRes = string.Empty;
                    foreach (GridViewRow row in grdUpdatedTcItemCode.Rows)
                    {

                        //if (((CheckBox)row.FindControl("chkSelect")).Checked == true)
                        {
                            sInsRes = (((Label)row.FindControl("lblInsResult")).Text.ToUpper() == "FAULTY FOR RECLASSIFICATION") ? "4" :
                                (((Label)row.FindControl("lblInsResult")).Text.ToUpper() == "PASS") ? "1" :
                                (((Label)row.FindControl("lblInsResult")).Text.ToUpper() == "ALL TEST OK EXPT CU LOSS") ? "1" :
                                 (((Label)row.FindControl("lblInsResult")).Text.ToUpper() == "ALL TEST OK EXPT IRON LOSS") ? "1" :
                                 (((Label)row.FindControl("lblInsResult")).Text.ToUpper() == "ALL TEST OK EXPT CU & IRON LOSS") ? "1" :
                                "3";
                            strQryVallist[i] = ((Label)row.FindControl("lblRepairDetailsId")).Text.Trim() + "~" 
                                + ((Label)row.FindControl("lblTcCode")).Text.Trim() + "~" + sInsRes + "~" 
                                + ((TextBox)row.FindControl("txtTCWarrenty")).Text.Trim() + "~" 
                                + ((DropDownList)row.FindControl("cmbGuarantyType")).SelectedValue 
                                + "~" + ((Label)row.FindControl("lblItemId")).Text.Trim();
                            bChecked = true;
                        }
                        i++;

                    }

                    if (bChecked == false)
                    {
                        ShowMsgBox("Please Select any Transformers");
                        return;
                    }

                    //string[] strQrylist = new string[grdReceivePending.Rows.Count];
                    //foreach (GridViewRow row in grdReceivePending.Rows)
                    //{
                    //    strQrylist[i] = ((Label)row.FindControl("lbltcid")).Text.Trim() + "~" + ((Label)row.FindControl("lbltransid")).Text.Trim();
                    //    i++;
                    //}


                    Arr = objDeliver.SaveDeliverTCDetails(strQryVallist, objDeliver);
                    if (Arr[1].ToString() == "0")
                    {
                        //ShowMsgBox(Arr[0].ToString());
                        grdReceivePending.DataSource = null;
                        grdReceivePending.DataBind();
                        grdUpdatedTcItemCode.DataSource = null;
                        grdUpdatedTcItemCode.DataBind();

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), UniqueID, "alert('" + Arr[0].ToString() + "'); location.href='DeliverPendingSearch.aspx';", true);
                        Reset();
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "RepairTransaction");
                        }
                        return;
                    }
                    else
                    {
                        ShowMsgBox("No Transformer Exists to Recieve");
                    }
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdSave_Click");
                lblMessage.Text = clsException.ErrorMsg();
            }
        }
        private void LoadRecievePending()
        {
            string sFolderPath = Convert.ToString(ConfigurationManager.AppSettings["LOGFILEPATH"]) + DateTime.Now.ToString("yyyyMM");
            string sPath = sFolderPath + "//" + DateTime.Now.ToString("yyyyMMdd") + "-ErrorLog.txt";
            try
            {
                clsDTrRepairActivity objDeliverpending = new clsDTrRepairActivity();

                DataTable dt = new DataTable();

                dt = objDeliverpending.LoadPendingForRecieve(txtRepairMasterId.Text);
                if (dt.Rows.Count > 0)
                {
                    string startDate = Convert.ToString(dt.Rows[0]["RSM_ISSUE_DATE"]);
                    SDtrCode = dt.Rows[0]["TC_CODE"].ToString();
                    DateTime Issuedate = Convert.ToDateTime(startDate);
                    int Isuedate = Convert.ToInt32((DateTime.Now.AddDays(-1) - Issuedate).TotalDays);
                    DeliverCalender.StartDate = DateTime.Today.AddDays(-Isuedate);
                    DeliverCalender.EndDate = System.DateTime.Now.AddDays(0);
                    RVDateCalender.StartDate = DateTime.Today.AddDays(-Isuedate);
                    RVDateCalender.EndDate = System.DateTime.Now.AddDays(0);
                    grdReceivePending.DataSource = dt;
                    ViewState["RecvPending"] = dt;
                    grdReceivePending.DataBind();
                    int i = 0;
                    foreach (GridViewRow row in grdReceivePending.Rows)
                    {
                        TextBox lblstatus = (TextBox)row.FindControl("txtWarrenty");
                        if (lblstatus.Text == "1")
                        {
                            ((TextBox)row.FindControl("txtTCWarrenty")).Enabled = false;
                            ((DropDownList)row.FindControl("cmbGuarantyType")).SelectedItem.Text = dt.Rows[i]["WARRENTY_TYPE"].ToString();
                            objDeliverpending.sGuarantyType = ((DropDownList)row.FindControl("cmbGuarantyType")).SelectedValue;
                            ((DropDownList)row.FindControl("cmbGuarantyType")).Enabled = false;
                        }
                        else
                        {
                            ((TextBox)row.FindControl("txtTCWarrenty")).Enabled = true;
                            objDeliverpending.sWarrantyPeriod = lblstatus.Text;
                            ((DropDownList)row.FindControl("cmbGuarantyType")).Enabled = true;
                            objDeliverpending.sGuarantyType = ((DropDownList)row.FindControl("cmbGuarantyType")).SelectedValue;
                        }
                        i++;
                    }

                    foreach (GridViewRow row in grdReceivePending.Rows)
                    {
                        Label lblstate = (Label)row.FindControl("lblInsResult");
                        if (lblstate.Text == "FAULTY FOR RECLASSIFICATION")
                        {
                            ((TextBox)row.FindControl("txtTCWarrenty")).Enabled = false;
                            ((DropDownList)row.FindControl("cmbGuarantyType")).SelectedItem.Text = dt.Rows[0]["WARRENTY_TYPE"].ToString();
                            objDeliverpending.sGuarantyType = ((DropDownList)row.FindControl("cmbGuarantyType")).SelectedValue;
                            ((DropDownList)row.FindControl("cmbGuarantyType")).Enabled = false;
                        }
                    }

                }
                else
                {
                    ShowEmptyGrid();
                }

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadRecievePending");
                lblMessage.Text = clsException.ErrorMsg();
            }
        }

        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("RSD_ID");
                dt.Columns.Add("STATE");
                dt.Columns.Add("ITEM_TYPE");
                dt.Columns.Add("TC_CODE");
                dt.Columns.Add("TC_SLNO");
                dt.Columns.Add("MAKE");
                dt.Columns.Add("CAPACITY");
                dt.Columns.Add("TC_MANF_DATE");
                dt.Columns.Add("SUP_REPNAME");
                dt.Columns.Add("TC_WARANTY_PERIOD");
                dt.Columns.Add("STATUS");
                dt.Columns.Add("TC_WARRENTY");
                dt.Columns.Add("REMARKS");
                dt.Columns.Add("REMARKS_EE");
                dt.Columns.Add("IND_DOC");
                dt.Columns.Add("OM_No");
                dt.Columns.Add("OM_DATE");
                dt.Columns.Add("OM_Doc");

                grdReceivePending.DataSource = dt;
                grdReceivePending.DataBind();
                int iColCount = grdReceivePending.Rows[0].Cells.Count;
                grdReceivePending.Rows[0].Cells.Clear();
                grdReceivePending.Rows[0].Cells.Add(new TableCell());
                grdReceivePending.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdReceivePending.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowEmptyGrid");

            }
        }

        private void LoadRecievedDTr()
        {
            try
            {
                clsDTrRepairActivity objDeliverpending = new clsDTrRepairActivity();

                DataTable dt = new DataTable();

                dt = objDeliverpending.LoadRecievedTransformers(txtRepairMasterId.Text);
                grdRecievedDTr.DataSource = dt;
                grdRecievedDTr.DataBind();
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadRecievePending");
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
            string selecteditem = string.Empty;
            string txtPeriod = string.Empty;

            try
            {
                if (cmbStore.SelectedIndex == 0)
                {
                    ShowMsgBox("Please select the Store");
                    cmbStore.Focus();
                    return bValidate;
                }

                if (cmbItemCode.SelectedIndex == 0)
                {
                    ShowMsgBox("Please select Item code");
                    cmbItemCode.Focus();
                    return bValidate;
                }

                if (cmbVerifiedBy.SelectedIndex == 0)
                {
                    ShowMsgBox("Please select Verified by");
                    cmbVerifiedBy.Focus();
                    return bValidate;
                }

                if (txtChallenNo.Text.Length == 0)
                {
                    ShowMsgBox("Please enter Deliver challan No.");
                    txtChallenNo.Focus();
                    return bValidate;
                }

                foreach (GridViewRow row in grdUpdatedTcItemCode.Rows)
                {
                    {
                        selecteditem = ((DropDownList)row.FindControl("cmbGuarantyType")).SelectedValue;
                        txtPeriod = ((TextBox)row.FindControl("txtTCWarrenty")).Text;

                        if (selecteditem == "--Select--")
                        {
                            ShowMsgBox("Please select Guarentee type");
                            ((DropDownList)row.FindControl("cmbGuarantyType")).Focus();
                            return bValidate;
                        }
                        if (txtPeriod == "" || txtPeriod == null)
                        {
                            if (((TextBox)row.FindControl("txtTCWarrenty")).Enabled == true)
                            {
                                ShowMsgBox("Please enter warranty period");
                                ((TextBox)row.FindControl("txtTCWarrenty")).Focus();
                                return bValidate;
                            }
                        }
                    }
                }
                if (txtDeliverdate.Text.Length == 0)
                {
                    ShowMsgBox("Please select the Deliver date");
                    txtDeliverdate.Focus();
                    return bValidate;
                }
                string sResult = Genaral.DateComparision(txtDeliverdate.Text, "", true, false);
                if (sResult == "1")
                {
                    ShowMsgBox("Deliver date should be Less than Current date.");
                    return bValidate;
                }
                if (txtRVNo.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Please enter valid RV No");
                    txtRVNo.Focus();
                    return bValidate;
                }
                if (txtRVDate.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Please select RV date");
                    txtRVDate.Focus();
                    return bValidate;
                }
                sResult = Genaral.DateComparision(txtRVDate.Text, "", true, false);
                if (sResult == "1")
                {
                    ShowMsgBox("RV date should be less than Current date.");
                    return bValidate;
                }
                if (divOmNo.Visible == true)
                {
                    if (txtOMNo.Text.Trim().Length == 0)
                    {
                        ShowMsgBox("Please enter valid OM No");
                        txtOMNo.Focus();
                        return bValidate;
                    }
                }
                if (divOmDate.Visible == true)
                {
                    if (txtOMDate.Text.Trim().Length == 0)
                    {
                        ShowMsgBox("Please enter valid OM date");
                        txtOMDate.Focus();
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
                txtDeliverdate.Text = string.Empty;
                txtChallenNo.Text = string.Empty;
                cmbVerifiedBy.SelectedIndex = 0;
                //txtRVNo.Text = string.Empty; // commented on the Jira ID: DTLMS-1815 on 19-09-2023.
                cmbItemCode.SelectedIndex = 0;
                txtRVDate.Text = string.Empty;
                grdUpdatedTcItemCode.DataSource = null;
                grdUpdatedTcItemCode.DataBind();
                grdReceivePending.DataSource = null;
                grdUpdatedTcItemCode.DataBind();
                ViewState["dtUpdatedDtrs"] = null;
                // cmdSave.Visible = false;
                foreach (GridViewRow row in grdReceivePending.Rows)
                {
                    ((CheckBox)row.FindControl("chkSelect")).Enabled = true;
                    ((CheckBox)row.FindControl("chkSelect")).Checked = false;
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
            string sFolderPath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["LOGFILEPATH"]) + DateTime.Now.ToString("yyyyMM");
            string sPath = sFolderPath + "//" + DateTime.Now.ToString("yyyyMMdd") + "-ErrorLog.txt";
            try
            {
                clsTcMaster objTcMaster = new clsTcMaster();
                cmbStore.SelectedValue = objTcMaster.GetStoreId(objSession.OfficeCode);
                //File.AppendAllText(sPath, "\n \n \n DTLMS \n DateTime    : " + System.DateTime.Now + " \n Date from storename : " + cmbStore.SelectedValue + " \n ############################################################## \n ");

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetStoreId");
            }
        }
        #region Access Rights
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

        #endregion
        public void GetRepairRecieveDetails(string sRepairDetailsId)
        {
            try
            {
                clsDTrRepairActivity objRepair = new clsDTrRepairActivity();

                objRepair.sRepairDetailsId = sRepairDetailsId;
                objRepair.GetRepairRecieveDetails(objRepair);

                txtDeliverdate.Text = objRepair.sDeliverDate;
                txtChallenNo.Text = objRepair.sDeliverChallenNo;
                cmbVerifiedBy.SelectedValue = objRepair.sVerifiedby;

                txtRVNo.Text = objRepair.sRVNo;
                txtRVDate.Text = objRepair.sRVDate;

                txtOMNo.Text = objRepair.sOMNo;
                txtOMDate.Text = objRepair.sOMDate;

                txtRepairMasterId.Text = objRepair.GetRepairDetailsId(sRepairDetailsId);

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetRepairSentDetails");

            }
        }
        protected void grdReceivePending_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string sTcCode = ((Label)e.Row.FindControl("lblTcCode")).Text;

                    DataTable dt = (DataTable)ViewState["RecvPending"];
                    if (dt != null)
                    {
                        DataRow[] dtrow = dt.Select("TC_CODE like '%" + sTcCode + "%'");
                        LinkButton lnkDnld = ((LinkButton)e.Row.FindControl("lnkDwnld"));
                        LinkButton lnkNodnld = ((LinkButton)e.Row.FindControl("lnkNodownload"));

                        LinkButton lnkDownloadOM_Doc = ((LinkButton)e.Row.FindControl("lnkDownloadOM_Doc"));
                        LinkButton lnkNoDnlOM_Doc = ((LinkButton)e.Row.FindControl("lnkNoDnlOM_Doc"));

                        if (dtrow[0]["IND_DOC"].ToString() == null || dtrow[0]["IND_DOC"].ToString() == "")
                        {
                            lnkDnld.Visible = false;
                            lnkNodnld.Visible = true;
                            lnkNodnld.CssClass = "blockpointer";
                        }
                        else
                        {
                            lnkDnld.Enabled = true;
                            lnkNodnld.Visible = false;
                            lnkDnld.CssClass = "handPointer";
                        }

                        if ((dtrow[0]["OM_DOC"].ToString() ?? "").Length == 0)
                        {
                            lnkDownloadOM_Doc.Visible = false;
                            lnkNoDnlOM_Doc.Visible = true;
                            lnkNoDnlOM_Doc.CssClass = "blockpointer";
                        }
                        else
                        {
                            lnkDownloadOM_Doc.Enabled = true;
                            lnkNoDnlOM_Doc.Visible = false;
                            lnkDownloadOM_Doc.CssClass = "handPointer";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdReceivePending_RowDataBound");
            }
        }

        protected void grdReceivePending_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Download")
                {

                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    string sTcCode = ((Label)row.FindControl("lblTcCode")).Text;
                    download(sTcCode);

                }
                if (e.CommandName == "")
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdFaultTC_RowCommand");
            }
            //finally
            //{
            //    Response.End();
            //}

        }


        private void download(string sTcCode)
        {
            try
            {
                DataTable dt = (DataTable)ViewState["RecvPending"];
                DataRow[] dtrow = dt.Select("TC_CODE like '%" + sTcCode + "%'");
                string Doc = dtrow[0]["IND_DOC"].ToString();
                Byte[] bytes = (Byte[])dtrow[0]["IND_DOC"];


                Response.Buffer = true;
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "image/png";

                Response.AppendHeader("Content-Disposition", "attachment; filename=" + dtrow[0]["TC_CODE"].ToString() + ".png");

                Response.BinaryWrite(bytes);
                Response.Flush();
                HttpContext.Current.ApplicationInstance.CompleteRequest();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "download");
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
                    //string PoNo = Regex.Replace(txtPONo.Text, @"[^0-9a-zA-Z]+", "");
                    clsSFTP objFtp = new clsSFTP(SFTPPath, UserName, Password);

                    string path = SFTPmainfolderpath + fileName;
                    ClientScript.RegisterStartupScript(this.GetType(), "Print", "<script>window.open('" + path + "','_blank')</script>");
                }
                else
                {
                    ShowMsgBox("EE as Not Uploaded the Documents.");
                }

            }
            catch (WebException ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name,
                 MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

            }

        }


    }
}