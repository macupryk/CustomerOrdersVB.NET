Imports System
Imports System.Collections.Generic
Imports System.Linq

Imports System.Web.Mvc

Imports log4net
Imports log4net.Appender
Imports log4net.Core
Imports log4net.Layout
Imports log4net.Repository

Imports App.Core

Namespace Controllers
    Public Class OrdersController
        Inherits BaseController

        Private _customerSvc As Services.CustomerService
        Private _fulfillmentSvc As Services.FulfillmentService

        Sub New()
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
            Return View(model)
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
            Return View()
        End Function
    End Class
End Namespace
