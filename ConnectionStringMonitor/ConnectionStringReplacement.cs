﻿// Copyright (c) 2012 Łukasz Patalas
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom
// the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies
// or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE
// AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

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
