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
#if NET35
using System.Linq;
#endif

namespace CatchException
{


    public interface IHTMLProperty
    {
        string Name { get; }
        string Value { get; }
        string ToString();
    }


    public class HtmlStyle
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public HtmlStyle(string Name, string Value)
        {
            this.Name = Name;
            this.Value = Value;
        }

        public HtmlStyle(string Name, UInt32 Value)
        {
            this.Name = Name;
            this.Value = "#" + Value.ToString("x6");
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", this.Name, Value);
        }

    }


    public class HtmlStyles : IHTMLProperty
    {
//#if !WindowsCE && NET35
        private HashSet<HtmlStyle> _Childs = new HashSet<HtmlStyle>();
//#else
//        private List<HtmlStyle> _Childs = new List<HtmlStyle>();
//#endif
        public HtmlStyles(params HtmlStyle[] hs)
        {
            foreach (var h in hs)
                _Childs.Add(h);
        }

        public HtmlStyles(string Name, string Value)
        {
            _Childs.Add(new HtmlStyle(Name, Value));
        }

        public HtmlStyles(string Name, UInt32 Value)
        {
            _Childs.Add(new HtmlStyle(Name, Value));
        }

        public HtmlStyles Add(string Name, string Value)
        {
            _Childs.Add(new HtmlStyle(Name, Value));
            return this;
        }

        public HtmlStyles Add(string Name, UInt32 Value)
        {
            _Childs.Add(new HtmlStyle(Name, Value));
            return this;
        }

        public HtmlStyles Add(HtmlStyle hs)
        {
            _Childs.Add(hs);
            return this;
        }

        public static HtmlStyles operator +(HtmlStyles x, HtmlStyle y)
        {
            x.Add(y);
            return x;
        }

        public static HtmlStyles operator +(HtmlStyles x, HtmlStyles y)
        {
            x._Childs.Union(y._Childs);
            return x;
        }

        public string Name { get; set; }
        public string Value
        {
            get
            {
                if (_Childs.Count() > 0)
#if NET45
                    return string.Join(";", _Childs.Where(x=> x != null).Select(x => x.ToString()));
#else
#if !NET35
                {
                    string ret = null;

                    foreach (var tmp in _Childs)
                    {
                        if (tmp != null)
                        {
                            if (!string.IsNullOrEmpty(ret))
                                ret += ";";
                            ret += tmp.ToString();
                        }
                    }
                    return ret;
                }

                   
#else
                    return string.Join(";", _Childs.Where(x => x != null).Select(x => x.ToString()).ToArray());
#endif
#endif
                else
                    return "";
            }
        }

        public override string ToString()
        {
            return string.Format("{0}='{1}'", "Style", this.Value);
        }

    }


    public class HtmlAttributes : IHTMLProperty
    {
#if !WindowsCE
        private HashSet<HtmlAttribute> _Childs = new HashSet<HtmlAttribute>();
#else
        private List<HtmlAttribute> _Childs = new List<HtmlAttribute>();
#endif
        public HtmlAttributes(params HtmlAttribute[] hs)
        {
            foreach (var h in hs)
                _Childs.Add(h);
        }

        public HtmlAttributes(string Name, string Value)
        {
            _Childs.Add(new HtmlAttribute(Name, Value));
        }

        public HtmlAttributes(string Name, UInt32 Value)
        {
            _Childs.Add(new HtmlAttribute(Name, Value));
        }

        public HtmlAttributes Add(string Name, string Value)
        {
            _Childs.Add(new HtmlAttribute(Name, Value));
            return this;
        }

        public HtmlAttributes Add(string Name, UInt32 Value)
        {
            _Childs.Add(new HtmlAttribute(Name, Value));
            return this;
        }

        public HtmlAttributes Add(HtmlAttribute hs)
        {
            _Childs.Add(hs);
            return this;
        }

        public static HtmlAttributes operator +(HtmlAttributes x, HtmlAttribute y)
        {
            x.Add(y);
            return x;
        }

        public static HtmlAttributes operator +(HtmlAttributes x, HtmlAttributes y)
        {
            x._Childs.Union(y._Childs);
            return x;
        }

        public string Name { get; set; }
        public string Value
        {
            get
            {
                if (_Childs.Count() > 0)
#if NET45
                    return string.Join(" ", _Childs.Where(x => x != null).Select(x => x.ToString()));
#else
#if !NET35
                {
                    string ret = null;

                    foreach (var tmp in _Childs)
                    {
                        if (tmp != null)
                        {
                            if (!string.IsNullOrEmpty(ret))
                                ret += " ";
                            ret += tmp.ToString();
                        }
                    }
                    return ret;
                }


#else
                     return string.Join(" ", _Childs.Where(x=> x != null).Select(x => x.ToString()).ToArray());
#endif


#endif
                else
                    return "";
            }
        }

        public override string ToString()
        {
            return this.Value;//string.Format("{0}='{1}'", "Style", this.Value);
        }

    }



    public class HtmlAttribute : IHTMLProperty
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

        public HtmlAttribute(
           string name,
           UInt32 value)
        {
            this.Name = name;
            this.Value = "#" + value.ToString("x6");
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(this.Value))
                return this.Name;

#if NET35
            if (this.Value.Contains("\'"))
                return string.Format("{0}=\"{1}\"", this.Name, this.Value);

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
        string Name { get; }
        string ToString();
    }


    public class HtmlElement : IHtmlElement
    {
        static bool bCompact = true;
        public string Name { get; set; }
        protected IHTMLProperty[] Attributes { get; set; }
        private StringBuilder _body = new StringBuilder();

//#if !WindowsCE && NET35
        private HashSet<IHtmlElement> _Child = new HashSet<IHtmlElement>();
//#else
//        private List<IHtmlElement> _Child = new List<IHtmlElement>();
//#endif

        public string Txt
        {
            set
            {
                _body.Append(value);
            }
        }

        public HtmlElement(string Name, params IHTMLProperty[] Attributes)
        {
            this.Name = Name;
            this.Attributes = Attributes;
        }

        public HtmlElement Add(string Name, params IHTMLProperty[] Attributes)
        {

            HtmlElement ret = null;

            _Child.Add( ret = new HtmlElement(Name, Attributes));
            return ret;
        }


        public HtmlElement(params IHTMLProperty[] Attributes)
        {
            this.Attributes = Attributes;
        }

        public static HtmlElement operator +(HtmlElement x, HtmlElement y)
        {
            return x.Child(y);
        }

        public static HtmlElement operator +(HtmlElement x, string str)
        {
            return x.Child(new HtmlElement() { Txt = str });
        }

        public HtmlElement Child(params IHtmlElement[] child)
        {
            foreach (var c in child)
                this._Child.Add(c);
            return this;
        }

        public HtmlElement Text(string txt)
        {
            _body.Append(txt);
            return this;
        }


        public override string ToString()
        {
            StringBuilder tag = new StringBuilder();

            if (!string.IsNullOrEmpty(this.Name))
                tag.AppendFormat("<{0}", Name);

            if (!string.IsNullOrEmpty(this.Name) && Attributes.Length > 0)
            {
                tag.Append(" ");
#if NET35
                tag.Append(
                    string.Join(" ",
                        Attributes
                            .Select(
                            kvp => kvp != null ? kvp.ToString() : null)
                            .ToArray()));
#else

                foreach (var tmp in Attributes)
                    tag.Append(tmp.ToString());
#endif
            }
           // Compatibility.NET20.Count(_Child);
            // body/ending
            if (_body.Length > 0 ||   _Child.Count() > 0)
            {
                if (!string.IsNullOrEmpty(this.Name))
                    tag.Append(">");

#if NET35
                _Child.All(c => tag.Append(c.ToString()) != null);
#else
                foreach (var tmp in _Child)
                    tag.Append(tmp.ToString());
#endif

                tag.Append(_body.ToString());
                if (!string.IsNullOrEmpty(this.Name))
                    tag.AppendFormat("</{0}>", this.Name);
            }
            else
                if (!string.IsNullOrEmpty(this.Name))
                    tag.Append("/>");

            if (!bCompact)
                tag.Append("\n");

            return tag.ToString();
        }

            }

        }
