// =Realms Engine=
// =Realms Of the Mind=
// =Programmers=
// =Mute Lovestone=
// =RaceEdi.cs=
// = 10/26/2014 =
// =Editor=
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Editor.EditorF;
using LibRealm.Base;
using LibRealm.Characters;
namespace Editor.Ra
{
    public partial class RaceEdi : Form
    {
        bool editing;
        EditMain editor = new EditMain();
        public RaceEdi(Form main, bool editing)
        {
            this.InitializeComponent();
            RaceEdi.CheckForIllegalCrossThreadCalls = false;
            this.editor = ( EditMain )main;
            this.editing = editing;
            if (editing)
            {
                this.numBox9.Text = this.editor.getid().ToString();
                Race m = this.editor.races[this.numBox9.IntValue];
                this.textBox1.Text = m.Name;
                this.textBox2.Text = m.BodyPath;
                this.numBox1.Text = m.BaseStats.Strength.ToString();
                this.numBox2.Text = m.BaseStats.Intelligence.ToString();
                this.numBox3.Text = m.BaseStats.Dextarity.ToString();
                this.numBox4.Text = m.BaseStats.Wisdom.ToString();
                this.numBox5.Text = m.BaseStats.Agility.ToString();
                this.numBox6.Text = m.BaseStats.Endure.ToString();
                this.numBox7.Text = m.BaseStats.Knoledge.ToString();
                this.numBox8.Text = m.BaseStats.Perception.ToString();
            }
            else
            { this.numBox9.Text = (this.editor.races.Count + 1).ToString(); }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Stats NewStats = new Stats();
            Race newRace = new Race();
            NewStats.Agility = this.numBox5.IntValue;
            NewStats.Dextarity = this.numBox3.IntValue;
            NewStats.Endure = this.numBox6.IntValue;
            NewStats.Intelligence = this.numBox2.IntValue;
            NewStats.Knoledge = this.numBox7.IntValue;
            NewStats.Perception = this.numBox8.IntValue;
            NewStats.Strength = this.numBox1.IntValue;
            NewStats.Wisdom = this.numBox4.IntValue;
            newRace = new Race(NewStats, this.textBox1.Text, this.numBox9.IntValue, this.textBox2.Text);

            if (this.editing)
            { this.editor.races.Remove(this.numBox9.IntValue); }
            this.editor.races.Add(this.numBox9.IntValue, newRace);
            this.editor.edits = Editing.True;
            this.editor.addRaces();
            this.Close();
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        { this.Close(); }
    }
}