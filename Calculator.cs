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
        public float PreviousCalculation {  get; set; }
        private Operand Calculation;

        /// <summary>
        /// 
        /// </summary>
        public Calculator()
        {
            Calculation = new Operand();
        }

        /// <summary>
        /// 
        /// </summary>
        public void MemoryClear()
        {
            PreviousCalculation = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="addValue"></param>
        public void MemoryAddStore(float addValue)
        {
            PreviousCalculation += addValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void CreateRightOperand(float value)
        {
            unsafe
            {
                Calculation.rightOperand = &value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void CreateLeftOperand(float value)
        {
            unsafe
            {
                Calculation.leftOperand = &value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationLiteral"></param>
        public void CreateOperation(string operationLiteral)
        {
            unsafe
            {
                Calculation.operation = &operationLiteral;
            }
        }

        public float Compute()
        {
            return 1.2f;
        }
    }


    unsafe struct Operand
    {
        public void* leftOperand;
        public void* rightOperand;
        public void* operation;
    };
}
