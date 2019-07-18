using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Clean.Microservice.Serverless.Core.Unit.Tests.Helpers
{
    [SuppressMessage("Maintainability Rules", "SA1119:StatementMustNotUseUnnecessaryParenthesis", Justification = "Wrong validation")]
    public static class PrivateSetterCaller
    {
        public static void SetPrivate<T, TValue>(this T instance, Expression<Func<T, TValue>> propertyExpression, TValue value)
        {
            instance.GetType().GetProperty(GetName(propertyExpression)).SetValue(instance, value, null);
        }

        private static string GetName<T, TValue>(Expression<Func<T, TValue>> exp)
        {
            if (!(exp.Body is MemberExpression body))
            {
                var ubody = (UnaryExpression)exp.Body;
                body = ubody.Operand as MemberExpression;
            }

            return body.Member.Name;
        }
    }
}
