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
using IIITS.DTLMS.BL.OilFlow;
using System.Collections;

namespace IIITS.DTLMS.OilFlow
{
    public partial class DeliverOil : System.Web.UI.Page
    {

        string strFormCode = "DeliverOil";
        clsSession objSession;
        string Qty;
        string Percentage = ConfigurationManager.AppSettings["Percentage"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                //dvReceive.Style.Add("display", "block");
                
                Form.DefaultButton = cmdSave.UniqueID;
                objSession = (clsSession)Session["clsSession"];
                lblMessage.Text = string.Empty;
                txtRVDate.Attributes.Add("readonly", "readonly");
                txtDeliverDate.Attributes.Add("readonly", "readonly");
                txtinspectedon.Enabled = false;
                CalendarExtender1.EndDate = System.DateTime.Now;
                CalendarExtender2.EndDate = System.DateTime.Now;
                OMDateCalender.EndDate = DateTime.Now;
                if (!IsPostBack)
                {

                    if (Request.QueryString["PoNo"] != null && Request.QueryString["PoNo"].ToString() != "")
                    {
                        if (Session["RepairDetailsId"] != null && Session["RepairDetailsId"].ToString() != "")
                        {
                            txtSelectedDetailsId.Text = Session["RepairDetailsId"].ToString();
                            // Session["RepairDetailsId"] = null;
                        }
                        txtPONo.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["PoNo"]));
                         Qty = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["Qty"]));
                        LoadTestingdoneoil();
                       
                        //LoadTestingPendingTC();
                    }
                    //DateTime testedon = DateTime.Now;
                    //testedon = Convert.ToDateTime(txtinspectedon.Text);
                    //CalendarExtender1.StartDate = testedon;

                    string testedon = txtinspectedon.Text;
                    DateTime DToDate = DateTime.ParseExact(testedon, "d-M-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string sFromDate = DToDate.ToString("yyyy/MM/dd");
                    CalendarExtender1.StartDate = Convert.ToDateTime(sFromDate);
                    CalendarExtender2.StartDate = Convert.ToDateTime(sFromDate);

                    string sOfficeCode = string.Empty;

                    if (objSession.OfficeCode.Length > 2)
                    {
                        sOfficeCode = objSession.OfficeCode.Substring(0, 2);
                    }
                    else
                    {
                        sOfficeCode = objSession.OfficeCode;
                    }

                   // Genaral.Load_Combo("SELECT US_ID,US_FULL_NAME FROM TBLUSER WHERE NVL(US_EFFECT_FROM,SYSDATE-1)<=SYSDATE AND US_STATUS='A' AND US_OFFICE_CODE LIKE '" + sOfficeCode + "%'", "--Select--", cmbVerifiedBy);
                    //Genaral.Load_Combo("SELECT MD_ID, MD_ID || '-' ||MD_NAME from TBLMASTERDATA  Where MD_TYPE='Oil' ORDER BY MD_ID ", "--Select--", cmbItemCode);
                    Genaral.Load_Combo("SELECT IT_ID, IT_CODE || '-' ||IT_NAME from TBLMMSITEMMASTER  Where IT_UOM='kLtr' and IT_CODE='601410' ", cmbItemCode);
                    LoadRecievedOil();
                    AutoGenerateRvNumber(sOfficeCode);

                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load");
                lblMessage.Text = clsException.ErrorMsg();
            }
        }

        private void LoadTestingdoneoil()
        {
            try
            {
                clsOilTest objoiltest = new clsOilTest();

                DataTable dt = new DataTable();
                objoiltest.sOfficeCode = objSession.OfficeCode;
                objoiltest.sPurchaseOrderNo = txtPONo.Text;
                objoiltest.sQty = Qty;
                objoiltest.sTestingDone = "1";
                //Genaral.Load_Combo("SELECT US_ID,US_FULL_NAME FROM TBLUSER WHERE NVL(US_EFFECT_FROM,SYSDATE-1)<=SYSDATE AND US_STATUS='A' AND US_OFFICE_CODE LIKE '" + objoiltest.sOfficeCode + "%'", "--Select--", cmbVerifiedBy);
                Genaral.Load_Combo("SELECT US_ID,US_FULL_NAME FROM TBLUSER WHERE NVL(US_EFFECT_FROM,SYSDATE-1)<=SYSDATE AND US_STATUS='A' AND US_OFFICE_CODE LIKE '%'", cmbVerifiedBy);


                dt = objoiltest.LoadTestingdoneoil(objoiltest);
                if (dt.Rows.Count > 0)
                {
                    txtPONo.Text = Convert.ToString(dt.Rows[0]["OSD_PO_NO"]);
                    //txtIssueDate.Text = Convert.ToString(dt.Rows[0]["ASD_PO_DATE"]);
                    txtDivision.Text = Convert.ToString(dt.Rows[0]["OSD_OFFICE_CODE"]);
                    hdfagency.Value = Convert.ToString(dt.Rows[0]["OSD_AGENCY"]);
                    cmbVerifiedBy.SelectedValue = Convert.ToString(dt.Rows[0]["AIN_INS_BY"]);
                    txtinspectedon.Text = Convert.ToString(dt.Rows[0]["AIN_INS_DATE"]);
                   hdfpercentagevalue.Value = Convert.ToString(dt.Rows[0]["OSD_PERCENTAGE_VALUE"]);
                   
                    cmbVerifiedBy.Enabled = false;
                    //hdfosdid.Value = Convert.ToString(dt.Rows[0]["OSD_ID"]);

                    Genaral.Load_Combo("SELECT US_ID,US_FULL_NAME FROM TBLUSER WHERE NVL(US_EFFECT_FROM,SYSDATE-1)<=SYSDATE AND US_STATUS='A' AND US_ID = '"+ dt.Rows[0]["AIN_INS_BY"] + "'", cmbVerifiedBy);
                }


                grdReceivePending.DataSource = dt;
                grdReceivePending.DataBind();
                ViewState["TestPass"] = dt;
            }
               

            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadTestingPendingTC");
                lblMessage.Text = clsException.ErrorMsg();
            }

        }

        private void LoadRecievedOil()
        {
            try
            {
                clsOilTest objoiltest = new clsOilTest();
                DataTable dt = new DataTable();
                objoiltest.sPurchaseOrderNo = txtPONo.Text;
                objoiltest.sOfficeCode = objSession.OfficeCode;
                dt = objoiltest.LoadRecievedOil(objoiltest,txtPONo.Text);
                grdRecievedDTr.DataSource = dt;
                grdRecievedDTr.DataBind();
                ViewState["Recievedoil"] = dt;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadRecievePending");
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


        //method to load the updated tc with there item code to the grdUpdatedTcItemCode 
        protected void cmdLoad_Click(object sender, EventArgs e)
        {
            try
            {
                //bool bValidate = false;
                string strvalue = string.Empty;
                string selecteditem = string.Empty;
                string txtPeriod = string.Empty;
                string tempquantity = string.Empty;
                string tempTC = string.Empty;

                bool flag = false;

                List<String> tempTcCodes = new List<string>();
                List<String> TcState = new List<string>();


                if (cmbItemCode.SelectedValue == "--Select--")
                {
                    ShowMsgBox("Please Select ITEM Code");
                    cmbItemCode.Focus();
                    return;
                }


                foreach (GridViewRow row in grdReceivePending.Rows)
                {
                    if (((CheckBox)row.FindControl("chkSelect")).Checked == true)
                    {
                        tempTC = ((Label)row.FindControl("lblPoNo")).Text;
                        tempTcCodes.Add(tempTC);
                        flag = true;
                        strvalue += ((Label)row.FindControl("lblStatus")).Text + ' ';
                        
                    }
                }







                if (!flag)
                {
                    ShowMsgBox("Please Select PO No");
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
                if (ViewState["TestPass"] != null)
                {
                    List<string> tmpstr = (List<string>)ViewState["TestPass"];
                    tempTcCodes.AddRange(tmpstr);
                }
                foreach (GridViewRow rows in grdReceivePending.Rows)
                {
                    Label lblPoNo = (Label)rows.FindControl("lblPoNo");
                    foreach (var row in tempTcCodes)
                    {
                        if (Convert.ToString(row) == lblPoNo.Text)
                        {
                            ((CheckBox)rows.FindControl("chkSelect")).Enabled = false;
                            ((CheckBox)rows.FindControl("chkSelect")).Checked = false;
                            //((CheckBox)rows.FindControl("chkSelect")).Checked = true;
                        }
                    }
                }
                ViewState["TestPass"] = tempTcCodes;
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
                string sTestLocation = cmbItemCode.SelectedItem.Text.Split('-').GetValue(0).ToString();
                //string sItemName = cmbTestLocation.SelectedItem.Text.Split('-').GetValue(1).ToString();
                DataTable dtTcItCode = new DataTable();
                DataTable tempDt = new DataTable();
                DataTable dtgoodTc = (DataTable)ViewState["TestPass"];
                if (ViewState["TestPass"] != null)
                {
                    dtTcItCode = ((DataTable)ViewState["TestPass"]).Clone();
                    dtTcItCode.Columns.Add("IT_CODE", typeof(string));
                    dtTcItCode.Columns.Add("IT_ID", typeof(string));
                    dtTcItCode.Columns.Add("IT_NAME", typeof(string));
                }
                foreach (string s in tempTcCodes)
                {
                    var filteredMRList = dtgoodTc.AsEnumerable().Where(r => r.Field<String>("OSD_PO_NO") == s);
                    if (filteredMRList.Any())
                    {
                        dtTcItCode.ImportRow(filteredMRList.CopyToDataTable().Rows[0]);
                    }
                    //getting null here
                }
                for (int i = 0; i < dtTcItCode.Rows.Count; i++)
                {
                    dtTcItCode.Rows[i]["IT_ID"] = sItemId;
                    dtTcItCode.Rows[i]["IT_CODE"] = sTestLocation;
                }
                if (ViewState["dtUpdatedDtrs"] != null)
                {
                    tempDt = (DataTable)ViewState["dtUpdatedDtrs"];

                    for (int i = 0; i < tempDt.Rows.Count; i++)
                    {

                        for (int j = 0; j < dtTcItCode.Rows.Count; j++)
                        {

                            //if (tempDt.Rows[i]["STATE"].ToString() == "NONE" && dtTcItCode.Rows[j]["STATE"].ToString() == "NONE")
                            //{


                            //    dtTcItCode.Merge(tempDt);
                            //   dtTcItCode = dtTcItCode.DefaultView.ToTable(true, "STATUS", "RSD_ID", "RSM_PO_NO", "RSM_ISSUE_DATE", "TC_CODE", "TC_SLNO", "IND_DOC", "CAPACITY", "IT_ID", "IT_CODE", "IT_NAME", "TC_WARANTY_PERIOD", "WARRENTY_TYPE", "STATE", "TC_MANF_DATE", "MAKE", "SUP_REPNAME", "TC_WARRENTY");
                            //    grdUpdatedTcItemCode.DataSource = dtTcItCode;
                            //    grdUpdatedTcItemCode.DataBind();

                            //    ViewState["dtUpdatedDtrs"] = dtTcItCode;

                            //}


                            //else if ((tempDt.Rows[i]["STATE"].ToString() == "SCRAP" || dtTcItCode.Rows[j]["STATE"].ToString() == "SCRAP") || (tempDt.Rows[i]["STATE"].ToString() == "PASS" || dtTcItCode.Rows[j]["STATE"].ToString() == "PASS") || (tempDt.Rows[i]["STATE"].ToString() == "SCRAP" || dtTcItCode.Rows[j]["STATE"].ToString() == "PASS") || (tempDt.Rows[i]["STATE"].ToString() == "PASS" || dtTcItCode.Rows[j]["STATE"].ToString() == "SCRAP"))
                            //{


                            //    dtTcItCode.Merge(tempDt);
                            //    dtTcItCode = dtTcItCode.DefaultView.ToTable(true, "STATUS", "RSD_ID", "RSM_PO_NO", "RSM_ISSUE_DATE", "TC_CODE", "TC_SLNO", "IND_DOC", "CAPACITY", "IT_ID", "IT_CODE", "IT_NAME", "TC_WARANTY_PERIOD", "WARRENTY_TYPE", "STATE", "TC_MANF_DATE", "MAKE", "SUP_REPNAME", "TC_WARRENTY");
                            //    grdUpdatedTcItemCode.DataSource = dtTcItCode;
                            //    grdUpdatedTcItemCode.DataBind();

                            //    ViewState["dtUpdatedDtrs"] = dtTcItCode;
                            //    // divOmNo.Visible = false;
                            //    // divOmDate.Visible = false;



                            //}



                        }

                    }


                    //dtTcItCode.Merge(tempDt);
                }
                dtTcItCode = dtTcItCode.DefaultView.ToTable(true,  "OSD_ID", "OSD_PO_NO", "OSD_INVOICE_NO", "OSD_PO_DATE", "OSD_QUANTITY", "OSD_OFFICE_CODE", "STATUS", "AIN_INSP_QTY", "OSD_STATUS", "IT_ID", "IT_CODE", "IT_NAME");

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

                    clsOilTest objoiltest = new clsOilTest();
                    //objoiltest.sPurchaseOrderNo = txtPONo.Text;
                    objoiltest.sDeliverChallenNo = txtChallenNo.Text;
                    objoiltest.sRVNo = txtRVNo.Text;
                    objoiltest.sVerifiedby = cmbVerifiedBy.SelectedValue;
                    //objoiltest.sItemCode = cmbItemCode.SelectedValue;
                    objoiltest.sCrby = objSession.UserId;
                    objoiltest.sPurchaseOrderNo = txtPONo.Text;
                    objoiltest.sItemCode = cmbItemCode.SelectedValue;
                    objoiltest.sRepairMasterId = txtRepairMasterId.Text;
                    objoiltest.sRVDate = txtRVDate.Text;
                    objoiltest.sOfficeCode = objSession.OfficeCode;
                    objoiltest.sAgencyValue = hdfagency.Value;
                    objoiltest.PercentageValue = hdfpercentagevalue.Value;
                    objoiltest.sDeliverDate = txtDeliverDate.Text;

                    objoiltest.sAgencyName = objoiltest.Getagencyname(objoiltest);

                    bool flag = false;
                    foreach (GridViewRow row in grdReceivePending.Rows)
                    {
                        if (((CheckBox)row.FindControl("chkSelect")).Checked == true)
                        {
                            string pono = ((Label)row.FindControl("lblPoNo")).Text;
                            flag = true;
                        }
                    }

                    if (!flag)
                    {
                        ShowMsgBox("Please Select PO No");
                        return;
                    }

                    if (grdReceivePending.Rows.Count == 0)
                    {
                        ShowMsgBox("No Oil Exists to Recieve");
                        return;
                    }
                    //if (grdUpdatedTcItemCode.Rows.Count == 0)
                    //{
                    //    ShowMsgBox("Please Select atleast One PO No to Receive");
                    //    return;
                    //}

                    int i = 0;
                    bool bChecked = false;
                    string[] strQryVallist = new string[grdReceivePending.Rows.Count];
                    string sInsRes = string.Empty;

                    //foreach (GridViewRow row in grdUpdatedTcItemCode.Rows)
                        foreach (GridViewRow row in grdReceivePending.Rows)
                        {


                        //string sRemarks = ((TextBox)row.FindControl("txtQuantity")).Text;
                        string sInvoicequantity= ((Label)row.FindControl("lblInvoiceQuantity")).Text.Trim();
                        string spodate = ((Label)row.FindControl("lblPodate")).Text.Trim();
                        string sInvoiceNo = ((Label)row.FindControl("lblInvoice")).Text.Trim();
                        string sInspqty = ((Label)row.FindControl("lblinspqty")).Text.Trim();
                        string sOsdid = ((Label)row.FindControl("lblOsdId")).Text.Trim();

                        objoiltest.sInspectedQty = objoiltest.LoadInspectoil(sOsdid);

                        if (objoiltest.sInspectedQty != "" && objoiltest.sInspectedQty != null)
                        {
                            //objoiltest.sPendingQty = (Convert.ToInt32(sInvoicequantity) - Convert.ToInt32(objoiltest.sInspectedQty)) * Convert.ToInt32(Percentage) / 100;
                           objoiltest.sPendingQty = (Convert.ToDouble(objoiltest.PercentageValue) - Convert.ToDouble(objoiltest.sInspectedQty));

                        }
                        else
                        {
                            objoiltest.sPendingQty = Convert.ToDouble(sInvoicequantity) * Convert.ToDouble(Percentage) / 100;
                            
                        }
                        //if (sRemarks != "")
                        //{
                        //    if (Convert.ToDouble(sRemarks) > (Convert.ToDouble(sInvoicequantity)))
                        //    {
                        //        ShowMsgBox("Received Quantity Should Be Less Than Or Equals to Invoice Quantity For - " + objoiltest.sPurchaseOrderNo);
                        //        return;
                        //    }
                        //}
                        //else
                        //{
                        //    ShowMsgBox(" Please Enter Received Quantity ");
                        //    return;
                        //}


                        //strQryVallist[i] = ((Label)row.FindControl("lblPONO")).Text.Trim() + "~" + sRemarks + "~" + sInvoicequantity + "~" + sInspqty;
                        strQryVallist[i] = ((Label)row.FindControl("lblPoNo")).Text.Trim() + "~" + sInvoicequantity + "~" + sInspqty + "~" + sOsdid; 
                         bChecked = true;
                        i++;
                        objoiltest.sPurchaseDate = spodate;
                        objoiltest.sInvoiceNo=sInvoiceNo;

                    }



                    //foreach (GridViewRow row in grdUpdatedTcItemCode.Rows)
                    //{

                    //    //if (((CheckBox)row.FindControl("chkSelect")).Checked == true)
                    //    {

                    //        sInsRes = (((Label)row.FindControl("lblStatus")).Text.ToUpper() == "NONE") ? "4" : (((Label)row.FindControl("lblStatus")).Text.ToUpper() == "PASS") ? "1" : "3";
                    //        strQryVallist[i] = ((Label)row.FindControl("lblRepairDetailsId")).Text.Trim() + "~" + ((Label)row.FindControl("lblPoNo")).Text.Trim() + "~" + sInsRes + "~" + ((TextBox)row.FindControl("txtQuantity")).Text.Trim() + "~" + ((DropDownList)row.FindControl("cmbGuarantyType")).SelectedValue + "~" + ((Label)row.FindControl("lblItemId")).Text.Trim();
                    //        bChecked = true;
                    //    }
                    //    i++;

                    //}

                    if (bChecked == false)
                    {
                        ShowMsgBox("Please Select any Po Oil");
                        return;
                    }


                    //string[] strQrylist = new string[grdReceivePending.Rows.Count];
                    //foreach (GridViewRow row in grdReceivePending.Rows)
                    //{
                    //    strQrylist[i] = ((Label)row.FindControl("lbltcid")).Text.Trim() + "~" + ((Label)row.FindControl("lbltransid")).Text.Trim();
                    //    i++;
                    //}


                    Arr = objoiltest.SaveOilReviceDetails(strQryVallist, objoiltest);
                    if (Arr[1].ToString() == "0")
                    {
                        //ShowMsgBox(Arr[0].ToString());
                        grdReceivePending.DataSource = null;
                        grdReceivePending.DataBind();
                        grdUpdatedTcItemCode.DataSource = null;
                        grdUpdatedTcItemCode.DataBind();
                        cmdSave.Enabled = false;

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), UniqueID, "alert('" + Arr[0].ToString() + "'); location.href='OilPendingSearch.aspx';", true);
                        Reset();
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "RepairTransaction");
                        }
                        return;
                    }
                    else
                    {
                        ShowMsgBox("No Oil Exists to Recieve");
                    }
                }


            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdSave_Click");
                lblMessage.Text = clsException.ErrorMsg();
            }
        }


        //private void LoadRecievePending()
        //{
        //    try
        //    {
        //        clsDTrRepairActivity objoiltestpending = new clsDTrRepairActivity();

        //        DataTable dt = new DataTable();

        //        dt = objoiltestpending.LoadPendingForRecieve(txtRepairMasterId.Text);
        //        if (dt.Rows.Count > 0)
        //        {
        //            string startDate = Convert.ToString(dt.Rows[0]["RSM_ISSUE_DATE"]);
        //            SDtrCode = dt.Rows[0]["TC_CODE"].ToString();
        //            DateTime Issuedate = Convert.ToDateTime(startDate);
        //            int Isuedate = Convert.ToInt32((DateTime.Now.AddDays(-1) - Issuedate).TotalDays);

        //            grdReceivePending.DataSource = dt;
        //            ViewState["RecvPending"] = dt;
        //            grdReceivePending.DataBind();
        //            int i = 0;
        //            foreach (GridViewRow row in grdReceivePending.Rows)
        //            {
        //                TextBox lblstatus = (TextBox)row.FindControl("lblOsdStatus");
        //                if (lblstatus.Text == "1")
        //                {
        //                    ((TextBox)row.FindControl("txtTCWarrenty")).Enabled = false;
        //                    ((DropDownList)row.FindControl("cmbGuarantyType")).SelectedItem.Text = dt.Rows[i]["WARRENTY_TYPE"].ToString();
        //                    objoiltestpending.sGuarantyType = ((DropDownList)row.FindControl("cmbGuarantyType")).SelectedValue;
        //                    ((DropDownList)row.FindControl("cmbGuarantyType")).Enabled = false;
        //                }
        //                else
        //                {
        //                    ((TextBox)row.FindControl("txtTCWarrenty")).Enabled = true;
        //                    objoiltestpending.sWarrantyPeriod = lblstatus.Text;
        //                    ((DropDownList)row.FindControl("cmbGuarantyType")).Enabled = true;
        //                    objoiltestpending.sGuarantyType = ((DropDownList)row.FindControl("cmbGuarantyType")).SelectedValue;
        //                }
        //                i++;
        //            }

        //            foreach (GridViewRow row in grdReceivePending.Rows)
        //            {
        //                Label lblstate = (Label)row.FindControl("lblStatus");
        //                if (lblstate.Text == "NONE")
        //                {
        //                    ((TextBox)row.FindControl("txtTCWarrenty")).Enabled = false;
        //                    ((DropDownList)row.FindControl("cmbGuarantyType")).SelectedItem.Text = dt.Rows[0]["WARRENTY_TYPE"].ToString();
        //                    objoiltestpending.sGuarantyType = ((DropDownList)row.FindControl("cmbGuarantyType")).SelectedValue;
        //                    ((DropDownList)row.FindControl("cmbGuarantyType")).Enabled = false;
        //                }
        //            }

        //        }
        //        else
        //        {
        //            ShowEmptyGrid();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadRecievePending");
        //        lblMessage.Text = clsException.ErrorMsg();
        //    }

        //}

        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("RSD_ID");
                dt.Columns.Add("STATE");
                dt.Columns.Add("TC_CODE");
                dt.Columns.Add("TC_SLNO");
                dt.Columns.Add("MAKE");
                dt.Columns.Add("CAPACITY");
                dt.Columns.Add("TC_MANF_DATE");
                dt.Columns.Add("SUP_REPNAME");
                dt.Columns.Add("TC_WARANTY_PERIOD");
                dt.Columns.Add("STATUS");
                dt.Columns.Add("TC_WARRENTY");

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

        //private void LoadRecievedDTr()
        //{
        //    try
        //    {
        //        clsDTrRepairActivity objoiltestpending = new clsDTrRepairActivity();

        //        DataTable dt = new DataTable();

        //        dt = objoiltestpending.LoadRecievedTransformers(txtRepairMasterId.Text);
        //        grdRecievedDTr.DataSource = dt;
        //        grdRecievedDTr.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadRecievePending");
        //        lblMessage.Text = clsException.ErrorMsg();
        //    }

        //}

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
                if (txtChallenNo.Text.Length == 0 )
                {
                    ShowMsgBox("Please Enter the Deliver Challen Number");
                    txtChallenNo.Focus();
                    return bValidate;
                }

                //if (cmbItemCode.SelectedIndex == 0)
                //{
                //    ShowMsgBox("Please Select Item Code");
                //    cmbItemCode.Focus();
                //    return bValidate;
                //}
                
                if (cmbVerifiedBy.Text=="")
                {
                    ShowMsgBox("Please Select Verified By");
                    cmbItemCode.Focus();
                    return bValidate;
                }


                if (txtRVNo.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Enter Valid RV No");
                    txtRVNo.Focus();
                    return bValidate;
                }
                if (txtRVDate.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Enter Valid RV Date");
                    txtRVDate.Focus();
                    return bValidate;
                }


                //foreach (GridViewRow row in grdUpdatedTcItemCode.Rows)
                //{
                //    //if (((CheckBox)row.FindControl("chkSelect")).Checked == true)
                //    {
                //        selecteditem = ((DropDownList)row.FindControl("cmbGuarantyType")).SelectedValue;
                //        txtPeriod = ((TextBox)row.FindControl("txtTCWarrenty")).Text;


                //        if (selecteditem == "--Select--")
                //        {
                //            ShowMsgBox("Please Select GUARENTEE TYPE");
                //            ((DropDownList)row.FindControl("cmbGuarantyType")).Focus();
                //            return bValidate;
                //        }
                //        if (txtPeriod == "" || txtPeriod == null)
                //        {
                //            if (((TextBox)row.FindControl("txtTCWarrenty")).Enabled == true)
                //            {
                //                ShowMsgBox("Please Enter warranty Period");
                //                ((TextBox)row.FindControl("txtTCWarrenty")).Focus();
                //                return bValidate;
                //            }

                //        }
                //    }
                //}




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
                //txtPONo.Text = string.Empty;
                //cmbVerifiedBy.SelectedIndex = 0;
                txtChallenNo.Text = string.Empty;
                txtRVDate.Text = string.Empty;



                grdUpdatedTcItemCode.DataSource = null;
                grdUpdatedTcItemCode.DataBind();
                grdReceivePending.DataSource = null;
                grdUpdatedTcItemCode.DataBind();
                ViewState["dtUpdatedDtrs"] = null;
                // cmdSave.Visible = false;
                foreach (GridViewRow row in grdReceivePending.Rows)
                {
                    ((CheckBox)row.FindControl("chkSelect")).Checked = false;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Reset");
            }
        }

        //public void GetStoreId()
        //{
        //    try
        //    {
        //        clsTcMaster objTcMaster = new clsTcMaster();
        //        //cmbStore.SelectedValue = objTcMaster.GetStoreId(objSession.OfficeCode);
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetStoreId");
        //    }
        //}


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


        //public void GetRepairRecieveDetails(string sRepairDetailsId)
        //{
        //    try
        //    {
        //        clsDTrRepairActivity objRepair = new clsDTrRepairActivity();

        //        objRepair.sRepairDetailsId = sRepairDetailsId;
        //        objRepair.GetRepairRecieveDetails(objRepair);

        //        txtPONo.Text = objRepair.sDeliverChallenNo;
        //        txtChallenNo.Text = objRepair.sDeliverChallenNo;
        //        txtRVNo.Text = objRepair.sRVNo;
        //        txtRVDate.Text = objRepair.sRVDate;
        //        //cmbVerifiedBy.SelectedValue = objRepair.sVerifiedby;


        //        txtRepairMasterId.Text = objRepair.GetRepairDetailsId(sRepairDetailsId);

        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetRepairSentDetails");

        //    }
        //}
        private void AutoGenerateRvNumber(string sOfficeLoc)
        {
            try
            {
                clsOilTest objoiltest = new clsOilTest();
                txtRVNo.Text = objoiltest.GenerateAutoRVNumber(sOfficeLoc);
                txtRVNo.ReadOnly = true;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "AutoGenerateRvNumber");
                lblMessage.Text = clsException.ErrorMsg();
            }
        }

        protected void grdReceivePending_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //string sTcStatus = ((Label)e.Row.FindControl("lblTCStatus")).Text;
                    string sTcCode = ((Label)e.Row.FindControl("lblPoNo")).Text;

                    DataTable dt = (DataTable)ViewState["RecvPending"];
                    if (dt != null)
                    {
                        DataRow[] dtrow = dt.Select("TC_CODE like '%" + sTcCode + "%'");
                        LinkButton lnkDnld = ((LinkButton)e.Row.FindControl("lnkDwnld"));
                        LinkButton lnkNodnld = ((LinkButton)e.Row.FindControl("lnkNodownload"));
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
                    string sTcCode = ((Label)row.FindControl("lblPoNo")).Text;
                    download(sTcCode);

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdFaultTC_RowCommand");
            }
            finally
            {
                Response.End();
            }

        }


        private void download(string sTcCode)
        {
            try
            {
                DataTable dt = (DataTable)ViewState["RecvPending"];
                DataRow[] dtrow = dt.Select("TC_CODE like '%" + sTcCode + "%'");
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

        protected void grdTestPending_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdRecievedDTr.PageIndex = e.NewPageIndex;
                PopulateCheckedValues();
                DataTable dt = (DataTable)ViewState["Recievedoil"];
                grdRecievedDTr.DataSource = SortDataTablePending(dt as DataTable, true);
                grdRecievedDTr.DataBind();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdTestPending_PageIndexChanging");
            }
        }

        private void PopulateCheckedValues()
        {
            try
            {
                ArrayList arrCheckedValues = (ArrayList)ViewState["CHECKED_ITEMS"];
                if (arrCheckedValues != null && arrCheckedValues.Count > 0)
                {
                    foreach (GridViewRow gvrow in grdRecievedDTr.Rows)
                    {
                        int index = Convert.ToInt32(grdRecievedDTr.DataKeys[gvrow.RowIndex].Values[0]);
                        if (arrCheckedValues.Contains(index))
                        {
                            CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkSelect");
                            myCheckBox.Checked = true;
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "PopulateCheckedValues");

            }
        }
        protected DataView SortDataTablePending(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                        ViewState["TestPending"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["TestPending"] = dataView.ToTable();

                    }


                }
                return dataView;
            }
            else
            {
                return new DataView();
            }

        }
        private string GridViewSortExpression
        {
            get { return ViewState["SortExpression"] as string ?? string.Empty; }
            set { ViewState["SortExpression"] = value; }
        }
        private string GridViewSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "ASC"; }
            set { ViewState["SortDirection"] = value; }


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

    }
}