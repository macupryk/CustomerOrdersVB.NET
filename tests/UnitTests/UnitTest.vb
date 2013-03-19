Imports System.Collections.Generic
Imports System.Linq

Imports NUnit.Framework
Imports Moq

Imports App.Core

Namespace UnitTests
    <TestFixture()>
    Public Class UnitTest
        Protected Property CustomersStub As IList(Of Models.Customer)
        Protected Property OrdersStub As IList(Of Models.Order)

        Protected Property MockCustomersDao As Core.DataAccess.ICustomersDao
        Protected Property MockOrdersDao As Core.DataAccess.IOrdersDao

        Protected Property MockFedExShippingSvc As Core.Services.IShippingService
        Protected Property MockFulfillmentSvc As Core.Services.IFulfillmentService
        Protected Property MockCustomerSvc As Core.Services.ICustomerService

        <SetUp>
        Public Sub SetUpTests()
            log4net.Config.XmlConfigurator.Configure()              'see app.config.  All you need is to configure what you have there.  Easy peasy.

            Me.OrdersStub = Me.GetOrdersStub                        'customers need orders to be created first ;)
            Me.CustomersStub = Me.GetCustomersSub

            Me.MockOrdersDao = Me.GetMockOrdersDao.Object
            Me.MockCustomersDao = Me.GetMockCustomersDao.Object

            Me.MockFedExShippingSvc = Me.GetMockFedExShippingService.Object
            Me.MockFulfillmentSvc = Me.GetMockFulfillmentService.Object
            Me.MockCustomerSvc = Me.GetMockCustomerService.Object
        End Sub

        Private Function GetCustomersSub() As IList(Of Models.Customer)
            Return New List(Of Models.Customer)(New Models.Customer() {
                New Models.Customer With {.CustomerId = "DISNEY1", .ContactName = "Mickey Mouse", .Orders = Me.OrdersStub.Where(Function(o) o.CustomerId = "DISNEY1").ToList},
                New Models.Customer With {.CustomerId = "DISNEY2", .ContactName = "Donald Duck", .Orders = Me.OrdersStub.Where(Function(o) o.CustomerId = "DISNEY2").ToList},
                New Models.Customer With {.CustomerId = "DISNEY3", .ContactName = "Goofy Goof", .Orders = Me.OrdersStub.Where(Function(o) o.CustomerId = "DISNEY3").ToList}
            })
        End Function

        Private Function GetOrdersStub() As IList(Of Models.Order)
            Return New List(Of Models.Order)(New Models.Order() {
                New Models.Order With {.CustomerId = "DISNEY1", .OrderId = "11111", .OrderDate = CDate("23 Feb 2013"), .ShippedDate = Nothing},
                New Models.Order With {.CustomerId = "DISNEY2", .OrderId = "11112", .OrderDate = CDate("26 Oct 2012"), .ShippedDate = CDate("27 Oct 2012")},
                New Models.Order With {.CustomerId = "DISNEY3", .OrderId = "11113", .OrderDate = CDate("4 Feb 2004"), .ShippedDate = CDate("7 Feb 2004")},
                New Models.Order With {.CustomerId = "DISNEY2", .OrderId = "11114", .OrderDate = CDate("25 Nov 2009"), .ShippedDate = Nothing},
                New Models.Order With {.CustomerId = "DISNEY3", .OrderId = "11115", .OrderDate = CDate("23 Feb 2012"), .ShippedDate = CDate("28 Feb 2012")},
                New Models.Order With {.CustomerId = "DISNEY3", .OrderId = "11116", .OrderDate = CDate("18 Dec 2012"), .ShippedDate = Nothing}
            })
        End Function

        Private Function GetMockCustomersDao() As Mock(Of Core.DataAccess.ICustomersDao)
            Dim dao = New Mock(Of Core.DataAccess.ICustomersDao)
            dao.SetupAllProperties()            'setup property automatically starts tracking properties and their values
            dao.Setup(Function(d) d.GetAll()).Returns(Me.CustomersStub)
            dao.Setup(Function(d) d.Get(It.IsAny(Of String))).Returns(Function(id As String)
                                                                          Return Me.CustomersStub.Where(Function(c) c.CustomerId = id).FirstOrDefault
                                                                      End Function)
            dao.Setup(Function(d) d.Update(It.IsAny(Of Models.Customer))).Returns(Function(cust As Models.Customer)
                                                                                      Me.CustomersStub.Remove(Me.CustomersStub.Where(Function(c) c.CustomerId.Equals(cust.CustomerId)).Single)
                                                                                      Me.CustomersStub.Add(cust)
                                                                                      Return True
                                                                                  End Function)

            Return dao
        End Function

        Private Function GetMockOrdersDao() As Mock(Of Core.DataAccess.IOrdersDao)
            Dim dao = New Mock(Of Core.DataAccess.IOrdersDao)
            dao.SetupAllProperties()            'setup property automatically starts tracking properties and their values
            dao.Setup(Function(d) d.GetAllForCustomer(It.IsAny(Of String))).Returns(Function(id As String)
                                                                                        Return Me.OrdersStub.Where(Function(c) c.CustomerId.Equals(id)).ToList
                                                                                    End Function)
            dao.Setup(Function(d) d.Update(It.IsAny(Of Models.Order))).Returns(Function(ord As Models.Order)
                                                                                   Me.OrdersStub.Remove(Me.OrdersStub.Where(Function(o) o.OrderId.Equals(ord.OrderId)).Single)
                                                                                   Me.OrdersStub.Add(ord)
                                                                                   Return True
                                                                               End Function)
            Return dao
        End Function

        Private Function GetMockFedExShippingService() As Mock(Of Core.Services.IShippingService)
            Dim orderPlaceHolder = Nothing
            Dim shippingSvc = New Mock(Of Core.Services.IShippingService)
            shippingSvc.SetupAllProperties()            'setup property automatically starts tracking properties and their values
            shippingSvc.Setup(Sub(svc) svc.ShipOrder(It.IsAny(Of Models.Order))).Callback(Of Models.Order)(Sub(o As Models.Order)
                                                                                                               Dim shippingMessage = "Shipping order id = " & o.OrderId
                                                                                                               shippingSvc.Object.ShippingMessage = shippingMessage

                                                                                                               Dim log As log4net.ILog = log4net.LogManager.GetLogger(GetType(Core.Services.IFulfillmentService))
                                                                                                               log.Info(shippingMessage)
                                                                                                               'or you can just do a simple 'Console.WriteLine(shippingMessage)' if you don't feel necessary to test the logger
                                                                                                           End Sub)

            Return shippingSvc
        End Function

        Private Function GetMockFulfillmentService() As Mock(Of Core.Services.IFulfillmentService)
            Dim fulfillmentSvc = New Mock(Of Core.Services.IFulfillmentService)
            fulfillmentSvc.SetupAllProperties()
            fulfillmentSvc.Setup(Sub(svc) svc.ProcessCustomer(It.IsAny(Of String))).Callback(Of String)(Sub(customerId As String)
                                                                                                            Dim log As log4net.ILog = log4net.LogManager.GetLogger(GetType(Core.Services.IFulfillmentService))
                                                                                                            Dim orders = Me.MockOrdersDao.GetAllForCustomer(customerId)
                                                                                                            For Each order As Models.Order In orders
                                                                                                                If order.ShippedDate.HasValue Then
                                                                                                                    log.Warn("Order with " & order.OrderId & " has already been shipped, skipping.")
                                                                                                                    'Console.WriteLine("Order with " & order.OrderId & " has already been shipped, skipping.")
                                                                                                                    Continue For
                                                                                                                End If

                                                                                                                'validated so ...
                                                                                                                log.Info("Order " & order.OrderId & " validated, proceeding with shipping..")
                                                                                                                'Console.WriteLine("Order " & order.OrderId & " validated, proceeding with shipping..")

                                                                                                                Me.MockFedExShippingSvc.ShipOrder(order)        'Ship with external shipping service

                                                                                                                'Update shipping date
                                                                                                                order.ShippedDate = DateTime.Now
                                                                                                                Try
                                                                                                                    Me.MockOrdersDao.Update(order)
                                                                                                                Catch ex As Exception
                                                                                                                    Throw ex
                                                                                                                End Try

                                                                                                                'Other operations...Decrease product quantity... etc
                                                                                                            Next
                                                                                                        End Sub)
            Return fulfillmentSvc
        End Function

        Private Function GetMockCustomerService() As Mock(Of Core.Services.ICustomerService)
            Dim customerSvc = New Mock(Of Core.Services.ICustomerService)
            customerSvc.SetupAllProperties()

            customerSvc.Setup(Function(svc) svc.GetCustomers).Returns(Function()
                                                                          Dim customers = Me.MockCustomersDao.GetAll
                                                                          For Each customer As Models.Customer In customers
                                                                              customer.Orders = Me.MockOrdersDao.GetAllForCustomer(customer.CustomerId)
                                                                          Next
                                                                          Return customers
                                                                      End Function)
            customerSvc.Setup(Function(svc) svc.GetCustomerById(It.IsAny(Of String))).Returns(Function(customerId As String)
                                                                                                  Return Me.MockCustomersDao.Get(customerId)
                                                                                              End Function)
            customerSvc.Setup(Function(svc) svc.EditCustomer(It.IsAny(Of String), It.IsAny(Of String))).Returns(Function(customerId As String, contactName As String)
                                                                                                                    Dim customer = Me.MockCustomersDao.Get(customerId)
                                                                                                                    customer.ContactName = contactName

                                                                                                                    Me.MockCustomersDao.Update(customer)

                                                                                                                    Return True
                                                                                                                End Function)
            Return customerSvc
        End Function
    End Class
End Namespace

