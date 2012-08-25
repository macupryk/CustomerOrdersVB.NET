Imports System
Imports System.Collections.Generic
Imports System.Linq

Imports System.Web.Mvc

Imports App.Core

Namespace Controllers
    Public Class CustomersController
        Inherits BaseController

        ReadOnly _customerSvc As Services.CustomerService

        Sub New()
            Me._customerSvc = New Services.CustomerService
        End Sub

        Function Index() As ActionResult
            Return View()
        End Function

        Function List() As ActionResult
            Dim model = Me._customerSvc.GetCustomers
            Return View(model)
        End Function

        Function FillCustomersGrid() As JsonResult '(sidx As String, sord As String, page As Integer, rows As Integer) As JsonResult
            Dim pageIndex As Integer = CInt(Request("page")) - 1
            Dim pageSize As Integer = CInt(Request("rows"))

            Dim customers = Me._customerSvc.GetCustomers
            Return New JsonDotNetResult With {.Data = New Utils.PagedList(customers, customers.Count, pageIndex, pageSize)}
        End Function

        Function Browse(customerId As String) As ActionResult
            Dim model = Me._customerSvc.GetCustomerById(customerId)
            Return View(model)
        End Function

        Function Edit(customerId As String) As ActionResult
            Dim model = Me._customerSvc.GetCustomerById(customerId)
            Return View(model)
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
            Return View()
        End Function
    End Class
End Namespace
