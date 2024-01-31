using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;

namespace IIITS.DTLMS.MasterForms
{
    public partial class Analysis : System.Web.UI.Page
    {
        string strFormCode = "Analysis";
        clsSession objSession = new clsSession();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
            {
                Response.Redirect("~/Login.aspx", false);
            }
            objSession = (clsSession)Session["clsSession"];
            string stroffCode = string.Empty;

            if (!IsPostBack)
            {
                Genaral.Load_Combo("SELECT CM_CIRCLE_CODE ,CM_CIRCLE_CODE  || '-' || CM_CIRCLE_NAME FROM TBLCIRCLE ORDER BY CM_CIRCLE_CODE ", "--Select--", cmbCircle);
                Genaral.Load_Combo(" SELECT MD_ID ,  MD_NAME  FROM TBLMASTERDATA  WHERE MD_TYPE = 'C' ORDER BY MD_ID ", "--SELECT--",cmbCapacity);

            }

        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            


        }

        protected void cmbCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCircle.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT DIV_CODE ,DIV_CODE  || '-' || DIV_NAME FROM TBLDIVISION ORDER BY DIV_CODE ", "--Select--", cmbDivision);
                }
                else
                {
                    cmbDivision.Items.Clear();
                }

             
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbSubDiv_SelectedIndexChanged");
            }
        }
    }
}