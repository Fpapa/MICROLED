using AD02Monitor.DAO;
using AD02Monitor.Extensions;
using AD02Monitor.Helpers;
using System;
using System.Linq;

namespace AD02Monitor
{
    public partial class Login : System.Web.UI.Page
    {
        private UsuarioDAO _usuarioDAO = new UsuarioDAO();
        private EmpresaDAO _empresaDAO = new EmpresaDAO();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Logado"] != null)
                Response.Redirect("Default.aspx");
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtCpf.Text))
                ModelState.AddModelError(string.Empty, "O campo CPF é obrigatório");

            if (string.IsNullOrEmpty(this.txtSenha.Text))
                ModelState.AddModelError(string.Empty, "O campo Senha é obrigatório");

            if (!ModelState.IsValid)
                return;

            var usuario = _usuarioDAO.ObterUsuarioPorCpf(this.txtCpf.Text.RemoverCaracteresEspeciaisCPF());

            if (usuario != null)
            {
                if (usuario.Ativo == false)
                {
                    ModelState.AddModelError(string.Empty, "Usuário inativo (pendente liberação)");
                    return;
                }

                if (usuario.Autenticar(this.txtCpf.Text.RemoverCaracteresEspeciaisCPF().Trim(), this.txtSenha.Text.Trim()))
                {
                    Session["Logado"] = true;
                    Session["UsuarioId"] = usuario.Id;
                    Session["UsuarioNome"] = usuario.Nome;
                    Session["UsuarioCpf"] = usuario.CPF;

                    var empresaBusca = _empresaDAO.ObterEmpresaPorId(this.cbEmpresa.SelectedValue.ToInt());

                    Session["UsuarioTabela"] = empresaBusca.Tabela;
                    Session["LogoUrl"] = empresaBusca.LogoUrl;
                    Session["Empresa"] = empresaBusca.Descricao;

                    Response.Redirect("Default.aspx");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Usuário / Senha inválidos");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Usuário não registrado");
            }
        }

        protected void txtCpf_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.txtCpf.Text))
            {
                var empresas = _empresaDAO.ObterEmpresasPorUsuarioCpf(this.txtCpf.Text.RemoverCaracteresEspeciaisCPF());

                if (empresas.Any())
                {
                    this.cbEmpresa.DataSource = empresas;
                    this.cbEmpresa.DataBind();
                }

                this.txtSenha.Focus();
            }
        }
    }
}