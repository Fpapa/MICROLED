<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AD02Monitor.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CSS" runat="server">
    <link href="Content/css/select2.css" rel="stylesheet" />
    <link href="Content/css/dashboard.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:HiddenField ID="hddnGUID" ClientIDMode="Static" Value="" runat="server" />
    
    <asp:UpdateProgress ID="UpdateProgress1" runat="server"
        AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="10">
        <ProgressTemplate>
            <div class="updateModal">
                
            </div>
            <div align="center" class="updateModalBox">
                </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <br />

            <div class="row" style="margin-bottom: 20px;">
                <div class="col-md-12">

                    <%--<div class="panel panel-primary">

                        <div class="panel-body">--%>

                            <div class="row">
                                <div class="col-md-3">
                                    <div class="card-contador primary">
                                        <i class="fa fa-cogs"></i>
                                        <span class="numeros">
                                            <asp:Label ID="txtTotal" runat="server"></asp:Label>
                                        </span>
                                        <span class="nome">Total Eventos</span>
                                    </div>
                                </div>

                                <div class="col-md-3">
                                    <div class="card-contador warning">
                                        <i class="fa fa-exclamation-triangle"></i>
                                        <span class="numeros">
                                            <asp:Label ID="txtPendentes" runat="server"></asp:Label>
                                        </span>
                                        <span class="nome">Pendentes</span>
                                    </div>
                                </div>

                                <div class="col-md-3">
                                    <div class="card-contador danger">
                                        <i class="fa fa-ban"></i>
                                        <span class="numeros">
                                            <asp:Label ID="txtComErro" runat="server"></asp:Label>
                                        </span>
                                        <span class="nome">Com falha</span>
                                    </div>
                                </div>

                                <div class="col-md-3">
                                    <div class="card-contador success">
                                        <i class="fa fa-check"></i>
                                        <span class="numeros">
                                            <asp:Label ID="txtEnviados" runat="server"></asp:Label>
                                        </span>
                                        <span class="nome">Enviados</span>
                                    </div>
                                </div>

                            </div>


                        <%--</div>
                    </div>--%>
                </div>
            </div>

            <div class="row">
                <div class="col-md-12">

                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <span class="panel-title">
                                <i class="fa fa-cogs"></i>&nbsp; Consultar Eventos

                            </span>
                            <span class="pull-right">
                                <a runat="server" id="lnkAbrirFiltro" href="#" class="link-filtro" onserverclick="lnkAbrirFiltro_ServerClick">
                                    <i class="fa fa-filter"></i>&nbsp; Filtrar Eventos

                                </a>
                            </span>
                        </div>
                        <div class="panel-body">

                            <asp:Panel ID="pnlFiltro" runat="server" Visible="false" CssClass="row no-gutter pnlFiltro">

                                <table>
                                    <tr>
                                        <td>
                                            <img src="Content/imagens/icone-procurar.png" width="64" height="64" />
                                        </td>
                                        <td>

                                            <div class="col-md-2">
                                                <label>Evento:</label>
                                                <asp:DropDownList ID="cbEventoFiltro" runat="server" CssClass="form-control">
                                                    <asp:ListItem Value="">Todos</asp:ListItem>
                                                    <asp:ListItem Value="1">Armazenamento de Lote</asp:ListItem>
                                                    <asp:ListItem Value="2">Troca de Navio</asp:ListItem>
                                                    <asp:ListItem Value="3">Avarias/Extravios de Lote</asp:ListItem>
                                                    <asp:ListItem Value="4">Acesso de Pessoas</asp:ListItem>
                                                    <asp:ListItem Value="5">Acesso de Veículos</asp:ListItem>
                                                    <asp:ListItem Value="6">Embarque/Desembarque de Navio</asp:ListItem>
                                                    <asp:ListItem Value="7">Credenciamento de Pessoas</asp:ListItem>
                                                    <asp:ListItem Value="8">Credenciamento de Veículos</asp:ListItem>
                                                    <asp:ListItem Value="9">Geração de Lote</asp:ListItem>
                                                    <asp:ListItem Value="10">Pesagem de Veículos/Cargas</asp:ListItem>
                                                    <asp:ListItem Value="11">Posição do Contêiner</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <label>Conteúdo:</label>
                                                <asp:TextBox ID="txtConteudo" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-md-1">
                                                <label>De:</label>
                                                <asp:TextBox ID="txtDataEnvioDeFiltro" runat="server" CssClass="form-control data" placeholder="__/__/____"></asp:TextBox>
                                            </div>
                                            <div class="col-md-1">
                                                <label>Até:</label>
                                                <asp:TextBox ID="txtDataEnvioAteFiltro" runat="server" CssClass="form-control data" placeholder="__/__/____"></asp:TextBox>
                                            </div>
                                            <div class="col-md-2">
                                                <label>Protocolo:</label>
                                                <asp:TextBox ID="txtNumeroProtocoloFiltro" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-md-2">
                                                <label>Status:</label>
                                                <asp:DropDownList ID="cbStatus" runat="server" CssClass="form-control">
                                                    <asp:ListItem Value="">Todos</asp:ListItem>
                                                    <asp:ListItem Value="1">Pendente</asp:ListItem>
                                                    <asp:ListItem Value="2">Envio com Falhas</asp:ListItem>
                                                    <asp:ListItem Value="3">Enviado</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <div>&nbsp;</div>
                                                <div style="margin-top: 6px;">
                                                    <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" CssClass="btn btn-info" OnClick="btnFiltrar_Click" />
                                                    <asp:Button ID="btnLimparFiltro" runat="server" Text="Limpar" CssClass="btn btn-secondary" OnClick="btnLimparFiltro_Click" />
                                                </div>
                                            </div>

                                        </td>
                                    </tr>
                                </table>


                            </asp:Panel>

                            <div class="row no-gutter" style="margin:0;">
                                <div class="col-md-12" style="padding:0;">
                                    <div class="table-responsive">
                                        <asp:GridView
                                            ID="gvEstoque"
                                            runat="server"
                                            Width="100%"
                                            CssClass="table table-sm table-hover tbEventos"
                                            GridLines="None"
                                            AutoGenerateColumns="False"
                                            Font-Size="14px"                                            
                                            ShowHeaderWhenEmpty="True" 
                                            DataKeyNames="Id" 
                                            AllowPaging="True" 
                                            PageSize="7" 
                                            OnPageIndexChanging="gvEstoque_PageIndexChanging" 
                                            OnRowDataBound="gvEstoque_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <a href="JavaScript:divexpandcollapse('div<%# Eval("Id") %>');">
                                                            <img id="imgdiv<%# Eval("Id") %>" border="0" src="Content/imagens/plus.png" alt="" /></a>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20px" VerticalAlign="Middle"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="EventoId" HeaderText="Evento" />
                                                <asp:BoundField DataField="Evento" HeaderText="Descrição" />
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <a href="#" onclick="ExibirJson(<%# Eval("Id") %>)">Exibir Conteúdo</a>
                                                        <div id="pnlJson<%# Eval("Id") %>" style="display: none;"><%# Eval("Json") %></div>
                                                        <div id="pnlJsonPretty<%# Eval("Id") %>" style="display: none;"></div>
                                                    </ItemTemplate>
                                                    <ItemStyle VerticalAlign="Middle"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="DataEnvio" HeaderText="Data Último Envio" />
                                                <asp:BoundField DataField="CodigoRetorno" HeaderText="Codigo Retorno" NullDisplayText="N/A" />
                                                <asp:BoundField DataField="Protocolo" HeaderText="Protocolo" NullDisplayText="N/A" />
                                                <asp:BoundField DataField="Tentativas" HeaderText="Número Tentativas" NullDisplayText="0" />
                                                <asp:TemplateField ItemStyle-Width="60px">
                                                    <ItemTemplate>
                                                        <asp:Panel ID="pnlPendente" runat="server" Visible='<%# Eval("Pendente") %>'>
                                                            <span class='label label-warning'>Envio Pendente</span>
                                                        </asp:Panel>
                                                        <asp:Panel ID="Panel3" runat="server" Visible='<%# Eval("EnvioComFalhas") %>'>
                                                            <span class='label label-danger'>Envio com Falhas</span>
                                                        </asp:Panel>
                                                        <asp:Panel ID="Panel2" runat="server" Visible='<%# Eval("ComErro") %>'>
                                                            <span class='label label-danger'>Erro</span>
                                                        </asp:Panel>
                                                        <asp:Panel ID="Panel1" runat="server" Visible='<%# Eval("Enviado") %>'>
                                                            <span class='label label-success'>Enviado</span>
                                                        </asp:Panel>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20px" VerticalAlign="Middle"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td colspan="11" style="padding: 0px; border: 0px;">

                                                                <div class="col-md-12" style="background: white;">
                                                                    <div id="div<%# Eval("Id") %>" style="display: none; position: relative; left: 15px; top: 10px; white-space: nowrap;">

                                                                        <div class="table-responsive">
                                                                            <asp:GridView ID="gvEstoqueDetalhes" Font-Size="11px" runat="server" AutoGenerateColumns="False"
                                                                                ForeColor="#333333" CssClass="table table-striped" EmptyDataText="Nenhum registro encontrado.">
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="DataRetorno" HeaderText="Data Retorno" />
                                                                                    <asp:BoundField DataField="MensagemRetorno" HeaderText="Mensagem Retorno" />
                                                                                    <asp:BoundField DataField="CodigoRetorno" HeaderText="Codigo Retorno" />
                                                                                </Columns>

                                                                            </asp:GridView>
                                                                        </div>

                                                                    </div>
                                                                </div>


                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle CssClass="pagination-ys" HorizontalAlign="Right" />
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">

    <script src="Content/js/select2.min.js"></script>
    <script src="Content/js/jquery.mask.min.js"></script>

    <script>

        $('#MainContent_txtDataEnvioDeFiltro').mask('00/00/0000');
        $('#MainContent_txtDataEnvioAteFiltro').mask('00/00/0000');

        function divexpandcollapse(divname) {

            var div = document.getElementById(divname);
            var img = document.getElementById('img' + divname);

            if (div.style.display === "none") {
                div.style.display = "inline";
                img.src = "Content/imagens/minus.png";
            } else {
                div.style.display = "none";
                img.src = "Content/imagens/plus.png";
            }
        }

        function abrirFiltro() {
            $('#<%= pnlFiltro.ClientID %>').toggle('slow');
        }

        function ExibirJson(id, strJson) {

            var json = $('#pnlJson' + id).html();

            $('#pnlJsonPretty' + id).html('<pre>' + JSON.stringify(JSON.parse(json), null, '\t') + '</pre>').toggle('slow');
        }

    </script>

</asp:Content>
