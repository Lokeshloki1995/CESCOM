using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IIITS.DTLMS.BL;
using System.Globalization;
using System.Data.OleDb;
using System.Text.RegularExpressions;

namespace IIITS.DTLMS.DTCFailure
{
    public partial class FailureEntry : System.Web.UI.Page
    {

        string strFormCode = "FailureEntry";
        string flag;
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                Form.DefaultButton = cmdSave.UniqueID;
                objSession = (clsSession)Session["clsSession"];
                lblMessage.Text = string.Empty;
                if (!IsPostBack)
                {

                    int days = ((System.DateTime.Now.Day - 1) <= 1) ? (System.DateTime.Now.Day - 1) : 0;
                    CalendarExtender2.StartDate = System.DateTime.Now.AddDays(-days);
                    CalendarExtender2.EndDate = System.DateTime.Now;
                    txtFailedDate.Attributes.Add("readonly", "readonly");
                    txtDTrCommDate.Attributes.Add("readonly", "readonly");
                    cmbStarRated.Enabled = false;

                    Genaral.Load_Combo("SELECT MD_ID, MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'FT' ORDER BY MD_ID", "--Select--", cmbFailureType);
                    Genaral.Load_Combo("SELECT MD_ID, MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'C' ORDER BY MD_ID", "--Select--", cmbEnhanceCapacity);
                    Genaral.Load_Combo("SELECT MD_ID, MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'SRT'", "--Select--", cmbStarRated);
                    if (Request.QueryString["DTCId"] != null && Request.QueryString["DTCId"].ToString() != "")
                    {

                        txtDtcId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["DTCId"]));
                        txtFailurId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["FailureId"]));




                        if (!txtFailurId.Text.Contains("-"))
                        {
                            GetFailureDetails();
                            if (cmbGuarenteeType.SelectedIndex == 0)
                            {
                                cmbGuarenteeType.Enabled = true;
                            }
                            else
                                cmbGuarenteeType.Enabled = false;
                            //ValidateFormUpdate();
                        }

                        if (txtFailedDate.Text.Trim() != "")
                        {
                            if (Request.QueryString["ActionType"] != null && Request.QueryString["ActionType"].ToString() != "")
                            {
                                txtActiontype.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));
                                if (txtActiontype.Text == "V")
                                {
                                    if (txtCapacity.Text != cmbEnhanceCapacity.SelectedItem.Text)
                                    {
                                        rdbFail.Checked = false;
                                        rdbFail.Enabled = false;
                                        rdbFailEnhance.Checked = true;
                                        cmbEnhanceCapacity.Enabled = false;
                                        cmbEnhanceCapacity.Visible = true;
                                        lblEnCap.Visible = true;
                                    }
                                    else
                                    {
                                        rdbFail.Checked = true;
                                        rdbFailEnhance.Enabled = false;
                                        rdbFailEnhance.Checked = false;
                                        cmbEnhanceCapacity.Visible = false;
                                        lblEnCap.Visible = false;
                                    }
                                }
                            }
                            cmdSave.Text = "View";
                            //rdbFailEnhance.Enabled = false;

                        }
                    }

                    //Call Search Window
                    LoadSearchWindow();

                    //WorkFlow / Approval
                    WorkFlowConfig();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "Page_Load");
            }
        }



        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCapacity.Text == cmbEnhanceCapacity.SelectedItem.Text)
                {
                    flag = "1";
                }
                else
                {
                    flag = "4";
                }
                //Check AccessRights
                bool bAccResult = true;
                if (cmdSave.Text == "Update")
                {
                    bAccResult = CheckAccessRights("3");
                }
                else if (cmdSave.Text == "Save")
                {
                    bAccResult = CheckAccessRights("2");
                }

                if (bAccResult == false)
                {
                    return;
                }

                if (cmdSave.Text == "View")
                {
                    if (hdfApproveStatus.Value != "")
                    {
                        if (hdfApproveStatus.Value == "1" || hdfApproveStatus.Value == "2")
                        {
                            EstimationReport(flag);
                        }
                    }
                    else
                    {
                        EstimationReport(flag);
                    }
                    return;
                }

                if (ValidateForm() == true)
                {
                    clsFailureEntry objFailure = new clsFailureEntry();

                    string[] Arr = new string[2];
                    objFailure.sFailureId = txtFailurId.Text;

                    objFailure.sDtcId = txtDtcId.Text;
                    objFailure.sDtcTcCode = txtTcCode.Text;
                    objFailure.sDtcCode = txtDTCCode.Text.Replace("'", "");
                    DateTime Date = System.DateTime.Now;
                    string time = Date.ToString("HH:mm:ss");
                    objFailure.sFailureDate = txtFailedDate.Text.Replace("'", "");
                    string combined = objFailure.sFailureDate + " " + time;
                    objFailure.sFailureDate = combined;
                    objFailure.sFailureReasure = txtReason.Text.Trim().Replace("'", "").Replace("\"", "").Replace(";", "").Replace(",", "ç");
                    objFailure.sDtcReadings = txtDTCRead.Text.Trim().Replace("'", "");
                    objFailure.sCrby = objSession.UserId;
                    objFailure.sEnhancedCapacity = cmbEnhanceCapacity.SelectedItem.Text;
                    objFailure.sDTrCommissionDate = txtDTrCommDate.Text;
                    objFailure.sDTRStarRating = cmbStarRated.SelectedItem.Value;
                    string myString = txtDTrCommDate.Text;
                    DateTime myDateTime = DateTime.ParseExact(myString.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string myString_new = Convert.ToDateTime(myDateTime).ToString("yyyy-MM-dd");
                    objFailure.sDtrSaveCommissionDate = myString_new;
                    if (objSession.RoleId == "4")
                    {
                        if (txtDTrCommDate.Enabled == true)
                        {
                            objFailure.UpdateDtrCommDate(objFailure);
                        }

                        if (cmbGuarenteeType.Enabled == true)
                        {
                            objFailure.sGuarantySource = "From DropDown(User Selected)";
                        }
                        else
                        {
                            objFailure.sGuarantySource = "From Query(Automatic)";
                        }

                    }
                    else
                    {
                        if (hdfGuarenteeSource.Value == "" || hdfGuarenteeSource.Value == null)
                        {

                        }
                        else
                        {
                            objFailure.sGuarantySource = hdfGuarenteeSource.Value;
                        }
                    }

                    objFailure.sGuarantyType = cmbGuarenteeType.SelectedValue;
                    objFailure.sFailureType = cmbFailureType.SelectedValue.Trim();
                    objFailure.sOilQuantity = txtQuantityOfOil.Text;
                    objFailure.sTankcapacity = txtTankcapacity.Text;
                    objFailure.Latitude = txtLatitude.Text;
                    objFailure.Longitude = txtLongitude.Text;
                    objFailure.sCustomerName = txtCustomerName.Text.ToUpper();
                    objFailure.sCustomerNumber = txtCustMobileNo.Text;
                    objFailure.sNumberOfInstalments = txtNumberofInstalmets.Text;

                    if (cmbHtBusing.SelectedIndex > 0)
                    {
                        objFailure.sHTBusing = cmbHtBusing.SelectedValue.Trim();
                    }
                    if (cmbLtBusing.SelectedIndex > 0)
                    {
                        objFailure.sLTBusing = cmbLtBusing.SelectedValue.Trim();
                    }
                    if (cmbHtBusingRod.SelectedIndex > 0)
                    {
                        objFailure.sHTBusingRod = cmbHtBusingRod.SelectedValue.Trim();
                    }
                    if (cmbLtBusingRod.SelectedIndex > 0)
                    {
                        objFailure.sLTBusingRod = cmbLtBusingRod.SelectedValue.Trim();
                    }
                    if (cmbOilLevel.SelectedIndex > 0)
                    {
                        objFailure.sOilLevel = cmbOilLevel.SelectedValue.Trim();
                    }
                    if (cmbTankCondition.SelectedIndex > 0)
                    {
                        objFailure.sTankCondition = cmbTankCondition.SelectedValue.Trim();
                    }

                    if (cmbExplosion.SelectedIndex > 0)
                    {
                        objFailure.sExplosionValve = cmbExplosion.SelectedValue.Trim();
                    }
                    if (cmbDrainValve.SelectedIndex > 0)
                    {
                        objFailure.sDrainValve = cmbDrainValve.SelectedValue.Trim();
                    }
                    if (cmbBreather.SelectedIndex > 0)
                    {
                        objFailure.sBreather = cmbBreather.SelectedValue.Trim();
                    }

                    if (txtActiontype.Text == "A" || txtActiontype.Text == "R")
                    {
                        ApproveRejectAction();
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "Failure");
                        }
                        return;
                    }

                    if (txtActiontype.Text == "A" || txtActiontype.Text == "M")
                    {
                        if (cmbEnhanceCapacity.SelectedItem.Text == txtCapacity.Text)
                        {
                            objFailure.sFailtype = "1";
                        }
                        else
                        {
                            objFailure.sFailtype = "4";
                        }
                    }
                    else
                    {
                        if (cmbEnhanceCapacity.Enabled == false)
                        {
                            objFailure.sFailtype = "1";
                        }
                        else
                        {
                            objFailure.sFailtype = "4";
                        }
                    }


                    //Workflow
                    WorkFlowObjects(objFailure);

                    Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "Failure");

                    #region Modify and Approve

                    // For Modify and Approve
                    if (txtActiontype.Text == "M")
                    {
                        if (txtComment.Text.Trim() == "")
                        {
                            ShowMsgBox("Enter Comments/Remarks");
                            txtComment.Focus();
                            return;

                        }
                        objFailure.sFailureId = "";
                        objFailure.sActionType = txtActiontype.Text;
                        objFailure.sOfficeCode = txtFailureOfficCode.Text;
                        objFailure.sCrby = hdfCrBy.Value;
                        Arr = objFailure.SaveFailureDetails(objFailure);
                        if (Arr[1].ToString() == "0")
                        {
                            hdfWFDataId.Value = objFailure.sWFDataId;
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

                    #endregion

                    Arr = objFailure.SaveFailureDetails(objFailure);
                    txtFailureOfficCode.Text = objSession.OfficeCode;
                    string sOffCode = txtFailureOfficCode.Text;
                    string sDtcCode = txtDTCCode.Text;
                    string sWoID = objFailure.getWoIDforEstimation(sOffCode, sDtcCode);

                    if (Arr[1].ToString() == "0")
                    {
                        txtFailurId.Text = objFailure.sFailureId;
                        cmdSave.Text = "Update";
                        ShowMsgBox(Arr[0].ToString());
                        txtDTCCode.Enabled = false;
                        //txtFailureOfficCode.Text = objSession.OfficeCode;
                        //Temporary
                        EstimationReportSO(sWoID);
                        cmdSave.Enabled = false;
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "Failure");
                        }
                        return;
                    }

                    if (Arr[1].ToString() == "1")
                    {

                        string status = EstimationReport(flag);
                        if (status == "SUCCESS")
                        {
                            ShowMsgBox(Arr[0]);
                        }
                        else
                        {
                            ShowMsgBox("Failure  has been declared successfully but estimation has been not updated Please contact suppport team ");
                        }
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "Failure");
                        }
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
            }
            catch (Exception ex)
            {
                //lblMessage.Text = clsException.ErrorMsg();
                ShowMsgBox("Something went wrong while saving, Please Approve Once Again.");
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "cmdSave_Click");
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
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "ShowMsgBox");
            }
        }
        /// <summary>
        /// This function used to get the failure details
        /// </summary>
        public void GetFailureDetails()
        {
            try
            {
                clsFailureEntry objFailure = new clsFailureEntry();

                objFailure.sFailureId = txtFailurId.Text;
                objFailure.sDtcId = txtDtcId.Text;

                objFailure.GetFailureDetails(objFailure);

                if (objFailure.sDTrCommissionDate == objFailure.sDTrEnumerationDate)
                {
                    txtDTrCommDate.Enabled = true;
                    txtDTrCommDate.ReadOnly = false;
                }
                else if (objFailure.sDTrCommissionDate != "")
                {
                    txtDTrCommDate.Enabled = false;
                    txtDTrCommDate.ReadOnly = true;
                }
                else
                {
                    txtDTrCommDate.Enabled = true;
                    txtDTrCommDate.ReadOnly = false;
                }

                #region
                if (objFailure.sGuarantyType == "" || objFailure.sGuarantyType != "")
                {
                    try
                    {

                        // BRAND NEW DTR TAKING FROM  TBLTCDRAWN AND  TBLINVOICE
                        if (objFailure.sDTrCommissionDate != "" && objFailure.sDTrCommissionDate != null && objFailure.sLastRepairedDate == null || objFailure.sLastRepairedDate == "")
                        {
                            DateTime WarantyCommDate = DateTime.ParseExact(objFailure.sDTrCommissionDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            WarantyCommDate = WarantyCommDate.AddMonths(12);
                            if ((WarantyCommDate >= Convert.ToDateTime(DateTime.Now.Date.ToShortDateString().Replace('-', '/'))))
                            {
                                cmbGuarenteeType.SelectedValue = "WGP";
                            }
                            else
                            {
                                cmbGuarenteeType.SelectedValue = "AGP";
                            }

                        }


                        // REPAIRER DTR
                        else if (objFailure.sTcInvoiceDate != null && objFailure.sTcInvoiceDate != "" && objFailure.sLastRepairedDate != null && objFailure.sLastRepairedDate != "")
                        {
                            DateTime lastRepDate = DateTime.ParseExact(objFailure.sLastRepairedDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            DateTime InvoiceDate = DateTime.ParseExact(objFailure.sTcInvoiceDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            double months = InvoiceDate.Subtract(lastRepDate).Days / (365.25 / 12);

                            if (months > 6)
                            {
                                lastRepDate = lastRepDate.AddMonths(18);
                                if ((lastRepDate >= Convert.ToDateTime(DateTime.Now.Date.ToShortDateString().Replace('-', '/'))))
                                {
                                    cmbGuarenteeType.SelectedValue = "WRGP";
                                }
                                else
                                {
                                    cmbGuarenteeType.SelectedValue = "AGP";
                                }
                            }
                            else
                            {
                                InvoiceDate = InvoiceDate.AddMonths(12);
                                if ((InvoiceDate >= Convert.ToDateTime(DateTime.Now.Date.ToShortDateString().Replace('-', '/'))))
                                {
                                    cmbGuarenteeType.SelectedValue = "WRGP";
                                }
                                else
                                {
                                    cmbGuarenteeType.SelectedValue = "AGP";
                                }
                            }
                        }
                        else
                        {
                            DateTime lastRepDate = DateTime.ParseExact(objFailure.sLastRepairedDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            DateTime InvoiceDate = DateTime.ParseExact(objFailure.sTcInvoiceDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            //int months = (InvoiceDate.Month - lastRepDate.Month);
                            double months = InvoiceDate.Subtract(lastRepDate).Days / (365.25 / 12);

                            if (months > 6)
                            {
                                lastRepDate = lastRepDate.AddMonths(18);
                                if ((lastRepDate > Convert.ToDateTime(DateTime.Now.Date.ToShortDateString().Replace('-', '/'))))
                                {
                                    cmbGuarenteeType.SelectedValue = "WRGP";
                                }
                                else
                                {
                                    cmbGuarenteeType.SelectedValue = "AGP";
                                }
                            }
                            else
                            {
                                InvoiceDate = InvoiceDate.AddMonths(12);
                                if ((InvoiceDate > Convert.ToDateTime(DateTime.Now.Date.ToShortDateString().Replace('-', '/'))))
                                {
                                    cmbGuarenteeType.SelectedValue = "WRGP";
                                }
                                else
                                {
                                    cmbGuarenteeType.SelectedValue = "AGP";
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = clsException.ErrorMsg();
                        clsException.LogError(ex.ToString(), ex.Message, strFormCode, "GetFailureDetails");
                    }
                }
                #endregion 


                txtDtcId.Text = objFailure.sDtcId;
                txtDTCCode.Text = objFailure.sDtcCode;
                txtDTCName.Text = objFailure.sDtcName;
                txtLoadKW.Text = objFailure.sDtcLoadKw;
                txtLoadHP.Text = objFailure.sDtcLoadHp;
                txtDTCCommissionDate.Text = objFailure.sCommissionDate;
                txtCapacity.Text = objFailure.sDtcCapacity;
                txtLocation.Text = objFailure.sDtcLocation;
                txtTcCode.Text = objFailure.sDtcTcCode;
                txtTCSlno.Text = objFailure.sDtcTcSlno;
                txtTCId.Text = objFailure.sTCId;
                if (objFailure.Latitude.Length > 3 && objFailure.Longitude.Length > 3)
                {
                    txtLatitude.Text = objFailure.Latitude;
                    txtLongitude.Text = objFailure.Longitude;
                    txtLatitude.Enabled = false;
                    txtLongitude.Enabled = false;
                }


                if (objFailure.sTankcapacity.Length > 0)
                {
                    txtTankcapacity.Text = objFailure.sTankcapacity;
                    txtTankcapacity.Enabled = false;
                }
                if (objFailure.sOilCapacity.Length > 0)
                {
                    txtQuantityOfOil.Text = objFailure.sOilCapacity;
                    txtQuantityOfOil.Enabled = false;
                }

                cmbEnhanceCapacity.SelectedItem.Text = objFailure.sEnhancedCapacity;

                if (objFailure.sDTRStarRating == "" || objFailure.sDTRStarRating == null)
                {
                    cmbStarRated.SelectedItem.Text = "--Select--";
                }
                else
                {
                    cmbStarRated.SelectedValue = objFailure.sDTRStarRating;

                }

                if (rdbFail.Checked)
                {
                    if (Request.QueryString["ActionType"] != null && Request.QueryString["ActionType"].ToString() != "")
                    {
                        txtActiontype.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));
                        if (txtActiontype.Text == "V")
                        {
                            if (txtCapacity.Text != cmbEnhanceCapacity.SelectedItem.Text)
                            {
                                rdbFailEnhance.Checked = true;
                                rdbFail.Checked = false;
                            }
                            if (cmbEnhanceCapacity.SelectedItem.Text == "--Select--" || cmbEnhanceCapacity.SelectedItem.Text == "")
                            {
                                rdbFailEnhance.Checked = false;
                                rdbFail.Checked = true;
                                cmbEnhanceCapacity.SelectedItem.Text = txtCapacity.Text.Trim();
                                lblEnCap.Visible = false;
                                cmbEnhanceCapacity.Visible = false;
                            }
                        }
                    }
                    else
                    {
                        if (txtFailurId.Text == "0")
                        {

                        }
                        else
                        {
                            if (cmbEnhanceCapacity.SelectedItem.Text == txtCapacity.Text)
                            {
                                cmbEnhanceCapacity.SelectedItem.Text = txtCapacity.Text.Trim();
                            }
                            else
                            {
                                if (cmbEnhanceCapacity.SelectedItem.Text == "--Select--" || cmbEnhanceCapacity.SelectedItem.Text == "")
                                {
                                    cmbEnhanceCapacity.SelectedItem.Text = txtCapacity.Text.Trim();
                                    rdbFailEnhance.Checked = false;
                                    rdbFail.Checked = true;
                                    rdbFailEnhance.Enabled = false;
                                    cmbEnhanceCapacity.Visible = false;
                                    lblEnCap.Visible = false;
                                }
                                else
                                {
                                    cmbEnhanceCapacity.SelectedItem.Text = objFailure.sEnhancedCapacity;
                                    rdbFailEnhance.Checked = true;
                                    rdbFail.Checked = false;
                                    cmbEnhanceCapacity.Visible = true;
                                    cmbEnhanceCapacity.Enabled = false;
                                    rdbFail.Enabled = false;
                                    lblEnCap.Visible = true;
                                }
                            }
                        }
                    }
                }
                if (rdbFailEnhance.Checked)
                {
                    cmbEnhanceCapacity.SelectedItem.Text = objFailure.sEnhancedCapacity;
                }

                txtLastRepairDate.Text = objFailure.sLastRepairedDate;
                txtLastRepairer.Text = objFailure.sLastRepairedBy;
                // cmbGuarenteeType.SelectedValue = objFailure.sGuarantyType;

                txtManfDate.Text = objFailure.sManfDate;
                txtDTrCommDate.Text = objFailure.sDTrCommissionDate;

                txtTCMake.Text = objFailure.sDtcTcMake;
                txtFailedDate.Text = objFailure.sFailureDate;
                txtReason.Text = objFailure.sFailureReasure;
                txtDTCRead.Text = objFailure.sDtcReadings;
                txtDTCCode.Enabled = false;
                txtFailureOfficCode.Text = objFailure.sOfficeCode;


                cmbFailureType.SelectedValue = objFailure.sFailureType;
                if (objFailure.sHTBusing != "")
                {
                    cmbHtBusing.SelectedValue = objFailure.sHTBusing;
                }
                if (objFailure.sLTBusing != "")
                {
                    cmbLtBusing.SelectedValue = objFailure.sLTBusing;
                }
                if (objFailure.sHTBusingRod != "")
                {
                    cmbHtBusingRod.SelectedValue = objFailure.sHTBusingRod;
                }
                if (objFailure.sLTBusingRod != "")
                {
                    cmbLtBusingRod.SelectedValue = objFailure.sLTBusingRod;
                }
                if (objFailure.sDrainValve != "")
                {
                    cmbDrainValve.SelectedValue = objFailure.sDrainValve;
                }
                if (objFailure.sOilLevel != "")
                {
                    cmbOilLevel.SelectedValue = objFailure.sOilLevel;
                }
                //if (objFailure.sOilQuantity != "")
                //{
                //    txtQuantityOfOil.Text = objFailure.sOilQuantity;

                //}
                if (objFailure.sTankCondition != "")
                {
                    cmbTankCondition.SelectedValue = objFailure.sTankCondition;
                }

                if (objFailure.sExplosionValve != "")
                {
                    cmbExplosion.SelectedValue = objFailure.sExplosionValve;
                }
                if (objFailure.sBreather != "")
                {
                    cmbBreather.SelectedValue = objFailure.sBreather;
                }

                if (objFailure.sFailureId != "0")
                {

                    //cmdSave.Text = "Update";
                    cmdSearch.Visible = false;
                    cmdReset.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "GetFailureDetails");
            }

        }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {

            try
            {
                clsFailureEntry objFailure = new clsFailureEntry();

                objFailure.sDtcCode = txtDTCCode.Text;

                objFailure.SearchFailureDetails(objFailure);

                if (objFailure.sDtcName == null)
                {

                }
                else
                {
                    txtDTCName.Text = objFailure.sDtcName;

                    if (objFailure.sDTrCommissionDate == objFailure.sDTrEnumerationDate)
                    {
                        txtDTrCommDate.Enabled = true;
                        txtDTrCommDate.ReadOnly = false;
                    }
                    else
                    {
                        txtDTrCommDate.Enabled = false;
                        txtDTrCommDate.ReadOnly = true;
                    }

                    txtDtcId.Text = objFailure.sDtcId;
                    txtDTCCode.Text = objFailure.sDtcCode;
                    txtDTCName.Text = objFailure.sDtcName;
                    txtLoadKW.Text = objFailure.sDtcLoadKw;
                    txtLoadHP.Text = objFailure.sDtcLoadHp;
                    txtDTCCommissionDate.Text = objFailure.sCommissionDate;
                    txtCapacity.Text = objFailure.sDtcCapacity;
                    txtLocation.Text = objFailure.sDtcLocation;
                    txtTcCode.Text = objFailure.sDtcTcCode;
                    txtTCSlno.Text = objFailure.sDtcTcSlno;
                    txtTCId.Text = objFailure.sTCId;
                    txtDTrCommDate.Text = objFailure.sDTrCommissionDate;
                    if (objFailure.sGuarantyType == "")
                    {
                        objFailure.sGuarantyType = "0";
                    }
                    cmbGuarenteeType.SelectedValue = objFailure.sGuarantyType;

                    //if (cmbGuarenteeType.SelectedValue == "0")
                    //{
                    //    if (cmbGuarenteeType.SelectedIndex == 0)
                    //    {
                    //        cmbGuarenteeType.Enabled = true;
                    //    }
                    //    else
                    //        cmbGuarenteeType.Enabled = false;
                    //}
                    if (objFailure.sDTRStarRating == "" || objFailure.sDTRStarRating == null)
                    {
                        cmbStarRated.SelectedItem.Text = "--Select--";
                    }
                    else
                    {
                        cmbStarRated.SelectedValue = objFailure.sDTRStarRating;

                    }

                    cmbEnhanceCapacity.SelectedItem.Text = objFailure.sEnhancedCapacity;
                    if (objFailure.Latitude.Length > 3 && objFailure.Longitude.Length > 3)
                    {
                        txtLatitude.Text = objFailure.Latitude;
                        txtLongitude.Text = objFailure.Longitude;
                        txtLatitude.Enabled = false;
                        txtLongitude.Enabled = false;
                    }


                    if (objFailure.sTankcapacity.Length > 0)
                    {
                        txtTankcapacity.Text = objFailure.sTankcapacity;
                        txtTankcapacity.Enabled = false;
                    }
                    if (objFailure.sOilCapacity.Length > 0)
                    {
                        txtQuantityOfOil.Text = objFailure.sOilCapacity;
                        txtQuantityOfOil.Enabled = false;
                    }
                    txtManfDate.Text = objFailure.sManfDate;
                    txtTCMake.Text = objFailure.sDtcTcMake;


                    if (objFailure.sGuarantyType == "" || objFailure.sGuarantyType != "")
                    {
                        try
                        {

                            // BRAND NEW DTR TAKING FROM  TBLTCDRAWN AND  TBLINVOICE
                            if (objFailure.sDTrCommissionDate != "" && objFailure.sDTrCommissionDate != null && objFailure.sLastRepairedDate == null || objFailure.sLastRepairedDate == "")
                            {
                                DateTime WarantyCommDate = DateTime.ParseExact(objFailure.sDTrCommissionDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                WarantyCommDate = WarantyCommDate.AddMonths(12);
                                if ((WarantyCommDate >= Convert.ToDateTime(DateTime.Now.Date.ToShortDateString().Replace('-', '/'))))
                                {
                                    cmbGuarenteeType.SelectedValue = "WGP";
                                }
                                else
                                {
                                    cmbGuarenteeType.SelectedValue = "AGP";
                                }

                            }


                            // REPAIRER DTR
                            else if (objFailure.sTcInvoiceDate != null && objFailure.sTcInvoiceDate != "" && objFailure.sLastRepairedDate != null && objFailure.sLastRepairedDate != "")
                            {
                                DateTime lastRepDate = DateTime.ParseExact(objFailure.sLastRepairedDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                DateTime InvoiceDate = DateTime.ParseExact(objFailure.sTcInvoiceDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                double months = InvoiceDate.Subtract(lastRepDate).Days / (365.25 / 12);

                                if (months > 6)
                                {
                                    lastRepDate = lastRepDate.AddMonths(18);
                                    if ((lastRepDate >= Convert.ToDateTime(DateTime.Now.Date.ToShortDateString().Replace('-', '/'))))
                                    {
                                        cmbGuarenteeType.SelectedValue = "WRGP";
                                    }
                                    else
                                    {
                                        cmbGuarenteeType.SelectedValue = "AGP";
                                    }
                                }
                                else
                                {
                                    InvoiceDate = InvoiceDate.AddMonths(12);
                                    if ((InvoiceDate >= Convert.ToDateTime(DateTime.Now.Date.ToShortDateString().Replace('-', '/'))))
                                    {
                                        cmbGuarenteeType.SelectedValue = "WRGP";
                                    }
                                    else
                                    {
                                        cmbGuarenteeType.SelectedValue = "AGP";
                                    }
                                }
                            }
                            else
                            {
                                DateTime lastRepDate = DateTime.ParseExact(objFailure.sLastRepairedDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                DateTime InvoiceDate = DateTime.ParseExact(objFailure.sTcInvoiceDate.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                //int months = (InvoiceDate.Month - lastRepDate.Month);
                                double months = InvoiceDate.Subtract(lastRepDate).Days / (365.25 / 12);

                                if (months > 6)
                                {
                                    lastRepDate = lastRepDate.AddMonths(18);
                                    if ((lastRepDate > Convert.ToDateTime(DateTime.Now.Date.ToShortDateString().Replace('-', '/'))))
                                    {
                                        cmbGuarenteeType.SelectedValue = "WRGP";
                                    }
                                    else
                                    {
                                        cmbGuarenteeType.SelectedValue = "AGP";
                                    }
                                }
                                else
                                {
                                    InvoiceDate = InvoiceDate.AddMonths(12);
                                    if ((InvoiceDate > Convert.ToDateTime(DateTime.Now.Date.ToShortDateString().Replace('-', '/'))))
                                    {
                                        cmbGuarenteeType.SelectedValue = "WRGP";
                                    }
                                    else
                                    {
                                        cmbGuarenteeType.SelectedValue = "AGP";
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            lblMessage.Text = clsException.ErrorMsg();
                            clsException.LogError(ex.ToString(), ex.Message, strFormCode, "GetFailureDetails");
                        }
                    }

                    if (rdbFail.Checked)
                    {
                        if (Request.QueryString["ActionType"] != null && Request.QueryString["ActionType"].ToString() != "")
                        {
                            txtActiontype.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));
                            if (txtActiontype.Text == "V")
                            {
                                if (txtCapacity.Text != cmbEnhanceCapacity.SelectedItem.Text)
                                {
                                    rdbFailEnhance.Checked = true;
                                    rdbFail.Checked = false;
                                }
                                if (cmbEnhanceCapacity.SelectedItem.Text == "--Select--" || cmbEnhanceCapacity.SelectedItem.Text == "")
                                {
                                    rdbFailEnhance.Checked = false;
                                    rdbFail.Checked = true;
                                    cmbEnhanceCapacity.SelectedItem.Text = txtCapacity.Text.Trim();
                                    lblEnCap.Visible = false;
                                    cmbEnhanceCapacity.Visible = false;
                                }
                            }
                        }
                        else
                        {
                            if (txtFailurId.Text == "0")
                            {

                            }
                            else
                            {
                                if (cmbEnhanceCapacity.SelectedItem.Text == txtCapacity.Text)
                                {
                                    cmbEnhanceCapacity.SelectedItem.Text = txtCapacity.Text.Trim();
                                }
                                else
                                {
                                    if (cmbEnhanceCapacity.SelectedItem.Text == "--Select--" || cmbEnhanceCapacity.SelectedItem.Text == "")
                                    {
                                        cmbEnhanceCapacity.SelectedItem.Text = txtCapacity.Text.Trim();
                                        rdbFailEnhance.Checked = false;
                                        rdbFail.Checked = true;
                                        rdbFailEnhance.Enabled = false;
                                        cmbEnhanceCapacity.Visible = false;
                                        lblEnCap.Visible = false;
                                    }
                                    else
                                    {
                                        cmbEnhanceCapacity.SelectedItem.Text = objFailure.sEnhancedCapacity;
                                        rdbFailEnhance.Checked = true;
                                        rdbFail.Checked = false;
                                        cmbEnhanceCapacity.Visible = true;
                                        cmbEnhanceCapacity.Enabled = false;
                                        rdbFail.Enabled = false;
                                        lblEnCap.Visible = true;
                                    }
                                }
                            }
                        }
                    }
                    if (txtDTCName.Text.Trim() == "")
                    {
                        EmptyDTCDetails();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "cmdSearch_Click");
            }



        }

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["DTCId"] != null && Request.QueryString["DTCId"].ToString() != "")
                {
                    cmbFailureType.SelectedIndex = 0;
                    txtReason.Text = string.Empty;
                    txtFailedDate.Text = string.Empty;
                    txtDTCRead.Text = string.Empty;
                    cmbHtBusing.SelectedIndex = 0;
                    cmbLtBusing.SelectedIndex = 0;
                    cmbDrainValve.SelectedIndex = 0;
                    cmbHtBusingRod.SelectedIndex = 0;
                    cmbLtBusingRod.SelectedIndex = 0;
                    cmbOilLevel.SelectedIndex = 0;
                    cmbTankCondition.SelectedIndex = 0;
                    cmbExplosion.SelectedIndex = 0;
                    cmbBreather.SelectedIndex = 0;
                    txtCustomerName.Text = string.Empty;
                    txtCustMobileNo.Text = string.Empty;
                    txtNumberofInstalmets.Text = string.Empty;
                }
                else
                {
                    txtDTCCode.Text = string.Empty;
                    txtDTCName.Text = string.Empty;
                    txtLoadKW.Text = string.Empty;
                    txtLoadHP.Text = string.Empty;
                    txtDTCCommissionDate.Text = string.Empty;
                    txtCapacity.Text = string.Empty;
                    txtLocation.Text = string.Empty;
                    txtTCSlno.Text = string.Empty;
                    txtTcCode.Text = string.Empty;
                    txtTCMake.Text = string.Empty;
                    txtFailedDate.Text = string.Empty;
                    txtReason.Text = string.Empty;
                    txtDTCRead.Text = string.Empty;
                    txtDtcId.Text = string.Empty;
                    txtFailurId.Text = string.Empty;
                    cmdSave.Text = "Save";
                    txtDTCCode.Enabled = true;
                    hdfDTCcode.Value = string.Empty;
                    cmdSearch.Visible = true;
                    txtManfDate.Text = string.Empty;
                    txtDTrCommDate.Text = string.Empty;
                    cmbStarRated.SelectedValue = "--Select--";
                    cmbFailureType.SelectedIndex = 0;
                    cmbHtBusing.SelectedIndex = 0;
                    cmbLtBusing.SelectedIndex = 0;
                    cmbDrainValve.SelectedIndex = 0;
                    cmbHtBusingRod.SelectedIndex = 0;
                    cmbLtBusingRod.SelectedIndex = 0;
                    cmbOilLevel.SelectedIndex = 0;
                    txtQuantityOfOil.Text = string.Empty;
                    cmbTankCondition.SelectedIndex = 0;
                    cmbExplosion.SelectedIndex = 0;
                    cmbBreather.SelectedIndex = 0;
                    txtDTCRead.Text = string.Empty;
                    txtCustMobileNo.Text = string.Empty;
                    txtCustomerName.Text = string.Empty;
                    txtNumberofInstalmets.Text = string.Empty;

                }
                if (txtTankcapacity.Enabled == true || txtQuantityOfOil.Enabled == true)
                {
                    txtTankcapacity.Text = string.Empty;
                    txtQuantityOfOil.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "cmdReset_Click");
            }
        }

        protected void cmdClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["ActionType"] != null && Request.QueryString["ActionType"].ToString() != "")
                {
                    Response.Redirect("/Approval/ApprovalInbox.aspx", false);
                }
                else
                {
                    Response.Redirect("FailureEntryView.aspx", false);
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "cmdClose_Click");
            }
        }

        public void ValidateFormUpdate()
        {
            try
            {
                clsFailureEntry objFailure = new clsFailureEntry();
                if (objFailure.ValidateUpdate(txtFailurId.Text) == true)
                {
                    cmdReset.Enabled = false;
                    //cmdSave.Enabled = false;
                }
                else
                {
                    cmdReset.Enabled = true;
                    cmdSave.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "ValidateFormUpdate");
            }
        }


        public bool ValidateForm()
        {
            bool bValidate = false;
            try
            {
                String a = cmbGuarenteeType.SelectedItem.Text;

                if (cmbFailureType.SelectedItem.Text == "--Select--")
                {
                    cmbFailureType.Focus();
                    ShowMsgBox(" Please Select Failure Type");
                    return bValidate;
                }
                if (txtDTrCommDate.Text.Trim() == "" || txtDTrCommDate.Text.Trim() == null)
                {
                    txtFailedDate.Focus();
                    ShowMsgBox("Dtr Commission Date is Empty Please verify");
                    return bValidate;
                }
                if (txtDTCCommissionDate.Text.Trim() == "" || txtDTCCommissionDate.Text.Trim() == null)
                {
                    txtDTCCommissionDate.Focus();
                    ShowMsgBox("Enter  DTC Commission Date");
                    return bValidate;
                }

                if (txtFailedDate.Text.Trim() == "")
                {
                    txtFailedDate.Focus();
                    ShowMsgBox("Enter Failed Date");
                    return bValidate;
                }

                if (cmbGuarenteeType.Enabled == true && cmbGuarenteeType.SelectedIndex == 0)
                {
                    ShowMsgBox("Please Select Guarentee Type");
                    cmbGuarenteeType.Focus();
                    return bValidate;
                }

                if (txtTcCode.Text.Trim() == "0" || txtTcCode.Text.Trim() == "" || txtTcCode.Text.Trim() == null)
                {
                    ShowMsgBox("This DTC is Currently having No TC, please contact the DTLMS Team");
                    return bValidate;
                }
                string sResult = Genaral.DateComparision(txtFailedDate.Text, txtDTCCommissionDate.Text, false, false);
                if (sResult == "2")
                {
                    ShowMsgBox("Failure Date should be Greater than Commission Date");
                    return bValidate;
                }
                sResult = Genaral.DateComparision(txtFailedDate.Text, "", true, false);
                if (sResult == "1")
                {
                    ShowMsgBox("Failure Date should be Less than Current Date");
                    return bValidate;
                }
                // DTR Commission Date should ge greater than DTC Commission Date
                sResult = Genaral.DateComparision(txtDTrCommDate.Text, txtDTCCommissionDate.Text, false, true);
                if (sResult == "2")
                {
                    ShowMsgBox("DTR Commission Date should ge greater than DTC Commission Date");
                    return bValidate;
                }



                sResult = Genaral.DateComparision(txtDTrCommDate.Text, txtManfDate.Text, false, true);
                if (sResult == "2")
                {
                    ShowMsgBox("Manufacturing  Date should ge lesser than DTR Commission Date");
                    return bValidate;
                }

                if (txtReason.Text.Trim() == "")
                {
                    txtReason.Focus();
                    ShowMsgBox("Enter the Failure Reason");
                    return bValidate;
                }
                if (rdbFailEnhance.Checked == true)
                {
                    if (cmbEnhanceCapacity.SelectedItem.Text == "--Select--")
                    {
                        cmbEnhanceCapacity.Focus();
                        ShowMsgBox("Select Enhance Capacity");
                        return bValidate;
                    }

                    if (cmbEnhanceCapacity.SelectedItem.Text == txtCapacity.Text)
                    {
                        cmbEnhanceCapacity.Focus();
                        ShowMsgBox("Select Different Capacity");
                        return bValidate;
                    }
                }
                if (txtCustomerName.Text.Trim() == "")
                {
                    txtCustomerName.Focus();
                    ShowMsgBox(" Please Enter Customer Name");
                    return bValidate;
                }
                if (txtCustMobileNo.Text.Trim() == "")
                {
                    txtCustMobileNo.Focus();
                    ShowMsgBox("Please Enter Customer Mobile No");
                    return bValidate;
                }

                if (txtCustMobileNo.Text.Replace(" ", string.Empty).Length < 10)
                {
                    txtCustMobileNo.Focus();
                    ShowMsgBox(" Please Enter Valid 10 Digit Customer Mobile No");
                    return bValidate;
                }

                if (txtNumberofInstalmets.Text.Trim() == "")
                {
                    txtNumberofInstalmets.Focus();
                    ShowMsgBox("Please Enter Number Of Installation");
                    return bValidate;
                }
                if (System.Text.RegularExpressions.Regex.IsMatch(txtNumberofInstalmets.Text, "[^0-9]"))
                {
                    txtNumberofInstalmets.Focus();
                    ShowMsgBox("Number Of Installation Should be in Numbers");
                    return bValidate;
                }


                if (txtLatitude.Text.Trim() == "")
                {
                    txtLatitude.Focus();
                    ShowMsgBox("Please Enter Latitude");
                    return bValidate;
                }
                if (Convert.ToDouble(txtLatitude.Text) <= 0)
                {
                    txtLatitude.Focus();
                    ShowMsgBox("Please Enter Valid Latitude");
                    return false;
                }

                if (txtLongitude.Text.Trim() == "")
                {
                    txtLongitude.Focus();
                    ShowMsgBox("Please Enter Longitude");
                    return bValidate;
                }
                if (Convert.ToDouble(txtLongitude.Text) <= 0)
                {
                    txtLongitude.Focus();
                    ShowMsgBox("Please Enter Valid Longitude");
                    return false;
                }
                if (txtTankcapacity.Text.Trim().Length == 0)
                {
                    txtTankcapacity.Focus();
                    ShowMsgBox("Enter Tank Capacity(in Liter)");
                    return false;
                }

              //  bool result = txtTankcapacity.Text.Any(x => !char.IsLetter(x));

                bool Exists = Regex.IsMatch(txtTankcapacity.Text, @"\d");

                if (Exists == true)
                {
                    if (Convert.ToDouble(txtTankcapacity.Text.Trim('.')) <= 0)
                    {
                        txtTankcapacity.Focus();
                        ShowMsgBox("Enter Valid Tank Capacity(in Liter)");
                        return false;
                    }
                }
                else
                {
                    txtTankcapacity.Focus();
                    ShowMsgBox("Enter Valid Tank Capacity(in Liter)");
                    return false;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(txtTankcapacity.Text, "^(\\d{1,4})?(\\.\\d{1,2})?$"))
                {
                    ShowMsgBox("Please Enter Valid Tank Capacity(in Liter) (eg:1111.00)");
                    return bValidate;
                }

                if (txtQuantityOfOil.Text.Trim().Length == 0)
                {
                    txtQuantityOfOil.Focus();
                    ShowMsgBox("Enter Total Oil Quantity(in Liter)");
                    return false;
                }
                bool Existsoil = Regex.IsMatch(txtQuantityOfOil.Text, @"\d");
            //    bool resulttank = txtQuantityOfOil.Text.Any(x => !char.IsLetter(x));
                if (Existsoil == true)
                {
                    if (Convert.ToDouble(txtQuantityOfOil.Text.Trim('.')) <= 0)
                    {
                        txtQuantityOfOil.Focus();
                        ShowMsgBox("Enter Total Oil Quantity(in Liter)");
                        return false;
                    }
                }
                else
                {
                    txtQuantityOfOil.Focus();
                    ShowMsgBox("Enter Valid Total Oil Quantity(in Liter)");
                    return false;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(txtQuantityOfOil.Text, "^(\\d{1,4})?(\\.\\d{1,2})?$"))
                {
                    ShowMsgBox("Please Enter Valid Oil Quantity(in Liter) (eg:1111.00)");
                    return bValidate;
                }
                if (!(Convert.ToDouble(txtQuantityOfOil.Text) <= Convert.ToDouble(txtTankcapacity.Text)))
                {
                    txtQuantityOfOil.Focus();
                    ShowMsgBox("Total Oil Quantity Should be less than or equals to Tank Capacity");
                    return false;
                }

                bValidate = true;
                return bValidate;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "ValidateForm");
                return bValidate;
            }
        }

        public void EmptyDTCDetails()
        {
            try
            {
                txtDTCName.Text = string.Empty;
                //txtServiceDate.Text = string.Empty;
                txtLoadKW.Text = string.Empty;
                txtLoadHP.Text = string.Empty;
                txtDTCCommissionDate.Text = string.Empty;
                txtCapacity.Text = string.Empty;
                txtLocation.Text = string.Empty;
                txtTcCode.Text = string.Empty;
                txtTCMake.Text = string.Empty;
                txtTCSlno.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "EmptyDTCDetails");
            }
        }

        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "FailureEntry";
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
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "CheckAccessRights");
                return false;

            }
        }

        #endregion

        public void WorkFlowObjects(clsFailureEntry objFailure)
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


                objFailure.sFormName = "FailureEntry";
                objFailure.sOfficeCode = objSession.OfficeCode;
                objFailure.sClientIP = sClientIP;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "WorkFlowObjects");
            }
        }

        #region Workflow/Approval

        public void SetControlText()
        {
            try
            {
                txtActiontype.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["ActionType"]));

                if (txtActiontype.Text == "A")
                {
                    if (txtCapacity.Text == cmbEnhanceCapacity.SelectedItem.Text)
                    {
                        rdbFail.Checked = true;
                        //rdbFailEnhance.Enabled = false;
                    }
                    else
                    {
                        if (objSession.RoleId == "4")
                        {
                            if (txtCapacity.Text == cmbEnhanceCapacity.SelectedItem.Text)
                            {
                                rdbFail.Checked = true;
                            }
                            else
                            {
                                rdbFailEnhance.Checked = true;
                                rdbFail.Checked = false;
                                cmbEnhanceCapacity.Visible = true;
                                cmbEnhanceCapacity.Enabled = true;
                                lblEnCap.Visible = true;
                            }
                        }
                        else
                        {
                            rdbFail.Enabled = false;
                            rdbFail.Checked = false;
                            rdbFailEnhance.Checked = true;
                            lblEnCap.Enabled = false;
                            cmbEnhanceCapacity.Enabled = false;
                            lblEnCap.Visible = true;
                            cmbEnhanceCapacity.Visible = true;
                        }
                    }

                    cmdSave.Text = "Approve";
                    pnlApproval.Enabled = false;
                }
                if (txtActiontype.Text == "R")
                {
                    if (txtCapacity.Text == cmbEnhanceCapacity.SelectedItem.Text)
                    {
                        rdbFail.Checked = true;
                        rdbFailEnhance.Enabled = false;
                    }
                    else
                    {
                        rdbFail.Enabled = false;
                        rdbFail.Checked = false;
                        rdbFailEnhance.Checked = true;
                        lblEnCap.Enabled = false;
                        cmbEnhanceCapacity.Enabled = false;
                        lblEnCap.Visible = true;
                        cmbEnhanceCapacity.Visible = true;
                    }
                    cmdSave.Text = "Reject";
                    pnlApproval.Enabled = false;
                }
                if (txtActiontype.Text == "M")
                {
                    //cmbGuarenteeType.Enabled = true;
                    if (txtCapacity.Text == cmbEnhanceCapacity.SelectedItem.Text)
                    {
                        rdbFail.Checked = true;
                        rdbFailEnhance.Enabled = false;
                    }
                    else
                    {
                        rdbFail.Enabled = false;
                        rdbFail.Checked = false;
                        rdbFailEnhance.Checked = true;
                        lblEnCap.Enabled = false;
                        cmbEnhanceCapacity.Enabled = true;
                        lblEnCap.Visible = true;
                        cmbEnhanceCapacity.Visible = true;

                    }
                    cmdSave.Text = "Modify and Approve";
                    pnlApproval.Enabled = true;
                }

                if (txtActiontype.Text == "V" && hdfWFDataId.Value != "")
                {
                    if (txtCapacity.Text == cmbEnhanceCapacity.SelectedItem.Text)
                    {
                        rdbFail.Checked = true;
                        rdbFailEnhance.Enabled = false;
                    }
                    else
                    {
                        rdbFail.Enabled = false;
                        rdbFail.Checked = false;
                        rdbFailEnhance.Checked = true;
                        lblEnCap.Enabled = false;
                        cmbEnhanceCapacity.Enabled = false;
                        lblEnCap.Visible = true;
                        cmbEnhanceCapacity.Visible = true;

                    }
                }

                dvComments.Style.Add("display", "block");
                cmdReset.Enabled = false;


                // Check for Creator of Form
                bool bResult = CheckFormCreatorLevel();
                if (bResult == true)
                {
                    cmdSave.Text = "Save";
                    pnlApproval.Enabled = true;

                    // To handle Record From Reject 
                    if (txtActiontype.Text == "A" && hdfWFDataId.Value != "")
                    {
                        txtActiontype.Text = "M";
                        hdfRejectApproveRef.Value = "RA";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "SetControlText");
            }
        }

        public void ApproveRejectAction()
        {
            try
            {
                clsApproval objApproval = new clsApproval();


                if (txtComment.Text.Trim() == "")
                {
                    ShowMsgBox("Enter Comments/Remarks");
                    txtComment.Focus();
                    return;
                }

                objApproval.sCrby = objSession.UserId;
                objApproval.sOfficeCode = objSession.OfficeCode;
                objApproval.sApproveComments = txtComment.Text.Trim();
                objApproval.sWFObjectId = hdfWFOId.Value;
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
                    bResult = objApproval.ModifyApproveWFRequest1(objApproval);
                }
                else
                {
                    bResult = objApproval.ApproveWFRequest1(objApproval);
                }

                if (bResult == true)
                {

                    if (objApproval.sApproveStatus == "1")
                    {
                        //  ShowMsgBox("Approved Successfully");
                        txtFailurId.Text = objApproval.sNewRecordId;

                        cmdSave.Enabled = false;

                        if (objSession.RoleId == "1")
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
                            sWoID = ObjFailure.getWoIDforEstimation(sOffCode, txtDTCCode.Text);
                            EstimationReportSO(sWoID);
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
                        txtFailurId.Text = objApproval.sNewRecordId;
                        cmdSave.Enabled = false;

                        if (objSession.RoleId == "1")
                        {
                            clsFailureEntry objFailure = new clsFailureEntry();
                            //objFailure.SendSMStoSectionOfficer(txtFailureOfficCode.Text, txtDTCCode.Text);
                            if (txtCapacity.Text == cmbEnhanceCapacity.SelectedItem.Text)
                            {
                                flag = "1";
                            }
                            else
                            {
                                flag = "4";
                            }
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
                //lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "cmdApprove_Click");
                throw ex;
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
                        hdfWFOId.Value = Convert.ToString(Session["WFOId"]);
                        hdfApproveStatus.Value = Convert.ToString(Session["ApproveStatus"]);
                        hdfWFOAutoId.Value = Convert.ToString(Session["WFOAutoId"]);

                        Session["WFDataId"] = null;
                        Session["WFOId"] = null;
                        Session["ApproveStatus"] = null;
                        Session["WFOAutoId"] = null;
                    }

                    if ((hdfWFDataId.Value ?? "").Length > 0)
                    {
                        GetFailureDetailsFromXML(hdfWFDataId.Value);
                    }

                    SetControlText();
                    ControlEnableDisable();
                    if (txtActiontype.Text == "V")
                    {
                        cmdSave.Text = "View";
                        cmdReset.Enabled = false;
                        dvComments.Style.Add("display", "none");
                    }
                }
                else
                {
                    if (cmdSave.Text != "Save" && cmdSave.Text != "View")
                    {
                        cmdSave.Enabled = false;

                    }
                    //cmdSave.Text = "View";
                }

                DisableControlForView();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                // clsException.LogError(ex.ToString(), ex.Message, strFormCode, "WorkFlowConfig");
            }
        }


        public bool CheckFormCreatorLevel()
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                string sResult = objApproval.GetFormCreatorLevel("", objSession.RoleId, "FailureEntry");
                if (sResult == "1")
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "GetFormCreatorLevel");
                return false;
            }
        }

        public void DisableControlForView()
        {
            try
            {
                if (cmdSave.Text.Contains("View"))
                {
                    pnlApproval.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "DisableControlForView");
            }
        }


        #endregion

        public string EstimationReport(string flag)
        {
            try
            {
                if (txtFailurId.Text.Contains("-"))
                {
                    return "FAILURE";
                }
                //if (cmdSave.Text == "Save")
                //{
                clsEstimation objEst = new clsEstimation();
                objEst.sOfficeCode = txtFailureOfficCode.Text;
                objEst.sFailureId = txtFailurId.Text;
                objEst.sLastRepair = txtLastRepairer.Text;
                objEst.sCrby = objSession.UserId;

                string status = objEst.SaveEstimationDetails(objEst);
                if (status != "SUCCESS")
                {
                    ShowMsgBox("OOPS Estimation report cannot be generated please contact support team ");
                    return "FAILURE";
                }
                //}

                string strParam = string.Empty;
                strParam = "id=Estimation&FailureId=" + txtFailurId.Text;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                return "SUCCESS";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "EstimationReport");
                return "FAILURE";
            }
        }

        public void EstimationReportSO(string sWoID)
        {
            try
            {


                string STCcode = txtTcCode.Text;
                string strParam = string.Empty;
                strParam = "id=EstimationSO&TCcode=" + STCcode + "&WOId=" + sWoID + "&Failtype=" + flag;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "EstimationReport");
            }
        }

        protected void lnkDTrDetails_Click(object sender, EventArgs e)
        {
            try
            {
                string sTCId = string.Empty;

                if (txtTcCode.Text.Trim() == "" || txtTcCode.Text.Trim() == null)
                {
                    txtTcCode.Focus();
                    ShowMsgBox("Enter the TC Code ");
                }
                else
                {
                    sTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtTCId.Text));
                    string url = "/MasterForms/TcMaster.aspx?TCId=" + sTCId;
                    string s = "window.open('" + url + "', '_blank');";
                    ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "lnkDTrDetails_Click");
            }
        }

        protected void lnkDTCDetails_Click(object sender, EventArgs e)
        {
            try
            {
                string sDTCId = string.Empty;
                if (txtDTCCode.Text.Trim() == "" || txtDTCCode.Text.Trim() == null)
                {
                    txtDTCCode.Focus();
                    ShowMsgBox("Enter the DTC Code ");
                }
                else
                {

                    sDTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtDtcId.Text));
                    string url = "/MasterForms/DTCCommision.aspx?QryDtcId=" + sDTCId;
                    string s = "window.open('" + url + "', '_blank');";
                    ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "lnkDTCDetails_Click");
            }

        }

        public void ControlEnableDisable()
        {
            try
            {
                txtDTCCode.Enabled = false;
                cmdSearch.Visible = false;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "ControlEnableDisable");
            }
        }

        #region Load From XML
        public void GetFailureDetailsFromXML(string sWFDataId)
        {
            try
            {

                clsFailureEntry objFailure = new clsFailureEntry();
                objFailure.sWFDataId = sWFDataId;
                objFailure.GetFailureDetailsFromXML(objFailure);

                txtDtcId.Text = objFailure.sDtcId;
                txtDTCCode.Text = objFailure.sDtcCode;
                txtDTCName.Text = objFailure.sDtcName;
                //txtServiceDate.Text = objFailure.sDtcServicedate;
                txtLoadKW.Text = objFailure.sDtcLoadKw;
                txtLoadHP.Text = objFailure.sDtcLoadHp;
                txtDTCCommissionDate.Text = objFailure.sCommissionDate;
                txtCapacity.Text = objFailure.sDtcCapacity;
                txtLocation.Text = objFailure.sDtcLocation;
                txtTcCode.Text = objFailure.sDtcTcCode;
                txtTCSlno.Text = objFailure.sDtcTcSlno;
                if (objFailure.sDTRStarRating == "")
                {
                    cmbStarRated.SelectedValue = "--Select--";
                }
                else
                {
                    cmbStarRated.SelectedValue = objFailure.sDTRStarRating;
                }
                if (objFailure.sEnhancedCapacity != null)
                    if (txtCapacity.Text == objFailure.sEnhancedCapacity)
                    {
                        cmbEnhanceCapacity.SelectedItem.Text = txtCapacity.Text;
                    }
                cmbEnhanceCapacity.SelectedItem.Text = objFailure.sEnhancedCapacity;

                //if (objFailure.sEnhancedCapacity == "" || objFailure.sEnhancedCapacity == null)
                if ((objFailure.sEnhancedCapacity ?? "").Length > 0)
                {
                    cmbEnhanceCapacity.SelectedItem.Text = txtCapacity.Text;
                }

                txtTCId.Text = objFailure.sTCId;

                txtLastRepairDate.Text = objFailure.sLastRepairedDate;
                txtLastRepairer.Text = objFailure.sLastRepairedBy;
                cmbGuarenteeType.SelectedValue = objFailure.sFirstGuarantyType;
                hdfGuarenteeSource.Value = objFailure.sGuarantySource;

                txtManfDate.Text = objFailure.sManfDate;
                txtDTrCommDate.Text = objFailure.sDTrCommissionDate;

                txtTCMake.Text = objFailure.sDtcTcMake;
                if ((objFailure.sFailureDate ?? "").Length > 0)
                {
                    if (objFailure.sFailureDate.Contains(':'))
                    {
                        DateTime DfDate = DateTime.ParseExact(objFailure.sFailureDate.Replace('-', '/'),
                            "d/M/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                        txtFailedDate.Text = DfDate.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        txtFailedDate.Text = objFailure.sFailureDate;
                    }
                }

                txtReason.Text = objFailure.sFailureReasure;
                txtDTCRead.Text = objFailure.sDtcReadings;
                txtDTCCode.Enabled = false;
                txtFailureOfficCode.Text = objFailure.sOfficeCode;
                hdfCrBy.Value = objFailure.sCrby;
                txtCustomerName.Text = objFailure.sCustomerName;
                txtCustMobileNo.Text = objFailure.sCustomerNumber;
                txtNumberofInstalmets.Text = objFailure.sNumberOfInstalments;

                if ((objFailure.sFailureType ?? "").Length > 0)
                {
                    cmbFailureType.SelectedValue = objFailure.sFailureType.Trim();
                }


                //if (objFailure.sHTBusing != "")
                if ((objFailure.sHTBusing ?? "").Length > 0)
                {
                    cmbHtBusing.SelectedValue = objFailure.sHTBusing;
                }
                //if (objFailure.sLTBusing != "")
                if ((objFailure.sLTBusing ?? "").Length > 0)
                {
                    cmbLtBusing.SelectedValue = objFailure.sLTBusing;
                }
                //if (objFailure.sHTBusingRod != "")
                if ((objFailure.sHTBusingRod ?? "").Length > 0)
                {
                    cmbHtBusingRod.SelectedValue = objFailure.sHTBusingRod;
                }
                //if (objFailure.sLTBusingRod != "")
                if ((objFailure.sLTBusingRod ?? "").Length > 0)
                {
                    cmbLtBusingRod.SelectedValue = objFailure.sLTBusingRod;
                }
                //if (objFailure.sDrainValve != "")
                if ((objFailure.sDrainValve ?? "").Length > 0)
                {
                    cmbDrainValve.SelectedValue = objFailure.sDrainValve;
                }
                //if (objFailure.sOilLevel != "")
                if ((objFailure.sOilLevel ?? "").Length > 0)
                {
                    cmbOilLevel.SelectedValue = objFailure.sOilLevel;
                }
                //if (objFailure.sOilQuantity != "")
                if ((objFailure.sOilQuantity ?? "").Length > 0)
                {
                    txtQuantityOfOil.Text = objFailure.sOilQuantity;

                }
                //if (objFailure.sTankCondition != "")
                if ((objFailure.sTankCondition ?? "").Length > 0)
                {
                    cmbTankCondition.SelectedValue = objFailure.sTankCondition;
                }

                //if (objFailure.sExplosionValve != "")
                if ((objFailure.sExplosionValve ?? "").Length > 0)
                {
                    cmbExplosion.SelectedValue = objFailure.sExplosionValve;
                }
                //if (objFailure.sBreather != "")
                if ((objFailure.sBreather ?? "").Length > 0)
                {
                    cmbBreather.SelectedValue = objFailure.sBreather;
                }

                if (objFailure.sFailureId != "0")
                {

                    //cmdSave.Text = "Update";
                    cmdSearch.Visible = false;
                    cmdReset.Enabled = false;
                }
                if ((objFailure.Latitude ?? "").Length > 0 && (objFailure.Longitude ?? "").Length > 0)
                {
                    if (objFailure.Latitude.Length > 3 && objFailure.Longitude.Length > 3)
                    {
                        txtLatitude.Text = objFailure.Latitude;
                        txtLongitude.Text = objFailure.Longitude;
                        txtLatitude.Enabled = false;
                        txtLongitude.Enabled = false;
                    }
                }



                if ((objFailure.sTankcapacity ?? "").Length > 0)
                {
                    txtTankcapacity.Text = objFailure.sTankcapacity;
                    txtTankcapacity.Enabled = false;
                }
                if ((objFailure.sOilCapacity ?? "").Length > 0)
                {
                    txtQuantityOfOil.Text = objFailure.sOilCapacity;
                    txtQuantityOfOil.Enabled = false;
                }


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "GetFailureDetailsFromXML");
            }

        }
        #endregion

        public void LoadSearchWindow()
        {
            try
            {
                string strQry = string.Empty;
                strQry = "Title=Search and Select DTC Failure Details&";
                strQry += "Query=select DT_CODE,DT_NAME FROM TBLDTCMAST WHERE DT_OM_SLNO LIKE '" + objSession.OfficeCode + "%' AND ";
                strQry += " DT_CODE NOT IN (SELECT DF_DTC_CODE FROM TBLDTCFAILURE WHERE  DF_REPLACE_FLAG = 0 ) AND {0} like %{1}% order by DT_CODE&";
                strQry += "DBColName=DT_CODE~DT_NAME&";
                strQry += "ColDisplayName=DTC Code~DTC Name&";

                strQry = strQry.Replace("'", @"\'");

                cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?"
                    + strQry + "tb=" + txtDTCCode.ClientID + "&btn=" + cmdSearch.ClientID
                    + "',520,520," + txtDTCCode.ClientID + ")");

                txtFailedDate.Attributes.Add("onblur", "return ValidateDate(" + txtFailedDate.ClientID + ");");

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "LoadSearchWindow");
            }
        }

        protected void rdbFailEnhance_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbFailEnhance.Checked == true)
            {
                lblEnCap.Visible = true;
                cmbEnhanceCapacity.Visible = true;
                cmbEnhanceCapacity.Enabled = true;
                Genaral.Load_Combo("SELECT MD_ID, MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'C' AND MD_NAME <>'"
                    + txtCapacity.Text + "' ORDER BY MD_ID", "--Select--", cmbEnhanceCapacity);
            }
            else
            {
                lblEnCap.Visible = false;
                cmbEnhanceCapacity.Visible = false;
                cmbEnhanceCapacity.Enabled = false;
                cmbEnhanceCapacity.SelectedItem.Text = txtCapacity.Text.Trim();
            }
        }

    }
}