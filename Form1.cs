using System.Diagnostics;
using System.IO;
using Reloaded.Injector;


namespace MGDMM
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }
        public string version = "v0.1.1";

        public void InjectDLLs(string dllName)
        {
            Process gdproc = Process.GetProcessesByName("GeometryDash")[0];
            Injector inj = new Injector(gdproc);
            inj.Inject(dllName);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            if (openFileDialog1.CheckFileExists && openFileDialog1.CheckPathExists)
            {
                if (openFileDialog1.FileNames.Length != 1)
                {
                    foreach (string file in openFileDialog1.FileNames)
                    {
                        if (file != "openFileDialog1")
                        {
                            if (!listBox1.Items.Contains(file))
                            {
                                listBox1.Items.Add(file);
                            }
                        }
                    }
                }
                else
                {
                    if (openFileDialog1.FileName != "openFileDialog1")
                    {
                        if (!listBox1.Items.Contains(openFileDialog1.FileName))
                        {
                            listBox1.Items.Add(openFileDialog1.FileName);
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process[] gd = Process.GetProcessesByName("GeometryDash");
            if (gd.Length == 0)
            {
                MessageBox.Show("Open Geometry Dash before clicking inject!", "Marko's Geometry Dash Mod Manager");
            }
            else
            {
                if (listBox1.Items.Count == 0)
                {
                    MessageBox.Show("No mods installed! Click \"Add DLLs\" to add your GD mods.", "Marko's Geometry Dash Mod Manager");
                }
                else
                {
                    foreach (string mod in listBox1.Items)
                    {
                        InjectDLLs(mod);
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "DLL|*.dll";
            label2.Text = version.ToString();
        }
    }
}
