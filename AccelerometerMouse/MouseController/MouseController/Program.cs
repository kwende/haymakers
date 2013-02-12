using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MouseController
{
    class Program
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        static SerialPort _port;
        static bool _continue = true;
        static bool _inClick = false; 

        static void Main(string[] args)
        {
            _port = new SerialPort();
            _port.PortName = "COM3";
            _port.BaudRate = 9600;

            _port.ReadTimeout = 500;
            _port.WriteTimeout = 500;

            _port.Open(); 

            Thread thread = new Thread(new ThreadStart(delegate()
                {
                    while (_continue)
                    {
                        try
                        {
                            string data = _port.ReadLine();
                            string[] bits = data.Replace("\r", "").Split('\t').ToArray(); 
                            Console.WriteLine(data);

                            if (bits.Length == 3 && bits[0].Length > 0 && bits[1].Length > 0 && bits[2].Length > 0)
                            {
                                int xOriginal = int.Parse(bits[1]); 
                                int yOriginal = int.Parse(bits[2]);

                                if (Math.Abs(xOriginal) > 50 || Math.Abs(yOriginal) > 50)
                                {
                                    Cursor.Position = new System.Drawing.Point
                                    {
                                        X = Cursor.Position.X + -1 * xOriginal / 10,
                                        Y = Cursor.Position.Y + yOriginal / 10
                                    };
                                }

                                if (bits[0] == "1" && !_inClick)
                                {
                                    _inClick = true;
                                    mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);
                                }
                                else if (bits[0] == "0")
                                {
                                    _inClick = false;
                                }
                            }
                        }
                        catch (TimeoutException) { }
                    }
                }));
            thread.Start();

            Console.WriteLine("Hit ENTER to quit."); 
            Console.ReadLine();

            _continue = false;
            thread.Join(); 
        }
    }
}
