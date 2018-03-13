using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;

using System.Text;
using System.Windows.Forms;

namespace CatchException
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
            label1.Text = "Une erreur irrécupérable s'est produite. Merci de contacter la hotline dans les plus bref délais";

            panel1.Visible = false;

            this.Size = new Size(this.Size.Width, Height = 127);

            Center();

        }
        public ExceptionDialog(CatchMe me)
        {
            InitializeComponent();

#if !WindowsCE
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
#endif


            this.Text = "Gestion d'Erreur " + Path.GetFileNameWithoutExtension(System.AppDomain.CurrentDomain.FriendlyName);
            label1.Text = "Une erreur irrécupérable s'est produite. Merci de contacter la hotline dans les plus bref délais";

            panel1.Visible = true;

            tbExplication.Text = me.ToString();
            this.Size = new Size(this.Size.Width, Height = 262);

            Center();

        }


        private void btOk_Click(object sender, EventArgs e)
        {
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
