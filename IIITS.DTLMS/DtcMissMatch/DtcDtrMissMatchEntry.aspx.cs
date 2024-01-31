using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IIITS.DTLMS.BL;
using System.Configuration;

namespace IIITS.DTLMS.DtcMissMatch
{
    public partial class DtcDtrMissMatchEntry : System.Web.UI.Page
    {
        string strFormCode = "DtcDtrSwapping";
        clsSession objSession;
        string sRemark;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
            {
                Response.Redirect("~/Login.aspx", false);
            }
            txtDTRCommDate.Attributes.Add("readOnly", "readOnly");
            objSession = (clsSession)Session["clsSession"];
            if (!IsPostBack)
            {
                checkboxdiv.Style.Add("display", "none");
                divOldTc.Style.Add("display", "none");
                LoadSearchWindow();
                CheckAccessRights("1", "1");
                sRemark = ConfigurationManager.AppSettings["MissMatchRemarks"].ToString();
                txtremarks.Text = sRemark;
            }
        }

        public void LoadSearchWindow()
        {
            try
            {

                //txtDtcCode.Attributes.Add("onblur", "return ValidateDate(" + txtInvoiceDate.ClientID + ");");

                string strQry = string.Empty;
                strQry = "Title=Search and Select DTC Details&";
                strQry += "Query=SELECT DT_CODE,DT_NAME FROM TBLDTCMAST";
                strQry += " WHERE  DT_CODE LIKE '" + txtDtcCode.Text.Trim() + "%' AND ROWNUM < 50";
                strQry += " AND {0} like %{1}% &";
                strQry += "DBColName=DT_CODE~DT_NAME&";
                strQry += "ColDisplayName=Dtc Code~Dtc Name&";

                strQry = strQry.Replace("'", @"\'");

                btnDtcSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtDtcCode.ClientID + "&btn=" + btnDtcSearch.ClientID + "',520,520," + txtDtcCode.ClientID + ")");



                strQry = "Title=Search and Select DTr Details&";
                strQry += "Query=SELECT TC_CODE,TO_CHAR(TC_CAPACITY)TC_CAPACITY FROM TBLTCMASTER";
                strQry += " WHERE  TC_CODE LIKE '" + txtDtrCode.Text + "%' AND TC_LIVE_FLAG = 1 AND  ROWNUM < 50";
                strQry += " AND {0} like %{1}% &";
                strQry += "DBColName=TC_CODE~TC_CAPACITY&";
                strQry += "ColDisplayName=Tc Code~Tc Capacity&";

                strQry = strQry.Replace("'", @"\'");

                btnDtrSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtDtrCode.ClientID + "&btn=" + btnDtrSearch.ClientID + "',520,520," + txtDtrCode.ClientID + ")");



                strQry = "Title=Search and Select DTr Details&";
                strQry += "Query=SELECT DT_CODE,DT_NAME FROM TBLDTCMAST";
                strQry += " WHERE  DT_CODE LIKE '" + txtOldDtc.Text + "%' ROWNUM < 50";
                strQry += " AND {0} like %{1}% &";
                strQry += "DBColName=DT_CODE~DT_NAME&";
                strQry += "ColDisplayName=Dtc Code~Dtc Name&";

                strQry = strQry.Replace("'", @"\'");

                btnSearchdtc2.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtOldDtc.ClientID + "&btn=" + btnSearchdtc2.ClientID + "',520,520," + txtOldDtc.ClientID + ")");

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadSearchWindow");
            }
        }

        #region Access Rights

        public bool CheckAccessRights(string sAccessType, string flag)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "DTCMissMatch";
                objApproval.sRoleId = objSession.RoleId;
                objApproval.sAccessType = "1" + "," + sAccessType;
                bool bResult = objApproval.CheckAccessRights(objApproval);
                if (flag == "2")
                {
                    //&& objSession.UserId != "39"
                    if (UserValid() == false)
                    {
                        if (bResult == true )
                        {
                            Response.Redirect("~/UserRestrict.aspx", false);
                            bResult = false;
                        }
                    }
                    
                }
                else if (flag == "1")
                {
                    if (UserValid() == false)
                    {
                        if (bResult == false)
                        {
                            Response.Redirect("~/UserRestrict.aspx", false);
                        }
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

        public bool UserValid()
        {
            bool res = true;
            try
            {
                string Userid = Convert.ToString(ConfigurationSettings.AppSettings["SELECTEDUSER"]);
                string[] sUserid = Userid.Split(',');
                for(int i = 0; i < sUserid.Length; i++)
                {
                    if (objSession.UserId != sUserid[i])
                    {
                        res = false;
                    }
                    else
                    {
                        res = true;
                        return res;
                    }
                }
                return res;
            }
            catch(Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "CheckAccessRights");
                return false;
            }
        }

        #endregion

        protected void btnDtcSearch_Click(object sender, EventArgs e)
        {
            try
            {
                clsDtcMissMatchEntry objDtcMissMatch = new clsDtcMissMatchEntry();
                objDtcMissMatch.sDtcCode = txtDtcCode.Text.Trim();
                DataTable dt = new DataTable();
                dt = objDtcMissMatch.LoadDtcDetails(objDtcMissMatch);
                if (dt.Rows.Count > 0)
                {
                    ViewState["OldtcCode"] = dt.Rows[0]["DT_TC_ID"].ToString();
                    lblDtrCode.Text = "TC: " + dt.Rows[0]["DT_TC_ID"].ToString();
                    checkboxdiv.Style.Add("display", "block");
                    hdfDtrCode.Value = dt.Rows[0]["DT_TC_ID"].ToString();
                    hdfLocType.Value = dt.Rows[0]["TC_CURRENT_LOCATION"].ToString();
                    hdfOffCode.Value = dt.Rows[0]["DT_OM_SLNO"].ToString();
                }
                else
                {
                    ShowMsgBox("DTC Code or DTR couldnt be found Please contact Backend Team");
                    return;
                }
                grdDtcDetails.DataSource = dt;
                grdDtcDetails.DataBind();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "btnDtcSearch_Click");
            }
        }

        protected void btnDtrSearch_Click(object sender, EventArgs e)
        {
            try
            {
                clsDtcMissMatchEntry objDtcMissMatch = new clsDtcMissMatchEntry();
                objDtcMissMatch.sDtrCode = txtDtrCode.Text;
                DataTable dt = new DataTable();
                dt = objDtcMissMatch.LoadDtrDetails(objDtcMissMatch);
                if (dt.Rows.Count > 0)
                {
                    hdfNewLocType.Value = dt.Rows[0]["TC_CURRENT_LOCATION"].ToString();
                    hdfNewOffCode.Value = dt.Rows[0]["TC_LOCATION_ID"].ToString();
                }
                grdDtrDetails.DataSource = dt;
                grdDtrDetails.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "btnDtrSearch_Click");
            }
        }

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {

            if (CheckBox1.Checked == true)
            {
                divOldTc.Style.Add("display", "block");
            }
            else
            {
                divOldTc.Style.Add("display", "none");
            }
        }

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            checkboxdiv.Style.Add("display", "none");
            divOldTc.Style.Add("display", "none");
            grdDtcDetails.DataSource = null;
            grdDtcDetails.DataBind();
            grdDtrDetails.DataSource = null;
            grdDtrDetails.DataBind();
            grdDtrDetails2.DataSource = null;
            grdDtrDetails2.DataBind();
            grdSecondDtcDetails.DataSource = null;
            grdSecondDtcDetails.DataBind();
            txtDtcCode.Text = string.Empty;
            txtDtrCode.Text = string.Empty;
            txtOldDtc.Text = string.Empty;
            CheckBox1.Checked = false;
            txtDTRCommDate.Text = string.Empty;
        }

        protected void btnSearchdtc2_Click(object sender, EventArgs e)
        {
            try
            {
                clsDtcMissMatchEntry objDtcMissMatch = new clsDtcMissMatchEntry();
                DataTable dt = new DataTable();
                objDtcMissMatch.sDtcCode = txtOldDtc.Text;
                dt = objDtcMissMatch.LoadDtcDetails(objDtcMissMatch);
                grdSecondDtcDetails.DataSource = dt;
                grdSecondDtcDetails.DataBind();
                objDtcMissMatch.sDtrCode = ViewState["OldtcCode"].ToString();

                dt = objDtcMissMatch.LoadDtrDetails(objDtcMissMatch);
                grdDtrDetails2.DataSource = dt;
                grdDtrDetails2.DataBind();

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "btnDtcSearch_Click");
            }
        }

        protected void cmdAllocate_Click(object sender, EventArgs e)
        {
            try
            {
                bool bResult = CheckAccessRights("1", "2");
                if (bResult == true)
                {
                    clsDtcMissMatchEntry objMissMatch = new clsDtcMissMatchEntry();
                    string[] sArr = new string[2];
                    objMissMatch.sDtrCode = hdfDtrCode.Value;
                    objMissMatch.sDtcCode = txtDtcCode.Text.Trim();
                    objMissMatch.sLocType = hdfLocType.Value;
                    objMissMatch.sNewDTCCode = txtOldDtc.Text;
                    objMissMatch.sNewLocType = hdfNewLocType.Value;
                    objMissMatch.sOfficeCode = hdfOffCode.Value;
                    objMissMatch.sNewOfficeCode = hdfNewOffCode.Value;
                    objMissMatch.sNewDtrCode = txtDtrCode.Text.Trim();
                    objMissMatch.sDTRCommissionDate = txtDTRCommDate.Text.Trim();


                    if (CheckBox1.Checked == true)
                    {
                        if (txtOldDtc.Text == "")
                        {
                            ShowMsgBox("Please Enter Dtc Code");
                            txtOldDtc.Focus();
                            return;
                        }
                    }
                    if (objMissMatch.sDtrCode == "" || objMissMatch.sNewDtrCode == "")
                    {

                        ShowMsgBox("Load Dtr and DTc Details Before Proceedding");
                        return;
                    }


                    objMissMatch.sRemarks = txtremarks.Text;
                    objMissMatch.sCrBy = objSession.UserId;


                    if (ValidateForm(objMissMatch))
                    {
                        sArr = objMissMatch.swapDetails(objMissMatch);
                    }
                    if (sArr[0] == "1")
                    {
                        ShowMsgBox("DTC Allocated Successfully");
                        cmdReset_Click(sender, e);
                    }
                    else if (sArr[0] == "2")
                    {
                        ShowMsgBox(sArr[1]);
                    }
                    else
                    {
                        ShowMsgBox("Something went wrong .....");
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdAllocate_Click");
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

        protected void grdDtcDetails_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    GridView HeaderGrid = (GridView)sender;
                    GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                    TableCell HeaderCell = new TableCell();
                    HeaderCell.Text = "DTC DETAILS";
                    HeaderCell.ColumnSpan = 4;
                    HeaderGridRow.Cells.Add(HeaderCell);
                    grdDtcDetails.Controls[0].Controls.AddAt(0, HeaderGridRow);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdDtcDetails_RowCreated");
            }
        }

        protected void grdNewMappingDetails_RowCreated(object sender, GridViewRowEventArgs e)
        {

        }

        protected void grdDtrDetails_RowCreated(object sender, GridViewRowEventArgs e)
        {

            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    GridView HeaderGrid = (GridView)sender;
                    GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                    TableCell HeaderCell = new TableCell();
                    HeaderCell.Text = "DTR DETAILS";
                    HeaderCell.ColumnSpan = 5;
                    HeaderGridRow.Cells.Add(HeaderCell);
                    grdDtrDetails.Controls[0].Controls.AddAt(0, HeaderGridRow);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdDtrDetails_RowCreated");
            }
        }

        protected void grdDtrDetails2_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    GridView HeaderGrid = (GridView)sender;
                    GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                    TableCell HeaderCell = new TableCell();
                    HeaderCell.Text = "DTR DETAILS";
                    HeaderCell.ColumnSpan = 5;
                    HeaderGridRow.Cells.Add(HeaderCell);
                    grdDtrDetails2.Controls[0].Controls.AddAt(0, HeaderGridRow);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdDtrDetails_RowCreated");
            }

        }

        protected void grdSecondDtcDetails_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    GridView HeaderGrid = (GridView)sender;
                    GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                    TableCell HeaderCell = new TableCell();
                    HeaderCell.Text = "DTC DETAILS";
                    HeaderCell.ColumnSpan = 4;
                    HeaderGridRow.Cells.Add(HeaderCell);
                    grdSecondDtcDetails.Controls[0].Controls.AddAt(0, HeaderGridRow);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "grdSecondDtcDetails_RowCreated");
            }
        }


        public bool ValidateForm(clsDtcMissMatchEntry objDtcMisEntry)
        {
            if (objDtcMisEntry.sDtrCode == objDtcMisEntry.sNewDtrCode)
            {
                ShowMsgBox("TC Already Allocated with the DTC");
                txtDtrCode.Focus();
                return false;
            }
            if (objDtcMisEntry.sDtcCode == objDtcMisEntry.sNewDTCCode)
            {
                ShowMsgBox("TC Already Allocated with the DTC");
                txtOldDtc.Focus();
                return false;
            }

            if(!(grdDtrDetails.Rows.Count > 0))
            {
                ShowMsgBox("Please Select the valid  DTR Code");
                return false;
            }
            if (!(grdDtcDetails.Rows.Count > 0))
            {
                ShowMsgBox("Please Select the valid  DTC Code");
                return false;
            }

            if(txtDTRCommDate.Text.Length == 0)
            {
                ShowMsgBox("Please Select the DTR Commission Date ");
                return false;
            }
            return true;

        }

    }
}