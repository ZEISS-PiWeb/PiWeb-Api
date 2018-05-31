#region Copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.IMT.PiWeb.Api.Common.Data.FilterString.Tree
{
	using System;
	using System.Globalization;

	public static class FilterTreeHelper
	{
		public static bool CompareTrees( IFilterTree expected, IFilterTree actual )
		{
			if( expected.Token.Type != actual.Token.Type )
				return false;

			if( expected.ChildCount != actual.ChildCount )
				return false;

			if( IsContentToken( expected.Token.Type ) )
			{
				if( expected.Token.Type == TokenType.Real )
				{
					var expectedValue = ToDouble( expected.Token.Value );
					var actualValue = ToDouble( actual.Token.Value );

					// a precision of 10^-12 is enough (same as defined in Zeiss.IMT.PiWeb.Common.Util.DoubleCompareHelper)
					return Math.Abs( expectedValue - actualValue ) <= 1e-12;
				}

				if( expected.Token.Type == TokenType.Integer )
				{
					var expectedValue = int.Parse( expected.Token.Value );
					var actualValue = int.Parse( actual.Token.Value );

					return expectedValue == actualValue;
				}

				if( !string.Equals( expected.Token.Value, actual.Token.Value ) )
					return false;
			}

			for( var i = 0; i < expected.ChildCount; ++i )
			{
				if( !CompareTrees( expected.GetChild( i ), actual.GetChild( i ) ) )
					return false;
			}

			return true;
		}

		public static double ToDouble( string value )
		{
			if( value.StartsWith( "." ) )
				value = "0" + value;

			if( value.StartsWith( "-." ) )
				value = "-0" + value.Substring( 1 );

			if( value.StartsWith( "+." ) )
				value = "+0" + value.Substring( 1 );

			return double.Parse( value, CultureInfo.InvariantCulture );
		}

		public static bool IsContentToken( TokenType tokenType )
		{
			return
				tokenType == TokenType.AttributeIdentifier ||
				tokenType == TokenType.LegacyValue ||
				tokenType == TokenType.Integer ||
				tokenType == TokenType.Real ||
				tokenType == TokenType.String;
		}
	}
}
