using IIITS.DTLMS.BL;
using IIITS.DTLMS.BL.MasterForms;
using System;

namespace IIITS.DTLMS.MasterForms
{
    public partial class Incharge : System.Web.UI.Page
    {
        clsSession objSession = new clsSession();
        string strFormCode = "Incharge";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                objSession = (clsSession)Session["clsSession"];
                string stroffCode = string.Empty;
                cmbCircle.Attributes.Add("readonly", "readonly");
                cmbDivision.Attributes.Add("readonly", "readonly");
                cmbsubdivision.Attributes.Add("readonly", "readonly");
                cmbSection.Attributes.Add("readonly", "readonly");
                txtfromdate.Attributes.Add("readonly", "readonly");
                txttodate.Attributes.Add("readonly", "readonly");
                txtomdate.Attributes.Add("readonly", "readonly");
                cmbActualRole.Attributes.Add("readonly", "readonly");
                cmbempname.Attributes.Add("readonly", "readonly");
                cmbname.Attributes.Add("readonly", "readonly");
                txtAutoOmnumber.Attributes.Add("readonly", "readonly");
                cmbempname.Enabled = false;
                cmbCircle.Enabled = false;
                cmbDivision.Enabled = false;
                cmbsubdivision.Enabled = false;
                cmbSection.Enabled = false;
                cmbname.Enabled = false;
                cmbActualRole.Enabled = false;
                if (objSession.OfficeCode.Length > 1)
                {
                    stroffCode = objSession.OfficeCode.Substring(0, 1);
                }
                else
                {
                    stroffCode = objSession.OfficeCode;
                }
                CalendarExtender1.StartDate = DateTime.Now;
                CalendarExtender3.StartDate = DateTime.Now;
                CalendarExtender3.EndDate = DateTime.Now;
                OMDateCalender.StartDate = DateTime.Now;
                if (!IsPostBack)
                {
                    if (stroffCode == null || stroffCode == "")
                    {
                        Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME FROM TBLCIRCLE ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle);

                    }

                    if (stroffCode.Length >= 1)
                    {
                        Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME FROM TBLCIRCLE ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle);


                        cmbCircle.Items.FindByValue(stroffCode).Selected = true;
                        stroffCode = string.Empty;
                        stroffCode = objSession.OfficeCode;
                    }

                    if (stroffCode.Length >= 1)
                    {
                        Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION WHERE DIV_CICLE_CODE='" + cmbCircle.SelectedValue + "'", "--Select--", cmbDivision);
                        if (stroffCode.Length >= 2)
                        {
                            stroffCode = objSession.OfficeCode.Substring(0, 2);
                            cmbDivision.Items.FindByValue(stroffCode).Selected = true;
                            stroffCode = string.Empty;
                            stroffCode = objSession.OfficeCode;
                        }
                    }

                    if (stroffCode.Length >= 2)
                    {
                        Genaral.Load_Combo("SELECT SD_SUBDIV_CODE,SD_SUBDIV_NAME from TBLSUBDIVMAST WHERE SD_DIV_CODE='" + cmbDivision.SelectedValue + "'", "--Select--", cmbsubdivision);
                        if (stroffCode.Length >= 3)
                        {
                            stroffCode = objSession.OfficeCode.Substring(0, 3);
                            cmbsubdivision.Items.FindByValue(stroffCode).Selected = true;
                            stroffCode = string.Empty;
                            stroffCode = objSession.OfficeCode;
                        }
                    }
                    if (stroffCode.Length >= 3)
                    {
                        Genaral.Load_Combo("SELECT OM_CODE,OM_NAME FROM TBLOMSECMAST WHERE OM_SUBDIV_CODE='" + cmbsubdivision.SelectedValue + "'", "--Select--", cmbSection);
                        if (stroffCode.Length >= 4)
                        {
                            stroffCode = objSession.OfficeCode.Substring(0, 4);
                            cmbSection.Items.FindByValue(stroffCode).Selected = true;
                        }

                    }

                    Genaral.Load_Combo("SELECT US_ID,US_LG_NAME FROM TBLUSER WHERE US_OFFICE_CODE='" + stroffCode + "' and US_ID = '"+objSession.UserId+"' and US_STATUS='A'", cmbempname);

                    if (cmbempname.SelectedValue !="")
                    {
                        Genaral.Load_Combo("SELECT US_ID,US_FULL_NAME FROM TBLUSER WHERE US_ID='" + cmbempname.SelectedValue + "'", cmbname);
                        //Genaral.Load_Combo("SELECT US_DESG_ID, DM_NAME FROM TBLUSER inner join TBLDESIGNMAST on US_DESG_ID = DM_DESGN_ID  WHERE US_ID='" + cmbempname.SelectedValue + "'", cmbActualdesignation);
                        Genaral.Load_Combo("SELECT US_ROLE_ID, RO_NAME FROM TBLUSER inner join TBLROLES on US_ROLE_ID = RO_ID  WHERE US_ID='" + cmbempname.SelectedValue + "'", cmbActualRole);
                        //Genaral.Load_Combo("SELECT DISTINCT IM_INC_ID,DM_NAME FROM TBL_INCHARGE_MASTER INNER JOIN TBLDESIGNMAST ON IM_INC_ID = DM_DESGN_ID   where IM_DM_ID = '" + cmbActualdesignation.SelectedValue + "' ORDER BY IM_DM_ID", "--Select--", cmbInchargeDesg);
                        Genaral.Load_Combo("SELECT DISTINCT IM_INC_ID,RO_NAME FROM TBL_INCHARGE_MASTER INNER JOIN TBLROLES ON IM_INC_ID = RO_ID where IM_RL_ID = '" + cmbActualRole.SelectedValue + "' AND IM_STATUS='1' ORDER BY IM_RL_ID", "--Select--", cmbInchargeRole);

                    }

                    else
                    {

                        cmbname.Items.Clear();
                        cmbActualRole.Items.Clear();
                        cmbInchargeRole.Items.Clear();


                    }
                    GenerateAutoOmNo();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "Page_Load");
            }
        }

       protected void cmbempname_SelectedIndexChanged(object sender,EventArgs e)
        {
            try
            {
                if (cmbempname.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT US_ID,US_FULL_NAME FROM TBLUSER WHERE US_ID='" + cmbempname.SelectedValue + "'", cmbname);
                    Genaral.Load_Combo("SELECT US_DESG_ID, DM_NAME FROM TBLUSER inner join TBLDESIGNMAST on US_DESG_ID = DM_DESGN_ID  WHERE US_ID='" + cmbempname.SelectedValue + "'", cmbActualRole);
                    //Genaral.Load_Combo("SELECT DISTINCT IM_DM_ID,DM_NAME FROM TBL_INCHARGE_MASTER INNER JOIN TBLDESIGNMAST ON IM_DM_ID = DM_DESGN_ID ORDER BY IM_DM_ID", "--Select--", cmbInchargeDesg);
                    Genaral.Load_Combo("SELECT DISTINCT IM_INC_ID,DM_NAME FROM TBL_INCHARGE_MASTER INNER JOIN TBLDESIGNMAST ON IM_INC_ID = DM_DESGN_ID   where IM_DM_ID = '"+ cmbActualRole.SelectedValue+ "' AND IM_STATUS='1' ORDER BY IM_DM_ID", "--Select--", cmbInchargeRole);

                }

                else
                {

                    cmbname.Items.Clear();
                    cmbActualRole.Items.Clear();
                    cmbInchargeRole.Items.Clear();


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbempname_SelectedIndexChanged");
            }
        }
        //protected void cmb_todate(object sender,EventArgs e)
        //{
        //    string fromdate = txtfromdate.Text;
        //    DateTime DToDate = DateTime.ParseExact(fromdate, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //    string stodate = DToDate.ToString("yyyy/MM/dd");
        //    OMDateCalender.StartDate = Convert.ToDateTime(stodate);
        //}

        protected void cmbInchargeRole_SelectedIndexChanged(object sender,EventArgs e)
        {
            try
            {
                if (cmbInchargeRole.SelectedIndex > 0)
                {
                    //Genaral.Load_Combo("SELECT US_ID,US_FULL_NAME FROM TBLUSER WHERE US_DESG_ID='" + cmbInchargeDesg.SelectedValue + "' AND US_OFFICE_CODE = '"+objSession.OfficeCode+"'", "--Select--", cmbHandoverEmp);
                    Genaral.Load_Combo("SELECT US_ID, US_FULL_NAME FROM TBLUSER WHERE US_ROLE_ID = '" + cmbInchargeRole.SelectedValue + "' AND US_OFFICE_CODE = '" + objSession.OfficeCode + "' and US_STATUS = 'A'", "--Select--", cmbHandoverEmp);
                }

                else
                {
                    cmbHandoverEmp.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbInchargeDesg_SelectedIndexChanged");
            }
        }

        public void GenerateAutoOmNo()
        {
            try
            {
                clsIncharge objIncharge = new clsIncharge();

                txtAutoOmnumber.Text = objIncharge.GenerateAutoOmNo();
                txtAutoOmnumber.ReadOnly = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "GenerateAutoOmNo");
            }
        }
        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateForm() == true)
                {
                    clsIncharge objIncharge = new clsIncharge();
                    string[] Arr = new string[2];


                    objIncharge.sEmployeeName = cmbname.SelectedValue;
                    objIncharge.sActualRoleID = cmbActualRole.SelectedValue;
                    objIncharge.sInchargeRoleID = cmbInchargeRole.SelectedValue;
                    objIncharge.sHandoverEmp = cmbHandoverEmp.SelectedValue;
                    objIncharge.sFromDate = txtfromdate.Text;
                    objIncharge.sToDate = txttodate.Text;
                    objIncharge.sRemarks = txtRemakrs.Text;
                    objIncharge.sAutoOmNumber = txtAutoOmnumber.Text;
                    objIncharge.sOmNumber = txtOmnumber.Text;
                    objIncharge.sCrby = objSession.UserId;
                    objIncharge.sOfficeCode = objSession.OfficeCode;
                    objIncharge.sOmdate = txtomdate.Text;

                    DateTime FromDate = DateTime.ParseExact(txtfromdate.Text, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string sFromDate = FromDate.ToString("yyyy/MM/dd");
                    DateTime ToDate = DateTime.ParseExact(txttodate.Text, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string sTodate = ToDate.ToString("yyyy/MM/dd");

                    DateTime d1 = Convert.ToDateTime(sFromDate);
                    DateTime d2 = Convert.ToDateTime(sTodate);

                    TimeSpan t = d1 - d2;

                    double NrOfDays = t.TotalDays-1;
                    objIncharge.sTotalDays = Convert.ToString(NrOfDays).Trim('-');
               



                    Arr = objIncharge.SaveInchargeDetails(objIncharge);
                    if (Arr[1].ToString() == "0")
                    {
                        ShowMsgBox("Saved Successfully");
                        cmdSave.Enabled = false;
                        cmdReset.Enabled = false;
                        return;
                    }
                    if (Arr[1].ToString() == "1")
                    {
                        ShowMsgBox("Incharge Already Exist");
                        return;
                    }


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdSave_Click");

            }
        }

        bool ValidateForm()
        {
            bool bValidate = false;
            try
            {
        
       
                //if (cmbempname.SelectedIndex == 0)
                //{
                //    cmbempname.Focus();
                //    ShowMsgBox("Please Select Employee Login Name");
                //    return bValidate;
                //}
                if (cmbInchargeRole.SelectedIndex == 0)
                {
                    cmbInchargeRole.Focus();
                    ShowMsgBox("Please Select Incharge Role");
                    return bValidate;
                }
                if (cmbHandoverEmp.SelectedIndex == 0)
                {
                    cmbHandoverEmp.Focus();
                    ShowMsgBox("Please Select Charge Handover Emplyee");
                    return bValidate;
                }

                if (txtfromdate.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Please Select From Date");
                    txtfromdate.Focus();
                    return bValidate;
                }

                if (txttodate.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Please Select To Date");
                    txttodate.Focus();
                    return bValidate;
                }


                if (txtOmnumber.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Please Enter Om Number");
                    txtOmnumber.Focus();
                    return bValidate;
                }


                if (txtomdate.Text.Trim().Length == 0)
                {
                    ShowMsgBox("Please Select OM Date");
                    txtomdate.Focus();
                    return bValidate;
                }

                string sResult = Genaral.DateComparision(txttodate.Text, txtfromdate.Text, false, false);
                if (sResult == "2")
                {
                    ShowMsgBox("To Date should be Greater than From Date");
                    return bValidate;
                }



                bValidate = true;
                return bValidate;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ValidateForm");
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
        protected void cmdReset_Click(object sender,EventArgs e)
        {
            try
            {
                cmbempname.SelectedIndex = 0;
                cmbname.SelectedIndex = 0;
                cmbActualRole.SelectedIndex = 0;
                cmbInchargeRole.SelectedIndex = 0;
                cmbHandoverEmp.SelectedIndex = 0;    
                txtfromdate.Text = string.Empty;
                txtomdate.Text = string.Empty;
                txttodate.Text = string.Empty;
                txtOmnumber.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdReset_Click");
            }
        }
        protected void cmdClose_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/MasterForms/InchargeView.aspx", false); 
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmdClose_Click");
            }
        }

    }
}