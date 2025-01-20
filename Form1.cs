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

        /// <summary>
        /// Handle the on off button toggle of the calculator.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Event handler for digit buttons, appends digits to the calculation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            // If the user does not specify a different operation, it is multiplication
            InsertMultiplicationAfterBracket();
            // Append the digit to the calculation
            this.textBox1.Text += buttonDigit.Text;
        }

        /// <summary>
        /// Event handler for calculator operations.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOperation_Click(object sender, EventArgs e)
        {
            Button buttonOperation = (Button)sender;
            if (textBox1.Text.Length > 0)
            {
                textBox1.Text += $" {buttonOperation.Text} ";
            }
        }

        /// <summary>
        /// Event handler for calculator special operations keys.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSpecialOperation_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length >= 1)
            {
                float res = TrySpecialOperation(((Button)sender).Text, textBox1.Text);
                textBox1.Text = res.ToString();
            }
        }

        /// <summary>
        /// Event handler for memory operations.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonMemoryOperation_Click(object sender, EventArgs e)
        {
            float? res = TryMemoryOperation(((Button)sender).Text, textBox1.Text);
            
            // No result to show
            if (res == null)
                return;
            if (textBox1.Text == "0")
            {
                textBox1.Text = res.ToString();
                return;
            }

            // Different cases of adding memory value to the string
            char last = textBox1.Text[textBox1.Text.Length - 1];
            if (!char.IsDigit(last) && textBox1.Text.Length >= 1)
            {
                textBox1.Text += res.ToString();
            }
            // Automatically insert addition if the user recalls without operator
            else
            {
                textBox1.Text += $" + {res.ToString()}";
            }
        }

        /// <summary>
        /// Event handler for the decimal button click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Event handler for open bracket button click, adds multiplication as default operation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOpenBracket_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "0")
            {
                textBox1.Text = $"{((Button)sender).Text} ";
                return;
            }
            if (textBox1.Text.Length > 0 && (char.IsDigit(textBox1.Text.Last())))
            {
                textBox1.Text += " * ";
            }
            textBox1.Text += $"{((Button)sender).Text} ";
        }

        /// <summary>
        /// Event handler for close bracket button click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCloseBracket_Click(object sender, EventArgs e)
        {
            textBox1.Text += $" {((Button)sender).Text}";
        }

        /// <summary>
        /// Event handler for clear all input, should calculation and memory.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClearAll_Click(object sender, EventArgs e)
        {
            textBox1.Text = "0";
        }

        /// <summary>
        /// Event handler for backspace button, remove last digit/operation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// Event handler for button clear entry click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Event handler for equals button, initate calculation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonKeyCalculate_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                float result = TryCalculation(textBox1.Text);
                textBox1.Text = result.ToString();
                containsResults = true;
            }
        }

        /// <summary>
        /// Helper method, checks if ')' was last input and adds multiplication operation as default.
        /// </summary>
        private void InsertMultiplicationAfterBracket()
        {
            if (textBox1.Text.Length > 0 && textBox1.Text.Last() == ')')
            {
                textBox1.Text += " * ";
            }
        }

        /// <summary>
        /// Error handling for memory operations.
        /// </summary>
        /// <param name="memExpr"></param>
        /// <param name="calculationLiteral"></param>
        /// <returns></returns>
        private float? TryMemoryOperation(string memExpr, string calculationLiteral)
        {
            try
            {
                return calculator.MemoryOperation(memExpr, calculationLiteral);
            }
            catch (UnknownOperationException ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// Error handling for special calculations.
        /// </summary>
        /// <param name="calculationLiteral"></param>
        /// <returns></returns>
        private float TrySpecialOperation(string specialExpr, string calculationLiteral)
        {
            try
            {
                return calculator.SpecialCalculate(specialExpr, calculationLiteral);
            }
            catch (UnknownOperationException ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// Error handling for calculations.
        /// </summary>
        /// <param name="calculationLiteral"></param>
        /// <returns></returns>
        private float TryCalculation(string calculationLiteral)
        {
            try
            {
                return calculator.Calculate(calculationLiteral);
            }
            catch (CalculatorException ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
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
    }
}

