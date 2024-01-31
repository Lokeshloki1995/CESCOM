using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Text.RegularExpressions;

namespace IIITS.DTLMS.MasterForms
{
    public partial class TcMaster : System.Web.UI.Page
    {
        string strFormCode = "TcMaster";
        clsTcMaster objTcMaster = new clsTcMaster();
        clsSession objSession;
        string sTcXmlData = string.Empty;
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
                txtManufactureDate.Attributes.Add("readonly", "readonly");
                txtLastServiceDate.Attributes.Add("readonly", "readonly");
                txtPurchaseDate.Attributes.Add("readonly", "readonly");
                if (!IsPostBack)
                {

                    ManufactureCalender.EndDate = System.DateTime.Now;
                    PurchaseCalender.EndDate = System.DateTime.Now;

                    if (Request.QueryString["TCId"] != null && Request.QueryString["TCId"].ToString() != "")
                    {
                        txtTcID.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["TCId"]));
                        Genaral.Load_Combo("SELECT MD_ID,MD_NAME from TBLMASTERDATA WHERE MD_TYPE='TCL'  ", "-Select-", cmbTcLocation);
                    }

                    LoadComboField();

                    if (txtTcID.Text != "")
                    {

                        Genaral.Load_Combo("SELECT MD_ID,MD_NAME from TBLMASTERDATA WHERE MD_TYPE='TCL'  ", "-Select-", cmbTcLocation);
                        GetTcMAsterDeatils(txtTcID.Text);
                        hdfTcCode.Value = txtTcCode.Text;
                        Genaral.Load_Combo("SELECT MD_ID, MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'SRT'", "--Select--", cmbStarRated);
                        cmbStarRated.SelectedValue = hdfStarRate.Value;
                        //if (cmbRating.SelectedValue == "1")
                        //{
                        //    cmbRating_SelectedIndexChanged(sender, e);
                        //    cmbStarRated.SelectedValue = hdfStarRate.Value;
                        //}



                        objTcMaster.sTcId = Convert.ToString(txtTcID.Text);
                        objTcMaster.GetTCDetails(objTcMaster);
                        // added by santhosh
                        if (objTcMaster.validaingFailurEntry == "Desable")
                        {
                            cmbStarRated.Enabled = false;
                        }
                        else
                        {
                            cmbStarRated.Enabled = true;
                        }
                        //-- end--

                        sTcXmlData = objTcMaster.SaveXmlData(objTcMaster);
                        ViewState["sTcXmlData"] = sTcXmlData;

                        cmbTcLocation_SelectedIndexChanged(sender, e);
                    }
                    txtPurchaseDate.Attributes.Add("onblur", "return ValidateDate(" + txtPurchaseDate.ClientID + ");");
                    txtManufactureDate.Attributes.Add("onblur", "return ValidateDate(" + txtManufactureDate.ClientID + ");");
                    txtLastServiceDate.Attributes.Add("onblur", "return ValidateDate(" + txtLastServiceDate.ClientID + ");");
                }
                string strQry = string.Empty;
                strQry = "Title=Search and Select Dtc Details&";
                strQry += "Query=select DT_CODE,DT_NAME FROM TBLDTCMAST where DT_OM_SLNO like '" + objSession.OfficeCode + "%' and  {0} like %{1}% order by DT_NAME&";
                strQry += "DBColName=DT_CODE~DT_NAME&";
                strQry += "ColDisplayName=DTC Code~DTC Name&";
                strQry = strQry.Replace("'", @"\'");
                btnDtcSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtDtcCode.ClientID + "&btn=" + btnDtcSearch.ClientID + "',520,520," + txtDtcCode.ClientID + ")");

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, " Page_Load");
            }

        }

        public void LoadComboField()
        {
            try
            {
                string strQry = string.Empty;


                strQry = "SELECT TM_ID,TM_NAME FROM TBLTRANSMAKES ORDER BY TM_NAME";

                Genaral.Load_Combo("SELECT MD_ID,MD_NAME from TBLMASTERDATA WHERE MD_TYPE='TCL' and MD_ID <> '2' ", "-Select-", cmbTcLocation);
                Genaral.Load_Combo(strQry, "-Select-", cmbMake);
                Genaral.Load_Combo("SELECT MD_NAME,MD_NAME from TBLMASTERDATA WHERE MD_TYPE='C' ORDER BY MD_ID", "-Select-", cmbCapacity);
                Genaral.Load_Combo("select TS_ID,TS_NAME  FROM TBLTRANSSUPPLIER WHERE TS_STATUS='A' ORDER BY TS_NAME", "-Select-", cmbSupplier);
                //   Genaral.Load_Combo("SELECT MD_ID, MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'SR'", "--Select--", cmbRating);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadComboField");
            }
        }

        public void GetTcMAsterDeatils(string strId)
        {
            try
            {
                clsTcMaster objTcMaster = new clsTcMaster();
                objTcMaster.sTcId = Convert.ToString(strId);
                objTcMaster.GetTCDetails(objTcMaster);

                txtTcID.Text = objTcMaster.sTcId;
                txtTcCode.Text = objTcMaster.sTcCode;
                txtSerialNo.Text = objTcMaster.sTcSlNo;
                cmbMake.SelectedValue = objTcMaster.sTcMakeId;
                cmbCapacity.SelectedValue = objTcMaster.sTcCapacity;
                txtManufactureDate.Text = objTcMaster.sManufacDate;
                txtPurchaseDate.Text = objTcMaster.sPurchaseDate;
                txtTcLifeSpan.Text = objTcMaster.sTcLifeSpan;
                cmbSupplier.SelectedValue = objTcMaster.sSupplierId;
                txtPoNo.Text = objTcMaster.sPoNo;
                txtPrice.Text = objTcMaster.sPrice;
                txtWarrentyPeriod.Text = objTcMaster.sWarrentyPeriod;
                txtLastServiceDate.Text = objTcMaster.sLastServiceDate;
                cmbTcLocation.SelectedValue = objTcMaster.sCurrentLocation;
                txtTcCode.Enabled = false;
                cmbCapacity.Enabled = false;

                //   cmbRating.SelectedValue = objTcMaster.sRating;
                hdfStarRate.Value = objTcMaster.sStarRate;

                txtWeight.Text = objTcMaster.sWeight;
                txtOilCapacity.Text = objTcMaster.sOilCapacity;
                txtTankcapacity.Text = objTcMaster.Tankcapacity;
                txtTCstatus.Text = objTcMaster.sTcStatus;

                if (cmbTcLocation.SelectedValue == "2")
                {
                    cmbTcLocation.Enabled = false;
                    txtDtcCode.Enabled = false;
                }

                cmdSave.Text = "Update";
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetTcMAsterDeatils");
            }

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
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowMsgBox");
            }
        }

        bool ValidateForm()
        {
            bool bValidate = false;

            if (txtTcCode.Text.Trim().Length == 0)
            {
                txtTcCode.Focus();
                ShowMsgBox("Enter TC Code");
                return false;
            }

            if (cmbMake.SelectedIndex == 0)
            {
                cmbMake.Focus();
                ShowMsgBox("Please Select the TC Make");
                return false;
            }
            if (cmbCapacity.SelectedIndex == 0)
            {
                cmbCapacity.Focus();
                ShowMsgBox("Select TC Capacity");
                return false;
            }
            if (txtTcLifeSpan.Text.Trim().Length > 0)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtTcLifeSpan.Text, "^(\\d{1,6})?(\\.\\d{1,2})?$"))
                {
                    ShowMsgBox("Enter valid Life Span");
                    txtTcLifeSpan.Focus();
                    return false;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(txtTcLifeSpan.Text, "[-+]?[0-9]{0,5}\\.?[0-9]{0,2}"))
                {
                    ShowMsgBox("Enter valid Life Span");
                    txtTcLifeSpan.Focus();
                    return false;
                }
                if (txtTcLifeSpan.Text.Contains("-"))
                {
                    ShowMsgBox("Enter valid Life Span");
                    txtTcLifeSpan.Focus();
                    return false;
                }
            }



            if (txtTcLifeSpan.Text != "")
            {
                if (txtTcLifeSpan.Text.Contains('%') ||
                    txtTcLifeSpan.Text.Contains('@') ||
                    txtTcLifeSpan.Text.Contains('!') ||
                    txtTcLifeSpan.Text.Contains('#') ||
                    txtTcLifeSpan.Text.Contains('&') ||
                    txtTcLifeSpan.Text.Contains('$') ||
                    txtTcLifeSpan.Text.Contains('*') ||
                    txtTcLifeSpan.Text.Contains('/') ||
                    txtTcLifeSpan.Text.Contains(',') ||
                    txtTcLifeSpan.Text.Contains('-') ||
                    txtTcLifeSpan.Text.Contains('^'))
                {
                    txtTcLifeSpan.Focus();
                    ShowMsgBox("Enter Valid  Life Span");
                    return false;
                }
            }
            if (txtTankcapacity.Text.Length == 0)
            {
                txtTankcapacity.Focus();
                ShowMsgBox("Enter Tank Capacity");
                return false;
            }
            if (txtTankcapacity.Text != "")
            {
                if (txtTankcapacity.Text.Contains('%') ||
                    txtTankcapacity.Text.Contains('@') ||
                    txtTankcapacity.Text.Contains('!') ||
                    txtTankcapacity.Text.Contains('#') ||
                    txtTankcapacity.Text.Contains('&') ||
                    txtTankcapacity.Text.Contains('$') ||
                    txtTankcapacity.Text.Contains('*') ||
                   txtTankcapacity.Text.Contains('?') ||
                   txtTankcapacity.Text.Contains(',') ||
                   txtTankcapacity.Text.Contains('/') ||
                    txtTankcapacity.Text.Contains('=') ||
                    txtTankcapacity.Text.Contains('^') ||
                     txtTankcapacity.Text.Contains('-'))
                {
                    txtTankcapacity.Focus();
                    ShowMsgBox("Enter Valid Tankcapacity");
                    return false;
                }
            }


            if (txtOilCapacity.Text != "")
            {
                if (txtOilCapacity.Text.Contains('%') ||
                    txtOilCapacity.Text.Contains('@') ||
                    txtOilCapacity.Text.Contains('!') ||
                    txtOilCapacity.Text.Contains('#') ||
                    txtOilCapacity.Text.Contains('&') ||
                    txtOilCapacity.Text.Contains('$') ||
                    txtOilCapacity.Text.Contains('*') ||
                    txtOilCapacity.Text.Contains('/') ||
                    txtOilCapacity.Text.Contains('?') ||
                   txtOilCapacity.Text.Contains(',') ||
                   txtOilCapacity.Text.Contains('/') ||
                   txtOilCapacity.Text.Contains('='))

                {
                    txtOilCapacity.Focus();
                    ShowMsgBox("Enter Valid Oil Capacity");
                    return false;
                }
            }


            bool Exists = Regex.IsMatch(txtTankcapacity.Text, @"\d");
            if (Exists == true)
            {
                if (Convert.ToDouble(txtTankcapacity.Text) <= 0)
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

            //  if (!System.Text.RegularExpressions.Regex.IsMatch(txtTankcapacity.Text, "^[0-9][0-9]{1,4}*[.]?[0-9]{1,2}$"))
            // if (!System.Text.RegularExpressions.Regex.IsMatch(txtTankcapacity.Text, "^(\\d{1,4})?(\\.\\d{1,2})?$"))
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtTankcapacity.Text, "^(\\d{1,4})?(\\.\\d{1,2})?$"))
            {
                ShowMsgBox("Please Enter Valid Tank Capacity(in Liter) (eg:1111.00)");
                return bValidate;
            }
            if (txtOilCapacity.Text.Length == 0)
            {
                txtOilCapacity.Focus();
                ShowMsgBox("Enter Total Oil Quantity");
                return false;
            }
            bool ExistsOil = Regex.IsMatch(txtOilCapacity.Text, @"\d");
            if (ExistsOil == true)
            {
                if (Convert.ToDouble(txtOilCapacity.Text.Trim('.')) <= 0)
                {
                    txtOilCapacity.Focus();
                    ShowMsgBox("Enter Total Oil Quantity(in Liter)");
                    return false;
                }
            }
            else
            {
                txtOilCapacity.Focus();
                ShowMsgBox("Enter Total Oil Quantity(in Liter)");
                return false;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtOilCapacity.Text, "^(\\d{1,4})?(\\.\\d{1,2})?$"))
            {
                ShowMsgBox("Please Enter Valid Oil Quantity(in Liter) (eg:1111.00)");
                return bValidate;
            }
            if (!(Convert.ToDouble(txtOilCapacity.Text) <= Convert.ToDouble(txtTankcapacity.Text)))
            {
                txtOilCapacity.Focus();
                ShowMsgBox("Total Oil Quantity Should be less than or equals to Tank Capacity");
                return false;
            }

            if (txtWeight.Text.Length == 0)
            {
                txtWeight.Focus();
                ShowMsgBox("Enter Weight of DTr");
                return false;
            }

            if (txtWeight.Text != "")
            {
                if (txtWeight.Text.Contains('%') ||
                    txtWeight.Text.Contains('@') ||
                    txtWeight.Text.Contains('!') ||
                    txtWeight.Text.Contains('#') ||
                    txtWeight.Text.Contains('&') ||
                    txtWeight.Text.Contains('$') ||
                    txtWeight.Text.Contains('*') ||
                    txtWeight.Text.Contains('/') ||
                    txtWeight.Text.Contains('?') ||
                   txtWeight.Text.Contains(',') ||
                   txtWeight.Text.Contains('/') ||
                   txtWeight.Text.Contains('='))

                {
                    txtWeight.Focus();
                    ShowMsgBox("Enter Valid Weight of DTr");
                    return false;
                }
            }




            if (txtPrice.Text.Trim() != "")
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtPrice.Text, "^(\\d{1,6})?(\\.\\d{1,2})?$"))
                {
                    txtPrice.Focus();
                    ShowMsgBox("Enter valid price (eg:111111.00)");
                    return false;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(txtPrice.Text, "[-+]?[0-9]{0,5}\\.?[0-9]{0,2}"))
                {
                    txtPrice.Focus();
                    ShowMsgBox("Enter valid price (eg:111111.00)");
                    return false;
                }
            }



            if (txtPrice.Text != "")
            {
                if (txtPrice.Text.Contains('%') ||
                    txtPrice.Text.Contains('@') ||
                    txtPrice.Text.Contains('!') ||
                    txtPrice.Text.Contains('#') ||
                    txtPrice.Text.Contains('&') ||
                    txtPrice.Text.Contains('$') ||
                     txtPrice.Text.Contains('*') ||
                     txtPrice.Text.Contains('/') ||
                      txtPrice.Text.Contains('-') ||
                      txtPrice.Text.Contains('*') ||
                      txtPrice.Text.Contains('^'))
                {
                    txtPrice.Focus();
                    ShowMsgBox("Enter Valid Price");
                    return false;
                }
            }


            if (txtWarrentyPeriod.Text != "")
            {
                if (txtWarrentyPeriod.Text.Contains('%') ||
                    txtWarrentyPeriod.Text.Contains('@') ||
                    txtWarrentyPeriod.Text.Contains('!') ||
                    txtWarrentyPeriod.Text.Contains('#') ||
                    txtWarrentyPeriod.Text.Contains('&') ||
                    txtWarrentyPeriod.Text.Contains('$') ||
                    txtWarrentyPeriod.Text.Contains('*') ||
                    txtWarrentyPeriod.Text.Contains('/') ||
                    txtWarrentyPeriod.Text.Contains('?') ||
                   txtWarrentyPeriod.Text.Contains(',') ||
                   txtWarrentyPeriod.Text.Contains('/') ||
                   txtWarrentyPeriod.Text.Contains('='))

                {
                    txtWarrentyPeriod.Focus();
                    ShowMsgBox("Enter Valid Warrenty Period");
                    return false;
                }
            }



            if (txtLastServiceDate.Text.Trim() != "")
            {
                string sResult = Genaral.DateComparision(txtLastServiceDate.Text, "", true, false);
                if (sResult == "1")
                {
                    txtLastServiceDate.Focus();
                    ShowMsgBox("Last Service Date should be Less than Current Date");
                    return bValidate;
                }
                sResult = Genaral.DateComparision(txtLastServiceDate.Text, txtManufactureDate.Text, false, false);
                if (sResult == "2")
                {
                    ShowMsgBox("Last Service Date should be Greater than Manufacturing Date");
                    return bValidate;
                }
            }
            if (cmbTcLocation.SelectedIndex == 0)
            {
                cmbTcLocation.Focus();
                ShowMsgBox("Select TC Current Location");
                return false;
            }
            if (cmbTcLocation.SelectedValue == "2")
            {
                if (txtDtcCode.Text.Trim() == "")
                {
                    txtDtcCode.Focus();
                    ShowMsgBox("Enter DTC Code ");
                    return false;
                }
            }

            bValidate = true;
            return bValidate;
        }
        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                clsTcMaster objTcMaster = new clsTcMaster();
                string[] Arr = new string[2];

                if (txtManufactureDate.Text != "" && txtPurchaseDate.Text != "")
                {
                    string sResult = string.Empty;
                    sResult = Genaral.DateComparision(txtPurchaseDate.Text, txtManufactureDate.Text, false, false);
                    if (sResult == "2")
                    {
                        ShowMsgBox("Purchasing date should be greater than manufacturing Date");
                        return;
                    }
                }

                //Check AccessRights
                bool bAccResult;
                if (cmdSave.Text == "Update")
                {
                    bAccResult = CheckAccessRights("3");
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
                    objTcMaster.sTcId = txtTcID.Text;
                    objTcMaster.sTcSlNo = txtSerialNo.Text;
                    if (cmbMake.SelectedIndex > 0)
                    {
                        objTcMaster.sTcMakeId = cmbMake.SelectedValue;
                    }
                    objTcMaster.sTcCode = txtTcCode.Text;
                    objTcMaster.sTcCapacity = cmbCapacity.SelectedValue;
                    objTcMaster.sManufacDate = txtManufactureDate.Text;
                    objTcMaster.sPurchaseDate = txtPurchaseDate.Text;
                    if (cmbSupplier.SelectedIndex > 0)
                    {
                        objTcMaster.sSupplierId = cmbSupplier.SelectedValue;
                    }
                    objTcMaster.sPoNo = txtPoNo.Text;
                    objTcMaster.sPrice = txtPrice.Text;
                    objTcMaster.sWarrentyPeriod = txtWarrentyPeriod.Text;
                    objTcMaster.sLastServiceDate = txtLastServiceDate.Text;
                    objTcMaster.sCurrentLocation = cmbTcLocation.SelectedValue;
                    objTcMaster.sTcLifeSpan = txtTcLifeSpan.Text;
                    objTcMaster.sCrBy = objSession.UserId;
                    objTcMaster.sOfficeCode = objSession.OfficeCode;
                    if (cmbStarRated.SelectedIndex > 0)
                    {
                        objTcMaster.sStarRate = cmbStarRated.SelectedValue;
                    }
                    if (txtDtcCode.Text.Trim() != "")
                    {
                        objTcMaster.sDtcCodes = txtDtcCode.Text;
                    }


                    objTcMaster.sOilCapacity = txtOilCapacity.Text;
                    objTcMaster.Tankcapacity = txtTankcapacity.Text;
                    objTcMaster.sWeight = txtWeight.Text;


                    objTcMaster.sFormName = strFormCode;
                    objTcMaster.sDescription = "DATA MODIFIED BY " + Session["FullName"];
                    objTcMaster.sXmlData = Convert.ToString(ViewState["sTcXmlData"]);

                    Arr = objTcMaster.SaveUpdateTransformerDetails(objTcMaster);

                    if (Arr[1].ToString() == "0")
                    {
                        ShowMsgBox(Arr[0]);
                        //txtTcID.Text = objTcMaster.sTcId;
                        //cmdSave.Text = "Update";
                        //txtTcCode.Enabled = false;
                        Reset();
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "TCMASTER");
                        }
                        return;
                    }

                    if (Arr[1].ToString() == "1")
                    {



                        objTcMaster.GetTCDetails(objTcMaster);
                        sTcXmlData = objTcMaster.SaveXmlData(objTcMaster);
                        bool sResult;
                        sResult = objTcMaster.SaveWorkFlowData(objTcMaster);

                        objTcMaster.GetTCDetails(objTcMaster);
                        sTcXmlData = objTcMaster.SaveXmlData(objTcMaster);
                        //To get last data
                        ViewState["sTcXmlData"] = sTcXmlData;


                        ShowMsgBox(Arr[0]);
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "TCMASTER");
                        }
                        return;
                    }

                    if (Arr[1].ToString() == "2")
                    {
                        ShowMsgBox(Arr[0]);
                        if (objSession.sTransactionLog == "1")
                        {
                            Genaral.TransactionLog(Genaral.GetClientIp(), objSession.UserId, "TCMASTER");
                        }
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, " cmdSave_Click");
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



        public void Reset()
        {
            try
            {
                txtTcCode.Enabled = true;
                txtTcID.Text = string.Empty;
                txtTcCode.Text = string.Empty;
                txtSerialNo.Text = string.Empty;
                cmbMake.SelectedIndex = 0;
                cmbCapacity.SelectedIndex = 0;
                txtManufactureDate.Text = string.Empty;
                txtPurchaseDate.Text = string.Empty;
                cmbSupplier.SelectedIndex = 0;
                txtPoNo.Text = string.Empty;
                txtPrice.Text = string.Empty;
                txtWarrentyPeriod.Text = string.Empty;
                txtLastServiceDate.Text = string.Empty;
                cmbTcLocation.SelectedIndex = 0;
                cmdSave.Text = "Save";
                txtTcLifeSpan.Text = string.Empty;

                //  cmbRating.SelectedIndex = 0;
                cmbStarRated.Items.Clear();
                dvStar.Style.Add("display", "none");
                cmbTcLocation.Enabled = true;
                txtDtcCode.Enabled = false;
                txtOilCapacity.Text = string.Empty;
                txtTankcapacity.Text = string.Empty;
                txtWeight.Text = string.Empty;
                divRepairer.Style.Add("display", "none");
                divStore.Style.Add("display", "none");
                divField.Style.Add("display", "none");
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Reset");
            }

        }

        protected void txtTcCode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                clsTcMaster objTcMaster = new clsTcMaster();
                objTcMaster.sTcCode = txtTcCode.Text;
                bool bResult = objTcMaster.CheckTransformerCodeExist(objTcMaster);
                if (bResult)
                {
                    txtTcCode.Focus();
                    ShowMsgBox("Transformer Code Already Exist");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "txtTcCode_TextChanged");
            }
        }

        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "TcMaster";
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

        //protected void cmbRating_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (cmbRating.SelectedValue == "1")
        //        {
        //            dvStar.Style.Add("display", "block");
        //            Genaral.Load_Combo("SELECT MD_ID, MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE = 'SRT'", "--Select--", cmbStarRated);
        //        }
        //        else
        //        {
        //            dvStar.Style.Add("display", "none");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = clsException.ErrorMsg();
        //        clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbRating_SelectedIndexChanged");
        //    }
        //}

        protected void cmbTcLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtTc = new DataTable();
            clsTcMaster objTcMaster = new clsTcMaster();
            try
            {

                if (cmbTcLocation.SelectedValue == "2")
                {
                    divField.Style.Add("display", "block");

                    if (objSession.OfficeCode.Length > 2)
                    {
                        objTcMaster.srepairOffCode = objSession.OfficeCode.Substring(0, 2);
                    }
                    else
                    {
                        objTcMaster.srepairOffCode = objSession.OfficeCode;
                    }
                    objTcMaster.sTcCode = hdfTcCode.Value;
                    dtTc = objTcMaster.GetFieldDetails(objTcMaster);
                    if (dtTc.Rows.Count > 0)
                    {
                        txtDtcCode.Text = dtTc.Rows[0]["DT_CODE"].ToString();
                        txtDtcName.Text = dtTc.Rows[0]["DT_NAME"].ToString();
                        divField.Style.Add("display", "block");
                    }

                    divRepairer.Style.Add("display", "none");
                    divStore.Style.Add("display", "none");

                }

                else if (cmbTcLocation.SelectedValue == "3")
                {
                    if (objSession.OfficeCode.Length > 2)
                    {
                        objTcMaster.srepairOffCode = objSession.OfficeCode.Substring(0, 2);
                    }
                    else
                    {
                        objTcMaster.srepairOffCode = objSession.OfficeCode;
                    }
                    dtTc = objTcMaster.GetRepairerDetails(objTcMaster);
                    if (dtTc.Rows.Count > 0)
                    {
                        txtRepairerName.Text = dtTc.Rows[0]["TR_NAME"].ToString();
                        txtReAddress.Text = dtTc.Rows[0]["TR_ADDRESS"].ToString();
                        txtReMobileNo.Text = dtTc.Rows[0]["TR_MOBILE_NO"].ToString();
                        txtReEmailId.Text = dtTc.Rows[0]["TR_EMAIL"].ToString();
                        divRepairer.Style.Add("display", "block");
                    }

                    divField.Style.Add("display", "none");
                    divStore.Style.Add("display", "none");
                }
                else if (cmbTcLocation.SelectedValue == "1")
                {
                    if (objSession.OfficeCode.Length > 2)
                    {
                        objTcMaster.srepairOffCode = objSession.OfficeCode.Substring(0, 2);
                    }
                    else
                    {
                        objTcMaster.srepairOffCode = objSession.OfficeCode;
                    }
                    dtTc = objTcMaster.GetStoreDetails(objTcMaster);
                    if (dtTc.Rows.Count > 0)
                    {
                        txtStoreName.Text = dtTc.Rows[0]["SM_NAME"].ToString();
                        txtStoreincharge.Text = dtTc.Rows[0]["SM_STORE_INCHARGE"].ToString();
                        txtStoreMobile.Text = dtTc.Rows[0]["SM_MOBILENO"].ToString();
                        txtStoreAddress.Text = dtTc.Rows[0]["SM_ADDRESS"].ToString();
                        divStore.Style.Add("display", "block");
                    }
                    divField.Style.Add("display", "none");
                    divRepairer.Style.Add("display", "none");
                }
                else
                {
                    if (objSession.OfficeCode.Length > 2)
                    {
                        objTcMaster.srepairOffCode = objSession.OfficeCode.Substring(0, 2);
                    }
                    else
                    {
                        objTcMaster.srepairOffCode = objSession.OfficeCode;
                    }
                    dtTc = objTcMaster.GetStoreDetails(objTcMaster);
                    if (dtTc.Rows.Count > 0)
                    {
                        txtStoreName.Text = dtTc.Rows[0]["SM_NAME"].ToString();
                        txtStoreincharge.Text = dtTc.Rows[0]["SM_STORE_INCHARGE"].ToString();
                        txtStoreMobile.Text = dtTc.Rows[0]["SM_MOBILENO"].ToString();
                        txtStoreAddress.Text = dtTc.Rows[0]["SM_ADDRESS"].ToString();
                        divStore.Style.Add("display", "block");
                    }

                    divField.Style.Add("display", "none");
                    divRepairer.Style.Add("display", "none");

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbTcLocation_SelectedIndexChanged");
            }

        }

        protected void btnDtcSearch_Click(object sender, EventArgs e)
        {
            try
            {
                clsTcMaster objTcMaster = new clsTcMaster();
                DataTable dtTc = new DataTable();
                objTcMaster.sDtcCodes = txtDtcCode.Text;
                dtTc = objTcMaster.GetDtcDetails(objTcMaster);
                if (dtTc.Rows.Count > 0)
                {
                    txtDtcCode.Text = dtTc.Rows[0]["DT_CODE"].ToString();
                    txtDtcName.Text = dtTc.Rows[0]["DT_NAME"].ToString();
                    divField.Style.Add("display", "block");
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "btnDtcSearch_Click");
            }

        }


        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtDtcCode.Text = string.Empty;
                txtDtcName.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "btnDtcSearch_Click");
            }
        }

        protected void lnkDTRHistory_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtTcID.Text.Trim() != "")
                {
                    string sTCCode = HttpUtility.UrlEncode(Genaral.UrlEncrypt(txtTcCode.Text));
                    Response.Redirect("/Transaction/TcTracker.aspx?TCCode=" + sTCCode, false);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "lnkDTRHistory_Click");
            }
        }


    }
}