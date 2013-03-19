Imports System.Collections.Generic
imports System.Linq

Imports NUnit.Framework

Imports App.Core

Namespace UnitTests.Services
    Public Class FedExShippingServiceTests
        Inherits UnitTest

        <Test>
        Public Sub Should_Ship_Order()
            'grab first order in the repository and test with that
            Dim order = Me.MockOrdersDao.GetAllForCustomer("DISNEY2").First
            Me.MockFedExShippingSvc.ShipOrder(order)

            Assert.AreEqual(Me.MockFedExShippingSvc.ShippingMessage, "Shipping order id = " & order.OrderId)
        End Sub
    End Class

    Public Class FulfillmentServiceTests
        Inherits UnitTest

        <Test>
        Public Sub Should_Process_Fulfill_Order()
            Dim customerId = "DISNEY3"

            Me.MockFulfillmentSvc.ProcessCustomer(customerId)

            'now, there should be at least one order with a shipping date of today 
            '(be ensure your orders stub has some null shipping dates so this test behaves as expected)
            Dim orders = Me.MockOrdersDao.GetAllForCustomer(customerId).Where(Function(o) CDate(o.ShippedDate.Value.ToShortDateString) = Date.Today).ToList

            Assert.Greater(orders.Count, 0)
        End Sub
    End Class

    Public Class CustomerServiceTests
        Inherits UnitTest

        <Test>
        Public Sub Should_Retrieve_All_Customers_With_Their_Orders()
            Dim customers = Me.MockCustomerSvc.GetCustomers
            Assert.IsNotNull(customers)
            Assert.IsInstanceOf(Of List(Of Models.Customer))(customers)
            Assert.Greater(customers.Count, 0)

            'each should have orders, but we'll just test one of them
            Dim orders = customers.Where(Function(c) c.CustomerId = "DISNEY1").SingleOrDefault.Orders
            Assert.IsNotNull(orders)
            Assert.IsInstanceOf(Of List(Of Models.Order))(orders)
            Assert.Greater(orders.Count, 0)
        End Sub

        <Test>
        Public Sub Should_Retrieve_Specified_Customer_By_Id()
            Dim customerId = "DISNEY2"
            Dim customer = Me.MockCustomerSvc.GetCustomerById(customerId)
            Assert.IsNotNull(customer)
            Assert.IsInstanceOf(Of Models.Customer)(customer)
            Assert.AreEqual(customerId, customer.CustomerId)
        End Sub

        <Test>
        Public Sub Should_Update_Customer_Contact()
            Dim customer = Me.MockCustomersDao.Get("DISNEY3")
            Dim preContact = customer.ContactName

            Dim newContact = "Mr. New Contact"
            Me.MockCustomerSvc.EditCustomer(customer.CustomerId, newContact)

            'retrieve our edited contact and test
            customer = Me.MockCustomersDao.Get("DISNEY3")
            Assert.AreNotEqual(preContact, customer.ContactName)
        End Sub
    End Class
End Namespace
