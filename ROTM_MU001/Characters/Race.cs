// =Realms Engine=
// =Realms Of the Mind=
// =Programmers=
// =Mute Lovestone=
// =Race.cs=
// = 10/25/2014 =
// =ROTM_MU001=

using System;
using System.IO;
using LibRealm.Base;
namespace LibRealm.Characters
{
    public sealed class Race
    {
        #region values
        private Stats baseStats;
        private string name;
        private Int32 raceID;
        #endregion
        #region Gets/sets
        
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
        #region logic Read/Write
        
        public void Write(BinaryWriter wr)
        {
            wr.Write("\0\0");
            wr.Write(this.name);
            wr.Write("\0\0");
            this.baseStats.Write(wr);
        }
        
        public void Read(BinaryReader re)
        {
            re.ReadBytes(2);
            this.name = re.ReadString();
            re.ReadBytes(2);
            this.baseStats.read(re);
        }
        #endregion
    }
}