<%@ WebHandler Language="VB" Class="Mantenedor_sesiones" %>


Imports System
Imports System.Web

Public Class Mantenedor_sesiones : Implements IHttpHandler, IRequiresSessionState
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.Cache.SetNoStore()
        context.Response.ContentType = "application/x-javascript"
        context.Response.Write("//")
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return True
        End Get
    End Property

End Class