using System;
using System.Runtime.InteropServices;

namespace cage {
    public static class Win32 {
        #region Layered windows
        
        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        
        [DllImport("user32.dll", SetLastError=true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        
        public const int GWL_EXSTYLE = -20;
        public const int WS_EX_TRANSPARENT = 0x0020;
        public const int WS_EX_LAYERED = 0x00080000;
        
        [DllImport("user32.dll")]
        public static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);
        
        public const int LWA_ALPHA = 2;
        
        #endregion
        
        #region Active window
        
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        
        #endregion
        
        #region Global hotkeys
        
        public static uint MOD_ALT = 0x1;
        public static uint MOD_CONTROL = 0x2;
        public static uint MOD_SHIFT = 0x4;
        public static uint MOD_WIN = 0x8;
        public static int WM_HOTKEY = 0x312;
        
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        
        #endregion
    }
}
