Imports System.Collections.Generic
Imports System.Configuration
Imports System.Linq
Imports System.Text

Imports Dapper
Imports Dapper.Contrib.Extensions

Imports log4net

Imports App.Core

Namespace Core.Services
    Public Interface IShippingService
        Sub ShipOrder(order As Models.Order)
    End Interface

    Public Class FedExShippingService
        Implements IShippingService

        Private Shared ReadOnly log As ILog = LogManager.GetLogger(GetType(FedExShippingService))

        Public Sub ShipOrder(order As Models.Order) Implements IShippingService.ShipOrder
            log.Info("Shipping order id = " & order.OrderId)
        End Sub
    End Class

    Public Class FulfillmentService
        Private Shared ReadOnly _log As ILog = LogManager.GetLogger(GetType(FulfillmentService))

        ReadOnly _shippingSvc As IShippingService
        ReadOnly _ordersDao As DataAccess.OrdersDao

        Sub New()
            Me._shippingSvc = New FedExShippingService
            Me._ordersDao = New DataAccess.OrdersDao
        End Sub

        Public Sub ProcessCustomer(customerId As String)
            Dim orders = _ordersDao.GetAllForCustomer(customerId)
            For Each order As Models.Order In orders
                If order.ShippedDate.HasValue Then
                    _log.Warn("Order with " & order.OrderId & " has already been shipped, skipping.")
                    Continue For
                End If

                Me.Validate(order)
                _log.Info("Order " & order.OrderId & " validated, proceeding with shipping..")

                Me._shippingSvc.ShipOrder(order)           'Ship with external shipping service

                'Update shipping date
                order.ShippedDate = DateTime.Now
                Try
                    Me._ordersDao.Update(order)
                Catch ex As Exception
                    Throw ex
                End Try

                'Other operations...Decrease product quantity... etc
            Next
        End Sub

        Private Sub Validate(order As Models.Order)
            'todo: implementation
        End Sub
    End Class

    Public Class CustomerService
        Private Shared ReadOnly _log As ILog = LogManager.GetLogger(GetType(CustomerService))
        ReadOnly _customersDao As DataAccess.CustomersDao
        ReadOnly _ordersDao As DataAccess.OrdersDao

        Sub New()
            Me._customersDao = New DataAccess.CustomersDao
            Me._ordersDao = New DataAccess.OrdersDao
        End Sub

        Public Function GetCustomers() As IList(Of Models.Customer)
            Dim customers = Me._customersDao.GetAll()
            For Each customer As Models.Customer In customers
                customer.Orders = Me.GetOrdersForCustomer(customer.CustomerId)
            Next

            Return customers
        End Function

        Public Function GetCustomerById(customerId As String) As Models.Customer
            Return Me._customersDao.Get(customerId)
        End Function

        Public Function GetOrdersForCustomer(customerId As String) As IList(Of Models.Order)
            Return Me._ordersDao.GetAllForCustomer(customerId)
        End Function

        Public Function EditCustomer(customerId As String, contactName As String) As Boolean
            Dim customer As New Models.Customer
            customer = Me.GetCustomerById(customerId)
            customer.ContactName = contactName

            Try
                Me._customersDao.Update(customer)
            Catch ex As Exception
                Throw ex
            End Try

            Return True
        End Function
    End Class
End Namespace

