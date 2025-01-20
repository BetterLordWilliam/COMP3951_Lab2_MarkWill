using System;
using System.Collections.Generic;
using System.Linq;

///
/// Lab 2, Will Otterbein, Mark P.
/// Date, January 19th 2025
/// Revision, 1
///
namespace COMP3951_Lab2_MarkWill
{
    /// <summary>
    /// Class representing an unknown operation exception, occurs when the calculator does not know an input operation.
    /// </summary>
    internal class UnknownOperationException : Exception
    { 
        /// <summary>
        /// Constructs an unknown operation exception.
        /// </summary>
        public UnknownOperationException() { }

        /// <summary>
        /// Consturcts an unknown operation exception, with a message.
        /// </summary>
        /// <param name="message"></param>
        public UnknownOperationException(string message) : base(message)
        {
        }
        
        /// <summary>
        /// Constructs an unknown operation exception, with a message and inner exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public UnknownOperationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class CalculatorException : Exception
    {
        /// <summary>
        /// Constructs a calculator exception.
        /// </summary>
        public CalculatorException()
        {
        }

        /// <summary>
        /// Constructs a calculator exception with a message.
        /// </summary>
        /// <param name="message"></param>
        public CalculatorException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructs a calculator exception with a message and an inner exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public CalculatorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Class that represents a simple calculator.
    /// </summary>
    internal class Calculator
    {
        // Delegate for calculator operations
        public delegate object CalcMemOperation(params object[] args);
        // Store previous calculation, null if cleared
        private float? memoryStore = null;

        // Operation strings mapped to functions to compute the values
        public Dictionary<string, Func<float, float, float>> CalculatorOperations;
        // Known single argument
        public Dictionary<string, Func<float, float>> CalculatorSpecialOperations;
        // Unknown multiple arguments
        public Dictionary<string, CalcMemOperation> CalculatorMemoryOperations;

        // Operator prescidence dictionary
        private Dictionary<string, int> operatorPrecedence = new Dictionary<string, int>()
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
        public Calculator()
        {
            CalculatorOperations = new Dictionary<string, Func<float, float, float>>()
            {
                { "+", (left, right) => left + right },
                { "-", (left, right) => left - right },
                { "*", (left, right) => left * right },
                { "/", (left, right) => left / right},
                { "%", (left, right) => left % right },
            };
            CalculatorSpecialOperations = new Dictionary<string, Func<float, float>>()
            {
                { "1/x", (num) => 1/num },
                { "x^2", (num) => num * num },
                { "sqrt", (num) => (float) Math.Sqrt(num) },
                { "+/-", (num) => -1 * num}
            };
            CalculatorMemoryOperations = new Dictionary<string, CalcMemOperation>
            {
                { "MR", args => memoryStore },
                { "MS", args => { memoryStore = Convert.ToSingle(args[0]); return null; } },
                { "M+", args =>
                    {
                        // Just set the memory if it is null
                        if (memoryStore != null)
                            memoryStore += Convert.ToSingle(args[0]);
                        else
                            memoryStore = Convert.ToSingle(args[0]);
                        return null;
                    }
                   
                },
                { "MC", args => { memoryStore = null; return null; } }
            };
        }

        /// <summary>
        /// Convert expression into postfix notation (reverse polish notation).
        /// 
        /// Implementation of shunting yard.
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
                    output += $"{value} ";
                // Ensure operator order of precedence
                else if (CalculatorOperations.ContainsKey(token))
                {
                    // While there is an operator with higher precedence at the top of the stack
                    while (operators.Count() > 0
                        && (operators.Peek() != "(" && operators.Peek() != ")")
                        && (operatorPrecedence[operators.Peek()] > operatorPrecedence[token]))
                    {
                        // Add the higher precedence operator to the output string
                        output += $"{operators.Pop()} ";
                    }
                    operators.Push(token);
                }
                // Ensure correct count of parenthesis
                // Ensure order of operations with parenthesis
                else if (token == "(")
                    operators.Push(token);
                else if (token == ")")
                {
                    // Validate correct presence of parenthesis
                    while (operators.Peek() != "(")
                    {
                        if (operators.Count() == 0)
                            throw new CalculatorException("Invalid expression, parenthesis mismatch.");
                        output += $"{operators.Pop()} ";
                    }
                    if (operators.Count() > 0 && operators.Peek() == "(")
                        operators.Pop();
                }
            }

            // Add remaining operators to the output string
            foreach (string opp in operators)
                output += $"{opp} ";

            return output;
        }

        /// <summary>
        /// Parses an expression.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public float Calculate(string expression)
        {
            Console.WriteLine($"Given expression {expression}");

            try
            {
                string rpn = ReversePolish(expression);
                string[] tokens = rpn.Split(' ');
                Stack<float> stack = new Stack<float>();

                // Simple parsing of a postfix notation mathematical expression
                foreach (string token in tokens)
                {
                    // Token is a number
                    if (float.TryParse(token, out float value))
                        stack.Push(value);
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
            catch (Exception)
            {
                throw new CalculatorException("Bad expression.");
            }
        }

        /// <summary>
        /// Handle special calculations.
        /// </summary>
        /// <param name="specExpr"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public float SpecialCalculate(string specExpr, string expression)
        {
            Console.WriteLine($"Special expression {specExpr}, given expression {expression}");

            // Invalid special expression is given
            if (!CalculatorSpecialOperations.ContainsKey(specExpr))
                throw new UnknownOperationException("Invalid special expression.");

            // Evaluate the expression given first
            float res = Calculate(expression);
            float res_real = CalculatorSpecialOperations[specExpr](res);
            return res_real;
        }

        /// <summary>
        /// Handle memory operations.
        /// </summary>
        /// <param name="memExpr"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        /// <exception cref="UnknownOperationException"></exception>
        public float? MemoryOperation(string memExpr, string expression)
        {
            Console.WriteLine($"Memory expression {memExpr}, given expression {expression}");

            // Invalid memory expression is given
            if (!CalculatorMemoryOperations.ContainsKey(memExpr))
                throw new UnknownOperationException("Invalid memory expression");

            // Evaluate the expression given first
            float res = 0;
            if (memExpr != "MR")
                res = Calculate(expression);
            // Will return float if such, else returns null
            return (float?) CalculatorMemoryOperations[memExpr](res);
        }
    }
}