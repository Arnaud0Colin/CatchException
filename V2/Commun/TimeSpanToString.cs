using System;
using System.Collections.Generic;
#if NET35
using System.Linq;
#endif
using System.Runtime.CompilerServices;
using System.Text;


namespace FransBonhomme.StringFormat
{
    public class TimeSpanToString
    {
        protected readonly static string[] Infos = { " {0} Semaine{1} ", " {0} Jour{1} ", " {0} Heure{1} ", " {0} Minute{1} ", " {0} Second{1} ", " {0} Millisecond{1} " };
        StringBuilder str = new StringBuilder();
        int i = 0;

        public TimeSpanToString()
        {
        }


#if  NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public void Clear()
        {

#if NET35
            str.Clear();
#else
             str.Length = 0;
            str.Capacity = 0;
#endif

            i = 0;
        }

        public string ToString(TimeSpan obj)
        {
            bool bZero = true;
            Clear();
            bZero &= Generate(obj.Days / 7);
            bZero &= Generate(obj.Days % 7);
            bZero &= Generate(obj.Hours);
            bZero &= Generate(obj.Minutes);
            Generate(obj.Seconds);
           // Generate(obj.Milliseconds);
            return str.ToString();
        }

#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private bool Generate(int vache)
        {
            if (vache > 0) str.Append(string.Format(Infos[i], vache, vache > 1 ? "s" : null));
            i++;

            if (i > Infos.Length)
                throw new Exception();

            return (vache == 0);
        }

    }
}
