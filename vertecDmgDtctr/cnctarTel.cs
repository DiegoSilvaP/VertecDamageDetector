using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using vertecDmgDtctr.Clases;

namespace vertecDmgDtctr
{
    public partial class cnctarTel : Form
    {
        Bitmap f1 = new Bitmap("recursos/wallDoor.png");
        Bitmap f2 = new Bitmap("recursos/waitDevice.png");
        Bitmap f3 = new Bitmap("recursos/wallLoading.png");
        instrucciones instr = new instrucciones();
        String gettingImei = "";
        String gettingMarca = "";
        String gettingModelo = "";
        String gettingVersionDroid = "";
        String gettingFabricante = "";
        String gettingSerial = "";
        int timer = 0;

        public cnctarTel()
        {
            InitializeComponent();
            FadeIn(this, 10);
            timer2.Interval = 1000;
            timer2.Start();
        }

        //Está revisando constantemente si ya se conectó el disp.
        //esto, para pasar automáticamente a la pantalla del procesado
        private void timer1_Tick(object sender, EventArgs e)
        {
            string estado = DeviceInfo.StartADB("devices");
            char[] arreglo = (estado.TrimEnd('\n').TrimEnd('\r').TrimEnd('"').TrimEnd('\f').TrimEnd('\v').TrimEnd('/')
            .TrimEnd('\t').TrimEnd(' ').TrimEnd('^').ToCharArray());
            StringBuilder sb = new StringBuilder();
            for (int i = arreglo.Length - 8; i < arreglo.Length - 2; i++)
            {
                if (arreglo[i] != ' ')
                {
                    sb.Append(arreglo[i], 1);
                }
            }
            estado = sb.ToString();
            if (estado == "device")
            {
                timer1.Stop();
                this.BackgroundImage = f1;
                DeviceInfo.StartADB("devices");
                datosTel();
            }
        }

        public void datosTel()
        {
            this.BackgroundImage = f3;
            //se obtiene la versión de android
            gettingVersionDroid = DeviceInfo.StartADB("shell getprop ro.build.version.release");

            double versionDeSO = 0;
            string version = gettingVersionDroid.Substring(0, 3);
            versionDeSO = double.Parse(version);

            //como de android 5.1 en adelante, pide permisos para usar partes del sistema
            //se decide en base a la version, cómo vamos a obtener los datos

            if (versionDeSO > 5.1)
            {
                //se instala e inicia la app, se dan toques en la pantalla para aceptar los permisos (android 5.1) y se obtienen los datos
                DeviceInfo.StartADB("install whitescreensettings.apk");
                DeviceInfo.StartADB("shell am start -n com.kronos.whitescreen/.MainActivity");
                DeviceInfo.StartADB("shell input keyevent 22");
                DeviceInfo.StartADB("shell input keyevent 22");
                DeviceInfo.StartADB("shell input keyevent 66");
                gettingMarca = DeviceInfo.StartADB("shell getprop ro.product.brand");
                gettingModelo = DeviceInfo.StartADB("shell getprop ro.product.model");
                gettingFabricante = DeviceInfo.StartADB("shell getprop ro.product.manufacturer");
                gettingSerial = DeviceInfo.StartADB("shell getprop ro.serialno");
                gettingImei = DeviceInfo.StartADB("shell am broadcast -a com.kronos.whitescreen.GET_IMEI");
            }
            else
            {
                DeviceInfo.StartADB("install whitescreensettings.apk");
                DeviceInfo.StartADB("shell am start -n com.kronos.whitescreen/.MainActivity");
                gettingMarca = DeviceInfo.StartADB("shell getprop ro.product.brand");
                gettingModelo = DeviceInfo.StartADB("shell getprop ro.product.model");
                gettingFabricante = DeviceInfo.StartADB("shell getprop ro.product.manufacturer");
                gettingSerial = DeviceInfo.StartADB("shell getprop ro.serialno");
                gettingImei = DeviceInfo.StartADB("shell am broadcast -a com.kronos.whitescreen.GET_IMEI");

            }

            //se tiene que cortar una parte de la cadena del imei, para que sólo obtengamos dicho dato
            char[] arreglos = (gettingImei.TrimEnd('\n').TrimEnd('\r').TrimEnd('"').ToCharArray());
            StringBuilder sba = new StringBuilder();
            for (int i = arreglos.Length - 15; i < arreglos.Length; i++)
            {
                if (arreglos[i] != ' ')
                {
                    sba.Append(arreglos[i], 1);
                }
            }

            gettingImei = sba.ToString();

            procFinal form1 = new procFinal(gettingImei, gettingMarca, gettingModelo, gettingFabricante, gettingVersionDroid, gettingSerial);
            form1.Show();
            FadeOut(this, 10);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer++;
            if (timer == 7)
            {
                this.BackgroundImage = f2;
                timer1.Interval = 100;
                timer1.Start();
                timer2.Stop();
            }
        }

        //cambia la imagen de "espera la puerta" a "conecta el telefono"

        #region Transiciones
        private async void FadeIn(Form o, int interval = 100)
        {
            //Object is not fully invisible. Fade it in
            while (o.Opacity < 0.75)
            {
                await Task.Delay(interval);
                o.Opacity += 0.05;
            }
            o.Opacity = 0.75; //make fully visible       
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
            this.Close();
            this.Dispose();
            instr.Dispose();
            instr.Close();
        }
        #endregion

        
    }
}
