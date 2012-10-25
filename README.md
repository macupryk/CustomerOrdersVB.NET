=============================
CustomerOrders-ASPNET-MVC3-VB
=============================

Example application using [ASP.NET Mvc3][1] and [Dapper][2].  Adapted from a [Spring.NET example application][3].  Written in VB 10 and distributed under the [Apache license version 2][6].

Notes:
* You can plug in mostly any database for this example, but by default I'm using [Sqlite][5] (as of 10/25/2012) here.  You can delete this database and easily re-create it if you have [Python][9] available on your machine.  Just click the script and it will re-create in the App_Data folder of the web site.
* The solution itself can be opened in VS Studio 2010 or 2012.
    
Please [enable NuGet Package Restore][7] to build with the necessary dependencies.

[1]: http://www.asp.net/mvc/mvc3
[2]: http://code.google.com/p/dapper-dot-net
[3]: https://github.com/SpringSource/spring-net/tree/master/examples/Spring/Spring.Data.NHibernate.Northwind 
[4]: http://xunit.codeplex.com 
[5]: http://system.data.sqlite.org/index.html/doc/trunk/www/index.wiki
[6]: https://github.com/WillSams/CustomerOrders-ASPNET-MVC3-VB/blob/master/license.txt
[7]: http://docs.nuget.org/docs/workflows/using-nuget-without-committing-packages
[8]: http://ironpython.net
[9]: http://www.python.org/