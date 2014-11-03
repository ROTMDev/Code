// =Realms Engine=
// =Realms Of the Mind=
// =Programmers=
// =Mute Lovestone=
// =Laguage.cs=
// = 11/3/2014 =
// =ROTM_MU002=
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace ROTM_MU002.Windows
{
    public class Laguage
    {
        #region values
        private string languageName = "";
        #endregion
        #region class
        public Laguage(string name)
        { this.languageName = name; }
        public Laguage()
        { }
        #endregion
        #region logic
        public void Write(BinaryWriter wr)
        { wr.Write(this.languageName); }
        public void Read(BinaryReader re)
        { this.languageName = re.ReadString(); }
        #endregion
        #region get/sets
        public string LanguageName
        {
            get { return this.languageName; }
            set { this.languageName = value; }
        }
        #endregion
    }
    public class LanTrans
    {
        #region values
        public Laguage name = new Laguage();
        public Dictionary<string, String> Langagetrans = new Dictionary<string, string>();
        #endregion
        #region logic
        public LanTrans(string name)
        { this.name.LanguageName = name; }
        public LanTrans()
        { }
        public void addTrans(string english, string trans)
        {
            try
            { this.Langagetrans.Remove(english); }
            catch
            { }this.Langagetrans.Add(english, trans);
        }
        public string tr(string x)
        { return this.Langagetrans[x]; }
        #endregion
        #region read,write
        public void read(BinaryReader re)
        {
            this.name.LanguageName = re.ReadString();
            int x = re.ReadInt32();
            for (int p = 0; p < x; p++)
            { this.Langagetrans.Add(re.ReadString(), re.ReadString()); }
            re.Close();
        }
        public void write(BinaryWriter wr)
        {
            wr.Write(this.name.LanguageName);
            wr.Write(this.Langagetrans.Keys.Count);
            foreach (string x in this.Langagetrans.Keys)
            {
                wr.Write(x);
                wr.Write(this.Langagetrans[x]);
            }
        }
        #endregion
    }
}