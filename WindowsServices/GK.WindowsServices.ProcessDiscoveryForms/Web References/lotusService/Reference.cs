﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace GK.WindowsServices.ProcessDiscoveryForms.lotusService {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="DominoSoapBinding", Namespace="urn:DefaultNamespace")]
    public partial class UcretsizKesifService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback CREATERECORDOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public UcretsizKesifService() {
            this.Url = global::GK.WindowsServices.ProcessDiscoveryForms.Properties.Settings.Default.GK_WindowsServices_ProcessDiscoveryForms_lotusService_UcretsizKesifService;
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
        public event CREATERECORDCompletedEventHandler CREATERECORDCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("CREATERECORD", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("CREATERECORDReturn", Namespace="urn:DefaultNamespace")]
        public RESPONSE CREATERECORD([System.Xml.Serialization.XmlElementAttribute(Namespace="urn:DefaultNamespace")] string PASSWORD, [System.Xml.Serialization.XmlElementAttribute(Namespace="urn:DefaultNamespace")] double ID, [System.Xml.Serialization.XmlElementAttribute(Namespace="urn:DefaultNamespace")] string AD, [System.Xml.Serialization.XmlElementAttribute(Namespace="urn:DefaultNamespace")] string SOYAD, [System.Xml.Serialization.XmlElementAttribute(Namespace="urn:DefaultNamespace")] string EMAIL, [System.Xml.Serialization.XmlElementAttribute(Namespace="urn:DefaultNamespace")] string TEL, [System.Xml.Serialization.XmlElementAttribute(Namespace="urn:DefaultNamespace")] string SAAT, [System.Xml.Serialization.XmlElementAttribute(Namespace="urn:DefaultNamespace")] string IL, [System.Xml.Serialization.XmlElementAttribute(Namespace="urn:DefaultNamespace")] string ILCE, [System.Xml.Serialization.XmlElementAttribute(Namespace="urn:DefaultNamespace")] string KONUTTIPI, [System.Xml.Serialization.XmlElementAttribute(Namespace="urn:DefaultNamespace")] string OKUNDU, [System.Xml.Serialization.XmlElementAttribute(Namespace="urn:DefaultNamespace")] string BASVURUTARIH, [System.Xml.Serialization.XmlElementAttribute(Namespace="urn:DefaultNamespace")] string NEREDEGORDUNUZ, [System.Xml.Serialization.XmlElementAttribute(Namespace="urn:DefaultNamespace")] string ANAHTARCI) {
            object[] results = this.Invoke("CREATERECORD", new object[] {
                        PASSWORD,
                        ID,
                        AD,
                        SOYAD,
                        EMAIL,
                        TEL,
                        SAAT,
                        IL,
                        ILCE,
                        KONUTTIPI,
                        OKUNDU,
                        BASVURUTARIH,
                        NEREDEGORDUNUZ,
                        ANAHTARCI});
            return ((RESPONSE)(results[0]));
        }
        
        /// <remarks/>
        public void CREATERECORDAsync(string PASSWORD, double ID, string AD, string SOYAD, string EMAIL, string TEL, string SAAT, string IL, string ILCE, string KONUTTIPI, string OKUNDU, string BASVURUTARIH, string NEREDEGORDUNUZ, string ANAHTARCI) {
            this.CREATERECORDAsync(PASSWORD, ID, AD, SOYAD, EMAIL, TEL, SAAT, IL, ILCE, KONUTTIPI, OKUNDU, BASVURUTARIH, NEREDEGORDUNUZ, ANAHTARCI, null);
        }
        
        /// <remarks/>
        public void CREATERECORDAsync(string PASSWORD, double ID, string AD, string SOYAD, string EMAIL, string TEL, string SAAT, string IL, string ILCE, string KONUTTIPI, string OKUNDU, string BASVURUTARIH, string NEREDEGORDUNUZ, string ANAHTARCI, object userState) {
            if ((this.CREATERECORDOperationCompleted == null)) {
                this.CREATERECORDOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCREATERECORDOperationCompleted);
            }
            this.InvokeAsync("CREATERECORD", new object[] {
                        PASSWORD,
                        ID,
                        AD,
                        SOYAD,
                        EMAIL,
                        TEL,
                        SAAT,
                        IL,
                        ILCE,
                        KONUTTIPI,
                        OKUNDU,
                        BASVURUTARIH,
                        NEREDEGORDUNUZ,
                        ANAHTARCI}, this.CREATERECORDOperationCompleted, userState);
        }
        
        private void OnCREATERECORDOperationCompleted(object arg) {
            if ((this.CREATERECORDCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CREATERECORDCompleted(this, new CREATERECORDCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1067.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:DefaultNamespace")]
    public partial class RESPONSE {
        
        private short eRRORCODEField;
        
        private string eRRORDESCRIPTIONField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public short ERRORCODE {
            get {
                return this.eRRORCODEField;
            }
            set {
                this.eRRORCODEField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ERRORDESCRIPTION {
            get {
                return this.eRRORDESCRIPTIONField;
            }
            set {
                this.eRRORDESCRIPTIONField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void CREATERECORDCompletedEventHandler(object sender, CREATERECORDCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CREATERECORDCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal CREATERECORDCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public RESPONSE Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((RESPONSE)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591