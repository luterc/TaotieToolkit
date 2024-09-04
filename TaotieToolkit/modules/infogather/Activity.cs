using System;
using System.Runtime.InteropServices;
using System.Text;
using TaotieToolkit.config;

namespace TaotieToolkit.modules.infogather
{
	internal class GetWindowTitle : ICommand, ICommandMarker
    {
        public string Name => "GetWindowTitle";
        public string Description => "获取活动窗口";
       
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowTextLength(IntPtr hWnd);
        public void Execute(string[] args)
        {

            string result = string.Empty;
            IntPtr foregroundWindow = GetForegroundWindow();
            int num = GetWindowTextLength(foregroundWindow) + 1;
            StringBuilder stringBuilder = new StringBuilder(num);
            bool flag = GetWindowText(foregroundWindow, stringBuilder, num) > 0;
            if (flag)
            {
                result = stringBuilder.ToString();
            }
            Console.WriteLine("[*] Active window title received:" + result);
        }


    }
    class UserIsActive : ICommand, ICommandMarker
    {
        public string Name => "UserIsActive";
        public string Description => "用户当前是否处于活动状态";
        public void Execute(string[] args)
        {
            int wait = 3000;
            int[] c1 = getCursorLocation();
            System.Threading.Thread.Sleep(wait);
            int[] c2 = getCursorLocation();
            if (c1[0] != c2[0] || c1[1] != c1[1])
            {
                Console.WriteLine("[*] User is active");
            }
            else
            {

                Console.WriteLine("\t[-] User is not active");
            }

        }
        public struct POINT
        {
            public int X;
            public int Y;
        }

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);
        public int[] getCursorLocation()
        {
            POINT point;
            GetCursorPos(out point);
            int[] result = new int[2]
            {
                point.X,
                point.Y
            };
            return result;
        }

    }
}
