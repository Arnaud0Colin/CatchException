using System;
using System.Collections.Generic;
using System.Text;

namespace LogTrace
{
   public class HttpUtility
    {
      public static string HtmlEncode(string s)
        {
            string ret = string.Empty;
            foreach (char c in s)
            {
                if ((int)c <= 62)
                {
                    switch (c)
                    {
                        case '"':
                            ret += "&quot;";
                            break;
                        case '&':
                            ret += "&amp;";
                            break;
                        case '\'':
                            ret +="&#39;";
                            break;
                        case '<':
                            ret +="&lt;";
                            break;
                        case '>':
                            ret += "&gt;";
                            break;
                        default:
                            ret += c;
                            break;
                    }
                }
                else
                {
                    int num2 = -1;
                    if ((int)c >= 160 && (int)c < 256)
                        num2 = (int)c;
                    else if (char.IsSurrogate(c))
                    {
                       /* int fromUtf16Surrogate = WebUtility.GetNextUnicodeScalarValueFromUtf16Surrogate(ref pch, ref charsRemaining);
                        if (fromUtf16Surrogate >= 65536)
                            num2 = fromUtf16Surrogate;
                        else
                            c = (char)fromUtf16Surrogate; */
                    }
                    if (num2 >= 0)
                    {
                         ret +="&#";
                         ret += num2.ToString(); // (IFormatProvider)NumberFormatInfo.InvariantInfo
                         ret += ';';
                    }
                    else
                        ret += c;
                }
            }
            return ret;
        }
    }
}
