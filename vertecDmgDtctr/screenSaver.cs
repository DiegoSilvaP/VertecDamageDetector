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
    public partial class screenSaver : Form
    {
        public screenSaver()
        {
            InitializeComponent();
            FadeIn(this, 20);
            Bitmap fondo = new Bitmap("recursos/wall3d.gif");
            pictureBox1.Image = fondo;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            logueo inicio = new logueo();
            inicio.Show();
            FadeOut(this, 20);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

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
    }
}
