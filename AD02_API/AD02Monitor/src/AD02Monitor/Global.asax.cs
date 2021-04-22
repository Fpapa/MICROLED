using System;
using System.Web;

namespace AD02Monitor
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {

        }

        void Application_Error(object sender, EventArgs e)
        {
           //Response.Redirect("Erro.aspx");
        }
    }
}