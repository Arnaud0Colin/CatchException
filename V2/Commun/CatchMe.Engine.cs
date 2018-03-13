#region Copyright(c) 1998-2018, Arnaud Colin Licence GNU GPL version 3
/* Copyright(c) 1998-2018, Arnaud Colin
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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;

namespace CatchException
{


     public partial class CatchMe : MyException, IDisposable
    {
        public static string GetCurrentPath => System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName);

        public static string GetProcessName => System.Diagnostics.Process.GetCurrentProcess().ProcessName.Trim(System.IO.Path.GetInvalidFileNameChars());

        public static int? TheApplicationId { get; set; }

#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private void CheckApplicationNotNull()
        {
            if (TheApplicationId == null)
                ApplicationId = 0;
            else
                ApplicationId = TheApplicationId;
        }
        public CatchMe()
        {
            CheckApplicationNotNull();
            CurrentPath = GetCurrentPath;
            ProcessName = GetProcessName;
            ComputerName = SystemInformation.ComputerName;
            OsPlatform = Environment.OSVersion.Platform.ToString();
            OsVersion = Environment.OSVersion.VersionString.ToString();
            OsServicePack = Environment.OSVersion.ServicePack.ToString();
            var winlogin = WindowsIdentity.GetCurrent();
            Login = winlogin?.Name;

            Date = DateTime.Now;

            SID = new byte[winlogin.User.BinaryLength];
            winlogin.User.GetBinaryForm(SID, 0);


            System.Reflection.Assembly TheExe = null;
#if !WindowsCE

            if ((TheExe = System.Reflection.Assembly.GetEntryAssembly()) == null)
                TheExe = System.Reflection.Assembly.GetCallingAssembly();

            if(TheExe != null)
#else
             if ( (TheExe = MyAssembly) != null)            
#endif
            {
                this.Program = TheExe.GetName().Name;
                this.Version = TheExe.GetName().Version.ToString();
                Path = TheExe.ManifestModule.FullyQualifiedName;
            }

        }


        public int GetApplicationId()
        {
            if (ApplicationId.HasValue)
                return ApplicationId.Value;
            else
            {
                return -1;
            }
        }



#if WindowsCE
        public static string SerialNumber = string.Empty;
        public static System.Reflection.Assembly MyAssembly = null;
#endif


        IEnumerable<WebCatchException.MyExceptionDetail> GetMyExpDetail(Exception theEx)
        {
            Exception ex = null;
            int i = 1;
            if ((ex = theEx) != null)
            {
                while (ex != null)
                {
                    var value = new WebCatchException.MyExceptionDetail();

                    value.code = i;
#if NET45
                    value.HResult = ex.HResult;
#else
                   // value.HResult = ex.GetHResult();
                     value.HResult = 0;
#endif

                    value.Exception = ex.GetType().Name;

                    value.Message = ex.Message;
                    value.HelpLink = ex.HelpLink;
                    value.Source = ex.Source;
                    value.StackTrace = ex.StackTrace;
                    value.TargetSite = ex.TargetSite?.ToString();

                    if (ex.Data != null && ex.Data.Count > 0)
                    {
                        List<WebCatchException.ElVar> ld = new List<WebCatchException.ElVar>();

                        foreach (var fo in ex.Data.Keys)
                        {
                            var o = ex.Data[fo];

                            ld.Add(Affect(fo.ToString(), o));
                        }

                        value.Data = ld.ToArray();
                    }


                    yield return value;
                    i++;
                    ex = ex.InnerException;
                }
            }
        }

        public void Write()
        {

           if ((OutPut & RenderType.Wcf) == RenderType.Wcf)
                this.WriteWcf();

         
            if ((OutPut & RenderType.File) == RenderType.File)
                this.WriteFile();
        }

        public CatchMe Screen()
        {
            Bitmap = Tools.ScreenShot.TakeScreen();
            //  Bitmap = Tools.ScreenShot2.CaptureApplication();
            // Bitmap = Tools.ScreenShot.TakeScreen();
            return this;
        }

#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static CatchMe Instance()
        {
            return new CatchMe();
        }


#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static CatchMe WriteException(string message)
        {
            return CatchMe.Instance().CallException(new FransBonhommeException(message));
        }


#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static CatchMe WriteException(Exception ex)
        {
            return CatchMe.Instance().CallException(ex);
        }


     


#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static CatchMe WriteInfo(string message)
        {
            return CatchMe.Instance().CallException(new MessageInfo(message));
        }

        //#if NET45
        //        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //#endif
        //        public static CatchMe WriteMessage(Exception ex)
        //        {
        //            return CatchMe.Instance().CallException(ex);
        //        }

        //#if NET45
        //        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //#endif
        //        public static CatchMe WriteMessage(string message)
        //        {
        //            return CatchMe.Instance().CallException(new FransBonhommeException(message));
        //        }

        //#if NET45
        //        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //#endif
        //        private CatchMe CallException(string message)
        //        {
        //            this.Type = message != null ? message.GetType().Name : string.Empty;
        //            _Excep = GetMyExpDetail(new FransBonhommeException(message));
        //            return this;
        //        }

#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private CatchMe CallException(Exception ex)
        {
            this.Type = ex != null ? ex.GetType().Name : string.Empty;
            _Excep = GetMyExpDetail(ex);
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
        public CatchMe Where(string Method)
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

        public override string ToString()
        {
            return _Excep != null ? ExtentionException.GetMessage(_Excep) : null;
        }


        #region IDisposable Support
        private bool disposedValue = false; // Pour détecter les appels redondants

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }

                disposedValue = true;
            }
        }

        // Ce code est ajouté pour implémenter correctement le modèle supprimable.
        public  void Dispose()
        {
            Dispose(true);
            // GC.SuppressFinalize(this);
        }
        #endregion

    }

    public class MyException
    {

        public static explicit operator WebCatchException.MyException(MyException ex)
        {

            var value = new WebCatchException.MyException();

            value.OsPlatform = ex.OsPlatform;

            value.OsServicePack = ex.OsServicePack;
            value.OsVersion = ex.OsVersion;
            value.UrgenceLevel = ex.UrgenceLevel;
            value.ApplicationId = ex.ApplicationId;
            value.Date = ex.Date;
#if NET35
            value._Excep = ex._Excep != null ? ex._Excep.ToArray() : null;
#else
            value._Excep = ex._Excep != null ? NET20.ToArray(ex._Excep) : null;
#endif
            value.Method = ex.Method;
            value.sourceFilePath = ex.sourceFilePath;
            value.sourceLineNumber = ex.sourceLineNumber;
            if(ex.Bitmap != null)
                value.bitmap = CatchException.Tools.ScreenShot.GetBase64(ex.Bitmap, System.Drawing.Imaging.ImageFormat.Jpeg);
            value.CurrentPath = ex.CurrentPath;
            value.ProcessName = ex.ProcessName;
            value.ComputerName = ex.ComputerName;
            value.Login = ex.Login;
            value.SID = ex.SID;
            value.Type = ex.Type;
            value.Program = ex.Program;
            value.Debug = Debugger.IsAttached;
            value.Version = ex.Version;
            value.Path = ex.Path;
#if NET35
            value._Var = ex._Var != null ? ex._Var.ToArray() : null;
#else
            value._Var = ex._Var != null ? NET20.ToArray(ex._Var) : null;
#endif
            return value;
        }

        public string OsPlatform { get; set; }
        public string OsServicePack { get; set; }
        public string OsVersion { get; set; }
        public ushort? UrgenceLevel { get; set; }
        public int? ApplicationId { get; set; }

        public DateTime Date { get; set; }
        public IEnumerable<WebCatchException.MyExceptionDetail> _Excep { get; set; }

        public string Method { get; set; }

        public string sourceFilePath { get; set; }

        public int sourceLineNumber { get; set; }

        public Bitmap Bitmap { get; set; }

        public string CurrentPath { get; set; }

        public string ProcessName { get; set; }

        public string ComputerName { get; set; }


        public string Login { get; set; }


        public byte[] SID { get; set; }


        public string Type { get; set; }


        public string Program { get; set; }


        public string Version { get; set; }

        public bool Debug { get; set; }

        public HashSet<WebCatchException.ElVar> _Var { get; set; }

        public string Path { get; set; }

    }
}

