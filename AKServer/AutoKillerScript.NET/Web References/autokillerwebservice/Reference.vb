﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Xml.Serialization

'
'This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
'
Namespace autokillerwebservice
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3062.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Web.Services.WebServiceBindingAttribute(Name:="EncryptSoap", [Namespace]:="http://webservices.autokiller.com/AutokillerWebService/rsa.asmx")>  _
    Partial Public Class Encrypt
        Inherits System.Web.Services.Protocols.SoapHttpClientProtocol
        
        Private authInfoValueField As AuthInfo
        
        Private EncryptFileOperationCompleted As System.Threading.SendOrPostCallback
        
        Private LogUserOutOperationCompleted As System.Threading.SendOrPostCallback
        
        Private useDefaultCredentialsSetExplicitly As Boolean
        
        '''<remarks/>
        Public Sub New()
            MyBase.New
            Me.Url = Global.AutoKillerScript.My.MySettings.Default.AutoKillerScript_com_autokiller_webservices_Encrypt
            If (Me.IsLocalFileSystemWebService(Me.Url) = true) Then
                Me.UseDefaultCredentials = true
                Me.useDefaultCredentialsSetExplicitly = false
            Else
                Me.useDefaultCredentialsSetExplicitly = true
            End If
        End Sub
        
        Public Property AuthInfoValue() As AuthInfo
            Get
                Return Me.authInfoValueField
            End Get
            Set
                Me.authInfoValueField = value
            End Set
        End Property
        
        Public Shadows Property Url() As String
            Get
                Return MyBase.Url
            End Get
            Set
                If (((Me.IsLocalFileSystemWebService(MyBase.Url) = true)  _
                            AndAlso (Me.useDefaultCredentialsSetExplicitly = false))  _
                            AndAlso (Me.IsLocalFileSystemWebService(value) = false)) Then
                    MyBase.UseDefaultCredentials = false
                End If
                MyBase.Url = value
            End Set
        End Property
        
        Public Shadows Property UseDefaultCredentials() As Boolean
            Get
                Return MyBase.UseDefaultCredentials
            End Get
            Set
                MyBase.UseDefaultCredentials = value
                Me.useDefaultCredentialsSetExplicitly = true
            End Set
        End Property
        
        '''<remarks/>
        Public Event EncryptFileCompleted As EncryptFileCompletedEventHandler
        
        '''<remarks/>
        Public Event LogUserOutCompleted As LogUserOutCompletedEventHandler
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapHeaderAttribute("AuthInfoValue"),  _
         System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://webservices.autokiller.com/AutokillerWebService/rsa.asmx/EncryptFile", RequestNamespace:="http://webservices.autokiller.com/AutokillerWebService/rsa.asmx", ResponseNamespace:="http://webservices.autokiller.com/AutokillerWebService/rsa.asmx", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function EncryptFile(ByVal key As String) As <System.Xml.Serialization.XmlElementAttribute(DataType:="base64Binary")> Byte()
            Dim results() As Object = Me.Invoke("EncryptFile", New Object() {key})
            Return CType(results(0),Byte())
        End Function
        
        '''<remarks/>
        Public Overloads Sub EncryptFileAsync(ByVal key As String)
            Me.EncryptFileAsync(key, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub EncryptFileAsync(ByVal key As String, ByVal userState As Object)
            If (Me.EncryptFileOperationCompleted Is Nothing) Then
                Me.EncryptFileOperationCompleted = AddressOf Me.OnEncryptFileOperationCompleted
            End If
            Me.InvokeAsync("EncryptFile", New Object() {key}, Me.EncryptFileOperationCompleted, userState)
        End Sub
        
        Private Sub OnEncryptFileOperationCompleted(ByVal arg As Object)
            If (Not (Me.EncryptFileCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent EncryptFileCompleted(Me, New EncryptFileCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapHeaderAttribute("AuthInfoValue"),  _
         System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://webservices.autokiller.com/AutokillerWebService/rsa.asmx/LogUserOut", RequestNamespace:="http://webservices.autokiller.com/AutokillerWebService/rsa.asmx", ResponseNamespace:="http://webservices.autokiller.com/AutokillerWebService/rsa.asmx", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function LogUserOut() As Integer
            Dim results() As Object = Me.Invoke("LogUserOut", New Object(-1) {})
            Return CType(results(0),Integer)
        End Function
        
        '''<remarks/>
        Public Overloads Sub LogUserOutAsync()
            Me.LogUserOutAsync(Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub LogUserOutAsync(ByVal userState As Object)
            If (Me.LogUserOutOperationCompleted Is Nothing) Then
                Me.LogUserOutOperationCompleted = AddressOf Me.OnLogUserOutOperationCompleted
            End If
            Me.InvokeAsync("LogUserOut", New Object(-1) {}, Me.LogUserOutOperationCompleted, userState)
        End Sub
        
        Private Sub OnLogUserOutOperationCompleted(ByVal arg As Object)
            If (Not (Me.LogUserOutCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent LogUserOutCompleted(Me, New LogUserOutCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        Public Shadows Sub CancelAsync(ByVal userState As Object)
            MyBase.CancelAsync(userState)
        End Sub
        
        Private Function IsLocalFileSystemWebService(ByVal url As String) As Boolean
            If ((url Is Nothing)  _
                        OrElse (url Is String.Empty)) Then
                Return false
            End If
            Dim wsUri As System.Uri = New System.Uri(url)
            If ((wsUri.Port >= 1024)  _
                        AndAlso (String.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) = 0)) Then
                Return true
            End If
            Return false
        End Function
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3062.0"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://webservices.autokiller.com/AutokillerWebService/rsa.asmx"),  _
     System.Xml.Serialization.XmlRootAttribute([Namespace]:="http://webservices.autokiller.com/AutokillerWebService/rsa.asmx", IsNullable:=false)>  _
    Partial Public Class AuthInfo
        Inherits System.Web.Services.Protocols.SoapHeader
        
        Private userNameField As String
        
        Private passwordField As String
        
        '''<remarks/>
        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set
                Me.userNameField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property Password() As String
            Get
                Return Me.passwordField
            End Get
            Set
                Me.passwordField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3062.0")>  _
    Public Delegate Sub EncryptFileCompletedEventHandler(ByVal sender As Object, ByVal e As EncryptFileCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3062.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class EncryptFileCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As Byte()
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),Byte())
            End Get
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3062.0")>  _
    Public Delegate Sub LogUserOutCompletedEventHandler(ByVal sender As Object, ByVal e As LogUserOutCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3062.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class LogUserOutCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As Integer
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),Integer)
            End Get
        End Property
    End Class
End Namespace
