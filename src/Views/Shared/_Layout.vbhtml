
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

    <script type="text/javascript" src="http://cdnjs.cloudflare.com/ajax/libs/jquery/1.8.2/jquery.min.js"></script> 
    <script type="text/javascript" src="http://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.8.24/jquery-ui.min.js"></script>
    <script type="text/javascript" src="http://cdnjs.cloudflare.com/ajax/libs/sugar/1.3.4/sugar-1.3.4.min.js"></script>
        <script type="text/javascript" src="@Url.Content("~/Content/Scripts/Common.js")"> </script>
        <script type="text/javascript">
            var _appCommon = null;              //our global object

            $(function () {
                _appCommon = AppCommon;
            });
        </script>
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
                    <li>@Html.ActionLink("Home", "Index", "Home")</li>
                    <li>@Html.ActionLink("About", "About", "Home")</li>
                    <li>@Html.ActionLink("Customers", "Index", "Customers")</li>
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
        @RenderSection("ScriptsContent", required:=False)
    </span>
</body>
</html>
