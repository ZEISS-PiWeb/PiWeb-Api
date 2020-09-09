#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Data.FilterString.Tree
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Diagnostics;
	using System.Globalization;
	using JetBrains.Annotations;

	#endregion

	[DebuggerDisplay( "{Token.Type}" )]
	public class FilterTree : IFilterTree
	{
		#region members

		private readonly IList<IFilterTree> _Children;

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="FilterTree"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="token"/> is <see langword="null" />.</exception>
		public FilterTree( [NotNull] Token token )
		{
			_Children = new ReadOnlyCollection<IFilterTree>( new List<IFilterTree>() );
			Token = token ?? throw new ArgumentNullException( nameof( token ) );

			PositionSetup();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FilterTree"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="token"/> or <paramref name="children"/> is <see langword="null" />.</exception>
		public FilterTree( [NotNull] Token token, [NotNull] IEnumerable<IFilterTree> children )
		{
			if( children == null )
				throw new ArgumentNullException( nameof( children ) );

			_Children = new ReadOnlyCollection<IFilterTree>( new List<IFilterTree>( children ) );
			Token = token ?? throw new ArgumentNullException( nameof( token ) );

			PositionSetup();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FilterTree"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="token"/> or <paramref name="child"/> is <see langword="null" />.</exception>
		public FilterTree( [NotNull] Token token, [NotNull] IFilterTree child )
		{
			if( child == null )
				throw new ArgumentNullException( nameof( child ) );

			_Children = new ReadOnlyCollection<IFilterTree>( new List<IFilterTree> { child } );
			Token = token ?? throw new ArgumentNullException( nameof( token ) );

			PositionSetup();
		}

		#endregion

		#region methods

		public static IFilterTree MakeAnd( params IFilterTree[] children )
		{
			return children.Length == 0 ? MakeTrue() : new FilterTree( new Token( TokenType.And ), children );
		}

		public static IFilterTree MakeOr( params IFilterTree[] children )
		{
			return children.Length == 0 ? MakeFalse() : new FilterTree( new Token( TokenType.Or ), children );
		}

		public static IFilterTree MakeNot( IFilterTree child )
		{
			return new FilterTree( new Token( TokenType.Not ), new[] { child } );
		}

		public static IFilterTree MakeTrue()
		{
			return new FilterTree( new Token( TokenType.True ) );
		}

		public static IFilterTree MakeFalse()
		{
			return new FilterTree( new Token( TokenType.False ) );
		}


		public static IFilterTree MakeEqual( string attrName, string value )
		{
			var attrItem = MakeAttr( attrName );
			var valueTree = MakeValue( value );

			return new FilterTree( new Token( TokenType.Equal ), new[] { attrItem, valueTree } );
		}

		public static IFilterTree MakeEqual( string attrName, int? value )
		{
			var attrItem = MakeAttr( attrName );
			var valueTree = MakeValue( value );

			return new FilterTree( new Token( TokenType.Equal ), new[] { attrItem, valueTree } );
		}

		public static IFilterTree MakeEqual( string attrName, double? value )
		{
			var attrItem = MakeAttr( attrName );
			var valueTree = MakeValue( value );

			return new FilterTree( new Token( TokenType.Equal ), new[] { attrItem, valueTree } );
		}

		public static IFilterTree MakeEqualToNull( string attrName )
		{
			var attrItem = MakeAttr( attrName );
			var valueTree = MakeNull();

			return new FilterTree( new Token( TokenType.Equal ), new[] { attrItem, valueTree } );
		}

		public static IFilterTree MakeLegacyEqual( string attrName, string value )
		{
			var attrItem = MakeAttr( attrName );
			var valueTree = MakeLegacyValue( value );

			return new FilterTree( new Token( TokenType.Equal ), new[] { attrItem, valueTree } );
		}

		public static IFilterTree MakeNotEqual( string attrName, string value )
		{
			var attrItem = MakeAttr( attrName );
			var valueTree = MakeValue( value );

			return new FilterTree( new Token( TokenType.NotEqual ), new[] { attrItem, valueTree } );
		}

		public static IFilterTree MakeNotEqual( string attrName, int? value )
		{
			var attrItem = MakeAttr( attrName );
			var valueTree = MakeValue( value );

			return new FilterTree( new Token( TokenType.NotEqual ), new[] { attrItem, valueTree } );
		}

		public static IFilterTree MakeNotEqual( string attrName, double? value )
		{
			var attrItem = MakeAttr( attrName );
			var valueTree = MakeValue( value );

			return new FilterTree( new Token( TokenType.NotEqual ), new[] { attrItem, valueTree } );
		}

		public static IFilterTree MakeLegacyNotEqual( string attrName, string value )
		{
			var attrItem = MakeAttr( attrName );
			var valueTree = MakeLegacyValue( value );

			return new FilterTree( new Token( TokenType.NotEqual ), new[] { attrItem, valueTree } );
		}

		public static IFilterTree MakeLess( string attrName, string value )
		{
			var attrItem = MakeAttr( attrName );
			var valueTree = MakeValue( value );

			return new FilterTree( new Token( TokenType.Less ), new[] { attrItem, valueTree } );
		}

		public static IFilterTree MakeLess( string attrName, int? value )
		{
			var attrItem = MakeAttr( attrName );
			var valueTree = MakeValue( value );

			return new FilterTree( new Token( TokenType.Less ), new[] { attrItem, valueTree } );
		}

		public static IFilterTree MakeLess( string attrName, double? value )
		{
			var attrItem = MakeAttr( attrName );
			var valueTree = MakeValue( value );

			return new FilterTree( new Token( TokenType.Less ), new[] { attrItem, valueTree } );
		}

		public static IFilterTree MakeLegacyLess( string attrName, string value )
		{
			var attrItem = MakeAttr( attrName );
			var valueTree = MakeLegacyValue( value );

			return new FilterTree( new Token( TokenType.Less ), new[] { attrItem, valueTree } );
		}

		public static IFilterTree MakeLessOrEqual( string attrName, string value )
		{
			var attrItem = MakeAttr( attrName );
			var valueTree = MakeValue( value );

			return new FilterTree( new Token( TokenType.LessOrEqual ), new[] { attrItem, valueTree } );
		}

		public static IFilterTree MakeLessOrEqual( string attrName, int? value )
		{
			var attrItem = MakeAttr( attrName );
			var valueTree = MakeValue( value );

			return new FilterTree( new Token( TokenType.LessOrEqual ), new[] { attrItem, valueTree } );
		}

		public static IFilterTree MakeLessOrEqual( string attrName, double? value )
		{
			var attrItem = MakeAttr( attrName );
			var valueTree = MakeValue( value );

			return new FilterTree( new Token( TokenType.LessOrEqual ), new[] { attrItem, valueTree } );
		}

		public static IFilterTree MakeLegacyLessOrEqual( string attrName, string value )
		{
			var attrItem = MakeAttr( attrName );
			var valueTree = MakeLegacyValue( value );

			return new FilterTree( new Token( TokenType.LessOrEqual ), new[] { attrItem, valueTree } );
		}

		public static IFilterTree MakeGreaterOrEqual( string attrName, string value )
		{
			var attrItem = MakeAttr( attrName );
			var valueTree = MakeValue( value );

			return new FilterTree( new Token( TokenType.GreaterOrEqual ), new[] { attrItem, valueTree } );
		}

		public static IFilterTree MakeGreaterOrEqual( string attrName, int? value )
		{
			var attrItem = MakeAttr( attrName );
			var valueTree = MakeValue( value );

			return new FilterTree( new Token( TokenType.GreaterOrEqual ), new[] { attrItem, valueTree } );
		}

		public static IFilterTree MakeGreaterOrEqual( string attrName, double? value )
		{
			var attrItem = MakeAttr( attrName );
			var valueTree = MakeValue( value );

			return new FilterTree( new Token( TokenType.GreaterOrEqual ), new[] { attrItem, valueTree } );
		}

		public static IFilterTree MakeLegacyGreaterOrEqual( string attrName, string value )
		{
			var attrItem = MakeAttr( attrName );
			var valueTree = MakeLegacyValue( value );

			return new FilterTree( new Token( TokenType.GreaterOrEqual ), new[] { attrItem, valueTree } );
		}

		public static IFilterTree MakeGreater( string attrName, string value )
		{
			var attrItem = MakeAttr( attrName );
			var valueTree = MakeValue( value );

			return new FilterTree( new Token( TokenType.Greater ), new[] { attrItem, valueTree } );
		}

		public static IFilterTree MakeGreater( string attrName, int? value )
		{
			var attrItem = MakeAttr( attrName );
			var valueTree = MakeValue( value );

			return new FilterTree( new Token( TokenType.Greater ), new[] { attrItem, valueTree } );
		}

		public static IFilterTree MakeGreater( string attrName, double? value )
		{
			var attrItem = MakeAttr( attrName );
			var valueTree = MakeValue( value );

			return new FilterTree( new Token( TokenType.Greater ), new[] { attrItem, valueTree } );
		}

		public static IFilterTree MakeLegacyGreater( string attrName, string value )
		{
			var attrItem = MakeAttr( attrName );
			var valueTree = MakeLegacyValue( value );

			return new FilterTree( new Token( TokenType.Greater ), new[] { attrItem, valueTree } );
		}

		public static IFilterTree MakeLike( string attrName, string value )
		{
			var attrItem = MakeAttr( attrName );
			var valueTree = MakeValue( value );

			return new FilterTree( new Token( TokenType.Like ), new[] { attrItem, valueTree } );
		}

		public static IFilterTree MakeLegacyLike( string attrName, string value )
		{
			var attrItem = MakeAttr( attrName );
			var valueTree = MakeLegacyValue( value );

			return new FilterTree( new Token( TokenType.Like ), new[] { attrItem, valueTree } );
		}

		public static IFilterTree MakeIn( string attrName, params IFilterTree[] values )
		{
			var attrItem = MakeAttr( attrName );
			var valueList = MakeValueList( values );

			return new FilterTree( new Token( TokenType.In ), new[] { attrItem, valueList } );
		}

		public static IFilterTree MakeIn( string attrName, params string[] values )
		{
			var attrItem = MakeAttr( attrName );
			var valueList = MakeValueList( values );

			return new FilterTree( new Token( TokenType.In ), new[] { attrItem, valueList } );
		}

		public static IFilterTree MakeIn( string attrName, params int?[] values )
		{
			var attrItem = MakeAttr( attrName );
			var valueList = MakeValueList( values );

			return new FilterTree( new Token( TokenType.In ), new[] { attrItem, valueList } );
		}

		public static IFilterTree MakeIn( string attrName, params double?[] values )
		{
			var attrItem = MakeAttr( attrName );
			var valueList = MakeValueList( values );

			return new FilterTree( new Token( TokenType.In ), new[] { attrItem, valueList } );
		}

		public static IFilterTree MakeLegacyIn( string attrName, params string[] values )
		{
			var attrItem = MakeAttr( attrName );
			var valueList = MakeLegacyValueList( values );

			return new FilterTree( new Token( TokenType.In ), new[] { attrItem, valueList } );
		}

		public static IFilterTree MakeNotIn( string attrName, params IFilterTree[] values )
		{
			var attrItem = MakeAttr( attrName );
			var valueList = MakeValueList( values );

			return new FilterTree( new Token( TokenType.NotIn ), new[] { attrItem, valueList } );
		}

		public static IFilterTree MakeNotIn( string attrName, params string[] values )
		{
			var attrItem = MakeAttr( attrName );
			var valueList = MakeValueList( values );

			return new FilterTree( new Token( TokenType.NotIn ), new[] { attrItem, valueList } );
		}

		public static IFilterTree MakeNotIn( string attrName, params int?[] values )
		{
			var attrItem = MakeAttr( attrName );
			var valueList = MakeValueList( values );

			return new FilterTree( new Token( TokenType.NotIn ), new[] { attrItem, valueList } );
		}

		public static IFilterTree MakeNotIn( string attrName, params double?[] values )
		{
			var attrItem = MakeAttr( attrName );
			var valueList = MakeValueList( values );

			return new FilterTree( new Token( TokenType.NotIn ), new[] { attrItem, valueList } );
		}

		public static IFilterTree MakeLegacyNotIn( string attrName, params string[] values )
		{
			var attrItem = MakeAttr( attrName );
			var valueList = MakeLegacyValueList( values );

			return new FilterTree( new Token( TokenType.NotIn ), new[] { attrItem, valueList } );
		}

		public static IFilterTree MakeAttr( string name )
		{
			return new FilterTree( new Token( TokenType.AttributeIdentifier, name ) );
		}

		public static IFilterTree MakeValueList( params string[] values )
		{
			var items = new IFilterTree[ values.Length ];
			for( var i = 0; i < values.Length; ++i )
			{
				items[ i ] = MakeValue( values[ i ] );
			}

			return new FilterTree( new Token( TokenType.ValueList ), items );
		}

		public static IFilterTree MakeValueList( params int?[] values )
		{
			var items = new IFilterTree[ values.Length ];
			for( var i = 0; i < values.Length; ++i )
			{
				items[ i ] = MakeValue( values[ i ] );
			}

			return new FilterTree( new Token( TokenType.ValueList ), items );
		}

		public static IFilterTree MakeValueList( params double?[] values )
		{
			var items = new IFilterTree[ values.Length ];
			for( var i = 0; i < values.Length; ++i )
			{
				items[ i ] = MakeValue( values[ i ] );
			}

			return new FilterTree( new Token( TokenType.ValueList ), items );
		}

		public static IFilterTree MakeValueList( params IFilterTree[] values )
		{
			return new FilterTree( new Token( TokenType.ValueList ), values );
		}

		public static IFilterTree MakeLegacyValueList( params string[] values )
		{
			var items = new IFilterTree[ values.Length ];
			for( var i = 0; i < values.Length; ++i )
			{
				items[ i ] = MakeLegacyValue( values[ i ] );
			}

			return new FilterTree( new Token( TokenType.LegacyValueList ), items );
		}

		public static IFilterTree MakeValue( string value )
		{
			return value != null
				? new FilterTree( new Token( TokenType.String, value ) )
				: MakeNull();
		}

		public static IFilterTree MakeValue( double? value )
		{
			return value.HasValue
				? new FilterTree( new Token( TokenType.Real, value.Value.ToString( CultureInfo.InvariantCulture ) ) )
				: MakeNull();
		}

		public static IFilterTree MakeValue( int? value )
		{
			return value.HasValue
				? new FilterTree( new Token( TokenType.Integer, value.Value.ToString( CultureInfo.InvariantCulture ) ) )
				: MakeNull();
		}

		public static IFilterTree MakeLegacyValue( string value )
		{
			return new FilterTree( new Token( TokenType.LegacyValue, value ) );
		}

		public static IFilterTree MakeNull()
		{
			return new FilterTree( new Token( TokenType.Null ) );
		}

		private void PositionSetup()
		{
			var hasValue = false;
			var minPosition = int.MaxValue;
			var maxPosition = int.MinValue;

			if( Token.LexicalToken != null )
			{
				minPosition = Math.Min( minPosition, Token.LexicalToken.Position );
				maxPosition = Math.Max( maxPosition, Token.LexicalToken.Position + Token.LexicalToken.Length );
				hasValue = true;
			}

			foreach( var treeItem in _Children )
			{
				if( treeItem.Token.LexicalToken != null )
				{
					minPosition = Math.Min( minPosition, treeItem.Token.LexicalToken.Position );
					maxPosition = Math.Max( maxPosition, treeItem.Token.LexicalToken.Position + treeItem.Token.LexicalToken.Length );
					hasValue = true;
				}
			}

			if( hasValue )
			{
				Position = minPosition;
				if( maxPosition >= minPosition )
					Length = maxPosition - minPosition;
			}
		}

		#endregion

		#region interface IFilterTree

		/// <inheritdoc />
		public Token Token { get; }

		/// <inheritdoc />
		public int ChildCount => _Children.Count;

		/// <inheritdoc />
		public int? Position { get; private set; }

		/// <inheritdoc />
		public int? Length { get; private set; }

		/// <inheritdoc />
		public IFilterTree GetChild( int index )
		{
			return _Children[ index ];
		}

		/// <inheritdoc />
		public IEnumerable<IFilterTree> GetChildren()
		{
			return _Children;
		}

		#endregion
	}
}