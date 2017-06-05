using System.Text;

namespace Organizer.DAL.QueryBuilders
{
    internal struct WhereCondition
    {
        private string _condition;
        private CombineOperator _combineOperator;

        public string Condition => _condition;

        public CombineOperator CombineOperator => _combineOperator;

        private WhereCondition(string condition, CombineOperator combineOperator = CombineOperator.And)
        {
            _condition = condition;
            _combineOperator = combineOperator;
        }

        public WhereCondition MakeExpressionComparisonCondition(string firstExpression, string secondExpression, WhereConditionType condition)
        {
            var conditionString = new StringBuilder();
            switch (condition)
            {
                case WhereConditionType.Equal:
                    conditionString.AppendFormat("{0} = {1}", firstExpression, secondExpression);
                    break;

                case WhereConditionType.Greater:
                    conditionString.AppendFormat("{0} > {1}", firstExpression, secondExpression);
                    break;

                case WhereConditionType.Lower:
                    conditionString.AppendFormat("{0} < {1}", firstExpression, secondExpression);
                    break;

                case WhereConditionType.GreaterOrEqual:
                    conditionString.AppendFormat("{0} >= {1}", firstExpression, secondExpression);
                    break;

                case WhereConditionType.LowerOrEqual:
                    conditionString.AppendFormat("{0} <= {1}", firstExpression, secondExpression);
                    break;

                case WhereConditionType.NotEqual:
                    conditionString.AppendFormat("{0} != {1}", firstExpression, secondExpression);
                    break;

                case WhereConditionType.NotLower:
                    conditionString.AppendFormat("{0} !< {1}", firstExpression, secondExpression);
                    break;

                case WhereConditionType.NotGreater:
                    conditionString.AppendFormat("{0} !> {1}", firstExpression, secondExpression);
                    break;
            }

            return new WhereCondition(conditionString.ToString());
        }

        public WhereCondition MakeLikeExpression(string expression, string otherExpression, bool isNotLike = false)
        {
            var conditionString = new StringBuilder();
            conditionString.Append(expression + " ");
            if (isNotLike)
            {
                conditionString.Append("NOT ");
            }

            conditionString.AppendFormat("LIKE {0}", otherExpression);

            return new WhereCondition(conditionString.ToString());
        }

        public WhereCondition MakeNotExpression(string predicate)
        {
            var conditionString = new StringBuilder();
            conditionString.AppendFormat("NOT {0}", predicate);
            return new WhereCondition(conditionString.ToString());
        }

        public WhereCondition MakeBetweenExpression(string expression, string lowerBound, string upperBound, bool isNotBetween = false)
        {
            var conditionString = new StringBuilder();
            conditionString.Append(expression + " ");
            if (isNotBetween)
            {
                conditionString.Append("NOT ");
            }

            conditionString.AppendFormat("BETWEEN {0} AND {1}", lowerBound, upperBound);
            return new WhereCondition(conditionString.ToString());
        }

        public WhereCondition MakeIsNullCondition(string expression, bool isNotNull)
        {
            var conditionString = new StringBuilder();
            conditionString.AppendFormat("{0} IS ", expression);
            if (isNotNull)
            {
                conditionString.Append("NOT ");
            }
            conditionString.Append("NULL");
            return new WhereCondition(conditionString.ToString());
        }

        public WhereCondition MakeInExpression(string expression, string[] range, bool notInRange = false)
        {
            var conditionString = new StringBuilder();
            conditionString.Append(expression + " ");
            if (notInRange)
            {
                conditionString.Append("NOT ");
            }
            conditionString.Append("IN (");
            int i = 0;
            foreach (var item in range)
            {
                conditionString.AppendFormat("'{0}'", item);
                if (i != range.Length - 1)
                {
                    conditionString.Append(", ");
                }
                i++;
            }
            conditionString.Append(")");
            return new WhereCondition(conditionString.ToString());
        }

        public WhereCondition MakeOrCondition(WhereCondition defaultCondition)
        {
            return new WhereCondition(defaultCondition._condition, CombineOperator.Or);
        }
    }
}