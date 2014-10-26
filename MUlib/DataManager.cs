// =Realms Engine=
// =Realms Of the Mind=
// =Programmers=
// =Mute Lovestone=
// =DataManager.cs=
// = 10/26/2014 =
// =MUlib=
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibRealm.Base;
using LibRealm.Characters;
namespace LibRealm
{
    public sealed class DataManager
    {
        #region values
        Dictionary<int, Race> Races = new Dictionary<int, Race>();
        Dictionary<int, Elements> elems = new Dictionary<int, Elements>();
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