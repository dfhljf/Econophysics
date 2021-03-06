﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Client.aspx.cs" Inherits="Interface.Client" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        html, body, form#form1 {
            height: 100%;
        }

        li, ul {
            float: left;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" style="font-family: 'Microsoft YaHei'">
        <div style="height: 100%; width: 100%;">
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <asp:Timer ID="Timer1" runat="server" Interval="1000" OnTick="Timer1_Tick"></asp:Timer>

                    <div style="width: 100%; height: 15%; text-align: center;padding: 1% 0 1% 0; background-color: #333333; font-size: x-large;color:white;">
                        <asp:Label ID="ExpInfo" runat="server">Label</asp:Label>
                    </div>
                    <div style="height: 25%; width: 100%;">
                    <ul style="list-style: none;height:100%; width: 100%; margin: 0 0 0 0; padding: 0 0 0 0; text-align: center; font-size: x-large; border-top: solid; border-color: white; border-width: 2px;">
                        <li style="width: 10%; background-color: #FFCCFF; padding: 1% 0 1% 0;">编号<br />
                            <asp:Label ID="Id" runat="server" Text="Label"></asp:Label>
                        </li>
                        <li style="width: 20%; background-color: #FFCCFF;padding: 1% 0 1% 0;">轮次<br />
                            <asp:Label ID="Turn" runat="server" Text="Label"></asp:Label>
                        </li>
                        <li style="width: 20%; background-color: #FFCCFF;padding: 1% 0 1% 0;">倒计时<br />
                            <asp:Label ID="TimeTick" runat="server" Text="Label" Style="color: red; font-weight: bold;"></asp:Label></li>
                        <li style="width: 20%; background-color: #FFFF99;padding: 1% 0 1% 0;">现金<br />
                            <asp:Label ID="Cash" runat="server" Text="Label"></asp:Label></li>
                        <li style="width: 20%; background-color: #FFFF99;padding: 1% 0 1% 0;">股票<br />
                            <asp:Label ID="Stocks" runat="server" Text="Label"></asp:Label></li>
                        <li style="width: 10%; background-color: #FFFF99;padding: 1% 0 1% 0;">总资产<br />
                            <asp:Label ID="Endowment" runat="server" Text="Label"></asp:Label></li>
                    </ul>
                    </div>
               <table style="float: left;">
                        <tr>
                            <td id="PriceImage" runat="server">
                                <object data="PriceImage.svg" width="900" height="500" type="image/svg+xml" />
                            </td>
                        </tr>
                    </table>
                    <table style="float:right;width:31.7%;height:60%;text-align:center;background-color:#99ccff;font-size:x-large;">
                        <tr style="height:50%;">
                            <td rowspan="2" style="padding:10% 0 10% 0;">
<strong>分红比例<br /><br/>
                            <asp:Label ID="Dividend" runat="server" Text="Label" Style="color: red; font-weight: bold;" Font-Size="XX-Large" ></asp:Label></strong>
                            </td>
                                                        <td style="padding:10% 0 10% 0;">持续轮数<br />
                        <asp:Label runat="server" ID="DividendTime" Text="Label" Font-Size="XX-Large" ></asp:Label></td>
                        </tr>
                        <tr style:"height:50%;">
                            
                            <td style="padding:10% 0 10% 0;">下一轮分红收入<br />
                        <asp:Label runat="server" ID="DividendIncome" Text="Label" Font-Size="XX-Large" /></td>
                        </tr>
                    </table>


                                <div style="position:absolute;right:0.5%;top:71%;height: 15%; width: 31.5%; float: left; background-color: #99FF99;">
                <asp:Button ID="Sell" runat="server" Text="卖" OnClick="Sell_Click" Height="80%" BackColor="#6699FF" Width="45%" Font-Size="XX-Large" Font-Names="Microsoft YaHei" Style="margin: 2% 0 0 3%;" />
                <asp:Button ID="Buy" runat="server" Text="买" OnClick="Buy_Click" Height="80%" BackColor="#FF5050" Width="45%" Font-Size="XX-Large" Font-Names="Microsoft YaHei" Style="margin: 2% 0 0 3%;" />

            </div>

                </ContentTemplate>
<%--                <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="Buy" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="Sell" EventName="Click" />
                                        </Triggers>--%>
            </asp:UpdatePanel>
            <div style="position:absolute;right:0.5%;top:55.5%;height: 20%; width: 31.5%;">
            <ul style="height:60%; list-style: none;  margin: 0 0 0 0; padding:10% 0 0 0; text-align: center; font-size: x-large; background-color: #99FF99;width:100%;">
                <li style="width: 35%;">最大交易数量<br />
                    <asp:Label runat="server" ID="MaxStocks" Text="Label" />
                </li>

                <li style="width: 35%;">交易数量<br />
                   <%-- <asp:TextBox ID="TradeStocks" runat="server" Font-Size="X-Large" Width="80%"></asp:TextBox>--%>
                    <asp:RadioButtonList runat="server" ID="TradeStocks" RepeatDirection="Horizontal" Font-Size="X-Large">
                        <asp:ListItem Selected="True">1</asp:ListItem>
                        <asp:ListItem>5</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                    </asp:RadioButtonList>
                </li>
                <li style="width: 30%;">交易费<br />
                    <asp:Label runat="server" ID="TradeFee" Text="Label" />
                </li>
            </ul>
                </div>



        </div>
    </form>
</body>
</html>
