using Games.Common;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Games.Infrastructure.Repositories.CosmosDb
{
    internal class CosmosDbQueryBuilder<T>
    {
        private readonly IList<QueryCondition> _conditions;
        private readonly IList<JoinStatement> _joins;
        private string _sql;
        private string _paging;
        private string _ordering;

        private const int DefaultPageSize = 10;
        private const int DefaultPageIndex = 1;

        private CosmosDbQueryBuilder(string sql)
        {
            _conditions = new List<QueryCondition>();
            _joins = new List<JoinStatement>();
            _sql = sql;
        }

        internal static CosmosDbQueryBuilder<T> GetQuery(string containerId)
        {
            if (containerId == null) throw new ArgumentNullException(nameof(containerId));

            return new CosmosDbQueryBuilder<T>($"SELECT * FROM {containerId} c");
        }

        internal static CosmosDbQueryBuilder<T> GetQuery(string selectStatement, string containerId, bool isDistinct = false)
        {
            if (containerId == null) throw new ArgumentNullException(nameof(containerId));
            if (selectStatement == null) throw new ArgumentNullException(nameof(selectStatement));

            var distinct = isDistinct ? "DISTINCT " : "";
            return new CosmosDbQueryBuilder<T>($"SELECT {distinct}{selectStatement} from {containerId} c");
        }

        internal CosmosDbQueryBuilder<T> Join(string alias, string property)
        {
            if (alias == null) throw new ArgumentNullException(nameof(alias));
            if (property == null) throw new ArgumentNullException(nameof(property));

            _joins.Add(new JoinStatement(alias, property));

            return this;
        }

        internal CosmosDbQueryBuilder<T> Append(string sql, string param, object data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (param == null) throw new ArgumentNullException(nameof(param));

            var property = $"@{param.ToCamelCase()}";
            var sqlStatment = string.Format(sql, property);
            _conditions.Add(new QueryCondition(sqlStatment, property, data));

            return this;
        }

        internal CosmosDbQueryBuilder<T> AppendIf(Func<bool> condition, string sql, string property, object data)
        {
            if (condition())
            {
                Append(sql, property, data);
            }

            return this;
        }

        internal CosmosDbQueryBuilder<T> OrderBy(string property, bool? isDescending = null)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            var order = isDescending.HasValue && isDescending.Value ? Order.Desc : Order.Asc;
            _ordering = $"ORDER BY {property} {order}";

            return this;
        }

        internal CosmosDbQueryBuilder<T> OrderByIf(Func<bool> condition, string property, bool? isDescending)
        {
            if (condition())
            {
                OrderBy(property, isDescending);
            }

            return this;
        }

        internal CosmosDbQueryBuilder<T> GetPage(int? pageSize = null, int? pageIndex = null)
        {
            var size = pageSize ?? DefaultPageSize;
            var index = pageIndex ?? DefaultPageIndex;

            _paging = $"OFFSET {(index - 1) * size} LIMIT {size}";

            return this;
        }

        public static implicit operator QueryDefinition(CosmosDbQueryBuilder<T> x) => x.Build();

        private QueryDefinition Build()
        {
            var queryDefinition = new QueryDefinition(GetSql());

            EnsureParameters(queryDefinition);

            return queryDefinition;
        }

        private void EnsureParameters(QueryDefinition sql)
        {
            foreach (var condition in _conditions)
            {
                sql.WithParameter(condition.Property, condition.Data);
            }
        }

        private string GetSql()
        {
            if (_joins.Any())
            {
                _sql += string.Join("", _joins.Select(x => x.GetJoin()));
            }

            if (_conditions.Any())
            {
                _sql += $" WHERE {string.Join(" AND ", _conditions.Select(x => x.Sql))}";
            }

            if (!string.IsNullOrWhiteSpace(_ordering))
            {
                _sql += $" {_ordering}";
            }

            if (!string.IsNullOrWhiteSpace(_paging))
            {
                _sql += $" {_paging}";
            }

            return _sql;
        }

        private class QueryCondition
        {
            public QueryCondition(string sql, string property, object date)
            {
                Sql = sql;
                Property = property;
                Data = date;
            }

            public string Sql { get; }
            public string Property { get; }
            public object Data { get; }
        }

        private class JoinStatement
        {
            public JoinStatement(string alias, string property)
            {
                _alias = alias;
                _property = property;
            }

            private readonly string _alias;
            private readonly string _property;

            public string GetJoin()
            {
                return $" JOIN {_alias} IN {_property}";
            }
        }

        private enum Order
        {
            Asc,
            Desc
        }
    }
}
