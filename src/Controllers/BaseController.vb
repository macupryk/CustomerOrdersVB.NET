Imports System.Globalization
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading

Imports System.Web
Imports System.Web.Caching
Imports System.Web.Mvc

Namespace Controllers
    Public Class BaseController
        Inherits Controller

        Protected Overrides Sub OnActionExecuting(filterContext As System.Web.Mvc.ActionExecutingContext)
            MyBase.OnActionExecuting(filterContext)

            If Session("Culture") Is Nothing Then
                Session("Culture") = Me.GetAvailableCulture
            End If

            filterContext.Controller.ViewData("PageTitle") = "ASP.NET MVC3 Example"
            filterContext.Controller.ViewData("Header") = "Another Northwind Example But with ASP.NET MVC 3, Spring.NET, jqGrid, and Fluent NHibernate!"
            filterContext.Controller.ViewData("Locale") = Session("Culture").ToString
        End Sub

        Protected Overrides Function Json(data As Object, contentType As String, contentEncoding As Encoding) As System.Web.Mvc.JsonResult
            Return New JsonDotNetResult With {.Data = data}
        End Function

        Protected Sub SetCulture(locale As String)
            If locale IsNot Nothing Then
                If Me.IsValidLocale(locale) Then
                    Thread.CurrentThread.CurrentUICulture = New CultureInfo(locale)
                End If
            End If
            Session("Culture") = Thread.CurrentThread.CurrentUICulture.ToString()
        End Sub

        Private Function IsValidLocale(locale As String) As Boolean
            ' match en, uk, ru, nl || match en-US, uk-UA, ru-RU, nl-BE
            If Not String.IsNullOrEmpty(locale) Then
                If Regex.IsMatch(locale, "^[a-z]{2}$") OrElse Regex.IsMatch(locale, "[a-z]{2}-[A-Z]{2}") Then
                    Return True
                End If
            End If

            Return False           'if it got here, return false
        End Function

        Private Function GetAvailableCulture() As String
            Dim culture = Thread.CurrentThread.CurrentUICulture.ToString()
            If culture.StartsWith("de") Then
                Return "de"
            ElseIf culture.StartsWith("es") Then
                Return "es"
            ElseIf culture.StartsWith("fr") Then
                Return "fr"
            ElseIf culture.StartsWith("ja") Then
                Return "ja"
            ElseIf culture.StartsWith("zh") Then
                Return "zh"
            Else
                Return "en"
            End If
        End Function

    End Class
End Namespace

