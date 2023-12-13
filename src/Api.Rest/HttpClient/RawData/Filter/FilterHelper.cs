#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.HttpClient.RawData.Filter
{
	#region usings

	using System;
	using JetBrains.Annotations;
	using Zeiss.PiWeb.Api.Rest.Common.Data.FilterString.Tree;

	#endregion

	public static class FilterHelper
	{
		#region constants

		public const string AttrFilename = "filename";
		public const string AttrMimeType = "mimetype";
		public const string AttrLastModified = "lastModified";
		public const string AttrCreated = "created";
		public const string AttrLength = "length";
		public const string AttrMd5 = "md5";

		#endregion

		#region methods

		public static string GetAttributeName( StringAttributes attribute )
		{
			switch( attribute )
			{
				case StringAttributes.Filename:
					return AttrFilename;

				case StringAttributes.Mimetype:
					return AttrMimeType;

				case StringAttributes.Md5:
					return AttrMd5;

				default:
					throw new ArgumentOutOfRangeException( nameof( attribute ), attribute, "Unknown enumeration value." );
			}
		}

		public static string GetAttributeName( DateTimeAttributes attribute )
		{
			switch( attribute )
			{
				case DateTimeAttributes.LastModified:
					return AttrLastModified;

				case DateTimeAttributes.Created:
					return AttrCreated;

				default:
					throw new ArgumentOutOfRangeException( nameof( attribute ), attribute, "Unknown enumeration value." );
			}
		}

		public static string GetAttributeName( IntegerAttributes attribute )
		{
			switch( attribute )
			{
				case IntegerAttributes.Length:
					return AttrLength;

				default:
					throw new ArgumentOutOfRangeException( nameof( attribute ), attribute, "Unknown enumeration value." );
			}
		}

		public static TokenType GetOperatorTokenType( CompareOperation operation )
		{
			switch( operation )
			{
				case CompareOperation.Equal:
					return TokenType.Equal;

				case CompareOperation.NotEqual:
					return TokenType.NotEqual;

				case CompareOperation.Greater:
					return TokenType.Greater;

				case CompareOperation.GreaterOrEqual:
					return TokenType.GreaterOrEqual;

				case CompareOperation.Less:
					return TokenType.Less;

				case CompareOperation.LessOrEqual:
					return TokenType.LessOrEqual;

				default:
					throw new ArgumentOutOfRangeException( nameof( operation ), operation, "Unknown enumeration value." );
			}
		}

		public static TokenType GetOperatorTokenType( ListOperation operation )
		{
			switch( operation )
			{
				case ListOperation.In:
					return TokenType.In;

				case ListOperation.NotIn:
					return TokenType.NotIn;

				default:
					throw new ArgumentOutOfRangeException( nameof( operation ), operation, "Unknown enumeration value." );
			}
		}

		/// <exception cref="ArgumentNullException"><paramref name="attrName"/> is <see langword="null" />.</exception>
		internal static IFilterTree MakeComparison( TokenType operationToken, [NotNull] string attrName, IFilterTree valueTree )
		{
			if( attrName == null )
				throw new ArgumentNullException( nameof( attrName ) );

			var attrItem = FilterTree.MakeAttr( attrName );
			return new FilterTree( new Token( operationToken ), new[] { attrItem, valueTree } );
		}

		#endregion
	}
}