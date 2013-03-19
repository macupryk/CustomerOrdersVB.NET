Imports System.Collections.Generic
Imports System.Linq


Imports NUnit.Framework

Imports App.Core

Namespace IntegrationTests.Daos
    <TestFixture>
    Public Class CustomersDaoTests
        Inherits IntegrationTest

        <Test>
        Public Sub Should_Retrieve_All_Customers()
            Dim customers = Me.CustomersDao.GetAll
            Assert.IsNotNull(customers)
            Assert.IsInstanceOf(Of List(Of Models.Customer))(customers)
            Assert.Greater(customers.Count, 0)
        End Sub

        <Test>
        Public Sub Should_Retrieve_Specified_Customer_By_Id()
            Dim customerId = "ALFKI"
            Dim customer = Me.CustomersDao.Get(customerId)
            Assert.IsNotNull(customer)
            Assert.IsInstanceOf(Of Models.Customer)(customer)
            Assert.AreEqual(customerId, customer.CustomerId)
        End Sub

        <Test>
        Public Sub Should_Update_Customer_Contact()
            Dim customerId = "BOTTM"
            Dim customer = Me.CustomersDao.Get(customerId)
            Dim preContact = customer.ContactName
            Dim newContact = "Elizabeth Lincoln-Rose"

            customer.ContactName = newContact
            Me.CustomersDao.Update(customer)

            'retrieve our edited contact and test
            customer = Me.CustomersDao.Get(customerId)
            Assert.AreNotEqual(preContact, customer.ContactName)
        End Sub

    End Class

    <TestFixture>
    Public Class OrdersDaoTests
        Inherits IntegrationTest

        <Test>
        Public Sub Should_Retrieve_Orders_For_Specified_Customer()
            Dim customerId = "CHOPS"
            Dim orders = Me.OrdersDao.GetAllForCustomer(customerId)
            Assert.IsNotNull(orders)
            Assert.IsInstanceOf(Of List(Of Models.Order))(orders)
        End Sub
    End Class
End Namespace
