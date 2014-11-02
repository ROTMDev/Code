// =Realms Engine=
// =Realms Of the Mind=
// =Programmers=
// =Mute Lovestone=
// =Elements.cs=
// = 10/26/2014 =
// =MUlib=
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
namespace LibRealm.Base
{
    public sealed class Elements
    {
        #region values
        private String name;
        private Int32 iD;
        #endregion
        #region gets/sets
        public string Name
        {
            get { return this.name; }
            private set { this.name = value; }
        }
        public int ID
        {
            get { return this.iD; }
            private set { this.iD = value; }
        }
        #endregion
        #region classes
        public Elements()
        { }
        public Elements(string name, int id)
        {
            this.name = name;
            this.iD = id;
        }
        #endregion
        #region logic
        #endregion
        #region read/write
        public void write(BinaryWriter wr)
        {
            wr.Write(this.name);
            wr.Write(this.iD);
        }
        public void read(BinaryReader re)
        {
            this.name = re.ReadString();
            this.iD = re.ReadInt32();
        }
        #endregion
    }
}