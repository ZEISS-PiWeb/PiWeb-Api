#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Dtos.Data
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Resources;
	using System.Text;

	#endregion

	/// <summary>
	/// Parser class for search condition strings. 
	/// </summary>
	public static class SearchConditionParser
	{
		#region members

		private static readonly Dictionary<string, Operation> Operations = new Dictionary<string, Operation>
		{
			{ "<>", Operation.NotEqual },
			{ "=", Operation.Equal },
			{ ">", Operation.GreaterThan },
			{ "<", Operation.LessThan },
			{ ">=", Operation.GreaterThanOrEqual },
			{ "<=", Operation.LessThanOrEqual },
			{ "In", Operation.In },
			{ "NotIn", Operation.NotIn },
			{ "Like", Operation.Like }
		};

		private static readonly string[] AllOperationStrings = Operations.Keys.ToArray();
		private static readonly Dictionary<Operation, string> InverseOperations = Operations.ToDictionary( k => k.Value, v => v.Key );

		private static readonly ResourceManager ResourceManager = new ResourceManager( typeof( SearchConditionParser ) );

		#endregion

		#region methods

		/// <summary>
		/// Returns <code>true</code> if the search filter is valid and can be parsed.
		/// </summary>
		public static bool CanParse( string searchFilter )
		{
			try
			{
				Parse( searchFilter );
			}
			catch
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Converts the search filter into a string.
		/// </summary>
		public static string GenericConditionToString( GenericSearchCondition condition )
		{
			var result = new StringBuilder();

			var and = condition as GenericSearchAnd;
			var attr = condition as GenericSearchAttributeCondition;
			var field = condition as GenericSearchFieldCondition;
			if( and != null )
			{
				var first = true;
				foreach( var subCondition in and.Conditions )
				{
					if( !first ) result.Append( "+" );
					result.Append( GenericConditionToString( subCondition ) );
					first = false;
				}
			}
			else if( attr != null )
			{
				result.Append( attr.Attribute ).Append( InverseOperations[ attr.Operation ] );
				result.Append( "[" ).Append( attr.Value ).Append( "]" );
			}
			else if( field != null )
			{
				result.Append( field.FieldName ).Append( InverseOperations[ field.Operation ] );
				result.Append( "[" ).Append( field.Value ).Append( "]" );
			}
			return result.ToString();
		}


		/// <summary>
		/// Finds the first  das erste Vorkommens eines beliebigen Strings aus dem values-Array.
		/// </summary>
		/// <param name="str">String in dem gesucht wird.</param>
		/// <param name="values">Strings nach denen gesucht wird.</param>
		/// <param name="startIndex">Index von dem an im String gesucht wird.</param>
		/// <returns>Der erste vorkommende String, ansonsten null.</returns>
		private static string FindFirst( string str, IEnumerable<string> values, int startIndex = 0 )
		{
			var first = -1;
			string result = null;
			foreach( var item in values )
			{
				var i = str.IndexOf( item, startIndex, StringComparison.Ordinal );
				if( i < 0 )
					continue;

				if( first >= 0 )
				{
					if( i < first )
					{
						first = i;
						result = item;
					}
					if( i == first )
					{
					    result = ( result?.Length ?? 0 ) > ( item?.Length ?? 0 ) ? result : item;
                    }
				}
				else
				{
					first = i;
					result = item;
				}
			}
			return result;
		}

		/// <summary>
		/// Parses the search filter string.
		/// </summary>
		/// <param name="searchFilter">The search filter string.</param>
		public static GenericSearchCondition Parse( string searchFilter )
		{
			if( !string.IsNullOrEmpty( searchFilter ) )
			{
				var conditions = new List<GenericSearchCondition>();

				var searchIndex = 0;
				var markerIndex = -1;

				while( searchIndex < searchFilter.Length )
				{
					var condition = default ( GenericSearchValueCondition );
					searchFilter = searchFilter.Substring( searchIndex ).Trim();

					// --- AND ---
					if( markerIndex != -1 )
					{
						if( searchFilter.StartsWith( "+" ) )
							searchFilter = searchFilter.Substring( 1 ).Trim();
						else
							throw new InvalidOperationException( string.Format( ResourceManager.GetString( "ParsingError.AND" ), searchFilter ) );
					}

					// --- Attribute key ---
					var opStr = FindFirst( searchFilter, AllOperationStrings );
					if( opStr == null )
						throw new InvalidOperationException( string.Format( ResourceManager.GetString( "ParsingError.Incomplete" ), searchFilter ) );

					searchIndex = searchFilter.IndexOf( opStr, StringComparison.Ordinal );
					markerIndex = searchIndex + opStr.Length;
					var token = searchFilter.Substring( 0, searchIndex );

					ushort key;
					if( !ushort.TryParse( token, out key ) )
					{
						var fieldCondition = new GenericSearchFieldCondition { FieldName = token };
						condition = fieldCondition;
					}
					else
					{
						var attributeCondition = new GenericSearchAttributeCondition { Attribute = key };
						condition = attributeCondition;
					}

					// --- Operation ---
					Operation op;
					if( !Operations.TryGetValue( opStr, out op ) )
						throw new InvalidOperationException( string.Format( ResourceManager.GetString( "ParsingError.InvalidOperator" ), token ) );
					condition.Operation = op;

					// --- Parameters ---
					searchIndex = searchFilter.IndexOf( '[', markerIndex );
					if( searchIndex == -1 )
						throw new InvalidOperationException( string.Format( ResourceManager.GetString( "ParsingError.NoValues" ), searchFilter ) );
					if( searchFilter.Substring( markerIndex, searchIndex - markerIndex ).Trim() != String.Empty )
						throw new InvalidOperationException( string.Format( ResourceManager.GetString( "ParsingError.Quotation" ), condition.Operation ) );

					markerIndex = searchIndex + 1;
					searchIndex = searchFilter.IndexOf( ']', markerIndex );

					if( searchIndex == -1 )
					{
						throw new InvalidOperationException( string.Format( ResourceManager.GetString( "ParsingError.NoClosingQuotes" ),
							searchFilter.Substring( markerIndex ) ) );
					}

					token = searchFilter.Substring( markerIndex, searchIndex - markerIndex );

					condition.Value = token;
					conditions.Add( condition );

					++searchIndex;
				}
				return conditions.Count == 1 ? conditions[ 0 ] : new GenericSearchAnd { Conditions = conditions.ToArray() };
			}
			return null;
		}

		/// <summary>
		/// Tries to parse the <code>searchFilter</code> and returns the filter as a <see cref="GenericSearchCondition"/> if successful.
		/// </summary>
		public static bool TryParse( string searchFilter, out GenericSearchCondition condition )
		{
			try
			{
				condition = Parse( searchFilter );
				return true;
			}
			catch
			{
				condition = null;
				return false;
			}
		}

		#endregion
	}
}