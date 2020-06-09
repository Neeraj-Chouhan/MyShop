using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Specifications
{
    public class BaseSpecifications<T> : ISpecification<T>
    {
        public BaseSpecifications()
        {
            
        }
        public BaseSpecifications(Expression<Func<T,bool>> _criteria)
        {
            this.Criteria = _criteria;
        }
        public Expression<Func<T, bool>> Criteria {get;}

        public List<Expression<Func<T, object>>> Includes {get;} = new List<Expression<Func<T, object>>>();

        public Expression<Func<T, object>> OrderBy {get;private set;}

        public Expression<Func<T, object>> OrderByDescending {get;private set;}

        public int Take {get;private set;}
        public int Skip {get;private set;}
        public bool IsPagingEnabled {get;private set;}

        protected void AddIncludes(Expression<Func<T, object>> include){

            Includes.Add(include);
        }

        protected void OrderByAcse(Expression<Func<T, object>> OrderByExpression){

            OrderBy = OrderByExpression;
        }

        protected void OrderByDesc(Expression<Func<T, object>> OrderByDescExpression){

            OrderByDescending = OrderByDescExpression;
        }
          
        protected void ApplyPaging(int take,int skip){

            Take = take;
            Skip = skip;
            IsPagingEnabled = true;
        }
       
    }
}