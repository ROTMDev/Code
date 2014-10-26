// =Realms Engine=
// =Realms Of the Mind=
// =Programmers=
// =Mute Lovestone=
// =Tags.cs=
// = 10/25/2014 =
// =MUlib=

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LuaInterface;
namespace LibRealm.Base
{
    public class Tags
    {
        #region values
        
        protected string name;
        protected int id;
        protected string scriptName;
        protected bool hasScript;
        #endregion
        #region gets/sets
        
        public bool HasScript
        {
            get { return this.hasScript; }
        }
        
        public string ScriptName
        {
            get { return this.scriptName; }
            protected set { this.scriptName = value; }
        }
        
        public int Id
        {
            get { return this.id; }
            protected set { this.id = value; }
        }
        
        public string Name
        {
            get { return this.name; }
            protected set { this.name = value; }
        }
        #endregion
    }
}