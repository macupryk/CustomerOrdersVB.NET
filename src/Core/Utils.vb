Imports System
Imports System.Collections

Namespace Core.Utils
    Public Class PagedList
        Private mRows As IEnumerable
        Private mTotalRecords As Integer
        Private mPageIndex As Integer
        Private mPageSize As Integer
        Private mUserData As Object

        Public ReadOnly Property total() As Integer
            Get
                Return CInt(Math.Ceiling(CDec(mTotalRecords) / CDec(mPageSize)))
            End Get
        End Property

        Public ReadOnly Property page() As Integer
            Get
                Return mPageIndex
            End Get
        End Property

        Public ReadOnly Property records() As Integer
            Get
                Return mTotalRecords
            End Get
        End Property

        Public ReadOnly Property rows() As IEnumerable
            Get
                Return mRows
            End Get
        End Property

        Public ReadOnly Property userData() As Object
            Get
                Return mUserData
            End Get
        End Property

        Public Sub New(rows As IEnumerable, totalRecords As Integer, pageIndex As Integer, pageSize As Integer, userData As Object)
            mRows = rows
            mTotalRecords = totalRecords
            mPageIndex = pageIndex
            mPageSize = pageSize
            mUserData = userData
        End Sub

        Public Sub New(rows As IEnumerable, totalRecords As Integer, pageIndex As Integer, pageSize As Integer)
            Me.New(rows, totalRecords, pageIndex, pageSize, Nothing)
        End Sub

        Public Overrides Function ToString() As String
            Return Newtonsoft.Json.JsonConvert.SerializeObject(Me)
        End Function
    End Class
End Namespace
