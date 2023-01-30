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
///    This console program is a testing program for the
///    FormulaEvaluator library.
/// </summary>

using FormulaEvaluator;
using System.Net;

public class FormulaEvaluatorTester
{
    private static void Main(string[] args)
    {
        BasicOperationTests();
        MixedOperationAndParenthesesTests();
        //VariableTests();
        //EdgeCaseAndErrorTests();
    }

    /// <summary>
    /// A testing method with tests for +, -, *, and / operations.
    /// </summary>
    private static void BasicOperationTests()
    {
        //ADDITION

        //Console.WriteLine(Evaluator.Evaluate("1+3", null) + " = 1+3");
        if (Evaluator.Evaluate("1+3", null) != 4)
            Console.WriteLine("Addition Test (basic) failed!!!");

        //Console.WriteLine(Evaluator.Evaluate("1+(2+3)", null) + " = 1+(2+3)");
        if (Evaluator.Evaluate("1+(2+3)", null) != 6)
            Console.WriteLine("Addition Test (with parentheses) failed!!!");

        //Console.WriteLine(Evaluator.Evaluate("0+0+0", null) + " = 0+0+0");
        if (Evaluator.Evaluate("0+0+0", null) != 0)
            Console.WriteLine("Addition Test (Zeros) failed!!!");


        //SUBTRACTION

        //Console.WriteLine(Evaluator.Evaluate("7-3", null) + " = 7-3");
        if (Evaluator.Evaluate("7-3", null) != 4)
            Console.WriteLine("Subtraction Test (basic) failed!!!");

        //Console.WriteLine(Evaluator.Evaluate("125-25-25", null) + " = 125-25-25");
        if (Evaluator.Evaluate("125-25-25", null) != 75)
            Console.WriteLine("Subtraction Test (big numbers) failed!!!");

        //Console.WriteLine(Evaluator.Evaluate("5-5", null) + " = 5-5");
        if (Evaluator.Evaluate("5-5", null) != 0)
            Console.WriteLine("Subtraction Test (equals zero) failed!!!");

        //Console.WriteLine(Evaluator.Evaluate("0-0-0", null) + " = 0-0-0");
        if (Evaluator.Evaluate("0-0-0", null) != 0)
            Console.WriteLine("Subtraction Test (Zeros) failed!!!");

        //MULTIPLICATION

        if (Evaluator.Evaluate("3*15", null) != 45)
            Console.WriteLine("Multiplication Test (basic) failed!!!");

        if (Evaluator.Evaluate("3*1", null) != 3)
            Console.WriteLine("Multiplication Test (times 1) failed!!!");

        if (Evaluator.Evaluate("2*2", null) != 4)
            Console.WriteLine("Multiplication Test (times itself, small) failed!!!");

        if (Evaluator.Evaluate("20*20", null) != 400)
            Console.WriteLine("Multiplication Test (times itself, big) failed!!!");

        if (Evaluator.Evaluate("0*7+0*4", null) != 0)
            Console.WriteLine("Multiplication Test (Zeros) failed!!!");

        //DIVISION

        if (Evaluator.Evaluate("15/3", null) != 5)
            Console.WriteLine("Division Test (basic) failed!!!");

        if (Evaluator.Evaluate("3/1", null) != 3)
            Console.WriteLine("Division Test (/1) failed!!!");

        if (Evaluator.Evaluate("0/5", null) != 0)
            Console.WriteLine("Division Test (Zero / ?) failed!!!");

        if (Evaluator.Evaluate("27 /3 / 3", null) != 3)
            Console.WriteLine("Division Test (Complex) failed!!!");


        // OBSOLETE HELPER METHOD TESTS
        // All methods these used to call are now private.
        //if (Evaluator.Calculate("1", "+", "4") == 5)
        //    Console.WriteLine("WOOP WOOP! + Calculate test succeeded");
        //if (Evaluator.Calculate("1", "-", "4") == -3)
        //    Console.WriteLine("WOOP WOOP! - Calculate test succeeded");
        //if (Evaluator.Calculate("2", "*", "4") == 8)
        //    Console.WriteLine("WOOP WOOP! * Calculate test succeeded");
        //if (Evaluator.Calculate("12", "/", "4") == 3)
        //    Console.WriteLine("WOOP WOOP! / Calculate test succeeded");
    }

    /// <summary>
    /// A testing method with tests for mixed operations and parentheses, some with whitespace.
    /// </summary>
    private static void MixedOperationAndParenthesesTests()
    {
        //Simple
        if (Evaluator.Evaluate("(1)", null) != 1)
            Console.WriteLine("Parentheses Test (bb ez) failed");

        if (Evaluator.Evaluate("((1))", null) != 1)
            Console.WriteLine("Parentheses Test (double parentheses) failed");

        if (Evaluator.Evaluate("(((((((1)))))))", null) != 1)
            Console.WriteLine("Parentheses Test (SO many parentheses) failed");

        //Order of Operations required
        if (Evaluator.Evaluate("1-(2-3)", null) != 2)
            Console.WriteLine("OoO Test 1 failed");

        if (Evaluator.Evaluate("1-2*3", null) != -5)
            Console.WriteLine("OoO Test 2 failed");

        if (Evaluator.Evaluate("(1-2)*3", null) != -3)
            Console.WriteLine("OoO Test 3 failed");

        if (Evaluator.Evaluate("3*4/(4-2)", null) != 6)
            Console.WriteLine("OoO Test 4 failed");

        if (Evaluator.Evaluate("1+3*(7-7)", null) != 1)
            Console.WriteLine("OoO Test 5 failed");

        if (Evaluator.Evaluate("(2+3)*5+2", null) != 27)
            Console.WriteLine("OoO Test 6 failed");

        //Console.WriteLine(Evaluator.Evaluate("(((24 * 3) / 3) + (3 + (1 / 1))) - (4 + (4 * 4))", null));
        if (Evaluator.Evaluate("(((24 * 3) / 3) + (3 + (1 / 1))) - (4 + (4 * 4))", null) != 8)
            Console.WriteLine("OoO Test 7 failed");
    }

    /// <summary>
    /// A delegate method which stores my various testing variable values as according the their names
    /// and passes them into tests in VariableTests and EdgeCaseAndErrorTests.
    /// </summary>
    /// <param name="variableName">The name of the variable, which must be in the format:
    /// >0 capital or lowercase letters, followed by >0 numbers.</param>
    /// <returns>The integer value of the variable.</returns>
    /// <exception cref="ArgumentException">The variable name input does not match a value in the class.</exception>
    static int Lookup(string variableName)
    {
        if (variableName == "A1")
            return 5;
        else if (variableName == "a1")
            return 4;
        else if (variableName == "B2")
            return 2;
        else if (variableName == "FxLB27847")
            return 34;
        else if (variableName == "NOODLE")
            return 7;
        else if (variableName == "2D")
            return 2;
        else
            throw new ArgumentException("Not a known variable.");
    }

    /// <summary>
    /// A testing method with tests for variable looking up, parsing, and calculation
    /// </summary>
    //public static void VariableTests(); 

    //{
    //    if (Evaluator.Evaluate("(B2+3)*5+2", Lookup) != 27)
    //        Console.WriteLine("Variable Test 1 failed");

    //    if (Evaluator.Evaluate("(a1+A1)", Lookup) != 9)
    //        Console.WriteLine("Variable Test 2 failed");

    //    if (Evaluator.Evaluate("FxLB27847", Lookup) != 34)
    //        Console.WriteLine("Variable Test 3 failed");
    //}

    /// <summary>
    /// A testing method which tests for various edge cases and error handling situations.
    /// If an error is caught, a message will be sent to the console.
    /// </summary>
    //public static void EdgeCaseAndErrorTests();
    //{
    //    // Dividing by 0 (by itself, in large formula)
    //    try
    //    {
    //        Evaluator.Evaluate("2/0", null);
    //        Console.WriteLine("Uh oh, Error (dividing by 0) not caught");
    //    }
    //    catch (ArgumentException)
    //    {
    //        Console.WriteLine("Error /0: 2/0 (test passed!) Cannot divide by 0.");
    //    }

    //    //Invalid inputs
    //    try
    //    {
    //        Evaluator.Evaluate(" -A- ", null);
    //        Console.WriteLine("Uh oh, Error 1 not caught");
    //    }
    //    catch (ArgumentException)
    //    {
    //        Console.WriteLine("Error 1: ' -A- ' (test passed!)");
    //    }

    //    try
    //    {
    //        Evaluator.Evaluate("", null);
    //        Console.WriteLine("Uh oh, Error 2 not caught");
    //    }
    //    catch (ArgumentException)
    //    {
    //        Console.WriteLine("Error 2: no string (test passed!)");
    //    }

    //    try
    //    {
    //        Evaluator.Evaluate(" ", null);
    //        Console.WriteLine("Uh oh, Error 3 not caught");
    //    }
    //    catch (ArgumentException)
    //    {
    //        Console.WriteLine("Error 3: space only (test passed!)");
    //    }

    //    try
    //    {
    //        Evaluator.Evaluate("~", null);
    //        Console.WriteLine("Uh oh, Error 4 not caught");
    //    }
    //    catch (ArgumentException)
    //    {
    //        Console.WriteLine("Error 4: ~ (test passed!)");
    //    }

    //    try
    //    {
    //        Evaluator.Evaluate("8 * () 2", null);
    //        Console.WriteLine("Uh oh, Error 5 not caught");
    //    }
    //    catch (ArgumentException)
    //    {
    //        Console.WriteLine("Error 5: () (test passed!)");
    //    }

    //    try
    //    {
    //        Evaluator.Evaluate("-2", null);
    //        Console.WriteLine("Uh oh, Error 6 not caught");
    //    }
    //    catch (ArgumentException)
    //    {
    //        Console.WriteLine("Error 6: -2 (test passed!)");
    //    }

    //    //variable mistakes
    //    try
    //    {
    //        Evaluator.Evaluate("4+-32", null);
    //        Console.WriteLine("Uh oh, Error 7 not caught");
    //    }
    //    catch (ArgumentException)
    //    {
    //        Console.WriteLine("Error 7: 4+-32 (test passed!)");
    //    }

    //    try
    //    {
    //        Evaluator.Evaluate("-A1", Lookup);
    //        Console.WriteLine("Uh oh, Error 8 not caught");
    //    }
    //    catch (ArgumentException)
    //    {
    //        Console.WriteLine("Error 8: -A1 (test passed!)");
    //    }

    //    try
    //    {
    //        Evaluator.Evaluate("2/2D", Lookup);
    //        Console.WriteLine("Uh oh, Error 8 not caught");
    //    }
    //    catch (ArgumentException)
    //    {
    //        Console.WriteLine("Error 9: 2/2D (test passed!)");
    //    }

    //    try
    //    {
    //        Evaluator.Evaluate("7/4+(NOODLE*3)", Lookup);
    //        Console.WriteLine("Uh oh, Error 10 not caught");
    //    }
    //    catch (ArgumentException)
    //    {
    //        Console.WriteLine("Error 10: 7/4+(NOODLE*3) (test passed!)");
    //    }

    //    //variable not defined (delegate throws)
    //    try
    //    {
    //        Evaluator.Evaluate("7/4+(coolguy2*3)", Lookup);
    //        Console.WriteLine("Uh oh, Error 11 not caught");
    //    }
    //    catch (ArgumentException)
    //    {
    //        Console.WriteLine("Error 11: 7/4+(coolguy2*3) (test passed!)");
    //    }

    //    //missing parentheses "(" ")" "(3-2" "3-2)"
    //    try
    //    {
    //        Evaluator.Evaluate("3-2)", null);
    //        Console.WriteLine("Uh oh, Error 12 not caught");
    //    }
    //    catch (ArgumentException)
    //    {
    //        Console.WriteLine("Error 12: (3-2 (test passed!)");
    //    }

    //    try
    //    {
    //        Evaluator.Evaluate("3-2)", null);
    //        Console.WriteLine("Uh oh, Error 13 not caught");
    //    }
    //    catch (ArgumentException)
    //    {
    //        Console.WriteLine("Error 13: 3-2) (test passed!)");
    //    }

    //    //incorrect values or operators left in the stack "3 4 + 2" "2 / / 4"
    //    try
    //    {
    //        Evaluator.Evaluate("3 4 + 2\" \"2 / / 4", null);
    //        Console.WriteLine("Uh oh, Error 14 not caught");
    //    }
    //    catch (ArgumentException)
    //    {
    //        Console.WriteLine("Error 14: 3 4 + 2\" \"2 / / 4 (test passed!)");
    //    }
    //}
}