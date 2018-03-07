using LogTrace.CatchException;
using System;
using System.Collections.Generic;
#if NET35
using System.Linq;
#endif
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace LogTrace.CatchException
{
  
    public  class DumpObject 
    {
       int limite = 50;
#if NET20
       List<object> listhash = null; 
#else
       HashSet<object> listhash = null; 
#endif
       private string Write(string value, int Level)
       {
           return new string('\t', Level) + value;
       }

       public static bool Any(List<object> source, object Obj)
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
           if (type.IsPrimitive || type.Equals(typeof(string)))
           {
               return Obj.ToString();
           }

           if (type.IsArray)
           {
               return DumpArray(Obj, Level );
           }

           if (type.Name.Equals("RuntimeType"))
               return string.Empty;

           
#if NET20
           if (listhash == null)
               listhash = new List<object>();

          if (Any(listhash, Obj))
              return Write("<...>", Level);
#else
           if (listhash == null)
                listhash = new HashSet<object>();

            if (listhash.Any(x=> object.ReferenceEquals( x, Obj)))
               return Write("<...>", Level);
#endif
          //  listhash = new HashSet<object>();

         /*  if (listhash.Any(x=> object.ReferenceEquals( x, Obj)))
               return Write("<...>", Level);*/
          
            listhash.Add(Obj);

           if (Level >= limite)
               return Write("...", Level);
            

           string typename = LogTrace.CatchException.CatchMe.GetTypeName(type);
           string ret = string.Empty;
           ret += "\r\n"; 

           FieldInfo[] Fields = type.GetFields();
           ret += Dump(Fields, Obj, Level);

         /*  MemberInfo[] Members = type.GetMembers();
           ret += Dump(Members, Obj, Level);*/

           PropertyInfo[] Properties = type.GetProperties();
           ret += Dump(Properties, Obj, Level);

#if NET20
           if ((Fields.Length + Properties.Length) == 0)               
#else
           if ((Fields.Count() + Properties.Count()) == 0)
#endif
               return Write("Empty", Level);

           return ret;
       }

     


       string Dump(FieldInfo[] Fields, object Obj, int Level)
       {
           string ret = string.Empty;
           
#if NET20
           if (Fields.Length > 0)
#else
           if (Fields.Count() > 0)
#endif
           {
                foreach (var field in Fields)
                {
                    var Objval = field.GetValue(Obj);

                    Type type = Objval.GetType();

                   if (type == Obj.GetType())
                       continue;


                   string Value = null;

                   if (Objval == null)
                    {
                        Value = "Null";
                    } else
                    if (type.IsPrimitive || type.Equals(typeof(string)))
                    {
                        Value = Objval.ToString();
                    } else
                    if (type.IsArray)
                    {
                        Value = DumpArray(Objval, Level + 1);
                    }
                    else
                    {
                        Value = Dump(Objval, Level + 1);
                    }

                    ret += Write(string.Format("[{0}] {1} : {2} \r\n", CatchMe.GetTypeName(field.FieldType), field.Name, Value), Level);
                }
            }
           return ret;
       }

       /*
      static string Dump(MemberInfo[] Members, object Obj, int Level)
        {
            string ret = string.Empty;
            if (Members.Count() > 0)
            {
                foreach (var Member in Members)
                {
                    object val = null; // Member.GetValue(Obj);

                    if (val == null)
                    {
                        string ligne = string.Format("[{0}] {1} : {2}", CatchMe.GetTypeName(Member.DeclaringType), Member.Name, "Null");
                        ret += ligne;
                        continue;
                    }
                    if (Member.MemberType.GetType().IsPrimitive || Member.MemberType.Equals(typeof(string)))
                    {
                        string ligne = string.Format("[{0}] {1} : {2}", CatchMe.GetTypeName(Member.DeclaringType), Member.Name, val.ToString());
                        ret += ligne;
                    }
                    else
                    {

                        string ligne = string.Format("[{0}] {1} : {2}", CatchMe.GetTypeName(Member.DeclaringType), Member.Name, Dump(val, Level + 1));
                        ret += ligne;
                    }
                }
            }
            return ret;
        }
       */
       string Dump(PropertyInfo[] Properties, object Obj, int Level)
      {
          string ret = string.Empty;
#if NET20
          if (Properties.Length > 0)
#else
               if (Properties.Count() > 0)
#endif
          {
              foreach (var Property in Properties)
              {

                  string Value = null;
#if NET35r
                  if (!Property.GetMethod.Attributes.HasFlag(MethodAttributes.Static))
#else
               
                 if (!((Property.GetGetMethod().Attributes & MethodAttributes.Static) == MethodAttributes.Static))
#endif
                  {
                      if ( Property.PropertyType.Equals(typeof(BindingManagerBase))
                        //  || Property.PropertyType.Equals(typeof(System.Windows.Forms.Control.ControlCollection)) 
                          || Property.PropertyType.Equals(typeof(AccessibleRole))
                           || Property.PropertyType.Equals(typeof(ControlBindingsCollection)) 
                           || Property.PropertyType.Equals(typeof(Control))
                           || Property.PropertyType.Equals(typeof(MethodBase))
                           )
                          continue;

                      

                      if (Property.PropertyType.IsArray)
                      {
                          object ObjVal = Property.GetValue(Obj, null);
                          Value = DumpArray(ObjVal, Level + 1);                  
                      } else
                          if (Property.Name == "Item")
                          {

                              PropertyInfo count = null;
#if NET35
                              count = Obj.GetType()
                                   .GetProperties()
                                   .SingleOrDefault(prop => prop.Name == "Count");
#else
                          foreach(var prop in Obj.GetType().GetProperties())
                          {
                              if (prop.Name == "Count")
                              {
                                  count = prop;
                                  break;
                              }
                          }
#endif

                              string small = string.Empty;
                              small += "{";

                              if (count != null)
                              {
                                  for (int i = 0; i < (int)(count.GetValue(Obj, null)); i++)
                                  {
                                      object ObjVal = Property.GetValue(Obj, new object[] { i });
                                      if (i > 0)
                                          small += ",";

                                          if (Property.PropertyType.IsPrimitive || Property.PropertyType.Equals(typeof(string)))
                                           {
                                               small += ObjVal.ToString();
                                           }
                                           else
                                           {
                                               small += Dump(ObjVal, Level + 1);
                                           }
                                  }
                              }
                              small += "}";
                              Value = small;
                          }
                          else
                          {
                              object ObjVal = Property.GetValue(Obj, null);
                             
                              if (ObjVal == null)
                              {
                                  Value = "Null";
                              }
                              else
                                  if (Property.PropertyType.IsPrimitive || Property.PropertyType.Equals(typeof(string)))
                                  {
                                      Value = ObjVal.ToString();
                                  }
                                  else
                                  {
                                      Value = Dump(ObjVal, Level + 1);
                                  }
                          }

                      ret += Write(string.Format("[{0}] {1} : {2} \r\n", CatchMe.GetTypeName(Property.PropertyType), Property.Name, Value), Level);
                  }
              }
          }
          return ret;
      }


       string DumpArray(object Obj, int level)
       {
           string ret = string.Empty;
           if (Obj.GetType().IsArray)
           {
               var dimension = Obj.GetType().GetArrayRank();
               var length = ((Array)Obj).Length;

               ret += "{";
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

                       if( dimension == 1)
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
