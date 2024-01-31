using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Collections;
using System.Data.OleDb;

namespace IIITS.DTLMS.MasterForms
{
    public partial class NewTcMaster : System.Web.UI.Page
    {
        string strFormCode = "NewTcMaster";     
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

                   
                    Genaral.Load_Combo("select TS_ID,TS_NAME  FROM TBLTRANSSUPPLIER WHERE TS_STATUS='A' ORDER BY TS_NAME", "-Select-", cmbSupplier);
                    

                    string strQry = string.Empty;
                    strQry = "Title=Search and Select Po Details&";
                    strQry += "Query=SELECT PO_NO,PO_ID,TS_NAME FROM TBLPOMASTER,TBLTRANSSUPPLIER WHERE PO_SUPPLIER_ID=TS_ID AND  {0} like %{1}% &";
                    strQry += "DBColName=PO_NO~TS_NAME&";
                    strQry += "ColDisplayName=PO No~Supplier Name&";
                    btnPoSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtPONo.ClientID + "&btn=" + btnPoSearch.ClientID + "',520,520," + txtPONo.ClientID + ")");

                    ManufactureCalender.EndDate = System.DateTime.Now;
                    PurchaseCalender.EndDate = System.DateTime.Now;
                    txtPurchaseDate.Attributes.Add("onblur", "return ValidateDate(" + txtPurchaseDate.ClientID + ");");
                    txtManufactureDate.Attributes.Add("onblur", "return ValidateDate(" + txtManufactureDate.ClientID + ");");
                    CheckAccessRights("2");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode,"Page_Load");
            }

         }
        public void LoadPoDetails()
        {
            try
            {
                clsTcMaster objTcMaster = new clsTcMaster();
                //objTcMaster.sPoId = hdfPOId.Value;
                //txtPOId.Text = hdfPOId.Value;
                //hdfPOId.Value = "";
                objTcMaster.sPoNo = txtPONo.Text.Trim().ToUpper();
                objTcMaster.GetPoDetails(objTcMaster);
               
                txtPOId.Text = objTcMaster.sPoId;
                txtPONo.Text = objTcMaster.sPoNo;
                txtPurchaseDate.Text = objTcMaster.sPurchaseDate;
                txtQuantity.Text = objTcMaster.sQuantity;
                cmbSupplier.SelectedValue = objTcMaster.sSupplierId;
                LoadTcGrid(txtPOId.Text);

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadPoDetails");
            }
        }

        public void LoadTcGrid(string strPoId)
        {
            DataTable dtTcDetails = new DataTable();
            try
            {
                clsTcMaster objTcMaster = new clsTcMaster();
                objTcMaster.sPoId = strPoId;
                dtTcDetails = objTcMaster.LoadTcGrid(objTcMaster);
                grdPOQuantity.DataSource = dtTcDetails;
                grdPOQuantity.DataBind();
                ViewState["TcDetailsGrid"] = dtTcDetails;
                grdPOQuantity.Visible = true;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadTcGrid");
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
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ShowMsgBox");
            }
        }

        bool ValidateForm()
        {
            bool bValidate = false;
            if (txtTcCode.Text.Trim().Length == 0)
            {
                txtTcCode.Focus();
                ShowMsgBox("Enter DTr Code");
                return false;
            }
                       
            if (txtSerialNo.Text.Trim().Length == 0)
            {
                txtSerialNo.Focus();
                ShowMsgBox("Enter DTr Serial Number");
                 return false;
            }
            if (cmbMake.SelectedIndex < 0)
            {
                cmbMake.Focus();
                ShowMsgBox("Please Select the DTr Make");              
                return false;
            }
            if (cmbCapacity.SelectedIndex < 0)
            {
                cmbCapacity.Focus();
                ShowMsgBox("Select DTr Capacity");
                return false;
            }
            if (txtManufactureDate.Text.Trim().Length == 0)
            {
                txtManufactureDate.Focus();
                ShowMsgBox("Select the Manufacture Date");
                return false;
            }
            string  sResult = Genaral.DateComparision(txtManufactureDate.Text, "", true, false);
            if (sResult == "1")
            {
                ShowMsgBox("Manufacturing Date should be Less than Current Date");
                return bValidate;
            }
            if (txtTcLifeSpan.Text.Trim().Length == 0)
            {
                txtTcLifeSpan.Focus();
                ShowMsgBox("Enter Valid Life Span");
                return false;
            }
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
            if (txtWarrentyPeriod.Text.Length == 0)
            {
                txtWarrentyPeriod.Focus();
                ShowMsgBox("Select Warrenty Period up to");
                return false;
            }

            if (txtOilCapacity.Text.Length == 0)
            {
                txtOilCapacity.Focus();
                ShowMsgBox("Enter Oil Capacity");
                return false;
            }

            if (txtWeight.Text.Length == 0)
            {
                txtWeight.Focus();
                ShowMsgBox("Enter Weight of DTr");
                return false;
            }
            if (txtOilCapacity.Text == "0")
            {
                txtOilCapacity.Focus();
                ShowMsgBox("Enter valid Oil Capacity");
                return false;
            }
            if (txtWeight.Text == "0")
            {
                txtWeight.Focus();
                ShowMsgBox("Enter valid Weight of DTr");
                return false;
            }

              DataTable dt=(DataTable)ViewState["TcDetailsGrid"];
              for (int i = 0; i < dt.Rows.Count; i++)
              {
                  if (cmbCapacity.SelectedItem.Text == Convert.ToString(dt.Rows[i]["CAPACITY"]))
                  {
                      if (cmbMake.SelectedItem.Text != Convert.ToString(dt.Rows[i]["MAKE"]))
                      {
                          ShowMsgBox("The Combination of Capacity and Make not valid");
                          return false;
                      }
                      else
                      {
                          return true;
                      }
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

                if (ValidateSave() == true)
                {
                    
                    objTcMaster.sPoId = txtPOId.Text ;
                    objTcMaster.sPoNo = txtPONo.Text;
                    objTcMaster.sPurchaseDate = txtPurchaseDate.Text;
                    objTcMaster.sSupplierId = cmbSupplier.SelectedValue;
                    //objTcMaster.sQuantity = txtQuantity.Text;
                    objTcMaster.sCrBy = objSession.UserId;
                    objTcMaster.sOfficeCode = objSession.OfficeCode;

                  

                    int i = 0;
                    string[] strQryVallist = new string[grdTCDetails.Rows.Count];
                    foreach (GridViewRow row in grdTCDetails.Rows)
                    {
                        strQryVallist[i] = ((Label)row.FindControl("lblTCCode")).Text.Trim() + "~" + ((Label)row.FindControl("lblTCSlNo")).Text.Trim() + "~" + ((Label)row.FindControl("lblMakeID")).Text.Trim()
                            + "~" + ((Label)row.FindControl("lblCapacity")).Text.Trim() + "~" + ((Label)row.FindControl("lblManfDate")).Text.Trim() + "~" + ((Label)row.FindControl("lblLifeSpan")).Text.Trim()
                            + "~" + ((Label)row.FindControl("lblWarrenty")).Text.Trim() + "~" + ((Label)row.FindControl("lblOilCapacity")).Text.Trim() + "~" + ((Label)row.FindControl("lblWeight")).Text.Trim();
                        i++;
                    }
                    objTcMaster.sQuantity = Convert.ToString(grdTCDetails.Rows.Count);
                    Arr = objTcMaster.SaveTCDetails(strQryVallist, objTcMaster);
             

                    if (Arr[1].ToString() == "0")
                    {
                        ShowMsgBox(Arr[0].ToString());
                        Reset();
                        cmdSave.Visible = false;
                        LoadTcGrid(txtPOId.Text);
                        grdTCDetails.DataSource = null;
                        grdTCDetails.DataBind();
                        ViewState["TC"] = null;
                        return;
                    }

                    if (Arr[1].ToString() == "2")
                    {
                        ShowMsgBox(Arr[0]);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, " cmdSave_Click");
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
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdReset_Click");
            }
        }

        public void Reset()
        {
            try
            {
                txtTcCode.Enabled = true;             
                txtTcCode.Text = string.Empty;
                txtSerialNo.Text = string.Empty;
                cmbMake.SelectedIndex = 0;
                cmbCapacity.SelectedIndex = 0;
                txtManufactureDate.Text = string.Empty;
                txtWarrentyPeriod.Text = string.Empty;                
                txtTcLifeSpan.Text = string.Empty;
                txtOilCapacity.Text = string.Empty;
                txtWeight.Text = string.Empty;
               
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "Reset");
            }
        }

        public void AddTCtoGrid(string sTcCode,string sTCSlno)
        {
            try
            {
                if (ValidateGridValue(sTcCode,sTCSlno) == true)
                {

                    if (ViewState["TC"] != null)
                    {
                        DataTable dtTc = (DataTable)ViewState["TC"];
                        DataRow drow;
                        if (dtTc.Rows.Count > 0)
                        {
                            bool isCount = isCountCapacity(sTcCode);
                            if (isCount)
                            {
                                drow = dtTc.NewRow();

                                drow["TC_CODE"] = txtTcCode.Text;
                                drow["TC_SLNO"] = txtSerialNo.Text;
                                drow["TM_NAME"] = cmbMake.SelectedItem.Text;
                                drow["MAKE_ID"] = cmbMake.SelectedValue;
                                drow["TC_CAPACITY"] = cmbCapacity.SelectedItem.Text;
                                drow["TC_MANF_DATE"] = txtManufactureDate.Text;
                                drow["LIFE_SPAN"] = txtTcLifeSpan.Text;
                                drow["TC_WARANTY_PERIOD"] = txtWarrentyPeriod.Text;
                                drow["TC_OIL_CAPACITY"] = txtOilCapacity.Text;
                                drow["TC_WEIGHT"] = txtWeight.Text;
                                dtTc.Rows.Add(drow);
                                grdTCDetails.DataSource = dtTc;
                                grdTCDetails.DataBind();
                                ViewState["TC"] = dtTc;
                            }
                            else
                            {
                                ShowMsgBox("You Already Allocated Requested number of transformers");
                                return;
                            }
                        }
                    }
                    else
                    {
                        DataTable dtTC = new DataTable();
                        DataRow drow;
                        bool isCount = CheckQuantityAvailable(sTcCode);
                         if (isCount)
                         {

                             //dtFaultTc.Columns.Add(new DataColumn("TC_ID"));
                             dtTC.Columns.Add(new DataColumn("TC_CODE"));
                             dtTC.Columns.Add(new DataColumn("TC_SLNO"));
                             dtTC.Columns.Add(new DataColumn("TM_NAME"));
                             dtTC.Columns.Add(new DataColumn("MAKE_ID"));
                             dtTC.Columns.Add(new DataColumn("TC_CAPACITY"));
                             dtTC.Columns.Add(new DataColumn("TC_MANF_DATE"));
                             dtTC.Columns.Add(new DataColumn("LIFE_SPAN"));
                             dtTC.Columns.Add(new DataColumn("TC_WARANTY_PERIOD"));
                             dtTC.Columns.Add(new DataColumn("TC_OIL_CAPACITY"));
                             dtTC.Columns.Add(new DataColumn("TC_WEIGHT"));

                             drow = dtTC.NewRow();

                             drow["TC_CODE"] = txtTcCode.Text;
                             drow["TC_SLNO"] = txtSerialNo.Text;
                             drow["TM_NAME"] = cmbMake.SelectedItem.Text;
                             drow["MAKE_ID"] = cmbMake.SelectedValue;
                             drow["TC_CAPACITY"] = cmbCapacity.SelectedItem.Text;
                             drow["TC_MANF_DATE"] = txtManufactureDate.Text;
                             drow["LIFE_SPAN"] = txtTcLifeSpan.Text;
                             drow["TC_WARANTY_PERIOD"] = txtWarrentyPeriod.Text;
                             drow["TC_OIL_CAPACITY"] = txtOilCapacity.Text;
                             drow["TC_WEIGHT"] = txtWeight.Text;

                             dtTC.Rows.Add(drow);
                             grdTCDetails.DataSource = dtTC;
                             grdTCDetails.DataBind();
                             ViewState["TC"] = dtTC;
                         }
                         else
                         {
                             ShowMsgBox("Check with Available/Pending Quantity");
                             return;
                         }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "AddTCtoGrid");
            }
        }
        public bool isCountCapacity(string strTcCode)
        {
            bool isCapacity = false;
            try
            {
                DataTable dtTcDetails;
                DataTable dtTcCapacityGrid;
                int Count = 0;
                dtTcDetails = (DataTable)ViewState["TC"];
                dtTcCapacityGrid = (DataTable)ViewState["TcDetailsGrid"];

                for (int i = 0; i < dtTcCapacityGrid.Rows.Count; i++)
                {
                    for (int j = 0; j < dtTcDetails.Rows.Count; j++)
                    {
                        //Taking count of number of transformers selected 
                        //if (Convert.ToString(dtTcDetails.Rows[j]["TC_CAPACITY"]) == cmbCapacity.SelectedItem.Text)
                        //{
                        //    Count++;
                        //}

                        if (Convert.ToString(dtTcCapacityGrid.Rows[i]["CAPACITY"]) == Convert.ToString(dtTcDetails.Rows[j]["TC_CAPACITY"]))
                        {
                            Count++;
                        }
                    }
                    //To check whether selected transformers doesnot exceed requested number of transformers
                    if (Convert.ToString(dtTcCapacityGrid.Rows[i]["CAPACITY"]) == cmbCapacity.SelectedItem.Text)
                    {
                        if (Convert.ToInt32(dtTcCapacityGrid.Rows[i]["PENDINGCOUNT"]) > Count)
                        {
                            isCapacity = true;
                        }

                        Count--;
                    }
                    else
                    {
                        Count = 0;
                    }
                   
                }
                return isCapacity;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "isCountCapacity");
                return isCapacity;
            }
          
        }

        public bool CheckQuantityAvailable(string strTcCode)
        {
            bool isCapacity = false;
            try
            {
              
                DataTable dtTcCapacityGrid;
                int Count = 0;
               
                dtTcCapacityGrid = (DataTable)ViewState["TcDetailsGrid"];

                for (int i = 0; i < dtTcCapacityGrid.Rows.Count; i++)
                {
                    
                    //To check whether selected transformers doesnot exceed requested number of transformers
                    if (Convert.ToString(dtTcCapacityGrid.Rows[i]["CAPACITY"]) == cmbCapacity.SelectedItem.Text)
                    {
                        if (Convert.ToInt32(dtTcCapacityGrid.Rows[i]["PENDINGCOUNT"]) > Count)
                        {
                            isCapacity = true;
                        }
                    }
                }
                return isCapacity;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "CheckQuantityAvailable");
                return isCapacity;
            }

        }

        public bool ValidateGridValue(string sTcCode,string sTCSlno)
        {
            bool bValidate = false;
            try
            {
                ArrayList objArrlist = new ArrayList();
                ArrayList objArrlistSlno = new ArrayList();

                foreach (GridViewRow row in grdTCDetails.Rows)
                {
                    objArrlist.Add(((Label)row.FindControl("lblTCCode")).Text.Trim());
                    objArrlistSlno.Add(((Label)row.FindControl("lblTCSlNo")).Text.Trim());
                }

                if (objArrlist.Contains(sTcCode))
                {
                    ShowMsgBox("Same Transformer Code Already Added");
                    DataTable dtFaultTc = (DataTable)ViewState["TC"];
                    grdTCDetails.DataSource = dtFaultTc;
                    grdTCDetails.DataBind();
                    return bValidate;
                }

                if (objArrlistSlno.Contains(sTCSlno))
                {
                    ShowMsgBox("Same Transformer Serial No Already Added");
                    DataTable dtFaultTc = (DataTable)ViewState["TC"];
                    grdTCDetails.DataSource = dtFaultTc;
                    grdTCDetails.DataBind();
                    return bValidate;
                }

                bValidate = true;
                return bValidate;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ValidGridValue");
                return bValidate;
            }
        }

        protected void cmdAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateForm() == true)
                {
                    AddTCtoGrid(txtTcCode.Text, txtSerialNo.Text);
                    Reset();
                    DataTable  dtTcDetails = (DataTable)ViewState["TC"];
                    if (dtTcDetails.Rows.Count > 0)
                    {
                        cmdSave.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdAdd_Click");
            }
        }

        public bool ValidateSave()
        {
            bool bValidate = false;
            try
            {
                if (txtPONo.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Enter the PO Number");
                    txtPONo.Focus();
                    return false;
                }
                if (txtPurchaseDate.Text.Trim() == "")
                {
                    txtPurchaseDate.Focus();
                    ShowMsgBox("Enter Purchasing Date");
                    return false;
                }
                if (txtQuantity.Text.Trim() == "")
                {
                    txtQuantity.Focus();
                    ShowMsgBox("Enter Quantity");
                    return false;
                }
                if (cmbSupplier.SelectedIndex == 0)
                {
                    ShowMsgBox("Select the Supplier");
                    cmbSupplier.Focus();
                    return false;
                }
                if (txtQuantity.Text.Trim() == "0")
                {
                    ShowMsgBox("Enter Valid Quantity");
                    txtQuantity.Focus();
                    return false;
                }

                //if (ViewState["TC"] != null)
                //{
                    
                //    DataTable dt = (DataTable)ViewState["TC"];
                //     DataTable dtTcDetails=(DataTable)ViewState["TcDetailsGrid"];
                //    {
                //        ShowMsgBox("Mentioned Quantity not Matching with added TC Quantity");
                //        return false;
                //    }
                //}

                bValidate = true;
                return bValidate;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ValidateSave");
                return bValidate;
            }
        }

        protected void grdTCDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "remove")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    int iRowIndex = row.RowIndex;

                    DataTable dt = (DataTable)ViewState["TC"];
                    dt.Rows[iRowIndex].Delete();
                    if (dt.Rows.Count == 0)
                    {
                        ViewState["TC"] = null;
                    }
                    else
                    {
                        ViewState["TC"] = dt;
                    }

                    grdTCDetails.DataSource = dt;
                    grdTCDetails.DataBind();
                    
                }
                if (e.CommandName == "editT")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    int iRowIndex = row.RowIndex;

                    Label lblTCCode = (Label)row.FindControl("lblTCCode");
                    Label lblTCSlNo = (Label)row.FindControl("lblTCSlNo");
                    Label lblMakeID = (Label)row.FindControl("lblMakeID");
                    Label lblCapacity = (Label)row.FindControl("lblCapacity");
                    Label lblManfDate = (Label)row.FindControl("lblManfDate");
                    Label lblLifeSpan = (Label)row.FindControl("lblLifeSpan");
                    Label lblWarrenty = (Label)row.FindControl("lblWarrenty");
                    Label lblServiceDate = (Label)row.FindControl("lblServiceDate");
                    Label lblOilCapacity = (Label)row.FindControl("lblOilCapacity");
                    Label lblWeight = (Label)row.FindControl("lblWeight");

                    txtTcCode.Text = lblTCCode.Text;
                    txtSerialNo.Text = lblTCSlNo.Text;
                    cmbMake.SelectedValue = lblMakeID.Text;
                    cmbCapacity.SelectedValue = lblCapacity.Text;
                    txtManufactureDate.Text = lblManfDate.Text;
                    txtTcLifeSpan.Text = lblLifeSpan.Text;
                    txtWarrentyPeriod.Text = lblWarrenty.Text;
                    txtOilCapacity.Text = lblOilCapacity.Text;
                    txtWeight.Text = lblWeight.Text;

                    DataTable dt = (DataTable)ViewState["TC"];
                    dt.Rows[iRowIndex].Delete();
                    if (dt.Rows.Count == 0)
                    {
                        ViewState["TC"] = null;
                    }
                    else
                    {
                        ViewState["TC"] = dt;
                    }

                    grdTCDetails.DataSource = dt;
                    grdTCDetails.DataBind();

                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "grdTCDetails_RowCommand");
            }
        }

        protected void grdTCDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdTCDetails.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["TC"];
                grdTCDetails.DataSource = dt;
                grdTCDetails.DataBind();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "grdTCDetails_PageIndexChanging");
            }
        }

        protected void btnPoSearch_Click(object sender, EventArgs e)
        {

            try
            {
                LoadPoDetails();
                Genaral.Load_Combo("SELECT DISTINCT TM_ID,TM_NAME FROM TBLTRANSMAKES,TBLPOOBJECTS WHERE TM_ID=PB_MAKE AND PB_PO_ID='"+ txtPOId.Text +"' ORDER BY TM_NAME", "--Select--", cmbMake);
                Genaral.Load_Combo("SELECT DISTINCT TO_CHAR(PB_CAPACITY),TO_CHAR(PB_CAPACITY) PB_CAPACITY FROM TBLPOOBJECTS WHERE PB_PO_ID='" + txtPOId.Text + "'", "-Select-", cmbCapacity);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "btnPoSearch_Click");
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

                    Response.Redirect("~/UserRestrict.aspx", false);

                }
                return bResult;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "CheckAccessRights");
                return false;

            }
        }

        #endregion

        protected void cmdResetPO_Click(object sender, EventArgs e)
        {
            try
            {
                txtPONo.Text = string.Empty;
                hdfPOId.Value = "";
                txtPurchaseDate.Text = string.Empty;
                txtQuantity.Text = string.Empty;
                cmbSupplier.SelectedIndex = 0;
                grdPOQuantity.DataSource = null;
                grdPOQuantity.DataBind();
                txtPOId.Text = string.Empty;
                grdPOQuantity.Visible = false;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdResetPO_Click");
            }
        }

        protected void grdPOQuantity_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdPOQuantity.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["TcDetailsGrid"];
                grdPOQuantity.DataSource = dt;
                grdPOQuantity.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdPOQuantity_PageIndexChanging");
            }
        }

       
    }
}