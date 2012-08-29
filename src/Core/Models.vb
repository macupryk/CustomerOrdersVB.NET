Imports System
Imports System.Collections.Generic

Namespace Core.Models
    Public Class Customer
        Property CustomerId As String
        Property CompanyName As String
        Property ContactName As String
        Property ContactTitle As String
        Property Address As String
        Property City As String
        Property Region As String
        Property PostalCode As String
        Property Country As String
        Property Phone As String
        Property Fax As String
        Property Orders As IList(Of Order)

        Sub New()
            Me.CustomerId = String.Empty
            Me.CompanyName = String.Empty
            Me.ContactName = String.Empty
            Me.ContactTitle = String.Empty
            Me.Address = String.Empty
            Me.City = String.Empty
            Me.Region = String.Empty
            Me.PostalCode = String.Empty
            Me.Country = String.Empty
            Me.Phone = String.Empty
            Me.Fax = String.Empty
            Me.Orders = New List(Of Order)
        End Sub
    End Class

    Public Class Order
        Property OrderId As Integer
        Property OrderDate As DateTime?
        Property RequiredDate As DateTime?
        Property ShippedDate As DateTime?
        Property Freight As Single
        Property ShipName As String
        Property ShipAddress As String
        Property ShipCity As String
        Property ShipRegion As String
        Property ShipPostalCode As String
        Property ShipCountry As String
        Property CustomerId As String

        Sub New()
            Me.OrderId = 0
            Me.Freight = 0D
            Me.ShipName = String.Empty
            Me.ShipAddress = String.Empty
            Me.ShipCity = String.Empty
            Me.ShipRegion = String.Empty
            Me.ShipPostalCode = String.Empty
            Me.ShipCountry = String.Empty
            Me.CustomerId = String.Empty
        End Sub
    End Class
End Namespace
