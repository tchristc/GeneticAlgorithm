using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DynamicExpression = System.Linq.Dynamic.DynamicExpression;

namespace ExpressionLib.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //ParameterExpression argParam = Expression.Parameter(typeof(Service), "s");
            //Expression nameProperty = Expression.Property(argParam, "Name");
            //Expression namespaceProperty = Expression.Property(argParam, "Namespace");

            //var val1 = Expression.Constant("Modules");
            //var val2 = Expression.Constant("Namespace");

            //Expression e1 = Expression.Equal(nameProperty, val1);
            //Expression e2 = Expression.Equal(namespaceProperty, val2);
            //var andExp = Expression.AndAlso(e1, e2);

            //var lambda = Expression.Lambda<Func<Service, bool>>(andExp, argParam);

            //var a = Expression.Lambda<Func<Service, bool>>(andExp, argParam)
            string input = string.Empty;
            do
            {
                System.Console.Write("Input an Expression: ");
                input = System.Console.ReadLine();
                Expression e;
                double result;
                input.TryParse(out e, out result);
                System.Console.WriteLine(e + " = " + result);
            } while (!string.IsNullOrEmpty(input) || input == "q");
            System.Console.ReadLine();
        }
    }

    public static class ExpressionExtensions
    {
        public static bool TryParse<T>(this string exp, out Expression expression, out T t)
        {
            expression = null;
            t = default(T);
            if (string.IsNullOrEmpty(exp))
                return false;
            //throw new ArgumentNullException(nameof(exp));
            try
            {
                var externals = new Dictionary<string, object>
                {
                    {"x", 5},
                    {"y", Math.PI}
                };
                externals["x"] = 6;
                expression = DynamicExpression.ParseLambda(new ParameterExpression[] {}, typeof(T), exp, externals);
                t = (T)((LambdaExpression)expression).Compile().DynamicInvoke();
            }catch(Exception ex)
            {
                System.Console.WriteLine("Exception: " + ex);
                return false;
            }

            return true;
        }
    }
}
