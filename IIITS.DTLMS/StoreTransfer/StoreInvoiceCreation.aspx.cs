using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Data.OleDb;
using System.Globalization;

namespace IIITS.DTLMS.StoreTransfer
{
    public partial class StoreInvoiceCreation : System.Web.UI.Page
    {
        string strFormCode = "StoreInvoiceCreation";
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
                if (!IsPostBack)
                {
                    GenerateInvoiceNo();

                    Genaral.Load_Combo("SELECT SM_ID,SM_NAME FROM TBLSTOREMAST ORDER BY SM_NAME", "--Select--", ddlFromStore);

                    if (Request.QueryString["QryIndentId"] != null && Request.QueryString["QryIndentId"].ToString() != "")
                    {
                        txtIndentId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["QryIndentId"]));                     
                        LoadIndentDetails(txtIndentId.Text);
                        LoadTcDetails(txtIndentId.Text);
                        GetStoreInvoiceDetails();
                       
                    }
                    if (Request.QueryString["RefType"] != null && Request.QueryString["RefType"].ToString() != "")
                    {
                        string sRefType = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["RefType"]));
                        EnableDisableControl(sRefType);
                    }


                    LoadSearchWindow();

                    //WorkFlow / Approval
                    WorkFlowConfig();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "Page_Load");
            }  
        }

        public void LoadSearchWindow()
        {
            try
            {
                txtInvoiceDate.Attributes.Add("onblur", "return ValidateDate(" + txtInvoiceDate.ClientID + ");");

                string strQry = string.Empty;
                strQry = "Title=Search and Select Indent Details&";
                strQry += "Query=select SI_NO, SI_ID FROM TBLSTOREINDENT,TBLSTOREMAST  where SI_TRANSFER_FLAG=0 and SI_TO_STORE=SM_ID AND SM_OFF_CODE='" + objSession.OfficeCode.Substring(0, 2) + "' and  {0} like %{1}% order by SI_ID&";
                strQry += "DBColName=SI_NO~SI_ID&";
                strQry += "ColDisplayName=Indent Number~Indent Id&";

                strQry = strQry.Replace("'", @"\'");

                btnIndentSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtIndentNumber.ClientID + "&btn=" + btnIndentSearch.ClientID + "',520,520," + txtIndentNumber.ClientID + ")");

                strQry = "Title=Search and Select Tc Details&";
                strQry += "Query=SELECT TC_CODE,TO_CHAR(TC_CAPACITY) TC_CAPACITY FROM TBLTCMASTER,TBLTRANSMAKES  WHERE TC_MAKE_ID= TM_ID ";
                strQry += "AND TC_CURRENT_LOCATION=1 AND TC_LOCATION_ID LIKE '" + objSession.OfficeCode + "%' AND TC_CAPACITY IN ";
                strQry += " (SELECT SO_CAPACITY FROM TBLSINDENTOBJECTS WHERE SO_SI_ID='"+ txtIndentId.Text +"') ";
                //strQry += " TC_CODE NOT IN (SELECT IO_TCCODE FROM TBLSINVOICEOBJECTS,TBLSTOREINVOICE WHERE TC_CODE=IO_TCCODE AND IS_APPROVE_FLAG='0' AND IO_IS_ID=IS_ID) ";
                strQry += " AND {0} like %{1}% order by TC_SLNO&";
                strQry += "DBColName=TC_CODE~TC_CAPACITY&";
                strQry += "ColDisplayName=DTr Code~DTr Capacity&";

                strQry = strQry.Replace("'", @"\'");

                btnTcSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtTcCode.ClientID + "&btn=" + btnTcSearch.ClientID + "',520,520," + txtTcCode.ClientID + ")");

               
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadSearchWindow");
            }  
        }

        public void LoadIndentDetails(string strIndentId)
        {
            try
            {
                clsStoreInvoice objInvoice = new clsStoreInvoice();
                objInvoice.sIndentId = Convert.ToString(strIndentId);
                objInvoice.sIndentNo = txtIndentNumber.Text.Replace("'", ""); 
                objInvoice.LoadIndentDetails(objInvoice);
                //txtSiId.Text = objTcTransfer.sSiId;
                if (objInvoice.sIndentNo == "")
                {
                    ShowMsgBox("Enter Valid Indent Number");
                    txtIndentNumber.Text = string.Empty;
                    return;
                }
                txtIndentId.Text = objInvoice.sIndentId;
                txtInvoiceId.Text = objInvoice.sInvoiceId;
                txtIndentNumber.Text = objInvoice.sIndentNo;
                ddlFromStore.SelectedValue = objInvoice.sFromStoreId;
                txtIndentDate.Text = objInvoice.sIndentDate;
                txtQuantity.Text = objInvoice.sQuantity;
                LoadTcCapacity(txtIndentId.Text);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadIndentDetails");
            }
        }

        public void LoadTcCapacity(string strIndentId)
        {
            DataTable dtTcCapacity = new DataTable();
            try
            {
                clsStoreInvoice objInvoice = new clsStoreInvoice();
                objInvoice.sIndentId = Convert.ToString(strIndentId);

                dtTcCapacity = objInvoice.LoadCapacityGrid(objInvoice);

                grdTcTransfer.DataSource = dtTcCapacity;
                grdTcTransfer.DataBind();
                ViewState["dtTcCapacity"] = dtTcCapacity;
                grdTcTransfer.Visible = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadTcCapacity");
            }
        }

        public void LoadTC(DataTable dt)
        {
            try
            {
                //dt = objTcTransfer.LoadCapacity();
                grdTcDetails.DataSource = dt;
                grdTcDetails.DataBind();
                txtTcCode.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadTC");
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
                    Response.Redirect("StoreInvoiceView.aspx", false);
                }

               
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdClose_Click");
            }
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {

                //View For Generate Report
                if (cmdSave.Text == "View")
                {
                    if (hdfApproveStatus.Value != "")
                    {
                        if (hdfApproveStatus.Value == "1" || hdfApproveStatus.Value == "2")
                        {

                            GenerateInvoiceReport();
                        }
                    }
                    else
                    {
                        GenerateInvoiceReport();
                    }
                    return;
                }

                if (ViewState["TCDetails"] != null)
                {
                    SaveStoreInvoice();
                }
                else
                {
                    ShowMsgBox("Add Transformer and then Proceed");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdSave_Click");
            }
        }

        public void SaveStoreInvoice()
        {
            clsStoreInvoice objInvoice = new clsStoreInvoice();
            DataTable  dtTCDetails;
            try
            {


               
                if (ValidateForm() == true)
                {
                    string[] Arr = new string[3];
                    dtTCDetails = (DataTable)ViewState["TCDetails"];
                    objInvoice.sTcCode = txtTcCode.Text;
                    objInvoice.sInvoiceDate = txtInvoiceDate.Text.Trim();
                    objInvoice.sInvoiceNo = txtInvoiceNumber.Text.Trim();
                    objInvoice.sRemarks = txtRemarks.Text.Trim();
                    objInvoice.sCrBy = objSession.UserId;
                    objInvoice.ddtTcGrid = dtTCDetails;
                    objInvoice.sIndentId = txtIndentId.Text.Replace("'", "");
                    objInvoice.sInvoiceId = txtInvoiceId.Text.Replace("'", "");
                    objInvoice.sQuantity = txtQuantity.Text.Replace("'", "");
                    objInvoice.sIndentNo = txtIndentNumber.Text;
                    //if (txtSiId.Text != "")
                    //{
                    //    objTransfer.sSiId = txtSiId.Text;
                    //}

                    //Workflow
                    WorkFlowObjects(objInvoice);


                    Arr = objInvoice.SaveStoreInvoice(objInvoice);
                    if (Arr[1].ToString() == "0")
                    {
                        //txtSiId.Text = objTransfer.sSiId;
                        cmdSave.Enabled = false;
                        txtInvoiceId.Text = objInvoice.sInvoiceId;
                        LoadTcCapacity(objInvoice.sIndentId);
                        ShowMsgBox(Arr[0]);
                        GenerateInvoiceReport();
                        dvGatePass.Style.Add("display", "block");

                        return;
                    }
                    else
                    {
                        ShowMsgBox(Arr[0]);
                        LoadTcCapacity(objInvoice.sIndentId);
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

        bool ValidateForm()
        {
            bool bValidate = false;
            try
            {

                string[] Arr = new string[3];
                if (txtRemarks.Text.Trim() == string.Empty)
                {
                    ShowMsgBox("Please Enter Remarks");
                    txtRemarks.Focus();
                    return bValidate;
                }
                if (txtInvoiceNumber.Text.Trim() == string.Empty)
                {
                    ShowMsgBox("Please Enter Invoice Number");
                    txtInvoiceNumber.Focus();
                    return bValidate;
                }
                if (txtInvoiceDate.Text.Trim() == "")
                {
                    ShowMsgBox("Please Enter Invoice Date");
                    txtInvoiceDate.Focus();
                    return bValidate;
                }
                string sResult = Genaral.DateComparision(txtInvoiceDate.Text, txtIndentDate.Text, false, false);
                if (sResult == "2")
                {
                    ShowMsgBox("Invoice Date should be Greater than Indent Date");
                    txtInvoiceDate.Focus();
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

        protected void cmdAdd_Click(object sender, EventArgs e)
        {
            try
            {
                clsStoreInvoice objInvoice = new clsStoreInvoice();
                DataTable dtIndentTcGrid;
                objInvoice.sTcCode = txtTcCode.Text;
                objInvoice.sOfficeCode = objSession.OfficeCode;
                objInvoice.sIndentId = txtIndentId.Text;
                objInvoice.LoadTcDetails(objInvoice);
                if (ViewState["dtTcCapacity"] == null)
                {
                    ShowMsgBox("Select Indent Details and then proceed!!!");
                    return;
                }
                if (objInvoice.sTcCode == "")
                {
                    ShowMsgBox("Requested DTr Already Allocated");
                    return;
                }
                if (objInvoice.sTcId == "")
                {
                    txtTcCode.Text = "";
                    ShowMsgBox("TC is not in Store or Good Condition");
                    return;
                }
                dtIndentTcGrid = (DataTable)ViewState["dtTcCapacity"];

                bool isCapacity = false;
                bool isCount = false;
                isCapacity = ischeckcapacity(objInvoice);

                if (isCapacity)
                {

                    if (ViewState["TCDetails"] != null)
                    {
                        DataTable dtTcDetails = (DataTable)ViewState["TCDetails"];
                        DataRow drow;

                        for (int i = 0; i < dtTcDetails.Rows.Count; i++)
                        {
                            if (txtTcCode.Text == Convert.ToString(dtTcDetails.Rows[i]["TC_CODE"]))
                            {
                                ShowMsgBox("DTr Already Added");
                                return;
                            }
                        }
                        if (dtTcDetails.Rows.Count > 0)
                        {
                            isCount = isCountCapacity(objInvoice);
                            if (isCount)
                            {
                                drow = dtTcDetails.NewRow();
                                drow["TC_ID"] = objInvoice.sTcId;
                                drow["TC_CODE"] = objInvoice.sTcCode;
                                drow["TC_SLNO"] = objInvoice.sTcSlNo;
                                drow["TM_NAME"] = objInvoice.sTcName;
                                drow["TC_CAPACITY"] = objInvoice.sTcCapacity;
                                dtTcDetails.Rows.Add(drow);
                                grdTcDetails.DataSource = dtTcDetails;
                                grdTcDetails.DataBind();
                                txtTcCode.Text = string.Empty;
                                ViewState["TCDetails"] = dtTcDetails;
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

                        DataTable dtTcDetails = new DataTable();
                        DataRow drow;
                        dtTcDetails.Columns.Add(new DataColumn("TC_ID"));
                        dtTcDetails.Columns.Add(new DataColumn("TC_SLNO"));
                        dtTcDetails.Columns.Add(new DataColumn("TC_CODE"));
                        dtTcDetails.Columns.Add(new DataColumn("TM_NAME"));
                        dtTcDetails.Columns.Add(new DataColumn("TC_CAPACITY"));
                        drow = dtTcDetails.NewRow();
                        drow["TC_ID"] = objInvoice.sTcId;
                        drow["TC_SLNO"] = objInvoice.sTcSlNo;
                        drow["TC_CODE"] = objInvoice.sTcCode;
                        drow["TM_NAME"] = objInvoice.sTcName;
                        drow["TC_CAPACITY"] = objInvoice.sTcCapacity;
                        dtTcDetails.Rows.Add(drow);
                        grdTcDetails.DataSource = dtTcDetails;
                        grdTcDetails.DataBind();
                        txtTcCode.Text = string.Empty;
                        ViewState["TCDetails"] = dtTcDetails;
                        grdTcDetails.Visible = true;

                    }
                }
                else
                {
                    ShowMsgBox("You did not requested transformer of this capacity");
                    return;
                }
            }

            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdAdd_Click");
            }
        }

        public bool ischeckcapacity(clsStoreInvoice objInvoice)
            {
                bool isCapacity = false;
                try
                {
                    DataTable dtIndentTcGrid;
                    dtIndentTcGrid = (DataTable)ViewState["dtTcCapacity"];
                    for (int i = 0; i < dtIndentTcGrid.Rows.Count; i++)
                    {
                        //to check whether selected capacity matches with the requested Capacity
                        if (Convert.ToString(dtIndentTcGrid.Rows[i]["CAPACITY"]) == (objInvoice.sTcCapacity))
                        {
                            isCapacity = true;
                        }
                    }
                    return isCapacity;
                }
                catch (Exception ex)
                {
                    lblMessage.Text = clsException.ErrorMsg();
                    clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ischeckcapacity");
                }
                return isCapacity;
            }

        public bool isCountCapacity(clsStoreInvoice objInvoice)
        {
            //bool isCapacity = false;
            //try
            //{
            //    DataTable dtIndentTcGrid;
            //    DataTable dtTcGrid;
            //    int Count = 0;
            //    dtIndentTcGrid = (DataTable)ViewState["dtTcCapacity"];
            //    dtTcGrid = (DataTable)ViewState["TCDetails"];

            //    for (int i = 0; i < dtIndentTcGrid.Rows.Count; i++)
            //    {
            //        for (int j = 0; j < dtTcGrid.Rows.Count; j++)
            //        {
            //            //Taking count of number of transformers selected 
            //            if (Convert.ToString(dtTcGrid.Rows[j]["TC_CAPACITY"]) == objInvoice.sTcCapacity)
            //            {
            //                Count++;
            //            }
            //        }
            //        //To check whether selected transformers doesnot exceed requested number of transformers
            //        if (Convert.ToInt32(dtIndentTcGrid.Rows[i]["PENDINGCOUNT"]) > Count)
            //        {
            //            isCapacity = true;
            //        }
            //    }


            bool isCapacity = false;
            try
            {
                List<clsStoreInvoice> lst = new List<clsStoreInvoice>();
                List<clsStoreInvoice> lstCheckTcCapa = new List<clsStoreInvoice>();
                DataTable dtIndentTcGrid;
                DataTable dtTcGrid;
                dtIndentTcGrid = (DataTable)ViewState["dtTcCapacity"];
                dtTcGrid = (DataTable)ViewState["TCDetails"];
                foreach (DataRow drow in dtTcGrid.Rows)
                {
                    lst.Add(new clsStoreInvoice
                    {

                        sTcId = Convert.ToString(drow["TC_ID"]),
                        sTcSlNo = Convert.ToString(drow["TC_SLNO"]),
                        sTcCode = Convert.ToString(drow["TC_CODE"]),
                        sTcName = Convert.ToString(drow["TM_NAME"]),
                        sTcCapacity = Convert.ToString(drow["TC_CAPACITY"])
                    });
                }
                foreach (DataRow drow in dtIndentTcGrid.Rows)
                {
                    lstCheckTcCapa.Add(new clsStoreInvoice
                    {
                        sIndentId = Convert.ToString(drow["SI_ID"]),
                        sQuantity = Convert.ToString(drow["REQ_QNTY"]),
                        sTcCapacity = Convert.ToString(drow["CAPACITY"]),
                        sSiId = Convert.ToString(drow["PENDINGCOUNT"])

                    });
                }
                int i = lst.FindAll(item => item.sTcCapacity == Convert.ToString(objInvoice.sTcCapacity)).Count();

                var j = lstCheckTcCapa.Find(item => item.sTcCapacity == Convert.ToString(objInvoice.sTcCapacity));

                if (i == Convert.ToInt32(j.sSiId))
                {
                    return false;
                }
                else
                {
                    return true;
                }

                return isCapacity;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "isCountCapacity");
            }
            return isCapacity;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                //txtInvoiceNumber.Text = string.Empty;
                txtInvoiceId.Text = string.Empty;
                txtRemarks.Text = string.Empty;
                txtTcCode.Text = string.Empty;
                txtInvoiceDate.Text = string.Empty;
                cmdSave.Text = "Save";
                //ViewState["TCDetails"] = null;
                lblMessage.Text = string.Empty;
               
                grdTcDetails.Visible = false;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "btnReset_Click");
            }
        }

        protected void grdTcDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Remove")
                {
                    DataTable dt = (DataTable)ViewState["TCDetails"];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                        Label lblTcSerialNo = (Label)row.FindControl("lblTcCode");
                        //to remove selected Capacity from grid
                        if (lblTcSerialNo.Text == Convert.ToString(dt.Rows[i]["TC_CODE"]))
                        {
                            dt.Rows[i].Delete();
                            dt.AcceptChanges();
                            clsStoreInvoice objDelete = new clsStoreInvoice();
                            objDelete.sTcCode = lblTcSerialNo.Text;
                            objDelete.UpdateDeleteItem(objDelete);
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        ViewState["TCDetails"] = dt;
                    }
                    else
                    {
                        ViewState["TCDetails"] = null;
                    }
                    LoadTC(dt);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "grdTcDetails_RowCommand");
            }
        }

        protected void btnIndentSearch_Click(object sender, EventArgs e)
        {
            try
            {
               
                LoadIndentDetails("");
                LoadTcDetails(txtIndentId.Text);

                string strQry = string.Empty;
                strQry = "Title=Search and Select Tc Details&";
                strQry += "Query=SELECT TC_CODE,TO_CHAR(TC_CAPACITY) TC_CAPACITY FROM TBLTCMASTER,TBLTRANSMAKES  WHERE TC_MAKE_ID= TM_ID ";
                strQry += " AND TC_CURRENT_LOCATION=1 AND TC_LOCATION_ID LIKE '" + objSession.OfficeCode + "%' AND TC_CAPACITY IN ";
                strQry += " (SELECT SO_CAPACITY FROM TBLSINDENTOBJECTS WHERE SO_SI_ID='" + txtIndentId.Text + "')";
                strQry += " AND {0} like %{1}% order by TC_SLNO&";
                strQry += "DBColName=TC_CODE~TC_CAPACITY&";
                strQry += "ColDisplayName=DTr Code~DTr Capacity&";

                strQry = strQry.Replace("'", @"\'");

                btnTcSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtTcCode.ClientID + "&btn=" + btnTcSearch.ClientID + "',520,520," + txtTcCode.ClientID + ")");

               
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "btnIndentSearch_Click");
            }
        }

        public void GenerateInvoiceNo()
        {
            try
            {
                clsInvoice objInvoice = new clsInvoice();

                txtInvoiceNumber.Text = objInvoice.GenerateInvoiceNo(objSession.OfficeCode);
                txtInvoiceNumber.ReadOnly = true;
                //hdfInvoiceNo.Value = txtInvoiceNo.Text;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GenerateInvoiceNo");
            }
        }

        protected void grdTcTransfer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtTcCapacity = new DataTable();
                grdTcTransfer.PageIndex = e.NewPageIndex;
                dtTcCapacity = (DataTable)ViewState["dtTcCapacity"];
                LoadCapacity(dtTcCapacity);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdTcTransfer_PageIndexChanging");
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
                txtQuantity.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, " LoadCapacity");
            }
        }

        public void LoadTcDetails(string strIndentId)
        {
            try
            {
                clsStoreInvoice objInvoice = new clsStoreInvoice();
                objInvoice.sIndentId = Convert.ToString(strIndentId);
                objInvoice.sIndentNo = txtIndentNumber.Text.Replace("'", "");
                DataTable dt = new DataTable();
                dt = objInvoice.LoadDtrDetails(objInvoice);
                grdDtrDetails.DataSource = dt;
                grdDtrDetails.DataBind();

                grdDtrDetails.Visible = true;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadTcDetails");
            }
        }

        public void EnableDisableControl(string sRefType)
        {
            try
            {
                if (sRefType == "View")
                {
                    dvInvoiceCreate.Style.Add("display", "block");
                    cmdSave.Enabled = true;

                }
                if (sRefType == "Edit")
                {
                    dvInvoiceCreate.Style.Add("display", "none");
                    dvDTRDetails.Style.Add("display", "none");
                    dvGatePass.Style.Add("display", "none");
                    cmdSave.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "EnableDisableControl");
            }
        }

        #region GatePass

        protected void cmdGatePass_Click(object sender, EventArgs e)
        {
            try
            {
                clsInvoice objInvoice = new clsInvoice();
                string[] Arr = new string[2];
                if (ValidateGatePass() == true)
                {
                    objInvoice.sGatePassId = txtGpId.Text;
                    objInvoice.sVehicleNumber = txtVehicleNo.Text.Replace("'", "");
                    objInvoice.sReceiptientName = txtReciepient.Text.Replace("'", "");
                    objInvoice.sChallenNo = txtChallen.Text.Replace("'", "");
                    objInvoice.sCreatedBy = objSession.UserId;
                    //objInvoice.sTcCode = txtTCCode.Text.Replace("'", "");
                    objInvoice.sInvoiceNo = txtInvoiceNumber.Text.Replace("'", "");
                    //objInvoice.sInvoiceNo = hdfInvoiceNo.Value.Replace("'", "");

                    Arr = objInvoice.SaveUpdateGatePassDetails(objInvoice);

                    if (Arr[1].ToString() == "0")
                    {
                        txtGpId.Text = objInvoice.sGatePassId;
                        string strParam = "id=StoreGatepass&InvoiceId=" + txtInvoiceNumber.Text;
                        RegisterStartupScript("PrintD", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

                        return;
                    }
                    if (Arr[1].ToString() == "1")
                    {
                        string strParam = "id=StoreGatepass&InvoiceId=" + txtInvoiceNumber.Text;
                        RegisterStartupScript("PrintD", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");

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
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdGatePass_Click");

            }
        }

        bool ValidateGatePass()
        {
            bool bValidate = false;

            try
            {

                if (txtVehicleNo.Text.Length == 0)
                {
                    txtVehicleNo.Focus();
                    ShowMsgBox("Enter Vehicle No");
                    return bValidate;
                }
                if (txtChallen.Text.Length == 0)
                {
                    txtChallen.Focus();
                    ShowMsgBox("Enter Challen Number");
                    return bValidate;
                }

                if (txtReciepient.Text.Trim().Length == 0)
                {
                    txtReciepient.Focus();
                    ShowMsgBox("Enter Reciepient Name");
                    return bValidate;
                }



                bValidate = true;
                return bValidate;
            }
            catch (Exception ex)
            {
                bValidate = false;
                return bValidate;
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
                    //pnlApprovalInVoice.Enabled = false;
                    
                }
                if (txtActiontype.Text == "R")
                {
                    cmdSave.Text = "Reject";
                   // pnlApprovalInVoice.Enabled = false;
                    
                }
                if (txtActiontype.Text == "M")
                {
                    cmdSave.Text = "Modify and Approve";
                    pnlApprovalInVoice.Enabled = true;
                  
                }

                if (hdfWFOAutoId.Value != "0")
                {
                    //cmdSave.Text = "Save";
                    //dvComments.Style.Add("display", "none");
                }

                // Check for Creator of Form
                bool bResult = CheckFormCreatorLevel();
                if (bResult == true)
                {
                    // cmdSave.Text = "Save";
                    cmdApprove.Visible = true;
                    pnlApprovalInVoice.Enabled = true;
                    dvComments.Style.Add("display", "block");
                    cmdSave.Visible = false;
                    btnReset.Visible = false;

                    // To handle Record From Reject 
                    if (txtActiontype.Text == "A" && hdfWFOAutoId.Value == "0")
                    {
                        txtActiontype.Text = "M";
                        hdfRejectApproveRef.Value = "RA";
                    }
                }
                else
                {
                    dvInvoiceCreate.Style.Add("display", "block");
                    dvComments.Style.Add("display", "none");
                    if (txtActiontype.Text == "A")
                    {
                        txtActiontype.Text = "M";
                    }
                    cmdApprove.Visible = false;
                    dvGatePass.Style.Add("display", "block");
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
                    //objApproval.sRefOfficeCode = GetOfficeCodeFromStore();
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
                    //objApproval.sRefOfficeCode = GetOfficeCodeFromStore();
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
                objApproval.sRecordId = objApproval.GetRecordIdForWorkFlow();

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
                        ShowMsgBox("Approved Successfully");
                        cmdSave.Enabled = false;
                        GenerateInvoiceReport();
                    }
                    else if (objApproval.sApproveStatus == "3")
                    {

                        ShowMsgBox("Rejected Successfully");
                        cmdSave.Enabled = false;
                    }

                    else if (objApproval.sApproveStatus == "2")
                    {
                        ShowMsgBox("Modified and Approved Successfully");
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
                        hdfRecordId.Value = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["RecordId"]));

                        Session["WFDataId"] = null;
                        Session["WFOId"] = null;
                        Session["WFOAutoId"] = null;
                        Session["ApproveStatus"] = null;
                    }


                    if (hdfWFDataId.Value != "0")
                    {
                        //GetStoreIndentDetailsFromXML(hdfWFDataId.Value);
                    }

                    SetControlText();

                    if (txtActiontype.Text == "V")
                    {
                        cmdSave.Text = "View";
                        //cmdSave.Enabled = false;
                        dvComments.Style.Add("display", "none");
                        pnlApprovalInVoice.Enabled = false;
                    }

                }
                else
                {
                    dvDTRDetails.Style.Add("display", "block");
                    dvGatePass.Style.Add("display", "block");
                    cmdSave.Text = "View";
                    //cmdSave.Enabled = false;
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
                string sResult = objApproval.GetFormCreatorLevel("", objSession.RoleId, "StoreInvoiceCreation");
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
                    pnlApprovalInVoice.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "DisableControlForView");
            }
        }
        #endregion

        protected void cmdApprove_Click(object sender, EventArgs e)
        {
            try
            {
                clsApproval objApproval = new clsApproval();
                clsStoreInvoice objInvoice = new clsStoreInvoice();

                WorkFlowObjects(objInvoice);

                objApproval.sFormName = objInvoice.sFormName;
                objApproval.sRecordId = objApproval.GetRecordIdForWorkFlow();
                //objApproval.sNewRecordId = objInvoice.sInvoiceId;
                objApproval.sOfficeCode = objInvoice.sOfficeCode;
                objApproval.sClientIp = objInvoice.sClientIP;
                objApproval.sCrby = objSession.UserId;
                objApproval.sWFObjectId = objInvoice.sWFOId;
                objApproval.sRefOfficeCode = objInvoice.sOfficeCode;
                objApproval.sDescription = "Store Invoice Creation for Indent No " + txtIndentNumber.Text;
                

                objApproval.SaveWorkflowObjects(objApproval);

                ShowMsgBox("Approved Successfully");
                cmdApprove.Enabled = false;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdApprove_Click");
              
            }
        }

        public string GetOfficeCodeFromStore()
        {
            try
            {
                clsStoreIndent objStoreIndent = new clsStoreIndent();
                string sOfficeCode = objStoreIndent.GetOfficeCodeFromStore(ddlFromStore.SelectedValue);
                return sOfficeCode;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetOfficeCodeFromStore");
                return ex.Message;
            }
        }

        public void WorkFlowObjects(clsStoreInvoice objStoreInvoice)
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


                objStoreInvoice.sFormName = "StoreInvoiceCreation";
                objStoreInvoice.sOfficeCode = objSession.OfficeCode;
                objStoreInvoice.sClientIP = sClientIP;
                objStoreInvoice.sRefOfficeCode = GetOfficeCodeFromStore();
                objStoreInvoice.sWFOId = hdfWFOId.Value;
                objStoreInvoice.sRecordId = hdfRecordId.Value;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "WorkFlowObjects");
            }
        }


        public void GetStoreInvoiceDetails()
        {
            try
            {
                clsStoreInvoice objSInvoice = new clsStoreInvoice();

                objSInvoice.sIndentId = txtIndentId.Text;
                objSInvoice.GetStoreInvoiceDetails(objSInvoice);

                if (objSInvoice.sInvoiceNo != null)
                {
                    txtInvoiceNumber.Text = objSInvoice.sInvoiceNo;
                    txtInvoiceDate.Text = objSInvoice.sInvoiceDate;
                    txtRemarks.Text = objSInvoice.sRemarks;

                    dvDTRDetails.Style.Add("display", "block");
                    GetGatePassDetails();
                   
                }

               
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetStoreInvoiceDetails");
            }
        }

        public void GetGatePassDetails()
        {
            try
            {
                clsInvoice objInvoice = new clsInvoice();
                objInvoice.sInvoiceNo = txtInvoiceNumber.Text;
                objInvoice.GetGatePassDetials(objInvoice);

                txtChallen.Text = objInvoice.sChallenNo;
                txtVehicleNo.Text = objInvoice.sVehicleNumber;
                txtReciepient.Text = objInvoice.sReceiptientName;

                if (txtVehicleNo.Text != "")
                {
                    txtChallen.ReadOnly = true;
                    txtVehicleNo.ReadOnly = true;
                    txtReciepient.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdSearchIndent_Click");
            }
        }

        public void GenerateInvoiceReport()
        {
            try
            {
                
                string strParam = string.Empty;
                strParam = "id=InterStoreInvoice&InvoiceNo=" + txtInvoiceNumber.Text + "&OfficeCode=" + objSession.OfficeCode ;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                
               
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GenerateIndentReport");
            }
        }

        protected void grdTcDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdTcDetails.PageIndex = e.NewPageIndex;
                grdTcDetails.DataSource = ViewState["TCDetails"];
                grdTcDetails.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdTcTransfer_PageIndexChanging");
            }
        }
    }
}