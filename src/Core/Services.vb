Imports System.Collections.Generic
Imports System.Configuration
Imports System.Linq
Imports System.Text

Imports Dapper

Imports log4net

Imports App.Core

Namespace Core.Services
    Public Interface IShippingService
        Property ShippingMessage As String
        Sub ShipOrder(order As Models.Order)
    End Interface

    Public Class FedExShippingService
        Implements IShippingService

        Private Shared ReadOnly log As ILog = LogManager.GetLogger(GetType(FedExShippingService))

        Property ShippingMessage As String Implements IShippingService.ShippingMessage

        Public Sub ShipOrder(order As Models.Order) Implements IShippingService.ShipOrder
            Me.ShippingMessage = "Shipping order id = " & order.OrderId
            log.Info(Me.ShippingMessage)
        End Sub
    End Class

    Public Interface IFulfillmentService
        Sub ProcessCustomer(customerId As String)
    End Interface

    Public Class FulfillmentService
        Implements IFulfillmentService

        Private Shared ReadOnly _log As ILog = LogManager.GetLogger(GetType(FulfillmentService))

        ReadOnly _shippingSvc As IShippingService
        ReadOnly _ordersDao As DataAccess.OrdersDao

        Sub New()
            Me._shippingSvc = New FedExShippingService
            Me._ordersDao = New DataAccess.OrdersDao
        End Sub

        'overload for dependency injection.  I may do DI later....
        Sub New(shippingSvc As IShippingService, ordersDao As DataAccess.IOrdersDao)
            Me._shippingSvc = shippingSvc
            Me._ordersDao = ordersDao
        End Sub

        Public Sub ProcessCustomer(customerId As String) Implements IFulfillmentService.ProcessCustomer
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

    Public Interface ICustomerService
        Function GetCustomers() As IList(Of Models.Customer)
        Function GetCustomerById(customerId As String) As Models.Customer
        Function GetOrdersForCustomer(customerId As String) As IList(Of Models.Order)
        Function EditCustomer(customerId As String, contactName As String) As Boolean
    End Interface

    Public Class CustomerService
        Implements ICustomerService

        Private Shared ReadOnly _log As ILog = LogManager.GetLogger(GetType(CustomerService))
        ReadOnly _customersDao As DataAccess.CustomersDao
        ReadOnly _ordersDao As DataAccess.OrdersDao

        Sub New()
            Me._customersDao = New DataAccess.CustomersDao
            Me._ordersDao = New DataAccess.OrdersDao
        End Sub

        'overload for dependency injection.  I may do DI later....
        Sub New(customersDao As DataAccess.ICustomersDao, ordersDao As DataAccess.IOrdersDao)
            Me._customersDao = customersDao
            Me._ordersDao = ordersDao
        End Sub

        Public Function GetCustomers() As IList(Of Models.Customer) Implements ICustomerService.GetCustomers
            Dim customers = Me._customersDao.GetAll()
            For Each customer As Models.Customer In customers
                customer.Orders = Me.GetOrdersForCustomer(customer.CustomerId)
            Next

            Return customers
        End Function

        Public Function GetCustomerById(customerId As String) As Models.Customer Implements ICustomerService.GetCustomerById
            Return Me._customersDao.Get(customerId)
        End Function

        Public Function GetOrdersForCustomer(customerId As String) As IList(Of Models.Order) Implements ICustomerService.GetOrdersForCustomer
            Return Me._ordersDao.GetAllForCustomer(customerId)
        End Function

        Public Function EditCustomer(customerId As String, contactName As String) As Boolean Implements ICustomerService.EditCustomer
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

