using System.IO;

namespace SerialComunnication.Class
{
    class Log
    {
        public Time now = new Time();
       public void WriteFile(string dataFile,string response) {
            StreamWriter agregar = File.AppendText("Resources/LogFile.txt");
            agregar.WriteLine("[{Fecha: "+now.getTimeNow()+"},{Comando: "+dataFile+"},{Respuesta: "+response+"}]");
            agregar.Close();
        }
    }
}