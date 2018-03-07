# CatchException

CatchException (Manage Exception c#)
The library capable to catch Unhandled exception and write exception on logging file, in database and by email. Can also Write a trace file, Dump object.

I made this module to facilitate bug hunting, be really proactive, corrected the error before the user does not call you to report the problem.
This program has basically made for enterprise who developer software internally

Example call WriteException
catch (Exception ex)
{
    CatchMe.WriteException(ex).Where().Write();
}

CatchMe.WriteMessage("Service Shutdown").Where().Write<LogSmtp>();



How Configure Smtp module :
  public static void InitSmtp()
        {
            SmtpInfo smtpInfo = new SmtpInfo();
            smtpInfo.from = @"CatchException@LogTrace.net";
            smtpInfo.to.Add(@"Administrateur@YourCompany.fr");
            smtpInfo.subject = "Test Sur Windows";
            smtpInfo.server = "127.0.0.1" /* Ip Server Smtp  */;
            smtpInfo.Credential = new System.Net.NetworkCredential(@"Administrateur@YourCompany.fr", "YourCompany");
            LogSmtp.Info = smtpInfo;
        }

Code to put in the Main procedure :
   static void Main()
        {
      /* Inisilize the logfile module*/
      LogFile.Fichier = CatchMe.ApplicationPath + @"\" + CatchMe.ProcessName + @".html";
      /* Inisilize the logfile module*/  
      CatchMe.Bind<LogFile>();    

      LogWcf.IpAddress = "localhost:60559";
      /* the logsmtp module  */
      InitSmtp();
      /* Id */
      CatchMe.ApplicationId = 1;
#if !DEBUG
      /*     */
      CatchMe.CatchUnhandled();    
#endif
      /*      */
      CatchMe.Bind(SendWcfLog);   

      /* Code remove */
        }

 /*  Execute at unhandled exception   */
 private static void SendWcfLog(CatchMe me)  
        {
             /*  Write the Exception by the Web Service   */
            new LogWcf().Write(me);     
            /*  Write the Exception by the Smtp */
            new LogSmtp().Write(me);   
        }

You can also Dump Object
private void button1_Click(object sender, EventArgs e)
{         
   string s =  DumpObject.Dump(e, 0);
 }

EvenArgs
[MouseButtons] Button : 
 	[Int32] value__ : 1048576 
 
[Int32] Clicks : 1 
[Int32] X : 13 
[Int32] Y : 9 
[Int32] Delta : 0 
[Point] Location : 
 	[Boolean] IsEmpty : False 
	[Int32] X : 13 
	[Int32] Y : 9 


If you AllowDump catchexception zip the dump file and put in the html file.
<tr> 
<td align='left' bgcolor='#eeeeec' colspan='1' >EventArgs</td><td align='left' bgcolor='#eeeeec' colspan='1' >EventArgs</td><td align='left' bgcolor='#eeeeec' colspan='1' >System.Windows.Forms.MouseEventArgs
<a href="data:application/zip;base64,H4sIAAAAAAAEAO29B2AcSZYlJi9tynt/SvVK1+B0oQiAYBMk2JBAEOzBiM3mkuwdaUcjKasqgcplVmVdZhZAzO2dvPfee++999577733ujudTif33/8/XGZkAWz2zkrayZ4hgKrIHz9+fB8/Ir73RbVu8ifrtq2WzfdT+SV9lD6e1HeP0l+4nDSrw++dLdt7e99PL7Nynf/+vz99u7uzf3D/wafaiv81jU7KYvq2QZvw89+bPtrTN8xnvw+addo9zcs2o8939OOXVbFsv58+r6ZZW/RRe1JVZZ4tv5+eNaeLVXtN3z/LyiaXViH+PgrhNz4i8u//A436NfkYAQAA"/>
</td></tr> 

Using Trace
 /* Inisilize the TraceFile module*/
   TraceFile.Fichier = CatchMe.ApplicationPath + @"\" + CatchMe.ProcessName + @".Txt";
   TraceFile.Level =3;

   TraceFile.StartClose("Service Start");
