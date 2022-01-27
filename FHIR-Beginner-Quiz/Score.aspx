<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Score.aspx.cs" Inherits="FHIR_Beginner_Quiz.Score" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="LblWelcome" runat="server" Text="Hello!"></asp:Label>
            <br />
            <asp:Label ID="LblScore" runat="server" Text="Score: "></asp:Label>

        </div>
    </form>
</body>
</html>
