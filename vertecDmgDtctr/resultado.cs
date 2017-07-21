using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using holaMundoOpenCV.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace vertecDmgDtctr
{
    public partial class resultado : Form
    {
        private Capture captar;
        private Bitmap originalBitmap = null, resultBitmap = null;
        private Mat imgCanny, imgOriginal, imgSobel, imgLaplace, imgPrewitt, imgKirch;
        bool telefono;
        SerialCom serial = new SerialCom();
        TelefonoEnCabina frm1 = new TelefonoEnCabina();
        NoTelefonoCabina frm2 = new NoTelefonoCabina();
        public resultado()
        {
            InitializeComponent();
            FadeIn(this, 20);
        }



        private void iniciarCamara()
        {
            try
            {
                //Se intenta obtener imagen en tiempo real desde la camara 0
                captar = new Capture(0);
                captar.SetCaptureProperty(CapProp.FrameWidth, 1280);
                captar.SetCaptureProperty(CapProp.FrameHeight, 720);
            }
            catch (Exception ex)
            {
                return;
            }
            Application.Idle += revisarTelefono;       // add process image function to the application's list of tasks
        }

        public void revisarTelefono(object sender, EventArgs arg)
        {
            // Asigna la imagen original a lo que se esta captando en tiempo real
            imgOriginal = captar.QueryFrame();
            if (imgOriginal == null)
            {
                //MessageBox.Show("No se puede leer la webcam" + Environment.NewLine + Environment.NewLine + "Saliendo del programa");
                //Environment.Exit(0);
                return;
            }
            imgCanny = new Mat(imgOriginal.Size, DepthType.Cv8U, 1); //Inicialización del objeto tipo Mat
            CvInvoke.Canny(imgOriginal, imgCanny, 200, 200);     //Aplacación del filtro
            //ibOriginal.Image = imgOriginal;     //Muestra la imagen original en el imageBox asignado
            //ibCanny.Image = imgCanny;

            //Objeto de tipo MCvScalar que nos servirá para obtener la media de la imagen
            MCvScalar media = new MCvScalar();
            //Objeto de tipo MCvScalar que nos servirá para obtener la desviacion estandar de la imagen
            MCvScalar desviacion = new MCvScalar();
            //La función MeanStdDev realiza los calculos que nos devolverán la desviacion y la media
            CvInvoke.MeanStdDev(imgCanny, ref media, ref desviacion, null);
            double dts1 = desviacion.V0;
            double averageDou1 = media.V0;
            //textBox1.Text = dts1.ToString();
            //textBox2.Text = averageDou1.ToString();


            ///checar si el telefono está dentro
            //El teléfono está
            if (dts1 >= 10.0)
            {
                Console.WriteLine("Gracias, el telefono está en la cabina");
                FadeOut(this, 20);
                frm1.Show();
                frm2.Hide();
                serial.SendCommand("cmd02a");//verificar plataforma
                Thread.Sleep(1000);
                serial.SendCommand("cmd05a");
                Thread.Sleep(1000);
                frm1.FadeOut(frm1, 20);
                finalScrnSvr final = new finalScrnSvr();
                final.Show();
                Application.Idle -= revisarTelefono;
            }
            //el telefono no está
            else if (dts1 <= 2.0)
            {
                frm2.Show();
                frm1.Hide();

                Console.WriteLine("coloque el telefono en la cabina");
                Application.Idle -= revisarTelefono;
            }

        }

        //Aceptar el dinero a cambio del telefono
        private async void button2_Click(object sender, EventArgs e)
        {
            abrirPuerta frm3 = new abrirPuerta();
            DesconectarDisp frm5 = new DesconectarDisp();
            frm3.Show();
            await frm3.FadeIn(frm3, 20);
            serial.SendCommand("cmd09a");//abrir puerta
            Thread.Sleep(2000);

            await frm3.FadeOut(frm3, 20);

            frm5.Show();
            await frm5.FadeIn(frm5,20);
            //Pedir a usuario retirar cable
            serial.SendCommand("cmd08a");//cerrar puerta
            Thread.Sleep(2000);
            //En pantalla mensale: "Desconecta el cable y manten la pantalla encendida"
            //////Verificar si el telefono está

            //Para verificar si él usuario dejó el teléfono en la cabina
            //hay que volver a hacer un pequeño procesado de imagen
            
            iniciarCamara();

            await frm5.FadeOut(frm5, 20);

        }

        private async void button3_Click(object sender, EventArgs e)
        {
            abrirPuerta frm3 = new abrirPuerta();
            frm3.Show();
            await frm3.FadeIn(frm3, 20);
            FadeOut(this, 20);
            SerialCom serial = new SerialCom();
            serial.SendCommand("cmd09a");//Abrir
            Thread.Sleep(1000);
            //Pedir que se llever su @%&....
            await frm3.FadeOut(frm3, 20);
            //si no detecta un disp.
            serial.SendCommand("cmd08a");//Cerrar puerta
            Thread.Sleep(1000);
            retirarDispositivo frm4 = new retirarDispositivo();
            frm4.Show();
            await frm4.FadeIn(frm4, 20);

            Thread.Sleep(1000);
            await frm4.FadeOut(frm4, 20);
            finalScrnSvr final = new finalScrnSvr();
            final.Show();
        }

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
        }
        #endregion

    }
}
