using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebCatchException
{
    

    [DataContract]
    public class ElVar
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Value { get; set; }
    }

    [DataContract]
    public class MyExceptionDetail
    {
        [DataMember]
        public int code { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public int HResult { get; set; }

        [DataMember]
        public string HelpLink { get; set; }

        [DataMember]
        public string Source { get; set; }


        [DataMember]
        public string Exception { get; set; }

        [DataMember]
        public string StackTrace { get; set; }

        [DataMember]
        public string TargetSite { get; set; }

        [DataMember]
        public ElVar[] Data { get; set; }

    }



    [DataContract]
    public class MyException
    {
        [DataMember]
        public string OsPlatform { get; set; }
        [DataMember]
        public string OsServicePack { get; set; }
        [DataMember]
        public string OsVersion { get; set; }

        [DataMember]
        public ushort? UrgenceLevel { get; set; }

        [DataMember]
        public int? ApplicationId { get; set; }

        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public MyExceptionDetail[] _Excep { get; set; }
        [DataMember]
        public string Method { get; set; }
        [DataMember]
        public string sourceFilePath { get; set; }
        [DataMember]
        public int sourceLineNumber { get; set; }
        [DataMember]
        public string bitmap { get; set; }
        [DataMember]
        public string CurrentPath { get; set; }
        [DataMember]
        public string ProcessName { get; set; }
        [DataMember]
        public string ComputerName { get; set; }

        [DataMember]
        public bool Debug { get; set; }

        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public byte[] SID { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Program { get; set; }

        [DataMember]
        public string Version { get; set; }

        [DataMember]
        public string Path { get; set; }


        [DataMember]
        public ElVar[] _Var { get; set; }


    }
}