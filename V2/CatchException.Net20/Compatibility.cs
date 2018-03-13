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
using System.Text;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace System.Runtime.CompilerServices
    {
        [AttributeUsage(AttributeTargets.Method
          | AttributeTargets.Class
          | AttributeTargets.Assembly)]
        public sealed class ExtensionAttribute : Attribute
        {

        }
    }


    namespace System.IO
    {
        internal static class Pathz
        {
            internal static bool HasIllegalCharacters(string path, bool checkAdditional)
            {              
                for (int index = 0; index < path.Length; ++index)
                {
                    int num = (int)path[index];
                    switch (num)
                    {
                        case 34:
                        case 60:
                        case 62:
                        case 124:
                            return true;
                        default:
                            if (num >= 32 && (!checkAdditional || num != 63 && num != 42))
                                continue;
                            else
                                goto case 34;
                    }
                }
                return false;
            }

            internal static void CheckInvalidPathChars(string path, bool checkAdditional /*= false*/)
            {
                if (path == null)
                    throw new ArgumentNullException("path");
                if (Pathz.HasIllegalCharacters(path, checkAdditional))
                    throw new ArgumentException("Argument_InvalidPathChars");
            }

            public static string Combine(string path1, string path2)
            {
                if (path1 == null || path2 == null)
                    throw new ArgumentNullException(path1 == null ? "path1" : "path2");
                Pathz.CheckInvalidPathChars(path1, false);
                Pathz.CheckInvalidPathChars(path2, false);
                return Pathz.CombineNoChecks(path1, path2);
            }

            private static string CombineNoChecks(string path1, string path2)
            {
                if (path2.Length == 0)
                    return path1;
                if (path1.Length == 0 || Path.IsPathRooted(path2))
                    return path2;
                char ch = path1[path1.Length - 1];
                if ((int)ch != (int)Path.DirectorySeparatorChar && (int)ch != (int)Path.AltDirectorySeparatorChar && (int)ch != (int)Path.VolumeSeparatorChar)
                    return path1 + (object)Path.DirectorySeparatorChar + path2;
                else
                    return path1 + path2;
            }

            public static char[] GetInvalidPathChars()
            {
                return (char[])Pathz.RealInvalidPathChars.Clone();
            }

            private static char[] RealInvalidPathChars = new char[36]
          {
                '"',
                '<',
                '>',
                '|',
                char.MinValue,
                '\x0001',
                '\x0002',
                '\x0003',
                '\x0004',
                '\x0005',
                '\x0006',
                '\a',
                '\b',
                '\t',
                '\n',
                '\v',
                '\f',
                '\r',
                '\x000E',
                '\x000F',
                '\x0010',
                '\x0011',
                '\x0012',
                '\x0013',
                '\x0014',
                '\x0015',
                '\x0016',
                '\x0017',
                '\x0018',
                '\x0019',
                '\x001A',
                '\x001B',
                '\x001C',
                '\x001D',
                '\x001E',
                '\x001F'
          };
        }

    }



namespace System
{
//    public static class stringNET20
//    {
//#if !WindowsCE
//        public static bool IsNullOrWhiteSpace(this string str)
//#else
//      public static bool IsNullOrWhiteSpace(this string str)
//#endif
//        {
//            return string.IsNullOrEmpty(str?.Trim());
//        }
//    }
}


namespace System.Linq
{

    public static class NET20
    {


#if !WindowsCE
        public static IEnumerable<Reflection.PropertyInfo> FilterProperties(this IEnumerable<Reflection.PropertyInfo> Array)
#else
       public static IEnumerable<Reflection.PropertyInfo> FilterProperties(IEnumerable<Reflection.PropertyInfo> Array)
#endif
        {
            foreach (var item in Array)
            {
                if (!item.PropertyType.Equals(typeof(BindingManagerBase)) &&
                  !item.PropertyType.Equals(typeof(AccessibleRole)) &&
                  !item.PropertyType.Equals(typeof(ControlBindingsCollection)) &&
                  !item.PropertyType.Equals(typeof(Control)) &&
                   !item.PropertyType.Equals(typeof(Reflection.MethodBase)))
                    yield return item;
            }
        }

      



#if !WindowsCE
        public static IEnumerable<Reflection.FieldInfo> FilterFields(this IEnumerable<Reflection.FieldInfo> Array)
#else
       public static IEnumerable<Reflection.FieldInfo> FilterFields(IEnumerable<Reflection.FieldInfo> Array)
#endif
        {
            foreach (var item in Array)
            {
                if(((item.Attributes & Reflection.FieldAttributes.InitOnly) != Reflection.FieldAttributes.InitOnly) &&
           ((item.Attributes & Reflection.FieldAttributes.Literal) != Reflection.FieldAttributes.Literal) &&
           item.DeclaringType.IsGenericType == false)
                    yield return item;
            }       
        }

#if !WindowsCE
        public static bool IsIEnumerable(this IEnumerable<Type> Array)
#else
       public static bool IsIEnumerable(IEnumerable<Type> Array)
#endif
        {
            foreach (Type item in Array)
            {
                if( item.IsGenericType && item.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    return true;
            }

            return false;
        }

#if !WindowsCE
        public static bool Any<T>(this IEnumerable<T> Array, object Obj)
#else
       public static bool Any<T>(IEnumerable<T> Array)
#endif
        {
            foreach (T item in Array)
            {
                if(object.ReferenceEquals(item, Obj))
                    return true;
            }
            return false;
        }


#if !WindowsCE
        public static bool Any<T>(this IEnumerable<T> Array)
#else
       public static bool Any<T>(IEnumerable<T> Array)
#endif
        {

            foreach (T item in Array)
            {
                return true;
            }

            return false;
        }

       
        public delegate TResult Func<in T1, in T2, out TResult>(T1 arg1, T2 arg2);

#if !WindowsCE
        public static bool Aggregate<T>(this IEnumerable<T> Array, Func<T, T, T> func)
#else
       public static bool Any<T>(IEnumerable<T> Array)
#endif
        {

            foreach (T item in Array)
            {
                return true;
            }

            return false;
        }


#if !WindowsCE
        public static T FirstOrDefault<T>(this IEnumerable<T> Array)
#else
         public static T FirstOrDefault<T>(IEnumerable<T> Array )
#endif
        {
            if (Array != null)
                foreach (T item in Array)
                {
                    return item;
                }
            return default(T);
        }


#if !WindowsCE
        public static T[] ToArray<T>(this IEnumerable<T> Array)
#else
        public static T[] ToArray<T>(IEnumerable<T> Array)
#endif
        {
            List<T> List = new List<T>(Array);
            T[] destinationArray = new T[List.Count];
            List.CopyTo(destinationArray);
            return destinationArray;
        }


#if !WindowsCE
        public static int Count<T>(this IEnumerable<T> Array)
#else
                 public static int Count<T>(IEnumerable<T> Array)
#endif
        {
            int i = 0;
            foreach (T item in Array)
            {
                i++;
            }

            return i;
        }

#if !WindowsCE
        public static T[] ToArray<T>(this CatchException.HashSet<T> Array)
#else
        public static bool ToArray<T>(CatchException.HashSet<T> Array)
#endif
        {
            List<T> List = new List<T>(Array);
            T[] destinationArray = new T[List.Count];
            List.CopyTo(destinationArray);
            return destinationArray;
        }

       
        /// <summary>
        /// <para>
        /// <param name="source">IList TItem </param>
        /// <param name="SeachItem">TItem</param>
        /// </para>
        /// Replace the linq function Contains
        /// <example> Contains(Array, 'C') = Array.Contains('C')  </example>
        /// <returns>bool</returns>
        /// </summary>
#if !WindowsCE
        public static bool Contains<TItem>(this TItem[] source, TItem SeachItem)
#else
            public static bool Contains<TItem>( IList<TItem> source, TItem SeachItem)
#endif
        {
            //bool Isfound = false;
            foreach (TItem Item in source)
            {
                if (Item.Equals(SeachItem))
                    return true;
                //{
                //    Isfound = true;
                //    break;
                //}

            }
            return false; // Isfound;
        }

        public delegate TResult Func<in T, out TResult>(T arg);

#if !WindowsCE
        public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
#else
         public static IEnumerable<TSource> Where<TSource>( IEnumerable<TSource> source, Func<TSource, bool> predicate)
#endif
        {
          //  List<T> ret = new List<T>();

            foreach (TSource item in source)
            {
                if(predicate(item))
                {
                   yield return item;
                }
            }
           // return ret;
        }


#if !WindowsCE
        public static List<T> Where<T>(this T[] Array, Type IfType)
#else
        public static List<T> Where<T>(CatchException.HashSet<T> Array, string IfMethod )
#endif
        {
            List<T> ret = new List<T>();

            foreach (T item in Array)
            {
                if (item.GetType() == IfType)
                {
                    ret.Add(item);
                }
            }
            return ret;
        }

    }

}