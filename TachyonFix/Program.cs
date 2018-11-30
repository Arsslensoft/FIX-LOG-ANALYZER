using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace VisualStudio2012Style
{
    static class Program
    {
        public static MainForm MainInstance { get; set; }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainInstance = new MainForm();
            Application.Run(MainInstance);
        }
    }
}