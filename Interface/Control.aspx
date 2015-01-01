<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Control.aspx.cs" Inherits="Interface.Control" %>

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
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div>

                    <div id="ExpInfo">
                        <table>
                            <tr>
                                <td>编号</td>
                                <td>
                                    <asp:Label ID="ExpId" runat="server"></asp:Label></td>
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
                                    <asp:Timer ID="Timer1" runat="server" Interval="1000" OnTick="Timer1_Tick">
                                    </asp:Timer>
                                    <asp:Label ID="TimeTick" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>

                    <div id="Parameters">
                        <table id="Agent">
                            <tr>
                                <td>轮次</td>
                                <td>
                                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>轮次</td>
                                <td>
                                    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>轮次</td>
                                <td>
                                    <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>轮次</td>
                                <td>
                                    <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>轮次</td>
                                <td>
                                    <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>轮次</td>
                                <td>
                                    <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox></td>
                            </tr>
                        </table>
                        <table id="Market">
                            <tr>
                                <td>轮次</td>
                                <td>
                                    <asp:TextBox ID="TextBox7" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>轮次</td>
                                <td>
                                    <asp:TextBox ID="TextBox8" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>轮次</td>
                                <td>
                                    <asp:TextBox ID="TextBox9" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>轮次</td>
                                <td>
                                    <asp:TextBox ID="TextBox10" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>轮次</td>
                                <td>
                                    <asp:TextBox ID="TextBox11" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>轮次</td>
                                <td>
                                    <asp:TextBox ID="TextBox12" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>轮次</td>
                                <td>
                                    <asp:TextBox ID="TextBox13" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>轮次</td>
                                <td>
                                    <asp:TextBox ID="TextBox14" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>轮次</td>
                                <td>
                                    <asp:TextBox ID="TextBox15" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>轮次</td>
                                <td>
                                    <asp:TextBox ID="TextBox16" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>轮次</td>
                                <td>
                                    <asp:TextBox ID="TextBox17" runat="server"></asp:TextBox></td>
                            </tr>
                        </table>
                        <table id="Exp">
                            <tr>
                                <td>轮次</td>
                                <td>
                                    <asp:TextBox ID="TextBox18" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>轮次</td>
                                <td>
                                    <asp:TextBox ID="TextBox19" runat="server"></asp:TextBox></td>
                            </tr>
                        </table>
                    </div>

                    <div id="PauseList">
                        <asp:ListBox ID="CurrentPauseList" runat="server"></asp:ListBox>
                        <asp:Button ID="RemovePause" runat="server" Text="Button" />
                        <asp:TextBox ID="PauseTurn" runat="server"></asp:TextBox>
                        <asp:Button ID="AddPause" runat="server" Text="Button" />
                    </div>

                    <div id="Operate">
                        <asp:Button ID="BuildExp" runat="server" Text="Button" />
                        <asp:Button ID="RecoveryExp" runat="server" Text="Button" />
                        <asp:Button ID="StartExp" runat="server" Text="Button" />
                        <asp:Button ID="ContinueExp" runat="server" Text="Button" />
                        <asp:Button ID="ResetExp" runat="server" Text="Button" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
