using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Configuration;
using System.Web.Configuration;
using IIITS.DTLMS.BL;

namespace IIITS.DTLMS
{
    public class Global : System.Web.HttpApplication
    {

        //protected void Application_PreRequestHandlerExecute(Object sender, EventArgs e)
        //{
        //    // only apply session cookie persistence to requests requiring session information
        //    if (Context.Handler is IRequiresSessionState || Context.Handler is IReadOnlySessionState)
        //    {
        //        var sessionState = ConfigurationManager.GetSection("system.web/sessionState") as SessionStateSection;
        //        var cookieName = sessionState != null && !string.IsNullOrEmpty(sessionState.CookieName) ? sessionState.CookieName : "ASP.NET_SessionId";

        //        var timeout = sessionState != null ? sessionState.Timeout : TimeSpan.FromMinutes(20);

        //        // Ensure ASP.NET Session Cookies are accessible throughout the subdomains.
        //        if (Request.Cookies[cookieName] != null && Session != null && Session.SessionID != null)
        //        {
        //            Response.Cookies[cookieName].Path = "/MyApp";
        //            Response.Cookies[cookieName].Value = Session.SessionID;
        
        //            Response.Cookies[cookieName].Expires = DateTime.Now.Add(timeout);
        //        }
        //    }
        //}
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup

        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
            string sBaseType = "C";
            string sInnerException = string.Empty;
            string sLocCode = string.Empty;
            int iErrorCode = 0;
            
         HttpException ObjException;
            try
            {
                if (Server.GetLastError().InnerException != null)
                {
                    sInnerException = Convert.ToString(Server.GetLastError().InnerException.Message);
                }
                if (HttpContext.Current.Session != null)
                {
                    sLocCode = Convert.ToString(HttpContext.Current.Session["FullName"]);
                     clsSession sessionobj = (clsSession)(HttpContext.Current.Session["clsSession"]);
                    sLocCode = sLocCode + " " +  sessionobj.OfficeCode;
                    if (sLocCode == null)
                    {
                        sLocCode = "SESSIONTIMEOUT";
                    }
                }
                else
                {
                    sLocCode = "SESSIONTIMEOUT_01";
                }
                //AppException.LogError(Convert.ToString(Server.GetLastError().GetBaseException().Message), sLocCode, "Global Error Page", Convert.ToString(Server.GetLastError().StackTrace), sInnerException, sBaseType);

                ObjException = new HttpException();
                iErrorCode = ObjException.GetHttpCode();

                clsException.ApplicationErrorLog(sLocCode, "" , "" , sInnerException + " ERROR_CODE : " + iErrorCode, "" ,  "GLOBAL_ERROR");

            }
            catch (Exception ex)
            {
                //cls.LogError(ex.Message, sLocCode, "Global Error Page", "BLANK", "BLANK", sBaseType);
            }
            finally
            {
                if (iErrorCode == 404)
                {
                    string file = HttpContext.Current.Request.Url.ToString();
                    string page = HttpContext.Current.Request.UrlReferrer.ToString();
                    //AppException.LogError(Convert.ToString(Server.GetLastError().GetBaseException().Message), sLocCode, "Global Error Page", file, page, sBaseType);
                    clsException.ApplicationErrorLog(file, Convert.ToString(iErrorCode), page, "GLOBALERROR");

                    Server.ClearError();
                    //Server.Transfer("/img/ERROR.png");
                }
                else
                {
                    Server.ClearError();
                  //  Server.Transfer("/img/ERROR.png");
                }
            }
        }

        void Session_Start(object sender, EventArgs e)
        
        {
            // Code that runs when a new session is started

        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }

    }
}
