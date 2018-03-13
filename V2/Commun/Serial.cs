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
using CatchException;
using System;
using System.Collections.Generic;
#if NET35
using System.Linq;
#endif
using System.Reflection;
using System.Text;


namespace FransBonhomme.StringFormat
{
    public class Serial
    {

       static List<PropertyInfo> find(Type type)
       {
            List<PropertyInfo> ret = new List<PropertyInfo>();

           foreach ( var pro in  type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if(pro.GetIndexParameters().Length == 0
                       && pro.CanRead)
                {
                    ret.Add(pro);
                }
            }
            return ret;
        }

        private static PropertyInfo[] GetArray(IList<PropertyInfo> iList) 
        {
            var result = new PropertyInfo[iList.Count];

            iList.CopyTo(result, 0);

            return result;
        }


    public static void Get<U>(U obj, HtmlElement level)
        {
            Type type = typeof(U);

            if (obj == null)
            {
                level.Child(new HtmlElement("i").Text("NULL"));
                return;
            }


            if (!type.IsValueType)
            {
                var t = new HtmlElement("td").Text(type.Name);
                level.Child(t);
                level = t;
            }

            bool isString = obj is string;
            string typeName = obj.GetType().FullName;
            string formattedValue = obj.ToString();

            if (formattedValue == typeName)
                formattedValue = string.Empty;
            else
            {
                level.Child(new HtmlElement("i").Text(formattedValue));
                return;
            }

#if NET35
            PropertyInfo[] properties =
                (from property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                 where property.GetIndexParameters().Length == 0
                       && property.CanRead
                 select property).ToArray();
            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToArray();

#else
            PropertyInfo[] properties = GetArray(find(type));
            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
#endif

            if (properties.Length == 0 && fields.Length == 0)
                return ;

               HtmlElement level2 = new HtmlElement("ul");
            if (properties.Length > 0)
            {
                level.Child(level2);
                foreach (PropertyInfo pi in properties)
                {
                   
                        object propertyValue = pi.GetValue(obj, null);
                        Get(propertyValue, level2);
                   
                }
            }
            HtmlElement level3 = new HtmlElement("ul");
            if (fields.Length > 0)
            {
                level.Child(level3);
                foreach (FieldInfo  field  in fields)
                {

                    object propertyValue = field.GetValue(obj);
                    Get(propertyValue, level3);

                }
            }
            return ;
        }
    }
}
