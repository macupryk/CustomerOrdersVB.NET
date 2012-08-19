Imports System
Imports System.Linq
Imports System.Text

Imports System.Web.Mvc
Imports System.Web.Routing
Imports System.IO

Public Class MvcApplication
    Inherits System.Web.HttpApplication

    Private Shared ReadOnly _logger As log4net.ILog = log4net.LogManager.GetLogger(
                                                        System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType)

    Shared Sub RegisterGlobalFilters(ByVal filters As GlobalFilterCollection)
        filters.Add(New HandleErrorAttribute())
    End Sub

    Shared Sub RegisterRoutes(ByVal routes As RouteCollection)
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}")

        'RouteTable.Routes.RouteExistingFiles = True
        AppRoutes.MapRoutes(RouteTable.Routes)
    End Sub

    Sub Application_Start()
        AreaRegistration.RegisterAllAreas()

        Me.ConfigureLogging()

        RegisterGlobalFilters(GlobalFilters.Filters)
        RegisterRoutes(RouteTable.Routes)
    End Sub

    Private Sub ConfigureLogging()
        'configure log4net through common.logging bindings
        Dim logFilePath As String = AppDomain.CurrentDomain.BaseDirectory + "Content\Config\log4net-config.xml"
        Dim finfo As New FileInfo(logFilePath)
        log4net.Config.XmlConfigurator.ConfigureAndWatch(finfo)
    End Sub

    Public Sub Application_Error()
        Dim ex = Me.Server.GetLastError()
        Dim serverVariableLog = BuildServerVariableLog()

        Dim sb = New StringBuilder("Error trapped in Application_Error:")
        sb.AppendLine().Append(serverVariableLog)
        sb.AppendLine().AppendLine()
        sb.Append(ex)

        _logger.Error(sb.ToString())
    End Sub

    Private Function BuildServerVariableLog() As String
        Dim serverVariablesToLog() =
            {
                    "REQUEST_METHOD",
                    "HTTP_HOST",
                    "SCRIPT_NAME",
                    "LOGON_USER",
                    "HTTP_USER_AGENT",
                    "HTTP_COOKIE",
                    "REMOTE_ADDR",
                    "LOCAL_ADDR",
                    "QUERY_STRING"
                }

        Dim serverVariables = Me.Context.Request.ServerVariables

        Dim log = serverVariablesToLog.Aggregate(New StringBuilder(),
                                         Function(sb, serverVariable)
                                             sb.AppendFormat("{0}: {1}", serverVariable, serverVariables(serverVariable))
                                             Return sb.AppendLine()
                                         End Function)

        Return log.ToString()
    End Function
End Class
