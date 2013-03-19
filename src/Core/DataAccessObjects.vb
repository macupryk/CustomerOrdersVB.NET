Imports System.Collections.Generic
Imports System.Configuration
Imports System.Data
Imports System.Globalization
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.CompilerServices

Imports System.Data.SQLite

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
                'ElseIf provider = "MySql.Data.MySqlClient" Then
                '    connection = New MySql.Data.MySqlClient.MySqlConnection(connStr)
            ElseIf provider = "System.Data.Sqlite" Then
                connection = New SQLite.SQLiteConnection(connStr)
            End If

            connection.Open()
            Return connection
        End Function

        Public Function GetModelData(query As String) As IList(Of T)
            Return Me.GetModelData(query, Nothing)
        End Function

        Public Function GetModelData(query As String, params As DynamicParameters) As IList(Of T)
            Using cn As IDbConnection = OpenConnection()
                Return cn.Query(Of T)(query, params, commandType:=CommandType.Text)
            End Using
        End Function

        Public Function ExecuteDataManipulation(query As String, params As DynamicParameters) As Integer
            Using cn As IDbConnection = OpenConnection()
                Return cn.Execute(query, params, commandType:=CommandType.Text)
            End Using
        End Function
    End Class

    Public Interface ICustomersDao
        Function [Get](id As String) As Models.Customer
        Function GetAll() As IList(Of Models.Customer)
        Function Update(customer As Models.Customer) As Boolean
    End Interface

    Public Class CustomersDao
        Inherits DataAccessObject(Of Models.Customer)
        Implements ICustomersDao

        Sub New(connName As String, connStr As String, providerName As String)
            MyBase.New(New ConnectionStringSettings(connName, connStr, providerName))
        End Sub

        Sub New()
            MyBase.New(ConfigurationManager.ConnectionStrings("Northwind"))
        End Sub

        Public Function [Get](id As String) As Models.Customer Implements ICustomersDao.Get
            Dim paramsList = New DynamicParameters
            paramsList.Add("@CustomerId", id)

            Return Me.GetModelData("select * from customers where CustomerId = @CustomerId", paramsList)(0)
        End Function

        Public Function GetAll() As IList(Of Models.Customer) Implements ICustomersDao.GetAll
            Return Me.GetModelData("select * from Customers")
        End Function

        Public Function Update(customer As Models.Customer) As Boolean Implements ICustomersDao.Update
            Dim paramsList = New DynamicParameters
            paramsList.Add("@CustomerId", customer.CustomerId)
            paramsList.Add("@CompanyName", customer.CompanyName)
            paramsList.Add("@ContactName", customer.ContactName)
            paramsList.Add("@ContactTitle", customer.ContactTitle)
            paramsList.Add("@Address", customer.Address)
            paramsList.Add("@City", customer.City)
            paramsList.Add("@Region", customer.Region)
            paramsList.Add("@PostalCode", customer.PostalCode)
            paramsList.Add("@Country", customer.Country)
            paramsList.Add("@Phone", customer.Phone)
            paramsList.Add("@Fax", customer.Fax)

            Dim query = "update Customers set CompanyName = @CompanyName, ContactName = @ContactName, " &
                            "ContactTitle = @ContactTitle, Address = @Address, City = @City, " &
                            "Region = @Region, PostalCode = @PostalCode, Country = @Country, " &
                            "Phone = @Phone, Fax = @Fax where CustomerId = @CustomerId"

            Dim ret = Me.ExecuteDataManipulation(query, paramsList)
            Return (ret = 1)
        End Function
    End Class

    Public Interface IOrdersDao
        Function GetAllForCustomer(customerId As String) As IList(Of Models.Order)
        Function Update(order As Models.Order) As Boolean
    End Interface

    Public Class OrdersDao
        Inherits DataAccessObject(Of Models.Order)
        Implements IOrdersDao

        Sub New(connName As String, connStr As String, providerName As String)
            MyBase.New(New ConnectionStringSettings(connName, connStr, providerName))
        End Sub

        Sub New()
            MyBase.New(ConfigurationManager.ConnectionStrings("Northwind"))
        End Sub

        Public Function GetAllForCustomer(customerId As String) As IList(Of Models.Order) Implements IOrdersDao.GetAllForCustomer
            Dim paramsList = New DynamicParameters
            paramsList.Add("@CustomerId", customerId)

            Return Me.GetModelData("select * from Orders where CustomerId = @CustomerId", paramsList)
        End Function

        Public Function Update(order As Models.Order) As Boolean Implements IOrdersDao.Update
            Dim paramsList = New DynamicParameters
            paramsList.Add("@ShippedDate", order.ShippedDate)
            paramsList.Add("@OrderId", order.OrderId)

            Dim query = "update Orders set ShippedDate = @ShippedDate where OrderId = @OrderId"

            Dim ret = CBool(Me.ExecuteDataManipulation(query, paramsList))
            Return (ret = 1)
        End Function
    End Class
End Namespace