using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionStringMonitor
{
	internal sealed class ConnectionStringReplacement
	{
		public string Pattern
		{
			get { return _pattern; }
		}

		public ConnectionStringReplacement(string name)
		{
			_accessor = CreateAccessor(name);
			_pattern = string.Format("{0}{1}{0}", _patternDelimiter, name);
		}

		public string Apply(string input, SqlConnectionStringBuilder builder)
		{
			return input.Replace(_pattern, _accessor(builder));
		}

		private static AccessorCallback CreateAccessor(string propertyName)
		{
			var parameter = Expression.Parameter(
				typeof(SqlConnectionStringBuilder), "builder");

			var accessorExpression = Expression.Lambda<AccessorCallback>(
				Expression.Property(parameter, propertyName),
				parameter);

			return accessorExpression.Compile();
		}

		private readonly AccessorCallback _accessor;
		private readonly string _pattern;

		private const string _patternDelimiter = "%";

		private delegate string AccessorCallback(SqlConnectionStringBuilder builder);
	}
}
