using AD02Monitor.DAO;
using AD02Monitor.Extensions;
using System;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace AD02Monitor
{
    public partial class Default : System.Web.UI.Page
    {
        private ConsultaDAO _consultaDAO;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Logado"] == null)
                Response.Redirect("Login.aspx");

            _consultaDAO = new ConsultaDAO(Session["UsuarioTabela"].ToString());

            if (!Page.IsPostBack)
            {
                Consultar();

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
        private void Consultar(string filtro = "")
        {
            this.gvEstoque.DataSource = _consultaDAO.ObterListaEnvios(filtro);
            this.gvEstoque.DataBind();
        }

        protected void gvEstoque_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            this.gvEstoque.PageIndex = e.NewPageIndex;
            Consultar(GerarFiltro());
        }

        protected void gvEstoque_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var id = gvEstoque.DataKeys[e.Row.RowIndex].Value.ToString().ToInt();

                if (id > 0)
                {
                    GridView gvEstoqueDetalhes = (GridView)e.Row.FindControl("gvEstoqueDetalhes");

                    if (gvEstoqueDetalhes != null)
                    {
                        gvEstoqueDetalhes.DataSource = _consultaDAO.ObterListaEnviosRecusados(id);
                        gvEstoqueDetalhes.DataBind();
                    }
                }
            }
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {          
            Consultar(GerarFiltro());
        }

        public string GerarFiltro()
        {
            StringBuilder filtro = new StringBuilder();

            if (this.cbEventoFiltro.SelectedValue != null)
            {
                if (this.cbEventoFiltro.SelectedValue.ToInt() > 0)
                {
                    filtro.Append(" AND Evento = " + this.cbEventoFiltro.SelectedValue.ToString() + " ");
                }
            }

            if (!string.IsNullOrWhiteSpace(this.txtConteudo.Text))
            {
                filtro.Append(" AND Envio_JSON LIKE '%" + this.txtConteudo.Text + "%' ");
            }

            if (!string.IsNullOrWhiteSpace(this.txtDataEnvioDeFiltro.Text))
            {
                filtro.Append(" AND Envio_Dth >= CONVERT(DATETIME,'" + this.txtDataEnvioDeFiltro.Text + " 00:00:00',103) ");
            }

            if (!string.IsNullOrWhiteSpace(this.txtDataEnvioAteFiltro.Text))
            {
                filtro.Append(" AND Envio_Dth <= CONVERT(DATETIME,'" + this.txtDataEnvioAteFiltro.Text + " 23:59:59',103) ");
            }

            if (!string.IsNullOrWhiteSpace(this.txtNumeroProtocoloFiltro.Text))
            {
                filtro.Append(" AND Protocolo = '" + this.txtNumeroProtocoloFiltro.Text + "' ");
            }

            if (this.cbStatus.SelectedValue != null)
            {
                if (this.cbStatus.SelectedValue.ToInt() == 1)
                {
                    filtro.Append(" AND Envio_Dth IS NULL AND Retorno_Codigo IS NULL AND ISNULL(Tentativas,0) = 0 ");
                }

                if (this.cbStatus.SelectedValue.ToInt() == 2)
                {
                    filtro.Append(" AND Envio_Dth IS NOT NULL AND Retorno_Codigo IS NULL AND ISNULL(Tentativas,0) > 0 ");
                }

                if (this.cbStatus.SelectedValue.ToInt() == 3)
                {
                    filtro.Append(" AND ISNULL(LEN(Protocolo),0) > 0 AND ISNULL(Retorno_Codigo,0) = 201 ");
                }
            }

            return filtro.ToString();
        }

        protected void lnkAbrirFiltro_ServerClick(object sender, EventArgs e)
        {
            this.pnlFiltro.Visible = !this.pnlFiltro.Visible;
        }

        protected void btnLimparFiltro_Click(object sender, EventArgs e)
        {
            this.cbEventoFiltro.SelectedIndex = -1;
            this.txtDataEnvioAteFiltro.Text = string.Empty;
            this.txtDataEnvioDeFiltro.Text = string.Empty;
            this.txtNumeroProtocoloFiltro.Text = string.Empty;

            Consultar("");
        }
    }
}