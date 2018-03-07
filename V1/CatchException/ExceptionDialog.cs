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
using System.ComponentModel;
using System.Drawing;
using System.IO;

using System.Text;
using System.Windows.Forms;

namespace LogTrace.CatchException
{

    
    public partial class ExceptionDialog : Form
    {
        public ExceptionDialog()
        {
            InitializeComponent();

#if !WindowsCE
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
#endif


            this.Text = "Gestion d'Erreur " + Path.GetFileNameWithoutExtension(System.AppDomain.CurrentDomain.FriendlyName);
            lbMessage.Text = "Une erreur irrécupérable s'est produite. Merci de contacter la hotline dans les plus bref délais";

            Center();

        }

        private void btOk_Click(object sender, EventArgs e)
        {
            if(CatchMe.RestartApp)
                Application.Restart();
            this.Close();
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// <para>
        /// <param name="ctrl">Control</param>
        /// </para>
        /// Center a control
        /// </summary>
        protected static void Center(Control ctrl)
        {
            Point location = new Point(
                 Screen.PrimaryScreen.WorkingArea.Width / 2 - ctrl.Width / 2,
                 Screen.PrimaryScreen.WorkingArea.Height / 2 - ctrl.Height / 2);
            ctrl.Location = location;
        }

        /// <summary>
        /// Center the form
        /// </summary>
        protected void Center()
        {
            Center(this);
        }

      
    }
}
