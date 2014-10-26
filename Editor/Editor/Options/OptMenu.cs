// =Realms Engine=
// =Realms Of the Mind=
// =Programmers=
// =Mute Lovestone=
// =OptMenu.cs=
// = 10/26/2014 =
// =Editor=
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ROTM_MU002.Windows;
namespace Editor.Options
{
    public partial class OptMenu : Form
    {
        Laguage lan = new Laguage();
        LanTrans language = new LanTrans();
        Dictionary<string, LanTrans> languages = new Dictionary<string, LanTrans>();
        public OptMenu()
        { this.InitializeComponent(); }
        public void filllanguages()
        {
            foreach (string x in this.languages.Keys)
            {
                if (x == "")
                { this.comboBox1.Items.Add("English"); }
                else
                { this.comboBox1.Items.Add(x); }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            BinaryWriter wri = new BinaryWriter(File.Open(string.Format("{0}{1}", Environment.CurrentDirectory, @"\opt.op"), FileMode.Create));
            this.lan.Write(wri);
            wri.Close();
            this.Close();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        { this.lan.LanguageName = this.comboBox1.SelectedItem.ToString(); }
        private void OptMenu_Load(object sender, EventArgs e)
        {
            string[] lanfiles = Directory.GetFiles(string.Format("{0}{1}", Environment.CurrentDirectory, @"\Editor\Translation\"), "*.lan");
            foreach (string x in lanfiles)
            {
                BinaryReader read = new BinaryReader(File.Open(x, FileMode.Open));
                LanTrans language = new LanTrans();
                language.read(read);
                this.languages.Add(language.name.LanguageName, language);
            }
            this.filllanguages();
        }
    }
}
