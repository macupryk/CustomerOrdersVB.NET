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

        Function List() As ActionResult
            Dim model = Me._customerSvc.GetCustomers
            Return View(model)
        End Function

        Function FillCustomersGrid(sidx As String, sord As String, page As Integer, rows As Integer) As JsonResult
            Dim pageIndex As Integer = page - 1
            Dim pageSize As Integer = rows

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
        Function Edit(ByVal customerId As String, ByVal contactName As String) As ActionResult
            Me._customerSvc.EditCustomer(customerId, contactName)

            Return New EmptyResult
        End Function

        Function EditSuccess(customerName As String) As ActionResult
            ViewData("Message") = "Successful update for '" + customerName + "'."
            Return View()
        End Function
    End Class
End Namespace
