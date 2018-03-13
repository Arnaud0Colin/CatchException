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
#if NET35
using System.Linq;
#endif
using System.Text;


namespace CatchException
{


  


    public partial class CatchMe
    {


        static WebCatchException.ElVar Affect<U>(string Name, U instance)
        {
            //  var hhh = Dump.GetValue(instance);

            return new WebCatchException.ElVar() { Name = Name, Type = typeof(U).GetTypeName(), Value = Zip.GetBase64(Zip.Compress(Dump.GetValue(instance))) };
        }
    

#if NET46 && !WindowsCE

        static string GetName<T>(T item) where T : class
        {
            var properties = typeof(T).GetProperties();
           // Enforce.That(properties.Length == 1);
            return properties[0].Name;
        }

        public CatchMe Variable<T>(System.Linq.Expressions.Expression<Func<T>> expr)
        {
          var body = ((System.Linq.Expressions.MemberExpression)expr.Body);

                    string nom = body.Member.Name;

            if (_Var == null)
                _Var = new HashSet<WebCatchException.ElVar>();

            while (_Var.Any(p => p.Name == nom))
            {
                nom += '-';
            }

            _Var.Add(Affect(nom, ((System.Reflection.FieldInfo)body.Member).GetValue(((System.Linq.Expressions.ConstantExpression)body.Expression).Value)));

          return this;

        }


        //public CatchMe Variable<U>(U instance)
        //    {
        //        string nom = nameof(instance);

        //        //nom = instance.GetType().GetProperty("Name", typeof(string)).GetValue(instance, null).ToString();



        //        if (_Var == null)
        //            _Var = new HashSet<WebCatchException.ElVar>();

        //        while (_Var.Any(p => p.Name == nom))
        //        {
        //            nom += '-';
        //        }

        //        _Var.Add(Affect(nom, instance));
        //        return this;
        //    }  

#endif

#if !NET35
        public bool Find(string nom)
        {
            foreach (var tmp in _Var)
            {
                if (tmp.Name == nom)
                    return true;
            }
            return false;
        }
#endif

        public CatchMe Variable(string nom, object instance)
        {
            if (_Var == null)
                _Var = new HashSet<WebCatchException.ElVar>();

#if NET35
            while (_Var.Any(p => p.Name == nom))
            {
                nom += '-';
            }
#else
            while (Find( nom))
            {
                nom += '-';
            }
#endif
            _Var.Add(Affect(nom, instance));

            return this;
        }

        public CatchMe Variable<U>(string nom, U instance)
        {

            if (_Var == null)
                _Var = new HashSet<WebCatchException.ElVar>();

#if NET35
            while (_Var.Any(p => p.Name == nom))
            {
                nom += '-';
            }
#else
            while (Find( nom))
            {
                nom += '-';
            }
#endif
            _Var.Add(Affect(nom, instance));

            return this;
        }


        public static string VariableType<U>(U instance)
        {
            return typeof(U).GetTypeName();

        }

    

    }
}
