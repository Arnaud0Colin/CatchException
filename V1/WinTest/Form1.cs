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

using LogTrace;
using LogTrace.CatchException;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
#if NET35
using System.Linq;
#endif
using System.Reflection;
using System.Text;

using System.Windows.Forms;
using System.IO;

namespace WinTest
{
    public partial class Form1 : Form
    {
        int limite = 5;
        public Form1()
        {
            InitializeComponent();


         // textBox1.Text =  Zip.Base64ToUncompress(hhh);
        }


        string hhh = @"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";
    

        private static string ShowSquares(object val)
        {
            return val.ToString();
        }

      
/*
            if (props.Count() > 0)
            {
                foreach (var y in props)
                {
                    if (!y.GetMethod.Attributes.HasFlag(MethodAttributes.Static) && y.Name != "Chars")
                    {

                        if (y.PropertyType.Equals(typeof(AccessibleObject)) || y.PropertyType.Equals(typeof(BindingManagerBase)) 
                           || y.PropertyType.Equals(typeof(ControlCollection)) || y.PropertyType.Equals(typeof(AccessibleRole))
                            || y.PropertyType.Equals(typeof(ControlBindingsCollection)) || y.PropertyType.Equals(typeof(Control))
                            || y.PropertyType.Equals(typeof(MethodBase))
                            )
                            continue;
                        try
                        {
                            object hhh = y.GetValue(Obj);
                        
                            if (hhh == null)
                            {
                                string ligne = string.Format("[{0}] {1} : {2}", CatchMe.GetTypeName(y.PropertyType), y.Name, "Null");
                                srt += ligne;
                                continue;
                            }
                            if (y.PropertyType.IsPrimitive || y.PropertyType.Equals(typeof(string)))
                            {
                                string ligne = string.Format("[{0}] {1} : {2}", CatchMe.GetTypeName(y.PropertyType), y.Name, hhh.ToString());
                                srt += ligne;
                            }
                            else
                            {

                                string ligne = string.Format("[{0}] {1} : {2}", CatchMe.GetTypeName(y.PropertyType), y.Name, Traite(hhh, niveau + 1));
                                srt += ligne;
                            }
                           
                        }
                        catch
                        {
                          //  string ligne = "[" + CatchMe.GetTypeName(y.PropertyType) + "] " + y.Name + " : " + Obj.ToString() +"\r\n";
                          //  srt += ligne;
                        }
                    }
                }
            }
            else
                srt = Obj.ToString();
            */
     

        public class top
        {
            public top()
            {
                chiffre2.Add(8);
                chiffre2.Add(78);
                chiffre2.Add(89);
            }
            public object chiffre = new int[5, 2] { { 5, 2 }, { 4, 2 }, { 7, 2 }, { 8, 2 }, { 9, 2 } };
            public List<int> chiffre2 = new List<int>();
               
        }

        private void button1_Click(object sender, EventArgs e)
        {

            

            this.Tag = this;
            CatchMe.AllowDump = true;
            string yyy = null;
            using (StreamWriter sw = new StreamWriter(@"c:\ProjetTemp\Variable.txt"))
            {
                Button[] hh = new Button [] { new Button(), new Button() };

                int[,] hh2 = new int[2,2]  { {0,0}, {5,8} };

                List<int> hh3 = new List<int>();
                hh3.Add(5);
                hh3.Add(5);
                hh3.Add(5);
                hh3.Add(5);
                hh3.Add(5);

                //sw.Write(yyy = DumpObject.Dump(this, 0));
                sw.Write(yyy = new DumpObject().Dump(hh3, 0));
            }
            textBox1.Text = yyy;

          //  object ret2 = null;
           // ret2.ToString();
            /*
            try
            {
                object ret = null;
                ret.ToString();
            }
            catch(Exception ex)
            {
               // string str = DumpObject.Dump(this, 0);
                    
                LogTrace.CatchException.CatchMe.WriteMessage("Test").Level(1).Screen().Variable("EventArgs", e).Variable("Sender", sender).Where().Write();

                LogTrace.CatchException.CatchMe.WriteException(ex).Where().Write();
        }*/
            }
    }
}
