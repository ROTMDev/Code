// =Realms Engine=
// =Realms Of the Mind=
// =Programmers=
// =Mute Lovestone=
// =EditMain.cs=
// = 11/3/2014 =
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
using Editor;
using Editor.Options;
using Editor.Ra;
using Editor.Trans;
using LibRealm;
using LibRealm.Base;
using LibRealm.Characters;
using ROTM_MU002.Windows;
using ROTM_MU002.Windows.Builder;
namespace Editor.EditorF
{
    public enum Editing
    {
        True,
        False
    }
    public partial class EditMain : Form
    {
        #region values
        public Editing edits = Editing.False;
        public Dictionary<int, Race> races = new Dictionary<int, Race>();
        public Dictionary<int, Elements> elems = new Dictionary<int, Elements>();
        public LanTrans trns = new LanTrans();
        public DataManager Man = new DataManager();
        #endregion
        #region premade(Need Edited)
        private int childFormNumber = 0;
        public EditMain()
        { this.InitializeComponent(); }
        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        { this.Close(); }
        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        { this.toolStrip.Visible = this.toolBarToolStripMenuItem.Checked; }
        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        { this.statusStrip.Visible = this.statusBarToolStripMenuItem.Checked; }
        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        { this.LayoutMdi(MdiLayout.Cascade); }
        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        { this.LayoutMdi(MdiLayout.TileVertical); }
        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        { this.LayoutMdi(MdiLayout.TileHorizontal); }
        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        { this.LayoutMdi(MdiLayout.ArrangeIcons); }
        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in this.MdiChildren)
            { childForm.Close(); }
        }
        #endregion
        #region Custom events
        private void EditMain_Load(object sender, EventArgs e)
        {
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            EditMain.CheckForIllegalCrossThreadCalls = false;
            CheckForIllegalCrossThreadCalls = false;
            ListView.CheckForIllegalCrossThreadCalls = false;
            this.backgroundWorker1.RunWorkerAsync();
            this.setLanguage();
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            ToolStrip.CheckForIllegalCrossThreadCalls = false;
            try
            {
                Race x = new Race();
                using (BinaryReader re = new BinaryReader(File.Open(string.Format("{0}{1}", Environment.CurrentDirectory, @"\Data\Races\Race.ra"), FileMode.Open)))
                {
                    int p = re.ReadInt32();
                    this.toolStripProgressBar1.Maximum = p;
                    this.toolStripStatusLabel2.Text = "Races";
                    ListView.CheckForIllegalCrossThreadCalls = false;
                    for (int i = 0; i < p; i++)
                    {
                        x.Read(re);
                        this.races.Add(x.RaceID, x);
                        this.backgroundWorker1.ReportProgress(i + 1);
                    }
                }
            }
            catch
            { }
            
            try
            {
                using (BinaryReader re = new BinaryReader(File.Open(string.Format("{0}{1}", Environment.CurrentDirectory, @"\Data\Elems\Elems.elm"), FileMode.Open)))
                {
                    
                    Elements x2 = new Elements();
                    int p = re.ReadInt32();
                    this.toolStripProgressBar1.Maximum = this.toolStripProgressBar1.Maximum +p;
                    this.toolStripStatusLabel2.Text = "Elements";
                    for (int i = 0; i < p; i++)
                    {
                        x2.read(re);
                        this.elems.Add(x2.ID, x2);
                        int xh = this.toolStripProgressBar1.Value + 1;
                        this.backgroundWorker1.ReportProgress(xh);
                    }
                }
            }
            catch
            { }
        }
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        { this.toolStripProgressBar1.Value = e.ProgressPercentage; MessageBox.Show(e.ProgressPercentage.ToString()+toolStripProgressBar1.Value.ToString()); }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        { this.toolStripStatusLabel2.Text = "Complete"; }
        private void treeView1_DoubleClick(object sender, EventArgs e)
        { }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        { }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        { this.backgroundWorker2.RunWorkerAsync(); }
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            this.diCheck(string.Format("{0}{1}", Environment.CurrentDirectory, @"\Data\Races\"));
            using (BinaryWriter wr = new BinaryWriter(File.Open(string.Format("{0}{1}", Environment.CurrentDirectory, @"\Data\Races\Race.ra"), FileMode.Create)))
            {
                this.toolStripProgressBar1.Maximum = this.races.Keys.Count;
                this.toolStripStatusLabel4.Text = "Races";
                wr.Write(this.races.Keys.Count);
                foreach (int x in this.races.Keys)
                {
                    Race wriRace = this.races[x];
                    wriRace.Write(wr);
                    this.backgroundWorker2.ReportProgress(x + 1);
                }
            }
            this.diCheck(string.Format("{0}{1}", Environment.CurrentDirectory, @"\Data\Elems\"));
            using (BinaryWriter wr = new BinaryWriter(File.Open(string.Format("{0}{1}", Environment.CurrentDirectory, @"\Data\Elems\Elems.elm"), FileMode.Create)))
            {
                this.toolStripProgressBar1.Maximum += this.elems.Keys.Count;
                this.toolStripStatusLabel4.Text = "Elements";
                wr.Write(this.elems.Keys.Count);
                foreach (int x in this.elems.Keys)
                {
                    Elements wriRace = this.elems[x];
                    wriRace.write(wr);
                    this.backgroundWorker2.ReportProgress(this.toolStripProgressBar1.Value + 1);
                }
            }
        }
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            #region Races
            if (this.treeView1.SelectedNode.Text == this.TransWord("Races"))
            {
                this.listView1.Items.Clear();
                this.listView1.Columns.Clear();
                ColumnHeader head = new ColumnHeader();
                head.Text = this.TransWord("Name");
                this.listView1.Columns.Add(head);
                head = new ColumnHeader();
                head.Text = this.TransWord("ID");
                this.listView1.Columns.Add(head);
                head = new ColumnHeader();
                head.Text = this.TransWord("BodyFile");
                this.listView1.Columns.Add(head);
                head = new ColumnHeader();
                head.Text = this.TransWord("Stat-Str");
                this.listView1.Columns.Add(head);
                head = new ColumnHeader();
                head.Text = this.TransWord("Stat-Int");
                this.listView1.Columns.Add(head);
                head = new ColumnHeader();
                head.Text = this.TransWord("Stat-Wis");
                this.listView1.Columns.Add(head);
                head = new ColumnHeader();
                head.Text = this.TransWord("stat-Dex");
                this.listView1.Columns.Add(head);
                head = new ColumnHeader();
                head.Text = this.TransWord("Stat-Agi");
                this.listView1.Columns.Add(head);
                head = new ColumnHeader();
                head.Text = this.TransWord("Stat-Endure");
                this.listView1.Columns.Add(head);
                head = new ColumnHeader();
                head.Text = this.TransWord("Stat-Kno");
                this.listView1.Columns.Add(head);
                head = new ColumnHeader();
                head.Text = this.TransWord("Stat-Per");
                this.listView1.Columns.Add(head);
                this.listView1.ContextMenuStrip = this.RaceMen;
                this.addRaces();
            }
            #endregion
            #region elem
            if (this.treeView1.SelectedNode.Text == "Elements")
            {
                this.listView1.Items.Clear();
                this.listView1.Columns.Clear();
                ColumnHeader head = new ColumnHeader();
                head.Text = this.TransWord("Name");
                this.listView1.Columns.Add(head);
                head = new ColumnHeader();
                head.Text = this.TransWord("ID");
                this.listView1.Columns.Add(head);
                this.listView1.ContextMenuStrip = this.ElemCon;
                this.AddElems();
            }
            #endregion
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RaceEdi nx = new RaceEdi(this, true);
            nx.Show(this);
        }
        private void newToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RaceEdi nx = new RaceEdi(this, false);
            nx.Show(this);
        }
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptMenu opts = new OptMenu();opts.ShowDialog();
        }
        private void EditMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.edits == Editing.True)
            { this.backgroundWorker2.RunWorkerAsync(); }
        }
        private void editTranslationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TransCreator creator = new TransCreator();
            creator.Show(this);
        }
        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        { }
        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        { this.toolStripStatusLabel2.Text = "Complete"; }
        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        { this.toolStripProgressBar1.Value = e.ProgressPercentage; }
        #endregion
        #region custom logic
        public void addRaces()
        {
            this.listView1.Items.Clear();
            foreach (int x in this.races.Keys)
            {
                Race p = this.races[x];
                List<string> xz = new List<string>();
                xz.Add(p.Name);
                xz.Add(p.RaceID.ToString());
                xz.Add(p.BodyPath);
                xz.Add(p.BaseStats.Strength.ToString());
                xz.Add(p.BaseStats.Intelligence.ToString());
                xz.Add(p.BaseStats.Wisdom.ToString());
                xz.Add(p.BaseStats.Dextarity.ToString());
                xz.Add(p.BaseStats.Agility.ToString());
                xz.Add(p.BaseStats.Endure.ToString());
                xz.Add(p.BaseStats.Knoledge.ToString());
                xz.Add(p.BaseStats.Perception.ToString());
                ListViewItem xd = new ListViewItem(xz.ToArray());
                this.listView1.Items.Add(xd);
            }
        }
        public Int32 getid()
        {
            int intselectedindex = this.listView1.SelectedIndices[0];
            return intselectedindex + 1;
        }
        public void AddElems()
        {
            this.listView1.Items.Clear();
            foreach (int x in this.elems.Keys)
            {
                Elements xp = this.elems[x];
                List<string> tolist = new List<string>();
                tolist.Add(xp.Name);
                tolist.Add(xp.ID.ToString());
                ListViewItem toitem = new ListViewItem(tolist.ToArray());
                this.listView1.Items.Add(toitem);
            }
        }
        public void setLanguage()
        {
            try
            {
                BinaryReader wr = new BinaryReader(File.Open(string.Format("{0}{1}", Environment.CurrentDirectory, @"\opt.op"), FileMode.Open));
                Laguage xp = new Laguage();
                xp.Read(wr);
                wr.Close();
                if (xp.LanguageName == "")
                { xp.LanguageName = "English"; }
                wr = new BinaryReader(File.Open(string.Format("{0}{1}{2}.lan", Environment.CurrentDirectory, @"\Editor\Translation\", xp.LanguageName), FileMode.Open));
                this.trns.read(wr);
                wr.Close();
            }
            catch
            { MessageBox.Show("Please Select your Language,go Tools>Options then choose your language and save"); }
            this.translate();
        }
        public void translate()
        {
            #region controls
            foreach (Control x in this.Controls)
            {
                try
                { x.Text = this.trns.tr(x.Text); }
                catch
                { x.Text = x.Text; }
                foreach (Control x2 in x.Controls)
                {
                    try
                    { x2.Text = this.trns.tr(x2.Text); }
                    catch
                    { x2.Text = x2.Text; }
                    foreach (Control x3 in x2.Controls)
                    {
                        try
                        { x3.Text = this.trns.tr(x3.Text); }
                        catch
                        { x3.Text = x3.Text; }
                    }
                }
            }
            #endregion
            #region treenodes
            foreach (TreeNode x in this.treeView1.Nodes)
            {
                try
                { x.Text = this.trns.tr(x.Text); }
                catch
                { x.Text = x.Text; }
                foreach (TreeNode x2 in x.Nodes)
                {
                    try
                    { x2.Text = this.trns.tr(x2.Text); }
                    catch
                    { x2.Text = x2.Text; }
                    foreach (TreeNode x3 in x2.Nodes)
                    {
                        try
                        { x3.Text = this.trns.tr(x3.Text); }
                        catch
                        { x3.Text = x3.Text; }
                    }
                }
            }
            #endregion
            #region menus
            foreach (ToolStripMenuItem x in this.menuStrip.Items)
            {
                try
                { x.Text = this.trns.tr(x.Text); }
                catch
                { x.Text = x.Text; }
                foreach (ToolStripMenuItem x2 in x.DropDownItems)
                {
                    try
                    { x2.Text = this.trns.tr(x2.Text); }
                    catch
                    { x2.Text = x2.Text; }
                    foreach (ToolStripMenuItem x3 in x2.DropDownItems)
                    {
                        try
                        { x3.Text = this.trns.tr(x3.Text); }
                        catch
                        { x3.Text = x3.Text; }
                    }
                }
            }
            foreach (ToolStripItem x in this.statusStrip.Items)
            {
                try
                { x.Text = this.trns.tr(x.Text); }
                catch
                { x.Text = x.Text; }
            }
            #endregion
        }
        public string TransWord(string word)
        {
            try
            { return this.trns.tr(word); }
            catch
            { return word; }
        }
        public void diCheck(string s)
        {
            if (!Directory.Exists(s))
            { Directory.CreateDirectory(s); }
        }
        #endregion
        #region unfinished
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ElemEdit xp = new ElemEdit(this); xp.Show();
        }
        private void editToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ElemEdit xp = new ElemEdit(this,this.elems[this.getid()]); xp.Show();
        }
        #endregion
        private void convertModelsToolStripMenuItem_Click(object sender, EventArgs e)
        { this.backgroundWorker4.RunWorkerAsync(); }
        private void backgroundWorker4_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] modelfiles = Directory.GetFiles(string.Format("{0}{1}", Environment.CurrentDirectory, @"\Data\Models\UnCon\"), "*.fbx");
            string outpu = string.Format("{0}{1}", Environment.CurrentDirectory, @"\Data\Models\Useable\");
            ContentBuilder tem = new ContentBuilder(outpu);
            tem.Clear();
            foreach (string modfile in modelfiles)
            { tem.Add(modfile); }
            try
            { tem.Build(); }
            catch (Exception x)
            { MessageBox.Show(x.ToString()); }
            File.Delete(string.Format("{0}{1}", Environment.CurrentDirectory, @"\Data\Models\Useable\cachefile--targetpath.txt"));
            Directory.Delete(string.Format("{0}{1}", Environment.CurrentDirectory, @"\Data\Models\Useable\obj\"), true);
            this.convertModelsToolStripMenuItem.Visible = false;
            MessageBox.Show("Finished Converting Model Files");
        }
    }
}