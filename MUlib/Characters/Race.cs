// =Realms Engine=
// =Realms Of the Mind=
// =Programmers=
// =Mute Lovestone=
// =Race.cs=
// = 11/3/2014 =
// =MUlib=
using System;
using System.IO;
using LibRealm.Base;
namespace LibRealm.Characters
{
    public sealed class Race
    {
        #region values
        private Stats baseStats = new Stats();
        private string name;
        private Int32 raceID;
        private string bodyPath;
        private string modelPath;
        #endregion
        #region Gets/sets
        public string BodyPath
        {
            get { return this.bodyPath; }
            private set { this.bodyPath = value; }
        }
        public int RaceID
        {
            get { return this.raceID; }
            private set { this.raceID = value; }
        }
        public string Name
        {
            get { return this.name; }
            private set { this.name = value; }
        }
        public Stats BaseStats
        {
            get { return this.baseStats; }
            private set { this.baseStats = value; }
        }
        #endregion
        #region logic
        #endregion
        #region class
        public Race(Stats Base, string NaMe, Int32 ID, string body,string model)
        {
            this.baseStats = Base;
            this.name = NaMe;
            this.raceID = ID;
            this.bodyPath = body;
            this.modelPath = model;
        }
        public Race()
        { }
        #endregion
        #region logic Read/Write
        public void Write(BinaryWriter wr)
        {
            wr.Write(this.name);
            wr.Write(this.bodyPath);
            wr.Write(this.modelPath);
            wr.Write(this.raceID);
            this.baseStats.Write(wr);
        }
        public void Read(BinaryReader re)
        {
            this.name = re.ReadString();
            this.bodyPath = re.ReadString();
            this.modelPath = re.ReadString();
            this.raceID = re.ReadInt32();
            this.baseStats.read(re);
        }
        #endregion
    }
}