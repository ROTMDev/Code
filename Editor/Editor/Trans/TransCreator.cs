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
namespace Editor.Trans
{
    public partial class TransCreator : Form
    {
        Laguage language = new Laguage();
        LanTrans translation = new LanTrans();
        Dictionary<string, LanTrans> tanslations = new Dictionary<string, LanTrans>();
        public TransCreator()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.language = new Laguage(textBox1.Text);
            comboBox2.Items.Add(textBox1.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            translation.addTrans(textBox2.Text, textBox3.Text);
            if (this.translation.name.LanguageName == "")
            {
                translation.name.LanguageName = textBox1.Text;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!Directory.Exists(Environment.CurrentDirectory + @"\Editor\Translation\"))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + @"\Editor\Translation\");
            }
            BinaryWriter wr = new BinaryWriter(File.Open(Environment.CurrentDirectory + @"\Editor\Translation\" + textBox1.Text + ".lan",FileMode.Create));
            this.translation.write(wr);
        }

        private void TransCreator_Load(object sender, EventArgs e)
        {
            string[] lanfiles = Directory.GetFiles(Environment.CurrentDirectory + @"\Editor\Translation\", "*.lan");
            foreach (string x in lanfiles)
            {
                BinaryReader read = new BinaryReader(File.Open(x, FileMode.Open));
                LanTrans language = new LanTrans();
                language.read(read);
                tanslations.Add(language.name.LanguageName, language);
            }
            filllanguages();
        }
        public void filllanguages()
        {
            foreach (string x in this.tanslations.Keys)
            {
                if (x == "")
                {
                    comboBox2.Items.Add("English");
                }
                else
                {
                    comboBox2.Items.Add(x);
                }
            }
        }
    }
}
