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
using System.Runtime.CompilerServices;
using System.Text;
using System.IO;

namespace Compatibility
{

    internal class MappingFieldAttribute : Attribute
    {
        internal MappingFieldAttribute(string fieldName)
        {
            this.fieldName = fieldName;
        }
        private string fieldName;
        internal string FieldName
        {
            get
            {
                return fieldName;
            }
        }
    }

    public static class PATH
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
      if (PATH.HasIllegalCharacters(path, checkAdditional))
        throw new ArgumentException("Argument_InvalidPathChars");
    }

        public static string Combine(string path1, string path2)
        {
            if (path1 == null || path2 == null)
                throw new ArgumentNullException(path1 == null ? "path1" : "path2");
            PATH.CheckInvalidPathChars(path1, false);
            PATH.CheckInvalidPathChars(path2, false);
            return PATH.CombineNoChecks(path1, path2);
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
            return (char[])PATH.RealInvalidPathChars.Clone();
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



    /// <summary>
    /// Class de Compatibility with Net20
    /// <example>  </example>/// 
    /// </summary>
    public static class NET20
    {


        /// <summary>
        /// <para>
        /// <param name="source">IList TItem </param>
        /// <param name="SeachItem">TItem</param>
        /// </para>
        /// Replace the linq function Contains
        /// <example> Contains(Array, 'C') = Array.Contains('C')  </example>
        /// <returns>bool</returns>
        /// </summary>
#if NET35
        public static bool Contains(this string source, char SeachItem)
#else
        public static bool Contains(string source, char SeachItem)
#endif
        {
            bool Isfound = false;
            foreach (char Item in source)
            {
                if (Item.Equals(SeachItem))
                {
                    Isfound = true;
                    break;
                }

            }
            return Isfound;
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
#if NET35
        public static bool Contains<TItem>(this IList<TItem> source, TItem SeachItem)
#else
        public static bool Contains<TItem>( IList<TItem> source, TItem SeachItem)
#endif
        {
            bool Isfound = false;
            foreach (TItem Item in source)
            {
                if (Item.Equals(SeachItem))
                {
                    Isfound = true;
                    break;
                }

            }
            return Isfound;
        }


        private delegate void SetValue<T>(T value);
        private delegate T GetValue<T>();

        /// <summary>
        /// <para>
        /// <param name="array">IList</param>
        /// <param name="ValueMethod">string</param>
        /// <param name="split">char?</param>
        /// </para>
        /// Replace the linq function Aggregate and Select
        /// <example> Aggregatechar(Array, "Filtre", '.') = Array.Select(i => i.Filtre).Aggregate((i, j) => i + "." + j); </example>/// 
        /// <returns>string</returns>
        /// </summary>
#if NET35
    #if !WindowsCE
            public static string Aggregate<TItem>(this IList<TItem> array, string ValueMethod  = null, char? split = null)
#else
            public static string Aggregate<TItem>(this IList<TItem> array, string ValueMethod, char? split )
#endif
#else
#if !WindowsCE
        public static string Aggregate<TItem>(IList<TItem> array, string ValueMethod = null, char? split = null)
    #else
                public static string Aggregate<TItem>(IList<TItem> array, string ValueMethod, char? split )
#endif
#endif
        {
            System.Reflection.PropertyInfo propertyValue = null;
            System.Reflection.MethodInfo methodValue = null;

            Type type = typeof(TItem);
            if (ValueMethod != null)
                propertyValue = type.GetProperty(ValueMethod);
                if(propertyValue != null)
                    methodValue = propertyValue.GetGetMethod();
        
            string Result = string.Empty;
            foreach (TItem item in array)
            {
                if (split.HasValue && Result.Length > 0)
                    Result += split.Value;

                if (methodValue != null)
                {
                    GetValue<string> getValue = (GetValue<string>)Delegate.CreateDelegate(typeof(GetValue<string>), item, methodValue);
                    Result += getValue();
                }
                else
                    Result += item.ToString();
            }
            return Result;
        }


#if NET35
       /// <summary>
        /// <para>
        /// <param name="array">IList</param>
            /// <param name="predicate">Func</param>
            /// <param name="condition">Func</param>
            /// <param name="split">char</param>
        /// </para>
        /// Replace the linq function Aggregate and Select
        /// <example> Aggregatechar(Array, "Filtre", '.') = Array.Select(i => i.Filtre).Aggregate((i, j) => i + "." + j); </example>/// 
        /// <returns>string</returns>
        /// </summary>
#if !WindowsCE
            public static string Aggregate<TItem>(this IList<TItem> array, Func<TItem, string> predicate, Func<TItem, bool> condition, char? split = null) 
#else
            public static string Aggregate<TItem>(this IList<TItem> array, Func<TItem, string> predicate, Func<TItem, bool> condition, char? split ) 
#endif
            {
            string result = default(string);
            foreach (TItem item in array)
            {
                if (condition != null)
                {
                    if (condition.Invoke(item))
                    {
                        if (split.HasValue && result.Length > 0)
                            result += split.Value;
                        result += predicate.Invoke(item);
                    }
                }
                else
                {
                    if (split.HasValue && result.Length > 0)
                        result += split.Value;

                    result += predicate.Invoke(item);
                }
            }
            return result;
        }
#endif


        /// <summary>
        /// <para>
        /// <param name="Array">List</param>
        /// <param name="IfMethod">string</param>
        /// <param name="ValueMethod">string</param>
        /// <param name="split">char?</param>
        /// </para>
        /// Replace the linq function Aggregate,Select and Where
        /// <example> AggregateWherechar(Array, "HasScript", "Script", ".")  =  Array.Where(i => i.HasScript).Select(i => i.Script).Aggregate((i, j) => i + " + " + j); </example>/// 
        /// <returns>string</returns>
        /// </summary>
#if !WindowsCE
        public static string AggregateWhere<T>(List<T> Array, string IfMethod, string ValueMethod, char? split = null)
#else
         public static string AggregateWhere<T>(List<T> Array, string IfMethod, string ValueMethod, char? split )
#endif
        {
            Type type = typeof(T);
            System.Reflection.PropertyInfo propertyValue = type.GetProperty(ValueMethod);
            System.Reflection.PropertyInfo propertyIf = type.GetProperty(IfMethod);
            System.Reflection.MethodInfo methodValue = propertyValue.GetGetMethod();
            System.Reflection.MethodInfo methodIf = propertyIf.GetGetMethod();
            string stmp = string.Empty;
            foreach (T item in Array)
            {
                

                GetValue<string> getValue = (GetValue<string>)Delegate.CreateDelegate(typeof(GetValue<string>), item, methodValue);
                GetValue<bool> getBoolean = (GetValue<bool>)Delegate.CreateDelegate(typeof(GetValue<bool>), item, methodIf);

                if (getBoolean())
                {
                    if (split.HasValue && stmp.Length > 0)
                        stmp += split.Value;

                    stmp += getValue();
                }
            }
            return stmp;
        }


        /*
         *  
        Func<TestClass, int> lambdaGet = x => x.Value;
        Action<TestClass, int> lambdaSet = (x, val) => x.Value = val;
         * */
  


       
#if !WindowsCE
        public static List<T> Where<T>(List<T> Array, string IfMethod)
#else
         public static List<T> Where<T>(List<T> Array, string IfMethod )
#endif
        {
            Type type = typeof(T);
       
            System.Reflection.PropertyInfo propertyIf = type.GetProperty(IfMethod);
       
            System.Reflection.MethodInfo methodIf = propertyIf.GetGetMethod();
            string stmp = string.Empty;

            List<T> ret = new List<T>();

            foreach (T item in Array)
            {

             
                GetValue<bool> getBoolean = (GetValue<bool>)Delegate.CreateDelegate(typeof(GetValue<bool>), item, methodIf);

                if (getBoolean())
                {
                    ret.Add(item);
                }
            }
            return ret;
        }


#if !WindowsCE
        public static bool Any<T>(List<T> Array, string IfMethod)
#else
         public static bool Any<T>(List<T> Array, string IfMethod )
#endif
        {
            Type type = typeof(T);

            System.Reflection.PropertyInfo propertyIf = type.GetProperty(IfMethod);

            System.Reflection.MethodInfo methodIf = propertyIf.GetGetMethod();
            string stmp = string.Empty;

            List<T> ret = new List<T>();

            foreach (T item in Array)
            {


                GetValue<bool> getBoolean = (GetValue<bool>)Delegate.CreateDelegate(typeof(GetValue<bool>), item, methodIf);

                if (getBoolean())
                {
                    return true; // ret.Add(item);
                }
            }
            //return ret;
            return false;
        }

        /*
         *  
        Func<TestClass, int> lambdaGet = x => x.Value;
        Action<TestClass, int> lambdaSet = (x, val) => x.Value = val;
         * */
    }

      }

