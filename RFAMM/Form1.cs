using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace RFAMM
{
    public partial class Form1 : Form
    {

        public Form1()
        {


            InitializeComponent();

            modList.ExpandAll();

            try
            {
                if (new FileInfo(Environment.CurrentDirectory + "\\donotdelete\\path.txt").Length == 0)
                {
                    return;
                }
                else
                {
                    game_DirectoryText.Text = File.ReadAllText(Environment.CurrentDirectory + "\\donotdelete\\path.txt");
                    DetectMods();
                }
            }
            catch (Exception a)
            {
                MessageBox.Show(a.ToString(), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //// ACTIVATE/DEACTIVATE
            //XDocument xdoc = XDocument.Load(Environment.CurrentDirectory + "\\donotdelete\\info.xml");
            //// Descendants inside Mod 
            //var elements = from r in xdoc.Descendants("program")
            //               select new
            //               {
            //                   gamepath = r.Element("gamepath").Value
            //               };

            //var elements2 = from r in xdoc.Descendants("program").Descendants("activated")
            //               select new
            //               {
            //                   modname = r.Element("mod").Attribute("Name").Value ?? "Not Set",
            //                   truefalse = r.Element("mod").Value
            //               };

            //foreach (var r in elements)
            //{
            //    // Set mod info text for viewing by the user.
            //    //mod_NameText.Text = r.ModName;
            //    //mod_AuthorText.Text = r.Author;
            //    //mod_DescriptionText.Text = r.Description;
            //}
        }

        private static void OnChanged(object source, FileSystemEventArgs e) =>
        // Specify what is done when a file is changed, created, or deleted.
        Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");

        public void wait(int milliseconds)
        {
            System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer();
            if (milliseconds == 0 || milliseconds < 0) return;
            //Console.WriteLine("start wait timer");
            timer1.Interval = milliseconds;
            timer1.Enabled = true;
            timer1.Start();
            timer1.Tick += (s, e) =>
            {
                timer1.Enabled = false;
                timer1.Stop();
                //Console.WriteLine("stop wait timer");
            };
            while (timer1.Enabled)
            {
                Application.DoEvents();
            }
        }

        public void DetectMods()
        {
            try
            {
                modList.Nodes["Node0"].Nodes.Clear();

                string gamePath = game_DirectoryText.Text + "\\mods";

                // Get directory info
                DirectoryInfo dInfo = new DirectoryInfo(gamePath);
                // Get the XML files from the start directory and sub-directories
                FileInfo[] fInfo = dInfo.GetFiles("*.xml", SearchOption.AllDirectories);

                foreach (FileInfo file in fInfo)
                {
                    // Load the XML document that was found
                    XDocument xdoc = XDocument.Load(file.FullName);
                    

                    var elements = from r in xdoc.Descendants("Mod")
                                   select new
                                   {
                                       ModName = r.Attribute("Name").Value ?? "Not Set"
                                   };

                    foreach (var r in elements)
                    {

                        modList.Nodes["Node0"].Nodes.Add(r.ModName);
                    }

                    
                }

                try
                {
                    if (new FileInfo(Environment.CurrentDirectory + "\\donotdelete\\path.txt").Length == 0)
                    {
                        File.WriteAllText(Environment.CurrentDirectory + "\\donotdelete\\path.txt", game_DirectoryText.Text);
                    }
                    else
                    {
                        // there is something in it
                    }
                }
                catch (Exception a)
                {
                    MessageBox.Show(a.ToString(), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
               

            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("Your RF:A folder does not include a \"mods\" folder!", "DirectoryNotFoundException", MessageBoxButtons.OK, MessageBoxIcon.Error);
                game_DirectoryText.Text = "";
            }
            catch (Exception a)
            {
                MessageBox.Show("The process failed: " + a.ToString(), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                game_DirectoryText.Text = "";
            }
        }

        private async void button1_ClickAsync(object sender, EventArgs e)
        {
            TreeNode tn = modList.SelectedNode;

            if (tn == null )
            {
                MessageBox.Show("You don't have a mod selected yet!", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (modList.SelectedNode.Nodes.Count == 0)
                {
                    this.Enabled = false;

                    string text;

                    text = modList.Nodes.IndexOf(modList.SelectedNode).ToString();

                    Console.WriteLine(text);

                    string selecteditem = text;

                    try
                    {
                        try
                        {
                            if (!File.Exists(game_DirectoryText.Text + "\\build\\pc\\cache\\misc.vpp_pc.BACKUP"))
                            {
                                File.Copy(game_DirectoryText.Text + "\\build\\pc\\cache\\misc.vpp_pc", game_DirectoryText.Text + "\\build\\pc\\cache\\misc.vpp_pc.BACKUP", true);
                            }
                            else if (File.Exists(game_DirectoryText.Text + "\\build\\pc\\cache\\misc.vpp_pc.BACKUP"))
                            {
                                Console.WriteLine("Backup file already exists");
                            }
                        }
                        catch (Exception a)
                        {
                            MessageBox.Show(a.ToString(), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        try
                        {
                            if (!File.Exists(game_DirectoryText.Text + "\\build\\pc\\cache\\table.vpp_pc"))
                            {
                                Console.WriteLine("Table file already renamed or removed");
                            }
                            else if (File.Exists(game_DirectoryText.Text + "\\build\\pc\\cache\\table.vpp_pc"))
                            {
                                File.Move(game_DirectoryText.Text + "\\build\\pc\\cache\\table.vpp_pc", game_DirectoryText.Text + "\\build\\pc\\cache\\table.vpp_pc.RENAMED");
                            }
                        }
                        catch (Exception a)
                        {
                            MessageBox.Show(a.ToString(), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        UnpackVPP();

                        //string selecteditem = modList.(modList.SelectedItem);
                        string path = game_DirectoryText.Text + "\\build\\pc\\cache\\_TEMP\\misc.vpp_pc\\";
                        string dirScanner = game_DirectoryText.Text + "\\mods";

                        // Scan for XML files located in the user specified game directory.
                        string[] allFiles = Directory.GetFiles(dirScanner, "*.xml", SearchOption.AllDirectories);

                        foreach (string file in allFiles)
                        {
                            // Read XML files found in the game directory.
                            string[] lines = File.ReadAllLines(file);

                            // Find and search XML files with the same name of the listbox item.
                            string firstOccurrence = lines.FirstOrDefault(l => l.Contains(selecteditem));
                            if (firstOccurrence != null)
                            {
                                string parent_directory_path;
                                parent_directory_path = Path.GetDirectoryName(file);

                                string[] files = Directory.GetFiles(parent_directory_path + "\\misc.vpp_pc\\");
                                try
                                {
                                    foreach (string file2 in files)
                                    {
                                        // Delete files with names from program folder
                                        string filenames = Path.GetFileName(file2);
                                        string filestocopy = Path.Combine(parent_directory_path + "\\misc.vpp_pc\\", filenames);
                                        DirectoryInfo d = new DirectoryInfo(parent_directory_path + "\\misc.vpp_pc\\");
                                        File.Delete(Path.Combine(path, filenames));
                                    }

                                    DirectoryInfo d2 = new DirectoryInfo(parent_directory_path + "\\misc.vpp_pc\\");
                                    string d4 = parent_directory_path + "\\misc.vpp_pc\\";
                                    string d3 = game_DirectoryText.Text + "\\build\\pc\\cache\\_TEMP\\misc.vpp_pc\\";
                                    Console.WriteLine(d3);
                                    Console.WriteLine(d2);

                                    foreach (FileInfo files2 in d2.GetFiles("*.*", SearchOption.AllDirectories))
                                    {
                                        files2.CopyTo(Path.Combine(d3, files2.Name));
                                    }
                                }
                                catch (Exception a)
                                {
                                    MessageBox.Show(a.ToString(), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }

                            }
                        }

                        //Delete VPP so that the VPP can be packed without an error.
                        File.Delete(game_DirectoryText.Text + "\\build\\pc\\cache\\misc.vpp_pc");

                        while (!Directory.Exists(game_DirectoryText.Text + "\\build\\pc\\cache\\_TEMP\\"))
                        {
                            wait(1000);
                            Console.WriteLine("_TEMP is not created yet.");
                        }
                        while (Directory.Exists(game_DirectoryText.Text + "\\build\\pc\\cache\\_TEMP\\"))
                        {
                            string[] Args2 = { game_DirectoryText.Text + "\\build\\pc\\cache\\misc.vpp_pc", game_DirectoryText.Text + "\\build\\pc\\cache\\_TEMP\\misc.vpp_pc\\" };
                            int UnpackExitCode2 = new Gibbed.Volition.Pack.VPP.Packer<Gibbed.Volition.FileFormats.PackageFileV6, Gibbed.Volition.FileFormats.Package.Entry>().Main(Args2);
                            break;
                        }

                        Directory.Delete(game_DirectoryText.Text + "\\build\\pc\\cache\\_TEMP\\", true);

                        this.Enabled = true;

                        MessageBox.Show("Mod activated.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (FileNotFoundException a)
                    {
                        MessageBox.Show("The misc.vpp_pc file could not be found in your build\\pc\\cache directory!", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Enabled = true;
                    }
                    catch (Exception b)
                    {
                        MessageBox.Show(b.ToString(), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Enabled = true;
                    }
                }
                else
                {
                    MessageBox.Show("You don't have a mod selected yet!", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
            }
            

        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.ProcessStartInfo proc = new System.Diagnostics.ProcessStartInfo();
            proc.FileName = "CMD.exe";
            proc.Arguments = "/c start steam://rungameid/55110";
            System.Diagnostics.Process.Start(proc);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(File.Exists(game_DirectoryText.Text + "\\build\\pc\\cache\\misc.vpp_pc.BACKUP"))
            {
                try
                {
                    File.Delete(game_DirectoryText.Text + "\\build\\pc\\cache\\misc.vpp_pc");
                    File.Move(game_DirectoryText.Text + "\\build\\pc\\cache\\misc.vpp_pc.BACKUP", game_DirectoryText.Text + "\\build\\pc\\cache\\misc.vpp_pc");

                    try
                    {
                        if (!File.Exists(game_DirectoryText.Text + "\\build\\pc\\cache\\table.vpp_pc"))
                        {
                            Console.WriteLine("Table file already renamed or removed");
                        }
                        else if (File.Exists(game_DirectoryText.Text + "\\build\\pc\\cache\\table.vpp_pc"))
                        {
                            File.Move(game_DirectoryText.Text + "\\build\\pc\\cache\\table.vpp_pc", game_DirectoryText.Text + "\\build\\pc\\cache\\table.vpp_pc.RENAMED");
                        }
                    }
                    catch (Exception a)
                    {
                        MessageBox.Show(a.ToString(), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    File.Move(game_DirectoryText.Text + "\\build\\pc\\cache\\table.vpp_pc.RENAMED", game_DirectoryText.Text + "\\build\\pc\\cache\\table.vpp_pc");
                    MessageBox.Show("Your files have been restored! You can now run RF:A without mods.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception b)
                {
                    MessageBox.Show(b.ToString(), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("A backup file hasn't been created in your directory yet! This may be because you haven't activated any mods yet.", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        public void UnpackVPP()
        {
            string[] Args = { game_DirectoryText.Text + "\\build\\pc\\cache\\misc.vpp_pc", game_DirectoryText.Text + "\\build\\pc\\cache\\_TEMP\\misc.vpp_pc" };
            int UnpackExitCode = new Gibbed.Volition.Pack.VPP.Unpacker<Gibbed.Volition.FileFormats.PackageFileV6>().Main(Args);
        }

        public void PackVPP()
        {
            string[] Args2 = { game_DirectoryText.Text + "\\build\\pc\\cache\\misc.vpp_pc", game_DirectoryText.Text + "\\build\\pc\\cache\\_TEMP\\misc.vpp" };
            int UnpackExitCode2 = new Gibbed.Volition.Pack.VPP.Packer<Gibbed.Volition.FileFormats.PackageFileV3, Gibbed.Volition.FileFormats.Package.Entry>().Main(Args2);
        }

        private void modList_SelectedIndexChanged(object sender, EventArgs e)
        {
            


        }

        private void mod_AuthorText_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void treeView1_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = false;
            folderDlg.Description = "Browse for your RF:A folder! Do NOT select a directory that isn't your game folder.";
            // Show the FolderBrowserDialog.  
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                game_DirectoryText.Text = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
                DetectMods();
            }
        }

        private void modList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (modList.SelectedNode.Nodes.Count == 0)
                {
                    string text = "";

                    text = modList.SelectedNode.Text;

                    Console.WriteLine(text + "t");

                    string selecteditem = text;

                    string dirScanner = game_DirectoryText.Text + "\\mods";

                    // Scan for XML files located in the user specified game directory.
                    string[] allFiles = Directory.GetFiles(dirScanner, "*.xml", SearchOption.AllDirectories);

                    foreach (string file in allFiles)
                    {
                        // Read XML files found in the game directory.
                        string[] lines = File.ReadAllLines(file);

                        // Find and search XML files with the same name of the listbox item.
                        string firstOccurrence = lines.FirstOrDefault(l => l.Contains(selecteditem));
                        if (firstOccurrence != null)
                        {
                            XDocument xdoc = XDocument.Load(file);

                            // Descendants inside Mod 
                            var elements = from r in xdoc.Descendants("Mod")
                                           select new
                                           {
                                               ModName = r.Attribute("Name").Value ?? "Not Set",
                                               Author = r.Element("Author").Value,
                                               Description = r.Element("Description").Value
                                           };

                            foreach (var r in elements)
                            {
                                // Set mod info text for viewing by the user.
                                mod_NameText.Text = r.ModName;
                                mod_AuthorText.Text = r.Author;
                                mod_DescriptionText.Text = r.Description;
                            }
                        }
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception b)
            {
                MessageBox.Show(b.ToString(), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Enabled = true;
            }
        }

        private void modList_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
        }

        void button5_Click(object sender, EventArgs e)
        {

            Form fc = Application.OpenForms["newMod"];
            if (fc != null)
            {
                Console.WriteLine("newMod is already opened, not showing.");
                return;
            }
            else
            {
                newMod nW = new newMod();
                nW.Show();
                Console.WriteLine("newMod is not opened, showing.");
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
