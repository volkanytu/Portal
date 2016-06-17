﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.18444.
// 
#pragma warning disable 1591

namespace GK.WindowsServices.SendSms.smsService {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="smsserviceSoap", Namespace="http://83.66.137.24/PgApiWs")]
    public partial class smsservice : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback SmsInsert_1_NOperationCompleted;
        
        private System.Threading.SendOrPostCallback SmsInsert_N_NOperationCompleted;
        
        private System.Threading.SendOrPostCallback SmsStatusOperationCompleted;
        
        private System.Threading.SendOrPostCallback CreditBalanceOperationCompleted;
        
        private System.Threading.SendOrPostCallback IdCheckOperationCompleted;
        
        private System.Threading.SendOrPostCallback IdCheck2OperationCompleted;
        
        private System.Threading.SendOrPostCallback SmsStatus2OperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public smsservice() {
            this.Url = global::GK.WindowsServices.SendSms.Properties.Settings.Default.GK_WindowsServices_SendSms_smsService_smsservice;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event SmsInsert_1_NCompletedEventHandler SmsInsert_1_NCompleted;
        
        /// <remarks/>
        public event SmsInsert_N_NCompletedEventHandler SmsInsert_N_NCompleted;
        
        /// <remarks/>
        public event SmsStatusCompletedEventHandler SmsStatusCompleted;
        
        /// <remarks/>
        public event CreditBalanceCompletedEventHandler CreditBalanceCompleted;
        
        /// <remarks/>
        public event IdCheckCompletedEventHandler IdCheckCompleted;
        
        /// <remarks/>
        public event IdCheck2CompletedEventHandler IdCheck2Completed;
        
        /// <remarks/>
        public event SmsStatus2CompletedEventHandler SmsStatus2Completed;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://83.66.137.24/PgApiWs/SmsInsert_1_N", RequestNamespace="http://83.66.137.24/PgApiWs", ResponseNamespace="http://83.66.137.24/PgApiWs", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string[] SmsInsert_1_N(string Username, string Password, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] System.Nullable<System.DateTime> SendDate, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] System.Nullable<System.DateTime> ExpireDate, string[] Recepients, string Message) {
            object[] results = this.Invoke("SmsInsert_1_N", new object[] {
                        Username,
                        Password,
                        SendDate,
                        ExpireDate,
                        Recepients,
                        Message});
            return ((string[])(results[0]));
        }
        
        /// <remarks/>
        public void SmsInsert_1_NAsync(string Username, string Password, System.Nullable<System.DateTime> SendDate, System.Nullable<System.DateTime> ExpireDate, string[] Recepients, string Message) {
            this.SmsInsert_1_NAsync(Username, Password, SendDate, ExpireDate, Recepients, Message, null);
        }
        
        /// <remarks/>
        public void SmsInsert_1_NAsync(string Username, string Password, System.Nullable<System.DateTime> SendDate, System.Nullable<System.DateTime> ExpireDate, string[] Recepients, string Message, object userState) {
            if ((this.SmsInsert_1_NOperationCompleted == null)) {
                this.SmsInsert_1_NOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSmsInsert_1_NOperationCompleted);
            }
            this.InvokeAsync("SmsInsert_1_N", new object[] {
                        Username,
                        Password,
                        SendDate,
                        ExpireDate,
                        Recepients,
                        Message}, this.SmsInsert_1_NOperationCompleted, userState);
        }
        
        private void OnSmsInsert_1_NOperationCompleted(object arg) {
            if ((this.SmsInsert_1_NCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SmsInsert_1_NCompleted(this, new SmsInsert_1_NCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://83.66.137.24/PgApiWs/SmsInsert_N_N", RequestNamespace="http://83.66.137.24/PgApiWs", ResponseNamespace="http://83.66.137.24/PgApiWs", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string[] SmsInsert_N_N(string Username, string Password, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] System.Nullable<System.DateTime> SendDate, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] System.Nullable<System.DateTime> ExpireDate, string[] Recepients, string[] Messages) {
            object[] results = this.Invoke("SmsInsert_N_N", new object[] {
                        Username,
                        Password,
                        SendDate,
                        ExpireDate,
                        Recepients,
                        Messages});
            return ((string[])(results[0]));
        }
        
        /// <remarks/>
        public void SmsInsert_N_NAsync(string Username, string Password, System.Nullable<System.DateTime> SendDate, System.Nullable<System.DateTime> ExpireDate, string[] Recepients, string[] Messages) {
            this.SmsInsert_N_NAsync(Username, Password, SendDate, ExpireDate, Recepients, Messages, null);
        }
        
        /// <remarks/>
        public void SmsInsert_N_NAsync(string Username, string Password, System.Nullable<System.DateTime> SendDate, System.Nullable<System.DateTime> ExpireDate, string[] Recepients, string[] Messages, object userState) {
            if ((this.SmsInsert_N_NOperationCompleted == null)) {
                this.SmsInsert_N_NOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSmsInsert_N_NOperationCompleted);
            }
            this.InvokeAsync("SmsInsert_N_N", new object[] {
                        Username,
                        Password,
                        SendDate,
                        ExpireDate,
                        Recepients,
                        Messages}, this.SmsInsert_N_NOperationCompleted, userState);
        }
        
        private void OnSmsInsert_N_NOperationCompleted(object arg) {
            if ((this.SmsInsert_N_NCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SmsInsert_N_NCompleted(this, new SmsInsert_N_NCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://83.66.137.24/PgApiWs/SmsStatus", RequestNamespace="http://83.66.137.24/PgApiWs", ResponseNamespace="http://83.66.137.24/PgApiWs", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string[] SmsStatus(string Username, string Password, string[] MessageIDs) {
            object[] results = this.Invoke("SmsStatus", new object[] {
                        Username,
                        Password,
                        MessageIDs});
            return ((string[])(results[0]));
        }
        
        /// <remarks/>
        public void SmsStatusAsync(string Username, string Password, string[] MessageIDs) {
            this.SmsStatusAsync(Username, Password, MessageIDs, null);
        }
        
        /// <remarks/>
        public void SmsStatusAsync(string Username, string Password, string[] MessageIDs, object userState) {
            if ((this.SmsStatusOperationCompleted == null)) {
                this.SmsStatusOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSmsStatusOperationCompleted);
            }
            this.InvokeAsync("SmsStatus", new object[] {
                        Username,
                        Password,
                        MessageIDs}, this.SmsStatusOperationCompleted, userState);
        }
        
        private void OnSmsStatusOperationCompleted(object arg) {
            if ((this.SmsStatusCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SmsStatusCompleted(this, new SmsStatusCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://83.66.137.24/PgApiWs/CreditBalance", RequestNamespace="http://83.66.137.24/PgApiWs", ResponseNamespace="http://83.66.137.24/PgApiWs", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int CreditBalance(string Username, string Password) {
            object[] results = this.Invoke("CreditBalance", new object[] {
                        Username,
                        Password});
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void CreditBalanceAsync(string Username, string Password) {
            this.CreditBalanceAsync(Username, Password, null);
        }
        
        /// <remarks/>
        public void CreditBalanceAsync(string Username, string Password, object userState) {
            if ((this.CreditBalanceOperationCompleted == null)) {
                this.CreditBalanceOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCreditBalanceOperationCompleted);
            }
            this.InvokeAsync("CreditBalance", new object[] {
                        Username,
                        Password}, this.CreditBalanceOperationCompleted, userState);
        }
        
        private void OnCreditBalanceOperationCompleted(object arg) {
            if ((this.CreditBalanceCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CreditBalanceCompleted(this, new CreditBalanceCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://83.66.137.24/PgApiWs/IdCheck", RequestNamespace="http://83.66.137.24/PgApiWs", ResponseNamespace="http://83.66.137.24/PgApiWs", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string IdCheck(string Username, string Password) {
            object[] results = this.Invoke("IdCheck", new object[] {
                        Username,
                        Password});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void IdCheckAsync(string Username, string Password) {
            this.IdCheckAsync(Username, Password, null);
        }
        
        /// <remarks/>
        public void IdCheckAsync(string Username, string Password, object userState) {
            if ((this.IdCheckOperationCompleted == null)) {
                this.IdCheckOperationCompleted = new System.Threading.SendOrPostCallback(this.OnIdCheckOperationCompleted);
            }
            this.InvokeAsync("IdCheck", new object[] {
                        Username,
                        Password}, this.IdCheckOperationCompleted, userState);
        }
        
        private void OnIdCheckOperationCompleted(object arg) {
            if ((this.IdCheckCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.IdCheckCompleted(this, new IdCheckCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://83.66.137.24/PgApiWs/IdCheck2", RequestNamespace="http://83.66.137.24/PgApiWs", ResponseNamespace="http://83.66.137.24/PgApiWs", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string IdCheck2(string Username, string Password) {
            object[] results = this.Invoke("IdCheck2", new object[] {
                        Username,
                        Password});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void IdCheck2Async(string Username, string Password) {
            this.IdCheck2Async(Username, Password, null);
        }
        
        /// <remarks/>
        public void IdCheck2Async(string Username, string Password, object userState) {
            if ((this.IdCheck2OperationCompleted == null)) {
                this.IdCheck2OperationCompleted = new System.Threading.SendOrPostCallback(this.OnIdCheck2OperationCompleted);
            }
            this.InvokeAsync("IdCheck2", new object[] {
                        Username,
                        Password}, this.IdCheck2OperationCompleted, userState);
        }
        
        private void OnIdCheck2OperationCompleted(object arg) {
            if ((this.IdCheck2Completed != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.IdCheck2Completed(this, new IdCheck2CompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://83.66.137.24/PgApiWs/SmsStatus2", RequestNamespace="http://83.66.137.24/PgApiWs", ResponseNamespace="http://83.66.137.24/PgApiWs", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string[] SmsStatus2(string Username, string Password, string[] MessageIDs) {
            object[] results = this.Invoke("SmsStatus2", new object[] {
                        Username,
                        Password,
                        MessageIDs});
            return ((string[])(results[0]));
        }
        
        /// <remarks/>
        public void SmsStatus2Async(string Username, string Password, string[] MessageIDs) {
            this.SmsStatus2Async(Username, Password, MessageIDs, null);
        }
        
        /// <remarks/>
        public void SmsStatus2Async(string Username, string Password, string[] MessageIDs, object userState) {
            if ((this.SmsStatus2OperationCompleted == null)) {
                this.SmsStatus2OperationCompleted = new System.Threading.SendOrPostCallback(this.OnSmsStatus2OperationCompleted);
            }
            this.InvokeAsync("SmsStatus2", new object[] {
                        Username,
                        Password,
                        MessageIDs}, this.SmsStatus2OperationCompleted, userState);
        }
        
        private void OnSmsStatus2OperationCompleted(object arg) {
            if ((this.SmsStatus2Completed != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SmsStatus2Completed(this, new SmsStatus2CompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void SmsInsert_1_NCompletedEventHandler(object sender, SmsInsert_1_NCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SmsInsert_1_NCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SmsInsert_1_NCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void SmsInsert_N_NCompletedEventHandler(object sender, SmsInsert_N_NCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SmsInsert_N_NCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SmsInsert_N_NCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void SmsStatusCompletedEventHandler(object sender, SmsStatusCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SmsStatusCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SmsStatusCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void CreditBalanceCompletedEventHandler(object sender, CreditBalanceCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CreditBalanceCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal CreditBalanceCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void IdCheckCompletedEventHandler(object sender, IdCheckCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class IdCheckCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal IdCheckCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void IdCheck2CompletedEventHandler(object sender, IdCheck2CompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class IdCheck2CompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal IdCheck2CompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void SmsStatus2CompletedEventHandler(object sender, SmsStatus2CompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SmsStatus2CompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SmsStatus2CompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string[])(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591