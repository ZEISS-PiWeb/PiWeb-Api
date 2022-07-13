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
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Text;
	using System.Text.Json.Serialization;
	using JetBrains.Annotations;
	using Zeiss.PiWeb.Api.Rest.Dtos.Converter;
	using Zeiss.PiWeb.Api.Rest.Dtos.Converters;

	#endregion

	/// <summary>
	/// Holds information of a <see cref="CatalogDto"/>'s entry
	/// </summary>
	public class CatalogEntryDto : IAttributeItemDto, IFormattable
	{
		#region members

		private IReadOnlyList<AttributeDto> _Attributes = Array.Empty<AttributeDto>();

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="CatalogEntryDto"/> class.
		/// </summary>
		public CatalogEntryDto()
		{
			Key = -1;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the unique key of this catalog entry.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "key" )]
		[JsonPropertyName( "key" )]
		public short Key { get; set; }

		#endregion

		#region methods

		/// <inheritdoc />
		public override string ToString()
		{
			return ToString( CultureInfo.CurrentUICulture );
		}

		/// <summary>
		/// Returns a string representation of this entry using the format provider
		/// <code>provider</code>.
		/// </summary>
		public string ToString( IFormatProvider provider )
		{
			if( Attributes == null )
				return "";

			if( Attributes.Count == 1 )
				return Convert.ToString( GetTypedAttributeValue( Attributes[ 0 ] ), provider );

			var allSameEntries = true;

			var sb = new StringBuilder();
			foreach( var attribute in Attributes )
			{
				if( sb.Length > 0 )
					sb.Append( " - " );

				if( attribute.Value.Trim().Length > 0 )
					sb.Append( Convert.ToString( GetTypedAttributeValue( attribute ), provider ) );

				allSameEntries &= attribute.Value == _Attributes[ 0 ].Value;
			}

			if( allSameEntries && _Attributes != null && _Attributes.Count > 0 )
				return Convert.ToString( GetTypedAttributeValue( Attributes[ 0 ] ), provider );

			return sb.ToString();
		}

		private static object GetTypedAttributeValue( AttributeDto attribute )
		{
			if( attribute.RawValue != null )
				return ( attribute.RawValue is DateTime time ) ? time.ToLocalTime() : attribute.RawValue;

			return attribute.Value;
		}

		#endregion

		#region interface IAttributeItemDto

		/// <inheritdoc />
		[Newtonsoft.Json.JsonProperty( "attributes" ), Newtonsoft.Json.JsonConverter( typeof( AttributeArrayConverter ) )]
		[JsonPropertyName( "attributes" ), JsonConverter( typeof( AttributeArrayJsonConverter ) )]
		public IReadOnlyList<AttributeDto> Attributes
		{
			[NotNull] get => _Attributes;
			set
			{
				value ??= Array.Empty<AttributeDto>();
				_Attributes = value.All( attr => attr.IsNull() ) ? Array.Empty<AttributeDto>() : value;
			}
		}

		#endregion

		#region interface IFormattable

		/// <inheritdoc />
		public string ToString( string format, IFormatProvider formatProvider )
		{
			return ToString( formatProvider );
		}

		#endregion
	}
}