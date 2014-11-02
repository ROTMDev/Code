using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LibRealm.Base;
using Editor.EditorF;
namespace Editor
{
    public partial class ElemEdit : Form
    {
        EditMain xz;
        Elements x = new Elements();
        bool edit = false;
        public ElemEdit(Form editor,Elements xp)
        {
            InitializeComponent();
            x = xp;
            xz =(EditMain) editor;
            textBox1.Text = x.Name;
            label2.Text = x.ID.ToString();
            edit = true;
        }
        public ElemEdit(Form editor)
        {
            InitializeComponent();
            xz = (EditMain)editor;
            label2.Text = (xz.elems.Count + 1).ToString();
        }

        private void ElemEdit_Load(object sender, EventArgs e)
        {
            ElemEdit.CheckForIllegalCrossThreadCalls = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            x=new Elements(textBox1.Text,Convert.ToInt32(label2.Text));
            if (xz.elems.Keys.Contains(x.ID))
            {
                xz.elems.Remove(x.ID);
            }
            xz.elems.Add(x.ID, x);
            xz.AddElems();
            xz.edits = Editing.True;
            this.Close();
        }
    }
}
