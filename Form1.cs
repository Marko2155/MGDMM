using System.IO;
using System.Diagnostics;
using Reloaded.Injector;
using System.Text;


namespace MGDMM
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }
        public string version = "0.1.1\n";

        static readonly HttpClient client = new HttpClient();
        List<string> config = new List<string>();

        public void ConvertArrayToMSML(string[] array, string msmlname)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (!File.Exists(msmlname + ".msml"))
            {
                FileStream stream = File.Create(msmlname + ".msml");
                stream.Dispose();
            } else
            {
                File.Delete(msmlname + ".msml");
                FileStream stream = File.Create(msmlname + ".msml");
                stream.Dispose();
            }
                foreach (string item in array)
                {
                    stringBuilder.Append(item + ",");
                }
            File.WriteAllText(msmlname + ".msml", stringBuilder.ToString());
        }

        public string[] ConvertMSMLToArray(string msmlname)
        {
            if (!File.Exists(msmlname + ".msml"))
            {
                FileStream stream = File.Create(msmlname + ".msml");
                stream.Dispose();
            }
            string msml = File.ReadAllText(msmlname + ".msml");
            return msml.Split(",");
        }

        public async Task CheckForUpdates()
        {
            using HttpResponseMessage httpResponse = await client.GetAsync("https://raw.githubusercontent.com/Marko2155/MGDMM/refs/heads/main/currentVersion.txt?time=");
            httpResponse.EnsureSuccessStatusCode();
            httpResponse.Headers.CacheControl.NoCache = true;
            string responseBody = await httpResponse.Content.ReadAsStringAsync();
            Debug.WriteLine(responseBody);
            Debug.WriteLine(responseBody == version);
            if (!httpResponse.IsSuccessStatusCode)
            {
                MessageBox.Show("Error occured while checking for updates!", "Marko's Geometry Dash Mod Manager");
            } else
            {
                if (responseBody.ToString() != version.ToString())
                {
                    DialogResult updateBox = MessageBox.Show("New Update!\nv" + responseBody + "\nClick OK to go to the latest update.", "Marko's Geometry Dash Mod Manager");
                    if (updateBox == DialogResult.OK)
                    {
                        ProcessStartInfo updateProc = new ProcessStartInfo();
                        updateProc.UseShellExecute = true;
                        updateProc.FileName = "https://github.com/Marko2155/MGDMM/releases/tag/v" + responseBody;
                        updateProc.CreateNoWindow = true;
                        Process.Start(updateProc);
                    }
                }
            }
        }


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
                                config.Add(file);
                                ConvertArrayToMSML(config.ToArray(), "config");
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
                            config.Add(openFileDialog1.FileName);
                            ConvertArrayToMSML(config.ToArray(), "config");
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
            label2.Text = "v" + version.ToString();
            CheckForUpdates();
            config = ConvertMSMLToArray("config").ToList<string>();
            foreach (string item in config)
            {
                listBox1.Items.Add((string)item);
            }
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                listBox1.Items.Remove("");
            }
        }
    }
}
