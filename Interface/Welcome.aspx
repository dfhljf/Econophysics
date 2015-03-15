<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Welcome.aspx.cs" Inherits="Interface.Welcome" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Label ID="LoginInfo" runat="server"></asp:Label>
        <asp:Button ID="Login" runat="server" Text="登陆" OnClick="Login_Click"/>
    
    </div>
    </form>
</body>
</html>
