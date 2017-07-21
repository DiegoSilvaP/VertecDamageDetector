using System;

namespace SerialComunnication.Class
{
    class Time
    {
        public string getTimeNow() {
           return DateTime.Now.ToString("G");
        }
    }
}