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
using System.Xml.Linq;
#endif
using System.Text;

using System.Xml;


namespace CatchException
{
#if NET35

    /// <summary>
    /// Convert Linq to/From Data.Linq
    /// </summary>
    /// <example>  </example>/// 
    public static class LinqExtension
    {

        /// <summary>
        /// Converts an XDocument to an XmlDocument.
        /// </summary>
        /// <param name="xdoc">The XDocument to convert.</param>
        /// <returns>The equivalent XmlDocument.</returns>
        public static XmlDocument ToXmlDocument(this XDocument xdoc)
        {
            var xmldoc = new XmlDocument();
            xmldoc.Load(xdoc.CreateReader());
            return xmldoc;
        }


#if !WindowsCE
        /// <summary>
        /// Converts an XmlDocument to an XDocument.
        /// </summary>
        /// <param name="xmldoc">The XmlDocument to convert.</param>
        /// <returns>The equivalent XDocument.</returns>
        public static XDocument ToXDocument(this XmlDocument xmldoc)
        {

            return XDocument.Load(xmldoc.CreateNavigator().ReadSubtree());
        }
#endif


        /// <summary>
        /// Converts an XElement to an XmlElement.
        /// </summary>
        /// <param name="xelement">The XElement to convert.</param>
        /// <returns>The equivalent XmlElement.</returns>
        public static XmlElement ToXmlElement(this XElement xelement)
        {
            return new XmlDocument().ReadNode(xelement.CreateReader()) as XmlElement;
        }

#if !WindowsCE
        /// <summary>
        /// Converts an XmlElement to an XElement.
        /// </summary>
        /// <param name="xmlelement">The XmlElement to convert.</param>
        /// <returns>The equivalent XElement.</returns>
        public static XElement ToXElement(this XmlElement xmlelement)
        {
            if (xmlelement != null)
                return XElement.Load(xmlelement.CreateNavigator().ReadSubtree());
            else
                return default(XElement);
        }
#endif

    }
#endif
}
