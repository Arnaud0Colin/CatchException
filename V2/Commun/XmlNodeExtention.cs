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
using System.Globalization;
using System.IO;
#if NET35
using System.Linq;
#endif
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;

namespace CatchException
{

    /// <summary>
    /// ConfigXML
    /// <example>  </example>/// 
    /// </summary>
    public class ConfigXML : DocXML
    {

        /// <summary>
        /// ConfigXML
        /// </summary>
        public ConfigXML()
        {
            DocumentElement = @"ConfigXML";
        }


    }

    /// <summary>
    /// XmlNodeExtention
    /// <example>  </example>/// 
    /// </summary>
    public static class XmlNodeExtention
    {
        /// <summary>
        /// Get Attribute of the node
        /// <para>
        /// <param name="xn">XmlNode</param>
        /// <param name="Attribute">string</param>
        /// </para>
        /// <example> xn.Get ushort("Vache"); </example>/// 
        /// <returns>T</returns>
        /// </summary>
#if NET35
        public static T Get<T>(this XmlNode xn, string Attribute)
#else
        public static T Get<T>(XmlNode xn, string Attribute)
#endif
        {
            XmlAttribute xName = xn.Attributes[Attribute];
            if (xName != null)
                return (T)Convert.ChangeType(xName.Value, typeof(T), CultureInfo.InvariantCulture);
            else
                return default(T);
        }

        /// <summary>
        /// Get Attribute of the node
        /// <para>
        /// <param name="xn">XmlNode</param>
        /// <param name="Attribute">string</param>
        /// </para>
        /// <example> xn.Get("Vache"); </example>/// 
        /// <returns>string</returns>
        /// </summary>
#if NET35
        public static string Get(this XmlNode xn, string Attribute)
#else
        public static string Get(XmlNode xn, string Attribute)
#endif
        {
            XmlAttribute xName = xn.Attributes[Attribute];
            if (xName != null)
                return xName.Value;
            else
                return null;
        }
    }


    /// <summary>
    /// ConfigXML
    /// <example>  </example>/// 
    /// </summary>
    public class DocXML
    {
        private XmlDocument _fXml = null;
        /// <summary>
        /// LastException
        /// </summary>
        public Exception LastException = null;

#if !WindowsCE
        /// <summary>
        /// Raises when the configuraiton file is modified
        /// </summary>
        public event System.EventHandler FileChanged;
#endif

        /// <summary>
        /// DocumentElement
        /// </summary>
        public string DocumentElement = null;


        /// <summary>
        /// constructor
        /// </summary>
        public DocXML()
        {
            if (_fXml == null)
                _fXml = new XmlDocument();
        }


        /// <summary>
        /// Get all Childs of Element
        /// <example> GetChilds(); </example>/// 
        /// <returns>XmlNodeList</returns>
        /// </summary>
        public XmlNodeList GetChilds()
        {
            if (Element != null)
                return Element.ChildNodes;
            else
                return null;
        }

        /// <summary>
        /// return DocumentElement
        /// <example> Element; </example>/// 
        /// <returns>XmlElement</returns>
        /// </summary>
        public XmlDocument Doc
        {
            get
            {
                return _fXml;
            }
        }

        /// <summary>
        /// return DocumentElement
        /// <example> Element; </example>/// 
        /// <returns>XmlElement</returns>
        /// </summary>
        public XmlElement Element
        {
            get
            {
                return _fXml.DocumentElement;
            }
        }
        /// <summary>
        /// <para>
        /// <param name="uri">string</param>
        /// <param name="bCreate">bool</param>
        /// </para>
        ///  Return the right XmlElement
        /// <example> Road("Config/Init",true); </example>/// 
        /// <returns>XmlElement</returns>
        /// </summary>
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public XmlElement Road(string uri, bool bCreate)
        {
            XmlElement xe = null;
            XmlElement xet = null;

            if (_fXml.DocumentElement == null)
            {
                if (DocumentElement == null || !bCreate)
                    return xet;

                /*if(bCreate)
                {*/
                xe = _fXml.CreateElement(DocumentElement);
                _fXml.AppendChild(xe);
                /*}
                else
                    return xet;*/
            }
            else
                xe = _fXml.DocumentElement;

            if (!string.IsNullOrEmpty(uri))
            {
                string[] ArStr = uri.Split('/', '.');
                foreach (string s in ArStr)
                {
                    if (xe == null)
                        xet = _fXml[s];
                    else
                        xet = xe[s];

                    if (bCreate)
                    {
                        if (xet == null)
                        {
                            xet = _fXml.CreateElement(s);
                            xe.AppendChild(xet);
                        }
                    }

                }
            }
            else
                xet = xe;

            return xet;
        }


        /// <summary>
        /// <para>
        /// <param name="Attribute">string</param>
        /// <param name="args">string[]</param>
        /// </para>
        /// Read value in XML file
        /// <example> ReadXml("", "Config" ,"Init"); </example>/// 
        /// <returns>TResult</returns>
        /// </summary>
#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public string ReadXml(string Attribute, params string[] args)
        {
            XmlElement xe = null;
            if ((xe = Road(null, false)) != null)
                return null;

            foreach (string s in args)
            {
                if (xe[s] != null)
                    xe = xe[s];
                else
                    return null;
            }

            if (Attribute == null)
                return xe.InnerXml;
            else
                if (xe.GetAttribute(Attribute) != string.Empty)
                return xe.GetAttribute(Attribute);
            else
                return null;
        }

        /// <summary>
        /// <para>
        /// <param name="uri">string</param>
        /// <param name="Attribute">string</param>
        /// <param name="def">TResult</param>
        /// </para>
        /// Read value in XML file
        /// <example> ReadXml("Config/Init",true, 5); </example>/// 
        /// <returns>TResult</returns>
        /// </summary>
        public TResult ReadXml<TResult>(string uri, string Attribute, TResult def)
        {
            bool bCreate = ((def != null) && !def.Equals(default(TResult)));

            if (_fXml == null)
                _fXml = new XmlDocument();

            XmlElement xe = Road(uri, bCreate);
            if (xe != null)
            {
                if (Attribute == null)
                {
                    try
                    {
                        return (TResult)Convert.ChangeType(xe.InnerXml, typeof(TResult), CultureInfo.InvariantCulture);
                    }
                    catch (FormatException)
                    {
                        return def;
                    }
                }
                else
                    if (xe.GetAttribute(Attribute) != string.Empty)
                {
                    try
                    {
                        return (TResult)Convert.ChangeType(xe.GetAttribute(Attribute), typeof(TResult), CultureInfo.InvariantCulture); //
                    }
                    catch (FormatException)
                    {
                        return def;
                    }
                }
                else
                    return def;
            }
            else
                return def;
        }

        /// <summary>
        /// <para>
        /// <param name="uri">string</param>
        /// <param name="Attribute">string</param>
        /// <param name="value">TResult</param>
        /// </para>
        /// Write value in XML file
        /// <example> ReadXml("Config/Init",true, 5); </example>/// 
        /// <returns>void</returns>
        /// </summary>
        public void WriteXml<TResult>(string uri, string Attribute, TResult value)
        {
            if (_fXml == null)
                _fXml = new XmlDocument();

            XmlElement xe = Road(uri, true);

            if (xe != null)
            {
                if (Attribute == null)
                    xe.InnerXml = value.ToString();
                else
                    xe.SetAttribute(Attribute, (string)Convert.ChangeType(value, typeof(string), CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// <para>
        /// <param name="path">string</param>
        /// </para>
        /// Create and Load XML file in memory
        /// <example> CreateLoad("c:\\ConfigXmlTest1.xml"); </example>/// 
        /// <returns>ConfigXML</returns>
        /// </summary>
        public static ConfigXML CreateLoad(string path)
        {
            var ff = new ConfigXML();
            ff.Load(path, null);
            return ff;
        }

        /// <summary>
        /// <para>
        /// <param name="path">string</param>
        /// <param name="CheckName">string</param>
        /// </para>
        /// Load XML file in memory
        /// <example> Load("c:\\ConfigXmlTest1.xml"); </example>/// 
        /// <returns>bool</returns>
        /// </summary>
#if !WindowsCE
        public bool Load(string path, string CheckName = null)
#else
        public bool Load(string path, string CheckName)
#endif
        {

            try
            {
                _fXml = new XmlDocument();
                _fXml.Load(path);
                if (CheckName != null && _fXml.DocumentElement.Name != CheckName)
                    return false;
            }
            catch (System.IO.IOException ex)
            {
                LastException = ex;
                return false;
            }
            catch (ArgumentException ex)
            {
                LastException = ex;
                return false;
            }

#if !WindowsCE
            try
            {

                string FullPath = new Uri(_fXml.BaseURI).LocalPath;
                var watcher = new FileSystemWatcher(Path.GetDirectoryName(FullPath), Path.GetFileName(FullPath));
                watcher.Changed += FileChangedEvent;

            }
            catch
            {
            }
#endif

            return true;


        }

#if !WindowsCE

        /// <summary>
        /// Called when the configuration file changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileChangedEvent(object sender, FileSystemEventArgs e)
        {
            // Check if the file changed event has listeners
            if (FileChanged != null)
                // Raise the event
                FileChanged(this, new EventArgs());
        }
#endif

        /// <summary>
        /// <para>
        /// <param name="path">string</param>
        /// </para>
        /// Save XML file in disk
        /// <example> Save("c:\\ConfigXmlTest1.xml"); </example>/// 
        /// <returns>bool</returns>
        /// </summary>
        public bool Save(string path)
        {
            try
            {
                _fXml.Save(path);
                return true;
            }
            catch (System.IO.IOException ex)
            {
                LastException = ex;
                return false;
            }
            catch (ArgumentException ex)
            {
                LastException = ex;
                return false;
            }
        }

    }
}
