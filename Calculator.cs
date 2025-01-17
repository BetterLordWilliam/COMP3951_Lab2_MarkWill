using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace COMP3951_Lab2_MarkWill
{
    internal class Calculator
    {
        /// <summary>
        /// Store information about a calculation.
        /// 
        /// Idea is to use this in order to computer order of operations down the line.
        /// </summary>
        struct Calculation
        {
            /// <summary>
            /// Construct a Calculation object.
            /// </summary>
            /// <param name="calculationLiteral"></param>
            /// <param name="calculationValue"></param>
            public Calculation(string calculationLiteral, float calculationValue)
            {
                CalculationLiteral = calculationLiteral;
                CalculationValue = calculationValue;
            }

            public string CalculationLiteral { get; }
            public float CalculationValue { get; set; }
        };

        public float PreviousCalculation {  get; set; }
        public float Operand1 { get; set; }
        public float Operand2 { get; set; }

        public void MemoryClear()
        {
            PreviousCalculation = 0;
        }

        public void MemoryAddStore(float addValue)
        {
            PreviousCalculation += addValue;
        }
    }
}
