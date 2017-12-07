#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Common.Data
{
	#region usings

	using System;
	using System.Text;
	using System.Collections.Generic;

	using DataService;

	#endregion

	/// <summary> Helping class for converting <see cref="PathInformation"/> objects to strings and vice versa. </summary>
	public static class PathHelper
	{
		#region constants

		/// <summary> Delimiter character for the path components. </summary>
		public const char Delimiter = '/';

		/// <summary> Escaped delimiter character for the path components </summary>
		public const string EscapedDelimiter = @"\/";

		/// <summary> Delimiter string for the path components </summary>
		public const string DelimiterString = "/";

		#endregion

		#region methods

		/// <summary> Creates from <paramref name="path"/> a part <see cref="PathInformation"/> object. </summary>
		public static PathInformation String2PartPathInformation( string path )
		{
			return String2PathInformationInternal( path, null, InspectionPlanEntity.Part );
		}

		/// <summary> Creates from <paramref name="path"/> a characteristic <see cref="PathInformation"/> object. </summary>
		public static PathInformation String2CharPathInformation( string path )
		{
			return String2PathInformationInternal( path, null, InspectionPlanEntity.Characteristic );
		}

		/// <summary> Creates from <paramref name="path"/> a <see cref="PathInformation"/> object. 
		/// Depending on <paramref name="structure"/> it will be a part or a characteristic.</summary>
		public static PathInformation String2PathInformation( string path, string structure )
		{
			return String2PathInformationInternal( path, structure, null );
		}

		/// <summary> Internal method to create a <see cref="PathInformation"/> object from <paramref name="path"/>. </summary>
		private static PathInformation String2PathInformationInternal( string path, string structure, InspectionPlanEntity? entity )
		{
			var resultPath = PathInformation.Root;
			if( !string.IsNullOrEmpty( path ) && !( path == Delimiter.ToString() && string.IsNullOrEmpty( structure ) ) )
			{
				var result = new List<PathElement>( structure != null ? structure.Length : 10 );

				int i = 0;
				var startIndex = path[ 0 ] == Delimiter ? 1 : 0;
				var sb = new StringBuilder( path.Length );
				for( int j = startIndex; j < path.Length; j++ ) // ersten Slash überlesen
				{
					char current = path[ j ];
					if( current == '\\' )
					{
						current = path[ ++j ];
						sb.Append( current );
					}
					else if( current == Delimiter )
					{
						if( structure == null )
							result.Add( new PathElement( entity.GetValueOrDefault( InspectionPlanEntity.Part ), sb.ToString() ) );
						else if (structure.Length <= i)
							throw new ArgumentException( string.Format( "The path structure does not match the actual path: The path is '{0}' but structure is '{1}' ({2} segements).", path, structure, structure.Length ), "structure" );
						else if( structure[ i++ ] == 'P' )
							result.Add( new PathElement( InspectionPlanEntity.Part, sb.ToString() ) );
						else
							result.Add( new PathElement( InspectionPlanEntity.Characteristic, sb.ToString() ) );

						sb = new StringBuilder( path.Length );
					}
					else
					{
						sb.Append( current );
					}
				}
				if( sb.Length > 0 )
				{
					if( structure == null )
						result.Add( new PathElement( entity.GetValueOrDefault( InspectionPlanEntity.Part ), sb.ToString() ) );
					else if( structure.Length <= i )
						throw new ArgumentException( string.Format( "The path structure does not match the actual path: The path is '{0}' but structure is '{1}' ({2} segements).", path, structure, structure.Length ), "structure" );
					else if( structure[ i++ ] == 'P' )
						result.Add( new PathElement( InspectionPlanEntity.Part, sb.ToString() ) );
					else
						result.Add( new PathElement( InspectionPlanEntity.Characteristic, sb.ToString() ) );
				}

				resultPath = new PathInformation( result );
			}
			return resultPath;
		}

		/// <summary> Returns the depth of the <paramref name="path"/>. </summary>
		public static int GetDepth( string path )
		{
			int i = 0;

			var sb = new StringBuilder( path.Length );
			for( int j = 0; j < path.Length; j++ )
			{
				char current = path[ j ];
				if( current == '\\' )
				{
					current = path[ ++j ];
					sb.Append( current );
				}
				else if( current == Delimiter )
				{
					i++;
					sb = new StringBuilder( path.Length );
				}
				else
				{
					sb.Append( current );
				}
			}
			if( sb.Length > 0 )
			{
				i++;
			}
			return i - 1;
		}

		/// <summary> Converts <paramref name="path"/> to a string which can be stored in the database. </summary>
		public static string PathInformation2String( PathInformation path )
		{
			var sb = new StringBuilder();

			sb.Append( Delimiter );

			var count = path.Count;
			for( var i = 0; i < count; i++ )
			{
				sb.Append( path[ i ].Value.Replace( @"\", @"\\" ).Replace( DelimiterString, EscapedDelimiter ) );
				sb.Append( Delimiter );
			}
			return sb.ToString();
		}

		/// <summary> Returns the structure of the <paramref name="path"/>. </summary>
		public static string GetStructure( PathInformation path )
		{
			var sb = new StringBuilder( path.Count );
			foreach( var elem in path )
			{
				sb.Append( elem.Type == InspectionPlanEntity.Part ? 'P' : 'C' );
			}
			return sb.ToString();
		}

		#endregion
	}
}