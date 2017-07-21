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
    public partial class acercaDe : Form
    {
        Bitmap f1 = new Bitmap("recursos/aboutWall.png");
        public acercaDe()
        {
            InitializeComponent();
            this.BackgroundImage = f1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
