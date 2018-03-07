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
#if !WindowsCE
using System.Net.Mail;
#endif
using System.Text;

namespace LogTrace.CatchException
{


    public class LogSmtp : NotificationSmtp, ICatchAction
    {
          public static ushort Level = 0;

          public void Write(ICatchMe Entry)
          {
              if (Entry.UrgenceLevel.HasValue && (Level != 0))
                  if (Entry.UrgenceLevel.Value > Level)
                      return;

              if (_Info == null)
                  return;

              Send(Entry);

          }

    }


}
