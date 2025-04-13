using System.Text.RegularExpressions;

namespace Evaluator.UI.Windows
{
    public static class ExpressionValidator
    {
        public static (bool, string) ValidateExpression(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
                return (false, "The expression cannot be empty.");

            var expr = expression.Replace(" ", "");

            var allowedRegex = new Regex(@"^[0-9\.,\+\-\*\/\^\(\)]+$");
            if (!allowedRegex.IsMatch(expr))
                return (false, "The expression contains illegal characters.");

            var stack = new Stack<char>();
            foreach (var ch in expr)
            {
                if (ch == '(')
                    stack.Push(ch);
                else if (ch == ')')
                {
                    if (stack.Count == 0)
                        return (false, "The parentheses are not balanced.");
                    stack.Pop();
                }
            }
            if (stack.Count != 0)
                return (false, "The parentheses are not balanced.");

            if (expr.Length > 0 && !(char.IsDigit(expr[0]) || expr[0] == '-' || expr[0] == '('))
                return (false, "The expression must begin with a number, '-' o '('.");
         
            if (expr.Length > 0 && IsOperator(expr[expr.Length - 1]) && expr[expr.Length - 1] != ')')
                return (false, "The expression can not end with an operator.");

            string arithmeticOps = "+-*/^";
            for (int i = 1; i < expr.Length; i++)
            {
                if (arithmeticOps.Contains(expr[i]) && arithmeticOps.Contains(expr[i - 1]))
                {
                   if (expr[i] == '-' && (expr[i - 1] == '(' || i - 1 == 0))
                        continue;
                   return (false, "The expression contains invalid operator sequences.");
                }
            }

            return (true, "The expression is valid.");
        }

        private static bool IsOperator(char item)
            => "()^*/+-".IndexOf(item) >= 0;
    }
}
