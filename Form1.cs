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
        private Calculator calculator = new Calculator();
        private bool isOn = false;
        private bool isNegativeSigned = false;
        private bool containsResults = false;

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
                textBox1.Text = "0";
                textBox1.Enabled = true;
                tableLayoutPanel1.Enabled = true;
                tableLayoutPanel2.Enabled = true;
                tableLayoutPanel3.Enabled = true;
                tableLayoutPanel4.Enabled = true;
            }
            else
            {
                offOnButton.Text = "ON";
                textBox1.Text = "0";
                textBox1.Enabled = false;
                tableLayoutPanel1.Enabled = false;
                tableLayoutPanel2.Enabled = false;
                tableLayoutPanel3.Enabled = false;
                tableLayoutPanel4.Enabled = false;
            }
        }

        private void buttonDigit_Click(object sender, EventArgs e)
        {           
            Button buttonDigit = (Button)sender;
            if (textBox1.Text.StartsWith("0"))
            {
                if (!(textBox1.Text.Contains(".")))
                {
                    textBox1.Text = textBox1.Text.Remove(0, 1);
                }
            }
            this.textBox1.Text += buttonDigit.Text;
        }

        private void buttonDecimal_Click(object sender, EventArgs e)
        {
            Button buttonDigit = (Button)sender;
            if (!this.textBox1.Text.Contains('.'))
            {
                this.textBox1.Text += buttonDigit.Text;
            }
        }

        // TODO:  If i press +/- button when its initially 0, i need to press an extra time to get the negative sign. Need to fix this - MP
        private void buttonPlusMinusSign_Click(object sender, EventArgs e)
        {
            isNegativeSigned = !isNegativeSigned;

            if (isNegativeSigned)
            {
                if (this.textBox1.Text != "0")
                {
                    this.textBox1.Text = this.textBox1.Text.Insert(0, "-");
                }
            }
            else
            {
                if (this.textBox1.Text.StartsWith("-"))
                {
                    this.textBox1.Text = this.textBox1.Text.Remove(0, 1);
                }
            }
        }

        private void buttonClearAll_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        //TODO: Fix bug where i need to backspace the "-" with no digits in order to get back to "0" - MP
        private void buttonBackspace_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length >= 1)
            {
                this.textBox1.Text = this.textBox1.Text.Remove(textBox1.Text.Length - 1, 1);
            }
            else
            {
                this.textBox1.Text = "0";
            }
        }

        /// <summary>
        /// Plus button of the calculator clicked (should do these in one group perchance).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonPlus_Click(object sender, EventArgs e)
        {
            // There are things in the textbox
            if (textBox1.Text.Length > 1)
            {
                // Key here is to be putting spaces in front and behind
                // Use spaces as a delimeter for parsing the equation string
                textBox1.Text += $" {((Button)sender).Text} ";
            }
        }

        /// <summary>
        /// Event handler for open bracket buttons.
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOpenBracket_Click(object sender, EventArgs e)
        {
            textBox1.Text += $"{((Button)sender).Text} ";
        }

        /// <summary>
        /// Event handler for close bracket button.
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCloseBracket_Click(object sender, EventArgs e)
        {
            textBox1.Text += $" {((Button)sender).Text}";
        }

        private void buttonSpecialOperation_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length >= 1)
            {
                try
                {
                    Console.WriteLine(((Button)sender).Text + " " + textBox1.Text);
                    float res = calculator.SpecialCalculate(((Button)sender).Text, textBox1.Text);
                    textBox1.Text = res.ToString();
                    containsResults = true;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// Calculate, initiates backed calculator logic.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonKeyCalculate_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length >= 1)
            {
                float res = calculator.Calculate(textBox1.Text);
                textBox1.Text = res.ToString();
                containsResults = true;
            }
        }

        private void buttonKeyMemAdd_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length >= 1)
            {
                try
                {
                    // Evaluate expression
                    float res = calculator.Calculate(textBox1.Text);
                    // Store in memory
                    calculator.MemoryAddStore(res);
                    textBox1.Text = res.ToString();
                    containsResults = true;
                }
                catch
                {

                }
            }
        }

        private void buttonKeyMemStore_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length >= 1)
            {
                try
                {
                    // Evaluate expression
                    float res = calculator.Calculate(textBox1.Text);
                    // Store in memory
                    calculator.MemoryStore = res;
                    textBox1.Text = res.ToString();
                    containsResults = true;
                }
                catch
                {

                }
            }
        }

        private void buttonKeyMemRecall_Click(object sender, EventArgs e)
        {
            textBox1.Text = calculator.MemoryStore.ToString();
        }

        private void buttonKeyMemClear_Click(object sender, EventArgs e)
        {
            calculator.MemoryStore = 0;
        }
    }
}
