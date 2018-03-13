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
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

using System.Windows.Forms;

namespace CatchException
{

    static class DumpExt
    {
        internal static bool IsSimpleType(this Type type)
        {
            return (
                   type.IsPrimitive 
                || type.Equals(typeof(string))
#if !WindowsCE
                 || type.GetInterfaces().Contains( typeof(IFormattable))
#else
                || Compatibility.NET20.Contains(type.GetInterfaces(), typeof(IFormattable))
#endif
               
                ); 
        }

        //internal static string GetNullableArrayTypeName(this Type type)
        //{
        //    string h = null;
        //    int d = type.FullName.IndexOf('`') + 4;
        //    int f = type.FullName.IndexOf(',');
        //    if (f > d)
        //    {
        //        h = "<";
        //        h += type.FullName.Substring(d, f - d).Replace("System.", "");
        //        h += '>';
        //    }

        //    return h;
        //}


        internal static string GetTypeName(this Type type)
        {
            int pos = 0;
            StringBuilder ret = new StringBuilder();
            if ((pos = type.Name.IndexOf('`')) > 0)
            {
                ret.Append( type.Name.Substring(0, pos));
                if (type.Name.IndexOf("[]") > pos)
                {
                    if (type.BaseType?.Name == "Array")
                    {
                        int fullpos = type.FullName.IndexOf('`') + 4;
                        int lenght = type.FullName.IndexOf(',') - fullpos;
                        if (lenght > 0)
                        {
                            ret.Append('<');
                            ret.Append(type.FullName.Substring(fullpos, lenght).Replace("System.", ""));
                            ret.Append('>');
                        }
                    }
                    ret.Append("[]");
                }
            }
            else
                ret.Append( type.Name);

            if (!type.IsGenericType) return ret.ToString();
#if NET45
            if (type.GenericTypeArguments.Count() > 0)
            {
                ret.Append('<');
                ret.Append(string.Join(",", type.GenericTypeArguments.Select(p => GetTypeName(p))));
                ret.Append('>');
            }
#endif
            return ret.ToString();
        }
    }

    public class Dump
    {

        public static string GetValue(object Obj)
        {
            string ret = null;

            try
            {
                ret=  new Dump().Save(Obj, 0);
            }
            catch
            {
                ret = $"Erreur *--> {Obj?.GetType()}";
            }

            return ret;
        }

        public int limite = 50;
        private HashSet<object> CheckHash = null;
        private Mutex mutex = new Mutex();

        private string Write(string value, int Level)
        {
            return new string('\t', Level) + value;
        }

        public bool oldReadyDump(object Obj)
        {
            bool ret = false;
            this.mutex.WaitOne();

            if (CheckHash == null)
                CheckHash = new HashSet<object>();

#if NET35
            if (CheckHash.Any(x => object.ReferenceEquals(x, Obj)))
#else
            if ( NET20.Any(CheckHash, Obj))
#endif
                ret = true;
            else
                CheckHash.Add(Obj);
           
            this.mutex.ReleaseMutex();

            return ret;
        }

        public string Save(object Obj, int Level)
        {
            string ret = string.Empty;
            bool bNotEmpty = false;

            if (Obj == null)
                return Write("Null", Level);

            var type = Obj.GetType();
            string typename = type.GetTypeName();

            if (type.IsSimpleType())
            {
                return Obj.ToString();
            }

            if (type.Name.Equals("RuntimeType"))
                return string.Empty;


            if (oldReadyDump(Obj))
                return Write("<...>", Level);

            if (Level >= limite)
                return Write("...", Level);


            ret += "\r\n";

        ///   if(type == typeof(System.Collections.Specialized.NameValueCollection))
        //    {
        //        ret += SaveArray(Obj, Level);
        //        bNotEmpty |= true;
        //    }
        //    else
            if (type.IsArray )
            {
                 ret += SaveArray(Obj, Level);
                bNotEmpty |= true;
            }
            //   else if ( Obj is System.Collections.IEnumerable)
#if NET35
            else if (type.GetInterfaces().Any(x=> x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
#else
            else if (type.GetInterfaces().IsIEnumerable() )
#endif
            {
                 ret += SaveIEnumerable(Obj, Level);
                bNotEmpty |= true;
            }

            ret += GetFields(type, Obj, Level, ref bNotEmpty);
            ret += GetProperties(type, Obj, Level, ref bNotEmpty);
        //    ret += GetMembers(type, Obj, Level, ref bNotEmpty);

            if (!bNotEmpty)
                return Write("Empty", Level);

            return ret;
        }

        string GetMembers(Type type, object Obj, int Level, ref bool bNotEmpty)
        {
            StringBuilder ret = new StringBuilder();

#if NET35
            var Members = type.GetMembers().Where(x => (x is MethodInfo));
#else
            var Members = NET20.Where( type.GetMembers(), typeof(MethodInfo));
#endif

            if (Members != null && Members.Any())
            {
                foreach (var Member in Members)
                {
                    ret.Append(Save(Member, Obj, Level));
                    bNotEmpty |= true;
                }
            }

            return ret.ToString();
        }

        string GetFields(Type type , object Obj, int Level, ref bool bNotEmpty)
        {
            StringBuilder ret = new StringBuilder();

#if NET35
            IEnumerable<FieldInfo> Fields = type.GetFields().Where(x =>
             ((x.Attributes & FieldAttributes.InitOnly) != FieldAttributes.InitOnly) &&
             ((x.Attributes & FieldAttributes.Literal) != FieldAttributes.Literal) &&
             x.DeclaringType.IsGenericType == false
             );
#else
            IEnumerable<FieldInfo> Fields = NET20.FilterFields(type.GetFields());
#endif

            if (Fields != null && Fields.Any())
            {
                foreach (var Field in Fields)
                {
                    ret.Append(Save(Field, Obj, Level));
                    bNotEmpty |= true;
                }
            }

            return ret.ToString();
        }

        string GetProperties(Type type, object Obj, int Level, ref bool bNotEmpty)
        {
            StringBuilder ret = new StringBuilder();

#if NET35
            var Properties = type.GetProperties().Where(x =>
                    // ( x.GetGetMethod().Attributes & MethodAttributes.Static) != MethodAttributes.Static &&
                    !x.PropertyType.Equals(typeof(BindingManagerBase)) &&
                    !x.PropertyType.Equals(typeof(AccessibleRole)) &&
                    !x.PropertyType.Equals(typeof(ControlBindingsCollection)) &&
                    !x.PropertyType.Equals(typeof(Control)) &&
                     !x.PropertyType.Equals(typeof(MethodBase))// &&                        x.DeclaringType.IsGenericType == false
                   );
#else
           var Properties = NET20.FilterProperties(type.GetProperties());

#endif

            if (Properties != null && Properties.Any())
            {
                foreach (var Propertie in Properties)
                {
                    ret.Append( Save(Propertie, Obj, Level) );
                    bNotEmpty |= true;
                }
            }
            return ret.ToString();
        }


        string Save<T>(T Element, object Obj, int Level) where T : MemberInfo
        {
            string ret = string.Empty;
            string Value = null;
            object Objval = null;
            Type type = null;
            bool isStatic = false;
         

            if (Element is FieldInfo)
            {
                var Field = (Element as FieldInfo);
                Objval = Field.GetValue(Obj);
                type = Field.FieldType;
                isStatic = Field.IsStatic;
            }
            else if (Element is PropertyInfo)
            {
                var Property = (Element as PropertyInfo);

                if (!Property.GetIndexParameters().Any())
                {
#if NET45
                    Objval = Property.GetValue(Obj);
#else
                    Objval = Property.GetValue(Obj, null);
#endif
                }
                else
                {

                    var asq = Property.GetIndexParameters().FirstOrDefault().ParameterType;

                    string str = null;

                    var IEnuObj = Obj as System.Collections.IEnumerable;
                    if (IEnuObj != null)
                    {
                        int i = 0;
                        str += "{";
                        foreach (var key in IEnuObj)
                        {
                            if (key.GetType() != asq)
                                goto fff;

                            if (i > 0)
                                str += ",";
                            if (i > 50)
                            {
                                str += $"...";
                                break;
                            }

                            Objval = Property.GetValue(Obj, new object[] { key });
                            str += $"{key} = " + Save(Objval, Level + 1);
                            i++;
                               

                           
                        }

                        str += "}";
                    }

                    Objval = str;
                    fff:;
                }

                type = Property.PropertyType;
                isStatic = ((Property.GetGetMethod().Attributes & MethodAttributes.Static) == MethodAttributes.Static);
              //  isStatic = Property.Attributes & PropertyAttributes.s;
            }
            else if (Element is MemberInfo)
            {
                //  (element as MemberInfo).in
            }

            if (Objval == null)
            {
                Value = "Null";
            }
            else
            {

                if (type == Obj.GetType())
                    return null;

                if (type.IsSimpleType())
                {
                    Value = Objval.ToString();
                }
                else if (type.IsArray)
                {
                    Value = SaveArray(Objval, Level + 1);
                }
                else
                {
                    Value = Save(Objval, Level + 1);
                }
            }

            ret += Write(string.Format("[{0}] {1} : {2} \r\n", type?.GetTypeName(), Element.Name, Value), Level);

            return ret;
        }

        string SaveArray(object Obj, int level)
        {
            string ret = string.Empty;
            if (Obj == null)
                ret = "Null";
            else
           if (Obj.GetType().IsArray)
            {
                var dimension = Obj.GetType().GetArrayRank();
                var length = ((Array)Obj).Length;
                ret += "{";
                //if (length > 50)
                //{
                //    ret += $"...";
                //}
                //else
                    for (int i = 0; i < length / dimension; i++)
                    {
                        if (i > 0)
                            ret += ",";
                        for (int dim = 0; dim < dimension; dim++)
                        {
                            if (dim == 0 && dimension > 1)
                            {
                                ret += "{";
                            }
                            if (dim > 0)
                            {
                                ret += ",";
                            }

                            object val = null;

                            if (dimension == 1)
                                val = ((Array)Obj).GetValue(i);
                            else
                                val = ((Array)Obj).GetValue(i, dim);

                            ret += Save(val, level);

                            if (dim > 0 && dim == (dimension - 1))
                            {
                                ret += "}";
                            }

                        }

                    if (i > limite)
                    {
                       ret += $"...";
                       break;
                    }

                }
                ret += "}";

            }
            else
                throw new Exception();

            return ret;
        }

        string SaveIEnumerable(object Obj, int level)
        {
            string Value = null;
            string ret = string.Empty;
            string typeEnum = null;
            if (Obj == null)
                Value = "Null";
            else
            {
                int? count = null;

                PropertyInfo CountProperty = Obj.GetType().GetProperty("Count");
                if (CountProperty == null)
                    CountProperty = Obj.GetType().GetProperty("Length");

                if (CountProperty != null)
#if NET45
                    count = CountProperty.GetValue(Obj) as int?;
#else
                 count = CountProperty.GetValue(Obj, null) as int?;
#endif

                var ienum = Obj.GetType().GetInterface(typeof(IEnumerable<>).Name);
                typeEnum = ienum != null ? ienum.GetGenericArguments()[0].Name : null;

                System.Collections.IEnumerable ienu = Obj as System.Collections.IEnumerable;

                if (ienu == null)
                {
                    Value = "Null";
                }         
                else
                {
                    int i = 0;
                    Value += "{";
                    foreach (var f in ienu)
                    {
                        if (i > 0)
                            Value += ",";
                        if (i > 50)
                        {
                            Value += $"...";
                            break;
                        }

                        Value += Save(f, level + 1);
                        i++;
                    }
                    Value += "}";
                }
            }

            ret += Write(string.Format("[{0}] {1} : {2} \r\n", string.Format("IEnumerable<{0}>", typeEnum), "", Value), level);

            return ret;
        }
    }
}
