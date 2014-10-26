// =Realms Engine=
// =Realms Of the Mind=
// =Programmers=
// =Mute Lovestone=
// =Stats.cs=
// = 10/25/2014 =
// =MUlib=

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
namespace LibRealm.Base
{
    public sealed class Stats
    {
        #region classes
        
        public Stats()
        { }
        #endregion
        #region Values
        
        private Int32 strength;
        private Int32 intelligence;
        private Int32 dextarity;
        private Int32 wisdom;
        private Int32 agility;
        private Int32 endure;
        private Int32 knoledge;
        private Int32 perception;
        #endregion
        #region gets/sets
        
        public int Perception
        {
            get { return this.perception; }
            set { this.perception = value; }
        }
        
        public int Knoledge
        {
            get { return this.knoledge; }
            set { this.knoledge = value; }
        }
        
        public int Endure
        {
            get { return this.endure; }
            set { this.endure = value; }
        }
        
        public int Agility
        {
            get { return this.agility; }
            set { this.agility = value; }
        }
        
        public int Wisdom
        {
            get { return this.wisdom; }
            set { this.wisdom = value; }
        }
        
        public int Dextarity
        {
            get { return this.dextarity; }
            set { this.dextarity = value; }
        }
        
        public int Intelligence
        {
            get { return this.intelligence; }
            set { this.intelligence = value; }
        }
        
        public int Strength
        {
            get { return this.strength; }
            set { this.strength = value; }
        }
        #endregion
        #region Logic
        #endregion
        #region Read/Write
        
        public void read(BinaryReader Reader)
        {
            this.agility = Reader.ReadInt32();
            this.dextarity = Reader.ReadInt32();
            this.endure = Reader.ReadInt32();
            this.intelligence = Reader.ReadInt32();
            this.knoledge = Reader.ReadInt32();
            this.perception = Reader.ReadInt32();
            this.strength = Reader.ReadInt32();
            this.wisdom = Reader.ReadInt32();
        }
        
        public void Write(BinaryWriter writer)
        {
            writer.Write(this.agility);
            writer.Write(this.dextarity);
            writer.Write(this.endure);
            writer.Write(this.intelligence);
            writer.Write(this.knoledge);
            writer.Write(this.perception);
            writer.Write(this.strength);
            writer.Write(this.wisdom);
        }
        #endregion
    }
}