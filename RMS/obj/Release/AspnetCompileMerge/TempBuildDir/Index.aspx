<%@  Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Title="3W" Inherits="RMS.Index" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
</head>
<body>
    <%//=Request.Url.ToString().ToLower()%>
    <div id="divSelectURL" runat="server" style="margin: 30px;" visible="false">
        <h2>Invalid URL</h2>
        <h3>
            Please enter correct url. Or you may select from the urls given below.</h3>
        <ul>
            <li><a id="erp" runat="server" href="http://www.3werp.com/index.aspx" title="http://www.3werp.com/index.aspx">
                3W ERP</a></li>
            <li><a id="pvt" runat="server" href="http://www.3wpvt.com/index.aspx" title="http://www.3wpvt.com/index.aspx">
                3W PVT</a></li>
            <li><a id="eec" runat="server" href="http://www.3weec.com/index.aspx" title="http://www.3weec.com/index.aspx">
                3W EEC</a></li>
        </ul>
    </div>
</body>
</html>
