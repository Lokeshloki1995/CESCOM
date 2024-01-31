using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;


namespace IIITS.DTLMS.Internal
{
    public partial class EnumLevel : System.Web.UI.Page
    {
        string strFormCode = "EnumLevel";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/InternalLogin.aspx", false);
                }
                lblmessage.Text = string.Empty;
                objSession = (clsSession)Session["clsSession"];
                if (!IsPostBack)
                {
                    Genaral.Load_Combo("SELECT DIV_CODE,DIV_CODE || '-' || DIV_NAME FROM TBLDIVISION  ORDER BY DIV_CODE", "--Select--", cmbDivision);
                    Genaral.Load_Combo("SELECT EL_ID,EL_NAME FROM TBLENUMLEVELS ORDER BY EL_ID", "--Select--", cmbLevels);
                   
                    cmbDivision.SelectedValue = hdfDivision.Value;
                    cmbDivision_SelectedIndexChanged(sender, e);
                    cmbsubdivision.SelectedValue = hdfSubdivision.Value;
                    cmbsubdivision_SelectedIndexChanged(sender, e);
                    cmbSection.SelectedValue = hdfSection.Value;
                    cmbFeeder.SelectedValue = hdfFeeder.Value;
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load");
            }
        }

   

        protected void cmbDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDivision.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT SD_SUBDIV_CODE, SD_SUBDIV_CODE || '-' || SD_SUBDIV_NAME FROM TBLSUBDIVMAST  WHERE SD_DIV_CODE='" + cmbDivision.SelectedValue + "' ORDER BY SD_SUBDIV_CODE", "--Select--", cmbsubdivision);
                }
                else
                {

                    cmbsubdivision.Items.Clear();
                    cmbSection.Items.Clear();
                    cmbFeeder.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbCircle_SelectedIndexChanged");
            }
        }

        protected void cmbsubdivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbsubdivision.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT OM_CODE, OM_CODE || '-' || OM_NAME FROM TBLOMSECMAST WHERE OM_SUBDIV_CODE = '" + cmbsubdivision.SelectedValue + "' ORDER BY OM_CODE", "--Select--", cmbSection);
                    string strQry = "SELECT FD_FEEDER_CODE,FD_FEEDER_CODE || '-' || FD_FEEDER_NAME FROM TBLFEEDERMAST, TBLFEEDEROFFCODE WHERE  FD_FEEDER_ID = FDO_FEEDER_ID AND";
                    strQry += " FDO_OFFICE_CODE LIKE '" + cmbsubdivision.SelectedValue + "%' AND FD_FEEDER_CODE IN (SELECT ED_FEEDERCODE FROM TBLENUMERATIONDETAILS) ORDER BY FD_FEEDER_CODE";
                    Genaral.Load_Combo(strQry, "--Select--", cmbFeeder);
                }
                else
                {

                    cmbSection.Items.Clear();
                    cmbFeeder.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbsubdivision_SelectedIndexChanged");
            }
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateForm() == true)
                {
                    SaveEnumLevel();                    
                }
            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdSave_Click");
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
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowMsgBox");
            }
        }


        bool ValidateForm()
        {
            bool bValidate = false;
                       
            if (cmbDivision.SelectedIndex == 0)
            {
                cmbDivision.Focus();
                ShowMsgBox("Please Select the Division");
                return false;
            }

            if (cmbsubdivision.SelectedIndex == 0)
            {
                cmbsubdivision.Focus();
                ShowMsgBox("Please Select the Subdivision");
                return false;
            }

            if (cmbSection.SelectedIndex == 0)
            {
                cmbSection.Focus();
                ShowMsgBox("Please Select the Section");
                return false;
            }

            if (cmbFeeder.SelectedIndex == 0)
            {
                cmbFeeder.Focus();
                ShowMsgBox("Please Select the Feeder");
                return false;
            }

            if (cmbLevels.SelectedIndex == 0)
            {
                cmbLevels.Focus();
                ShowMsgBox("Please Select the Levels");
                return false;
            }

            if (txtRemarks.Text.Trim().Length == 0)
            {
                txtRemarks.Focus();
                ShowMsgBox("Please Enter Remarks");
                return false;
            }
           
            bValidate = true;
            return bValidate;
        }


        public void SaveEnumLevel()
        {
            try
            {
                clsEnumLevel objEnumLevel = new clsEnumLevel();
                string[] Arr = new string[2];
                objEnumLevel.sOfficeCode = cmbSection.SelectedValue;
                objEnumLevel.sFeederCode = cmbFeeder.SelectedValue;
                objEnumLevel.sLevel = cmbLevels.SelectedValue;
                objEnumLevel.sRemarks = txtRemarks.Text;
                objEnumLevel.sCrBy = objSession.UserId;

                Arr = objEnumLevel.SaveEnumerationLevel(objEnumLevel);

                if (Arr[1].ToString() == "0")
                {
                   ShowMsgBox(Arr[0].ToString());
                }
               
                if (Arr[1].ToString() == "2")
                {
                    ShowMsgBox(Arr[0]);
                    return;
                }

            }
            catch (Exception ex)
            {
                lblmessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "SaveEnumLevel");
            }
        }





    }
}