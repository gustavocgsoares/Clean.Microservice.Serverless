﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Clean.Microservice.Serverless.SharedKernel.Core.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class HttpMessage {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal HttpMessage() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Clean.Microservice.Serverless.SharedKernel.Core.Resources.HttpMessage", typeof(HttpMessage).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INVALID_DATA_REQUEST.
        /// </summary>
        internal static string BAD_REQUEST_DEFAULT_CODE {
            get {
                return ResourceManager.GetString("BAD_REQUEST_DEFAULT_CODE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Dados enviados inválidos!.
        /// </summary>
        internal static string BAD_REQUEST_DEFAULT_MESSAGE {
            get {
                return ResourceManager.GetString("BAD_REQUEST_DEFAULT_MESSAGE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Não foi possível obter os dados solicitados..
        /// </summary>
        internal static string CACHE_MESSAGE_FAIL {
            get {
                return ResourceManager.GetString("CACHE_MESSAGE_FAIL", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Validação do campo {0} falhou!.
        /// </summary>
        internal static string COMPOSITE_VALIDATION_FAIL_MESSAGE {
            get {
                return ResourceManager.GetString("COMPOSITE_VALIDATION_FAIL_MESSAGE", resourceCulture);
            }
        }
    }
}
