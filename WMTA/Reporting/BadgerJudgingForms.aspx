﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="BadgerJudgingForms.aspx.cs" Inherits="WMTA.Reporting.BadgerJudgingForms" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="well bs-component col-md-6 main-div center">
            <section id="registrationForm">
                <asp:UpdatePanel ID="upFullPage" runat="server">
                    <ContentTemplate>
                        <div class="form-horizontal">
                            <%-- Start of form --%>
                            <fieldset>
                                <legend id="legend" runat="server">Audition Search</legend>
                                <%-- Audition search --%>
                                <asp:UpdatePanel ID="upSearch" runat="server">
                                    <ContentTemplate>
                                        <div>
                                            <h4>Select an Audition to Retrieve Reports On</h4>
                                            <br />
                                            <div class="form-group">
                                                <div class="col-md-3-margin">
                                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlDistrictSearch" CssClass="txt-danger vertical-center font-size-12" ErrorMessage="Region is required"></asp:RequiredFieldValidator>
                                                </div>
                                                <asp:Label runat="server" AssociatedControlID="ddlDistrictSearch" CssClass="col-md-3 control-label float-left">Region *</asp:Label>
                                                <div class="col-md-6">
                                                    <asp:DropDownList ID="ddlDistrictSearch" runat="server" CssClass="dropdown-list form-control" DataSourceID="SqlDataSource1" DataTextField="GeoName" DataValueField="GeoId" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlDistrictSearch_SelectedIndexChanged" AutoPostBack="true" >
                                                        <asp:ListItem Selected="True" Text="" Value=""></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:WmtaConnectionString %>" SelectCommand="sp_DropDownStateDistricts" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                                                </div>
                                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-min-width-72" OnClick="btnSearch_Click" />
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-3-margin">
                                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlYear" CssClass="txt-danger vertical-center font-size-12" ErrorMessage="Year is required"></asp:RequiredFieldValidator>
                                                </div>
                                                <asp:Label runat="server" AssociatedControlID="ddlYear" CssClass="col-md-3 control-label">Year *</asp:Label>
                                                <div class="col-md-6">
                                                    <asp:DropDownList ID="ddlYear" runat="server" CssClass="dropdown-list form-control" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" AutoPostBack="true" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="ddlTeacher" CssClass="col-md-3 control-label">Teacher</asp:Label>
                                                <div class="col-md-6">
                                                    <asp:DropDownList ID="ddlTeacher" runat="server" CssClass="dropdown-list form-control" AppendDataBoundItems="true" />
                                                </div>
                                            </div>
                                            <div class="center text-align-center">
                                                <label class="text-info smaller-font">Please be patient after clicking 'Search'.  Your reports may take several minutes.</label>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <%-- End Audition Search --%>
                            </fieldset>
                        </div>
                        <label id="lblErrorMessage" runat="server" style="color: transparent">.</label>
                        <label id="lblWarningMessage" runat="server" style="color: transparent">.</label>
                        <label id="lblInfoMessage" runat="server" style="color: transparent">.</label>
                        <label id="lblSuccessMessage" runat="server" style="color: transparent">.</label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </section>
        </div>
    </div>
    <div class="col-md-12">
        <div>
            <div class="text-align-center"><h3 class="center">Piano Judging Form</h3></div>
            <rsweb:ReportViewer ID="rptPianoForm" runat="server" CssClass="report-viewer"></rsweb:ReportViewer>
        </div>
        <div>
            <div class="text-align-center"><h3 class="center">Organ Judging Form</h3></div>
            <rsweb:ReportViewer ID="rptOrganForm" runat="server" CssClass="report-viewer"></rsweb:ReportViewer>
        </div>
        <div>
            <div class="text-align-center"><h3>Vocal Judging Form</h3></div>
            <rsweb:ReportViewer ID="rptVocalForm" runat="server" CssClass="report-viewer"></rsweb:ReportViewer>
        </div>
        <div>
            <div class="text-align-center"><h3 class="center">Strings Judging Form</h3></div>
            <rsweb:ReportViewer ID="rptStringsForm" runat="server" CssClass="report-viewer"></rsweb:ReportViewer>
        </div>
        <div>
            <div class="text-align-center"><h3 class="center">Instrumental Judging Form</h3></div>
            <rsweb:ReportViewer ID="rptInstrumentalForm" runat="server" CssClass="report-viewer"></rsweb:ReportViewer>
        </div>
    </div>
    <script>
        //show an error message
        function showMainError() {
            var message = $('#MainContent_lblErrorMessage').text();

            $.notify(message.toString(), { position: "left-top", className: "error" });
        };

        //show a warning message
        function showWarningMessage() {
            var message = $('#MainContent_lblWarningMessage').text();

            $.notify(message.toString(), { position: "left-top", className: "warning" });
        };

        //show an informational message
        function showInfoMessage() {
            var message = $('#MainContent_lblInfoMessage').text();

            $.notify(message.toString(), { position: "left-top", className: "info" });
        };
    </script>
</asp:Content>
