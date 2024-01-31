using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.Internal
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            for(int a=0; a < a + 1; a++)
            {
                CalendarExtender2.StartDate = System.DateTime.Now;
                //String day = System.DateTime.Now.DayOfWeek.ToString();
                int date = System.DateTime.Now.Day;
                CalendarExtender2.StartDate = System.DateTime.Now.AddDays(-(System.DateTime.Now.Day - 1));
            }
            
        }
    }
}