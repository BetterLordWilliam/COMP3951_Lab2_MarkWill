using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COMP3951_Lab2_MarkWill
{
    internal class Calculator
    {
        // Operation strings mapped to functions to compute the values
        public Dictionary<string, Func<float, float, float>> CalculatorOperations = new Dictionary<string, Func<float, float, float>>()
        {
            { "+", (left, right) => left + right },
            { "-", (left, right) => left - right },
            { "*", (left, right) => left * right },
            { "/", (left, right) => left / right},
            { "%", (left, right) => left % right },
        };

        // Special operation strings mapped to functions to compute the values
        public Dictionary<string, Func<float, float>> CalculatorSpecialOperations = new Dictionary<string, Func<float, float>>()
        {
            { "1/x", (num) => 1/num },
            { "x^2", (num) => num * num },
            { "sqrt", (num) => (float) Math.Sqrt(num) }
        };

        // Operator prescidence dictionary
        public Dictionary<string, int> OperatorPrecedence = new Dictionary<string, int>()
        {
            { "+", 2 },
            { "-", 2 },
            { "*", 3},
            { "/", 3},
            { "%", 3},
            { "^", 4}
        };

        /// <summary>
        /// Constructs a new Calculator class instance.
        /// </summary>
        public Calculator() { }

        // Store previous calculation
        public float MemoryStore {  get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="addValue"></param>
        public void MemoryAddStore(float addValue)
        {
            MemoryStore += addValue;
        }

        /// <summary>
        /// Convert expression into postfix notation (reverse polish notation).
        /// 
        /// Implementation shunting yard.
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private string ReversePolish(string expression)
        {
            string output = "";
            string[] tokens = expression.Split(' ');
            Stack<string> operators = new Stack<string>();

            foreach (string token in tokens)
            {
                // Numbers go directly to the output
                if (float.TryParse(token, out float value))
                {
                    output += $"{value} ";
                }
                // Ensure operator order of precedence
                else if (CalculatorOperations.ContainsKey(token))
                {
                    // While there is an operator with higher precedence at the top of the stack
                    while (operators.Count() > 0 
                        && (operators.Peek() != "(" && operators.Peek() != ")")
                        && (OperatorPrecedence[operators.Peek()] > OperatorPrecedence[token]))
                    {
                        // Add the higher precedence operator to the output string
                        output += $"{operators.Pop()} ";
                    }
                    operators.Push(token);
                }
                // Ensure correct count of parenthesis
                // Ensure order of operations with parenthesis
                else if (token == "(") operators.Push(token);
                else if (token == ")")
                {
                    // Validate correct presence of parenthesis
                    while (operators.Peek() != "(")
                    {
                        if (operators.Count() == 0) Console.WriteLine("Invalid expression: parenthesis mismatch.");
                        output += $"{operators.Pop()} ";
                    }
                    if (operators.Count() > 0 && operators.Peek() == "(") operators.Pop();
                }
            }

            // Add remaining operators to the output string
            foreach (string opp in operators)
            {
                output += $"{opp} ";
            }

            return output;
        }

        /// <summary>
        /// Parses an expression.
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public float Calculate(string expression)
        {
            Console.WriteLine(expression);

            string rpn = ReversePolish(expression);
            string[] tokens = rpn.Split(' ');
            Stack<float> stack = new Stack<float>();

            // Simple parsing of a postfix notation mathematical expression
            foreach (string token in tokens)
            {
                // Token is a number
                if (float.TryParse(token, out float value))
                {
                    stack.Push(value);
                }
                // If this is an expression string compute with the two values on the stack
                if (CalculatorOperations.ContainsKey(token))
                {
                    float right = stack.Pop();
                    float left = stack.Pop();
                    stack.Push(CalculatorOperations[token](left, right));
                }
            }

            return stack.Pop();
        }

        /// <summary>
        /// Handle special calculations.
        /// 
        /// </summary>
        /// <param name="specExpr"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public float SpecialCalculate(string specExpr, string expression)
        {
            // Invalid special expression is given
            if (!CalculatorSpecialOperations.ContainsKey(specExpr))
                throw new ArgumentException("Invalid special expression.");

            // Evaluate the expression given first
            float res = Calculate(expression);
            float res_real = CalculatorSpecialOperations[specExpr](res);
            return res_real;
        }
    }
}
