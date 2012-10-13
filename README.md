=============================
CustomerOrders-ASPNET-MVC3-VB
=============================

Example application using [ASP.NET Mvc3][1] and [Dapper][2].  Adapted from a [Spring.NET example application][3].  Written in VB 10 and distributed under the [Apache license version 2][6].  Includes 

Notes:
* Stored procedures created for a [MySQL][5] and SQL Server 2005/2008/2012 implementation of the Northwind database.  
* Automated tests written in [IronPython][8].  Install IronPython if you want to run my tests if you don't already have it installed.  I highly prefer you right your own tests with whatever framework you prefer, though.
* The solution itself can be opened in VS Studio 2012 or 2010.
    
Please [enable NuGet Package Restore][7] to build with the necessary dependencies.

[1]: http://www.asp.net/mvc/mvc3
[2]: http://code.google.com/p/dapper-dot-net
[3]: https://github.com/SpringSource/spring-net/tree/master/examples/Spring/Spring.Data.NHibernate.Northwind 
[4]: http://xunit.codeplex.com 
[5]: http://www.mysql.com
[6]: https://github.com/WillSams/CustomerOrders-ASPNET-MVC3-VB/blob/master/license.txt
[7]: http://docs.nuget.org/docs/workflows/using-nuget-without-committing-packages
[8]: http://ironpython.net