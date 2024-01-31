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
    public partial class DTRMissmatchEntry : System.Web.UI.Page
    {
        string strFormCode = "DTRUnAllocate";
        clsSession objSession;
        string sRemark;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
            {
                Response.Redirect("~/Login.+", false);
            }

            objSession = (clsSession)Session["clsSession"];
            try
            {
                if (!IsPostBack)
                {
                    CheckAccessRights("1", "1");
                    Genaral.Load_Combo("SELECT SM_CODE,SM_CODE || '-' || SM_NAME FROM TBLSTOREMAST ORDER BY SM_CODE", "--Select--", cmbStore);
                    LoadSearchWindow();
                    sRemark = ConfigurationManager.AppSettings["MissMatchRemarks"].ToString();
                    txtremarks.Text = sRemark;

                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load");
            }
        }


        #region Access Rights

        public bool CheckAccessRights(string sAccessType, string flag)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "DTRUnAllocate";
                objApproval.sRoleId = objSession.RoleId;
                objApproval.sAccessType = "1" + "," + sAccessType;
                bool bResult = objApproval.CheckAccessRights(objApproval);
                if (flag == "2")
                {
                    //&& objSession.UserId != "39"
                    if (UserValid() == false)
                    {
                        if (bResult == true)
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
                for (int i = 0; i < sUserid.Length; i++)
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
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "CheckAccessRights");
                return false;
            }
        }

        #endregion

        public void LoadSearchWindow()
        {
            try
            {
                string strQry = string.Empty;
                strQry = "Title=Search and Select DTr Details&";
                strQry += "Query=SELECT TC_CODE,TO_CHAR(TC_CAPACITY)TC_CAPACITY FROM TBLTCMASTER";
                strQry += " WHERE  TC_CODE LIKE '" + txtDtrCode.Text + "%' AND TC_LIVE_FLAG = 1  AND ROWNUM <50  ";
                strQry += " AND {0} like %{1}% &";
                strQry += "DBColName=TC_CODE~TC_CAPACITY&";
                strQry += "ColDisplayName=Tc Code~Tc Capacity&";

                strQry = strQry.Replace("'", @"\'");

                btnDtrSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtDtrCode.ClientID + "&btn=" + btnDtrSearch.ClientID + "',520,520," + txtDtrCode.ClientID + ")");

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "LoadSearchWindow");
            }

        }

        protected void btnDtrSearch_Click(object sender, EventArgs e)
        {
            try
            {
                clsDtcMissMatchEntry objDtcMissMatch = new clsDtcMissMatchEntry();
                objDtcMissMatch.sDtrCode = txtDtrCode.Text;
                DataTable dt = new DataTable();
                dt = objDtcMissMatch.LoadDtrDetails1(objDtcMissMatch);
                if (dt.Rows.Count > 0)
                {
                    //if (dt.Rows[0]["DF_REPLACE_FLAG"].ToString() == "0")
                    //{
                    //    ShowMsgBox("TC GOT FAILED PLEASE CONTACT ADMIN");
                    //    cmdReset_Click(sender, e);
                    //    return;
                    //}

                    hdfNewLocType.Value = dt.Rows[0]["TC_CURRENT_LOCATION"].ToString();
                    hdfNewOffCode.Value = dt.Rows[0]["TC_LOCATION_ID"].ToString();


                    if ((hdfNewOffCode.Value).Length == 2)
                    {
                        if (cmbStore.SelectedValue == hdfNewOffCode.Value)
                        {
                            ShowMsgBox("TC ALREADY IN STORE");
                            cmdReset_Click(sender, e);
                            return;
                        }

                    }
                    else
                    {
                        if (dt.Rows[0]["TC_UPDATED_EVENT"].ToString() == "Drawn" && dt.Rows[0]["DF_REPLACE_FLAG"].ToString() == "0")
                        {
                            ShowMsgBox("TC ALREADY GIVEN FOR INVOICE AND WAITING TO DECOMMISSION");
                            cmdReset_Click(sender, e);
                            //return;
                        }
                        //if((dt.Rows[0]["TC_UPDATED_EVENT"].ToString() != "Drawn" || dt.Rows[0]["TC_UPDATED_EVENT"].ToString() != "DTC MASTER ENTRY") &&( dt.Rows[0]["DF_REPLACE_FLAG"].ToString() == "0" || dt.Rows[0]["DF_REPLACE_FLAG"].ToString() == ""))
                        //{
                        //    ShowMsgBox("TC ALREADY FAILED NEED TO COMPLETE THE CYCLE");
                        //    cmdReset_Click(sender, e);
                        //}
                        if (dt.Rows[0]["TC_UPDATED_EVENT"].ToString() == "Drawn" && dt.Rows[0]["DF_REPLACE_FLAG"].ToString() == "1")
                        {
                            ShowMsgBox("TC INVOICED IT'S IN FIELD NOW");
                            //cmdReset_Click(sender, e);
                            //return;
                        }

                    }
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

        protected void BtnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (validation() == true)
                {
                    string[] Arr = new string[2];
                    clsDtcMissMatchEntry objDtcMissMatch = new clsDtcMissMatchEntry();
                    string sRemark = ConfigurationManager.AppSettings["MissMatchRemarks"].ToString();
                    objDtcMissMatch.sDtrCode = txtDtrCode.Text;
                    objDtcMissMatch.sStoreId = cmbStore.SelectedValue;
                    objDtcMissMatch.sCrBy = objSession.UserId;
                    objDtcMissMatch.sRemarks = txtremarks.Text;
                    objDtcMissMatch.sDtrStatus = cmbStatus.SelectedValue;
                    Arr = objDtcMissMatch.SendTOStore(objDtcMissMatch);
                    if (Arr[0] == "1")
                    {
                        ShowMsgBox(Arr[1]);
                    }
                    else
                        ShowMsgBox(Arr[1]);

                    cmdReset_Click(sender, e);
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "BtnSend_Click");
            }
        }

        public bool validation()
        {
            bool bValidate = false;
            try
            {

                if (txtDtrCode.Text.Trim() == "")
                {
                    txtDtrCode.Focus();
                    ShowMsgBox("Enter the Enter DTR Code");
                    return bValidate;
                }

                if (cmbStore.Text == "--Select--")
                {
                    cmbStore.Focus();
                    ShowMsgBox("Select Store");
                    return bValidate;
                }
                if (cmbStatus.SelectedValue == "0")
                {
                    cmbStatus.Focus();
                    ShowMsgBox("Select Condition of the DTR ");
                    return bValidate;
                }

                bValidate = true;
                return bValidate;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "validation");
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
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowMsgBox");
            }
        }

        protected void cmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtDtrCode.Text = string.Empty;
                cmbStore.SelectedIndex = 0;
                grdDtrDetails.DataSource = null;
                grdDtrDetails.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdReset_Click");
            }
        }
    }
}