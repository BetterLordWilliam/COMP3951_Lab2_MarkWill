using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COMP3951_Lab2_MarkWill
{
    public partial class Form1 : Form
    {

        private bool isOn = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void offOnButton_Click(object sender, EventArgs e)
        {
            isOn = !isOn;

            if (isOn)
            {
                offOnButton.Text = "OFF";
                textBox1.Enabled = true;
                tableLayoutPanel1.Enabled = true;
                tableLayoutPanel2.Enabled = true;
                tableLayoutPanel3.Enabled = true;
                tableLayoutPanel4.Enabled = true;
            }
            else
            {
                offOnButton.Text = "ON";
                textBox1.Enabled = false;
                tableLayoutPanel1.Enabled = false;
                tableLayoutPanel2.Enabled = false;
                tableLayoutPanel3.Enabled = false;
                tableLayoutPanel4.Enabled = false;
            }

        }
    }
}
