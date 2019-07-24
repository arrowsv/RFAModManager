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
using System.Xml.Linq;

namespace RFAMM
{
    public partial class newMod : Form
    {
        public newMod()
        {
            InitializeComponent();

            
        }

        private void game_DirectoryText_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();

            

        }

        private void button4_Click(object sender, EventArgs e)
        {
            fileList.ExpandAll();
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "Browse for edited files";
            fdlg.InitialDirectory = modName_Text.Text;
            fdlg.Filter = "All files (*.*)|*.*|XTBL files (*.xtbl)|*.xtbl";
            fdlg.FilterIndex = 2;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                fileList.Nodes["Files"].Nodes.Add(fdlg.FileName);
                fileList.ExpandAll();

                string fileToCopy = fdlg.FileName;
                string destinationDirectory = Environment.CurrentDirectory + "\\donotdelete\\temp\\";

                File.Copy(fileToCopy, destinationDirectory + Path.GetFileName(fileToCopy));
            }
        }

        private void fileList_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void newMod_FormClosing(object sender, FormClosingEventArgs e)
        {
            DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + "\\donotdelete\\temp\\");

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                this.Enabled = false; // Prevents the user from using any of the forms controls to prevent errors.

                string rootDirectory = Environment.CurrentDirectory + "\\mods\\";
                string subdir = rootDirectory + modName_Text.Text + "\\";
                if (!Directory.Exists(subdir))
                {
                    Directory.CreateDirectory(subdir); //Create this directory if it doesn't exist.
                    Directory.CreateDirectory(subdir + "misc.vpp_pc\\");

                    string dest = subdir + "misc.vpp_pc\\";
                    foreach (var file in Directory.EnumerateFiles(Environment.CurrentDirectory + "\\donotdelete\\temp\\"))
                    {
                        string destFile = Path.Combine(dest, Path.GetFileName(file));
                        if (!File.Exists(destFile))
                        {
                            File.Move(file, destFile);
                        }
                    }

                    new XDocument(
                        new XElement("Mod",
                            new XAttribute("Name", modName_Text.Text),
                            new XElement("Author", modAuthor_Text.Text),
                            new XElement("Description", modDescription_Text.Text)
                        )
                    )
                    .Save(subdir + "modinfo.xml");

                    var lines = File.ReadAllLines(subdir + "modinfo.xml");
                    File.WriteAllLines(subdir + "modinfo.xml", lines.Skip(1).ToArray());
                }
                else
                {
                    MessageBox.Show("There is already a mod with the same name, please choose another name.", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Enabled = true;
                    return; //Don't go any further if the directory does exist.
                }



                this.Enabled = true;
            }
            catch (Exception b)
            {
                MessageBox.Show(b.ToString(), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Enabled = true;
            }
        }
    }
}
