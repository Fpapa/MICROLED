<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AD02Monitor.Login" %>

<!DOCTYPE html>
<html lang="pt-br">
<head>
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <link href="Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/css/signup.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.6.1/css/font-awesome.min.css">

    <!-- Google Fonts -->
    <link href='https://fonts.googleapis.com/css?family=Passion+One' rel='stylesheet' type='text/css'>

    <title>Login</title>
</head>
<body>

    <form runat="server">

        <div class="container">
            <div class="row main">
                <div>
                    <div class="panel-title text-center">
                        <h1 class="title logo"><img src="Content/imagens/siscomex_logo.png" /></h1>
                    </div>
                </div>

                <div class="main-login main-center-login">

                    <asp:ValidationSummary ID="Validacoes" runat="server" ShowModelStateErrors="true" CssClass="alert alert-danger" />

                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label for="txtUsuario" class="control-label">CPF:</label>
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="fa fa-user fa" aria-hidden="true"></i></span>
                                    <asp:TextBox ID="txtCpf" runat="server" AutoPostBack="true" CssClass="form-control" placeholder="___.___.___-__" OnTextChanged="txtCpf_TextChanged"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label for="txtSenha" class="control-label">Senha:</label>
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="fa fa-key fa" aria-hidden="true"></i></span>
                                    <asp:TextBox ID="txtSenha" runat="server" CssClass="form-control" placeholder="Senha" TextMode="Password"></asp:TextBox>
                                </div>
                            </div>
                        </div>                      
                    </div>      
                    
                     <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label for="txtSenha" class="control-label">Empresa:</label>
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="fa fa-cogs fa" aria-hidden="true"></i></span>
                                    <asp:DropDownList ID="cbEmpresa" runat="server" CssClass="form-control" DataTextField="Descricao" DataValueField="Id"></asp:DropDownList>                                    
                                </div>
                            </div>
                        </div>                      
                    </div>      

                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-primary btn-lg btn-block login-button" OnClick="btnLogin_Click" />
                            </div>
                        </div>
                    </div>                   

                </div>
            </div>
        </div>

    </form>

    <script src="Content/js/jquery.min.js"></script>
    <script src="Content/js/bootstrap.min.js"></script>
    <script src="Content/js/jquery.mask.min.js"></script>

    <script>
        $(document).ready(function () {
            $('#txtCpf').mask('000.000.000-00');
        });
    </script>

</body>
</html>

