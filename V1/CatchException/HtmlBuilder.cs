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
using System.Text;


namespace LogTrace
{


    public class HtmlAttribute
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public HtmlAttribute(string name) : this(name, null) { }

        public HtmlAttribute(
            string name,
            string @value)
        {
            this.Name = name;
            this.Value = @value;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(this.Value))
                return this.Name;

#if NET35
            if (this.Value.Contains("\'"))
                return string.Format( "{0}=\"{1}\"", this.Name, this.Value);

#else
            foreach (char c in this.Value)
            {
                if (c.Equals('\''))
                    return string.Format( "{0}=\"{1}\"", this.Name, this.Value);
            }             
#endif

            return string.Format("{0}='{1}'", this.Name, this.Value);

        }
    }

    public interface IHtmlElement
    {
        string Text { get; }
        string ToString();
    }


    public class HtmlLink : IHtmlElement
   {
        

       public string Text
       {
           get { throw new NotImplementedException(); }
       }

       public override string ToString()
       {
           return null;
       }

   }
   


    public class HtmlElement : IHtmlElement
    {
        public string Text { get; set; }

        protected HtmlAttribute[] Attributes { get; set; }

        public HtmlElement(params HtmlAttribute[] Attributes)
        {
            this.Attributes = Attributes;
        }

       public override string ToString()
        {
            string res = string.Empty;
            foreach (HtmlAttribute att in Attributes)
            {
                if(att!= null)
                    res += att.ToString() + " ";
            }
            return res;
        }

    }

   

    public class HtmlBuilder
    {
       private StringBuilder s = new StringBuilder();

       public void ListLigne( string[] test)
       {
           s.Append("<ul> \n");           
           foreach (string c in test)           
           {
               s.Append("<li> \n");
               s.Append(c);
               s.Append("</li> \n");
           }          
           s.Append("</ul> \n");
       }


       public void TableLigneTitre(params IHtmlElement[] test)
       {
           s.Append("<tr> \n");
           foreach (IHtmlElement c in test)
               s.Append("<th " + c.ToString() + ">" + c.Text + "</th> \n");

           s.Append("</tr> \n");
       }

       public void TableLigne(params IHtmlElement[] test)
       {

           s.Append("<tr> \n");

           foreach (IHtmlElement c in test)
           {
               s.Append("<td " + c.ToString() + ">" + c.Text + "</td>");
           }
           s.Append("</tr> \n");

       }

        public void Append<T>(T t)
        {
            s.Append(t);
        }

        public override string ToString()
        {
            return s.ToString();
        }
    }
}
