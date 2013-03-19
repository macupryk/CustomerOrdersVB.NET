Imports System.Collections.Generic
Imports System.Linq

Imports NUnit.Framework
Imports Moq

Imports App.Core

Namespace IntegrationTests
    <TestFixture()>
    Public Class IntegrationTest
        Protected Property CustomersDao As Core.DataAccess.ICustomersDao
        Protected Property OrdersDao As Core.DataAccess.IOrdersDao

        Protected Property FedExShippingSvc As Core.Services.IShippingService
        Protected Property FulfillmentSvc As Core.Services.IFulfillmentService
        Protected Property CustomerSvc As Core.Services.ICustomerService

        <SetUp>
        Public Sub SetUpTests()
            'note:  In regards to unit testing with SQLite, there are pre-build events for two things:
            '       * An event to kill the testing engine lest you'll get error message pertaining to the fact SQLite.Interop.dll being used by another process
            '           --taskkill /F /IM vstest.executionengine.x86.exe /FI "MEMUSAGE gt 1"  (for x64, get rid of '.x86.' in executable name)
            '       * An event copying the database to the target folder.
            ' Remember, the test data will only copy (therefore resetting your test data changes) only when you do a build/rebuild.  Also, set those interop.dlls for Sqlite to "copy if newer"

            log4net.Config.XmlConfigurator.Configure()              'see app.config.  All you need is to configure what you have there.  Easy peasy.

            AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory)   'tells what |DataDirectory| is in your connection string.

            Me.OrdersDao = New Core.DataAccess.OrdersDao
            Me.CustomersDao = New Core.DataAccess.CustomersDao

            Me.FedExShippingSvc = New Core.Services.FedExShippingService
            Me.FulfillmentSvc = New Core.Services.FulfillmentService
            Me.CustomerSvc = New Core.Services.CustomerService
        End Sub

        <TearDown>
        Public Sub TearThisPuppyDown()
            System.Data.SQLite.SQLiteConnection.ClearAllPools()
        End Sub
    End Class
End Namespace

