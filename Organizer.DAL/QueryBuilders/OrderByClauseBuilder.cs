using System.Collections.Generic;
using System.Text;

namespace Organizer.DAL.QueryBuilders
{
    public class OrderByClauseBuilder
    {
        private IList<string> _columnNames;
        private SortDirection _direction;

        public OrderByClauseBuilder(SortDirection direction = SortDirection.Ascending, params string[] columnNames)
        {
            _direction = direction;
            _columnNames = new List<string>(columnNames);
        }

        public string Build()
        {
            var orderByClause = new StringBuilder();
            orderByClause.Append("ORDER BY ");

            int i = 0;
            foreach (var columnName in _columnNames)
            {
                orderByClause.Append(columnName);
                if (i != 0 && i != _columnNames.Count - 1)
                {
                    orderByClause.Append(", ");
                }
                i++;
            }
            orderByClause.AppendFormat(" {0}", _direction == SortDirection.Ascending ? "ASC" : "DESC");

            return orderByClause.ToString();
        }
    }
}