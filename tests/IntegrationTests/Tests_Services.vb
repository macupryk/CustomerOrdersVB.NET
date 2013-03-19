Imports System.Collections.Generic
Imports System.Linq

Imports NUnit.Framework

Imports App.Core

Namespace IntegrationTests.Services
    Public Class FedExShippingServiceTests
        Inherits IntegrationTest

        <Test>
        Public Sub Should_Ship_Order()
            'grab first order in the repository and test with that
            Dim order = Me.OrdersDao.GetAllForCustomer("BOTTM").First
            Me.FedExShippingSvc.ShipOrder(order)

            Assert.AreEqual(Me.FedExShippingSvc.ShippingMessage, "Shipping order id = " & order.OrderId)
        End Sub
    End Class

    Public Class FulfillmentServiceTests
        Inherits IntegrationTest

        <Test>
        Public Sub Should_Process_Fulfill_Order()
            Dim customerId = "BLAUS"

            Me.FulfillmentSvc.ProcessCustomer(customerId)

            'now, there should be at least one order with a shipping date of today 
            '(be ensure your orders stub has some null shipping dates so this test behaves as expected)
            Dim orders = Me.OrdersDao.GetAllForCustomer(customerId).Where(Function(o) CDate(o.ShippedDate.Value.ToShortDateString) = Date.Today).ToList

            Assert.Greater(orders.Count, 0)
        End Sub
    End Class

    Public Class CustomerServiceTests
        Inherits IntegrationTest

        <Test>
        Public Sub Should_Retrieve_All_Customers_With_Their_Orders()
            Dim customers = Me.CustomerSvc.GetCustomers
            Assert.IsNotNull(customers)
            Assert.IsInstanceOf(Of List(Of Models.Customer))(customers)
            Assert.Greater(customers.Count, 0)

            Dim orders = customers.Where(Function(c) c.CustomerId = "ALFKI").SingleOrDefault.Orders
            Assert.IsNotNull(orders)
            Assert.IsInstanceOf(Of List(Of Models.Order))(orders)
            Assert.Greater(orders.Count, 0)
        End Sub

        <Test>
        Public Sub Should_Retrieve_Specified_Customer_By_Id()
            Dim customerId = "BOTTM"
            Dim customer = Me.CustomerSvc.GetCustomerById(customerId)
            Assert.IsNotNull(customer)
            Assert.IsInstanceOf(Of Models.Customer)(customer)
            Assert.AreEqual(customerId, customer.CustomerId)
        End Sub

        <Test>
        Public Sub Should_Update_Customer_Contact()
            Dim customer = Me.CustomersDao.Get("CHOPS")
            Dim preContact = customer.ContactName

            Dim newContact = "Mr. New Contact"
            Me.CustomerSvc.EditCustomer(customer.CustomerId, newContact)

            'retrieve our edited contact and test
            customer = Me.CustomersDao.Get("CHOPS")
            Assert.AreNotEqual(preContact, customer.ContactName)
        End Sub
    End Class
End Namespace
