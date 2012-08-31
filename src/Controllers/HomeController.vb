Imports System
Imports System.Collections.Generic
Imports System.Linq

Imports System.Web.Mvc

Imports App.Core

Namespace Controllers
    Public Class HomeController
        Inherits BaseController

        Function Index() As ActionResult
            ViewData("Welcome") = "Welcome to ASP.NET MVC3 sample application!"
            ViewData("Home") = ""

            Return View()
        End Function

        Function About() As ActionResult
            ViewData("Home") = "About"
            Return View()
        End Function
    End Class
End Namespace
