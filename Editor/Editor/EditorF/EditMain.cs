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
using Editor.Ra;
using LibRealm.Characters;
namespace Editor.EditorF
{
    public partial class EditMain : Form
    {
        public Dictionary<int, Race> races = new Dictionary<int, Race>();
        #region premade(Need Edited)
        private int childFormNumber = 0;
        public EditMain()
        { this.InitializeComponent(); }
        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = string.Format("Window {0}", this.childFormNumber++);
            childForm.Show();
        }
        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }
        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }
        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        { this.Close(); }
        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        { }
        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        { }
        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        { }
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
        private void EditMain_Load(object sender, EventArgs e)
        {
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            EditMain.CheckForIllegalCrossThreadCalls = false;
            CheckForIllegalCrossThreadCalls = false;
            ListView.CheckForIllegalCrossThreadCalls = false;
            this.backgroundWorker1.RunWorkerAsync();
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
        public Int32 getid()
        {
            int intselectedindex = this.listView1.SelectedIndices[0];
            return intselectedindex + 1; 
        }
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
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            #region Races
            if (this.treeView1.SelectedNode.Text == "Races")
            {
                this.listView1.Items.Clear();
                this.listView1.Columns.Clear();
                ColumnHeader head = new ColumnHeader();
                head.Text = "Name";
                this.listView1.Columns.Add(head);
                ColumnHeader head2 = new ColumnHeader();
                head2.Text = "ID";
                this.listView1.Columns.Add(head2);
                head = new ColumnHeader();
                head.Text = "BodyFile";
                this.listView1.Columns.Add(head);
                head = new ColumnHeader();
                head.Text = "Stat-Str";
                this.listView1.Columns.Add(head);
                head = new ColumnHeader();
                head.Text = "Stat-Int";
                this.listView1.Columns.Add(head);
                head = new ColumnHeader();
                head.Text = "Stat-Wis";
                this.listView1.Columns.Add(head);
                head = new ColumnHeader();
                head.Text = "stat-Dex";
                this.listView1.Columns.Add(head);
                head = new ColumnHeader();
                head.Text = "Stat-Agi";
                this.listView1.Columns.Add(head);
                head = new ColumnHeader();
                head.Text = "Stat-Endure";
                this.listView1.Columns.Add(head);
                head = new ColumnHeader();
                head.Text = "Stat-Kno";
                this.listView1.Columns.Add(head);
                head = new ColumnHeader();
                head.Text = "Stat-Per";
                this.listView1.Columns.Add(head);
                this.listView1.ContextMenuStrip = this.RaceMen;
                this.addRaces();
            }
            #endregion
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RaceEdi nx = new RaceEdi(this, true);
            nx.ShowDialog();
        }
        private void newToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RaceEdi nx = new RaceEdi(this, false);
            nx.ShowDialog();
        }
    }
}