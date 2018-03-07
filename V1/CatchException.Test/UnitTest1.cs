using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.NetworkInformation;
using System.Net;

namespace CatchException.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
        }


        [TestMethod]
        public void TestCatch_Net()
        {
            //  ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            /*
            ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("",
                    "SELECT * FROM Win32_BaseBoard");

            foreach (ManagementObject queryObj in searcher.Get())
            {
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("Win32_BaseBoard instance");
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("SerialNumber: {0}", queryObj["SerialNumber"]);
                string a1 = queryObj["Manufacturer"].ToString();
                string a2 = queryObj["SerialNumber"].ToString();

                var g = a1 + a2;
            }


            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            int i = 5;
            i++;
            i = nics.Count();

            string ip = "";
            string strHostName = "";
            strHostName = System.Net.Dns.GetHostName();

            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

            IPAddress[] addr = ipEntry.AddressList;

            ip = addr[2].ToString();
            */
        }

    }
}
