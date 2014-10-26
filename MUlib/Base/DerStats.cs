// =Realms Engine=
// =Realms Of the Mind=
// =Programmers=
// =Mute Lovestone=
// =DerStats.cs=
// = 10/25/2014 =
// =MUlib=

using System;
namespace LibRealm.Base
{
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
            get { return this.phyDefence; }
            set { this.phyDefence = value; }
        }
        
        public int PhyAttack
        {
            get { return this.phyAttack; }
            set { this.phyAttack = value; }
        }
        
        public AttributePair Sp
        {
            get { return this.sp; }
            set { this.sp = value; }
        }
        
        public AttributePair Magic
        {
            get { return this.magic; }
            set { this.magic = value; }
        }
        
        public AttributePair Health
        {
            get { return this.health; }
            set { this.health = value; }
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
            
            this.health.SetMaximum(Convert.ToInt32(Main.Endure * 20));
            this.sp.SetMaximum(Convert.ToInt32(Main.Dextarity * 20));
            this.magic.SetMaximum(Convert.ToInt32(Main.Wisdom * 20));
            #endregion
            #region magic
            
            this.magAttack = Convert.ToInt32(Main.Intelligence * .88);
            this.magDefence = Convert.ToInt32(Main.Intelligence * .88);
            #endregion
            #endregion
            #region dex stats
            
            this.hitChance = Convert.ToInt32(Main.Dextarity / 20);
            this.speed = Convert.ToInt32(Main.Agility / 10);
            this.dodge = Convert.ToInt32(Main.Agility + Main.Dextarity / 50);
            this.counter = Convert.ToInt32(Main.Dextarity / 50 + (Main.Perception / 100));
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
        public void Read()
        { }
        
        /// <summary>
        /// might be used for deugging
        /// <para>unussed</para>
        /// </summary>
        public void Write()
        { }
        #endregion
    }
}