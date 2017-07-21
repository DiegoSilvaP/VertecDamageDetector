using System;
using System.Threading;
using System.IO.Ports;
using SerialComunnication.Class;

namespace holaMundoOpenCV.Class
{
    class SerialCom
    {
        public static SerialPort serialPort = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);
        public Log makeLog = new Log();
        public void SendCommand(string data)
        {
            serialPort.Handshake = Handshake.None;
            serialPort.RtsEnable = true;
            try
            {
                serialPort.Open();
                serialPort.Write(data);
                Thread.Sleep(100);
                string response = serialPort.ReadExisting();
                Console.WriteLine("Data Received: " + response);
                makeLog.WriteFile(data, response);
                serialPort.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("Data Received: " + ex.ToString());
                makeLog.WriteFile(data, ex.ToString());
                serialPort.Close();
            }
        }
    }
}