// =Realms Engine=
// =Realms Of the Mind=
// =Programmers=
// =Mute Lovestone=
// =ModelData.cs=
// = 11/3/2014 =
// =MUlib=
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
namespace LibRealm.Modelx
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ModelLogi
    {
        #region values
        private Model mod;
        private GraphicsDevice dev;
        private Texture2D tex;
        private ContentManager mana;
        #endregion
        #region class
        public ModelLogi(GraphicsDevice device, ContentManager manas)
        {
            this.dev = device;
            this.mana = manas;
        }
        #endregion
        #region logic
        public Model LoadMod(string modelName, string textureName)
        {
            string modelPath = string.Format("{0}{1}", @"\Models\Useable\", modelName);
            string Text = string.Format("{0}{1}{2}", Environment.CurrentDirectory, @"\Data\Textures\", textureName);
            this.mod = this.mana.Load<Model>(modelPath);
            //MemoryStream memStream = new MemoryStream(File.Open(Text, FileMode.Open));
            this.tex = Texture2D.FromStream(this.dev, File.Open(Text, FileMode.Open));
            foreach (ModelMesh mesh in this.mod.Meshes)
            {
                foreach (BasicEffect eff in mesh.Effects)
                {
                    eff.TextureEnabled = true;
                    eff.Texture = this.tex;
                }
            }
            this.tex = null;

            return this.mod;
        }
        public void Unload()
        { this.mod = null; }
        #endregion
    }
}