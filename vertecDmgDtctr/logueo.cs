using holaMundoOpenCV.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using vertecDmgDtctr.Clases;

namespace vertecDmgDtctr
{
    public partial class logueo : Form
    {
        string progFiles = @"C:\Program Files\Common Files\Microsoft Shared\ink\TabTip.exe";
        Bitmap gif = new Bitmap("recursos/welcomegif.gif");
        Bitmap f1 = new Bitmap("recursos/w11.png");

        //Creación del objeto reloj
        reloj mireloj = new reloj();


        #region Checar estado del teclado

        public const UInt32 WS_DISABLED = 0x8000000;
        public const UInt32 WS_VISIBLE = 0X94000000;
        public const int GWL_STYLE = -16;

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(String sClassName, String sAppName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern UInt32 GetWindowLong(IntPtr hWnd, int nIndex);

        public static IntPtr GetKeyboardWindowHandle()
        {
            return FindWindow("IPTip_Main_Window", null);
        }

        public static bool IsKeyboardVisible()
        {
            IntPtr keyboardHandle = GetKeyboardWindowHandle();

            bool visible = false;

            if (keyboardHandle != IntPtr.Zero)
            {
                UInt32 style = GetWindowLong(keyboardHandle, GWL_STYLE);
                visible = (style == WS_VISIBLE);
            }

            return visible;
        }

        public void actualizacion(object sender, EventArgs arg)
        {
            if (teclado == false)
            {
                if (IsKeyboardVisible())
                {
                    this.DoubleBuffered = true;
                }
                else
                {
                    Process keyboardProc = Process.Start(progFiles);
                }
            }

        }
#endregion

        public logueo()
        {
            InitializeComponent();
            SerialCom serial = new SerialCom();
            serial.SendCommand("cmd01a");
            Thread.Sleep(1000);
            serial.SendCommand("cmd01a");
            Thread.Sleep(1000);
            serial.SendCommand("cmd01a");
            Thread.Sleep(1000);
            pictureBox2.Image = gif;
            this.BackgroundImage = f1;

        }

        //botón salida de emergencia
        private void button3_Click_1(object sender, EventArgs e)
        {
            Application.ExitThread();
        }

        //al cargar el form
        private void logueo_Load(object sender, EventArgs e)
        {
            Application.Idle += actualizacion;
            mireloj.Show();
        }
        bool teclado = false;

        //botón iniciar sesión
        private void button1_Click(object sender, EventArgs e)
        {
            errorDeLogueo error = new errorDeLogueo();
            restServices conectar = new restServices();
            if (conectar.Validate(usrBox.Text, passBox.Text))
            {
                teclado = true;
                error.Close();
                error.Dispose();
                SerialCom serial = new SerialCom();
                serial.SendCommand("cmd02a");
                Thread.Sleep(1000);
                serial.SendCommand("cmd09a");
                Thread.Sleep(1000);
                instrucciones instr = new instrucciones();
                instr.Show();
                FadeOut(this,20);
                Process[] oskProcessArray = Process.GetProcessesByName("TabTip");
                foreach (Process onscreenProcess in oskProcessArray)
                {
                    onscreenProcess.Kill();
                }
            }
            else
            {
                teclado = false;
                error.Show();
            }
        }

        //al presionar enter
        private void passBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            errorDeLogueo error = new errorDeLogueo();
            restServices conectar = new restServices();
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                if (conectar.Validate(usrBox.Text, passBox.Text))
                {
                    teclado = true;
                    error.Close();
                    error.Dispose();
                    instrucciones instr = new instrucciones();
                    instr.Show();
                    FadeOut(this,20);
                    Process[] oskProcessArray = Process.GetProcessesByName("TabTip");
                    foreach (Process onscreenProcess in oskProcessArray)
                    {
                        onscreenProcess.Kill();
                    }
                }
            }
            else
            {
                teclado = false;
                error.Show();
            }
        }

        //boton acerca de
        private void button2_Click(object sender, EventArgs e)
        {
            acercaDe about = new acercaDe();
            about.Show();
        }

    #region Transiciones
        private async void FadeIn(Form o, int interval = 100)
        {
            //Object is not fully invisible. Fade it in
            while (o.Opacity < 1)
            {
                await Task.Delay(interval);
                o.Opacity += 0.05;
            }
            o.Opacity = 1; //make fully visible       
        }
        private async void FadeOut(Form o, int interval = 100)
        {
            //Object is fully visible. Fade it out
            while (o.Opacity > 0.0)
            {
                await Task.Delay(interval);
                o.Opacity -= 0.05;
            }
            o.Opacity = 0; //make fully invisible      
            this.Hide();
        }
#endregion
    }
}