<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WepAPI._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <link rel="stylesheet" href="http://cdn.kendostatic.com/2014.3.1316/styles/kendo.common.min.css" />
    <link rel="stylesheet" href="http://cdn.kendostatic.com/2014.3.1316/styles/kendo.default.min.css" />
    <link rel="stylesheet" href="http://cdn.kendostatic.com/2014.3.1316/styles/kendo.dataviz.min.css" />
    <link rel="stylesheet" href="http://cdn.kendostatic.com/2014.3.1316/styles/kendo.dataviz.default.min.css" />

<div ng-app="APP">

    <div ng-controller="membershipController">
          <div style="width:300px; margin-top:5px">
                <div id="SearchMember">
                    <input class="form-control" type="text" ng-model="search" placeholder="Search" style="width: 200px">
                </div>
                <div id="MemberListHeader">
                    <table>
                        <thead>
                            <tr>
                                <th>Members </th>
                                <td style="text-align: right; width: 100%">
                                    <a ng-click="ReLoadMembers()">
                                        <span data-ng-hide="loading">
                                            <i class="glyphicon glyphicon-refresh"></i>
                                        </span>
                                        <span data-ng-show="loading">
                                            <i class="glyphicon glyphicon-refresh glyphicon-refresh-animate"></i>
                                        </span>
                                    </a>

                                </td>
                            </tr>
                        </thead>
                    </table>
                </div>
                <div id="MemberList" style="height: calc(100% - 100px); overflow: auto">
                    <table class="table table-striped table-hover">
                        <tbody>
                            <tr ng-repeat="x in Members | filter:search" ng-click="select($event, x.Id)" data-ng-class="{success: isSelected(x.Id)}" style="cursor: pointer">
                                <td>{{ x.FirstName + ', ' + x.LastName }}</td>
                            </tr>
                        </tbody>
                    </table>

                </div>
            </div>
        <div kendo-window="win2" k-title="'New Member'"
            k-width="920" k-height="550" k-visible="false"
            k-content="{ url: '../member' }"
            k-on-open="win2visible = true"
            k-on-close="ShowMessage(e,'warning');"
            k-modal="true">
        </div>
    </div>


</div>

</asp:Content>
