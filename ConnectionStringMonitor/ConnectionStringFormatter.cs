// Copyright (c) 2012 Łukasz Patalas
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
using System.Text;
using System.Threading.Tasks;

namespace ConnectionStringMonitor
{
	internal static class ConnectionStringFormatter
	{
		public static IEnumerable<string> Patterns
		{
			get
			{
				return _replacements.Select(replacement => replacement.Pattern);
			}
		}

		public static string Format(string connectionString)
		{
			if (!string.IsNullOrEmpty(connectionString))
			{
				var csBuilder = new SqlConnectionStringBuilder(connectionString);
				var formattedText = Settings.Default.OutputFormat;

				foreach (var replacement in _replacements)
				{
					formattedText = replacement.Apply(
						formattedText, csBuilder);
				}

				return formattedText;
			}
			else
			{
				return string.Empty;
			}
		}

		private static IList<ConnectionStringReplacement> _replacements =
			new List<ConnectionStringReplacement>()
			{
				new ConnectionStringReplacement("AttachDBFilename"),
				new ConnectionStringReplacement("DataSource"),
				new ConnectionStringReplacement("InitialCatalog"),
				new ConnectionStringReplacement("UserID")
			}
			.AsReadOnly();
	}
}
