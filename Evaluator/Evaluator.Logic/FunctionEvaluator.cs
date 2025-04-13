using System.Globalization;

namespace Evaluator.Logic;

public class FunctionEvaluator
{
    public static double Evalute(string infix)
    {
        var postfix = ToPostfix(infix);
        return Calculate(postfix);
    }


    private static double Calculate(string postfix)
    {
        var stack = new Stack<double>();
        var tokens = postfix.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var token in tokens)
        {
            if (token.Length == 1 && IsOperator(token[0]))
            {
                if (stack.Count < 2)
                    throw new InvalidOperationException("The stack is empty. There are not enough operands.");
                double operator2 = stack.Pop();
                double operator1 = stack.Pop();
                stack.Push(Result(operator1, token[0], operator2));
            }
            else
            {
                try
                {
                    double value = double.Parse(token, CultureInfo.InvariantCulture);
                    stack.Push(value);
                }
                catch (FormatException ex)
                {
                    throw new FormatException($"The number '{token}' it is not in a correct format.", ex);
                }
            }
        }

        if (stack.Count != 1)
            throw new InvalidOperationException("The stack has more than one element. Invalid expression.");

        return stack.Pop();
    }

    private static double Result(double operator1, char item, double operator2)
    {
        return item switch
        {
            '+' => operator1 + operator2,
            '-' => operator1 - operator2,
            '*' => operator1 * operator2,
            '/' => operator1 / operator2,
            '^' => Math.Pow(operator1, operator2),
            _ => throw new Exception("Invalid expression."),
        };
    }

    private static string ToPostfix(string infix)
    {
        var stack = new Stack<char>();
        var postfix = string.Empty;
        string number = ""; 

        foreach (var item in infix)
        {
            if (IsOperator(item))
            {
                if (!string.IsNullOrEmpty(number))
                {
                    postfix += number + " ";
                    number = "";
                }

                if (item == ')')
                {
                    while (stack.Count > 0 && stack.Peek() != '(')
                    {
                        postfix += stack.Pop() + " ";
                    }
                    stack.Pop(); 
                }
                else
                {
                    while (stack.Count > 0 && PriorityExpression(item) <= PriorityStack(stack.Peek()))
                    {
                        postfix += stack.Pop() + " ";
                    }
                    stack.Push(item);
                }
            }
            else if (item == '(')
            {
                stack.Push(item);
            }
            else
            {
                number += item;
            }
        }

        if (!string.IsNullOrEmpty(number))
        {
            postfix += number + " ";
        }

        while (stack.Count > 0)
        {
            postfix += stack.Pop() + " ";
        }

        return postfix.Trim();
    }

    private static int PriorityStack(char item)
    {
        return item switch
        {
            '^' => 3,
            '*' => 2,
            '/' => 2,
            '+' => 1,
            '-' => 1,
            '(' => 0,
            _ => throw new Exception("Invalid expression.")
        };
    }

    private static int PriorityExpression(char item)
    {
        return item switch
        {
            '^' => 4,
            '*' => 2,
            '/' => 2,
            '+' => 1,
            '-' => 1,
            '(' => 5,
            _ => throw new Exception("Invalid expression.")
        };
    }

    private static bool IsOperator(char item) => "()^*/+-".IndexOf(item) >= 0;
}
