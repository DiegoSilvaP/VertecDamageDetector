using holaMundoOpenCV.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace vertecDmgDtctr
{
    public partial class instrucciones : Form
    {
        #region declaración de imagenes
        Bitmap f1 = new Bitmap("recursos/w2.1.png");
        Bitmap gif = new Bitmap("recursos/adb.gif");
        Bitmap gif1 = new Bitmap("recursos/usbgif.gif");
        Bitmap img1 = new Bitmap("recursos/instrEscr/1.png");
        Bitmap img2 = new Bitmap("recursos/instrEscr/2.png");
        Bitmap img3 = new Bitmap("recursos/instrEscr/3.png");
        Bitmap img4 = new Bitmap("recursos/instrEscr/4.png");
        Bitmap img5 = new Bitmap("recursos/instrEscr/5.png");
        Bitmap img6 = new Bitmap("recursos/instrEscr/6.png");
        Bitmap img7 = new Bitmap("recursos/instrEscr/7.png");
        Bitmap img8 = new Bitmap("recursos/instrEscr/8.png");
        Bitmap anim1 = new Bitmap("recursos/anim/anim1.gif");
        Bitmap anim2 = new Bitmap("recursos/anim/anim2.gif");
        Bitmap anim3 = new Bitmap("recursos/anim/anim3.gif");
        Bitmap anim4 = new Bitmap("recursos/anim/anim4.gif");
        Bitmap anim5 = new Bitmap("recursos/anim/anim5.gif");
        Bitmap anim6 = new Bitmap("recursos/anim/anim6.gif");
        Bitmap anim7 = new Bitmap("recursos/anim/anim7.gif");
        Bitmap anim8 = new Bitmap("recursos/anim/conect.gif");
#endregion

        int segundero = 0;
        int indiceImage = 0;
        public instrucciones()
        {
            InitializeComponent();
            FadeIn(this, 20);
            User.timeIn = DateTime.Now.ToString("yyyyMMddHHmmss");

            pictureBox1.Image = anim1;
            pictureBox2.Image = gif1;
            timer1.Enabled = true;
            this.BackgroundImage = f1;
            this.DoubleBuffered = true;
            pbEscrito.Image = img1;
            timer2.Start();
            timer2.Interval = 1000;
            //indiceImage++;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            pictureBox1.Image = gif;
        }

        private void instrucciones_Load_1(object sender, EventArgs e)
        {
            timer1.Interval = 100;
            timer1.Tick += new EventHandler(timer1_Tick);
            cnctarTel enEspera = new cnctarTel();
            enEspera.Show();
        }

        //Se comprueba constantemente la existencia del form
        private void timer1_Tick(object sender, EventArgs e)
        {
            Form existe = Application.OpenForms.OfType<Form>().Where(pre => pre.Name == "cnctarTel").SingleOrDefault<Form>();
            if (existe == null)
            {
                FadeOut(this,20);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }
        //Temporizador encargado del cambio automático de las animaciones
        private void timer2_Tick(object sender, EventArgs e)
        {

            if (segundero == 6 && indiceImage == 0)
            {
                pbEscrito.Visible = true;
                pictureBox1.Visible = true;
                button1.Visible = true;
                button2.Visible = true;
                //pictureBox2.Visible = true;
                pbEscrito.Image = img1;
                pictureBox1.Image = anim1;
                pictureBox2.Image = gif1;
                indiceImage++;
            }
            if (segundero == 16 && indiceImage == 1)
            {
                pbEscrito.Image = img2;
                pictureBox1.Image = anim2;
                indiceImage++;
            }
            if (segundero == 27 && indiceImage == 2)
            {
                pbEscrito.Image = img3;
                pictureBox1.Image = anim3;
                indiceImage++;
            }
            if (segundero == 44 && indiceImage == 3)
            {
                pbEscrito.Image = img4;
                pictureBox1.Image = anim4;
                indiceImage++;
            }
            if (segundero == 53 && indiceImage == 4)
            {
                pbEscrito.Image = img5;
                pictureBox1.Image = anim5;
                indiceImage++;
            }
            if (segundero == 63 && indiceImage == 5)
            {
                pbEscrito.Image = img6;
                pictureBox1.Image = anim6;
                indiceImage++;
            }
            if (segundero == 78 && indiceImage == 6)
            {
                pbEscrito.Image = img7;
                pictureBox1.Image = anim7;
                indiceImage++;
            }
            if (segundero == 86 && indiceImage == 7)
            {
                pbEscrito.Image = img8;
                pictureBox1.Image = anim8;
            }
            if (segundero == 96)
            {
                indiceImage = 0;
                segundero = 5;
            }
            segundero++;
        }

        //botón atras; regresa un punto anterior en el tutorial
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (segundero >= 6 && segundero < 16)
            {
                segundero = 6;
                indiceImage = 0;
            }
            else if (segundero >= 16 && segundero < 27)
            {
                segundero = 6;
                indiceImage = 0;
            }
            else if (segundero >= 27 && segundero < 44)
            {
                segundero = 16;
                indiceImage = 1;
            }
            else if (segundero >= 44 && segundero < 53)
            {
                segundero = 27;
                indiceImage = 2;
            }
            else if (segundero >= 53 && segundero < 63)
            {
                segundero = 44;
                indiceImage = 3;
            }
            else if (segundero >= 63 && segundero < 78)
            {
                segundero = 53;
                indiceImage = 4;
            }
            else if (segundero >= 78 && segundero < 86)
            {
                segundero = 63;
                pbEscrito.Image = img7;
                indiceImage = 5;
            }
            else if (segundero >= 86 && segundero < 96)
            {
                segundero = 78;
                indiceImage = 6;
            }
        }

        //botón adelante; avanza un punto en el tutorial
        private void button2_Click_1(object sender, EventArgs e)
        {
            if (segundero >= 6 && segundero < 16)
            {
                segundero = 16;
                indiceImage = 1;
            }
            else if (segundero >= 16 && segundero < 27)
            {
                segundero = 27;
                indiceImage = 2;
            }
            else if (segundero >= 27 && segundero < 44)
            {
                segundero = 44;
                indiceImage = 3;
            }
            else if (segundero >= 44 && segundero < 53)
            {
                segundero = 53;
                indiceImage = 4;
            }
            else if (segundero >= 53 && segundero < 63)
            {
                segundero = 63;
                indiceImage = 5;
            }
            else if (segundero >= 63 && segundero < 78)
            {
                segundero = 78;
                indiceImage = 6;
            }
            else if (segundero >= 78 && segundero < 86)
            {
                segundero = 86;
                indiceImage = 7;
            }
            else if (segundero >= 86 && segundero < 96)
            {
                segundero = 96;
                indiceImage = 8;
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

        private async void FadeOut(Form o, int interval = 100)
        {
            //Object is fully visible. Fade it out
            while (o.Opacity > 0.0)
            {
                await Task.Delay(interval);
                o.Opacity -= 0.05;
            }
            o.Opacity = 0; //make fully invisible      
            this.Dispose();
        }

        #endregion
    }
}
