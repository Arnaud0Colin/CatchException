using System;
using System.Collections.Generic;
#if NET35
using System.Linq;
#endif
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace CatchException
{

    public static class DumpObjectExt
    {
        internal static bool IsSimpleType(this Type type)
        {
            return (type.IsPrimitive || type.Equals(typeof(string)) || type.Equals(typeof(TimeSpan)) || type.Equals(typeof(DateTime)) ); //
        }

        internal static string GetNullableArrayTypeName(this Type type)
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


        internal static string GetTypeName(this Type type)
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
                h += string.Join(",", type.GenericTypeArguments.Select(p => GetTypeName(p)));
                h += '>';
            }
#endif
            return h;
        }

    }


    public  class DumpObject
    {

        static bool IsSimpleType(Type type)
        {
            return DumpObjectExt.IsSimpleType(type);
        }


       int limite = 50;
       HashSet<object> listhash = null; 

       private string Write(string value, int Level)
       {
           return new string('\t', Level) + value;
       }

       public static bool Any(HashSet<object> source, object Obj)
        {
            bool Isfound = false;
            foreach (object x in source)
            {
                if (object.ReferenceEquals(x, Obj))
                {
                    Isfound = true;
                    break;
                }

            }
            return Isfound;
        }


       public  string Dump(object Obj, int Level)
       {
           if (Obj == null)
              return Write("Null", Level);

           var type = Obj.GetType();
           string typename = type.GetTypeName();


           string DirectValue = Obj.ToString();
           int pos = type.FullName.IndexOf('`');
           bool IsComplexe = (pos > 0) && DirectValue.Contains(type.FullName.Substring(0, pos));
               

           if ( IsSimpleType(type))
           {
               return Obj.ToString();
           }

            if (type.Name.Equals("RuntimeType"))
                return string.Empty;

           if (listhash == null)
                listhash = new HashSet<object>();

#if NET35
            if (listhash.Any(x=> object.ReferenceEquals( x, Obj)))
#else
            if (Any(listhash, Obj))
#endif
                return Write("<...>", Level);
          
            listhash.Add(Obj);


            if (Level >= limite)
                return Write("...", Level);

            int nbElement = 0;

            string ret = string.Empty;
            ret += "\r\n";

            if (type.IsArray)
            {
                ret += DumpArray(Obj, Level);
                nbElement ++;
            }
            //   else if ( Obj is System.Collections.IEnumerable)
            else if (type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
            {
                ret += DumpIEnumerable(Obj, Level);
                nbElement++;
            }


           IEnumerable<FieldInfo> Fields = type.GetFields().Where(x => 
                ((x.Attributes & FieldAttributes.InitOnly) != FieldAttributes.InitOnly) && 
                ((x.Attributes & FieldAttributes.Literal) != FieldAttributes.Literal) &&

                x.DeclaringType.IsGenericType == false
                );

            if (Fields != null && Fields.Any())
            {
                ret += Dumpclass(Fields, Obj, Level);
                nbElement += Fields.Count();
            }


           MemberInfo[] Members = type.GetMembers();
            //  ret += Dump(Members, Obj, Level);

            var Properties = type.GetProperties().Where(x =>
                       (x.GetGetMethod().Attributes & MethodAttributes.Static) != MethodAttributes.Static &&
                         !x.PropertyType.Equals(typeof(BindingManagerBase)) &&
                         !x.PropertyType.Equals(typeof(AccessibleRole)) &&
                         !x.PropertyType.Equals(typeof(ControlBindingsCollection)) &&
                         !x.PropertyType.Equals(typeof(Control)) &&
                          !x.PropertyType.Equals(typeof(MethodBase)) &&
                          x.DeclaringType.IsGenericType == false 
                        );

            if (Properties != null && Properties.Any())
            {
                ret += Dumpclass(Properties, Obj, Level);
                nbElement += Properties.Count();
            }


#if !NET35
           if (nbElement == 0)               
#else
            if (nbElement == 0)
#endif
               return Write("Empty", Level);

           return ret;
       }



        string DumpIEnumerable(object Obj, int level)
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
                    count = CountProperty.GetValue(Obj) as int?;

                //MethodInfo CountMethod = Obj.GetType().GetMethod("GetLength");
                //if (CountMethod != null)
                //    count = CountMethod.Invoke(Obj, null ) as int?;

                var ienum = Obj.GetType().GetInterface(typeof(IEnumerable<>).Name);
                typeEnum = ienum != null ? ienum.GetGenericArguments()[0].Name : null;



                System.Collections.IEnumerable ienu = Obj as System.Collections.IEnumerable;

                if(ienu == null)
                {
                    Value = "Null";
                }
                else if (count > 50)
                {
                    Value += $"<....>";
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
                            Value += $"<....>";
                            break;
                        }

                        Value += Dump(f, level+1);
                        i++;
                    }
                    Value += "}";
                }
            }

            ret += Write(string.Format("[{0}] {1} : {2} \r\n", string.Format("IEnumerable<{0}>", typeEnum) , "", Value), level);

            return ret;
        }

        string Dumpclass<T>(IEnumerable<T> elements, object Obj, int Level) where T : MemberInfo
        {
            string ret = string.Empty;
            string Value = null;

            if (elements != null && elements.Count() > 0)
            {
                foreach (var element in elements)
                {
                    object Objval = null;

                    if (element is FieldInfo)
                        Objval = (element as FieldInfo).GetValue(Obj);
                    else if (element is PropertyInfo)
                        Objval = (element as PropertyInfo).GetValue(Obj);
                    else if (element is MemberInfo)
                    {
                      //  (element as MemberInfo).in
                    }

                    if (Objval == null)
                    {
                        Value = "Null";
                    }
                    else
                    {
                        Type type = Objval.GetType();

                        if (type == Obj.GetType())
                            continue;

                        if (IsSimpleType(type))
                        {
                            Value = Objval.ToString();
                        }
                        else if (type.IsArray)
                        {
                            Value = DumpArray(Objval, Level + 1);
                        }
                        else
                        {
                            Value = Dump(Objval, Level + 1);
                        }
                    }

                    ret += Write(string.Format("[{0}] {1} : {2} \r\n", element.DeclaringType.GetTypeName(), element.Name, Value), Level);
                }
            }

           return ret;
        }


//            string Dump(IEnumerable<FieldInfo> Fields, object Obj, int Level)
//       {
//           string ret = string.Empty;

//#if !NET35
//           if (Fields != null && Fields.Length > 0)
//#else
//            if (Fields != null &&  Fields.Count() > 0)
//#endif
//           {
//                foreach (var field in Fields)
//                {
//                    var Objval = field.GetValue(Obj);

//                    Type type = Objval.GetType();

//                   if (type == Obj.GetType())
//                       continue;

//                   string Value = null;

//                   if (Objval == null)
//                    {
//                        Value = "Null";
//                    }
//                    else if (IsSimpleType(type))
//                    {
//                        Value = Objval.ToString();
//                    }
//                    else if (type.IsArray)
//                    {
//                        Value = DumpArray(Objval, Level + 1);
//                    }
//                    else
//                    {
//                        Value = Dump(Objval, Level + 1);
//                    }

//                    ret += Write(string.Format("[{0}] {1} : {2} \r\n", field.FieldType.GetTypeName(), field.Name, Value), Level);
//                }
//            }
//           return ret;
//       }

//       /*
//      static string Dump(MemberInfo[] Members, object Obj, int Level)
//        {
//            string ret = string.Empty;
//            if (Members.Count() > 0)
//            {
//                foreach (var Member in Members)
//                {
//                    object val = null; // Member.GetValue(Obj);

//                    if (val == null)
//                    {
//                        string ligne = string.Format("[{0}] {1} : {2}", CatchMe.GetTypeName(Member.DeclaringType), Member.Name, "Null");
//                        ret += ligne;
//                        continue;
//                    }
//                    if (Member.MemberType.GetType().IsPrimitive || Member.MemberType.Equals(typeof(string)))
//                    {
//                        string ligne = string.Format("[{0}] {1} : {2}", CatchMe.GetTypeName(Member.DeclaringType), Member.Name, val.ToString());
//                        ret += ligne;
//                    }
//                    else
//                    {

//                        string ligne = string.Format("[{0}] {1} : {2}", CatchMe.GetTypeName(Member.DeclaringType), Member.Name, Dump(val, Level + 1));
//                        ret += ligne;
//                    }
//                }
//            }
//            return ret;
//        }
//       */
//       string Dump(IEnumerable<PropertyInfo> Properties, object Obj, int Level)
//      {
//          string ret = string.Empty;
//#if !NET35
//          if (Properties != null && Properties.Length > 0)
//#else
//            if (Properties != null && Properties.Count() > 0)
//#endif
//          {
//              foreach (var Property in Properties)
//              {

//                  string Value = null;
//#if NET35r
//                  if (!Property.GetMethod.Attributes.HasFlag(MethodAttributes.Static))
//#else
               
//                 if (!((Property.GetGetMethod().Attributes & MethodAttributes.Static) == MethodAttributes.Static))
//#endif
//                  {
//                      if ( Property.PropertyType.Equals(typeof(BindingManagerBase))
//                        //  || Property.PropertyType.Equals(typeof(System.Windows.Forms.Control.ControlCollection)) 
//                          || Property.PropertyType.Equals(typeof(AccessibleRole))
//                           || Property.PropertyType.Equals(typeof(ControlBindingsCollection)) 
//                           || Property.PropertyType.Equals(typeof(Control))
//                           || Property.PropertyType.Equals(typeof(MethodBase))
//                           )
//                          continue;

                      

//                      if (Property.PropertyType.IsArray)
//                      {
//                          object ObjVal = Property.GetValue(Obj, null);
//                          Value = DumpArray(ObjVal, Level + 1);                  
//                      } else
//                          if (Property.Name == "Item")
//                          {

//                              PropertyInfo count = null;
//#if NET35
//                              count = Obj.GetType()
//                                   .GetProperties()
//                                   .SingleOrDefault(prop => prop.Name == "Count");
//#else
//                          foreach(var prop in Obj.GetType().GetProperties())
//                          {
//                              if (prop.Name == "Count")
//                              {
//                                  count = prop;
//                                  break;
//                              }
//                          }
//#endif

//                              string small = string.Empty;
//                              small += "{";

//                              if (count != null)
//                              {
//                                  for (int i = 0; i < (int)(count.GetValue(Obj, null)); i++)
//                                  {
//                                      object ObjVal = Property.GetValue(Obj, new object[] { i });
//                                      if (i > 0)
//                                          small += ",";
//                                            if (IsSimpleType(Property.PropertyType))
//                                           {
//                                               small += ObjVal.ToString();
//                                           }
//                                           else
//                                           {
//                                               small += Dump(ObjVal, Level + 1);
//                                           }
//                                  }
//                              }
//                              small += "}";
//                              Value = small;
//                          }
//                          else
//                          {
//                              object ObjVal = Property.GetValue(Obj, null);
                             
//                              if (ObjVal == null)
//                              {
//                                  Value = "Null";
//                              }
//                              else
//                                   if (IsSimpleType(Property.PropertyType))
//                                  {
//                                      Value = ObjVal.ToString();
//                                  }
//                                  else
//                                  {
//                                      Value = Dump(ObjVal, Level + 1);
//                                  }
//                          }

//                      ret += Write(string.Format("[{0}] {1} : {2} \r\n", Property.PropertyType.GetTypeName(), Property.Name, Value), Level);
//                  }
//              }
//          }
//          return ret;
//      }


       string DumpArray(object Obj, int level)
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
                if(length > 50)
                {
                    ret += $"<....>";
                }
                else
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

                        ret += Dump(val, level);

                        if (dim > 0 && dim == (dimension - 1))
                        {
                            ret += "}";
                        }

                    }

                }
                ret += "}";

            }
            else
                throw new Exception();

           return ret;
       }

    }
}
