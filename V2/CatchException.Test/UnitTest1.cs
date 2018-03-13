using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CatchException;
using System.Collections.Generic;
using System.Linq;

namespace CatchEception.Test
{
    [TestClass]
    public class UnitTest1
    {

        class testy : List<DateTime>
        {
            public string ff = "ooioui";
        }

        [TestMethod]
        public void TestMethod1()
        {


            DateTime dtNow = DateTime.Now;

            IEnumerable<DateTime> jj = null;


            List<testy> ddss = new List<testy>();

            testy dd = new testy();

          //

            bool uu = dtNow is System.Collections.IEnumerable;

            for (int i = 0; i < 100; i++)
            {
                dd.Add(dtNow.AddDays(i));
            }

            ddss.Add(dd);



            string pp = ddd(dd);

            string ss = Dump.GetValue(ddss);

            var t = ss.Length;

            Assert.AreEqual(ss, dtNow.ToString());

            if (dd != null)
            {
                if (dd.GetType().GetInterfaces().Any(
                        i => i.IsGenericType &&
                        i.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                {
                    var ssss = dd.GetType().GetInterfaces().Where(
                         i => i.IsGenericType &&
                         i.GetGenericTypeDefinition() == typeof(IEnumerable<>));


                }
            }

        }

        string ddd<T>(T r)
        {
            string ttt = string.Empty;

            if(r is System.Collections.IEnumerable)
            {
                foreach( var f in r as System.Collections.IEnumerable)
                {
                   ttt += f.ToString();
                }
            }
            return ttt;
        }

    }
}

 
    

