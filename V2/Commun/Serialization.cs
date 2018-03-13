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
using System.IO;
#if NET35
using System.Linq;
#endif
using System.Text;

using System.Xml;
using System.Xml.Serialization;

namespace CatchException
{
    /// <summary>
    /// xml Serialization class    
    /// </summary>
    /// <example> Truc T = CXmlToString.DeserializeFromXml(""); </example>/// 
    public class CToXmlString
    {
        /// <summary>
        /// <para>
        /// <param name="data">T</param>
        /// </para>
        /// SerializeToXml
        /// <example> string xml = CToXmlString.SerializeToXml(T);</example>/// 
        /// <returns>T</returns>
        /// </summary>
        public static string SerializeToXml<T>(T data)
        {
            MemoryStream memStream = new MemoryStream();
            XmlSerializer mySerializer = new XmlSerializer(typeof(T));
            mySerializer.Serialize(memStream, data);
            memStream.Seek(0, System.IO.SeekOrigin.Begin);
            XmlDocument doc = new XmlDocument();
            doc.Load(memStream);
            memStream = null;
            mySerializer = null;
            return doc.OuterXml;
        }

        /// <summary>
        /// <para>
        /// <param name="value">string</param>
        /// </para>
        /// DeserializeFromXml
        /// <example> Truc T = CToXmlString.DeserializeFromXml(" xml ");</example>/// 
        /// <returns>T</returns>
        /// </summary>
        public static T DeserializeFromXml<T>(string value)
        {
            T result;
            MemoryStream memStream = new MemoryStream();
            XmlSerializer mySerializer = new XmlSerializer(typeof(T));
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            byte[] bytearray = enc.GetBytes(value);
            memStream.Write(bytearray, 0, bytearray.Length);
            memStream.Seek(0, System.IO.SeekOrigin.Begin);
            result = (T)mySerializer.Deserialize(memStream);
            memStream = null;
            mySerializer = null;
            return result;
        }
    }


    /// <summary>
    /// xml Serialization class    
    /// </summary>
    /// <example> Truc T = CToXmlDocument.DeserializeFromXml(""); </example>/// 
    public class CToXmlDocument
    {

        /// <summary>
        /// <para>
        /// <param name="data">T</param>
        /// </para>
        /// SerializeToXml
        /// <example> XmlDocument xml = CToXmlDocument.SerializeToXml(T);</example>/// 
        /// <returns>T</returns>
        /// </summary>
        public static XmlDocument SerializeToXml<T>(T data)
        {
            MemoryStream memStream = new MemoryStream();
            XmlSerializer mySerializer = new XmlSerializer(typeof(T));
            mySerializer.Serialize(memStream, data);
            memStream.Seek(0, System.IO.SeekOrigin.Begin);
            XmlDocument doc = new XmlDocument();
            doc.Load(memStream);
            memStream = null;
            mySerializer = null;
            return doc;
        }

        /// <summary>
        /// <para>
        /// <param name="value">string</param>
        /// </para>
        /// DeserializeFromXml
        /// <example> Truc T = CToXmlDocument.DeserializeFromXml(" xml ");</example>/// 
        /// <returns>T</returns>
        /// </summary>
        public static T DeserializeFromXml<T>(XmlDocument value)
        {
            T result;
            MemoryStream memStream = new MemoryStream();
            value.Save(memStream);
            XmlSerializer mySerializer = new XmlSerializer(typeof(T));
            memStream.Seek(0, System.IO.SeekOrigin.Begin);
            result = (T)mySerializer.Deserialize(memStream);
            memStream = null;
            mySerializer = null;
            return result;
        }

    }

    /// <summary>
    /// xml Serialization class    
    /// </summary>
    /// <example> Truc T = CToXmlElement.DeserializeFromXml(""); </example>/// 
    public class CToXmlElement
    {
        /// <summary>
        /// <para>
        /// <param name="data">T</param>
        /// </para>
        /// SerializeToXml
        /// <example> XmlElement xml = CToXmlElement.SerializeToXml(T);</example>/// 
        /// <returns>T</returns>
        /// </summary>
        public static XmlElement SerializeToXml<T>(T data)
        {
            MemoryStream memStream = new MemoryStream();
            XmlSerializer mySerializer = new XmlSerializer(typeof(T));
            mySerializer.Serialize(memStream, data);
            memStream.Seek(0, System.IO.SeekOrigin.Begin);
            XmlDocument doc = new XmlDocument();
            doc.Load(memStream);
            memStream = null;
            mySerializer = null;
            return doc.DocumentElement;
        }

        /// <summary>
        /// <para>
        /// <param name="value">string</param>
        /// </para>
        /// DeserializeFromXml
        /// <example> Truc T = CToXmlElement.DeserializeFromXml(" xml ");</example>/// 
        /// <returns>T</returns>
        /// </summary>
        public static T DeserializeFromXml<T>(XmlElement value)
        {
            T result;
            MemoryStream memStream = new MemoryStream();
            XmlSerializer mySerializer = new XmlSerializer(typeof(T));
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            byte[] bytearray = enc.GetBytes(value.OuterXml);
            memStream.Write(bytearray, 0, bytearray.Length);
            memStream.Seek(0, System.IO.SeekOrigin.Begin);
            result = (T)mySerializer.Deserialize(memStream);
            memStream = null;
            mySerializer = null;
            return result;
        }

    }

    /*
    /// <summary>
    /// <para>
    /// <param name="data">T</param>
    /// </para>
    /// SerializeToXml
    /// <example> XmlDocument xml = CXmlFile.SerializeToXml(T);</example>/// 
    /// <returns>T</returns>
    /// </summary>
    public static XmlDocument SerializeToXml<T>(T data)
    {
        MemoryStream memStream = new MemoryStream();
        XmlSerializer mySerializer = new XmlSerializer(typeof(T));
        mySerializer.Serialize(memStream, data);
        memStream.Seek(0, System.IO.SeekOrigin.Begin);
        XmlDocument doc = new XmlDocument();
        doc.Load(memStream);
        memStream = null;
        mySerializer = null;
        return doc; //mySerializer.ToString();
    }
    */




    /// <summary>
    /// xml Serialization class    
    /// </summary>
    /// <example> Truc T = CXmlFile.DeserializeFromXml("c:\sample.xml"); </example>/// 
    public class CXmlFile
    {
        /// <summary>
        /// <para>
        /// <param name="path">string</param>
        /// </para>
        /// DeserializeFromXml
        /// <example> Truc T = CXmlFile.DeserializeFromXml("c:\sample.xml");</example>/// 
        /// <returns>T</returns>
        /// </summary>
        public static T DeserializeFromXml<T>(string path)
        {
            T result;
            XmlSerializer mySerializer = new XmlSerializer(typeof(T));


            using (FileStream _fs = new FileStream(path,
                                      FileMode.Open,
                                      FileAccess.Read,
                                      FileShare.ReadWrite))
            {
                using (StreamReader _sr = new StreamReader(_fs))
                {
                    result = (T)mySerializer.Deserialize(_sr);
                }
            }
            return result;

        }

        /// <summary>
        /// <para>
        /// <param name="path">string</param>
        /// <param name="data">T</param>
        /// </para>
        /// SerializeToXml
        /// <example> CXmlFile.SerializeToXml("c:\sample.xml", sample); </example>/// 
        /// <returns>bool</returns>
        /// </summary>
        public static bool SerializeToXml<T>(string path, T data)
        {
            XmlSerializer mySerializer = new XmlSerializer(typeof(T));
            using (StreamWriter _fs = new StreamWriter(path))
            {
                mySerializer.Serialize(_fs, data);
            }
            return true;
        }




    }
}
