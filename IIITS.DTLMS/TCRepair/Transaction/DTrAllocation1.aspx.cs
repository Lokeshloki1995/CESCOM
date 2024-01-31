using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Data.OleDb;

namespace IIITS.DTLMS.Transaction
{
    public partial class DTrAllocation1 : System.Web.UI.Page
    {
        string strFormCode = "DTrAllocation1";
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
                if (!IsPostBack)
                {

                    //CheckAccessRights("4");

                    AdminAccess();

                    string strQry = string.Empty;
                    strQry += "Title=Search and Select DTr CODE Details&";
                    strQry += "Query=select  TC_CODE,TC_SLNO FROM TBLTCMASTER  WHERE TC_CURRENT_LOCATION=2 AND TC_STATUS IN (1,2) and  TC_LOCATION_ID LIKE '" + objSession.OfficeCode + "%' AND {0} like %{1}% order by TC_CODE&";
                    strQry += "DBColName=TC_CODE~TC_SLNO&";
                    strQry += "ColDisplayName=DTr Code~DTr Slno&";
                    strQry = strQry.Replace("'", @"\'");
                    cmdSearchId.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtTcCode.ClientID + "&btn=" + cmdSearchId.ClientID + "',520,520," + txtTcCode.ClientID + ")");


                    strQry = "Title=Search and Select DTr CODE Details&";
                    strQry += "Query=select  TC_CODE,TC_SLNO FROM TBLTCMASTER  WHERE TC_CURRENT_LOCATION=2  AND TC_STATUS IN (1,2) AND  TC_LOCATION_ID LIKE '" + objSession.OfficeCode + "%' AND {0} like %{1}% order by TC_CODE&";
                    strQry += "DBColName=TC_CODE~TC_SLNO&";
                    strQry += "ColDisplayName=DTr Code~DTr Slno&";
                    strQry = strQry.Replace("'", @"\'");
                    cmdSearch.Attributes.Add("onclick", "javascript:return OpenWindow('/SearchWindow.aspx?" + strQry + "tb=" + txtSecondDtrCode.ClientID + "&btn=" + cmdSearch.ClientID + "',520,520," + txtSecondDtrCode.ClientID + ")");


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load");
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


        protected void cmdSearchId_Click(object sender, EventArgs e)
        {
            try
            {
                GetDTrDetails();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdSearchId_Click");
            }
        }


        public void GetDTrDetails()
        {
            try
            {
                clsDTrAllocation objAllocation = new clsDTrAllocation();
                DataTable dt = new DataTable();

                objAllocation.sFirstDTrCode = txtTcCode.Text;
                objAllocation.GetDTrDetails(objAllocation);

                txtfirstDtrSlNo.Text = objAllocation.sTcSlNo;
                txtFirstCapacity.Text = objAllocation.sCapacity;
                txtfirstDtcCode.Text = objAllocation.sFirstDTCCode;
                txtFirstDtrName.Text = objAllocation.sMakeName;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetDTrDetails");
            }
        }


        public void GetSecondDTrDetails()
        {
            try
            {
                clsDTrAllocation objAllocation = new clsDTrAllocation();
                DataTable dt = new DataTable();
                objAllocation.sFirstDTrCode = txtSecondDtrCode.Text;
                objAllocation.GetDTrDetails(objAllocation);
                txtSecondDtrSlNo.Text = objAllocation.sTcSlNo;
                txtSecondCapacity.Text = objAllocation.sCapacity;
                txtSecondDtcCode.Text = objAllocation.sFirstDTCCode;
                txtSecondDtrName.Text = objAllocation.sMakeName;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GetSecondDTrDetails");
            }
        }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                GetSecondDTrDetails();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdSearch_Click");
            }
        }

        protected void cmdAllocate_Click(object sender, EventArgs e)
        {
            try
            {
                string[] Arr = new string[2];

                if (txtTcCode.Text.Trim() == txtSecondDtrCode.Text.Trim())
                {
                    ShowMsgBox("First DTr Code and Second DTr code should not be same");
                    return;
                }

                clsDTrAllocation objalloc = new clsDTrAllocation();
                objalloc.sFirstDTrCode = txtTcCode.Text;
                objalloc.sSecondDTrCode = txtSecondDtrCode.Text;
                objalloc.sFirstDTCCode = txtfirstDtcCode.Text;
                objalloc.sSecondDTCCode = txtSecondDtcCode.Text;
                objalloc.sCrby = objSession.UserId;
                objalloc.sUserName = objSession.FullName;

                Arr = objalloc.DTrAllocation(objalloc);
                if (Arr[1].ToString() == "0")
                {
                    ShowMsgBox(Arr[0].ToString());
                    cmdAllocate.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdAllocate_Click");
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
                txtTcCode.Text = string.Empty;
                txtfirstDtrSlNo.Text = string.Empty;
                txtFirstCapacity.Text = string.Empty;
                txtfirstDtcCode.Text = string.Empty;
                txtFirstDtrName.Text = string.Empty;
                txtSecondDtrCode.Text = string.Empty;
                txtSecondDtrSlNo.Text = string.Empty;
                txtSecondCapacity.Text = string.Empty;
                txtSecondDtcCode.Text = string.Empty;
                txtSecondDtrName.Text = string.Empty;
                cmdAllocate.Enabled = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Reset");
            }
        }


        #region Access Rights

        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "UserCreate";
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

        public void AdminAccess()
        {
            try
            {
                if (objSession.RoleId != "8")
                {
                    Response.Redirect("~/UserRestrict.aspx", false);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "AdminAccess");
            }
        }
    }
}