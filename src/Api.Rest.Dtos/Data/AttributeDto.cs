#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Data
{
	#region usings

	using System;
	using System.Globalization;
	using System.Xml;
	using JetBrains.Annotations;
	using Newtonsoft.Json;
	using Zeiss.PiWeb.Api.Rest.Dtos.Converter;

	#endregion

	/// <summary>
	/// Holds the unique key as well as description and further characterizes an entity.
	/// </summary>
	/// <remarks>This class is immutable.</remarks>
	[Serializable]
	[JsonConverter( typeof( AttributeConverter ) )]
	public readonly struct AttributeDto : IEquatable<AttributeDto>
	{
		#region members

		private readonly string _Value;

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="AttributeDto"/> class with key <code>key</code> and value <code>value</code>.
		/// </summary>
		/// <param name="key">The key of the newly created attribute.</param>
		/// <param name="value">The value.</param>
		public AttributeDto( ushort key, string value )
		{
			Key = key;
			_Value = value;
			RawValue = null;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AttributeDto"/> class with key <code>key</code> and value <code>rawValue</code>.
		/// </summary>
		/// <param name="key">The key of the newly created attribute.</param>
		/// <param name="rawValue">The raw value.</param>
		public AttributeDto( ushort key, object rawValue )
		{
			Key = key;
			_Value = null;
			RawValue = rawValue;
			ValidateRawValue();
		}

		#endregion

		#region properties

		/// <summary>
		/// Returns the key of this attribute.
		/// </summary>
		public ushort Key { get; }

		/// <summary>
		/// Returns the unparsed attribute value of this attribute.
		/// </summary>
		public object RawValue { get; }

		/// <summary>
		/// Returns the string parsed attribute value of this attribute.
		/// </summary>
		public string Value
		{
			get
			{
				if( _Value != null )
					return _Value;

				if( RawValue is double doubleValue )
					return doubleValue.ToString( "G17", CultureInfo.InvariantCulture );

				if( RawValue is DateTime dateTimeValue )
					return XmlConvert.ToString( dateTimeValue, XmlDateTimeSerializationMode.RoundtripKind );

				if( RawValue is CatalogEntryDto catalogEntryValue )
					return catalogEntryValue.Key.ToString();

				return Convert.ToString( RawValue, CultureInfo.InvariantCulture );
			}
		}

		#endregion

		#region methods

		/// <summary>
		/// Validate the raw value and only allows null, int, double, DateTime
		/// and CatalogEntry as a RawValue
		/// </summary>
		private void ValidateRawValue()
		{
			switch( RawValue )
			{
				case null:
				case string:
				case double:
				case int:
				case short:
				case DateTime:
				case CatalogEntryDto:
					return;
				default:
					throw new ArgumentException( $"Unable to initialize the attribute. Invalid value '{RawValue}' with type '{RawValue.GetType()}'" );
			}
		}

		/// <summary>
		/// Determines whether this attribute has a value or not. This allows us to distinguish between attributes defining the empty string
		/// and attributes defining a null value (which could be interpreted as deletion during a merge).
		/// </summary>
		/// <returns>
		///   <code>true</code> if this attribute has no value; otherwise, <code>false</code>.
		/// </returns>
		public bool IsNull()
		{
			return RawValue == null && _Value == null;
		}

		/// <summary>
		/// Returns the encapsulated value as a <see cref="string"/>. If the value is not a string <code>null</code> is returned.
		/// </summary>
		[CanBeNull]
		public string GetStringValue()
		{
			if( RawValue is string valueString )
				return valueString;

			if( !string.IsNullOrEmpty( Value ) )
				return Value;

			return null;
		}

		/// <summary>
		/// Returns the <see cref="RawValue"/> if not null. Otherwise parses the <see cref="Value"/> using the specified
		/// <code>type</code> and returns that value.
		/// </summary>
		/// <param name="type">The data type to use for parsing the value.</param>
		public object GetRawValue( Type type )
		{
			if( RawValue != null )
				return RawValue;

			if( string.IsNullOrEmpty( _Value ) )
				return null;

			if( type == typeof( string ) )
				return _Value;

			if( type == typeof( DateTime ) )
				return GetDateValue();

			if( type == typeof( float ) || type ==  typeof( double ) )
				return GetDoubleValue();

			if( type == typeof( int ) )
				return GetIntValue();

			return null;
		}

		/// <summary>
		/// Returns the value for key <code>key</code> as a <see cref="double"/>.
		/// </summary>
		/// <remarks>
		/// If the parsing fails, <code>null</code> is returned.
		/// </remarks>
		public double? GetDoubleValue()
		{
			switch( RawValue )
			{
				case int value:
					return value;
				case short value:
					return value;
				case double value:
					return value;
				case CatalogEntryDto entry:
					return entry.Key;
			}

#if NETSTANDARD
			if( double.TryParse( _Value, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var result ) )
				return result;
#else
			if( double.TryParse( _Value.AsSpan(), NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var result ) )
				return result;
#endif
			return null;
		}

		/// <summary>
		/// Returns the value for key <code>key</code> as a <see cref="int"/>.
		/// </summary>
		/// <remarks>
		/// If the parsing fails, <code>null</code> is returned.
		/// </remarks>
		public int? GetIntValue()
		{
			switch( RawValue )
			{
				case int value:
					return value;
				case short value:
					return value;
				case CatalogEntryDto entry:
					return entry.Key;
			}

#if NETSTANDARD
			if( int.TryParse( _Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result ) )
				return result;
#else
			if( int.TryParse( _Value.AsSpan(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var result ) )
				return result;
#endif

			return null;
		}

		/// <summary>
		/// Returns the value as a <see cref="DateTime"/>.
		/// </summary>
		/// <remarks>
		/// If the parsing fails, <code>null</code> is returned.
		/// </remarks>
		public DateTime? GetDateValue()
		{
			try
			{
				DateTime date;
				if( RawValue is DateTime rawDateTime )
					date = rawDateTime;
				else if( !string.IsNullOrEmpty( _Value ) )
					date = XmlConvert.ToDateTime( _Value, XmlDateTimeSerializationMode.RoundtripKind );
				else
					return null;

				if( date.Kind != DateTimeKind.Unspecified )
					return date.ToUniversalTime();

				return new DateTime( date.Ticks, DateTimeKind.Local );
			}
			catch
			{
				// return null in case of a parse error
				return null;
			}
		}

		private static bool RawValueEquals( [NotNull] object valueX, [NotNull] object valueY )
		{
			if( ReferenceEquals( valueX, valueY ) )
				return true;

			if( valueX.GetType() != valueY.GetType() )
				return false;

			if( valueX is int intX && valueY is int intY )
				return intX == intY;

			if( valueX is short shortX && valueY is short shortY )
				return shortX == shortY;

			if( valueX is double doubleX && valueY is double doubleY )
				return doubleX == doubleY;

			if( valueX is DateTime dateTimeValueX && valueY is DateTime dateTimeValueY )
				return DateTime.Equals( dateTimeValueX.ToUniversalTime(), dateTimeValueY.ToUniversalTime() );

			if( valueX is CatalogEntryDto catalogEntryX && valueY is CatalogEntryDto catalogEntryY )
				return catalogEntryX.Key == catalogEntryY.Key;

			return valueX.Equals( valueY );
		}

		/// <inheritdoc />
		public override bool Equals( object obj )
		{
			return obj is AttributeDto other && Equals( other );
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			// Always use string representation to compute hash code to avoid different hashes of the same attribute created
			// using different constructors.
			return HashCode.Combine( Key, Value );
		}

		public static bool operator ==( AttributeDto left, AttributeDto right )
		{
			return left.Equals( right );
		}

		public static bool operator !=( AttributeDto left, AttributeDto right )
		{
			return !left.Equals( right );
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return $"K{Key}: {Value}";
		}

		#endregion

		#region interface IEquatable<Attribute>

		/// <inheritdoc />
		public bool Equals( AttributeDto other )
		{
			if( Key != other.Key )
				return false;

			if( RawValue is null || other.RawValue is null )
				return Value == other.Value;

			return RawValueEquals( RawValue, other.RawValue );
		}

		#endregion
	}
}
