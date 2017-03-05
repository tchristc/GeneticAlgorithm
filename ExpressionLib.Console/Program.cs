using System;
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
                int result;
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

            expression = DynamicExpression.ParseLambda(new ParameterExpression[] {}, null, exp);
            t = (T)((LambdaExpression)expression).Compile().DynamicInvoke();

            return true;
        }
    }
}
