<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Question.aspx.cs" Inherits="FHIR_Beginner_Quiz.Question" %>

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
            <br />
            <asp:Panel ID="PanelQA" runat="server">
                <asp:Label ID="LblQuestion" runat="server" Text="Question"></asp:Label>
                <asp:RadioButtonList ID="RadOptions" runat="server" Visible="False">
                    <asp:ListItem>A</asp:ListItem>
                    <asp:ListItem>B</asp:ListItem>
                    <asp:ListItem>C</asp:ListItem>
                    <asp:ListItem>D</asp:ListItem>
                </asp:RadioButtonList>
                <asp:CheckBoxList ID="CheckBoxOptions" runat="server" Visible="False">
                    <asp:ListItem>A</asp:ListItem>
                    <asp:ListItem>B</asp:ListItem>
                    <asp:ListItem>C</asp:ListItem>
                    <asp:ListItem>D</asp:ListItem>
                </asp:CheckBoxList>
                <asp:Button ID="BtnNext" runat="server" Text="Next" OnClick="BtnNext_Click" />
            </asp:Panel>


        </div>
    </form>
</body>
</html>
