<%@ Page Language="C#" AutoEventWireup="true" Inherits="Default2" Codebehind="Default2.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>jQuery datepicker</title>


    <link rel="Stylesheet" type="text/css" href="Styles/jquery.datepick.css"/>
    <script type="text/javascript" src="Scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery.datepick.js"></script>


    <script type="text/javascript">
        $(function () {
            $('#txt1').datepick({ dateFormat: 'mm/dd/yyyy' });
            $('#txt2').datepick({ dateFormat: 'mm/dd/yyyy' });
            $("#content").animate({
                marginTop: "80px"
            }, 600);
        });    
    </script>



</head>
<body>
    <form id="form1" runat="server">
    <div id="div1" 
        
        
        style="font-family: 'Comic Sans MS'; font-size: xx-large; color: #800000; height: 14px;">Hiiiii It will shows that whenever you click on the textbox to enter the date it will open a calendar</div>
    <div id="content" style="background-color: #C8F986; height: 262px;">
     <h1 style="background-color: #008080; font-family: 'Comic Sans MS'; font-size: xx-large; color: #00FFFF; height: 46px;">Check in Date</h1>
     <asp:TextBox ID="txt1" class="field" runat="server" BackColor="#FF9999" 
            BorderColor="#CCFF99" Font-Names="Comic Sans MS" Font-Size="Large" 
            ForeColor="#99FF33" Height="31px" Width="149px"></asp:TextBox>
        <br />
        <br />
        <br />
        <h1 style="font-family: 'Comic Sans MS'; font-size: xx-large; color: #800000; background-color: #00FFFF;">Check out Date</h1>
       <asp:TextBox ID="txt2" class="field" runat="server" BackColor="#FF9966" 
            BorderColor="#FFFF66" BorderStyle="Groove" Font-Names="Comic Sans MS" 
            Font-Size="Large" Height="30px" Width="147px"></asp:TextBox>
    </div>
    </form>
</body>
</html>
