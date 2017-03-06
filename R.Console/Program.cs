using System;
using Microsoft.CodeAnalysis.Scripting;
using Script;

namespace R
{
    class Program
    {
        static void Main(string[] args)
        {
            var scriptEngine = new CSharpScriptEngine();

            while (true)
            {
                Console.Write(">> ");
                var input = Console.ReadLine();
                if (input == "exit")
                    break;
                try
                {
                    var result = scriptEngine.Execute(input);
                    if (result != null)
                    {
                        Console.WriteLine(result);
                    }
                }
                //CompilationErrorException
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
