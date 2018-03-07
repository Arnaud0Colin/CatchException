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
using System.Xml;


namespace LogTrace.CatchException
{

    /// <summary>
    /// Interface IActionLog To create a class action to write log data to a media
    /// </summary>
    public interface ICatchAction
    {
        /// <summary>
        /// Method call by the .Write();
        /// </summary>
        void Write(ICatchMe Entry);
    }

    /// <summary>
    /// LogEntry All the data
    /// </summary>
    public interface ICatchMe
    {

        /// <summary>
        /// test
        /// </summary>
        ushort? UrgenceLevel { get;  }
        string ToHtml();
        string ComputerName { get; }
        int GetApplicationId();
    }


  
}
