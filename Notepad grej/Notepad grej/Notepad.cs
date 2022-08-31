using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notepad_grej
{
    public partial class Notepad : Form
    {
        private string filename = null;
        private bool isUnsave = false;
        private bool ignoretextchangedevent = false;
        public Notepad()
        {
            InitializeComponent();
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            string file;

            if (string.IsNullOrEmpty(filename))
                file = "Unnamed";
            else
                file = Path.GetFileName(filename);

            if(isUnsave)
                Text = file + "* - Notepad"; 
            else
                Text = file + " - Notepad";

        }

        private void SaveFile()
        {
            if (string.IsNullOrEmpty(filename))
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    filename = saveFileDialog.FileName;
                else
                    return;
            }


            File.WriteAllText(filename, textBox.Text);
            isUnsave = false;
            UpdateTitle();
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            if(ignoretextchangedevent)
            {
                ignoretextchangedevent = false;
                return;
            }
            
            isUnsave = true;
            UpdateTitle();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var eventArgs = new FormClosingEventArgs(CloseReason.None, false);
            Notepad_FormClosing(null, eventArgs);

            if (eventArgs.Cancel)
                return;

            textBox.Text = string.Empty;
            filename = null;
            isUnsave = false;
            UpdateTitle();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var eventArgs = new FormClosingEventArgs(CloseReason.None, false);
            Notepad_FormClosing(null, eventArgs);

            if (eventArgs.Cancel)
                return;

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ignoretextchangedevent = true;
               textBox.Text = File.ReadAllText(openFileDialog.FileName);
                filename = openFileDialog.FileName;
                isUnsave = false;
                UpdateTitle();
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SaveFile();

        }

        private void Notepad_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(isUnsave)
            {
               var res = MessageBox.Show(this, "Spara Kanske???", "Notepad", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                
                if(res == DialogResult.Yes)
                {
                    SaveFile();
                }
                else if (res == DialogResult.No)
                {

                }
                else if (res == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }

            }
        }

        private void menuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
