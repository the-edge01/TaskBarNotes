using System;
using System.Drawing;
using System.Windows.Forms;

namespace TaskBarNotes
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = Properties.Settings.Default.TaskNotes;
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.SelectionLength = 0;
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            this.Left = Cursor.Position.X - this.Width;
            this.Top = Cursor.Position.Y - this.Height;
            this.Show();  
            this.Activate();
            this.Focus();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Size = Properties.Settings.Default.WindowSize; //do this first!
            this.Visible = false; //to make it hide at startup        
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            Properties.Settings.Default.WindowSize = this.Size;
            Properties.Settings.Default.TaskNotes = textBox1.Text;
            Properties.Settings.Default.Save();  
            this.Hide();
        }

    }
}
