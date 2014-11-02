using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
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
        public ModelLogi(GraphicsDevice device,ContentManager manas)
        {
            this.dev=device;
            this.mana = manas;
        }
        #endregion
        #region logic
        public Model LoadMod(string modelName,string textureName)
        {
            string modelPath=@"\Models\Useable\" + modelName;
            string Text = Environment.CurrentDirectory + @"\Data\Textures\" + textureName;
            this.mod = mana.Load<Model>(modelPath);
            //MemoryStream memStream = new MemoryStream(File.Open(Text, FileMode.Open));
            this.tex = Texture2D.FromStream(dev, File.Open(Text, FileMode.Open));
            foreach (ModelMesh mesh in this.mod.Meshes)
            {
                foreach (BasicEffect eff in mesh.Effects)
                {
                    eff.TextureEnabled = true;
                    eff.Texture = tex;
                }
            }
            this.tex = null;

            return mod;
        }
        public void Unload()
        {
            this.mod = null;
        }
        #endregion
    }
}
