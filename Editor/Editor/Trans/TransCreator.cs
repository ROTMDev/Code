// =Realms Engine=
// =Realms Of the Mind=
// =Programmers=
// =Mute Lovestone=
// =TransCreator.cs=
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
namespace Editor.Trans
{
    public partial class TransCreator : Form
    {
        Laguage language = new Laguage();
        Dictionary<string, LanTrans> tanslations = new Dictionary<string, LanTrans>();
        LanTrans translation = new LanTrans();
        public TransCreator()
        { this.InitializeComponent(); }
        public void fillItmes()
        {
            LanTrans x = this.tanslations["English"];
            foreach (string xp in x.Langagetrans.Keys)
            { this.comboBox1.Items.Add(xp); }
        }
        public void filllanguages()
        {
            foreach (string x in this.tanslations.Keys)
            {
                if (x == "")
                { this.comboBox2.Items.Add("English"); }
                else
                { this.comboBox2.Items.Add(x); }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(string.Format("{0}{1}", Environment.CurrentDirectory, @"\Editor\Translation\")))
            { Directory.CreateDirectory(string.Format("{0}{1}", Environment.CurrentDirectory, @"\Editor\Translation\")); }
            BinaryWriter wr = new BinaryWriter(File.Open(string.Format("{0}{1}{2}.lan", Environment.CurrentDirectory, @"\Editor\Translation\", this.translation.name.LanguageName), FileMode.Create));
            this.translation.write(wr);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.language = new Laguage(this.textBox1.Text);
            this.comboBox2.Items.Add(this.textBox1.Text);
            this.translation = new LanTrans(this.textBox1.Text);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.translation.addTrans(this.textBox2.Text, this.textBox3.Text);
            if (this.translation.name.LanguageName == "")
            { this.translation.name.LanguageName = this.comboBox2.Items[this.comboBox2.SelectedIndex].ToString(); }
            try
            { this.tanslations.Remove(this.comboBox2.Items[this.comboBox2.SelectedIndex].ToString()); }
            catch
            { }
            this.tanslations.Add(this.comboBox2.Items[this.comboBox2.SelectedIndex].ToString(), this.translation);
            richTextBox2.Text = "";
            List<string> xp = new List<string>();
            foreach (string x in this.translation.Langagetrans.Keys)
            {
                
                xp.Add(string.Format("{0}<to>{1}",x,translation.Langagetrans[x]));
                
            }
            richTextBox2.Lines = xp.ToArray();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        { this.textBox2.Text = this.comboBox1.Items[this.comboBox1.SelectedIndex].ToString();
        richTextBox2.Text = "";
        List<string> xp = new List<string>();
        foreach (string x in this.translation.Langagetrans.Keys)
        {

            xp.Add(string.Format("{0}<to>{1}", x, translation.Langagetrans[x]));

        }
        richTextBox2.Lines = xp.ToArray();
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.fillItmes();
            this.translation = this.tanslations[this.comboBox2.SelectedItem.ToString()];
        }
        private void TransCreator_Load(object sender, EventArgs e)
        {
            string[] lanfiles = Directory.GetFiles(string.Format("{0}{1}", Environment.CurrentDirectory, @"\Editor\Translation\"), "*.lan");
            foreach (string x in lanfiles)
            {
                BinaryReader read = new BinaryReader(File.Open(x, FileMode.Open));
                LanTrans language = new LanTrans();
                language.read(read);
                this.tanslations.Add(language.name.LanguageName, language);
            }
            this.filllanguages();
        }
    }
}