using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ROTM_MU002.Windows;
using System.IO;
namespace Editor.Options
{
    public partial class OptMenu : Form
    {
        LanTrans language = new LanTrans();
        Laguage lan = new Laguage();
        Dictionary<string, LanTrans> languages = new Dictionary<string, LanTrans>();
        public OptMenu()
        {
            InitializeComponent();
        }

        private void OptMenu_Load(object sender, EventArgs e)
        {
            string[] lanfiles = Directory.GetFiles(Environment.CurrentDirectory + @"\Editor\Translation\", "*.lan");
            foreach (string x in lanfiles)
            {
                BinaryReader read = new BinaryReader(File.Open(x, FileMode.Open));
                LanTrans language = new LanTrans();
                language.read(read);
                languages.Add(language.name.LanguageName, language);
            }
            filllanguages();
        }
        public void filllanguages()
        {
            foreach (string x in this.languages.Keys)
            {
                if (x == "")
                {
                    comboBox1.Items.Add("English");
                }
                else
                {
                    comboBox1.Items.Add(x);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BinaryWriter wri = new BinaryWriter(File.Open(Environment.CurrentDirectory + @"\opt.op", FileMode.Create));
            this.lan.Write(wri);
            wri.Close();
            this.Close();
        }
    }
}
