using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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
            get
            {
                return this.mainStats;
            }
            private set
            {
                this.mainStats = value;
            }
        }
        #endregion
    }
}
