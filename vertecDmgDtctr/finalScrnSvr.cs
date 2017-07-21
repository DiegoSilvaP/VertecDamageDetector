using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace vertecDmgDtctr
{
    public partial class finalScrnSvr : Form
    {
        Bitmap f1 = new Bitmap("recursos/wall3d2.gif");
        int _timer=0;
        public finalScrnSvr()
        {
            InitializeComponent();
            FadeIn(this,20);
            timer1.Start();
            timer1.Interval = 1000;
            pictureBox1.Image = f1;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _timer++;
            if (_timer==8)
            {
                timer1.Stop();
                Application.Restart();
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
