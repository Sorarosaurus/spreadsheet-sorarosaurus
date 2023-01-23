using System.Text.RegularExpressions;
using System.Threading.Tasks;
/// <summary>
/// Author:    Sora Roberts
/// Partner:   None
/// Date:      1/23/23
/// Course:    CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Sora Roberts - This work may not 
///            be copied for use in Academic Coursework.
///
/// I, Sora Roberts, certify that I wrote this code from scratch and
/// did not copy it in part or whole from another source.  All 
/// references used in the completion of the assignments are cited 
/// in my README file.
///
/// File Contents
///
///    This library is a reference for future programs that will
///    need the ability to compute valid formulas.
/// </summary>
namespace FormulaEvaluator
{
    /// <summary>
    /// 
    /// </summary>
    public class Evaluator
    {
        /// <summary>
        /// A delegate which allows library users to store the values of variables using the name of the variable.
        /// </summary>
        /// 
        /// <param name="variable_name">
        /// The name of the variable, which must be in the format:
        /// >0 capital or lowercase letters, followed by >0 numbers.
        /// </param>
        /// <returns>The integer value of the variable.</returns>
        /// <exception cref="ArgumentException">An invalid expression was passed in, and evaluation could not finish.</exception>
        public delegate int Lookup(String variable_name);


        //--------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// A method which evaluates the input expression,
        /// optionally using variable values as assigned by the Lookup delegate.
        /// It will throw an exception if any invalid inputs or algebra occur.
        /// </summary>
        /// 
        /// <param name="expression"> The expression to be evaluated.
        /// The only valid substrings include +, -, /, *, (, ), non-negative integers, whitespace,
        /// and variables of 1+ letters then 1+ digits. No implicit multiplication. </param>
        /// <param name="variableEvaluator"> A delegate which will allow the evaluator
        /// to lookup a variable's value. </param>
        /// <returns> The integer solution to the expression. </returns
        /// <exception cref="ArgumentException">An invalid expression was passed in, and evaluation could not finish.</exception>
        public static int Evaluate(string expression,
                                   Lookup variableEvaluator)
        {
            //turn the given expression string into an array of acceptable tokens
            string[] substrings = Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            // EVALUATE THE SUBSTRINGS ARRAY USING 2 STACKS
            Stack<string> valueStack = new Stack<string>();
            Stack<string> operatorStack = new Stack<string>();
            string currentToken;

            // Token by token, left to right, evaluate which token it is and proceed with the given algorithm using the empty stacks above.
            // (throw error if not a valid token, and ignore whitespace)
            for (int i = 0; i < substrings.Count(); i++)
            {
                currentToken = substrings[i];

                // ignore empty strings and leading/trailing whitespace
                if (currentToken.Length == 0 ||
                    Regex.IsMatch(currentToken, @"^\s+$"))
                    continue;

                // if + or -, check top of operatorStack. if + or -, pop and calculate 2 values. Regardless, then push token.
                if (currentToken == "+" ||
                    currentToken == "-")
                {
                    TryStackCalc(true, currentToken, operatorStack, valueStack);
                    operatorStack.Push(currentToken);
                    continue;
                }

                // just add these to the operatorStack.
                if (currentToken == "*" ||
                    currentToken == "/" ||
                    currentToken == "(")
                {
                    operatorStack.Push(currentToken);
                    continue;
                }

                // complete the three steps in the algorithm:
                // 1) if + or - are on the operatorStack, carry out that operation.
                // 2) Pop the ( at the top of the operatorStack. If this doesn't work, throw an error.
                // 3) if * or / are on the operatorStack, carry out that operation.
                if (currentToken == ")")
                {
                    TryStackCalc(true, currentToken, operatorStack, valueStack);
                    
                    string tryPeek;
                    operatorStack.TryPeek(out tryPeek);
                    if (tryPeek == null)
                        throw new ArgumentException("Invalid expression syntax: Incorrect parentheses.");
                    else //can peek
                        if (operatorStack.Peek() != "(")
                            throw new ArgumentException("Invalid expression syntax: Incorrect parentheses.");
                    operatorStack.Pop();

                    TryStackCalc(false, null, operatorStack, valueStack);
                    continue;
                }

                //if the code makes it here,
                //the last thing this can be is an int or variable.

                int currentValue;
                if (!int.TryParse(currentToken, out currentValue))
                {
                    //check to see if this is a valid variable, throw exception if not
                    //beginning of expression = ^
                    //>0 (+) letters a-z / A-Z
                    //THEN >0 (+) numbers 0-9
                    //ending of expression = $
                    if (!Regex.IsMatch(currentToken, @"^[a-zA-Z]+[0-9]+$"))
                    {
                        throw new ArgumentException("Invalid variable syntax.");
                    }
                    else //it's valid! get the delegate value
                        currentToken = variableEvaluator(currentToken).ToString();
                }
                //try to calculate using * or /
                //Otherwise, push currentToken to valueStack.
                if (!TryStackCalc(false, currentToken, operatorStack, valueStack))
                    valueStack.Push(currentToken);
            }
            // after the last token is processed, finish operation based on operator and value stack status

            // If no operators, the value in the valueStack is the result
            if (operatorStack.Count == 0)
            {
                if (valueStack.Count == 1)
                    return int.Parse(valueStack.Pop());
                else
                    throw new ArgumentException("Invalid expression syntax.");
            }

            // If one operator, complete the final calculation and return the result
            if (operatorStack.Count == 1)
            {
                string operatorSymbol = operatorStack.Pop();
                if ((operatorSymbol == "+" || operatorSymbol == "-") &&
                    valueStack.Count == 2)
                {
                    string value2 = valueStack.Pop();
                    string value1 = valueStack.Pop();
                    return int.Parse(Calculate(value1, operatorSymbol, value2));
                }
                else
                    throw new ArgumentException("Invalid expression syntax.");
            }

            else // operatorStack.Count > 1. This shouldn't happen.
            {
                throw new ArgumentException("Invalid expression syntax.");
            }
        }




        // PRIVATE HELPER METHODS -------------------------------------------------------------------------------------------

        /// <summary>
        /// A private helper method that aides the Evaluate class performing an operation on strings that are parsable as ints.
        /// </summary>
        /// <param name="value1">The value that comes first in the infix expression.</param>
        /// <param name="operatorSymbol">The operation to be performed, "+", "-", "/", or "*".</param>
        /// <param name="value2">The value that comes second in the infix expression.</param>
        /// <returns>The integer value result, turned back into a string.</returns>
        private static string Calculate(string value1, string operatorSymbol, string value2)
        {
            int int1 = int.Parse(value1);
            int int2 = int.Parse(value2);

            if (operatorSymbol == "+")
            {
                return (int1 + int2).ToString();
            }
            if (operatorSymbol == "-")
            {
                return (int1 - int2).ToString();
            }
            if (operatorSymbol == "*")
            {
                return (int1 * int2).ToString();
            }
            else // it's a "/"
            {
                return (int1 / int2).ToString();
            }

        }


        /// <summary>
        /// A private class that elimates a lot of repetitive code.
        /// Based on which boolean is entered, it will either try addition/subtraction OR multiplication/division
        /// (true or false returned based on if if the operation is set up via the operatorStack)
        /// and correctly manipulates the stacks as required.
        /// It handles most errors dealing with checking for stack occupancy (for popping and peeking)
        /// </summary>
        /// <param name="isAddSub">True if you want to add or subtract. False if you want to multiply or divide.</param>
        /// <param name="value2">An optional value2 in the attempted calculation. Otherwise, it pops from the valueStack for value2.</param>
        /// <param name="operatorStack">A stack handed in from Evaluate that holds ordered infix operators.</param>
        /// <param name="valueStack">A stack handed in from Evaluate that holds ordered infix values.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">If the stacks do not have the required values or operations to complete a necessary calculation, an error is thrown.</exception>
        private static bool TryStackCalc(bool isAddSub, string value2, Stack<string> operatorStack, Stack<string> valueStack)
        {
            string tryPeek;
            operatorStack.TryPeek(out tryPeek);

            if (isAddSub) //addition or subtraction
            {
                // 1) if + or - are on the operatorStack, carry out that operation and push the result to valueStack.
                if (tryPeek == "+" ||
                    tryPeek == "-")
                {
                    try
                    {
                        value2 = valueStack.Pop();
                        string value1 = valueStack.Pop();
                        string operatorSymbol = operatorStack.Pop();
                        valueStack.Push(Calculate(value1, operatorSymbol, value2));
                        return true;
                    }
                    catch (Exception)
                    {
                        throw new ArgumentException("Invalid expression syntax.");
                    }
                }
                else
                    return false;
            }

            else // multiplication or division
            {
                // 1) if * or / are on the operatorStack, carry out that operation and push the result to valueStack.
                if (tryPeek == "*" ||
                    tryPeek == "/")
                {
                    try
                    {
                        // use value2 given, if applicable
                        if (value2 == null)
                            value2 = valueStack.Pop();
                        string value1 = valueStack.Pop();
                        string operatorSymbol = operatorStack.Pop();
                        valueStack.Push(Calculate(value1, operatorSymbol, value2));
                    }
                    catch (Exception)
                    {
                        throw new ArgumentException("Invalid expression syntax or division by 0.");
                    }
                    return true;
                }
                else
                    return false;
            }
        }
    }
}