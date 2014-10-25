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
    public sealed class DerStats
    {
        #region values
        #region derived stats 1
        private AttributePair health;
        private AttributePair magic;
        private AttributePair sp;
        private Int32 phyAttack;
        private Int32 phyDefence;
        private Int32 magDefence;
        private Int32 magAttack;
        #endregion
        #region dex stats
        private Int32 hitChance;
        private Int32 speed;
        private Int32 dodge;
        private Int32 counter;
        #endregion
        #endregion
        #region gets/sets
        public int PhyDefence
        {
            get
            {
                return this.phyDefence;
            }
            set
            {
                this.phyDefence = value;
            }
        }
        public int PhyAttack
        {
            get
            {
                return this.phyAttack;
            }
            set
            {
                this.phyAttack = value;
            }
        }
        public AttributePair Sp
        {
            get
            {
                return this.sp;
            }
            set
            {
                this.sp = value;
            }
        }
        public AttributePair Magic
        {
            get
            {
                return this.magic;
            }
            set
            {
                this.magic = value;
            }
        }
        public AttributePair Health
        {
            get
            {
                return this.health;
            }
            set
            {
                this.health = value;
            }
        }
        #endregion
        #region class
        public DerStats(Stats Main)
        {
            #region derived stats 1
            #region physical
            this.phyAttack = Convert.ToInt32((Main.Strength * .88) + (Main.Dextarity * .333));
            this.phyDefence = Convert.ToInt32((Main.Strength * .88) + (Main.Dextarity * .333));
            #endregion
            #region values
            this.health = health.SetMaximum(Convert.ToInt32(Main.Endure * 20));
            this.sp = sp.SetMaximum(Convert.ToInt32(Main.Dextarity * 20));
            this.magic = magic.SetMaximum(Convert.ToInt32(Main.Wisdom * 20));
            #endregion
            #region magic
            this.magAttack = Convert.ToInt32(Main.Intelligence * .88);
            this.magDefence = Convert.ToInt32(Main.Intelligence * .88);
            #endregion
            #endregion
            #region dex stats
            #endregion
        }
        #endregion
        #region Logic
        #endregion
        #region Read/Write
        
        /// <summary>
        /// Might be used for debugging
        /// <para>Not used for anything yet</para>
        /// </summary>
        public void Read() { }
        /// <summary>
        /// might be used for deugging
        /// <para>unussed</para>
        /// </summary>
        public void Write() { }
        #endregion
    }
    public class AttributePair
    {
        #region Field Region

        int currentValue;
        int maximumValue;

        #endregion

        #region Property Region

        public int CurrentValue
        {
            get { return currentValue; }
        }

        public int MaximumValue
        {
            get { return maximumValue; }
        }

        public static AttributePair Zero
        {
            get { return new AttributePair(); }
        }

        #endregion

        #region Constructor Region

        private AttributePair()
        {
            currentValue = 0;
            maximumValue = 0;
        }

        public AttributePair(int maxValue)
        {
            currentValue = maxValue;
            maximumValue = maxValue;
        }

        #endregion

        #region Method Region

        public void Heal(ushort value)
        {
            currentValue += value;
            if (currentValue > maximumValue)
                currentValue = maximumValue;
        }

        public void Damage(ushort value)
        {
            currentValue -= value;
            if (currentValue < 0)
                currentValue = 0;
        }

        public void SetCurrent(int value)
        {
            currentValue = value;
            if (currentValue > maximumValue)
                currentValue = maximumValue;
        }

        public void SetMaximum(int value)
        {
            maximumValue = value;
            if (currentValue > maximumValue)
                currentValue = maximumValue;
        }

        #endregion
    }
}
