using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IRCommander
{
    class Program
    {
        private static SerialPort _port;

        private enum Codes : short
        {
            One = 0x10,
            Two = 0x810,
            Three = 0x410,
            Four = 0xc10,
            Five = 0x210,
            Six = 0xa10,
            Seven = 0x610,
            Eight = 0xe10,
            Nine = 0x910,
            VolumeUp = 0x490,
            VolumeDown = 0xc90,
            ChannelUp = 0x90,
            ChannelDown = 0x890,
            PowerOnOff = 0xa90
        };

        static void SendSignal(Codes code)
        {
            short command = (short)code; 

            byte[] bytes = new byte[2];
            bytes[0] = (byte)(command >> 8);
            bytes[1] = (byte)(0x00FF & command);

            _port.Write(bytes, 0, bytes.Length);
        }

        static void Main(string[] args)
        {
                _port = new SerialPort();
                _port.PortName = "COM3";
                _port.BaudRate = 9600;
                _port.Open();

            Thread thread = new Thread(new ThreadStart(delegate()
                {
                    Console.WriteLine(_port.ReadLine()); 
                }));
            thread.Start(); 

            SendSignal(Codes.PowerOnOff);
            //SendSignal(Codes.Five); 

            Console.ReadLine(); 
        }
    }
}
