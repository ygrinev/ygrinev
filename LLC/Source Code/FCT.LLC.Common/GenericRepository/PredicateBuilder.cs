using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FCT.LLC.GenericRepository
{
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> CurrentExp,
                                                            Expression<Func<T, bool>> AdditionalExp)
        {
            var invokedExpr = Expression.Invoke(AdditionalExp, CurrentExp.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.OrElse(CurrentExp.Body, invokedExpr), CurrentExp.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> CurrentExp,
                                                             Expression<Func<T, bool>> AdditionalExp)
        {
            var invokedExpr = Expression.Invoke(AdditionalExp, CurrentExp.Parameters.Cast<Expression>());

            return Expression.Lambda<Func<T, bool>>
                  (Expression.AndAlso(CurrentExp.Body, invokedExpr), CurrentExp.Parameters);
        }
    }
}
