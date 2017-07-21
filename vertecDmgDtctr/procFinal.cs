using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using holaMundoOpenCV.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using vertecDmgDtctr.Clases;

namespace vertecDmgDtctr
{
    public partial class procFinal : Form
    {
        restServices rest = new restServices();
        Bitmap gif1 = new Bitmap("recursos/ultigif.gif");
        Bitmap f1 = new Bitmap("recursos/w3.png");
        private Capture captar;
        bool verificacion=false;
        Mat imgOriginal;
        conexion bd = new conexion();
        esperaUnMomento wait = new esperaUnMomento();
        public string[] description = {"least","thin", "regular", "serious", "extreme" };
        public string[] type = { "tablet", "cellphone"};

        string gettingImei = "";
        string gettingMarca = "";
        string gettingModelo = "";
        string gettingVersionDroid = "";
        string gettingFabricante = "";
        string gettingSerial = "";
        public procFinal(string imei, string marca, string modelo, string fabricante, string versionSo, string serie)
        {
            InitializeComponent();
            FadeIn(this,20);
            //Cerrar puerta
            SerialCom serial = new SerialCom();
            serial.SendCommand("cmd08a");
            Thread.Sleep(1000);
            //wait.Show();
            timer1.Start();
            timer1.Interval = 100;
            this.BackgroundImage = f1;
            pictureBox1.Image = gif1;
            gettingImei = imei;
            gettingMarca = marca;
            gettingModelo = modelo;
            gettingVersionDroid = versionSo;
            gettingFabricante = fabricante;
            gettingSerial = serie;
            label1.Text = "Información:\nMarca: " + marca + "\nModelo: " + modelo + "\nFabricante: : " + fabricante + "\nVersión S.O.: " + versionSo + "\nNo. Serie: " + serie + "\nImei: " + imei;
        }

        private void procFinal_Load(object sender, EventArgs e)
        {
            

            try
            {
                captar = new Capture(0); // Se hace la captura con la webcam 
                captar.SetCaptureProperty(CapProp.FrameWidth, 1920);
                captar.SetCaptureProperty(CapProp.FrameHeight, 1080);
                //wait.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se encuentra la webcam : " + Environment.NewLine + Environment.NewLine +
                                ex.Message + Environment.NewLine + Environment.NewLine +
                                "Saliendo del programa");
                Environment.Exit(0);
                return;
            }
            // Añade la funcion de procesado a la imagen para poder hacer las diferentes tareas con ella
            Application.Idle += processFrameAndUpdateGUI;
            //Application.EnterThreadModal += processFrameAndUpdateGUI;
        }

        public void processFrameAndUpdateGUI(object sender, EventArgs arg)
        {
            // Asigna la imagen original a lo que se esta captando en tiempo real
            imgOriginal = captar.QueryFrame();
            ibOriginal.Image = imgOriginal;   //Muestra la imagen original en el imageBox asignado
            if (imgOriginal == null)
            {
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }

        private void analisis_Click(object sender, EventArgs e)
        {
            //si el telefono sigue conectado
            if (verificacion == true)
            {
                analizarDisp(gettingImei, gettingFabricante, gettingMarca, gettingModelo, gettingVersionDroid, gettingSerial);
            }
        }

        #region Procesamiento de Imagen
        private void analizarDisp(string imei, string fabricante, string marca, string modelo, string versionSo, string serie)
        {

            ///Declaración de variables para realizar el procesado
            Mat imgConErode = new Mat(imgOriginal.Size, DepthType.Cv8U, 1);//Creación de un objeto tipo Mat para almacenar la imagen erosionada
            Mat imgConDilatacion = new Mat(imgOriginal.Size, DepthType.Cv8U, 1);//Creación de un objeto tipo Mat para almacenar la imagen dilatada
            Mat suavizadoGauss = new Mat(imgOriginal.Size, DepthType.Cv8U, 1);
            Mat canny = new Mat(imgOriginal.Size, DepthType.Cv8U, 1);
            Mat blur = new Mat(imgOriginal.Size, DepthType.Cv8U, 1);
            Mat inpArray = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(3, 3), new Point(-1, -1)); //Array opcional para usarse como parámetro en el método erode o dilate 
            Mat imgD1 = new Mat(imgOriginal.Size, DepthType.Cv8U, 1);//Creación de un objeto tipo Mat para almacenar la imagen erosionada
            Point Point1 = new Point(-1, 1);
            Size kernel = new Size(3, 3);
            
            //PROCESAMIENTO//
            Image<Gray, Byte> grayImage = imgOriginal.ToImage<Gray, Byte>();
            CvInvoke.GaussianBlur(grayImage, suavizadoGauss, kernel, .3); //1
            CvInvoke.Canny(suavizadoGauss, canny, 100, 100, 3);//2
            CvInvoke.Blur(canny, blur, kernel, Point1, BorderType.Default);//3
            CvInvoke.Dilate(blur, imgConDilatacion, inpArray, Point1, 1, BorderType.Default, new MCvScalar());//método para dilatar la imagen
            CvInvoke.Erode(imgConDilatacion, imgConErode, inpArray, Point1, 1, BorderType.Default, new MCvScalar(0, 0, 0));//método para erosionar la imagen
            ibDamage.Image = imgConErode;

            Image<Gray, Byte> imgNegative = imgConErode.ToImage<Gray, Byte>();
            Image<Gray, Byte> imgNegative2 = new Image<Gray, Byte>(grayImage.Width, grayImage.Height);
            Image<Gray, Byte> imgProcessed = new Image<Gray, Byte>(grayImage.Width, grayImage.Height);
            Image<Gray, Byte> imgRellenado = new Image<Gray, Byte>(grayImage.Width, grayImage.Height);
            imgNegative2 = imgNegative.Not();
            imgProcessed = grayImage.Sub(imgNegative);
            imgRellenado = imgProcessed.Dilate(15).Erode(12);
            //ibVoidFill.Image = imgRellenado;
            Mat canny1 = new Mat(imgOriginal.Size, DepthType.Cv8U, 1);
            CvInvoke.Canny(imgProcessed, canny1, 100, 100, 3);//2
            ibDamage.Image = canny1;
            Image<Gray, byte> notProcess = imgProcessed.Not();
            //ibDamage.Image = notProcess;
            Mat sumatoriaNegativa = notProcess.Mat;

            ///Fin del procesado///
            

            MCvScalar averageFill = new MCvScalar();
            MCvScalar stdFill = new MCvScalar();
            CvInvoke.MeanStdDev(imgRellenado, ref averageFill, ref stdFill, null);
            double dtsFill = stdFill.V0;
            double averageDouFill = averageFill.V0;

            MCvScalar averageDamage = new MCvScalar();
            MCvScalar stdDamage = new MCvScalar();
            CvInvoke.MeanStdDev(imgProcessed, ref averageDamage, ref stdDamage, null);
            double dtsDamage = stdDamage.V0;
            double averageDouDamage = averageDamage.V0;

            float entropyrell;
            float entropyprocess;
            entropyrell = ((float)entropy(imgRellenado));
            entropyprocess = ((float)entropy(imgProcessed));

            double valorq = sumatoria(grayImage.Mat);
            valorq = valorq / (grayImage.Width * grayImage.Height);

            double valor = sumatoria(imgRellenado.Mat);
            valor = valor / (imgRellenado.Width * imgRellenado.Height);

            double valor2 = sumatoria(imgProcessed.Mat);
            valor2 = valor2 / (imgProcessed.Width * imgProcessed.Height);


            int daño = Comparator((float)averageDouFill, (float)averageDouDamage, (float)dtsFill, (float)dtsDamage);

            string horaGuardado = DateTime.Now.ToString("ddMMyyyyHHmmss");

            imgProcessed.Save("Imagenes/Procesado" + horaGuardado + ".jpg");
            imgRellenado.Save("Imagenes/Rellenado" + horaGuardado + ".jpg");
            imgOriginal.Save("Imagenes/Original" + horaGuardado + ".jpg");

            //bd.GuardarInfo(gettingImei, gettingFabricante,gettingMarca, gettingModelo, gettingVersionDroid, gettingSerial, daño);
            //bd.GuardarImg(gettingImei, "Imagenes/Original" + horaGuardado + ".jpg", "Imagenes/Procesado" + horaGuardado +
            //    ".jpg", "Imagenes/Rellenado" + horaGuardado + ".jpg", (float)dtsDamage, (float)averageDouDamage, entropy(imgProcessed), (float)dtsFill, (float)averageDouFill, entropy(imgRellenado));

            User.timeOut = DateTime.Now.ToString("yyyyMMddHHmmss");
            resultado result = new resultado();
            rest.Send(User.user_id,type[1], description[0], User.timeIn, User.timeOut, daño, (float)averageDouDamage, (float)averageDouFill, entropy(imgProcessed), entropy(imgRellenado), gettingImei, gettingFabricante, gettingModelo, gettingVersionDroid, gettingSerial);


            //if (daño == 0)
            //{
            //    label2.Text = "La pantalla no tiene ningún daño!";
            //    result.lblPrecio.Text ="$1500";
            //}
            //else if (daño == 1)
            //{
            //    label2.Text = "La pantalla tiene algún daño!";
            //    result.lblPrecio.Text = "$270";
            //}
            result.Show();
            
        }
        private double sumatoria(Mat img)
        {
            double resultado = 0;
            MCvScalar res;
            res = CvInvoke.Sum(img);
            resultado = res.V0;
            return resultado;
        }
        float suma;
        private float entropy(Image<Gray, byte> img)
        {
            int numBins = 256;
            float imgEntropy = 0, prob;
            int[] histSize = { 0, 255 };
            float[] DenseHisto = new float[256];
            //calculating the histogram
            DenseHistogram hist = new DenseHistogram(numBins, new RangeF(0, 255));
            hist.Calculate(new Image<Gray, byte>[] { img }, true, null);
            hist.CopyTo(DenseHisto);
            for (int i = 0; i < DenseHisto.Length; i++)
            {
                suma += DenseHisto[i];
            }
            double epsilon = 1.19209e-7;
            for (int i = 1; i < numBins; i++)
            {
                prob = DenseHisto[i] / suma;
                if (prob < epsilon)
                    continue;
                imgEntropy += prob * ((float)Math.Log10(prob) / (float)Math.Log10(2));
            }
            return imgEntropy * -1;
        }
        private int Comparator(float averageDouFill, float averageDouDamage, float dtsFill, float dtsDamage)
        {
            int return1 = 0;
            if ((averageDouFill - averageDouDamage >= 4.4803) && (averageDouFill > averageDouDamage) && (dtsFill - dtsDamage >= 1.31474) && (dtsFill > dtsDamage))
            {
                return1 = 1;
            }
            return return1;
        }

#endregion

        //Se está checando constantemente si aún está conectado el dispositivo
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
                wait.FadeOut(wait,20);
                verificacion = true;
            }
            else
            {
                wait.negativo();
                wait.Show();
                verificacion = false;
            }
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
        #endregion
    }
}
