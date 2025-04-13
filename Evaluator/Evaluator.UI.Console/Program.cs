using Evaluator.Logic;

try
{
    var result2 = FunctionEvaluator.Evalute("4*5/(4+6)");
    var result1 = FunctionEvaluator.Evalute("4*(5+6-(8/2^3)-7)-1");
    var result3 = FunctionEvaluator.Evalute("9^(1/2)");
    var result4 = FunctionEvaluator.Evalute("3,14+2,5");
    var result5 = FunctionEvaluator.Evalute("33,14+222,5");
    Console.WriteLine(result1);
    Console.WriteLine(result2);
    Console.WriteLine(result3);
    Console.WriteLine(result4);
    Console.WriteLine(result5);
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}