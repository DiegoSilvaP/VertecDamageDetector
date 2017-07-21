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
    public partial class esperaUnMomento : Form
    {
        Bitmap f1 = new Bitmap("recursos/waitDevice.png");
        public esperaUnMomento()
        {
            FadeIn(this,20);
            InitializeComponent();
        }
        public void negativo()
        {
            this.BackgroundImage = f1;
            FadeIn(this,20);
        }

        #region Transiciones
        public async void FadeIn(Form o, int interval = 100)
        {
            //Object is not fully invisible. Fade it in
            while (o.Opacity < 0.75)
            {
                await Task.Delay(interval);
                o.Opacity += 0.05;
            }
            o.Opacity = 0.75; //make fully visible       
        }


        public async void FadeOut(Form o, int interval = 100)
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
