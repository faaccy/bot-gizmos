using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Drawing;

namespace Bot.Core
{
    public static class WowProcess
    {

        private const UInt32 WM_KEYDOWN = 0x0100;
        private const UInt32 WM_KEYUP = 0x0101;
        private static ConsoleKey lastKey;
        private static Random random = new Random();
        public static int LootDelay=2000;

        public static bool IsWowClassic()
        {
            var wowProcess = Get();
            return wowProcess != null ? wowProcess.ProcessName.ToLower().Contains("classic") : false; ;
        }

        //Get the wow-process, if success returns the process else null
        public static Process Get(string name = "")
        {
            {
                var names = string.IsNullOrEmpty(name) ? new List<string> { "Wow", "WowClassic", "Wow-64" } : new List<string> { name };
                var processList = Process.GetProcesses();
                foreach (var p in processList)
                {
                    if (names.Select(s => s.ToLower()).Contains(p.ProcessName.ToLower()))
                    {
                       return p;
                    }
                }
            }
            return null;
        }

        //Get the wow-process, if success returns the process else null
        public static List<Process> GetList(string name = "")
        {
            var names = string.IsNullOrEmpty(name) ? new List<string> { "Wow", "WowClassic", "Wow-64" } : new List<string> { name };
            var processList = Process.GetProcesses();
            var list = processList.Where(c => names.Select(s => s.ToLower()).Contains(c.ProcessName.ToLower()) && !c.MainModule.FileName.Contains("_classic_era_")).ToList();
            return list;
        }

        [DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string strClassName, string strWindowName);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);
        // Find window by Caption only. Note you must pass IntPtr.Zero as the first parameter.
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }

        public static void GetWindowLocation(IntPtr mainWindowHandle)
        {
            Rect NotepadRect = new Rect();
            GetWindowRect(mainWindowHandle, ref NotepadRect);
        }

        private static Process GetActiveProcess()
        {
            IntPtr hwnd = GetForegroundWindow();
            uint pid;
            GetWindowThreadProcessId(hwnd, out pid);
            return Process.GetProcessById((int)pid);
        }

        private static void KeyDown(ConsoleKey key)
        {
            lastKey = key;
            var wowProcess = Get();
            if (wowProcess != null)
            {
                PostMessage(wowProcess.MainWindowHandle, WM_KEYDOWN, (int)key, 0);
            }
        }

        private static void KeysDown(ConsoleKey key)
        {
            lastKey = key;
            var wowProcessList = GetList();
            if (wowProcessList != null)
            {
                foreach (var item in wowProcessList)
                {
                    PostMessage(item.MainWindowHandle, WM_KEYDOWN, (int)key, 0);
                    Thread.Sleep(10 + random.Next(0, 37));
                }
            }
        }

        private static void KeysUp(ConsoleKey key)
        {
            var wowProcessList = GetList();
            if (wowProcessList != null)
            {
                foreach (var item in wowProcessList)
                {
                    PostMessage(item.MainWindowHandle, WM_KEYUP, (int)key, 0);
                    Thread.Sleep(10 + random.Next(0, 37));
                }
            }
        }

        public static void PressKeys(ConsoleKey key)
        {
            KeysDown(key);
            Thread.Sleep(50 + random.Next(0, 75));
            KeysUp(key);
        }


        private static void KeyUp()
        {
            KeyUp(lastKey);
        }

        public static void PressKey(ConsoleKey key)
        {
            KeyDown(key);
            Thread.Sleep(50 + random.Next(0, 75));
            KeyUp(key);
        }

        public static void KeyUp(ConsoleKey key)
        {
            var wowProcess = Get();
            if (wowProcess != null)
            {
                PostMessage(wowProcess.MainWindowHandle, WM_KEYUP, (int)key, 0);
            }
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetCursorPos(int x, int y);

        public static void RightClickMouse(System.Drawing.Point position)
        {
            //RightClickMouse_Original(logger, position);
            RightClickMouse_LiamCooper(position);
        }

        public static void RightClickMouse_Original(System.Drawing.Point position)
        {
            var activeProcess = GetActiveProcess();
            var wowProcess = WowProcess.Get();
            if (wowProcess != null)
            {
                var oldPosition = System.Windows.Forms.Cursor.Position;

                for (int i = 20; i > 0; i--)
                {
                    SetCursorPos(position.X + i, position.Y + i);
                    Thread.Sleep(1);
                }
                Thread.Sleep(1000);

                PostMessage(wowProcess.MainWindowHandle, Keys.WM_RBUTTONDOWN, Keys.VK_RMB, 0);
                Thread.Sleep(30 + random.Next(0, 47));
                PostMessage(wowProcess.MainWindowHandle, Keys.WM_RBUTTONUP, Keys.VK_RMB, 0);

                RefocusOnOldScreen(activeProcess, wowProcess, oldPosition);
            }
        }

        public static void RightClickMouse()
        {
            var activeProcess = GetActiveProcess();
            var wowProcess = WowProcess.Get();
            if (wowProcess != null)
            {
                var oldPosition = System.Windows.Forms.Cursor.Position;
                PostMessage(wowProcess.MainWindowHandle, Keys.WM_RBUTTONDOWN, Keys.VK_RMB, 0);
                Thread.Sleep(30 + random.Next(0, 47));
                PostMessage(wowProcess.MainWindowHandle, Keys.WM_RBUTTONUP, Keys.VK_RMB, 0);
            }
        }

        public static void RightClickMousePos(Point position)
        {
            var wowProcess = WowProcess.Get();
            if (wowProcess != null)
            {
                SetCursorPos(position.X, position.Y);
                Thread.Sleep(100);
                PostMessage(wowProcess.MainWindowHandle, Keys.WM_RBUTTONDOWN, Keys.VK_RMB, 0);
                Thread.Sleep(30 + random.Next(0, 47));
                PostMessage(wowProcess.MainWindowHandle, Keys.WM_RBUTTONUP, Keys.VK_RMB, 0);
            }
        }

        public static void LeftClickMouse()
        {
            var activeProcess = GetActiveProcess();
            var wowProcess = WowProcess.Get();
            if (wowProcess != null)
            {
                var oldPosition = System.Windows.Forms.Cursor.Position;
                PostMessage(wowProcess.MainWindowHandle, Keys.WM_LBUTTONDOWN, Keys.VK_RMB, 0);
                Thread.Sleep(30 + random.Next(0, 47));
                PostMessage(wowProcess.MainWindowHandle, Keys.WM_LBUTTONUP, Keys.VK_RMB, 0);
            }
        }

        public static void LeftClickMousePos(Point position)
        {
            var wowProcess = WowProcess.Get();
            if (wowProcess != null)
            {
                SetCursorPos(position.X, position.Y);
                Thread.Sleep(100);
                PostMessage(wowProcess.MainWindowHandle, Keys.WM_LBUTTONDOWN, Keys.VK_RMB, 0);
                Thread.Sleep(30 + random.Next(0, 47));
                PostMessage(wowProcess.MainWindowHandle, Keys.WM_LBUTTONUP, Keys.VK_RMB, 0);
            }
        }

        public static void RightClickMouse_LiamCooper( System.Drawing.Point position)
        {
            var activeProcess = GetActiveProcess();
            var wowProcess = WowProcess.Get();
            if (wowProcess != null)
            {
                mouse_event((int)MouseEventFlags.RightUp, position.X, position.Y, 0, 0);
                var oldPosition = System.Windows.Forms.Cursor.Position;

                Thread.Sleep(200);
                System.Windows.Forms.Cursor.Position = position;
                Thread.Sleep(LootDelay);
                mouse_event((int)MouseEventFlags.RightDown, position.X, position.Y, 0, 0);
                Thread.Sleep(30 + random.Next(0, 47));
                mouse_event((int)MouseEventFlags.RightUp, position.X, position.Y, 0, 0);
                RefocusOnOldScreen(activeProcess, wowProcess, oldPosition);
                Thread.Sleep(LootDelay / 2);
            }
        }

        private static void RefocusOnOldScreen( Process activeProcess, Process wowProcess, System.Drawing.Point oldPosition)
        {
            try
            {
                if (activeProcess.MainWindowTitle != wowProcess.MainWindowTitle)
                {
                    // get focus back on this screen
                    PostMessage(activeProcess.MainWindowHandle, Keys.WM_RBUTTONDOWN, Keys.VK_RMB, 0);
                    Thread.Sleep(30);
                    PostMessage(activeProcess.MainWindowHandle, Keys.WM_RBUTTONUP, Keys.VK_RMB, 0);

                    KeyDown(ConsoleKey.Escape);
                    Thread.Sleep(30);
                    KeyUp(ConsoleKey.Escape);

                    System.Windows.Forms.Cursor.Position = oldPosition;
                }
            }
            catch (Exception ex)
            {
               
            }
        }

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        [Flags]
        public enum MouseEventFlags
        {
            LeftDown = 0x00000002,
            LeftUp = 0x00000004,
            MiddleDown = 0x00000020,
            MiddleUp = 0x00000040,
            Move = 0x00000001,
            Absolute = 0x00008000,
            RightDown = 0x00000008,
            RightUp = 0x00000010
        }
    }
}