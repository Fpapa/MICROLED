<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="AD02Monitor.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CSS" runat="server">

    <link href="Content/css/dashboard.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:HiddenField ID="hddnGUID" ClientIDMode="Static" Value="" runat="server" />

    <br />
    <br />

    <div class="row">
        <div class="col-md-10 col-md-offset-1">

           <%-- <div class="panel panel-primary">

                <div class="panel-body">--%>

                    <div class="row">
                        <div class="col-md-6">
                            <div class="card-contador primary">
                                <i class="fa fa-cogs"></i>
                                <span class="numeros">
                                    <asp:Label ID="txtTotal" runat="server"></asp:Label>
                                </span>
                                <span class="nome">Total Eventos</span>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="card-contador warning">
                                <i class="fa fa-exclamation-triangle"></i>
                                <span class="numeros">
                                    <asp:Label ID="txtPendentes" runat="server"></asp:Label>
                                </span>
                                <span class="nome">Pendentes</span>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="card-contador danger">
                                <i class="fa fa-ban"></i>
                                <span class="numeros">
                                    <asp:Label ID="txtComErro" runat="server"></asp:Label>
                                </span>
                                <span class="nome">Com falha</span>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="card-contador success">
                                <i class="fa fa-check"></i>
                                <span class="numeros">
                                    <asp:Label ID="txtEnviados" runat="server"></asp:Label>
                                </span>
                                <span class="nome">Enviados</span>
                            </div>
                        </div>

                    </div>


               <%-- </div>
            </div>--%>
        </div>
    </div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">

    <script src="Content/js/select2.min.js"></script>
    <script src="Content/js/jquery.mask.min.js"></script>

    <script>



</script>

</asp:Content>
