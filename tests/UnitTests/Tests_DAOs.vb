Imports System.Collections.Generic

Imports NUnit.Framework

Imports App.Core

Namespace UnitTests.Daos
    Public Class CustomersDaoTests
        Inherits UnitTest

        <Test>
        Public Sub Should_Retrieve_All_Customers()
            Dim customers = Me.MockCustomersDao.GetAll()
            Assert.IsNotNull(customers)
            Assert.IsInstanceOf(Of List(Of Models.Customer))(customers)
            Assert.Greater(customers.Count, 0)
        End Sub

        <Test>
        Public Sub Should_Retrieve_Specified_Customer_By_Id()
            Dim customerId = "DISNEY1"
            Dim customer = Me.MockCustomersDao.Get(customerId)
            Assert.IsNotNull(customer)
            Assert.IsInstanceOf(Of Models.Customer)(customer)
            Assert.AreEqual(customerId, customer.CustomerId)
        End Sub

        <Test>
        Public Sub Should_Update_Customer_Contact()
            Dim customer = Me.MockCustomersDao.Get("DISNEY2")
            Dim preContact = customer.ContactName
            Dim newContact = "Mr. New Contact"

            customer.ContactName = newContact

            Me.MockCustomersDao.Update(customer)

            'retrieve our edited contact and test
            customer = Me.MockCustomersDao.Get("DISNEY2")
            Assert.AreNotEqual(preContact, customer.ContactName)
        End Sub
    End Class

    Public Class OrdersDaoTests
        Inherits UnitTest

        <Test>
        Public Sub Should_Retrieve_Orders_For_Specified_Customer()
            Dim customerId = "DISNEY3"
            Dim orders = Me.MockOrdersDao.GetAllForCustomer(customerId)
            Assert.IsNotNull(orders)
            Assert.IsInstanceOf(Of List(Of Models.Order))(orders)
        End Sub
    End Class
End Namespace
