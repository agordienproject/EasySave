﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EasySave.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.8.0.0")]
    public sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("en-EN")]
        public string CurrentCulture {
            get {
                return ((string)(this["CurrentCulture"]));
            }
            set {
                this["CurrentCulture"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(".\\\\Data\\\\States\\\\")]
        public string StateFolderPath {
            get {
                return ((string)(this["StateFolderPath"]));
            }
            set {
                this["StateFolderPath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("states.json")]
        public string StateFileName {
            get {
                return ((string)(this["StateFileName"]));
            }
            set {
                this["StateFileName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(".\\\\Data\\\\Logs\\\\")]
        public string LogsFolderPath {
            get {
                return ((string)(this["LogsFolderPath"]));
            }
            set {
                this["LogsFolderPath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("json")]
        public string LogsFileType {
            get {
                return ((string)(this["LogsFileType"]));
            }
            set {
                this["LogsFileType"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsd=\"http://www.w3." +
            "org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n  <s" +
            "tring>json</string>\r\n  <string>xml</string>\r\n  <string>html</string>\r\n</ArrayOfS" +
            "tring>")]
        public global::System.Collections.Specialized.StringCollection EncryptedExtensions {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["EncryptedExtensions"]));
            }
            set {
                this["EncryptedExtensions"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("calc")]
        public string BusinessAppName {
            get {
                return ((string)(this["BusinessAppName"]));
            }
            set {
                this["BusinessAppName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("100")]
        public int MaxKoToTransfert {
            get {
                return ((int)(this["MaxKoToTransfert"]));
            }
            set {
                this["MaxKoToTransfert"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsd=\"http://www.w3." +
            "org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n  <s" +
            "tring>json</string>\r\n  <string>xml</string>\r\n  <string>html</string>\r\n</ArrayOfS" +
            "tring>")]
        public global::System.Collections.Specialized.StringCollection PrioritizedExtensions {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["PrioritizedExtensions"]));
            }
            set {
                this["PrioritizedExtensions"] = value;
            }
        }
    }
}
