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
    public partial class Enhancement : System.Web.UI.Page
    {

        string strFormCode = "Enhancement";
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
                    Genaral.Load_Combo("SELECT MD_NAME,MD_NAME from TBLMASTERDATA WHERE MD_TYPE='C' ORDER BY MD_ID", "--Select--", cmbCapacity);

                    txtEnhanceDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");

                    if (Request.QueryString["StatusFalg"] != null && Request.QueryString["StatusFalg"].ToString() != "")
                    {
                        hdfStatusFlag.Value = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["StatusFalg"]));
                        if (hdfStatusFlag.Value == "5")
                        {
                            dvEnCapacity.Visible = false;
                            dvenhance.Visible = false;
                            dvenhance1.Visible = false;
                            dvEndate.Visible = false;
                            dvReCapacity.Visible = true;
                            dvreduction.Visible = true;
                            dvreduction1.Visible = true;
                            dvRedate.Visible = true;
                        }
                        else
                        {
                            dvEnCapacity.Visible = true;
                            dvenhance.Visible = true;
                            dvenhance1.Visible = true;
                            dvEndate.Visible = true;
                            dvReCapacity.Visible = false;
                            dvreduction.Visible = false;
                            dvreduction1.Visible = false;
                            dvRedate.Visible = false;
                        }

                    }


                    if (Request.QueryString["DTCId"] != null && Request.QueryString["DTCId"].ToString() != "")
                    {

                        txtDtcId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["DTCId"]));
                        txtEnhancementId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["EnhanceId"]));


                        if (!txtEnhancementId.Text.Contains("-"))
                        {
                            GetEnhancementDetails();

                            ValidateFormUpdate();
                        }
                    }

                    //Search Window Call
                    LoadSearchWindow();


                    //WorkFlow / Approval
                    WorkFlowConfig();
                    string Enhancedcapacity = cmbCapacity.SelectedValue;

                    if (hdfStatusFlag.Value == "5" && (txtCapacity.Text ?? "").Length > 0)
                    {
                        Genaral.Load_Combo("SELECT MD_NAME,MD_NAME from TBLMASTERDATA WHERE " +
                             " MD_TYPE='C' and MD_NAME < " + txtCapacity.Text + " ORDER BY MD_ID", "--Select--", cmbCapacity);
                        cmbCapacity.SelectedValue = Enhancedcapacity;
                    }
                    else if ((txtCapacity.Text ?? "").Length > 0)
                    {
                        Genaral.Load_Combo("SELECT MD_NAME,MD_NAME from TBLMASTERDATA WHERE MD_TYPE='C' and " +
                             " MD_NAME > " + txtCapacity.Text + " ORDER BY MD_ID", "--Select--", cmbCapacity);
                        cmbCapacity.SelectedValue = Enhancedcapacity;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load");
            }

        }


        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {

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
                            EstimationReport();
                        }
                    }
                    else
                    {
                        EstimationReport();
                    }
                    return;
                }


                if (ValidateForm() == true)
                {
                    clsEnhancement objEnhancement = new clsEnhancement();
                    string[] Arr = new string[2];

                    objEnhancement.sEnhancementId = txtEnhancementId.Text;

                    objEnhancement.sDtcId = txtDtcId.Text;
                    objEnhancement.sTcCode = txtTcCode.Text;
                    objEnhancement.sDtcCode = txtDTCCode.Text.Replace("'", "");
                    objEnhancement.sEnhancementDate = txtEnhanceDate.Text.Replace("'", "");
                    objEnhancement.sDtrcommdate = txtDTrCommDate.Text;
                    objEnhancement.sReason = txtReason.Text.Replace("'", "").Replace("\"", "").Replace(";", "").Replace(",", "ç");
                    objEnhancement.sDtcReadings = txtDTCRead.Text.Replace("'", "");
                    objEnhancement.sCrby = objSession.UserId;
                    objEnhancement.sEnhancedCapacity = cmbCapacity.SelectedValue;
                    objEnhancement.sStatusFlag = hdfStatusFlag.Value;
                    objEnhancement.Totaloilquantity = txtQuantityOfOil.Text;
                    objEnhancement.Tankcapacity = txtTankcapacity.Text;
                    objEnhancement.Latitude = txtLatitude.Text;
                    objEnhancement.Longitude = txtLongitude.Text;
                    if (txtActiontype.Text == "A" || txtActiontype.Text == "R")
                    {
                        ApproveRejectAction();
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "Enhancement");
                        }

                        return;
                    }

                    //Workflow
                    WorkFlowObjects(objEnhancement);

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
                        objEnhancement.sEnhancementId = "";
                        objEnhancement.sActionType = txtActiontype.Text;
                        objEnhancement.sOfficeCode = hdfEnhanceOffcode.Value;
                        objEnhancement.sCrby = hdfCrBy.Value;
                        Arr = objEnhancement.SaveEnhancementDetails(objEnhancement);
                        if (Arr[1].ToString() == "0")
                        {
                            hdfWFDataId.Value = objEnhancement.sWFDataId;
                            ApproveRejectAction();
                            if (objSession.sTransactionLog == "1")
                            {
                                Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "Enhancement");
                            }
                            return;
                        }
                        if (Arr[1].ToString() == "2")
                        {
                            ShowMsgBox(Arr[0]);
                            if (objSession.sTransactionLog == "1")
                            {
                                Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "Enhancement");
                            }
                            return;
                        }

                    }

                    #endregion

                    Arr = objEnhancement.SaveEnhancementDetails(objEnhancement);
                    string sOfcCode = objSession.OfficeCode;
                    string sdtcCode = txtDTCCode.Text;
                    string sWoid = objEnhancement.getWoIDforEstimation(sOfcCode, sdtcCode);

                    if (Arr[1].ToString() == "0")
                    {

                        txtEnhancementId.Text = objEnhancement.sEnhancementId;
                        cmdSave.Text = "Update";
                        ShowMsgBox(Arr[0].ToString());
                        cmdSave.Enabled = false;
                        EstimationReportSO(sWoid);
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "Enhancement");
                        }

                        return;
                    }
                    if (Arr[1].ToString() == "1")
                    {
                        if (txtActiontype.Text == "M")
                        {
                            ShowMsgBox("Modified and Approved Successfully");
                        }
                        else
                        {
                            ShowMsgBox(Arr[0]);
                        }
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "Enhancement");
                        }
                        return;
                    }

                    if (Arr[1].ToString() == "2")
                    {
                        ShowMsgBox(Arr[0]);
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "Enhancement");
                        }
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdSave_Click");
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


        public void GetEnhancementDetails()
        {
            try
            {
                clsEnhancement objEnhancement = new clsEnhancement();

                DataTable dtDetails = new DataTable();
                objEnhancement.sEnhancementId = txtEnhancementId.Text;
                objEnhancement.sDtcId = txtDtcId.Text;

                objEnhancement.GetEnhancementDetails(objEnhancement);

                txtDtcId.Text = objEnhancement.sDtcId;
                txtDTCCode.Text = objEnhancement.sDtcCode;
                txtDTCName.Text = objEnhancement.sDtcName;
                txtServiceDate.Text = objEnhancement.sServicedate;
                txtLoadKW.Text = objEnhancement.sLoadKw;
                txtLoadHP.Text = objEnhancement.sLoadHp;
                txtConnectionDate.Text = objEnhancement.sCommissionDate;
                txtCapacity.Text = objEnhancement.sCapacity;
                txtLocation.Text = objEnhancement.sLocation;
                txtTcCode.Text = objEnhancement.sTcCode;
                txtTCSlno.Text = objEnhancement.sTcSlno;
                txtTCMake.Text = objEnhancement.sTcMake;
                txtEnhanceDate.Text = objEnhancement.sEnhancementDate;
                txtReason.Text = objEnhancement.sReason;
                txtDTCRead.Text = objEnhancement.sDtcReadings;
                hdfTCId.Value = objEnhancement.sTCId;
                txtDTrCommDate.Text = objEnhancement.sDtrcommdate;

                txtDTCCode.Enabled = false;

                if (objEnhancement.Latitude.Length > 3 && objEnhancement.Longitude.Length > 3)
                {
                    txtLatitude.Text = objEnhancement.Latitude;
                    txtLongitude.Text = objEnhancement.Longitude;
                    txtLatitude.Enabled = false;
                    txtLongitude.Enabled = false;
                }


                if (objEnhancement.Tankcapacity.Length > 0)
                {
                    txtTankcapacity.Text = objEnhancement.Tankcapacity;
                    txtTankcapacity.Enabled = false;
                }
                if (objEnhancement.Totaloilquantity.Length > 0)
                {
                    txtQuantityOfOil.Text = objEnhancement.Totaloilquantity;
                    txtQuantityOfOil.Enabled = false;
                }
                hdfEnhanceOffcode.Value = objEnhancement.sOfficeCode;
                hdfCrBy.Value = objEnhancement.sCrby;
                if (objEnhancement.sEnhancementId != "0")
                {
                    //cmdSave.Text = "Update";
                    cmdSearch.Visible = false;
                }
                if (hdfStatusFlag.Value == "5")
                {
                    Genaral.Load_Combo("SELECT MD_NAME,MD_NAME from TBLMASTERDATA WHERE MD_TYPE='C' and MD_NAME < " + txtCapacity.Text + " ORDER BY MD_ID", "--Select--", cmbCapacity);

                }
                else
                {
                    Genaral.Load_Combo("SELECT MD_NAME,MD_NAME from TBLMASTERDATA WHERE MD_TYPE='C' and MD_NAME > " + txtCapacity.Text + " ORDER BY MD_ID", "--Select--", cmbCapacity);
                }
                if (objEnhancement.sEnhancedCapacity != "")
                {
                    cmbCapacity.SelectedValue = objEnhancement.sEnhancedCapacity;
                }

                if (txtEnhancementId.Text != "0")
                {
                    cmdSave.Text = "View";
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetEnhancementDetails");
            }

        }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {

            try
            {
                clsFailureEntry objFailure = new clsFailureEntry();
                //txtDTCCode.Text = hdfDTCcode.Value;
                objFailure.sDtcCode = txtDTCCode.Text;

                objFailure.SearchFailureDetails(objFailure);

                txtDtcId.Text = objFailure.sDtcId;
                txtDTCCode.Text = objFailure.sDtcCode;
                txtDTCName.Text = objFailure.sDtcName;
                //txtServiceDate.Text = objFailure.sDtcServicedate;
                txtLoadKW.Text = objFailure.sDtcLoadKw;
                txtLoadHP.Text = objFailure.sDtcLoadHp;
                txtConnectionDate.Text = objFailure.sCommissionDate;
                txtCapacity.Text = objFailure.sDtcCapacity;
                txtLocation.Text = objFailure.sDtcLocation;
                txtTcCode.Text = objFailure.sDtcTcCode;
                txtTCSlno.Text = objFailure.sDtcTcSlno;
                txtTCMake.Text = objFailure.sDtcTcMake;
                
                txtDTrCommDate.Text = objFailure.sDTrCommissionDate;


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
                if ((objFailure.sOilQuantity ?? "").Length > 0)
                {
                    txtQuantityOfOil.Text = objFailure.sOilQuantity;
                    txtQuantityOfOil.Enabled = false;
                }

                txtDTCCode.Enabled = false;

                if (hdfStatusFlag.Value == "5")
                {
                    Genaral.Load_Combo("SELECT MD_NAME,MD_NAME from TBLMASTERDATA WHERE MD_TYPE='C' and MD_NAME < " + txtCapacity.Text + " ORDER BY MD_ID", "--Select--", cmbCapacity);

                }
                else
                {
                    Genaral.Load_Combo("SELECT MD_NAME,MD_NAME from TBLMASTERDATA WHERE MD_TYPE='C' and MD_NAME > " + txtCapacity.Text + " ORDER BY MD_ID", "--Select--", cmbCapacity);

                }


                if (txtDTCName.Text.Trim() == "")
                {
                    EmptyDTCDetails();
                }





            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdSearch_Click");
            }



        }

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {

                if (Request.QueryString["DTCId"] == null && Request.QueryString["DTCId"].ToString() == "")
                {
                    txtDTCCode.Text = string.Empty;
                    txtDTCName.Text = string.Empty;
                    txtServiceDate.Text = string.Empty;
                    txtLoadKW.Text = string.Empty;
                    txtLoadHP.Text = string.Empty;
                    txtConnectionDate.Text = string.Empty;
                    txtCapacity.Text = string.Empty;
                    txtLocation.Text = string.Empty;
                    txtTCSlno.Text = string.Empty;
                    txtTcCode.Text = string.Empty;
                    txtTCMake.Text = string.Empty;
                    txtEnhanceDate.Text = string.Empty;
                    txtReason.Text = string.Empty;
                    txtDTCRead.Text = string.Empty;
                    txtDtcId.Text = string.Empty;
                    txtEnhancementId.Text = string.Empty;
                    cmdSave.Text = "Save";
                    cmdSearch.Visible = true;
                    txtDTCCode.Enabled = true;
                    
                }
                else
                {
                    txtReason.Text = string.Empty;
                    txtDTCRead.Text = string.Empty;
                    cmbCapacity.SelectedIndex = 0;
                }
                if (txtTankcapacity.Enabled == true || txtQuantityOfOil.Enabled == true)
                {
                    txtTankcapacity.Text = string.Empty;
                    txtQuantityOfOil.Text = string.Empty;
                }
                if(txtLatitude.Enabled == true)
                {
                    txtLatitude.Text = string.Empty;
                    txtLongitude.Text = string.Empty;
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdReset_Click");
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
                else if (hdfStatusFlag.Value == "2")
                {
                    Response.Redirect("EnhancementView.aspx", false);
                }
                else
                {
                    Response.Redirect("ReductionView.aspx", false);
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdClose_Click");
            }

        }

        public void ValidateFormUpdate()
        {
            try
            {
                clsEnhancement objEnhancement = new clsEnhancement();
                if (objEnhancement.ValidateEnhancementUpdate(txtEnhancementId.Text) == true)
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
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ValidateFormUpdate");
            }
        }


        public bool ValidateForm()
        {
            bool bValidate = false;
            try
            {

                if (hdfStatusFlag.Value == "2")
                {
                    if (cmbCapacity.SelectedIndex == 0)
                    {
                        cmbCapacity.Focus();
                        ShowMsgBox("Select Enhanced Capacity");
                        return bValidate;
                    }

                    if (txtReason.Text.Trim() == "")
                    {
                        txtReason.Focus();
                        ShowMsgBox("Enter the Enhancement Reason");
                        return bValidate;
                    }
                }
                else
                {
                    if (cmbCapacity.SelectedIndex == 0)
                    {
                        cmbCapacity.Focus();
                        ShowMsgBox("Select Reduction Capacity");
                        return bValidate;
                    }

                    if (txtReason.Text.Trim() == "")
                    {
                        txtReason.Focus();
                        ShowMsgBox("Enter the Reduction Reason");
                        return bValidate;
                    }
                }
                if (txtDTrCommDate.Text.Trim() == "")
                {
                    txtDTrCommDate.Focus();
                    ShowMsgBox("Enter the DTR Commission Date");
                    return bValidate;
                }
                if (txtConnectionDate.Text.Trim() == "")
                {
                    txtConnectionDate.Focus();
                    ShowMsgBox("Enter the DTC  Commission Date");
                    return bValidate;
                }


                // DTR Commission Date should ge greater than DTC Commission Date
                string sResult = Genaral.DateComparision(txtDTrCommDate.Text, txtConnectionDate.Text, false, true);
                if (sResult == "2")
                {
                    ShowMsgBox("DTR Commission Date should ge greater than DTC Commission Date");
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
                bool Existsoil = Regex.IsMatch(txtTankcapacity.Text, @"\d");
                if (Existsoil == true)
                {

                    if (Convert.ToDouble(txtQuantityOfOil.Text.Trim('.')) <= 0)
                    {
                        txtQuantityOfOil.Focus();
                        ShowMsgBox("Enter Valid Total Oil Quantity(in Liter)");
                        return false;
                    }
                }
                else
                {
                    txtQuantityOfOil.Focus();
                    ShowMsgBox("Enter Total Oil Quantity(in Liter)");
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
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ValidateForm");
                return bValidate;
            }
        }

        public void EmptyDTCDetails()
        {
            try
            {
                txtDTCName.Text = string.Empty;
                txtServiceDate.Text = string.Empty;
                txtLoadKW.Text = string.Empty;
                txtLoadHP.Text = string.Empty;
                txtConnectionDate.Text = string.Empty;
                txtCapacity.Text = string.Empty;
                txtLocation.Text = string.Empty;
                txtTcCode.Text = string.Empty;
                txtTCMake.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "EnableDisableControls");
            }
        }

        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "Enhancement";
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

        public void WorkFlowObjects(clsEnhancement objEnhance)
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


                objEnhance.sFormName = "Enhancement";
                objEnhance.sOfficeCode = objSession.OfficeCode;
                objEnhance.sClientIP = sClientIP;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "WorkFlowObjects");
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
                    cmdSave.Text = "Approve";
                    pnlApproval.Enabled = false;
                }
                if (txtActiontype.Text == "R")
                {
                    cmdSave.Text = "Reject";
                    pnlApproval.Enabled = false;
                }
                if (txtActiontype.Text == "M")
                {
                    cmdSave.Text = "Modify and Approve";
                    pnlApproval.Enabled = true;
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
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SetControlText");
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
                        //   ShowMsgBox("Approved Successfully");
                        txtEnhancementId.Text = objApproval.sNewRecordId;
                        cmdSave.Enabled = false;

                        if (objSession.RoleId == "1")
                        {
                            clsFailureEntry objFailure = new clsFailureEntry();
                            //objFailure.SendSMStoSectionOfficer(hdfEnhanceOffcode.Value, txtDTCCode.Text);
                            string result = EstimationReport();
                            if (result != "SUCCESS")
                            {
                                ShowMsgBox("OOPS Estimation report cannot be generated please contact support team ");
                            }
                            else
                            {
                                ShowMsgBox("Approved Successfully");
                            }
                        }
                        if (objSession.RoleId == "4")
                        {
                            clsEnhancement objEnhancen = new clsEnhancement();
                            string sWoid = objEnhancen.getWoIDforEstimation(objSession.OfficeCode, txtDTCCode.Text);
                            EstimationReportSO(sWoid);
                        }
                    }
                    else if (objApproval.sApproveStatus == "3")
                    {
                        ShowMsgBox("Rejected Successfully");
                        cmdSave.Enabled = false;
                    }
                    else if (objApproval.sApproveStatus == "2")
                    {
                        // ShowMsgBox("Modified and Approved Successfully");
                        txtEnhancementId.Text = objApproval.sNewRecordId;
                        cmdSave.Enabled = false;

                        if (objSession.RoleId == "1")
                        {
                            clsFailureEntry objFailure = new clsFailureEntry();
                            //objFailure.SendSMStoSectionOfficer(hdfEnhanceOffcode.Value, txtDTCCode.Text);
                            string result = EstimationReport();
                            if (result != "SUCCESS")
                            {
                                ShowMsgBox("OOPS Estimation report cannot be generated please contact support team ");
                            }
                            else
                            {
                                ShowMsgBox("Modified and Approved Successfully");
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdApprove_Click");
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
                        hdfWFDataId.Value = Session["WFDataId"].ToString();
                        hdfWFOId.Value = Convert.ToString(Session["WFOId"]);
                        hdfApproveStatus.Value = Convert.ToString(Session["ApproveStatus"]);
                        hdfWFOAutoId.Value = Convert.ToString(Session["WFOAutoId"]);

                        Session["WFDataId"] = null;
                        Session["WFOId"] = null;
                        Session["ApproveStatus"] = null;
                        Session["WFOAutoId"] = null;
                    }

                    GetEnhancementDetailsFromXML(hdfWFDataId.Value);
                    SetControlText();
                    ControlEnableDisable();
                    if (txtActiontype.Text == "V")
                    {
                        cmdSave.Text = "View";
                        //cmdSave.Enabled = false;
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
                }

                DisableControlForView();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "WorkFlowConfig");
            }
        }

        public bool CheckFormCreatorLevel()
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                string sResult = objApproval.GetFormCreatorLevel("", objSession.RoleId, "Enhancement");
                if (sResult == "1")
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetFormCreatorLevel");
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
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "DisableControlForView");
            }
        }
        #endregion

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
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ControlEnableDisable");
            }
        }

        public void LoadSearchWindow()
        {
            try
            {


                string strQry = string.Empty;
                strQry += "Title=Search and Select DTC Details&";
                strQry += "Query=select DT_CODE,DT_NAME FROM TBLDTCMAST WHERE DT_OM_SLNO LIKE '" + objSession.OfficeCode + "%' AND ";
                strQry += " DT_CODE NOT IN (SELECT DF_DTC_CODE FROM TBLDTCFAILURE WHERE  DF_REPLACE_FLAG = 0) AND {0} like %{1}% order by DT_CODE&";
                strQry += "DBColName=DT_CODE~DT_NAME&";
                strQry += "ColDisplayName=DTC Code~DTC Name&";

                strQry = strQry.Replace("'", @"\'");

                cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry
                    + "tb=" + txtDTCCode.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + txtDTCCode.ClientID + ")");

                txtEnhanceDate.Attributes.Add("onblur", "return ValidateDate(" + txtEnhanceDate.ClientID + ");");
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadSearchWindow");
            }
        }

        #region Load From XML
        public void GetEnhancementDetailsFromXML(string sWFDataId)
        {
            try
            {
                if (!txtEnhancementId.Text.Contains("-"))
                {
                    return;
                }

                clsEnhancement objEnhancement = new clsEnhancement();

                DataTable dtDetails = new DataTable();
                objEnhancement.sWFDataId = sWFDataId;

                objEnhancement.GetEnhancementDetailsFromXML(objEnhancement);

                txtDtcId.Text = objEnhancement.sDtcId;
                txtDTCCode.Text = objEnhancement.sDtcCode;
                txtDTCName.Text = objEnhancement.sDtcName;
                txtServiceDate.Text = objEnhancement.sServicedate;
                txtLoadKW.Text = objEnhancement.sLoadKw;
                txtLoadHP.Text = objEnhancement.sLoadHp;
                txtConnectionDate.Text = objEnhancement.sCommissionDate;
                txtCapacity.Text = objEnhancement.sCapacity;
                txtLocation.Text = objEnhancement.sLocation;
                txtTcCode.Text = objEnhancement.sTcCode;
                txtTCSlno.Text = objEnhancement.sTcSlno;
                txtTCMake.Text = objEnhancement.sTcMake;
                txtEnhanceDate.Text = objEnhancement.sEnhancementDate;
                txtReason.Text = objEnhancement.sReason;
                txtDTCRead.Text = objEnhancement.sDtcReadings;
                txtDTCCode.Enabled = false;
                hdfEnhanceOffcode.Value = objEnhancement.sOfficeCode;
                hdfCrBy.Value = objEnhancement.sCrby;
                txtDTrCommDate.Text = objEnhancement.sDtrcommdate;

                if (objEnhancement.sEnhancementId != "0")
                {
                    //cmdSave.Text = "Update";
                    cmdSearch.Visible = false;
                }


                if (objEnhancement.sEnhancedCapacity != "")
                {
                    cmbCapacity.SelectedValue = objEnhancement.sEnhancedCapacity;
                }
                if (objEnhancement.Latitude.Length > 3 && objEnhancement.Longitude.Length > 3)
                {
                    txtLatitude.Text = objEnhancement.Latitude;
                    txtLongitude.Text = objEnhancement.Longitude;
                    txtLatitude.Enabled = false;
                    txtLongitude.Enabled = false;
                }


                if (objEnhancement.Tankcapacity.Length > 0)
                {
                    txtTankcapacity.Text = objEnhancement.Tankcapacity;
                    txtTankcapacity.Enabled = false;
                }
                if (objEnhancement.Totaloilquantity.Length > 0)
                {
                    txtQuantityOfOil.Text = objEnhancement.Totaloilquantity;
                    txtQuantityOfOil.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetEnhancementDetailsFromXML");
            }

        }
        #endregion


        public string EstimationReport()
        {
            try
            {
                if (txtEnhancementId.Text.Contains("-"))
                {
                    return "SUCCESS";
                }
                //if (cmdSave.Text == "Save")
                //{
                clsEstimation objEst = new clsEstimation();
                objEst.sOfficeCode = hdfEnhanceOffcode.Value;
                objEst.sFailureId = txtEnhancementId.Text;
                //objEst.sLastRepair = txtLastRepairer.Text;

                string status = objEst.SaveEstimationDetails(objEst);
                if (status != "SUCCESS")
                {
                    return "FAILURE";
                }
                //}

                string strParam = string.Empty;
                strParam = "id=EnhanceEstimation&EnhanceId=" + txtEnhancementId.Text;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                return "SUCCESS";
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "EstimationReport");
                return "FAILURE";
            }
        }


        protected void lnkDTrDetails_Click(object sender, EventArgs e)
        {
            try
            {
                string sTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(hdfTCId.Value));

                string url = "/MasterForms/TcMaster.aspx?TCId=" + sTCId;
                //string s = "window.open('" + url + "', 'popup_window', 'width=300,height=100,left=100,top=100,resizable=yes');";
                string s = "window.open('" + url + "', '_blank');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "lnkDTrDetails_Click");
            }
        }

        protected void lnkDTCDetails_Click(object sender, EventArgs e)
        {
            try
            {
                string sDTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtDtcId.Text));

                string url = "/MasterForms/DTCCommision.aspx?QryDtcId=" + sDTCId;
                string s = "window.open('" + url + "', '_blank');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "lnkDTCDetails_Click");
            }
        }

        #region FunForEstimationSO

        public void EstimationReportSO(string sWoID)
        {
            try
            {


                string STCcode = txtTcCode.Text;
                string strParam = string.Empty;
                strParam = "id=EnhanceEstimationSO&TCcode=" + STCcode + "&WOId=" + sWoID;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "EstimationReport");
            }
        }

        #endregion


    }
}