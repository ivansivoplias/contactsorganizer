using System.Collections.Generic;
using System.Text;

namespace Organizer.DAL.QueryBuilders
{
    public class GroupByClauseBuilder
    {
        private IList<string> _columnNames;

        public GroupByClauseBuilder(params string[] columns)
        {
            _columnNames = new List<string>(columns);
        }

        public string Build()
        {
            var groupByClause = new StringBuilder();

            groupByClause.Append("ORDER BY ");

            int i = 0;
            foreach (var column in _columnNames)
            {
                groupByClause.Append(column);
                if (i != 0 && i != _columnNames.Count - 1)
                {
                    groupByClause.Append(", ");
                }
                i++;
            }

            return groupByClause.ToString();
        }
    }
}