Imports System.Collections.Generic
Imports System.Dynamic
Imports System.Globalization
Imports System.Linq
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading

Imports System.Web
Imports System.Web.Caching
Imports System.Web.Mvc

Imports log4net
Imports log4net.Appender
Imports log4net.Core
Imports log4net.Layout
Imports log4net.Repository

Imports App.Core

Namespace Controllers
    Public Class BaseController
        Inherits Controller

        ReadOnly _viewPrefix As String

        Sub New(viewPrefix As String)
            Me._viewPrefix = viewPrefix
        End Sub

        Protected Overrides Sub OnActionExecuting(filterContext As System.Web.Mvc.ActionExecutingContext)
            MyBase.OnActionExecuting(filterContext)

            If Session("Culture") Is Nothing Then
                Session("Culture") = Me.GetAvailableCulture
            End If

            filterContext.Controller.ViewData("PageTitle") = "ASP.NET MVC3 Example"
            filterContext.Controller.ViewData("Header") = "Another Northwind Example But with ASP.NET MVC 3, Spring.NET, jqGrid, and Fluent NHibernate!"
            filterContext.Controller.ViewData("Locale") = Session("Culture").ToString
        End Sub

        Protected Overrides Function Json(data As Object, contentType As String, contentEncoding As Encoding) As System.Web.Mvc.JsonResult
            Return New JsonDotNetResult With {.Data = data}
        End Function

        Protected Sub SetCulture(locale As String)
            If locale IsNot Nothing Then
                If Me.IsValidLocale(locale) Then
                    Thread.CurrentThread.CurrentUICulture = New CultureInfo(locale)
                End If
            End If
            Session("Culture") = Thread.CurrentThread.CurrentUICulture.ToString()
        End Sub

        Private Function IsValidLocale(locale As String) As Boolean
            ' match en, uk, ru, nl || match en-US, uk-UA, ru-RU, nl-BE
            If Not String.IsNullOrEmpty(locale) Then
                If Regex.IsMatch(locale, "^[a-z]{2}$") OrElse Regex.IsMatch(locale, "[a-z]{2}-[A-Z]{2}") Then
                    Return True
                End If
            End If

            Return False           'if it got here, return false
        End Function

        Private Function GetAvailableCulture() As String
            Dim culture = Thread.CurrentThread.CurrentUICulture.ToString()
            If culture.StartsWith("de") Then
                Return "de"
            ElseIf culture.StartsWith("es") Then
                Return "es"
            ElseIf culture.StartsWith("fr") Then
                Return "fr"
            ElseIf culture.StartsWith("ja") Then
                Return "ja"
            Else
                Return "en"
            End If
        End Function

        Protected Friend Function RenderView(Of T)(view As String, model As T) As System.Web.Mvc.ViewResult
            Return Me.View("~/Views/" & _viewPrefix & view, model)
        End Function

        Protected Friend Function RenderView(view As String) As System.Web.Mvc.ViewResult
            Return Me.RenderView(Of Object)(view, Nothing)
        End Function
    End Class
    
    '////////////////////////////////////////////////////////////////////////////////////////////////////////////

    Public Class CustomersController
        Inherits BaseController

        ReadOnly _customerSvc As Services.CustomerService

        Sub New()
            MyBase.New("Customers")
            Me._customerSvc = New Services.CustomerService
        End Sub

        Function Index() As ActionResult
            Return RenderView("Index.vbhtml")
        End Function

        Function List() As ActionResult
            Dim model = Me._customerSvc.GetCustomers
            Return RenderView("List.vbhtml", model)
        End Function

        Function FillCustomersGrid() As JsonResult '(sidx As String, sord As String, page As Integer, rows As Integer) As JsonResult
            Dim pageIndex As Integer = CInt(Request("page")) - 1
            Dim pageSize As Integer = CInt(Request("rows"))

            Dim customers = Me._customerSvc.GetCustomers
            Return New JsonDotNetResult With {.Data = New Utils.PagedList(customers, customers.Count, pageIndex, pageSize)}
        End Function

        Function Browse(customerId As String) As ActionResult
            Dim model = Me._customerSvc.GetCustomerById(customerId)
            Return RenderView("Browse.vbhtml", model)
        End Function

        Function Edit(customerId As String) As ActionResult
            Dim model = Me._customerSvc.GetCustomerById(customerId)
            Return RenderView("Edit.vbhtml", model)
        End Function

        <HttpPost()> _
        Function Edit() As ActionResult
            Dim customerId = CStr(Request("CustomerId"))
            Dim contactName = CStr(Request("ContactName"))
            Me._customerSvc.EditCustomer(customerId, contactName)

            Return RedirectToAction("EditSuccess", New With {.contactName = contactName})
        End Function

        Function EditSuccess(contactName As String) As ActionResult
            ViewData("Message") = "Successful update for '" & contactName & "'."
            Return RenderView("EditSuccess.vbhtml")
        End Function
    End Class
    
    '////////////////////////////////////////////////////////////////////////////////////////////////////////////

    Public Class HomeController
        Inherits BaseController

        Sub New()
            MyBase.New("")
        End Sub

        Function Index() As ActionResult
            ViewData("Welcome") = "Welcome to ASP.NET MVC3 sample application!"
            ViewData("Home") = ""

            Return RenderView("Index.vbhtml")
        End Function

        Function About() As ActionResult
            ViewData("Home") = "About"
            Return RenderView("About.vbhtml")
        End Function
    End Class
    
    '////////////////////////////////////////////////////////////////////////////////////////////////////////////

    Public Class OrdersController
        Inherits BaseController

        Private _customerSvc As Services.CustomerService
        Private _fulfillmentSvc As Services.FulfillmentService

        Sub New()
            MyBase.New("Orders")
            Me._customerSvc = New Services.CustomerService
            Me._fulfillmentSvc = New Services.FulfillmentService
        End Sub

        Function Process(customerId As String, contactName As String) As ActionResult
            ViewData("ProcessOrdersFor") = "Orders for customer " & contactName
            ViewData("Process") = "On this screen you can see all orders that customer has. By clicking 'Process Orders' " &
                "you can send service request to ship all unshipped orders for that customer."

            ViewData("CustomerId") = customerId
            ViewData("ContactName") = contactName

            Dim model = Me._customerSvc.GetOrdersForCustomer(customerId)
            Return RenderView("Process.vbhtml", model)
        End Function

        Function FillOrdersGrid(customerId As String) As JsonResult
            Dim pageIndex As Integer = CInt(Request("page")) - 1   'jqGrid will pass the page in the querystring
            Dim pageSize As Integer = CInt(Request("rows"))         'jqGrid will pass rows in the querystring

            Dim orders = Me._customerSvc.GetOrdersForCustomer(customerId)
            Return New JsonDotNetResult With {.Data = New Utils.PagedList(orders, orders.Count, pageIndex, pageSize)}
        End Function

        <HttpPost()>
        Function Process() As ActionResult
            ' gather log4net output with small hack to get results...
            Dim repository As ILoggerRepository = LogManager.GetRepository()
            Dim appenders As IAppender() = repository.GetAppenders()
            Dim appender As MemoryAppender = Nothing

            For Each a As IAppender In appenders
                If TypeOf a Is MemoryAppender Then
                    appender = TryCast(a, MemoryAppender)       ' we found our appender to look results from
                    Exit For
                End If
            Next

            If appender IsNot Nothing Then
                appender.Clear()

                Dim customerId = CStr(Request("CustomerId"))
                Me._fulfillmentSvc.ProcessCustomer(customerId)
                Dim events As LoggingEvent() = appender.GetEvents()
                Dim stringWriter As New IO.StringWriter()
                Dim layout As New PatternLayout("%date{HH:mm:ss} %-5level %logger{1}: %message<br />")
                For Each loggingEvent As LoggingEvent In events
                    layout.Format(stringWriter, loggingEvent)
                Next

                TempData("ProcessMessages") = stringWriter.ToString()
            Else
                TempData("ProcessMessages") = "Nothing to process?"
            End If

            Return RedirectToAction("ProcessResult", New With {.contactName = CStr(Request("ContactName"))})
        End Function

        Function ProcessResult(contactName As String) As ActionResult
            ViewData("ProcessMessages") = TempData("ProcessMessages")
            Return RenderView("ProcessResult.vbhtml")
        End Function
    End Class
End Namespace
