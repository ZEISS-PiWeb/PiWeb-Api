#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.IMT.PiWeb.Api.DataService.Rest
{
	#region using

	using System;
	using System.Xml;
	using Newtonsoft.Json;
	using Zeiss.IMT.PiWeb.Api.Common.Data.Converter;

	#endregion

	/// <summary>
	/// Holds the unique key as well as description and further characterizes an entity.
	/// </summary>
	/// <remarks>This class is immutable.</remarks>
	[Serializable]
	[JsonConverter(typeof(AttributeConverter))]
	public class Attribute
	{
		#region members

		private string _Value;

		#endregion

		#region constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		public Attribute()
		{ }

		/// <summary>
		/// Creates a new attribute with key <code>key</code> and value <code>value</code>.
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
		/// Creates a new attribute with key <code>key</code> and value <code>rawValue</code>.
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
					if( RawValue is double )
						return ( (double)RawValue ).ToString( "G17", System.Globalization.CultureInfo.InvariantCulture );
					if( RawValue is DateTime )
						return XmlConvert.ToString( (DateTime)RawValue, XmlDateTimeSerializationMode.RoundtripKind );
					if( RawValue is CatalogEntry )
						return ( ( CatalogEntry ) RawValue ).Key.ToString();
					return Convert.ToString( RawValue, System.Globalization.CultureInfo.InvariantCulture );
				}
				return _Value; 
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

		#endregion

		#region methods

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

					if( type == typeof( DateTime ) )
						return XmlConvert.ToDateTime( _Value, XmlDateTimeSerializationMode.RoundtripKind );
					
					if( type == typeof( float ) || type == typeof( double ) )
						return XmlConvert.ToDouble( _Value );

					if( type == typeof( int ) )
						return XmlConvert.ToInt32( _Value );
				}
				catch
				{
					// ignored
				}
			}
			return null;
		}

		/// <summary>
		/// Overridden <see cref="System.Object.ToString"/>-method.
		/// </summary>
		public override string ToString()
		{
			return $"K{Key}: {Value}";
		}

		#endregion
	}
}