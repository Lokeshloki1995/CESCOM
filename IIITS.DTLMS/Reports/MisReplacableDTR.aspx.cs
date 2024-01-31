using System;
using IIITS.DTLMS.BL;

namespace IIITS.DTLMS.Reports
{
    public partial class MisReplacableDTR : System.Web.UI.Page
    {
        String strFormName = "MisReplacableDTR";
        clsSession objSession;
        string offCode = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                objSession = (clsSession)Session["clsSession"];

                if (objSession.OfficeCode.Length > 2)
                {
                    offCode = objSession.OfficeCode.Substring(0, 1);
                }
                else
                {
                    offCode = objSession.OfficeCode;
                }

            
                if (!IsPostBack)
                {
                    //Load the Circles if  length of office code is greater than 1 
                    //if (offCode.Length >= 1)
                    //{
                    //    Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME FROM TBLCIRCLE  ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle1);
                    //    cmbCircle1.Items.FindByValue(offCode).Selected = true;
                    //    offCode = string.Empty;
                    //    offCode = objSession.OfficeCode;

                    //}
                    //if (offCode == null || offCode == "")
                    //{
                    //    Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME FROM TBLCIRCLE ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle1);
                    //}

                    //if (offCode.Length >= 1)
                    //{
                    //    Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION WHERE DIV_CICLE_CODE='" + cmbCircle1.SelectedValue + "'", "--Select--", cmbDiv1);
                    //    if (offCode.Length >= 2)
                    //    {
                    //        offCode = objSession.OfficeCode.Substring(0, 2);
                    //        cmbDiv1.Items.FindByValue(offCode).Selected = true;
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {

            }
        }


        protected void cmdReport_Click(object sender, EventArgs e)
        {
            try
            {

                string strParam = string.Empty;
                clsReports objReport = new clsReports();

                //if (cmbCircle1.SelectedIndex > 0)
                //{
                //    offCode = cmbCircle1.SelectedValue;
                //}
                //else if (cmbDiv1.SelectedIndex > 0)
                //{
                //    offCode = cmbDiv1.SelectedValue;
                //}
                //else
                //    offCode = "";

                objReport.sOfficeCode = offCode;

                strParam = "id=MisReplacableDTR&Officecode=" + objReport.sOfficeCode;
                RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormName, "cmdReport_Click");
            }
        }
    }
}