// =Realms Engine=
// =Realms Of the Mind=
// =Programmers=
// =Mute Lovestone=
// =EditMain.cs=
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
using Editor.Options;
using Editor.Ra;
using Editor.Trans;
using LibRealm.Base;
using LibRealm.Characters;
using ROTM_MU002.Windows;
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
            if (Directory.Exists(string.Format("{0}{1}", Environment.CurrentDirectory, @"\Data\Races\")))
            {
                Race x = new Race();
                using (BinaryReader re = new BinaryReader(File.Open(string.Format("{0}{1}", Environment.CurrentDirectory, @"\Data\Races\Races.ra"), FileMode.Open)))
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
            else
            { MessageBox.Show("Races File does not exits, or you have an error, please check file location, or reinstall Game"); }
        }
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        { this.toolStripProgressBar1.Value = e.ProgressPercentage; }
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
            if (!Directory.Exists(string.Format("{0}{1}", Environment.CurrentDirectory, @"\Data\Races\")))
            { Directory.CreateDirectory(string.Format("{0}{1}", Environment.CurrentDirectory, @"\Data\Races\")); }
            BinaryWriter wr = new BinaryWriter(File.Open(string.Format("{0}{1}", Environment.CurrentDirectory, @"\Data\Races\Races.ra"), FileMode.Create));
            wr.Write(this.races.Keys.Count);
            foreach (int x in this.races.Keys)
            {
                Race wriRace = this.races[x];
                wriRace.Write(wr);
            }
            wr.Close();
        }
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            #region Races
            if (this.treeView1.SelectedNode.Text == TransWord("Races"))
            {
                this.listView1.Items.Clear();
                this.listView1.Columns.Clear();
                ColumnHeader head = new ColumnHeader();
                head.Text =TransWord("Name");
                this.listView1.Columns.Add(head);
                head = new ColumnHeader();
                head.Text =TransWord("ID");
                this.listView1.Columns.Add(head);
                head = new ColumnHeader();
                head.Text =TransWord("BodyFile");
                this.listView1.Columns.Add(head);
                head = new ColumnHeader();
                head.Text =TransWord("Stat-Str");
                this.listView1.Columns.Add(head);
                head = new ColumnHeader();
                head.Text =TransWord("Stat-Int");
                this.listView1.Columns.Add(head);
                head = new ColumnHeader();
                head.Text =TransWord("Stat-Wis");
                this.listView1.Columns.Add(head);
                head = new ColumnHeader();
                head.Text =TransWord("stat-Dex");
                this.listView1.Columns.Add(head);
                head = new ColumnHeader();
                head.Text =TransWord("Stat-Agi");
                this.listView1.Columns.Add(head);
                head = new ColumnHeader();
                head.Text =TransWord("Stat-Endure");
                this.listView1.Columns.Add(head);
                head = new ColumnHeader();
                head.Text =TransWord("Stat-Kno");
                this.listView1.Columns.Add(head);
                head = new ColumnHeader();
                head.Text =TransWord("Stat-Per");
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
                head.Text =TransWord("Name");
                this.listView1.Columns.Add(head);
                head = new ColumnHeader();
                head.Text =TransWord("ID");
                this.listView1.Columns.Add(head);
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
            { this.backgroundWorker1.RunWorkerAsync(); }
        }
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
            catch { MessageBox.Show("Please Select your Language,go Options>Options then choose your language and save"); }
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

            #endregion
        }
        public string TransWord(string word)
        {
            try
            {
                return this.trns.tr(word);
            }
            catch { return word; }
        }
        #endregion
        #region unfinished
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        { }
        private void editToolStripMenuItem1_Click(object sender, EventArgs e)
        { }
        #endregion
        private void editTranslationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TransCreator creator = new TransCreator();
            creator.Show(this);
        }
    }
}