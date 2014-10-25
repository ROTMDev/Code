using System;
using System.Windows.Forms;
using Editor.EditorF;
namespace Editor
{
#if WINDOWS || XBOX
    static class Program
    {
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new EditMain());
        }
    }
#endif
}

