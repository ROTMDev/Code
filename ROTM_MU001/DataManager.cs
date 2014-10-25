using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibRealm.Characters;

namespace LibRealm
{
    public sealed class DataManager
    {
        #region values
        Dictionary<int, Race> Races = new Dictionary<int, Race>();
        #endregion
        #region gets/sets
        public Dictionary<int, Race> PRaces
        {
            get { return this.Races; }
            set { this.Races = value; }
        }
        #endregion
        #region logic
        #endregion
        #region class
        public DataManager()
        { }

        #endregion
    }
}
