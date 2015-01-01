<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Client.aspx.cs" Inherits="Interface.Client" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        .title {
            height: 10%;
            text-align: center;
            background-color: #333333;
            font-size: x-large;
        }

        .id {
            width: 8%;
            text-align: center;
            font-size: x-large;
            background-color: #FFCCFF;
        }

        .period {
            width: 26%;
            text-align: center;
            font-size: x-large;
            background-color: #FFCCFF;
        }

        .cash {
            width: 13%;
            text-align: center;
            font-size: x-large;
            background-color: #FFFF99;
        }

        .dividend {
            height: 20%;
            text-align: center;
            font-size: xx-large;
            background-color: #99CCFF;
        }

        .dividend_income {
            height: 15%;
            text-align: center;
            font-size: x-large;
            background-color: #99CCFF;
        }

        .dividend_time {
            text-align: center;
            font-size: x-large;
            background-color: #99CCFF;
        }

        .trade {
            height: 21%;
            width: 123px;
            text-align: center;
            font-size: x-large;
            background-color: #99FF99;
        }

        .buy {
            height: 15%;
            text-align: center;
            font-size: x-large;
            background-color: #99FF99;
        }

        .tradestocks {
            text-align: center;
            font-family: 'Microsoft YaHei';
            height: 15%;
            width: 60%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Timer ID="Timer1" runat="server" Interval="1000"></asp:Timer>
                    <table style="width: 100%; height: 100%;">
                        <tr>
                            <td class="title" colspan="7"><strong>
                                <label id="expinfo" runat="server"></label>
                            </strong></td>
                        </tr>

                        <tr style="width: 100%; height: 20%;">
                            <td class="id">编号<br />
                                <asp:Label ID="id" runat="server"></asp:Label>
                            </td>
                            <td class="period">轮次<br />
                                <asp:Label ID="turn" runat="server"></asp:Label>
                            </td>
                            <td class="period">倒计时<br />
                                <asp:Label ID="time" runat="server" Style="color: red; font-weight: bold;"></asp:Label></td>


                            <td class="cash">现金<br />
                                <asp:Label ID="cash" runat="server" Text="Label"></asp:Label></td>
                            <td class="cash" colspan="2">股票<br />
                                <asp:Label ID="stocks" runat="server" Text="Label"></asp:Label></td>
                            <td class="cash">总资产<br />
                                <asp:Label ID="endowment" runat="server" Text="Label"></asp:Label></td>
                        </tr>
                        <tr>
                            <td colspan="3" rowspan="4">
                                <div id="priceimage" runat="server">
                                    <object data="priceimage.svg" width="900" height="500" type="image/svg+xml" />
                                </div>
                            </td>
                            <td class="dividend" colspan="3"><strong>分红比例<br />
                                <asp:Label ID="dividend" runat="server" Text="Label" Style="color: red; font-weight: bold;"></asp:Label></strong></td>
                            <td class="dividend_time" rowspan="2">分红持续<br />
                                轮数<br />
                                <asp:Label runat="server" ID="dividendtime"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="dividend_income" colspan="3" id="auto-style2">下一轮分红收入<br />
                                <asp:Label runat="server" ID="dividendincome" />
                            </td>
                        </tr>
                        <tr class="trade">
                            <td>最大交易数<br />
                                <asp:Label runat="server" ID="maxstocks" />
                            </td>

                            <td colspan="2">交易数量<br />
                                <asp:TextBox ID="tradeStocks" runat="server" Height="36px" Font-Size="X-Large" CssClass="tradestocks"></asp:TextBox></td>
                            <td>交易费<br />
                                <asp:Label runat="server" ID="tradefee" />
                            </td>
                        </tr>
                        <tr class="buy">
                            <td colspan="2">
                                <asp:Button ID="sell" runat="server" Text="卖" OnClick="sell_Click" AccessKey="-" Height="80%" Style="font-size: x-large; font-weight: 700; font-family: 'Microsoft YaHei'; background-color: #6699FF" Width="164px" /></td>
                            <td colspan="2">
                                <asp:Button ID="buy" runat="server" Text="买" OnClick="buy_Click" AccessKey="+" Height="80%" Style="font-size: x-large; font-weight: 700; font-family: 'Microsoft YaHei'; background-color: #FF5050" Width="164px" /></td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
