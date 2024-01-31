using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;

namespace IIITS.DTLMS.DashboardForm
{
    public partial class TutorialVideos : System.Web.UI.Page
    {
        string strFormCode = "TutorialVideos";

        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
               try
            {
                if (Session["clsSession"] == null || Convert.ToString(Session["clsSession"]) == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                else
                {
 
                   objSession = (clsSession)Session["clsSession"];
                     
                

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(strFormCode, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }



        }
    }
