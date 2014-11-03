// =Realms Engine=
// =Realms Of the Mind=
// =Programmers=
// =Mute Lovestone=
// =DataManager.cs=
// = 11/3/2014 =
// =MUlib=
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LibRealm.Base;
using LibRealm.Characters;
namespace LibRealm
{
    public sealed class DataManager
    {
        #region values
        Dictionary<int, Race> races = new Dictionary<int, Race>();
        Dictionary<int, Elements> elems = new Dictionary<int, Elements>();
        #endregion
        #region gets/sets
        public Dictionary<int, Elements> Elems
        {
            get { return this.elems; }
            set { this.elems = value; }
        }
        public Dictionary<int, Race> Races
        {
            get { return this.races; }
            set { this.races = value; }
        }
        #endregion
        #region logic
        #endregion
        #region class
        public DataManager()
        { }
        #endregion
        #region read write
        #endregion
    }
}