﻿// =Realms Engine=
// =Realms Of the Mind=
// =Programmers=
// =Mute Lovestone=
// =Entity.cs=
// = 10/26/2014 =
// =MUlib=
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LibRealm.Base;
namespace LibRealm.Characters
{
    public class Entity
    {
        #region values
        private Stats mainStats;
        private string name;
        private string ID;
        private bool isControled;
        private bool isDead;
        #endregion
        #region Gets/Sets
        public Stats MainStats
        {
            get { return this.mainStats; }
            private set { this.mainStats = value; }
        }
        #endregion
    }
}