using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace cage {
    static class Program {
        [STAThread]
        private static void Main(string[] args) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            Assembly a = typeof(Program).Assembly;
            
            ImageSwitcherForm form = new ImageSwitcherForm();
            form.Image1 = new Bitmap(a.GetManifestResourceStream("creepy.jpg"));
            form.Image2 = new Bitmap(a.GetManifestResourceStream("creepy2.jpg"));
            
            Application.Run(form);
        }
        
    }
}
