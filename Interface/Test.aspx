<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="Interface.Test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>


            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Label ID="ExpState" runat="server"></asp:Label>
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Timer ID="Timer1" runat="server" Interval="1000" OnTick="Timer1_Tick"></asp:Timer>
                                            <asp:Label ID="TimeTick" runat="server"></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                 <asp:Button ID="BuildExp" runat="server" Text="建立实验" OnClick="BuildExp_Click" />
                            </ContentTemplate>
        </asp:UpdatePanel>

        </div>

    </form>
</body>
</html>
