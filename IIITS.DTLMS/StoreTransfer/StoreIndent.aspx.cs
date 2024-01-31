using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;


namespace IIITS.DTLMS.StoreTransfer
{
    public partial class StoreIndent : System.Web.UI.Page
    {
        string strFormCode = "StoreIndent";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }

                ShowMsgBox("This Functionality had moved to MMS .. Please Login to MMS to create indent for Inter Unit Transaction");

                #region intigration block (woking)
                //lblMessage.Text = string.Empty;
                //objSession = (clsSession)Session["clsSession"];
                //Form.DefaultButton = cmdSave.UniqueID;
                //if (!IsPostBack)
                //{
                //     GenerateInvoiceNo();
                //     txtIndentDate.Attributes.Add("onblur", "return ValidateDate(" + txtIndentDate.ClientID + ");");
                //     Genaral.Load_Combo("SELECT MD_ID,MD_NAME FROM TBLMASTERDATA WHERE MD_TYPE='C' ORDER BY MD_ID", "--Select--", ddlCapacity);

                //    //loading store dropdown except current logged in User store Name 
                //     string sOfficeCode = string.Empty;
                //     if (objSession.OfficeCode.Length > 1)
                //     {
                //         sOfficeCode = objSession.OfficeCode.Substring(0, 2);
                //     }
                //     Genaral.Load_Combo("SELECT SM_ID,SM_NAME FROM TBLSTOREMAST where SM_OFF_CODE<>'" + sOfficeCode  + "' order by SM_NAME", "--Select--", ddlStore);
                    
                //    if (Request.QueryString["QryIndentId"] != null && Request.QueryString["QryIndentId"].ToString() != "")
                //     {
                //         txtIndentId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["QryIndentId"]));
                //         if (!txtIndentId.Text.Contains("-"))
                //         {
                //             LoadIndentDetails(txtIndentId.Text);
                //             cmdSave.Text = "View";
                //         }

                         
                //     }

                //    //WorkFlow / Approval
                //    WorkFlowConfig();
                   
                //}
                #endregion

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, " Page_Load");
            }

        }
        public void LoadTcCapacity(string strIndentId)
        {
            DataTable dtTcCapacity = new DataTable();
            try
            {
                clsStoreIndent objTcTransfer = new clsStoreIndent();
                objTcTransfer.sIndentId = Convert.ToString(strIndentId);

                dtTcCapacity = objTcTransfer.LoadCapacityGrid(objTcTransfer);

                grdTcTransfer.DataSource = dtTcCapacity;
                grdTcTransfer.DataBind();
               // cmdSave.Text = "Update";
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
                //dt = objTcTransfer.LoadCapacity();
                grdTcTransfer.DataSource = dt;
                grdTcTransfer.DataBind();
                grdTcTransfer.Visible = true;
                ddlCapacity.SelectedIndex = 0;
                txtQuantity.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, " LoadCapacity");
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
        public void LoadIndentDetails(string strIndentId)
        {
            try
            {
                clsStoreIndent objTcTransfer = new clsStoreIndent();
                objTcTransfer.sIndentId = Convert.ToString(strIndentId);

                objTcTransfer.GetIndentDetails(objTcTransfer);

                txtSiId.Text = objTcTransfer.sSiId;
                ddlCapacity.SelectedValue = objTcTransfer.sTcCapacity;
                txtQuantity.Text = objTcTransfer.sQuantity;
                txtIndentNumber.Text = objTcTransfer.sIndentNo;
                txtIndentDate.Text = objTcTransfer.sIndentDate;
                ddlStore.SelectedValue = objTcTransfer.sToStoreId;
                txtDescription.Text = objTcTransfer.sDescription;

                LoadTcCapacity(txtIndentId.Text);

                //cmdSave.Enabled = false;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadIndentDetails");
            }
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                //GetTcDetails();

                //View For Generate Report
                if (cmdSave.Text == "View")
                {
                    if (hdfApproveStatus.Value != "")
                    {
                        if (hdfApproveStatus.Value == "1" || hdfApproveStatus.Value == "2")
                        {

                            GenerateIndentReport();
                        }
                    }
                    else
                    {
                        GenerateIndentReport();
                    }
                    return;
                }

                //to check whether capacity and quantity are added
                if (ViewState["dt"] != null)
                {
                    //CheckCapacity();
                    SaveStoreTransfer();
                }
                else
                {
                    ShowMsgBox("Please Add Capacity and Quantity");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdSave_Click");
            }
        }

        bool ValidateForm()
        {
            bool bValidate = false;
            try
            {

                string[] Arr = new string[3];
                if (txtIndentNumber.Text.Trim() == string.Empty)
                {
                    ShowMsgBox("Please Enter Indent Number");
                    return bValidate;
                }
                if (txtDescription.Text.Trim() == string.Empty)
                {
                    ShowMsgBox("Please Enter Description");
                    return bValidate;
                }
                bValidate = true;
                return bValidate;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ValidateForm");
                return bValidate;

            }
        }
        public void SaveStoreTransfer()
        {
            clsStoreIndent objStoreIndent = new clsStoreIndent();
            DataTable dt;
            try
            {
                if (ValidateForm() == true)
                {
                    string[] Arr = new string[2];
                    dt = (DataTable)ViewState["dt"];
                    objStoreIndent.sToStoreId = ddlStore.SelectedValue;
                    objStoreIndent.sIndentNo = txtIndentNumber.Text.Replace("'", "");
                    objStoreIndent.sIndentDate = txtIndentDate.Text.Replace("'", "");
                    objStoreIndent.sDescription = txtDescription.Text.Replace("'", "");
                    objStoreIndent.sCrBy = objSession.UserId;
                    objStoreIndent.sOfficeCode = objSession.OfficeCode;
                    if (txtSiId.Text != "")
                    {
                        objStoreIndent.sSiId = txtSiId.Text;
                    }
                    objStoreIndent.ddtCapacityGrid = dt;
                    objStoreIndent.sToStoreName = ddlStore.SelectedItem.Text.ToString();

                    if (txtActiontype.Text == "A" || txtActiontype.Text == "R")
                    {
                        ApproveRejectAction();
                        return;
                    }


                    //Workflow
                    WorkFlowObjects(objStoreIndent);


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

                        objStoreIndent.sActionType = txtActiontype.Text;
                        objStoreIndent.sCrBy = hdfCrBy.Value;

                        Arr = objStoreIndent.SaveStoreTransfer(objStoreIndent);
                        if (Arr[1].ToString() == "0")
                        {
                            hdfWFDataId.Value = objStoreIndent.sWFDataId;
                            ApproveRejectAction();
                            return;
                        }
                        if (Arr[1].ToString() == "2")
                        {
                            ShowMsgBox(Arr[0]);
                            return;
                        }
                    }

                    #endregion

                    Arr = objStoreIndent.SaveStoreTransfer(objStoreIndent);
                    if (Arr[1].ToString() == "0")
                    {
                        txtSiId.Text = objStoreIndent.sSiId;
                        cmdSave.Enabled = false;
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
                txtDescription.Text = string.Empty;
                txtIndentNumber.Text = string.Empty;
                txtIndentDate.Text = string.Empty;
                ddlStore.SelectedIndex = 0;
                txtIndentNumber.Enabled = true;
                cmdSave.Text = "Save";
                txtSiId.Text = string.Empty;
                ddlCapacity.SelectedIndex = 0;
                txtQuantity.Text = string.Empty;
                grdTcTransfer.Visible = false;
                ViewState["dt"] = null;
                lblMessage.Text = string.Empty;
                grdCapacityDetails.Visible = false;
                GenerateInvoiceNo();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "btnReset_Click");
            }

        }

        protected void grdTcTransfer_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            clsStoreIndent objTcTransfer = new clsStoreIndent();
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
                        if (lblCapacity.Text == Convert.ToString(dt.Rows[i]["SO_CAPACITY"]))
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
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "grdTcTransfer_RowCommand");
            }
        }

        protected void cmdAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                //if datatble is empty,add columns to table
                if (ViewState["dt"] !=null)
                {
                    dt =(DataTable) ViewState["dt"];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (ddlCapacity.SelectedItem.Text == Convert.ToString(dt.Rows[i]["SO_CAPACITY"]))
                        {
                            ShowMsgBox("Capacity Already Added");
                            return;

                        }

                    }
                }

                if (ViewState["dt"] == null)
                {
                    dt.Columns.Add("SO_ID");
                    dt.Columns.Add("SO_CAPACITY");
                    dt.Columns.Add("SO_QNTY");
                }
                else
                {
                    //load datatble from viewstate
                    dt = (DataTable)ViewState["dt"];
                }
                DataRow dRow = dt.NewRow();
                int qnty =Convert.ToInt32(txtQuantity.Text);
                //dRow["SO_QNTY"] =Convert.ToInt32(txtQuantity.Text);
                dRow["SO_QNTY"] = qnty;
                if (Convert.ToString(dRow["SO_QNTY"]) =="0")
                {
                    ShowMsgBox("Quantity Should Not be Zero");
                    return;
                }
                dRow["SO_CAPACITY"] = ddlCapacity.SelectedItem.Text;
                dt.Rows.Add(dRow);
                ViewState["dt"] = dt;
                LoadCapacity(dt);
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdAdd_Click");
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
                    Response.Redirect("StoreIndentView.aspx", false);
                }
               
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdClose_Click");
            }
        }

        protected void grdTcTransfer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtTcCapacity = new DataTable();
                grdTcTransfer.PageIndex = e.NewPageIndex;
                dtTcCapacity = (DataTable)ViewState["dt"];
                LoadCapacity(dtTcCapacity);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "grdTcTransfer_PageIndexChanging");
            }
        }

        public void GenerateInvoiceNo()
        {
            try
            {
                clsInvoice objInvoice = new clsInvoice();

                txtIndentNumber.Text = objInvoice.GenerateInvoiceNo(objSession.OfficeCode);
                txtIndentNumber.ReadOnly = true;
                //hdfInvoiceNo.Value = txtInvoiceNo.Text;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GenerateInvoiceNo");
            }
        }

        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlStore.SelectedIndex > 0)
                {
                    clsStoreIndent objSIndent = new clsStoreIndent();
                    DataTable dt = new DataTable();
                    dt = objSIndent.LoadStoreCapacityGrid(ddlStore.SelectedValue);
                    grdCapacityDetails.DataSource = dt;
                    grdCapacityDetails.DataBind();
                    grdCapacityDetails.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ddlStore_SelectedIndexChanged");
            }
        }

        protected void grdCapacityDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtTcCapacity = new DataTable();
                grdCapacityDetails.PageIndex = e.NewPageIndex;
                dtTcCapacity = (DataTable)ViewState["dt"];
                clsStoreIndent objSIndent = new clsStoreIndent();
                DataTable dt = new DataTable();
                dt = objSIndent.LoadStoreCapacityGrid(ddlStore.SelectedValue);
                grdCapacityDetails.DataSource = dt;
                grdCapacityDetails.DataBind();
                grdCapacityDetails.Visible = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdTcTransfer_PageIndexChanging");
            }
        }

        public void WorkFlowObjects(clsStoreIndent objStoreIndent)
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


                objStoreIndent.sFormName = "StoreIndent";
                objStoreIndent.sOfficeCode = objSession.OfficeCode;
                objStoreIndent.sClientIP = sClientIP;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "WorkFlowObjects");
            }
        }

        #region Load From XML
        public void GetStoreIndentDetailsFromXML(string sWFDataId)
        {
            try
            {

                // If the Data saved in Main Table then this function shd not execute, so done restriction like below
                // And commented for temprary purpose.. nee to change in future

                //if (!txtIndentId.Text.Contains("-"))
                //{
                //    return;
                //}

                clsStoreIndent objStoreIndent = new clsStoreIndent();
                objStoreIndent.sWFDataId = sWFDataId;
                objStoreIndent.GetStoreIndentDetailsFromXML(objStoreIndent);

                //txtIndentNumber.Text = objStoreIndent.sIndentNo;
                txtIndentDate.Text = objStoreIndent.sIndentDate;
                ddlStore.SelectedValue = objStoreIndent.sToStoreId;
                txtDescription.Text = objStoreIndent.sDescription;


                DataTable dt = objStoreIndent.ddtCapacityGrid;

                grdTcTransfer.DataSource = dt;
                grdTcTransfer.DataBind();
               
                ViewState["dt"] = dt;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetFailureDetailsFromXML");
            }

        }
        #endregion


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
                    pnlApprovalIndent.Enabled = false;
                }
                if (txtActiontype.Text == "R")
                {
                    cmdSave.Text = "Reject";
                    pnlApproval.Enabled = false;
                    pnlApprovalIndent.Enabled = false;
                }
                if (txtActiontype.Text == "M")
                {
                    cmdSave.Text = "Modify and Approve";
                    pnlApproval.Enabled = true;
                    pnlApprovalIndent.Enabled = true;
                }

                dvComments.Style.Add("display", "block");
                //cmdReset.Enabled = false;


                if (hdfWFOAutoId.Value != "0")
                {
                    cmdSave.Text = "Save";
                    dvComments.Style.Add("display", "none");
                }

            

                // Check for Creator of Form
                bool bResult = CheckFormCreatorLevel();
                if (bResult == true)
                {
                    cmdSave.Text = "Save";
                    pnlApproval.Enabled = true;
                    pnlApprovalIndent.Enabled = true;
                   
                    // To handle Record From Reject 
                    if (txtActiontype.Text == "A" && hdfWFOAutoId.Value == "0")
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

                //Approve
                if (txtActiontype.Text == "A")
                {
                    objApproval.sApproveStatus = "1";
                    objApproval.sRefOfficeCode = GetOfficeCodeFromStore();
                }
                //Reject
                if (txtActiontype.Text == "R")
                {
                    objApproval.sApproveStatus = "3";
                }
              
                //Modify and Approve
                if (txtActiontype.Text == "M")
                {
                    objApproval.sApproveStatus = "2";
                    if (objSession.RoleId != "5")
                    {
                        objApproval.sRefOfficeCode = GetOfficeCodeFromStore();
                    }
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
                        //ShowMsgBox("Approved Successfully");
                        //txtIndentId.Text = objApproval.sNewRecordId;
                        //GenerateIndentReport();
                        //cmdSave.Enabled = false;
                        ShowMsgBox("Approved Successfully");
                        txtIndentId.Text = objApproval.sNewRecordId;
                        if (objApproval.sRecordId.Contains("-"))
                        {
                        }
                        else
                        {
                            GenerateIndentReport();
                        }
                        cmdSave.Enabled = false;
                       
                    }
                    else if (objApproval.sApproveStatus == "3")
                    {

                        ShowMsgBox("Rejected Successfully");
                        
                        cmdSave.Enabled = false;
                    }
                  
                    else if (objApproval.sApproveStatus == "2")
                    {
                        ShowMsgBox("Modified and Approved Successfully");
                        txtIndentId.Text = objApproval.sNewRecordId;
                        GenerateIndentReport();
                        cmdSave.Enabled = false;
                       
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
                        hdfWFOAutoId.Value = Convert.ToString(Session["WFOAutoId"]);
                        hdfApproveStatus.Value = Convert.ToString(Session["ApproveStatus"]);

                        Session["WFDataId"] = null;
                        Session["WFOId"] = null;
                        Session["WFOAutoId"] = null;
                        Session["ApproveStatus"] = null;
                    }


                    if (hdfWFDataId.Value != "0")
                    {

                        GetStoreIndentDetailsFromXML(hdfWFDataId.Value);

                    }

                    SetControlText();

                    if (txtActiontype.Text == "V")
                    {
                        cmdSave.Text = "View";
                        cmdSave.Enabled = false;
                        dvComments.Style.Add("display", "none");
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
                string sResult = objApproval.GetFormCreatorLevel("", objSession.RoleId, "StoreIndent");
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


        public string GetOfficeCodeFromStore()
        {
            try
            {
                clsStoreIndent objStoreIndent = new clsStoreIndent();
                string sOfficeCode = objStoreIndent.GetOfficeCodeFromStore(ddlStore.SelectedValue);
                return sOfficeCode;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetOfficeCodeFromStore");
                return ex.Message;
            }
        }


        public void GenerateIndentReport()
        {
            try
            {

                string strParam = string.Empty;
                strParam = "id=InterStoreIndent&IndentId=" + txtIndentId.Text;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");


            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GenerateIndentReport");
            }
        }
    }
}