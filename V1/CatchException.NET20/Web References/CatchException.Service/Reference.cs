﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.34014
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// Ce code source a été automatiquement généré par Microsoft.VSDesigner, Version 4.0.30319.34014.
// 
#pragma warning disable 1591

namespace LogTrace.CatchException.Service {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="BasicHttpBinding_ICatchException", Namespace="http://tempuri.org/")]
    public partial class CatchException : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback WriteInJournalOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public CatchException() {
            this.Url = global::LogTrace.Properties.Settings.Default.CatchException_NET20_CatchException_Service_CatchException;
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
        public event WriteInJournalCompletedEventHandler WriteInJournalCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ICatchException/WriteInJournal", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void WriteInJournal([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string Ordi, int App, [System.Xml.Serialization.XmlIgnoreAttribute()] bool AppSpecified, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string xml) {
            this.Invoke("WriteInJournal", new object[] {
                        Ordi,
                        App,
                        AppSpecified,
                        xml});
        }
        
        /// <remarks/>
        public void WriteInJournalAsync(string Ordi, int App, bool AppSpecified, string xml) {
            this.WriteInJournalAsync(Ordi, App, AppSpecified, xml, null);
        }
        
        /// <remarks/>
        public void WriteInJournalAsync(string Ordi, int App, bool AppSpecified, string xml, object userState) {
            if ((this.WriteInJournalOperationCompleted == null)) {
                this.WriteInJournalOperationCompleted = new System.Threading.SendOrPostCallback(this.OnWriteInJournalOperationCompleted);
            }
            this.InvokeAsync("WriteInJournal", new object[] {
                        Ordi,
                        App,
                        AppSpecified,
                        xml}, this.WriteInJournalOperationCompleted, userState);
        }
        
        private void OnWriteInJournalOperationCompleted(object arg) {
            if ((this.WriteInJournalCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.WriteInJournalCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    public delegate void WriteInJournalCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
}

#pragma warning restore 1591