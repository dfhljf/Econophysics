<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Client.aspx.cs" Inherits="Interface.Client" %>

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

                    <div style="width: 100%; height: 10%; text-align: center; background-color: #333333; font-size: x-large;">
                        <asp:Label ID="ExpInfo" runat="server">Label</asp:Label>
                    </div>
                    <ul style="list-style: none; height: 20%; width: 100%; margin: 0 0 0 0; padding: 0 0 0 0; text-align: center; font-size: x-large; border-top: solid; border-color: white; border-width: 2px;">
                        <li style="width: 10%; background-color: #FFCCFF;">编号<br />
                            <asp:Label ID="Id" runat="server" Text="Label"></asp:Label>
                        </li>
                        <li style="width: 20%; background-color: #FFCCFF;">轮次<br />
                            <asp:Label ID="Turn" runat="server" Text="Label"></asp:Label>
                        </li>
                        <li style="width: 20%; background-color: #FFCCFF;">倒计时<br />
                            <asp:Label ID="TimeTick" runat="server" Text="Label" Style="color: red; font-weight: bold;"></asp:Label></li>
                        <li style="width: 20%; background-color: #FFFF99;">现金<br />
                            <asp:Label ID="Cash" runat="server" Text="Label"></asp:Label></li>
                        <li style="width: 20%; background-color: #FFFF99;">股票<br />
                            <asp:Label ID="Stocks" runat="server" Text="Label"></asp:Label></li>
                        <li style="width: 10%; background-color: #FFFF99;">总资产<br />
                            <asp:Label ID="Endowment" runat="server" Text="Label"></asp:Label></li>
                    </ul>

                    <table id="PriceImage" runat="server" style="float: left;">
                        <tr>
                            <td>
                                <object data="PriceImage.svg" width="900" height="500" type="image/svg+xml" />
                            </td>
                        </tr>
                    </table>

                    <div style="float: left; height: 30%; width: 15%; text-align: center; font-size: xx-large; background-color: #99CCFF; padding: 6% 1% 6% 1%;">
                        <strong>分红比例<br />
                            <asp:Label ID="Dividend" runat="server" Text="Label" Style="color: red; font-weight: bold;"></asp:Label></strong>
                    </div>
                    <div style="float: left; height: 30%; width: 13%; text-align: center; font-size: x-large; background-color: #99CCFF; padding: 1.5% 1% 1.5% 1%;">
                        分红持续<br />
                        轮数<br />
                        <asp:Label runat="server" ID="DividendTime" Text="Label"></asp:Label>
                    </div>

                    <div style="float: left; width: 13%; text-align: center; font-size: x-large; background-color: #99CCFF; padding: 1.5% 1% 1.9% 1%;">
                        下一轮分红收入<br />
                        <asp:Label runat="server" ID="DividendIncome" Text="Label" />
                    </div>
                </ContentTemplate>
                <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="Buy" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="Sell" EventName="Click" />
                                        </Triggers>
            </asp:UpdatePanel>

            <ul style="list-style: none; height: 15%; width: 32%; margin: 0 0 0 0; padding: 3% 0 0 0; text-align: center; font-size: x-large; background-color: #99FF99;">
                <li style="width: 30%;">最大交易数<br />
                    <asp:Label runat="server" ID="MaxStocks" Text="Label" />
                </li>

                <li style="width: 40%;">交易数量<br />
                    <asp:TextBox ID="TradeStocks" runat="server" Text="Label" Font-Size="X-Large" Width="80%"></asp:TextBox></li>
                <li style="width: 30%;">交易费<br />
                    <asp:Label runat="server" ID="TradeFee" Text="Label" />
                </li>
            </ul>
            <div style="height: 15%; width: 32%; float: left; background-color: #99FF99;">
                <asp:Button ID="Sell" runat="server" Text="卖" OnClick="Sell_Click" AccessKey="-" Height="80%" BackColor="#6699FF" Width="45%" Font-Size="X-Large" Style="margin: 2% 0 0 3%;" />
                <asp:Button ID="Buy" runat="server" Text="买" OnClick="Buy_Click" AccessKey="+" Height="80%" BackColor="#FF5050" Width="45%" Font-Size="X-Large" Style="margin: 2% 0 0 3%;" />

            </div>
        </div>
    </form>
</body>
</html>
