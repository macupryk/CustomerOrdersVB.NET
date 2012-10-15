Imports System.Collections.Generic
Imports System.Configuration
Imports System.Data
Imports System.Globalization
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.CompilerServices

Imports Dapper

Namespace Core.DataAccess
    Public Class DataAccessObject(Of T)

        Shared _connectionString As ConnectionStringSettings

        Sub New(strConnection As ConnectionStringSettings)
            _connectionString = strConnection
        End Sub

        Protected Shared Function OpenConnection() As IDbConnection
            Dim connection As IDbConnection = Nothing
            Dim connStr = _connectionString.ConnectionString
            Dim provider = _connectionString.ProviderName

            If provider = "System.Data.SqlClient" Then
                connection = New SqlClient.SqlConnection(connStr)
            ElseIf provider = "MySql.Data.MySqlClient" Then
                connection = New MySql.Data.MySqlClient.MySqlConnection(connStr)
                'ElseIf provider = "System.Data.Sqlite" Then
                '    connection = New System.Data.SqliteClient.SqliteConnection(connStr)
            End If

            connection.Open()
            Return connection
        End Function

        Public Function GetModelData(query As String, params As DynamicParameters, cmdType As CommandType) As IList(Of T)
            Using cn As IDbConnection = OpenConnection()
                Return cn.Query(Of T)(query, params, commandType:=cmdType)
            End Using
        End Function

        Public Function ExecuteDataManipulation(query As String, params As DynamicParameters, cmdType As CommandType) As Integer
            Using cn As IDbConnection = OpenConnection()
                Return cn.Execute(query, params, commandType:=cmdType)
            End Using
        End Function
    End Class

    Public Class CustomersDao
        Inherits DataAccessObject(Of Models.Customer)

        Sub New(connName As String, connStr As String, providerName As String)
            MyBase.New(New ConnectionStringSettings(connName, connStr, providerName))
        End Sub

        Sub New()
            MyBase.New(ConfigurationManager.ConnectionStrings("Northwind"))
        End Sub

        Public Function [Get](id As String) As Models.Customer
            Dim paramsList = New DynamicParameters
            paramsList.Add("@in_CustomerId", id)

            Return Me.GetModelData("GetCustomerById", paramsList, CommandType.StoredProcedure)(0)
        End Function

        Public Function GetAll() As IList(Of Models.Customer)
            Return Me.GetModelData("GetAllCustomers", Nothing, CommandType.StoredProcedure)
        End Function

        Public Function Update(customer As Models.Customer) As Boolean
            Dim paramsList = New DynamicParameters
            paramsList.Add("@in_CustomerId", customer.CustomerId)
            paramsList.Add("@in_CompanyName", customer.CompanyName)
            paramsList.Add("@in_ContactName", customer.ContactName)
            paramsList.Add("@in_ContactTitle", customer.ContactTitle)
            paramsList.Add("@in_Address", customer.Address)
            paramsList.Add("@in_City", customer.City)
            paramsList.Add("@in_Region", customer.Region)
            paramsList.Add("@in_PostalCode", customer.PostalCode)
            paramsList.Add("@in_Country", customer.Country)
            paramsList.Add("@in_Phone", customer.Phone)
            paramsList.Add("@in_Fax", customer.Fax)

            Dim ret = Me.ExecuteDataManipulation("UpdateCustomer", paramsList, CommandType.StoredProcedure)
            Return (ret = 1)
        End Function
    End Class

    Public Class OrdersDao
        Inherits DataAccessObject(Of Models.Order)

        Sub New(connName As String, connStr As String, providerName As String)
            MyBase.New(New ConnectionStringSettings(connName, connStr, providerName))
        End Sub

        Sub New()
            MyBase.New(ConfigurationManager.ConnectionStrings("Northwind"))
        End Sub

        Public Function GetAllForCustomer(customerId As String) As IList(Of Models.Order)
            Dim paramsList = New DynamicParameters
            paramsList.Add("@in_customerId", customerId)

            Return Me.GetModelData("GetAllOrdersForCustomer", paramsList, CommandType.StoredProcedure)
        End Function

        Public Function Update(order As Models.Order) As Boolean
            Dim paramsList = New DynamicParameters
            paramsList.Add("@in_OrderId", order.OrderId)
            paramsList.Add("@in_CustomerId", order.CustomerId)
            paramsList.Add("@in_Freight", order.Freight)
            paramsList.Add("@in_ShipName", order.ShipName)
            paramsList.Add("@in_ShipAddress", order.ShipAddress)
            paramsList.Add("@in_ShipCity", order.ShipCity)
            paramsList.Add("@in_ShipRegion", order.ShipRegion)
            paramsList.Add("@in_ShipPostalCode", order.ShipPostalCode)
            paramsList.Add("@in_ShipCountry", order.ShipCountry)

            Dim ret = CBool(Me.ExecuteDataManipulation("UpdateOrder", paramsList, CommandType.StoredProcedure))
            Return (ret = 1)
        End Function
    End Class
End Namespace