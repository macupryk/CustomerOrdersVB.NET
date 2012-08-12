﻿
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="content-type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=Edge,chrome=1" />   
    <meta name="viewport" content="width=device-width" />
    <title>@ViewData("PageTitle")</title>
    <link type="text/css" rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.21/themes/ui-lightness/jquery-ui.css" />  
    <link type="text/css" rel="Stylesheet" href="@Url.Content("~/Content/Styles/Site.css")" />
    @RenderSection("StylesContent", required:=False)

    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
</head>
<body>
    <div class="page">
        <div id="header">
            <div id="title">
                <h1>CustomerOrders ASP.NET Mvc 3 Example</h1>
            </div>
            <div id="logindisplay">
                &nbsp;
            </div>
            <div id="menucontainer">
                <ul id="menu">
                    <li><a href="/home">Home</a></li>
                    <li><a href="/about">About</a></li>
                    <li><a href="/customers">Customers</a></li>
                </ul>
            </div>
        </div>
        <div id="main">
            @RenderSection("MainContent", required:=True)
        </div>
        <div id="footer">
        </div>
    </div>
    <span style="position: absolute">
        @*<script type="text/javascript" src="https://getfirebug.com/firebug-lite.js"></script>*@
        <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.21/jquery-ui.min.js"></script>
        <script type="text/javascript" src="@Url.Content("~/Content/Scripts/sugar-1.2.4.min.js")"></script>  
        <script type="text/javascript" src="@Url.Content("~/Content/Scripts/Common.js")"></script>
        <script type="text/javascript">
            var _appCommon = null;              //our global object

            $(function () {
                _appCommon = new AppCommon();
            });
        </script>
        @RenderSection("ScriptsContent", required:=False)
    </span>
</body>
</html>