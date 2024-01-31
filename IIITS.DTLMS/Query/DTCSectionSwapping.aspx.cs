using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Configuration;

namespace IIITS.DTLMS.Query
{
    public partial class DTCSectionSwapping : System.Web.UI.Page
    {
        string sFormCode = "SectionSwap";
        string sofficeCode = "";
        
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
            {
                Response.Redirect("~/Login.aspx", false);
            }

            objSession = (clsSession)Session["clsSession"];
            try
            {
                if (!IsPostBack)
                {
                    //if (objSession.UserType != "4")
                    //{
                    //    Response.Redirect("~/UserRestrict.aspx", false);
                    //}
                    CheckAccessRights("1", "1");
                }

            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, sFormCode, "Page_Load");
            }
        }

        public bool CheckAccessRights(string sAccessType, string flag)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "SectionSwap";
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

                clsException.LogError(ex.StackTrace, ex.Message, sFormCode, "CheckAccessRights");
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

                clsException.LogError(ex.StackTrace, ex.Message, sFormCode, "CheckAccessRights");
                return false;
            }
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                string[] arr = new string[2];
                clsBackEndActivity objBackend = new clsBackEndActivity();
                objBackend.sDtcCode = txtDTCCode.Text;
                objBackend.sDTrCode = txtDTRCode.Text;
                objBackend.sSectionCode = sofficeCode ;
                objBackend.createdBy = objSession.UserId;

                arr = objBackend.ChangeDTCSection(objBackend);
                ShowMsgBox(arr[1]);
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
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, sFormCode, "ShowMsgBox");
            }
        }
        bool ValidateForm()
        {
            bool bValidate = false;

            try
            {
                if (txtDTCCode.Text.Trim() == "" || txtDTCCode.Text == null)
                {
                    txtDTCCode.Focus();
                    ShowMsgBox("Please Enter DTC Code");
                    return false;
                }
                if (txtDTRCode.Text.Trim() == "" || txtDTRCode.Text == null)
                {
                    txtDTRCode.Focus();
                    ShowMsgBox("Please Enter DTR Code");
                    return false;
                }
                sofficeCode = ReportFilterControl1.GetOfficeID();
                if (sofficeCode.Length != 4)
                {
                    ReportFilterControl1.Focus();
                    ShowMsgBox("Please select Section");
                    return false;
                }
                bValidate = true;
                return bValidate;
            }
            catch (Exception ex)
            {
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, sFormCode, "ShowMsgBox");
                return bValidate;
            }
            

        }
    }
}