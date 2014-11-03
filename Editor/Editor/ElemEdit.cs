// =Realms Engine=
// =Realms Of the Mind=
// =Programmers=
// =Mute Lovestone=
// =ElemEdit.cs=
// = 11/3/2014 =
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
namespace Editor
{
    public partial class ElemEdit : Form
    {
        bool edit = false;
        Elements x = new Elements();
        EditMain xz;
        public ElemEdit(Form editor, Elements xp)
        {
            this.InitializeComponent();
            this.x = xp;
            this.xz = ( EditMain )editor;
            this.textBox1.Text = this.x.Name;
            this.label2.Text = this.x.ID.ToString();
            this.edit = true;
        }
        public ElemEdit(Form editor)
        {
            this.InitializeComponent();
            this.xz = ( EditMain )editor;
            this.label2.Text = (this.xz.elems.Count + 1).ToString();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.x = new Elements(this.textBox1.Text,Convert.ToInt32(this.label2.Text));
            if (this.xz.elems.Keys.Contains(this.x.ID))
            { this.xz.elems.Remove(this.x.ID); }
            this.xz.elems.Add(this.x.ID, this.x);
            this.xz.AddElems();
            this.xz.edits = Editing.True;
            this.Close();
        }
        private void ElemEdit_Load(object sender, EventArgs e)
        { ElemEdit.CheckForIllegalCrossThreadCalls = false; }
    }
}