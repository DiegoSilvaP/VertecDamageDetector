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
    public partial class abrirPuerta : Form
    {
        public abrirPuerta()
        {
            InitializeComponent();
            FadeIn(this,20);
        }
        #region Transiciones
        public async Task FadeIn(Form o, int interval = 100)
        {
            //Object is not fully invisible. Fade it in
            while (o.Opacity < 0.75)
            {
                await Task.Delay(interval);
                o.Opacity += 0.05;
            }
            o.Opacity = 0.75; //make fully visible       
        }


        public async Task FadeOut(Form o, int interval = 100)
        {
            //Object is fully visible. Fade it out
            while (o.Opacity > 0.0)
            {
                await Task.Delay(interval);
                o.Opacity -= 0.05;
            }
            o.Opacity = 0; //make fully invisible      
            this.Close();
        }
        #endregion
    }
}
