using System.Collections.Generic;
using System.Text;

namespace Organizer.DAL.QueryBuilders
{
    public class WhereClauseBuilder
    {
        private IList<WhereCondition> _conditions;

        public WhereClauseBuilder()
        {
            _conditions = new List<WhereCondition>();
        }

        public string Build()
        {
            var resultString = new StringBuilder();
            int i = 0;
            foreach (var item in _conditions)
            {
                resultString.Append(item.Condition);
                if (i != 0 && i != _conditions.Count - 1)
                {
                    resultString.AppendFormat(" {0} ", item.CombineOperator == CombineOperator.And ? "AND" : "OR");
                }
                i++;
            }

            return resultString.ToString();
        }
    }
}