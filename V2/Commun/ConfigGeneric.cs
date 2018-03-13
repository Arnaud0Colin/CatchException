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
    public class ConfigGeneric<T>
    {
        public static T Instance = default(T);


        public static T Load(string FichierConfig)
        {
            try
            {
                return ConfigGeneric<T>.Instance = CXmlFile.DeserializeFromXml<T>(FichierConfig);
            }
            catch
            {
                return default(T);
            }
        }

    }

    /// <summary>
    /// Number Of Digit
    /// <example>  </example>/// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ConfigFile : Attribute
    {
        /// <summary>
        /// <para>
        /// <param name="nb">ushort</param>
        /// </para>
        /// Constructor
        /// </summary>
        public ConfigFile(string File)
        {
            this.FileName = File;
        }

        /// <summary>
        /// Number of digit
        /// <example>  </example>/// 
        /// <returns>ushort</returns>
        /// </summary>
        public string FileName { get; private set; }
    }
}
