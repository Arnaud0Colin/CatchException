#region Copyright(c) 1998-2014, Arnaud Colin Licence GNU GPL version 3
/* Copyright(c) 1998-2014, Arnaud Colin
 * All rights reserved.
 *
 * Licence GNU GPL version 3
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *
 *   -> Redistributions of source code must retain the above copyright
 *      notice, this list of conditions and the following disclaimer.
 *
 *   -> Redistributions in binary form must reproduce the above copyright
 *      notice, this list of conditions and the following disclaimer in the
 *      documentation and/or other materials provided with the distribution.
 */
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
#if NET35
using System.Linq;
using System.Xml.Linq;
#endif
using System.Runtime.CompilerServices;
#if !WindowsCE
using System.Security.Principal;
#endif
using System.Text;
using System.Windows.Forms;

using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;


namespace LogTrace.CatchException
{
    [Flags]
    public enum CatchAction 
    {
        File,
        Smtp,
        Wcf,
        DB
    }

    public struct ElVar
    {
        public string Type;
        public object Valeur;
    }


    public partial class CatchMe : ICatchMe, IDisposable
    {
        private static List<ICatchAction> _StaticListAction = new List<ICatchAction>();
        public ushort? UrgenceLevel { get; private set; }
        public static int? ApplicationId { /*private*/ get; set; }

        public int GetApplicationId()
        {
            if (ApplicationId.HasValue)
                return ApplicationId.Value;
            else
               return -1;
           
        }

        private Exception _Excep = null;
        private Dictionary<string, ElVar> _Var = new Dictionary<string, ElVar>();

        private string Method = string.Empty;
        private string sourceFilePath = string.Empty;
        private int sourceLineNumber = 0;
        private Bitmap bitmap = null;

        private List<ICatchAction> _ListAction = new List<ICatchAction>();

#if WindowsCE
        public static string SerialNumber = string.Empty;
        public static System.Reflection.Assembly MyAssembly = null;
#endif


        public string ComputerName
        {
#if WindowsCE
            get { return SerialNumber; }
#else
            get { return SystemInformation.ComputerName; }
#endif
        }


#if  NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static void CheckApplicationNotNull()
        {
            if (ApplicationId == null)
                ApplicationId = 0;
        }

        CatchMe()
        {
            CheckApplicationNotNull();
        }

        public static void Bind<T>() where T : ICatchAction, new()
        {
            if (!isExist(_StaticListAction, typeof(T)))
                _StaticListAction.Add(new T());
        }


        public void Write()
        {
            foreach (ICatchAction f in _StaticListAction)
            {
                f.Write(this);
            }
            foreach (ICatchAction f in _ListAction)
            {
                f.Write(this);
            }
            this.Dispose();
        }


        public void Write<T>() where T : ICatchAction, new()
        {

            foreach (ICatchAction f in _StaticListAction)
            {
                f.Write(this);
            }
            
            if (!isExist(_StaticListAction, typeof(T)))
                new T().Write(this);

        }

        public void WriteOnly<T>() where T : ICatchAction, new()
        {
            if (!isExist(_StaticListAction, typeof(T)))
                new T().Write(this);
        }


        public static bool isExist(List<ICatchAction> list, Type type)
        {
#if NET35
            var Exist = list.Where(p => p.GetType().Equals(type));
            return Exist.Count() > 0;
#else
            foreach (ICatchAction Item in list)
            {
                if (Item.GetType().Equals(type))
                    return true;
            }

            return false;
#endif    
        }


        public CatchMe To<T>() where T : ICatchAction, new()
        {
             if( !isExist(_ListAction, typeof(T)))
                 _ListAction.Add(new T());

            return this;
        }


        public CatchMe To(CatchAction act ) 
        {

            if ((act & CatchAction.File) == CatchAction.File)
                To<LogFile>();

            if ((act & CatchAction.Smtp) == CatchAction.Smtp)
                To<LogSmtp>();

            if ((act & CatchAction.Wcf) == CatchAction.Wcf)
                To<LogWcf>();

            if ((act & CatchAction.DB) == CatchAction.DB)
                To<LogDB>();

            return this;
        }

        public CatchMe Screen()
        {
            bitmap = ScreenShot.TakeScreen();
            return this;
        }

      
#if  NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static CatchMe Instance()
        {
            return new CatchMe();
        }

        public  void Dispose()
        {
            
        }


#if  NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static CatchMe WriteException(Exception ex)
        {
            return CatchMe.Instance().CallException(ex);
        }

#if  NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static CatchMe WriteMessage(string message)
        {
            return CatchMe.Instance().CallMessage(message);
        }


#if  NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static CatchMe WriteMessage(Exception ex)
        {
            return CatchMe.Instance().CallException(ex);
        }

#if  NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static CatchMe WriteException(string message)
        {
            return CatchMe.Instance().CallMessage(message);
        }


#if  NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private CatchMe CallException(Exception ex)
        {
            _Excep = ex;
            return this;
        }

#if  NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private CatchMe CallMessage(string message)
        {
            _Excep = new Exception(message);
            return this;
        }


#if NET45
                public CatchMe Where([CallerMemberName] string Method = "", 
                                [CallerFilePath] string sourceFilePath = "", 
                                [CallerLineNumber] int sourceLineNumber = 0)
#else
#if !WindowsCE
        public CatchMe Where(string Method = "",
                                string sourceFilePath = "",
                                int sourceLineNumber = 0)
#else
         public CatchMe Where( string Method ,
                                string sourceFilePath ,
                                int sourceLineNumber)
#endif
#endif
        {
            this.Method = Method;
            this.sourceFilePath = sourceFilePath;
            this.sourceLineNumber = sourceLineNumber;
            return this;
        }


        /// <summary>
        /// <para>
        /// <param name="Method">string</param>
        /// </para>
        /// Write debug information in the xml 
        /// <example> CLogger CLogFile.Open().Where("KeyDown").Write(); </example>/// 
        /// <returns>CLogger</returns>
        /// </summary>
     public CatchMe Where( string Method)
        {
            this.Method = Method;
            return this;
        }


     public CatchMe Level(ushort level)
     {
         if (UrgenceLevel.HasValue)
         {
             if (UrgenceLevel.Value > level)
                 UrgenceLevel = level;
         }
         else
             UrgenceLevel = level;
      
         return this;
     }

     public CatchMe Variable<U>(string nom, U instance)
     {

         
#if NET20
            while (_Var.ContainsKey(nom))
#else
            while(_Var.Any(p => p.Key == nom))
#endif  
            {
                nom += '-';
            }

            try
            {
                _Var.Add(nom, new ElVar() { Type = GetTypeName(typeof(U)), Valeur = instance });
                
            }
            catch
            { 
            }
         return this;
     }


     public static string VariableType<U>( U instance)
     {
         return GetTypeName(typeof(U));

     }

      private static string GetNullableArrayTypeName(Type type)
     {
         string h = null;
         int d = type.FullName.IndexOf('`') + 4;
         int f = type.FullName.IndexOf(',');
         if (f > d)
         {
             h = "<";
             h += type.FullName.Substring(d, f - d).Replace("System.", "");
             h += '>';
         }

         return h;
     }


     /*private*/ public static string GetTypeName(Type type)
     {
         int pos = 0;
         string h = null;
         if ((pos = type.Name.IndexOf('`')) > 0)
         {
             h = type.Name.Substring(0, pos);
             if (type.Name.IndexOf("[]") > pos)
             {
                 if (type.BaseType.Name == "Array")
                     h += GetNullableArrayTypeName(type);

                 h += "[]";
             }
         }
         else
             h = type.Name;

         if (!type.IsGenericType) return h;
#if NET45
         if (type.GenericTypeArguments.Count() > 0)
         {
             h += '<';
             h += type.GenericTypeArguments.Select(p => GetTypeName(p)).Aggregate((x, y) => x + ',' + y);
             h += '>';
         }
#endif
         return h;
     }




    public override string ToString()
    {
        return string.Empty;
    }


    }
}
