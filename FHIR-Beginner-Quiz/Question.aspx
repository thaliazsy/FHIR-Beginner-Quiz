<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Question.aspx.cs" Inherits="FHIR_Beginner_Quiz.Question" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <script type="text/javascript">
        var countDownDate = new Date("Jan 29, 2022 12:00:00").getTime();

        var myfunc = setInterval(function () {
            var now = new Date().getTime();
            var timeleft = countDownDate - now;

            // Calculating the minutes and seconds left
            var minutes = Math.floor((timeleft % (1000 * 60 * 60)) / (1000 * 60));
            var seconds = Math.floor((timeleft % (1000 * 60)) / 1000);

            // Result is output to the specific element
            document.getElementById("mins").innerHTML = minutes + "m ";
            document.getElementById("secs").innerHTML = seconds + "s ";
        }, 1000);
    </script>

    <form id="form1" runat="server">
        <div>
            <asp:Label ID="LblWelcome" runat="server" Text="Hello!"></asp:Label>
            <br />
            <div id="timer">
                This is only valid for the next
                <div id="mins" style="display: inline"></div>
                <div id="secs" style="display: inline"></div>
            </div>
            <br />
            <asp:Panel ID="PanelQA" runat="server">
                <asp:Image ID="Image1" runat="server" />
                <br />
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
