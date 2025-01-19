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
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
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
            if (textBox1.Text == "0")
            {
                if (!(textBox1.Text.Contains(".")))
                {
                    textBox1.Text = textBox1.Text.Remove(0, 1);
                }
            }
            InsertMultiplicationAfterBracket();
            this.textBox1.Text += buttonDigit.Text;
        }

        private void buttonDecimal_Click(object sender, EventArgs e)
        {
            Button buttonDecimal = (Button)sender;
            string[] parts = textBox1.Text.Split(' ');

            // Check the last part of the expression to see if it already contains a decimal point
            if (!parts.Last().Contains('.'))
            {
                textBox1.Text += buttonDecimal.Text;
            }
        }

        private void buttonPlusMinusSign_Click(object sender, EventArgs e)
        {
            isNegativeSigned = !isNegativeSigned;

            if (isNegativeSigned)
            {
                if (this.textBox1.Text != "0" && !this.textBox1.Text.StartsWith("-"))
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
            textBox1.Text = "0";
        }

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

        private void buttonOperation_Click(object sender, EventArgs e)
        {
            Button buttonOperation = (Button)sender;
            if (textBox1.Text.Length > 0)
            {
                textBox1.Text += $" {buttonOperation.Text} ";
            }
        }

        private void buttonOpenBracket_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0 && (char.IsDigit(textBox1.Text.Last())))
            {
                textBox1.Text += " * ";
            }
            textBox1.Text += $"{((Button)sender).Text} ";
        }

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

        private void buttonKeyCalculate_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                float result = calculator.Calculate(textBox1.Text);
                textBox1.Text = result.ToString();
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
            textBox1.Text += calculator.MemoryStore.ToString();
        }

        private void buttonKeyMemClear_Click(object sender, EventArgs e)
        {
            calculator.MemoryStore = 0;
        }

        private void buttonKeyClearEntry_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                int lastSpaceIndex = textBox1.Text.LastIndexOf(' ');
                if (lastSpaceIndex != -1)
                {
                    textBox1.Text = textBox1.Text.Substring(0, lastSpaceIndex).TrimEnd();
                }
                else
                {
                    textBox1.Text = "0";
                }
            }
        }

        //TODO: MP: Need to figure out how to stop turning the calculator on and off when enter is returned.
        //TODO: MP: There is a big bug if the user uses the keyboard to enter something and then clicks on the textbox to enter something (try it out)
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (isOn)
            {
                bool shiftPressed = e.Shift;

                switch (e.KeyCode)
                {
                    case Keys.D0:
                    case Keys.NumPad0:
                        buttonDigit_Click(buttonKey0, e);
                        break;
                    case Keys.D1:
                    case Keys.NumPad1:
                        buttonDigit_Click(buttonKey1, e);
                        break;
                    case Keys.D2:
                    case Keys.NumPad2:
                        buttonDigit_Click(buttonKey2, e);
                        break;
                    case Keys.D3:
                    case Keys.NumPad3:
                        buttonDigit_Click(buttonKey3, e);
                        break;
                    case Keys.D4:
                    case Keys.NumPad4:
                        buttonDigit_Click(buttonKey4, e);
                        break;
                    case Keys.D5:
                    case Keys.NumPad5:
                        buttonDigit_Click(buttonKey5, e);
                        break;
                    case Keys.D6:
                    case Keys.NumPad6:
                        buttonDigit_Click(buttonKey6, e);
                        break;
                    case Keys.D7:
                    case Keys.NumPad7:
                        buttonDigit_Click(buttonKey7, e);
                        break;
                    case Keys.D8:
                        if (shiftPressed)
                            buttonOperation_Click(buttonKeyMultiply, e);
                        else
                            buttonDigit_Click(buttonKey8, e);
                        break;
                    case Keys.NumPad8:
                        buttonDigit_Click(buttonKey8, e);
                        break;
                    case Keys.D9:
                    case Keys.NumPad9:
                        buttonDigit_Click(buttonKey9, e);
                        break;
                    case Keys.Add:
                        buttonOperation_Click(buttonKeyAdd, e);
                        break;
                    case Keys.Subtract:
                        buttonOperation_Click(buttonKeySubtract, e);
                        break;
                    case Keys.Multiply:
                        buttonOperation_Click(buttonKeyMultiply, e);
                        break;
                    case Keys.Divide:
                        buttonOperation_Click(buttonKeyDivide, e);
                        break;
                    case Keys.Decimal:
                        buttonDecimal_Click(buttonKeyDecimal, e);
                        break;
                    case Keys.Return:
                        buttonKeyCalculate_Click(buttonKeyCalculate, e);
                        break;
                    case Keys.Back:
                        buttonBackspace_Click(buttonKeyBackspace, e);
                        break;
                    case Keys.Delete:
                        buttonClearAll_Click(buttonKeyClearAll, e);
                        break;
                }
            }
        }


        private void InsertMultiplicationAfterBracket()
        {
            if (textBox1.Text.Length > 0 && textBox1.Text.Last() == ')')
            {
                textBox1.Text += " * ";
            }
        }
    }
}

