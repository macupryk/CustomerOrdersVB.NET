Imports System.Web
Imports System.Web.Mvc

Imports App.Core

Public Class JsonDotNetResult
    Inherits JsonResult

    ReadOnly _index As Integer
    ReadOnly _size As Integer

    Public Sub New()
        JsonRequestBehavior = JsonRequestBehavior.DenyGet
    End Sub

    Public Overrides Sub ExecuteResult(context As ControllerContext)
        If context Is Nothing Then
            Throw New ArgumentNullException("context")
        End If

        Dim response As HttpResponseBase = context.HttpContext.Response

        If Not [String].IsNullOrEmpty(ContentType) Then
            response.ContentType = ContentType
        Else
            response.ContentType = "application/json"
        End If
        If ContentEncoding IsNot Nothing Then
            response.ContentEncoding = ContentEncoding
        End If
        If Data IsNot Nothing Then
            response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(Data))
        End If
    End Sub
End Class