﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FbkiBot.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class CommandStrings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CommandStrings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("FbkiBot.Resources.CommandStrings", typeof(CommandStrings).Assembly);
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
        ///   Looks up a localized string similar to Ваше сообщение:.
        /// </summary>
        internal static string Cat_Found {
            get {
                return ResourceManager.GetString("Cat.Found", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Сообщения не найдены.
        /// </summary>
        internal static string Ls_Empty {
            get {
                return ResourceManager.GetString("Ls.Empty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Сообщения в данной категории:.
        /// </summary>
        internal static string Ls_Success {
            get {
                return ResourceManager.GetString("Ls.Success", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Монтирование с этим чатом уже существует.
        /// </summary>
        internal static string Mount_AlreadyExists {
            get {
                return ResourceManager.GetString("Mount.AlreadyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Команда вызвана из личного чата пользователя.
        /// </summary>
        internal static string Mount_IsPersonalChat {
            get {
                return ResourceManager.GetString("Mount.IsPersonalChat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Монтирование с таким названием уже существует.
        /// </summary>
        internal static string Mount_NameTaken {
            get {
                return ResourceManager.GetString("Mount.NameTaken", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Не передано название монтирования.
        /// </summary>
        internal static string Mount_NoName {
            get {
                return ResourceManager.GetString("Mount.NoName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Монтирование успешно произведено.
        /// </summary>
        internal static string Mount_Success {
            get {
                return ResourceManager.GetString("Mount.Success", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to У вас нет примонтированных чатов.
        /// </summary>
        internal static string Mounts_Empty {
            get {
                return ResourceManager.GetString("Mounts.Empty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ваши примонтированные чаты:.
        /// </summary>
        internal static string Mounts_Header {
            get {
                return ResourceManager.GetString("Mounts.Header", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Не передано название сохраненного сообщения.
        /// </summary>
        internal static string NoSavedMessageNameProvided {
            get {
                return ResourceManager.GetString("NoSavedMessageNameProvided", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Это сообщение может удалить только его автор.
        /// </summary>
        internal static string Rm_NotAuthor {
            get {
                return ResourceManager.GetString("Rm.NotAuthor", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Сообщение удалено.
        /// </summary>
        internal static string Rm_Success {
            get {
                return ResourceManager.GetString("Rm.Success", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Сообщение с таким названием уже существует в этом чате (оно отмечено в ответе).
        /// </summary>
        internal static string Save_NameTaken {
            get {
                return ResourceManager.GetString("Save.NameTaken", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Не выбрано сообщение для сохранения. Используйте функцию ответов в Telegram.
        /// </summary>
        internal static string Save_NoReply {
            get {
                return ResourceManager.GetString("Save.NoReply", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Сообщение сохранено.
        /// </summary>
        internal static string Save_Success {
            get {
                return ResourceManager.GetString("Save.Success", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Сохраненного сообщения с таким названием не найдено.
        /// </summary>
        internal static string SavedMessageNotFound {
            get {
                return ResourceManager.GetString("SavedMessageNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Бот для сохранения сообщений группы 14221 .
        /// </summary>
        internal static string Start_Welcome {
            get {
                return ResourceManager.GetString("Start.Welcome", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Монтирование не найдено.
        /// </summary>
        internal static string Umount_NotFound {
            get {
                return ResourceManager.GetString("Umount.NotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Монтирование удалено.
        /// </summary>
        internal static string Umount_Success {
            get {
                return ResourceManager.GetString("Umount.Success", resourceCulture);
            }
        }
    }
}
