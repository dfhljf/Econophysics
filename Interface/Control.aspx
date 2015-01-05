﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Control.aspx.cs" Inherits="Interface.Control" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" AutoPostBack="True">
            <ContentTemplate>
                <div>

                    <div id="ExpInfo">
                        <table>
                            <tr>
                                <td>编号</td>
                                <td>
                                    <asp:DropDownList ID="ExpId" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ExpId_SelectedIndexChanged">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td>状态</td>
                                <td>
                                    <asp:Label ID="ExpState" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>轮次</td>
                                <td>
                                    <asp:Label ID="Turn" runat="server"></asp:Label></td>
                            </tr>

                            <tr>
                                <td>倒计时</td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Timer ID="Timer1" runat="server" Interval="1000" OnTick="Timer1_Tick"></asp:Timer>
                                            <asp:Label ID="TimeTick" runat="server"></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>

                        </table>
                    </div>

                    <div id="Parameters">
                        <table id="Agent">
                            <tr>
                                <td>代理人初始现金</td>
                                <td>
                                    <asp:TextBox ID="InitCash" runat="server">10000</asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>代理人初始股票数目</td>
                                <td>
                                    <asp:TextBox ID="InitStocks" runat="server">100</asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>代理人初始分红大小</td>
                                <td>
                                    <asp:TextBox ID="InitDividend" runat="server">1</asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>最大交易股票数目</td>
                                <td>
                                    <asp:TextBox ID="MaxStock" runat="server">10</asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>分红更新周期</td>
                                <td>
                                    <asp:TextBox ID="PeriodOfUpdateDividend" runat="server">5</asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>交易费用</td>
                                <td>
                                    <asp:TextBox ID="TradeFee" runat="server">1</asp:TextBox></td>
                            </tr>

                        </table>
                        <table id="Market">
                            <tr>
                                <td>市场初始价格</td>
                                <td>
                                    <asp:TextBox ID="InitPrice" runat="server">100</asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>初始市场状态</td>
                                <td>
                                    <asp:RadioButtonList ID="InitMarketState" runat="server" AutoPostBack="True">
                                        <asp:ListItem Value="Active" Selected="True">活跃</asp:ListItem>
                                        <asp:ListItem Value="Inactive">平稳</asp:ListItem>
                                    </asp:RadioButtonList></td>
                            </tr>
                            <tr>
                                <td>画图价格数目</td>
                                <td>
                                    <asp:TextBox ID="Count" runat="server">50</asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>杠杆效应</td>
                                <td>
                                    <asp:RadioButtonList ID="LeverageEffect" runat="server" AutoPostBack="True">

                                        <asp:ListItem Selected="True" Value="Null">无杠杆效应</asp:ListItem>
                                        <asp:ListItem Value="Leverage">杠杆效应</asp:ListItem>
                                        <asp:ListItem Value="AntiLeverage">反杠杆效应</asp:ListItem>

                                    </asp:RadioButtonList>
                                </td>

                            </tr>
                            <tr>
                                <td>价格系数</td>
                                <td>
                                    <asp:TextBox ID="Lambda" runat="server">0.01</asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>市场状态0-&gt;1转换概率</td>
                                <td>
                                    <asp:TextBox ID="P01" runat="server">0.25</asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>市场状态1-&gt;0转换概率</td>
                                <td>
                                    <asp:TextBox ID="P10" runat="server">0.1</asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>分红存在概率</td>
                                <td>
                                    <asp:TextBox ID="PDividend" runat="server">0.8</asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>分红正负概率</td>
                                <td>
                                    <asp:TextBox ID="P" runat="server">0.7</asp:TextBox></td>
                            </tr>

                            <tr>
                                <td>分红正负转换概率</td>
                                <td>
                                    <asp:TextBox ID="TransP" runat="server">0.5</asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>分红时间窗口</td>
                                <td>
                                    <asp:TextBox ID="TimeWindow" runat="server">15</asp:TextBox></td>
                            </tr>
                        </table>
                        <table id="Exp">
                            <tr>
                                <td>每轮时间</td>
                                <td>
                                    <asp:TextBox ID="PeriodOfTurn" runat="server">10</asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>最大轮次</td>
                                <td>
                                    <asp:TextBox ID="MaxTurn" runat="server">1200</asp:TextBox></td>
                            </tr>
                        </table>
                    </div>

                    <div id="PauseList">
                        <asp:ListBox ID="CurrentPauseList" runat="server" AutoPostBack="True"></asp:ListBox>
                        <asp:Button ID="RemovePause" runat="server" Text="移除暂停" />
                        <asp:TextBox ID="PauseTurn" runat="server"></asp:TextBox>
                        <asp:Button ID="AddPause" runat="server" Text="添加暂停" />
                    </div>

                    <div id="Operate">
                        <asp:Button ID="BuildExp" runat="server" Text="建立实验" OnClick="BuildExp_Click" />
                        <asp:Button ID="RecoveryExp" runat="server" Text="恢复实验" />
                        <asp:Button ID="StartExp" runat="server" Text="开始实验" />
                        <asp:Button ID="ContinueExp" runat="server" Text="继续实验" />
                        <asp:Button ID="ResetExp" runat="server" Text="重置实验" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
