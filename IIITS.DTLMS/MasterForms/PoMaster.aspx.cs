using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;

namespace IIITS.DTLMS.MasterForms
{
    public partial class PoMaster : System.Web.UI.Page
    {
        string strFormCode = "PoMaster";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                lblMessage.Text = string.Empty;
                objSession = (clsSession)Session["clsSession"];
                Form.DefaultButton = btnSave.UniqueID;
                if (!IsPostBack)
                {

                    LoadComboField();
                    CheckAccessRights("4");

                    if (Request.QueryString["QryPoId"] != null && Request.QueryString["QryPoId"].ToString() != "")
                    {
                        txtPoId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["QryPoId"]));
                        LoadPoDetails(txtPoId.Text);
                      
                    }
                    txtDate.Attributes.Add("onblur", "return ValidateDate(" + txtDate.ClientID + ");");
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "Page_Load");
            }

        }

        public void LoadComboField()
        {
            try
            {
                string strQry = string.Empty;

                strQry = "SELECT TM_ID,TM_NAME FROM TBLTRANSMAKES WHERE (TM_STATUS='A' AND TO_CHAR(NVL(TM_EFFECT_FROM,SYSDATE),'YYYYMMDD') <= TO_CHAR(SYSDATE,'YYYYMMDD'))";
                strQry += " OR (TM_STATUS='D' AND  TO_CHAR(TM_EFFECT_FROM,'YYYYMMDD') >= TO_CHAR(SYSDATE,'YYYYMMDD')) ORDER BY TM_NAME";

                Genaral.Load_Combo(strQry, "-Select-", ddlMake);
                Genaral.Load_Combo("SELECT MD_ID,MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE='C' ORDER BY MD_ID", "--Select--", ddlCapacity);
                Genaral.Load_Combo("select TS_ID,TS_NAME  FROM TBLTRANSSUPPLIER WHERE TS_STATUS='A' ORDER BY TS_NAME", "--Select--", cmbSupplier);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadComboField");
            }
        }

        public void LoadPoDetails(string strPoId)
        {
            try
            {
                clsPoMaster objPoMaster = new clsPoMaster();
                objPoMaster.sPoId = Convert.ToString(strPoId);
                objPoMaster.GetPoDetails(objPoMaster);
               
                txtPoNumber.Text = objPoMaster.sPoNo;
                txtDate.Text = objPoMaster.sDate;
                txtRate.Text = objPoMaster.sPoRate;
                cmbSupplier.SelectedValue = objPoMaster.sSupplierId;
                txtPoNumber.Enabled = false;
                txtDate.Enabled = false;
                //cmbSupplier.Enabled = false;
                txtRate.Enabled = false;

                LoadTcCapacity(txtPoId.Text);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadIndentDetails");
            }
        }


        protected void cmdAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                //if datatble is empty,add columns to table
                if (ViewState["dt"] != null)
                {
                    dt = (DataTable)ViewState["dt"];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (ddlCapacity.SelectedItem.Text == Convert.ToString(dt.Rows[i]["PB_CAPACITY"]) && ddlMake.SelectedItem.Text == Convert.ToString(dt.Rows[i]["PB_MAKE"]))
                        {
                            //ShowMsgBox("Capacity("+ dt.Rows[i]["PB_CAPACITY"] + ")-MakeName(" + dt.Rows[i]["PB_MAKE"] + ") Combination Already Added");
                            ShowMsgBox("Capacity-MakeName Combination Already Added");
                            return;
                        }
                    }
                }

                if (ViewState["dt"] == null)
                {
                    dt.Columns.Add("PO_NO");
                    dt.Columns.Add("PB_CAPACITY");
                    dt.Columns.Add("PB_MAKE");
                    dt.Columns.Add("MAKE_ID");
                    //dt.Columns.Add("Po_Date");
                    dt.Columns.Add("PB_QUANTITY");
                }
                else
                {
                    //load datatble from viewstate
                    dt = (DataTable)ViewState["dt"];
                }
                DataRow dRow = dt.NewRow();
                int qnty = Convert.ToInt32(txtQuantity.Text);

                dRow["PB_QUANTITY"] = qnty;
                if (Convert.ToString(dRow["PB_QUANTITY"]) == "0")
                {
                    ShowMsgBox("Quantity Should Not be Zero");
                    return;
                }
                dRow["PB_CAPACITY"] = ddlCapacity.SelectedItem.Text;
                dRow["PB_MAKE"] = ddlMake.SelectedItem.Text;
                dRow["MAKE_ID"] = ddlMake.SelectedValue;
                dRow["PO_NO"] = txtPoNumber.Text;
                dt.Rows.Add(dRow);
                ViewState["dt"] = dt;
                LoadCapacity(dt);
                //btnSave.Enabled = true;
                //btnReset.Enabled = true;
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdAdd_Click");
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
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ShowMsgBox");
            }
        }

        public void LoadTcCapacity(string strIndentId)
        {
            DataTable dtTcCapacity = new DataTable();
            try
            {
                clsPoMaster objPoMaster = new clsPoMaster();
                objPoMaster.sPoId = Convert.ToString(strIndentId);
                dtTcCapacity = objPoMaster.LoadCapacityGrid(objPoMaster);
                grdPoMaster.DataSource = dtTcCapacity;
                grdPoMaster.DataBind();
                btnSave.Text = "Update";
                ViewState["dt"] = dtTcCapacity;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadTcCapacity");
            }
        }
        public void LoadCapacity(DataTable dt)
        {
            try
            {
               
                grdPoMaster.DataSource = dt;
                grdPoMaster.DataBind();
                grdPoMaster.Visible = true;
                txtQuantity.Text = string.Empty;
                ddlCapacity.SelectedIndex = 0;
                ddlMake.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadCapacity");
            }
        }


     
        protected void btnSave_Click1(object sender, EventArgs e)
        {
            try
            {
                
                //to check whether capacity and quantity are added
                if (ViewState["dt"] != null)
                {

                    //Check AccessRights
                    bool bAccResult;
                    if (btnSave.Text == "Update")
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

                    SavePoMaster();
                }
                else
                {
                    ShowMsgBox("Please Add Capacity and Quantity");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "btnSave_Click");
            }
        }


        public void SavePoMaster()
        {
            clsPoMaster objPoMaster = new clsPoMaster();
            DataTable dt;
            try
            {
                if (ValidateForm() == true)
                {
                    string[] Arr = new string[2];
                    dt = (DataTable)ViewState["dt"];
                    objPoMaster.sCrBy = objSession.UserId;
                    objPoMaster.sDate = txtDate.Text;
                    objPoMaster.sPoNo = txtPoNumber.Text;
                    objPoMaster.sSupplierId = cmbSupplier.SelectedValue;
                    objPoMaster.sPoRate = txtRate.Text;
                    objPoMaster.sPoId = txtPoId.Text;

                    objPoMaster.ddtCapacityGrid = dt;
                    Arr = objPoMaster.SavePoMaster(objPoMaster);
                    if (Arr[1].ToString() == "0")
                    {
                        txtPoId.Text = objPoMaster.sPoId;
                        btnSave.Text = "Update";
                        ShowMsgBox(Arr[0]);
                        return;
                    }
                    else
                    {
                        ShowMsgBox(Arr[0]);
                        return;
                    }
                }

                
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "SaveStoreTransfer");
            }
        }


        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtPoId.Text = string.Empty;
                txtPoNumber.Text = string.Empty;
                ddlMake.SelectedIndex = 0;
                txtDate.Text = string.Empty;
                btnSave.Text = "Save";
                txtQuantity.Text = string.Empty;
                ddlCapacity.SelectedIndex = 0;
                txtQuantity.Text = string.Empty;
                grdPoMaster.Visible = false;
                ViewState["dt"] = null;
                lblMessage.Text = string.Empty;
                txtPoNumber.Enabled = true;
                txtDate.Enabled = true;
                txtRate.Text = string.Empty;
                cmbSupplier.SelectedIndex = 0;
                txtRate.Text = string.Empty;
                txtRate.Enabled = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "btnReset_Click");
            }

        }

        protected void cmdClose_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("PoMasterView.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdClose_Click");
            }
        }
        protected void grdPoMaster_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            clsPoMaster objPoMaster = new clsPoMaster();
            try
            {
                if (e.CommandName == "Remove")
                {
                    DataTable dt = (DataTable)ViewState["dt"];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                        Label lblCapacity = (Label)row.FindControl("lblCapacity");
                        //to remove selected Capacity from grid
                        if (lblCapacity.Text == Convert.ToString(dt.Rows[i]["PB_CAPACITY"]))
                        {
                            dt.Rows[i].Delete();
                            dt.AcceptChanges();
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        ViewState["dt"] = dt;
                    }
                    else
                    {
                        ViewState["dt"] = null;
                    }
                    LoadCapacity(dt);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "grdPoMaster_RowCommand");
            }
        }

        protected void grdPoMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtTcCapacity = new DataTable();
                grdPoMaster.PageIndex = e.NewPageIndex;
                dtTcCapacity = (DataTable)ViewState["dt"];
                LoadCapacity(dtTcCapacity);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "grdPoMaster_PageIndexChanging");
            }
        }

        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "PoMaster";
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

        bool ValidateForm()
        {
            bool bValidate = false;

            if (txtRate.Text.Trim() != "")
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtRate.Text, "^(\\d{1,12})?(\\.\\d{1,2})?$"))
                {
                    txtRate.Focus();
                    ShowMsgBox("Enter valid price (eg:111111111111.00)");
                    return false;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(txtRate.Text, "[-+]?[0-9]{0,11}\\.?[0-9]{0,2}"))
                {
                    txtRate.Focus();
                    ShowMsgBox("Enter valid price (eg:111111111111.00)");
                    return false;
                }
            }

            bValidate = true;
            return bValidate;
        }
      
    }
}