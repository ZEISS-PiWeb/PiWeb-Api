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
	using Newtonsoft.Json;
	using Zeiss.PiWeb.Api.Rest.Dtos.Converter;

	#endregion

	/// <summary>
	/// Holds the unique key as well as description and further characterizes an entity.
	/// </summary>
	/// <remarks>This class is immutable.</remarks>
	[Serializable]
	[JsonConverter( typeof( AttributeConverter ) )]
	public class Attribute
	{
		#region members

		private string _Value;

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Attribute"/> class.
		/// </summary>
		public Attribute()
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="Attribute"/> class with key <code>key</code> and value <code>value</code>.
		/// </summary>
		/// <param name="key">The key of the newly created attribute.</param>
		/// <param name="value">The value.</param>
		public Attribute( ushort key, string value )
		{
			Key = key;
			_Value = value;
			RawValue = null;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Attribute"/> class with key <code>key</code> and value <code>rawValue</code>.
		/// </summary>
		/// <param name="key">The key of the newly created attribute.</param>
		/// <param name="rawValue">The raw value.</param>
		public Attribute( ushort key, object rawValue )
		{
			Key = key;
			_Value = null;
			RawValue = rawValue;
		}

		#endregion

		#region properties

		/// <summary>
		/// Returns the key of this attribute.
		/// </summary>
		public ushort Key { get; set; }

		/// <summary>
		/// Returns the unparsed attribute value of this attribute.
		/// </summary>
		public object RawValue { get; set; }

		/// <summary>
		/// Returns the string parsed attribute value of this attribute.
		/// </summary>
		public string Value
		{
			get
			{
				if( _Value == null )
				{
					if( RawValue is double doubleValue )
						return doubleValue.ToString( "G17", CultureInfo.InvariantCulture );
					if( RawValue is DateTime dateTimeValue )
						return XmlConvert.ToString( dateTimeValue, XmlDateTimeSerializationMode.RoundtripKind );
					if( RawValue is CatalogEntry catalogEntryValue )
						return catalogEntryValue.Key.ToString();
					return Convert.ToString( RawValue, CultureInfo.InvariantCulture );
				}

				return _Value;
			}
		}

		#endregion

		#region methods

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
		/// Returns the <see cref="RawValue"/> if not null. Otherwise parses the <see cref="Value"/> using the specified 
		/// <code>type</code> and returns that value.
		/// </summary>
		/// <param name="type">The data type to use for parsing the value.</param>
		public object GetRawValue( Type type )
		{
			if( RawValue != null )
				return RawValue;

			if( _Value != null )
			{
				try
				{
					if( type == typeof( string ) )
						return _Value;

					if( !string.IsNullOrEmpty( _Value ) )
					{
						if( type == typeof( DateTime ) )
							return XmlConvert.ToDateTime( _Value, XmlDateTimeSerializationMode.RoundtripKind );

						if( type == typeof( float ) || type == typeof( double ) )
							return XmlConvert.ToDouble( _Value );

						if( type == typeof( int ) )
							return XmlConvert.ToInt32( _Value );
					}
				}
				catch
				{
					// ignored
				}
			}

			return null;
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return $"K{Key}: {Value}";
		}

		#endregion
	}
}