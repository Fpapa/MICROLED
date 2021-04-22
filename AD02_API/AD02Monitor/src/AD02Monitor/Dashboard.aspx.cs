using AD02Monitor.DAO;
using AD02Monitor.Extensions;
using System;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace AD02Monitor
{
    public partial class Dashboard : System.Web.UI.Page
    {
        private ConsultaDAO _consultaDAO;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Logado"] == null)
                Response.Redirect("Login.aspx");

            _consultaDAO = new ConsultaDAO(Session["UsuarioTabela"].ToString());

            if (!Page.IsPostBack)
            {
                var totais = _consultaDAO.ObterTotais();

                if (totais != null)
                {
                    this.txtTotal.Text = totais.Total.ToString();
                    this.txtPendentes.Text = totais.Pendentes.ToString();
                    this.txtComErro.Text = totais.ComErro.ToString();
                    this.txtEnviados.Text = totais.Enviados.ToString();
                }

            }
        }
    }
}