using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace cage {
    public partial class ImageSwitcherForm : Form {
        bool showingImage1 = true;
        
        int hotkeyId = 1;
        
        IntPtr activeHwnd;
        
        public ImageSwitcherForm() {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
        }
        
        private Image _Image1;
        public Image Image1 {
            get {
                return _Image1;
            }
            set {
                _Image1 = value;
            }
        }
        
        private Image _Image2;
        public Image Image2 {
            get {
                return _Image2;
            }
            set {
                _Image2 = value;
            }
        }
        
        protected override void OnLoad(EventArgs e) {
            // Size the form to the dimensions of Image1
            
            if (Image1 != null) {
                this.Height = Image1.Height;
                this.Width = Image1.Width;
                pictureBox1.Image = Image1;
            }
            
            // Save the currently active window
            
            activeHwnd = Win32.GetForegroundWindow();
            
            // Position the window near the mouse
            
            Point mousePos = Control.MousePosition;
            foreach (Screen s in Screen.AllScreens) {
                if (s.Bounds.Contains(mousePos)) {
                    Rectangle positionWanted = new Rectangle(
                        mousePos.X - (this.Width / 2),
                        mousePos.Y - (this.Height / 2),
                        this.Width,
                        this.Height);
                    if (positionWanted.Bottom > s.Bounds.Bottom) {
                        positionWanted.Offset(0, s.Bounds.Bottom - positionWanted.Bottom);
                    }
                    if (positionWanted.Right > s.Bounds.Right) {
                        positionWanted.Offset(s.Bounds.Right - positionWanted.Right, 0);
                    }
                    if (positionWanted.Top < s.Bounds.Top) {
                        positionWanted.Offset(0, s.Bounds.Top - positionWanted.Top);
                    }
                    if (positionWanted.Left < s.Bounds.Left) {
                        positionWanted.Offset(s.Bounds.Left - positionWanted.Left, 0);
                    }
                    this.Bounds = positionWanted;
                }
            }
            
            // Make this a layered window
            // From http://www.codeproject.com/Articles/12877/Transparent-Click-Through-Forms
            
            Win32.SetWindowLong(this.Handle, Win32.GWL_EXSTYLE,
                Win32.GetWindowLong(this.Handle, Win32.GWL_EXSTYLE)
                | Win32.WS_EX_LAYERED | Win32.WS_EX_TRANSPARENT);
            
            Win32.SetLayeredWindowAttributes(this.Handle, 0,
                160, // 0=transparent 255=opaque
                Win32.LWA_ALPHA);
            
            this.TopMost = true;
            
            // Register global hotkey to close the window: Ctrl+Alt+Shift+F12
            // Adapted from http://www.pinvoke.net/default.aspx/user32/RegisterHotKey.html
            
            Win32.RegisterHotKey(this.Handle, hotkeyId,
                Win32.MOD_CONTROL | Win32.MOD_ALT | Win32.MOD_SHIFT,
                (uint)Keys.F12);
            
            // Start flipping between images
            
            timer1.Start();
            
            // Execute default OnLoad code
            
            base.OnLoad(e);
        }
        
        protected override void OnShown(EventArgs e) {
            // Activate the window that was active before this window came to the front
            
            Win32.SetForegroundWindow(activeHwnd);
            
            // Execute default OnShown code
            
            base.OnShown(e);
        }
        
        protected override void WndProc(ref Message m) {
            // Handle hotkey press: close the form
            
            if (m.Msg == Win32.WM_HOTKEY && (int)m.WParam == hotkeyId) {
                this.Close();
            }
            
            // Execute default WndProc code
            
            base.WndProc(ref m);
        }
        
        protected override void OnFormClosing(FormClosingEventArgs e) {
            // Unregister our hotkey
            
            Win32.UnregisterHotKey(this.Handle, hotkeyId);
            
            // Execute default OnFormClosing code
            
            base.OnFormClosing(e);
        }
        
        void timer1_Tick(object sender, EventArgs e) {
            if (showingImage1) {
                pictureBox1.Image = Image2;
                showingImage1 = false;
            } else {
                pictureBox1.Image = Image1;
                showingImage1 = true;
            }
        }
    }
}
