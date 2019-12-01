using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace TaskBarNotes
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<NameText> ntl = new List<NameText>();
        List<TabPage> tabpage;
        List<TextBox> textbox;

        private void display()
        {
            tabControl1.Controls.Clear();

            tabpage = new List<TabPage>();
            textbox = new List<TextBox>();
            for (int i = 0; i < ntl.Count; i++)
            {
                TextBox tbox = new TextBox();
                tbox.Dock = System.Windows.Forms.DockStyle.Fill;
                tbox.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                tbox.Location = new System.Drawing.Point(3, 3);
                tbox.Multiline = true;
                tbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
                tbox.Text = ntl[i].text;
                textbox.Add(tbox);

                TabPage tpage = new TabPage();
                tpage.Controls.Add(textbox[i]);
                tpage.Location = new System.Drawing.Point(4, 25);
                tpage.Padding = new System.Windows.Forms.Padding(3);
                tpage.Text = ntl[i].name;
                tpage.UseVisualStyleBackColor = true;
                this.tabControl1.Controls.Add(tpage);
                tabpage.Add(tpage);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                System.Collections.Specialized.StringCollection sc = Properties.Settings.Default.TaskNotesName;

                ntl.Clear();
                if (sc.Count == 0)
                {
                    goto Error;
                }
                for (int i = 0; i < sc.Count; i++)
                {
                    
                    ntl.Add(new NameText(sc[i], Properties.Settings.Default.TaskNotes[i].Replace("\n","\r\n")));
                }
                display();

                return;
            }
            catch (Exception)
            {
 
            }
            Error:

            ntl.Clear();
            ntl.Add(new NameText("Notes","Enter text here"));

            display();
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
            try
            {
                this.Size = Properties.Settings.Default.WindowSize; //do this first!
            }
            catch (Exception)
            {
                
            }
            this.Visible = false; //to make it hide at startup   
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            if (contextMenuStrip1.Visible)//if we are clicking on the text box, don't hide the form
            {
                return;
            }

            save();

            this.Hide();
        }

        private void save()
        {
            System.Collections.Specialized.StringCollection sc_name = new System.Collections.Specialized.StringCollection();
            System.Collections.Specialized.StringCollection sc_text = new System.Collections.Specialized.StringCollection();
            ntl.Clear();
            for (int i = 0; i < tabpage.Count; i++)
            {
                //save to NTL for future reference in code
                ntl.Add(new NameText(tabpage[i].Text, textbox[i].Text));

                //save to disk
                sc_name.Add(tabpage[i].Text);
                sc_text.Add(textbox[i].Text);
            }

            Properties.Settings.Default.TaskNotesName = sc_name;
            Properties.Settings.Default.TaskNotes = sc_text;

            Properties.Settings.Default.WindowSize = this.Size;
            Properties.Settings.Default.Save();  
        }

        class NameText{
            public string name;
            public string text;

            public NameText(string name_l, string text_l)
            {
                name = name_l;
                text = text_l;
            }
        }

        int clickedTab = 0;
        private void tabControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
            for (int i = 0; i < tabControl1.TabCount; i++ )
            {
                if (tabControl1.GetTabRect(i).Contains(e.Location))
                {
                    clickedTab = i;
                }
            }
            toolStripTextBox2.Text = tabpage[clickedTab].Text;
            
                contextMenuStrip1.Show(Cursor.Position);
            }
        }

        //Add
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            save();

            ntl.Add(new NameText("New Note", "Enter text here"));

            display();
        }

        //remove
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ntl.RemoveAt(clickedTab);
            display();
        }

        //rename
        private void toolStripTextBox2_TextChanged(object sender, EventArgs e)
        {
             tabpage[clickedTab].Text = toolStripTextBox2.Text;
             ntl[clickedTab].name = toolStripTextBox2.Text;
        }

        //move left
        private void moveLeftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            save();
            if (clickedTab > 0)
            {
                //swap 2 elements
                NameText nt = ntl[clickedTab];
                ntl[clickedTab] = ntl[clickedTab - 1];
                ntl[clickedTab - 1] = nt;
            }
            display();
        }


    }
}
