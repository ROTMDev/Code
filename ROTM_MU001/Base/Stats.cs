using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace LibRealm.Base
{
    public sealed class Stats
    {
        #region classes
        public Stats() { }
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
            get
            {
                return this.perception;
            }
            set
            {
                this.perception = value;
            }
        }
        public int Knoledge
        {
            get
            {
                return this.knoledge;
            }
            set
            {
                this.knoledge = value;
            }
        }
        public int Endure
        {
            get
            {
                return this.endure;
            }
            set
            {
                this.endure = value;
            }
        }
        public int Agility
        {
            get
            {
                return this.agility;
            }
            set
            {
                this.agility = value;
            }
        }
        public int Wisdom
        {
            get
            {
                return this.wisdom;
            }
            set
            {
                this.wisdom = value;
            }
        }
        public int Dextarity
        {
            get
            {
                return this.dextarity;
            }
            set
            {
                this.dextarity = value;
            }
        }
        public int Intelligence
        {
            get
            {
                return this.intelligence;
            }
            set
            {
                this.intelligence = value;
            }
        }
        public int Strength
        {
            get
            {
                return this.strength;
            }
            set
            {
                this.strength = value;
            }
        }
        #endregion
        #region Logic

        #endregion
        #region Read/Write
       
        public void read(BinaryReader Reader)
        {
        }
        public void Write(BinaryWriter writer)
        {

        }
        #endregion
    }
}
